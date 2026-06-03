import { Disposable, ICommandService, Inject } from '@univerjs/core';
import { ComponentManager, IMenuManagerService, RibbonStartGroup } from '@univerjs/ui';
import { ChartSingle, LineChartSingle, PieChartSingle, RadarChartSingle, SystemSingle } from '@univerjs/icons';

import {
  JnpfSheetsInsertedFloatEchartOperation,
  JnpfSheetsInsertFloatBarEchartOperation,
  JnpfSheetsInsertFloatLineEchartOperation,
  JnpfSheetsInsertFloatPieEchartOperation,
  JnpfSheetsInsertFloatRadarEchartOperation,
  JnpfSheetsFocusEchartOperation,
} from '../commands/operations/sheet-float-echart.operation';

import {
  JnpfSheetsFloatEchartMenuFactory,
  JnpfSheetsInsertFloatBarEchartMenuFactory,
  JnpfSheetsInsertFloatLineEchartMenuFactory,
  JnpfSheetsInsertFloatPieEchartMenuFactory,
  JnpfSheetsInsertFloatRadarEchartMenuFactory,
} from './menus/sheet-float-echart.menu';

import univerFloatEchartComponent from '../components/Echart/float.vue';

import { JnpfCommandIds, JnpfUniverFloatEchartKey } from '../utils/define';

export class JnpfSheetsFloatEchartController extends Disposable {
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
    [
      JnpfSheetsInsertFloatBarEchartOperation,
      JnpfSheetsInsertFloatLineEchartOperation,
      JnpfSheetsInsertFloatPieEchartOperation,
      JnpfSheetsInsertFloatRadarEchartOperation,
      JnpfSheetsInsertedFloatEchartOperation,
      JnpfSheetsFocusEchartOperation,
    ].forEach(command => {
      this.disposeWithMe(this._commandService.registerCommand(command));
    });
  }

  private _registerComponents(): void {
    // 注册图表组件进来
    this.disposeWithMe(this._componentManager.register(JnpfUniverFloatEchartKey, univerFloatEchartComponent, { framework: 'vue3' }));

    // 注册按钮图标
    this.disposeWithMe(this._componentManager.register('SystemSingle', SystemSingle));
    this.disposeWithMe(this._componentManager.register('ChartSingle', ChartSingle));
    this.disposeWithMe(this._componentManager.register('LineChartSingle', LineChartSingle));
    this.disposeWithMe(this._componentManager.register('PieChartSingle', PieChartSingle));
    this.disposeWithMe(this._componentManager.register('RadarChartSingle', RadarChartSingle));
  }

  private _initMenus(): void {
    this._menuManagerService.mergeMenu({
      // 主菜单
      [RibbonStartGroup.OTHERS]: {
        [JnpfCommandIds.floatEchartOperations]: {
          order: 10,
          menuItemFactory: JnpfSheetsFloatEchartMenuFactory,
          [JnpfSheetsInsertFloatBarEchartOperation.id]: {
            order: 0,
            menuItemFactory: JnpfSheetsInsertFloatBarEchartMenuFactory,
          },
          [JnpfSheetsInsertFloatLineEchartOperation.id]: {
            order: 1,
            menuItemFactory: JnpfSheetsInsertFloatLineEchartMenuFactory,
          },
          [JnpfSheetsInsertFloatPieEchartOperation.id]: {
            order: 2,
            menuItemFactory: JnpfSheetsInsertFloatPieEchartMenuFactory,
          },
          [JnpfSheetsInsertFloatRadarEchartOperation.id]: {
            order: 3,
            menuItemFactory: JnpfSheetsInsertFloatRadarEchartMenuFactory,
          },
        },
      },
    });
  }
}
