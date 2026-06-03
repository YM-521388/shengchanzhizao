<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    defaultFullscreen
    :footer="null"
    :closable="false"
    :keyboard="false"
    destroyOnClose
    class="jnpf-full-modal full-modal designer-modal jnpf-report-preview-modal">
    <template #title>
      <div class="jnpf-full-modal-header">
        <div class="header-title">
          <img src="@/assets/images/jnpf.png" class="header-logo" />
          <span class="header-dot"></span>
          <p class="header-txt">报表预览</p>
        </div>
        <a-space class="options" :size="10">
          <a-button type="primary" :disabled="loading" @click="handleDownload()" v-if="allowExport">{{ t('common.exportText') }}</a-button>
          <a-button type="primary" :disabled="loading" @click="handlePrint()" v-if="allowPrint">{{ t('common.printText') }}</a-button>
          <a-button @click="handleCancel()">{{ t('common.cancelText') }}</a-button>
        </a-space>
      </div>
    </template>
    <div class="basic-content bg-white">
      <div class="query-wrap" v-if="searchSchemas?.length">
        <BasicForm @register="registerSearchForm" :schemas="searchSchemas" @submit="handleSearchSubmit" @reset="searchFormSubmit" class="search-form" />
      </div>
      <div class="main-warp">
        <JnpfUniver ref="jnpfUniverRef" :key="key" />
      </div>
    </div>
    <ReportPrint @register="registerReportPrint" />
  </BasicModal>
</template>

<script lang="ts" setup>
  import { reactive, ref, unref, toRefs, nextTick } from 'vue';
  import { BasicModal, useModal, useModalInner } from '@/components/Modal';
  import { useI18n } from '@/hooks/web/useI18n';
  import { getPreviewDesign, getPreviewTemplate, exportFileExcel } from '@/api/onlineDev/report';
  import { downloadByUrlReport } from '@/utils/file/download';
  import { BasicForm, useForm } from '@/components/Form';
  import ReportPrint from '../reportPrint/index.vue';
  import { JnpfUniver } from '../jnpfUniver/index';
  import { useReport } from '../hooks/useReport';
  import { getFloatUrl } from '../helper';
  import { useGlobSetting } from '@/hooks/setting';
  defineOptions({ name: 'reportPreview' });

  interface State {
    id: string;
    versionId: string;
    reportData: any;
    searchSchemas: any[];
    jnpfUniverAPI: any;
    allowExport: number;
    allowPrint: number;
    sheetId: string;
    queryList: any[];
    sheetIndex: number;
    loading: boolean;
    key: any;
  }

  const { t } = useI18n();
  const [registerModal, { closeModal }] = useModalInner(init);
  const [registerReportPrint, { openModal: openPrintModal }] = useModal();
  const jnpfUniverRef = ref();
  const globSetting = useGlobSetting();
  const state = reactive<State>({
    id: '',
    versionId: '',
    reportData: {},
    searchSchemas: [],
    jnpfUniverAPI: null,
    allowExport: 0,
    allowPrint: 0,
    sheetId: '',
    queryList: [],
    sheetIndex: 0,
    loading: false,
    key: '',
  });
  const { allowExport, allowPrint, loading, key } = toRefs(state);

  const [registerSearchForm, { updateSchema, submit: searchFormSubmit, getFieldsValue }] = useForm({
    baseColProps: { span: 6 },
    showActionButtonGroup: true,
    showAdvancedButton: true,
    compact: true,
  });
  const formApi = { updateSchema, getFieldsValue };
  const { getRealEchart, getSearchSchema, clearSearchSchema, searchSchemas } = useReport(formApi);

  async function init(data) {
    state.loading = false;
    state.id = data.id;
    state.versionId = data.versionId;
    state.sheetIndex = 0;
    state.sheetId = data.sheetId || '';
    initData();
  }
  function initData(data = {}) {
    state.loading = true;
    const method = state.versionId ? getPreviewDesign : getPreviewTemplate;
    const id = state.versionId ? state.versionId : state.id;
    method(id, data).then(res => {
      state.reportData = res.data;
      state.queryList = res.data.queryList ? JSON.parse(res.data.queryList) : [];
      const snapshot = res.data.snapshot ? JSON.parse(res.data.snapshot) : null;
      const cells = res.data.cells ? JSON.parse(res.data.cells) : {};
      const chartData = res.data.chartData ? JSON.parse(res.data.chartData) : [];
      const floatEcharts = getRealEchart(cells.floatEcharts ?? null, chartData);
      let floatImages = cells.floatImages;
      let item = getFloatUrl(snapshot.resources, globSetting.reportApiUrl, floatImages);
      snapshot.resources = item.list;
      floatImages = item.floatImages;
      state.allowExport = res.data.allowExport || 0;
      state.allowPrint = res.data.allowPrint || 0;
      getSearchSchema(state.queryList, state.sheetId);
      state.key = +new Date();
      nextTick(() => {
        handleCreate(snapshot, floatEcharts, floatImages);
      });
    });
  }
  // 创建报表实例
  function handleCreate(snapshot, floatEcharts, floatImages) {
    const res = unref(jnpfUniverRef)?.handleCreateDesignUnit({
      mode: 'preview',
      snapshot,
      floatEcharts,
      uiHeader: false,
      uiContextMenu: false,
      workbookReadonly: true,
      defaultActiveSheetId: state.sheetId || '',
      floatImages,
      loading: true,
    });
    state.jnpfUniverAPI = res ? res?.jnpfUniverAPI : null;
    onReportCommandExecuted();
    state.jnpfUniverAPI?.getHooks()?.onSteady(() => {
      state.loading = false;
    });
  }
  function onReportCommandExecuted() {
    state.jnpfUniverAPI?.onCommandExecuted((command: any) => {
      const { id: commandId } = command ?? {};

      // 切换sheet
      if (commandId === 'sheet.operation.set-worksheet-active') {
        state.sheetIndex = state.queryList.findIndex(o => o.sheet === command.params.subUnitId);
        if (state.sheetId != command.params.subUnitId) {
          clearSearchSchema();
          if (state.queryList?.length) {
            nextTick(() => {
              state.key = +new Date();
              initData({ sheetId: command.params.subUnitId });
            });
          }
        }
        state.sheetId = command.params.subUnitId;
        nextTick(() => {
          getSearchSchema(state.queryList, state.sheetId);
        });
      }
    });
  }
  // 销毁示例
  function handleDisposeUnit() {
    unref(jnpfUniverRef)?.handleDisposeUnit();
  }
  function handleCancel() {
    handleDisposeUnit();
    closeModal();
  }
  // 筛选
  function handleSearchSubmit(data) {
    initData({ ...data, sheetId: state.sheetId });
  }
  // 打印
  function handlePrint() {
    const { snapshot } = unref(jnpfUniverRef)?.getPreviewWorkbookData();
    const sheetId = state.jnpfUniverAPI.getActiveWorkbook()?.getActiveSheet()?.getSheetId();
    const activeWorksheetId = unref(jnpfUniverRef)?.getActiveWorksheetId();
    const { xSplit = 0, ySplit = 0 } = snapshot?.sheets?.[activeWorksheetId]?.freeze ?? {};
    const hasXFreeze = xSplit > 0;
    const hasYFreeze = ySplit > 0;
    openPrintModal(true, { id: state.reportData.versionId, fullName: state.reportData.fullName, sheetId, snapshot, hasYFreeze, hasXFreeze });
  }
  // 导出
  function handleDownload() {
    const { snapshot } = unref(jnpfUniverRef)?.getPreviewWorkbookData();
    const sheetId = state.jnpfUniverAPI.getActiveWorkbook()?.getActiveSheet()?.getSheetId();
    const data = unref(searchSchemas).length ? getFieldsValue() : {};
    const query = { ...data, sheetId, fullName: state.reportData.fullName, snapshot: snapshot ? JSON.stringify(snapshot) : null };
    exportFileExcel(state.reportData.versionId, query).then(res => {
      downloadByUrlReport({ url: res.data.url });
    });
  }
</script>

<style lang="less">
  .jnpf-full-modal.jnpf-report-preview-modal {
    .basic-content {
      display: flex;
      flex-direction: column;
    }
    .query-wrap {
      flex-shrink: 0;
      padding: 10px 10px 0;
    }
    .main-warp {
      width: 100%;
      height: 100%;
      flex: 1;
    }
  }
</style>
