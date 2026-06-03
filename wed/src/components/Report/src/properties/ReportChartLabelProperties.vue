<template>
  <a-collapse-panel>
    <template #header>数值设置</template>
    <a-form-item label="显示">
      <a-switch v-model:checked="chart.label.show" />
    </a-form-item>
    <template v-if="chart.label.show">
      <template v-if="chart.chartType == 'pie'">
        <a-form-item label="显示位置">
          <jnpf-radio
            v-model:value="chart.seriesLabelPosition"
            :options="labelPositionList"
            optionType="button"
            button-style="solid"
            class="right-radio" />
        </a-form-item>
        <a-form-item label="显示内容">
          <jnpf-checkbox
            v-model:value="chart.seriesLabelShowInfo"
            :options="labelShowList"
            optionType="button"
            button-style="solid"
            class="right-radio" />
        </a-form-item>
      </template>
      <a-form-item label="字体大小">
        <a-input-number v-model:value="chart.label.fontSize" placeholder="请输入" :min="12" :max="25" />
      </a-form-item>
      <a-form-item label="字体加粗">
        <a-switch v-model:checked="chart.label.fontWeight" />
      </a-form-item>
    </template>
    <a-form-item label="字体颜色">
      <jnpf-color-picker v-model:value="chart.label.color" size="small" />
    </a-form-item>
    <a-form-item label="背景色">
      <jnpf-color-picker v-model:value="chart.label.backgroundColor" size="small" />
    </a-form-item>
  </a-collapse-panel>
</template>
<script lang="ts" setup>
  defineProps(['chart']);

  const labelPositionList = [
    { fullName: '外侧', id: 'outside' },
    { fullName: '内侧', id: 'inside' },
  ];
  const labelShowList = [
    { fullName: '数值', id: 'count' },
    { fullName: '百分比', id: 'percent' },
  ];
</script>
