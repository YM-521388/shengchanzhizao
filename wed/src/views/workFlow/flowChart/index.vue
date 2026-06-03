<template>
  <FlowProcessMain
    class="pt-10px"
    :flowInfo="state.flowInfo"
    :nodeList="state.nodeList"
    :isPreview="true"
    :noShowScale="true"
    v-if="state.flowInfo.id"
    :key="state.key" />
</template>
<script lang="ts" setup>
  import { reactive, onMounted, nextTick } from 'vue';
  import FlowProcessMain from '@/components/FlowProcess/src/Main.vue';
  import { getFlowTaskInfo } from '@/api/workFlow/task';
  import { useRoute } from 'vue-router';
  import { useUserStore } from '@/store/modules/user';

  interface State {
    nodeList: any;
    flowInfo: any;
    loading: boolean;
    key: number;
  }

  const state = reactive<State>({
    nodeList: {},
    flowInfo: {},
    loading: false,
    key: +new Date(),
  });
  const route = useRoute();
  const userStore = useUserStore();

  function init() {
    const query = route.query;
    if (!query.token) return;
    state.loading = true;
    userStore.updateToken(query.token as string);
    nextTick(() => {
      getBeforeInfo(query);
    });
  }
  function getBeforeInfo(query) {
    getFlowTaskInfo(query.id || '0', { operatorId: query.operatorId, flowId: query.flowId, opTypw: query.opTypw })
      .then(res => {
        state.flowInfo = res.data.flowInfo || {};
        state.nodeList = res.data.nodeList || [];
        state.loading = false;
        state.key = +new Date();
      })
      .catch(() => {
        state.loading = false;
      });
  }

  onMounted(() => {
    init();
  });
</script>
