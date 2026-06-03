<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    defaultFullscreen
    :footer="null"
    :closable="false"
    :keyboard="false"
    destroyOnClose
    class="jnpf-full-modal full-modal designer-modal jnpf-report-print-modal">
    <template #title>
      <div class="jnpf-full-modal-header">
        <div class="header-title">
          <img src="@/assets/images/jnpf.png" class="header-logo" />
          <span class="header-dot"></span>
          <p class="header-txt">报表打印预览</p>
        </div>
        <a-space class="options" :size="10">
          <a-button type="primary" :disabled="loading" @click="handlePrint()">{{ t('common.printText') }}</a-button>
          <a-button @click="handleCancel()">{{ t('common.cancelText') }}</a-button>
        </a-space>
      </div>
    </template>
    <div class="print-container" v-loading="loading" :loading-tip="printRenderTip">
      <div class="print-design-content">
        <JnpfUniver ref="jnpfUniverRef" />
      </div>
      <div class="print-main-container">
        <div class="main-warp">
          <JnpfUniverPrint
            ref="jnpfUniverPrintRef"
            :jnpf-univer-api="jnpfUniverAPI"
            @preview-render-progress="getPreviewRenderProgress"
            @print-render-progress="getPrintRenderProgress"
            @browser-print-show="handleBrowserShow" />
        </div>
        <div class="config-warp bg-white">
          <ConfigForm :formState="formState" :configOptions="configOptions" :freezeObject="freezeObject" v-if="!configLoading" />
        </div>
      </div>
    </div>
  </BasicModal>
</template>

<script lang="ts" setup>
  import { reactive, ref, toRefs, unref, watch, nextTick } from 'vue';
  import { BasicModal, useModalInner } from '@/components/Modal';
  import { useI18n } from '@/hooks/web/useI18n';
  import { JnpfUniver, JnpfUniverPrint } from '../jnpfUniver/index';
  import ConfigForm from './ConfigForm.vue';
  import { useDebounceFn } from '@vueuse/core';

  defineOptions({ name: 'reportPrint' });

  interface State {
    reportData: any;
    formState: any;
    jnpfUniverAPI: any;
    configOptions: any;
    loading: boolean;
    printRenderTip: string;
    configLoading: boolean;
    freezeObject: any;
  }
  const { t } = useI18n();
  const [registerModal, { closeModal }] = useModalInner(init);
  const defaultRenderProgressTip = ' '; // 默认的加载文案

  const state = reactive<State>({
    reportData: {},
    jnpfUniverAPI: null,
    formState: {},
    configOptions: {},
    loading: false,
    printRenderTip: defaultRenderProgressTip,
    configLoading: false,
    freezeObject: {},
  });
  const { jnpfUniverAPI, formState, configOptions, loading, printRenderTip, configLoading, freezeObject } = toRefs(state);
  const jnpfUniverRef = ref();
  const jnpfUniverPrintRef = ref();
  const debounceHandlePrintConfig = useDebounceFn(() => {
    handleChangePrintConfig(state.formState);
  }, 300);

  watch(
    () => state.formState,
    () => {
      state.loading = true;
      debounceHandlePrintConfig();
    },
    { deep: true },
  );

  async function init(data) {
    state.reportData = data;
    state.loading = true;
    state.configLoading = true;
    state.printRenderTip = defaultRenderProgressTip;
    state.freezeObject = { hasXFreeze: data.hasXFreeze, hasYFreeze: data.hasYFreeze };
    handleCreate(data.snapshot);
  }
  // 创建报表实例
  function handleCreate(snapshot) {
    nextTick(() => {
      const res = unref(jnpfUniverRef)?.handleCreateDesignUnit({
        mode: 'print',
        snapshot,
        loading: false,
      });
      state.jnpfUniverAPI = res ? res?.jnpfUniverAPI : null;
      // 切换到指定的工作表
      if (state.reportData.sheetId) unref(jnpfUniverRef)?.setWorksheetActiveOperation(state.reportData.sheetId);
      state.configOptions = unref(jnpfUniverRef).getPrintConfigs();
      state.jnpfUniverAPI?.getHooks()?.onSteady(() => {
        state.formState = { ...state.configOptions.defaultConfigForm, workbookTitleText: state.reportData.fullName };
        state.formState.yFreeze = state.freezeObject.hasYFreeze;
        state.formState.xFreeze = state.freezeObject.hasXFreeze;
        state.configLoading = false;
      });
    });
  }
  // 销毁示例
  function handleDisposeUnit() {
    unref(jnpfUniverRef)?.handleDisposeUnit();
    state.jnpfUniverAPI = null;
  }
  function handleCancel() {
    handleDisposeUnit();
    closeModal();
  }
  // 打印
  function handlePrint() {
    unref(jnpfUniverPrintRef)?.handleBrowserPrint();
  }
  function handleChangePrintConfig(config: any) {
    state.printRenderTip = defaultRenderProgressTip;

    if (state.jnpfUniverAPI) unref(jnpfUniverPrintRef)?.handlePrintRender(config);
  }
  // 获取打印预览渲染进度
  function getPreviewRenderProgress(res: any) {
    const { total, rendered } = res ?? {};

    state.printRenderTip = `共${total}页，已加载${rendered}页`;
    if (rendered === total) {
      state.loading = false;
    }
  }
  // 获取打印浏览器渲染进度
  function getPrintRenderProgress(res: any) {
    const { total, rendered } = res ?? {};

    state.printRenderTip = `共${total}页，已准备${rendered}页`;
    if (rendered === total) {
      state.printRenderTip = '整理中...';
    }
  }
  // 打印准备好了
  function handleBrowserShow() {
    state.loading = false;
  }
</script>

<style lang="less">
  .jnpf-full-modal.jnpf-report-print-modal {
    .ant-modal-body > .scrollbar {
      padding: 0 !important;
    }
    .print-container {
      z-index: 1000;
      height: 100%;
      flex: 1;
      position: relative;
      .print-design-content {
        position: absolute;
        inset: 0;
        z-index: 2;
        display: flex;
        .jnpf-univer-design-content {
          opacity: 0;
        }
      }
      .print-main-container {
        position: absolute;
        inset: 0;
        z-index: 99;
        overflow: hidden;
        display: flex;
        .main-warp {
          flex: 1 0 0;
          height: 100%;
          overflow: auto;
          display: flex;
        }
        .config-warp {
          flex: 270px 0 0;
          margin-top: 35px;
          padding: 10px;
          overflow-x: hidden;
          overflow-y: auto;
          .ant-radio-button-wrapper {
            padding: 0 11px;
          }
        }
      }
    }
  }
</style>
