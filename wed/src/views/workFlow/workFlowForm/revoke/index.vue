<template>
  <div class="flow-form">
    <a-form :colon="false" :labelCol="{ style: { width: '100px' } }" :model="dataForm" ref="formRef" :disabled="config.disabled">
      <a-row>
        <a-col :span="24">
          <a-form-item label="审批编号" name="billRule">
            <jnpf-input v-model:value="dataForm.billRule" disabled />
          </a-form-item>
        </a-col>
        <a-col :span="24">
          <a-form-item label="提交时间" name="creatorTime">
            <jnpf-date-picker v-model:value="dataForm.creatorTime" format="YYYY-MM-DD HH:mm:ss" disabled />
          </a-form-item>
        </a-col>
        <a-col :span="24">
          <a-form-item label="撤销理由" name="handleOpinion">
            <jnpf-textarea v-model:value="dataForm.handleOpinion" disabled />
          </a-form-item>
        </a-col>
        <a-col :span="24">
          <a-form-item label="关联流程" name="revokeFlow">
            <jnpf-link :content="dataForm.revokeTaskName" @click="openRevokeFlow" />
          </a-form-item>
        </a-col>
      </a-row>
    </a-form>
  </div>
</template>

<script lang="ts" setup>
  import { reactive, toRefs, onMounted, ref } from 'vue';
  import { useFlowForm } from '@/views/workFlow/workFlowForm/hooks/useFlowForm';
  import type { FormInstance } from 'ant-design-vue';

  interface State {
    dataForm: any;
    dataRule: any;
  }

  defineOptions({ name: 'revoke' });
  const props = defineProps(['config']);
  const emit = defineEmits(['setPageLoad', 'eventReceiver', 'openRevokeFlow']);
  const formRef = ref<FormInstance>();
  const state = reactive<State>({
    dataForm: {
      billRule: '',
      creatorTime: undefined,
      handleOpinion: '',
      revokeTaskName: '',
      revokeTaskId: '',
    },
    dataRule: {},
  });
  const { dataForm } = toRefs(state);
  const { init, dataFormSubmit } = useFlowForm({
    config: props.config,
    selfState: state,
    emit,
    formRef,
  });

  defineExpose({ dataFormSubmit, openRevokeFlow });

  function openRevokeFlow() {
    emit('openRevokeFlow', state.dataForm.revokeTaskId);
  }

  onMounted(() => {
    init();
  });
</script>
