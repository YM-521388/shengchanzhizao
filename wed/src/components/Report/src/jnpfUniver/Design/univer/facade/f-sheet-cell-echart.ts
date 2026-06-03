import { JnpfSheetsCellEchartService } from '../services/sheet-cell-echart.service';

export class JnpfFacadeSheetsCellEchart {
  constructor(private readonly _jnpfSheetsCellEchartService: JnpfSheetsCellEchartService) {}

  insertCellEchart(file: File, row: number, col: number, echartConfig: any) {
    return this._jnpfSheetsCellEchartService.insertCellEchart(file, row, col, echartConfig);
  }
}
