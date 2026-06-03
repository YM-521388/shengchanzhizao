<template>
  <HeaderContainer :formConf="formConf" @onNodeNameChange="updateBpmnProperties('nodeName', $event)">
    <a-button type="primary" @click="handleSubmit" class="m-r-10">确认</a-button>
  </HeaderContainer>
  <div class="config-content">
    <FlowFormModalNode ref="flowFormModalNode" :value="formConf.formId" :title="formConf.formName" :formConf="formConf"
      :updateBpmnProperties="updateBpmnProperties" @change="onFormIdChange" placeholder="请选择"
      :disabled="flowState != 0" />

  </div>
  <AssignRuleModal @register="registerAssignRuleModal" v-bind="getAssignBindValue" />
  <ConditionModal @register="registerConditionModal" @confirm="updatePrintConfig" />
  <ApproversSortModal @register="registerApproversSortModal" @confirm="updateApproversSortList" />
  <StyleScript type="workflow" @register="registerStyleScriptModal" @confirm="updateStyleScript" />
  <AuxiliaryMessageModal @register="registerAuxiliaryMessageModal" @confirm="updateAuxiliaryMessage" />
</template>
<script lang="ts" setup>
import { reactive, toRefs, inject, computed, unref, watch, h, ref } from 'vue';
import { cloneDeep } from 'lodash-es';
import { useModal } from '@/components/Modal';
import {
  typeOptions,
  defaultStep,
  systemFieldOptions,
} from '../helper/define';
import { NodeUtils } from '../bpmn/utils/nodeUtil';
import { formatToDateTime } from '@/utils/dateUtil';
import HeaderContainer from './components/HeaderContainer.vue';
import FlowFormModalNode from './components/FlowFormModalNode.vue';
import AssignRuleModal from './components/AssignRuleModal.vue';
import ConditionModal from './components/ConditionModal.vue';
import ApproversSortModal from './components/ApproversSortModal.vue';
import { typeConfig } from '../bpmn/config';
import { bpmnTask } from '../bpmn/config/variableName/index';
import { buildBitUUID } from '@/utils/uuid';
import StyleScript from './components/StyleScript.vue';
import AuxiliaryMessageModal from './components/AuxiliaryMessage.vue';

interface State {
  activeKey: number;
  assignList: any[];
  userLabel: string;
  readAllChecked: boolean;
  writeAllChecked: boolean;
  requiredAllChecked: boolean;
  isReadIndeterminate: boolean;
  isWriteIndeterminate: boolean;
  isRequiredIndeterminate: boolean;
  conditionType: string;
  currentBtnIndex: number;
  jsJson: string;
  formLoading: boolean;
}

const state = reactive<State>({
  activeKey: 1,
  assignList: [],
  userLabel: '',
  readAllChecked: false,
  writeAllChecked: false,
  requiredAllChecked: false,
  isReadIndeterminate: false,
  isWriteIndeterminate: false,
  isRequiredIndeterminate: false,
  conditionType: '',
  currentBtnIndex: 0,
  jsJson: '({ flowInfo, formData, taskInfo, onlineUtils }) => {\n    // 在此编写代码\n    \n}',
  formLoading: false,
});
defineOptions({ inheritAttrs: false });
defineExpose({ getContent, updateCheckStatus });
const emit = defineEmits(['updateDivideRule']);
const props = defineProps([
  'formConf',
  'printTplOptions',
  'prevNodeList',
  'beforeNodeOptions',
  'funcOptions',
  'noticeOptions',
  'formFieldsOptions',
  'usedFormItems',
  'getFormFieldList',
  'initFormOperates',
  'updateJnpfData',
  'nodeOptions',
  'flowState',
  'type',
  'updateFormFieldList',
  'updateBpmnProperties',
]);
const [registerAssignRuleModal, { openModal: openAssignRuleModal }] = useModal();
const [registerConditionModal, { openModal: openConditionModal }] = useModal();
const [registerApproversSortModal, { openModal: openApproversSortModal }] = useModal();
const [registerStyleScriptModal, { openModal: openStyleScriptModal }] = useModal();
const [registerAuxiliaryMessageModal, { openModal: openAuxiliaryModal }] = useModal();
const bpmn: (() => string | undefined) | undefined = inject('bpmn');

const flowFormModalNode = ref<any>(null)

const extraRuleOptions = [
  { id: 1, fullName: '无审批人范围' },
  { id: 6, fullName: '同一公司' },
  { id: 2, fullName: '同一部门' },
  { id: 3, fullName: '同一岗位' },
  { id: 4, fullName: '发起人上级' },
  { id: 5, fullName: '发起人下属' },
];
const candidateExtraRuleOptions = [
  { id: 1, fullName: '无候选人范围' },
  { id: 6, fullName: '同一公司' },
  { id: 2, fullName: '同一部门' },
  { id: 3, fullName: '同一岗位' },
];
const overTimeRuleOptions = [
  { id: 2, fullName: '同一部门' },
  { id: 7, fullName: '同一角色' },
  { id: 3, fullName: '同一岗位' },
  { id: 8, fullName: '同一分组' },
];
const extraCopyRuleOptions = extraRuleOptions.map(o => (o.id === 1 ? { id: 1, fullName: '无抄送人范围' } : o));
const counterSignOptions = [
  { id: 0, fullName: '或签（一名审批人同意或拒绝即可）' },
  { id: 1, fullName: '会签（无序会签，审批达到会签比例，审批通过）' },
  { id: 2, fullName: '依次审批（按顺序依次审批）' },
];
const auditTypeOptions = [
  { id: 1, fullName: '按百分比' },
  { id: 2, fullName: '按人数' },
];
const calculateTypeOptions = [
  { id: 1, fullName: '实时计算' },
  { id: 2, fullName: '延后计算' },
];
const rejectTypeOptions = [{ id: 0, fullName: '无' }, ...auditTypeOptions];
const formOperatesColumns = [
  { title: '表单字段', dataIndex: 'name', key: 'name' },
  { title: '操作', dataIndex: 'write', key: 'write', align: 'center', width: 250 },
];
const conditionTypeOptions = [
  { id: 1, fullName: '字段' },
  { id: 2, fullName: '自定义' },
  { id: 3, fullName: '系统参数' },
];
const approverTypeOptions = [
  { id: 1, fullName: '发起人' },
  { id: 2, fullName: '上节点审批人' },
];
let transferTypeOptions = [
  { fullName: '指定成员', id: 6 },
  { fullName: '超时审批人', id: 11 },
  { fullName: '数据接口', id: 9 },
];
const getBpmn = computed(() => (bpmn as () => any)());
const getJnpfGlobalData = computed(() => {
  const jnpfData: any = unref(getBpmn).get('jnpfData');
  return jnpfData?.getValue('global') || {};
});
const getBindValue = computed(() => ({
  funcOptions: props.noticeOptions,
  isApprover: true,
}));
const getAssignBindValue = computed(() => ({
  formConf: props.formConf,
  formFieldsOptions: props.formFieldsOptions,
}));
const backNodeCodeOptions = computed(() => {
  let options = [...defaultStep, ...props.beforeNodeOptions];
  if (props.formConf.backType == 2) options = options.filter(o => o.id != 1);
  return options;
});
const getPrintConditionsContent = computed(() =>
  NodeUtils.getConditionsContent(props.formConf.printConfig.conditions, props.formConf.printConfig.matchLogic),
);
const getAuditConditionsContent = computed(() =>
  NodeUtils.getConditionsContent(props.formConf.autoAuditRule.conditions, props.formConf.autoAuditRule.matchLogic),
);
const getRejectConditionsContent = computed(() =>
  NodeUtils.getConditionsContent(props.formConf.autoRejectRule.conditions, props.formConf.autoRejectRule.matchLogic),
);
const getSystemFieldOptions = computed(() => systemFieldOptions.map(o => ({ ...o, label: o.fullName ? o.id + '(' + o.fullName + ')' : o.id })));
const getParameterList = computed(() => unref(getJnpfGlobalData).globalParameterList || []);
const getApprovalFieldOptions = computed(() => unref(getJnpfGlobalData).approvalFieldList || []);
const getCanSetApproversSort = computed(() => {
  return (
    props.formConf.assigneeType === 6 &&
    props.formConf.approvers?.length &&
    props.formConf.approvers.every(o => o.indexOf('--user') !== -1) &&
    props.formConf.counterSign == 2
  );
});
const getHasAssign = computed(() => props.formConf.assignList?.length && props.formConf.assignList.some(o => o.ruleList?.length));
const getFormFieldOptions = computed(() => getFormField(props.usedFormItems, props.formConf.formFieldType));
const getCopyFormFieldOptions = computed(() => getFormField(props.usedFormItems, props.formConf.copyFormFieldType));

watch(
  () => props.formConf,
  () => props.updateJnpfData(props.formConf),
  { deep: true, immediate: true },
);
watch(
  () => state.activeKey,
  val => {
    val == 3 && updateCheckStatus();
  },
);

function onFormIdChange(id, item) {
  if (!id) return handleNull();
  const isSameForm = props.formConf.formId === id;
  props.formConf.formName = item.fullName;
  props.formConf.formId = id;
  props.formConf.assignList = [];
  props.getFormFieldList(id, 'approverForm', isSameForm);
}
function handleNull() {
  props.formConf.formName = '';
  props.formConf.formId = '';
  let formFieldList = [];
  const jnpfData: any = unref(getBpmn).get('jnpfData');
  const global = jnpfData.getValue('global');
  const formId = global.formId;
  if (formId && global.allFormMap && global.allFormMap['form_' + formId]) {
    formFieldList = global.allFormMap['form_' + formId] || [];
  }
  props.updateFormFieldList(formFieldList);
  props.formConf.formOperates = props.initFormOperates(props.formConf, true);
}
function onBackTypeChange(e) {
  if (e?.target?.value == 2 && props.formConf.backNodeCode == 1) props.formConf.backNodeCode = 0;
}
function openTransmitRuleBox() {
  const assignList = getRealAssignList(props.formConf.assignList ? cloneDeep(props.formConf.assignList) : []);
  openAssignRuleModal(true, { assignList });
}
function getRealAssignList(assignList) {
  const jnpfData: any = unref(getBpmn).get('jnpfData');
  const global = jnpfData.getValue('global');
  let newAssignList = props.prevNodeList.map(o => {
    let formFieldList: any[] = [];
    const globalFormId = global.formId;
    const formId = o.formId || globalFormId;
    if (formId && o.type != 'start' && global.allFormMap && global.allFormMap['form_' + formId]) {
      let list = cloneDeep(global.allFormMap['form_' + formId]) || [];
      list = list.filter(o => o.__config__.jnpfKey !== 'table');
      list = list.map(o => ({ ...o, id: o.id + '|' + formId }));
      const sysFieldList = [{ id: '@prevNodeFormId', fullName: '上节点表单Id' }];
      formFieldList.push({ fullName: '上节点表单', children: [...sysFieldList, ...list] });
    }
    if (globalFormId && global.allFormMap && global.allFormMap['form_' + globalFormId]) {
      let list = cloneDeep(global.allFormMap['form_' + globalFormId]) || [];
      list = list.filter(o => o.__config__.jnpfKey !== 'table');
      list = list.map(o => ({ ...o, id: o.id + '|' + globalFormId }));
      const sysFieldList = [{ id: '@startNodeFormId', fullName: '发起节点表单Id' }];
      formFieldList.push({ fullName: '发起节点表单', children: [...sysFieldList, ...list] });
    }
    const globalParameterList = cloneDeep(global.globalParameterList).map(o => ({ ...o, fullName: o.fieldName, id: o.fieldName + '|globalParameter' }));
    globalParameterList.length && formFieldList.push({ fullName: '全局变量', children: globalParameterList || [] });
    return {
      nodeId: o.id,
      title: o.fullName,
      formFieldList,
      ruleList: [],
    };
  });
  if (!assignList.length) return newAssignList;
  let list: any[] = [];
  // 去掉被删掉的节点
  for (let i = 0; i < assignList.length; i++) {
    const e = assignList[i];
    inter: for (let j = 0; j < newAssignList.length; j++) {
      if (e.nodeId === newAssignList[j].nodeId) {
        const item = {
          nodeId: e.nodeId,
          title: newAssignList[j].title,
          formFieldList: newAssignList[j].formFieldList,
          ruleList: e.ruleList,
        };
        list.push(item);
        break inter;
      }
    }
  }
  const addList = newAssignList.filter(o => !assignList.some(item => item.nodeId === o.nodeId));
  return [...list, ...addList];
}
function onAssigneeTypeChange() {
  props.formConf.approvers = [];
  props.formConf.approversSortList = [];
  props.formConf.extraRule = 1;
  props.updateBpmnProperties('elementBodyName', getContent());
}
function onFormFieldTypeChange() {
  props.formConf.formField = '';
}
function onCopyFormFieldTypeChange() {
  props.formConf.copyFormField = '';
}
function getFormField(list, type) {
  if (type === 2) return list.filter(o => o.__config__.jnpfKey == 'depSelect');
  if (type === 3) return list.filter(o => o.__config__.jnpfKey == 'posSelect');
  if (type === 4) return list.filter(o => o.__config__.jnpfKey == 'roleSelect');
  if (type === 5) return list.filter(o => o.__config__.jnpfKey == 'groupSelect');
  return list.filter(o => o.__config__.jnpfKey == 'userSelect' || o.__config__.jnpfKey == 'usersSelect');
}
function onApproversChange(val) {
  if (props.formConf.assigneeType != 6 || !val || !val.length || !val.every(o => o.indexOf('--user') !== -1)) return (props.formConf.approversSortList = []);
  if (!props.formConf.approversSortList?.length) return (props.formConf.approversSortList = val);
  const arr = props.formConf.approversSortList.filter(o => val.indexOf(o) !== -1);
  const updated = val.filter(o => props.formConf.approversSortList.indexOf(o) === -1);
  props.formConf.approversSortList = [...arr, ...updated];
}
function onLabelChange(val) {
  state.userLabel = val;
  props.updateBpmnProperties('elementBodyName', getContent());
}
function getContent() {
  let content = typeConfig[bpmnTask].renderer.bodyDefaultText;
  if (props.formConf.assigneeType != 6) {
    content = typeOptions.find(o => o.id === props.formConf.assigneeType)?.fullName || typeConfig[bpmnTask].renderer.bodyDefaultText;
  } else {
    content = state.userLabel || typeConfig[bpmnTask].renderer.bodyDefaultText;
  }
  props.formConf.content = content;
  return content;
}
function onInterfaceChange(val, row) {
  if (!val) {
    props.formConf.interfaceConfig.interfaceId = '';
    props.formConf.interfaceConfig.interfaceName = '';
    props.formConf.interfaceConfig.templateJson = [];
    return;
  }
  if (props.formConf.interfaceConfig.interfaceId === val) return;
  props.formConf.interfaceConfig.interfaceId = val;
  props.formConf.interfaceConfig.interfaceName = row.fullName;
  props.formConf.interfaceConfig.templateJson = row.templateJson.map(o => ({ ...o, sourceType: 1 })) || [];
}
function onOverTimeConfigInterfaceChange(val, row) {
  if (!val) {
    props.formConf.overTimeConfig.interfaceId = '';
    props.formConf.overTimeConfig.interfaceName = '';
    props.formConf.overTimeConfig.templateJson = [];
    return;
  }
  if (props.formConf.overTimeConfig.interfaceId === val) return;
  props.formConf.overTimeConfig.interfaceId = val;
  props.formConf.overTimeConfig.interfaceName = row.fullName;
  props.formConf.overTimeConfig.templateJson = row.templateJson.map(o => ({ ...o, sourceType: 1 })) || [];
}
function onRelationFieldChange(val, row) {
  if (!val) return;
  let list = props.funcOptions.filter(o => o.id === val);
  if (!list.length) return;
  let item = list[0];
  row.isSubTable = item.__config__ && item.__config__.isSubTable ? item.__config__.isSubTable : false;
}
function onSignNumberChange(val, key) {
  if (val) return;
  props.formConf.counterSignConfig[key] = 1;
}
function handleCheckAllChange(e, type) {
  const val = e.target.checked;
  if (type == 1) state.isReadIndeterminate = false;
  if (type == 2) state.isWriteIndeterminate = false;
  if (type == 3) state.isRequiredIndeterminate = false;
  props.formConf.formOperates.forEach(item => {
    if (type == 1) item.read = val;
    if (type == 2) item.write = val;
    if (type == 3 && !item.requiredDisabled) item.required = val;
  });
}
function handleCheckedChange() {
  updateCheckStatus();
}
function updateCheckStatus() {
  const formOperatesLen = props.formConf.formOperates?.length || 0;
  const requiredDisabledCount = props.formConf.formOperates?.filter(o => !o.requiredDisabled).length || 0;
  let readCount = 0;
  let writeCount = 0;
  let requiredCount = 0;
  props.formConf.formOperates?.forEach(item => {
    if (item.read) readCount++;
    if (item.write) writeCount++;
    if (item.required && !item.requiredDisabled) requiredCount++;
  });
  state.readAllChecked = !!formOperatesLen ? formOperatesLen === readCount : false;
  state.writeAllChecked = !!formOperatesLen ? formOperatesLen === writeCount : false;
  state.requiredAllChecked = !!formOperatesLen ? requiredDisabledCount === requiredCount : false;
  state.isReadIndeterminate = !!readCount && readCount < formOperatesLen;
  state.isWriteIndeterminate = !!writeCount && writeCount < formOperatesLen;
  state.isRequiredIndeterminate = !!requiredCount && requiredCount < requiredDisabledCount;
}
function handlePrintConfig() {
  state.conditionType = 'print';
  openConditionModal(true, { usedFormItems: props.usedFormItems, formFieldsOptions: props.formFieldsOptions, ...props.formConf.printConfig });
}
function updatePrintConfig(data) {
  const form = state.conditionType == 'print' ? 'printConfig' : state.conditionType == 'audit' ? 'autoAuditRule' : 'autoRejectRule';
  props.formConf[form] = { ...props.formConf[form], ...data };
}
function handlePrintConfigRemove() {
  props.formConf.printConfig.matchLogic = 'and';
  props.formConf.printConfig.conditions = [];
}
function handleAutoApproverRule(type) {
  state.conditionType = type;
  const rule = type == 'audit' ? 'autoAuditRule' : 'autoRejectRule';
  openConditionModal(true, { usedFormItems: props.usedFormItems, formFieldsOptions: props.formFieldsOptions, ...props.formConf[rule] });
}
function handleAddParameter() {
  props.formConf.parameterList.push({
    field: '',
    fieldValueType: 2,
    fieldValue: '',
  });
}
function onFieldValueChange(item, val, data) {
  item.fieldLabel = val ? data.fullName : '';
  item.fieldValueJnpfKey = val ? data.__config__.jnpfKey : '';
}
function onConditionDateChange(val, item) {
  if (!val) return (item.fieldLabel = '');
  const format = item.format || 'YYYY-MM-DD HH:mm:ss';
  item.fieldLabel = formatToDateTime(val, format);
}
function onConditionListChange(item, val, data) {
  if (!val) return (item.fieldLabel = '');
  const labelList = data.map(o => o.fullName);
  item.fieldLabel = labelList.join('/');
}
function onConditionOrganizeChange(item, val, data) {
  if (!val) return (item.fieldLabel = '');
  item.fieldLabel = data.organize || '';
}
function onConditionObjChange(item, val, data) {
  if (!val) return (item.fieldLabel = '');
  item.fieldLabel = data.fullName || '';
}
function handleDelItem(index) {
  props.formConf.parameterList.splice(index, 1);
}
function onFieldValueTypeChange(item) {
  item.fieldValue = '';
  item.fieldLabel = '';
}
function openApproversSortListModal() {
  openApproversSortModal(true, { ids: props.formConf.approversSortList });
}
function updateApproversSortList(data) {
  props.formConf.approversSortList = data;
}
function onTimeLimitChange(val) {
  if (val == 0) props.formConf.overTimeConfig.on = 0;
}
function onHasFreeApproverBtnChange(val) {
  if (!val) props.formConf.hasReduceApproverBtn = false;
}
function onFuncChange(key, val, row) {
  if (!val) {
    props.formConf[key].interfaceId = '';
    props.formConf[key].interfaceName = '';
    props.formConf[key].templateJson = [];
    return;
  }
  if (props.formConf[key].interfaceId === val) return;
  props.formConf[key].interfaceId = val;
  props.formConf[key].interfaceName = row.fullName;
  props.formConf[key].templateJson = row.templateJson.map(o => ({ ...o, sourceType: 1 })) || [];
}
function onOverAutoTransferChange(value) {
  if (value) props.formConf.overTimeConfig.overAutoApprove = false;
}
function onOverAutoApproveChange(value) {
  if (value) props.formConf.overTimeConfig.overAutoTransfer = false;
}
function addCustomBtn() {
  const data = {
    value: 'btn_' + buildBitUUID(),
    label: getFieldName(),
    jsJson: state.jsJson,
  };
  props.formConf.customBtns.push(data);
}
function setDefaultValue(e) {
  if (e.element.label === '') {
    const data = {
      ...e.element,
      label: getFieldName(),
    };
    props.formConf.customBtns[e.index] = data;
  }
}
// 生成参数名称
function getFieldName() {
  if (!props.formConf.customBtns?.length) return '按钮1';
  let maxNumber = 0;
  const regex = /按钮(\d+)/;
  props.formConf.customBtns.map(o => {
    const match = o.label.match(regex);
    if (match) {
      const number = parseInt(match[1]);
      if (number > maxNumber) maxNumber = number;
    }
  });
  return '按钮' + ++maxNumber;
}
function editStyle(element, index) {
  state.currentBtnIndex = index;
  openStyleScriptModal(true, { text: element.jsJson || state.jsJson });
}
function updateStyleScript(data) {
  props.formConf.customBtns[state.currentBtnIndex].jsJson = data;
}
function handleRefreshFormField() {
  if (state.formLoading == true) return;
  state.formLoading = true;
  props
    .getFormFieldList(props.formConf.formId, 'approverForm', true)
    .then(() => {
      state.formLoading = false;
    })
    .catch(() => {
      state.formLoading = false;
    });
}
function openAuxiliaryMessageModal() {
  openAuxiliaryModal(true, { auxiliaryInfo: props.formConf.auxiliaryInfo, formFieldsOptions: props.formFieldsOptions });
}
function updateAuxiliaryMessage(data) {
  props.formConf.auxiliaryInfo = data;
}
function handleSelectChange(value, fieldPath, min) {
  let keys = fieldPath.split('.');
  let obj = props.formConf;
  while (keys.length > 1) {
    const key = keys.shift();
    obj = obj[key];
  }
  const lastKey = keys[0];
  obj[lastKey] = value || min;
}
function handleDivideRule() {
  emit('updateDivideRule', props.formConf.divideRule);
}
function handleSubmit() {
  flowFormModalNode.value?.handleSubmit();

}

</script>
<style scoped>
.m-r-10 {
  margin-right: 10px;
}
</style>