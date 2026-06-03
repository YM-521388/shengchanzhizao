<template>
  <a-modal v-model:open="internalOpen" title="打印预览" width="100%" wrap-class-name="print-full-modal" @cancel="handleCancel">
    <div class="content-main" v-loading="loading" :loading-tip="printRenderTip">
      <div class="control-buttons" style="background: lightpink">
        <a-button type="primary" @click="handleBrowserPrint">浏览器打印</a-button>
      </div>
      <div class="print-container">
        <div class="print-design-content">
          <JnpfUniverDesign v-if="internalOpen" ref="jnpfUniverDesignPrintRef" />
        </div>
        <div class="print-main-container">
          <JnpfUniverPrintRender
            ref="jnpfUniverPrintRenderRef"
            :jnpf-univer-api="jnpfUniverAPI"
            @preview-render-progress="getPreviewRenderProgress"
            @print-render-progress="getPrintRenderProgress"
            @browser-print-show="handleBrowserShow" />
          <JnpfUniverPrintForm
            v-if="internalOpen"
            :default-print-config="DefaultPrintConfig"
            :sheet-freeze-status="sheetFreezeStatus"
            @change="handleChangePrintConfig" />
        </div>
      </div>
    </div>
  </a-modal>
</template>

<script lang="ts" setup>
  import { nextTick, reactive, ref, toRefs, unref } from 'vue';
  import { Modal as AModal, Button as AButton } from 'ant-design-vue';
  import { IWorkbookData } from '@univerjs/core';
  import { SetWorksheetActiveOperation } from '@univerjs/sheets';
  import { DefaultPrintConfig } from '../Design/univer/utils/define';

  import JnpfUniverDesign from '../Design/index.vue';
  import JnpfUniverPrintRender from './Render/index.vue';
  import JnpfUniverPrintForm from './Form/index.vue';

  const jnpfUniverDesignPrintRef = ref();
  const jnpfUniverPrintRenderRef = ref();

  const defaultRenderProgressTip = '　'; // 默认的加载文案
  const defaultFreezeStatus = {
    rowHasFreeze: false,
    colHasFreeze: false,
  };

  interface State {
    jnpfUniverAPI: any;

    internalOpen: boolean;
    loading: boolean;

    sheetFreezeStatus: any;

    printRenderTip: string;
  }
  const state = reactive<State>({
    jnpfUniverAPI: null,

    internalOpen: false,
    loading: false,

    sheetFreezeStatus: { ...defaultFreezeStatus },

    printRenderTip: defaultRenderProgressTip,
  });
  const { jnpfUniverAPI, internalOpen, loading, sheetFreezeStatus, printRenderTip } = toRefs(state);

  // 创建打印实例
  function handleCreatePrintUnit(data: { snapshot: IWorkbookData; activeWorksheetId?: string }) {
    state.internalOpen = true;

    state.printRenderTip = defaultRenderProgressTip;
    state.loading = true;

    const { snapshot, activeWorksheetId } = data ?? {};

    nextTick(() => {
      const res = unref(jnpfUniverDesignPrintRef)?.handleCreateDesignUnit({
        mode: 'print',
        snapshot,
        loading: false,
      });

      state.jnpfUniverAPI = res ? res?.jnpfUniverAPI : null;
      if (!state.jnpfUniverAPI) {
        return;
      }

      // 切换到指定的工作表
      const unitId = jnpfUniverAPI.value?.getActiveWorkbook()?.getId();
      const subUnitId = activeWorksheetId ?? jnpfUniverAPI.value?.getActiveWorkbook()?.getActiveSheet()?.getSheetId();
      if (activeWorksheetId) {
        jnpfUniverAPI.value?.executeCommand(SetWorksheetActiveOperation.id, { unitId, subUnitId });
      }

      // 判断行列的冻结状态
      const { xSplit = 0, ySplit = 0 } = snapshot?.sheets?.[subUnitId]?.freeze ?? {};
      const colHasFreeze = xSplit > 0; // 列冻结（冻结左侧的列）
      const rowHasFreeze = ySplit > 0; // 行冻结（冻结上方的行）

      state.sheetFreezeStatus = {
        rowHasFreeze, // 行冻结
        colHasFreeze, // 列冻结
      };

      jnpfUniverAPI.value?.getHooks()?.onSteady(() => {
        console.log('univer render finished');
        unref(jnpfUniverPrintRenderRef)?.handlePrintRender(DefaultPrintConfig);
      });
    });
  }

  // 变更打印配置
  function handleChangePrintConfig(config: any) {
    state.printRenderTip = defaultRenderProgressTip;
    state.loading = true;

    unref(jnpfUniverPrintRenderRef)?.handlePrintRender(config);
  }

  // 获取打印预览渲染进度
  function getPreviewRenderProgress(res: any) {
    const { total, rendered } = res ?? {};

    state.printRenderTip = `共${total}页，已加载${rendered}页`;
    if (rendered === total) {
      state.loading = false;
    }
  }

  // 执行浏览器打印
  function handleBrowserPrint() {
    state.printRenderTip = defaultRenderProgressTip;
    state.loading = true;

    unref(jnpfUniverPrintRenderRef)?.handleBrowserPrint();
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

  // 关闭弹窗
  function handleCancel() {
    unref(jnpfUniverDesignPrintRef)?.handleDisposeUnit();

    resetState();
    // 建议有个时间缓存周期再关闭弹窗
  }

  // 重置
  function resetState() {
    state.jnpfUniverAPI = null;

    state.loading = false;
    state.printRenderTip = defaultRenderProgressTip;

    state.sheetFreezeStatus = { ...defaultFreezeStatus };

    unref(jnpfUniverPrintRenderRef)?.resetState();
  }

  defineExpose({ handleCreatePrintUnit });
</script>

<style lang="less">
  .print-full-modal {
    .ant-modal {
      max-width: 100%;
      top: 0;
      padding-bottom: 0;
      margin: 0;
    }
    .ant-modal-content {
      display: flex;
      flex-direction: column;
      height: calc(100vh);
      padding: 20px 10px 0;
    }
    .ant-modal-body {
      flex: 1;
      display: flex;

      .content-main {
        flex: 1;
        display: flex;
        flex-direction: column;
      }

      .print-container {
        flex: 1;
        position: relative;

        .print-design-content {
          position: absolute;
          left: 0;
          right: 0;
          top: 0;
          bottom: 0;
          z-index: 2;

          display: flex;

          .jnpf-univer-design-content {
            opacity: 0;
          }
        }

        .print-main-container {
          position: absolute;
          top: 0;
          left: 0;
          right: 0;
          bottom: 0;
          z-index: 99;
          background: #f2f3f4;
          overflow: hidden;

          display: flex;
        }
      }
    }
    .ant-modal-footer {
      display: none;
    }
  }

  html[data-theme='dark'] {
    .print-render-content .tools-wrap {
      .first-page,
      .prev-page,
      .next-page,
      .last-page {
        color: rgba(255, 255, 255, 1);

        .icon {
          fill: rgba(255, 255, 255, 1);
        }

        &.disabled {
          cursor: not-allowed;
          color: rgba(255, 255, 255, 0.7);

          .icon {
            fill: rgba(255, 255, 255, 0.7);
          }
        }
      }

      .total-page-number {
        color: rgba(255, 255, 255, 1);
      }
    }
  }
</style>
