<template>
  <a-collapse-panel>
    <template #header>柱体设置</template>
    <a-form-item label="宽度">
      <a-input-number v-model:value="chart.barWidth" :min="0" :max="100" placeholder="请输入" />
    </a-form-item>
    <a-form-item label="弧度">
      <a-input-number v-model:value="chart.itemStyle.borderRadius" :min="0" :max="100" placeholder="请输入" />
    </a-form-item>
  </a-collapse-panel>
</template>

<script setup lang="ts">
  defineProps(['chart']);
</script>
