<template>
  <a-collapse-panel>
    <template #header>副标题设置</template>
    <a-form-item label="副标题名称">
      <jnpf-i18n-input v-model:value="chart.title.subtext" v-model:i18n="chart.title.subtextI18nCode" placeholder="请输入" />
    </a-form-item>
    <a-form-item label="字体大小">
      <a-input-number v-model:value="chart.title.subtextStyle.fontSize" placeholder="请输入" :min="12" :max="25" />
    </a-form-item>
    <a-form-item label="字体加粗">
      <a-switch v-model:checked="chart.title.subtextStyle.fontWeight" checkedValue="bold" unCheckedValue="normal" />
    </a-form-item>
    <a-form-item label="字体颜色">
      <jnpf-color-picker v-model:value="chart.title.subtextStyle.color" size="small" />
    </a-form-item>
  </a-collapse-panel>
</template>

<script setup lang="ts">
  defineProps(['chart', 'dataSetList']);
</script>
