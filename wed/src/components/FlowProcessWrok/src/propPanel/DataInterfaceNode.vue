<template>
  <HeaderContainer :formConf="formConf" @onNodeNameChange="updateBpmnProperties('nodeName', $event)" />
  <a-form :colon="false" :model="formConf" class="config-content" layout="vertical">
    <a-form-item label="选择数据源">
      <div class="countersign-cell">
        <jnpf-select v-model:value="formConf.dataSourceForm" :options="dataSourceFormList" showSearch allowClear @change="onDataSourceFormChange" />
        <span class="w-220px ml-8px">的数据赋值给接口</span>
      </div>
    </a-form-item>
    <a-form-item label="执行数据接口">
      <select-interface
        :value="formConf.formId"
        :title="formConf.formName"
        :templateJson="formConf.templateJson"
        :fieldOptions="getDataSourceForm"
        :showSystemFormId="false"
        :sourceType="3"
        :allowClear="false"
        @change="onFormIdChange"
        @fieldChange="onRelationFieldChange" />
    </a-form-item>
  </a-form>
</template>
<script lang="ts" setup>
  import { computed, watch } from 'vue';
  import { SelectInterface } from '@/components/Interface';
  import HeaderContainer from './components/HeaderContainer.vue';

  defineOptions({ inheritAttrs: false });

  const props = defineProps(['formFieldsOptions', 'formConf', 'updateJnpfData', 'dataSourceFormList', 'dataSourceForm', 'updateBpmnProperties']);

  const getDataSourceForm = computed(() => (props.formConf.dataSourceForm ? props.dataSourceForm.filter(o => o.id == props.formConf.dataSourceForm) : []));

  watch(
    () => props.formConf,
    () => props.updateJnpfData(),
    { deep: true, immediate: true },
  );

  function onFormIdChange(id, item) {
    if (!id) return handleNull();
    if (props.formConf.formId === id) return;
    props.formConf.formName = item.fullName;
    props.formConf.formId = id;
    props.formConf.templateJson = (item.templateJson || []).map(o => ({ ...o, sourceType: 1, relationField: '' }));
    props.formConf.content = `执行[${props.formConf.formName}]数据接口`;
    props.updateBpmnProperties('elementBodyName', props.formConf.content);
  }
  function handleNull() {
    props.formConf.formName = '';
    props.formConf.formId = '';
    props.formConf.templateJson = [];
    props.formConf.content = '';
    props.updateBpmnProperties('elementBodyName', '');
  }
  function onRelationFieldChange(val, row) {
    if (!val) return;
    let list = props.formFieldsOptions.filter(o => o.id === val);
    if (!list.length) return;
    let item = list[0];
    row.isSubTable = item.__config__ && item.__config__.isSubTable ? item.__config__.isSubTable : false;
  }
  function onDataSourceFormChange() {
    props.formConf.templateJson = (props.formConf.templateJson || []).map(o => ({ ...o, relationField: o.sourceType == 1 ? '' : o.relationField }));
  }
</script>
