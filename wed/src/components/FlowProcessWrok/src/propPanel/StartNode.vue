<template>
  <HeaderContainer :formConf="formConf" @onNodeNameChange="updateBpmnProperties('nodeName', $event)" />

<div class="config-content">
    <FlowFormModalStart :value="formConf.formId" :title="formConf.formName" :formConf="formConf" :isStartForm="1" @change="onFormIdChange"
    :disabled="flowState != 0 || versionList.length > 1" />
</div>
</template>
<script lang="ts" setup>
import { reactive, toRefs, watch, computed, h } from 'vue';
import HeaderContainer from './components/HeaderContainer.vue';
import FlowFormModalStart from './components/FlowFormModalStart.vue';
import { ClockSingle } from '@univerjs/icons';

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
const { formLoading } = toRefs(state);


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

function onFormIdChange(fromId, item) {
  props.updateBpmnProperties('elementBodyName', item.fullName);
  emit('updateGlobalFormId', fromId, item.fullName);
  if (!fromId) return handleNull();
  const isSameForm = props.formConf.formId === fromId;
  props.formConf.formId = fromId;
  props.formConf.formName = item.fullName;
  props.getFormFieldList(fromId, 'startForm', isSameForm);
}
function handleNull() {
  props.formConf.formId = '';
  props.formConf.formName = '';
  props.updateFormFieldList([]);
  props.formConf.formOperates = props.initFormOperates(props.formConf, true);
  props.updateAllNodeFormOperates();
}


function updateCheckStatus() {
  const formOperatesLen = props.formConf.formOperates?.length || 0;
  const requiredDisabledCount = props.formConf.formOperates?.filter(o => !o.requiredDisabled).length || 0;
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
</script>
