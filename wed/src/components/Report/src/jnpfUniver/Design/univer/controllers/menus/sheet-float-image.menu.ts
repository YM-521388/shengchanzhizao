import { IMenuButtonItem, MenuItemType } from '@univerjs/ui';
import { JnpfSheetsInsertFloatImageOperation } from '../../commands/operations/sheet-float-image.operation';

export const JnpfSheetsInsertFloatImageMenuFactory = (): IMenuButtonItem => {
  return {
    id: JnpfSheetsInsertFloatImageOperation.id,
    type: MenuItemType.BUTTON,
    tooltip: 'jnpfSheetInsertFloatImageMenu.tooltip',
    title: 'jnpfSheetInsertFloatImageMenu.title',
  };
};
