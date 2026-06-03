import { type ICommand, type IAccessor, CommandType } from '@univerjs/core';
import { JnpfSheetsExcelFileService } from '../../services/sheet-excel-file.service';
import { JnpfCommandIds } from '../../utils/define';

export const JnpfSheetsImportExcelFileOperation: ICommand = {
  id: JnpfCommandIds.importExcelFile,
  type: CommandType.OPERATION,
  handler: async (_: IAccessor) => {
    return true;
  },
};

export const JnpfSheetsDownloadExcelFileOperation: ICommand = {
  id: JnpfCommandIds.downloadExcelFile,
  type: CommandType.OPERATION,
  handler: async (_: IAccessor) => {
    return true;
  },
};

export const JnpfSheetsImportCsvFileOperation: ICommand = {
  id: JnpfCommandIds.importCsvFile,
  type: CommandType.OPERATION,
  handler: async (accessor: IAccessor) => {
    const jnpfSheetsExcelFileService = accessor.get(JnpfSheetsExcelFileService);
    jnpfSheetsExcelFileService?.handleImportCsv();

    return true;
  },
};
