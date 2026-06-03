<template>
  <BasicModal v-bind="$attrs" width="1000px" class="jnpf-flow-parameter-modal" @register="registerModal" title="排序配置" @ok="handleSubmit" destroyOnClose>
    <a-tabs v-model:activeKey="activeKey" class="pane-tabs approver-pane-tabs" @change="onChange">
      <a-tab-pane v-for="item in sortList" :key="item.sheet">
        <template #tab>{{ item.sheetName }}</template>
        <a-table
          :data-source="item.sortList"
          rowKey="id"
          :columns="getParameterColumns"
          size="small"
          :pagination="false"
           class="mt-10px"
          :class="'columnTable' + activeKey"
          v-if="item.sheet == activeKey">
          <template #bodyCell="{ column, record, index }">
            <template v-if="column.key === 'drag'">
              <i class="drag-handler icon-ym icon-ym-darg" title="点击拖动" />
            </template>
            <template v-if="column.key === 'vModel'">
              <JnpfTreeSelect
                v-model:value="record.vModel"
                placeholder="请选择"
                allowClear
                lastLevel
                lastLevelKey="children"
                :options="fieldOptions"
                :fieldNames="{ value: 'jnpfId' }"
                class="!w-254px" />
            </template>
            <template v-if="column.key === 'type'">
              <jnpf-radio v-model:value="record.type" :options="sortTypeOptions"  buttonStyle="solid" class="!w-160px" />
            </template>
            <template v-if="column.key === 'action'">
              <a-button type="link" danger @click="handleDel(index)"><i class="icon-ym icon-ym-app-delete"></i></a-button>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
    </a-tabs>
    <div class="table-add-action" @click="handleAdd">
      <a-button type="link" preIcon="icon-ym icon-ym-btn-add">新增</a-button>
    </div>
  </BasicModal>
</template>
<script lang="ts" setup>
  import { computed, nextTick, reactive, toRefs } from 'vue';
  import Sortablejs from 'sortablejs';
  import { cloneDeep } from 'lodash-es';
  import { buildUUID } from '@/utils/uuid';
  import { BasicModal, useModalInner } from '@/components/Modal';
  import { useMessage } from '@/hooks/web/useMessage';

  const emit = defineEmits(['register', 'confirm']);

  interface State {
    sortList: any[];
    fieldOptions: any[];
    activeKey: String;
    sheetIndex: number;
  }

  const state = reactive<State>({
    sortList: [],
    fieldOptions: [],
    activeKey: '',
    sheetIndex: 0,
  });
  const { sortList, fieldOptions, activeKey } = toRefs(state);
  const [registerModal, { closeModal }] = useModalInner(init);
  const { createMessage } = useMessage();

  const columns = [
    { title: '拖动', dataIndex: 'drag', key: 'drag', align: 'center', width: 50 },
    { title: '字段', dataIndex: 'vModel', key: 'vModel', width: 270 },
    { title: '类型', dataIndex: 'type', key: 'type', width: 100 },
    { title: '操作', dataIndex: 'action', key: 'action', width: 80, align: 'center' },
  ];
  const sortTypeOptions = [
    { id: 'asc', fullName: '升序' },
    { id: 'desc', fullName: '降序' },
  ];

  const getParameterColumns = computed(() => columns.map(o => ({ ...o, customCell: () => ({ class: 'align-top' }) })));

  function onChange(id: string) {
    state.sheetIndex = state.sortList.findIndex(item => item.sheet === id);
    nextTick(() => {
      initSortable();
    });
  }

  function init(data: any) {
    state.sortList = getSortList(data);
    state.activeKey = state.sortList[0].sheet;
    state.fieldOptions = cloneDeep(data.dataSetList ?? []);
    nextTick(() => {
      initSortable();
    });
  }

  function initSortable() {
    const columnTable: any = document.querySelector(`.columnTable${state.activeKey} .ant-table-tbody`);
    Sortablejs.create(columnTable, {
      handle: '.drag-handler',
      animation: 150,
      easing: 'cubic-bezier(1, 0, 0, 1)',
      onStart: () => {},
      onEnd: ({ newIndex, oldIndex }: any) => {
        const currRow = state.sortList[state.sheetIndex].sortList.splice(oldIndex, 1)[0];
        state.sortList[state.sheetIndex].sortList.splice(newIndex, 0, currRow);
      },
    });
  }
  function getSortList(data) {
    const sheetList = data.sheetList.map(item => ({ sheet: item.id, sheetName: item.fullName, sortList: [] }));
    if (!data.sortList?.length) return sheetList;
    for (let index = 0; index < sheetList.length; index++) {
      const element = sheetList[index];
      for (let i = 0; i < data.sortList.length; i++) {
        const el = data.sortList[i];
        if (element.sheet == el.sheet) sheetList[index].sortList = el.sortList || [];
      }
    }
    return sheetList;
  }
  function handleAdd() {
    state.sortList[state.sheetIndex].sortList.push({
      id: buildUUID(),
      vModel: '',
      type: 'asc',
    });
  }

  function handleDel(index: number) {
    state.sortList[state.sheetIndex].sortList.splice(index, 1);
  }

  function handleSubmit() {
    for (let index = 0; index < state.sortList.length; index++) {
      const element = state.sortList[index];

      for (let i = 0; i < element.sortList.length; i++) {
        const item = element.sortList[i];
        if (!item.vModel) {
          createMessage.warning(element.sheetName + `第${i + 1}项的字段不能为空`);
          return;
        }
      }
    }
    emit('confirm', state.sortList);
    nextTick(() => closeModal());
  }
</script>
<style lang="less">
  .jnpf-flow-parameter-modal {
    .ant-modal-body > .scrollbar {
      padding: 0;
      height: 50vh;
    }
    .pane-tabs {
      flex-shrink: 0;
      .ant-tabs-nav {
        margin-bottom: 0;
        padding-left: 10px;
        .ant-tabs-tab + .ant-tabs-tab {
          margin-left: 35px;
          padding: 0 20px
        }
      }
    }
    .approver-pane-tabs {
      .ant-tabs-tab + .ant-tabs-tab {
        margin-left: 10px !important;
      }
    }
  }
</style>
