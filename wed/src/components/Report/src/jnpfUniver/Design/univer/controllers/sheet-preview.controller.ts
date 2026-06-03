import { Disposable, ICommandService, Inject } from '@univerjs/core';
import { ComponentManager, IMenuManagerService, RibbonStartGroup } from '@univerjs/ui';
import { ViewModeSingle } from '@univerjs/icons';

import { JnpfSheetsPreviewOperation } from '../commands/operations/sheet-preview.operation';
import { JnpfSheetsPreviewMenuFactory } from '../controllers/menus/sheet-preview.menu';

export class JnpfSheetsPreviewController extends Disposable {
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
    [JnpfSheetsPreviewOperation].forEach(command => {
      this.disposeWithMe(this._commandService.registerCommand(command));
    });
  }

  private _registerComponents(): void {
    // 注册按钮图标
    this.disposeWithMe(this._componentManager.register('ViewModeSingle', ViewModeSingle));
  }

  private _initMenus(): void {
    this._menuManagerService.mergeMenu({
      // 主菜单
      [RibbonStartGroup.OTHERS]: {
        [JnpfSheetsPreviewOperation.id]: {
          order: 99,
          menuItemFactory: JnpfSheetsPreviewMenuFactory,
        },
      },
      // 右键菜单
      // [ContextMenuPosition.MAIN_AREA]: {
      //   [ContextMenuGroup.OTHERS]: {
      //     [JnpfSheetsPreviewOperation.id]: {
      //       order: 0,
      //       menuItemFactory: JnpfSheetsPreviewMenuFactory,
      //     },
      //   },
      // },
    });
  }
}
