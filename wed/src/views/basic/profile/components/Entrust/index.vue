<template>
  <div class="jnpf-content-wrapper mt-10px">
    <div class="jnpf-content-wrapper-center">
      <div class="jnpf-content-wrapper-search-box">
        <BasicForm class="search-form" @register="registerSearchForm" @submit="handleSubmit" @reset="handleReset"></BasicForm>
      </div>
      <div class="jnpf-content-wrapper-content bg-white">
        <a-tabs v-model:activeKey="activeKey" class="jnpf-content-wrapper-tabs" destroyInactiveTabPane>
          <a-tab-pane :key="item.id" :tab="item.fullName" v-for="item in tabList"></a-tab-pane>
        </a-tabs>
        <BasicTable @register="registerMyEntrustTable" :searchInfo="getSearchInfo" :columns="getColumns">
          <template #headerTop v-if="activeKey == 1 || activeKey == 3">
            <a-alert
              :message="activeKey == 1 ? '委托是指允许受委托人代替委托人在系统中发起流程。' : '代理是指允许代理人代替被代理人在系统中处理流程审批。'"
              showIcon
              type="warning"
              class="mt-12px" />
          </template>
          <template #tableTitle v-if="activeKey == 1 || activeKey == 3">
            <a-button type="primary" preIcon="icon-ym icon-ym-btn-add" @click="addOrUpdateHandle()">新建</a-button>
          </template>
          <template #expandedRowRender="{ record }" v-if="activeKey == 1 || activeKey == 3">
            <BasicTable @register="registerUserTable" :data-source="record.userList">
              <template #bodyCell="{ column, record }">
                <template v-if="column.key === 'status'">
                  <a-tag :color="getConfirmStatusColor(record.status)">{{ getUserStatusContent(record.status) }}</a-tag>
                </template>
              </template>
            </BasicTable>
          </template>
          <template #bodyCell="{ column, record }">
            <template v-if="column.key === 'status'">
              <a-tag :color="getStatusColor(record.status)">{{ getStatusContent(record.status) }}</a-tag>
            </template>
            <template v-if="column.key === 'confirmStatus'">
              <a-tag :color="getConfirmStatusColor(record.confirmStatus)">{{ getConfirmStatusContent(record.confirmStatus) }}</a-tag>
            </template>
            <template v-if="column.key === 'action'">
              <TableAction :actions="getTableActions(record)" />
            </template>
          </template>
        </BasicTable>
      </div>
    </div>
    <Form @register="registerForm" @reload="reload" />
  </div>
</template>
<script lang="ts" setup>
  import { reactive, toRefs, onMounted, computed, nextTick, watch } from 'vue';
  import { getFlowDelegateList, del, stop, notarize, getFlowDelegateInfo } from '@/api/workFlow/flowDelegate';
  import { BasicForm, useForm } from '@/components/Form';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useI18n } from '@/hooks/web/useI18n';
  import { BasicTable, useTable, TableAction, BasicColumn, ActionItem } from '@/components/Table';
  import { useModal } from '@/components/Modal';
  import Form from './Form.vue';
  import { useRoute } from 'vue-router';

  interface State {
    activeKey: number;
    keyword: string;
    userList: any[];
    activeUser: string;
  }

  defineOptions({ name: 'workFlow-entrust' });

  const { createMessage } = useMessage();
  const { t } = useI18n();
  const [registerForm, { openModal: openFormModal }] = useModal();
  const state = reactive<State>({
    activeKey: 1,
    keyword: '',
    userList: [],
    activeUser: '',
  });
  const { activeKey } = toRefs(state);
  const tabList = [
    { fullName: '我的委托', id: 1 },
    { fullName: '委托给我', id: 2 },
    { fullName: '我的代理', id: 3 },
    { fullName: '代理给我', id: 4 },
  ];
  const [registerSearchForm, { resetFields }] = useForm({
    baseColProps: { span: 6 },
    showActionButtonGroup: true,
    showAdvancedButton: true,
    compact: true,
    schemas: [
      {
        field: 'keyword',
        label: t('common.keyword'),
        component: 'Input',
        componentProps: {
          placeholder: t('common.enterKeyword'),
          submitOnPressEnter: true,
        },
      },
    ],
  });
  const userColumns: BasicColumn[] = [
    { title: '', dataIndex: 'entrust', width: 38 },
    { title: '序号', dataIndex: 'index', width: 50, align: 'center', customRender: ({ index }) => index + 1 },
    { title: '受委托人', dataIndex: 'toUserName', width: 650 },
    { title: '流程状态', dataIndex: 'status', width: 120, align: 'center' },
    { title: '', dataIndex: 'flow' },
  ];
  const [registerMyEntrustTable, { reload }] = useTable({
    api: getFlowDelegateList,
    actionColumn: {
      width: 150,
      title: '操作',
      dataIndex: 'action',
    },
    immediate: false,
    rowKey: 'jnpfId',
    afterFetch: data => {
      const list = data.map(o => ({
        ...o,
        jnpfId: o.id + Math.random(),
      }));
      return list;
    },
    onExpand: handleExpand,
  });
  const [registerUserTable] = useTable({
    columns: userColumns,
    showHeader: false,
    showTableSetting: false,
    pagination: false,
    showIndexColumn: false,
    immediate: false,
  });

  watch(
    () => state.activeKey,
    () => {
      resetFields();
    },
  );

  const getSearchInfo = computed(() => ({ keyword: state.keyword, type: state.activeKey }));
  const getColumns = computed(() => {
    const myEntrustColumns: BasicColumn[] = [
      { title: '受委托人', dataIndex: 'toUserName', width: 200 },
      { title: '委托流程', dataIndex: 'flowName', width: 150 },
      { title: '开始时间', dataIndex: 'startTime', width: 150, format: 'date|YYYY-MM-DD HH:mm:ss' },
      { title: '结束时间', dataIndex: 'endTime', width: 150, format: 'date|YYYY-MM-DD HH:mm:ss' },
      { title: '状态', dataIndex: 'status', width: 120, align: 'center' },
      { title: '委托说明', dataIndex: 'description' },
    ];
    const entrustColumns: BasicColumn[] = [
      { title: '委托人', dataIndex: 'userName', width: 200 },
      { title: '委托流程', dataIndex: 'flowName', width: 150 },
      { title: '开始时间', dataIndex: 'startTime', width: 150, format: 'date|YYYY-MM-DD HH:mm:ss' },
      { title: '结束时间', dataIndex: 'endTime', width: 150, format: 'date|YYYY-MM-DD HH:mm:ss' },
      { title: '生效状态', dataIndex: 'status', width: 120, align: 'center' },
      { title: '委托说明', dataIndex: 'description' },
      { title: '确认状态', dataIndex: 'confirmStatus', width: 120, align: 'center' },
    ];
    if (state.activeKey === 1) return myEntrustColumns;
    if (state.activeKey === 2) return entrustColumns;
    if (state.activeKey === 3 || state.activeKey === 4) {
      let list = state.activeKey === 3 ? myEntrustColumns : entrustColumns;
      list[0].title = state.activeKey === 3 ? '代理人' : '被代理人';
      list[1].title = '代理流程';
      list[5].title = '代理说明';
      return list;
    }
  });

  function handleExpand(expanded, record) {
    if (!expanded || record.userList?.length) return;
    getFlowDelegateInfo(record.id).then(res => {
      record.userList = res.data;
    });
  }
  function getTableActions(record): ActionItem[] {
    if (state.activeKey === 1 || state.activeKey === 3) {
      return [
        {
          label: t('common.editText'),
          disabled: record.status !== 0 || !record.isEdit,
          onClick: addOrUpdateHandle.bind(null, record.id),
        },
        {
          label: t('common.delText'),
          color: 'error',
          disabled: record.status === 1,
          modelConfirm: {
            onOk: handleDelete.bind(null, record.id),
          },
        },
        {
          ifShow: record.status === 1,
          label: '终止',
          color: 'error',
          modelConfirm: {
            onOk: handleStop.bind(null, record.id),
            content: '终止后，流程不再进行委托！',
          },
        },
      ];
    } else {
      return [
        {
          ifShow: record.status !== 2 && record.confirmStatus === 0,
          label: '接受',
          modelConfirm: {
            onOk: handleAcceptOrReject.bind(null, record.id, 1),
            content: '您确认要接受请求吗，是否继续？',
          },
        },
        {
          ifShow: record.status !== 2 && record.confirmStatus === 0,
          label: '拒绝',
          color: 'error',
          modelConfirm: {
            onOk: handleAcceptOrReject.bind(null, record.id, 2),
            content: '您确认要拒绝请求吗，是否继续？',
          },
        },
      ];
    }
  }
  function handleStop(id) {
    stop(id).then(res => {
      createMessage.success(res.msg);
      reload();
    });
  }
  function handleAcceptOrReject(id, type) {
    notarize(id, type).then(res => {
      createMessage.success(res.msg);
      reload();
    });
  }
  function handleSubmit(values) {
    state.keyword = values?.keyword || '';
    handleSearch();
  }
  function handleReset() {
    state.keyword = '';
    handleSearch();
  }
  function handleSearch() {
    nextTick(() => reload({ page: 1 }));
  }
  // 新增委托/代理
  function addOrUpdateHandle(id = '') {
    openFormModal(true, { id, type: state.activeKey === 1 ? 0 : 1 });
  }
  // 删除委托
  function handleDelete(id) {
    del(id).then(res => {
      createMessage.success(res.msg);
      reload();
    });
  }
  function getUserStatusContent(status) {
    return status === 0 ? '待确认' : status === 1 ? '已接受' : '已拒绝';
  }
  function getStatusContent(status) {
    return status === 0 ? '未生效' : status === 1 ? '生效中' : '已失效';
  }
  function getStatusColor(status) {
    return status === 0 ? '' : status === 1 ? 'processing' : 'error';
  }
  function getConfirmStatusContent(status) {
    return status === 0 ? '待确认' : status === 1 ? '已接受' : '已拒绝';
  }
  function getConfirmStatusColor(status) {
    return status === 0 ? '' : status === 1 ? 'success' : 'error';
  }

  onMounted(() => {
    const route = useRoute();
    if (route.query.config) {
      state.activeKey = Number(route.query.config) || 1;
      nextTick(() => reload({ page: 1 }));
    } else {
      reload({ page: 1 });
    }
  });
</script>
