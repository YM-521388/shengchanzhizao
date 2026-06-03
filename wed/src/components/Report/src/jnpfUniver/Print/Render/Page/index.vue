<template>
  <div ref="pageContainerRef" :style="containerStyle" class="print-page-container"></div>
</template>

<script lang="ts" setup>
  import { nextTick, onMounted, onUnmounted, reactive, ref, toRefs, unref } from 'vue';
  import { JnpfSheetsPrintUiService } from '../../../Design/univer/services/sheet-print-ui.service';
  import { printA4PaperHeight, printA4PaperWidth } from '../../../Design/univer/utils/define';

  const props = defineProps(['jnpfUniverApi', 'pageLayout', 'printConfig']);
  const emits = defineEmits(['pageRendered']);

  // ✅ 页面容器引用
  const pageContainerRef = ref<HTMLElement | null>(null);
  const defaultContainerStyle = {
    width: printA4PaperWidth,
    height: printA4PaperHeight,
  };

  const state = reactive<{
    renderInstance: any;
    containerStyle: any;
  }>({
    renderInstance: null,
    containerStyle: defaultContainerStyle,
  });
  const { containerStyle } = toRefs(state);

  onMounted(() => {
    const { jnpfUniverApi, pageLayout = {}, printConfig = {} } = props ?? {};

    const accessor = jnpfUniverApi?.getInjector();
    if (!accessor) {
      return;
    }

    const { paperSize = {} } = pageLayout;
    const containerScale = countContainerScale(paperSize); // 打印预览容器的伸缩比

    state.renderInstance = new JnpfSheetsPrintUiService(accessor, pageLayout, printConfig, containerScale);
    if (!state.renderInstance) {
      return;
    }

    const { w: paperSizeWidth = printA4PaperWidth, h: paperSizeHeight = printA4PaperHeight } = paperSize;
    state.containerStyle = {
      width: `${paperSizeWidth * containerScale}px`,
      height: `${paperSizeHeight * containerScale}px`,
    };

    state.renderInstance.container.style.cssText = 'width: 100%; height: 100%;';

    nextTick(() => {
      unref(pageContainerRef)?.appendChild(state.renderInstance.container);

      state.renderInstance?.setDirty(true); // 设置脏状态（用于调整视口大小和滚动值的状态）
      state.renderInstance?.renderPage(); // 渲染页面
      state.renderInstance?.renderOnReady(); // 主要是在页面变换时确保页面内容的及时更新，并且做了订阅和销毁的管理。

      emits('pageRendered');
    });
  });

  onUnmounted(async () => {
    await state.renderInstance?.dispose();

    state.renderInstance = null;
    state.containerStyle = defaultContainerStyle;
  });

  // 计算打印预览容器的伸缩比
  function countContainerScale(paperSize: any) {
    const { w = printA4PaperWidth } = paperSize;

    return Math.round((printA4PaperWidth / w) * 10) / 10;
  }
</script>

<style lang="less">
  .print-page-container {
    position: relative;
    flex: 0 0 auto;
    margin: 28px auto;
    background-color: #fff;
    box-shadow: 0 4px 24px 0 rgba(30, 34, 43, 0.08);
  }
</style>
