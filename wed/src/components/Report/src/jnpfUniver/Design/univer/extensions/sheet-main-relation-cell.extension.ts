import type { IScale } from '@univerjs/core';
import { SpreadsheetSkeleton, UniverRenderingContext } from '@univerjs/engine-render';
import { DEFAULT_FONTFACE_PLANE, FIX_ONE_PIXEL_BLUR_OFFSET, getColor, SheetExtension } from '@univerjs/engine-render';

type CellCoordinates = Array<{ row: number; col: number }>; // 单元格坐标数组

interface ClearSheetRelationCellProps {
  key: string;
  sheetId: string;
}

interface SetSheetRelationCellProps extends ClearSheetRelationCellProps {
  rightArrowCells?: CellCoordinates;
  downArrowCells?: CellCoordinates;
}

interface RenderArrowsProps {
  ctx: UniverRenderingContext;
  cells: CellCoordinates;
  arrow: string;
  rowHeights: number[];
  colWidths: number[];
  startRow: number;
  endRow: number;
  startCol: number;
  endCol: number;
  offsetsX?: number;
  offsetsY?: number;
}

// 定义主要扩展类，用于在表格中绘制自定义内容
export class SetSheetRelationCellExtension extends SheetExtension {
  override uKey: string;
  private readonly _sheetId: string; // 用于存储动态传入的 SheetId
  private readonly _rightArrowCells: CellCoordinates; // 渲染 → 的条件数组
  private readonly _downArrowCells: CellCoordinates; // 渲染 ↓ 的条件数组

  constructor(props: SetSheetRelationCellProps) {
    super();

    const { key, sheetId, rightArrowCells, downArrowCells } = props ?? {};
    this.uKey = key;
    this._sheetId = sheetId;
    this._rightArrowCells = rightArrowCells ?? [];
    this._downArrowCells = downArrowCells ?? [];
  }

  // 设置扩展的层级（z-index），必须大于 50
  override get zIndex() {
    return 50;
  }

  // 重写绘制方法
  override draw(ctx: UniverRenderingContext, _parentScale: IScale, skeleton: SpreadsheetSkeleton) {
    if (!this.isValidSkeleton(skeleton)) {
      return;
    }

    if (!this._rightArrowCells?.length && !this._downArrowCells?.length) {
      return;
    }

    const { rowColumnSegment, rowHeightAccumulation, columnTotalWidth, columnWidthAccumulation, rowTotalHeight } = skeleton;
    const { startRow, endRow, startColumn, endColumn } = rowColumnSegment;
    if (!rowHeightAccumulation || !columnWidthAccumulation || columnTotalWidth === undefined || rowTotalHeight === undefined) {
      return;
    }

    this.setupCanvas(ctx);

    // 绘制 → 箭头
    this.renderArrows({
      ctx,
      cells: this._rightArrowCells,
      arrow: '→',
      rowHeights: rowHeightAccumulation,
      colWidths: columnWidthAccumulation,
      startRow,
      endRow,
      startCol: startColumn,
      endCol: endColumn,
      offsetsX: undefined,
      offsetsY: -2,
    });

    // 绘制 ↓ 箭头
    this.renderArrows({
      ctx,
      cells: this._downArrowCells,
      arrow: '↓',
      rowHeights: rowHeightAccumulation,
      colWidths: columnWidthAccumulation,
      startRow,
      endRow,
      startCol: startColumn,
      endCol: endColumn,
      offsetsX: 0,
      offsetsY: 12,
    });

    // 绘制分隔线
    ctx.stroke();
  }

  // 验证骨架和必要属性是否有效
  private isValidSkeleton(skeleton: SpreadsheetSkeleton): boolean {
    // 如果骨架信息不存在，则退出绘制
    if (!skeleton) {
      return false;
    }

    // 限制扩展仅在指定的工作表上生效
    return skeleton?.worksheet?.getSheetId() === this._sheetId;
  }

  // 设置 Canvas 的绘图属性
  private setupCanvas(ctx: UniverRenderingContext) {
    // 设置背景色为浅灰色
    ctx.fillStyle = getColor([255, 255, 255]);
    // 设置文本水平对齐方式
    ctx.textAlign = 'left';
    // 设置文本垂直对齐方式
    ctx.textBaseline = 'top';
    // 设置文本颜色为黑色
    ctx.fillStyle = getColor([24, 144, 255])!;
    // 开始绘制路径
    ctx.beginPath();
    // 设置线宽为 1
    ctx.lineWidth = 1;
    // 防止边界模糊的偏移值，使绘制的边界在高分辨率屏幕下更清晰
    ctx.translateWithPrecisionRatio(FIX_ONE_PIXEL_BLUR_OFFSET, FIX_ONE_PIXEL_BLUR_OFFSET);
    // 设置分隔线颜色为浅灰色
    ctx.strokeStyle = getColor([217, 217, 217]);
    // 设置字体样式
    ctx.font = `10px ${DEFAULT_FONTFACE_PLANE}`;
  }

  // 渲染箭头
  private renderArrows(props: RenderArrowsProps) {
    const { ctx, cells, arrow, rowHeights, colWidths, startRow, endRow, startCol, endCol, offsetsX, offsetsY = 0 } = props ?? {};

    for (const { row, col } of cells) {
      if (row >= startRow - 1 && row <= endRow && col >= startCol - 1 && col <= endCol) {
        const rowStart = rowHeights[row - 1] || 0;
        const colStart = colWidths[col - 1] || 0;

        const colEnd = colWidths[col] || colWidths[col - 1] || 0;
        const cellWidth = colEnd - colStart;

        const xPosition = offsetsX !== undefined ? colStart + offsetsX : colStart + cellWidth / 2 - 6;
        const yPosition = rowStart + offsetsY;

        ctx.fillText(arrow, xPosition, yPosition);
      }
    }
  }
}

// 定义清理扩展类，用于覆盖并清除扩展效果
export class ClearSheetRelationCellExtension extends SheetExtension {
  override uKey: string;
  private readonly _sheetId: string; // 用于存储动态传入的 SheetId

  constructor(props: ClearSheetRelationCellProps) {
    super();

    const { key, sheetId } = props ?? {};
    this.uKey = key;
    this._sheetId = sheetId; // 将传入的 SheetId 存储到类中
  }

  // 空的绘制方法，不进行任何绘制操作
  override draw(_ctx: UniverRenderingContext, _parentScale: IScale, skeleton: SpreadsheetSkeleton) {
    if (skeleton?.worksheet?.getSheetId() !== this._sheetId) {
      return;
    }
  }
}
