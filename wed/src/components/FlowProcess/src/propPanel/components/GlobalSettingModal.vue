<template>
  <BasicModal v-bind="$attrs" width="1000px" class="jnpf-global-setting-modal" @register="registerModal" title="全局设置" @ok="handleSubmit" destroyOnClose>
    <div class="global-setting">
      <a-tabs v-model:activeKey="activeKey" tab-position="left" class="common-left-tabs flow-left-tabs">
        <a-tab-pane :key="0" tab="全局变量"></a-tab-pane>
        <a-tab-pane :key="1" tab="审批字段"></a-tab-pane>
      </a-tabs>
      <div class="table-box">
        <template v-if="activeKey == 0">
          <a-alert message="可通过节点属性给全局变量赋值" type="warning" show-icon />
          <a-table :data-source="globalParameterList" :columns="globalParameterColumns" size="small" :pagination="false" class="mt-10px">
            <template #bodyCell="{ column, record, index }">
              <template v-if="column.key === 'dataType'">
                <jnpf-select v-model:value="record.dataType" :options="dataTypeOptions" />
              </template>
              <template v-if="column.key === 'fieldName'">
                <a-input v-model:value="record.fieldName" placeholder="请输入" :maxlength="50" />
              </template>
              <template v-if="column.key === 'defaultValue'">
                <jnpf-input-number v-if="['int', 'decimal'].includes(record.dataType)" v-model:value="record.defaultValue" placeholder="请输入" />
                <jnpf-date-picker v-else-if="record.dataType === 'datetime'" v-model:value="record.defaultValue" class="w-full" placeholder="请选择" />
                <jnpf-input v-else v-model:value="record.defaultValue" placeholder="请输入" />
              </template>
              <template v-if="column.key === 'action'">
                <a-button type="link" danger @click="handleDel(index)"><i class="icon-ym icon-ym-app-delete"></i></a-button>
              </template>
            </template>
          </a-table>
          <span class="link-text inline-block mt-10px" @click="handleAdd()"><i class="icon-ym icon-ym-btn-add text-14px mr-4px"></i>新增变量</span>
        </template>
        <template v-else>
          <a-alert message="用于流程审批时自定义审批拓展字段，可在节点属性中启用字段。" type="warning" show-icon />
          <a-table :data-source="approvalFieldList" :columns="approvalFieldColumns" size="small" :pagination="false" class="mt-10px">
            <template #bodyCell="{ column, record, index }">
              <template v-if="column.key === 'jnpfKey'">
                <jnpf-select v-model:value="record.jnpfKey" :options="jnpfKeyOptions" />
              </template>
              <template v-if="column.key === 'fieldName'">
                <a-input v-model:value="record.fieldName" placeholder="请输入" :maxlength="50" />
              </template>
              <template v-if="column.key === 'action'">
                <a-button type="link" danger @click="handleDel(index)"><i class="icon-ym icon-ym-app-delete"></i></a-button>
              </template>
            </template>
          </a-table>
          <span class="link-text inline-block mt-10px" @click="handleAdd()"><i class="icon-ym icon-ym-btn-add text-14px mr-4px"></i>新增字段</span>
        </template>
      </div>
    </div>
  </BasicModal>
</template>
<script lang="ts" setup>
  import { ref } from 'vue';
  import { BasicModal, useModalInner } from '@/components/Modal';
  import { buildUUID } from '@/utils/uuid';
  import { useMessage } from '@/hooks/web/useMessage';
  import { cloneDeep } from 'lodash-es';

  const globalParameterList = ref<any[]>([]);
  const approvalFieldList = ref<any[]>([]);
  const activeKey = ref<number>(0);
  const emit = defineEmits(['register', 'confirm']);
  const { createMessage } = useMessage();
  const [registerModal, { closeModal }] = useModalInner(init);
  const globalParameterColumns = [
    { title: '变量类型', dataIndex: 'dataType', key: 'dataType', width: 220 },
    { title: '变量名称', dataIndex: 'fieldName', key: 'fieldName', width: 250 },
    { title: '默认值', dataIndex: 'defaultValue', key: 'defaultValue', width: 280 },
    { title: '操作', dataIndex: 'action', key: 'action', width: 50, align: 'center' },
  ];
  const approvalFieldColumns = [
    { title: '字段名称', dataIndex: 'fieldName', key: 'fieldName', width: 450 },
    { title: '控件类型', dataIndex: 'jnpfKey', key: 'jnpfKey', width: 300 },
    { title: '操作', dataIndex: 'action', key: 'action', width: 50, align: 'center' },
  ];
  const dataTypeOptions = [
    { fullName: '字符串', id: 'varchar' },
    { fullName: '整型', id: 'int' },
    { fullName: '日期时间', id: 'datetime' },
    { fullName: '浮点', id: 'decimal' },
  ];
  const jnpfKeyOptions = [
    { fullName: '单行输入', id: 'input' },
    { fullName: '多行输入', id: 'textarea' },
    { fullName: '数字输入', id: 'inputNumber' },
  ];

  function init(data) {
    globalParameterList.value = cloneDeep(data.globalParameterList) || [];
    approvalFieldList.value = cloneDeep(data.approvalFieldList) || [];
  }
  function handleAdd() {
    if (activeKey.value == 0) {
      globalParameterList.value.push({ dataType: 'varchar', fieldName: getFieldName(), id: buildUUID() });
    } else {
      approvalFieldList.value.push({ jnpfKey: 'input', fieldName: '', id: buildUUID() });
    }
  }
  function handleDel(index) {
    let list = activeKey.value == 0 ? globalParameterList.value : approvalFieldList.value;
    list.splice(index, 1);
  }
  // 生成参数名称
  function getFieldName() {
    if (!globalParameterList.value.length) return 'flow_var1';
    let maxNumber = -1;
    const regex = /flow_var(\d+)/;
    globalParameterList.value.map(o => {
      const match = o.fieldName.match(regex);
      if (match) {
        const number = parseInt(match[1]);
        if (number > maxNumber) maxNumber = number;
      }
    });
    return 'flow_var' + ++maxNumber;
  }
  async function handleSubmit() {
    for (let i = 0; i < globalParameterList.value.length; i++) {
      if (!globalParameterList.value[i].fieldName) return createMessage.warning('变量名称不能为空');
      if (globalParameterList.value.find((o, index) => o?.fieldName == globalParameterList.value[i]?.fieldName && index != i))
        return createMessage.warning('变量名称不能重复');
    }
    for (let i = 0; i < approvalFieldList.value.length; i++) {
      if (!approvalFieldList.value[i]?.fieldName) return createMessage.warning('字段名称不能为空');
      if (approvalFieldList.value.find((o, index) => o?.fieldName == approvalFieldList.value[i]?.fieldName && index != i))
        return createMessage.warning('字段名称不能重复');
    }
    emit('confirm', { globalParameterList: globalParameterList.value, approvalFieldList: approvalFieldList.value });
    closeModal();
  }
</script>
<style lang="less">
  .jnpf-global-setting-modal {
    .ant-modal-body > .scrollbar {
      padding: 0;
      height: 60vh;
      .ant-tabs.common-left-tabs {
        height: 60vh;
        display: flex;
      }
      .global-setting {
        display: flex;
        height: 60vh;
        .table-box {
          padding: 20px 20px 20px 10px;
          flex: 1;
          overflow: auto;
        }
      }
    }
  }
</style>
