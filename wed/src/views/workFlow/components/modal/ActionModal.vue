<template>
  <BasicModal v-bind="$attrs" @register="registerModal" :title="state.title" @ok="handleSubmit" :minHeight="52">
    <a-form :colon="false" :labelCol="{ style: { width: '80px' } }" :model="dataForm" ref="formElRef">
      <template v-if="['transfer', 'assign', 'assist'].includes(eventType)">
        <a-form-item label="指派节点" name="nodeCode" v-if="eventType === 'assign'" :rules="[{ required: true, message: '必填', trigger: 'change' }]">
          <jnpf-select v-model:value="dataForm.nodeCode" :options="state.assignNodeList" showSearch />
        </a-form-item>
        <a-form-item
          :label="label + '给谁'"
          name="handleIds"
          v-if="['transfer', 'assign', 'assist'].includes(eventType)"
          :rules="[{ required: true, message: '必填', trigger: 'change' }]">
          <jnpf-user-select v-model:value="dataForm.handleIds" :multiple="eventType == 'assist' ? true : false" />
        </a-form-item>
      </template>
      <a-form-item label="暂停类型" name="pause" v-if="eventType === 'pause'">
        <a-select v-model:value="dataForm.pause" placeholder="请选择">
          <a-select-option :value="0">全部流程暂停</a-select-option>
          <a-select-option :value="1" v-if="pauseType">主流程暂停</a-select-option>
        </a-select>
      </a-form-item>
      <a-form-item :label="`${label}原因`" name="handleOpinion">
        <jnpf-textarea v-model:value="dataForm.handleOpinion" placeholder="请输入" />
      </a-form-item>
      <a-form-item label="手写签名" name="signImg" v-if="properties?.hasSign">
        <jnpf-sign v-model:value="dataForm.signImg" isInvoke />
      </a-form-item>
      <a-form-item :label="`${label}附件`" name="fileList" v-if="properties?.hasFile">
        <jnpf-upload-file v-model:value="dataForm.fileList" type="workFlow" :limit="3" />
      </a-form-item>
    </a-form>
  </BasicModal>
</template>
<script lang="ts" setup>
  import { reactive, ref, toRefs, nextTick, computed, unref } from 'vue';
  import { BasicModal, useModalInner } from '@/components/Modal';
  import type { FormInstance } from 'ant-design-vue';
  import { useUserStore } from '@/store/modules/user';

  interface State {
    dataForm: any;
    properties: any;
    eventType: string;
    title: string;
    label: string;
    assignNodeList: any[];
    pauseType: boolean;
  }

  const emit = defineEmits(['register', 'confirm']);
  const userStore = useUserStore();
  const formElRef = ref<FormInstance>();
  const state = reactive<State>({
    dataForm: {
      handleOpinion: '',
      handleIds: '',
      nodeCode: '',
      fileList: [],
      signImg: '',
      pause: 0,
    },
    properties: {},
    eventType: '',
    title: '',
    label: '',
    assignNodeList: [],
    pauseType: false,
  });
  const { dataForm, eventType, label, properties, pauseType } = toRefs(state);
  const [registerModal, { changeOkLoading }] = useModalInner(init);

  const getUserInfo = computed(() => userStore.getUserInfo || {});

  function init(data) {
    changeOkLoading(false);
    state.dataForm = {
      handleOpinion: '',
      handleIds: '',
      nodeCode: '',
      fileList: [],
      signImg: '',
      pause: 0,
    };
    state.properties = data.properties;
    state.eventType = data.eventType;
    state.pauseType = data.pauseType;
    if (state.properties.hasSign) state.dataForm.signImg = unref(getUserInfo).signImg;
    state.assignNodeList = (data.assignNodeList || []).map(o => ({ id: o.nodeCode, fullName: o.nodeName }));
    switch (data.eventType) {
      case 'transfer':
        state.title = data.properties?.type === 'processing' ? '转办' : '转审';
        state.label = data.properties?.type === 'processing' ? '转办' : '转审';
        break;
      case 'assist':
        state.title = '协办';
        state.label = '协办';
        break;
      case 'revoke':
        state.title = '撤销申请';
        state.label = '撤销';
        break;
      case 'launchRecall':
        state.title = '撤回流程';
        state.label = '撤回';
        break;
      case 'auditRecall':
        state.title = '撤回审核';
        state.label = '撤回';
        break;
      case 'assign':
        state.title = '指派';
        state.label = '指派';
        break;
      case 'cancel':
        state.title = '终止流程';
        state.label = '终止';
        break;
      case 'pause':
        state.title = '暂停流程';
        state.label = '暂停';
        break;
      case 'resurgence':
        state.title = '复活流程';
        state.label = '复活';
        break;
      default:
        break;
    }
    nextTick(() => {
      formElRef.value?.clearValidate();
    });
  }
  async function handleSubmit() {
    try {
      const values = await formElRef.value?.validate();
      if (!values) return;
      changeOkLoading(true);
      emit('confirm', { ...state.dataForm, handleIds: state.eventType == 'assist' ? state.dataForm.handleIds.join(',') : state.dataForm.handleIds });
    } catch (_) {}
  }
</script>
