import { Injector, Univer, IUniverInstanceService, ICommandService, FUniver, LifecycleService } from '@univerjs/core';
import '@univerjs/sheets/facade';
import '@univerjs/ui/facade';
import '@univerjs/docs-ui/facade';
import '@univerjs/sheets-ui/facade';

import '@univerjs/sheets-filter/facade';
import '@univerjs/sheets-data-validation/facade';
import '@univerjs/sheets-hyper-link/facade';
import '@univerjs/sheets-hyper-link-ui/facade';
import '@univerjs/watermark/facade';
import '@univerjs/sheets-thread-comment/facade';
import '@univerjs/sheets-crosshair-highlight/facade';

import { JnpfFacadeSheetsCell } from '../facade/f-sheet-cell';
import { JnpfSheetsCellService } from '../services/sheet-cell.service';

import { JnpfFacadeSheetsRange } from '../facade/f-sheet-range';
import { JnpfSheetsRangeService } from '../services/sheet-range.service';

import { JnpfFacadeSheetsFloatDom } from '../facade/f-sheet-float-dom';
import { JnpfSheetsFloatDomService } from '../services/sheet-float-dom.service';

import { JnpfFacadeSheetsFloatEchart } from './f-sheet-float-echart';
import { JnpfSheetsFloatEchartService } from '../services/sheet-float-echart.service';

import { JnpfFacadeSheetsCellEchart } from '../facade/f-sheet-cell-echart';
import { JnpfSheetsCellEchartService } from '../services/sheet-cell-echart.service';

import { JnpfFacadeSheetsFloatImage } from './f-sheet-float-image';
import { JnpfSheetsFloatImageService } from '../services/sheet-float-image.service';

import { JnpfFacadeSheetsPrint } from '../facade/f-sheet-print';
import { JnpfSheetsPrintService } from '../services/sheet-print.service';

export class JnpfFUniver extends FUniver {
  static newAPI(wrapped: Univer | Injector): JnpfFUniver {
    const injector = wrapped instanceof Univer ? wrapped.__getInjector() : wrapped;
    const commandService = injector.get<ICommandService>(ICommandService);
    const instanceService = injector.get<IUniverInstanceService>(IUniverInstanceService);

    if (!commandService || !instanceService) {
      throw new Error('注入ICommandService或IUniverInstanceService发生错误');
    }

    return new JnpfFUniver(injector, commandService, instanceService);
  }

  constructor(injector: Injector, commandService: ICommandService, instanceService: IUniverInstanceService) {
    const lifecycleService = injector.get<LifecycleService>(LifecycleService);
    if (!lifecycleService) {
      throw new Error('LifecycleService 未正确注入');
    }

    super(injector, commandService, instanceService, lifecycleService);
  }

  getInjector() {
    return this._injector;
  }

  getSheetsCell(): JnpfFacadeSheetsCell | null {
    const jnpfSheetsCellService = this._injector.get(JnpfSheetsCellService);

    if (!jnpfSheetsCellService) {
      return null;
    }

    return this._injector.createInstance(JnpfFacadeSheetsCell, jnpfSheetsCellService);
  }

  getSheetsRange(): JnpfFacadeSheetsRange | null {
    const jnpfSheetsRangeService = this._injector.get(JnpfSheetsRangeService);

    if (!jnpfSheetsRangeService) {
      return null;
    }

    return this._injector.createInstance(JnpfFacadeSheetsRange, jnpfSheetsRangeService);
  }

  getSheetsFloatDom(): JnpfFacadeSheetsFloatDom | null {
    const jnpfSheetsFloatDomService = this._injector.get(JnpfSheetsFloatDomService);

    if (!jnpfSheetsFloatDomService) {
      return null;
    }

    return this._injector.createInstance(JnpfFacadeSheetsFloatDom, jnpfSheetsFloatDomService);
  }

  getSheetsFloatEchart(): JnpfFacadeSheetsFloatEchart | null {
    const jnpfSheetsFloatEchartService = this._injector.get(JnpfSheetsFloatEchartService);

    if (!jnpfSheetsFloatEchartService) {
      return null;
    }

    return this._injector.createInstance(JnpfFacadeSheetsFloatEchart, jnpfSheetsFloatEchartService);
  }

  getSheetsCellEchart(): JnpfFacadeSheetsCellEchart | null {
    const jnpfSheetsCellEchartService = this._injector.get(JnpfSheetsCellEchartService);

    if (!jnpfSheetsCellEchartService) {
      return null;
    }

    return this._injector.createInstance(JnpfFacadeSheetsCellEchart, jnpfSheetsCellEchartService);
  }

  getSheetsFloatImage(): JnpfFacadeSheetsFloatImage | null {
    const jnpfSheetsFloatImageService = this._injector.get(JnpfSheetsFloatImageService);

    if (!jnpfSheetsFloatImageService) {
      return null;
    }

    return this._injector.createInstance(JnpfFacadeSheetsFloatImage, jnpfSheetsFloatImageService);
  }

  getSheetsPrint(): JnpfFacadeSheetsPrint | null {
    const jnpfSheetsPrintService = this._injector.get(JnpfSheetsPrintService);

    if (!jnpfSheetsPrintService) {
      return null;
    }

    return this._injector.createInstance(JnpfFacadeSheetsPrint, jnpfSheetsPrintService);
  }
}
