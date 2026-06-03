import { Disposable, ICommandService, Inject, IUniverInstanceService, UniverInstanceType, Workbook } from '@univerjs/core';
import { SheetsSelectionsService } from '@univerjs/sheets';
import { attachRangeWithCoord, SheetSkeletonManagerService } from '@univerjs/sheets-ui';
import { IDrawingManagerService } from '@univerjs/drawing';
import { SheetCanvasFloatDomManagerService } from '@univerjs/sheets-drawing-ui';
import { DeviceInputEventType, IRenderManagerService } from '@univerjs/engine-render';
import { buildUUID } from '../utils/uuid';
import { JnpfCommandIds, JnpfUniverFloatImageKey, DefaultFloatImageWidth, DefaultFloatImageHeight, DefaultFloatImageOption } from '../utils/define';

export class JnpfSheetsFloatImageService extends Disposable {
  private _piniaStoreId: string | null = null;
  private _focusDrawingId: string | null = null;

  private readonly defaultPosition = { startX: 100, endX: DefaultFloatImageWidth, startY: 100, endY: DefaultFloatImageHeight };

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

  // 创建并插入悬浮图片DOM
  public insertFloatImage() {
    this._commandService
      .executeCommand('sheet.operation.set-cell-edit-visible', {
        visible: false,
        _eventType: DeviceInputEventType.PointerUp,
      })
      .then();

    const initPosition = this._calculateInitPosition();
    const domId = `float_image_${buildUUID()}`;

    const addResult = this._sheetCanvasFloatDomManagerService?.addFloatDomToPosition({
      componentKey: JnpfUniverFloatImageKey,
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

    const newParams = {
      drawingId: addResult.id,
      domId,
      imageType: 'BASE64',
      option: JSON.parse(JSON.stringify(DefaultFloatImageOption)),
    };

    this._commandService.executeCommand(JnpfCommandIds?.insertedFloatImage, newParams).then();
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
    const unit = this._univerInstanceService.getCurrentUnitForType<Workbook>(UniverInstanceType.UNIVER_SHEET);
    if (!unit) {
      console.warn('警告：未找到单元。使用默认位置 (100, 100)。');
      return this.defaultPosition;
    }

    const unitId = unit!.getUnitId();
    const currentSelections = this._selectionManagerService.getCurrentSelections();
    const firstSelection = currentSelections?.[0] ?? {};
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
    const startX = Math.min(rangeStartX + DefaultFloatImageWidth, columnScreenTotalWidth) - DefaultFloatImageWidth;
    const endX = Math.min(rangeStartX + DefaultFloatImageWidth, columnScreenTotalWidth);
    const startY = Math.min(rangeStartY + DefaultFloatImageHeight, columnScreenTotalHeight) - DefaultFloatImageHeight;
    const endY = Math.min(rangeStartY + DefaultFloatImageHeight, columnScreenTotalHeight);

    return { startX, endX, startY, endY };
  }

  // 订阅图表的焦点事件
  private _subscribeToFocusEvents() {
    this.disposeWithMe(
      this._drawingManagerService.focus$.subscribe(params => {
        if (params && params?.length) {
          const firstParam = (params?.[0] ?? {}) as any;
          const { drawingId, componentKey } = firstParam;

          if (drawingId !== this._focusDrawingId && componentKey === JnpfUniverFloatImageKey) {
            this._focusDrawingId = drawingId;

            this._commandService?.executeCommand(JnpfCommandIds.focusFloatImage, firstParam);
          }
        }
      }),
    );
  }
}
