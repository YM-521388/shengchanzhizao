<template>
  <BasicPopup v-bind="$attrs" @register="registerPopup" :title="state.title" class="full-popup" destroyOnClose :closeFunc="onClose">
    <div class="h-full p-10px overflow-y-auto">
      <component :is="state.currentView" :config="state.config" @setPageLoad="setPageLoad" />
    </div>
  </BasicPopup>
</template>
<script lang="ts" setup>
  import { BasicPopup, usePopupInner } from '@/components/Popup';
  import { getConfigData } from '@/api/onlineDev/visualDev';
  import { defineAsyncComponent, reactive, markRaw } from 'vue';
  import { importViewsFile } from '@/utils';

  interface State {
    currentView: any;
    title: string;
    config: any;
  }

  defineEmits(['register']);
  const [registerPopup, { changeLoading }] = usePopupInner(init);
  const state = reactive<State>({
    currentView: null,
    title: '',
    config: {},
  });

  function init(data) {
    changeLoading(true);
    state.title = '预览表单【' + data.fullName + '】';
    getConfigData(data.modelId, { type: data.previewType }).then(res => {
      if (!res.data || !res.data.formData) return changeLoading(false);
      data.formConf = res.data.formData;
      data.formOperates = [];
      state.config = data;
      const formUrl = res.data.urlAddress ? res.data.urlAddress.replace(/\s*/g, '') : `workFlow/workFlowForm/${data.enCode}`;
      state.currentView = markRaw(defineAsyncComponent(() => importViewsFile(formUrl)));
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
