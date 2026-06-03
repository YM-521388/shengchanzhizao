<template>
  <div class="jnpf-content-wrapper bg-white">
    <FlowParser @register="registerFlowParser" @reload="handleClose" />
  </div>
</template>
<script lang="ts" setup>
  import { onMounted, computed, unref, watch } from 'vue';
  import { useRoute } from 'vue-router';
  import { usePopup } from '@/components/Popup';
  import FlowParser from '@/views/workFlow/components/FlowParser.vue';
  import { decodeByBase64 } from '@/utils/cipher';
  import { checkInfo } from '@/api/workFlow/task';
  import { useTabs } from '@/hooks/web/useTabs';

  defineOptions({ name: 'workFlowDetail' });

  const route = useRoute();
  const { closeCurrent } = useTabs();
  const [registerFlowParser, { openPopup: openFlowParser }] = usePopup();

  const getConfig = computed(() => route.query.config);

  watch(
    () => unref(getConfig),
    () => {
      // initData();
    },
    { deep: true },
  );

  function initData() {
    // type 1-我发起的 2-待办 3-抄送
    if (!unref(getConfig)) return;
    const config = JSON.parse(decodeByBase64(unref(getConfig) as string));
    const data = {
      id: config.taskId,
      flowId: config.flowId,
      opType: config.opType,
      operatorId: config.operatorId,
      hideCancelBtn: true,
    };
    checkInfo(data.operatorId || data.id, data.opType)
      .then(res => {
        data.opType = res.data.opType;
        openFlowParser(true, data);
      })
      .catch(() => {
        closeCurrent();
      });
  }
  function handleClose(close = true) {
    close && closeCurrent();
  }

  onMounted(() => {
    initData();
  });
</script>
