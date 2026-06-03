<template>
  <a-collapse-panel>
    <template #header>X轴设置</template>
    <a-form-item label="坐标轴类型">
      <jnpf-radio v-model:value="chart.xAxis.type" :options="categoryList" optionType="button" button-style="solid" class="right-radio" />
    </a-form-item>
    <a-form-item label="显示坐标轴">
      <a-switch v-model:checked="chart.xAxis.show" />
    </a-form-item>
    <template v-if="chart.xAxis.show">
      <a-form-item label="坐标轴颜色">
        <jnpf-color-picker v-model:value="chart.xAxis.axisLine.lineStyle.color" size="small" />
      </a-form-item>
      <a-form-item label="X轴名称">
        <a-input v-model:value="chart.xAxis.name" placeholder="请输入" />
      </a-form-item>
      <a-form-item label="字体大小">
        <a-input-number v-model:value="chart.xAxis.nameTextStyle.fontSize" placeholder="请输入" :min="12" :max="25" />
      </a-form-item>
      <a-form-item label="字体加粗">
        <a-switch v-model:checked="chart.xAxis.nameTextStyle.fontWeight" />
      </a-form-item>
      <a-form-item label="字体颜色">
        <jnpf-color-picker v-model:value="chart.xAxis.nameTextStyle.color" size="small" />
      </a-form-item>
      <a-form-item label="标签大小">
        <a-input-number v-model:value="chart.xAxis.axisLabel.fontSize" :min="12" :max="25" placeholder="请输入" />
      </a-form-item>
      <a-form-item label="标签加粗">
        <a-switch v-model:checked="chart.xAxis.axisLabel.fontWeight" />
      </a-form-item>
      <a-form-item label="标签颜色">
        <jnpf-color-picker v-model:value="chart.xAxis.axisLabel.color" size="small" />
      </a-form-item>
      <a-form-item label="标签角度">
        <a-input-number v-model:value="chart.xAxis.axisLabel.rotate" :min="0" :max="100" placeholder="请输入" />
      </a-form-item>
      <a-form-item label="显示网格线">
        <a-switch v-model:checked="chart.xAxis.splitLine.show" />
      </a-form-item>
      <a-form-item label="网格线颜色" v-if="chart.xAxis.splitLine.show">
        <jnpf-color-picker v-model:value="chart.xAxis.splitLine.lineStyle.color" size="small" />
      </a-form-item>
      <a-form-item label="刻度对齐">
        <a-switch v-model:checked="chart.xAxis.axisTick.alignWithLabel" />
      </a-form-item>
    </template>
    <a-form-item label="反转">
      <a-switch v-model:checked="chart.xAxis.inverse" />
    </a-form-item>
  </a-collapse-panel>
</template>

<script setup lang="ts">
  defineProps(['chart']);

  const categoryList = [
    { fullName: '类型', id: 'category' },
    { fullName: '数值', id: 'value' },
  ];
</script>
