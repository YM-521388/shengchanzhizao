<template>
  <a-popover trigger="click" placement="bottom" overlayClassName="jnpf-common-history-popover" arrow-point-at-center destroyTooltipOnHide>
    <template #content>
      <div class="history-popover-content">
        <div class="title"><i class="icon-ym icon-ym-flow-history" />历史记录</div>
        <ScrollContainer class="contain">
          <div
            v-if="recordList.length"
            class="item"
            v-for="(item, index) in recordList"
            :key="index"
            @click="handleJump(item)"
            :class="{ 'current-item': activeIndex == item.id, 'past-item': activeIndex < item.id }">
            <i :class="item.icon" v-if="item.icon" />
            {{ item.fullName }}
          </div>
          <jnpf-empty class="my-85px" v-else />
        </ScrollContainer>
      </div>
    </template>
    <a-tooltip placement="top" title="历史记录">
      <a-button type="text" class="jnpf-history-btn">
        <i class="icon-ym icon-ym-flow-history" />
      </a-button>
    </a-tooltip>
  </a-popover>
</template>

<script lang="ts" setup>
  import { ScrollContainer } from '@/components/Container';

  defineProps({
    activeIndex: { type: Number, default: 0 },
    recordList: { type: Array as PropType<any[]>, default: () => [] },
  });
  const emit = defineEmits(['jump']);

  function handleJump(item) {
    emit('jump', item.id);
  }
</script>
