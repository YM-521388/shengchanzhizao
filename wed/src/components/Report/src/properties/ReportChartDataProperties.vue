<template>
  <a-collapse-panel>
    <template #header>数据设置</template>
    <a-form-item label="系列">
      <jnpf-tree-select v-model:value="chart.classifyNameField" lastLevel lastLevelKey="children" :options="dataSetList" :fieldNames="{ value: 'jnpfId' }" />
    </a-form-item>
    <a-form-item label="维度">
      <jnpf-tree-select v-model:value="chart.seriesNameField" lastLevel lastLevelKey="children" :options="dataSetList" :fieldNames="{ value: 'jnpfId' }" />
    </a-form-item>
    <a-form-item label="数值">
      <jnpf-tree-select v-model:value="chart.seriesDataField" lastLevel lastLevelKey="children" :options="dataSetList" :fieldNames="{ value: 'jnpfId' }" />
    </a-form-item>
    <a-form-item label="最大值" v-if="chart.chartType == 'radar'">
      <jnpf-tree-select v-model:value="chart.maxField" lastLevel lastLevelKey="children" :options="dataSetList" :fieldNames="{ value: 'jnpfId' }" />
    </a-form-item>
    <a-form-item label="汇总方式">
      <jnpf-select v-model:value="chart.summaryType" :options="summaryTypeOptions" />
    </a-form-item>
    <div v-if="chart.styleType == 7 && chart.chartType == 'bar'" class="my-20px">
      <a-table :data-source="chart.bar.barTypeList" :columns="columns" size="small" :pagination="false">
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'title'"> {{ record[column.key] }} </template>
          <template v-if="column.key === 'type'">
            <jnpf-select v-model:value="record[column.key]" placeholder="请选择" :options="barTypeList" showSearch />
          </template>
        </template>
      </a-table>
    </div>
  </a-collapse-panel>
</template>

<script lang="ts" setup>

  defineProps(['chart', 'dataSetList']);

  const summaryTypeOptions = [
    { id: 'none', fullName: '无' },
    { id: 'sum', fullName: '合计' },
    { id: 'max', fullName: '最大值' },
    { id: 'min', fullName: '最小值' },
    { id: 'count', fullName: '计数' },
    { id: 'avg', fullName: '平均值' },
  ];
  const barTypeList = [
    { fullName: '柱体', id: 'bar' },
    { fullName: '折线', id: 'line' },
  ];

  const columns = [
    { title: '系列', key: 'title', ellipsis: true },
    { title: '类型', key: 'type' },
  ];

</script>
