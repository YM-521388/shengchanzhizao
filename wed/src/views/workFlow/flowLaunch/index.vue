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
        <BasicTable @register="registerTable" :searchInfo="state.listQuery">
          <template #tableTitle>
            <a-button type="primary" preIcon="icon-ym icon-ym-btn-add" @click="openFlowPopup()">{{ t('routes.workFlow-addFlow') }}</a-button>
          </template>
          <template #bodyCell="{ column, record }">
            <template v-if="column.key === 'fullName'">
              <a-tag color="success" v-if="record.delegateUser">委托</a-tag>
              {{ record.fullName }}
            </template>
            <template v-if="column.key === 'flowUrgent'">
              <JnpfTextTag :content="getUrgentText(record.flowUrgent)" :color="getUrgentTextColor(record.flowUrgent)" :showTag="false" showTextColor />
            </template>
            <template v-if="column.key === 'status'">
              <JnpfTextTag :content="getFlowStatusContent(record.status)" :color="getFlowStatusColor(record.status)" />
            </template>
            <template v-if="column.key === 'action'">
              <TableAction :actions="getTableActions(record)" />
            </template>
          </template>
        </BasicTable>
      </div>
    </div>
    <FlowPopup @register="registerFlowPopup" @select="onSelect" />
    <FlowParser @register="registerFlowParser" @reload="reload" />
  </div>
</template>
<script lang="ts" setup>
  import { onMounted, reactive, nextTick } from 'vue';
  import { getFlowLaunchList, delFlowLaunch } from '@/api/workFlow/task';
  import { getTreeList } from '@/api/workFlow/template';
  import { BasicTable, useTable, TableAction, BasicColumn, ActionItem } from '@/components/Table';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useI18n } from '@/hooks/web/useI18n';
  import { usePopup } from '@/components/Popup';
  import { useBaseStore } from '@/store/modules/base';
  import { BasicForm, useForm } from '@/components/Form';
  import FlowPopup from './FlowPopup.vue';
  import FlowParser from '@/views/workFlow/components/FlowParser.vue';
  import { useDefineSetting } from '@/hooks/setting/useDefineSetting';
  import dayjs from 'dayjs';
  defineOptions({ name: 'workFlow-flowLaunch' });

  interface State {
    listQuery: any;
    activeKey: string;
  }

  const { t } = useI18n();
  const { createMessage } = useMessage();
  const baseStore = useBaseStore();
  const state = reactive<State>({
    listQuery: {
      keyword: '',
      startTime: '',
      endTime: '',
      flowCategory: '',
      templateId: '',
      flowUrgent: '',
    },
    activeKey: '',
  });
  const { flowUrgentList, getUrgentText, getUrgentTextColor, getFlowStatusContent, getFlowStatusColor } = useDefineSetting();
  const [registerFlowPopup, { openPopup: openFlowPopup }] = usePopup();
  const [registerFlowParser, { openPopup: openFlowParser }] = usePopup();
  const columns: BasicColumn[] = [
    { title: '流程标题', dataIndex: 'fullName', width: 200 },
    { title: '所属流程', dataIndex: 'flowName', width: 150 },
    { title: '发起时间', dataIndex: 'startTime', width: 150, format: 'date|YYYY-MM-DD HH:mm:ss' },
    { title: '审批节点', dataIndex: 'currentNodeName', width: 150 },
    { title: '紧急程度', dataIndex: 'flowUrgent', width: 100, align: 'center' },
    { title: '流程状态', dataIndex: 'status', width: 120, align: 'center' },
  ];
  const tabList = [
    { fullName: '全部', id: '' },
    { fullName: '待提交', id: 0 },
    { fullName: '进行中', id: 1 },
    { fullName: '已完成', id: 2 },
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
        field: 'flowUrgent',
        label: '紧急程度',
        component: 'Select',
        componentProps: { showSearch: true, options: flowUrgentList },
      },
    ],
    fieldMapToTime: [['pickerVal', ['startTime', 'endTime']]],
  });
  const [registerTable, { reload, redoHeight }] = useTable({
    api: getFlowLaunchList,
    columns,
    actionColumn: {
      width: 150,
      title: '操作',
      dataIndex: 'action',
    },
  });

  function getTableActions(record): ActionItem[] {
    return [
      {
        label: t('common.editText'),
        disabled: ![0, 8, 9].includes(record.status),
        onClick: toDetail.bind(null, record, '-1'),
      },
      {
        label: t('common.delText'),
        color: 'error',
        disabled: ![0, 9].includes(record.status),
        modelConfirm: {
          onOk: handleDelete.bind(null, record.id),
        },
      },
      {
        label: t('common.detailText'),
        disabled: !record.status,
        onClick: toDetail.bind(null, record, 0),
      },
    ];
  }
  function handleDelete(id) {
    delFlowLaunch(id).then(res => {
      createMessage.success(res.msg);
      reload();
    });
  }
  function toDetail(record, opType) {
    const data = {
      id: record.id,
      flowId: record.flowId,
      opType,
      parentId: record.parentId,
    };
    openFlowParser(true, data);
  }
  function onSelect(record) {
    const data = {
      id: '',
      flowId: record.id,
      opType: '-1',
      isFlow: 1,
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
    state.listQuery.flowCategory = '';
    state.listQuery.templateId = '';
    state.listQuery.flowUrgent = '';
    nextTick(() => reload());
  }
  function onTabChange(val) {
    state.listQuery.status = val;
    nextTick(() => reload());
  }

  onMounted(() => {
    getOptions();
  });
</script>
