import { type Dependency, UniverInstanceType, Plugin, Inject, Injector, DependentOn, LocaleService } from '@univerjs/core';
import { UniverSheetsUIPlugin } from '@univerjs/sheets-ui';
import { JnpfSheetsPrintService } from '../services/sheet-print.service';
import { JnpfSheetsPrintUiService } from '../services/sheet-print-ui.service';
import zhCN from '../locales/zh-CN';

@DependentOn(UniverSheetsUIPlugin)
export class JnpfSheetsPrintPlugin extends Plugin {
  static override pluginName = 'JNPF_SHEET_PRINT_PLUGIN';
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
    ([[JnpfSheetsPrintService], [JnpfSheetsPrintUiService]] as Dependency[]).forEach(d => this._injector.add(d));
  }
}
