<template>
  <div class="jnpf-content-wrapper">
    <div class="jnpf-content-wrapper-center">
      <div class="jnpf-content-wrapper-search-box">
        <BasicForm
          class="search-form"
          @register="registerForm"
          :schemas="activeKey === 0 ? schemasForm : taskSchemasForm"
          @advanced-change="redoHeight"
          @submit="handleSubmit"
          @reset="handleReset"
          :key="formKey" />
      </div>
      <div class="jnpf-content-wrapper-content bg-white">
        <a-tabs v-model:activeKey="activeKey" class="jnpf-content-wrapper-tabs" destroyInactiveTabPane @change="onTabChange">
          <a-tab-pane :key="0" tab="标准/简单流程监控">
            <BasicTable @register="registerTable" :searchInfo="searchInfo">
              <template #tableTitle>
                <a-button type="error" preIcon="icon-ym icon-ym-delete" @click="handleDelete">{{ t('common.delText') }}</a-button>
              </template>
              <template #bodyCell="{ column, record }">
                <template v-if="column.key === 'flowUrgent'">
                  <JnpfTextTag :content="getUrgentText(record.flowUrgent)" :color="getUrgentTextColor(record.flowUrgent)" :showTag="false" showTextColor />
                </template>
                <template v-if="column.key === 'flowVersion'">
                  <a-tag color="processing">V:{{ record.flowVersion }}</a-tag>
                </template>
                <template v-if="column.key === 'status'">
                  <JnpfTextTag :content="getFlowStatusContent(record.status)" :color="getFlowStatusColor(record.status)" />
                </template>
                <template v-if="column.key === 'action'">
                  <TableAction :actions="getTableActions(record)" />
                </template>
              </template>
            </BasicTable>
          </a-tab-pane>
          <a-tab-pane :key="1" tab="任务流程监控">
            <BasicTable @register="registerTaskTable" :searchInfo="searchInfo">
              <template #tableTitle>
                <a-button type="error" preIcon="icon-ym icon-ym-delete" @click="handleDelete">{{ t('common.delText') }}</a-button>
              </template>
              <template #bodyCell="{ column, record }">
                <template v-if="column.key === 'fullName'">
                  {{ record.fullName }}
                  <template v-if="record.isRetry">
                    <a-tag color="blue" class="!mx-8px">重试</a-tag>
                    <BasicHelp :text="[`原实例ID：${record.parentId}`, `原实例执行时间：${dayjs(record.parentTime).format('YYYY-MM-DD HH:mm:ss')}`]">
                      <template #default><i class="icon-ym icon-ym-generator-link" /></template>
                    </BasicHelp>
                  </template>
                </template>
                <template v-if="column.key === 'status'">
                  <JnpfTextTag :content="getTaskStatusContent(record.status)" :color="getTaskStatusColor(record.status)" />
                </template>
                <template v-if="column.key === 'action'">
                  <TableAction :actions="getTaskTableActions(record)" />
                </template>
              </template>
            </BasicTable>
          </a-tab-pane>
        </a-tabs>
      </div>
    </div>
    <FlowParser @register="registerFlowParser" @reload="handleSearch" />
  </div>
</template>
<script lang="ts" setup>
  import { onMounted, ref, nextTick } from 'vue';
  import { getFlowMonitorList, delMonitorList } from '@/api/workFlow/flowMonitor';
  import { getTaskMonitorList, delTaskMonitor, retryTask } from '@/api/workFlow/trigger';
  import { getTreeList } from '@/api/workFlow/template';
  import { BasicTable, useTable, TableAction, BasicColumn, ActionItem } from '@/components/Table';
  import { BasicForm, useForm } from '@/components/Form';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useI18n } from '@/hooks/web/useI18n';
  import { usePopup } from '@/components/Popup';
  import { useBaseStore } from '@/store/modules/base';
  import FlowParser from '@/views/workFlow/components/FlowParser.vue';
  import { useDefineSetting } from '@/hooks/setting/useDefineSetting';
  import dayjs from 'dayjs';
  import { cloneDeep } from 'lodash-es';

  defineOptions({ name: 'workFlow-flowMonitor' });

  const activeKey = ref<Number>(0);
  const searchInfo = ref<any>({});
  const formKey = ref<any>(0);
  const { createMessage, createConfirm } = useMessage();
  const baseStore = useBaseStore();
  const { t } = useI18n();
  const { flowStatusList, flowUrgentList, getUrgentText, getUrgentTextColor, getFlowStatusContent, getFlowStatusColor } = useDefineSetting();
  const [registerFlowParser, { openPopup: openFlowParser }] = usePopup();

  const columns: BasicColumn[] = [
    { title: '流程标题', dataIndex: 'fullName', width: 200 },
    { title: '所属流程', dataIndex: 'flowName', width: 150 },
    { title: '发起时间', dataIndex: 'startTime', width: 150, format: 'date|YYYY-MM-DD HH:mm:ss' },
    { title: '发起人员', dataIndex: 'creatorUser', width: 120 },
    { title: '审批节点', dataIndex: 'currentNodeName', width: 150 },
    { title: '是否归档', dataIndex: 'isFile', width: 100, align: 'center' },
    { title: '紧急程度', dataIndex: 'flowUrgent', width: 100, align: 'center' },
    { title: '流程版本', dataIndex: 'flowVersion', width: 70, align: 'center' },
    { title: '流程状态', dataIndex: 'status', width: 120, align: 'center' },
  ];
  const taskColumns: BasicColumn[] = [
    { title: '实例ID', dataIndex: 'id', width: 200 },
    { title: '流程标题', dataIndex: 'fullName' },
    { title: '发起时间', dataIndex: 'startTime', width: 150, format: 'date|YYYY-MM-DD HH:mm:ss' },
    { title: '流程状态', dataIndex: 'status', width: 120, align: 'center' },
  ];
  const isFileList = [
    { fullName: '是', id: 1 },
    { fullName: '否', id: 0 },
  ];
  const schemasForm: any[] = [
    {
      field: 'keyword',
      label: t('common.keyword'),
      component: 'Input',
      componentProps: { placeholder: t('common.enterKeyword'), submitOnPressEnter: true },
    },
    {
      field: 'pickerVal',
      label: '发起时间',
      component: 'DateRange',
      componentProps: {
        format: 'YYYY-MM-DD HH:mm:ss',
        showTime: { defaultValue: [dayjs('00:00:00', 'HH:mm:ss'), dayjs('23:59:59', 'HH:mm:ss')] },
        placeholder: ['开始时间', '结束时间'],
      },
    },
    {
      field: 'flowCategory',
      label: '分类',
      component: 'Select',
      componentProps: { showSearch: true },
    },
    {
      field: 'templateId',
      label: '所属流程',
      component: 'TreeSelect',
      componentProps: { lastLevel: true },
    },
    {
      field: 'creatorUserId',
      label: '发起人员',
      component: 'UserSelect',
    },
    {
      field: 'status',
      label: '流程状态',
      component: 'Select',
      componentProps: { showSearch: true, options: cloneDeep(flowStatusList).filter(o => o.id != 6) },
    },
    {
      field: 'flowUrgent',
      label: '紧急程度',
      component: 'Select',
      componentProps: { showSearch: true, options: flowUrgentList },
    },
    {
      field: 'isFile',
      label: '是否归档',
      component: 'Select',
      componentProps: { showSearch: true, options: isFileList },
    },
  ];
  const taskSchemasForm: any[] = [
    {
      field: 'keyword',
      label: t('common.keyword'),
      component: 'Input',
      componentProps: { placeholder: t('common.enterKeyword'), submitOnPressEnter: true },
    },
    {
      field: 'pickerVal',
      label: '发起时间',
      component: 'DateRange',
      componentProps: {
        format: 'YYYY-MM-DD HH:mm:ss',
        showTime: { defaultValue: [dayjs('00:00:00', 'HH:mm:ss'), dayjs('23:59:59', 'HH:mm:ss')] },
        placeholder: ['开始时间', '结束时间'],
      },
    },
  ];
  const useTableAttrs: any = {
    rowSelection: { type: 'checkbox' },
    immediate: false,
    clickToRowSelect: false,
    clearSelectOnPageChange: true,
    actionColumn: {
      width: 60,
      title: '操作',
      dataIndex: 'action',
    },
  };
  const [registerTable, { reload, getSelectRowKeys, redoHeight }] = useTable({ ...useTableAttrs, columns, api: getFlowMonitorList });
  const [registerTaskTable, { reload: reloadTaskTable, getSelectRowKeys: getTaskSelectRowKeys }] = useTable({
    ...useTableAttrs,
    columns: taskColumns,
    api: getTaskMonitorList,
    actionColumn: {
      width: 100,
      title: '操作',
      dataIndex: 'action',
    },
  });
  const [registerForm, { updateSchema }] = useForm({
    baseColProps: { span: 6 },
    showActionButtonGroup: true,
    showAdvancedButton: true,
    compact: true,
    fieldMapToTime: [['pickerVal', ['startTime', 'endTime']]],
  });

  function getTableActions(record): ActionItem[] {
    return [
      {
        label: t('common.detailText'),
        onClick: toDetail.bind(null, record),
      },
    ];
  }
  function getTaskTableActions(record): ActionItem[] {
    return [
      {
        label: t('common.detailText'),
        onClick: toDetail.bind(null, record),
      },
      {
        ifShow: record.status === 10,
        label: '重试',
        color: 'error',
        disabled: record.templateStatus != 1,
        modelConfirm: {
          content: '确定要将本实例进行重试, 是否继续?',
          onOk: handleRetryTask.bind(null, record.id),
        },
      },
    ];
  }
  function handleDelete() {
    const selectKeys = activeKey.value == 0 ? getSelectRowKeys() : getTaskSelectRowKeys();
    if (!selectKeys.length) return createMessage.error(t('common.selectDataTip'));
    createConfirm({
      iconType: 'warning',
      title: t('common.tipTitle'),
      content: t('common.batchDelTip'),
      onOk: () => {
        const methods = activeKey.value == 0 ? delMonitorList : delTaskMonitor;
        methods({ ids: selectKeys }).then(res => {
          createMessage.success(res.msg);
          handleSearch();
        });
      },
    });
  }
  function toDetail(record) {
    let data: any = {
      id: record.id,
      flowId: record.flowId,
      opType: 6,
    };
    if (activeKey.value == 1) data.type = 2;
    openFlowParser(true, data);
  }
  function handleRetryTask(id) {
    retryTask(id)
      .then(res => {
        createMessage.success(res.msg);
        handleSearch();
      })
      .catch(() => {
        handleSearch();
      });
  }
  function getFlowEngineList() {
    getTreeList().then(res => {
      updateSchema({ field: 'templateId', componentProps: { options: res.data.list || [] } });
    });
  }
  async function getOptions() {
    const res = await baseStore.getDictionaryData('businessType');
    updateSchema({ field: 'flowCategory', componentProps: { options: res } });
    getFlowEngineList();
  }
  function handleSubmit(data) {
    let obj = {};
    for (let [key, value] of Object.entries(data)) {
      if (value || value == 0) {
        if (Array.isArray(value)) {
          if (value.length) obj[key] = value;
        } else {
          obj[key] = value;
        }
      }
    }
    searchInfo.value = obj;
    handleSearch();
  }
  function handleReset() {
    searchInfo.value = {};
    handleSearch();
  }
  function handleSearch() {
    nextTick(() => (activeKey.value == 0 ? reload() : reloadTaskTable()));
  }
  function getTaskStatusContent(value = 0) {
    if (value === 1) return '进行中';
    if (value === 2) return '已完成';
    if (value === 4) return '终止';
    if (value === 10) return '异常';
  }
  function getTaskStatusColor(value = 0) {
    if (value === 1) return '#F09437';
    if (value === 2) return 'rgba(35,162,5,0.39)';
    if (value === 4) return 'rgba(241,61,61,0.85)';
    if (value === 10) return 'rgba(241,61,61,0.85)';
  }
  function onTabChange(value) {
    formKey.value = +new Date();
    if (value == 0) getOptions();
    handleReset();
  }

  onMounted(() => {
    handleSearch();
    getOptions();
  });
</script>
