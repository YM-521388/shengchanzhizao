<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    width="50%"
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
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_DeviceId')">
            <a-form-item name="F_DeviceId" :labelCol="{ style: { width: '100px' } }">
              <template #label>维修设备</template>
              <JnpfPopupMultipleSelect
                v-model:value="dataForm.F_DeviceId"
                placeholder="请选择"
                :templateJson="optionsObj.F_DeviceIdTemplateJson"
                allowClear
                field="F_DeviceId"
                interfaceId="2016476904974061568"
                :columnOptions="optionsObj.F_DeviceIdOptions"
                relationField="F_DeviceName"
                propsValue="id"
                :pageSize="20"
                popupType="dialog"
                popupTitle="选择数据"
                popupWidth="1000px"
                hasPage
                :style="{
                  width: '100%',
                }"
                :extraOptions="optionsObj.F_DeviceId" />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_RepairNo')">
            <a-form-item name="F_RepairNo" :labelCol="{ style: { width: '100px' } }">
              <template #label>工单编号</template>
              <JnpfInput
                v-model:value="dataForm.F_RepairNo"
                placeholder="请填写，忽略将自动生成"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>
          <!-- <a-col :span="12" class="ant-col-item">
            <a-form-item name="F_GroupId" :labelCol="{ style: { width: '100px' } }">
              <template #label>分组</template>
              <JnpfGroupSelect
                v-model:value="dataForm.F_GroupId"
                placeholder="请选择"
                selectType="all"
                allowClear
                :disabled="state.dataForm.id"
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col> -->
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_DispatchType')">
            <a-form-item name="F_DispatchType" :labelCol="{ style: { width: '100px' } }">
              <template #label>派单方式</template>
              <JnpfSelect
                v-model:value="dataForm.F_DispatchType"
                placeholder="请选择"
                :options="optionsObj.F_DispatchTypeOptions"
                :fieldNames="optionsObj.F_DispatchTypeProps"
                allowClear
                showSearch
                :disabled="state.dataForm.id"
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_HandlerUserId') && dataForm.F_DispatchType == '0'">
            <a-form-item name="F_HandlerUserId" :labelCol="{ style: { width: '100px' } }">
              <template #label>处理人</template>
              <JnpfUserSelect
                v-model:value="dataForm.F_HandlerUserId"
                placeholder="请选择"
                selectType="all"
                allowClear
                :disabled="state.dataForm.id"
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_Description')">
            <a-form-item name="F_Description" :labelCol="{ style: { width: '100px' } }">
              <template #label>故障描述</template>
              <JnpfTextarea
                v-model:value="dataForm.F_Description"
                placeholder="请输入"
                :maxlength="500"
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
  import { create, update, getInfo } from './helper/api';
  import { reactive, toRefs, nextTick, ref, unref, computed, toRaw, inject } from 'vue';
  import { BasicModal, useModal } from '@/components/Modal';
  import SelectModal from '@/components/CommonModal/src/SelectModal.vue';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useI18n } from '@/hooks/web/useI18n';
  import { useUserStore } from '@/store/modules/user';
  import type { FormInstance } from 'ant-design-vue';
  import { thousandsFormat } from '@/utils/jnpf';
  import { getTimeUnit, getDateTimeUnit } from '@/utils/jnpf';
  import { getDictionaryDataSelector } from '@/api/systemData/dictionary';
  import dayjs from 'dayjs';
  import { cloneDeep } from 'lodash-es';
  import { buildUUID } from '@/utils/uuid';
  import { usePermission } from '@/hooks/web/usePermission';

  interface State {
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
  const { hasFormP } = usePermission();
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
      F_RepairNo: undefined,
      F_DeviceId: [],
      F_GroupId: userInfo.groupIds[0],
      F_DispatchType: '0',
      F_HandlerUserId: userInfo.userId ? userInfo.userId : '',
      F_Description: undefined,
    },
    dataRule: {
      F_DeviceId: [
        {
          required: true,
          message: '请输入维修设备',
          trigger: 'change',
        },
      ],
      F_DispatchType: [
        {
          required: true,
          message: '请输入派单方式',
          trigger: 'change',
        },
      ],
      F_HandlerUserId: [
        {
          required: true,
          message: '请输入处理人',
          trigger: 'change',
        },
      ],
      F_Description: [
        {
          required: true,
          message: '请输入故障描述',
          trigger: 'blur',
        },
      ],
    },
    optionsObj: {
      F_DeviceIdOptions: [
        {
          value: 'F_DeviceNo',
          label: '设备编号',
        },
        {
          value: 'F_DeviceName',
          label: '设备名称',
        },
        {
          value: 'F_DeviceSpec',
          label: '设备规格',
        },
        {
          value: 'F_DeviceStatus',
          label: '设备状态',
        },
        {
          value: 'F_DeviceType',
          label: '设备类别',
        },
        {
          value: 'F_WorkshopId',
          label: '车间',
        },
        {
          value: 'F_LineId',
          label: '产线',
        },
        {
          value: 'F_DeviceUsersId',
          label: '负责人',
        },
        {
          value: 'F_GroupId',
          label: '分组',
        },
        {
          value: 'F_CreatorTime',
          label: '创建时间',
        },
      ],
      F_DeviceIdTemplateJson: [],
      F_DispatchTypeProps: { label: 'fullName', value: 'enCode' },
      F_StateProps: { label: 'fullName', value: 'enCode' },
      F_IsSolvedOptions: [
        { fullName: '是', id: '1' },
        { fullName: '否', id: '0' },
      ],
      F_IsSolvedProps: { label: 'fullName', value: 'id' },
      F_DeviceId: [],
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

  const getPrevDisabled = computed(() => state.currIndex === 0);
  const getNextDisabled = computed(() => state.currIndex === state.allList.length - 1);

  defineExpose({ init });

  function init(data) {
    state.submitType = 0;
    state.showContinueBtn = true;
    state.title = !data.id ? '新增' : '编辑';
    setFormProps({ continueLoading: false });
    state.dataForm.id = data.id;
    openModal();
    nextTick(() => {
      getForm().resetFields();
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
        F_RepairNo: undefined,
        F_DeviceId: [],
        F_GroupId: userInfo.groupIds[0],
        F_DispatchType: '0',
        F_HandlerUserId: userInfo.userId ? userInfo.userId : '',
        F_Description: undefined,
      };
      getF_DispatchTypeOptions();
      getF_StateOptions();
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
      getF_DispatchTypeOptions();
      getF_StateOptions();
      changeLoading(false);
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
  function handlePrev() {
    state.currIndex--;
    handleGetNewInfo();
  }
  function handleNext() {
    state.currIndex++;
    handleGetNewInfo();
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
  function getF_DispatchTypeOptions() {
    getDictionaryDataSelector('2016473380785623040').then(res => {
      state.optionsObj.F_DispatchTypeOptions = res.data.list;
    });
  }
  function getF_StateOptions() {
    getDictionaryDataSelector('2016473505696190464').then(res => {
      state.optionsObj.F_StateOptions = res.data.list;
    });
  }
  function openSelectDialog(key, value) {
    state.currTableConf = state.addTableConf[key + 'List' + value];
    state.currVmodel = key;
    nextTick(() => {
      (selectModal.value as any)?.openSelectModal();
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
