<template>
  <HeaderContainer :formConf="formConf" @onNodeNameChange="updateBpmnProperties('nodeName', $event)" />
  <a-tabs v-model:activeKey="activeKey" size="small" class="pane-tabs">
    <a-tab-pane :key="1" tab="基础信息" />
    <a-tab-pane :key="2" tab="节点通知" />
  </a-tabs>
  <a-form :colon="false" :model="formConf" class="config-content" layout="vertical">
    <!-- 基础信息 -->
    <template v-if="activeKey === 1">
      <a-form-item label="子流程类型">
        <a-radio-group v-model:value="formConf.isAsync" class="common-radio">
          <a-radio :value="0">同步</a-radio>
          <a-radio :value="1">异步</a-radio>
        </a-radio-group>
      </a-form-item>
      <a-form-item label="子流程表单">
        <FlowModal :value="formConf.flowId" :isCheckSubFlow="1" :title="formConf.flowName" @change="onSubFlowIdChange" />
      </a-form-item>
      <a-form-item label="子流程传递">
        <a-input :value="formConf.assignList?.length ? '已设置' : ''" placeholder="请设置子流程传递规则" readonly class="hand" @click="openTransmitRuleBox">
          <template #suffix>
            <span class="ant-select-arrow"><down-outlined /></span>
          </template>
        </a-input>
      </a-form-item>
      <a-form-item label="发起设置">
        <div class="common-tip mb-10px">指定的成员作为子流程发起人，多人时，发起多个子流程</div>
        <template v-if="props.formConf.subFlowLaunchPermission == 1">
          <jnpf-radio v-model:value="formConf.assigneeType" :options="typeOptions" class="type-radio" @change="onAssigneeTypeChange" />
          <div :class="{ 'mb-10px': formConf.assigneeType !== 3, 'mt-10px': true }">
            <div v-if="[1, 2].includes(formConf.assigneeType)" class="common-tip">选择主管</div>
            <div v-if="formConf.assigneeType === 3" class="common-tip">发起者自己将作为审批人处理审批单</div>
            <div v-if="formConf.assigneeType === 4" class="common-tip">选择流程表单字段的值作为审批人</div>
            <div v-if="formConf.assigneeType === 5" class="common-tip">设置审批人为审批流程中某个环节的审批人</div>
            <div v-if="formConf.assigneeType === 6" class="common-tip">指定审批人处理审批单</div>
            <div v-if="formConf.assigneeType === 7" class="common-tip">默认所有人，需要设置请指定候选人范围处理审批单</div>
            <div v-if="formConf.assigneeType === 9" class="common-tip">从目标接口中获取审批人</div>
          </div>
          <a-form-item class="!mb-0" v-if="formConf.assigneeType == 1">
            <div class="countersign-cell">
              <jnpf-select v-model:value="formConf.approverType" :options="approverTypeOptions" class="!w-130px mr-8px" />
              的
              <a-select v-model:value="formConf.managerLevel" placeholder="请选择" class="flex-1 ml-8px">
                <a-select-option v-for="item in 10" :key="item" :value="item">{{ item === 1 ? '直属主管' : '第' + item + '级主管' }}</a-select-option>
              </a-select>
            </div>
          </a-form-item>
          <a-form-item class="!mb-0" v-if="formConf.assigneeType == 2">
            <div class="countersign-cell">
              <jnpf-select v-model:value="formConf.managerApproverType" :options="approverTypeOptions" class="!w-130px mr-8px" />
              的部门主管
            </div>
          </a-form-item>
          <a-form-item label="表单字段" class="!mb-0" v-if="formConf.assigneeType === 4">
            <a-input-group compact>
              <jnpf-select v-model:value="formConf.formFieldType" :options="formFieldTypeOptions" class="!w-100px" @change="onFormFieldTypeChange" />
              <jnpf-select
                v-model:value="formConf.formField"
                :options="getFormFieldOptions"
                :fieldNames="{ options: 'options1' }"
                showSearch
                style="width: calc(100% - 100px)" />
            </a-input-group>
          </a-form-item>
          <a-form-item label="工序节点" class="!mb-0" v-if="formConf.assigneeType === 5">
            <jnpf-select v-model:value="formConf.approverNodeId" showSearch :options="beforeNodeOptions" />
          </a-form-item>
          <a-form-item class="!mb-0" v-if="formConf.assigneeType === 9">
            <div class="ant-form-item-label">
              <label class="ant-form-item-no-colon">数据接口</label>
              <BasicHelp text='请求自带参数：taskId、taskNodeId，返回结构：JSON对象{"handleId":"id1,id2"}' />
            </div>
            <interface-modal :value="formConf.interfaceConfig.interfaceId" :title="formConf.interfaceConfig.interfaceName" @change="onInterfaceChange" />
            <template v-if="formConf.interfaceConfig.templateJson?.length">
              <div class="ant-form-item-label !mt-12px"><label class="ant-form-item-no-colon">参数设置</label></div>
              <select-interface-btn
                :templateJson="formConf.interfaceConfig.templateJson"
                :fieldOptions="childFieldOptions"
                isFlow
                showFieldFullLabel
                showSystemFullLabel
                @fieldChange="onRelationFieldChange" />
            </template>
          </a-form-item>
          <div v-if="formConf.assigneeType === 6 || formConf.assigneeType === 7">
            <jnpf-users-select
              v-model:value="formConf.approvers"
              buttonType="button"
              :modalTitle="formConf.assigneeType === 6 ? '添加审批人' : '添加候选人'"
              multiple
              @labelChange="onLabelChange" />
          </div>
          <a-form-item class="!mb-0 !mt-10px" v-if="formConf.assigneeType === 6 || (formConf.assigneeType === 7 && formConf.approvers?.length)">
            <template #label>
              {{ formConf.assigneeType === 6 ? '审批人范围' : '候选人范围' }}
              <BasicHelp :text="formConf.assigneeType === 6 ? '指定成员增加人员选择范围附加条件' : '候选人增加人员选择范围附加条件'" />
            </template>
            <jnpf-select v-model:value="formConf.extraRule" :options="formConf.assigneeType === 6 ? extraRuleOptions : candidateExtraRuleOptions" />
          </a-form-item>
        </template>
        <template v-else-if="props.formConf.subFlowLaunchPermission == 2">
          <initiator-user-select
            v-model:value="formConf.approvers"
            buttonType="button"
            multiple
            :flowId="formConf.flowId"
            @change="onApproversChange"
            @labelChange="onLabelChange" />
        </template>
      </a-form-item>
      <a-form-item label="子流程创建规则">
        <a-radio-group v-model:value="formConf.createRule" class="common-radio">
          <a-radio :value="0">同时创建</a-radio>
          <a-radio :value="1">依次创建</a-radio>
        </a-radio-group>
        <div class="common-tip !mt-12px" v-if="formConf.createRule === 0">多个发起人时，同时创建多条子流程。</div>
        <div class="common-tip !mt-12px" v-if="formConf.createRule === 1"> 多个发起人时依次创建子流程，上一条流程审批结束才创建下一条。</div>
      </a-form-item>
      <a-form-item>
        <template #label>异常处理<BasicHelp text="子流程发起节点人员异常时遵循该规则" /></template>
        <jnpf-select v-model:value="formConf.errorRule" :options="subFlowErrorRuleOptions" @change="formConf.errorRuleUser = []" />
        <jnpf-user-select v-model:value="formConf.errorRuleUser" buttonType="button" multiple class="mt-10px" v-if="formConf.errorRule === 2" />
      </a-form-item>
      <a-form-item v-if="props.type != 1" label="分流规则">
        <jnpf-select v-model:value="formConf.divideRule" :options="divideRuleOptions" @change="handleDivideRule()" />
      </a-form-item>
      <a-form-item>
        <template #label>子流程发起后自动提交<BasicHelp text="流程发起的下一节点设置候选人时无法自动发起审批" /></template>
        <a-radio-group v-model:value="formConf.autoSubmit" class="common-radio">
          <a-radio :value="0">否</a-radio>
          <a-radio :value="1">是</a-radio>
        </a-radio-group>
      </a-form-item>
    </template>
    <!-- 流程通知 -->
    <template v-if="activeKey === 2">
      <a-alert message="自定义通知以“消息中心-发送配置”为主，请移步设置；关闭：表示不提醒，默认：表示站内提醒" type="warning" showIcon class="!mb-10px" />
      <!-- 子流程发起 -->
      <NoticeConfig :noticeConfig="formConf.launchMsgConfig" :funcOptions="noticeOptions" type="launch" />
    </template>
  </a-form>
  <AssignRuleModal @register="registerAssignRuleModal" v-bind="getAssignBindValue" />
</template>
<script lang="ts" setup>
  import { reactive, toRefs, watch, computed, inject, unref, onMounted } from 'vue';
  import { getFlowFormInfo } from '@/api/workFlow/template';
  import { DownOutlined } from '@ant-design/icons-vue';
  import { useMessage } from '@/hooks/web/useMessage';
  import { cloneDeep } from 'lodash-es';
  import { useModal } from '@/components/Modal';
  import HeaderContainer from './components/HeaderContainer.vue';
  import NoticeConfig from './components/NoticeConfig.vue';
  import FlowModal from './components/FlowModal.vue';
  import AssignRuleModal from './components/AssignRuleModal.vue';
  import InitiatorUserSelect from './components/InitiatorUserSelect.vue';
  import { typeOptions, formFieldTypeOptions, divideRuleOptions, subFlowErrorRuleOptions } from '../helper/define';
  import { typeConfig } from '../bpmn/config';
  import { bpmnTask } from '../bpmn/config/variableName';
  import { InterfaceModal } from '@/components/CommonModal';
  import { SelectInterfaceBtn } from '@/components/Interface';

  interface State {
    activeKey: number;
    childFieldOptions: any[];
    userLabel: string;
    childOptions: any[];
  }
  const state = reactive<State>({
    activeKey: 1,
    childFieldOptions: [],
    userLabel: '',
    childOptions: [],
  });
  const { childFieldOptions } = toRefs(state);

  defineOptions({ inheritAttrs: false });
  defineExpose({ getSubFlowFormInfo });
  const { createMessage } = useMessage();
  const emit = defineEmits(['updateDivideRule']);
  const props = defineProps([
    'printTplOptions',
    'prevNodeList',
    'noticeOptions',
    'formFieldsOptions',
    'formConf',
    'getFormFieldList',
    'updateJnpfData',
    'transformFormJson',
    'transformFieldList',
    'beforeNodeOptions',
    'type',
    'updateBpmnProperties',
  ]);
  const { activeKey } = toRefs(state);
  const bpmn: (() => string | undefined) | undefined = inject('bpmn');
  const [registerAssignRuleModal, { openModal: openAssignRuleModal }] = useModal();

  const approverTypeOptions = [
    { id: 1, fullName: '发起人' },
    { id: 2, fullName: '上节点审批人' },
  ];

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

  const getBpmn = computed(() => (bpmn as () => any)());
  const getAssignBindValue = computed(() => ({
    formConf: props.formConf,
    formFieldsOptions: props.formFieldsOptions,
  }));
  const getFormFieldOptions = computed(() => getFormField(state.childOptions, props.formConf.formFieldType));

  watch(
    () => props.formConf,
    () => props.updateJnpfData(),
    { deep: true, immediate: true },
  );

  function getFormField(list, type) {
    if (type === 2) return list.filter(o => o.__config__.jnpfKey == 'depSelect');
    if (type === 3) return list.filter(o => o.__config__.jnpfKey == 'posSelect');
    if (type === 4) return list.filter(o => o.__config__.jnpfKey == 'roleSelect');
    if (type === 5) return list.filter(o => o.__config__.jnpfKey == 'groupSelect');
    return list.filter(o => o.__config__.jnpfKey == 'userSelect' || o.__config__.jnpfKey == 'usersSelect');
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
  function onFormFieldTypeChange() {
    props.formConf.formField = '';
  }
  function onRelationFieldChange(val, row) {
    if (!val) return;
    let list = state.childFieldOptions.filter(o => o.id === val);
    if (!list.length) return;
    let item = list[0];
    row.isSubTable = item.__config__ && item.__config__.isSubTable ? item.__config__.isSubTable : false;
  }
  function onAssigneeTypeChange() {
    props.formConf.approvers = [];
    props.formConf.approversSortList = [];
    props.formConf.extraRule = 1;
    props.updateBpmnProperties('elementBodyName', getContent());
  }
  function getContent() {
    let content = typeConfig[bpmnTask].renderer.bodyDefaultText;
    if (props.formConf.assigneeType != 6 && props.formConf.subFlowLaunchPermission == 1) {
      content = typeOptions.find(o => o.id === props.formConf.assigneeType)?.fullName || typeConfig[bpmnTask].renderer.bodyDefaultText;
    } else {
      content = state.userLabel || typeConfig[bpmnTask].renderer.bodyDefaultText;
    }
    props.formConf.content = content;
    return content;
  }
  function onSubFlowIdChange(id, item) {
    if (!id) {
      props.formConf.flowId = '';
      props.formConf.flowName = '';
      props.formConf.assignList = [];
      props.formConf.approvers = [];
      state.childFieldOptions = [];
      state.childOptions = [];
      props.formConf.subFlowLaunchPermission = 1;
      props.updateBpmnProperties('elementBodyName', getContent());
      return;
    }
    if (props.formConf.flowId === id) return;
    props.formConf.flowId = id;
    props.formConf.flowName = item.fullName;
    props.formConf.assignList = [];
    props.formConf.approvers = [];
    props.formConf.subFlowLaunchPermission = item.visibleType || 1;
    props.updateBpmnProperties('elementBodyName', getContent());
    getSubFlowFormInfo();
  }
  function getSubFlowFormInfo() {
    if (!props.formConf.flowId) return;
    getFlowFormInfo(props.formConf.flowId).then(res => {
      let { type = 1, formData } = res.data;
      props.formConf.formId = res.data.id;
      let formJson: any = {},
        fieldList: any[] = [],
        fieldOptions: any[] = [];
      if (formData) formJson = JSON.parse(formData);
      fieldList = type == 2 ? props.transformFormJson(formJson) : formJson.fields;
      fieldOptions = props.transformFieldList(fieldList).filter(o => o.__config__.jnpfKey !== 'table');
      state.childFieldOptions = fieldOptions.map(o => ({ ...o, label: o.fullName ? o.id + '(' + o.fullName + ')' : o.id, disabled: false }));
      state.childOptions = state.childFieldOptions.filter(o => o.__config__.jnpfKey !== 'table' && !o.__config__.isSubTable);
    });
  }
  function onApproversChange(_val, selectedData) {
    const labels = selectedData.map(o => o.fullName).join();
    onLabelChange(labels);
  }
  function onLabelChange(val) {
    state.userLabel = val;
    props.updateBpmnProperties('elementBodyName', getContent());
  }
  function openTransmitRuleBox() {
    if (!props.formConf.flowId) return createMessage.error('请选择子流程表单');
    const assignList = getRealAssignList(props.formConf.assignList ? cloneDeep(props.formConf.assignList) : []);
    openAssignRuleModal(true, { assignList, childFieldOptions: state.childFieldOptions, isSubFlow: 1 });
  }
  function getRealAssignList(assignList) {
    const jnpfData: any = unref(getBpmn).get('jnpfData');
    const global = jnpfData.getValue('global');
    let newAssignList = props.prevNodeList.map(o => {
      let formFieldList: any[] = [];
      const preFormId = o.formId;
      const globalFormId = global.formId;
      const formId = preFormId || globalFormId;
      if (formId && o.type != 'start' && global.allFormMap && global.allFormMap['form_' + formId]) {
        let list = cloneDeep(global.allFormMap['form_' + formId]) || [];
        list = list.filter(o => o.__config__.jnpfKey !== 'table');
        list = list.map(o => ({ ...o, id: o.id + '|' + formId }));
        const sysFieldList = [{ id: '@prevNodeFormId', fullName: '上节点表单Id' }];
        formFieldList.push({ fullName: '上节点表单', children: [...sysFieldList, ...list] });
      }
      if (preFormId && globalFormId && global.allFormMap && global.allFormMap['form_' + globalFormId]) {
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
  function handleDivideRule() {
    emit('updateDivideRule', props.formConf.divideRule);
  }
  onMounted(() => {
    getSubFlowFormInfo();
  });
</script>
