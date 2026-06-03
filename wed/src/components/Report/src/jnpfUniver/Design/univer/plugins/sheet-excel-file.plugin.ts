import { type Dependency, UniverInstanceType, Plugin, Inject, Injector, DependentOn, LocaleService } from '@univerjs/core';
import { UniverSheetsUIPlugin } from '@univerjs/sheets-ui';
import { JnpfSheetsExcelFileController } from '../controllers/sheet-excel-file.controller';
import { JnpfSheetsExcelFileService } from '../services/sheet-excel-file.service';
import zhCN from '../locales/zh-CN';

@DependentOn(UniverSheetsUIPlugin)
export class JnpfSheetsExcelFilePlugin extends Plugin {
  static override pluginName = 'JNPF_SHEET_EXCEL_FILE_PLUGIN';
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
    ([[JnpfSheetsExcelFileController], [JnpfSheetsExcelFileService]] as Dependency[]).forEach(d => this._injector.add(d));

    this._injector.get(JnpfSheetsExcelFileController);
  }
}
