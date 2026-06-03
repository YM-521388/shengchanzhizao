import { Disposable, ICellData, IUniverInstanceService, UniverInstanceType, Workbook } from '@univerjs/core';
import { SheetSkeletonManagerService } from '@univerjs/sheets-ui';
import { IRenderManagerService } from '@univerjs/engine-render';

export class JnpfSheetsCellService extends Disposable {
  constructor(
    @IUniverInstanceService private readonly _univerInstanceService: IUniverInstanceService,
    @IRenderManagerService private readonly _renderManagerService: IRenderManagerService,
  ) {
    super();
  }

  // 获取单元格数据
  public getCellData(col: number, row: number) {
    const sheet = this._univerInstanceService.getCurrentUnitForType<Workbook>(UniverInstanceType.UNIVER_SHEET)!.getActiveSheet();
    const cellMatrix = sheet.getCellMatrix().getMatrix();

    return cellMatrix?.[row]?.[col] ?? null; // 如果未找到匹配的单元格，返回 null
  }

  // 设置单元格数据
  public setCellData(col: number, row: number, cellData: ICellData) {
    const sheet = this._univerInstanceService.getCurrentUnitForType<Workbook>(UniverInstanceType.UNIVER_SHEET)!.getActiveSheet();
    const cellMatrix = sheet.getCellMatrix();

    try {
      cellMatrix.setValue(row, col, cellData);
    } catch (e) {
      console.warn(e);
    }
  }

  // 获取当前单元格的自定义属性
  public getCurrentSheetCellsCustom() {
    const sheet = this._univerInstanceService.getCurrentUnitForType<Workbook>(UniverInstanceType.UNIVER_SHEET)!.getActiveSheet();
    const cellMatrix = sheet?.getCellMatrix()?.getMatrix() ?? {};

    const result: { [row: string]: { [col: string]: any } } = {};
    Object.entries(cellMatrix)?.forEach(([row, colObj]) => {
      if (colObj && typeof colObj === 'object') {
        const rowData: { [col: string]: any } = {};

        Object.entries(colObj)?.forEach(([col, value]: any) => {
          if (value?.custom) {
            rowData[col] = { ...value };
          }
        });

        if (Object.keys(rowData)?.length) {
          result[row] = rowData;
        }
      }
    });

    return result;
  }

  // 获取激活单元格的尺寸信息
  public getTargetCellSize(col: number, row: number): { cellWidth: number; cellHeight: number } | null {
    // 获取当前的工作簿实例
    const workbook = this._univerInstanceService.getCurrentUnitForType<Workbook>(UniverInstanceType.UNIVER_SHEET);
    if (!workbook) {
      return null;
    }

    const unitId = workbook.getUnitId();

    // 获取当前激活的工作表
    const worksheet = workbook.getActiveSheet();
    if (!worksheet) {
      return null;
    }

    const subUnitId = worksheet.getSheetId();

    // 获取对应的骨架渲染服务
    const skeleton = this._renderManagerService?.getRenderById(unitId)?.with(SheetSkeletonManagerService)?.getUnitSkeleton(unitId, subUnitId)?.skeleton as any;

    if (!skeleton) {
      return null;
    }

    // 获取指定单元格的位置信息
    const cellInfo = skeleton.getCellWithCoordByIndex(row, col);
    if (!cellInfo) {
      return null;
    }

    // 解构单元格信息
    const { startX, startY, endX, endY, isMergedMainCell, mergeInfo } = cellInfo;

    let cellWidth: number;
    let cellHeight: number;

    if (isMergedMainCell && mergeInfo) {
      cellWidth = mergeInfo.endX - mergeInfo.startX;
      cellHeight = mergeInfo.endY - mergeInfo.startY;
    } else {
      cellWidth = endX - startX;
      cellHeight = endY - startY;
    }

    return {
      cellWidth: Math.floor(cellWidth),
      cellHeight: Math.floor(cellHeight),
    };
  }

  // 刷新所有单元格的视图
  public refreshRangeCellsView(range: any) {
    if (!range) {
      return;
    }

    const originalValue = range?.getValue() ?? '';
    range?.setValue(originalValue);
  }
}
