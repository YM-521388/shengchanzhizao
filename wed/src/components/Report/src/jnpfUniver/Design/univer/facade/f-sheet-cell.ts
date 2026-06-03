import { ICellData } from '@univerjs/core';
import { JnpfSheetsCellService } from '../services/sheet-cell.service';

export class JnpfFacadeSheetsCell {
  constructor(private readonly _jnpfSheetsCellService: JnpfSheetsCellService) {}

  getCellData(col: number, row: number) {
    return this._jnpfSheetsCellService?.getCellData(col, row);
  }

  setCellData(col: number, row: number, cellData: ICellData) {
    this._jnpfSheetsCellService?.setCellData(col, row, cellData);
  }

  getTargetCellSize(col: number, row: number) {
    return this._jnpfSheetsCellService?.getTargetCellSize(col, row);
  }

  getCurrentSheetCellsCustom() {
    return this._jnpfSheetsCellService?.getCurrentSheetCellsCustom();
  }

  refreshRangeCellsView(range: any) {
    this._jnpfSheetsCellService?.refreshRangeCellsView(range);
  }
}
