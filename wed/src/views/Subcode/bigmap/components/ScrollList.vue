<!-- ScrollList.vue - 支持动态高度版本 -->
<template>
  <div ref="wrapperRef" class="scroll-wrapper">
    <!-- 测量用的隐藏容器，用于计算实际高度 -->
    <div ref="measureRef" class="measure-container" style="position: absolute; visibility: hidden; width: 100%; pointer-events: none;">
      <div v-for="(item, index) in data" :key="`measure-${index}`" class="scroll-item-measure">
        <slot name="item" :item="item" :index="index"></slot>
      </div>
    </div>

    <!-- 当需要滚动时使用 vue3-seamless-scroll -->
    <vue3-seamless-scroll 
      v-if="shouldScroll" 
      :list="data" 
      :step="step" 
      :hover="hover" 
      :limitScrollNum="data.length"
      :singleHeight="actualItemHeight" 
      :copyNum="copyNum" 
      class="scroll-box">
      <div class="scroll-content" :style="{ height: totalHeight + 'px' }">
        <div v-for="(item, index) in data" :key="item.id" class="scroll-item"
          :style="{ height: actualItemHeight + 'px' }">
          <slot name="item" :item="item" :index="index"></slot>
        </div>
      </div>
    </vue3-seamless-scroll>

    <!-- 当不需要滚动时直接渲染静态列表 -->
    <div v-else class="static-list">
      <div class="scroll-content">
        <div v-for="(item, index) in data" :key="item.id" class="scroll-item"
          :style="{ height: actualItemHeight + 'px' }">
          <slot name="item" :item="item" :index="index"></slot>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onBeforeUnmount, computed, watch, nextTick } from 'vue'
import { Vue3SeamlessScroll } from "vue3-seamless-scroll";

interface Props {
  data: any[]
  itemHeight?: number
  step?: number
  hover?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  step: 0.3,
  hover: true
})

const wrapperRef = ref<HTMLElement>()
const measureRef = ref<HTMLElement>()
const parentHeight = ref(0)
const actualItemHeight = ref(props.itemHeight || 60) // 默认60px

// 计算实际项目高度
const calculateItemHeight = async () => {
  if (props.itemHeight) {
    actualItemHeight.value = props.itemHeight
    return
  }

  await nextTick()
  if (measureRef.value && measureRef.value.children.length > 0) {
    // 取第一个项目的高度作为标准高度
    const firstItem = measureRef.value.children[0] as HTMLElement
    if (firstItem && firstItem.offsetHeight > 0) {
      actualItemHeight.value = firstItem.offsetHeight
    }
  }
}

// 总高度
const totalHeight = computed(() => props.data.length * actualItemHeight.value)

// 判断是否需要滚动
const shouldScroll = computed(() => {
  if (props.data.length === 0) return false
  return totalHeight.value > parentHeight.value
})

const copyNum = computed(() => shouldScroll.value ? 3 : 1)

// 更新父容器高度
const updateParentHeight = () => {
  if (wrapperRef.value?.parentElement) {
    parentHeight.value = wrapperRef.value.parentElement.clientHeight
  }
}

// 监听窗口大小变化
const handleResize = () => {
  updateParentHeight()
  calculateItemHeight()
}

let resizeObserver: ResizeObserver | null = null

onMounted(() => {
  updateParentHeight()
  calculateItemHeight()

  // 监听父容器大小变化
  if (wrapperRef.value?.parentElement) {
    resizeObserver = new ResizeObserver(() => {
      updateParentHeight()
      calculateItemHeight()
    })
    resizeObserver.observe(wrapperRef.value.parentElement)
  }

  // 监听窗口大小变化
  window.addEventListener('resize', handleResize)
})

onBeforeUnmount(() => {
  resizeObserver?.disconnect()
  window.removeEventListener('resize', handleResize)
})

// 监听数据变化
watch(() => props.data, async () => {
  await calculateItemHeight()
  updateParentHeight()
}, { deep: true })

defineExpose({
  shouldScroll: () => shouldScroll.value,
  getParentHeight: () => parentHeight.value,
  getItemHeight: () => actualItemHeight.value
})
</script>

<style scoped>
.scroll-wrapper {
  width: 100%;
  height: 100%;
  position: relative;
  overflow: hidden;
}

.scroll-box {
  width: 100%;
  height: 100%;
  overflow: hidden;
}

.static-list {
  width: 100%;
  height: 100%;
  overflow-y: auto;
}

.scroll-content {
  display: flex;
  flex-direction: column;
}

.scroll-item {
  display: flex;
  align-items: center;
  box-sizing: border-box;
  background-color: #233E53;
}

.scroll-item:nth-child(2n) {
  background-color: #034669;
}

/* 测量容器样式 */
.measure-container {
  position: absolute;
  visibility: hidden;
  width: 100%;
  pointer-events: none;
  top: 0;
  left: 0;
  z-index: -1;
}

.scroll-item-measure {
  display: flex;
  align-items: center;
  box-sizing: border-box;
  background-color: transparent;
}
</style>