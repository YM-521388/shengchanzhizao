import { ref } from 'vue';
import { defineStore } from 'pinia';
import { getAlphabetFromIndexRule, isNullOrUndefined } from '../univer/utils';

// 初始状态的默认值
const defaultFocusFloatDomData = {
  domId: undefined,
  drawingId: undefined,
  option: {},
};

export const useJnpfUniverStore = (id: string) => {
  return defineStore(id, () => {
    const jnpfUniverApiCache: any = ref(null); // 整个api实例
    const univerCreateModeCache: any = ref(null); // univer创建模式

    const focusedFloatEchartDataCache = ref({ ...defaultFocusFloatDomData }); // 已聚焦的悬浮图表数据
    const floatEchartDataCaches: Record<string, any> = ref({}); // 工作表中所有悬浮图表的数据
    const floatEchartToUniverImgDataCaches: Record<string, any> = ref({}); // 工作表中所有悬浮图表转成univer图片后的数据

    const focusedFloatImageDataCache = ref({ ...defaultFocusFloatDomData }); // 已聚焦的悬浮图片数据
    const floatImageDataCaches: Record<string, any> = ref({}); // 工作表中所有悬浮图片的数据
    const floatImageToUniverImgDataCaches: Record<string, any> = ref({}); // 工作表中所有悬浮图片转成univer图片后的数据

    const dialogSelectCellDataCache: any = ref(null); // 弹窗选择区域的单元格信息
    const dialogSelectCellStateCache: any = ref(null); // 弹窗选择单元格状态 --- 值为时间戳，用于触发页面的状态更新

    // 重置存储状态为初始值
    const resetStore = () => {
      jnpfUniverApiCache.value = null;
      univerCreateModeCache.value = null;

      focusedFloatEchartDataCache.value = { ...defaultFocusFloatDomData };
      floatEchartDataCaches.value = {};
      floatEchartToUniverImgDataCaches.value = {};

      focusedFloatImageDataCache.value = { ...defaultFocusFloatDomData };
      floatImageDataCaches.value = {};
      floatImageToUniverImgDataCaches.value = {};

      dialogSelectCellDataCache.value = null;
      dialogSelectCellStateCache.value = null;
    };

    // 存储univer创建模式
    const setUniverCreateModeCache = (type: string) => {
      univerCreateModeCache.value = type;
    };

    // 存储jnpfUniverApi
    const setJnpfUniverApiCache = (value: any) => {
      jnpfUniverApiCache.value = value ?? null;
    };

    // 根据drawingId获取悬浮图表信息
    const getFloatEchartDataByDrawingId = (drawingId: string) => {
      return floatEchartDataCaches.value?.[drawingId] ?? null;
    };

    // 更新已聚焦的悬浮图表数据
    const updateFocusedFloatEchartDataCache = (value: any = {}) => {
      focusedFloatEchartDataCache.value = value;
    };

    // 存储工作本中所有悬浮图表的数据
    const setFloatEchartDataCaches = (value: any = {}) => {
      floatEchartDataCaches.value = value;

      jnpfUniverApiCache.value?.getSheetsFloatDom()?.saveFloatEchartItems(value);
    };

    // 存储工作本中所有悬浮图表转成univer图片后的数据
    const setFloatEchartToUniverImgDataCaches = (value: any = {}) => {
      floatEchartToUniverImgDataCaches.value = value;
    };

    // 根据drawingId获取悬浮图片信息
    const getFloatImageDataByDrawingId = (drawingId: string) => {
      return floatImageDataCaches.value?.[drawingId] ?? null;
    };

    // 更新已聚焦的悬浮图片数据
    const updateFocusedFloatImageDataCache = (deliverConfig: any = {}) => {
      focusedFloatImageDataCache.value = deliverConfig;
    };

    // 存储工作本中所有悬浮图片的数据
    const setFloatImageDataCaches = (value: any = {}) => {
      floatImageDataCaches.value = value;

      jnpfUniverApiCache.value?.getSheetsFloatDom()?.saveFloatImageItems(value);
    };

    // 存储工作本中所有悬浮图片转成univer图片后的数据
    const setFloatImageToUniverImgDataCaches = (value: any) => {
      floatImageToUniverImgDataCaches.value = value;
    };

    // 存储弹窗选择单元格的值
    const setDialogSelectCellDataCache = (value: any) => {
      if (!value || isNullOrUndefined(value?.startRow) || isNullOrUndefined(value?.startColumn)) {
        dialogSelectCellDataCache.value = null;
        return;
      }

      dialogSelectCellDataCache.value = `${getAlphabetFromIndexRule(value.startColumn)}${value.startRow + 1}`;
    };

    // 取消弹窗选择单元格结果
    const cancelDialogSelectCell = () => {
      setDialogSelectCellDataCache(null);

      dialogSelectCellStateCache.value = new Date().getTime();
    };

    // 确认弹窗选择单元格结果
    const confirmDialogSelectCell = () => {
      dialogSelectCellStateCache.value = new Date().getTime();
    };

    return {
      univerCreateModeCache,

      focusedFloatEchartDataCache,
      floatEchartDataCaches,
      floatEchartToUniverImgDataCaches,

      focusedFloatImageDataCache,
      floatImageDataCaches,
      floatImageToUniverImgDataCaches,

      dialogSelectCellDataCache,
      dialogSelectCellStateCache,

      setUniverCreateModeCache,
      setJnpfUniverApiCache,

      getFloatEchartDataByDrawingId,
      updateFocusedFloatEchartDataCache,
      setFloatEchartDataCaches,
      setFloatEchartToUniverImgDataCaches,

      getFloatImageDataByDrawingId,
      updateFocusedFloatImageDataCache,
      setFloatImageDataCaches,
      setFloatImageToUniverImgDataCaches,

      setDialogSelectCellDataCache,
      cancelDialogSelectCell,
      confirmDialogSelectCell,

      resetStore,
    };
  })();
};
