<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    width="1200px"
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_ProcessName')">
            <a-form-item name="F_ProcessName" :labelCol="{ style: { width: '100px' } }">
              <template #label>工序名</template>
              <JnpfInput
                v-model:value="dataForm.F_ProcessName"
                placeholder="请输入"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_ProcessCode')">
            <a-form-item name="F_ProcessCode" :labelCol="{ style: { width: '100px' } }">
              <template #label>工序编号</template>
              <JnpfInput
                v-model:value="dataForm.F_ProcessCode"
                placeholder="请输入"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_WorkshopId')">
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
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_Line')">
            <a-form-item name="F_Line" :labelCol="{ style: { width: '100px' } }">
              <template #label>产线</template>
              <JnpfSelect
                v-model:value="dataForm.F_Line"
                placeholder="请选择"
                :options="optionsObj.F_LineOptions"
                :fieldNames="optionsObj.F_LineProps"
                allowClear
                showSearch
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_ReportUnit')">
            <a-form-item name="F_ReportUnit" :labelCol="{ style: { width: '100px' } }">
              <template #label>报工单位</template>
              <JnpfSelect
                v-model:value="dataForm.F_ReportUnit"
                placeholder="请选择"
                :options="optionsObj.F_ReportUnitOptions"
                :fieldNames="optionsObj.F_ReportUnitProps"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_UnitRatio')">
            <a-form-item name="F_UnitRatio" :labelCol="{ style: { width: '100px' } }">
              <template #label>单位关系</template>
              <div style="display: flex; align-items: center; gap: 4px">
                <span>生产1个计划数，需要</span>
                <span style="display: inline-block; width: 25%">
                  <JnpfInputNumber
                    v-model:value="dataForm.F_UnitRatio"
                    placeholder=""
                    :controls="false"
                    :style="{
                      width: '100%',
                    }" />
                </span>
                <span style="display: inline-block; width: 25%">
                  <JnpfSelect
                    v-model:value="dataForm.F_ReportUnit"
                    placeholder=""
                    :options="optionsObj.F_ReportUnitOptions"
                    :fieldNames="optionsObj.F_ReportUnitProps"
                    allowClear
                    :style="{
                      width: '100%',
                    }"
                /></span>
              </div>
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_PriceType')">
            <a-form-item name="F_PriceType" :labelCol="{ style: { width: '100px' } }">
              <template #label>计价方式</template>
              <JnpfSelect
                v-model:value="dataForm.F_PriceType"
                placeholder="请选择"
                :options="optionsObj.F_PriceTypeOptions"
                :fieldNames="optionsObj.F_PriceTypeProps"
                allowClear
                showSearch
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_WagePrice')">
            <a-form-item name="F_WagePrice" :labelCol="{ style: { width: '100px' } }">
              <template #label>工资单价</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_WagePrice"
                placeholder="请输入"
                min="0"
                :precision="2"
                :controls="false"
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="5" class="ant-col-item" v-if="hasFormP('F_StdHour')">
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
          <a-col :span="3" class="ant-col-item" v-if="hasFormP('F_StdHour')">
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
          <a-col :span="3" class="ant-col-item" v-if="hasFormP('F_StdHour')">
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
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_State')">
            <a-form-item name="F_State" :labelCol="{ style: { width: '100px' } }">
              <template #label>启用状态</template>
              <JnpfSwitch v-model:value="dataForm.F_State" />
            </a-form-item>
          </a-col>
          <a-col :span="10" class="ant-col-item" v-if="hasFormP('F_Files')">
            <a-form-item name="F_Files" :labelCol="{ style: { width: '100px' } }">
              <template #label>附件</template>
              <JnpfUploadFile
                v-model:value="dataForm.F_Files"
                buttonText="点击上传"
                pathType="selfPath"
                :isAccount="-1"
                folder=""
                :fileSize="10"
                sizeUnit="MB"
                :limit="3"
                :sortRule="['1', '2']"
                timeFormat="YYYYMMDD" />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_Description')">
            <a-form-item name="F_Description" :labelCol="{ style: { width: '100px' } }">
              <template #label>备注</template>
              <JnpfTextarea
                v-model:value="dataForm.F_Description"
                placeholder="请输入"
                :maxlength="500"
                allowClear
                :autoSize="{
                  minRows: 2,
                  maxRows: 2,
                }"
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item">
            <a-tabs v-model:activeKey="state.active1" type="" tabPosition="top" class="mb-20">
              <a-tab-pane tab="生产信息" key="1" forceRender>
                <a-row>
                  <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_MachineId')">
                    <a-form-item name="F_MachineId" :labelCol="{ style: { width: '100px' } }">
                      <template #label>机台</template>
                      <JnpfPopupMultipleSelect
                        v-model:value="dataForm.F_MachineId"
                        placeholder="请选择"
                        :templateJson="optionsObj.F_MachineIdTemplateJson"
                        allowClear
                        field="F_MachineId"
                        interfaceId="2011729284707782656"
                        :columnOptions="optionsObj.F_MachineIdOptions"
                        relationField="F_MachineName"
                        propsValue="id"
                        :pageSize="20"
                        popupType="dialog"
                        popupTitle="选择数据"
                        popupWidth="1000px"
                        hasPage
                        :style="{
                          width: '100%',
                        }"
                        :extraOptions="optionsObj.F_MachineId" />
                    </a-form-item>
                  </a-col>
                  <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_ProdUserIds')">
                    <a-form-item name="F_ProdUserIds" :labelCol="{ style: { width: '100px' } }">
                      <template #label>生产人员</template>
                      <JnpfUserSelect
                        v-model:value="dataForm.F_ProdUserIds"
                        placeholder="请选择"
                        selectType="all"
                        multiple
                        allowClear
                        :style="{
                          width: '100%',
                        }" />
                    </a-form-item>
                  </a-col>
                  <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_DefectIds')">
                    <a-form-item name="F_DefectIds" :labelCol="{ style: { width: '100px' } }">
                      <template #label>不良品项</template>
                      <JnpfSelect
                        v-model:value="dataForm.F_DefectIds"
                        placeholder="请选择"
                        :options="optionsObj.F_DefectIdsOptions"
                        :fieldNames="optionsObj.F_DefectIdsProps"
                        allowClear
                        showSearch
                        multiple
                        :style="{
                          width: '100%',
                        }" />
                    </a-form-item>
                  </a-col>
                </a-row>
              </a-tab-pane>
              <a-tab-pane tab="质检/外协信息" key="2" forceRender>
                <a-row>
                  <!-- <a-col :span="24" class="ant-col-item">
                    <a-form-item :labelCol="{ style: { width: '100px' } }">
                      <JnpfText
                        content="开启配置后工序默认为外协工序，外协工序无法报工和质检。"
                        :textStyle="{
                          color: '#F20808',
                          'text-align': 'left',
                          'font-weight': 'bold',
                          'font-style': 'normal',
                          'text-decoration': 'none',
                          'line-height': 20,
                          'font-size': 14,
                        }" />
                    </a-form-item>
                  </a-col> -->
                  <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_IsOutsource')">
                    <a-form-item name="F_IsOutsource" :labelCol="{ style: { width: '100px' } }">
                      <template #label>工序需外协</template>
                      <JnpfSwitch @change="IsOutsourceBtn" v-model:value="dataForm.F_IsOutsource" />
                    </a-form-item>
                  </a-col>
                  <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_IsQc')">
                    <a-form-item name="F_IsQc" :labelCol="{ style: { width: '100px' } }">
                      <template #label>工序需质检</template>
                      <JnpfSwitch @change="IsQcBtn" v-model:value="dataForm.F_IsQc" />
                    </a-form-item>
                  </a-col>
                  <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_DefectHandle') && state.dataForm.F_IsQc == 1">
                    <a-form-item name="F_DefectHandle" :labelCol="{ style: { width: '140px' } }">
                      <template #label>不良品需报废、返工</template>
                      <JnpfSwitch v-model:value="dataForm.F_DefectHandle" />
                    </a-form-item>
                  </a-col>
                  <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_QcUserIds') && state.dataForm.F_IsQc == 1">
                    <a-form-item name="F_QcUserIds" :labelCol="{ style: { width: '100px' } }">
                      <template #label>质检人员</template>
                      <JnpfUserSelect
                        v-model:value="dataForm.F_QcUserIds"
                        placeholder="请选择"
                        selectType="all"
                        multiple
                        allowClear
                        :style="{
                          width: '100%',
                        }" />
                    </a-form-item>
                  </a-col>
                </a-row>
              </a-tab-pane>
              <!-- <a-tab-pane tab="工序打卡设置" key="2585b9" forceRender>
                <a-row>
                  <a-col :span="24" class="ant-col-item">
                    <a-form-item :labelCol="{ style: { width: '100px' } }">
                      <JnpfText
                        content="开启后可以通过打卡的方式记录生产的总用时。"
                        :textStyle="{
                          color: '#F20808',
                          'text-align': 'left',
                          'font-weight': 'bold',
                          'font-style': 'normal',
                          'text-decoration': 'none',
                          'line-height': 20,
                          'font-size': 14,
                        }" />
                    </a-form-item>
                  </a-col>
                  <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_TaskTimer')">
                    <a-form-item name="F_TaskTimer" :labelCol="{ style: { width: '100px' } }">
                      <template #label>生产任务计时</template>
                      <JnpfSwitch v-model:value="dataForm.F_TaskTimer" />
                    </a-form-item>
                  </a-col>
                </a-row>
              </a-tab-pane> -->
            </a-tabs>
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
  import { getDataInterfaceRes } from '@/api/systemData/dataInterface';
  import dayjs from 'dayjs';
  import { cloneDeep } from 'lodash-es';
  import { buildUUID } from '@/utils/uuid';
  import { CaretRightOutlined } from '@ant-design/icons-vue';
  import { systemComponentsList } from '@/components/FormGenerator/src/helper/config';
  import { usePermission } from '@/hooks/web/usePermission';

  interface State {
    dataForm: any;
    dataRule: any;
    optionsObj: any;
    active1: any;
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
      F_ProcessName: undefined,
      F_ProcessCode: undefined,
      F_WorkshopId: [],
      F_Line: undefined,
      F_ReportUnit: undefined,
      F_UnitRatio: undefined,
      F_PriceType: '0',
      F_WagePrice: undefined,
      F_StdHour: 0,
      F_StdMinute: 0,
      F_StdSecond: 0,
      F_State: 1,
      F_Files: [],
      F_Description: undefined,
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
      F_MachineId: undefined,
      F_ProdUserIds: [],
      F_DefectIds: [],
      F_IsOutsource: 0,
      F_IsQc: 0,
      F_DefectHandle: 0,
      F_QcUserIds: [],
      F_TaskTimer: 0,
    },
    dataRule: {
      F_ProcessName: [
        {
          required: true,
          message: '请输入工序名',
          trigger: 'blur',
        },
      ],
      F_PriceType: [
        {
          required: true,
          message: '请输入计价方式',
          trigger: 'change',
        },
      ],
    },
    active1: '1',
    optionsObj: {
      F_WorkshopIdProps: {
        label: 'F_Name',
        value: 'F_Id',
        children: 'children',
      },
      F_LineProps: { label: 'fullName', value: 'enCode' },
      F_ReportUnitProps: { label: 'fullName', value: 'enCode' },
      F_PriceTypeProps: { label: 'fullName', value: 'enCode' },
      F_MachineIdOptions: [
        {
          value: 'F_MachineName',
          label: '机台',
        },
        {
          value: 'F_MachineSpec',
          label: '机台规格',
        },
        {
          value: 'F_MachineStatus',
          label: '机台状态',
        },
        {
          value: 'F_MachineType',
          label: '机台类别',
        },
        {
          value: 'F_WorkshopId',
          label: '车间',
        },
        {
          value: 'F_LineId',
          label: '产线',
        },
      ],
      F_MachineIdTemplateJson: [],
      F_DefectIdsProps: { label: 'F_Name', value: 'F_Id' },
      F_MachineId: [],
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
    state.active1 = '1';
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
        F_ProcessName: undefined,
        F_ProcessCode: undefined,
        F_WorkshopId: [],
        F_Line: undefined,
        F_ReportUnit: undefined,
        F_UnitRatio: undefined,
        F_PriceType: '0',
        F_WagePrice: undefined,
        F_StdHour: 0,
        F_StdMinute: 0,
        F_StdSecond: 0,
        F_State: 1,
        F_Files: [],
        F_Description: undefined,
        F_CreatorUserId: undefined,
        F_CreatorTime: undefined,
        F_MachineId: undefined,
        F_ProdUserIds: [],
        F_DefectIds: [],
        F_IsOutsource: 0,
        F_IsQc: 0,
        F_DefectHandle: 0,
        F_QcUserIds: [],
        F_TaskTimer: 0,
      };
      getF_WorkshopIdOptions();
      getF_LineOptions();
      getF_ReportUnitOptions();
      getF_PriceTypeOptions();
      getF_DefectIdsOptions();
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
      getF_WorkshopIdOptions();
      getF_LineOptions();
      getF_ReportUnitOptions();
      getF_PriceTypeOptions();
      getF_DefectIdsOptions();
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
  function getF_LineOptions() {
    getDictionaryDataSelector('2011623836151320576').then(res => {
      state.optionsObj.F_LineOptions = res.data.list;
    });
  }
  function getF_ReportUnitOptions() {
    getDictionaryDataSelector('2008448689420505088').then(res => {
      state.optionsObj.F_ReportUnitOptions = res.data.list;
    });
  }
  function getF_PriceTypeOptions() {
    getDictionaryDataSelector('2011642610875240448').then(res => {
      state.optionsObj.F_PriceTypeOptions = res.data.list;
    });
  }
  function getF_DefectIdsOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2011648481097289728', query).then(res => {
      let data = res.data;
      state.optionsObj.F_DefectIdsOptions = Array.isArray(data) ? data : [];
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

  function IsOutsourceBtn(val) {
    if (val == 1) {
      state.dataForm.F_IsQc = 0;
    }
  }
  function IsQcBtn(val) {
    if (val == 1) {
      state.dataForm.F_IsOutsource = 0;
    }
  }
</script>
