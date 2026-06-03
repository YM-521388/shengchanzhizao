<template>
  <HeaderContainer :formConf="formConf" @onNodeNameChange="updateBpmnProperties('nodeName', $event)" />
  <a-form :colon="false" :model="formConf" class="config-content" layout="vertical">
    <a-form-item label="获取方式">
      <a-radio-group v-model:value="formConf.formType" class="common-radio formType-radio" @change="onFormTypeChange">
        <a-radio :value="1">从表单中获取</a-radio>
        <a-radio :value="2">从流程中获取<BasicHelp text="只获取已完成的流程数据" /></a-radio>
        <a-radio :value="3">从数据接口中获取</a-radio>
        <a-radio :value="4">从子表中获取</a-radio>
      </a-radio-group>
      <FlowModal :value="formConf.formId" :title="formConf.formName" @change="onFormIdChange" v-if="formConf.formType == 2" />
      <template v-else-if="formConf.formType == 3">
        <select-interface
          :value="formConf.formId"
          :title="formConf.formName"
          :templateJson="formConf.interfaceTemplateJson"
          :fieldOptions="dataSourceForm"
          :showSystemFormId="false"
          fieldWidth="60px"
          @change="onFormIdChange" />
      </template>
      <div class="countersign-cell" v-else>
        <div :class="formConf.formType == 4 ? '!w-221px' : '!w-full'">
          <FlowFormModal
            :value="formConf.formId"
            :title="formConf.formName"
            :enableFlow="formConf.formType == 1 ? '0' : '1'"
            type="1"
            @change="onFormIdChange" />
        </div>
        <jnpf-select
          class="ml-8px !w-221px"
          v-model:value="formConf.subTable"
          :options="formConf.subTableList"
          allowClear
          showSearch
          @change="onSubTableChange"
          v-if="formConf.formType == 4" />
      </div>
    </a-form-item>
    <template v-if="formConf.formType != 3">
      <a-form-item label="获取条件">
        <div class="common-tip mb-12px">查询满足条件的数据，未设置条件则查询全部数据</div>
        <ConditionMain ref="conditionMainRef" bordered compact showFieldValueType :valueFieldOptions="dataSourceForm" @confirm="onConditionConfirm" />
      </a-form-item>
      <a-form-item label="排序规则">
        <div class="common-tip mb-12px">本节点作为单值数据源时，将按照排序规则取第一条数据</div>
        <div class="condition-main mb-10px">
          <a-row :gutter="8" v-for="(item, index) in formConf.sortList" :key="index" class="mt-10px">
            <a-col :span="15">
              <jnpf-select v-model:value="item.field" :options="getSortOptions" />
            </a-col>
            <a-col :span="8">
              <jnpf-select v-model:value="item.sortType" :options="sortTypeOptions" />
            </a-col>
            <a-col :span="1" class="text-center" v-if="!item.required">
              <i class="icon-ym icon-ym-btn-clearn" @click="delSortItem(index)" />
            </a-col>
          </a-row>
        </div>
        <span class="link-text inline-block" @click="addSortItem()"><i class="icon-ym icon-ym-btn-add text-14px mr-4px"></i>添加字段</span>
      </a-form-item>
    </template>
  </a-form>
</template>
<script lang="ts" setup>
  import { ref, watch, computed } from 'vue';
  import { SelectInterface } from '@/components/Interface';
  import HeaderContainer from './components/HeaderContainer.vue';
  import FlowFormModal from './components/FlowFormModal.vue';
  import FlowModal from './components/FlowModal.vue';
  import ConditionMain from '@/components/ColumnDesign/src/components/ConditionMain.vue';
  import { getFlowFormInfo } from '@/api/workFlow/template';
  import { cloneDeep } from 'lodash-es';
  import { notSupportList } from '../helper/define';

  defineOptions({ inheritAttrs: false });
  defineExpose({ initCondition });

  const conditionMainRef = ref();
  const props = defineProps([
    'formConf',
    'getFormFieldList',
    'updateJnpfData',
    'transformFormJson',
    'transformFieldList',
    'dataSourceForm',
    'updateBpmnProperties',
  ]);
  const sortTypeOptions = [
    { id: 'asc', fullName: '升序' },
    { id: 'desc', fullName: '降序' },
  ];

  const getSortOptions = computed(() => {
    let list: any[] =
      props.formConf.formType != 4
        ? (props.formConf.formFieldList || []).filter(o => o.id.indexOf('_jnpf_') < 0 && o.__config__.jnpfKey !== 'table' && !o?.__config__?.isSubTable)
        : props.formConf.formFieldList || [];
    return list.filter(o => !notSupportList.includes(o?.__config__?.jnpfKey)).map(o => ({ id: o.id, fullName: o.fullName }));
  });

  watch(
    () => props.formConf,
    () => props.updateJnpfData(),
    { deep: true, immediate: true },
  );

  function onFormTypeChange() {
    handleNull();
    initCondition();
  }
  function initCondition() {
    conditionMainRef.value?.init({
      conditionList: props.formConf.ruleList || [],
      matchLogic: props.formConf.ruleMatchLogic,
      fieldOptions: props.formConf.formFieldList || [],
    });
  }
  function onFormIdChange(id, item) {
    if (!id) return onFormTypeChange();
    if (props.formConf.formId === id) return;
    props.formConf.formName = item.fullName;
    props.formConf.formId = id;
    props.formConf.ruleList = [];
    props.formConf.ruleMatchLogic = 'and';
    props.formConf.sortList = [];
    props.formConf.subTable = '';
    const formType = props.formConf.formType;
    if (formType !== 4) setContent(props.formConf.formName);
    if (formType == 1 || formType == 4) props.getFormFieldList(id, 'getDataForm', false, false);
    if (formType == 2) handleFlowInfo(id);
    if (formType == 3) handleInterfaceInfo(item);
  }
  function handleInterfaceInfo(item) {
    const formFieldList = item.fieldJson ? JSON.parse(item.fieldJson) : [];
    props.formConf.formFieldList = formFieldList.map(o => ({
      ...o,
      id: o.defaultValue,
      fullName: o.field,
      label: o.field ? o.defaultValue + '(' + o.field + ')' : o.defaultValue,
    }));
    props.formConf.interfaceTemplateJson = (item.templateJson || []).map(o => ({ ...o, sourceType: 1, relationField: '' }));
  }
  function handleFlowInfo(id) {
    getFlowFormInfo(id).then(res => {
      let { type = 1, formData } = res.data;
      let formJson: any = {},
        fieldList: any[] = [];
      if (formData) formJson = JSON.parse(formData);
      fieldList = type == 2 ? props.transformFormJson(formJson) : formJson.fields;
      props.formConf.formFieldList = props.transformFieldList(fieldList);
      initCondition();
    });
  }
  function handleNull() {
    props.formConf.formName = '';
    props.formConf.formId = '';
    props.formConf.formFieldList = [];
    props.formConf.sortList = [];
    props.formConf.ruleList = [];
    props.formConf.ruleMatchLogic = 'and';
    props.formConf.interfaceTemplateJson = [];
    props.formConf.subTable = '';
    setContent();
    initCondition();
  }
  function onConditionConfirm(data) {
    props.formConf.ruleList = data.conditionList || [];
    props.formConf.ruleMatchLogic = data.matchLogic || [];
  }
  function setContent(content = '') {
    props.formConf.content = content ? `从[${content}]中获取数据` : '';
    props.updateBpmnProperties('elementBodyName', props.formConf.content);
  }
  function onSubTableChange(id, item) {
    let formFieldList = cloneDeep(props.formConf.cacheFormFieldList).filter(o => id == o.id?.split('-')[0] && o?.__config__?.jnpfKey !== 'table');
    formFieldList = (formFieldList || []).map(o => ({ ...o, fullName: o.fullName.replace(item.fullName + '-', '') }));
    props.formConf.formFieldList = formFieldList;
    setContent(item?.fullName || '');
    initCondition();
  }
  function delSortItem(index) {
    props.formConf.sortList.splice(index, 1);
  }
  function addSortItem() {
    props.formConf.sortList.push({ sortType: 'asc', field: '' });
  }
</script>
