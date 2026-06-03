import { JnpfSheetsFloatEchartService } from '../services/sheet-float-echart.service';

export class JnpfFacadeSheetsFloatEchart {
  constructor(private readonly _jnpfSheetsFloatEchartService: JnpfSheetsFloatEchartService) {}

  savePiniaStoreId(value: any) {
    this._jnpfSheetsFloatEchartService.savePiniaStoreId(value);
  }

  clearFocusDrawingId() {
    this._jnpfSheetsFloatEchartService.clearFocusDrawingId();
  }
}
