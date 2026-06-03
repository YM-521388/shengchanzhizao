import { SheetsSelectionsService } from '@univerjs/sheets';
import { SheetPrintInterceptorService, SheetSkeletonManagerService } from '@univerjs/sheets-ui';
import { Inject, UniverInstanceType, Disposable, type IFreeze, IUniverInstanceService, type Workbook } from '@univerjs/core';
import { IRenderManagerService, SpreadsheetSkeleton } from '@univerjs/engine-render';
import { isEmptyObject, isNullOrUndefined } from '../utils';
import { buildUUID } from '../utils/uuid';
import {
  DefaultPrintConfig,
  JnpfPrintAreaEnum,
  JnpfPrintDirectionEnum,
  JnpfPrintScaleEnum,
  JnpfPrintFreezeEnum,
  JnpfPrintOtherParamsEnum,
  JnpfPrintPaperSizeForType,
  JnpfPaperPaddingForType,
} from '../utils/define';

export class JnpfSheetsPrintService extends Disposable {
  private _configParams: Record<string, any> = {};
  private _layoutHeaderFooterParams: any[] = [];

  constructor(
    @IUniverInstanceService protected readonly _univerInstanceService: IUniverInstanceService,
    @IRenderManagerService private readonly _renderManagerService: IRenderManagerService,
    @Inject(SheetsSelectionsService) private readonly _selectionManagerService: SheetsSelectionsService,
    @Inject(SheetPrintInterceptorService) private readonly _sheetPrintInterceptorService: SheetPrintInterceptorService,
  ) {
    super();
  }

  /**
   * 获取当前活动的工作簿实例。
   * @returns  返回当前活动的工作簿实例，如果不存在则返回 `undefined`。
   */
  private _getWorkbook() {
    // 使用 `UniverInstanceService` 获取当前类型为 `UNIVER_SHEET` 的活动单元实例
    return this._univerInstanceService.getCurrentUnitForType<Workbook>(UniverInstanceType.UNIVER_SHEET);
  }

  /**
   * 获取指定单元的骨架信息。
   * @param {string} unitId - 单元的唯一标识符。
   * @param {string} subUnitId - 子单元的唯一标识符。
   * @returns 返回单元骨架信息对象，如果未找到则返回空对象。
   */
  private _getUnitSkeleton(unitId: string, subUnitId: string) {
    // 从 `RenderManagerService` 中获取指定单元 ID 的渲染服务
    return (this._renderManagerService
      ?.getRenderById(unitId) // 根据单元 ID 获取对应的渲染服务
      ?.with(SheetSkeletonManagerService) // 使用骨架管理服务进行扩展
      ?.getUnitSkeleton(unitId, subUnitId)?.skeleton ?? {}) as any; // 获取单元对应的骨架信息 // 如果骨架信息不存在，则返回空对象 // 强制将返回值类型设置为 `any`
  }

  /**
   * 获取当前配置的纸张大小。
   * @returns 返回包含宽度和高度的纸张尺寸对象，或者返回 `null`。
   */
  private _getPaperSize() {
    // 从配置参数中解构获取纸张类型（paperType）和方向（direction）
    const { paperType, direction } = this._configParams ?? {};

    // 根据纸张类型获取对应的尺寸数据
    const paperSize = JnpfPrintPaperSizeForType[paperType as keyof typeof JnpfPrintPaperSizeForType] ?? {};
    // 如果纸张尺寸为空，返回 null
    if (isEmptyObject(paperSize)) {
      return null;
    }

    // 根据打印方向返回调整后的纸张尺寸
    return direction === JnpfPrintDirectionEnum?.portrait
      ? { ...paperSize } // 如果是纵向模式，直接返回原始纸张尺寸
      : {
          w: paperSize?.h, // 如果是横向模式，交换宽度和高度
          h: paperSize?.w,
        };
  }

  /**
   * 获取当前配置的纸张边距信息。
   * @returns 返回包含边距信息的对象，或者返回 `null`。
   */
  private _getPaperPadding() {
    // 从配置参数中解构获取边距类型（padding）
    const { padding } = this._configParams ?? {};

    // 根据边距类型从预定义的边距配置映射中查找对应的边距值
    const paperPadding = JnpfPaperPaddingForType[padding as keyof typeof JnpfPaperPaddingForType] ?? {};

    // 如果找到有效的边距配置，返回边距配置；否则返回 null
    return !isEmptyObject(paperPadding) ? paperPadding : null;
  }

  /**
   * 计算打印范围，根据配置的区域选择合适的打印范围。
   * @returns 返回包含一个或多个打印范围的数组。
   */
  private _computePrintRanges() {
    // 初始化空数组 result，用于存储计算的打印范围
    let result: any[] = [];

    // 根据配置的打印区域 执行不同的处理
    switch (this._configParams.printArea) {
      // 如果区域是整个工作簿
      case JnpfPrintAreaEnum.workbook:
        // 获取工作簿中的所有工作表
        const sheets = this._getWorkbook()!.getSheets();

        // 对每个工作表计算打印范围，并过滤掉无效的范围
        result = sheets
          ?.map(sheet => {
            const subUnitId = sheet?.getSheetId();
            // 对每个工作表计算并返回其打印范围
            return this._computePrintSheetRange(subUnitId);
          })
          .filter(Boolean); // 过滤掉无效值（如 null 或 undefined）
        break;

      // 如果区域是当前工作表
      case JnpfPrintAreaEnum.currentSheet:
        // 获取当前活动工作表的 ID
        const subUnitId = this._getWorkbook()!.getActiveSheet()?.getSheetId();

        // 如果工作表 ID 不存在，则跳出
        if (!subUnitId) {
          break;
        }

        // 计算当前工作表的打印范围
        const range = this._computePrintSheetRange(subUnitId);
        // 如果计算结果有效，返回一个包含该范围的数组
        result = range ? [range] : [];
        break;

      // 如果区域是当前选择的区域
      case JnpfPrintAreaEnum.currentSelection:
        // 计算当前选择区域的打印范围，并返回该范围的数组
        result = [this._computePrintSelectionRange()];
        break;

      // 如果区域类型不匹配，返回空数组
      default:
        break;
    }

    // 返回计算出的打印范围数组
    return result;
  }

  /**
   * 计算工作表的打印范围。
   * @param {string} subUnitId - 工作表的唯一标识符。
   * @returns 返回计算出的打印范围，包含页面范围、冻结信息等。
   */
  private _computePrintSheetRange(subUnitId: string) {
    // 获取工作簿实例
    const workbook = this._getWorkbook();

    // 如果没有获取到工作簿，返回 undefined
    if (!workbook) {
      return;
    }

    // 获取工作簿的唯一标识符
    const unitId = workbook!.getUnitId();

    // 获取当前工作表
    const worksheet = workbook!.getSheetBySheetId(subUnitId);

    // 如果工作表不存在，返回 undefined
    if (!worksheet) {
      return;
    }

    // 获取工作表的骨架信息，包含如行列宽度、冻结状态等数据
    const skeleton = this._getUnitSkeleton(unitId, subUnitId);

    // 如果工作表的骨架信息不存在，返回 undefined
    if (!skeleton) {
      return;
    }

    // 获取工作表的冻结信息，包括冻结行列位置
    const freeze = worksheet?.getFreeze();

    // 如果冻结信息不存在，返回 undefined
    if (!freeze) {
      return;
    }

    // 获取当前工作表的单元格矩阵打印范围
    const cellMatrixPrintRange = worksheet?.getCellMatrixPrintRange();

    // 如果没有获取到单元格矩阵打印范围，返回 undefined
    if (!cellMatrixPrintRange) {
      return;
    }

    // 获取当前工作表骨架中的溢出缓存数据
    const { overflowCache } = skeleton;
    if (overflowCache) {
      // 如果有溢出缓存，遍历其值并调整打印范围
      overflowCache.forValue((_row: any, _col: any, value: any) => {
        const { endColumn } = value;

        // 如果溢出的单元格列超过当前打印范围的结束列，则调整打印范围
        if (endColumn > cellMatrixPrintRange?.endColumn) {
          cellMatrixPrintRange.endColumn = endColumn;
        }
      });
    }

    // 使用拦截器调整打印范围
    const realCellMatrixPrintRange = this._sheetPrintInterceptorService.interceptor?.fetchThroughInterceptors(
      this._sheetPrintInterceptorService.interceptor?.getInterceptPoints()?.PRINTING_RANGE,
    )(cellMatrixPrintRange, { unitId, subUnitId });

    // 返回最终计算出的打印页面范围，包括冻结行列的调整
    return this._computePrintPageRange(subUnitId, {
      // 调整起始行列位置，确保不小于零
      ...realCellMatrixPrintRange,
      startRow: Math.max(freeze.startRow - freeze.ySplit, 0),
      startColumn: Math.max(freeze.startColumn - freeze.xSplit, 0),
    });
  }

  /**
   * 计算并返回打印页面的范围，包括内容区域、冻结行列以及打印比例。
   * @param {string} subUnitId - 工作表的唯一标识符。
   * @param {any} sheetRange - 需要打印的工作表范围，包含起始和结束的行列。
   * @returns 返回打印页面的范围、冻结信息、内容区域和打印比例。
   */
  private _computePrintPageRange(subUnitId: string, sheetRange: any) {
    // 获取工作簿实例
    const workbook = this._getWorkbook();

    // 如果没有获取到工作簿，返回 undefined
    if (!workbook) {
      return;
    }

    // 获取工作簿的唯一标识符
    const unitId = workbook!.getUnitId();

    // 获取纸张大小
    const paperSize = this._getPaperSize();
    if (!paperSize) {
      return;
    }

    // 获取纸张的边距
    const paperPadding = this._getPaperPadding();
    if (!paperPadding) {
      return;
    }

    // 计算内容区域的大小，减去左右、上下的边距
    const contentSize = {
      w: paperSize.w - (paperPadding.left + paperPadding.right),
      h: paperSize.h - (paperPadding.top + paperPadding.bottom),
    };

    // 如果起始行列小于等于 -1，返回默认的打印范围，表示无效的打印范围
    if (sheetRange?.startRow <= -1 || sheetRange?.startColumn <= -1) {
      return {
        unitId,
        subUnitId,
        range: { startColumn: 0, endColumn: 0, startRow: 0, endRow: 0 },
        freeze: { startColumn: -1, startRow: -1, xSplit: 0, ySplit: 0 },
        contentSize,
        printScale: 1,
      };
    }

    // 获取工作表实例
    const worksheet = workbook!.getSheetBySheetId(subUnitId);
    if (!worksheet) {
      return;
    }

    // 获取工作表的冻结信息
    const freeze = worksheet?.getFreeze();
    if (!freeze) {
      return;
    }

    // 获取工作表的骨架信息（如列宽、行高等）
    const skeleton = this._getUnitSkeleton(unitId, subUnitId);
    if (!skeleton) {
      return;
    }

    // 计算打印比例
    const printScale = this._computePrintScale(skeleton, sheetRange, freeze);

    // 初始化打印范围和冻结信息的默认值
    let startRow = -1,
      startColumn = -1,
      xSplit = 0,
      ySplit = 0;

    // 获取骨架中的列宽和行高累积数据
    const { columnWidthAccumulation, rowHeightAccumulation } = skeleton ?? {};
    let { startRow: sheetRangeStartRow, startColumn: sheetRangeStartColumn, endRow: sheetRangeEndRow, endColumn: sheetRangeEndColumn } = sheetRange ?? {};
    const { startRow: freezeStartRow, startColumn: freezeStartColumn, xSplit: freezeXSplit, ySplit: freezeYSplit } = freeze ?? {};

    // 处理冻结行（如果配置了冻结行）
    if (freezeYSplit && this._configParams?.freeze.includes(JnpfPrintFreezeEnum?.row)) {
      // 如果打印范围的结束行大于等于冻结行，则计算冻结区域的高度并调整打印区域的高度
      if (sheetRangeEndRow >= freezeStartRow) {
        const offsetHeight = rowHeightAccumulation[freezeStartRow] - rowHeightAccumulation[freezeStartRow - freezeYSplit];

        if (offsetHeight < contentSize?.h) {
          startRow = freezeStartRow;
          ySplit = freezeYSplit;
          contentSize.h -= offsetHeight;
        }
      } else {
        // 如果打印范围的结束行小于冻结行，则计算冻结区域的高度并调整打印区域的高度
        startRow = sheetRangeEndRow + 1;
        ySplit = sheetRangeEndRow + 1 - (freezeStartRow - freezeYSplit);
        contentSize.h = contentSize?.h - rowHeightAccumulation[freezeStartRow] - rowHeightAccumulation[freezeStartRow - freezeYSplit];
      }
    }

    // 更新起始行，确保不小于冻结区域的起始行
    sheetRangeStartRow = Math.max(startRow, sheetRangeStartRow);

    // 处理冻结列（如果配置了冻结列）
    if (freezeXSplit && this._configParams?.freeze.includes(JnpfPrintFreezeEnum?.column)) {
      // 如果打印范围的结束列大于等于冻结列，则计算冻结区域的宽度并调整打印区域的宽度
      if (sheetRangeEndColumn >= freezeStartColumn) {
        const offsetWidth =
          printScale * (columnWidthAccumulation[freezeStartColumn - 1] - (columnWidthAccumulation[freezeStartColumn - freezeXSplit - 1] || 0));

        if (offsetWidth < contentSize?.w) {
          startColumn = freezeStartColumn;
          xSplit = freezeXSplit;
          contentSize.w -= offsetWidth;
        }
      } else {
        // 如果打印范围的结束列小于冻结列，则计算冻结区域的宽度并调整打印区域的宽度
        const offsetWidth =
          printScale * (columnWidthAccumulation[freezeStartColumn - 1] - (columnWidthAccumulation[freezeStartColumn - freezeXSplit] - 1 || 0));

        if (offsetWidth < contentSize?.w) {
          const startColumnCache = sheetRangeEndColumn + 1;
          const xSplitCache = sheetRangeEndColumn + 1 - (freezeStartColumn - freezeXSplit);

          startColumn = startColumnCache;
          xSplit = xSplitCache;
          contentSize.w -= offsetWidth;
        }
      }
    }

    // 更新起始列，确保不小于冻结区域的起始列
    sheetRangeStartColumn = Math.max(startColumn, sheetRangeStartColumn);

    // 返回计算出的打印页面范围，包含打印区域、冻结信息、内容大小和打印比例
    return {
      unitId,
      subUnitId,
      range: { startRow: sheetRangeStartRow, startColumn: sheetRangeStartColumn, endRow: sheetRangeEndRow, endColumn: sheetRangeEndColumn },
      freeze: { startRow, startColumn, xSplit, ySplit },
      contentSize,
      printScale,
    };
  }

  /**
   * 计算并返回打印选择范围的详细信息。
   * @returns 返回选区的打印范围信息。
   */
  private _computePrintSelectionRange() {
    // 获取当前工作簿实例
    const workbook = this._getWorkbook();

    // 如果没有获取到工作簿，返回 undefined
    if (!workbook) {
      return;
    }

    // 获取工作簿的唯一标识符
    const unitId = workbook!.getUnitId();

    // 获取当前活动工作表
    const worksheet = workbook!.getActiveSheet();

    // 如果没有获取到工作表，返回 undefined
    if (!worksheet) {
      return;
    }

    // 获取当前工作表的唯一标识符（subUnitId）
    const subUnitId = worksheet?.getSheetId();

    // 获取当前工作簿的最后选择区域
    const lastSelection = this._selectionManagerService?.getWorkbookSelections(unitId)?.getCurrentLastSelection();

    // 如果没有获取到选择区域，返回 undefined
    if (!lastSelection) {
      return;
    }

    // 根据最后选择的范围计算打印的页面范围
    return this._computePrintPageRange(subUnitId, lastSelection?.range);
  }

  /**
   * 计算页面布局，根据给定的打印范围和工作表的骨架信息，计算每一页的布局。
   * @param printRange {any} 打印范围，包含工作簿信息、工作表信息、打印范围、冻结信息、缩放比例及内容尺寸
   * @returns 返回计算出的页面布局信息
   */
  private _computePageLayout(printRange: any) {
    // 解构打印范围信息
    const { unitId, subUnitId, range, freeze, printScale, contentSize } = printRange ?? {};

    // 获取工作表的骨架信息
    const skeleton = this._getUnitSkeleton(unitId, subUnitId);
    if (!skeleton) {
      return;
    }

    // 解构工作表的行高和列宽累计值
    const { rowHeightAccumulation, columnWidthAccumulation } = skeleton;

    // 解构打印范围的行列信息
    const { startRow, startColumn, endRow, endColumn } = range;

    const calcRow = (start: any) => {
      let end = start;
      const offset = start === 0 ? 0 : rowHeightAccumulation[start - 1];
      for (; end < endRow; end++) {
        const current = (rowHeightAccumulation[end] - offset) * printScale;
        const next = (rowHeightAccumulation[end + 1] - offset) * printScale;
        if (current > contentSize.h || (current < contentSize.h && next > contentSize.h)) return { startRow: start, endRow: end - 1 };
      }
      return { startRow: start, endRow: end };
    };

    const calcColumn = (start: any) => {
      let end = start;
      const offset = start === 0 ? 0 : columnWidthAccumulation[start - 1];
      for (; end < endColumn; end++) {
        const current = (columnWidthAccumulation[end] - offset) * printScale;
        const next = (columnWidthAccumulation[end + 1] - offset) * printScale;

        if (current >= contentSize.w || (current < contentSize.w && next > contentSize.w)) {
          return { startColumn: start, endColumn: end };
        }
      }
      return { startColumn: start, endColumn: end };
    };

    // 计算所有行的分页范围
    const rows = [];
    for (let start = startRow; start <= endRow; ) {
      const row = calcRow(start);
      rows.push(row);
      start = row.endRow + 1; // 移动到下一个分页起始行
    }

    // 计算所有列的分页范围
    const columns: any[] = [];
    for (let start = startColumn; start <= endColumn; ) {
      const column = calcColumn(start);
      columns.push(column);
      start = column.endColumn + 1; // 移动到下一个分页起始列
    }

    // 将行列的分页信息组合成每一页的布局
    const pages: any[] = [];
    rows.forEach(row => {
      columns.forEach(column => {
        pages.push({ ...row, ...column });
      });
    });

    // 返回计算的页面布局信息，包括单位ID、子单位ID、页面信息、冻结信息和缩放比例
    return { unitId, subUnitId, pages, freeze, printScale };
  }

  /**
   * 计算打印比例。
   * @param {SpreadsheetSkeleton} skeleton - 工作表的骨架信息，包括行高、列宽累积数据。
   * @param {any} sheetRange - 打印的单元格范围，包括起始行、起始列、结束行、结束列。
   * @param {IFreeze} freeze - 冻结范围信息，包括冻结的起始行列和冻结的分割范围。
   * @returns 返回适合的打印缩放比例，范围在 [0, 1] 之间。
   */
  private _computePrintScale(skeleton: SpreadsheetSkeleton, sheetRange: any, freeze: IFreeze) {
    // 解构骨架中的列宽和行高累积信息
    const { columnWidthAccumulation, rowHeightAccumulation } = skeleton ?? {};

    // 骨架信息为空时，返回默认比例 1
    if (isNullOrUndefined(columnWidthAccumulation) || isNullOrUndefined(rowHeightAccumulation)) {
      return 1;
    }

    // 解构打印范围的起始和结束行列
    const { startRow: sheetRangeStartRow, startColumn: sheetRangeStartColumn, endRow: sheetRangeEndRow, endColumn: sheetRangeEndColumn } = sheetRange ?? {};

    // 打印范围信息为空时，返回默认比例 1
    if (
      isNullOrUndefined(sheetRangeStartRow) ||
      isNullOrUndefined(sheetRangeStartColumn) ||
      isNullOrUndefined(sheetRangeEndRow) ||
      isNullOrUndefined(sheetRangeEndColumn)
    ) {
      return 1;
    }

    // 解构冻结范围的起始行列和分割范围
    const { startRow: freezeStartRow, startColumn: freezeStartColumn, xSplit: freezeXSplit, ySplit: freezeYSplit } = freeze ?? {};

    // 冻结范围信息为空时，返回默认比例 1
    if (isNullOrUndefined(freezeXSplit) || isNullOrUndefined(freezeYSplit) || isNullOrUndefined(freezeStartRow) || isNullOrUndefined(freezeStartColumn)) {
      return 1;
    }

    // 确定打印范围的起始行：对冻结范围与打印范围取交集
    const startRow = sheetRangeEndRow < freezeStartRow ? sheetRangeStartRow : Math.max(freezeStartRow, sheetRangeStartRow);

    // 确定打印范围的起始列：对冻结范围与打印范围取交集
    const startColumn = sheetRangeEndColumn < freezeStartColumn ? sheetRangeStartColumn : Math.max(freezeStartColumn, sheetRangeStartColumn);

    // 获取纸张尺寸
    const paperSize = this._getPaperSize();
    if (!paperSize) {
      return 1;
    }

    // 获取纸张边距
    const paperPadding = this._getPaperPadding();
    if (!paperPadding) {
      return 1;
    }

    // 计算冻结区域的宽度（列）和高度（行）
    const xSplit = freezeXSplit > 0 ? columnWidthAccumulation[freezeStartColumn - 1] - (columnWidthAccumulation[freezeStartColumn - freezeXSplit - 1] || 0) : 0;
    const ySplit = freezeYSplit > 0 ? rowHeightAccumulation[freezeStartRow - 1] - (rowHeightAccumulation[freezeStartRow - freezeYSplit - 1] || 0) : 0;

    // 计算打印范围的高度和宽度
    const rowHeight = rowHeightAccumulation[sheetRangeEndRow] - (rowHeightAccumulation[startRow - 1] || 0);
    const columnWidth = columnWidthAccumulation[sheetRangeEndColumn] - (columnWidthAccumulation[startColumn - 1] || 0);

    // 加上冻结区域的尺寸，得到总的打印高度和宽度
    const xScale = columnWidth + xSplit;
    const yScale = rowHeight + ySplit;

    let result = 1;

    // 根据配置参数计算打印比例
    switch (this._configParams.printScale) {
      case JnpfPrintScaleEnum.fitWidth:
        // 宽度适应比例
        result = Math.min(1, (paperSize.w - paperPadding.left - paperPadding.right) / xScale);
        break;
      case JnpfPrintScaleEnum.fitHeight:
        // 高度适应比例
        result = Math.min(1, (paperSize.h - paperPadding.top - paperPadding.bottom) / yScale);
        break;
      case JnpfPrintScaleEnum.fitPage:
        // 页面完全适应比例（宽度和高度都适应）
        result = Math.min(1, (paperSize.w - paperPadding.left - paperPadding.right) / xScale, (paperSize.h - paperPadding.top - paperPadding.bottom) / yScale);
        break;
      default:
        break;
    }

    // 返回比例值，保留两位小数
    return Math.floor(result * 100) / 100;
  }

  /**
   * 解析页面布局中的页眉和页脚配置。
   * @param {any} printConfig - 页眉页脚的配置对象，包含多个布尔值属性来决定是否显示某些信息。
   * @returns 返回需要在页眉或页脚中包含的参数数组。
   */
  private _analyseLayoutHeaderFooterConfig(printConfig: any) {
    // 定义一个数组，用于存储需要包含的页眉和页脚参数
    const layoutHeaderFooterParams = [];

    // 如果配置中启用了工作簿标题，则将其参数加入数组
    if (printConfig?.workbookTitle) {
      layoutHeaderFooterParams.push(JnpfPrintOtherParamsEnum.workbookTitle);
    }

    // 如果配置中启用了工作表标题，则将其参数加入数组
    if (printConfig?.worksheetTitle) {
      layoutHeaderFooterParams.push(JnpfPrintOtherParamsEnum.worksheetTitle);
    }

    // 如果配置中启用了当前日期，则将其参数加入数组
    if (printConfig?.printDate) {
      layoutHeaderFooterParams.push(JnpfPrintOtherParamsEnum.printDate);
    }

    // 如果配置中启用了当前时间，则将其参数加入数组
    if (printConfig?.printTime) {
      layoutHeaderFooterParams.push(JnpfPrintOtherParamsEnum.printTime);
    }

    // 如果配置中启用了页码显示，则将其参数加入数组
    if (printConfig?.pageNumber) {
      layoutHeaderFooterParams.push(JnpfPrintOtherParamsEnum.pageNumber);
    }

    // 返回最终解析出来的页眉页脚参数数组
    return layoutHeaderFooterParams;
  }

  /**
   * 根据配置获取打印布局。
   * @param {any} printConfig - 打印配置对象，默认为 `DefaultPrintConfig`。
   */
  getLayouts(printConfig: any = DefaultPrintConfig) {
    // 分析布局的页眉和页脚配置，并将结果存储在 `_layoutHeaderFooterParams` 中
    this._layoutHeaderFooterParams = this._analyseLayoutHeaderFooterConfig(printConfig);

    // 获取参数出来判断是不是存在冻结
    const subUnitId = this._getWorkbook()!.getActiveSheet()?.getSheetId();
    if (!subUnitId) {
      return;
    }

    const snapshot = this._getWorkbook()!.save();
    const { xSplit = 0, ySplit = 0 } = snapshot?.sheets?.[subUnitId]?.freeze ?? {};

    // 正确的冻结状态判断
    const colHasFreeze = xSplit > 0; // 列冻结（xSplit 对应列冻结）
    const rowHasFreeze = ySplit > 0; // 行冻结（ySplit 对应行冻结）

    // 判断冻结状态
    const freeze = [
      rowHasFreeze && printConfig?.yFreeze && JnpfPrintFreezeEnum.row, // 行冻结
      colHasFreeze && printConfig?.xFreeze && JnpfPrintFreezeEnum.column, // 列冻结
    ].filter(Boolean); // 过滤掉 false/null/undefined

    // 提取剩余配置
    const { xFreeze, yFreeze, ...restPrintConfig } = printConfig ?? {};

    // 将配置参数存储在 `_configParams` 中
    this._configParams = {
      ...restPrintConfig,
      freeze,
    };

    // 获取所有需要打印的范围
    const printRanges = this._computePrintRanges();
    // 初始化空数组，用于存储计算出的布局
    const layouts: any[] = [];

    // 遍历每个打印范围，计算对应的页面布局
    printRanges?.forEach(range => {
      // 如果当前范围存在，继续处理
      if (range) {
        // 计算页面布局
        const pageLayout = this._computePageLayout(range);

        // 如果页面布局存在，将其添加到 layouts 数组中
        if (pageLayout) {
          layouts.push(pageLayout);
        }
      }
    });

    // 获取纸张大小
    const paperSize = this._getPaperSize();
    // 获取纸张边距
    const paperPadding = this._getPaperPadding();

    let pageNumber = 1;
    const pagesLayout = layouts.flatMap(({ pages: pageRanges, ...info }) =>
      pageRanges.map((range: any) => {
        return {
          ...info,
          pageNumber: pageNumber++,
          range,
          paperSize,
          paperPadding,
          browserPreviewScale: 1,
          layoutId: `layout_${buildUUID()}`,
        };
      }),
    );

    return {
      pagesLayout,
      printConfig: {
        ...this._configParams,
        headerFooterParams: this._layoutHeaderFooterParams,
      },
      paperSize,
    };
  }
}
