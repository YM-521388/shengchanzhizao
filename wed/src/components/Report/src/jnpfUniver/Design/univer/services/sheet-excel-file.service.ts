import { Disposable, ICellData, ICommandService, IUniverInstanceService, UniverInstanceType, Workbook } from '@univerjs/core';
import { SetRangeValuesCommand } from '@univerjs/sheets';

export class JnpfSheetsExcelFileService extends Disposable {
  constructor(
    @IUniverInstanceService private readonly _univerInstanceService: IUniverInstanceService,
    @ICommandService private readonly _commandService: ICommandService,
  ) {
    super();
  }

  public handleImportCsv() {
    // 获取当前工作表
    const sheet = this._univerInstanceService.getCurrentUnitForType<Workbook>(UniverInstanceType.UNIVER_SHEET)!.getActiveSheet();

    // 等待用户选择 CSV 文件
    this._waitUserSelectCSVFile(({ data, rowsCount, colsCount }) => {
      // 设置工作表大小
      sheet.setColumnCount(colsCount);
      sheet.setRowCount(rowsCount);

      // 设置工作表数据
      this._commandService
        .executeCommand(SetRangeValuesCommand.id, {
          range: {
            startColumn: 0, // 起始列索引
            startRow: 0, // 起始行索引
            endColumn: colsCount - 1, // 结束列索引
            endRow: rowsCount - 1, // 结束行索引
          },
          value: this._parseCSV2UniverData(data),
        })
        .then();
    });
  }

  /**
   * 等待用户选择 CSV 文件
   */
  private _waitUserSelectCSVFile(onSelect: (data: { data: string[][]; colsCount: number; rowsCount: number }) => void) {
    const input = document.createElement('input');
    input.type = 'file';
    input.accept = '.csv';
    input.click();

    input.onchange = () => {
      const file = input.files?.[0];
      if (!file) return;
      const reader = new FileReader();
      reader.onload = () => {
        const text = reader.result;
        if (typeof text !== 'string') return;

        // 提示：使用 npm 包来解析 CSV
        const rows = text.split(/\r\n|\n/);
        const data = rows.map(line => line.split(','));

        const colsCount = data.reduce((max, row) => Math.max(max, row.length), 0);

        onSelect({
          data,
          colsCount,
          rowsCount: data.length,
        });
      };
      reader.readAsText(file);
    };
  }

  /**
   * 将 CSV 解析为 Univer 数据
   * @param csv CSV 数据
   * @returns { v: string }[][] 返回解析后的数据
   */
  private _parseCSV2UniverData(csv: string[][]): ICellData[][] {
    return csv.map(row => {
      return row.map(cell => {
        return {
          v: cell || '',
        };
      });
    });
  }
}
