<template>
  <section class="common-pane">
    <template v-if="element">
      <StartNode ref="startRef" v-bind="getBindValue" v-if="isStartNode()" @updateGlobalFormId="updateGlobalFormId" @updateDivideRule="updateDivideRule" />
      <ApproverNode ref="approverRef" v-bind="getBindValue" v-else-if="isApproverNode()" @updateDivideRule="updateDivideRule" />
      <ProcessingNode ref="processingRef" v-bind="getBindValue" v-else-if="isProcessingNode()" @updateDivideRule="updateDivideRule" />
      <SubFlowNode ref="subFlowRef" v-bind="getBindValue" v-else-if="isSubFlowNode()" @updateDivideRule="updateDivideRule" />
      <ConnectNode v-bind="getBindValue" v-else-if="isConnectNode()" />
      <TriggerNode ref="triggerRef" v-bind="getBindValue" v-else-if="isTriggerNode() || isEventTriggerNode()" />
      <TimeTriggerNode v-bind="getBindValue" v-else-if="isTimeTriggerNode()" />
      <NoticeTriggerNode v-bind="getBindValue" v-else-if="isNoticeTriggerNode()" />
      <WebhookTriggerNode v-bind="getBindValue" v-else-if="isWebhookTriggerNode()" />
      <GetDataNode ref="getDataRef" v-bind="getBindValue" v-else-if="isGetDataNode()" />
      <AddDataNode ref="addDataRef" v-bind="getBindValue" v-else-if="isAddDataNode()" />
      <UpdateDataNode ref="updateDataRef" v-bind="getBindValue" v-else-if="isUpdateDataNode()" />
      <DeleteDataNode ref="deleteDataRef" v-bind="getBindValue" v-else-if="isDeleteDataNode()" />
      <DataInterfaceNode v-bind="getBindValue" v-else-if="isDataInterfaceNode()" />
      <MessageNode v-bind="getBindValue" v-else-if="isMessageNode()" />
      <LaunchFlowNode ref="launchDataRef" v-bind="getBindValue" v-else-if="isLaunchFlowNode()" />
      <ScheduleNode v-bind="getBindValue" v-else-if="isScheduleNode()" />
      <EndNode v-else />
    </template>
    <GlobalProperties v-bind="getBindValue" @updateConnectNameType="updateConnectNameType" v-else />
  </section>
</template>
<script lang="ts" setup>
  import { computed, reactive, onMounted, watch, unref, toRaw, ref, nextTick } from 'vue';
  import { getConfigData } from '@/api/onlineDev/visualDev';
  import { getInfo } from '@/api/workFlow/template';
  import { cloneDeep, omit } from 'lodash-es';
  import { NodeUtils } from '../bpmn/utils/nodeUtil';
  import { getPrintDevSelector } from '@/api/system/printDev';
  import { systemFieldOptions } from '../helper/define';
  import { useDebounceFn } from '@vueuse/core';
  import { useAppStore } from '@/store/modules/app';
  import { dyOptionsList } from '@/components/FormGenerator/src/helper/config';
  import { getWebhookUrl } from '@/api/workFlow/template';
  import GlobalProperties from './GlobalProperties.vue';
  import StartNode from './StartNode.vue';
  import ApproverNode from './ApproverNode.vue';
  import ProcessingNode from './ProcessingNode.vue';
  import SubFlowNode from './SubFlowNode.vue';
  import EndNode from './EndNode.vue';
  import ConnectNode from './ConnectNode.vue';
  import TriggerNode from './TriggerNode.vue';
  import NoticeTriggerNode from './NoticeTriggerNode.vue';
  import TimeTriggerNode from './TimeTriggerNode.vue';
  import WebhookTriggerNode from './WebhookTriggerNode.vue';
  import GetDataNode from './GetDataNode.vue';
  import AddDataNode from './AddDataNode.vue';
  import UpdateDataNode from './UpdateDataNode.vue';
  import DeleteDataNode from './DeleteDataNode.vue';
  import DataInterfaceNode from './DataInterfaceNode.vue';
  import MessageNode from './MessageNode.vue';
  import LaunchFlowNode from './LaunchFlowNode.vue';
  import ScheduleNode from './ScheduleNode.vue';
  import nodeConfig from '../helper/config';
  import { hasGatewayType } from '../bpmn/config';
  import { typeCondition } from '../bpmn/config/variableName';

  interface state {
    activeKey: number;
    globalForm: any;
    startForm: any;
    approverForm: any;
    processingForm: any;
    subFlowForm: any;
    connectForm: any;
    triggerForm: any;
    eventTriggerForm: any;
    timeTriggerForm: any;
    noticeTriggerForm: any;
    webhookTriggerForm: any;
    getDataForm: any;
    addDataForm: any;
    updateDataForm: any;
    deleteDataForm: any;
    dataInterfaceForm: any;
    messageForm: any;
    launchFlowForm: any;
    scheduleForm: any;
    formFieldList: any[];
    printTplOptions: any[];
    prevNodeList: any[];
    beforeNodeOptions: any[];
    oldFormId: string;
    sourceIsStart: boolean;
    dataSourceFormList: any[];
  }
  interface ComType {
    getContent: () => any;
  }
  interface OperateNodeType extends ComType {
    updateCheckStatus: () => any;
    getSubFlowFormInfo: () => any;
    getFlowFormFieldList: () => any;
  }

  const props = defineProps(['element', 'bpmn', 'flowState', 'versionList', 'type', 'flowInfo']);
  const state = reactive<state>({
    activeKey: 1,
    globalForm: cloneDeep(props.type == 2 ? nodeConfig.defaultTaskGlobalForm : nodeConfig.defaultGlobalForm),
    startForm: cloneDeep(nodeConfig.defaultStartForm),
    approverForm: cloneDeep(nodeConfig.defaultApproverForm),
    processingForm: cloneDeep(nodeConfig.defaultProcessingForm),
    subFlowForm: cloneDeep(nodeConfig.defaultSubFlowForm),
    connectForm: cloneDeep(nodeConfig.defaultConnectForm),
    triggerForm: cloneDeep(nodeConfig.defaultTriggerForm),
    eventTriggerForm: cloneDeep(nodeConfig.defaultEventTriggerForm),
    timeTriggerForm: cloneDeep(nodeConfig.defaultTimeTriggerForm),
    noticeTriggerForm: cloneDeep(nodeConfig.defaultNoticeTriggerForm),
    webhookTriggerForm: cloneDeep(nodeConfig.defaultWebhookTriggerForm),
    getDataForm: cloneDeep(nodeConfig.defaultGetDataForm),
    addDataForm: cloneDeep(nodeConfig.defaultAddDataForm),
    updateDataForm: cloneDeep(nodeConfig.defaultUpdateDataForm),
    deleteDataForm: cloneDeep(nodeConfig.defaultDeleteDataForm),
    dataInterfaceForm: cloneDeep(nodeConfig.defaultDataInterfaceForm),
    messageForm: cloneDeep(nodeConfig.defaultMessageForm),
    launchFlowForm: cloneDeep(nodeConfig.defaultLaunchFlowForm),
    scheduleForm: cloneDeep(nodeConfig.defaultScheduleForm),
    formFieldList: [],
    printTplOptions: [],
    prevNodeList: [],
    beforeNodeOptions: [],
    oldFormId: '',
    sourceIsStart: false,
    dataSourceFormList: [],
  });
  const appStore = useAppStore();
  const updateJnpfData = useDebounceFn(handleSetJnpfData, 200);
  const startRef = ref<Nullable<OperateNodeType>>(null);
  const approverRef = ref<Nullable<OperateNodeType>>(null);
  const processingRef = ref<Nullable<OperateNodeType>>(null);
  const subFlowRef = ref<Nullable<OperateNodeType>>(null);
  const triggerRef = ref<Nullable<OperateNodeType>>(null);
  const getDataRef = ref<Nullable<OperateNodeType>>(null);
  const addDataRef = ref<Nullable<OperateNodeType>>(null);
  const updateDataRef = ref<Nullable<OperateNodeType>>(null);
  const deleteDataRef = ref<Nullable<OperateNodeType>>(null);
  const launchDataRef = ref<Nullable<OperateNodeType>>(null);
  const requiredDisabled = jnpfKey => {
    return ['billRule', 'createUser', 'createTime', 'modifyTime', 'modifyUser', 'currPosition', 'currOrganize', 'table'].includes(jnpfKey);
  };
  const getDataType = data => {
    if (!data.__config__ || !data.__config__.jnpfKey) return '';
    const jnpfKey = data.__config__.jnpfKey;
    if (['inputNumber', 'datePicker', 'rate', 'slider'].includes(jnpfKey)) {
      return 'number';
    } else if (['checkbox', 'uploadFile', 'uploadImg', 'cascader', 'organizeSelect', 'areaSelect'].includes(jnpfKey)) {
      return 'array';
    } else if (
      ['select', 'treeSelect', 'depSelect', 'posSelect', 'userSelect', 'usersSelect', 'roleSelect', 'groupSelect', 'popupSelect', 'popupTableSelect'].includes(
        jnpfKey,
      )
    ) {
      if (data.multiple) return 'array';
    }
    return '';
  };

  const getBindValue = computed(() => ({
    ...props,
    formConf: getFormConf(),
    printTplOptions: state.printTplOptions,
    prevNodeList: state.prevNodeList,
    beforeNodeOptions: state.beforeNodeOptions,
    sourceIsStart: state.sourceIsStart,
    dataSourceFormList: state.dataSourceFormList,
    formFieldsOptions: unref(formFieldsOptions),
    usedFormItems: unref(usedFormItems),
    funcOptions: unref(funcOptions),
    noticeOptions: unref(noticeOptions),
    nodeOptions: unref(getApproverNodeOptions),
    dataSourceForm: unref(getDataSourceFormList),
    getFormFieldList,
    initFormOperates,
    updateJnpfData,
    updateAllNodeFormOperates,
    transformFormJson,
    transformFieldList,
    updateTransferList,
    updateFormFieldList,
    updateBpmnProperties,
    onConnectNameChange,
  }));
  // 所有表单字段
  const formFieldsOptions = computed(() => state.formFieldList.filter(o => o.__config__.jnpfKey !== 'table').map(o => ({ ...o, disabled: false })));
  // 不包含子表的数据来源
  const getDataSourceFormList = computed(() =>
    state.dataSourceFormList.map(o => ({ ...o, children: (o.children || []).filter(o => o?.__config__?.jnpfKey !== 'table' && !o?.__config__?.isSubTable) })),
  );
  // 不包含子表的表单字段
  const usedFormItems = computed(() => unref(formFieldsOptions).filter(o => o.id.indexOf('-') < 0));
  // 流程时间及通知等用到表单字段
  const funcOptions = computed(() => unref(formFieldsOptions).map(o => ({ ...o, label: o.fullName ? o.id + '(' + o.fullName + ')' : o.id })));
  const noticeOptions = computed(() => {
    const options = [...systemFieldOptions, ...unref(formFieldsOptions)];
    return options.map(o => ({ ...o, label: o.fullName ? o.id + '(' + o.fullName + ')' : o.id }));
  });
  // 审批节点字段
  const getApproverNodeOptions = computed(() => {
    const jnpfData: any = props.bpmn?.get('jnpfData');
    if (!props.element) return [];
    const list: any = [];
    for (const key in jnpfData?.data) {
      const item = jnpfData?.data[key];
      if (NodeUtils.isApproverNode(item) && key != props.element?.id) {
        list.push({ fullName: `${item.nodeName}(${item.nodeId})`, id: item.nodeId });
      }
    }
    return list;
  });

  watch(
    () => props.element,
    () => {
      init();
    },
  );

  function init() {
    handleNull();
    isStartNode() && initStartNodeData();
    isApproverNode() && initApproverNodeData();
    isProcessingNode() && initProcessingNodeData();
    isSubFlowNode() && initSubFlowNodeData();
    isConnectNode() && initConnectNodeData();
    if (
      isTriggerNode() ||
      isEventTriggerNode() ||
      isTimeTriggerNode() ||
      isNoticeTriggerNode() ||
      isWebhookTriggerNode() ||
      isGetDataNode() ||
      isAddDataNode() ||
      isUpdateDataNode() ||
      isDeleteDataNode() ||
      isDataInterfaceNode() ||
      isMessageNode() ||
      isLaunchFlowNode() ||
      isScheduleNode()
    ) {
      initCommonTaskData();
    }
    !props.element && initGlobalNodeData();
  }
  function handleNull() {
    state.formFieldList = [];
    state.dataSourceFormList = [];
  }
  // 初始化全局属性
  function initGlobalNodeData() {
    state.globalForm = cloneDeep(props.type == 2 ? nodeConfig.defaultTaskGlobalForm : nodeConfig.defaultGlobalForm);
    const properties = cloneDeep(getJnpfData('global') || {});
    Object.assign(state.globalForm, properties);
    if (!state.globalForm.formId) return;
    getFormByAllFormMap(state.globalForm.formId);
  }
  // 初始化发起人节点数据
  function initStartNodeData() {
    state.startForm = cloneDeep(nodeConfig.defaultStartForm);
    const properties = cloneDeep(getJnpfData(props.element.id) || {});
    Object.assign(state.startForm, properties);
    state.oldFormId = state.startForm.formId;
    if (!state.oldFormId) return;
    getFormByAllFormMap(state.oldFormId);
    nextTick(() => unref(startRef)?.updateCheckStatus());
    getFormFieldList(state.startForm.formId, 'startForm', true);
  }
  // 初始化审批节点数据
  function initApproverNodeData() {
    state.approverForm = cloneDeep(nodeConfig.defaultApproverForm);
    const properties = cloneDeep(getJnpfData(props.element.id) || {});
    const global = getJnpfData('global');
    const formId = properties.formId || global.formId;
    state.oldFormId = formId;
    getFormByAllFormMap(formId);
    properties.formOperates = initFormOperates(properties);
    Object.assign(state.approverForm, properties);
    nextTick(() => unref(approverRef)?.updateCheckStatus());
    getBeforeNodeOptions();
    getPrevNodeOptions();
    if (global.hasAloneConfigureForms && state.approverForm.formId) getFormFieldList(state.approverForm.formId, 'approverForm', true);
  }
  // 初始化办理节点数据
  function initProcessingNodeData() {
    state.processingForm = cloneDeep(nodeConfig.defaultProcessingForm);
    const properties = cloneDeep(getJnpfData(props.element.id) || {});
    const global = getJnpfData('global');
    const formId = properties.formId || global.formId;
    state.oldFormId = formId;
    getFormByAllFormMap(formId);
    properties.formOperates = initFormOperates(properties);
    Object.assign(state.processingForm, properties);
    nextTick(() => unref(processingRef)?.updateCheckStatus());
    getBeforeNodeOptions();
    getPrevNodeOptions();
    if (global.hasAloneConfigureForms && state.processingForm.formId) getFormFieldList(state.processingForm.formId, 'processingForm', true);
  }
  // 初始化子流程
  function initSubFlowNodeData() {
    state.subFlowForm = cloneDeep(nodeConfig.defaultSubFlowForm);
    const properties = cloneDeep(getJnpfData(props.element.id) || {});
    Object.assign(state.subFlowForm, properties);
    getBeforeNodeOptions();
    getPrevNodeOptions();
    const global = getJnpfData('global');
    const formId = properties.formId || global.formId;
    getFormByAllFormMap(formId);
    updateFlowInfo('subFlowForm');
  }
  // 初始化连接线
  function initConnectNodeData() {
    getConnectNodeFieldList();
    state.connectForm = cloneDeep(nodeConfig.defaultConnectForm);
    const properties = cloneDeep(getJnpfData(props.element.id) || { nodeId: props.element.id });
    Object.assign(state.connectForm, properties);
    // 初始化条件表单数据
    let nodeConditions = properties.conditions || [];
    for (let i = 0; i < nodeConditions.length; i++) {
      for (let j = 0; j < unref(usedFormItems).length; j++) {
        if (nodeConditions[i].id === unref(usedFormItems)[j].id) {
          nodeConditions[i] = { ...nodeConditions[i], ...unref(usedFormItems)[j] };
        }
      }
    }
    state.connectForm.conditions = cloneDeep(nodeConditions || []);
    state.connectForm.matchLogic = properties.matchLogic || 'and';
    // 连接线来源是否是开始节点
    let elementRegistry: any = props.bpmn.get('elementRegistry');
    let connectElement = elementRegistry.get(props.element.id);
    state.sourceIsStart = connectElement?.source?.wnType == 'start';
  }
  //初始化任务流程节点
  function initCommonTaskData() {
    let key = props.element.wnType;
    const type: any = props.element.wnType;
    const form = type + 'Form';
    handleProperties(form);
    if (isTriggerNode()) {
      const preNode = NodeUtils.getPreNodeList(toRaw(props.element))[0];
      const preNodeForm = getJnpfData(preNode?.id);
      const global = getJnpfData('global');
      state.triggerForm.formId = preNodeForm?.formId || global.formId;
      state.triggerForm.formName = preNodeForm?.formName || global.formName;
      if (state.triggerForm.triggerEvent == 1) state.triggerForm.triggerEvent = 2;
    }
    state.oldFormId = state[form].formId;
    if (isTriggerNode() || isEventTriggerNode() || isAddDataNode() || isUpdateDataNode() || isDeleteDataNode()) {
      getFormByAllFormMap(state[form].formId);
    }
    if (isScheduleNode() && !state[form]?.duration) state[form].duration = appStore.getSysConfigInfo?.duration;
    if (isTriggerNode() || isEventTriggerNode() || isGetDataNode() || isAddDataNode() || isUpdateDataNode() || isDeleteDataNode()) {
      if (isEventTriggerNode()) key = 'trigger';
      nextTick(() => unref(eval(key + 'Ref'))?.initCondition());
    }
    isWebhookTriggerNode() && initWebhookTriggerNodeData(form); //初始化webhook
    isTriggerNode() && getFormFieldList(state[form].formId, form); //初始化表单信息
    isLaunchFlowNode() && updateFlowInfo('launchFlowForm'); //更新流程信息
    // 标准、简单流程的分组id
    if (props?.element?.businessObject.$attrs?.customGroupId) {
      const nodeForm = getJnpfData(props?.element.id);
      nodeForm.groupId = props.element.businessObject.$attrs.customGroupId;
    }
    nextTick(() => {
      getDataSourceForm(); //数据来源
    });
  }
  // 判断是否是发起节点
  function isStartNode() {
    return props.element?.wnType ? NodeUtils.isStartNode({ type: props.element.wnType }) : false;
  }
  // 判断是否是审批节点
  function isApproverNode() {
    return props.element?.wnType ? NodeUtils.isApproverNode({ type: props.element.wnType }) : false;
  }
  // 判断是否是办理节点
  function isProcessingNode() {
    return props.element?.wnType ? NodeUtils.isProcessingNode({ type: props.element.wnType }) : false;
  }
  //  判断是否是子流程节点
  function isSubFlowNode() {
    return props.element?.wnType ? NodeUtils.isSubFlowNode({ type: props.element.wnType }) : false;
  }
  //  判断是否是连接线
  function isConnectNode() {
    return props.element?.type ? NodeUtils.isConnectNode({ type: props.element.type }) : false;
  }
  //  判断是否是触发节点
  function isTriggerNode() {
    return props.element?.wnType ? NodeUtils.isTriggerNode({ type: props.element.wnType }) : false;
  }
  //  判断是否是事件触发节点
  function isEventTriggerNode() {
    return props.element?.wnType ? NodeUtils.isEventTriggerNode({ type: props.element.wnType }) : false;
  }
  //  判断是否是定时触发节点
  function isTimeTriggerNode() {
    return props.element?.wnType ? NodeUtils.isTimeTriggerNode({ type: props.element.wnType }) : false;
  }
  //  判断是否是通知触发节点
  function isNoticeTriggerNode() {
    return props.element?.wnType ? NodeUtils.isNoticeTriggerNode({ type: props.element.wnType }) : false;
  }
  //  判断是否是Webhook触发节点
  function isWebhookTriggerNode() {
    return props.element?.wnType ? NodeUtils.isWebhookTriggerNode({ type: props.element.wnType }) : false;
  }
  //  判断是否是获取数据节点
  function isGetDataNode() {
    return props.element?.wnType ? NodeUtils.isGetDataNode({ type: props.element.wnType }) : false;
  }
  //  判断是否是新增数据节点
  function isAddDataNode() {
    return props.element?.wnType ? NodeUtils.isAddDataNode({ type: props.element.wnType }) : false;
  }
  //  判断是否是更新数据节点
  function isUpdateDataNode() {
    return props.element?.wnType ? NodeUtils.isUpdateDataNode({ type: props.element.wnType }) : false;
  }
  //  判断是否是删除数据节点
  function isDeleteDataNode() {
    return props.element?.wnType ? NodeUtils.isDeleteDataNode({ type: props.element.wnType }) : false;
  }
  //  判断是否是数据接口节点
  function isDataInterfaceNode() {
    return props.element?.wnType ? NodeUtils.isDataInterfaceNode({ type: props.element.wnType }) : false;
  }
  //  判断是否是消息通知节点
  function isMessageNode() {
    return props.element?.wnType ? NodeUtils.isMessageNode({ type: props.element.wnType }) : false;
  }
  //  判断是否是发起审批节点
  function isLaunchFlowNode() {
    return props.element?.wnType ? NodeUtils.isLaunchFlowNode({ type: props.element.wnType }) : false;
  }
  //  判断是否是日程节点
  function isScheduleNode() {
    return props.element?.wnType ? NodeUtils.isScheduleNode({ type: props.element.wnType }) : false;
  }
  // 初始化通用节点数据
  async function initWebhookTriggerNodeData(form) {
    if (!state[form].webhookUrl) {
      const res = await getWebhookUrl(props.flowInfo.flowId);
      if (!res) return;
      state[form].webhookUrl = res.data.webhookUrl;
      state[form].webhookGetFieldsUrl = res.data.requestUrl;
      state[form].webhookRandomStr = res.data.randomStr;
    }
  }
  function getFormByAllFormMap(formId) {
    const global = getJnpfData('global');
    state.formFieldList = [];
    if (formId && global.allFormMap && global.allFormMap['form_' + formId]) state.formFieldList = global.allFormMap['form_' + formId] || [];
  }
  function handleProperties(form) {
    state[form] = cloneDeep(nodeConfig[transformLetter(form)]);
    const properties = cloneDeep(getJnpfData(props.element.id) || {});
    Object.assign(state[form], properties);
  }
  function transformLetter(str) {
    return 'default' + str[0].toUpperCase() + str.slice(1);
  }
  function getJnpfData(key) {
    const jnpfData: any = props.bpmn?.get('jnpfData');
    return jnpfData?.data[key];
  }
  function getFormConf() {
    if (isStartNode()) return state.startForm;
    if (isApproverNode()) return state.approverForm;
    if (isProcessingNode()) return state.processingForm;
    if (isSubFlowNode()) return state.subFlowForm;
    if (isConnectNode()) return state.connectForm;
    if (isTriggerNode()) return state.triggerForm;
    if (isNoticeTriggerNode()) return state.noticeTriggerForm;
    if (isEventTriggerNode()) return state.eventTriggerForm;
    if (isTimeTriggerNode()) return state.timeTriggerForm;
    if (isWebhookTriggerNode()) return state.webhookTriggerForm;
    if (isGetDataNode()) return state.getDataForm;
    if (isAddDataNode()) return state.addDataForm;
    if (isUpdateDataNode()) return state.updateDataForm;
    if (isDeleteDataNode()) return state.deleteDataForm;
    if (isDataInterfaceNode()) return state.dataInterfaceForm;
    if (isMessageNode()) return state.messageForm;
    if (isLaunchFlowNode()) return state.launchFlowForm;
    if (isScheduleNode()) return state.scheduleForm;
    return state.globalForm;
  }
  function getPrintTplList() {
    getPrintDevSelector().then(res => {
      state.printTplOptions = res.data.list.filter(o => o.children && o.children.length).map(o => ({ ...o, hasChildren: true }));
    });
  }
  /**
   * 获取连接线的表单字段
   */
  function getConnectNodeFieldList() {
    //获取当前选中节点的来源节点（上一节点）
    let prevNodeId = toRaw(props.element).source.id;
    if (hasGatewayType.has(toRaw(props.element).source.wnType)) {
      prevNodeId = toRaw(props.element).source.incoming[0]?.source.id;
    }
    if (!prevNodeId) {
      state.formFieldList = [];
    } else {
      const jnpfData: any = props.bpmn.get('jnpfData');
      const global = jnpfData.getValue('global');
      const formId = jnpfData.getValue(prevNodeId, 'formId') || global.formId;
      getFormByAllFormMap(formId);
      state.formFieldList = state.formFieldList.filter(o => o.__config__.jnpfKey !== 'table');
    }
  }
  /**
   * 更新bpmn中属性和展示
   * @param key  更新的key
   * @param value  更新的value
   */
  function updateBpmnProperties(key, value) {
    if (!props.element) return;
    const modeling: any = props.bpmn.get('modeling');
    const element = props.bpmn.get('elementRegistry').get(props.element.id);
    element[key] = value;
    modeling.updateProperties(element, {});
  }
  /**
   * 往global节点设置发起节点的表单id（方便allFormMap）
   */
  function updateGlobalFormId(formId, formName) {
    const jnpfData: any = props.bpmn.get('jnpfData');
    jnpfData.setValue('global', { formId, formName });
  }
  function getFormFieldList(id, form, isSameForm = false, updateFormOperates = true) {
    if (!id) return;
    return new Promise((resolve, reject) => {
      getConfigData(id)
        .then(res => {
          const { type = 1, formData } = res.data;
          let formJson: any = {},
            fieldList: any = [];
          if (formData) formJson = JSON.parse(formData);
          fieldList = type == 2 ? transformFormJson(formJson) : formJson.fields;
          let list: any[] = transformFieldList(fieldList);
          state.formFieldList = list;
          if (updateFormOperates) state[form].formOperates = initFormOperates(state[form], true, isSameForm);
          updateAllFormMap(id, list);
          // 更新所有没设置表单的节点的表单权限
          if (form === 'startForm') updateAllNodeFormOperates(isSameForm);
          // 任务流程更新条件组
          if (['getDataForm', 'deleteDataForm'].includes(form)) handleGetDataForm(form, list);
          if (form === 'addDataForm') updateTransferList(form);
          if (['eventTriggerForm', 'getDataForm', 'addDataForm', 'updateDataForm', 'deleteDataForm'].includes(form)) {
            if (form == 'eventTriggerForm') form = 'triggerForm';
            unref(eval(state[form].type + 'Ref'))?.initCondition();
          }
          resolve({});
        })
        .catch(() => {
          reject();
        });
    });
  }
  /**
   * 更新全局属性中的allFormMap
   * @param id  id
   * @param list 数据
   */
  function updateAllFormMap(id, list) {
    const jnpfData: any = props.bpmn.get('jnpfData');
    const global = jnpfData.getValue('global');
    if (state.oldFormId && !getHasSameFormId(state.oldFormId)) delete global.allFormMap['form_' + state.oldFormId];
    if (id) global.allFormMap['form_' + id] = list;
    jnpfData.setValue('global', global);
    state.oldFormId = id;
  }
  /**
   * 判断其他节点是否跟当前节点旧表单同一表单
   * @param formId  表单id
   */
  function getHasSameFormId(formId) {
    if (!formId) return false;
    let hasSameFormId = false;
    const jnpfData: any = props.bpmn.get('jnpfData');
    for (const key in jnpfData.data) {
      const data = jnpfData.data[key];
      if (data?.nodeId !== props.element?.id && data?.formId === formId) {
        hasSameFormId = true;
        break;
      }
    }
    return hasSameFormId;
  }
  function transformFormJson(list) {
    const fieldList = list.map(o => ({
      __config__: {
        label: o.filedName,
        jnpfKey: o.jnpfKey || '',
        required: o.required || false,
      },
      __vModel__: o.filedId,
      multiple: o.multiple || false,
    }));
    return fieldList;
  }
  function transformFieldList(formFieldList) {
    let list: any[] = [];
    const loop = (data, parent?) => {
      if (!data) return;
      if (data.__vModel__) {
        const isTableChild = parent && parent.__config__ && parent.__config__.jnpfKey === 'table';
        const item: any = {
          id: isTableChild ? parent.__vModel__ + '-' + data.__vModel__ : data.__vModel__,
          fullName: isTableChild ? parent.__config__.label + '-' + data.__config__.label : data.__config__.label,
          ...omit(data, ['__config__', 'on', 'style', 'templateJson', 'addTableConf', 'tableConf']),
          __vModel__: isTableChild ? parent.__vModel__ + '-' + data.__vModel__ : data.__vModel__,
          __config__: {
            label: isTableChild ? parent.__config__.label + '-' + data.__config__.label : data.__config__.label,
            ...data.__config__,
          },
        };
        const config = item.__config__;
        if (dyOptionsList.includes(config.jnpfKey) && config.dataType !== 'static') delete item.options;
        list.push(item);
      }
      if (Array.isArray(data)) data.forEach(d => loop(d, parent));
      if (data.__config__ && data.__config__.children && Array.isArray(data.__config__.children)) {
        loop(data.__config__.children, data);
      }
    };
    loop(formFieldList);
    return list;
  }
  function updateFormFieldList(data) {
    state.formFieldList = data || [];
    updateAllFormMap('', data);
  }
  function initFormOperates(form, isUpdate = false, isSameForm = false) {
    const formOperates = form.formOperates || [];
    let res: any[] = [];
    const getWriteById = id => {
      const arr = formOperates.filter(o => o.id === id);
      return arr.length ? arr[0].write : NodeUtils.isStartNode(form);
    };
    const getReadById = id => {
      const arr = formOperates.filter(o => o.id === id);
      return arr.length ? arr[0].read : true;
    };
    const getRequiredById = id => {
      const arr = formOperates.filter(o => o.id === id);
      return arr.length ? arr[0].required : false;
    };
    if (!formOperates.length || isUpdate) {
      for (let i = 0; i < state.formFieldList.length; i++) {
        const data = state.formFieldList[i];
        res.push({
          id: data.id,
          name: data.fullName,
          required: !isSameForm ? data.__config__.required : data.__config__.required || getRequiredById(data.id),
          requiredDisabled: requiredDisabled(data.__config__.jnpfKey) || data.__config__.required,
          jnpfKey: data.__config__.jnpfKey,
          dataType: getDataType(data),
          read: !isSameForm ? true : getReadById(data.id),
          write: !isSameForm ? NodeUtils.isStartNode(form) : getWriteById(data.id),
        });
      }
    } else {
      res = formOperates;
    }
    return res;
  }
  // 更新所有节点表单权限
  function updateAllNodeFormOperates(isSameForm = false, clearAll = false) {
    const jnpfData: any = props.bpmn.get('jnpfData');
    for (const key in jnpfData.data) {
      const data = jnpfData.data[key];
      if ((data.type === 'approver' || data.type === 'processing') && !data.formId) {
        jnpfData.setValue(key, { formOperates: initFormOperates(data, true, isSameForm) });
      }
      //取消勾选允许审批节点独立配置表单对每个审批节点的表单数据做清空处理
      if ((data.type === 'approver' || data.type === 'processing') && clearAll) {
        let formFieldList = [];
        const global = jnpfData.getValue('global');
        const formId = global.formId;
        state.oldFormId = data.formId;
        if (formId && global.allFormMap && global.allFormMap['form_' + formId]) {
          formFieldList = global.allFormMap['form_' + formId] || [];
        }
        updateFormFieldList(formFieldList);
        jnpfData.setValue(key, { formOperates: initFormOperates(data, true, isSameForm), formId: '', formName: '', assignList: [] });
      }
    }
  }
  function handleSetJnpfData() {
    const formConf = getFormConf();
    const jnpfData: any = props.bpmn?.get('jnpfData');
    if (formConf.nodeId) jnpfData?.setValue(formConf.nodeId, formConf);
  }
  function getPrevNodeOptions() {
    const jnpfData: any = props.bpmn.get('jnpfData');
    const elementRegistry = props.bpmn.get('elementRegistry');
    state.prevNodeList = [];
    const getPrevNodeList = list => {
      for (let i = 0; i < list.length; i++) {
        const id = list[i].businessObject.sourceRef.id;
        const fullName = jnpfData.getValue(id, 'nodeName');
        const formId = jnpfData.getValue(id, 'formId');
        const type = jnpfData.getValue(id, 'type');
        if (['subFlow', 'gateway', 'confluence'].includes(type) || hasGatewayType.has(type)) {
          const subFlowElement = elementRegistry.get(id);
          getPrevNodeList(subFlowElement?.incoming || []);
        } else {
          state.prevNodeList.push({ fullName, id, type, formId });
        }
      }
    };
    getPrevNodeList(toRaw(props.element)?.incoming || []);
  }
  function getBeforeNodeOptions() {
    const jnpfData: any = props.bpmn.get('jnpfData');
    const elementRegistry = props.bpmn.get('elementRegistry');
    const list: any[] = toRaw(props.element)?.incoming || [];
    let newList: any[] = [];
    let newLineList: any[] = [];
    const loop = data => {
      for (let i = 0; i < data.length; i++) {
        const id = data[i].businessObject.sourceRef.id;
        const fullName = jnpfData.getValue(id, 'nodeName');
        const formId = jnpfData.getValue(id, 'formId');
        const type = jnpfData.getValue(id, 'type');
        if (NodeUtils.isApproverNode({ type }) || NodeUtils.isProcessingNode({ type })) {
          newList.push({ fullName, id, type, formId });
          newLineList.push(data[i].id);
        }
        const element = elementRegistry.get(id);
        if (element?.incoming?.length) {
          for (let j = 0; j < element?.incoming.length; j++) {
            const item = element?.incoming[j];
            if (!newLineList.includes(item.id)) loop([item]);
          }
        }
      }
    };
    loop(list);
    let beforeNodeList: any[] = [];
    for (let i = 0; i < newList.length; i++) {
      if (newList[i].id !== props.element.id && !beforeNodeList.some(o => o.id === newList[i].id)) {
        beforeNodeList.push(newList[i]);
      }
    }
    state.beforeNodeOptions = beforeNodeList;
  }
  function updateFlowInfo(form) {
    if (!state[form].flowId) return;
    getInfo(state[form].flowId).then(res => {
      const visibleType = res.data.visibleType || 1;
      const launchPermission = form == 'subFlowForm' ? 'subFlowLaunchPermission' : 'launchPermission';
      const approvers = form == 'subFlowForm' ? 'approvers' : 'initiator';
      form == 'subFlowForm' && unref(subFlowRef)?.getSubFlowFormInfo();
      if (visibleType != state[form][launchPermission]) {
        state[form][approvers] = [];
        state[form][launchPermission] = visibleType;
        form == 'launchFlowForm' && unref(launchDataRef)?.getFlowFormFieldList();
      }
    });
  }
  /**
   * 获取数据源表单（往上递归找获取数据节点和触发节点）
   */
  function getDataSourceForm() {
    const global = getJnpfData('global');
    const dataSourceFormList: any = [];
    const loop = data => {
      const node = getJnpfData(data?.id);
      if (node.type == 'approver') return;
      const preNodeList = NodeUtils.getPreNodeList(data);
      if (preNodeList?.length) preNodeList.map(o => loop(o));
      if (
        ['getData', 'trigger', 'eventTrigger', 'webhookTrigger'].includes(node.type) &&
        data.id !== props.element.id &&
        !dataSourceFormList.some(o => o.id == data.id)
      ) {
        let formFieldList: any[] = [];
        if (node.type == 'getData' || node.type == 'webhookTrigger') {
          formFieldList = cloneDeep(node.formFieldList) || [];
        } else {
          formFieldList = node.formId && global.allFormMap['form_' + node.formId] ? cloneDeep(global.allFormMap['form_' + node.formId]) : [];
        }
        const boo = formFieldList.some(o => o.id === '@formId');
        if (!boo) formFieldList.unshift({ fullName: '@表单ID', id: '@formId', label: '@formId(@表单ID)' });
        formFieldList = formFieldList
          .filter(o => o?.__config__?.jnpfKey !== 'table')
          .map(o => ({
            ...omit(o, ['disabled']),
            label: o.fullName,
            id: `${o.id}|${node.nodeId}`,
            __config__: {
              ...o.__config__,
              isSubTable: node?.formType === 4 ? false : o?.__config__?.isSubTable,
            },
          }));
        formFieldList?.length && dataSourceFormList.push({ id: data.id, fullName: node.nodeName, label: node.nodeName, children: formFieldList });
      }
    };
    props?.element && loop(toRaw(props.element));
    state.dataSourceFormList = dataSourceFormList;
  }
  function updateTransferList(form) {
    const global = getJnpfData('global');
    const formFieldList = form == 'addDataForm' ? global.allFormMap['form_' + state[form]?.formId] || [] : state[form].formFieldList || [];
    let list: any = [];
    for (let i = 0; i < formFieldList.length; i++) {
      if (formFieldList[i].__config__?.required) {
        list.push({ targetField: formFieldList[i].id, targetFieldLabel: formFieldList[i].fullName, sourceType: 1, sourceValue: '', required: true });
      }
    }
    state[form].transferList = list;
  }
  function handleGetDataForm(form, list) {
    if (state[form].formType == 4 || (form == 'deleteDataForm' && state[form].tableType == 1)) {
      state[form].subTableList = cloneDeep(list)
        .filter(o => o.__config__.jnpfKey == 'table')
        .map(o => ({ id: o.id, fullName: o.fullName }));
      state[form].cacheFormFieldList = list;
    } else if (form == 'deleteDataForm' && state[form].deleteType == 0 && state[form].tableType == 0) {
      state[form].formFieldList = list.filter(o => !o?.__config__?.isSubTable);
    } else {
      state[form].formFieldList = list;
    }
  }
  function updateConnectNameType(data) {
    let elementRegistry: any = props.bpmn.get('elementRegistry');
    let modeling: any = props.bpmn.get('modeling');
    let jnpfData: any = props.bpmn.get('jnpfData');
    let labelElements = elementRegistry.getAll().filter(element => element.wnType === typeCondition);
    jnpfData.setValue('global', { isShowConditions: data.isShowConditions, showNameType: data.showNameType });
    labelElements.map(label => {
      modeling.updateProperties(label, {});
    });
  }
  async function onConnectNameChange(key, value) {
    if (!props.element) return;
    const modeling: any = props.bpmn.get('modeling');
    const element = props.bpmn.get('elementRegistry').get(props.element.id + '_label');
    element[key] = value;
    modeling.updateProperties(element, {});
  }
  function updateDivideRule(type) {
    let element = props.bpmn.get('elementRegistry').get(props.element.id);
    let jnpfData = props.bpmn.get('jnpfData');
    let modeling: any = props.bpmn.get('modeling');
    if (type === 'parallel' || type === 'choose') {
      element?.outgoing.forEach(connect => {
        let label = props.bpmn.get('elementRegistry').get(connect.id + '_label');
        label && modeling.removeShape(label, { nested: true });
        let data = jnpfData.getValue(connect.id);
        data && jnpfData.setValue(connect.id, { ...data, conditions: [] });
      });
    }
  }

  onMounted(() => {
    getPrintTplList();
    nextTick(() => initGlobalNodeData());
  });
</script>
