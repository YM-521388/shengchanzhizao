<template>
  <div class="print-render-form-content">
    <a-form layout="vertical">
      <a-form-item label="纸张类型">
        <a-select v-model:value="formData.paperType" :options="jnpfPaperTypeOptions" :field-names="{ label: 'fullName', value: 'id' }" />
      </a-form-item>
    </a-form>
    <a-form layout="vertical">
      <a-form-item label="纸张边距">
        <a-select v-model:value="formData.padding" :options="jnpfPaperPaddingTypeOptions" :field-names="{ label: 'fullName', value: 'id' }" />
      </a-form-item>
    </a-form>
    <a-form layout="vertical">
      <a-form-item label="纸张方向">
        <a-radio-group
          v-model:value="formData.direction"
          :options="getRadioGroupOptions(jnpfPrintDirectionOptions)"
          option-type="button"
          button-style="solid" />
      </a-form-item>
    </a-form>
    <a-form layout="vertical">
      <a-form-item label="页面缩放">
        <a-select v-model:value="formData.printScale" :options="jnpfPrintScaleOptions" :field-names="{ label: 'fullName', value: 'id' }" />
      </a-form-item>
    </a-form>
    <a-form layout="vertical">
      <a-form-item label="左右对齐">
        <a-radio-group v-model:value="formData.hAlign" :options="getRadioGroupOptions(jnpfPrintHAlignOptions)" option-type="button" button-style="solid" />
      </a-form-item>
    </a-form>
    <a-form layout="vertical">
      <a-form-item label="上下对齐">
        <a-radio-group v-model:value="formData.vAlign" :options="getRadioGroupOptions(jnpfPrintVAlignOptions)" option-type="button" button-style="solid" />
      </a-form-item>
    </a-form>
    <a-form layout="vertical">
      <a-form-item label="网格线">
        <a-switch v-model:checked="formData.gridlines" />
      </a-form-item>
    </a-form>
    <a-form layout="vertical" style="display: flex">
      <div style="flex: 1">
        <a-form-item label="报表名称">
          <a-switch v-model:checked="formData.workbookTitle" />
        </a-form-item>
      </div>
      <div style="flex: 1">
        <a-form-item label="工作本名称">
          <a-switch v-model:checked="formData.worksheetTitle" />
        </a-form-item>
      </div>
    </a-form>
    <a-form layout="vertical" style="display: flex">
      <div style="flex: 1">
        <a-form-item label="当前日期">
          <a-switch v-model:checked="formData.printDate" />
        </a-form-item>
      </div>
      <div style="flex: 1">
        <a-form-item label="当前时间">
          <a-switch v-model:checked="formData.printTime" />
        </a-form-item>
      </div>
    </a-form>
    <a-form layout="vertical">
      <a-form-item label="页码">
        <a-switch v-model:checked="formData.pageNumber" />
      </a-form-item>
    </a-form>
    <a-form layout="vertical" style="display: flex">
      <div style="flex: 1">
        <a-form-item label="重复冻结行">
          <a-switch v-model:checked="formData.yFreeze" :disabled="!sheetFreezeStatus.rowHasFreeze" />
        </a-form-item>
      </div>
      <div style="flex: 1">
        <a-form-item label="重复冻结列">
          <a-switch v-model:checked="formData.xFreeze" :disabled="!sheetFreezeStatus.colHasFreeze" />
        </a-form-item>
      </div>
    </a-form>
  </div>
</template>

<script lang="ts" setup>
  import { nextTick, reactive, toRefs, watch } from 'vue';
  import { debounce } from 'lodash-es';

  import { Switch as ASwitch, Form as AForm, Select as ASelect, FormItem as AFormItem, RadioGroup as ARadioGroup } from 'ant-design-vue';
  import {
    jnpfPaperPaddingTypeOptions,
    jnpfPaperTypeOptions,
    jnpfPrintDirectionOptions,
    jnpfPrintHAlignOptions,
    jnpfPrintScaleOptions,
    jnpfPrintVAlignOptions,
  } from '../../Design/univer/utils/define';

  const props = defineProps(['defaultPrintConfig', 'sheetFreezeStatus']);

  const emits = defineEmits(['change']);

  interface State {
    formData: Record<string, any>;
  }
  const state = reactive<State>({
    formData: {},
  });
  const { formData } = toRefs(state);

  let isInitialized = false; // 标志位，确保 `watch` 仅在初始化后触发

  // radio数据转化下格式
  function getRadioGroupOptions(data: any[] = []) {
    return data?.map((item: any) => {
      return { label: item.fullName, value: item.id };
    });
  }

  watch(
    () => props.sheetFreezeStatus,
    val => {
      const updatePrintConfig = {
        ...(props.defaultPrintConfig ?? {}),
        yFreeze: val?.rowHasFreeze,
        xFreeze: val?.colHasFreeze,
      };

      state.formData = JSON.parse(JSON.stringify(updatePrintConfig));

      nextTick(() => {
        isInitialized = true;
      });
    },
  );

  // 防抖后的触发方法
  const emitChangeDebounced = debounce((val: any) => {
    emits('change', val);
  }, 300);

  // 监听更改
  watch(
    () => state.formData,
    val => {
      if (isInitialized) {
        emitChangeDebounced(val);
      }
    },
    { deep: true, flush: 'post' },
  );
</script>

<style lang="less">
  .print-render-form-content {
    width: 290px;
    height: calc(100%);
    padding: 10px;
    margin-left: 16px;
    background: #fff;
    box-sizing: border-box;
    overflow-x: hidden;
    overflow-y: auto;

    .ant-form-item-label {
      padding-bottom: 2px;
    }

    .ant-form-item {
      margin-bottom: 9px;
    }
  }
</style>
