import { type ICommand, type IAccessor, CommandType } from '@univerjs/core';
import { JnpfSheetsFloatEchartService } from '../../services/sheet-float-echart.service';
import { JnpfCommandIds } from '../../utils/define';

export const JnpfSheetsInsertedFloatEchartOperation: ICommand = {
  id: JnpfCommandIds.insertedFloatEchart,
  type: CommandType.OPERATION,
  handler: async (_: IAccessor) => {
    return true;
  },
};

export const JnpfSheetsInsertFloatBarEchartOperation: ICommand = {
  id: JnpfCommandIds.insertFloatBarEchart,
  type: CommandType.OPERATION,
  handler: async (accessor: IAccessor) => {
    const jnpfSheetsFloatEchartService = accessor.get(JnpfSheetsFloatEchartService);
    jnpfSheetsFloatEchartService.insertFloatEchart('JnpfUniverFloatBarEchart');

    return true;
  },
};

export const JnpfSheetsInsertFloatLineEchartOperation: ICommand = {
  id: JnpfCommandIds.insertFloatLineEchart,
  type: CommandType.OPERATION,
  handler: async (accessor: IAccessor) => {
    const jnpfSheetsFloatEchartService = accessor.get(JnpfSheetsFloatEchartService);
    jnpfSheetsFloatEchartService.insertFloatEchart('JnpfUniverFloatLineEchart');

    return true;
  },
};

export const JnpfSheetsInsertFloatPieEchartOperation: ICommand = {
  id: JnpfCommandIds.insertFloatPieEchart,
  type: CommandType.OPERATION,
  handler: async (accessor: IAccessor) => {
    const jnpfSheetsFloatEchartService = accessor.get(JnpfSheetsFloatEchartService);
    jnpfSheetsFloatEchartService.insertFloatEchart('JnpfUniverFloatPieEchart');

    return true;
  },
};

export const JnpfSheetsInsertFloatRadarEchartOperation: ICommand = {
  id: JnpfCommandIds.insertFloatRadarEchart,
  type: CommandType.OPERATION,
  handler: async (accessor: IAccessor) => {
    const jnpfSheetsFloatEchartService = accessor.get(JnpfSheetsFloatEchartService);
    jnpfSheetsFloatEchartService.insertFloatEchart('JnpfUniverFloatRadarEchart');

    return true;
  },
};

export const JnpfSheetsFocusEchartOperation: ICommand = {
  id: JnpfCommandIds.focusFloatEchart,
  type: CommandType.OPERATION,
  handler: async (_: IAccessor) => {
    return true;
  },
};
