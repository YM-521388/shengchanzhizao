import { type ICommand, type IAccessor, CommandType } from '@univerjs/core';
import { JnpfCommandIds } from '../../utils/define';

export const JnpfSheetsPreviewOperation: ICommand = {
  id: JnpfCommandIds.preview,
  type: CommandType.OPERATION,
  handler: async (_: IAccessor) => {
    return true;
  },
};
