<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    width="900px"
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
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_Image')">
            <a-form-item name="F_Image" :labelCol="{ style: { width: '100px' } }">
              <template #label>机台图片</template>
              <JnpfUploadImg
                v-model:value="dataForm.F_Image"
                pathType="selfPath"
                :isAccount="-1"
                folder=""
                sizeUnit="MB"
                :fileSize="10"
                :sortRule="['1', '2']"
                timeFormat="YYYYMMDD"
                showType="card"
                buttonText="点击上传" />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_MachineCode')">
            <a-form-item name="F_MachineCode" :labelCol="{ style: { width: '100px' } }">
              <template #label>机台编号</template>
              <JnpfInput
                v-model:value="dataForm.F_MachineCode"
                placeholder="请填写，忽略将自动生成"
                allowClear
                :disabled="state.dataForm.id"
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_MachineName')">
            <a-form-item name="F_MachineName" :labelCol="{ style: { width: '100px' } }">
              <template #label>机台名称</template>
              <JnpfInput
                v-model:value="dataForm.F_MachineName"
                placeholder="请输入"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_MachineSpec')">
            <a-form-item name="F_MachineSpec" :labelCol="{ style: { width: '100px' } }">
              <template #label>机台规格</template>
              <JnpfInput
                v-model:value="dataForm.F_MachineSpec"
                placeholder="请输入"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_MachineStatus')">
            <a-form-item name="F_MachineStatus" :labelCol="{ style: { width: '100px' } }">
              <template #label>机台状态</template>
              <JnpfSelect
                v-model:value="dataForm.F_MachineStatus"
                placeholder="请选择"
                :options="optionsObj.F_MachineStatusOptions"
                :fieldNames="optionsObj.F_MachineStatusProps"
                allowClear
                showSearch
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_MachineType')">
            <a-form-item name="F_MachineType" :labelCol="{ style: { width: '100px' } }">
              <template #label>机台类别</template>
              <JnpfCascader
                v-model:value="dataForm.F_MachineType"
                placeholder="请选择"
                :options="optionsObj.F_MachineTypeOptions"
                :fieldNames="optionsObj.F_MachineTypeProps"
                allowClear
                showSearch
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_WorkshopId')">
            <a-form-item name="F_WorkshopId" :labelCol="{ style: { width: '100px' } }">
              <template #label>车间</template>
              <JnpfCascader
                v-model:value="dataForm.F_WorkshopId"
                placeholder="请选择"
                :options="optionsObj.F_WorkshopIdOptions"
                :fieldNames="optionsObj.F_WorkshopIdProps"
                allowClear
                showSearch
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_LineId')">
            <a-form-item name="F_LineId" :labelCol="{ style: { width: '100px' } }">
              <template #label>产线</template>
              <JnpfSelect
                v-model:value="dataForm.F_LineId"
                placeholder="请选择"
                :options="optionsObj.F_LineIdOptions"
                :fieldNames="optionsObj.F_LineIdProps"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_DailyRunHours')">
            <a-form-item name="F_DailyRunHours" :labelCol="{ style: { width: '100px' } }">
              <template #label>单日运行时长</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_DailyRunHours"
                placeholder="请输入"
                addonAfter="小时/天"
                :controls="false"
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_StdHour')">
            <a-form-item name="F_StdHour" :labelCol="{ style: { width: '100px' } }">
              <template #label>标准工时</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_StdHour"
                placeholder="请输入"
                addonAfter="小时"
                min="0"
                :controls="false"
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="4" class="ant-col-item" v-if="hasFormP('F_StdHour')">
            <a-form-item name="F_StdMinute" :labelCol="{ style: { width: '100px' } }">
              <JnpfInputNumber
                v-model:value="dataForm.F_StdMinute"
                placeholder="请输入"
                addonAfter="分钟"
                min="0"
                max="59"
                :controls="false"
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="4" class="ant-col-item" v-if="hasFormP('F_StdHour')">
            <a-form-item name="F_StdSecond" :labelCol="{ style: { width: '100px' } }">
              <JnpfInputNumber
                v-model:value="dataForm.F_StdSecond"
                placeholder="请输入"
                addonAfter="秒"
                min="0"
                max="59"
                :controls="false"
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_StdOutput')">
            <a-form-item name="F_StdOutput" :labelCol="{ style: { width: '100px' } }">
              <template #label>标准产出</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_StdOutput"
                placeholder="请输入"
                :controls="false"
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_State')">
            <a-form-item name="F_State" :labelCol="{ style: { width: '100px' } }">
              <template #label>启用状态</template>
              <JnpfSwitch v-model:value="dataForm.F_State" />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item">
            <a-form-item name="F_StdOutput" :labelCol="{ style: { width: '100px' } }">
              <template #label>标准效率<BasicHelp text="标准效率是单位时间内按照标准操作流程生产出的产品数量。标准效率 = 标准产出 / 标准工时。" /> </template>
              <JnpfInput
                :value="
                  (dataForm.F_StdOutput ?? 0) +
                  '/' +
                  (dataForm.F_StdHour ?? 0) +
                  '小时' +
                  (dataForm.F_StdMinute ?? 0) +
                  '分钟' +
                  (dataForm.F_StdSecond ?? 0) +
                  '秒'
                "
                placeholder=""
                disabled
                :controls="false"
                :style="{
                  width: '100%',
                }" />
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
  import { getDictionaryDataSelector } from '@/api/systemData/dictionary';
  import { getDataInterfaceRes } from '@/api/systemData/dataInterface';
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
  const getLeftTreeActiveInfo: (() => any) | null = inject('getLeftTreeActiveInfo', null);
  const userStore = useUserStore();
  const userInfo = userStore.getUserInfo;
  const { createMessage, createConfirm } = useMessage();
  const { t } = useI18n();
  const { hasFormP } = usePermission();
  const [registerModal, { openModal, setModalProps }] = useModal();
  const formRef = ref<FormInstance>();
  const selectModal = ref(null);
  const state = reactive<State>({
    dataForm: {
      id: '',
      F_Image: [],
      F_MachineCode: undefined,
      F_MachineName: undefined,
      F_MachineSpec: undefined,
      F_MachineStatus: undefined,
      F_MachineType: [],
      F_WorkshopId: [],
      F_LineId: undefined,
      F_DailyRunHours: undefined,
      F_StdHour: undefined,
      F_StdMinute: undefined,
      F_StdSecond: undefined,
      F_StdOutput: undefined,
      F_State: 1,
    },
    dataRule: {
      F_MachineName: [
        {
          required: true,
          message: '请输入机台名称',
          trigger: 'blur',
        },
      ],
      F_MachineStatus: [
        {
          required: true,
          message: '请输入机台状态',
          trigger: 'change',
        },
      ],
      F_MachineType: [
        {
          required: true,
          message: '请输入机台类别',
          trigger: 'change',
        },
      ],
    },
    optionsObj: {
      F_MachineStatusProps: { label: 'fullName', value: 'enCode' },
      F_MachineTypeProps: {
        label: 'F_Name',
        value: 'F_Id',
        children: 'children',
      },
      F_WorkshopIdProps: {
        label: 'F_Name',
        value: 'F_Id',
        children: 'children',
      },
      F_LineIdProps: { label: 'fullName', value: 'enCode' },
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
        F_Image: [],
        F_MachineCode: undefined,
        F_MachineName: undefined,
        F_MachineSpec: undefined,
        F_MachineStatus: undefined,
        F_MachineType: [],
        F_WorkshopId: [],
        F_LineId: undefined,
        F_DailyRunHours: undefined,
        F_StdHour: undefined,
        F_StdMinute: undefined,
        F_StdSecond: undefined,
        F_StdOutput: undefined,
        F_State: 1,
      };
      getF_MachineStatusOptions();
      getF_MachineTypeOptions();
      getF_WorkshopIdOptions();
      getF_LineIdOptions();
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
      getF_MachineStatusOptions();
      getF_MachineTypeOptions();
      getF_WorkshopIdOptions();
      getF_LineIdOptions();
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
  function getF_MachineStatusOptions() {
    getDictionaryDataSelector('2011620395194650624').then(res => {
      state.optionsObj.F_MachineStatusOptions = res.data.list;
    });
  }
  function getF_MachineTypeOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2011621811959238656', query).then(res => {
      let data = res.data;
      state.optionsObj.F_MachineTypeOptions = Array.isArray(data) ? data : [];
    });
  }
  function getF_WorkshopIdOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2011621894347952128', query).then(res => {
      let data = res.data;
      state.optionsObj.F_WorkshopIdOptions = Array.isArray(data) ? data : [];
    });
  }
  function getF_LineIdOptions() {
    getDictionaryDataSelector('2011623836151320576').then(res => {
      state.optionsObj.F_LineIdOptions = res.data.list;
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
