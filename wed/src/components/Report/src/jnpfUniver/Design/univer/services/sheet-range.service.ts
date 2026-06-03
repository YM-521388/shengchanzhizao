import { SetSelectionsOperation } from '@univerjs/sheets';
import { Disposable, ICommandService, IUniverInstanceService, UniverInstanceType, Workbook } from '@univerjs/core';

export class JnpfSheetsRangeService extends Disposable {
  constructor(
    @IUniverInstanceService private readonly _univerInstanceService: IUniverInstanceService,
    @ICommandService private readonly _commandService: ICommandService,
  ) {
    super();
  }

  // 恢复选区
  public recoveryRange(row: number, col: number) {
    const unit = this._univerInstanceService.getCurrentUnitForType<Workbook>(UniverInstanceType.UNIVER_SHEET);
    const unitId = unit!.getUnitId();
    const sheet = unit!.getActiveSheet();
    const subUnitId = sheet?.getSheetId();

    this._commandService
      .executeCommand(SetSelectionsOperation.id, {
        unitId,
        subUnitId,
        selections: [
          {
            range: {
              startRow: row,
              startColumn: col,
              endRow: row,
              endColumn: col,
              rangeType: 0,
            },
          },
        ],
        type: 2,
      })
      .then();
  }
}
