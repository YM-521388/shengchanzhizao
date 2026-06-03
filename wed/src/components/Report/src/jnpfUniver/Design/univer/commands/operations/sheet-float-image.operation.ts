import { type ICommand, type IAccessor, CommandType } from '@univerjs/core';
import { JnpfSheetsFloatImageService } from '../../services/sheet-float-image.service';
import { JnpfCommandIds } from '../../utils/define';

export const JnpfSheetsInsertFloatImageOperation: ICommand = {
  id: JnpfCommandIds.insertFloatImage,
  type: CommandType.OPERATION,
  handler: async (accessor: IAccessor) => {
    const jnpfSheetsFloatImageService = accessor.get(JnpfSheetsFloatImageService);
    jnpfSheetsFloatImageService.insertFloatImage();

    return true;
  },
};

export const JnpfSheetsInsertedFloatImageOperation: ICommand = {
  id: JnpfCommandIds.insertedFloatImage,
  type: CommandType.OPERATION,
  handler: async (_: IAccessor) => {
    return true;
  },
};

export const JnpfSheetsFocusFloatImageOperation: ICommand = {
  id: JnpfCommandIds.focusFloatImage,
  type: CommandType.OPERATION,
  handler: async (_: IAccessor) => {
    return true;
  },
};
