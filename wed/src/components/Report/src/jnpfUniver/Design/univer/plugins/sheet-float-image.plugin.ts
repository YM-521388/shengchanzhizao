import { type Dependency, UniverInstanceType, Plugin, Inject, Injector, DependentOn, LocaleService } from '@univerjs/core';
import { UniverSheetsUIPlugin } from '@univerjs/sheets-ui';
import { JnpfSheetsFloatImageController } from '../controllers/sheet-float-image.controller';
import { JnpfSheetsFloatImageService } from '../services/sheet-float-image.service';
import zhCN from '../locales/zh-CN';

@DependentOn(UniverSheetsUIPlugin)
export class JnpfSheetsFloatImagePlugin extends Plugin {
  static override pluginName = 'JNPF_SHEET_FLOAT_IMAGE_PLUGIN';
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
    ([[JnpfSheetsFloatImageController], [JnpfSheetsFloatImageService]] as Dependency[]).forEach(d => this._injector.add(d));

    this._injector.get(JnpfSheetsFloatImageController);
  }
}
