import { Disposable, ICommandService, Inject } from '@univerjs/core';
import { ComponentManager, IMenuManagerService, RibbonStartGroup } from '@univerjs/ui';
import { DirectExportSingle, UploadSingle, UpperFloorSingle } from '@univerjs/icons';

import {
  JnpfSheetsDownloadExcelFileOperation,
  JnpfSheetsImportCsvFileOperation,
  JnpfSheetsImportExcelFileOperation,
} from '../commands/operations/sheet-excel-file.operation';

import { JnpfSheetsExcelFileMenuFactory, JnpfSheetsImportExcelFileMenuFactory } from '../controllers/menus/sheet-excel-file.menu';
import { JnpfCommandIds } from '../utils/define';

export class JnpfSheetsExcelFileController extends Disposable {
  constructor(
    @ICommandService private readonly _commandService: ICommandService,
    @IMenuManagerService private readonly _menuManagerService: IMenuManagerService,
    @Inject(ComponentManager) private readonly _componentManager: ComponentManager,
  ) {
    super();

    this._initCommands();
    this._registerComponents();
    this._initMenus();
  }

  private _initCommands(): void {
    [JnpfSheetsImportExcelFileOperation, JnpfSheetsDownloadExcelFileOperation, JnpfSheetsImportCsvFileOperation].forEach(command => {
      this.disposeWithMe(this._commandService.registerCommand(command));
    });
  }

  private _registerComponents(): void {
    // 注册按钮图标
    this.disposeWithMe(this._componentManager.register('DirectExportSingle', DirectExportSingle));
    this.disposeWithMe(this._componentManager.register('UploadSingle', UploadSingle));
    // this.disposeWithMe(this._componentManager.register('ExportSingle', ExportSingle));
    this.disposeWithMe(this._componentManager.register('UpperFloorSingle', UpperFloorSingle));
  }

  private _initMenus(): void {
    this._menuManagerService.mergeMenu({
      [RibbonStartGroup.OTHERS]: {
        [JnpfCommandIds.excelFileOperations]: {
          order: 100,
          menuItemFactory: JnpfSheetsExcelFileMenuFactory,
          [JnpfSheetsImportExcelFileOperation.id]: {
            order: 0,
            menuItemFactory: JnpfSheetsImportExcelFileMenuFactory,
          },
          // [JnpfSheetsDownloadExcelFileOperation.id]: {
          //   order: 1,
          //   menuItemFactory: JnpfSheetsDownloadExcelFileMenuFactory,
          // },
          // [JnpfSheetsImportCsvFileOperation.id]: {
          //   order: 2,
          //   menuItemFactory: JnpfSheetsImportCsvFileMenuFactory,
          // },
        },
      },
    });
  }
}
