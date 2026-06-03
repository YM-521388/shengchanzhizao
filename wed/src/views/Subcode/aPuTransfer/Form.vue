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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_TransferDate')">
            <a-form-item name="F_TransferDate" :labelCol="{ style: { width: '100px' } }">
              <template #label>调拨日期</template>
              <JnpfDatePicker
                v-model:value="dataForm.F_TransferDate"
                placeholder="请选择"
                format="yyyy-MM-dd"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_TransferUserId')">
            <a-form-item name="F_TransferUserId" :labelCol="{ style: { width: '100px' } }">
              <template #label>调拨人</template>
              <JnpfUserSelect
                v-model:value="dataForm.F_TransferUserId"
                placeholder="请选择"
                selectType="all"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_FromWarehouseId')">
            <a-form-item name="F_FromWarehouseId" :labelCol="{ style: { width: '100px' } }">
              <template #label>发出仓库</template>
              <JnpfCascader
                v-model:value="dataForm.F_FromWarehouseId"
                placeholder="请选择"
                :options="optionsObj.F_FromWarehouseIdOptions"
                :fieldNames="optionsObj.F_FromWarehouseIdProps"
                allowClear
                showSearch
                :changeOnSelect="true"
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_ToWarehouseId')">
            <a-form-item name="F_ToWarehouseId" :labelCol="{ style: { width: '100px' } }">
              <template #label>收入仓库</template>
              <JnpfCascader
                v-model:value="dataForm.F_ToWarehouseId"
                placeholder="请选择"
                :options="optionsObj.F_ToWarehouseIdOptions"
                :fieldNames="optionsObj.F_ToWarehouseIdProps"
                allowClear
                showSearch
                :changeOnSelect="true"
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
              <JnpfGroupTitle content="选择商品" :bordered="false" />
              <a-col :span="24" class="ant-col-item">
                <a-form-item name="GoodsList" :labelCol="{ style: { width: '100px' } }">
                  <template #label>选择商品</template>
                  <JnpfPopupMultipleSelect
                    v-model:value="state.GoodsList"
                    :formData="state.dataForm"
                    placeholder="请选择"
                    :templateJson="optionsObj.GoodsListTemplateJson"
                    allowClear
                    field="GoodsList"
                    interfaceId="2012085692393459712"
                    :columnOptions="optionsObj.GoodsListOptions"
                    relationField="F_GoodsName"
                    @change="GoodsListBtn"
                    propsValue="id"
                    :pageSize="20"
                    popupType="dialog"
                    popupTitle="选择数据"
                    popupWidth="1000px"
                    hasPage
                    :style="{
                      width: '100%',
                    }"
                    :extraOptions="optionsObj.GoodsList" />
                </a-form-item>
              </a-col>
              <a-table
                :data-source="dataForm.tableFieldaf2f04"
                :columns="tableFieldaf2f04Columns"
                size="small"
                rowKey="jnpfId"
                :pagination="false"
                :scroll="{ x: 'max-content' }"
                :rowSelection="gettableFieldaf2f04RowSelection">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}
                  <BasicHelp :text="column.tipLabel" v-if="column.tipLabel" />
                </template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                  <template v-if="column.key === 'F_CustomerId'">
                    <JnpfSelect
                      v-model:value="record.F_CustomerId"
                      placeholder="请选择"
                      :options="optionsObj.tableFieldaf2f04_F_CustomerIdOptions"
                      :fieldNames="optionsObj.tableFieldaf2f04_F_CustomerIdProps"
                      allowClear
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
                  <template v-if="column.key === 'F_Unit'">
                    <JnpfInput
                      v-model:value="record.F_Unit"
                      placeholder=""
                      :controls="false"
                      disabled
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_Inventory'">
                    <JnpfInput
                      v-model:value="record.F_Inventory"
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
                      :precision="0"
                      :min="1"
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <!-- <template v-if="column.key === 'F_Price'">
                    <JnpfInputNumber
                      v-model:value="record.F_Price"
                      placeholder="请输入"
                      :controls="false"
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_SalesPrice'">
                    <JnpfInputNumber
                      v-model:value="record.F_SalesPrice"
                      placeholder="请输入"
                      :controls="false"
                      :style="{
                        width: '100%',
                      }" />
                  </template> -->
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
                      <a-button class="action-btn" type="link" color="error" @click="removetableFieldaf2f04Row(index, true)" size="small">{{
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
  import { getGoodsUnit } from '@/views/Subcode/aGoods/helper/api';
  import { reactive, toRefs, nextTick, ref, unref, computed, toRaw, inject, watch } from 'vue';
  import { BasicModal, useModal } from '@/components/Modal';
  import SelectModal from '@/components/CommonModal/src/SelectModal.vue';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useI18n } from '@/hooks/web/useI18n';
  import { useUserStore } from '@/store/modules/user';
  import type { FormInstance } from 'ant-design-vue';
  import { thousandsFormat } from '@/utils/jnpf';
  import { getTimeUnit, getDateTimeUnit } from '@/utils/jnpf';
  import { getDataInterfaceRes } from '@/api/systemData/dataInterface';
  import dayjs from 'dayjs';
  import { cloneDeep } from 'lodash-es';
  import { buildUUID } from '@/utils/uuid';
  import { usePermission } from '@/hooks/web/usePermission';

  interface State {
    GoodsList: any[];
    dataForm: any;
    dataRule: any;
    optionsObj: any;
    currVmodel: any;
    currTableConf: any;
    addTableConf: any;
    tableRows: any;
    tableFieldaf2f04outerActiveKey: number[];
    tableFieldaf2f04innerActiveKey: string[];
    tableFieldaf2f04DefaultExpandAll: boolean;
    selectedtableFieldaf2f04RowKeys: any[];
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
  const tableFieldaf2f04Columns: any[] = computed(() => {
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
        width: '150px',
        fixed: false,
        formP: 'F_Specification',
        isSystemControl: false,
      },
      {
        title: '库存',
        dataIndex: 'F_Inventory',
        key: 'F_Inventory',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '80px',
        fixed: false,
        formP: 'F_Inventory',
        isSystemControl: false,
      },
      {
        title: '数量',
        dataIndex: 'F_Num',
        key: 'F_Num',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '80px',
        fixed: false,
        formP: 'F_Num',
        isSystemControl: false,
      },
      {
        title: '单位',
        dataIndex: 'F_Unit',
        key: 'F_Unit',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '120px',
        fixed: false,
        formP: 'F_Unit',
        isSystemControl: false,
      },
      // {
      //   title: '成本单价(元)',
      //   dataIndex: 'F_Price',
      //   key: 'F_Price',
      //   tipLabel: '',
      //   required: false,
      //   align: 'left',
      //   span: '24',
      //   labelWidth: '',
      //   width: '100px',
      //   fixed: false,
      //   formP: 'F_Price',
      //   isSystemControl: false,
      // },
      // {
      //   title: '销售单价(元)',
      //   dataIndex: 'F_SalesPrice',
      //   key: 'F_SalesPrice',
      //   tipLabel: '',
      //   required: false,
      //   align: 'left',
      //   span: '24',
      //   labelWidth: '',
      //   width: '100px',
      //   fixed: false,
      //   formP: 'F_SalesPrice',
      //   isSystemControl: false,
      // },
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
      // {
      //   title: '创建人员',
      //   dataIndex: 'F_CreatorUserId',
      //   key: 'F_CreatorUserId',
      //   tipLabel: '',
      //   required: false,
      //   align: 'left',
      //   span: '24',
      //   labelWidth: '',
      //   width: '',
      //   fixed: false,
      //   formP: 'F_CreatorUserId',
      //   isSystemControl: true,
      // },
      // {
      //   title: '创建时间',
      //   dataIndex: 'F_CreatorTime',
      //   key: 'F_CreatorTime',
      //   tipLabel: '',
      //   required: false,
      //   align: 'left',
      //   span: '24',
      //   labelWidth: '',
      //   width: '',
      //   fixed: false,
      //   formP: 'F_CreatorTime',
      //   isSystemControl: true,
      // },
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
  const gettableFieldaf2f04RowSelection = computed(() => {
    const rowSelection = {
      selectedRowKeys: state.selectedtableFieldaf2f04RowKeys,
      onChange: (selectedRowKeys: string[]) => {
        state.selectedtableFieldaf2f04RowKeys = (selectedRowKeys || []).sort().reverse();
      },
    };
    return rowSelection;
  });
  const state = reactive<State>({
    GoodsList: [],
    dataForm: {
      id: '',
      F_TransferDate: dayjs(new Date()).startOf(getDateTimeUnit('yyyy-MM-dd')).valueOf(),
      F_TransferUserId: userInfo.userId ? userInfo.userId : '',
      F_FromWarehouseId: [],
      F_ToWarehouseId: [],
      F_Description: undefined,
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
      tableFieldaf2f04: [],
    },
    tableFieldaf2f04outerActiveKey: [0],
    tableFieldaf2f04innerActiveKey: [],
    tableFieldaf2f04DefaultExpandAll: true,
    selectedtableFieldaf2f04RowKeys: [],
    dataRule: {
      F_TransferDate: [
        {
          required: true,
          message: '请输入调拨日期',
          trigger: 'change',
        },
      ],
      F_FromWarehouseId: [
        {
          required: true,
          message: '请输入发出仓库',
          trigger: 'change',
        },
      ],
      F_ToWarehouseId: [
        {
          required: true,
          message: '请输入收入仓库',
          trigger: 'change',
        },
      ],
    },
    optionsObj: {
      GoodsList: [],
      GoodsListOptions: [
        {
          value: 'F_GoodsName',
          label: '商品名',
        },
        {
          value: 'F_GoodsNo',
          label: '商品编号',
        },
        {
          value: 'F_Specification',
          label: '商品规格',
        },
        // {
        //   value: 'F_CategoryId',
        //   label: '商品类别',
        // },
        {
          value: 'F_Type',
          label: '商品类型',
        },
        {
          value: 'F_InventoryNum',
          label: '库存数量',
        },
        // {
        //   value: 'F_Unit',
        //   label: '单位',
        // },
        // {
        //   value: 'F_InspectRule',
        //   label: '检验规范',
        // },
        {
          value: 'F_Remark',
          label: '备注',
        },
      ],
      GoodsListTemplateJson: [
        {
          defaultValue: '',
          field: 'warehouseId',
          dataType: 'varchar',
          required: 0,
          fieldName: '',
          relationField: 'F_FromWarehouseId',
          jnpfKey: 'select',
          complexHeaderList: null,
          sourceType: 2,
          isChildren: false,
          IsMultiple: false,
        },
      ],
      tableFieldaf2f04_F_CustomerIdOptions: [],
      tableFieldaf2f04_F_CustomerIdProps: { label: 'F_GoodsName', value: 'id' },
      F_FromWarehouseIdProps: {
        label: 'F_Name',
        value: 'F_Id',
        children: 'children',
      },
      F_ToWarehouseIdProps: {
        label: 'F_Name',
        value: 'F_Id',
        children: 'children',
      },
    },
    currVmodel: '',
    currTableConf: {},
    tableRows: {
      tableFieldaf2f04: {
        enabledmark: undefined,
        F_CustomerId: undefined,
        F_Num: undefined,
        F_Price: undefined,
        F_SalesPrice: undefined,
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

  const filteredToWarehouseOptions = computed(() => {
    // 确保选项数组存在，默认为空数组
    const options = state.optionsObj?.F_ToWarehouseIdOptions ?? [];
    const fromWarehouseId = state.dataForm?.F_FromWarehouseId;

    // 如果没有选择发出仓库，返回全部选项
    if (!fromWarehouseId) return options;

    // 安全获取 value 字段名
    const valueKey = state.optionsObj?.F_ToWarehouseIdProps?.value ?? 'value';

    // 过滤掉与发出仓库相同的选项
    return options.filter(option => option?.[valueKey] !== fromWarehouseId);
  });
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
      state.dataForm.tableFieldaf2f04 = [];
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
        F_TransferDate: dayjs(new Date()).startOf(getDateTimeUnit('yyyy-MM-dd')).valueOf(),
        F_TransferUserId: userInfo.userId ? userInfo.userId : '',
        F_FromWarehouseId: [],
        F_ToWarehouseId: [],
        F_Description: undefined,
        F_CreatorUserId: undefined,
        F_CreatorTime: undefined,
        tableFieldaf2f04: [],
      };
      state.GoodsList = [];
      getF_FromWarehouseIdOptions();
      getF_ToWarehouseIdOptions();
      gettableFieldaf2f04_F_CustomerIdOptions();
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
      state.GoodsList = [];
      for (let i = 0; i < state.dataForm.tableFieldaf2f04.length; i++) {
        const element = state.dataForm.tableFieldaf2f04[i];
        element.jnpfId = buildUUID();
        state.GoodsList.push(state.dataForm.tableFieldaf2f04[i].F_CustomerId);
      }
      getF_FromWarehouseIdOptions();
      getF_ToWarehouseIdOptions();
      gettableFieldaf2f04_F_CustomerIdOptions();
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
  // 监听发出仓库变化，打印值并更新 relationField
  watch(
    () => state.dataForm.F_FromWarehouseId,
    (newVal, oldVal) => {
      // 将仓库 ID 转换为 relationField 格式（JSON 字符串数组）
      const warehouseIdArr = Array.isArray(newVal) ? newVal : newVal ? [newVal] : [];
      state.optionsObj.GoodsListTemplateJson[0].relationField = JSON.stringify(warehouseIdArr);
    },
  );

  function handleFromWarehouseChange(value) {
    // 当发出仓库改变时，如果收入仓库选择了相同的值，清空收入仓库
    if (state.dataForm.F_ToWarehouseId === value) {
      state.dataForm.F_ToWarehouseId = undefined;
    }
    state.GoodsList = [];
    state.dataForm.tableFieldaf2f04 = [];
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
  function getF_FromWarehouseIdOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2021045201115680768', query).then(res => {
      let data = res.data;
      state.optionsObj.F_FromWarehouseIdOptions = Array.isArray(data) ? data : [];
    });
  }
  function getF_ToWarehouseIdOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2021045201115680768', query).then(res => {
      let data = res.data;
      state.optionsObj.F_ToWarehouseIdOptions = Array.isArray(data) ? data : [];
    });
  }

  function gettableFieldaf2f04_F_CustomerIdOptions() {
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
      state.optionsObj.tableFieldaf2f04_F_CustomerIdOptions = Array.isArray(data) ? data : [];
    });
  }
  function addtableFieldaf2f04Row() {
    let item = {
      jnpfId: buildUUID(),
      F_CustomerId: undefined,
      F_Num: undefined,
      F_Price: undefined,
      F_SalesPrice: undefined,
      F_Description: undefined,
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
    };
    state.dataForm.tableFieldaf2f04.push(item);
    state.tableFieldaf2f04innerActiveKey.push(item.jnpfId);
  }
  function removetableFieldaf2f04Row(index, showConfirm = false) {
    if (showConfirm) {
      createConfirm({
        iconType: 'warning',
        title: '提示',
        content: '此操作将永久删除该数据, 是否继续?',
        onOk: () => {
          state.dataForm.tableFieldaf2f04.splice(index, 1);
          state.GoodsList.splice(index, 1);
        },
      });
    } else {
      state.dataForm.tableFieldaf2f04.splice(index, 1);
      state.GoodsList.splice(index, 1);
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

  function GoodsListBtn(val, option) {
    /* 兜底 */
    if (!Array.isArray(state.dataForm.tableFieldaf2f04)) {
      state.dataForm.tableFieldaf2f04 = [];
    }
    const optionIdSet = new Set(option.map(o => o.id));

    /* 1. 删掉已取消勾选的行 */
    state.dataForm.tableFieldaf2f04 = state.dataForm.tableFieldaf2f04.filter(item => optionIdSet.has(item.F_CustomerId));

    /* 2. 计算当前最大序号（0 表示还没有任何行） */
    const maxSort = state.dataForm.tableFieldaf2f04.reduce((max, item) => {
      return item.F_SortCode > max ? item.F_SortCode : max;
    }, 0);
    /* 3. 追加本地还没有的行，序号依次累加 */
    let nextSort = maxSort + 1;
    option.forEach(o => {
      const exist = state.dataForm.tableFieldaf2f04.some(item => item.F_CustomerId === o.id);
      if (!exist) {
        getGoodsUnit(o.id).then(res => {
          const newRow = {
            jnpfId: buildUUID(),
            F_CustomerId: o.id,
            F_Specification: o.F_Specification,
            F_Inventory: o.F_Num,
            F_Unit: res?.data,
            F_Num: 1,
            F_Price: o.F_SalePrice,
            F_SalesPrice: o.F_CostPrice,
            F_Description: undefined,
            F_CreatorUserId: undefined,
            F_CreatorTime: undefined,
          };
          state.dataForm.tableFieldaf2f04.push(newRow);
          state.tableFieldaf2f04innerActiveKey.push(newRow.jnpfId);
          nextSort++; // 下一行继续 +1
        });
      }
    });
  }
</script>
