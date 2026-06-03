<template>
  <a-collapse-panel>
    <template #header>基础设置</template>
    <a-form-item :name="['chart', 'chartType']" label="图表类型">
      <jnpf-select v-model:value="chart.chartType" :options="chartTypeOptions" @change="onChartTypeChange" />
    </a-form-item>
  </a-collapse-panel>
</template>

<script setup lang="ts">
  defineProps(['chart', 'dataSetList']);

  const emit = defineEmits(['updateDefaultChart']);

  const chartTypeOptions = [
    { id: 'bar', fullName: '柱状图' },
    { id: 'line', fullName: '折线图' },
    { id: 'pie', fullName: '饼状图' },
    { id: 'radar', fullName: '雷达图' },
  ];

  // 设置默认展示数据
  function onChartTypeChange(value) {
    emit('updateDefaultChart', value);
  }
</script>
