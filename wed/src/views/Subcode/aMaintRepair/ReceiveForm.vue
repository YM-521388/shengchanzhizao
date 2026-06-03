<template>
  <BasicModal v-bind="$attrs" @register="registerModal" width="1000px" :minHeight="100" okText="确定" cancelText="取消" @ok="handleSubmit" :closeFunc="onClose">
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
    <a-row class="dynamic-form">
      <a-form :colon="false" layout="horizontal" labelAlign="right" :labelCol="{ style: { width: '100px' } }" :model="dataForm" :rules="dataRule" ref="formRef">
        <a-row :gutter="15">
          <!-- <a-col :span="12" class="ant-col-item">
            <a-form-item name="F_GroupId" :labelCol="{ style: { width: '100px' } }">
              <template #label>分组</template>
              <JnpfGroupSelect
                v-model:value="dataForm.F_GroupId"
                placeholder="请选择"
                selectType="all"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col> -->
          <a-col :span="12" class="ant-col-item">
            <a-form-item name="F_DispatchType" :labelCol="{ style: { width: '100px' } }">
              <template #label>派单方式</template>
              <JnpfSelect
                v-model:value="dataForm.F_DispatchType"
                placeholder="请选择"
                :options="optionsObj.F_DispatchTypeOptions"
                :fieldNames="optionsObj.F_DispatchTypeProps"
                allowClear
                showSearch
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="dataForm.F_DispatchType == '0'">
            <a-form-item name="F_HandlerUserId" :labelCol="{ style: { width: '100px' } }">
              <template #label>处理人</template>
              <JnpfUserSelect
                v-model:value="dataForm.F_HandlerUserId"
                placeholder="请选择"
                selectType="all"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
        </a-row>
      </a-form>
    </a-row>
  </BasicModal>
</template>
<script lang="ts" setup>
  import { receiveUpdate, getInfo } from './helper/api';
  import { reactive, toRefs, nextTick, ref, unref, computed, toRaw, inject } from 'vue';
  import { BasicModal, useModal } from '@/components/Modal';
  import SelectModal from '@/components/CommonModal/src/SelectModal.vue';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useI18n } from '@/hooks/web/useI18n';
  import { useUserStore } from '@/store/modules/user';
  import type { FormInstance } from 'ant-design-vue';
  import { getDictionaryDataSelector } from '@/api/systemData/dictionary';
  import { getDataInterfaceRes } from '@/api/systemData/dataInterface';

  interface State {
    type: string;
    dataForm: any;
    dataRule: any;
    optionsObj: any;
    currVmodel: any;
    currTableConf: any;
    addTableConf: any;
    tableRows: any;
    title: string;
    continueText: string;
    allList: any[];
    currIndex: number;
    isContinue: boolean;
    submitType: number;
    showContinueBtn: boolean;
  }

  const emit = defineEmits(['reload']);
  const getLeftTreeActiveInfo: (() => any) | null = inject('getLeftTreeActiveInfo', null);
  const userStore = useUserStore();
  const userInfo = userStore.getUserInfo;
  const { createMessage, createConfirm } = useMessage();
  const { t } = useI18n();
  const [registerModal, { openModal, setModalProps }] = useModal();
  const formRef = ref<FormInstance>();
  const selectModal = ref(null);

  const state = reactive<State>({
    type: '',
    dataForm: {
      id: '',
      F_GroupId: userInfo.groupIds[0],
      F_DispatchType: '0',
      F_HandlerUserId: userInfo.userId ? userInfo.userId : '',
    },
    dataRule: {
      F_GroupId: [
        {
          required: true,
          message: '请选择分组',
          trigger: 'change',
        },
      ],
      F_DispatchType: [
        {
          required: true,
          message: '请选择派单方式',
          trigger: 'change',
        },
      ],
      F_HandlerUserId: [
        {
          required: true,
          message: '请选择处理人',
          trigger: 'change',
        },
      ],
    },
    optionsObj: {
      F_DispatchTypeProps: { label: 'fullName', value: 'enCode' },
    },
    currVmodel: '',
    currTableConf: {},
    tableRows: {},
    addTableConf: {},
    title: '',
    continueText: '',
    allList: [],
    currIndex: 0,
    isContinue: false,
    submitType: 0,
    showContinueBtn: true,
  });
  const { title, continueText, showContinueBtn, dataForm, submitType, dataRule, optionsObj } = toRefs(state);

  defineExpose({ init });

  function init(data) {
    state.submitType = 0;
    state.showContinueBtn = true;
    state.title = data.fullName;
    setFormProps({ continueLoading: false });
    state.dataForm.id = data.id;
    state.dataForm.F_Reason = undefined;
    state.type = data.type;
    openModal();
    nextTick(() => {
      getForm().resetFields();
      state.dataForm.tableField0bb944 = [];
      setTimeout(initData, 0);
    });
  }
  function initData() {
    changeLoading(true);
    getData(state.dataForm.id);
    if (getLeftTreeActiveInfo) state.dataForm = { ...state.dataForm, ...(getLeftTreeActiveInfo() || {}) };
    changeLoading(false);
  }
  function getForm() {
    const form = unref(formRef);
    if (!form) {
      throw new Error('form is null!');
    }
    return form;
  }
  function getData(id) {
    getInfo(id).then(res => {
      state.dataForm = res.data || {};
      getF_DispatchTypeOptions();
      changeLoading(false);
    });
  }
  async function handleSubmit() {
    try {
      const values = await getForm()?.validate();
      if (!values) return;
      setFormProps({ confirmLoading: true });
      const formMethod = receiveUpdate;
      formMethod(state.dataForm)
        .then(res => {
          createMessage.success(res.msg);
          setFormProps({ confirmLoading: false });
          if (state.submitType == 1) {
            initData();
            state.isContinue = true;
          } else {
            setFormProps({ open: false });
            emit('reload');
          }
        })
        .catch(() => {
          setFormProps({ confirmLoading: false });
        });
    } catch (_) {}
  }
  function getF_DispatchTypeOptions() {
    getDictionaryDataSelector('2016473380785623040').then(res => {
      state.optionsObj.F_DispatchTypeOptions = res.data.list;
    });
  }
  function setFormProps(data) {
    setModalProps(data);
  }
  function changeLoading(loading) {
    setModalProps({ loading });
  }
  async function onClose() {
    if (state.isContinue) emit('reload');
    return true;
  }
</script>
