<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    width="80%"
    :minHeight="750"
    okText="确定"
    cancelText="取消"
    @ok="handleSubmit"
    :closeFunc="onClose"
    :footer="null"
    :canFullscreen="true">
    <template #title>
      <a-space :size="10">
        <div class="text-16px font-medium">{{ title }}</div>
      </a-space>
    </template>
    <template #insertToolbar>
      <div class="float-left mt-5px">
        <JnpfCheckboxSingle v-model:value="submitType" :label="continueText" />
      </div>
    </template>
    <div class="content-container">
      <FlowProcessMain
        :flowInfo="state.flowInfo"
        :nodeList="state.nodeList"
        :isPreview="true"
        :lineKeyList="state.lineKeyList"
        v-if="state.flowInfo.id"
        :key="flowKey"
        formType="detail" />
    </div>
  </BasicModal>
</template>
<script lang="ts" setup>
  import { create, update, getInfo, getUserCon } from '@/views/Subcode/aProdTask/helper/api';
  import { getFlowInfo } from './helper/api';

  import { reactive, toRefs, nextTick, ref, unref, computed, toRaw, inject, provide } from 'vue';
  import { BasicModal, useModal } from '@/components/Modal';
  import SelectModal from '@/components/CommonModal/src/SelectModal.vue';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useI18n } from '@/hooks/web/useI18n';
  import { useUserStore } from '@/store/modules/user';
  import type { FormInstance } from 'ant-design-vue';
  import FlowProcessMain from '@/components/FlowProcessWrok/src/Main.vue';

  interface State {
    title: string;
    submitType: number;
    continueText: string;
    isContinue: boolean;
    flowInfo: any;
    nodeList: any;
    lineKeyList: any;
    flowKey: number;
    taskId: string;
  }

  const emit = defineEmits(['reload']);
  const userStore = useUserStore();
  const userInfo = userStore.getUserInfo;
  const { createMessage, createConfirm } = useMessage();
  const { t } = useI18n();
  const [registerModal, { openModal, setModalProps }] = useModal();
  const formRef = ref<FormInstance>();
  const selectModal = ref(null);

  const state = reactive<State>({
    title: '',
    submitType: 0,
    continueText: '',
    isContinue: false,
    lineKeyList: [],
    nodeList: [],
    flowKey: 0,
    flowInfo: {},
    taskId: '',
  });
  const { title, continueText, submitType, flowKey } = toRefs(state);

  defineExpose({ init });

  // 刷新页面
  const refreshPage = () => {
    getFlowInfoData(state.taskId);
    emit('reload');
  };
  provide('refreshPage', refreshPage);

  function init(data) {
    state.submitType = 0;
    state.title = data.F_ProcessNo + '-生产任务';
    state.taskId = data.id;
    getFlowInfoData(data.id);
  }
  async function onClose() {
    if (state.isContinue) emit('reload');
    return true;
  }
  async function handleSubmit() {}
  // 获取加工单 生产计划信息.
  function getFlowInfoData(id) {
    getFlowInfo(id)
      .then(res => {
        state.flowInfo = {};
        state.flowInfo = res.data || {};
        state.flowKey = new Date().getTime();
        openModal();
      })
      .catch(err => {});
  }
</script>
<style lang="less" scoped>
  .content-container {
    width: 100%;
    height: 750px;
  }
</style>
