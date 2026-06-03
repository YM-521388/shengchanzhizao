<template>
  <a-collapse-panel>
    <template #header>提示语设置</template>
    <a-form-item label="显示">
      <a-switch v-model:checked="chart.tooltip.show" />
    </a-form-item>
    <template v-if="chart.tooltip.show">
      <a-form-item label="字体大小">
        <a-input-number v-model:value="chart.tooltip.textStyleFontSize" placeholder="请输入" :min="12" :max="25" />
      </a-form-item>
      <a-form-item label="字体加粗">
        <a-switch v-model:checked="chart.tooltip.textStyleFontWeight" />
      </a-form-item>
      <a-form-item label="字体颜色">
        <jnpf-color-picker v-model:value="chart.tooltip.textStyleColor" size="small" />
      </a-form-item>
      <a-form-item label="背景色">
        <jnpf-color-picker v-model:value="chart.tooltip.bgColor" size="small" />
      </a-form-item>
    </template>
  </a-collapse-panel>
</template>

<script setup lang="ts">
   defineProps(['chart']);
</script>
