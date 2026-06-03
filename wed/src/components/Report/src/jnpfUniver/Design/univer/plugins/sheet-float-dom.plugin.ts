import { type Dependency, UniverInstanceType, Plugin, Inject, Injector, DependentOn, LocaleService } from '@univerjs/core';
import { UniverSheetsUIPlugin } from '@univerjs/sheets-ui';
import { JnpfSheetsFloatDomService } from '../services/sheet-float-dom.service';
import zhCN from '../locales/zh-CN';

@DependentOn(UniverSheetsUIPlugin)
export class JnpfSheetsFloatDomPlugin extends Plugin {
  static override pluginName = 'JNPF_SHEET_FLOAT_DOM_PLUGIN';
  static override type = UniverInstanceType.UNIVER_SHEET;

  constructor(
    @Inject(Injector) protected readonly _injector: Injector,
    @Inject(LocaleService) private readonly _localeService: LocaleService,
  ) {
    super();

    this._localeService.load({
      zhCN,
    });
  }

  override onStarting(): void {
    ([[JnpfSheetsFloatDomService]] as Dependency[]).forEach(d => this._injector.add(d));
  }
}
