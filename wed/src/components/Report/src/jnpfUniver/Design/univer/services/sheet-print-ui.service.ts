import { Inject, Injector, Disposable, IUniverInstanceService, type Workbook } from '@univerjs/core';
import { SheetPrintInterceptorService, SheetSkeletonManagerService } from '@univerjs/sheets-ui';
import { UniverType } from '@univerjs/protocol';
import {
  CanvasRenderMode,
  DEFAULT_FONTFACE_PLANE,
  Engine,
  IRenderManagerService,
  Scene,
  SHEET_VIEWPORT_KEY,
  Spreadsheet,
  SpreadsheetSkeleton,
  Viewport,
} from '@univerjs/engine-render';
import { JnpfPrintAlignEnum, JnpfPrintOtherParamsEnum } from '../utils/define';
import { buildUUID } from '../utils/uuid';

export class JnpfSheetsPrintUiService extends Disposable {
  private _univerInstanceService: IUniverInstanceService;
  private _sheetPrintInterceptorService: SheetPrintInterceptorService;
  private readonly _skeleton!: SpreadsheetSkeleton;

  private _containerEle = document.createElement('div');
  private readonly _sceneKey: string;

  private _layoutConfig: any;
  private readonly _printConfig: any;
  private readonly _previewContainerScale: number;

  private _engine!: Engine;
  private _scene!: Scene;

  private _spreadsheetObject: any;

  private _mainViewport!: Viewport;
  private _leftViewport!: Viewport;
  private _topViewport!: Viewport;
  private _leftTopViewport!: Viewport;

  private _dirty: boolean = false;

  private _totalWidth: number = 0;
  private _totalHeight: number = 0;

  constructor(
    @Inject(Injector) private readonly _accessor: Injector,
    layoutConfig = {},
    printConfig = {},
    previewContainerScale = 1, // 打印预览容器的伸缩比
  ) {
    super();

    this._univerInstanceService = this._getUniverInstanceService();
    this._sheetPrintInterceptorService = this._getSheetPrintInterceptorService();

    this._layoutConfig = layoutConfig as any;
    this._printConfig = printConfig as any;
    this._previewContainerScale = previewContainerScale;

    const { unitId, subUnitId } = this._layoutConfig;
    this._sceneKey = this._getSceneKey(unitId, subUnitId);

    const skeleton = this._getSkeleton(unitId, subUnitId);
    if (!skeleton) {
      return;
    }
    this._skeleton = skeleton;

    this._initRender(); // 初始化渲染
  }

  // 获取容器元素
  get container() {
    return this._containerEle;
  }

  /**
   * 获取到univerInstanceService
   */
  private _getUniverInstanceService() {
    return this._accessor.get(IUniverInstanceService);
  }

  /**
   * 获取到sheetPrintInterceptorService
   */
  private _getSheetPrintInterceptorService() {
    return this._accessor.get(SheetPrintInterceptorService);
  }

  /**
   * 获得场景专一key
   * @param unitId
   * @param subUnitId
   */
  private _getSceneKey(unitId: string, subUnitId: string) {
    return `${unitId}_${subUnitId}_${buildUUID()}`;
  }

  /**
   * 获取到skeleton
   * @param unitId
   * @param subUnitId
   */
  private _getSkeleton(unitId: string, subUnitId: string) {
    return this._accessor.get(IRenderManagerService)?.getRenderById(unitId)?.with(SheetSkeletonManagerService)?.getOrCreateSkeleton({ sheetId: subUnitId });
  }

  /**
   * 获取纸张容器的尺寸
   */
  private _getPaperContainerSize() {
    // 从布局配置中解构出纸张尺寸（宽度 w 和高度 h）
    const { w = 0, h = 0 } = this._layoutConfig?.paperSize ?? {};

    // 检查宽度和高度是否有效
    if (w <= 0 || h <= 0) {
      console.warn('纸张尺寸配置无效，宽度或高度小于等于0。');
    }

    // 根据预览容器的缩放比例，计算实际渲染的尺寸
    const scaledWidth = w * this._previewContainerScale; // 宽度按缩放比例调整
    const scaledHeight = h * this._previewContainerScale; // 高度按缩放比例调整

    // 返回计算后的尺寸对象
    return {
      w: scaledWidth,
      h: scaledHeight,
    };
  }

  /**
   * 获取纸张边距（Padding）
   */
  private _getPaperPadding() {
    const { top = 0, right = 0, bottom = 0, left = 0 } = this._layoutConfig?.paperPadding ?? {};
    const previewContainerScale = this._previewContainerScale;

    return {
      top: top * previewContainerScale,
      bottom: bottom * previewContainerScale,
      left: left * previewContainerScale,
      right: right * previewContainerScale,
    };
  }

  /**
   * 将电子表格对象添加到场景中
   */
  private _addSpreadsheetToScene() {
    // 创建电子表格对象，传入唯一标识符、骨架对象，并设置初始化选项
    const spreadsheet = new Spreadsheet('__JnpfSheetPrintRender__', this._skeleton, false);

    // 将电子表格设置为打印模式，确保以打印样式进行渲染
    spreadsheet.isPrinting = true;

    // 将电子表格对象添加到当前场景中，纳入渲染流程
    this._scene.addObject(spreadsheet);

    // 缓存电子表格对象，便于后续访问或操作
    this._spreadsheetObject = spreadsheet;

    // 从布局配置中获取单元 ID 和子单元 ID，用于拦截器逻辑
    const { unitId, subUnitId } = this._layoutConfig ?? {};

    // 获取拦截器实例与拦截点，确保存在后再进行触发
    const interceptor = this._sheetPrintInterceptorService?.interceptor;
    const interceptPoints = interceptor?.getInterceptPoints();
    const printCollectPoint = interceptPoints?.PRINTING_COMPONENT_COLLECT;

    // 如果拦截器和拦截点存在，则触发拦截逻辑
    if (interceptor && printCollectPoint) {
      interceptor.fetchThroughInterceptors(printCollectPoint)(
        undefined, // 没有特定请求参数，传入 undefined
        { unitId, subUnitId, scene: this._scene }, // 将单元 ID、子单元 ID 和场景信息传递给拦截器
      );
    }
  }

  /**
   * 添加并初始化多个视口（Viewport）到场景中
   */
  private _addViewport() {
    // 从视口键名常量中解构出各个视口的标识符
    const { VIEW_MAIN, VIEW_MAIN_LEFT, VIEW_MAIN_TOP, VIEW_MAIN_LEFT_TOP } = SHEET_VIEWPORT_KEY;

    // 创建主视口，负责渲染主要内容区域
    this._mainViewport = new Viewport(VIEW_MAIN, this._scene);

    // 创建左侧视口，负责渲染左侧固定区域
    this._leftViewport = new Viewport(VIEW_MAIN_LEFT, this._scene);

    // 创建顶部视口，负责渲染顶部固定区域
    this._topViewport = new Viewport(VIEW_MAIN_TOP, this._scene);

    // 创建左上角视口，负责渲染左上角交叉固定区域
    this._leftTopViewport = new Viewport(VIEW_MAIN_LEFT_TOP, this._scene);
  }

  /**
   * 初始化渲染引擎、场景、视口，并配置缩放比例和资源管理。
   */
  private _initRender() {
    // 获取纸张容器的宽度和高度，确保渲染区域的尺寸准确
    const { w: paperContainerW, h: paperContainerH } = this._getPaperContainerSize();

    // 设置像素比为 1，通常用于标准屏幕显示
    const pixelRatio = 1;

    // 设置渲染模式为打印模式（Printing），确保画布的内容适配打印需求
    const renderMode = CanvasRenderMode.Printing;

    // 创建渲染引擎实例
    // 参数：宽度、高度、像素比、渲染模式
    this._engine = new Engine(paperContainerW, paperContainerH, pixelRatio, renderMode);

    // 创建场景对象，并将其与渲染引擎关联
    // 参数：场景的唯一标识符、渲染引擎实例
    this._scene = new Scene(this._sceneKey, this._engine);

    // 将渲染引擎绑定到指定的 DOM 容器中
    // 参数：容器元素、是否清除现有内容（false 表示不清除）
    this._engine.setContainer(this._containerEle, false);

    // 从布局配置中获取打印缩放比例，若未配置则默认为 1
    const { printScale = 1 } = this._layoutConfig;

    // 计算场景的最终缩放比例，结合打印缩放比例和预览容器的缩放比例
    const sceneScale = printScale * this._previewContainerScale;

    // 将计算得到的缩放比例应用到场景中
    // 参数：x 方向缩放比例，y 方向缩放比例
    this._scene.scale(sceneScale, sceneScale);

    // 将电子表格组件添加到场景中
    this._addSpreadsheetToScene();

    // 创建视口并与当前场景关联，确保视口正确渲染内容
    this._addViewport();

    // 注册资源销毁回调，确保在对象生命周期结束时释放资源，防止内存泄漏
    this.disposeWithMe({
      dispose: () => {
        // 释放引擎资源，确保渲染引擎的内存被回收
        this._engine.dispose();

        // 释放场景资源，确保场景相关的内存被回收
        this._scene.dispose();
      },
    });
  }

  /**
   * 调整视口大小和滚动值
   */
  private _resizeViewport() {
    // 从布局配置中解构冻结和范围设置
    const { freeze, range, printScale = 1 } = this._layoutConfig;
    // 从冻结设置中解构x轴和y轴的分割点以及起始行列
    const { xSplit, ySplit, startRow, startColumn } = freeze;
    // 从骨架配置中解构行头宽度和列头高度
    const { rowHeaderWidth, columnHeaderHeight } = this._skeleton;

    // 缩放场景以适应预览容器的缩放比例
    const sceneScale = printScale * this._previewContainerScale;
    this._scene.scale(sceneScale, sceneScale);

    // 获取无合并单元格的位置，基于起始行列
    const cellPosition = this._skeleton?.getNoMergeCellPositionByIndexWithNoHeader(startRow, startColumn);
    // 获取无合并单元格的位置，基于起始行列减去分割点
    const cellPositionSplit = this._skeleton?.getNoMergeCellPositionByIndexWithNoHeader(startRow - ySplit, startColumn - xSplit);

    // 计算起始X分割位置
    const startXSplit = xSplit > 0 ? cellPosition.startX - cellPositionSplit.startX : 0;
    // 计算起始Y分割位置
    const startYSplit = ySplit > 0 ? cellPosition.startY - cellPositionSplit.startY : 0;

    // 获取场景的精度缩放比例
    const precisionScale = this._scene.getPrecisionScale();
    // 定义用于缩放X值的函数
    const getScaleX = (value: number) => Math.round(value * precisionScale.scaleX) / precisionScale.scaleX;
    // 定义用于缩放Y值的函数
    const getScaleY = (value: number) => Math.round(value * precisionScale.scaleY) / precisionScale.scaleY;

    // 获取范围起始单元格的位置
    const startCellPosition = this._skeleton.getNoMergeCellPositionByIndexWithNoHeader(range.startRow, range.startColumn);
    // 计算结束单元格的位置，并处理边界情况
    const endCellPosition = this._skeleton.getNoMergeCellPositionByIndexWithNoHeader(range.endRow, range.endColumn);

    // 定义范围对象，包含起始和结束的X、Y坐标
    const _range = {
      startX: xSplit > 0 ? cellPositionSplit.startX : 0,
      endX: xSplit > 0 ? cellPosition.startX : 0,
      startY: ySplit > 0 ? cellPositionSplit.startY : 0,
      endY: ySplit > 0 ? cellPosition.startY : 0,
    };

    const endX = Math.max(0, getScaleX(endCellPosition.endX) - Math.max(startCellPosition.startX, _range.endX));
    const endY = Math.max(0, getScaleY(endCellPosition.endY) - getScaleY(Math.max(startCellPosition.startY, _range.endY)));
    // 计算缩放比例
    const scale = 1 / Math.max(precisionScale.scaleX, precisionScale.scaleY);

    this._totalWidth = startXSplit + endX + scale;
    this._totalHeight = startYSplit + endY + scale;

    const startSplit = { x: startXSplit, y: startYSplit };

    const scaleXY = {
      x: getScaleX(endCellPosition.endX) - getScaleX(endX) - getScaleX(Math.max(startCellPosition.startX, _range.endX)),
      y: getScaleY(endCellPosition.endY) - getScaleY(endY) - getScaleY(Math.max(startCellPosition.startY, _range.endY)),
    };

    // 如果x轴和y轴都有分割点，启用左上角视口并调整其大小和滚动值
    if (xSplit > 0 && ySplit > 0) {
      this._leftTopViewport.enable();
      this._leftTopViewport.resizeWhenFreezeChange({
        top: 0,
        left: 0,
        height: getScaleY(startSplit.y),
        width: getScaleX(startSplit.x),
      });
      this._leftTopViewport.updateScrollVal({
        viewportScrollX: getScaleX(_range.startX) + getScaleX(rowHeaderWidth),
        viewportScrollY: getScaleY(_range.startY) + getScaleY(columnHeaderHeight),
      });
    } else {
      // 否则禁用左上角视口
      this._leftTopViewport.disable();
    }

    // 如果x轴有分割点，启用左视口并调整其大小和滚动值
    if (xSplit > 0) {
      this._leftViewport.enable();
      this._leftViewport.resizeWhenFreezeChange({
        top: getScaleY(startSplit.y),
        left: 0,
        height: getScaleY(endY) + scale + scaleXY.y,
        width: getScaleX(startSplit.x),
      });
      this._leftViewport.updateScrollVal({
        viewportScrollX: getScaleX(_range.startX) + getScaleX(rowHeaderWidth),
        viewportScrollY: getScaleY(startCellPosition.startY) + getScaleY(columnHeaderHeight) - getScaleY(startSplit.y),
      });
    } else {
      // 否则禁用左视口
      this._leftViewport.disable();
    }

    // 如果y轴有分割点，启用顶视口并调整其大小和滚动值
    if (ySplit > 0) {
      this._topViewport.enable();
      this._topViewport.resizeWhenFreezeChange({
        top: 0,
        left: getScaleX(startSplit.x),
        height: getScaleY(startSplit.y),
        width: getScaleX(endX) + scale + scaleXY.x,
      });
      this._topViewport.updateScrollVal({
        viewportScrollX: getScaleX(startCellPosition.startX) + getScaleX(rowHeaderWidth) - getScaleX(startSplit.x),
        viewportScrollY: getScaleY(_range.startY) + getScaleY(columnHeaderHeight),
      });
    } else {
      // 否则禁用顶视口
      this._topViewport.disable();
    }

    // 调整主视口的大小和滚动值
    this._mainViewport.resizeWhenFreezeChange({
      top: getScaleY(startSplit.y),
      left: getScaleX(startSplit.x),
      height: getScaleY(endY) + scale + scaleXY.y,
      width: getScaleX(endX) + scale + scaleXY.x,
    });

    this._mainViewport.updateScrollVal({
      viewportScrollX: getScaleX(startCellPosition.startX) + getScaleX(rowHeaderWidth) - getScaleX(startSplit.x),
      viewportScrollY: getScaleY(startCellPosition.startY) + getScaleY(columnHeaderHeight) - getScaleY(startSplit.y),
    });
  }

  /**
   * 获取平移偏移量，用于将打印内容定位到页面上的正确位置
   *
   * 根据纸张的尺寸、内边距、打印配置中的对齐方式以及总内容的宽度和高度，计算水平和垂直方向的偏移量。
   * 最终返回的偏移量用于定位打印内容的起始位置，以便它能够根据配置进行正确的对齐。
   */
  private _getTranslateOffset() {
    // 获取纸张的内边距和纸张容器的尺寸
    const paperContainerSize = this._getPaperContainerSize();
    const paperPadding = this._getPaperPadding();

    // 解构获取打印配置中的对齐方式（水平对齐和垂直对齐）
    const { vAlign, hAlign } = this._printConfig;

    // 计算最大缩放比例，确保内容在X和Y方向上都能够缩放适应
    const scale = Math.max(this._scene.scaleX, this._scene.scaleY);

    // 计算水平偏移量（offsetX）
    let offsetX: number;
    if (hAlign === JnpfPrintAlignEnum.start) {
      // 水平对齐方式为左对齐，偏移量等于左边距
      offsetX = paperPadding.left;
    } else if (hAlign === JnpfPrintAlignEnum.end) {
      // 水平对齐方式为右对齐，偏移量等于纸张宽度减去内容宽度和右边距
      offsetX = paperContainerSize.w - this._totalWidth * scale - paperPadding.right;
    } else {
      // 水平对齐方式为居中对齐，计算居中位置
      offsetX = paperPadding.left + (paperContainerSize.w - this._totalWidth * scale - paperPadding.left - paperPadding.right) / 2;
    }

    // 计算垂直偏移量（offsetY）
    let offsetY: number;
    if (vAlign === JnpfPrintAlignEnum.start) {
      // 垂直对齐方式为上对齐，偏移量等于上边距
      offsetY = paperPadding.top;
    } else if (vAlign === JnpfPrintAlignEnum.end) {
      // 垂直对齐方式为下对齐，偏移量等于纸张高度减去内容高度和下边距
      offsetY = paperContainerSize.h - this._totalHeight * scale - paperPadding.bottom;
    } else {
      // 垂直对齐方式为居中对齐，计算居中位置
      offsetY = (paperContainerSize.h - this._totalHeight * scale - paperPadding.top - paperPadding.bottom) / 2 + paperPadding.top;
    }

    // 返回四舍五入后的偏移量，确保偏移值为整数
    return { offsetX: Math.round(offsetX), offsetY: Math.round(offsetY) };
  }

  /**
   * 清空引擎内画布
   */
  private _clearEngineCanvas() {
    this._engine?.clearCanvas();
  }

  /**
   * 渲染主场景
   */
  private _renderMainScene() {
    // 如果场景被标记为脏，需要重新调整视口大小
    this._dirty && this._resizeViewport();

    // 获取Canvas的绘图上下文
    const context = this._engine.getCanvas().getContext();

    // 获取打印配置中的网格线设置和偏移量
    const { gridlines } = this._printConfig;
    const { offsetX, offsetY } = this._getTranslateOffset();

    // 根据网格线设置禁用或启用网格线
    this._spreadsheetObject.setForceDisableGridlines(!gridlines);

    // 强制标记表格为脏，以便重新渲染
    this._spreadsheetObject.makeForceDirty();
    this._scene.makeDirty();

    // 保存当前绘图上下文状态
    context.save();
    // 应用偏移量
    context.translateWithPrecision(offsetX, offsetY);

    // 渲染场景
    this._scene.render();

    // 恢复绘图上下文状态
    context.restore();
  }

  /**
   * 渲染页眉和页脚
   */
  private _renderHeaderFooter() {
    // 从布局配置中解构获取单元ID和子单元ID
    const { unitId, subUnitId } = this._layoutConfig ?? {};

    // 获取工作簿对象
    const workbook = this._univerInstanceService?.getUnit<Workbook>(unitId, UniverType.UNIVER_SHEET);
    if (!workbook) return; // 如果未找到工作簿，直接返回

    // 获取工作表对象
    const worksheet = workbook?.getSheetBySheetId(subUnitId);
    if (!worksheet) return; // 如果未找到工作表，直接返回

    // 获取Canvas上下文对象
    const ctx = this._engine?.getCanvas()?.getContext();
    ctx.save(); // 保存当前绘图状态
    ctx.font = `13px ${DEFAULT_FONTFACE_PLANE}`; // 设置字体样式

    // 获取纸张尺寸并计算绘制位置
    const { w, h } = this._getPaperContainerSize();
    const x = 20 * this._previewContainerScale;
    const y = 20 * this._previewContainerScale;

    // 获取页眉页脚的参数
    const { headerFooterParams, workbookTitleText } = this._printConfig ?? {};

    // 渲染日期和时间
    if (headerFooterParams?.includes(JnpfPrintOtherParamsEnum?.printDate) || headerFooterParams?.includes(JnpfPrintOtherParamsEnum?.printTime)) {
      let dateStr = new Date().toLocaleString();
      if (headerFooterParams?.includes(JnpfPrintOtherParamsEnum?.printDate) && !headerFooterParams?.includes(JnpfPrintOtherParamsEnum?.printTime)) {
        dateStr = new Date().toLocaleDateString();
      } else if (headerFooterParams?.includes(JnpfPrintOtherParamsEnum?.printTime) && !headerFooterParams?.includes(JnpfPrintOtherParamsEnum?.printDate)) {
        dateStr = new Date().toLocaleTimeString();
      }

      ctx.fillText(dateStr, x, h - y); // 绘制时间/日期
    }

    // 渲染工作簿标题
    if (headerFooterParams?.includes(JnpfPrintOtherParamsEnum?.workbookTitle)) {
      const text = ctx.measureText(workbookTitleText); // 测量文本宽度
      const length = text.fontBoundingBoxAscent + text.fontBoundingBoxDescent; // 计算文本高度
      ctx.fillText(workbookTitleText, x, y + length); // 绘制工作簿标题
    }

    // 渲染工作表名称
    if (headerFooterParams?.includes(JnpfPrintOtherParamsEnum?.worksheetTitle)) {
      const name = worksheet?.getName() ?? '';
      const text = ctx.measureText(name);
      const length = text.fontBoundingBoxAscent + text.fontBoundingBoxDescent;
      ctx.fillText(name, w - (text.width + x), y + length); // 绘制工作表名称到右上角
    }

    // 渲染页码
    if (headerFooterParams?.includes(JnpfPrintOtherParamsEnum?.pageNumber)) {
      const name = `${this._layoutConfig?.pageNumber}`;
      if (name) {
        const text = ctx.measureText(name); // 测量页码文本宽度
        ctx.fillText(name, w - (text.width + x), h - y); // 绘制页码到右下角
      }
    }

    ctx.restore(); // 恢复绘图状态
  }

  /**
   * 标记当前状态为脏（需要重新渲染）
   */
  setDirty(state: boolean) {
    this._dirty = state;
  }

  /**
   * 渲染页面
   */
  renderPage() {
    this._clearEngineCanvas(); // 清除画布

    this._renderMainScene(); // 渲染主场景(核心内容)
    this._renderHeaderFooter(); // 渲染页眉页脚
  }

  /**
   * 监听变换并在准备好时渲染
   */
  renderOnReady() {
    const observable = this._engine?.onTransformChange$?.subscribeEvent(() => {
      this.renderPage();
    });

    this.disposeWithMe(observable); // 订阅事件并在销毁时清理
  }

  /**
   * 释放资源
   */
  dispose() {
    super.dispose(); // 调用父类的dispose方法

    if (this._containerEle.parentElement) {
      this._containerEle.parentElement?.removeChild(this._containerEle); // 移除容器
    }
  }
}
