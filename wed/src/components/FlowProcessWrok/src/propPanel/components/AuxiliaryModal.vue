<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerAuxiliaryModal"
    :title="type == 'add' ? '新增' : '编辑'"
    :width="600"
    @ok="handleSubmit"
    class="add-config-modal"
    destroyOnClose>
    <a-form :colon="false" labelAlign="right" ref="formElRef" :model="dataForm" :labelCol="{ style: { width: '80px' } }">
      <a-form-item label="数据接口">
        <interface-modal :value="dataForm.interfaceId" :title="dataForm.interfaceName" :sourceType="1" @change="onInterfaceChange" />
      </a-form-item>
      <a-form-item label="参数设置" style="margin-bottom: 0"></a-form-item>
      <a-table :data-source="dataForm.templateJson" :columns="templateJsonColumns" size="small" :pagination="false">
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'field'">
            <span class="required-sign">{{ record.required ? '*' : '' }}</span>
            {{ record.field }}{{ record.fieldName ? '(' + record.fieldName + ')' : '' }}
          </template>
          <template v-if="column.key === 'sourceType'">
            <jnpf-select
              v-model:value="record.sourceType"
              :options="getSourceTypeOptions(record.required)"
              class="!w-100px"
              @change="record.relationField = ''" />
          </template>
          <template v-if="column.key === 'relationField'">
            <jnpf-select
              v-model:value="record.relationField"
              placeholder="请选择"
              :options="formFieldsOptions"
              :fieldNames="{ options: 'options1' }"
              allowClear
              showSearch
              class="!w-204px"
              v-if="record.sourceType === 1" />
            <template v-else-if="record.sourceType == 2">
              <jnpf-input-number v-if="['int', 'decimal'].includes(record.dataType)" v-model:value="record.relationField" placeholder="请输入" allowClear />
              <jnpf-date-picker
                v-else-if="record.dataType == 'datetime'"
                class="!w-full"
                v-model:value="record.relationField"
                placeholder="请选择"
                format="YYYY-MM-DD HH:mm:ss"
                allowClear />
              <a-input v-else v-model:value="record.relationField" placeholder="请输入" allowClear />
            </template>
          </template>
        </template>
        <template #emptyText>
          <p class="leading-60px">暂无数据</p>
        </template>
      </a-table>
      <a-form-item label="设置列表字段" style="margin-bottom: 0" :labelCol="{ style: { width: '100px' } }"></a-form-item>
      <a-table :data-source="dataForm.columnOptions" :columns="columns" size="small" :pagination="false">
        <template #headerCell="{ column }">
          <template v-if="column.key === 'width'">{{ column.title }}<BasicHelp text="移动端不支持宽度设置" /></template>
        </template>
        <template #bodyCell="{ column, record, index }">
          <template v-if="column.key === 'label'">
            <a-input v-model:value="record.label" placeholder="请输入" />
          </template>
          <template v-if="column.key === 'value'">
            <a-auto-complete
              v-model:value="record.value"
              placeholder="请输入"
              :options="options"
              @focus="onFocus(record.value)"
              @search="debounceOnSearch(record.value)" />
          </template>
          <template v-if="column.key === 'width'">
            <a-input-number v-model:value="record.width" placeholder="请输入" :min="0" :precision="0" />
          </template>
          <template v-if="column.key === 'ifShow'">
            <a-checkbox v-model:checked="record.ifShow" />
          </template>
          <template v-if="column.key === 'action'">
            <a-button class="action-btn" type="link" color="error" @click="handleDelItem(index)" size="small">删除</a-button>
          </template>
        </template>
        <template #emptyText>
          <p class="leading-60px">暂无数据</p>
        </template>
      </a-table>
      <div class="table-add-action" @click="handleAddColumnOption()">
        <a-button type="link" preIcon="icon-ym icon-ym-btn-add">新增</a-button>
      </div>
    </a-form>
  </BasicModal>
</template>
<script lang="ts" setup>
  import { reactive, toRefs } from 'vue';
  import { sourceTypeOptions, templateJsonColumns } from '@/components/FlowProcess/src/helper/define';
  import { BasicModal, useModalInner } from '@/components/Modal';
  import { cloneDeep } from 'lodash-es';
  import { InterfaceModal } from '@/components/CommonModal';
  import { getDataInterfaceInfo } from '@/api/systemData/dataInterface';
  import { useDebounceFn } from '@vueuse/core';
  import { useMessage } from '@/hooks/web/useMessage';

  interface State {
    dataForm: any;
    formFieldsOptions: any[];
    allOptions: any[];
    options: any[];
    type: string;
  }

  const state = reactive<State>({
    dataForm: {},
    formFieldsOptions: [],
    allOptions: [],
    options: [],
    type: 'add',
  });
  const { dataForm, formFieldsOptions, options, type } = toRefs(state);
  const [registerAuxiliaryModal, { closeModal }] = useModalInner(init);
  const columns = [
    { width: 50, title: '序号', align: 'center', customRender: ({ index }) => index + 1 },
    { title: '列名', dataIndex: 'label', key: 'label', width: 150 },
    { title: '字段', dataIndex: 'value', key: 'value' },
    { title: '宽度', dataIndex: 'width', key: 'width', width: 100 },
    { title: '显示', dataIndex: 'ifShow', key: 'ifShow', width: 50, align: 'center' },
    { title: '操作', dataIndex: 'action', key: 'action', width: 50 },
  ];

  const debounceOnSearch = useDebounceFn(onSearch, 300);
  const { createMessage } = useMessage();
  const emit = defineEmits(['register', 'confirm']);

  function init(data) {
    state.type = data.type || 'add';
    state.dataForm = cloneDeep(data.config);
    state.formFieldsOptions = data.formFieldsOptions || [];
  }

  function getSourceTypeOptions(isRequired) {
    return isRequired ? sourceTypeOptions.filter(o => o.id != 3 && o.id != 4) : sourceTypeOptions.filter(o => o.id != 4);
  }
  function onInterfaceChange(val, row) {
    if (!val) {
      state.dataForm.interfaceId = '';
      state.dataForm.interfaceName = '';
      state.dataForm.templateJson = [];
      initFieldData();
      return;
    }
    if (state.dataForm.interfaceId === val) return;
    state.dataForm.interfaceId = val;
    state.dataForm.interfaceName = row.fullName;
    state.dataForm.templateJson = row.templateJson.map(o => ({ ...o, sourceType: 1 })) || [];
    initFieldData();
  }

  function onSearch(searchText: string) {
    state.options = state.allOptions.filter(o => o.value.toLowerCase().indexOf(searchText.toLowerCase()) !== -1);
  }
  function onFocus(searchText) {
    onSearch(searchText);
  }
  function initFieldData() {
    if (!state.dataForm.interfaceId) return (state.allOptions = []);
    getDataInterfaceInfo(state.dataForm.interfaceId).then(res => {
      const data = res.data;
      let list = data.fieldJson ? JSON.parse(data.fieldJson) : [];
      state.allOptions = list.map(o => ({ ...o, value: o.defaultValue }));
    });
  }
  function handleDelItem(index) {
    state.dataForm.columnOptions.splice(index, 1);
  }
  function handleAddColumnOption() {
    (state.dataForm.columnOptions as any).push({
      value: '',
      label: '',
      width: null,
      ifShow: true,
    });
  }
  function exist() {
    let isOk = true;
    for (let i = 0; i < state.dataForm.columnOptions?.length; i++) {
      const e = state.dataForm.columnOptions[i];
      if (!e.value) {
        createMessage.error('列表字段中字段不能为空');
        isOk = false;
        break;
      }
    }
    return isOk;
  }
  function handleSubmit() {
    if (!state.dataForm.interfaceId) return createMessage.warning('请选择数据接口');
    if (!exist()) return;
    emit('confirm', state.dataForm);
    closeModal();
  }
</script>
