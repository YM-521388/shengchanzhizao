<template>
  <a-form-item label="左父格">
    <a-radio-group v-model:value="text.leftParentCellType" class="right-radio" @change="onParentCellType('left')">
      <a-radio v-for="option in cellTypeOptions" :value="option.id"
        >{{ option.fullName }}<BasicHelp v-if="option.id === 'custom'" text="表格位置发生变化后，父格需重新设置"
      /></a-radio>
    </a-radio-group>
  </a-form-item>
  <a-form-item v-if="text.leftParentCellType === 'custom'" class="-mt-14px">
    <a-input-group compact>
      <a-select class="!w-1/3" v-model:value="text.leftParentCellCustomRowName">
        <a-select-option v-for="item in getCustomRowNameList" :key="item" :value="item">{{ item }}</a-select-option>
      </a-select>
      <a-input-number class="!w-1/2" :min="1" :max="getMaxRows" v-model:value="text.leftParentCellCustomColName" />
      <a-button class="!w-1/6" pre-icon="icon-ym icon-ym-select-cell" @click="openCellByDialogSelect('left')"></a-button>
    </a-input-group>
  </a-form-item>
  <a-form-item label="上父格">
    <a-radio-group v-model:value="text.topParentCellType" class="right-radio" @change="onParentCellType('top')">
      <a-radio v-for="option in cellTypeOptions" :value="option.id"
        >{{ option.fullName }}<BasicHelp v-if="option.id === 'custom'" text="表格位置发生变化后，父格需重新设置"
      /></a-radio>
    </a-radio-group>
  </a-form-item>
  <a-form-item v-if="text.topParentCellType === 'custom'" class="-mt-14px">
    <a-input-group compact>
      <a-select class="!w-1/3" v-model:value="text.topParentCellCustomRowName">
        <a-select-option v-for="item in getCustomRowNameList" :key="item" :value="item">{{ item }}</a-select-option>
      </a-select>
      <a-input-number class="!w-1/2" :min="1" :max="getMaxRows" v-model:value="text.topParentCellCustomColName" />
      <a-button class="!w-1/6" pre-icon="icon-ym icon-ym-select-cell" @click="openCellByDialogSelect('top')"></a-button>
    </a-input-group>
  </a-form-item>
</template>

<script setup lang="ts">
  import { reactive, watch } from 'vue';
  import { getAlphabetFromIndexRule } from '../helper';

  const props = defineProps([
    'text',
    'getCustomRowNameList',
    'getMaxRows',
    'jnpfUniverRef',
    'cellFromDialogValue',
    'startColumn',
    'startRow',
    'dataSetList',
    'queryList',
  ]);

  interface State {
    type: string;
  }

  const state = reactive<State>({
    type: '',
  });
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
      props.text[`${state.type}ParentCellCustomRowName`] = str;
      props.text[`${state.type}ParentCellCustomColName`] = str1[1];
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
    if (props.text[`${type}ParentCellType`] == 'custom') {
      let startColumn = type == 'top' ? (props.startRow === 0 ? 0 : props.startColumn) : props.startColumn === 0 ? 0 : props.startColumn - 1;
      let startRow = type == 'top' ? (props.startRow === 0 ? 1 : props.startRow) : props.startColumn == 0 ? 1 : props.startRow + 1;
      props.text[customRowName] = props.text[customRowName] || getAlphabetFromIndexRule(startColumn);
      props.text[customColName] = props.text[customColName] || startRow;
    }
  }
</script>

<style lang="less" scoped>
  .right-radio .ant-radio-wrapper {
    margin-inline-end: 6px;
  }
</style>
