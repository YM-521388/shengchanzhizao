<template>
  <div :class="`${prefixCls}__wrap`" style="position:relative">
    <EditTableHeaderCell v-if="getIsEdit">
      {{ getTitle }}
    </EditTableHeaderCell>
    <span v-else>{{ getTitle }}</span>
    <BasicHelp v-if="getHelpMessage" :text="getHelpMessage" :class="`${prefixCls}__help`" />
    <div
      v-if="isResizable"
      class="jnpf-table-resize-handle"
      @mousedown.prevent="onMouseDown"
      title="调整列宽"
    />
  </div>
</template>
<script lang="ts">
  import type { PropType } from 'vue';
  import type { BasicColumn } from '../types/table';
  import { defineComponent, computed, ref, onBeforeUnmount } from 'vue';
  import BasicHelp from '@/components/Basic/src/BasicHelp.vue';
  import EditTableHeaderCell from './EditTableHeaderIcon.vue';
  import { useDesign } from '@/hooks/web/useDesign';

  export default defineComponent({
    name: 'TableHeaderCell',
    components: {
      EditTableHeaderCell,
      BasicHelp,
    },
    props: {
      column: {
        type: Object as PropType<BasicColumn>,
        default: () => ({}),
      },
    },
    setup(props) {
      const { prefixCls } = useDesign('basic-table-header-cell');

      const getIsEdit = computed(() => !!props.column?.edit);
      const getTitle = computed(() => {
        const title = props.column?.customTitle || props.column?.title;
        if (typeof title !== 'string') return '';
        return title;
      });
      const getHelpMessage = computed(() => props.column?.helpMessage);

      const isResizable = computed(() => !!props.column?.resizable);

      const startX = ref(0);
      const startWidth = ref<number>(0);
      function onMouseMove(e: MouseEvent) {
        const delta = e.clientX - startX.value;
        const newW = Math.max(40, Math.round(startWidth.value + delta));
        try {
          // 直接修改 column 对象的 width，columns 在上层已为 reactive，页面会响应
          (props.column as any).width = newW;
        } catch (err) {
          // ignore
        }
      }
      function onMouseUp() {
        window.removeEventListener('mousemove', onMouseMove);
        window.removeEventListener('mouseup', onMouseUp);
      }
      function onMouseDown(e: MouseEvent) {
        e.stopPropagation();
        startX.value = e.clientX;
        startWidth.value = Number((props.column as any).width) || (e.target as HTMLElement)?.parentElement?.offsetWidth || 100;
        window.addEventListener('mousemove', onMouseMove);
        window.addEventListener('mouseup', onMouseUp);
      }
      onBeforeUnmount(() => {
        window.removeEventListener('mousemove', onMouseMove);
        window.removeEventListener('mouseup', onMouseUp);
      });

      return { prefixCls, getIsEdit, getTitle, getHelpMessage, isResizable, onMouseDown };
    },
  });
</script>
<style lang="less">
  @prefix-cls: ~'@{namespace}-basic-table-header-cell';

  .@{prefix-cls} {
    &__help {
      margin-left: 8px;
      color: rgb(0 0 0 / 65%) !important;
    }
    &__wrap {
      display: inline-flex;
      align-items: center;
    }
  }
  .jnpf-table-resize-handle {
    position: absolute;
    right: 0;
    top: 0;
    width: 8px;
    height: 100%;
    cursor: col-resize;
    z-index: 20;
  }
</style>
