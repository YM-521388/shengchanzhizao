<template>
  <BasicPopup v-bind="$attrs" @register="registerPopup" destroyOnClose :closeFunc="onClose" class="full-popup basic-flow-parser">
    <template #title>
      <div class="header-title">{{ config.title }}</div>
    </template>
    <div class="p-10px h-full overflow-auto">
      <component :is="state.currentView" ref="formRef" :config="config" @setPageLoad="setPageLoad" />
    </div>
  </BasicPopup>
</template>
<script lang="ts" setup>
  import { reactive, toRefs, defineAsyncComponent, markRaw, ref } from 'vue';
  import { BasicPopup, usePopupInner } from '@/components/Popup';
  import { importViewsFile } from '@/utils';
  import { getFlowTaskInfo } from '@/api/workFlow/task';

  interface State {
    config: any;
    properties: any;
    taskInfo: any;
    formInfo: any;
    flowInfo: any;
    formData: any;
    currentView: any;
  }

  defineEmits(['register']);
  const [registerPopup, { changeLoading }] = usePopupInner(init);
  const formRef = ref<any>(null);
  const state = reactive<State>({
    config: {},
    properties: {},
    taskInfo: {},
    formInfo: {},
    flowInfo: {},
    formData: {},
    currentView: null,
  });
  const { config } = toRefs(state);

  function init(data) {
    changeLoading(true);
    state.config = data;
    getBeforeInfo(state.config);
  }
  function getBeforeInfo(config) {
    const query = { taskNodeId: config.taskNodeId, operatorId: config.taskId, flowId: config.flowId, opType: config.opType };
    getFlowTaskInfo(config.id || '0', query)
      .then(res => {
        state.formInfo = res.data.formInfo;
        state.taskInfo = res.data.taskInfo || {};
        state.flowInfo = res.data.flowInfo;
        const fullName = state.taskInfo.fullName;
        config.fullName = fullName;
        config.type = state.flowInfo.type;
        config.draftData = res.data.draftData || null;
        config.formData = res.data.formData || {};
        config.formEnCode = state.formInfo.enCode;
        state.properties = res.data.nodeProperties || {};
        config.formConf = state.formInfo.formData;
        config.formOperates = res.data.formOperates || [];
        if (config.opType == 0) {
          for (let i = 0; i < config.formOperates.length; i++) {
            config.formOperates[i].write = false;
          }
        }
        const formUrl =
          state.formInfo.type == 1
            ? 'workFlow/workFlowForm/dynamicForm'
            : state.formInfo.urlAddress
            ? state.formInfo.urlAddress.replace(/\s*/g, '')
            : `workFlow/workFlowForm/${state.formInfo.enCode}`;
        state.currentView = markRaw(defineAsyncComponent(() => importViewsFile(formUrl)));
      })
      .catch(() => {
        changeLoading(false);
      });
  }
  function setPageLoad(val: any = false) {
    changeLoading(!!val);
  }
  function onClose() {
    state.currentView = null;
    return true;
  }
</script>
<style lang="less">
  @import './style/index.less';
</style>
