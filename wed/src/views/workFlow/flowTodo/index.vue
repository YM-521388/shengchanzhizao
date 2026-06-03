<template>
  <div class="jnpf-content-wrapper">
    <div class="jnpf-content-wrapper-center">
      <div class="jnpf-content-wrapper-search-box">
        <BasicForm class="search-form" @advanced-change="redoHeight" @register="registerForm" @submit="handleSubmit" @reset="handleReset" />
      </div>
      <div class="jnpf-content-wrapper-content bg-white">
        <a-tabs v-model:activeKey="state.activeKey" class="jnpf-content-wrapper-tabs" @change="onTabChange">
          <a-tab-pane v-for="tab in tabList" :key="tab.id" :tab="tab.fullName"></a-tab-pane>
        </a-tabs>
        <BasicTable @register="registerTable" :searchInfo="getSearchInfo">
          <template #tableTitle>
            <a-button type="primary" preIcon="icon-ym icon-ym-btn-batch" @click="handleBatch">批量办理</a-button>
            <a-button type="warning" preIcon="icon-ym icon-ym-btn-batch" @click="handleBatch('sign')" v-if="appStore?.getSysConfigInfo?.flowSign"
              >批量退签</a-button
            >
          </template>
          <template #bodyCell="{ column, record }">
            <template v-if="column.key === 'fullName'">
              <a-tag color="success" v-if="record.delegateUser">代理</a-tag>
              {{ record.fullName }}
            </template>
            <template v-if="column.key === 'flowUrgent'">
              <JnpfTextTag :content="getUrgentText(record.flowUrgent)" :color="getUrgentTextColor(record.flowUrgent)" :showTag="false" showTextColor />
            </template>
            <template v-if="column.key === 'status'">
              <JnpfTextTag :content="getHandlingStatusContent(record.status)" :color="getHandlingStatusColor(record.status)" />
            </template>
            <template v-if="column.key === 'action'">
              <TableAction :actions="getTableActions(record)" />
            </template>
          </template>
        </BasicTable>
      </div>
    </div>
    <FlowParser @register="registerFlowParser" @reload="reload" />
  </div>
</template>
<script lang="ts" setup>
  import { onMounted, reactive, nextTick, computed } from 'vue';
  import { getFlowBeforeList, batchHandle, batchSign } from '@/api/workFlow/task';
  import { getTreeList } from '@/api/workFlow/template';
  import { BasicTable, useTable, TableAction, BasicColumn, ActionItem } from '@/components/Table';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useI18n } from '@/hooks/web/useI18n';
  import { usePopup } from '@/components/Popup';
  import { useBaseStore } from '@/store/modules/base';
  import { BasicForm, useForm } from '@/components/Form';
  import { useDefineSetting } from '@/hooks/setting/useDefineSetting';
  import FlowParser from '@/views/workFlow/components/FlowParser.vue';
  import dayjs from 'dayjs';
  import { useAppStore } from '@/store/modules/app';

  interface State {
    listQuery: any;
    activeKey: string;
  }

  defineOptions({ name: 'workFlow-flowTodo' });

  const state = reactive<State>({
    listQuery: {
      keyword: '',
      startTime: '',
      endTime: '',
      flowCategory: '',
      creatorUserId: '',
      templateId: '',
      flowUrgent: '',
    },
    activeKey: '',
  });
  const { createMessage, createConfirm } = useMessage();
  const baseStore = useBaseStore();
  const appStore = useAppStore();
  const { t } = useI18n();
  const { flowUrgentList, getUrgentText, getUrgentTextColor, getHandlingStatusContent, getHandlingStatusColor } = useDefineSetting();
  const [registerFlowParser, { openPopup: openFlowParser }] = usePopup();
  const columns: BasicColumn[] = [
    { title: '流程标题', dataIndex: 'fullName', width: 200 },
    { title: '所属流程', dataIndex: 'flowName', width: 150 },
    { title: '发起时间', dataIndex: 'startTime', width: 150, format: 'date|YYYY-MM-DD HH:mm:ss' },
    { title: '发起人员', dataIndex: 'creatorUser', width: 120 },
    { title: '审批节点', dataIndex: 'currentNodeName', width: 150 },
    { title: '紧急程度', dataIndex: 'flowUrgent', width: 100, align: 'center' },
    { title: '流程状态', dataIndex: 'status', width: 120, align: 'center', helpMessage: '流转：上个节点提交或同意的审批数据' },
    { title: '接收时间', dataIndex: 'creatorTime', width: 150, format: 'date|YYYY-MM-DD HH:mm:ss' },
  ];
  const tabList = [
    { fullName: '全部', id: '' },
    { fullName: '协办', id: 7 },
    { fullName: '退回', id: 5 },
    { fullName: '超时', id: -2 },
  ];
  const [registerForm, { updateSchema }] = useForm({
    baseColProps: { span: 6 },
    showActionButtonGroup: true,
    showAdvancedButton: true,
    compact: true,
    schemas: [
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
        field: 'flowUrgent',
        label: '紧急程度',
        component: 'Select',
        componentProps: { showSearch: true, options: flowUrgentList },
      },
    ],
    fieldMapToTime: [['pickerVal', ['startTime', 'endTime']]],
  });
  const [registerTable, { reload, getSelectRows, clearSelectedRowKeys, redoHeight }] = useTable({
    api: getFlowBeforeList,
    columns,
    rowSelection: { type: 'checkbox' },
    clickToRowSelect: false,
    clearSelectOnPageChange: true,
    actionColumn: {
      width: 50,
      title: '操作',
      dataIndex: 'action',
    },
  });

  const getSearchInfo = computed(() => ({ category: 1, ...state.listQuery }));

  function getTableActions(record): ActionItem[] {
    return [
      {
        label: '办理',
        onClick: toDetail.bind(null, record),
      },
    ];
  }
  function toDetail(record) {
    const data = {
      id: record.taskId,
      flowId: record.flowId,
      opType: 2,
      operatorId: record.id,
    };
    openFlowParser(true, data);
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
  function handleSubmit(values) {
    state.listQuery = { status: state.listQuery.status, ...values };
    nextTick(() => reload());
  }
  function handleReset() {
    state.listQuery.keyword = '';
    state.listQuery.startTime = '';
    state.listQuery.endTime = '';
    state.listQuery.creatorUserId = '';
    state.listQuery.flowCategory = '';
    state.listQuery.templateId = '';
    state.listQuery.flowUrgent = '';
    nextTick(() => reload());
  }
  function onTabChange(val) {
    state.listQuery.status = val;
    nextTick(() => reload());
  }
  function handleBatch(type?) {
    const list: any[] = getSelectRows() || [];
    if (!list.length) return createMessage.error(t('common.selectDataTip'));
    const query: any = {
      ids: list.map(item => item.id),
    };
    if (type == 'sign') query['type'] = 1;
    createConfirm({
      iconType: 'warning',
      title: t('common.tipTitle'),
      content: type == 'sign' ? '确定退签，确定后进入我的待签' : '确定办理，办理后进入我的在办。',
      onOk: () => {
        const method = type == 'sign' ? batchSign : batchHandle;
        method(query).then(res => {
          createMessage.success(res.msg);
          clearSelectedRowKeys();
          reload();
        });
      },
    });
  }

  onMounted(() => {
    getOptions();
  });
</script>
