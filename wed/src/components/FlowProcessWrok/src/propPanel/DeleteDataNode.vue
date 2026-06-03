<template>
  <HeaderContainer :formConf="formConf" @onNodeNameChange="updateBpmnProperties('nodeName', $event)" />
  <a-form :colon="false" :model="formConf" class="config-content" layout="vertical">
    <a-form-item label="触发类型">
      <a-radio-group v-model:value="formConf.deleteType" class="common-radio" @change="onDeleteTypeChange">
        <a-radio :value="0">直接删除表</a-radio>
        <a-radio :value="1">按节点删除</a-radio>
      </a-radio-group>
      <template v-if="formConf.deleteType === 0">
        <div class="common-tip my-12px">主表模式删除主表时同步删除子表数据，子表模式只删除子表数据</div>
        <div class="countersign-cell">
          <span class="w-30px mr-8px">删除</span>
          <jnpf-select class="!w-80px mr-8px" v-model:value="formConf.tableType" :options="tableTypeOptions" @change="onTableTypeChange" />
          <div :class="formConf.tableType === 0 ? '!w-280px' : '!w-142px mr-8px'">
            <FlowFormModal :value="formConf.formId" :title="formConf.formName" type="1" @change="onFormIdChange" />
          </div>
          <jnpf-select
            class="!w-120px"
            v-if="formConf.tableType == 1"
            v-model:value="formConf.subTable"
            :options="formConf.subTableList"
            showSearch
            allowClear
            @change="onSubTableChange" />
          <span class="w-30px ml-8px">数据</span>
        </div>
      </template>
      <div class="countersign-cell mt-12px" v-else>
        <span class="w-30px mr-8px">删除</span>
        <div class="!w-135px mr-8px">
          <FlowFormModal :value="formConf.formId" :title="formConf.formName" type="1" @change="onFormIdChange" />
        </div>
        <span class="w-10px mr-8px">中</span>
        <jnpf-select class="!w-85px mr-8px" v-model:value="formConf.deleteCondition" :options="deleteConditionOptions" @change="onDeleteConditionChange" />
        <jnpf-select
          class="!w-120px"
          v-model:value="formConf.dataSourceForm"
          :options="dataSourceFormList"
          showSearch
          allowClear
          @change="onDataSourceFormChange" />
        <span class="w-30px ml-8px">数据</span>
      </div>
    </a-form-item>
    <a-form-item :label="formConf.deleteType === 0 ? '删除条件' : '数据校验'">
      <div class="common-tip mb-12px">{{ formConf.deleteType === 0 ? '未设置条件时删除目标表单的全部数据，请慎用' : '判断目标表单中是否存在获取的数据' }}</div>
      <ConditionMain
        ref="conditionMainRef"
        bordered
        compact
        showFieldValueType
        :valueFieldOptions="formConf.deleteType === 0 ? dataSourceForm : getDataSourceForm"
        @confirm="onConditionConfirm" />
    </a-form-item>
  </a-form>
</template>
<script lang="ts" setup>
  import { ref, watch, computed } from 'vue';
  import HeaderContainer from './components/HeaderContainer.vue';
  import FlowFormModal from './components/FlowFormModal.vue';
  import ConditionMain from '@/components/ColumnDesign/src/components/ConditionMain.vue';
  import { cloneDeep } from 'lodash-es';

  defineOptions({ inheritAttrs: false });
  defineExpose({ initCondition });

  const props = defineProps([
    'formFieldsOptions',
    'formConf',
    'updateJnpfData',
    'getFormFieldList',
    'dataSourceFormList',
    'dataSourceForm',
    'updateBpmnProperties',
  ]);
  const conditionMainRef = ref();
  const tableTypeOptions = [
    { fullName: '主表', id: 0 },
    { fullName: '子表', id: 1 },
  ];
  const deleteConditionOptions = [
    { fullName: '存在', id: 1 },
    { fullName: '不存在', id: 0 },
  ];

  watch(
    () => props.formConf,
    () => props.updateJnpfData(),
    { deep: true, immediate: true },
  );

  const getDataSourceForm = computed(() => cloneDeep(props.dataSourceForm).filter(o => o.id == props.formConf.dataSourceForm));

  function initCondition() {
    conditionMainRef.value?.init({
      conditionList: props.formConf.ruleList || [],
      matchLogic: props.formConf.ruleMatchLogic,
      fieldOptions: props.formConf.formFieldList || [],
    });
  }
  function onFormIdChange(id, item) {
    if (!id) return handleNull();
    if (props.formConf.formId === id) return;
    props.formConf.formName = item.fullName;
    props.formConf.formId = id;
    props.formConf.ruleList = [];
    props.formConf.ruleMatchLogic = 'and';
    props.formConf.subTable = '';
    props.formConf.formFieldList = [];
    setContent();
    props.getFormFieldList(id, 'deleteDataForm', false, false);
  }
  function handleNull() {
    props.formConf.formName = '';
    props.formConf.formId = '';
    props.formConf.formFieldList = [];
    props.formConf.ruleList = [];
    props.formConf.ruleMatchLogic = 'and';
    props.formConf.subTable = '';
    initCondition();
    setContent();
  }
  function onDeleteTypeChange() {
    props.formConf.tableType = 0;
    props.formConf.deleteCondition = 1;
    props.formConf.dataSourceForm = '';
    handleNull();
  }
  function onConditionConfirm(data) {
    props.formConf.ruleList = data.conditionList || [];
    props.formConf.ruleMatchLogic = data.matchLogic || [];
  }
  function onSubTableChange(id, item) {
    let formFieldList = cloneDeep(props.formConf.cacheFormFieldList).filter(o => id == o.id?.split('-')[0] && o?.__config__?.jnpfKey !== 'table');
    formFieldList = (formFieldList || []).map(o => ({ ...o, fullName: o.fullName.replace(item.fullName + '-', '') }));
    props.formConf.formFieldList = formFieldList;
    props.formConf.ruleList = [];
    props.formConf.ruleMatchLogic = 'and';
    initCondition();
  }
  function onTableTypeChange() {
    handleNull();
  }
  function onDeleteConditionChange() {
    setContent();
  }
  function onDataSourceFormChange(_id, item) {
    props.formConf.dataSourceFormName = item?.fullName;
    setContent();
  }
  function setContent() {
    let content = '';
    if (props.formConf.deleteType == 1) {
      if (props.formConf.formName && props.formConf.dataSourceFormName) {
        content = `删除[${props.formConf.formName}]${props.formConf.deleteCondition == 1 ? '存在' : '不存在'}[${props.formConf.dataSourceFormName}]数据`;
      }
    } else {
      if (props.formConf.formName) {
        content = `删除[${props.formConf.formName}]数据`;
      }
    }
    props.formConf.content = content;
    props.updateBpmnProperties('elementBodyName', content);
  }
</script>
