import { Disposable, Inject } from '@univerjs/core';
import { ComponentManager } from '@univerjs/ui';

import univerDialogSelectCellComponent from '../components/Dialog/selectCell.vue';
import { JnpfUniverDialogSelectCellKey } from '../utils/define';

export class JnpfSheetsDialogController extends Disposable {
  constructor(@Inject(ComponentManager) private readonly _componentManager: ComponentManager) {
    super();

    this._registerComponents();
  }

  private _registerComponents(): void {
    // 注册选区组件进来
    this.disposeWithMe(this._componentManager.register(JnpfUniverDialogSelectCellKey, univerDialogSelectCellComponent, { framework: 'vue3' }));
  }
}
