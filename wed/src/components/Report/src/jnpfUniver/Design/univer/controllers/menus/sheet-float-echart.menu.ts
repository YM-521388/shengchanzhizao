import { IMenuButtonItem, IMenuSelectorItem, MenuItemType } from '@univerjs/ui';
import {
  JnpfSheetsInsertFloatBarEchartOperation,
  JnpfSheetsInsertFloatLineEchartOperation,
  JnpfSheetsInsertFloatPieEchartOperation,
  JnpfSheetsInsertFloatRadarEchartOperation,
} from '../../commands/operations/sheet-float-echart.operation';
import { JnpfCommandIds } from '../../utils/define';

export const JnpfSheetsFloatEchartMenuFactory = (): IMenuSelectorItem => {
  return {
    id: JnpfCommandIds.floatEchartOperations,
    type: MenuItemType.SUBITEMS,
    icon: 'SystemSingle',
    tooltip: 'jnpfSheetFloatEchartMenu.tooltip',
  };
};

export const JnpfSheetsInsertFloatBarEchartMenuFactory = (): IMenuButtonItem => {
  return {
    id: JnpfSheetsInsertFloatBarEchartOperation.id,
    type: MenuItemType.BUTTON,
    icon: 'ChartSingle',
    tooltip: 'jnpfSheetInsertFloatBarEchartMenu.tooltip',
    title: 'jnpfSheetInsertFloatBarEchartMenu.title',
  };
};

export const JnpfSheetsInsertFloatLineEchartMenuFactory = (): IMenuButtonItem => {
  return {
    id: JnpfSheetsInsertFloatLineEchartOperation.id,
    type: MenuItemType.BUTTON,
    icon: 'LineChartSingle',
    tooltip: 'jnpfSheetInsertFloatLineEchartMenu.tooltip',
    title: 'jnpfSheetInsertFloatLineEchartMenu.title',
  };
};

export const JnpfSheetsInsertFloatPieEchartMenuFactory = (): IMenuButtonItem => {
  return {
    id: JnpfSheetsInsertFloatPieEchartOperation.id,
    type: MenuItemType.BUTTON,
    icon: 'PieChartSingle',
    tooltip: 'jnpfSheetInsertFloatPieEchartMenu.tooltip',
    title: 'jnpfSheetInsertFloatPieEchartMenu.title',
  };
};

export const JnpfSheetsInsertFloatRadarEchartMenuFactory = (): IMenuButtonItem => {
  return {
    id: JnpfSheetsInsertFloatRadarEchartOperation.id,
    type: MenuItemType.BUTTON,
    icon: 'RadarChartSingle',
    tooltip: 'jnpfSheetInsertFloatRadarEchartMenu.tooltip',
    title: 'jnpfSheetInsertFloatRadarEchartMenu.title',
  };
};
