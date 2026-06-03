<template>
  <HeaderContainer :formConf="formConf" @onNodeNameChange="updateBpmnProperties('nodeName', $event)" />
  <a-tabs v-model:activeKey="activeKey" size="small" class="pane-tabs">
    <a-tab-pane :key="1" tab="基础信息" />
    <a-tab-pane :key="2" tab="表单权限" />
    <a-tab-pane :key="3" tab="流程事件" />
    <a-tab-pane :key="4" tab="流程通知" />
    <a-tab-pane :key="5" tab="超时设置" />
  </a-tabs>
  <template v-if="activeKey !== 2">
    <a-form :colon="false" :model="formConf" class="config-content" layout="vertical">
      <!-- 基础信息 -->
      <template v-if="activeKey === 1">
        <a-form-item label="流程表单" class="form-item-reload">
          <FlowFormModal
            :value="formConf.formId"
            :title="formConf.formName"
            :isStartForm="1"
            @change="onFormIdChange"
            :disabled="flowState != 0 || versionList.length > 1" />
          <a-tooltip title="刷新表单" v-if="flowState == 0 && formConf.formId">
            <a-button @click="handleRefreshFormField">
              <ReloadOutlined :spin="formLoading" />
            </a-button>
          </a-tooltip>
        </a-form-item>
        <a-form-item v-if="props.type != 1" label="分流规则">
          <jnpf-select v-model:value="formConf.divideRule" :options="divideRuleOptions" @change="handleDivideRule()" />
        </a-form-item>
        <a-form-item label="节点打印">
          <a-checkbox class="leading-32px" v-model:checked="formConf.printConfig.on">节点打印</a-checkbox>
          <template v-if="formConf.printConfig.on">
            <a-form-item label="请选择打印模板" class="!mt-12px">
              <jnpf-tree-select
                v-model:value="formConf.printConfig.printIds"
                :options="printTplOptions"
                multiple
                lastLevel
                :showCheckedStrategy="TreeSelect.SHOW_CHILD" />
            </a-form-item>
            <a-form-item label="打印条件">
              <jnpf-select v-model:value="formConf.printConfig.conditionType" :options="printConditionTypeOptions" />
            </a-form-item>
            <template v-if="formConf.printConfig.conditionType == 4">
              <a-button @click="handlePrintConfig">设置打印条件</a-button>
              <div class="conditions-content" v-if="formConf.printConfig.conditions.length">
                <span @click="handlePrintConfig"> {{ getPrintConditionsContent }}</span>
                <i class="icon-ym icon-ym-delete" @click="handlePrintConfigRemove"></i>
              </div>
            </template>
          </template>
        </a-form-item>
      </template>
      <!-- 流程事件 -->
      <template v-if="activeKey === 3">
        <a-form-item label="发起事件" class="normal-item-content">
          <a-switch v-model:checked="formConf.initFuncConfig.on" />
        </a-form-item>
        <a-form-item class="!-mt-12px" v-if="formConf.initFuncConfig.on">
          <div class="ant-form-item-label"><label class="ant-form-item-no-colon">数据接口</label></div>
          <select-interface
            :value="formConf.initFuncConfig.interfaceId"
            :title="formConf.initFuncConfig.interfaceName"
            :templateJson="formConf.initFuncConfig.templateJson"
            :fieldOptions="funcOptions"
            :sourceType="3"
            showSystemFullLabel
            isFlow
            fieldWidth="60px"
            @change="(val, data) => onFuncChange(keyMap.initFuncConfig, val, data)"
            @fieldChange="onRelationFieldChange" />
        </a-form-item>
        <a-form-item label="结束事件" class="normal-item-content">
          <a-switch v-model:checked="formConf.endFuncConfig.on" />
        </a-form-item>
        <a-form-item class="!-mt-12px" v-if="formConf.endFuncConfig.on">
          <div class="ant-form-item-label"><label class="ant-form-item-no-colon">数据接口</label></div>
          <select-interface
            :value="formConf.endFuncConfig.interfaceId"
            :title="formConf.endFuncConfig.interfaceName"
            :templateJson="formConf.endFuncConfig.templateJson"
            :fieldOptions="funcOptions"
            :sourceType="3"
            showSystemFullLabel
            isFlow
            fieldWidth="60px"
            @change="(val, data) => onFuncChange(keyMap.endFuncConfig, val, data)"
            @fieldChange="onRelationFieldChange" />
        </a-form-item>
        <a-form-item label="撤回事件" class="normal-item-content">
          <a-switch v-model:checked="formConf.flowRecallFuncConfig.on" />
        </a-form-item>
        <a-form-item class="!-mt-12px" v-if="formConf.flowRecallFuncConfig.on">
          <div class="ant-form-item-label"><label class="ant-form-item-no-colon">数据接口</label></div>
          <select-interface
            :value="formConf.flowRecallFuncConfig.interfaceId"
            :title="formConf.flowRecallFuncConfig.interfaceName"
            :templateJson="formConf.flowRecallFuncConfig.templateJson"
            :fieldOptions="funcOptions"
            :sourceType="3"
            showSystemFullLabel
            isFlow
            fieldWidth="60px"
            @change="(val, data) => onFuncChange(keyMap.flowRecallFuncConfig, val, data)"
            @fieldChange="onRelationFieldChange" />
        </a-form-item>
      </template>
      <!-- 流程通知 -->
      <template v-if="activeKey === 4">
        <a-alert message="自定义通知以“消息中心-发送配置”为主，请移步设置；关闭：表示不提醒，默认：表示站内提醒" type="warning" showIcon class="!mb-10px" />
        <!-- 流程待办 -->
        <NoticeConfig :noticeConfig="formConf.waitMsgConfig" :funcOptions="noticeOptions" type="wait" />
        <!-- 流程结束 -->
        <NoticeConfig :noticeConfig="formConf.endMsgConfig" :funcOptions="noticeOptions" type="end" />
        <!-- 流程评论 -->
        <NoticeConfig :noticeConfig="formConf.commentMsgConfig" :funcOptions="noticeOptions" type="comment" />
        <!-- 节点同意 -->
        <NoticeConfig :noticeConfig="formConf.approveMsgConfig" :funcOptions="noticeOptions" type="approve" />
        <!-- 节点拒绝 -->
        <NoticeConfig :noticeConfig="formConf.rejectMsgConfig" :funcOptions="noticeOptions" type="reject" />
        <!-- 节点退回 -->
        <NoticeConfig :noticeConfig="formConf.backMsgConfig" :funcOptions="noticeOptions" type="back" />
        <!-- 节点抄送 -->
        <NoticeConfig :noticeConfig="formConf.copyMsgConfig" :funcOptions="noticeOptions" type="copy" />
        <!-- 节点超时 -->
        <NoticeConfig :noticeConfig="formConf.overTimeMsgConfig" :funcOptions="noticeOptions" type="overTime" />
        <!-- 节点提醒 -->
        <NoticeConfig :noticeConfig="formConf.noticeMsgConfig" :funcOptions="noticeOptions" type="notice" />
      </template>
      <!-- 超时设置 -->
      <template v-if="activeKey === 5">
        <a-form-item label="限时设置">
          <jnpf-select v-model:value="formConf.timeLimitConfig.on" :options="overTimeMsgOptions" @change="onTimeLimitChange" />
          <template v-if="formConf.timeLimitConfig.on === 1">
            <a-form-item class="!mt-12px" label="节点处理截止时间">
              <div class="flex items-center">
                <jnpf-select v-model:value="formConf.timeLimitConfig.nodeLimit" :options="overTimeOptions" disabled />
                <span class="!ml-10px flex-shrink-0">开始</span>
              </div>
              <div class="countersign-cell mt-12px">
                <span class="w-90px inline-block">流程到达节点</span>
                <a-input-number
                  v-model:value="formConf.timeLimitConfig.duringDeal"
                  @change="e => handleSelectChange(e, 'timeLimitConfig.duringDeal', 1)"
                  :min="1"
                  :precision="0"
                  class="!w-90px !mx-8px" />
                <span>小时后，视为超时</span>
              </div>
            </a-form-item>
            <a-form-item label="限时提醒规则" class="!mt-12px">
              <jnpf-select v-model:value="formConf.noticeConfig.on" :options="overTimeMsgOptions" />
            </a-form-item>
            <a-form-item v-if="formConf.noticeConfig.on === 1">
              <div class="countersign-cell">
                <span class="w-90px inline-block">流程到达节点</span>
                <a-input-number
                  v-model:value="formConf.noticeConfig.firstOver"
                  @change="e => handleSelectChange(e, 'noticeConfig.firstOver', 1)"
                  :min="1"
                  :precision="0"
                  class="!w-90px !mx-8px" />
                <span>小时，开始提醒通知</span>
              </div>
              <div class="countersign-cell mt-12px">
                <span class="w-90px inline-block">每间隔</span>
                <a-input-number
                  v-model:value="formConf.noticeConfig.overTimeDuring"
                  @change="e => handleSelectChange(e, 'noticeConfig.overTimeDuring', 1)"
                  :min="1"
                  :precision="0"
                  class="!w-90px !mx-8px" />
                <span>小时，提醒通知一次</span>
              </div>
              <div class="ant-form-item-label !mt-12px"><label class="ant-form-item-no-colon">提醒事务</label></div>
              <a-row class="leading-32px block">
                <a-checkbox v-model:checked="formConf.noticeConfig.overNotice">
                  提醒通知<BasicHelp text="勾选后才能进行提醒消息发送（站内信系统默认发送，第三方超时消息需在节点通知内配置）" />
                </a-checkbox>
              </a-row>
              <a-row class="leading-32px block" v-if="false">
                <a-checkbox v-model:checked="formConf.noticeConfig.overEvent">提醒事件<BasicHelp text="请在节点事件内配置提醒事件" /></a-checkbox>
              </a-row>
              <div class="countersign-cell mt-12px" v-if="formConf.noticeConfig.overEvent">
                <span class="w-90px inline-block">在提醒通知第</span>
                <a-input-number
                  v-model:value="formConf.noticeConfig.overEventTime"
                  @change="e => handleSelectChange(e, 'noticeConfig.overEventTime', 1)"
                  :min="1"
                  :precision="0"
                  class="!w-90px !mx-8px" />
                <span>次时，执行提醒事件</span>
              </div>
            </a-form-item>
          </template>
        </a-form-item>
        <a-form-item label="超时设置">
          <jnpf-select v-model:value="formConf.overTimeConfig.on" :options="overTimeMsgOptions" :disabled="formConf.timeLimitConfig.on == 0" />
        </a-form-item>
        <a-form-item v-if="formConf.overTimeConfig.on === 1">
          <div class="countersign-cell">
            <span class="w-90px inline-block">超时</span>
            <a-input-number
              v-model:value="formConf.overTimeConfig.firstOver"
              @change="e => handleSelectChange(e, 'overTimeConfig.firstOver', 0)"
              :min="0"
              :precision="0"
              class="!w-90px !mx-8px" />
            <span>小时，开始超时通知</span>
          </div>
          <div class="countersign-cell mt-12px">
            <span class="w-90px inline-block">每间隔</span>
            <a-input-number
              v-model:value="formConf.overTimeConfig.overTimeDuring"
              @change="e => handleSelectChange(e, 'overTimeConfig.overTimeDuring', 1)"
              :min="1"
              :precision="0"
              class="!w-90px !mx-8px" />
            <span>小时，超时通知一次</span>
          </div>
          <div class="ant-form-item-label !mt-12px"><label class="ant-form-item-no-colon">超时事务</label></div>
          <a-row class="leading-32px block">
            <a-checkbox v-model:checked="formConf.overTimeConfig.overNotice">
              超时通知<BasicHelp text="勾选后才能进行提醒消息发送（站内信系统默认发送，第三方超时消息需在节点通知内配置）" />
            </a-checkbox>
          </a-row>
          <a-row class="leading-32px block" v-if="false">
            <a-checkbox v-model:checked="formConf.overTimeConfig.overEvent">超时事件<BasicHelp text="请在节点事件内配置提醒事件" /></a-checkbox>
          </a-row>
          <div class="countersign-cell" v-if="formConf.overTimeConfig.overEvent">
            <span class="w-90px inline-block">在超时通知</span>
            <a-input-number
              v-model:value="formConf.overTimeConfig.overEventTime"
              @change="e => handleSelectChange(e, 'overTimeConfig.overEventTime', 1)"
              :min="1"
              :precision="0"
              class="!w-90px !mx-8px" />
            <span>次时，执行超时事件</span>
          </div>
          <a-row class="leading-32px block">
            <a-checkbox v-model:checked="formConf.overTimeConfig.overAutoApprove">
              超时自动审批
              <BasicHelp text="当前审批节点表单必填字段为空工单流转时不做校验，下一审批节点设置候选人员、选择分支、异常节点时当前审批节点规则失效" />
            </a-checkbox>
          </a-row>
          <div class="countersign-cell" v-if="formConf.overTimeConfig.overAutoApprove">
            <span class="w-90px inline-block">在超时通知</span>
            <a-input-number
              v-model:value="formConf.overTimeConfig.overAutoApproveTime"
              @change="e => handleSelectChange(e, 'overTimeConfig.overAutoApproveTime', 1)"
              :min="1"
              :precision="0"
              class="!w-90px !mx-8px" />
            <span>次，执行超时自动审批</span>
          </div>
        </a-form-item>
      </template>
    </a-form>
  </template>
  <!-- 表单权限 -->
  <template v-else>
    <a-table :data-source="formConf.formOperates" :columns="formOperatesColumns" size="small" :pagination="false" :scroll="{ y: 'calc(100vh - 197px)' }">
      <template #headerCell="{ column }">
        <template v-if="column.key === 'write'">
          <a-checkbox v-model:checked="readAllChecked" :indeterminate="isReadIndeterminate" @change="handleCheckAllChange($event, 1)">查看</a-checkbox>
          <a-checkbox v-model:checked="writeAllChecked" :indeterminate="isWriteIndeterminate" @change="handleCheckAllChange($event, 2)">编辑</a-checkbox>
          <a-checkbox v-model:checked="requiredAllChecked" :indeterminate="isRequiredIndeterminate" @change="handleCheckAllChange($event, 3)">
            必填
          </a-checkbox>
        </template>
      </template>
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'write'">
          <a-checkbox v-model:checked="record.read" @change="handleCheckedChange">查看</a-checkbox>
          <a-checkbox v-model:checked="record.write" @change="handleCheckedChange">编辑</a-checkbox>
          <a-checkbox v-model:checked="record.required" :disabled="record.requiredDisabled" @change="handleCheckedChange">必填</a-checkbox>
        </template>
      </template>
    </a-table>
  </template>
  <ConditionModal @register="registerConditionModal" @confirm="updatePrintConfig" />
</template>
<script lang="ts" setup>
  import { reactive, toRefs, watch, computed, h } from 'vue';
  import { TreeSelect } from 'ant-design-vue';
  import { overTimeMsgOptions, overTimeOptions, printConditionTypeOptions, divideRuleOptions, keyMap } from '../helper/define';
  import { useModal } from '@/components/Modal';
  import HeaderContainer from './components/HeaderContainer.vue';
  import NoticeConfig from './components/NoticeConfig.vue';
  import FlowFormModal from './components/FlowFormModal.vue';
  import ConditionModal from './components/ConditionModal.vue';
  import { NodeUtils } from '../bpmn/utils/nodeUtil';
  import { SelectInterface } from '@/components/Interface';
  import { ReloadOutlined } from '@ant-design/icons-vue';

  interface State {
    activeKey: number;
    readAllChecked: boolean;
    writeAllChecked: boolean;
    requiredAllChecked: boolean;
    isReadIndeterminate: boolean;
    isWriteIndeterminate: boolean;
    isRequiredIndeterminate: boolean;
    formLoading: boolean;
  }
  const state = reactive<State>({
    activeKey: 1,
    readAllChecked: false,
    writeAllChecked: false,
    requiredAllChecked: false,
    isReadIndeterminate: false,
    isWriteIndeterminate: false,
    isRequiredIndeterminate: false,
    formLoading: false,
  });
  defineOptions({ inheritAttrs: false });
  defineExpose({ updateCheckStatus });
  const emit = defineEmits(['updateGlobalFormId', 'updateDivideRule']);
  const props = defineProps([
    'printTplOptions',
    'noticeOptions',
    'formConf',
    'getFormFieldList',
    'updateJnpfData',
    'initFormOperates',
    'updateAllNodeFormOperates',
    'usedFormItems',
    'formFieldsOptions',
    'flowState',
    'versionList',
    'type',
    'funcOptions',
    'updateFormFieldList',
    'updateBpmnProperties',
  ]);
  const { activeKey, readAllChecked, writeAllChecked, requiredAllChecked, isReadIndeterminate, isWriteIndeterminate, isRequiredIndeterminate, formLoading } =
    toRefs(state);
  const [registerConditionModal, { openModal: openConditionModal }] = useModal();
  const formOperatesColumns = [
    { title: '表单字段', dataIndex: 'name', key: 'name' },
    { title: '操作', dataIndex: 'write', key: 'write', align: 'center', width: 200 },
  ];

  const getPrintConditionsContent = computed(() =>
    NodeUtils.getConditionsContent(props.formConf.printConfig.conditions, props.formConf.printConfig.matchLogic),
  );

  watch(
    () => state.activeKey,
    val => {
      if (val === 2) updateCheckStatus();
    },
  );
  watch(
    () => props.formConf,
    () => props.updateJnpfData(),
    { deep: true, immediate: true },
  );

  function onFormIdChange(id, item) {
    props.updateBpmnProperties('elementBodyName', item.fullName);
    emit('updateGlobalFormId', id, item.fullName);
    if (!id) return handleNull();
    const isSameForm = props.formConf.formId === id;
    props.formConf.formId = id;
    props.formConf.formName = item.fullName;
    props.getFormFieldList(id, 'startForm', isSameForm);
  }
  function handleNull() {
    props.formConf.formId = '';
    props.formConf.formName = '';
    props.updateFormFieldList([]);
    props.formConf.formOperates = props.initFormOperates(props.formConf, true);
    props.updateAllNodeFormOperates();
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
    const formOperatesLen = props.formConf.formOperates.length;
    const requiredDisabledCount = props.formConf.formOperates.filter(o => !o.requiredDisabled).length;
    let readCount = 0;
    let writeCount = 0;
    let requiredCount = 0;
    props.formConf.formOperates.forEach(item => {
      if (item.read) readCount++;
      if (item.write) writeCount++;
      if (item.required) requiredCount++;
    });
    state.readAllChecked = !!formOperatesLen ? formOperatesLen === readCount : false;
    state.writeAllChecked = !!formOperatesLen ? formOperatesLen === writeCount : false;
    state.requiredAllChecked = !!formOperatesLen ? requiredDisabledCount === requiredCount : false;
    state.isReadIndeterminate = !!readCount && readCount < formOperatesLen;
    state.isWriteIndeterminate = !!writeCount && writeCount < formOperatesLen;
    state.isRequiredIndeterminate = !!requiredCount && requiredCount < requiredDisabledCount;
  }
  function handlePrintConfig() {
    openConditionModal(true, { usedFormItems: props.usedFormItems, formFieldsOptions: props.formFieldsOptions, ...props.formConf.printConfig });
  }
  function updatePrintConfig(data) {
    props.formConf.printConfig = { ...props.formConf.printConfig, ...data };
  }
  function handlePrintConfigRemove() {
    props.formConf.printConfig.matchLogic = 'and';
    props.formConf.printConfig.conditions = [];
  }
  function onTimeLimitChange(val) {
    if (val == 0) props.formConf.overTimeConfig.on = 0;
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
    props.formConf[key].templateJson = row.templateJson.map(o => ({ ...o, relationField: '', sourceType: 1 })) || [];
  }
  function onRelationFieldChange(val, row) {
    if (!val) return;
    let list = props.funcOptions.filter(o => o.id === val);
    if (!list.length) return;
    let item = list[0];
    row.isSubTable = item.__config__ && item.__config__.isSubTable ? item.__config__.isSubTable : false;
  }
  function handleRefreshFormField() {
    if (state.formLoading == true) return;
    state.formLoading = true;
    props
      .getFormFieldList(props.formConf.formId, 'startForm', true)
      .then(() => {
        state.formLoading = false;
      })
      .catch(() => {
        state.formLoading = false;
      });
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
</script>
