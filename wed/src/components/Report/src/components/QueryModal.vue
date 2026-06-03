<template>
  <BasicModal v-bind="$attrs" width="1200px" class="jnpf-flow-parameter-modal" @register="registerModal" title="查询配置" @ok="handleSubmit" destroyOnClose>
    <a-tabs v-model:activeKey="activeKey" class="pane-tabs approver-pane-tabs" @change="onChange">
      <a-tab-pane v-for="item in queryList" :key="item.sheet">
        <template #tab>{{ item.sheetName }}</template>
        <a-table
          :data-source="item.queryList"
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
                class="!w-270px"
                @change="(value:any, option:any) => handleChangeField(value, option, record, index)" />
            </template>
            <template v-if="column.key === 'label'">
              <a-input v-model:value="record.label" placeholder="请输入" allowClear :maxlength="50" />
            </template>
            <template v-if="column.key === 'type'">
              <JnpfSelect v-model:value="record.type" placeholder="请选择" :options="typeList" class="!w-150px" @change="onTypeChange($event, record, index)" />
              <template v-if="canSetAttrs.includes(record.type)">
                <i class="icon-ym icon-ym-shezhi cursor-pointer ml-8px leading-30px" title="配置" @click="openExtraConfig(record, index)" />
              </template>
            </template>
            <template v-if="column.key === 'searchType'">
              <JnpfSelect v-model:value="record.searchType" :options="searchTypeOptions" :disabled="!['input'].includes(record.type)" />
            </template>
            <template v-if="column.key === 'searchMultiple'">
              <a-checkbox
                v-model:checked="record.searchMultiple"
                :disabled="!multipleList.includes(record.type)"
                @change="onSearchMultipleChange(record, index)" />
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
    <ExtraConfigModal @register="registerExtraConfigModal" @confirm="updateRow" />
  </BasicModal>
</template>
<script lang="ts" setup>
  import { computed, nextTick, reactive, toRefs } from 'vue';
  import Sortablejs from 'sortablejs';
  import { cloneDeep } from 'lodash-es';
  import { buildUUID } from '@/utils/uuid';
  import { BasicModal, useModal, useModalInner } from '@/components/Modal';
  import ExtraConfigModal from '@/components/PrintDesign/PrintDesign/ExtraConfigModal.vue';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useInputList, useDateList } from '@/components/FormGenerator/src/helper/config';

  const emit = defineEmits(['register', 'confirm']);

  interface State {
    queryList: any[];
    list: any[];
    fieldOptions: any[];
    activeIndex: number;
    activeKey: String;
    sheetList: any[];
    sheetIndex: number;
  }

  const state = reactive<State>({
    queryList: [],
    list: [],
    fieldOptions: [],
    activeIndex: 0,
    activeKey: '',
    sheetList: [],
    sheetIndex: 0,
  });
  const { queryList, fieldOptions, activeKey } = toRefs(state);
  const [registerModal, { closeModal }] = useModalInner(init);
  const [registerExtraConfigModal, { openModal: openExtraConfigModal }] = useModal();
  const { createMessage } = useMessage();

  const canSetAttrs = ['select', 'date', 'time', 'organizeSelect', 'depSelect', 'userSelect'];
  const multipleList = ['select', 'depSelect', 'roleSelect', 'userSelect', 'usersSelect', 'organizeSelect', 'posSelect', 'groupSelect'];
  const columns = [
    { title: '拖动', dataIndex: 'drag', key: 'drag', align: 'center', width: 50 },
    { title: '字段', dataIndex: 'vModel', key: 'vModel', width: 270 },
    { title: '列名', dataIndex: 'label', key: 'label', width: 200 },
    { title: '输入类型', dataIndex: 'type', key: 'type', width: 200 },
    { title: '条件类型', dataIndex: 'searchType', key: 'searchType', width: 200 },
    { title: '是否多选', dataIndex: 'searchMultiple', key: 'searchMultiple', width: 80, align: 'center' },
    { title: '操作', dataIndex: 'action', key: 'action', width: 80, align: 'center' },
  ];
  const searchTypeOptions = [
    { id: 1, fullName: '等于查询' },
    { id: 2, fullName: '模糊查询' },
    { id: 3, fullName: '范围查询' },
  ];
  const typeList = [
    { id: 'input', fullName: '单行输入' },
    { id: 'inputNumber', fullName: '数字输入' },
    { id: 'select', fullName: '下拉选择' },
    { id: 'date', fullName: '日期选择' },
    { id: 'time', fullName: '时间选择' },
    { id: 'organizeSelect', fullName: '组织选择' },
    { id: 'depSelect', fullName: '部门选择' },
    { id: 'roleSelect', fullName: '角色选择' },
    { id: 'posSelect', fullName: '岗位选择' },
    { id: 'groupSelect', fullName: '分组选择' },
    { id: 'userSelect', fullName: '用户选择' },
  ];

  const getSearchType = type => {
    // 等于-1  模糊-2  范围-3
    const fuzzyList = [...useInputList];
    const RangeList = [...useDateList, 'time', 'date', 'inputNumber', 'calculate', 'rate', 'slider'];
    if (RangeList.includes(type)) return 3;
    if (fuzzyList.includes(type)) return 2;
    return 1;
  };
  const getSearchMultiple = type => {
    const searchMultipleList = ['select', 'depSelect', 'roleSelect', 'userSelect', 'usersSelect', 'organizeSelect', 'posSelect', 'groupSelect'];
    return searchMultipleList.includes(type);
  };
  const getDefaultValue = item => {
    const type = item.type;
    const list = ['areaSelect', 'timePicker', 'datePicker', 'inputNumber', 'organizeSelect', 'calculate'];
    return list.includes(type) || item.multiple ? [] : undefined;
  };

  const getParameterColumns = computed(() => columns.map(o => ({ ...o, customCell: () => ({ class: 'align-top' }) })));

  function onChange(id: any) {
    state.sheetIndex = state.queryList.findIndex(item => item.sheet === id);
    nextTick(() => {
      initSortable();
    });
  }

  function init(data: any) {
    state.queryList = getQueryList(data);
    state.activeKey = state.queryList[0].sheet;
    state.fieldOptions = cloneDeep(data.dataSetList ?? []);
    state.sheetIndex = 0;
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
        const currRow = state.queryList[state.sheetIndex].queryList.splice(oldIndex, 1)[0];
        state.queryList[state.sheetIndex].queryList.splice(newIndex, 0, currRow);
      },
    });
  }
  function getQueryList(data) {
    const sheetList = data.sheetList.map(item => ({ sheet: item.id, sheetName: item.fullName, queryList: [] }));
    if (!data.queryList?.length) return sheetList;
    for (let index = 0; index < sheetList.length; index++) {
      const element = sheetList[index];
      for (let i = 0; i < data.queryList.length; i++) {
        const el = data.queryList[i];
        if (element.sheet == el.sheet) sheetList[index].queryList = el.queryList || [];
      }
    }
    return sheetList;
  }
  function handleAdd() {
    state.queryList[state.sheetIndex].queryList.push({
      id: buildUUID(),
      vModel: '',
      label: '',
      type: 'input',
      searchType: 2,
      config: { dataType: 'static', options: [], dictionaryType: '', propsValue: 'id', format: 'yyyy-MM-dd', precision: 0, thousands: false },
    });
  }
  function onSearchMultipleChange(record, index) {
    state.list[index].value = getDefaultValue(record);
  }
  function onTypeChange(val, record, i) {
    let defaultItem: any = {
      id: record.id,
      vModel: record.vModel,
      fullName: record.fullName,
      label: record.label,
      prop: record.prop,
      type: val,
      searchType: getSearchType(val),
      searchMultiple: getSearchMultiple(val),
      config: {},
    };
    if (val === 'date') defaultItem.config.format = 'yyyy-MM-dd';
    if (val === 'time') defaultItem.config.format = 'HH:mm:ss';
    if (val === 'select') {
      defaultItem.options = [];
      defaultItem.props = { label: 'fullName', value: 'id' };
      defaultItem.config = {
        dataType: 'static',
        propsUrl: '',
        propsName: '',
        dictionaryType: '',
        options: [],
        propsValue: 'id',
      };
    }
    if (['organizeSelect', 'depSelect', 'userSelect'].includes(val)) {
      defaultItem.isIncludeSubordinate = false;
    }
    state.queryList[state.sheetIndex].queryList[i] = cloneDeep(defaultItem);
  }
  function handleDel(index: number) {
    state.queryList[state.sheetIndex].queryList.splice(index, 1);
  }
  function openExtraConfig(record: any, index: number) {
    state.activeIndex = index;
    openExtraConfigModal(true, { ...record });
  }
  function updateRow(data: any) {
    state.queryList[state.sheetIndex].queryList[state.activeIndex] = data;
  }
  function handleChangeField(_: any, option: any, record: any, index: number) {
    const { label } = record;
    if (label !== '') {
      return;
    }
    const newRecord = {
      ...record,
      label: option?.fullName ?? '',
    };
    state.queryList[state.sheetIndex].queryList[index] = { ...newRecord };
  }
  function handleSubmit() {
    const fileNames: any = [];

    for (let index = 0; index < state.queryList.length; index++) {
      const element = state.queryList[index];

      for (let i = 0; i < element.queryList.length; i++) {
        const item = element.queryList[i];
        if (!item.vModel) {
          createMessage.warning(element.sheetName + `第${i + 1}项的字段不能为空`);
          return;
        }
        if (!item.label) {
          createMessage.warning(element.sheetName + `第${i + 1}项的列名不能为空`);
          return;
        }

        fileNames.push({ vModel: item.vModel, id: element.sheet });

        let num = fileNames.filter(o => o.vModel == item.vModel && element.sheet == o.id);

        if (num.length > 1) {
          createMessage.warning(element.sheetName + `第${i + 1}项的字段重复`);
          return;
        }
      }
    }
    emit('confirm', state.queryList);
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
        }
      }
    }
    .approver-pane-tabs {
      .ant-tabs-tab + .ant-tabs-tab {
        margin-left: 10px !important;
        padding: 0 20px;
      }
    }
  }
</style>
