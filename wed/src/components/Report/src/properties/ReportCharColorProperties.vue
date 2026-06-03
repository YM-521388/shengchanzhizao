<template>
  <a-collapse-panel>
    <template #header>自定义配色设置</template>
    <template v-if=" ['bar','line'].includes(chart.chartType)">
      <a-form-item label="文字颜色">
        <jnpf-color-picker v-model:value="chart.color.AxisTextStyleColor" size="small" />
      </a-form-item>
      <a-form-item label="轴线颜色">
        <jnpf-color-picker v-model:value="chart.color.AxisLineStyleColor" size="small" />
      </a-form-item>
    </template>
    <a-table :data-source="chart.color.list" :columns="getColumns" :pagination="false" size="small">
      <template #bodyCell="{ column, record, index }">
        <template v-if="column.key === 'color1' || column.key === 'color2'">
          <jnpf-color-picker v-model:value="record[column.key]" size="small" />
        </template>
        <template v-if="column.key === 'action'">
          <a-button type="link" danger @click="handleDel(index)"><i class="icon-ym icon-ym-delete"></i></a-button>
        </template>
      </template>
    </a-table>
    <div class="table-add-action mb-15px !mt-unset" @click="handleAdd()">
      <a-button type="link" preIcon="icon-ym icon-ym-btn-add">新增</a-button>
    </div>
  </a-collapse-panel>
</template>

<script setup lang="ts">
  import { computed } from 'vue';

  import { useI18n } from '@/hooks/web/useI18n';
  import { useMessage } from '@/hooks/web/useMessage';

  const { t } = useI18n();
  const { createMessage, createConfirm } = useMessage();

  const props = defineProps(['chart']);

  const getColumns = computed(() => {
    const color = { title: '颜色', key: 'color1', align: 'center' };
    const color1 = { title: '渐变色', key: 'color2', align: 'center' };
    const action = { title: '操作', key: 'action', align: 'center' };
    return [color, color1, action];
  });

  function handleAdd() {
    props.chart.color.list.push({ color1: '#1890ff', color2: '#4696DD' });
  }
  function handleDel(index) {
    createConfirm({
      iconType: 'warning',
      title: t('common.tipTitle'),
      content: t('common.delTip'),
      onOk: () => {
        props.chart.color.list.splice(index, 1);
        createMessage.success('删除成功');
      },
    });
  }
</script>
