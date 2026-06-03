import { type Dependency, UniverInstanceType, Plugin, Inject, Injector, DependentOn, LocaleService } from '@univerjs/core';
import { UniverSheetsUIPlugin } from '@univerjs/sheets-ui';
import { JnpfSheetsPreviewController } from '../controllers/sheet-preview.controller';
import zhCN from '../locales/zh-CN';

@DependentOn(UniverSheetsUIPlugin)
export class JnpfSheetsPreviewPlugin extends Plugin {
  static override pluginName = 'JNPF_SHEET_PREVIEW_PLUGIN';
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
    ([[JnpfSheetsPreviewController]] as Dependency[]).forEach(d => this._injector.add(d));

    this._injector.get(JnpfSheetsPreviewController);
  }
}
