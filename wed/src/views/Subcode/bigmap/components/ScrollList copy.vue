<!-- ScrollList.vue -->
<template>
  <div ref="wrapperRef" class="scroll-wrapper">
    <!-- 当需要滚动时使用 vue3-seamless-scroll -->
    <vue3-seamless-scroll v-if="shouldScroll" :list="data" :step="step" :hover="hover" :limitScrollNum="data.length"
      :singleHeight="itemHeight" :copyNum="copyNum" class="scroll-box">
      <div class="scroll-content" :style="{ height: contentHeight + 'px' }">
        <div v-for="(item, index) in data" :key="item.id" class="scroll-item"
          :style="{ height: itemHeight + 'px', lineHeight: itemHeight + 'px' }">
          <slot name="item" :item="item" :index="index">
          </slot>
        </div>
      </div>
    </vue3-seamless-scroll>

    <!-- 当不需要滚动时直接渲染静态列表 -->
    <div v-else class="static-list">
      <div class="scroll-content">
        <div v-for="(item, index) in data" :key="item.id" class="scroll-item"
          :style="{ height: itemHeight + 'px', lineHeight: itemHeight + 'px' }">
          <slot name="item" :item="item" :index="index">
          </slot>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onBeforeUnmount, computed, watch } from 'vue'
import { Vue3SeamlessScroll } from "vue3-seamless-scroll";

interface Props {
  /** 列表数据 */
  data: any[]
  /** 每项高度（px），默认42 */
  itemHeight?: number
  /** 滚动步长，默认0.3 */
  step?: number
  /** 鼠标悬停是否暂停，默认true */
  hover?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  itemHeight: 42,
  step: 0.3,
  hover: true
})

// DOM 引用
const wrapperRef = ref<HTMLElement>()
// 父级容器高度
const parentHeight = ref(0)

// 内容总高度
const contentHeight = computed(() => props.data.length * props.itemHeight)

// 判断是否需要滚动 - 内容高度大于父级容器高度就滚动
const shouldScroll = computed(() => {
  if (props.data.length === 0) return false
  return contentHeight.value > parentHeight.value
})

// 计算需要克隆的份数 - 交给 vue3-seamless-scroll 处理
const copyNum = computed(() => {
  if (!shouldScroll.value) return 1
  return 3 // 让 vue3-seamless-scroll 自己处理克隆份数
})

// 格式化显示项
const formatItem = (item: any): string => {
  if (typeof item === 'string') return item
  if (typeof item === 'number') return String(item)
  if (item.name && item.value) return `${item.name} - ${item.value}`
  return JSON.stringify(item)
}

// 获取父级容器高度
const updateParentHeight = () => {
  if (wrapperRef.value?.parentElement) {
    parentHeight.value = wrapperRef.value.parentElement.clientHeight
  }
}

// 监听父级容器大小变化
let resizeObserver: ResizeObserver | null = null

onMounted(() => {
  updateParentHeight()

  if (wrapperRef.value?.parentElement) {
    resizeObserver = new ResizeObserver(updateParentHeight)
    resizeObserver.observe(wrapperRef.value.parentElement)
  }
})

onBeforeUnmount(() => {
  resizeObserver?.disconnect()
})

// 监听数据变化
watch(() => props.data, updateParentHeight, { deep: true })

// 暴露方法
defineExpose({
  shouldScroll: () => shouldScroll.value,
  getParentHeight: () => parentHeight.value
})
</script>

<style scoped>
.scroll-wrapper {
  width: 100%;
  height: 100%;
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
  overflow: visible;
}

.scroll-content {
  display: flex;
  flex-direction: column;
}

.scroll-item {
  display: flex;
  align-items: center;
  /* padding: 0 16px;
  border-bottom: 1px solid rgba(255, 255, 255, 0.1);
  box-sizing: border-box; */
  /* color: #fff;
  font-size: 16px;
  overflow: hidden;
  white-space: nowrap;
  text-overflow: ellipsis; */
  background-color: #233E53;

}

.scroll-item:nth-child(2n) {
  background-color: #034669;
}

.scroll-item:last-child {
  border-bottom: none;
}
</style>