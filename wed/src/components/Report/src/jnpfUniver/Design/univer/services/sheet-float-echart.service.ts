import { Disposable, ICommandService, Inject, IUniverInstanceService, UniverInstanceType } from '@univerjs/core';
import { SheetsSelectionsService } from '@univerjs/sheets';
import { attachRangeWithCoord, SheetSkeletonManagerService } from '@univerjs/sheets-ui';
import { IDrawingManagerService } from '@univerjs/drawing';
import { SheetCanvasFloatDomManagerService } from '@univerjs/sheets-drawing-ui';
import { DeviceInputEventType, IRenderManagerService } from '@univerjs/engine-render';
import { buildUUID } from '../utils/uuid';
import { JnpfCommandIds, JnpfUniverFloatEchartKey, DefaultFloatEchartWidth, DefaultFloatEchartHeight, DefaultFloatEchartOptions } from '../utils/define';

const regex = /^JnpfUniverFloat(.*?)Echart$/; // 正则表达式匹配 JnpfUniver 和 Echart

export class JnpfSheetsFloatEchartService extends Disposable {
  private _piniaStoreId: string | null = null;
  private _focusDrawingId: string | null = null;

  private readonly defaultPosition = { startX: 100, endX: DefaultFloatEchartWidth, startY: 100, endY: DefaultFloatEchartHeight };

  constructor(
    @IUniverInstanceService private readonly _univerInstanceService: IUniverInstanceService,
    @ICommandService private readonly _commandService: ICommandService,
    @IRenderManagerService private readonly _renderManagerService: IRenderManagerService,
    @IDrawingManagerService private readonly _drawingManagerService: IDrawingManagerService,
    @Inject(SheetCanvasFloatDomManagerService) private readonly _sheetCanvasFloatDomManagerService: SheetCanvasFloatDomManagerService,
    @Inject(SheetsSelectionsService) private readonly _selectionManagerService: SheetsSelectionsService,
  ) {
    super();

    this._subscribeToFocusEvents();
  }

  // 创建并插入图表DOM
  public insertFloatEchart(componentKey: string) {
    this._commandService
      .executeCommand('sheet.operation.set-cell-edit-visible', {
        visible: false,
        _eventType: DeviceInputEventType.PointerUp,
      })
      .then();

    const initPosition = this._calculateInitPosition();
    const domId = `echart_${buildUUID()}`;

    const addResult = this._sheetCanvasFloatDomManagerService?.addFloatDomToPosition({
      componentKey: JnpfUniverFloatEchartKey,
      initPosition,
      allowTransform: true,
      props: {
        id: domId,
        piniaStoreId: this._piniaStoreId,
      },
    });

    if (!addResult) {
      return;
    }

    const type = componentKey?.replace(regex, '$1');
    const echartType = type ? type.charAt(0)?.toLowerCase() + type.slice(1) : undefined;

    const newParams = {
      drawingId: addResult.id,
      domId,
      echartType: echartType,
      option: JSON.parse(JSON.stringify(DefaultFloatEchartOptions?.[echartType as keyof typeof DefaultFloatEchartOptions])),
    };

    // 自定义命令出来，告知相关的信息
    this._commandService.executeCommand(JnpfCommandIds?.insertedFloatEchart, newParams).then();
  }

  // 存入store的id
  public savePiniaStoreId(data: string) {
    this._piniaStoreId = data;
  }

  // 清空焦点对象的id
  public clearFocusDrawingId() {
    if (!this._focusDrawingId) {
      return;
    }

    this._focusDrawingId = null;
  }

  // 计算初始位置
  private _calculateInitPosition() {
    // 获取当前单元格数据
    const unit = this._univerInstanceService.getCurrentUnitForType(UniverInstanceType.UNIVER_SHEET);
    if (!unit) {
      console.warn('警告：未找到单元。使用默认位置 (100, 100)。');
      return this.defaultPosition;
    }

    const unitId = unit.getUnitId();
    const currentSelections = this._selectionManagerService.getCurrentSelections();
    const firstSelection = currentSelections?.[0];
    if (!firstSelection?.range) {
      console.warn('警告：未找到选择范围。使用默认位置 (100, 100)。');
      return this.defaultPosition;
    }

    // 获取当前表格骨架信息
    const sheetSkeletonService = this._renderManagerService.getRenderById(unitId)?.with(SheetSkeletonManagerService);
    const currentSkeleton = sheetSkeletonService?.getCurrent()?.skeleton;
    if (!currentSkeleton) {
      console.warn('警告：未找到骨架。使用默认位置 (100, 100)。');
      return this.defaultPosition;
    }

    // 解构表格尺寸信息
    const { columnTotalWidth = 0, rowTotalHeight = 0, columnHeaderHeight = 0, rowHeaderWidth = 0 } = currentSkeleton;
    const columnScreenTotalWidth = columnTotalWidth + rowHeaderWidth;
    const columnScreenTotalHeight = rowTotalHeight + columnHeaderHeight;

    // 获取范围信息并计算坐标
    const rangeInfo = attachRangeWithCoord(currentSkeleton, firstSelection.range) || { startX: 0, startY: 0 };
    const { startX: rangeStartX, startY: rangeStartY } = rangeInfo;

    // 计算图表的边界坐标
    const startX = Math.min(rangeStartX + DefaultFloatEchartWidth, columnScreenTotalWidth) - DefaultFloatEchartWidth;
    const endX = Math.min(rangeStartX + DefaultFloatEchartWidth, columnScreenTotalWidth);
    const startY = Math.min(rangeStartY + DefaultFloatEchartHeight, columnScreenTotalHeight) - DefaultFloatEchartHeight;
    const endY = Math.min(rangeStartY + DefaultFloatEchartHeight, columnScreenTotalHeight);

    return { startX, endX, startY, endY };
  }

  // 订阅图表的焦点事件
  private _subscribeToFocusEvents() {
    this.disposeWithMe(
      this._drawingManagerService.focus$.subscribe(params => {
        if (params && params?.length) {
          const firstParam = (params?.[0] ?? {}) as any;
          const { drawingId, componentKey } = firstParam;

          if (drawingId !== this._focusDrawingId && componentKey === JnpfUniverFloatEchartKey) {
            this._focusDrawingId = drawingId;

            this._commandService?.executeCommand(JnpfCommandIds.focusFloatEchart, firstParam);
          }
        }
      }),
    );
  }
}
