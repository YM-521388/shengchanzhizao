<template>
  <a-collapse-panel>
    <template #header>{{ getHeaderText }}</template>
    <template v-if="['bar', 'line'].includes(chart.chartType)">
      <a-form-item  label="左边距">
        <a-slider v-model:value="chart.grid.left" :max="100" />
      </a-form-item>
      <a-form-item  label="顶边距">
        <a-slider v-model:value="chart.grid.top" :max="100" />
      </a-form-item>
      <a-form-item  label="右边距">
        <a-slider v-model:value="chart.grid.right" :max="100" />
      </a-form-item>
      <a-form-item  label="底边距">
        <a-slider v-model:value="chart.grid.bottom" :max="100" />
      </a-form-item>
    </template>
    <template v-if="['pie', 'radar'].includes(chart.chartType)">
      <a-form-item  label="上下边距">
        <a-slider v-model:value="chart.seriesCenter.seriesCenterTop" :max="100" />
      </a-form-item>
      <a-form-item  label="左右边距">
        <a-slider v-model:value="chart.seriesCenter.seriesCenterLeft" :max="100" />
      </a-form-item>
    </template>
  </a-collapse-panel>
</template>

<script setup lang="ts">
  import { computed } from 'vue';
  const props = defineProps(['chart', 'dataSetList']);

 
  const getHeaderText = computed(() => {
    if (['pie', 'radar'].includes(props.chart.chartType)) return '中心坐标设置';
    return '坐标轴边距设置';
  });
</script>
