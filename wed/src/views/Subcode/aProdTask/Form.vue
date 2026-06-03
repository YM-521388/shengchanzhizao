<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    width="1000px"
    :minHeight="100"
    okText="确定"
    cancelText="取消"
    @ok="handleSubmit"
    :closeFunc="onClose"
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
    <a-row class="dynamic-form">
      <a-form :colon="false" layout="horizontal" labelAlign="right" :labelCol="{ style: { width: '100px' } }" :model="dataForm" :rules="dataRule" ref="formRef">
        <a-row :gutter="15">
          <a-col :span="12" class="ant-col-item">
            <a-form-item name="F_PlanStartDate" :labelCol="{ style: { width: '100px' } }">
              <template #label>计划开始</template>
              <JnpfDatePicker
                v-model:value="dataForm.F_PlanStartDate"
                placeholder="请选择"
                format="yyyy-MM-dd"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item">
            <a-form-item name="F_PlanEndDate" :labelCol="{ style: { width: '100px' } }">
              <template #label>计划结束</template>
              <JnpfDatePicker
                v-model:value="dataForm.F_PlanEndDate"
                placeholder="请选择"
                format="yyyy-MM-dd"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item">
            <a-form-item name="F_ProdDispatch" :labelCol="{ style: { width: '100px' } }">
              <template #label>生产派工</template>
              <JnpfSelect
                v-model:value="dataForm.F_ProdDispatch"
                placeholder="请选择"
                :options="optionsObj.F_ProdDispatchOptions"
                :fieldNames="optionsObj.F_ProdDispatchProps"
                allowClear
                showSearch
                multiple
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item">
            <a-form-item name="F_QcDispatch" :labelCol="{ style: { width: '100px' } }">
              <template #label>质检派工</template>
              <JnpfSelect
                v-model:value="dataForm.F_QcDispatch"
                placeholder="请选择"
                :options="optionsObj.F_QcDispatchOptions"
                :fieldNames="optionsObj.F_QcDispatchProps"
                allowClear
                showSearch
                multiple
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item">
            <a-form-item name="F_ProdQty" :labelCol="{ style: { width: '100px' } }">
              <template #label>生产数量</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_ProdQty"
                placeholder="请输入"
                :min="1"
                :controls="false"
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item">
            <a-form-item name="F_Description" :labelCol="{ style: { width: '100px' } }">
              <template #label>备注</template>
              <JnpfTextarea
                v-model:value="dataForm.F_Description"
                placeholder="请输入"
                allowClear
                :autoSize="{
                  minRows: 4,
                  maxRows: 4,
                }"
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>
        </a-row>
      </a-form>
    </a-row>
  </BasicModal>
  <SelectModal :config="state.currTableConf" :formData="state.dataForm" ref="selectModal" @select="addForSelect" />
</template>
<script lang="ts" setup>
  import { create, update, getInfo, getUserCon } from './helper/api';
  import { reactive, toRefs, nextTick, ref, unref, computed, toRaw, inject } from 'vue';
  import { BasicModal, useModal } from '@/components/Modal';
  import SelectModal from '@/components/CommonModal/src/SelectModal.vue';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useI18n } from '@/hooks/web/useI18n';
  import { useUserStore } from '@/store/modules/user';
  import type { FormInstance } from 'ant-design-vue';
  import { getDictionaryDataSelector } from '@/api/systemData/dictionary';
  import { getDataInterfaceRes } from '@/api/systemData/dataInterface';
  import dayjs from 'dayjs';
  import { cloneDeep } from 'lodash-es';
  import { buildUUID } from '@/utils/uuid';

  interface State {
    dataForm: any;
    dataRule: any;
    optionsObj: any;
    currVmodel: any;
    currTableConf: any;
    addTableConf: any;
    tableRows: any;
    tableField0bb944outerActiveKey: number[];
    tableField0bb944innerActiveKey: string[];
    tableField0bb944DefaultExpandAll: boolean;
    selectedtableField0bb944RowKeys: any[];
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
    dataForm: {
      id: '',
      F_ProcessingId: undefined,
      F_ProcessId: undefined,
      F_PlanStartDate: undefined,
      F_PlanEndDate: undefined,
      F_ProdDispatch: [],
      F_QcDispatch: [],
      F_ProdQty: undefined,
      F_SortCode: undefined,
      F_Description: undefined,
      F_TaskStatus: undefined,
      F_TaskType: undefined,
      F_CreatorTime: undefined,
      tableField0bb944: [],
    },
    tableField0bb944outerActiveKey: [0],
    tableField0bb944innerActiveKey: [],
    tableField0bb944DefaultExpandAll: true,
    selectedtableField0bb944RowKeys: [],
    dataRule: {},
    optionsObj: {
      F_ProcessingIdProps: { label: 'F_ProcessNo', value: 'F_Id' },
      F_ProcessIdProps: { label: 'F_ProcessName', value: 'id' },
      F_ProdDispatchOptions: [],
      F_ProdDispatchProps: { label: 'fullName', value: 'id' },
      F_QcDispatchOptions: [],
      F_QcDispatchProps: { label: 'fullName', value: 'id' },
      F_TaskStatusProps: { label: 'fullName', value: 'enCode' },
      F_TaskTypeProps: { label: 'fullName', value: 'enCode' },
      tableField0bb944_F_GoodsIdOptions: [],
      tableField0bb944_F_GoodsIdProps: { label: 'F_GoodsName', value: 'id' },
    },
    currVmodel: '',
    currTableConf: {},
    tableRows: {
      tableField0bb944: {
        enabledmark: undefined,
        F_GoodsId: undefined,
        F_Num: undefined,
        F_Description: undefined,
        F_CreatorTime: undefined,
      },
    },
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
    state.title = !data.id ? '新增' : '编辑';
    setFormProps({ continueLoading: false });
    state.dataForm.id = data.id;
    openModal();
    state.allList = data.allList;
    state.currIndex = state.allList.length && data.id ? state.allList.findIndex(item => item.id === data.id) : 0;
    nextTick(() => {
      getForm().resetFields();
      state.dataForm.tableField0bb944 = [];
      setTimeout(initData, 0);
    });
  }
  function initData() {
    changeLoading(true);
    if (state.dataForm.id) {
      getData(state.dataForm.id);
    } else {
      // 设置默认值
      state.dataForm = {
        F_ProcessingId: undefined,
        F_ProcessId: undefined,
        F_PlanStartDate: undefined,
        F_PlanEndDate: undefined,
        F_ProdDispatch: [],
        F_QcDispatch: [],
        F_ProdQty: undefined,
        F_SortCode: undefined,
        F_Description: undefined,
        F_TaskStatus: undefined,
        F_TaskType: undefined,
        F_CreatorTime: undefined,
        tableField0bb944: [],
      };
      getF_ProcessingIdOptions();
      getF_ProcessIdOptions();
      getF_TaskStatusOptions();
      getF_TaskTypeOptions();
      gettableField0bb944_F_GoodsIdOptions();
      if (getLeftTreeActiveInfo) state.dataForm = { ...state.dataForm, ...(getLeftTreeActiveInfo() || {}) };
      changeLoading(false);
    }
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
      for (let i = 0; i < state.dataForm.tableField0bb944.length; i++) {
        const element = state.dataForm.tableField0bb944[i];
        element.jnpfId = buildUUID();
      }
      getF_ProcessingIdOptions();
      getF_ProcessIdOptions();
      getF_TaskStatusOptions();
      getF_TaskTypeOptions();
      gettableField0bb944_F_GoodsIdOptions();
      changeLoading(false);
    });
    getUserCon(id).then(res => {
      state.optionsObj.F_ProdDispatchOptions = res.data.prodUserList || [];
      state.optionsObj.F_QcDispatchOptions = res.data.qcUserList || [];
    });
  }
  async function handleSubmit() {
    try {
      const values = await getForm()?.validate();
      if (!values) return;
      setFormProps({ confirmLoading: true });
      const formMethod = state.dataForm.id ? update : create;
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
  function handleGetNewInfo() {
    changeLoading(true);
    getForm().resetFields();
    const id = state.allList[state.currIndex].id;
    getData(id);
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
  function getF_ProcessingIdOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2013860549426810880', query).then(res => {
      let data = res.data;
      state.optionsObj.F_ProcessingIdOptions = Array.isArray(data) ? data : [];
    });
  }
  function getF_ProcessIdOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2012092092830060544', query).then(res => {
      let data = res.data;
      state.optionsObj.F_ProcessIdOptions = Array.isArray(data) ? data : [];
    });
  }
  function getF_TaskStatusOptions() {
    getDictionaryDataSelector('2013859706900189184').then(res => {
      state.optionsObj.F_TaskStatusOptions = res.data.list;
    });
  }
  function getF_TaskTypeOptions() {
    getDictionaryDataSelector('2013859641523572736').then(res => {
      state.optionsObj.F_TaskTypeOptions = res.data.list;
    });
  }
  function gettableField0bb944_F_GoodsIdOptions() {
    let templateJson = [
      {
        defaultValue: '',
        field: 'contractId',
        dataType: 'varchar',
        required: 0,
        fieldName: '合同ID',
        relationField: '',
        jnpfKey: null,
        complexHeaderList: null,
        sourceType: 3,
        isChildren: false,
        IsMultiple: false,
      },
    ];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2012085692393459712', query).then(res => {
      let data = res.data;
      state.optionsObj.tableField0bb944_F_GoodsIdOptions = Array.isArray(data) ? data : [];
    });
  }
  function addForSelect(data) {
    for (let i = 0; i < data.length; i++) {
      let item = { ...state.tableRows[state.currVmodel], ...data[i], jnpfId: buildUUID() };
      state.dataForm[state.currVmodel].push(cloneDeep(item));
      if (state[state.currVmodel + 'DefaultExpandAll']) state[state.currVmodel + 'innerActiveKey'].push(item.jnpfId);
    }
  }
  function getParamList(templateJson, formData, index?) {
    for (let i = 0; i < templateJson.length; i++) {
      if (templateJson[i].relationField && templateJson[i].sourceType == 1) {
        //区分是否子表
        if (templateJson[i].relationField.includes('-')) {
          let tableVModel = templateJson[i].relationField.split('-')[0];
          let childVModel = templateJson[i].relationField.split('-')[1];
          templateJson[i].defaultValue = (formData[tableVModel] && formData[tableVModel][index] && formData[tableVModel][index][childVModel]) || '';
        } else {
          templateJson[i].defaultValue = formData[templateJson[i].relationField] || '';
        }
      }
    }
    return templateJson;
  }
</script>
