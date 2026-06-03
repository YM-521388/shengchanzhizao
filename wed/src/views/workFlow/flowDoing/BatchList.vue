<template>
  <BasicPopup v-bind="$attrs" @register="registerPopup" title="批量审批" class="full-popup">
    <BasicTable :columns="columns" @register="registerTable">
      <template #headerTop>
        <a-alert message="请先选择所属流程、名称和节点。（同一流程同一节点的审批数据才能使用批量审批）" type="warning" show-icon />
      </template>
      <template #tableTitle>
        <a-button color="success" :disabled="!state.nodeCode || state.allBtnDisabled" @click="handleBatch(2)">批量转审</a-button>
        <a-button color="primary" :disabled="!state.nodeCode || state.allBtnDisabled" @click="handleBatch(0)">批量同意</a-button>
        <a-button color="error" :disabled="!state.nodeCode || state.allBtnDisabled" @click="handleBatch(1)">批量拒绝</a-button>
        <a-button color="warning" :disabled="!state.nodeCode || state.allBtnDisabled" @click="handleBatch(3)">批量退回</a-button>
      </template>
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'fullName'">
          <a-tag color="success" v-if="record.delegateUser">代理</a-tag>
          {{ record.fullName }}
        </template>
        <template v-if="column.key === 'flowVersion'">
          <a-tag color="processing">V:{{ record.flowVersion }}</a-tag>
        </template>
        <template v-if="column.key === 'flowUrgent'">
          <JnpfTextTag :content="getUrgentText(record.flowUrgent)" :color="getUrgentTextColor(record.flowUrgent)" :showTag="false" showTextColor />
        </template>
        <template v-if="column.key === 'status'">
          <JnpfTextTag :content="getHandlingStatusContent(record.status)" :color="getHandlingStatusColor(record.status)" />
        </template>
      </template>
    </BasicTable>
    <ErrorModal @register="registerError" @confirm="handleBatchOperation" />
    <ActionModal @register="registerAction" @confirm="approvalReceiver" />
    <ApprovalModal @register="registerApproval" @confirm="approvalReceiver" />
  </BasicPopup>
</template>
<script lang="ts" setup>
  import {
    getFlowBeforeList,
    getBatchFlowSelector,
    getBatchVersionSelector,
    getBatchNodeSelector,
    getBatchNode,
    getBatchCandidate,
    batchOperation,
    getBackNodeCodeList,
  } from '@/api/workFlow/task';
  import { reactive, watch } from 'vue';
  import { BasicPopup, usePopupInner } from '@/components/Popup';
  import { BasicTable, useTable, BasicColumn } from '@/components/Table';
  import { useI18n } from '@/hooks/web/useI18n';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useBaseStore } from '@/store/modules/base';
  import { useDefineSetting } from '@/hooks/setting/useDefineSetting';
  import { useModal } from '@/components/Modal';
  import ApprovalModal from '@/views/workFlow/components/modal/ApprovalModal.vue';
  import ActionModal from '@/views/workFlow/components/modal/ActionModal.vue';
  import ErrorModal from '@/views/workFlow/components/modal/ErrorModal.vue';

  interface State {
    batchType: number;
    candidateType: number;
    allBtnDisabled: boolean;
    approvalData: any;
    flowId: string;
    nodeCode: string;
    clickReset: boolean;
  }

  defineEmits(['register']);
  const baseStore = useBaseStore();
  const { createMessage } = useMessage();
  const { t } = useI18n();
  const [registerPopup] = usePopupInner(init);
  const [registerError, { openModal: openErrorModal, closeModal: closeErrorModal }] = useModal();
  const [registerAction, { openModal: openActionModal, closeModal: closeActionModal, setModalProps: setActionModalProps }] = useModal();
  const [registerApproval, { openModal: openApprovalModal, closeModal: closeApprovalModal, setModalProps: setApprovalModalProps }] = useModal();
  const { getUrgentText, getUrgentTextColor, getHandlingStatusContent, getHandlingStatusColor } = useDefineSetting();
  const columns: BasicColumn[] = [
    { title: '流程标题', dataIndex: 'fullName', width: 200 },
    { title: '所属流程', dataIndex: 'flowName', width: 150 },
    { title: '流程版本', dataIndex: 'flowVersion', width: 130, align: 'center' },
    { title: '发起时间', dataIndex: 'startTime', width: 150, format: 'date|YYYY-MM-DD HH:mm:ss' },
    { title: '发起人员', dataIndex: 'creatorUser', width: 120 },
    { title: '审批节点', dataIndex: 'currentNodeName', width: 150 },
    { title: '紧急程度', dataIndex: 'flowUrgent', width: 100, align: 'center' },
    { title: '流程状态', dataIndex: 'status', width: 120, align: 'center' },
    { title: '接收时间', dataIndex: 'creatorTime', width: 150, format: 'date|YYYY-MM-DD HH:mm:ss' },
  ];
  const defaultApprovalData = {
    branchList: [],
    fileList: [],
    handleOpinion: '',
    signImg: '',
    copyIds: '',
    backNodeCode: '',
    backType: 1,
  };

  const searchInfo = reactive({
    category: 5,
  });
  const state = reactive<State>({
    batchType: 0,
    candidateType: 1,
    allBtnDisabled: false,
    approvalData: {},
    flowId: '',
    nodeCode: '',
    clickReset: false,
  });
  const [registerTable, { getForm, reload, getSelectRows, getSelectRowKeys, clearSelectedRowKeys, setTableData }] = useTable({
    api: getFlowBeforeList,
    searchInfo: searchInfo,
    useSearchForm: true,
    formConfig: {
      schemas: [
        {
          field: 'templateId',
          label: '所属流程',
          component: 'Select',
          componentProps: { showSearch: true, onChange: onTemplateIdChange },
        },
        {
          field: 'flowId',
          label: '所属版本',
          component: 'Select',
          componentProps: { showSearch: true, onChange: onFlowIdChange, onDropdownVisibleChange: visibleFlowIdChange },
        },
        {
          field: 'nodeCode',
          label: '所属节点',
          component: 'Select',
          componentProps: { showSearch: true, onChange: onNodeCodeChange, onDropdownVisibleChange: visibleNodeCodeChange },
        },
      ],
      submitFunc: async () => {
        setTableData([]);
        if (state.clickReset) return (state.clickReset = false);
        const values = getForm().getFieldsValue();
        if (!values.templateId) return createMessage.warning('请先选择所属流程');
        if (!values.flowId) return createMessage.warning('请先选择所属版本');
        if (!values.nodeCode) return createMessage.warning('请先选择所属节点');
        reload({ page: 1 });
      },
      resetFunc: async () => {
        getForm().updateSchema([
          { field: 'flowId', componentProps: { options: [] } },
          { field: 'nodeCode', componentProps: { options: [] } },
        ]);
        state.nodeCode = '';
        state.clickReset = true;
      },
    },
    immediate: false,
    tableSetting: { setting: false, redo: false },
    rowSelection: { type: 'checkbox' },
    clickToRowSelect: false,
  });

  watch(
    () => state.nodeCode,
    val => {
      if (val) {
        clearSelectedRowKeys();
      }
    },
  );

  function init() {
    state.nodeCode = '';
    clearSelectedRowKeys();
    getOptions();
    state.allBtnDisabled = false;
    state.clickReset = false;
  }
  function onNodeCodeChange(val) {
    state.nodeCode = val || '';
  }
  async function getOptions() {
    const res = await baseStore.getDictionaryData('businessType');
    getForm().updateSchema({ field: 'flowCategory', componentProps: { options: res } });
    getBatchTemplateOptions();
  }
  function getBatchTemplateOptions() {
    getBatchFlowSelector().then(res => {
      getForm().updateSchema({ field: 'templateId', componentProps: { options: res.data } });
    });
  }
  function onTemplateIdChange(val) {
    state.nodeCode = '';
    getForm().setFieldsValue({ flowId: '', nodeCode: '' });
    getForm().updateSchema([{ field: 'flowId', componentProps: { options: [] } }]);
    getForm().updateSchema([{ field: 'nodeCode', componentProps: { options: [] } }]);
    if (!val) return;
    getFlowIdChange(val);
  }
  function getFlowIdChange(val) {
    getBatchVersionSelector(val).then(res => {
      getForm().updateSchema({ field: 'flowId', componentProps: { options: res.data } });
    });
  }
  function onFlowIdChange(val) {
    state.nodeCode = '';
    getForm().setFieldsValue({ nodeCode: '' });
    getForm().updateSchema({ field: 'nodeCode', componentProps: { options: [] } });
    if (!val) return;
    getNodeOptions(val);
  }
  function getNodeOptions(val) {
    getBatchNodeSelector(val).then(res => {
      getForm().updateSchema({ field: 'nodeCode', componentProps: { options: res.data } });
    });
  }
  function visibleFlowIdChange(val) {
    if (!val) return;
    const values = getForm().getFieldsValue();
    if (!values.templateId) createMessage.warning('请先选择所属流程');
  }
  function visibleNodeCodeChange(val) {
    if (!val) return;
    const values = getForm().getFieldsValue();
    if (!values.flowId) createMessage.warning('请先选择所属版本');
  }
  /**
   * 批量审批
   * @param batchType  0-通过 1-拒绝 2-转审 3-退回
   */
  async function handleBatch(batchType) {
    state.batchType = batchType;
    const selectedData = getSelectRows();
    if (!selectedData.length) return createMessage.error(t('common.selectDataTip'));
    let isDiffer = selectedData.some(o => o.version !== selectedData[0].version);
    if (isDiffer) return createMessage.error('请选择相同的版本审批单');
    state.approvalData = { ...defaultApprovalData };
    const item = selectedData[0];
    const res = await getBatchNode(item.flowId, state.nodeCode);
    const properties = res.data || {};
    if (batchType === 0 || batchType === 1) {
      if (batchType === 0 && !properties.hasAuditBtn) return createMessage.error('当前审批节点无同意权限');
      if (batchType === 1 && !properties.hasRejectBtn) return createMessage.error('当前审批节点无拒绝权限');
      state.allBtnDisabled = true;
      const query = { flowId: item.flowId, operatorId: item.id, batchType: state.batchType };
      getBatchCandidate(query)
        .then(res => {
          const data = res.data;
          state.allBtnDisabled = false;
          state.candidateType = data.type;
          const candidateList = res.data.list.filter(o => !o.isBranchFlow && o.isCandidates);
          openApprovalModal(true, {
            branchList: [],
            candidateList,
            backNodeCodeList: [],
            taskId: item.id,
            formData: {
              flowId: item.flowId,
              data: {},
            },
            eventType: batchType == 0 ? 'audit' : 'reject',
            properties,
          });
        })
        .catch(() => {
          state.allBtnDisabled = false;
        });
      return;
    }
    if (batchType === 2) {
      if (!properties.hasTransferBtn) return createMessage.error('当前审批节点无转审权限');
      openActionModal(true, { properties: properties, eventType: 'transfer' });
    }
    if (batchType == 3) {
      if (!properties.hasBackBtn) return createMessage.error('当前审批节点无退回权限');
      state.allBtnDisabled = true;
      getBackNodeCodeList(item.id)
        .then(res => {
          state.allBtnDisabled = false;
          const data = {
            backNodeCodeList: res.data.list || [],
            branchList: [],
            candidateList: [],
            taskId: item.id,
            formData: {
              flowId: item.flowId,
              data: {},
            },
            eventType: 'back',
            properties,
          };
          openApprovalModal(true, data);
        })
        .catch(() => {
          state.allBtnDisabled = false;
        });
    }
  }
  function approvalReceiver(data) {
    state.approvalData = data;
    handleBatchOperation();
  }
  function handleBatchOperation(errorRuleUserList: any = null) {
    state.allBtnDisabled = true;
    const query = {
      ...state.approvalData,
      batchType: state.batchType,
      candidateType: state.candidateType,
      ids: getSelectRowKeys(),
    };
    if (errorRuleUserList) query.errorRuleUserList = errorRuleUserList;
    batchOperation(query)
      .then(res => {
        handleError(res);
      })
      .catch(() => {
        state.allBtnDisabled = false;
        setActionModalProps({ confirmLoading: false });
        setApprovalModalProps({ confirmLoading: false });
      });
  }
  // 异常处理
  function handleError(res) {
    const errorData = res.data;
    if (errorData && Array.isArray(errorData) && errorData.length) {
      state.allBtnDisabled = false;
      openErrorModal(true, { errorData });
    } else {
      createMessage.success(res.msg).then(() => {
        closeErrorModal();
        closeApprovalModal();
        closeActionModal();
        clearSelectedRowKeys();
        state.allBtnDisabled = false;
        reload();
      });
    }
  }
</script>
