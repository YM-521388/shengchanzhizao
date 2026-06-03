<template>
  <div class="jnpf-content-wrapper">
    <div class="jnpf-content-wrapper-center">
      <div class="jnpf-content-wrapper-content">
        <BasicTable @register="registerTable">
          <template #tableTitle>
            <a-button type="primary" preIcon="icon-ym icon-ym-btn-batch" @click="handleBatchSign()">批量签收</a-button>
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
  import { onMounted } from 'vue';
  import { getFlowBeforeList, batchSign } from '@/api/workFlow/task';
  import { getTreeList } from '@/api/workFlow/template';
  import { BasicTable, useTable, TableAction, BasicColumn, ActionItem } from '@/components/Table';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useI18n } from '@/hooks/web/useI18n';
  import { usePopup } from '@/components/Popup';
  import { useBaseStore } from '@/store/modules/base';
  import FlowParser from '@/views/workFlow/components/FlowParser.vue';
  import { useDefineSetting } from '@/hooks/setting/useDefineSetting';
  import dayjs from 'dayjs';

  defineOptions({ name: 'workFlow-flowToSign' });

  const baseStore = useBaseStore();
  const { t } = useI18n();
  const { createMessage, createConfirm } = useMessage();
  const { flowUrgentList, getUrgentText, getUrgentTextColor, getHandlingStatusContent, getHandlingStatusColor } = useDefineSetting();
  const [registerFlowParser, { openPopup: openFlowParser }] = usePopup();
  const columns: BasicColumn[] = [
    { title: '流程标题', dataIndex: 'fullName', width: 200 },
    { title: '所属流程', dataIndex: 'flowName', width: 150 },
    { title: '发起时间', dataIndex: 'startTime', width: 150, format: 'date|YYYY-MM-DD HH:mm:ss' },
    { title: '发起人员', dataIndex: 'creatorUser', width: 120 },
    { title: '审批节点', dataIndex: 'currentNodeName', width: 150 },
    { title: '紧急程度', dataIndex: 'flowUrgent', width: 100, align: 'center' },
    { title: '流程状态', dataIndex: 'status', width: 120, align: 'center' },
    { title: '接收时间', dataIndex: 'creatorTime', width: 150, format: 'date|YYYY-MM-DD HH:mm:ss' },
  ];
  const [registerTable, { reload, getForm, getSelectRows }] = useTable({
    api: getFlowBeforeList,
    columns,
    searchInfo: { category: 0 },
    useSearchForm: true,
    rowSelection: { type: 'checkbox' },
    clickToRowSelect: false,
    formConfig: {
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
    },
    clearSelectOnPageChange: true,
    actionColumn: {
      width: 50,
      title: '操作',
      dataIndex: 'action',
    },
  });

  function getTableActions(record): ActionItem[] {
    return [
      {
        label: '签收',
        onClick: toDetail.bind(null, record),
      },
    ];
  }
  function toDetail(record) {
    const data = {
      id: record.taskId,
      flowId: record.flowId,
      opType: 1,
      operatorId: record.id,
    };
    openFlowParser(true, data);
  }
  function getFlowEngineList() {
    getTreeList().then(res => {
      getForm().updateSchema({ field: 'templateId', componentProps: { options: res.data.list || [] } });
    });
  }
  async function getOptions() {
    const res = await baseStore.getDictionaryData('businessType');
    getForm().updateSchema({ field: 'flowCategory', componentProps: { options: res } });
    getFlowEngineList();
  }
  function handleBatchSign() {
    const list: any[] = getSelectRows() || [];
    if (!list.length) return createMessage.error(t('common.selectDataTip'));
    const query = {
      ids: list.map(item => item.id),
    };
    createConfirm({
      iconType: 'warning',
      title: t('common.tipTitle'),
      content: '确定签收，签收后进入待办。',
      onOk: () => {
        batchSign(query).then(res => {
          createMessage.success(res.msg);
          reload();
        });
      },
    });
  }

  onMounted(() => {
    getOptions();
  });
</script>
