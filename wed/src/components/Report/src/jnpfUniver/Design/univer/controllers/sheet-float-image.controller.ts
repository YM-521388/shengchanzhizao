import { Disposable, ICommandService, Inject } from '@univerjs/core';
import { ComponentManager, IMenuManagerService, RibbonStartGroup } from '@univerjs/ui';
import { SHEETS_IMAGE_MENU_ID } from '@univerjs/sheets-drawing-ui';

import {
  JnpfSheetsInsertFloatImageOperation,
  JnpfSheetsInsertedFloatImageOperation,
  JnpfSheetsFocusFloatImageOperation,
} from '../commands/operations/sheet-float-image.operation';
import { JnpfSheetsInsertFloatImageMenuFactory } from './menus/sheet-float-image.menu';

import univerFloatImageComponent from '../components/Image/float.vue';

import { JnpfUniverFloatImageKey } from '../utils/define';

export class JnpfSheetsFloatImageController extends Disposable {
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
    [JnpfSheetsInsertFloatImageOperation, JnpfSheetsInsertedFloatImageOperation, JnpfSheetsFocusFloatImageOperation].forEach(command => {
      this.disposeWithMe(this._commandService.registerCommand(command));
    });
  }

  private _registerComponents(): void {
    // 注册图片组件进来
    this.disposeWithMe(this._componentManager.register(JnpfUniverFloatImageKey, univerFloatImageComponent, { framework: 'vue3' }));
  }

  private _initMenus(): void {
    this._menuManagerService.mergeMenu({
      [RibbonStartGroup.FORMULAS_INSERT]: {
        [SHEETS_IMAGE_MENU_ID]: {
          [JnpfSheetsInsertFloatImageOperation.id]: {
            order: 10,
            menuItemFactory: JnpfSheetsInsertFloatImageMenuFactory,
          },
        },
      },
    });
  }
}
