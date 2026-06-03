<template>
  <HeaderContainer :formConf="formConf" @onNodeNameChange="updateBpmnProperties('nodeName', $event)" />
  <a-form :colon="false" :model="formConf" class="config-content" layout="vertical">
    <a-form-item label="触发类型" v-if="isTrigger">
      <a-radio-group v-model:value="formConf.isAsync" class="common-radio">
        <a-radio :value="1">异步</a-radio>
        <a-radio :value="0">同步</a-radio>
      </a-radio-group>
      <div class="common-tip mt-12px">
        {{
          formConf.isAsync === 0 ? '与审批流程串行，下一审批节点等待任务流程结束。当事件执行失败审批流程终止' : '与审批流程并行，当事件执行失败审批流程继续流转'
        }}
      </div>
    </a-form-item>
    <a-form-item label="触发表单">
      <FlowFormModal :value="formConf.formId" :title="formConf.formName" :disabled="isTrigger" type="1" @change="onFormIdChange" />
    </a-form-item>
    <a-form-item label="触发事件">
      <a-radio-group v-model:value="formConf.triggerEvent" class="common-radio" v-if="isTrigger">
        <a-radio :value="2">{{ triggerContext }}</a-radio>
        <a-radio :value="3">空白事件</a-radio>
      </a-radio-group>
      <div class="common-tip py-12px" v-if="isTrigger">{{ getTriggerEventTips }}</div>
      <template v-if="formConf.triggerEvent === 1 || !isTrigger">
        <jnpf-radio class="leading-32px" v-model:value="formConf.triggerFormEvent" :options="triggerFormEventOptions" />
        <template v-if="formConf.triggerFormEvent === 2">
          <jnpf-select
            v-model:value="formConf.updateFieldList"
            :options="getUpdateFieldOptions"
            :fieldNames="{ options: 'options1' }"
            showSearch
            allowClear
            multiple />
          <div class="common-tip mt-12px">未选择表示任意字段修改都会触发，选择了表示指定字段修改时触发</div>
        </template>
      </template>
      <div class="countersign-cell" v-if="formConf.triggerEvent === 2">
        <span class="w-70px">当操作</span>
        <jnpf-select v-model:value="formConf.actionList" :options="actionOptions()" showSearch allowClear multiple />
        <span class="w-70px ml-8px">时触发</span>
      </div>
    </a-form-item>
    <a-form-item label="触发条件" v-if="!isTrigger">
      <div class="common-tip mb-12px">满足以下条件，触发执行动作</div>
      <ConditionMain ref="conditionMainRef" bordered compact isCustomFieldValueType @confirm="onConditionConfirm" />
    </a-form-item>
  </a-form>
</template>
<script lang="ts" setup>
  import { ref, watch, computed, onMounted, nextTick } from 'vue';
  import HeaderContainer from './components/HeaderContainer.vue';
  import FlowFormModal from './components/FlowFormModal.vue';
  import ConditionMain from '@/components/ColumnDesign/src/components/ConditionMain.vue';
  import { NodeUtils } from '../bpmn/utils/nodeUtil';
  import { typeProcessing, typeTask } from '../bpmn/config/variableName';

  defineOptions({ inheritAttrs: false });
  defineExpose({ initCondition });

  const triggerFormEventOptions = [
    { id: 1, fullName: '新增数据' },
    { id: 2, fullName: '修改数据' },
    { id: 3, fullName: '删除数据' },
  ];
  const actionOptions = () => {
    const defaultOptions = [
      { id: 1, fullName: '同意' },
      { id: 2, fullName: '拒绝' },
      { id: 3, fullName: '退回' },
    ];
    const element = props.element && props.bpmn.get('elementRegistry')?.get(props.element.id);
    return element?.incoming?.[0]?.source?.wnType === typeProcessing
      ? [
          { id: 4, fullName: '确认办理' },
          { id: 3, fullName: '退回' },
        ]
      : defaultOptions;
  };
  const conditionMainRef = ref();
  const isTrigger = ref<boolean>(false);
  const props = defineProps([
    'formFieldsOptions',
    'formConf',
    'updateJnpfData',
    'bpmn',
    'getFormFieldList',
    'element',
    'updateFormFieldList',
    'updateBpmnProperties',
  ]);
  const getTriggerEventTips = computed(() => {
    if (props.formConf.triggerEvent === 2) return '通过流程的处置动作，触发执行后续流程';
    if (props.formConf.triggerEvent === 3) return '直接触发，执行后续流程';
    return '通过流程表单的增删改，触发执行后续流程';
  });
  const getUpdateFieldOptions = computed(() => props.formFieldsOptions.filter(o => !o?.__config__?.isSubTable));
  const triggerContext = computed(() => {
    let elementRegistry: any = props.bpmn.get('elementRegistry');
    let element = elementRegistry.get(props.element.id);
    let wnType = typeTask;
    if (element.incoming.length) wnType = element.incoming[0].source.wnType;
    return wnType === typeTask ? '审批事件' : '办理事件';
  });
  watch(
    () => props.formConf,
    () => props.updateJnpfData(),
    { deep: true, immediate: true },
  );
  watch(
    [() => props.formConf.formName, () => props.formConf.triggerEvent, () => props.formConf.triggerFormEvent, () => props.formConf.actionList],
    () => {
      let content = '';
      if (props.formConf.formName) {
        let triggerEvent = '空白事件';
        if (props.formConf.triggerEvent == 1) {
          triggerEvent = triggerFormEventOptions[props.formConf.triggerFormEvent - 1]?.fullName;
        } else if (props.formConf.triggerEvent == 2) {
          triggerEvent = props.formConf.actionList.map(o => actionOptions().filter(e => e?.id == o)[0]?.fullName).join('/');
        }
        content = `当[${props.formConf.formName}]表单[${triggerEvent}]时`;
      }
      props.formConf.content = content;
      props.updateBpmnProperties('elementBodyName', content);
    },
    { immediate: true },
  );

  function initCondition() {
    nextTick(() => {
      conditionMainRef.value?.init({
        conditionList: props.formConf.ruleList || [],
        matchLogic: props.formConf.ruleMatchLogic,
        fieldOptions: props.formFieldsOptions || [],
      });
    });
  }
  function onConditionConfirm(data) {
    props.formConf.ruleList = data.conditionList || [];
    props.formConf.ruleMatchLogic = data.matchLogic || [];
  }
  function onFormIdChange(id, item) {
    if (id && props.formConf.formId === id) return;
    if (!id) return handleNull();
    props.getFormFieldList(id, 'eventTriggerForm', false, false);
    props.formConf.formName = item.fullName;
    props.formConf.formId = id;
    props.formConf.ruleList = [];
    props.formConf.ruleMatchLogic = 'and';
    props.formConf.updateFieldList = [];
  }
  function handleNull() {
    props.formConf.formName = '';
    props.formConf.formId = '';
    props.formConf.ruleList = [];
    props.formConf.ruleMatchLogic = 'and';
    props.formConf.updateFieldList = [];
    props.updateFormFieldList();
    initCondition();
  }

  onMounted(() => {
    isTrigger.value = NodeUtils.isTriggerNode({ type: props.element.wnType });
  });
</script>
