import { JnpfSheetsFloatImageService } from '../services/sheet-float-image.service';

export class JnpfFacadeSheetsFloatImage {
  constructor(private readonly _jnpfSheetsFloatImageService: JnpfSheetsFloatImageService) {}

  savePiniaStoreId(value: any) {
    this._jnpfSheetsFloatImageService.savePiniaStoreId(value);
  }

  clearFocusDrawingId() {
    this._jnpfSheetsFloatImageService.clearFocusDrawingId();
  }
}
