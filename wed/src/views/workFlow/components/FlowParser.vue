<template>
  <BasicPopup
    v-bind="$attrs"
    @register="registerPopup"
    :showBackIcon="!config.hideCancelBtn"
    :showCancelBtn="false"
    destroyOnClose
    :closeFunc="onClose"
    :defaultFullscreen="defaultFullscreen"
    class="full-popup basic-flow-parser">
    <template #title>
      <a-space :size="10">
        <div class="header-title" :title="getTitle">{{ getTitle }}</div>
        <template v-if="(!loading || getTitle) && state.flowInfo?.type != 2">
          <a-dropdown :disabled="config.opType != '-1'">
            <template #overlay>
              <a-menu @click="handleUrgentClick">
                <a-menu-item :key="item.id" v-for="item in flowUrgentList">
                  <div class="flow-urgent-value" :class="'urgent' + item.id">{{ item.fullName }}</div>
                </a-menu-item>
              </a-menu>
            </template>
            <div class="flow-urgent-value" :class="'urgent' + state.flowUrgent">{{ getUrgentName }}</div>
          </a-dropdown>
        </template>
      </a-space>
    </template>
    <template #insertToolbar>
      <div class="header-options" v-if="config.opType == 6">
        <a-button
          :color="item.type"
          @click="handleBtnClick(item.id)"
          :loading="btnLoading && eventType == item.id"
          :disabled="loading || allBtnDisabled || (btnLoading && eventType !== item.id)"
          v-for="item in monitorBtnList">
          {{ item.fullName }}
        </a-button>
      </div>
      <div class="header-btn" v-else>
        <Tooltip title="查看发起表单" placement="bottom" v-if="btnInfo?.hasViewStartFormBtn">
          <a-button type="text" @click="viewStartForm">
            <i class="icon-ym icon-ym-report-icon-preview"></i>
          </a-button>
        </Tooltip>
        <Tooltip :title="getFullscreenTitle" placement="bottom" v-if="showFullscreen">
          <a-button type="text" @click="handleToggleFullscreen">
            <i class="icon-ym icon-ym-header-fullscreen" v-if="!isFullscreen"></i>
            <i class="icon-ym icon-ym-header-fullscreen-exit" v-else></i>
          </a-button>
        </Tooltip>
        <Tooltip title="流程图" placement="bottom" @click="handleOpenFlowInfo">
          <a-button type="text" :disabled="loading">
            <i class="icon-ym icon-ym-flow-node-condition" />
          </a-button>
        </Tooltip>
      </div>
    </template>
    <div class="jnpf-common-form-wrapper" v-loading="loading">
      <div class="jnpf-common-form-wrapper__main">
        <div class="approve-result" v-if="getFlowStatusIcon">
          <div class="approve-result-img" :class="getFlowStatusClass"></div>
        </div>
        <a-tabs v-model:activeKey="state.activeKey" class="flow-parser-tabs" :class="{ 'no-head-margin': state.activeKey == '3' }" v-if="config.opType == 6">
          <a-tab-pane key="2" tab="流程信息" class="!overflow-hidden">
            <FlowProcessMain
              :flowInfo="state.flowInfo"
              :nodeList="state.nodeList"
              :isPreview="true"
              :lineKeyList="state.lineKeyList"
              v-if="state.flowInfo.id"
              :key="flowKey"
              @viewSubFlow="viewSubFlow" />
          </a-tab-pane>
          <a-tab-pane key="3" tab="流转记录" class="!p-0" v-if="config.opType != '-1' || (config.opType == '-1' && config.status == 7)">
            <RecordList
              :list="state.recordList"
              :opType="config.opType"
              :type="state.flowInfo.type"
              :status="state.taskInfo?.status"
              @viewDetail="viewNodeDetail"
              v-if="state.activeKey == '3'" />
          </a-tab-pane>
        </a-tabs>
        <div class="flow-parser-container" v-else v-show="!loading">
          <div class="flow-top-container" v-if="config.opType != '-1'">
            <div class="item">
              流程类型：<span class="content" :title="taskInfo.flowCategory">{{ taskInfo.flowCategory }}</span>
            </div>
            <div class="item">
              流程状态：
              <span class="content" :title="getFlowStatusContent(taskInfo.status)">
                <JnpfTextTag :content="getFlowStatusContent(taskInfo.status)" :color="getFlowStatusColor(taskInfo.status)" />
              </span>
            </div>
            <div class="item">
              发起人：
              <span class="content" :title="taskInfo.creatorUser">
                <a-avatar :size="24" :src="apiUrl + taskInfo.headIcon" />
                {{ taskInfo.creatorUser }}
              </span>
            </div>
            <div class="item">
              发起时间：
              <span class="content" :title="formatToDateTime(taskInfo.creatorTime, 'YYYY-MM-DD HH:mm')">
                {{ formatToDateTime(taskInfo.creatorTime, 'YYYY-MM-DD HH:mm') }}
              </span>
            </div>
          </div>
          <div class="flow-form-container">
            <component
              :is="state.currentView"
              ref="formRef"
              :config="config"
              :key="formKey"
              @eventReceiver="eventReceiver"
              @setPageLoad="setPageLoad"
              @openRevokeFlow="openRevokeFlow" />
          </div>
          <div class="flow-approval-form" v-if="config.opType == '3' && (btnInfo?.hasRejectBtn || btnInfo?.hasAuditBtn)">
            <a-form :colon="false" :labelCol="{ style: { width: '100px' } }" class="opinion-btn" @click="handleShowApprovalForm">
              <a-form-item :label="state.approverText.approverLabel" name="handleOpinion" required>
                <jnpf-input disabled :placeholder="state.approverText.approverLabel" v-model:value="approvalForm.handleOpinion" />
              </a-form-item>
              <div class="cover-btn"></div>
            </a-form>
            <Drawer placement="bottom" :closable="false" :open="showApprovalForm" :get-container="false" :style="{ position: 'absolute' }" destroyOnClose>
              <div class="approval-form">
                <a-form :colon="false" :labelCol="{ style: { width: '100px' } }" :model="approvalForm" ref="formElRef">
                  <a-row :gutter="16">
                    <a-col :span="12">
                      <a-form-item :label="state.approverText.approver"> {{ userInfo.userName }} </a-form-item>
                    </a-col>
                    <a-col :span="12">
                      <a-form-item :label="state.approverText.approverTime">{{ formatToDate(+new Date()) }} </a-form-item>
                    </a-col>
                  </a-row>
                  <a-form-item :label="state.approverText.approverLabel" name="handleOpinion" required>
                    <OpinionTextarea
                      v-model:value="approvalForm.handleOpinion"
                      :messageText="state.properties?.type === 'processing' ? '请先填写办理意见' : '请先填写审批意见'" />
                  </a-form-item>
                  <a-form-item label="签名" name="signImg" :rules="[{ required: true, message: `请签名`, trigger: 'change' }]" v-if="properties.hasSign">
                    <Sign v-model:value="approvalForm.signImg" @change="onSignChange" @useSignNextChange="onUseSignNextChange" />
                  </a-form-item>
                  <a-form-item label="附件" name="fileList" v-if="properties.hasFile">
                    <jnpf-upload-file v-model:value="approvalForm.fileList" type="workFlow" :limit="3" />
                  </a-form-item>
                  <a-form-item v-for="item in approvalForm.approvalField" :label="item.fieldName">
                    <jnpf-input placeholder="请输入" v-model:value="item.value" v-if="item.jnpfKey == 'input'" />
                    <jnpf-textarea placeholder="请输入" v-model:value="item.value" v-if="item.jnpfKey == 'textarea'" />
                    <jnpf-input-number placeholder="请输入" v-model:value="item.value" v-if="item.jnpfKey == 'inputNumber'" />
                  </a-form-item>
                </a-form>
                <Tooltip title="收起" placement="bottom">
                  <i class="icon-ym icon-ym-report-icon-arrow-down arrow-icon" @click="handleShowApprovalForm" />
                </Tooltip>
              </div>
            </Drawer>
          </div>
          <div class="flow-btn-container" v-if="leftBtnList.length || rightBtnList.length">
            <template v-if="config.opType != 2">
              <div class="flow-btn">
                <a-button
                  :type="item.type"
                  @click="handleBtnClick(item.id)"
                  :loading="btnLoading && eventType == item.id"
                  :disabled="loading || allBtnDisabled || (btnLoading && eventType !== item.id)"
                  v-for="item in leftBtnList">
                  {{ item.fullName }}
                </a-button>
              </div>
              <div class="flow-btn">
                <a-button
                  :type="item.type"
                  @click="handleBtnClick(item.id)"
                  :loading="btnLoading && eventType == item.id"
                  :disabled="loading || allBtnDisabled || (btnLoading && eventType !== item.id)"
                  v-for="item in rightBtnList">
                  {{ item.fullName }}
                </a-button>
              </div>
            </template>
            <template v-else>
              <div class="flow-btn"></div>
              <div class="flow-btn">
                <a-button
                  :type="item.type"
                  @click="handleBtnClick(item.id)"
                  :loading="btnLoading && eventType == item.id"
                  :disabled="loading || allBtnDisabled || (btnLoading && eventType !== item.id)"
                  v-for="item in todoBtnList">
                  {{ item.fullName }}
                </a-button>
              </div>
            </template>
          </div>
        </div>
      </div>
      <ExtraPanel v-bind="getBindExtraPanelValue" v-if="getShowExtraPanel" />
    </div>
    <FlowFile ref="flowFileRef" @fileEnd="handleFileEnd" />
  </BasicPopup>
  <ErrorModal @register="registerError" @confirm="errorReceiver" @close="onErrorModalClose" />
  <CandidateModal @register="registerCandidate" @confirm="candidateReceiver" />
  <ActionModal @register="registerAction" @confirm="actionReceiver" />
  <ApprovalModal @register="registerApproval" @confirm="approvalReceiver" />
  <NodeFormParser @register="registerNodeFormParser" />
  <PrintSelect @register="registerPrintSelect" @change="handleShowBrowse" />
  <PrintBrowse @register="registerPrintBrowse" />
  <ReduceApproverModal @register="registerReduceApprover" />
  <SubFlowParser :defaultFullscreen="isFullscreen" @register="registerSubFlowParser" />
  <StartFormParser :defaultFullscreen="isFullscreen" @register="registerStartFormParser" />
  <FlowChart :defaultFullscreen="isFullscreen" @register="registerFlowChart" />
  <FlowParser :defaultFullscreen="isFullscreen" :showFullscreen="false" @register="registerFlowParser" v-if="showFlowParser" />
  <UserListModal @register="registerUserList" @confirm="selectUser" />
</template>
<script lang="ts" setup>
  import { computed, reactive, toRefs, defineAsyncComponent, markRaw, ref, nextTick } from 'vue';
  import { BasicPopup, usePopupInner, usePopup } from '@/components/Popup';
  import { useModal } from '@/components/Modal';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useI18n } from '@/hooks/web/useI18n';
  import { importViewsFile } from '@/utils';
  import {
    getFlowTaskInfo,
    create,
    update,
    getCandidates,
    sign,
    cancelSign,
    handle,
    transfer,
    assist,
    saveAudit,
    saveAssist,
    getBackNodeCodeList,
    audit,
    back,
    freeApprover,
    revoke,
    press,
    launchRecall,
    auditRecall,
  } from '@/api/workFlow/task';
  import { getDelegateUser } from '@/api/workFlow/flowDelegate';
  import { assign, cancel, resurgence, pause, getPauseType, reboot } from '@/api/workFlow/flowMonitor';
  import { formatToDateTime, formatToDate } from '@/utils/dateUtil';
  import { useGlobSetting } from '@/hooks/setting';
  import { useDefineSetting } from '@/hooks/setting/useDefineSetting';
  import { Tooltip, Drawer } from 'ant-design-vue';
  import { useUserStore } from '@/store/modules/user';
  import OpinionTextarea from './components/OpinionTextarea.vue';
  import Sign from './components/Sign.vue';
  import FlowFile from './components/FlowFile.vue';
  import FlowProcessMain from '@/components/FlowProcess/src/Main.vue';
  import ErrorModal from './modal/ErrorModal.vue';
  import CandidateModal from './modal/CandidateModal.vue';
  import ActionModal from './modal/ActionModal.vue';
  import ApprovalModal from './modal/ApprovalModal.vue';
  import ReduceApproverModal from './modal/ReduceApproverModal.vue';
  import UserListModal from './modal/UserListModal.vue';
  import RecordList from './RecordList.vue';
  import NodeFormParser from './NodeFormParser.vue';
  import SubFlowParser from './SubFlowParser.vue';
  import StartFormParser from './StartFormParser.vue';
  import FlowChart from './FlowChart.vue';
  import PrintSelect from '@/components/PrintDesign/printSelect/index.vue';
  import PrintBrowse from '@/components/PrintDesign/printBrowse/index.vue';
  import ExtraPanel from './ExtraPanel.vue';
  import FlowParser from '@/views/workFlow/components/FlowParser.vue';
  import { useAppStore } from '@/store/modules/app';
  import { getScriptFunc, onlineUtils } from '@/utils/jnpf';

  interface State {
    config: any;
    properties: any;
    loading: boolean;
    btnLoading: boolean;
    allBtnDisabled: boolean;
    flowUrgent: any;
    leftBtnList: any[];
    rightBtnList: any[];
    monitorBtnList: any[];
    todoBtnList: any[];
    activeKey: any;
    taskInfo: any;
    formInfo: any;
    flowInfo: any;
    btnInfo: any;
    nodeList: any;
    recordList: any[];
    recordTimeList: any[];
    assignNodeList: any;
    thisStep: string;
    endTime: any;
    currentView: any;
    formData: any;
    eventType: string;
    approvalData: any;
    resurgenceData: any;
    flowKey: number;
    formKey: number;
    hasComment: boolean;
    showDataLog: boolean;
    isFullscreen: boolean;
    approvalForm: any;
    showApprovalForm: boolean;
    isFlow: number;
    isApprovalFile: boolean;
    showFlowParser: boolean;
    approverText: {
      approverLabel: string;
      approver: string;
      approverTime: string;
    };
    lineKeyList: any[] | undefined;
  }

  defineOptions({ name: 'flow-parser' });
  const props = defineProps({
    defaultFullscreen: { type: Boolean, default: false },
    showFullscreen: { type: Boolean, default: true },
  });
  const emit = defineEmits(['register', 'reload', 'close']);
  const appStore = useAppStore();
  const { t } = useI18n();
  const { createMessage, createConfirm } = useMessage();
  const [registerPopup, { closePopup, setPopupProps }] = usePopupInner(init);
  const [registerError, { openModal: openErrorModal, closeModal: closeErrorModal, setModalProps: setErrorModalProps }] = useModal();
  const [registerCandidate, { openModal: openCandidateModal, closeModal: closeCandidateModal, setModalProps: setCandidateModalProps }] = useModal();
  const [registerAction, { openModal: openActionModal, closeModal: closeActionModal, setModalProps: setActionModalProps }] = useModal();
  const [registerApproval, { openModal: openApprovalModal, closeModal: closeApprovalModal, setModalProps: setApprovalModalProps }] = useModal();
  const [registerNodeFormParser, { openPopup: openNodeFormParserPopup }] = usePopup();
  const [registerSubFlowParser, { openPopup: openSubFlowParserPopup }] = usePopup();
  const [registerStartFormParser, { openPopup: openStartFormParserPopup }] = usePopup();
  const [registerFlowChart, { openPopup: openFlowChartPopup }] = usePopup();
  const [registerPrintSelect, { openModal: openPrintSelect }] = useModal();
  const [registerPrintBrowse, { openModal: openPrintBrowse }] = useModal();
  const [registerReduceApprover, { openModal: openReduceApprover }] = useModal();
  const [registerFlowParser, { openPopup: openFlowParser }] = usePopup();
  const [registerUserList, { openModal: openUserListModal }] = useModal();
  const { getFlowStatusContent, getFlowStatusColor } = useDefineSetting();
  const flowUrgentList = [
    { id: 1, fullName: '普通' },
    { id: 2, fullName: '重要' },
    { id: 3, fullName: '紧急' },
  ];
  const formRef = ref<any>(null);
  const formElRef = ref<any>(null);
  const flowFileRef = ref<any>(null);
  const globSetting = useGlobSetting();
  const apiUrl = ref(globSetting.apiUrl);
  const userStore = useUserStore();
  const userInfo = userStore.getUserInfo;
  const state = reactive<State>({
    config: {},
    properties: {},
    loading: false,
    btnLoading: false,
    allBtnDisabled: false,
    flowUrgent: 1,
    leftBtnList: [],
    rightBtnList: [],
    monitorBtnList: [],
    todoBtnList: [],
    activeKey: ' 1',
    taskInfo: {},
    formInfo: {},
    flowInfo: {},
    btnInfo: {},
    nodeList: [],
    recordList: [],
    recordTimeList: [],
    assignNodeList: [],
    thisStep: '',
    endTime: 0,
    currentView: null,
    formData: {},
    eventType: '',
    approvalData: {},
    resurgenceData: {},
    flowKey: 0,
    formKey: 0,
    hasComment: false,
    showDataLog: false,
    isFullscreen: false,
    approvalForm: {
      handleOpinion: '',
      fileList: [],
      signImg: '',
      useSignNext: false,
    },
    showApprovalForm: false,
    isFlow: 0,
    isApprovalFile: false,
    showFlowParser: false,
    approverText: {
      approverLabel: '审批意见',
      approver: '审批人',
      approverTime: '审批时间',
    },
    lineKeyList: undefined,
  });
  const {
    config,
    loading,
    btnLoading,
    allBtnDisabled,
    flowKey,
    formKey,
    taskInfo,
    monitorBtnList,
    leftBtnList,
    rightBtnList,
    todoBtnList,
    eventType,
    isFullscreen,
    approvalForm,
    showApprovalForm,
    properties,
    btnInfo,
    showFlowParser,
  } = toRefs(state);

  const getTitle = computed(() => {
    if (state.flowInfo?.type === 2) return state.flowInfo.fullName || '';
    const fullName = state.config.fullName || '';
    if ([4, 5, 6].includes(state.config.opType)) return fullName;
    return state.thisStep ? fullName + '/' + state.thisStep : fullName;
  });
  const getUrgentName = computed(() => {
    const list = flowUrgentList.filter(o => o.id === state.flowUrgent);
    if (!list.length) return '普通';
    return list[0].fullName || '普通';
  });
  const getFlowStatusClass = computed(() => {
    const status = state.taskInfo.status;
    if (status == 1) return 'doing'; //进行中
    if (status == 2) return 'adopt'; //已通过
    if (status == 3) return 'reject'; //已拒绝
    if (status == 4) return 'cancel'; //已终止
    if (status == 5) return 'pause'; //已暂停
    if (status == 6) return 'revoking'; //撤销中
    if (status == 7) return 'revoke'; //已撤销
    if (status == 8) return 'back'; //已退回
    if (status == 9) return 'recall'; //已撤回
    return '';
  });
  const getFlowStatusIcon = computed(() => state.config.opType == 6 && state.activeKey === '2' && state.flowInfo?.type !== 2);
  const getShowExtraPanel = computed(() => state.config.opType != '-1' && (state.hasComment || state.config.opType != '6') && !state.loading);
  const getBindExtraPanelValue = computed(() => ({
    taskId: state.config.id,
    recordTimeList: state.recordTimeList,
    showComment: state.hasComment,
    showRecord: state.config.opType != '6',
    showDataLog: state.showDataLog,
    modelId: state.formInfo?.id,
    formDataId: state.config?.formData?.id,
    auxiliaryInfo: state.properties.auxiliaryInfo || [],
    formData: state.config?.formData,
  }));
  const getFullscreenTitle = computed(() => (state.isFullscreen ? t('layout.header.tooltipExitFull') : t('layout.header.tooltipEntryFull')));

  function init(data) {
    state.hasComment = false;
    state.showDataLog = false;
    state.isApprovalFile = false;
    state.showApprovalForm = false;
    state.approvalForm = {
      handleOpinion: '',
      fileList: [],
      signImg: '',
      useSignNext: false,
    };
    state.isFullscreen = props.defaultFullscreen || false;
    state.loading = true;
    state.btnLoading = false;
    state.config = data;
    state.config.disabled = data.opType != 3 && data.opType != '-1';
    state.flowUrgent = 1;
    state.activeKey = data.opType == 6 ? '2' : '1';
    state.isFlow = data.isFlow || 0;
    /**
     * opType
     * -1 - 我发起的新建/编辑
     * 0 - 我发起的详情
     * 1 - 待签事宜
     * 2 - 待办事宜
     * 3 - 在办事宜
     * 4 - 已办事宜
     * 5 - 抄送事宜
     * 6 - 流程监控
     */
    getBeforeInfo(state.config);
  }
  function getBeforeInfo(config) {
    let query: any = {
      taskNodeId: config.taskNodeId,
      operatorId: config.operatorId,
      flowId: config.flowId, // 流程大Id
      opType: config.opType == 2 ? 3 : config.opType, //特殊：待办点击开始办理需要跳到在办页面
    };
    if (config.type) query.type = config.type;
    getFlowTaskInfo(config.id || '0', query)
      .then(res => {
        state.formInfo = res.data.formInfo || {};
        state.taskInfo = res.data.taskInfo || {};
        state.flowInfo = res.data.flowInfo || {};
        state.btnInfo = res.data.btnInfo || {};
        state.lineKeyList = res.data.lineKeyList;
        const fullName = config.opType == '-1' ? state.flowInfo.fullName : state.taskInfo.fullName;
        config.fullName = fullName;
        state.thisStep = state.taskInfo.currentNodeName || '';
        state.flowUrgent = state.taskInfo.flowUrgent || 1;
        config.status = state.taskInfo.status;
        config.type = state.flowInfo.type;
        config.draftData = res.data.draftData || null;
        config.formData = res.data.formData || {};
        config.formEnCode = state.formInfo.enCode;
        state.nodeList = res.data.nodeList || [];
        state.properties = res.data.nodeProperties || {};
        state.approverText = {
          approverLabel: res.data.nodeProperties?.type === 'processing' ? '办理意见' : '审批意见',
          approver: res.data.nodeProperties?.type === 'processing' ? '办理人' : '审批人',
          approverTime: res.data.nodeProperties?.type === 'processing' ? '办理时间' : '审批时间',
        };
        state.recordList = (res.data.recordList || []).reverse();
        state.recordTimeList = (res.data.progressList || []).reverse();
        // 设置默认审批data
        state.approvalData = {
          fileList: [],
          handleOpinion: '',
          signImg: '',
          copyIds: '',
          backNodeCode: '',
          backType: state.properties.backType || 1,
        };
        config.formConf = state.formInfo.formData;
        config.formOperates = res.data.formOperates || [];
        state.hasComment = state.flowInfo?.flowNodes?.global?.hasComment || false;
        state.showDataLog = config.formConf && state.formInfo.type === 1 ? JSON.parse(config.formConf)?.dataLog : false;
        if (config.opType == 0) {
          for (let i = 0; i < config.formOperates.length; i++) {
            config.formOperates[i].write = false;
          }
        }
        state.assignNodeList = state.nodeList.filter(o => config.opType == 6 && o.type == 1 && (o.nodeType === 'approver' || o.nodeType === 'processing'));
        initBtnList();
        initApprovalField();
        if (config.opType == 6) {
          state.activeKey = '2';
          setPageLoad();
          state.flowKey = +new Date();
          return;
        }
        const formUrl =
          state.formInfo.type == 1
            ? 'workFlow/workFlowForm/dynamicForm'
            : state.formInfo.urlAddress
            ? state.formInfo.urlAddress.replace(/\s*/g, '')
            : `workFlow/workFlowForm/${state.formInfo.enCode}`;
        state.currentView = markRaw(defineAsyncComponent(() => importViewsFile(formUrl)));
        state.flowKey = +new Date();
      })
      .catch(() => {
        state.loading = false;
      });
  }
  function initBtnList() {
    const properties = state.properties;
    const btnInfo = state.btnInfo;
    state.monitorBtnList = [];
    state.leftBtnList = [];
    state.rightBtnList = [];
    state.todoBtnList = [];
    // 流程监控
    if (state.config.opType == 6) {
      if (btnInfo?.hasCancelBtn) state.monitorBtnList.push({ id: 'cancel', fullName: '终止', type: 'error' });
      if (btnInfo?.hasActivateBtn) state.monitorBtnList.push({ id: 'resurgence', fullName: '复活', type: 'primary' });
      if (btnInfo?.hasPauseBtn) state.monitorBtnList.push({ id: 'pause', fullName: '暂停', type: 'info' });
      if (btnInfo?.hasRebootBtn) state.monitorBtnList.push({ id: 'reboot', fullName: '恢复' });
      if (btnInfo?.hasAssignBtn) state.monitorBtnList.push({ id: 'assign', fullName: '指派', type: 'warning' });
      if (btnInfo?.hasFileBtn) state.monitorBtnList.push({ id: 'file', fullName: '归档', type: 'primary' });
    } else {
      // 右侧按钮
      // 提交
      if (btnInfo?.hasSaveBtn && !state.config.hideSaveBtn) state.rightBtnList.push({ id: 'save', fullName: properties.saveBtnText || '暂存', type: '' });
      if (btnInfo?.hasSubmitBtn) state.rightBtnList.push({ id: 'submit', fullName: properties.submitBtnText || '提交', type: 'primary' });
      if (btnInfo?.hasDelegateSubmitBtn) state.rightBtnList.push({ id: 'delegateSubmit', fullName: '委托发起', type: 'primary' });
      // 待签
      if (btnInfo?.hasSignBtn) state.rightBtnList.push({ id: 'sign', fullName: btnInfo?.proxyMark ? '代签' : '签收', type: 'primary' });
      //特殊：待办点击开始办理需要跳到在办页面
      if (state.config.opType == 2) {
        // 待办
        const hasCancelSignBtn = appStore?.getSysConfigInfo?.flowSign && state.flowInfo?.flowNodes?.global?.hasSignFor;
        if (hasCancelSignBtn) state.todoBtnList.push({ id: 'cancelSign', fullName: '退签', type: 'warning' });
        state.todoBtnList.push({ id: 'startHandle', fullName: btnInfo?.proxyMark ? '代办' : '开始办理', type: 'primary' });
      }
      // 在办
      if (btnInfo?.hasRecallLaunchBtn) state.rightBtnList.push({ id: 'launchRecall', fullName: '撤回', type: 'warning' });
      if (btnInfo?.hasRecallAuditBtn) state.rightBtnList.push({ id: 'auditRecall', fullName: '撤回', type: 'warning' });
      if (btnInfo?.hasAssistSaveBtn) state.rightBtnList.push({ id: 'saveAssist', fullName: '保存' });
      if (btnInfo?.hasRejectBtn && state.properties.type != 'processing')
        state.rightBtnList.push({ id: 'reject', fullName: properties.rejectBtnText || '拒绝', type: 'error' });
      if (btnInfo?.hasAuditBtn)
        state.rightBtnList.push({
          id: 'audit',
          fullName: properties.auditBtnText || (state.properties.type === 'processing' ? '确认办理' : '同意'),
          type: 'primary',
        });
      if (btnInfo?.hasReduceApproverBtn)
        state.rightBtnList.push({ id: 'reduceApprover', fullName: properties.reduceApproverBtnText || '减签', type: 'primary' });
      // 左侧按钮
      if (btnInfo?.hasRevokeBtn) state.leftBtnList.push({ id: 'revoke', fullName: '撤销' });
      if (btnInfo?.hasPressBtn) state.leftBtnList.push({ id: 'press', fullName: properties.pressBtnText || '催办' });
      if (btnInfo?.hasAssistBtn) state.leftBtnList.push({ id: 'assist', fullName: properties.assistBtnText || '协办' });
      if (btnInfo?.hasTransferBtn) state.leftBtnList.push({ id: 'transfer', fullName: properties.transferBtnText || '转审' });
      if (btnInfo?.hasFreeApproverBtn) state.leftBtnList.push({ id: 'freeApprover', fullName: properties.freeApproverBtnText || '加签' });
      if (btnInfo?.hasBackBtn) state.leftBtnList.push({ id: 'back', fullName: properties.backBtnText || '退回' });
      if (btnInfo?.hasSaveAuditBtn) state.leftBtnList.push({ id: 'saveAudit', fullName: '暂存' });
      // 流程打印
      if (btnInfo?.hasPrintBtn) state.leftBtnList.push({ id: 'print', fullName: '打印' });
      // 自定义按钮
      if (properties?.customBtns?.length) {
        properties?.customBtns.map((item: any) => {
          state.leftBtnList.push({ id: item.value, fullName: item.label, jsJson: item.jsJson });
        });
      }
    }
  }
  function eventLauncher(eventType) {
    formRef.value?.dataFormSubmit(eventType, state.flowUrgent);
  }
  function eventReceiver(formData, eventType) {
    state.formData = formData;
    state.formData.flowId = state.flowInfo.flowId;
    state.formData.id = state.config.id;
    state.eventType = eventType;
    // 发起暂存/提交
    if (eventType === 'save' || eventType === 'submit') return submitOrSave();
    // 委托发起
    if (eventType === 'delegateSubmit') return handleDelegateSubmit();
    // 审批暂存/协办保存
    if (eventType === 'saveAudit' || eventType === 'saveAssist') return handleSaveAudit();
    // 同意/拒绝
    if (eventType === 'audit' || eventType === 'reject') return handleAuditOrReject();
    // 加签
    if (eventType === 'freeApprover') return handleFreeApprover();
  }
  // 提交或者暂存
  function submitOrSave() {
    state.formData.status = state.eventType === 'save' ? 0 : 1;
    state.formData.flowUrgent = state.flowUrgent;
    //受委托人不为空的时候走委托创建流程
    delete state.formData.delegateUser;
    if (state.eventType === 'delegateSubmit') state.formData.delegateUser = state.config.delegateUser;
    if (state.eventType === 'save') return handleRequest();
    state.btnLoading = true;
    getCandidates('0', state.formData)
      .then(res => {
        state.btnLoading = false;
        const candidateList = res.data.list.filter(o => !o.isBranchFlow && o.isCandidates);
        const branchList = res.data.list.filter(o => o.isBranchFlow);
        if (!candidateList.length && res.data.type == 3) {
          createConfirm({
            iconType: 'warning',
            title: '提示',
            content: '您确定要提交当前流程吗, 是否继续?',
            onOk: () => {
              handleRequest();
            },
          });
          return;
        }
        openCandidateModal(true, {
          branchList,
          candidateList,
          isCustomCopy: state.properties.isCustomCopy,
          formData: state.formData,
          eventType: state.eventType,
        });
      })
      .catch(() => {
        state.btnLoading = false;
      });
  }
  // 提交或者暂存请求接口
  function handleRequest(candidateData: any = null) {
    if (candidateData) state.formData = { ...state.formData, ...candidateData };
    if (!state.formData.id) delete state.formData.id;
    state.formData.isFlow = state.isFlow;
    state.allBtnDisabled = true;
    state.btnLoading = true;
    const formMethod = state.formData.id ? update : create;
    formMethod(state.formData)
      .then(res => {
        handleError(res);
      })
      .catch(() => {
        setCandidateModalProps({ confirmLoading: false });
        setErrorModalProps({ confirmLoading: false });
        state.allBtnDisabled = false;
        state.btnLoading = false;
      });
  }
  // 异常处理
  function handleError(res) {
    const errorData = res.data?.errorCodeList;
    if (errorData && Array.isArray(errorData) && errorData.length) {
      state.allBtnDisabled = false;
      state.btnLoading = false;
      openErrorModal(true, { errorData });
    } else {
      createMessage.success(res.msg).then(() => {
        closeErrorModal();
        closeCandidateModal();
        closeApprovalModal();
        closeActionModal();
        if (res.data?.isEnd) {
          state.isApprovalFile = true;
          handleFile(res.data.taskId);
          return;
        }
        state.allBtnDisabled = false;
        state.btnLoading = false;
        handleClose(true);
      });
    }
  }
  // 异常处理弹窗提交
  function errorReceiver(data) {
    state.formData.errorRuleUserList = data;
    if (state.eventType === 'submit') return handleRequest();
    if (state.eventType === 'audit' || state.eventType === 'reject') return handleApproval();
  }
  // 关闭异常处理弹窗
  function onErrorModalClose() {
    setCandidateModalProps({ confirmLoading: false });
    setApprovalModalProps({ confirmLoading: false });
    setActionModalProps({ confirmLoading: false });
    setErrorModalProps({ confirmLoading: false });
    state.allBtnDisabled = false;
    state.btnLoading = false;
  }
  // 审批通过/审批退回/加签
  function handleApproval() {
    const query = {
      ...state.approvalData,
      ...state.formData,
    };
    if (state.eventType === 'audit') query.handleStatus = 1;
    if (state.eventType === 'reject') query.handleStatus = 0;
    const approvalMethod = state.eventType === 'freeApprover' ? freeApprover : state.eventType === 'back' ? back : audit;
    state.allBtnDisabled = true;
    state.btnLoading = true;
    approvalMethod(state.config.operatorId, query)
      .then(res => {
        handleError(res);
        if (query.useSignNext) userStore.setUserInfo({ signImg: query.signImg, signId: query.signId });
      })
      .catch(() => {
        onErrorModalClose();
      });
  }
  // 审批通过/审批退回/加签弹窗提交
  function approvalReceiver(data) {
    state.approvalData = data;
    handleApproval();
  }
  // 审批暂存
  function handleSaveAudit() {
    state.allBtnDisabled = true;
    state.btnLoading = true;
    const save = state.eventType == 'saveAssist' ? saveAssist : saveAudit;
    save(state.config.operatorId, state.formData)
      .then(res => {
        createMessage.success(res.msg).then(() => {
          state.allBtnDisabled = false;
          state.btnLoading = false;
          handleClose(true);
        });
      })
      .catch(() => {
        state.allBtnDisabled = false;
        state.btnLoading = false;
      });
  }
  // 流程催办
  function handlePress() {
    createConfirm({
      iconType: 'warning',
      title: '提示',
      content: '此操作将提示该节点尽快处理，是否继续?',
      onOk: () => {
        press(state.config.id).then(res => {
          createMessage.success(res.msg);
        });
      },
    });
  }
  // 撤回、转审、终止、驳回等操作弹窗
  function actionLauncher(eventType) {
    state.eventType = eventType;
    const list = ['sign', 'cancelSign', 'startHandle'];
    if (list.includes(eventType) || (eventType == 'cancel' && state.flowInfo?.type == 2)) {
      let content = '';
      if (eventType == 'sign') content = '确定签收，签收后进入待办。';
      if (eventType == 'cancelSign') content = '确定退签，确定后进入我的待签。';
      if (eventType == 'startHandle') content = '确定开始办理流程';
      if (eventType == 'cancel') content = '终止后流程不可恢复，是否继续？';
      createConfirm({
        iconType: 'warning',
        title: '提示',
        content,
        onOk: () => {
          actionReceiver();
        },
      });
      return;
    }
    openActionModal(true, { properties: state.properties, assignNodeList: state.assignNodeList, eventType });
  }
  // 签收、办理、撤回、转审、终止、驳回等操作请求
  function actionReceiver(data: any = null) {
    if (!data) data = { handleOpinion: '', signImg: '', fileList: [] };
    const list = ['resurgence', 'assign', 'pause', 'revoke', 'cancel', 'launchRecall'];
    const id = list.includes(state.eventType) ? state.config.id : state.config.operatorId;
    const actionMethod = getActionMethod();
    state.allBtnDisabled = true;
    state.btnLoading = true;
    actionMethod(id, data)
      .then(res => {
        state.eventType == 'startHandle' ? handleStartHandle(res) : handleError(res);
      })
      .catch(() => {
        setActionModalProps({ confirmLoading: false });
        state.allBtnDisabled = false;
        state.btnLoading = false;
      });
  }
  function getActionMethod() {
    if (state.eventType === 'sign') return sign;
    if (state.eventType === 'cancelSign') return cancelSign;
    if (state.eventType === 'startHandle') return handle;
    if (state.eventType === 'transfer') return transfer;
    if (state.eventType === 'assist') return assist;
    if (state.eventType === 'assign') return assign;
    if (state.eventType === 'revoke') return revoke;
    if (state.eventType === 'launchRecall') return launchRecall;
    if (state.eventType === 'auditRecall') return auditRecall;
    if (state.eventType === 'cancel') return cancel;
    if (state.eventType === 'pause') return pause;
    if (state.eventType === 'resurgence') return resurgence;
    return cancel;
  }
  function handleUrgentClick({ key }) {
    state.flowUrgent = key;
  }
  // 更多按钮事件
  function handleBtnClick(key) {
    state.eventType = key;
    const list = ['save', 'submit', 'delegateSubmit', 'audit', 'reject', 'saveAudit', 'saveAssist', 'freeApprover']; // 发起暂存,委托发起,流程提交,同意,拒绝,审批暂存,协办暂存,
    if (list.includes(key)) return eventLauncher(key);
    if (key == 'reduceApprover') return handleReduceApprover(); //减签
    if (key == 'back') return handleBack(); //退回
    if (key == 'press') return handlePress(); //催办
    if (key == 'resurgence') return handleOpenResurgenceModal(key); //流程监控复活
    if (key == 'pause') return handleOpenPauseModal(key); //流程监控暂停
    if (key == 'reboot') return handleReboot(); //流程监控恢复
    if (key == 'print') return handlePrint(); //流程打印
    if (key == 'file') return handleFile(); //流程归档
    if (key.includes('btn_')) return handleCustomBtns(key);
    actionLauncher(key);
  }
  // 委托发起
  function handleDelegateSubmit() {
    getDelegateUser(state.flowInfo.id).then(res => {
      const userList = res.data.list || [];
      if (userList.length == 1) return selectUser(userList[0].id);
      if (userList.length) return openUserListModal(true, { userList });
    });
  }
  function selectUser(id) {
    state.config.delegateUser = id;
    submitOrSave();
  }
  // 审批、拒绝
  async function handleAuditOrReject() {
    try {
      const values = await formElRef.value?.validate();
      if (!values) return (state.showApprovalForm = true);
      state.allBtnDisabled = true;
      state.btnLoading = true;
      state.formData.handleStatus = state.eventType === 'audit' ? 1 : 0;
      getCandidates(state.config.operatorId, state.formData)
        .then(res => {
          state.allBtnDisabled = false;
          state.btnLoading = false;
          const candidateList = res.data.list.filter(o => !o.isBranchFlow && o.isCandidates);
          const branchList = res.data.list.filter(o => o.isBranchFlow);
          if (!candidateList.length && !state.properties.isCustomCopy && res.data.type == 3) {
            createConfirm({
              iconType: 'warning',
              title: '提示',
              content:
                state.properties.type === 'processing'
                  ? '是否确认办理该审批单？'
                  : `此操作将${state.eventType === 'audit' ? '同意' : '拒绝'}该审批单，是否继续?`,
              onOk: () => {
                state.approvalData = state.approvalForm;
                handleApproval();
              },
            });
            return;
          }
          openCandidateModal(true, {
            branchList,
            candidateList,
            isCustomCopy: state.properties.isCustomCopy,
            formData: state.formData,
            eventType: state.eventType,
            operatorId: state.config.operatorId,
          });
        })
        .catch(() => {
          state.allBtnDisabled = false;
          state.btnLoading = false;
        });
    } catch (_) {
      state.showApprovalForm = true;
    }
  }
  // 加签
  function handleFreeApprover() {
    state.btnLoading = true;
    getCandidates(state.config.operatorId, state.formData)
      .then(res => {
        state.btnLoading = false;
        const candidateList = res.data.list.filter(o => !o.isBranchFlow && o.isCandidates);
        const branchList = res.data.list.filter(o => o.isBranchFlow);
        openApprovalModal(true, {
          branchList,
          candidateList,
          backNodeCodeList: [],
          operatorId: state.config.operatorId,
          formData: state.formData,
          eventType: state.eventType,
          properties: state.properties,
        });
      })
      .catch(() => {
        state.btnLoading = false;
      });
  }
  // 退回
  function handleBack() {
    state.btnLoading = true;
    getBackNodeCodeList(state.config.operatorId)
      .then(res => {
        state.btnLoading = false;
        const data = {
          backNodeCodeList: res.data.list || [],
          branchList: [],
          candidateList: [],
          operatorId: state.config.operatorId,
          formData: state.formData,
          eventType: state.eventType,
          properties: state.properties,
        };
        openApprovalModal(true, data);
      })
      .catch(() => {
        state.btnLoading = false;
      });
  }
  // 流程复活
  function handleOpenResurgenceModal(key) {
    openActionModal(true, { properties: state.properties, eventType: key });
  }
  // 流程暂停
  function handleOpenPauseModal(key) {
    getPauseType(state.config.id).then(res => {
      openActionModal(true, { properties: state.properties, eventType: key, pauseType: res.data || false });
    });
  }
  // 流程恢复
  function handleReboot() {
    state.allBtnDisabled = true;
    state.btnLoading = true;
    reboot(state.config.id).then(res => {
      createMessage.success(res.msg).then(() => {
        state.allBtnDisabled = false;
        state.btnLoading = false;
        handleClose(true);
      });
    });
  }
  // 流程打印
  function handlePrint() {
    if (!state.properties.printConfig.printIds?.length) return createMessage.error('未配置打印模板');
    if (state.properties.printConfig.printIds?.length === 1) return handleShowBrowse(state.properties.printConfig.printIds[0]);
    openPrintSelect(true, state.properties.printConfig.printIds);
  }
  function handleShowBrowse(id) {
    openPrintBrowse(true, { id, formInfo: [{ formId: state.config.formData.id, flowTaskId: state.config.id }] });
  }
  // 流程监控查看各节点表单详情
  function viewNodeDetail(data) {
    openNodeFormParserPopup(true, { ...data, flowId: state.flowInfo.flowId });
  }
  // 查看子流程
  function viewSubFlow(nodeCode) {
    const data = {
      opType: state.config.opType,
      nodeCode: nodeCode,
      taskId: state.config.id,
    };
    openSubFlowParserPopup(true, data);
  }
  function viewStartForm() {
    openStartFormParserPopup(true, { taskId: state.config.id, id: state.config.id, title: state.config.fullName });
  }
  function setPageLoad(val: any = false) {
    state.loading = !!val;
  }
  function handleClose(reload = false) {
    emit('close');
    state.currentView = null;
    closePopup();
    if (reload) emit('reload');
  }
  function onClose() {
    emit('close');
    state.currentView = null;
    return true;
  }
  // 显示减签弹窗
  function handleReduceApprover() {
    openReduceApprover(true, { id: state.config.operatorId });
  }
  function handleShowApprovalForm() {
    state.showApprovalForm = !state.showApprovalForm;
  }
  // 打开流程图
  function handleOpenFlowInfo() {
    openFlowChartPopup(true, {
      flowInfo: state.flowInfo,
      nodeList: state.nodeList,
      opType: state.config.opType,
      taskId: state.config.id,
      isRevokeTask: state.taskInfo.isRevokeTask,
      lineKeyList: state.lineKeyList,
    });
  }
  // 流程归档
  function handleFile(task?) {
    state.allBtnDisabled = true;
    state.btnLoading = true;
    nextTick(() => {
      flowFileRef.value?.init({
        id: state.flowInfo?.flowNodes?.global?.fileConfig?.templateId,
        formInfo: [{ formId: state.config.formData.id, flowTaskId: task || state.config.id }],
        opType: state.config.opType,
      });
    });
  }
  // 自定义按钮
  function handleCustomBtns(key) {
    state.leftBtnList.length &&
      state.leftBtnList.map(item => {
        if (item.id === key) {
          const parameter = { flowInfo: state.flowInfo, formData: state.formData, taskInfo: state.taskInfo, onlineUtils };
          const func: any = getScriptFunc(item.jsJson);
          func && !func(parameter);
        }
      });
    return null;
  }
  function handleFileEnd() {
    state.allBtnDisabled = false;
    state.btnLoading = false;
    if (state.config.opType == '6') state.monitorBtnList = state.monitorBtnList.filter(o => o.id != 'file');
    if (state.isApprovalFile) {
      state.allBtnDisabled = false;
      state.btnLoading = false;
      handleClose(true);
    }
  }
  // 切换全屏
  function handleToggleFullscreen() {
    state.isFullscreen = !state.isFullscreen;
    setPopupProps({ defaultFullscreen: state.isFullscreen });
  }
  function onUseSignNextChange(value) {
    state.approvalForm.useSignNext = value;
  }
  function onSignChange(_value, signId) {
    state.approvalForm.signId = signId;
  }
  function candidateReceiver(data) {
    if (state.eventType == 'audit' || state.eventType == 'reject') {
      state.approvalData = { ...state.approvalForm, ...data };
      handleApproval();
      return;
    }
    handleRequest(data);
  }
  function handleStartHandle(res) {
    createMessage.success(res.msg).then(() => {
      state.allBtnDisabled = false;
      state.btnLoading = false;
      emit('reload', false);
      state.config.opType = 3;
      state.config.disabled = false;
      initApprovalField();
      state.formKey = +new Date();
    });
  }
  function initApprovalField() {
    state.approvalForm.approvalField = [];
    if (state.config.opType == 3 && state.properties.hasApprovalField) {
      const approvalFieldList = state.flowInfo?.flowNodes?.global?.approvalFieldList || [];
      state.properties.approvalField.map(o => {
        const list = approvalFieldList.filter(item => item.id == o);
        list?.length && state.approvalForm.approvalField.push({ ...list[0], value: null });
      });
    }
  }
  function openRevokeFlow(id) {
    state.showFlowParser = true;
    nextTick(() => {
      openFlowParser(true, { id, opType: 5, showFullscreen: false, isFullscreen: state.isFullscreen });
    });
  }
</script>
<style lang="less">
  @import url('./style/index.less');
</style>
