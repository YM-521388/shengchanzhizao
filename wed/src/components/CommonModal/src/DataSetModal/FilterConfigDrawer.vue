<template>
  <BasicDrawer
    v-bind="$attrs"
    width="500px"
    @register="registerDrawer"
    title="筛选设置"
    class="dataSet-table-config-drawer"
    showFooter
    :maskClosable="false"
    @ok="handleSubmit"
    destroyOnClose>
    <div class="p-20px">
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
                <jnpf-select v-model:value="child.field" :options="fieldList" placeholder="请选择字段" allowClear showSearch />
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
    </div>
  </BasicDrawer>
</template>
<script lang="ts" setup>
  import { reactive, toRefs } from 'vue';
  import { BasicDrawer, useDrawerInner } from '@/components/Drawer';
  import { cloneDeep } from 'lodash-es';
  import { useMessage } from '@/hooks/web/useMessage';
  import { isEmpty } from '@/utils/is';
  import { treeToList } from '@/utils/helper/treeHelper';

  interface State {
    dataForm: any;
    fieldList: any[];
  }

  defineProps({
    sysVariableList: { type: Array, default: () => [] },
  });

  const emit = defineEmits(['register', 'confirm']);
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

  const state = reactive<State>({
    dataForm: {
      ruleList: [],
    },
    fieldList: [],
  });
  const { dataForm, fieldList } = toRefs(state);
  const [registerDrawer, { closeDrawer }] = useDrawerInner(init);
  const { createMessage } = useMessage();

  function init(data) {
    getTableFieldList(data.visualConfigJson);
    state.dataForm = cloneDeep(data.data);
  }

  function getTableFieldList(treeList = []) {
    const list = treeToList(treeList, { id: 'table' });
    state.fieldList = list.map(o => ({
      id: o.table,
      fullName: o.tableName,
      options: o.fieldList.map(c => ({ id: o.table + '-' + c.field, fullName: c.fieldName, dataType: c.dataType })),
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
    if (!conditionExist()) return;
    emit('confirm', state.dataForm);
    closeDrawer();
  }
</script>
<style lang="less">
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
