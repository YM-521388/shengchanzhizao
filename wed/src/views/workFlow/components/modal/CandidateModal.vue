<template>
  <BasicModal v-bind="$attrs" @register="registerModal" title="提交审核" @ok="handleSubmit" :minHeight="52" destroyOnClose @cancel="onClose">
    <a-form
      :colon="false"
      :labelCol="{ style: { width: dataForm.candidateList.length || branchList.length ? '130px' : '80px' } }"
      :model="dataForm"
      ref="formElRef"
      v-if="!loading">
      <a-form-item label="分支选择" name="branchList" :rules="[{ required: true, message: `必填`, trigger: 'change', type: 'array' }]" v-if="branchList.length">
        <jnpf-select v-model:value="dataForm.branchList" multiple placeholder="请选择" allowClear @change="onBranchChange" :options="branchList" showSearch />
      </a-form-item>
      <template v-for="(item, i) in dataForm.candidateList" :key="i">
        <a-form-item :label="item.nodeName + item.label" :name="['candidateList', i, 'value']" :rules="item.rules">
          <candidate-user-select
            v-model:value="item.value"
            multiple
            :query="{ ...state.formData, nodeCode: item.nodeCode, operatorId }"
            :api="getCandidateUser"
            modalTitle="候选人员"
            v-if="item.hasCandidates" />
          <jnpf-user-select v-model:value="item.value" multiple placeholder="请选择" modalTitle="候选人员" v-else />
        </a-form-item>
        <a-form-item label="已选审批人" v-if="item.selected" class="candidate-selected">
          <jnpf-textarea v-model:value="item.selected" :autoSize="{ minRows: 1, maxRows: 4 }" disabled />
        </a-form-item>
      </template>
      <a-form-item label="抄送人员" name="copyIds" v-if="isCustomCopy">
        <jnpf-user-select v-model:value="dataForm.copyIds" multiple allowClear />
      </a-form-item>
    </a-form>
  </BasicModal>
</template>
<script lang="ts" setup>
  import { reactive, ref, toRefs, nextTick } from 'vue';
  import { BasicModal, useModalInner } from '@/components/Modal';
  import type { FormInstance } from 'ant-design-vue';
  import { UserSelect as CandidateUserSelect } from '@/components/CommonModal';
  import { getCandidateUser } from '@/api/workFlow/task';
  import { omit } from 'lodash-es';

  interface State {
    dataForm: any;
    defaultCandidateList: any[];
    branchList: any[];
    isCustomCopy: boolean;
    formData: any;
    eventType: string;
    loading: boolean;
    operatorId: string;
  }

  const emit = defineEmits(['register', 'confirm']);

  const formElRef = ref<FormInstance>();
  const state = reactive<State>({
    dataForm: {
      copyIds: [],
      branchList: [],
      candidateList: [],
    },
    defaultCandidateList: [],
    branchList: [],
    isCustomCopy: false,
    formData: {},
    eventType: '',
    loading: false,
    operatorId: '',
  });
  const { dataForm, isCustomCopy, loading, operatorId, branchList } = toRefs(state);
  const [registerModal, { changeOkLoading }] = useModalInner(init);

  function init(data) {
    changeOkLoading(false);
    state.loading = true;
    state.isCustomCopy = data.isCustomCopy;
    state.eventType = data.eventType;
    state.formData = data.formData;
    state.operatorId = data.operatorId;
    state.dataForm.copyIds = [];
    state.dataForm.candidateList = [];
    state.dataForm.branchList = [];
    state.branchList = data.branchList.map(o => ({ ...omit(o, ['nodeCode', 'nodeName']), id: o.nodeCode, fullName: o.nodeName }));
    state.defaultCandidateList = data.candidateList.map(o => ({
      ...o,
      label: '审批人',
      value: [],
      rules: !o.selected ? [{ required: true, message: '必填', trigger: 'change', type: 'array' }] : [],
    }));
    state.dataForm.candidateList = state.defaultCandidateList;
    nextTick(() => {
      state.loading = false;
      formElRef.value?.clearValidate();
    });
  }
  async function handleSubmit() {
    try {
      const values = await formElRef.value?.validate();
      if (!values) return;
      let candidateList = {};
      for (let i = 0; i < state.dataForm.candidateList.length; i++) {
        candidateList[state.dataForm.candidateList[i].nodeCode] = state.dataForm.candidateList[i].value;
      }
      changeOkLoading(true);
      emit('confirm', {
        candidateList,
        copyIds: state.dataForm.copyIds.join(','),
        branchList: state.dataForm.branchList,
        eventType: state.eventType,
      });
    } catch (_) {}
  }
  function onClose() {
    state.dataForm.copyIds = [];
    state.dataForm.candidateList = [];
  }
  function onBranchChange(val) {
    if (!val.length) return (state.dataForm.candidateList = state.defaultCandidateList);
    let list: any[] = [];
    for (let i = 0; i < val.length; i++) {
      inner: for (let j = 0; j < state.branchList.length; j++) {
        let o = state.branchList[j];
        if (val[i] === o.id && o.isCandidates) {
          list.push({
            nodeCode: o.id,
            nodeName: o.fullName,
            isCandidates: o.isCandidates,
            hasCandidates: o.hasCandidates,
            label: '审批人',
            value: [],
            rules: !o.selected ? [{ required: true, message: `必填`, trigger: 'change', type: 'array' }] : [],
          });
          break inner;
        }
      }
    }
    state.dataForm.candidateList = [...state.defaultCandidateList, ...list];
  }
</script>
