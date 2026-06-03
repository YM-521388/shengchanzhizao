<template>
  <BasicPopup v-bind="$attrs" @register="registerPopup" :title="title" :showCancelBtn="false" destroyOnClose class="full-popup basic-flow-parser">
    <div class="jnpf-common-form-wrapper" v-loading="loading">
      <div class="jnpf-common-form-wrapper__main p-10px">
        <component :is="currentView" :config="config" />
      </div>
    </div>
  </BasicPopup>
</template>
<script lang="ts" setup>
  import { reactive, toRefs, markRaw, defineAsyncComponent } from 'vue';
  import { BasicPopup, usePopupInner } from '@/components/Popup';
  import { getStartFormInfo } from '@/api/workFlow/task';
  import { importViewsFile } from '@/utils';

  interface State {
    config: any;
    loading: boolean;
    currentView: any;
    formData: any;
    formInfo: any;
    title: string;
  }

  defineEmits(['register']);
  const [registerPopup] = usePopupInner(init);

  const state = reactive<State>({
    config: {},
    loading: false,
    currentView: null,
    formData: {},
    formInfo: {},
    title: '',
  });
  const { config, loading, currentView, title } = toRefs(state);

  function init(data) {
    state.title = data.title;
    state.currentView = null;
    state.loading = true;
    state.config = data;
    getBeforeInfo(state.config);
  }
  function getBeforeInfo(config) {
    getStartFormInfo(config.taskId)
      .then(res => {
        state.formInfo = res.data.formInfo;
        config.formData = res.data.formData || {};
        config.formConf = state.formInfo.formData;
        config.disabled = true;
        const formUrl =
          state.formInfo.type == 1
            ? 'workFlow/workFlowForm/dynamicForm'
            : state.formInfo.urlAddress
            ? state.formInfo.urlAddress.replace(/\s*/g, '')
            : `workFlow/workFlowForm/${state.formInfo.enCode}`;
        state.currentView = markRaw(defineAsyncComponent(() => importViewsFile(formUrl)));
        state.loading = false;
      })
      .catch(() => {
        state.loading = false;
      });
  }
</script>
