<template>
  <BasicModal v-bind="$attrs" @register="registerModal" title="异常详情" destroyOnClose :footer="null">
    <a-form :labelCol="{ style: { width: '90px' } }" layout="vertical" class="h-600px">
      <div class="mb-20px">
        <label class="ant-form-item-no-colon">错误提示：</label>
        {{ dataForm.errorTip }}
      </div>
      <a-form-item label="错误内容：">
        <jnpf-textarea v-model:value="dataForm.errorData" :rows="17" readOnly />
      </a-form-item>
    </a-form>
  </BasicModal>
</template>
<script lang="ts" setup>
  import { ref } from 'vue';
  import { BasicModal, useModalInner } from '@/components/Modal';

  const dataForm = ref({
    errorTip: '',
    errorData: '',
  });
  const [registerModal] = useModalInner(init);

  function init(data) {
    dataForm.value = data;
  }
</script>
