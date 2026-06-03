<template>
  <HeaderContainer :formConf="formConf" @onNodeNameChange="updateBpmnProperties('nodeName', $event)" />
  <a-form :colon="false" :model="formConf" class="config-content" layout="vertical">
    <a-form-item label="选择流程">
      <div class="common-tip mb-12px">自动发起流程，第一个工序节点设置候选人时无法自动发起</div>
      <FlowModal :value="formConf.flowId" :title="formConf.flowName" @change="onFlowIdChange" />
    </a-form-item>
    <a-form-item label="选择数据源">
      <div class="countersign-cell">
        <jnpf-select v-model:value="formConf.dataSourceForm" :options="dataSourceFormList" showSearch allowClear @change="onDataSourceFormChange" />
        <span class="w-220px ml-8px">的数据赋值给流程表单</span>
      </div>
    </a-form-item>
    <a-form-item label="字段设置">
      <div class="common-tip mb-12px">[必填]字段默认展示且不可删除，请至少设置一个字段</div>
      <div class="condition-main mb-10px">
        <a-row :gutter="8" v-for="(item, index) in formConf.transferList" :key="index" class="mt-10px">
          <a-col :span="8">
            <jnpf-select
              v-model:value="item.targetField"
              :options="getTargetOptions(index)"
              placeholder="目标表单字段"
              showSearch
              allowClear
              :disabled="item.required"
              :fieldNames="{ options: 'options1' }" />
          </a-col>
          <a-col :span="2" class="leading-32px">设为</a-col>
          <a-col :span="5">
            <jnpf-select v-model:value="item.sourceType" :options="sourceTypeOptions" @change="item.sourceValue = ''" />
          </a-col>
          <a-col :span="8">
            <jnpf-select
              v-model:value="item.sourceValue"
              :options="getDataSourceFormOptions(item)"
              placeholder="数据源字段"
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
      <span class="link-text inline-block" @click="addTransferItem()"><i class="icon-ym icon-ym-btn-add text-14px mr-4px"></i>添加字段</span>
    </a-form-item>
    <a-form-item label="发起人">
      <div class="common-tip mb-12px">选择多个审批人产生多条实例</div>
      <a-button preIcon="icon-ym icon-ym-btn-add" @click="createMessage.error('请选择流程表单')" v-if="!formConf.flowId">添加发起人</a-button>
      <template v-else>
        <initiator-user-select
          v-model:value="formConf.initiator"
          :flowId="formConf.flowId"
          buttonType="button"
          multiple
          v-if="formConf.launchPermission == 2" />
        <jnpf-users-select v-model:value="formConf.initiator" buttonType="button" modalTitle="添加发起人" multiple v-else />
      </template>
    </a-form-item>
  </a-form>
</template>
<script lang="ts" setup>
  import { watch, computed, unref } from 'vue';
  import { getFlowFormInfo } from '@/api/workFlow/template';
  import { cloneDeep } from 'lodash-es';
  import { useMessage } from '@/hooks/web/useMessage';
  import { systemComponentsList } from '@/components/FormGenerator/src/helper/config';
  import HeaderContainer from './components/HeaderContainer.vue';
  import FlowModal from './components/FlowModal.vue';
  import InitiatorUserSelect from './components/InitiatorUserSelect.vue';

  defineOptions({ inheritAttrs: false });
  defineExpose({ getFlowFormFieldList });

  const { createMessage } = useMessage();
  const props = defineProps([
    'formConf',
    'updateJnpfData',
    'dataSourceFormList',
    'transformFormJson',
    'transformFieldList',
    'updateTransferList',
    'updateBpmnProperties',
  ]);
  const sourceTypeOptions = [
    { id: 1, fullName: '字段' },
    { id: 2, fullName: '自定义' },
  ];

  watch(
    () => props.formConf,
    () => props.updateJnpfData(),
    { deep: true, immediate: true },
  );

  const getDataSourceFormList = computed(() => cloneDeep(props.dataSourceFormList || []).filter(o => o.id == props.formConf.dataSourceForm));

  function onFlowIdChange(id, item) {
    if (!id) return handleNull();
    props.formConf.flowName = item.fullName;
    props.formConf.flowId = id;
    props.formConf.transferList = [];
    props.formConf.initiator = [];
    props.formConf.launchPermission = item.visibleType || 1;
    props.formConf.content = `发起[${props.formConf.flowName}]流程`;
    props.updateBpmnProperties('elementBodyName', props.formConf.content);
    getFlowFormFieldList(true);
  }
  function getFlowFormFieldList(isInit = false) {
    if (!props.formConf.flowId) return;
    getFlowFormInfo(props.formConf.flowId).then(res => {
      let { type = 1, formData } = res.data;
      let formJson: any = {},
        fieldList: any[] = [];
      if (formData) formJson = JSON.parse(formData);
      fieldList = type == 2 ? props.transformFormJson(formJson) : formJson.fields;
      let list: any[] = props.transformFieldList(fieldList);
      props.formConf.formFieldList = list.map(o => ({ ...o, label: o.fullName ? o.id + '(' + o.fullName + ')' : o.id }));
      isInit && props.updateTransferList('launchFlowForm');
    });
  }
  function handleNull() {
    props.formConf.flowName = '';
    props.formConf.flowId = '';
    props.formConf.formFieldList = [];
    props.formConf.transferList = [];
    props.formConf.initiator = [];
    props.formConf.launchPermission = 1;
    props.formConf.content = '';
    props.updateBpmnProperties('elementBodyName', '');
  }
  function getTargetOptions(index: number) {
    let ignoreOptions: any[] = [];
    for (let i = 0; i < (props.formConf.transferList?.length || 0); i++) {
      const e = props.formConf.transferList[i];
      if (e.targetField && index !== i) ignoreOptions.push(e.targetField);
    }
    const list = props.formConf.formFieldList.filter(
      o => !ignoreOptions.includes(o.id) && !['table', ...systemComponentsList].includes(o?.__config__?.jnpfKey),
    );
    return list;
  }
  function addTransferItem() {
    props.formConf.transferList.push({ targetField: '', targetFieldLabel: '', sourceType: 1, sourceValue: '', required: false });
  }
  function delTransferItem(index) {
    props.formConf.transferList.splice(index, 1);
  }
  function onDataSourceFormChange() {
    props.updateTransferList('launchFlowForm');
  }
  function getDataSourceFormOptions(item) {
    if (!item.targetField) return [];
    if (item.targetField.includes('-')) return unref(getDataSourceFormList)[0]?.children;
    return unref(getDataSourceFormList)[0]?.children.filter(o => !o?.__config__?.isSubTable);
  }
</script>
