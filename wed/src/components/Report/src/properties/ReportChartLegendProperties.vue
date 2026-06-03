<template>
  <a-collapse-panel>
    <template #header>图例设置</template>
    <a-form-item label="显示图例">
      <a-switch v-model:checked="chart.legend.show" />
    </a-form-item>
    <a-form-item label="字体大小">
      <a-input-number v-model:value="chart.legend.textStyle.fontSize" placeholder="请输入" :min="12" :max="25" />
    </a-form-item>
    <a-form-item label="布局">
      <jnpf-radio v-model:value="chart.legend.orient" :options="orientOptions" optionType="button" button-style="solid" class="right-radio" />
    </a-form-item>
    <a-form-item label="上下边距">
      <a-slider v-model:value="chart.legendTop" :max="100" />
    </a-form-item>
    <a-form-item label="左右边距">
      <a-slider v-model:value="chart.legendLeft" :max="100" />
    </a-form-item>
  </a-collapse-panel>
</template>
<script setup lang="ts">
  defineProps(['chart', 'dataSetList']);

  const orientOptions = [
    { fullName: '横排', id: 'horizontal' },
    { fullName: '竖排', id: 'vertical' },
  ];
</script>
