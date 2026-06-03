import { IMenuButtonItem, IMenuSelectorItem, MenuItemType } from '@univerjs/ui';
import {
  JnpfSheetsDownloadExcelFileOperation,
  JnpfSheetsImportCsvFileOperation,
  JnpfSheetsImportExcelFileOperation,
} from '../../commands/operations/sheet-excel-file.operation';
import { JnpfCommandIds } from '../../utils/define';

export const JnpfSheetsExcelFileMenuFactory = (): IMenuSelectorItem => {
  return {
    id: JnpfCommandIds.excelFileOperations,
    type: MenuItemType.SUBITEMS,
    icon: 'DirectExportSingle',
    tooltip: 'jnpfSheetExcelFileMenu.tooltip',
  };
};

export const JnpfSheetsImportExcelFileMenuFactory = (): IMenuButtonItem => {
  return {
    id: JnpfSheetsImportExcelFileOperation.id,
    type: MenuItemType.BUTTON,
    icon: 'UploadSingle',
    tooltip: 'jnpfSheetImportExcelFileMenu.tooltip',
    title: 'jnpfSheetImportExcelFileMenu.title',
  };
};

export const JnpfSheetsDownloadExcelFileMenuFactory = (): IMenuButtonItem => {
  return {
    id: JnpfSheetsDownloadExcelFileOperation.id,
    type: MenuItemType.BUTTON,
    icon: 'ExportSingle',
    tooltip: 'jnpfSheetDownloadExcelFileMenu.tooltip',
    title: 'jnpfSheetDownloadExcelFileMenu.title',
  };
};

export const JnpfSheetsImportCsvFileMenuFactory = (): IMenuButtonItem => {
  return {
    id: JnpfSheetsImportCsvFileOperation.id,
    type: MenuItemType.BUTTON,
    icon: 'UpperFloorSingle',
    tooltip: 'jnpfSheetImportCsvFileMenu.tooltip',
    title: 'jnpfSheetImportCsvFileMenu.title',
  };
};
