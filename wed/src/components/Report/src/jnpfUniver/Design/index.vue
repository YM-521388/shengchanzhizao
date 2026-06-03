<template>
  <div class="jnpf-univer-design-content" v-loading="loading">
    <div class="univer-design-container" :id="containerEleId"></div>
  </div>
  <div id="cellEchartsContent" class="cell-echarts-content">
    <JnpfUniverCellEchart ref="jnpfUniverCellEchartRef" :univer-create-mode="univerCreateMode" />
  </div>
</template>

<script lang="ts" setup>
  import '@univerjs/design/lib/index.css';
  import '@univerjs/ui/lib/index.css';
  import '@univerjs/docs-ui/lib/index.css';
  import '@univerjs/sheets-ui/lib/index.css';
  import '@univerjs/sheets-formula-ui/lib/index.css';
  import '@univerjs/sheets-numfmt-ui/lib/index.css';

  import '@univerjs/sheets-filter-ui/lib/index.css';
  import '@univerjs/sheets-sort-ui/lib/index.css';
  import '@univerjs/sheets-data-validation-ui/lib/index.css';
  import '@univerjs/sheets-conditional-formatting-ui/lib/index.css';
  import '@univerjs/sheets-hyper-link-ui/lib/index.css';
  import '@univerjs/drawing-ui/lib/index.css';
  import '@univerjs/sheets-drawing-ui/lib/index.css';
  import '@univerjs/thread-comment-ui/lib/index.css';
  import '@univerjs/find-replace/lib/index.css';
  import '@univerjs/sheets-crosshair-highlight/lib/index.css';
  import '@univerjs/sheets-zen-editor/lib/index.css';
  import '@univerjs/uniscript/lib/index.css';

  import { IWorkbookData } from '@univerjs/core';
  import { SetWorksheetActiveOperation, WorkbookEditablePermission, SetSelectionsOperation } from '@univerjs/sheets';
  import { DeviceInputEventType } from '@univerjs/engine-render';
  import { JnpfFUniver } from './univer/facade';

  import { reactive, ref, toRefs, unref, watch } from 'vue';
  import { storeToRefs } from 'pinia';
  import { useJnpfUniverStore } from './store';

  import JnpfUniverCellEchart from '../Design/univer/components/Echart/cell.vue';

  import {
    JnpfCommandIds,
    DefaultFloatEchartOptions,
    DefaultFloatImageOption,
    JnpfUniverDialogSelectCellKey,
    DefaultDialogSelectCellConfig,
    jnpfPrintAreaOptions,
    jnpfPaperTypeOptions,
    jnpfPrintDirectionOptions,
    jnpfPrintScaleOptions,
    jnpfPrintVAlignOptions,
    jnpfPrintHAlignOptions,
    jnpfPaperPaddingTypeOptions,
    DefaultPrintConfig,
  } from './univer/utils/define';
  import { base64ToFile, isEmptyObject, isNullOrUndefined } from './univer/utils';
  import {
    createUniverInstance,
    analysisCreateUnitData,
    arrangeDesignWorkbookData,
    arrangePreviewWorkbookData,
    judgeSheetHasCustomFloatDom,
    onInsertedFloatEchart,
    onInsertedFloatImage,
  } from './univer/utils/univer';
  import { buildUUID } from './univer/utils/uuid';
  import { CreateUnitProps, DeliverCellDataProps, DeliverFloatEchartOptionProps, DeliverFloatImageOptionProps, DesignStateProps } from './index';

  defineOptions({ name: 'JnpfUniverDesign' });
  const emits = defineEmits(['preview', 'focusFloatEchart', 'focusFloatImage', 'changeCell', 'changeDialogSelectCell']);

  const state = reactive<DesignStateProps>({
    containerEleId: null,
    univer: null,
    jnpfUniverAPI: null,

    univerCreateMode: 'design',

    activeWorkbook: null,
    activeWorkbookId: null,
    activeWorksheet: null,
    activeWorksheetId: null,

    selectionChangeMonitor: null,
    beforeCommandExecuteMonitor: null,
    commandExecuteMonitor: null,

    configDialogCellTarget: null,
    openDialogCell: false,
    dialogCellInstance: null,

    selectCellInfo: {
      row: undefined,
      column: undefined,
    },

    loading: false,
  });
  const {
    containerEleId,
    univer,
    jnpfUniverAPI,
    univerCreateMode,
    activeWorkbook,
    activeWorkbookId,
    activeWorksheet,
    activeWorksheetId,
    selectionChangeMonitor,
    beforeCommandExecuteMonitor,
    commandExecuteMonitor,
    configDialogCellTarget,
    openDialogCell,
    dialogCellInstance,
    selectCellInfo,
    loading,
  } = toRefs(state);

  const dynamicJnpfUniverStoreId = `jnpfUniver_${buildUUID()}`;
  state.containerEleId = dynamicJnpfUniverStoreId;

  const jnpfUniverStore = useJnpfUniverStore(dynamicJnpfUniverStoreId);
  const { dialogSelectCellStateCache, dialogSelectCellDataCache } = storeToRefs(jnpfUniverStore);

  const jnpfUniverCellEchartRef = ref();

  // 创建工作簿
  function handleCreateDesignUnit(options?: CreateUnitProps) {
    // 销毁实例
    handleDisposeUnit();

    const {
      mode = 'design',
      snapshot: workbookData,
      floatEcharts = {},
      floatImages = {},

      uiHeader = true,
      uiFooter = true,
      uiContextMenu = true,

      readonly: isReadonly = false,

      defaultActiveSheetId,

      loading: createLoading = false,
    } = options ?? {};

    state.univerCreateMode = mode;
    state.loading = createLoading;

    const containerEle = getDesignContainerEle();
    if (!containerEle) {
      console.warn('实例构建失败，无法找到设计器容器');
      return;
    }

    const realWorkbookData = !workbookData ? {} : analysisCreateUnitData(workbookData, univerCreateMode.value, isReadonly);

    const univerInstance = createUniverInstance({
      container: containerEle,
      header: isReadonly ? false : uiHeader,
      footer: uiFooter,
      contextMenu: uiContextMenu,
      workbookData: realWorkbookData,
    });

    univer.value = univerInstance;
    jnpfUniverAPI.value = JnpfFUniver.newAPI(univerInstance);

    activeWorkbook.value = jnpfUniverAPI.value?.getActiveWorkbook();
    activeWorkbookId.value = activeWorkbook.value?.getId();

    // 改变默认激活的工作表
    if (defaultActiveSheetId) {
      jnpfUniverAPI.value?.executeCommand(SetWorksheetActiveOperation.id, { unitId: activeWorkbookId.value, subUnitId: defaultActiveSheetId });
    }

    activeWorksheet.value = activeWorkbook.value?.getActiveSheet();
    activeWorksheetId.value = activeWorksheet.value?.getSheetId();

    jnpfUniverStore?.setUniverCreateModeCache(univerCreateMode.value);
    jnpfUniverStore?.setJnpfUniverApiCache(jnpfUniverAPI.value);

    jnpfUniverAPI.value?.getSheetsFloatDom()?.savePiniaStoreId(dynamicJnpfUniverStoreId);
    jnpfUniverAPI.value?.getSheetsFloatEchart()?.savePiniaStoreId(dynamicJnpfUniverStoreId);
    jnpfUniverAPI.value?.getSheetsFloatImage()?.savePiniaStoreId(dynamicJnpfUniverStoreId);

    jnpfUniverStore?.setFloatEchartDataCaches(floatEcharts);
    jnpfUniverStore?.setFloatImageDataCaches(floatImages);

    setWorkbookPermission(isReadonly);
    bindUniverLifeCycle(realWorkbookData);

    bindSelectionChange(isReadonly);
    bindBeforeCommandExecute();
    bindAfterCommandExecuted();

    return {
      jnpfUniverAPI: jnpfUniverAPI.value,
    };
  }

  // 设置unit权限
  function setWorkbookPermission(isReadonly: boolean) {
    if (univerCreateMode.value === 'design' && !isReadonly) {
      return;
    }

    updateWorkbookPermission(false);
  }

  // 更新权限控制
  function updateWorkbookPermission(state: boolean) {
    const permission = jnpfUniverAPI.value?.getPermission();

    permission?.setWorkbookPermissionPoint(activeWorkbookId.value, WorkbookEditablePermission, state);
    permission?.setPermissionDialogVisible(state);
  }

  // 绑定univer的生命周期 文档地址：https://univer.ai/zh-CN/guides/sheet/features/core/general-api
  function bindUniverLifeCycle(workbookData: IWorkbookData) {
    const univerHooks = jnpfUniverAPI.value?.getHooks();
    const univerSheetHooks = jnpfUniverAPI.value?.getSheetHooks();

    univerHooks?.onStarting(() => {});
    univerHooks?.onReady(() => {});
    univerHooks?.onRendered(() => {
      if (!jnpfUniverAPI.value) return;
      // 0.5.3版本开始初始的情况下，不会执行onSelectionChange事件
      jnpfUniverAPI.value?.executeCommand(SetSelectionsOperation.id, {
        unitId: activeWorkbookId.value,
        subUnitId: activeWorksheetId.value,
        selections: [
          {
            range: {
              startRow: 0,
              startColumn: 0,
              endRow: 0,
              endColumn: 0,
              rangeType: 0,
            },
          },
        ],
        type: 2,
      });
    });
    univerHooks?.onSteady(() => {
      // 判断当前激活的工作表有没有图表
      const { resources = [] } = workbookData ?? {};
      const activeWorksheetHasFloatDomState = resources.some((resource: any) => {
        if (resource?.name !== 'SHEET_DRAWING_PLUGIN') {
          return false;
        }

        const parsedResourceData = JSON.parse(resource?.data || '{}');
        const currentSheetResourceOption = parsedResourceData?.[activeWorksheetId.value];

        return currentSheetResourceOption?.data && judgeSheetHasCustomFloatDom(currentSheetResourceOption.data);
      });

      // 根据结果更新 Loading 状态
      if (activeWorksheetHasFloatDomState) {
        setTimeout(() => {
          loading.value = false;
        }, 500);
      } else {
        loading.value = false;
      }
    });
  }

  // 绑定选择变更事件
  function bindSelectionChange(isReadonly: boolean) {
    if (univerCreateMode.value !== 'design') {
      selectionChangeMonitor.value = null;

      return;
    }

    selectionChangeMonitor.value = activeWorkbook.value?.onSelectionChange((params: any) => {
      if (!params?.length) {
        return;
      }

      // 业务逻辑只选取第一选区的第一个单元格
      const firstCell = params[0] ?? {};
      const { startRow, startColumn } = firstCell;
      const firstCellData = jnpfUniverAPI.value?.getSheetsCell()?.getCellData(startColumn, startRow) ?? {};

      if (!isReadonly) {
        if (openDialogCell.value) {
          jnpfUniverStore?.setDialogSelectCellDataCache({
            startRow,
            startColumn,
          });
        } else {
          selectCellInfo.value = {
            row: startRow,
            column: startColumn,
          };

          emits('changeCell', {
            startRow,
            startColumn,
            cellData: firstCellData,
          });
        }
      }
    });
  }

  // 绑定监听命令执行之前
  function bindBeforeCommandExecute() {
    beforeCommandExecuteMonitor.value = jnpfUniverAPI.value?.onBeforeCommandExecute((_command: any) => {});
  }

  // 绑定监听命令执行之后
  function bindAfterCommandExecuted() {
    commandExecuteMonitor.value = jnpfUniverAPI.value?.onCommandExecuted((command: any) => {
      const { id: commandId } = command ?? {};

      // 切换工作本
      if (commandId === 'sheet.operation.set-worksheet-active' && activeWorkbook.value) {
        activeWorksheet.value = activeWorkbook.value?.getActiveSheet();
        activeWorksheetId.value = activeWorksheet.value?.getSheetId();
      }

      // 插入悬浮图表
      if (commandId === JnpfCommandIds.insertedFloatEchart) {
        onInsertedFloatEchart(jnpfUniverStore, command);
      }

      // 聚焦悬浮图表
      if (commandId === JnpfCommandIds.focusFloatEchart) {
        const { drawingId } = command?.params ?? {};
        if (!drawingId) {
          return;
        }

        const res = jnpfUniverStore?.getFloatEchartDataByDrawingId(drawingId);
        if (!res) {
          return;
        }

        const { echartType, option } = res ?? {};
        emits('focusFloatEchart', { drawingId, echartType, option } as DeliverFloatEchartOptionProps);
      } else {
        jnpfUniverAPI.value?.getSheetsFloatEchart()?.clearFocusDrawingId();
      }

      // 插入悬浮图片
      if (commandId === JnpfCommandIds.insertedFloatImage) {
        onInsertedFloatImage(jnpfUniverStore, command);
      }

      // 聚焦图片
      if (commandId === JnpfCommandIds.focusFloatImage) {
        const { drawingId } = command?.params ?? {};
        if (!drawingId) {
          return;
        }

        const res = jnpfUniverStore?.getFloatImageDataByDrawingId(drawingId);
        if (!res) {
          return;
        }

        const { imageType, option } = res ?? {};
        emits('focusFloatImage', { drawingId, imageType, option } as DeliverFloatImageOptionProps);
      } else {
        jnpfUniverAPI.value?.getSheetsFloatImage()?.clearFocusDrawingId();
      }

      // 预览设计
      if (commandId === JnpfCommandIds.preview) {
        emits('preview');
      }
    });
  }

  // 暴露 - 获取设计的工作簿数据
  function getDesignWorkbookData() {
    const snapshot = activeWorkbook.value ? activeWorkbook.value?.save() : null;
    const { floatEchartDataCaches = {}, floatImageDataCaches = {} } = jnpfUniverStore ?? {};

    return arrangeDesignWorkbookData(snapshot, floatEchartDataCaches, floatImageDataCaches);
  }

  // 暴露 - 获取预览的工作簿数据
  function getPreviewWorkbookData() {
    const designData = getDesignWorkbookData();
    const { floatEchartToUniverImgDataCaches = {}, floatImageToUniverImgDataCaches = {} } = jnpfUniverStore ?? {};

    return arrangePreviewWorkbookData(designData, floatEchartToUniverImgDataCaches, floatImageToUniverImgDataCaches);
  }

  // 暴露 - 更新悬浮图表配置
  function updateFloatEchartConfig(config: DeliverFloatEchartOptionProps) {
    const { drawingId, echartType, option } = config ?? {};

    // 校验参数
    if (!drawingId || !jnpfUniverAPI.value) {
      return;
    }

    const originalFloatEchartData = jnpfUniverStore?.getFloatEchartDataByDrawingId(drawingId);
    if (!originalFloatEchartData) {
      return;
    }

    const updateFloatEchartData = {
      ...originalFloatEchartData,
      echartType,
      option,
    };

    const floatEchartDataCaches = jnpfUniverStore?.floatEchartDataCaches ?? {};
    const updateFloatEchartsData = {
      ...floatEchartDataCaches,
      [drawingId]: updateFloatEchartData,
    };

    jnpfUniverStore?.updateFocusedFloatEchartDataCache({
      domId: originalFloatEchartData?.domId,
      drawingId,
      option,
    });
    jnpfUniverStore?.setFloatEchartDataCaches(updateFloatEchartsData);
  }

  // 暴露 - 更新悬浮图片配置
  function updateFloatImageConfig(config: DeliverFloatImageOptionProps) {
    const { drawingId, imageType, option } = config ?? {};

    // 校验参数
    if (!drawingId || !jnpfUniverAPI.value) {
      return;
    }

    const originalFloatImageData = jnpfUniverStore?.getFloatImageDataByDrawingId(drawingId);
    if (!originalFloatImageData) {
      return;
    }

    const updateFloatImageData = {
      ...originalFloatImageData,
      imageType,
      option,
    };

    const floatImageDataCaches = jnpfUniverStore?.floatImageDataCaches ?? {};
    const updateFloatImagesData = {
      ...floatImageDataCaches,
      [drawingId]: updateFloatImageData,
    };

    jnpfUniverStore?.updateFocusedFloatImageDataCache({
      domId: originalFloatImageData?.domId,
      drawingId,
      option,
    });
    jnpfUniverStore?.setFloatImageDataCaches(updateFloatImagesData);
  }

  // 暴露 - 更新多个单元格值
  function updateCellsData(cellsData: DeliverCellDataProps[]) {
    if (!cellsData?.length) {
      return;
    }

    cellsData?.forEach((config: any) => {
      const { startColumn, startRow, cellData: configuringCellData = {} } = config ?? {};

      if (isNullOrUndefined(startColumn) || isNullOrUndefined(startRow)) {
        return;
      }

      const oldCellData = jnpfUniverAPI.value?.getSheetsCell()?.getCellData(startColumn, startRow) ?? {};

      const { v = '', custom = {} } = configuringCellData;
      const { type } = custom;

      // 纯文本
      if (!type || type === 'text') {
        try {
          const newCellData = {
            ...oldCellData,
            v,
            custom: {
              ...(oldCellData?.custom ?? {}),
              ...custom,
              type,
            },
          };

          delete newCellData?.t;
          delete newCellData?.custom?.type;
          delete newCellData?.custom?.fillDirection;

          if (isEmptyObject(newCellData?.custom)) {
            delete newCellData?.custom;
          }

          handleSetCellData({ newCellData, startColumn, startRow });
        } catch (e) {
          console.log(e);
        }
      }

      // 数据集
      if (type === 'dataSource') {
        const newCellData = {
          ...oldCellData,
          v: isNullOrUndefined(v) || v === '' ? '' : `\${${v}}`,
          t: 1,
          custom: {
            ...(oldCellData?.custom ?? {}),
            ...custom,
            type,
          },
        };
        delete newCellData?.p;

        handleSetCellData({ newCellData, startColumn, startRow });
      }

      // 参数
      if (type === 'parameter') {
        const newCellData = {
          ...oldCellData,
          v: isNullOrUndefined(v) || v === '' ? '' : `\#{${v}}`,
          t: 1,
          custom: {
            ...(oldCellData?.custom ?? {}),
            ...custom,
            type,
          },
        };
        delete newCellData?.p;

        handleSetCellData({ newCellData, startColumn, startRow });
      }

      // 单元格图表
      if (type === 'cellEchart') {
        // 获取单元格的宽高
        const targetCellSize = jnpfUniverAPI.value?.getSheetsCell()?.getTargetCellSize(startColumn, startRow);
        if (!targetCellSize) {
          return;
        }

        const { cellWidth, cellHeight } = targetCellSize;

        // 取长宽中较小的一个数据作为边长，参考单元格图片的做法
        const echartSideLength = Math.min(cellWidth, cellHeight) - 2;
        if (echartSideLength <= 0) {
          return;
        }

        const { echartOption } = configuringCellData;

        // 得到了echart的图片
        const echartImg = unref(jnpfUniverCellEchartRef).render(echartOption, echartSideLength);

        // base64转成文件流后去插入图片
        base64ToFile(echartImg, 'image.jpeg', 'image/jpeg')
          .then((file: File) => {
            jnpfUniverAPI.value?.getSheetsCellEchart()?.insertCellEchart(file, startRow, startColumn, {
              echartType: custom.echartType,
              sideLength: echartSideLength,
              option: {
                ...(echartOption ?? {}),
              },
            });
          })
          .catch((err: Error) => {
            console.log(err);
          });
      }
    });
  }

  // 设置单元格值
  function handleSetCellData(config: any) {
    const { newCellData, startColumn, startRow } = config ?? {};

    jnpfUniverAPI.value?.getSheetsCell()?.setCellData(startColumn, startRow, newCellData); // 设置值
    jnpfUniverAPI.value?.getSheetsCell()?.refreshRangeCellsView(activeWorksheet.value?.getRange(startRow, startColumn)); // 更新视图
  }

  // 获取设计器容器元素
  function getDesignContainerEle() {
    if (!containerEleId.value) {
      return false;
    }

    const containerEle = document.getElementById(containerEleId.value);
    return containerEle || false;
  }

  // 暴露 - 打开弹窗去选单元格
  function getCellFromDialogSelect(targetCall: any) {
    const { startRow, startColumn } = targetCall ?? {};
    if (dialogCellInstance.value || isNullOrUndefined(startRow) || isNullOrUndefined(startColumn)) {
      return;
    }

    configDialogCellTarget.value = { ...targetCall }; // 记录配置中的单元格
    updateWorkbookPermission(false); // 开启只读权限
    state.openDialogCell = true; // 打开弹窗

    dialogCellInstance.value = activeWorkbook.value?.openDialog({
      id: `dialogSelectCell${dynamicJnpfUniverStoreId}`,
      children: {
        label: JnpfUniverDialogSelectCellKey,
        value: dynamicJnpfUniverStoreId,
      },
      ...DefaultDialogSelectCellConfig,
    });
  }

  // 暴露 - 让聚焦中的单元格失焦
  async function setCellEditVisible(): Promise<void> {
    if (!jnpfUniverAPI.value) {
      return;
    }

    jnpfUniverAPI.value?.executeCommand('sheet.operation.set-cell-edit-visible', {
      visible: false,
      _eventType: DeviceInputEventType.PointerUp,
    });

    return new Promise(resolve => {
      setTimeout(() => {
        resolve();
      }, 200);
    });
  }

  // 暴露 - 销毁工作簿
  function handleDisposeUnit() {
    if (activeWorkbookId?.value) {
      loading.value = true;

      selectionChangeMonitor.value && selectionChangeMonitor.value?.dispose();
      beforeCommandExecuteMonitor.value && beforeCommandExecuteMonitor.value?.dispose();
      commandExecuteMonitor.value && commandExecuteMonitor.value?.dispose();

      jnpfUniverAPI.value && jnpfUniverAPI.value?.disposeUnit(activeWorkbookId.value);

      resetUniverState();

      jnpfUniverStore?.resetStore();

      loading.value = false;
      console.log('工作簿已销毁');
    } else {
      console.warn('销毁工作簿失败');
    }
  }

  // 暴露 - 获取工作簿id
  function getActiveWorkbookId() {
    return activeWorkbookId.value ?? null;
  }

  // 暴露 - 获取激活的工作本id
  function getActiveWorksheetId() {
    return activeWorksheetId.value ?? null;
  }

  // 暴露 - 获取默认的图表配置
  function getDefaultFloatEchartOptions() {
    return { ...DefaultFloatEchartOptions };
  }

  // 暴露 - 获取默认的图片配置
  function getDefaultFloatImageOption() {
    return { ...DefaultFloatImageOption };
  }

  // 暴露 - 获取默认的打印配置
  function getPrintConfigs() {
    return {
      printAreaOptions: jnpfPrintAreaOptions,
      paperTypeOptions: jnpfPaperTypeOptions,
      printDirectionOptions: jnpfPrintDirectionOptions,
      printScaleOptions: jnpfPrintScaleOptions,
      printVAlignOptions: jnpfPrintVAlignOptions,
      printHAlignOptions: jnpfPrintHAlignOptions,
      paperPaddingTypeOptions: jnpfPaperPaddingTypeOptions,
      defaultConfigForm: DefaultPrintConfig,
    };
  }
  // 暴露 - 选中工作簿
  function setWorksheetActiveOperation(sheetId) {
    if (!sheetId) return;
    const unitId = state.jnpfUniverAPI?.getActiveWorkbook()?.getId();
    state.jnpfUniverAPI?.executeCommand(SetWorksheetActiveOperation.id, { unitId, subUnitId: sheetId });
  }

  // 返回弹窗选中的单元格
  function callbackDialogSelectCell() {
    if (!dialogCellInstance.value) {
      return;
    }

    dialogCellInstance.value?.dispose(); // 销毁弹窗
    dialogCellInstance.value = null;
    const selectCellCache = dialogSelectCellDataCache.value; // 存起来，不然选区变化的时候会被同步修改

    const { startRow, startColumn } = (configDialogCellTarget.value ?? {}) as any;
    jnpfUniverAPI.value?.getSheetsRange()?.recoveryRange(startRow, startColumn);

    configDialogCellTarget.value = null;
    updateWorkbookPermission(true);
    state.openDialogCell = false;

    jnpfUniverStore?.setDialogSelectCellDataCache(null);

    emits('changeDialogSelectCell', selectCellCache);
  }

  // 重置实例状态
  function resetUniverState() {
    containerEleId.value = null;
    univer.value = null;
    jnpfUniverAPI.value = null;

    univerCreateMode.value = 'design';

    activeWorkbook.value = null;
    activeWorkbookId.value = null;
    activeWorksheet.value = null;
    activeWorksheetId.value = null;

    selectionChangeMonitor.value = null;
    beforeCommandExecuteMonitor.value = null;
    commandExecuteMonitor.value = null;

    configDialogCellTarget.value = null;
    openDialogCell.value = false;
    //关闭单元格弹窗
    dialogCellInstance.value && dialogCellInstance.value?.dispose();
    dialogCellInstance.value = null;

    selectCellInfo.value = {
      row: undefined,
      column: undefined,
    };

    loading.value = false;
  }

  watch(
    () => dialogSelectCellStateCache.value,
    () => {
      callbackDialogSelectCell();
    },
  );

  defineExpose({
    handleCreateDesignUnit,
    handleDisposeUnit,

    getDesignWorkbookData,
    getPreviewWorkbookData,

    updateFloatEchartConfig,
    updateFloatImageConfig,
    updateCellsData,

    setCellEditVisible, // 让单元格失焦

    getCellFromDialogSelect,

    getActiveWorkbookId,
    getActiveWorksheetId,
    getDefaultFloatEchartOptions,
    getDefaultFloatImageOption,
    getPrintConfigs,
    setWorksheetActiveOperation,
  });
</script>

<style lang="less" scoped>
  .jnpf-univer-design-content {
    flex: 1;
    height: 100%;
    overflow-x: hidden;
    overflow-y: auto;
    position: relative;

    .univer-design-container {
      width: 100%;
      height: calc(100%);
    }
  }

  .cell-echarts-content {
    position: absolute;
    top: 0;
    right: 0;
    bottom: 0;
    left: 0;
    z-index: -1;
  }
</style>
