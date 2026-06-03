<template>
  <teleport to="body">
    <transition name="file-viewer-fade">
      <div v-if="innerVisible" class="file-viewer-fullscreen" @keydown.esc="handleClose">
        <div class="file-viewer-header">
          <!-- <span class="file-viewer-title">{{ filename || '文件预览' }}</span> -->
          <a-button type="text" class="file-viewer-close" @click="handleClose">
            <CloseOutlined />
          </a-button>
        </div>
        <div class="file-viewer-body" v-if="innerVisible && file" ref="containerRef"></div>
      </div>
    </transition>
  </teleport>
</template>

<script setup lang="ts">
  import { onUnmounted, ref, watch, nextTick } from 'vue';
  import { createViewer, type ViewerInstance } from 'jit-viewer';
  import 'jit-viewer/style.css';
  import { CloseOutlined } from '@ant-design/icons-vue';
  import { func } from 'vue-types';

  const props = withDefaults(
    defineProps<{
      /** 控制显隐 */
      visible?: boolean;
      /** 文件源：URL 字符串、File 对象、Blob 或 ArrayBuffer */
      file?: string | File | Blob | ArrayBuffer;
      /** 文件名（用于判断文件类型、标题和下载） */
      filename?: string;
      /** 主题 */
      theme?: 'light' | 'dark';
      /** 是否显示工具栏 */
      toolbar?: boolean;
      /** 语言 */
      locale?: 'zh-CN' | 'en';
      /** 水印配置（不传则不显示水印） */
      watermark?: {
        /** 水印类型：文字 或 图片 */
        type: 'text' | 'image';
        /** 文字水印内容 */
        content?: string;
        /** 图片水印 URL */
        image?: string;
        /** 字体大小，默认 16 */
        fontSize?: number;
        /** 字体颜色，默认 rgba(0,0,0,0.15) */
        color?: string;
        /** 透明度 0-1，默认 0.15 */
        opacity?: number;
        /** 旋转角度，默认 -22 */
        rotate?: number;
        /** 水平间距，默认 100 */
        gapX?: number;
        /** 垂直间距，默认 80 */
        gapY?: number;
        /** 图片宽度（图片水印时） */
        imageWidth?: number;
        /** 图片高度（图片水印时） */
        imageHeight?: number;
      };
    }>(),
    {
      visible: false,
      file: undefined,
      filename: undefined,
      theme: 'light',
      toolbar: true,
      locale: 'zh-CN',
      watermark: undefined,
    },
  );

  const emit = defineEmits<{
    'update:visible': [value: boolean];
  }>();

  const innerVisible = ref(props.visible);
  const containerRef = ref<HTMLDivElement | null>(null);
  let viewer: ViewerInstance | null = null;

  watch(
    () => props.visible,
    val => {
      innerVisible.value = val;
      if (val) {
        document.body.style.overflow = 'hidden';
        nextTick(() => mountViewer());
      } else {
        document.body.style.overflow = '';
        destroyViewer();
      }
    },
  );

  watch(innerVisible, val => {
    emit('update:visible', val);
  });

  function handleClose() {
    innerVisible.value = false;
    document.body.style.overflow = '';
    destroyViewer();
  }

  async function mountViewer() {
    await nextTick();
    if (!containerRef.value || !props.file) return;

    destroyViewer();

    const options: any = {
      target: containerRef.value,
      theme: props.theme,
      locale: props.locale,
      width: '100%',
      height: '100%',
    };

    // 工具栏配置：排除下载按钮
    options.toolbar = {
      visible: props.toolbar,
      items: [
        { type: 'zoom' },
        { type: 'rotate' },
        { type: 'pagination' },
        { type: 'print' },
        { type: 'fullscreen' },
        // 不包含 { type: 'download' } → 隐藏下载按钮
      ],
    };

    // 如果 file 是 URL 字符串且提供了 filename，使用 FileSource 对象格式确保下载时使用正确的文件名
    if (typeof props.file === 'string' && props.filename) {
      options.file = { url: props.file, method: 'GET' };
    } else {
      options.file = props.file;
    }
    if (props.filename) {
      options.filename = props.filename;
    }
    // 水印配置
    // 文字水印
    // :watermark="{ type: 'text', content: '内部文件 禁止外传', color: 'rgba(0,0,0,0.1)' }"
    // 图片水印
    //  :watermark="{ type: 'image', image: '/logo.png', opacity: 0.08 }"
    if (props.watermark) {
      options.watermark = props.watermark;
    }
    viewer = createViewer(options);
    await viewer.mount();
  }

  function destroyViewer() {
    viewer?.destroy();
    viewer = null;
  }

  onUnmounted(() => {
    destroyViewer();
    if (innerVisible.value) {
      document.body.style.overflow = '';
    }
  });

  defineExpose({
    getViewer: () => viewer,
  });
</script>

<style scoped>
  .file-viewer-fullscreen {
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    z-index: 99999;
    background: #fff;
    display: flex;
    flex-direction: column;
  }
  .file-viewer-header {
    display: flex;
    align-items: center;
    justify-content: right;
    height: 48px;
    padding: 0 16px;
    border-bottom: 1px solid #e8e8e8;
    flex-shrink: 0;
  }
  .file-viewer-title {
    font-size: 16px;
    font-weight: 500;
    overflow: hidden;
    text-overflow: ellipsis;
    white-space: nowrap;
    max-width: calc(100% - 60px);
  }
  .file-viewer-close {
    font-size: 18px;
    cursor: pointer;
  }
  .file-viewer-body {
    flex: 1;
    overflow: hidden;
  }
  /* 隐藏 jit-viewer 内部文件名标题行的空白区域 */
  .file-viewer-body :deep(.jv-title),
  .file-viewer-body :deep([class*='jv-toolbar__bottom']),
  /* 禁止隐藏版权信息 */
  /* .file-viewer-body :deep([class*='jv-viewer__branding']), */
  .file-viewer-body :deep([class*='header']) {
    display: none !important;
  }
  .file-viewer-fade-enter-active,
  .file-viewer-fade-leave-active {
    transition: opacity 0.2s ease;
  }
  .file-viewer-fade-enter-from,
  .file-viewer-fade-leave-to {
    opacity: 0;
  }
</style>
