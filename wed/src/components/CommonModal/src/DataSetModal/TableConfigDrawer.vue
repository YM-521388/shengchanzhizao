<template>
  <BasicDrawer
    v-bind="$attrs"
    width="500px"
    title="数据库表设置"
    @register="registerDrawer"
    class="dataSet-table-config-drawer"
    showFooter
    :maskClosable="false"
    @ok="handleSubmit"
    :closeFunc="handleClose"
    destroyOnClose>
    <div class="p-20px">
      <div class="common-cap !mt-0">
        <span class="title w-80px flex-shrink-0">数据库表</span>
        <div class="flex-1">
          <TableSelect :value="dataForm.table" :title="dataForm.tableName" :linkId="linkId" @change="onTableChange" />
        </div>
      </div>
      <template v-if="dataForm.table">
        <div class="common-cap">
          <span class="title">选择 {{ dataForm?.fieldList?.length || 0 }}/{{ allFieldList.length }}</span>
        </div>
        <a-table
          :data-source="allFieldList"
          :columns="fieldColumns"
          size="small"
          :pagination="false"
          :scroll="{ y: '300px' }"
          rowKey="field"
          :row-selection="{ columnWidth: 50, selectedRowKeys: selectedRowKeys, onChange: onSelectedChange }" />
        <div class="common-cap">
          <span class="title">条件筛选</span>
        </div>
        <div class="condition-main overflow-auto">
          <div class="mb-10px" v-if="dataForm.ruleList?.length">
            <jnpf-radio v-model:value="dataForm.matchLogic" :options="logicOptions" optionType="button" button-style="solid" />
          </div>
          <div class="condition-item" v-for="(item, index) in dataForm.ruleList" :key="index">
            <div class="condition-item-title">
              <div>条件组</div>
              <i class="icon-ym icon-ym-nav-close" @click="delRuleGroup(index)"></i>
            </div>
            <div class="condition-item-content">
              <div class="condition-item-cap">
                以下条件全部执行：
                <jnpf-radio v-model:value="item.logic" :options="logicOptions" optionType="button" button-style="solid" size="small" />
              </div>
              <a-row :gutter="8" v-for="(child, childIndex) in item.groups" :key="index + childIndex" wrap class="mb-10px">
                <a-col :span="18" class="!flex items-center">
                  <jnpf-select
                    v-model:value="child.field"
                    :options="dataForm.fieldList"
                    placeholder="请选择字段"
                    allowClear
                    showSearch
                    :fieldNames="{ label: 'fieldName', value: 'field' }" />
                </a-col>
                <a-col :span="6">
                  <jnpf-select class="w-full" v-model:value="child.dataType" :options="dataTypeOptions" @change="onDataTypeChange(child)" />
                </a-col>
                <a-col :span="6" class="mt-10px">
                  <jnpf-select
                    class="w-full"
                    v-model:value="child.symbol"
                    :options="child.dataType === 'text' ? baseSymbolOptions : rangeSymbolOptions"
                    @change="onSymbolChange(child)" />
                </a-col>
                <a-col :span="16" class="mt-10px" v-if="['double', 'bigint', 'date', 'time'].includes(child.dataType)">
                  <template v-if="['double', 'bigint'].includes(child.dataType)">
                    <jnpf-number-range v-model:value="child.fieldValue" v-if="child.symbol == 'between'" />
                    <jnpf-input-number v-model:value="child.fieldValue" placeholder="请输入" :disabled="['null', 'notNull'].includes(child.symbol)" v-else />
                  </template>
                  <template v-else-if="['date', 'time'].includes(child.dataType)">
                    <jnpf-date-range
                      v-model:value="child.fieldValue"
                      :format="child.dataType === 'time' ? 'YYYY-MM-DD HH:mm:ss' : 'YYYY-MM-DD'"
                      allowClear
                      v-if="child.symbol == 'between'" />
                    <jnpf-date-picker
                      v-model:value="child.fieldValue"
                      :format="child.dataType === 'time' ? 'YYYY-MM-DD HH:mm:ss' : 'YYYY-MM-DD'"
                      allowClear
                      :disabled="['null', 'notNull'].includes(child.symbol)"
                      v-else />
                  </template>
                </a-col>
                <template v-else>
                  <a-col :span="6" class="mt-10px">
                    <jnpf-select
                      v-model:value="child.fieldValueType"
                      :options="fieldValueTypeOptions"
                      @change="onFieldValueTypeChange(child)"
                      :disabled="['null', 'notNull'].includes(child.symbol)" />
                  </a-col>
                  <a-col :span="10" class="mt-10px">
                    <a-input
                      v-model:value="child.fieldValue"
                      placeholder="请输入"
                      allowClear
                      :disabled="['null', 'notNull'].includes(child.symbol)"
                      v-if="child.fieldValueType === 1" />
                    <jnpf-select v-model:value="child.fieldValue" :options="sysVariableList" allowClear showSearch placeholder="请选择系统参数" v-else />
                  </a-col>
                </template>
                <a-col :span="2" class="text-center mt-10px">
                  <i class="icon-ym icon-ym-btn-clearn" @click="delRuleItem(index, childIndex)" />
                </a-col>
              </a-row>
              <span class="link-text inline-block" @click="addRuleItem(index)"><i class="icon-ym icon-ym-btn-add text-14px mr-4px"></i>添加条件</span>
            </div>
          </div>
          <span class="link-text inline-block" @click="addRuleGroup()"><i class="icon-ym icon-ym-btn-add text-14px mr-4px"></i>添加条件组</span>
        </div>
        <template v-if="!!dataForm.parentTable">
          <div class="common-cap">
            <span class="title">数据连接</span>
          </div>
          <a-button block @click="openRelationConfig()">配置数据连接</a-button>
        </template>
      </template>
    </div>
    <RelationConfigDrawer v-bind="getConfigBind" @register="registerRelationConfigDrawer" @confirm="onRelationConfigConfirm" />
  </BasicDrawer>
</template>
<script lang="ts" setup>
  import { reactive, toRefs, computed } from 'vue';
  import { getDataModelFieldList } from '@/api/systemData/dataModel';
  import { BasicDrawer, useDrawer, useDrawerInner } from '@/components/Drawer';
  import { cloneDeep } from 'lodash-es';
  import TableSelect from './TableSelect.vue';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useI18n } from '@/hooks/web/useI18n';
  import { isEmpty } from '@/utils/is';
  import RelationConfigDrawer from './RelationConfigDrawer.vue';

  interface State {
    dataForm: any;
    isEdit: boolean;
    allFieldList: any[];
    selectedRowKeys: string[];
    oldTable: string;
  }

  const props = defineProps({
    linkId: { type: String, default: '0' },
    sysVariableList: { type: Array, default: () => [] },
    getTableConfigTree: { type: Function },
  });

  const emit = defineEmits(['register', 'confirm', 'close']);
  const fieldColumns = [
    { title: '字段名', dataIndex: 'field', key: 'field', width: 200 },
    { title: '字段描述', dataIndex: 'fieldName', key: 'fieldName' },
  ];
  const logicOptions = [
    { id: 'and', fullName: '且' },
    { id: 'or', fullName: '或' },
  ];
  const dataTypeOptions = [
    { id: 'text', fullName: 'text' },
    { id: 'double', fullName: 'double' },
    { id: 'bigint', fullName: 'bigint' },
    { id: 'date', fullName: 'date' },
    { id: 'time', fullName: 'time' },
  ];
  const baseSymbolOptions = [
    { id: '==', fullName: '等于' },
    { id: '<>', fullName: '不等于' },
    { id: 'like', fullName: '包含' },
    { id: 'notLike', fullName: '不包含' },
    { id: 'null', fullName: '为空' },
    { id: 'notNull', fullName: '不为空' },
  ];
  const rangeSymbolOptions = [
    { id: '>=', fullName: '大于等于' },
    { id: '>', fullName: '大于' },
    { id: '==', fullName: '等于' },
    { id: '<=', fullName: '小于等于' },
    { id: '<', fullName: '小于' },
    { id: '<>', fullName: '不等于' },
    { id: 'between', fullName: '介于' },
    { id: 'null', fullName: '为空' },
    { id: 'notNull', fullName: '不为空' },
  ];
  const fieldValueTypeOptions = [
    { id: 1, fullName: '固定值' },
    { id: 2, fullName: '系统参数' },
  ];
  const emptyChildItem = {
    field: '',
    dataType: 'text',
    symbol: '==',
    fieldValue: '',
    fieldValueType: 1,
  };
  const emptyItem = { logic: 'and', groups: [emptyChildItem] };
  const defaultRelationConfig = {
    ruleList: [],
    matchLogic: 'and',
    type: 1,
    relationList: [],
  };
  const defaultDataForm = {
    parentTable: '',
    table: '',
    tableName: '',
    fieldList: [],
    ruleList: [],
    matchLogic: 'and',
    relationConfig: defaultRelationConfig,
    children: [],
  };
  const state = reactive<State>({
    dataForm: {},
    isEdit: false,
    allFieldList: [],
    selectedRowKeys: [],
    oldTable: '',
  });
  const { dataForm, allFieldList, selectedRowKeys } = toRefs(state);
  const [registerDrawer, { closeDrawer }] = useDrawerInner(init);
  const [registerRelationConfigDrawer, { openDrawer: openConfigDrawer }] = useDrawer();
  const { t } = useI18n();
  const { createMessage, createConfirm } = useMessage();

  const getConfigBind = computed(() => ({ ...props }));

  function init(data) {
    state.selectedRowKeys = [];
    state.allFieldList = [];
    state.isEdit = !!data.data;
    const dataForm = data.data ? cloneDeep(data.data) : cloneDeep(defaultDataForm);
    if (!state.isEdit && data.parentTable) dataForm.parentTable = data.parentTable;
    if (state.isEdit) getTableFieldList(dataForm.table, true);
    state.oldTable = dataForm.table;
    state.dataForm = dataForm;
  }
  function onTableChange(table, item) {
    if (!table) return handleNull();
    if (state.dataForm.table !== table) handleNull();
    getTableFieldList(table, state.dataForm.table === table);
    state.dataForm.table = table;
    state.dataForm.tableName = table + (item.tableName ? `(${item.tableName})` : '');
  }
  function handleNull() {
    state.dataForm.table = '';
    state.dataForm.tableName = '';
    state.dataForm.fieldList = [];
    state.dataForm.ruleList = [];
  }
  function getTableFieldList(table, isInit = false) {
    getDataModelFieldList(props.linkId, table).then(res => {
      state.allFieldList = res.data.list;
      if (!isInit) {
        state.selectedRowKeys = state.allFieldList.map(o => o.field);
        const fieldList = state.allFieldList.map(o => ({
          field: o.field,
          fieldName: o.field + (o.fieldName ? `(${o.fieldName})` : ''),
          dataType: o.dataType,
        }));
        state.dataForm.fieldList = fieldList;
      } else {
        const fieldList = state.dataForm.fieldList.filter(o => state.allFieldList.some(e => o.field === e.field));
        state.dataForm.fieldList = fieldList;
        state.selectedRowKeys = state.dataForm.fieldList.map(o => o.field);
      }
    });
  }
  function onSelectedChange(selectedRowKeys, selectedRows) {
    state.selectedRowKeys = selectedRowKeys;
    state.dataForm.fieldList = selectedRows.map(o => ({
      field: o.field,
      fieldName: o.field + (o.fieldName ? `(${o.fieldName})` : ''),
      dataType: o.dataType,
    }));
  }
  function addRuleItem(index) {
    state.dataForm.ruleList[index].groups.push(cloneDeep(emptyChildItem));
  }
  function delRuleItem(index, childIndex) {
    state.dataForm.ruleList[index].groups.splice(childIndex, 1);
    if (!state.dataForm.ruleList[index].groups.length) delRuleGroup(index);
  }
  function addRuleGroup() {
    state.dataForm.ruleList.push(cloneDeep(emptyItem));
  }
  function delRuleGroup(index) {
    state.dataForm.ruleList.splice(index, 1);
  }
  function onDataTypeChange(item) {
    item.fieldValueType = 1;
    if (item.dataType === 'text') {
      if (!baseSymbolOptions.some(o => o.id === item.symbol)) {
        item.symbol = '==';
      }
      item.fieldValue = '';
    } else {
      if (!rangeSymbolOptions.some(o => o.id === item.symbol)) {
        item.symbol = '==';
      }
      item.fieldValue = undefined;
    }
  }
  function onSymbolChange(item) {
    if (item.dataType === 'text') {
      if (['null', 'notNull'].includes(item.symbol)) {
        item.fieldValueType = 1;
        item.fieldValue = '';
      }
    } else {
      if (['null', 'notNull'].includes(item.symbol)) {
        item.fieldValue = undefined;
      } else if (item.symbol === 'between') {
        !Array.isArray(item.fieldValue) && (item.fieldValue = []);
      } else {
        Array.isArray(item.fieldValue) && (item.fieldValue = undefined);
      }
    }
  }
  function onFieldValueTypeChange(item) {
    item.fieldValue = '';
  }
  function openRelationConfig() {
    if (!state.dataForm.fieldList.length) return createMessage.warning('请至少选择一个字段');
    openConfigDrawer(true, { ...state.dataForm });
  }
  function onRelationConfigConfirm(data) {
    state.dataForm.relationConfig = data;
  }
  function conditionExist() {
    const list = state.dataForm.ruleList;
    let isOk = true;
    outer: for (let i = 0; i < list.length; i++) {
      const e = list[i];
      for (let j = 0; j < e.groups.length; j++) {
        const child = e.groups[j];
        if (!child.field) {
          createMessage.warning(`字段不能为空`);
          isOk = false;
          break outer;
        }
        if (child.fieldValueType === 2 && !child.fieldValue) {
          createMessage.warning(`系统参数不能为空`);
          isOk = false;
          break outer;
        }
        if (
          child.fieldValueType === 1 &&
          !['null', 'notNull'].includes(child.symbol) &&
          ((!child.fieldValue && child.fieldValue !== 0) || isEmpty(child.fieldValue))
        ) {
          createMessage.warning('数据值不能为空');
          isOk = false;
          return;
        }
      }
    }
    return isOk;
  }
  function handleSubmit() {
    if (!state.dataForm.table) return createMessage.warning('请选择数据库表');
    if (state.dataForm.parentTable) {
      let allKeys = (props.getTableConfigTree as any)().getAllKeys();
      if (state.isEdit) allKeys = allKeys.filter(o => o != state.oldTable);
      if (allKeys.includes(state.dataForm.table)) return createMessage.warning('数据库表不能重复,请重新选择');
    }
    if (!state.dataForm.fieldList.length) return createMessage.warning('请至少选择一个字段');
    if (!conditionExist()) return;
    if (state.dataForm.parentTable && !state.dataForm?.relationConfig?.relationList?.length) return createMessage.warning('请至少配置一组字段关联');
    if (state.oldTable && state.dataForm.table != state.oldTable) {
      createConfirm({
        iconType: 'warning',
        title: t('common.tipTitle'),
        content: `切换数据库表将清空关联子表，确定要继续?`,
        onOk: () => {
          emit('confirm', state.dataForm, state.isEdit, true);
          closeDrawer();
        },
      });
      return;
    }
    emit('confirm', state.dataForm, state.isEdit);
    closeDrawer();
  }
  function handleClose() {
    emit('close');
    return true;
  }
</script>
<style lang="less">
  .dataSet-table-config-drawer {
    .common-cap {
      font-size: 14px;
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin: 15px 0;
      .title {
        font-size: 16px;
        overflow: hidden;
        text-overflow: ellipsis;
        white-space: nowrap;
      }
    }
  }
  html[data-theme='dark'] {
    .dataSet-table-config-drawer {
      .common-cap {
        .title {
          color: #fff !important;
        }
      }
      .condition-item-title,
      .condition-item-cap {
        color: #fff !important;
      }
    }
  }
</style>
