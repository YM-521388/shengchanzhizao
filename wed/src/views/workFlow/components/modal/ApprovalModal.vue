<template>
  <BasicModal v-bind="$attrs" @register="registerModal" :title="state.title" @ok="handleSubmit" :minHeight="52">
    <a-form
      :colon="false"
      :labelCol="{ style: { width: dataForm.candidateList?.length || branchList.length ? '130px' : '80px' } }"
      :model="dataForm"
      ref="formElRef">
      <template v-if="eventType === 'freeApprover'">
        <a-form-item label="加签人员" :name="['addSignParameter', 'addSignUserIdList']" :rules="[{ required: true, message: `必填`, trigger: 'change' }]">
          <jnpf-user-select v-model:value="dataForm.addSignParameter.addSignUserIdList" multiple allowClear />
        </a-form-item>
        <a-form-item label="加签类型" name="addSignType">
          <a-radio-group v-model:value="dataForm.addSignParameter.addSignType" button-style="solid">
            <a-radio-button :value="1">审批前</a-radio-button>
            <a-radio-button :value="2">审批后</a-radio-button>
          </a-radio-group>
        </a-form-item>
        <a-form-item label="审批方式" name="counterSign">
          <a-radio-group v-model:value="dataForm.addSignParameter.counterSign">
            <a-radio :value="0">或签</a-radio>
            <a-radio :value="1">会签</a-radio>
            <a-radio :value="2">依次审批</a-radio>
          </a-radio-group>
        </a-form-item>
        <a-form-item label="会签比例" name="auditRatio" v-if="dataForm.addSignParameter.counterSign === 1">
          <div class="flex items-center">
            <a-select v-model:value="dataForm.addSignParameter.auditRatio" class="flex-1 !mr-10px">
              <a-select-option v-for="item in 10" :key="item" :value="item * 10">{{ item * 10 + '%' }}</a-select-option>
            </a-select>
            达到会签比例则通过
          </div>
        </a-form-item>
      </template>
      <template v-if="eventType === 'audit' || eventType === 'reject' || (eventType === 'freeApprover' && dataForm.addSignParameter.addSignType === 2)">
        <a-form-item
          label="分支选择"
          name="branchList"
          :rules="[{ required: true, message: `必填`, trigger: 'change', type: 'array' }]"
          v-if="branchList.length">
          <jnpf-select v-model:value="dataForm.branchList" multiple placeholder="请选择" allowClear @change="onBranchChange" :options="branchList" showSearch />
        </a-form-item>
        <template v-for="(item, i) in dataForm.candidateList" :key="i">
          <a-form-item :label="item.nodeName + item.label" :name="['candidateList', i, 'value']" :rules="item.rules">
            <candidate-user-select
              v-model:value="item.value"
              multiple
              :placeholder="'请选择' + item.label"
              :query="{ ...state.formData, nodeCode: item.nodeCode, operatorId: state.operatorId }"
              :api="getCandidateUser"
              modalTitle="候选人员"
              v-if="item.hasCandidates" />
            <jnpf-user-select v-model:value="item.value" multiple :placeholder="'请选择' + item.label" modalTitle="候选人员" v-else />
          </a-form-item>
          <a-form-item label="已选审批人" v-if="item.selected" class="candidate-selected">
            <jnpf-textarea v-model:value="item.selected" :autoSize="{ minRows: 1, maxRows: 4 }" disabled />
          </a-form-item>
        </template>
      </template>
      <template v-if="eventType === 'back' && properties.backType">
        <a-form-item label="退回节点" name="backNodeCode" :rules="[{ required: true, message: `必填`, trigger: 'change' }]">
          <jnpf-select v-model:value="dataForm.backNodeCode" :options="state.backNodeCodeList" :disabled="properties.backNodeCode !== 2" showSearch />
        </a-form-item>
        <a-form-item label=" " name="backType" v-if="properties.backType == 3">
          <a-radio-group v-model:value="dataForm.backType">
            <a-radio :value="1">重新审批<BasicHelp text="若流程为A->B->C，C退回至A，则C->A->B->C" /></a-radio>
            <a-radio :value="2">直接提交给我<BasicHelp text="若流程为A->B->C，C退回至A，则C->A->C" /></a-radio>
          </a-radio-group>
        </a-form-item>
      </template>
      <a-form-item label="抄送人员" name="copyIds" v-if="['audit', 'reject'].includes(eventType) && properties.isCustomCopy">
        <jnpf-user-select v-model:value="dataForm.copyIds" multiple allowClear />
      </a-form-item>
      <a-form-item
        :label="`${label}意见`"
        name="handleOpinion"
        :rules="['audit', 'reject'].includes(eventType) ? [{ required: true, message: '必填', trigger: 'change' }] : []">
        <OpinionTextarea v-model:value="dataForm.handleOpinion" v-if="['audit', 'reject'].includes(eventType)" :commonWordsCount="2" />
        <jnpf-textarea v-model:value="dataForm.handleOpinion" placeholder="请输入" v-else />
      </a-form-item>
      <a-form-item
        :label="`${label}签名`"
        name="signImg"
        v-if="properties.hasSign"
        :rules="['audit', 'reject'].includes(eventType) ? [{ required: true, message: `请签名`, trigger: 'change' }] : []">
        <jnpf-sign v-model:value="dataForm.signImg" isInvoke />
      </a-form-item>
      <a-form-item :label="`${label}附件`" name="fileList" v-if="properties.hasFile">
        <jnpf-upload-file v-model:value="dataForm.fileList" type="workFlow" :limit="3" />
      </a-form-item>
    </a-form>
  </BasicModal>
</template>
<script lang="ts" setup>
  import { reactive, ref, toRefs, unref, nextTick, computed } from 'vue';
  import { BasicModal, useModalInner } from '@/components/Modal';
  import type { FormInstance } from 'ant-design-vue';
  import { UserSelect as CandidateUserSelect } from '@/components/CommonModal';
  import { useUserStore } from '@/store/modules/user';
  import OpinionTextarea from '../components/OpinionTextarea.vue';
  import { getCandidateUser } from '@/api/workFlow/task';
  import { omit } from 'lodash-es';

  interface State {
    dataForm: any;
    defaultCandidateList: any[];
    branchList: any[];
    operatorId: string;
    formData: any;
    eventType: string;
    properties: any;
    backNodeCodeList: any[];
    title: string;
    label: string;
  }

  const emit = defineEmits(['register', 'confirm']);
  const userStore = useUserStore();
  const formElRef = ref<FormInstance>();
  const state = reactive<State>({
    dataForm: {
      copyIds: [],
      branchList: [],
      candidateList: [],
      fileList: [],
      handleOpinion: '',
      signImg: '',
      backType: 1,
      backNodeCode: '',
      // 加签
      addSignParameter: {
        addSignUserIdList: [],
        addSignType: 1,
        counterSign: 0,
        auditRatio: 100,
      },
    },
    defaultCandidateList: [],
    branchList: [],
    operatorId: '',
    formData: {},
    eventType: '',
    properties: {},
    backNodeCodeList: [],
    title: '',
    label: '',
  });
  const { dataForm, properties, eventType, label, branchList } = toRefs(state);
  const [registerModal, { changeOkLoading }] = useModalInner(init);

  const getUserInfo = computed(() => userStore.getUserInfo || {});

  function init(data) {
    changeOkLoading(false);
    state.dataForm = {
      copyIds: [],
      branchList: [],
      candidateList: [],
      fileList: [],
      handleOpinion: '',
      signImg: '',
      backType: 1,
      backNodeCode: '',
      addSignParameter: {
        addSignUserIdList: [],
        addSignType: 1,
        counterSign: 0,
        auditRatio: 100,
      },
    };
    state.operatorId = data.operatorId || '';
    state.formData = data.formData;
    state.eventType = data.eventType;
    state.title = getTitle();
    state.label = state.eventType === 'freeApprover' ? '加签' : state.eventType === 'back' ? '退回' : '审批';
    state.properties = data.properties;
    state.backNodeCodeList = data.backNodeCodeList.map(o => ({ id: o.nodeCode, fullName: o.nodeName }));
    if (state.properties.hasSign) state.dataForm.signImg = unref(getUserInfo).signImg;
    state.dataForm.backNodeCode = data.backNodeCodeList.length ? data.backNodeCodeList[0].nodeCode : '';
    state.branchList = data.branchList.map(o => ({ ...omit(o, ['nodeCode', 'nodeName']), id: o.nodeCode, fullName: o.nodeName }));
    state.defaultCandidateList = data.candidateList.map(o => ({
      ...o,
      label: '审批人',
      value: [],
      rules: !o.selected ? [{ required: true, message: '必填', trigger: 'change', type: 'array' }] : [],
    }));
    state.dataForm.candidateList = state.defaultCandidateList;
    nextTick(() => {
      formElRef.value?.clearValidate();
    });
  }
  async function handleSubmit() {
    try {
      const values = await formElRef.value?.validate();
      if (!values) return;
      let candidateList = {};
      if (state.dataForm.candidateList.length) {
        for (let i = 0; i < state.dataForm.candidateList.length; i++) {
          candidateList[state.dataForm.candidateList[i].nodeCode] = state.dataForm.candidateList[i].value;
        }
      }
      changeOkLoading(true);
      const data = {
        ...state.dataForm,
        candidateList,
        copyIds: state.dataForm.copyIds.join(','),
      };
      if (state.properties.backType != 3) data.backType = state.properties.backType;
      if (state.eventType !== 'freeApprover') delete data.addSignParameter;
      emit('confirm', data);
    } catch (_) {}
  }
  function getTitle() {
    const key = state.eventType;
    if (key == 'reject') return '审批拒绝';
    if (key == 'back') return '审批退回';
    if (key == 'freeApprover') return '加签';
    return '审批同意';
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
