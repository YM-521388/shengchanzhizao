import { JnpfSheetsRangeService } from '../services/sheet-range.service';

export class JnpfFacadeSheetsRange {
  constructor(private readonly _jnpfSheetsRangeService: JnpfSheetsRangeService) {}

  recoveryRange(row: number, col: number) {
    return this._jnpfSheetsRangeService?.recoveryRange(row, col);
  }
}
