<template>
  <div style="width: 0; height: 0; display: none"></div>
</template>

<script lang="ts" setup>
  import * as echarts from 'echarts';

  defineOptions({ name: 'JnpfUniverCellEchart' });

  interface PropsType {
    univerCreateMode: 'design' | 'preview' | 'print';
  }
  const props = defineProps<PropsType>();

  // 渲染
  function render(option: echarts.EChartsOption, sideLength: number) {
    // 创建一个临时容器
    const tempContainer = document.createElement('div');
    tempContainer.style.width = `${sideLength}px`;
    tempContainer.style.height = `${sideLength}px`;
    tempContainer.style.position = 'absolute';
    tempContainer.style.visibility = 'hidden'; // 避免影响页面视觉
    document.body.appendChild(tempContainer);

    let chartToImageUrl: string | null = null;

    if (props.univerCreateMode !== 'print') {
      try {
        // 初始化 ECharts 实例
        const tempChartInstance = echarts.init(tempContainer);

        // 配置图表选项
        const updateOption = {
          ...option,
          animation: false, // 禁用动画以提高生成效率
        };

        // 设置图表选项
        tempChartInstance.setOption(updateOption);

        // 获取图表的 Base64 图片 URL
        chartToImageUrl = tempChartInstance.getDataURL({
          type: 'jpeg',
          backgroundColor: '#f9f9f9',
        });

        // 销毁图表实例
        tempChartInstance.dispose();
      } catch (error) {
        console.error('图表生成失败:', error);
      } finally {
        // 移除临时容器
        document.body.removeChild(tempContainer);
      }

      return chartToImageUrl;
    }
  }

  defineExpose({
    render,
  });
</script>
