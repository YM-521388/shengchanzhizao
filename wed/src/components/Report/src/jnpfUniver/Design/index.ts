import { ICellData, IWorkbookData } from '@univerjs/core';
import * as echarts from 'echarts';

export interface DesignStateProps {
  containerEleId: any;

  univer: any;
  jnpfUniverAPI: any;

  univerCreateMode: 'design' | 'preview' | 'print';

  activeWorkbook: any;
  activeWorkbookId: any;
  activeWorksheet: any;
  activeWorksheetId: any;

  selectionChangeMonitor: any;
  beforeCommandExecuteMonitor: any;
  commandExecuteMonitor: any;

  configDialogCellTarget: null;
  openDialogCell: boolean;
  dialogCellInstance: any;

  loading: boolean;
  [prop: string]: any;
}

export interface CreateUnitProps {
  mode?: 'design' | 'preview' | 'print';

  snapshot?: IWorkbookData;
  floatEcharts?: Record<string, any>;
  cellEcharts?: Record<string, any>;
  floatImages?: Record<string, any>;

  uiHeader?: boolean;
  uiFooter?: boolean;
  uiContextMenu?: boolean;

  readonly?: boolean;

  defaultActiveSheetId?: string;

  loading?: boolean;
}

export interface DeliverFloatEchartOptionProps {
  drawingId: string;
  echartType: 'bar' | 'line' | 'pie';
  option: echarts.EChartsOption;
}

export interface DeliverFloatImageOptionProps {
  drawingId: string;
  imageType: 'BASE64' | 'URL';
  option: any;
}

export interface DeliverCellDataProps {
  startColumn: number;
  startRow: number;
  cellData: ICellData;
}
