<template>
  <div ref="floatImageContainerRef" :id="props.id" class="jnpf-univer-float-image-ele">
    <img id="imageEle" src="" alt="" />
  </div>
</template>

<script lang="ts" setup>
  import { onMounted, ref, watch } from 'vue';
  import { storeToRefs } from 'pinia';
  import { useJnpfUniverStore } from '../../../store';

  defineOptions({ name: 'JnpfUniverFloatImage' });

  interface PropsType {
    id: string;
    piniaStoreId: string;
  }
  const props = defineProps<PropsType>();
  const floatImageContainerRef = ref<HTMLDivElement | null>(null);

  const jnpfUniverStore = useJnpfUniverStore(props?.piniaStoreId);
  const { univerCreateModeCache, focusedFloatImageDataCache, floatImageToUniverImgDataCaches } = storeToRefs(jnpfUniverStore);

  // 初始化或更新图表
  function initialize(option: any) {
    if (!floatImageContainerRef.value) {
      return;
    }

    const imageEle = floatImageContainerRef.value?.querySelector('#imageEle') as HTMLImageElement | null;
    if (!imageEle) {
      return;
    }

    const { src = '', alt = '' } = option ?? {};
    imageEle.setAttribute('src', src);
    imageEle.setAttribute('alt', alt);

    // 非设计状态下，图片直接转成univer图片
    if (univerCreateModeCache.value !== 'design') {
      const { id: domId } = props ?? {};

      if (domId) {
        const caches = {
          ...floatImageToUniverImgDataCaches.value,
          [domId]: src,
        };

        jnpfUniverStore?.setFloatImageToUniverImgDataCaches(caches);
      }
    }
  }

  onMounted(() => {
    const { id: domId, piniaStoreId } = props ?? {};

    if (!floatImageContainerRef.value || !domId || !piniaStoreId) {
      return;
    }

    const floatImageDataCaches = jnpfUniverStore?.floatImageDataCaches ?? {};

    const targetOption = Object.values(floatImageDataCaches as Record<string, { domId: string; option: any }>)?.find(item => item?.domId === domId)?.option;
    initialize(targetOption);
  });

  watch(
    () => focusedFloatImageDataCache.value,
    value => {
      const { domId, drawingId, option } = value;

      if (!domId && !drawingId) {
        // store置空的情况下，不响应
        return;
      }

      const containerId = floatImageContainerRef.value?.getAttribute('id');
      if (domId !== containerId) {
        return;
      }

      initialize(option);
    },
  );
</script>

<style lang="less" scoped>
  .jnpf-univer-float-image-ele {
    position: absolute;
    width: 100%;
    height: 100%;
    background: #fff;

    img {
      display: block;
      width: 100%;
      height: 100%;
    }
  }
</style>
