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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_StockInNo')">
            <a-form-item name="F_StockInNo" :labelCol="{ style: { width: '100px' } }">
              <template #label>成品入库单号</template>
              <JnpfInput
                v-model:value="dataForm.F_StockInNo"
                placeholder="请填写，忽略将自动生成"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_StockInDate')">
            <a-form-item name="F_StockInDate" :labelCol="{ style: { width: '100px' } }">
              <template #label>入库日期</template>
              <JnpfDatePicker
                v-model:value="dataForm.F_StockInDate"
                placeholder="请选择"
                format="yyyy-MM-dd"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_WarehouseId')">
            <a-form-item name="F_WarehouseId" :labelCol="{ style: { width: '100px' } }">
              <template #label>入库仓库</template>
              <JnpfSelect
                v-model:value="dataForm.F_WarehouseId"
                placeholder="请选择"
                :options="optionsObj.F_WarehouseIdOptions"
                :fieldNames="optionsObj.F_WarehouseIdProps"
                allowClear
                :disabled="state.dataForm.id"
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
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_StockInType')">
            <a-form-item name="F_StockInType" :labelCol="{ style: { width: '100px' } }">
              <template #label>入库类型</template>
              <JnpfSelect
                v-model:value="dataForm.F_StockInType"
                placeholder="请选择"
                :options="optionsObj.F_StockInTypeOptions"
                :fieldNames="optionsObj.F_StockInTypeProps"
                allowClear
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
                :showCount="true" />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="选择入库商品" :bordered="false" />

              <a-col :span="24" class="ant-col-item" v-if="dataForm.F_Type == '0'">
                <a-form-item name="ProcessingList" :labelCol="{ style: { width: '100px' } }">
                  <template #label>加工单</template>
                  <JnpfPopupMultipleSelect
                    v-model:value="state.ProcessingList"
                    placeholder="请选择"
                    :templateJson="optionsObj.ProcessingListTemplateJson"
                    allowClear
                    field="ProcessingList"
                    interfaceId="2014906814872817664"
                    :columnOptions="optionsObj.ProcessingListOptions"
                    relationField="F_ProcessNo"
                    propsValue="id"
                    :pageSize="20"
                    :disabled="state.dataForm.id"
                    @change="ProcessingBtn"
                    popupType="dialog"
                    popupTitle="选择数据"
                    popupWidth="1000px"
                    hasPage
                    :style="{
                      width: '100%',
                    }"
                    :extraOptions="optionsObj.ProcessingList" />
                </a-form-item>
              </a-col>
              <a-col :span="24" class="ant-col-item" v-if="dataForm.F_Type == '1'">
                <a-form-item name="OutsourceList" :labelCol="{ style: { width: '100px' } }">
                  <template #label>外协工单</template>
                  <JnpfPopupMultipleSelect
                    v-model:value="state.OutsourceList"
                    placeholder="请选择"
                    :templateJson="optionsObj.OutsourceListTemplateJson"
                    allowClear
                    field="OutsourceList"
                    interfaceId="2014907198513221632"
                    :columnOptions="optionsObj.OutsourceListOptions"
                    relationField="F_OutsourceNo"
                    propsValue="id"
                    :pageSize="20"
                    :disabled="state.dataForm.id"
                    @change="OutsourceBtn"
                    popupType="dialog"
                    popupTitle="选择数据"
                    popupWidth="1000px"
                    hasPage
                    :style="{
                      width: '100%',
                    }"
                    :extraOptions="optionsObj.OutsourceList" />
                </a-form-item>
              </a-col>
              <a-table
                :data-source="dataForm.tableFieldc43f9b"
                :columns="tableFieldc43f9bColumns"
                size="small"
                rowKey="jnpfId"
                :pagination="false"
                :scroll="{ x: 'max-content' }"
                :rowSelection="gettableFieldc43f9bRowSelection">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                  <template v-if="column.key === 'F_WorkOrderId'">
                    <JnpfSelect
                      v-model:value="record.F_WorkOrderId"
                      :options="optionsObj.tableFieldc43f9b_F_WorkOrderIdOptions"
                      :fieldNames="optionsObj.tableFieldc43f9b_F_WorkOrderIdProps"
                      placeholder=""
                      allowClear
                      showSearch
                      disabled
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_CustomerId'">
                    <JnpfSelect
                      v-model:value="record.F_CustomerId"
                      :options="optionsObj.tableFieldc43f9b_F_CustomerIdOptions"
                      :fieldNames="optionsObj.tableFieldc43f9b_F_CustomerIdProps"
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
                  <template v-if="column.key === 'F_PlanQty'">
                    <JnpfInput
                      v-model:value="record.F_PlanQty"
                      placeholder=""
                      :controls="false"
                      disabled
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_YseNum'">
                    <JnpfInput
                      v-model:value="record.F_YseNum"
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
                      :min="0"
                      :controls="false"
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_Price'">
                    <JnpfInputNumber
                      v-model:value="record.F_Price"
                      placeholder="请输入"
                      :min="0"
                      :controls="false"
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
                  <template v-if="column.key === 'F_CreatorTime'">
                    <JnpfOpenData
                      v-model:value="record.F_CreatorTime"
                      type="currTime"
                      readonly
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'action'">
                    <a-space>
                      <a-button class="action-btn" type="link" color="error" @click="removetableFieldc43f9bRow(index, true)" size="small">{{
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
    ProcessingList: any[];
    OutsourceList: any[];
    dataForm: any;
    dataRule: any;
    optionsObj: any;
    currVmodel: any;
    currTableConf: any;
    addTableConf: any;
    tableRows: any;
    tableFieldc43f9bouterActiveKey: number[];
    tableFieldc43f9binnerActiveKey: string[];
    tableFieldc43f9bDefaultExpandAll: boolean;
    selectedtableFieldc43f9bRowKeys: any[];
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
  const tableFieldc43f9bColumns: any[] = computed(() => {
    let list = [
      {
        title: '工单号',
        dataIndex: 'F_WorkOrderId',
        key: 'F_WorkOrderId',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_WorkOrderId',
        isSystemControl: false,
      },
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
        width: '150px',
        fixed: false,
        formP: 'F_Specification',
        isSystemControl: false,
      },
      {
        title: '库存数量',
        dataIndex: 'F_InventoryNum',
        key: 'F_InventoryNum',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '100px',
        fixed: false,
        formP: 'F_InventoryNum',
        isSystemControl: false,
      },
      {
        title: '计划数量',
        dataIndex: 'F_PlanQty',
        key: 'F_PlanQty',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '100px',
        fixed: false,
        formP: 'F_PlanQty',
        isSystemControl: false,
      },
      {
        title: '已入库数量',
        dataIndex: 'F_YseNum',
        key: 'F_YseNum',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '100px',
        fixed: false,
        formP: 'F_YseNum',
        isSystemControl: false,
      },
      {
        title: '入库数量',
        dataIndex: 'F_Num',
        key: 'F_Num',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '100px',
        fixed: false,
        formP: 'F_Num',
        isSystemControl: false,
      },
      {
        title: '成本单价(元)',
        dataIndex: 'F_Price',
        key: 'F_Price',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '150px',
        fixed: false,
        formP: 'F_Price',
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
  const gettableFieldc43f9bRowSelection = computed(() => {
    const rowSelection = {
      selectedRowKeys: state.selectedtableFieldc43f9bRowKeys,
      onChange: (selectedRowKeys: string[]) => {
        state.selectedtableFieldc43f9bRowKeys = (selectedRowKeys || []).sort().reverse();
      },
    };
    return rowSelection;
  });
  const state = reactive<State>({
    ProcessingList: [],
    OutsourceList: [],
    dataForm: {
      id: '',
      F_StockInNo: undefined,
      F_StockInDate: undefined,
      F_WarehouseId: undefined,
      F_Type: '0',
      F_StockInType: undefined,
      F_Method: undefined,
      F_Description: undefined,
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
      tableFieldc43f9b: [],
    },
    tableFieldc43f9bouterActiveKey: [0],
    tableFieldc43f9binnerActiveKey: [],
    tableFieldc43f9bDefaultExpandAll: true,
    selectedtableFieldc43f9bRowKeys: [],
    dataRule: {
      F_StockInDate: [
        {
          required: true,
          message: '请输入入库日期',
          trigger: 'change',
        },
      ],
      F_WarehouseId: [
        {
          required: true,
          message: '请输入入库仓库',
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
    },
    optionsObj: {
      ProcessingListOptions: [
        {
          value: 'F_ProcessNo',
          label: '加工单号',
        },
        {
          value: 'F_GoodsName',
          label: '商品名称',
        },
        {
          value: 'F_GoodsNo',
          label: '商品编号',
        },
        {
          value: 'F_Specification',
          label: '商品规格',
        },
        {
          value: 'F_InventoryNum',
          label: '库存数量',
        },
        {
          value: 'F_PlanQty',
          label: '计划数量',
        },
        {
          value: 'F_YseNum',
          label: '已入库数量',
        },
        {
          value: 'F_Description',
          label: '备注',
        },
      ],
      ProcessingListTemplateJson: [],
      OutsourceListOptions: [
        {
          value: 'F_OutsourceNo',
          label: '外协工单号',
        },
        {
          value: 'F_GoodsName',
          label: '商品名称',
        },
        {
          value: 'F_GoodsNo',
          label: '商品编号',
        },
        {
          value: 'F_Specification',
          label: '商品规格',
        },
        {
          value: 'F_InventoryNum',
          label: '库存数量',
        },
        {
          value: 'F_PlanNum',
          label: '计划数量',
        },
        {
          value: 'F_YseNum',
          label: '已入库数量',
        },
        {
          value: 'F_Description',
          label: '备注',
        },
      ],
      ProcessingList: [],
      OutsourceList: [],
      OutsourceListTemplateJson: [],
      F_WarehouseIdProps: { label: 'F_Name', value: 'F_Id' },
      F_TypeProps: { label: 'fullName', value: 'enCode' },
      F_StockInTypeProps: { label: 'fullName', value: 'enCode' },
      F_MethodProps: { label: 'fullName', value: 'enCode' },
      tableFieldc43f9b_F_WorkOrderIdOptions: [],
      tableFieldc43f9b_F_WorkOrderIdProps: { label: 'F_ProcessNo', value: 'id' },
      tableFieldc43f9b_F_CustomerIdOptions: [],
      tableFieldc43f9b_F_CustomerIdProps: { label: 'F_GoodsName', value: 'id' },
    },
    currVmodel: '',
    currTableConf: {},
    tableRows: {
      tableFieldc43f9b: {
        enabledmark: undefined,
        F_WorkOrderId: undefined,
        F_CustomerId: undefined,
        F_Num: undefined,
        F_Price: undefined,
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
    state.dataForm.F_Method = data.F_Method;
    if (data.F_ProcessId) {
      state.dataForm.F_ProcessId = data.F_ProcessId;
    }
    openModal();
    nextTick(() => {
      getForm().resetFields();
      state.dataForm.tableFieldc43f9b = [];
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
        F_StockInNo: undefined,
        F_StockInDate: undefined,
        F_WarehouseId: undefined,
        F_Type: '0',
        F_StockInType: undefined,
        F_Method: undefined,
        F_Description: undefined,
        F_CreatorUserId: undefined,
        F_CreatorTime: undefined,
        tableFieldc43f9b: [],
      };
      (state.ProcessingList = []), (state.OutsourceList = []), getF_WarehouseIdOptions();
      getF_TypeOptions();
      getF_StockInTypeOptions();
      getF_MethodOptions();
      gettableFieldc43f9b_F_WorkOrderIdOptions();
      gettableFieldc43f9b_F_CustomerIdOptions();
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
      state.ProcessingList = [];
      state.OutsourceList = [];
      for (let i = 0; i < state.dataForm.tableFieldc43f9b.length; i++) {
        const element = state.dataForm.tableFieldc43f9b[i];
        element.jnpfId = buildUUID();
        if (state.dataForm.F_Type == '0') {
          state.ProcessingList.push(state.dataForm.tableFieldc43f9b[i].F_WorkOrderId);
        } else {
          state.OutsourceList.push(state.dataForm.tableFieldc43f9b[i].F_WorkOrderId);
        }
      }
      getF_WarehouseIdOptions();
      getF_TypeOptions();
      getF_StockInTypeOptions();
      getF_MethodOptions();
      gettableFieldc43f9b_F_WorkOrderIdOptions();
      gettableFieldc43f9b_F_CustomerIdOptions();
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
  function getF_TypeOptions() {
    getDictionaryDataSelector('2014894783159472128').then(res => {
      state.optionsObj.F_TypeOptions = res.data.list;
    });
  }
  function getF_StockInTypeOptions() {
    getDictionaryDataSelector('2012074650393251840').then(res => {
      state.optionsObj.F_StockInTypeOptions = res.data.list;
    });
  }
  function getF_MethodOptions() {
    getDictionaryDataSelector('2014907575941861376').then(res => {
      state.optionsObj.F_MethodOptions = res.data.list;
    });
  }
  //加工单
  function gettableFieldc43f9b_F_WorkOrderIdOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2014906814872817664', query).then(res => {
      let data = res.data;
      state.optionsObj.tableFieldc43f9b_F_WorkOrderIdOptions = Array.isArray(data) ? data : [];
    });
  }
  //外协工单
  function gettableFieldc43f9b_F_OutsourceIdOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2014907198513221632', query).then(res => {
      let data = res.data;
      state.optionsObj.tableFieldc43f9b_F_WorkOrderIdOptions = Array.isArray(data) ? data : [];
    });
  }
  function gettableFieldc43f9b_F_CustomerIdOptions() {
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
      state.optionsObj.tableFieldc43f9b_F_CustomerIdOptions = Array.isArray(data) ? data : [];
    });
  }
  function removetableFieldc43f9bRow(index, showConfirm = false) {
    if (showConfirm) {
      createConfirm({
        iconType: 'warning',
        title: '提示',
        content: '此操作将永久删除该数据, 是否继续?',
        onOk: () => {
          state.dataForm.tableFieldc43f9b.splice(index, 1);
          if (state.dataForm.F_Type == '0') {
            state.ProcessingList.splice(index, 1);
          } else {
            state.OutsourceList.splice(index, 1);
          }
        },
      });
    } else {
      state.dataForm.tableFieldc43f9b.splice(index, 1);
      if (state.dataForm.F_Type == '0') {
        state.ProcessingList.splice(index, 1);
      } else {
        state.OutsourceList.splice(index, 1);
      }
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

  //工单类型
  function ProcessBtn(val, option) {
    if (val == '0') {
      gettableFieldc43f9b_F_WorkOrderIdOptions();
    } else if (val == '1') {
      gettableFieldc43f9b_F_OutsourceIdOptions();
    }
    if (val == '0') {
      state.OutsourceList = [];
      state.dataForm.tableFieldc43f9b = [];
    } else if (val == '1') {
      state.ProcessingList = [];
      state.dataForm.tableFieldc43f9b = [];
    }
  }

  function ProcessingBtn(val, option) {
    /* 兜底 */
    if (!Array.isArray(state.dataForm.tableFieldc43f9b)) {
      state.dataForm.tableFieldc43f9b = [];
    }
    const optionIdSet = new Set(option.map(o => o.id));

    /*  删掉已取消勾选的行 */
    state.dataForm.tableFieldc43f9b = state.dataForm.tableFieldc43f9b.filter(item => optionIdSet.has(item.F_WorkOrderId));

    option.forEach(o => {
      const exist = state.dataForm.tableFieldc43f9b.some(item => item.F_WorkOrderId === o.id);
      if (!exist) {
        const newRow = {
          jnpfId: buildUUID(),
          F_WorkOrderId: o.id,
          F_CustomerId: o.F_GoodsId,
          F_GoodsName: o.F_GoodsName,
          F_GoodsNo: o.F_GoodsNo,
          F_Specification: o.F_Specification,
          F_PlanQty: o.F_PlanQty,
          F_InventoryNum: o.F_InventoryNum,
          F_YseNum: o.F_YseNum,
          F_Num: undefined,
          F_Price: undefined,
          F_Description: undefined,
          F_CreatorTime: undefined,
        };
        state.dataForm.tableFieldc43f9b.push(newRow);
        state.tableFieldc43f9binnerActiveKey.push(newRow.jnpfId);
      }
    });
  }

  function OutsourceBtn(val, option) {
    /* 兜底 */
    if (!Array.isArray(state.dataForm.tableFieldc43f9b)) {
      state.dataForm.tableFieldc43f9b = [];
    }
    const optionIdSet = new Set(option.map(o => o.id));

    /*  删掉已取消勾选的行 */
    state.dataForm.tableFieldc43f9b = state.dataForm.tableFieldc43f9b.filter(item => optionIdSet.has(item.F_WorkOrderId));

    option.forEach(o => {
      const exist = state.dataForm.tableFieldc43f9b.some(item => item.F_WorkOrderId === o.id);
      if (!exist) {
        const newRow = {
          jnpfId: buildUUID(),
          F_WorkOrderId: o.id,
          F_CustomerId: o.F_GoodsId,
          F_GoodsName: o.F_GoodsName,
          F_GoodsNo: o.F_GoodsNo,
          F_Specification: o.F_Specification,
          F_PlanQty: o.F_PlanQty,
          F_InventoryNum: o.F_InventoryNum,
          F_YseNum: o.F_YseNum,
          F_Num: undefined,
          F_Price: undefined,
          F_Description: undefined,
          F_CreatorTime: undefined,
        };
        state.dataForm.tableFieldc43f9b.push(newRow);
        state.tableFieldc43f9binnerActiveKey.push(newRow.jnpfId);
      }
    });
  }
</script>
