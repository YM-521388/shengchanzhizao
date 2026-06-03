<template>
  <HeaderContainer :formConf="formConf" @onNodeNameChange="updateBpmnProperties('nodeName', $event)" />
  <a-form :colon="false" :model="formConf" class="config-content" layout="vertical">
    <a-form-item label="更新方式">
      <div class="countersign-cell">
        <jnpf-select class="!w-165px" v-model:value="formConf.dataSourceForm" :options="dataSourceFormList" showSearch allowClear />
        <span class="w-100px mx-8px">的数据，更新至</span>
        <div class="!w-165px">
          <FlowFormModal :value="formConf.formId" :title="formConf.formName" type="1" @change="onFormIdChange" />
        </div>
      </div>
    </a-form-item>
    <a-form-item label="更新字段">
      <div class="common-tip">请至少设置一个字段</div>
      <div class="condition-main mt-12px" v-if="formConf?.transferList?.length">
        <a-row :gutter="8" v-for="(item, index) in formConf.transferList" :key="index" class="mt-10px">
          <a-col :span="7">
            <jnpf-select
              v-model:value="item.targetField"
              :options="getTargetOptions(index)"
              showSearch
              allowClear
              :disabled="item.required"
              :fieldNames="{ options: 'options1' }" />
          </a-col>
          <a-col :span="2" class="leading-32px">设为</a-col>
          <a-col :span="6">
            <jnpf-select v-model:value="item.sourceType" :options="simpleSourceTypeOptions" @change="item.sourceValue = ''" />
          </a-col>
          <a-col :span="8">
            <jnpf-select
              v-model:value="item.sourceValue"
              :options="getDataSourceFormList"
              showSearch
              allowClear
              :fieldNames="{ options: 'children' }"
              v-if="item.sourceType === 1" />
            <a-input v-model:value="item.sourceValue" placeholder="请输入" allowClear v-if="item.sourceType === 2" />
          </a-col>
          <a-col :span="1" class="text-center" v-if="!item.required">
            <i class="icon-ym icon-ym-btn-clearn" @click="delTransferItem(index)" />
          </a-col>
        </a-row>
      </div>
      <span class="link-text mt-10px inline-block" @click="addTransferItem"><i class="icon-ym icon-ym-btn-add text-14px mr-4px"></i>添加字段</span>
    </a-form-item>
    <a-form-item label="更新条件">
      <div class="common-tip mb-12px">设置数据源与目标表单是1:1/1:N的匹配关系</div>
      <ConditionMain ref="conditionMainRef" bordered compact showFieldValueType :valueFieldOptions="getDataSourceForm" @confirm="onConditionConfirm" />
      <a-checkbox class="leading-32px mt-12px" v-model:checked="formConf.unFoundRule">没有可修改的数据时，向对应表单中新增一条数据</a-checkbox>
    </a-form-item>
  </a-form>
</template>
<script lang="ts" setup>
  import { ref, watch, nextTick, computed } from 'vue';
  import { simpleSourceTypeOptions } from '../helper/define';
  import { cloneDeep } from 'lodash-es';
  import { systemComponentsList } from '@/components/FormGenerator/src/helper/config';
  import HeaderContainer from './components/HeaderContainer.vue';
  import FlowFormModal from './components/FlowFormModal.vue';
  import ConditionMain from '@/components/ColumnDesign/src/components/ConditionMain.vue';

  defineOptions({ inheritAttrs: false });
  defineExpose({ initCondition });

  const conditionMainRef = ref();
  const props = defineProps([
    'formFieldsOptions',
    'formConf',
    'updateJnpfData',
    'dataSourceForm',
    'dataSourceFormList',
    'getFormFieldList',
    'updateBpmnProperties',
  ]);

  const getDataSourceFormList = computed(() => cloneDeep(props.dataSourceFormList || []).filter(o => o.id == props.formConf.dataSourceForm));
  const getDataSourceForm = computed(() => cloneDeep(props.dataSourceForm || []).filter(o => o.id == props.formConf.dataSourceForm));

  watch(
    () => props.formConf,
    () => props.updateJnpfData(),
    { deep: true, immediate: true },
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
  function onFormIdChange(id, item) {
    if (id && props.formConf.formId === id) return;
    if (!id) return handleNull();
    props.getFormFieldList(id, 'updateDataForm', false, false);
    props.formConf.formName = item.fullName;
    props.formConf.formId = id;
    props.formConf.ruleList = [];
    props.formConf.ruleMatchLogic = 'and';
    props.formConf.transferList = [];
    props.formConf.content = `在[${item.fullName}]表单中更新数据`;
    props.updateBpmnProperties('elementBodyName', props.formConf.content);
  }
  function handleNull() {
    props.formConf.formName = '';
    props.formConf.formId = '';
    props.formConf.ruleList = [];
    props.formConf.ruleMatchLogic = 'and';
    props.formConf.transferList = [];
    props.formConf.content = '';
    props.updateBpmnProperties('elementBodyName', '');
    initCondition();
  }
  function onConditionConfirm(data) {
    props.formConf.ruleList = data.conditionList || [];
    props.formConf.ruleMatchLogic = data.matchLogic || [];
  }
  function delTransferItem(index) {
    props.formConf.transferList.splice(index, 1);
  }
  function addTransferItem() {
    props.formConf.transferList.push({ targetField: '', targetFieldLabel: '', sourceType: 1, sourceValue: '', required: false });
  }
  function getTargetOptions(index: number) {
    let ignoreOptions: any[] = [];
    for (let i = 0; i < props.formConf.transferList.length; i++) {
      const e = props.formConf.transferList[i];
      if (e.targetField && index !== i) ignoreOptions.push(e.targetField);
    }
    return (props.formFieldsOptions || []).filter(o => !ignoreOptions.includes(o.id) && !['table', ...systemComponentsList].includes(o?.__config__?.jnpfKey));
  }
</script>
