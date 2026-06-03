<template>
  <a-form-item label="左父格">
    <a-radio-group v-model:value="dataSource.leftParentCellType" class="right-radio" @change="onParentCellType('left')">
      <a-radio v-for="option in cellTypeOptions" :value="option.id"
        >{{ option.fullName }}<BasicHelp v-if="option.id === 'custom'" text="表格位置发生变化后，父格需重新设置"
      /></a-radio>
    </a-radio-group>
  </a-form-item>
  <a-form-item v-if="dataSource.leftParentCellType === 'custom'" class="-mt-14px">
    <a-input-group compact>
      <a-select class="!w-1/3" v-model:value="dataSource.leftParentCellCustomRowName">
        <a-select-option v-for="item in getCustomRowNameList" :key="item" :value="item">{{ item }}</a-select-option>
      </a-select>
      <a-input-number class="!w-1/2" :min="1" :max="getMaxRows" v-model:value="dataSource.leftParentCellCustomColName" />
      <a-button class="!w-1/6" pre-icon="icon-ym icon-ym-select-cell" @click="openCellByDialogSelect('left')"></a-button>
    </a-input-group>
  </a-form-item>
  <a-form-item label="上父格">
    <a-radio-group v-model:value="dataSource.topParentCellType" class="right-radio" @change="onParentCellType('top')">
      <a-radio v-for="option in cellTypeOptions" :value="option.id"
        >{{ option.fullName }}<BasicHelp v-if="option.id === 'custom'" text="表格位置发生变化后，父格需重新设置"
      /></a-radio>
    </a-radio-group>
  </a-form-item>
  <a-form-item v-if="dataSource.topParentCellType === 'custom'" class="-mt-14px">
    <a-input-group compact>
      <a-select class="!w-1/3" v-model:value="dataSource.topParentCellCustomRowName">
        <a-select-option v-for="item in getCustomRowNameList" :key="item" :value="item">{{ item }}</a-select-option>
      </a-select>
      <a-input-number class="!w-1/2" :min="1" :max="getMaxRows" v-model:value="dataSource.topParentCellCustomColName" />
      <a-button class="!w-1/6" pre-icon="icon-ym icon-ym-select-cell" @click="openCellByDialogSelect('top')"></a-button>
    </a-input-group>
  </a-form-item>
  <a-form-item label="字段名称">
    <jnpf-tree-select v-model:value="dataSource.field" lastLevel lastLevelKey="children" :options="dataSetList" :fieldNames="{ value: 'jnpfId' }" />
  </a-form-item>
  <a-form-item label="聚合方式">
    <jnpf-radio
      v-model:value="dataSource.polymerizationType"
      :options="polymerizationTypeOptions"
      optionType="button"
      buttonStyle="solid"
      class="right-radio-more" />
  </a-form-item>
  <a-form-item v-if="dataSource.polymerizationType === '3'" label="汇总方式">
    <jnpf-select v-model:value="dataSource.summaryType" :options="summaryTypeOptions" />
  </a-form-item>
  <a-form-item v-else label="扩展方向">
    <jnpf-radio v-model:value="dataSource.fillDirection" :options="textAutoplayModeList" optionType="button" buttonStyle="solid" class="right-radio-more" />
  </a-form-item>
</template>

<script setup lang="ts">
  import { reactive, watch } from 'vue';
  import { getAlphabetFromIndexRule } from '../helper';
  const props = defineProps([
    'dataSource',
    'dataSetList',
    'queryList',
    'getCustomRowNameList',
    'getMaxRows',
    'jnpfUniverRef',
    'cellFromDialogValue',
    'startColumn',
    'startRow',
  ]);
  interface State {
    type: string;
  }

  const state = reactive<State>({
    type: '',
  });
  const textAutoplayModeList = [
    { fullName: '纵向', id: 'portrait' },
    { fullName: '横向', id: 'landscape' },
  ];
  const summaryTypeOptions = [
    { id: 'none', fullName: '无' },
    { id: 'sum', fullName: '合计' },
    { id: 'max', fullName: '最大值' },
    { id: 'min', fullName: '最小值' },
    { id: 'count', fullName: '计数' },
    { id: 'avg', fullName: '平均值' },
  ];
  const polymerizationTypeOptions = [
    { id: '1', fullName: '列表' },
    { id: '2', fullName: '分组' },
    { id: '3', fullName: '汇总' },
  ];
  const cellTypeOptions = [
    { id: 'none', fullName: '无' },
    { id: 'default', fullName: '默认' },
    { id: 'custom', fullName: '自定义' },
  ];

  watch(
    () => props.cellFromDialogValue,
    value => {
      if (!value) return;
      let str = value.match(/[a-zA-Z]+/)[0];
      let str1 = value.split(str);
      props.dataSource[`${state.type}ParentCellCustomRowName`] = str;
      props.dataSource[`${state.type}ParentCellCustomColName`] = str1[1];
    },
  );

  function openCellByDialogSelect(type?) {
    state.type = type || '';
    let focusedCell = {
      startColumn: props.startColumn,
      startRow: props.startRow,
    };
    props.jnpfUniverRef.value?.getCellFromDialogSelect(focusedCell);
  }
  function onParentCellType(type) {
    let customRowName = `${type}ParentCellCustomRowName`;
    let customColName = `${type}ParentCellCustomColName`;
    if (props.dataSource[`${type}ParentCellType`] == 'custom') {
      let startColumn = type == 'top' ? (props.startRow === 0 ? 0 : props.startColumn) : props.startColumn === 0 ? 0 : props.startColumn - 1;
      let startRow = type == 'top' ? (props.startRow === 0 ? 1 : props.startRow) : props.startColumn == 0 ? 1 : props.startRow + 1;
      props.dataSource[customRowName] = props.dataSource[customRowName] || getAlphabetFromIndexRule(startColumn);
      props.dataSource[customColName] = props.dataSource[customColName] || startRow;
    }
  }
</script>

<style lang="less" scoped>
  .right-radio .ant-radio-wrapper {
    margin-inline-end: 6px;
  }
</style>
