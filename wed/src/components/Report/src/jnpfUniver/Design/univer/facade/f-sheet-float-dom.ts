import { JnpfSheetsFloatDomService } from '../services/sheet-float-dom.service';

export class JnpfFacadeSheetsFloatDom {
  constructor(private readonly _jnpfSheetsFloatDomService: JnpfSheetsFloatDomService) {}

  savePiniaStoreId(value: any) {
    this._jnpfSheetsFloatDomService.savePiniaStoreId(value);
  }

  saveFloatEchartItems(data: any) {
    this._jnpfSheetsFloatDomService.saveFloatEchartItems(data);
  }

  saveFloatImageItems(data: any) {
    this._jnpfSheetsFloatDomService.saveFloatImageItems(data);
  }
}
