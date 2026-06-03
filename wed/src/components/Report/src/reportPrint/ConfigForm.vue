<template>
  <a-form ref="formRef" layout="vertical" :model="formState" :colon="false">
    <!-- <a-form-item ref="name" label="打印范围" name="area">
      <jnpf-select v-model:value="formState.area" :options="configOptions.printAreaOptions" />
    </a-form-item> -->
    <a-form-item label="纸张类型" name="paperType">
      <jnpf-select v-model:value="formState.paperType" :options="configOptions.paperTypeOptions" />
    </a-form-item>
    <a-form-item label="纸张方向" name="direction">
      <jnpf-radio v-model:value="formState.direction" :options="configOptions.printDirectionOptions" optionType="button" buttonStyle="solid" />
    </a-form-item>
    <a-form-item label="页面缩放" name="printScale">
      <jnpf-select v-model:value="formState.printScale" :options="configOptions.printScaleOptions" />
    </a-form-item>
    <a-form-item label="上下对齐" name="vAlign">
      <jnpf-radio v-model:value="formState.vAlign" :options="configOptions.printVAlignOptions" optionType="button" buttonStyle="solid" />
    </a-form-item>
    <a-form-item label="左右对齐" name="hAlign">
      <jnpf-radio v-model:value="formState.hAlign" :options="configOptions.printHAlignOptions" optionType="button" buttonStyle="solid" />
    </a-form-item>
    <a-form-item label="网格线" name="gridlines">
      <a-switch v-model:checked="formState.gridlines" />
    </a-form-item>
    <a-form-item label="页码" name="pageNumber">
      <a-switch v-model:checked="formState.pageNumber" />
    </a-form-item>
    <a-form-item label="报表名称" name="workbookTitle">
      <a-switch v-model:checked="formState.workbookTitle" />
    </a-form-item>
    <a-form-item label="工作表名" name="WorksheetTitle">
      <a-switch v-model:checked="formState.worksheetTitle" />
    </a-form-item>
    <a-form-item label="当前日期" name="printDate">
      <a-switch v-model:checked="formState.printDate" />
    </a-form-item>
    <a-form-item label="当前时间" name="printTime">
      <a-switch v-model:checked="formState.printTime" />
    </a-form-item>
    <a-form-item name="yFreeze">
      <template #label>重复冻结行<BasicHelp text="无冻结/无冻结行，打印属性不可配置；反之，允许配置在打印预览/打印时是否显示重复的冻结区域" /></template>
      <a-switch v-model:checked="formState.yFreeze" :disabled="!freezeObject.hasYFreeze" />
    </a-form-item>
    <a-form-item name="xFreeze">
      <template #label>重复冻结列<BasicHelp text="无冻结/无冻结列，打印属性不可配置；反之，允许配置在打印预览/打印时是否显示重复的冻结区域" /></template>
      <a-switch v-model:checked="formState.xFreeze" :disabled="!freezeObject.hasXFreeze" />
    </a-form-item>
  </a-form>
</template>

<script lang="ts" setup>
  defineProps(['formState', 'configOptions', 'freezeObject']);
</script>
