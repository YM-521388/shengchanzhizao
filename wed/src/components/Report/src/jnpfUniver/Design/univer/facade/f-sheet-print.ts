import { JnpfSheetsPrintService } from '../services/sheet-print.service';

export class JnpfFacadeSheetsPrint {
  constructor(private readonly _jnpfSheetsPrintService: JnpfSheetsPrintService) {}

  getLayouts(config: any) {
    return this._jnpfSheetsPrintService.getLayouts(config);
  }
}
