<template>
  <a-collapse-panel>
    <template #header>{{ getTitle() }}</template>
    <a-form-item label="风格类型">
      <jnpf-select v-model:value="chart.styleType" :options="getStyleTypeOptions" placeholder="请选择" showSearch @change="getBarTypeList" />
    </a-form-item>
    <template v-if="chart.chartType == 'line'">
      <a-form-item label="面积堆积">
        <a-switch v-model:checked="chart.areaStyle" />
      </a-form-item>
      <a-form-item label="线条宽度">
        <a-slider v-model:value="chart.line.width" :min="1" :max="20" />
      </a-form-item>
      <a-form-item label="点的大小">
        <a-slider v-model:value="chart.line.symbolSize" :max="20" />
      </a-form-item>
    </template>
    <template v-if="chart.chartType == 'pie'">
      <a-form-item label="南丁格尔图">
        <a-switch v-model:checked="chart.pie.roseType" />
      </a-form-item>
      <a-form-item label="自动排序">
        <a-switch v-model:checked="chart.pie.sortable" />
      </a-form-item>
      <a-form-item label="不展示零">
        <a-switch v-model:checked="chart.pie.showZero" />
      </a-form-item>
    </template>
    <template v-if="chart.chartType == 'radar'">
      <a-form-item label="指示器大小">
        <a-input-number v-model:value="chart.radar.axisName.fontSize" :min="12" :max="25" />
      </a-form-item>
      <a-form-item label="指示器加粗">
        <a-switch v-model:checked="chart.radar.axisName.fontWeight" />
      </a-form-item>
      <a-form-item label="指示器颜色">
        <jnpf-color-picker v-model:value="chart.radar.axisName.color" size="small" />
      </a-form-item>
      <a-form-item label="区域透明度">
        <a-slider v-model:value="chart.seriesAreaStyleOpacity" :max="1" :step="0.1" />
      </a-form-item>
    </template>
  </a-collapse-panel>
</template>
<script setup lang="ts">
  import { computed } from 'vue';
  import { barStyleList, lineStyleList, pieStyleList, radarStyleList } from '@/components/VisualPortal/Design/helper/dataMap';

  const props = defineProps(['chart', 'dataSetList']);

  const getStyleTypeOptions = computed(() => {
    const chartType = props.chart.chartType;
    if (chartType == 'bar') return barStyleList.filter(o => o.id != 7);
    if (chartType == 'line') return lineStyleList;
    if (chartType == 'pie') return pieStyleList;
    if (chartType == 'radar') return radarStyleList;
  });

  function getTitle() {
    const chartType = props.chart.chartType;
    if (chartType == 'bar') return '柱状图设置';
    if (chartType == 'line') return '折线图设置';
    if (chartType == 'pie') return '饼图设置';
    if (chartType == 'radar') return '雷达图设置';
  }

  function getBarTypeList() {
    let list: any = [];
    if (props.chart.styleType == 7 && !props.chart.bar.barTypeList.length && props.dataSetList.length) {
      for (let index = 0; index < props.dataSetList.length; index++) {
        const element = props.dataSetList[index];
        for (let index = 0; index < element.children.length; index++) {
          const ele = element.children[index];
          list.push({ id: ele.jnpfId, title: ele.fullName, type: 'bar' });
        }
      }
      props.chart.bar.barTypeList = list;
    }
  }
</script>
