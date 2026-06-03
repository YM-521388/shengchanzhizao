<template>
  <a-collapse-panel>
    <template #header>标题设置</template>
    <a-form-item  label="标题名称">
      <jnpf-i18n-input v-model:value="chart.title.text" v-model:i18n="chart.title.textI18nCode" placeholder="请输入" />
    </a-form-item>
    <a-form-item  label="字体大小">
      <a-input-number v-model:value="chart.title.textStyle.fontSize" placeholder="请输入" :min="12" :max="25" />
    </a-form-item>
    <a-form-item  label="字体加粗">
      <a-switch v-model:checked="chart.title.textStyle.fontWeight" checkedValue="bold" unCheckedValue="normal" />
    </a-form-item>
    <a-form-item  label="字体颜色">
      <jnpf-color-picker v-model:value="chart.title.textStyle.color" size="small" />
    </a-form-item>
    <a-form-item  label="字体位置">
      <jnpf-radio v-model:value="chart.title.left" :options="textAlignOptions" optionType="button" button-style="solid" class="right-radio" />
    </a-form-item>
    <a-form-item label="背景色">
      <jnpf-color-picker v-model:value="chart.title.titleBgColor" size="small" />
    </a-form-item>
  </a-collapse-panel>
</template>

<script setup lang="ts">
  defineProps(['chart', 'dataSetList']);

  const textAlignOptions = [
    { id: 'left', fullName: '左对齐' },
    { id: 'center', fullName: '居中对齐' },
    { id: 'right', fullName: '右对齐' },
  ];
</script>
