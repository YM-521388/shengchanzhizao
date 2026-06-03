<template>
  <BasicModal v-bind="$attrs" @register="registerModal" width="70%" :minHeight="100" okText="确定" cancelText="取消" @ok="handleSubmit" :closeFunc="onClose">
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_StockOutNo')">
            <a-form-item name="F_StockOutNo" :labelCol="{ style: { width: '100px' } }">
              <template #label>生产出库单号</template>
              <JnpfInput
                v-model:value="dataForm.F_StockOutNo"
                placeholder="请填写，忽略将自动生成"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_WarehouseId')">
            <a-form-item name="F_WarehouseId" :labelCol="{ style: { width: '100px' } }">
              <template #label>仓库</template>
              <JnpfSelect
                v-model:value="dataForm.F_WarehouseId"
                placeholder="请选择"
                :options="optionsObj.F_WarehouseIdOptions"
                :fieldNames="optionsObj.F_WarehouseIdProps"
                allowClear
                :disabled="state.dataForm.id"
                @change="WorkOrderChange"
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_StockOutType')">
            <a-form-item name="F_StockOutType" :labelCol="{ style: { width: '100px' } }">
              <template #label>出库类型</template>
              <JnpfSelect
                v-model:value="dataForm.F_StockOutType"
                placeholder="请选择"
                :options="optionsObj.F_StockOutTypeOptions"
                :fieldNames="optionsObj.F_StockOutTypeProps"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_StockOutDate')">
            <a-form-item name="F_StockOutDate" :labelCol="{ style: { width: '100px' } }">
              <template #label>出库日期</template>
              <JnpfDatePicker
                v-model:value="dataForm.F_StockOutDate"
                placeholder="请选择"
                format="yyyy-MM-dd"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_Type')">
            <a-form-item name="F_Type" :labelCol="{ style: { width: '100px' } }">
              <template #label>工单类型</template>
              <JnpfSelect
                v-model:value="dataForm.F_Type"
                placeholder="请选择"
                :options="optionsObj.F_TypeOptions"
                :fieldNames="optionsObj.F_TypeProps"
                :disabled="state.dataForm.id"
                @change="ProcessBtn"
                showSearch
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_WorkOrderId')">
            <a-form-item name="F_WorkOrderId" :labelCol="{ style: { width: '100px' } }">
              <template #label>工单号</template>
              <JnpfSelect
                v-model:value="dataForm.F_WorkOrderId"
                placeholder="请选择"
                :options="optionsObj.F_WorkOrderIdOptions"
                :fieldNames="optionsObj.F_WorkOrderIdProps"
                allowClear
                @change="WorkOrderChange"
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_Num')">
            <a-form-item name="F_Num" :labelCol="{ style: { width: '100px' } }">
              <template #label>出库套数</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_Num"
                placeholder="请输入"
                :controls="false"
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_Description')">
            <a-form-item name="F_Description" :labelCol="{ style: { width: '100px' } }">
              <template #label>备注</template>
              <JnpfTextarea
                v-model:value="dataForm.F_Description"
                placeholder="请输入"
                allowClear
                :autoSize="{
                  minRows: 2,
                  maxRows: 4,
                }"
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="商品" :bordered="false" />
              <a-table
                :data-source="dataForm.tableField2752cf"
                :columns="tableField2752cfColumns"
                size="small"
                rowKey="jnpfId"
                :pagination="false"
                :scroll="{ x: 'max-content' }"
                :rowSelection="gettableField2752cfRowSelection">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>

                  <template v-if="column.key === 'F_CustomerId'">
                    <JnpfSelect
                      v-model:value="record.F_CustomerId"
                      :options="optionsObj.tableField449e6c_F_CustomerIdOptions"
                      :fieldNames="optionsObj.tableField449e6c_F_CustomerIdProps"
                      placeholder=""
                      allowClear
                      showSearch
                      disabled
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_Specification'">
                    <JnpfInput
                      v-model:value="record.F_Specification"
                      placeholder=""
                      :controls="false"
                      disabled
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_InventoryNum'">
                    <JnpfInput
                      v-model:value="record.F_InventoryNum"
                      placeholder=""
                      :controls="false"
                      disabled
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_Num'">
                    <JnpfInputNumber
                      v-model:value="record.F_Num"
                      placeholder="请输入"
                      :controls="false"
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_Price'">
                    <JnpfInputNumber
                      v-model:value="record.F_Price"
                      placeholder="请输入"
                      :controls="false"
                      thousands
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_UnitTwo'">
                    <JnpfInput
                      v-model:value="record.F_UnitTwo"
                      placeholder=""
                      :controls="false"
                      disabled
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_Description'">
                    <JnpfTextarea
                      v-model:value="record.F_Description"
                      placeholder="请输入"
                      allowClear
                      :autoSize="{
                        minRows: 1,
                        maxRows: 4,
                      }"
                      :style="{
                        width: '100%',
                      }"
                      :showCount="false" />
                  </template>
                  <template v-if="column.key === 'action'">
                    <a-space>
                      <a-button class="action-btn" type="link" color="error" @click="removetableField2752cfRow(index, true)" size="small">{{
                        t('common.delText', '删除')
                      }}</a-button>
                    </a-space>
                  </template>
                </template>
              </a-table>
              <a-space class="input-table-footer-btn"> </a-space>
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
  import { GetProcessingGoodsList } from '@/views/Subcode/aProdProcess/helper/api';
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
  import { usePermission } from '@/hooks/web/usePermission';

  interface State {
    dataForm: any;
    dataRule: any;
    optionsObj: any;
    currVmodel: any;
    currTableConf: any;
    addTableConf: any;
    tableRows: any;
    tableField2752cfouterActiveKey: number[];
    tableField2752cfinnerActiveKey: string[];
    tableField2752cfDefaultExpandAll: boolean;
    selectedtableField2752cfRowKeys: any[];
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
  const tableField2752cfColumns: any[] = computed(() => {
    let list = [
      {
        title: '商品',
        dataIndex: 'F_CustomerId',
        key: 'F_CustomerId',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_CustomerId',
        isSystemControl: false,
      },
      {
        title: '商品规格',
        dataIndex: 'F_Specification',
        key: 'F_Specification',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Specification',
        isSystemControl: false,
      },
      {
        title: '库存',
        dataIndex: 'F_InventoryNum',
        key: 'F_InventoryNum',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_InventoryNum',
        isSystemControl: false,
      },
      {
        title: '出库数量',
        dataIndex: 'F_Num',
        key: 'F_Num',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Num',
        isSystemControl: false,
      },
      {
        thousands: true,
        title: '成本单价(元)',
        dataIndex: 'F_Price',
        key: 'F_Price',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Price',
        isSystemControl: false,
      },
      {
        title: '单位',
        dataIndex: 'F_UnitTwo',
        key: 'F_UnitTwo',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_UnitTwo',
        isSystemControl: false,
      },
      {
        title: '备注',
        dataIndex: 'F_Description',
        key: 'F_Description',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Description',
        isSystemControl: false,
      },
    ];
    list = list.filter(o => hasFormP('tableField2752cf-' + o.formP));
    let columnList = list;
    let complexHeaderList: any[] = [];
    if (complexHeaderList.length) {
      let childColumns: any[] = [];
      let firstChildColumns: string[] = [];
      for (let i = 0; i < complexHeaderList.length; i++) {
        const e = complexHeaderList[i];
        e.title = e.fullName;
        e.align = e.align;
        e.children = [];
        e.jnpfKey = 'complexHeader';
        if (e.childColumns?.length) {
          childColumns.push(...e.childColumns);
          for (let k = 0; k < e.childColumns.length; k++) {
            const item = e.childColumns[k];
            for (let j = 0; j < list.length; j++) {
              const o = list[j];
              if (o.dataIndex == item && o.fixed !== 'left' && o.fixed !== 'right') e.children.push({ ...o });
            }
          }
        }
        if (e.children.length) firstChildColumns.push(e.children[0].dataIndex);
      }
      complexHeaderList = complexHeaderList.filter(o => o.children.length);
      let newList: any[] = [];
      for (let i = 0; i < list.length; i++) {
        const e = list[i];
        if (!childColumns.includes(e.dataIndex) || e.fixed === 'left' || e.fixed === 'right') {
          newList.push(e);
        } else {
          if (firstChildColumns.includes(e.dataIndex)) {
            const item = complexHeaderList.find(o => o.childColumns.includes(e.dataIndex));
            newList.push(item);
          }
        }
      }
      columnList = newList;
    }
    const noColumn = { title: t('component.table.index'), dataIndex: 'index', key: 'index', align: 'center', width: 50, fixed: 'left' };
    const actionColumn = { title: t('component.table.action'), dataIndex: 'action', key: 'action', align: 'center', width: 100, fixed: 'right' };
    let columns = [noColumn, ...columnList, actionColumn];
    const leftFixedList = columns.filter(o => o.fixed === 'left');
    const rightFixedList = columns.filter(o => o.fixed === 'right');
    const noFixedList = columns.filter(o => o.fixed !== 'left' && o.fixed !== 'right');
    return [...leftFixedList, ...noFixedList, ...rightFixedList];
  });
  const gettableField2752cfRowSelection = computed(() => {
    const rowSelection = {
      selectedRowKeys: state.selectedtableField2752cfRowKeys,
      onChange: (selectedRowKeys: string[]) => {
        state.selectedtableField2752cfRowKeys = (selectedRowKeys || []).sort().reverse();
      },
    };
    return rowSelection;
  });
  const state = reactive<State>({
    dataForm: {
      id: '',
      F_StockOutNo: undefined,
      F_WarehouseId: undefined,
      F_StockOutType: undefined,
      F_StockOutDate: undefined,
      F_WorkOrderId: undefined,
      F_Num: undefined,
      F_Type: '0',
      F_Method: undefined,
      F_Description: undefined,
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
      tableField2752cf: [],
    },
    tableField2752cfouterActiveKey: [0],
    tableField2752cfinnerActiveKey: [],
    tableField2752cfDefaultExpandAll: true,
    selectedtableField2752cfRowKeys: [],
    dataRule: {
      F_WarehouseId: [
        {
          required: true,
          message: '请输入仓库',
          trigger: 'change',
        },
      ],
      F_Type: [
        {
          required: true,
          message: '请输入工单类型',
          trigger: 'change',
        },
      ],
      F_WorkOrderId: [
        {
          required: true,
          message: '请输入工单号',
          trigger: 'change',
        },
      ],
    },
    optionsObj: {
      tableField449e6c_F_CustomerIdOptions: [],
      tableField449e6c_F_CustomerIdProps: { label: 'F_GoodsName', value: 'id' },
      F_WarehouseIdProps: { label: 'F_Name', value: 'F_Id' },
      F_StockOutTypeProps: { label: 'fullName', value: 'enCode' },

      F_TypeProps: { label: 'fullName', value: 'enCode' },
      F_WorkOrderIdProps: { label: 'F_ProcessNo', value: 'id' },
      F_MethodProps: { label: 'fullName', value: 'enCode' },
      tableField2752cf_F_CustomerIdOptions: [],
      tableField2752cf_F_CustomerIdTemplateJson: [],
    },
    currVmodel: '',
    currTableConf: {},
    tableRows: {
      tableField2752cf: {
        enabledmark: undefined,
        F_CustomerId: undefined,
        F_InventoryNum: undefined,
        F_Specification: undefined,
        F_UnitTwo: undefined,
        F_Num: undefined,
        F_Price: undefined,
        F_Description: undefined,
        F_CreatorUserId: undefined,
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
    state.dataForm.F_Method = data.F_Method;
    openModal();
    state.allList = data.allList;
    state.currIndex = state.allList.length && data.id ? state.allList.findIndex(item => item.id === data.id) : 0;
    nextTick(() => {
      getForm().resetFields();
      state.dataForm.tableField2752cf = [];
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
        F_StockOutNo: undefined,
        F_WarehouseId: undefined,
        F_StockOutType: undefined,
        F_StockOutDate: undefined,
        F_WorkOrderId: undefined,
        F_Num: undefined,
        F_Type: '0',
        F_Method: state.dataForm.F_Method,
        F_Description: undefined,
        F_CreatorUserId: undefined,
        F_CreatorTime: undefined,
        tableField2752cf: [],
      };
      getF_WarehouseIdOptions();
      getF_StockOutTypeOptions();
      getF_WorkOrderIdOptions();
      getF_TypeOptions();
      getF_MethodOptions();
      gettableField449e6c_F_CustomerIdOptions();
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
      for (let i = 0; i < state.dataForm.tableField2752cf.length; i++) {
        const element = state.dataForm.tableField2752cf[i];
        element.jnpfId = buildUUID();
      }
      getF_WarehouseIdOptions();
      getF_StockOutTypeOptions();
      getF_TypeOptions();
      if (state.dataForm.F_Type == '0') {
        getF_WorkOrderIdOptions();
      } else if (state.dataForm.F_Type == '1') {
        getF_OutsourceIdOptions();
      }
      getF_MethodOptions();
      gettableField449e6c_F_CustomerIdOptions();
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
  function gettableField449e6c_F_CustomerIdOptions() {
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
      state.optionsObj.tableField449e6c_F_CustomerIdOptions = Array.isArray(data) ? data : [];
    });
  }
  function getF_MethodOptions() {
    getDictionaryDataSelector('2014907575941861376').then(res => {
      state.optionsObj.F_MethodOptions = res.data.list;
    });
  }
  function getF_TypeOptions() {
    getDictionaryDataSelector('2014894783159472128').then(res => {
      state.optionsObj.F_TypeOptions = res.data.list;
    });
  }
  //外协工单
  function getF_OutsourceIdOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2014969808717746176', query).then(res => {
      let data = res.data;
      state.optionsObj.F_WorkOrderIdOptions = Array.isArray(data) ? data : [];
    });
  }
  //加工单
  function getF_WorkOrderIdOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2013860549426810880', query).then(res => {
      let data = res.data;
      state.optionsObj.F_WorkOrderIdOptions = Array.isArray(data) ? data : [];
    });
  }
  function getF_WarehouseIdOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2012073572784279552', query).then(res => {
      let data = res.data;
      state.optionsObj.F_WarehouseIdOptions = Array.isArray(data) ? data : [];
    });
  }
  function getF_StockOutTypeOptions() {
    getDictionaryDataSelector('2013096194263355392').then(res => {
      state.optionsObj.F_StockOutTypeOptions = res.data.list;
    });
  }
  function removetableField2752cfRow(index, showConfirm = false) {
    if (showConfirm) {
      createConfirm({
        iconType: 'warning',
        title: '提示',
        content: '此操作将永久删除该数据, 是否继续?',
        onOk: () => {
          state.dataForm.tableField2752cf.splice(index, 1);
        },
      });
    } else {
      state.dataForm.tableField2752cf.splice(index, 1);
    }
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

  function WorkOrderChange(val, option) {
    if (state.dataForm.F_WorkOrderId && state.dataForm.F_WarehouseId) {
      GetProcessingGoodsList(val, state.dataForm.F_WarehouseId).then(res => {
        state.dataForm.tableField2752cf = [];
        res.data.forEach(o => {
          const newRow = {
            jnpfId: buildUUID(),
            F_CustomerId: o.F_GoodsId,
            F_InventoryNum: o.F_InventoryNum,
            F_Specification: o.F_Specification,
            F_UnitTwo: o.F_UnitTwo,
            F_Num: o.F_Unit,
            F_Price: undefined,
            F_Description: undefined,
            F_CreatorUserId: undefined,
            F_CreatorTime: undefined,
          };
          state.dataForm.tableField2752cf.push(newRow);
          state.tableField2752cfinnerActiveKey.push(newRow.jnpfId);
        });
      });
    }
  }

  //工单类型
  function ProcessBtn(val, option) {
    if (val == '0') {
      getF_WorkOrderIdOptions();
    } else if (val == '1') {
      getF_OutsourceIdOptions();
    }
    state.dataForm.F_WorkOrderId = undefined;
    state.dataForm.tableField2752cf = [];
  }
</script>
