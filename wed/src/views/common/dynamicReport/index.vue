<template>
  <div class="jnpf-content-wrapper jnpf-dynamicReport-wrapper" v-loading="pageLoading">
    <div class="tool-wrap" v-if="getHasBtn">
      <a-tooltip title="打印" v-if="allowPrint">
        <a-button type="text" :disabled="pageLoading" class="action-bar-btn" @click="handlePrint">
          <i class="icon-ym icon-ym-printExample" />
        </a-button>
      </a-tooltip>
      <a-tooltip title="导出" v-if="allowExport">
        <a-button type="text" :disabled="pageLoading" class="action-bar-btn" @click="handleDownload">
          <i class="icon-ym icon-ym-btn-export1" />
        </a-button>
      </a-tooltip>
    </div>
    <div class="query-wrap" v-if="searchSchemas?.length">
      <BasicForm @register="registerSearchForm" :schemas="searchSchemas" @submit="handleSearchSubmit" @reset="searchFormSubmit" class="search-form">
      </BasicForm>
    </div>
    <div class="content-main">
      <JnpfUniver ref="jnpfUniverRef" :key="jnpfUniverKey" />
    </div>
  </div>
  <ReportPrint @register="registerReportPrint" />
</template>

<script lang="ts" setup>
  import { reactive, onMounted, onUnmounted, toRefs, ref, unref, nextTick, computed } from 'vue';
  import { useRoute } from 'vue-router';
  import { getPreviewTemplate, exportFileExcel } from '@/api/onlineDev/report';
  import { BasicForm, useForm } from '@/components/Form';
  import { JnpfUniver } from '@/components/Report/src/jnpfUniver';
  import { useReport } from '@/components/Report/src/hooks/useReport';
  import { ReportPrint } from '@/components/Report';
  import { downloadByUrlReport } from '@/utils/file/download';
  import { useModal } from '@/components/Modal';
  import { getFloatUrl } from '@/components/Report/src/helper';
  import { useGlobSetting } from '@/hooks/setting';

  defineOptions({ name: 'dynamicReport' });

  interface State {
    id: string;
    reportData: any;
    pageLoading: boolean;
    jnpfUniverAPI: any;
    allowExport: number;
    allowPrint: number;
    sheetId: string;
    jnpfUniverKey: number;
    queryList: any[];
  }

  const jnpfUniverRef = ref();
  const state = reactive<State>({
    id: '',
    reportData: {},
    pageLoading: false,
    jnpfUniverAPI: null,
    allowExport: 0,
    allowPrint: 0,
    sheetId: '',
    jnpfUniverKey: 0,
    queryList: [],
  });
  const { pageLoading, allowExport, allowPrint, jnpfUniverKey } = toRefs(state);

  const [registerReportPrint, { openModal: openPrintModal }] = useModal();
  const [registerSearchForm, { updateSchema, submit: searchFormSubmit, getFieldsValue }] = useForm({
    baseColProps: { span: 6 },
    showActionButtonGroup: true,
    showAdvancedButton: true,
    compact: true,
  });
  const formApi = { updateSchema, getFieldsValue };
  const { getRealEchart, getSearchSchema, searchSchemas } = useReport(formApi);
  const globSetting = useGlobSetting();
  const getHasBtn = computed(() => {
    if (state.allowExport || state.allowPrint) return true;
    return false;
  });

  // 初始化
  function init() {
    const route = useRoute();
    state.id = route?.meta?.relationId as string;
    if (!state.id) return;
    state.pageLoading = true;
    initData();
  }
  function initData(data = {}) {
    state.pageLoading = true;
    getPreviewTemplate(state.id, data).then(res => {
      state.reportData = res.data;
      state.allowExport = res.data.allowExport || 0;
      state.allowPrint = res.data.allowPrint || 0;
      state.queryList = res.data.queryList ? JSON.parse(res.data.queryList) : [];
      const snapshot = res.data.snapshot ? JSON.parse(res.data.snapshot) : null;
      const cells = res.data.cells ? JSON.parse(res.data.cells) : {};
      const chartData = res.data.chartData ? JSON.parse(res.data.chartData) : [];
      const floatEcharts = getRealEchart(cells.floatEcharts ?? null, chartData);
      let floatImages = cells.floatImages || {};
      let item = getFloatUrl(snapshot.resources, globSetting.reportApiUrl, floatImages);
      snapshot.resources = item.list;
      floatImages = item?.floatImages || {};
      getSearchSchema(state.queryList, state.sheetId || '');
      state.jnpfUniverKey = +new Date();
      nextTick(() => {
        handleCreate(snapshot, floatEcharts, floatImages);
      });
      state.pageLoading = false;
    });
  }
  // 创建报表实例
  function handleCreate(snapshot, floatEcharts, floatImages) {
    const res = unref(jnpfUniverRef)?.handleCreateDesignUnit({
      mode: 'preview',
      snapshot,
      floatEcharts,
      floatImages,
      uiHeader: false,
      uiContextMenu: false,
      workbookReadonly: true,
      defaultActiveSheetId: state.sheetId || '',
      loading: true,
    });
    state.jnpfUniverAPI = res ? res?.jnpfUniverAPI : null;
    onReportCommandExecuted();
  }
  function onReportCommandExecuted() {
    state.jnpfUniverAPI?.onCommandExecuted((command: any) => {
      const { id: commandId } = command ?? {};
      // 切换sheet
      if (commandId === 'sheet.operation.set-worksheet-active') {
        state.sheetId = command.params.subUnitId;
        getSearchSchema(state.queryList, state.sheetId);
      }
    });
  }
  // 销毁示例
  function handleDisposeUnit() {
    unref(jnpfUniverRef)?.handleDisposeUnit();
  }
  function handleSearchSubmit(data) {
    initData({ sheetId: state.sheetId, ...data });
  }
  // 打印
  function handlePrint() {
    const { snapshot } = unref(jnpfUniverRef)?.getPreviewWorkbookData();
    const sheetId = state.jnpfUniverAPI.getActiveWorkbook()?.getActiveSheet()?.getSheetId();
    const activeWorksheetId = unref(jnpfUniverRef)?.getActiveWorksheetId();
    const { xSplit = 0, ySplit = 0 } = snapshot?.sheets?.[activeWorksheetId]?.freeze ?? {};
    const hasXFreeze = xSplit > 0;
    const hasYFreeze = ySplit > 0;
    openPrintModal(true, { id: state.reportData.versionId, fullName: state.reportData.fullName, sheetId, snapshot, hasXFreeze, hasYFreeze });
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

  onMounted(() => {
    init();
  });

  onUnmounted(() => {
    handleDisposeUnit();
  });
</script>

<style lang="less">
  .jnpf-dynamicReport-wrapper {
    background-color: @component-background;
    display: flex;
    flex-direction: column;
    .tool-wrap {
      height: 53px;
      padding: 10px;
      border-bottom: 1px solid @border-color-base1;
    }
    .query-wrap {
      padding: 10px 10px 0;
      border-bottom: 1px solid #f0f0f0;
    }
    .content-main {
      flex: 1;
      overflow: hidden;
      position: relative;
    }
  }
</style>
