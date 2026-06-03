<template>
  <a-collapse-panel>
    <template #header>Y轴设置</template>
    <a-form-item label="显示坐标轴" >
      <a-switch v-model:checked="chart.yAxis.show" />
    </a-form-item>
    <template v-if="chart.yAxis.show" >
      <a-form-item label="坐标轴颜色">
        <jnpf-color-picker v-model:value="chart.yAxis.axisLine.lineStyle.color" size="small" />
      </a-form-item>
        <a-form-item label="Y轴名称" >
          <a-input v-model:value="chart.yAxis.name" placeholder="请输入" />
        </a-form-item>
        <a-form-item label="字体大小" >
          <a-input-number v-model:value="chart.yAxis.nameTextStyle.fontSize" placeholder="请输入" :min="12" :max="25" />
        </a-form-item>
        <a-form-item label="字体加粗" >
          <a-switch v-model:checked="chart.yAxis.nameTextStyle.fontWeight" />
        </a-form-item>
        <a-form-item label="字体颜色" >
          <jnpf-color-picker v-model:value="chart.yAxis.nameTextStyle.color" size="small" />
        </a-form-item>
        <a-form-item label="标签大小" >
          <a-input-number v-model:value="chart.yAxis.axisLabel.fontSize" :min="12" :max="25" placeholder="请输入" />
        </a-form-item>
        <a-form-item label="标签加粗">
          <a-switch v-model:checked="chart.yAxis.axisLabel.fontWeight" />
        </a-form-item>
      <a-form-item label="标签颜色">
        <jnpf-color-picker v-model:value="chart.yAxis.axisLabel.color" size="small" />
      </a-form-item>
      <a-form-item label="显示网格线">
        <a-switch v-model:checked="chart.yAxis.splitLine.show" />
      </a-form-item>
      <a-form-item label="网格线颜色" v-if="chart.yAxis.splitLine.show">
        <jnpf-color-picker v-model:value="chart.yAxis.splitLine.lineStyle.color" size="small" />
      </a-form-item>
    </template>
    <a-form-item label="反转">
      <a-switch v-model:checked="chart.yAxis.inverse" />
    </a-form-item>
  </a-collapse-panel>
</template>

<script setup lang="ts">
  defineProps(['chart']);
</script>
