import { Disposable, Inject } from '@univerjs/core';
import { SheetCanvasFloatDomManagerService } from '@univerjs/sheets-drawing-ui';

export class JnpfSheetsFloatDomService extends Disposable {
  private _piniaStoreId: string | null = null;

  private _floatImageItems: Record<string, any> = {};
  private _floatEchartItems: Record<string, any> = {};

  constructor(@Inject(SheetCanvasFloatDomManagerService) private readonly _sheetCanvasFloatDomManagerService: SheetCanvasFloatDomManagerService) {
    super();

    this._addFloatDomHook();
  }

  // 存入图表列表数据
  public saveFloatEchartItems(data: any) {
    this._floatEchartItems = data;
  }

  // 存入图片列表数据
  public saveFloatImageItems(data: any) {
    this._floatImageItems = data;
  }

  // 存入store的id
  public savePiniaStoreId(data: any) {
    this._piniaStoreId = data;
  }

  // 增加Hook
  private _addFloatDomHook() {
    this.disposeWithMe(
      this._sheetCanvasFloatDomManagerService.addHook({
        onGetFloatDomProps: (id: string) => {
          if (this._floatImageItems?.hasOwnProperty(id)) {
            const { domId } = this._floatImageItems[id] ?? {};
            return { id: domId, piniaStoreId: this._piniaStoreId };
          }

          if (this._floatEchartItems?.hasOwnProperty(id)) {
            const { domId } = this._floatEchartItems[id] ?? {};
            return { id: domId, piniaStoreId: this._piniaStoreId };
          }

          return {
            id,
          };
        },
      }),
    );
  }
}
