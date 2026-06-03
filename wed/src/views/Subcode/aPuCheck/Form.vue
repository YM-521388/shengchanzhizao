<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    width="70%"
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_CheckDate')">
            <a-form-item name="F_CheckDate" :labelCol="{ style: { width: '100px' } }">
              <template #label>盘点日期</template>
              <JnpfDatePicker
                v-model:value="dataForm.F_CheckDate"
                placeholder="请选择"
                format="yyyy-MM-dd"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_CheckUserId')">
            <a-form-item name="F_CheckUserId" :labelCol="{ style: { width: '100px' } }">
              <template #label>盘点人</template>
              <JnpfUserSelect
                v-model:value="dataForm.F_CheckUserId"
                placeholder="请选择"
                selectType="all"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_WarehouseId')">
            <a-form-item name="F_WarehouseId" :labelCol="{ style: { width: '100px' } }">
              <template #label>盘点仓库</template>
              <JnpfCascader
                v-model:value="dataForm.F_WarehouseId"
                placeholder="请选择"
                :options="optionsObj.F_WarehouseIdOptions"
                :fieldNames="optionsObj.F_WarehouseIdProps"
                allowClear
                showSearch
                :disabled="state.dataForm.id"
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
                :showCount="true" />
            </a-form-item>
          </a-col>

          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="选择库存商品" :bordered="false" />
              <a-col :span="24" class="ant-col-item">
                <a-form-item name="GoodsList" :labelCol="{ style: { width: '100px' } }">
                  <template #label>选择库存商品</template>
                  <JnpfPopupMultipleSelect
                    v-model:value="state.GoodsList"
                    :formData="state.dataForm"
                    placeholder="请选择"
                    :templateJson="optionsObj.GoodsListTemplateJson"
                    allowClear
                    field="GoodsList"
                    interfaceId="2034873228723359744"
                    :columnOptions="optionsObj.GoodsListOptions"
                    relationField="F_Code"
                    @change="GoodsListBtn"
                    propsValue="F_Code"
                    :pageSize="20"
                    popupType="dialog"
                    popupTitle="选择数据"
                    popupWidth="1000px"
                    :disabled="dataForm.id"
                    hasPage
                    :style="{
                      width: '100%',
                    }"
                    :extraOptions="optionsObj.GoodsList" />
                </a-form-item>
              </a-col>
              <a-table
                :data-source="dataForm.tableField91ea29"
                :columns="tableField91ea29Columns"
                size="small"
                rowKey="jnpfId"
                :pagination="listPagination"
                :scroll="{ x: 'max-content' }"
                :rowSelection="gettableField91ea29RowSelection">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                  <template v-if="column.key === 'F_Code'">
                    <JnpfInput
                      v-model:value="record.F_Code"
                      placeholder=""
                      :controls="false"
                      disabled
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_CustomerId'">
                    <JnpfSelect
                      v-model:value="record.F_CustomerId"
                      placeholder=""
                      :options="optionsObj.tableField91ea29_F_CustomerIdOptions"
                      :fieldNames="optionsObj.tableField91ea29_F_CustomerIdProps"
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
                  <template v-if="column.key === 'F_CheckQtyBefore'">
                    <template v-if="getUnitRatioLevels(record)?.level2?.name">
                      <a-space>
                        <JnpfInputNumber v-model:value="record.F_NumLevel1" placeholder="" :controls="false" disabled :style="{ width: '80px' }" />
                        <span>{{ getUnitRatioLevels(record)?.level1?.name }}</span>
                        <JnpfInputNumber v-model:value="record.F_NumLevel2" placeholder="" :controls="false" disabled :style="{ width: '80px' }" />
                        <span>{{ getUnitRatioLevels(record)?.level2?.name }}</span>
                      </a-space>
                    </template>
                    <template v-else>
                      <a-space>
                        <JnpfInputNumber v-model:value="record.F_NumLevel1" placeholder="" :controls="false" disabled :style="{ width: '80px' }" />
                        <span>{{ getUnitRatioLevels(record)?.level1?.name }}</span>
                      </a-space>
                    </template>
                  </template>
                  <template v-if="column.key === 'F_CheckQtyDone'">
                    <template v-if="getUnitRatioLevels(record)?.level2?.name">
                      <a-space>
                        <JnpfInputNumber
                          v-model:value="record.F_CheckQtyDoneLevel1"
                          placeholder="请输入"
                          :controls="false"
                          :min="0"
                          disabled
                          :style="{ width: '80px' }"
                          @change="handleCheckQtyDoneLevel1Change(record)" />
                        <span>{{ getUnitRatioLevels(record)?.level1?.name }}</span>
                        <JnpfInputNumber
                          v-model:value="record.F_CheckQtyDoneLevel2"
                          placeholder="请输入"
                          :controls="false"
                          :min="0"
                          :max="record._F_NumLevel2Max"
                          :disabled="record.F_Id"
                          :style="{ width: '80px' }"
                          @change="handleCheckQtyDoneLevel2Change(record)" />
                        <span>{{ getUnitRatioLevels(record)?.level2?.name }}</span>
                      </a-space>
                    </template>
                    <template v-else>
                      <a-space>
                        <JnpfInputNumber
                          v-model:value="record.F_CheckQtyDoneLevel1"
                          placeholder="请输入"
                          :controls="false"
                          :min="0"
                          :max="1"
                          :disabled="record.F_Id"
                          :style="{ width: '80px' }"
                          @change="handleCheckQtyDoneLevel1Change(record)" />
                        <span>{{ getUnitRatioLevels(record)?.level1?.name }}</span>
                      </a-space>
                    </template>
                  </template>
                  <template v-if="column.key === 'F_Differences'">
                    <template v-if="getUnitRatioLevels(record)?.level2?.name">
                      <a-space>
                        <!-- <JnpfInput
                          v-model:value="record.F_DifferencesLevel1"
                          placeholder=""
                          :controls="false"
                          disabled
                          :style="{
                            width: '80px',
                          }" />
                        <span>{{ getUnitRatioLevels(record)?.level1?.name }}</span> -->
                        <JnpfInput
                          :value="record.F_CheckQtyDoneLevel2 - record.F_NumLevel2"
                          placeholder=""
                          :controls="false"
                          disabled
                          :style="{
                            width: '80px',
                          }" />
                        <span>{{ getUnitRatioLevels(record)?.level2?.name }}</span>
                      </a-space>
                    </template>
                    <template v-else>
                      <a-space>
                        <JnpfInput
                          :value="record.F_CheckQtyDoneLevel1 - record.F_NumLevel1"
                          placeholder=""
                          :controls="false"
                          disabled
                          :style="{
                            width: '80px',
                          }" />
                        <span>{{ getUnitRatioLevels(record)?.level1?.name }}</span>
                      </a-space>
                    </template>
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

                  <template v-if="column.key === 'action' && !state.dataForm.id">
                    <a-space>
                      <a-button class="action-btn" type="link" color="error" @click="removetableField91ea29Row(index, true)" size="small">{{
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
  import { getDataInterfaceDataInfoByIds } from '@/api/systemData/dataInterface';
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
  import { CaretRightOutlined } from '@ant-design/icons-vue';
  import { systemComponentsList } from '@/components/FormGenerator/src/helper/config';
  import { usePermission } from '@/hooks/web/usePermission';

  interface State {
    listPageSize: number;
    listCurrentPage: number;
    GoodsList: any[];
    dataForm: any;
    dataRule: any;
    optionsObj: any;
    currVmodel: any;
    currTableConf: any;
    addTableConf: any;
    tableRows: any;
    tableField91ea29outerActiveKey: number[];
    tableField91ea29innerActiveKey: string[];
    tableField91ea29DefaultExpandAll: boolean;
    selectedtableField91ea29RowKeys: any[];
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
  const tableField91ea29Columns: any[] = computed(() => {
    let list = [
      {
        title: '库存编码',
        dataIndex: 'F_Code',
        key: 'F_Code',
        tipLabel: '',
        required: true,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Code',
        isSystemControl: false,
      },
      {
        title: '商品',
        dataIndex: 'F_CustomerId',
        key: 'F_CustomerId',
        tipLabel: '',
        required: true,
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
        title: '盘点前数量',
        dataIndex: 'F_CheckQtyBefore',
        key: 'F_CheckQtyBefore',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '200px',
        fixed: false,
        formP: 'F_CheckQtyBefore',
        isSystemControl: false,
      },
      {
        title: '已盘点数量',
        dataIndex: 'F_CheckQtyDone',
        key: 'F_CheckQtyDone',
        tipLabel: '',
        required: true,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '200px',
        fixed: false,
        formP: 'F_CheckQtyDone',
        isSystemControl: false,
      },
      {
        title: '差异数量',
        dataIndex: 'F_Differences',
        key: 'F_Differences',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '100px',
        fixed: false,
        formP: 'F_Differences',
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
    list = list.filter(o => hasFormP('tableField91ea29-' + o.formP));
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
  const gettableField91ea29RowSelection = computed(() => {
    const rowSelection = {
      selectedRowKeys: state.selectedtableField91ea29RowKeys,
      onChange: (selectedRowKeys: string[]) => {
        state.selectedtableField91ea29RowKeys = (selectedRowKeys || []).sort().reverse();
      },
    };
    return rowSelection;
  });

  const state = reactive<State>({
    listPageSize: 20,
    listCurrentPage: 1,
    GoodsList: [],
    dataForm: {
      id: '',
      F_CheckDate: dayjs(new Date()).startOf(getDateTimeUnit('yyyy-MM-dd')).valueOf(),
      F_CheckUserId: userInfo.userId ? userInfo.userId : '',
      F_WarehouseId: [],
      F_Description: undefined,
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
      tableField91ea29: [],
    },
    tableField91ea29outerActiveKey: [0],
    tableField91ea29innerActiveKey: [],
    tableField91ea29DefaultExpandAll: true,
    selectedtableField91ea29RowKeys: [],
    dataRule: {
      F_CheckDate: [
        {
          required: true,
          message: '请输入盘点日期',
          trigger: 'change',
        },
      ],
      F_CheckUserId: [
        {
          required: true,
          message: '请输入盘点人',
          trigger: 'change',
        },
      ],
      F_WarehouseId: [
        {
          required: true,
          message: '请输入盘点仓库',
          trigger: 'change',
        },
      ],
    },
    optionsObj: {
      GoodsList: [],
      GoodsListOptions: [
        {
          value: 'F_Code',
          label: '库存编码',
        },
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
          value: 'F_NumInfo',
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
          relationField: 'F_WarehouseId',
          jnpfKey: 'select',
          complexHeaderList: null,
          sourceType: 2,
          isChildren: false,
          IsMultiple: false,
        },
      ],
      F_WarehouseIdProps: {
        label: 'F_Name',
        value: 'F_Id',
        children: 'children',
      },
      tableField91ea29_F_CustomerIdOptions: [],
      tableField91ea29_F_CustomerIdProps: { label: 'F_GoodsName', value: 'id' },
    },
    currVmodel: '',
    currTableConf: {},
    tableRows: {
      tableField91ea29: {
        enabledmark: undefined,
        F_CustomerId: undefined,
        F_Specification: undefined,
        F_Unit: undefined,
        F_CheckQtyBefore: undefined,
        F_CheckQtyDone: undefined,
        F_Description: undefined,
        F_CreatorUserId: undefined,
        F_CreatorTime: undefined,
        F_Unit_Ratio: undefined,
        F_NumLevel1: undefined,
        F_NumLevel2: undefined,
        F_CheckQtyDoneLevel1: undefined,
        F_CheckQtyDoneLevel2: undefined,
        _F_NumLevel2Max: 1,
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

  // 监听盘点仓库变化，更新 relationField 格式
  watch(
    () => state.dataForm.F_WarehouseId,
    newVal => {
      // 将仓库 ID 转换为 relationField 格式（JSON 字符串数组）
      const warehouseIdArr = Array.isArray(newVal) ? newVal : newVal ? [newVal] : [];
      state.optionsObj.GoodsListTemplateJson[0].relationField = JSON.stringify(warehouseIdArr);
    },
  );

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
      state.dataForm.tableField91ea29 = [];
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
        F_CheckDate: dayjs(new Date()).startOf(getDateTimeUnit('yyyy-MM-dd')).valueOf(),
        F_CheckUserId: userInfo.userId ? userInfo.userId : '',
        F_WarehouseId: [],
        F_Description: undefined,
        F_CreatorUserId: undefined,
        F_CreatorTime: undefined,
        tableField91ea29: [],
      };
      state.GoodsList = [];
      getF_WarehouseIdOptions();
      gettableField91ea29_F_CustomerIdOptions();
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
    getInfo(id).then(async res => {
      state.dataForm = res.data || {};
      state.dataForm.tableField91ea29 = state.dataForm.tableField91ea29 || [];
      state.GoodsList = [];

      // 收集需要获取商品信息的 ID
      const goodsIdsToFetch: string[] = [];
      for (let i = 0; i < state.dataForm.tableField91ea29.length; i++) {
        const element = state.dataForm.tableField91ea29[i];
        if (element.F_CustomerId && !element.F_Unit_Ratio) {
          goodsIdsToFetch.push(element.F_CustomerId);
        }
      }

      // 批量获取商品信息
      const goodsIdMap = new Map<string, any>();
      if (goodsIdsToFetch.length > 0) {
        try {
          const uniqueIds = [...new Set(goodsIdsToFetch)];
          const goodsRes = await getDataInterfaceDataInfoByIds('2008721559174385664', {
            ids: uniqueIds,
            interfaceId: '2008721559174385664',
            propsValue: 'F_Id',
            relationField: 'F_GoodsName',
            paramList: [],
          });
          if (goodsRes.data) {
            for (const goods of goodsRes.data) {
              goodsIdMap.set(goods.F_Id, goods);
            }
          }
        } catch (e) {
          console.error('获取商品信息失败', e);
        }
      }

      for (let i = 0; i < state.dataForm.tableField91ea29.length; i++) {
        const element = state.dataForm.tableField91ea29[i];
        element.jnpfId = buildUUID();
        state.GoodsList.push(state.dataForm.tableField91ea29[i].F_Code);

        // 如果没有 F_Unit_Ratio，从商品信息获取
        if (!element.F_Unit_Ratio && element.F_CustomerId) {
          const goodsInfo = goodsIdMap.get(element.F_CustomerId);
          if (goodsInfo) {
            element.F_Unit_Ratio = goodsInfo.F_Unit_Ratio;
          }
        }

        // 使用 F_Unit_Ratio 计算数量层级
        const levels = getUnitRatioLevels(element);
        if (levels && levels.level2?.qty) {
          const ratio = Number(levels.level2.qty) || 1;
          const totalBefore = Number(element.F_CheckQtyBefore) || 0;
          const totalDone = Number(element.F_CheckQtyDone) || 0;
          // 盘点前数量
          element.F_NumLevel1 = 1;
          element.F_NumLevel2 = totalBefore;
          element._F_NumLevel2Max = totalBefore;
          // 已盘点数量
          element.F_CheckQtyDoneLevel1 = 1;
          element.F_CheckQtyDoneLevel2 = totalDone;
          // 差异数量
          const diff = totalDone - totalBefore;
          element.F_Differences = diff;
        } else {
          // 没有二级单位
          const totalBefore = Number(element.F_CheckQtyBefore) || 0;
          const totalDone = Number(element.F_CheckQtyDone) || 0;
          element.F_NumLevel1 = totalBefore;
          element.F_NumLevel2 = 0;
          element._F_NumLevel2Max = totalBefore;
          element.F_CheckQtyDoneLevel1 = totalDone;
          element.F_CheckQtyDoneLevel2 = 0;
          const diff = totalDone - totalBefore;
          element.F_Differences = diff;
        }
      }
      getF_WarehouseIdOptions();
      gettableField91ea29_F_CustomerIdOptions();
      changeLoading(false);
    });
  }
  async function handleSubmit() {
    try {
      // 提交前重新计算 F_CheckQtyDone
      for (const row of state.dataForm.tableField91ea29 || []) {
        const levels = getUnitRatioLevels(row);
        if (levels?.level2?.name) {
          row.F_CheckQtyDone = Number(row.F_CheckQtyDoneLevel2) || 0;
        } else {
          // 无二级单位：F_CheckQtyDone = Level1
          row.F_CheckQtyDone = Number(row.F_CheckQtyDoneLevel1) || 0;
        }
      }
      const values = await getForm()?.validate();
      if (!values) return;
      if (!tableField91ea29Exist()) return;
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
  function getF_WarehouseIdOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2021045201115680768', query).then(res => {
      let data = res.data;
      state.optionsObj.F_WarehouseIdOptions = Array.isArray(data) ? data : [];
    });
  }
  function gettableField91ea29_F_CustomerIdOptions() {
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
      state.optionsObj.tableField91ea29_F_CustomerIdOptions = Array.isArray(data) ? data : [];
    });
  }
  function tableField91ea29Exist() {
    let isOk = true;
    for (let i = 0; i < state.dataForm.tableField91ea29.length; i++) {
      const e = state.dataForm.tableField91ea29[i];
      if (!e.F_CustomerId) {
        createMessage.error('商品' + t('sys.validate.textRequiredSuffix'));
        isOk = false;
        break;
      }
      if (!e.F_CheckQtyDone && e.F_CheckQtyDone != 0) {
        createMessage.error('已盘点数量' + t('sys.validate.textRequiredSuffix'));
        isOk = false;
        break;
      }
    }
    return isOk;
  }
  function addtableField91ea29Row() {
    let item = {
      jnpfId: buildUUID(),
      F_CustomerId: undefined,
      F_Specification: undefined,
      F_Unit: undefined,
      F_CheckQtyBefore: undefined,
      F_CheckQtyDone: undefined,
      F_Description: undefined,
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
    };
    state.dataForm.tableField91ea29.push(item);
    state.tableField91ea29innerActiveKey.push(item.jnpfId);
  }
  function removetableField91ea29Row(index, showConfirm = false) {
    if (showConfirm) {
      createConfirm({
        iconType: 'warning',
        title: '提示',
        content: '此操作将永久删除该数据, 是否继续?',
        onOk: () => {
          state.dataForm.tableField91ea29.splice(index, 1);
          state.GoodsList.splice(index, 1);
        },
      });
    } else {
      state.dataForm.tableField91ea29.splice(index, 1);
      state.GoodsList.splice(index, 1);
    }
  }
  function copytableField91ea29Row(index) {
    let item = cloneDeep(state.dataForm.tableField91ea29[index]);
    let copyData = {};
    for (let i = 0; i < unref(tableField91ea29Columns).length; i++) {
      const cur = unref(tableField91ea29Columns)[i];
      if (cur.key != 'index' && cur.key != 'action') {
        if (cur.children?.length && cur.jnpfKey == 'complexHeader') {
          for (let j = 0; j < cur.children.length; j++) {
            copyData[cur.children[j].key] = cur.isSystemControl ? null : item[cur.children[j].key];
          }
        } else {
          copyData[cur.key] = cur.isSystemControl ? null : item[cur.key];
        }
      }
    }
    const copyItem = { ...copyData, jnpfId: buildUUID() };
    state.dataForm.tableField91ea29.push(copyItem);
    state.tableField91ea29innerActiveKey.push(copyItem.jnpfId);
  }
  function batchRemovetableField91ea29Row(showConfirm = false) {
    if (!state.selectedtableField91ea29RowKeys.length) return createMessage.error('请选择一条数据');
    const handleBatchRemove = () => {
      state.dataForm.tableField91ea29 = state.dataForm.tableField91ea29.filter(o => !state.selectedtableField91ea29RowKeys.includes(o.jnpfId));
      nextTick(() => {
        state.selectedtableField91ea29RowKeys = [];
      });
    };
    if (!showConfirm) return handleBatchRemove();
    createConfirm({
      iconType: 'warning',
      title: '提示',
      content: '此操作将永久删除该数据, 是否继续?',
      onOk: handleBatchRemove,
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

  function handleFromWarehouseChange(value) {
    state.GoodsList = [];
    state.dataForm.tableField91ea29 = [];
  }

  function QtyDoneBtn(jnpfId) {
    // 使用 find 直接获取对象引用
    const row = state.dataForm.tableField91ea29?.find(item => item.jnpfId === jnpfId);

    if (row) {
      // 计算总数量
      const levels = getUnitRatioLevels(row);
      if (levels && levels.level2?.qty) {
        const ratio = Number(levels.level2.qty) || 1;
        row.F_CheckQtyDone = (Number(row.F_CheckQtyDoneLevel1) || 0) * ratio + (Number(row.F_CheckQtyDoneLevel2) || 0);
      } else {
        row.F_CheckQtyDone = Number(row.F_CheckQtyDoneLevel1) || Number(row.F_CheckQtyDoneLevel2) || 0;
      }
      // 计算差异数量
      const diff = (row.F_CheckQtyDone || 0) - (row.F_CheckQtyBefore || 0);
      row.F_Differences = diff;
    }
  }

  // 解析 F_Unit_Ratio，只取前两级，返回 { level1: { name, qty }, level2: { name, qty } } 或 null
  function getUnitRatioLevels(record) {
    const raw = record?.F_Unit_Ratio;
    if (!raw) return null;
    try {
      const str = typeof raw === 'string' ? raw : String(raw);
      const arr = JSON.parse(str);
      if (!Array.isArray(arr) || arr.length < 1) return null;
      if (arr.length < 2) return { level1: arr[0], level2: null };
      return { level1: arr[0], level2: arr[1] };
    } catch (e) {
      return null;
    }
  }

  // 一级数量变化处理
  function handleCheckQtyDoneLevel1Change(record) {
    const levels = getUnitRatioLevels(record);
    if (!levels || !levels.level2?.qty) {
      record.F_CheckQtyDone = Number(record.F_CheckQtyDoneLevel1) || 0;
      QtyDoneBtn(record.jnpfId);
      return;
    }
    // 有二级单位时，一级数量变化会改变总数量
    const ratio = Number(levels.level2.qty) || 1;
    const qty1 = Number(record.F_CheckQtyDoneLevel1) || 0;
    const qty2 = Number(record.F_CheckQtyDoneLevel2) || 0;
    record.F_CheckQtyDone = qty1 * ratio + qty2;
    QtyDoneBtn(record.jnpfId);
  }

  // 二级数量变化处理
  function handleCheckQtyDoneLevel2Change(record) {
    const levels = getUnitRatioLevels(record);
    if (!levels || !levels.level2?.qty) {
      record.F_CheckQtyDone = Number(record.F_CheckQtyDoneLevel2) || 0;
      QtyDoneBtn(record.jnpfId);
      return;
    }
    // 有二级单位时，二级数量变化会改变总数量
    const ratio = Number(levels.level2.qty) || 1;
    const qty1 = Number(record.F_CheckQtyDoneLevel1) || 0;
    const qty2 = Number(record.F_CheckQtyDoneLevel2) || 0;
    record.F_CheckQtyDone = qty1 * ratio + qty2;
    QtyDoneBtn(record.jnpfId);
  }

  async function GoodsListBtn(val, option) {
    /* 兜底 */
    if (!Array.isArray(state.dataForm.tableField91ea29)) {
      state.dataForm.tableField91ea29 = [];
    }
    const optionIdSet = new Set(option.map(o => o.id));

    /* 1. 删掉已取消勾选的行 */
    state.dataForm.tableField91ea29 = state.dataForm.tableField91ea29.filter(item => optionIdSet.has(item.F_CustomerId));

    /* 2. 计算当前最大序号（0 表示还没有任何行） */
    const maxSort = state.dataForm.tableField91ea29.reduce((max, item) => {
      return item.F_SortCode > max ? item.F_SortCode : max;
    }, 0);
    /* 3. 追加本地还没有的行，序号依次累加 */
    let nextSort = maxSort + 1;

    // 收集需要获取商品信息的 ID
    const goodsIdsToFetch: string[] = [];
    const goodsIdMap = new Map<string, any>();

    for (const o of option) {
      const exist = state.dataForm.tableField91ea29.some(item => item.id === o.id);
      if (!exist && o.F_GoodsId && !o.F_Unit_Ratio) {
        goodsIdsToFetch.push(o.F_GoodsId);
      }
    }

    // 批量获取商品信息
    if (goodsIdsToFetch.length > 0) {
      try {
        const uniqueIds = [...new Set(goodsIdsToFetch)];
        const goodsRes = await getDataInterfaceDataInfoByIds('2008721559174385664', {
          ids: uniqueIds,
          interfaceId: '2008721559174385664',
          propsValue: 'F_Id',
          relationField: 'F_GoodsName',
          paramList: [],
        });
        if (goodsRes.data) {
          for (const goods of goodsRes.data) {
            goodsIdMap.set(goods.F_Id, goods);
          }
        }
      } catch (e) {
        console.error('获取商品信息失败', e);
      }
    }

    for (const o of option) {
      const exist = state.dataForm.tableField91ea29.some(item => item.id === o.id);
      if (!exist) {
        // 获取单位比例：优先使用接口返回的，否则从商品信息获取
        let unitRatio = o.F_Unit_Ratio;
        if (!unitRatio && o.F_GoodsId) {
          const goodsInfo = goodsIdMap.get(o.F_GoodsId);
          if (goodsInfo) {
            unitRatio = goodsInfo.F_Unit_Ratio;
          }
        }

        const newRow: any = {
          jnpfId: buildUUID(),
          F_CustomerId: o.F_GoodsId,
          F_Code: o.F_Code,
          F_Specification: o.F_Specification,
          F_Unit: o.F_Unit,
          F_Unit_Ratio: unitRatio,
          F_CheckQtyBefore: o.F_Num,
          F_CheckQtyDone: o.F_Num,
          F_Differences: 0,
          F_Description: undefined,
          F_CreatorUserId: undefined,
          F_CreatorTime: undefined,
        };
        // 计算数量层级
        const levels = getUnitRatioLevels(newRow);
        if (levels && levels.level2?.qty) {
          const ratio = Number(levels.level2.qty) || 1;
          const totalBefore = Number(newRow.F_CheckQtyBefore) || 0;
          const totalDone = Number(newRow.F_CheckQtyDone) || 0;
          // 盘点前数量
          newRow.F_NumLevel1 = 1;
          newRow.F_NumLevel2 = totalBefore;
          newRow._F_NumLevel2Max = ratio;
          // 已盘点数量
          newRow.F_CheckQtyDoneLevel1 = 1;
          newRow.F_CheckQtyDoneLevel2 = totalDone;
        } else {
          // 没有二级单位，只有一级
          newRow.F_NumLevel1 = Number(newRow.F_CheckQtyBefore) || 0;
          newRow.F_NumLevel2 = 0;
          newRow._F_NumLevel2Max = newRow.F_NumLevel1;
          newRow.F_CheckQtyDoneLevel1 = Number(newRow.F_CheckQtyDone) || 0;
          newRow.F_CheckQtyDoneLevel2 = 0;
        }
        state.dataForm.tableField91ea29.push(newRow);
        state.tableField91ea29innerActiveKey.push(newRow.jnpfId);
        nextSort++; // 下一行继续 +1
      }
    }
  }
  const listPagination = computed(() => ({
    current: state.listCurrentPage,
    pageSize: state.listPageSize,
    size: 'small',
    showSizeChanger: true,
    pageSizeOptions: [20, 50, 100],
    showTotal: (total: number) => `共 ${total} 条`,
    onChange: (page: number) => {
      state.listCurrentPage = page;
    },
    onShowSizeChange: (_current: number, size: number) => {
      state.listPageSize = size;
      state.listCurrentPage = 1; // 切换页大小时回到第1页
    },
  }));
</script>
<!-- 全局样式：分页下拉菜单宽度 -->
<style lang="less">
  /* 分页下拉框触发器和弹出菜单都加宽 */
  .ant-pagination-options-size-changer {
    min-width: 100px;
    .ant-select {
      width: 100%;
    }
  }
</style>
