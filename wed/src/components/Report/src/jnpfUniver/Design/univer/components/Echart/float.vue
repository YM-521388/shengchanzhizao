<template>
  <div ref="floatChartContainerRef" :id="props.id" class="jnpf-univer-float-echart-ele"></div>
</template>

<script lang="ts" setup>
  import { onMounted, onBeforeUnmount, ref, watch } from 'vue';
  import { storeToRefs } from 'pinia';
  import * as echarts from 'echarts';
  import { debounce } from 'lodash-es';
  import { useJnpfUniverStore } from '../../../store';

  defineOptions({ name: 'JnpfUniverFloatEchart' });

  interface PropsType {
    id: string;
    piniaStoreId: string;
  }
  const props = defineProps<PropsType>();
  const floatChartContainerRef = ref<HTMLDivElement | null>(null);

  const jnpfUniverStore = useJnpfUniverStore(props?.piniaStoreId);
  const { univerCreateModeCache, focusedFloatEchartDataCache, floatEchartToUniverImgDataCaches } = storeToRefs(jnpfUniverStore);

  let floatChartInstance: echarts.ECharts | null = null;
  let resizeObserver: ResizeObserver | null = null;

  // 处理窗口大小变化的函数，带300ms 防抖
  const handleResize = debounce(() => {
    floatChartInstance?.resize();
  }, 300);

  // 初始化或更新图表
  function initialize(option: echarts.EChartsOption) {
    if (!floatChartContainerRef.value) {
      return;
    }

    if (!floatChartInstance) {
      floatChartInstance = echarts?.init(floatChartContainerRef.value);
    }

    try {
      floatChartInstance?.clear();

      // 使用传入的 options 配置图表
      floatChartInstance?.setOption(option);

      // 非设计状态下，图表直接转成图片
      if (univerCreateModeCache.value !== 'design') {
        const { id: domId } = props ?? {};

        if (domId) {
          try {
            const chartToImageUrl = floatChartInstance.getDataURL({
              type: 'jpeg',
              backgroundColor: '#f9f9f9',
            });

            const caches = {
              ...floatEchartToUniverImgDataCaches.value,
              [domId]: chartToImageUrl,
            };

            jnpfUniverStore?.setFloatEchartToUniverImgDataCaches(caches);
          } catch (e) {}
        }
      }

      // 设置 ResizeObserver 监听容器大小变化
      if (!resizeObserver) {
        resizeObserver = new ResizeObserver(handleResize);
        resizeObserver.observe(floatChartContainerRef.value);
      }
    } catch (e) {
      console.error(`图表生成失败：${e}`);
    }
  }

  onMounted(() => {
    const { id: domId, piniaStoreId } = props ?? {};

    if (!floatChartContainerRef.value || !domId || !piniaStoreId) {
      return;
    }

    const floatEchartDataCaches = jnpfUniverStore?.floatEchartDataCaches ?? {};

    const targetOption = Object.values(floatEchartDataCaches as Record<string, { domId: string; option: any }>)?.find(item => item?.domId === domId)?.option;
    initialize(targetOption);
  });

  // 销毁
  onBeforeUnmount(() => {
    if (resizeObserver && floatChartContainerRef.value) {
      resizeObserver.unobserve(floatChartContainerRef.value);
    }

    resizeObserver?.disconnect();
    floatChartInstance?.dispose();
  });

  watch(
    () => focusedFloatEchartDataCache.value,
    value => {
      const { domId, drawingId, option } = value;

      if (!domId && !drawingId) {
        // store置空的情况下，不响应
        return;
      }

      const containerId = floatChartContainerRef.value?.getAttribute('id');
      if (domId !== containerId) {
        return;
      }

      initialize(option);
    },
  );
</script>

<style lang="less" scoped>
  .jnpf-univer-float-echart-ele {
    position: absolute;
    width: 100%;
    height: 100%;
    background: #f9f9f9;
  }
</style>
