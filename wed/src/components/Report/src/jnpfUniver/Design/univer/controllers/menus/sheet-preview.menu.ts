import { IMenuButtonItem, MenuItemType } from '@univerjs/ui';
import { JnpfSheetsPreviewOperation } from '../../commands/operations/sheet-preview.operation';

export const JnpfSheetsPreviewMenuFactory = (): IMenuButtonItem => {
  return {
    id: JnpfSheetsPreviewOperation.id,
    type: MenuItemType.BUTTON,
    icon: 'ViewModeSingle',
    tooltip: 'jnpfSheetPreviewMenu.tooltip',
    title: 'jnpfSheetPreviewMenu.title',
  };
};
