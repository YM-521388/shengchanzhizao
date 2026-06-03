<template>
  <div class="print-render-content">
    <div class="tools-wrap">
      <span :class="['first-page', { disabled: disabledForward }]" @click="setFirstViewPageNumber" title="首页">
        <svg class="icon" viewBox="0 0 1024 1024" xmlns="http://www.w3.org/2000/svg" width="16" height="16">
          <path
            d="M507.733333 490.666667L768 230.4 704 170.666667 384 490.666667l320 320 59.733333-59.733334-256-260.266666zM341.333333 170.666667H256v640h85.333333V170.666667z" />
        </svg>
        <em>首页</em>
      </span>
      <span :class="['prev-page', { disabled: disabledForward }]" @click="setSingleViewPageNumber('prev')" title="上一页" style="margin-right: 12px">
        <svg class="icon" viewBox="0 0 1024 1024" xmlns="http://www.w3.org/2000/svg" width="16" height="16">
          <path
            d="M473.6 490.666667L789.333333 170.666667 853.333333 230.4l-260.266666 260.266667 260.266666 260.266666-64 59.733334-315.733333-320z m-302.933333 0L490.666667 170.666667l59.733333 59.733333-260.266667 260.266667 260.266667 260.266666-59.733333 59.733334L170.666667 490.666667z" />
        </svg>
        <em>上一页</em>
      </span>
      <input class="target-page" v-model="currentViewPageNumber" type="number" @blur="setInputViewPageNumber" min="1" :max="completePagesLayout.length" />
      <span class="total-page-number">
        <em>/</em>
        <strong>{{ completePagesLayout.length }}</strong>
      </span>
      <span :class="['next-page', { disabled: disabledBackward }]" @click="setSingleViewPageNumber('next')" title="下一页" style="margin-left: 12px">
        <em>下一页</em>
        <svg class="icon" viewBox="0 0 1024 1024" xmlns="http://www.w3.org/2000/svg" width="16" height="16">
          <path
            d="M550.4 490.666667L230.4 170.666667 170.666667 230.4l260.266666 260.266667L170.666667 750.933333 230.4 810.666667l320-320z m298.666667 0L533.333333 170.666667 469.333333 230.4l260.266667 260.266667-260.266667 260.266666 59.733334 59.733334 320-320z" />
        </svg>
      </span>
      <span :class="['last-page', { disabled: disabledBackward }]" @click="setLastViewPageNumber" title="尾页">
        <em>尾页</em>
        <svg class="icon" viewBox="0 0 1024 1024" xmlns="http://www.w3.org/2000/svg" width="16" height="16">
          <path
            d="M516.266667 490.666667L256 230.4 315.733333 170.666667l320 320L315.733333 810.666667 256 750.933333l260.266667-260.266666zM678.4 170.666667h85.333333v640h-85.333333V170.666667z" />
        </svg>
      </span>
    </div>
    <div ref="pagesContainerRef" class="print-render-pages-content">
      <template v-for="pageLayout in processPagesLayout" :key="pageLayout?.layoutId">
        <JnpfUniverPrintRenderPage
          :jnpf-univer-api="props.jnpfUniverApi"
          :page-layout="pageLayout"
          :print-config="printConfig"
          @page-rendered="handlePreviewPageRendered" />
      </template>
    </div>
  </div>
</template>

<script lang="ts" setup>
  import { reactive, toRefs, ref, nextTick, computed } from 'vue';
  import { debounce } from 'lodash-es';
  import JnpfUniverPrintRenderPage from './Page/index.vue';
  import { JnpfSheetsPrintUiService } from '../../Design/univer/services/sheet-print-ui.service';
  import { getPrintPageStyle } from '../../Design/univer/utils';
  import { printA4PaperHeight, printA4PaperWidth } from '../../Design/univer/utils/define';

  defineOptions({ name: 'JnpfUniverPrint' });

  const props = defineProps(['jnpfUniverApi']);
  const emits = defineEmits(['previewRenderProgress', 'printRenderProgress', 'browserPrintShow']);

  const batchRenderSize = 10; // 分页加载的数量
  const pagesContainerRef = ref();

  const state = reactive<{
    completePagesLayout: any[];
    processPagesLayout: any[];

    pagingTargetQuantity: number;
    pagingRenderedQuantity: number;
    totalRenderedQuantity: number;

    printConfig: any;
    paperSize: any;
    paperViewHeight: number;

    printRenderInstances: any[];

    currentViewPageNumber: number;
  }>({
    completePagesLayout: [], // 完整的页面架构
    processPagesLayout: [], // 进程中页面架构

    pagingTargetQuantity: 0,
    pagingRenderedQuantity: 0,
    totalRenderedQuantity: 0,

    printConfig: null,
    paperSize: null,
    paperViewHeight: printA4PaperHeight,

    printRenderInstances: [], // 打印渲染的实例

    currentViewPageNumber: 1, // 当前视图页码
  });
  const { completePagesLayout, processPagesLayout, printConfig, currentViewPageNumber } = toRefs(state);

  // 禁止向前翻页状态
  const disabledForward = computed(() => {
    return state.currentViewPageNumber <= 1;
  });

  // 禁止向后翻页状态
  const disabledBackward = computed(() => {
    return state.currentViewPageNumber >= state.completePagesLayout.length;
  });

  // 翻页 - 去首页
  function setFirstViewPageNumber() {
    state.currentViewPageNumber = 1;

    handleScrollToPosition();
  }

  // 翻页 - 去尾页
  function setLastViewPageNumber() {
    state.currentViewPageNumber = state.completePagesLayout.length;

    handleScrollToPosition();
  }

  // 翻页 - 前后单页控制
  function setSingleViewPageNumber(direction: 'prev' | 'next') {
    const maxPage = state.completePagesLayout.length;
    const currentPage = state.currentViewPageNumber;

    // 根据方向计算目标页码
    const newPageNumber =
      direction === 'prev'
        ? Math.max(currentPage - 1, 1) // 前一页，不能小于 1
        : Math.min(currentPage + 1, maxPage); // 下一页，不能大于总页数

    // 如果页码发生变化，更新并滚动
    if (newPageNumber !== currentPage) {
      state.currentViewPageNumber = newPageNumber;
      handleScrollToPosition();
    }
  }

  // 翻页 - 直接输入页码
  function setInputViewPageNumber() {
    const pageNumber = Number(state.currentViewPageNumber);

    if (isNaN(pageNumber) || pageNumber < 1) {
      state.currentViewPageNumber = 1;
    } else if (pageNumber > state.completePagesLayout?.length) {
      state.currentViewPageNumber = state.completePagesLayout.length;
    }

    handleScrollToPosition();
  }

  // 设置滚动的位置
  function handleScrollToPosition() {
    const targetScrollTop = state.paperViewHeight * (state.currentViewPageNumber - 1) + 28 * (state.currentViewPageNumber - 1);

    pagesContainerRef.value.scrollTo({
      top: targetScrollTop,
      behavior: 'smooth',
    });
  }

  // 定义防抖后的函数
  const debouncedOnPagesContainerScroll = debounce(() => {
    const scrollTop = pagesContainerRef.value.scrollTop;

    state.currentViewPageNumber = Math.ceil((scrollTop + 28) / (state.paperViewHeight + 28));
  }, 100); // 100 毫秒的防抖时间，可以根据需要调整

  // 开启监听容器滚动事件
  function onPagesContainerScroll() {
    if (pagesContainerRef.value) {
      pagesContainerRef.value.addEventListener('scroll', debouncedOnPagesContainerScroll);
    }
  }

  // 关闭监听容器滚动事件
  function offPagesContainerScroll() {
    if (pagesContainerRef.value) {
      pagesContainerRef.value.removeEventListener('scroll', debouncedOnPagesContainerScroll);
    }
  }

  // 暴露 - 处理打印渲染
  async function handlePrintRender(printConfig: any) {
    resetState();

    const { jnpfUniverApi } = props ?? {};

    try {
      if (!jnpfUniverApi) {
        return;
      }

      const res = await getLayoutsAsync(printConfig);
      const { pagesLayout, printConfig: updatePrintConfig, paperSize } = (res ?? {}) as any;

      if (!pagesLayout.length) {
        emits('previewRenderProgress', {
          total: 0,
          rendered: 0,
        });
        return;
      }

      state.completePagesLayout = Array.isArray(pagesLayout) ? [...pagesLayout] : [];
      state.printConfig = { ...updatePrintConfig };
      state.paperSize = { ...paperSize };

      batchRenderPreviewPages();
    } catch (error) {
      console.error('Error:', error);
    }
  }

  // 获得布局的结果
  function getLayoutsAsync(config: any) {
    const { jnpfUniverApi } = props ?? {};

    return new Promise((resolve, reject) => {
      try {
        const result = jnpfUniverApi?.getSheetsPrint()?.getLayouts(config);
        resolve(result);
      } catch (error) {
        reject(error);
      }
    });
  }

  // 批量加载预览页面
  function batchRenderPreviewPages() {
    const { completePagesLayout, processPagesLayout } = state;

    state.pagingRenderedQuantity = 0;

    // 全部都渲染完了
    if (processPagesLayout.length >= completePagesLayout.length) {
      state.pagingTargetQuantity = 0;
      return;
    }

    if (completePagesLayout.length <= batchRenderSize) {
      state.pagingTargetQuantity = completePagesLayout.length;
      state.processPagesLayout = [...completePagesLayout];
    } else {
      const lastPageIndex = processPagesLayout.length;
      const batchData = completePagesLayout.slice(lastPageIndex, lastPageIndex + batchRenderSize) ?? [];

      state.pagingTargetQuantity = batchData.length;
      state.processPagesLayout = processPagesLayout.concat(batchData);
    }
  }

  // 预览页面渲染回调
  function handlePreviewPageRendered() {
    state.pagingRenderedQuantity += 1;
    state.totalRenderedQuantity += 1;

    emits('previewRenderProgress', {
      total: state.completePagesLayout.length,
      rendered: state.totalRenderedQuantity,
    });

    if (state.pagingRenderedQuantity === state.pagingTargetQuantity && state.processPagesLayout.length < state.completePagesLayout.length) {
      setTimeout(batchRenderPreviewPages, 500);
    } else if (state.pagingRenderedQuantity === state.pagingTargetQuantity && state.processPagesLayout.length >= state.completePagesLayout.length) {
      nextTick(() => {
        const firstPageDom = document.getElementsByClassName('print-page-container')?.[0] as HTMLElement | undefined;

        if (firstPageDom) {
          const styleHeight = firstPageDom?.style?.height;
          state.paperViewHeight = parseInt(styleHeight);
        } else {
          state.paperViewHeight = printA4PaperHeight;
        }

        // 绑定容器的滚动事件
        onPagesContainerScroll();
      });
    }
  }

  // 暴露 - 浏览器打印
  function handleBrowserPrint() {
    emits('printRenderProgress', {
      total: state.completePagesLayout.length,
      rendered: 0,
    });

    state.printRenderInstances = [];

    const { jnpfUniverApi } = props ?? {};

    const accessor = jnpfUniverApi?.getInjector();
    if (!accessor) {
      return;
    }

    batchRenderPrintPages(accessor);
  }

  // 批量加载打印页面
  function batchRenderPrintPages(accessor: any) {
    const { completePagesLayout, printRenderInstances, printConfig } = state;

    // 已经全部渲染完成，直接返回
    if (printRenderInstances.length >= completePagesLayout.length) {
      createPrintEle();

      return;
    }

    // 获取未渲染的页面数据
    const lastPageIndex = printRenderInstances.length;
    const batchData = completePagesLayout.slice(lastPageIndex, lastPageIndex + batchRenderSize) ?? [];

    if (!batchData.length) {
      return printRenderInstances;
    }

    // 批量创建渲染实例
    const renderInstances = batchData.map((pageLayout: any) => {
      const renderInstance = new JnpfSheetsPrintUiService(accessor, pageLayout, printConfig);
      renderInstance.container.style.cssText = 'width: 100%; height: 100%;';
      return renderInstance;
    });

    // 更新数据
    state.printRenderInstances.push(...renderInstances);

    emits('printRenderProgress', {
      total: state.completePagesLayout.length,
      rendered: state.printRenderInstances.length,
    });

    setTimeout(() => batchRenderPrintPages(accessor), 500);
  }

  // 创建打印节点
  function createPrintEle() {
    const { completePagesLayout, printRenderInstances, paperSize, printConfig } = state;
    const { w: paperSizeWidth = printA4PaperWidth, h: paperSizeHeight } = paperSize ?? {};
    const { paperType, direction } = printConfig ?? {};
    const printStyle = getPrintPageStyle(paperType, direction);

    const containerEle = document.createElement('div');
    containerEle.id = 'jnpfReportPrint';

    printRenderInstances.forEach((renderInstance: any, index: number) => {
      const pageLayout = completePagesLayout[index];

      if (pageLayout) {
        const { browserPreviewScale } = pageLayout ?? {};
        const scale = Math.round(browserPreviewScale * (printA4PaperWidth / paperSizeWidth) * 10) / 10;

        const pageEle = document.createElement('div');
        pageEle.className = 'printContainer';

        Object.assign(pageEle.style, {
          width: `${paperSizeWidth * scale}px`,
          height: `${paperSizeHeight * scale}px`,
        });

        pageEle.appendChild(renderInstance.container);
        containerEle.appendChild(pageEle);
      } else {
        containerEle.appendChild(renderInstance.container);
      }
    });

    document.head.appendChild(printStyle);
    document.body.appendChild(containerEle);

    nextTick(() => {
      setTimeout(() => {
        emits('browserPrintShow');

        window.onbeforeprint = () => {
          printRenderInstances.forEach((renderInstance: any) => {
            renderInstance?.setDirty(true); // 设置脏状态（用于调整视口大小和滚动值的状态）
            renderInstance?.renderPage(); // 渲染页面
            renderInstance?.renderOnReady(); // 主要是在页面变换时确保页面内容的及时更新，并且做了订阅和销毁的管理。
          });
        };

        window.onafterprint = () => {
          printRenderInstances.forEach(async (renderInstance: any) => {
            await renderInstance.dispose();
            renderInstance = null;
          });
          document.head.removeChild(printStyle);
          document.body.removeChild(containerEle);
        };

        window.print();
      }, 100);
    });
  }

  // 暴露 - 重置
  function resetState() {
    if (pagesContainerRef.value) {
      pagesContainerRef.value.scrollTop = 0;
    }

    state.completePagesLayout = [];
    state.processPagesLayout = [];

    state.printRenderInstances = [];

    state.pagingTargetQuantity = 0;
    state.pagingRenderedQuantity = 0;
    state.totalRenderedQuantity = 0;

    state.printConfig = null;
    state.paperSize = null;
    state.paperViewHeight = printA4PaperHeight;

    state.currentViewPageNumber = 1;

    offPagesContainerScroll();
  }

  defineExpose({ handlePrintRender, handleBrowserPrint, resetState });
</script>

<style lang="less">
  .print-render-content {
    flex: 1;
    overflow: auto;
    display: flex;
    flex-direction: column;

    .tools-wrap {
      display: flex;
      justify-content: center;
      user-select: none;
      padding: 10px 0;
      min-width: 500px;

      .first-page,
      .prev-page,
      .next-page,
      .last-page {
        //width: 32px;
        height: 32px;
        padding: 0 8px;
        display: flex;
        justify-content: center;
        align-items: center;
        cursor: pointer;
        border-radius: 6px;
        transition: all 0.2s;
        color: rgba(0, 0, 0, 0.88);

        .icon {
          fill: rgba(0, 0, 0, 0.88);
        }

        &.disabled {
          cursor: not-allowed;
          color: rgba(0, 0, 0, 0.25);

          .icon {
            fill: rgba(0, 0, 0, 0.25);
          }
        }

        em {
          font-size: 14px;
          font-style: normal;
          margin-left: 2px;
        }

        &:hover {
          background: rgba(0, 0, 0, 0.06);
        }
      }

      .target-page {
        -webkit-appearance: none;
        -moz-appearance: textfield;
        appearance: none;
        outline: none;

        width: 48px;
        height: 32px;

        border: 1px solid #d9d9d9;
        background: #ffffff;

        color: rgba(0, 0, 0, 0.88);
        font-size: 14px;
        text-align: center;

        border-radius: 6px;
        box-sizing: border-box;
        transition: all 0.2s;

        &::-webkit-inner-spin-button,
        &::-webkit-outer-spin-button {
          -webkit-appearance: none;
          margin: 0;
        }
      }

      .total-page-number {
        line-height: 32px;
        color: rgba(0, 0, 0, 0.88);

        display: flex;

        em {
          font-style: normal;
          margin: 0 4px;
          color: #1677ff;
        }

        strong {
          font-weight: normal;
        }
      }
    }

    .print-render-pages-content {
      flex: 1;
      overflow: auto;
      position: relative;

      .print-page-container:first-child {
        margin-top: 0;
      }
    }
  }
</style>
