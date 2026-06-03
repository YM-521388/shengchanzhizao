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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('SCTL_ReturnInNo')">
            <a-form-item name="F_ReturnInNo" :labelCol="{ style: { width: '100px' } }">
              <template #label>生产退料单号</template>
              <JnpfInput
                v-model:value="dataForm.F_ReturnInNo"
                placeholder="请填写，忽略将自动生成"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>

          <a-col :span="8" class="ant-col-item" v-if="hasFormP('SCTL_ReturnInType')">
            <a-form-item name="F_ReturnInType" :labelCol="{ style: { width: '100px' } }">
              <template #label>入库类型</template>
              <JnpfSelect
                v-model:value="dataForm.F_ReturnInType"
                placeholder="请选择"
                :options="optionsObj.F_ReturnInTypeOptions"
                :fieldNames="optionsObj.F_ReturnInTypeProps"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <!-- <a-col :span="8" class="ant-col-item" v-if="hasFormP('SCTL_Type')">
            <a-form-item name="F_Type" :labelCol="{ style: { width: '100px' } }">
              <template #label>工单类型</template>
              <JnpfSelect
                v-model:value="dataForm.F_Type"
                placeholder="请选择"
                :options="optionsObj.F_TypeOptions"
                :fieldNames="optionsObj.F_TypeProps"
                :disabled="state.dataForm.id || state.WorkOrderId != ''"
                @change="ProcessBtn"
                showSearch
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col> -->
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('SCTL_WorkOrderId')">
            <a-form-item name="F_WorkOrderId" :labelCol="{ style: { width: '100px' } }">
              <template #label>加工单号</template>
              <JnpfSelect
                v-model:value="dataForm.F_WorkOrderId"
                placeholder="请选择"
                :options="optionsObj.F_WorkOrderIdOptions"
                :fieldNames="optionsObj.F_WorkOrderIdProps"
                allowClear
                :disabled="!!state.dataForm.id"
                @change="WorkOrderChange"
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('SCTL_ReturnInDate')">
            <a-form-item name="F_ReturnInDate" :labelCol="{ style: { width: '100px' } }">
              <template #label>退料日期</template>
              <JnpfDatePicker
                v-model:value="dataForm.F_ReturnInDate"
                placeholder="请选择"
                format="yyyy-MM-dd"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <!-- <a-col :span="8" class="ant-col-item" v-if="hasFormP('SCTL_Method')">
            <a-form-item name="F_Method" :labelCol="{ style: { width: '100px' } }">
              <template #label>操作方式</template>
              <JnpfInput
                v-model:value="dataForm.F_Method"
                placeholder="请输入"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col> -->
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('SCTL_Description')">
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

          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="商品" :bordered="false" />
              <a-row :gutter="15">
                <a-col :span="16" class="ant-col-item">
                  <a-form-item :labelCol="{ style: { width: '100px' } }">
                    <template #label>库存商品选择</template>
                    <JnpfPopupSelect
                      v-model:value="state.tempGoodsSelector"
                      :formData="state.dataForm"
                      placeholder="请先选择工单号，再进行商品选择"
                      :templateJson="optionsObj.tableFieldbb1084_F_CustomerIdTemplateJson"
                      allowClear
                      :field="'tempGoodsSelector'"
                      :interfaceId="state.goodsInterfaceId"
                      :columnOptions="optionsObj.tableFieldbb1084_F_CustomerIdOptions"
                      relationField="F_GoodsName"
                      propsValue="F_Code"
                      :pageSize="20"
                      popupType="dialog"
                      popupTitle="选择数据"
                      popupWidth="50%"
                      hasPage
                      :disabled="!!state.dataForm.id"
                      multiple
                      :style="{
                        width: '100%',
                      }"
                      :extraOptions="optionsObj.tableFieldbb1084_F_CustomerId"
                      @change="(ids, rows) => handleGoodsSelectorChange(ids, rows)" />
                  </a-form-item>
                </a-col>
                <a-col :span="8" class="ant-col-item" v-if="hasFormP('SCTL_WarehouseId')">
                  <a-form-item name="F_WarehouseId" :labelCol="{ style: { width: '100px' } }">
                    <template #label>仓库</template>
                    <JnpfCascader
                      v-model:value="dataForm.F_WarehouseId"
                      placeholder=""
                      :options="optionsObj.F_WarehouseIdOptions"
                      :fieldNames="optionsObj.F_WarehouseIdProps"
                      allowClear
                      :changeOnSelect="true"
                      showSearch
                      :style="{
                        width: '100%',
                      }" />
                  </a-form-item>
                </a-col>
              </a-row>

              <a-table
                :data-source="dataForm.tableFieldbb1084"
                :columns="tableFieldbb1084Columns"
                size="small"
                rowKey="jnpfId"
                :pagination="listPagination"
                :scroll="{ x: 'max-content' }"
                :rowSelection="gettableFieldbb1084RowSelection">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                  <template v-if="column.key === 'F_CustomerId'">
                    <JnpfPopupSelect
                      v-model:value="record.F_CustomerId"
                      placeholder="请选择"
                      :templateJson="optionsObj.tableFieldbb1084_F_CustomerIdTemplateJson"
                      allowClear
                      :field="'F_CustomerId' + index"
                      interfaceId="2008721559174385664"
                      :columnOptions="optionsObj.tableFieldbb1084_F_CustomerIdOptions"
                      relationField="F_GoodsName"
                      propsValue="F_Id"
                      :pageSize="20"
                      :disabled="true"
                      popupType="dialog"
                      popupTitle="选择数据"
                      popupWidth="800px"
                      hasPage
                      :style="{
                        width: '100%',
                      }"
                      :extraOptions="optionsObj.tableFieldbb1084_F_CustomerId" />
                  </template>
                  <template v-if="column.key === 'F_Num'">
                    <template v-if="getUnitRatioLevels(record)?.level2?.name">
                      <a-space>
                        <JnpfInputNumber
                          v-model:value="record.F_NumLevel1"
                          :placeholder="record._quantityDisabled ? '自动带出' : '请输入'"
                          :controls="false"
                          disabled
                          :style="{ width: '80px' }"
                          @change="handleNumLevel1Change(record)" />
                        <span>{{ getUnitRatioLevels(record)?.level1?.name }}</span>
                        <JnpfInputNumber
                          v-model:value="record.F_NumLevel2"
                          placeholder="请输入"
                          :controls="false"
                          :disabled="state.dataForm.id"
                          :min="1"
                          :max="record._F_NumLevel2Max"
                          :style="{ width: '80px' }"
                          @change="handleNumLevel2Change(record)" />
                        <span>{{ getUnitRatioLevels(record)?.level2?.name }}</span>
                      </a-space>
                    </template>
                    <template v-else>
                      <a-space>
                        <JnpfInputNumber
                          v-model:value="record.F_NumLevel2"
                          placeholder="请输入"
                          :controls="false"
                          disabled
                          :min="1"
                          :max="record._F_NumLevel2Max"
                          :style="{ width: '80px' }"
                          @change="handleNumLevel2Change(record)" />
                        <span>{{ getUnitRatioLevels(record)?.level1?.name }}</span>
                      </a-space>
                    </template>
                  </template>
                  <template v-if="column.key === 'F_Encoding'">
                    <JnpfInput
                      v-model:value="record.F_Encoding"
                      placeholder=""
                      allowClear
                      disabled
                      :style="{
                        width: '100%',
                      }"
                      :showCount="false" />
                  </template>
                  <template v-if="column.key === 'F_Price'">
                    <JnpfInputNumber
                      v-model:value="record.F_Price"
                      placeholder="请输入"
                      :controls="false"
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_WarehouseID'">
                    <JnpfCascader
                      v-model:value="record.F_WarehouseID"
                      placeholder="请选择"
                      :options="optionsObj.tableFieldbb1084_F_WarehouseIDOptions"
                      :fieldNames="optionsObj.tableFieldbb1084_F_WarehouseIDProps"
                      :disabled="state.dataForm.id"
                      allowClear
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_TotalPrice'">
                    <span>{{ ((parseFloat(record.F_Num) || 0) * (parseFloat(record.F_Price) || 0)).toFixed(2) }}</span>
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
                  <template v-if="column.key === 'F_CreatorUserId'">
                    <JnpfOpenData
                      v-model:value="record.F_CreatorUserId"
                      type="currUser"
                      readonly
                      :style="{
                        width: '100%',
                      }" />
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
                  <template v-if="column.key === 'action' && !state.dataForm.id">
                    <a-space>
                      <!-- <a-button class="action-btn" type="link" @click="copytableFieldbb1084Row(index)" size="small">{{
                        t('common.copyText', '复制')
                      }}</a-button> -->
                      <a-button class="action-btn" type="link" color="error" @click="removetableFieldbb1084Row(index, true)" size="small">{{
                        t('common.delText', '删除')
                      }}</a-button>
                    </a-space>
                  </template>
                </template>
              </a-table>
              <a-space class="input-table-footer-btn">
                <!-- <a-button type="primary" preIcon="icon-ym icon-ym-btn-add" @click="addtableFieldbb1084Row">{{ t('common.add1Text', '添加') }}</a-button>
                <a-button type="danger" preIcon="icon-ym icon-ym-btn-clearn" @click="batchRemovetableFieldbb1084Row(true)">{{
                  t('common.batchDelText', '批量删除')
                }}</a-button> -->
              </a-space>
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
  import { GetOutsourceGoodsList } from '@/views/Subcode/aProdOutsource/helper/api';
  import { reactive, toRefs, nextTick, ref, unref, computed, toRaw, inject, watch } from 'vue';
  import { BasicModal, useModal } from '@/components/Modal';
  import SelectModal from '@/components/CommonModal/src/SelectModal.vue';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useI18n } from '@/hooks/web/useI18n';
  import { useUserStore } from '@/store/modules/user';
  import type { FormInstance } from 'ant-design-vue';
  import { thousandsFormat } from '@/utils/jnpf';
  import { getTimeUnit, getDateTimeUnit } from '@/utils/jnpf';
  import { getDictionaryDataSelector } from '@/api/systemData/dictionary';
  import { getDataInterfaceRes, getDataInterfaceDataInfoByIds } from '@/api/systemData/dataInterface';
  import dayjs from 'dayjs';
  import { cloneDeep } from 'lodash-es';
  import { buildUUID } from '@/utils/uuid';
  import { CaretRightOutlined } from '@ant-design/icons-vue';
  import { systemComponentsList } from '@/components/FormGenerator/src/helper/config';
  import { usePermission } from '@/hooks/web/usePermission';

  interface State {
    listPageSize: number;
    listCurrentPage: number;
    WorkOrderId: string;
    dataForm: any;
    dataRule: any;
    optionsObj: any;
    currVmodel: any;
    goodsCostList: any[];
    isEdit: boolean;
    currTableConf: any;
    addTableConf: any;
    tableRows: any;
    tableFieldbb1084outerActiveKey: number[];
    tableFieldbb1084innerActiveKey: string[];
    tableFieldbb1084DefaultExpandAll: boolean;
    selectedtableFieldbb1084RowKeys: any[];
    title: string;
    continueText: string;
    allList: any[];
    currIndex: number;
    isContinue: boolean;
    submitType: number;
    showContinueBtn: boolean;
    tempGoodsSelector: any[];
    goodsInterfaceId: string;
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
  const tableFieldbb1084Columns: any[] = computed(() => {
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
        title: '库存编码',
        dataIndex: 'F_Encoding',
        key: 'F_Encoding',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Encoding',
        isSystemControl: false,
      },
      {
        title: '退料数量',
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
        title: '成本单价(元)',
        dataIndex: 'F_Price',
        key: 'F_Price',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '100px',
        fixed: false,
        formP: 'F_Price',
        isSystemControl: false,
      },
      {
        title: '成本总价(元)',
        dataIndex: 'F_TotalPrice',
        key: 'F_TotalPrice',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_TotalPrice',
        isSystemControl: true,
      },
      {
        title: '入库仓库',
        dataIndex: 'F_WarehouseID',
        key: 'F_WarehouseID',
        tipLabel: '',
        required: true,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '200px',
        fixed: false,
        formP: 'F_WarehouseID',
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
    list = list.filter(o => hasFormP('tableFieldbb1084-' + o.formP));
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
  const gettableFieldbb1084RowSelection = computed(() => {
    const rowSelection = {
      selectedRowKeys: state.selectedtableFieldbb1084RowKeys,
      onChange: (selectedRowKeys: string[]) => {
        state.selectedtableFieldbb1084RowKeys = (selectedRowKeys || []).sort().reverse();
      },
    };
    return rowSelection;
  });
  const state = reactive<State>({
    listPageSize: 20,
    listCurrentPage: 1,
    WorkOrderId: '',
    dataForm: {
      id: '',
      F_ReturnInNo: undefined,
      F_WarehouseId: [],
      F_ReturnInType: undefined,
      F_WorkOrderId: undefined,
      F_ReturnInDate: dayjs(new Date()).startOf(getDateTimeUnit('yyyy-MM-dd')).valueOf(),
      F_Type: '0',
      F_Method: undefined,
      F_Description: undefined,
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
      tableFieldbb1084: [],
    },
    tableFieldbb1084outerActiveKey: [0],
    tableFieldbb1084innerActiveKey: [],
    tableFieldbb1084DefaultExpandAll: true,
    selectedtableFieldbb1084RowKeys: [],
    goodsCostList: [],
    isEdit: false,
    tempGoodsSelector: [],
    goodsInterfaceId: '2036358647373762560', // 默认加工单接口
    dataRule: {
      // F_WarehouseId: [
      //   {
      //     required: true,
      //     message: '请输入仓库',
      //     trigger: 'change',
      //   },
      // ],
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
      F_ReturnInDate: [
        {
          required: true,
          message: '请输入退料日期',
          trigger: 'change',
        },
      ],
    },
    optionsObj: {
      F_TypeProps: { label: 'fullName', value: 'enCode' },
      F_WarehouseIdProps: {
        label: 'F_Name',
        value: 'F_Id',
        children: 'children',
      },
      F_ReturnInTypeProps: { label: 'fullName', value: 'enCode' },
      F_WorkOrderIdProps: { label: 'F_ProcessNo', value: 'F_Id' },
      tableFieldbb1084_F_CustomerIdOptions: [
        {
          value: 'F_Code',
          label: '库存编码',
        },
        {
          value: 'F_GoodsName',
          label: '商品名称',
        },
        {
          value: 'F_GoodsNo',
          label: '商品编码',
        },
        {
          value: 'F_Num',
          label: '库存数量',
        },

        {
          value: 'F_WarehouseId',
          label: '商品仓库',
        },
      ],
      tableFieldbb1084_F_CustomerIdTemplateJson: [
        {
          defaultValue: '',
          field: 'F_ProcessingId',
          dataType: 'varchar',
          required: 0,
          fieldName: '加工单',
          relationField: 'F_WorkOrderId',
          jnpfKey: null,
          complexHeaderList: null,
          sourceType: 1,
          isChildren: false,
          IsMultiple: false,
        },
        {
          defaultValue: '',
          field: 'F_WarehouseId',
          dataType: 'varchar',
          required: 0,
          fieldName: '仓库',
          relationField: '',
          jnpfKey: null,
          complexHeaderList: null,
          sourceType: 2,
          isChildren: false,
          IsMultiple: false,
        },
      ],
      tableFieldbb1084_F_WarehouseIDOptions: [],
      tableFieldbb1084_F_WarehouseIDProps: {
        label: 'F_Name',
        value: 'F_Id',
        children: 'children',
      },
    },
    currVmodel: '',
    currTableConf: {},
    tableRows: {
      tableFieldbb1084: {
        enabledmark: undefined,
        F_CustomerId: undefined,
        F_Encoding: undefined,
        _F_Encoding_Suffix: undefined,
        _F_CustomerObj: undefined,
        F_Num: undefined,
        F_WarehouseID: [],
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
  const { title, continueText, showContinueBtn, dataForm, submitType, dataRule, optionsObj, isEdit } = toRefs(state);

  const getPrevDisabled = computed(() => state.currIndex === 0);
  const getNextDisabled = computed(() => state.currIndex === state.allList.length - 1);

  // 监听
  watch(
    () => state.dataForm.tableFieldbb1084,
    newVal => {
      if (!Array.isArray(newVal)) return;
      for (let i = 0; i < newVal.length; i++) {
        const row = newVal[i] || {};
        const levels = getUnitRatioLevels(row);
        row.F_Num = row.F_Num;
        row.F_NumLevel1 = 1;
      }
    },
    { deep: true },
  );

  // 监听仓库变化，将仓库数组转为 JSON 字符串传递给接口参数
  watch(
    () => state.dataForm.F_WarehouseId,
    val => {
      const warehouseIdArr = Array.isArray(val) ? val : val ? [val] : [];
      const target = state.optionsObj.tableFieldbb1084_F_CustomerIdTemplateJson.find((t: any) => t.field === 'F_WarehouseId');
      if (target) {
        target.relationField = warehouseIdArr.length ? JSON.stringify(warehouseIdArr) : '';
      }
    },
    { immediate: true },
  );

  defineExpose({ init });

  function init(data) {
    state.submitType = 0;
    state.showContinueBtn = true;
    state.title = !data.id ? '新增' : '编辑';
    state.tempGoodsSelector = [];
    setFormProps({ continueLoading: false });
    state.dataForm.id = data.id;
    state.dataForm.F_Method = data.F_Method;
    if (data.F_ProcessId) {
      state.dataForm.F_Type = data.F_Type;
      state.WorkOrderId = data.F_ProcessId;
      state.title = data.fullName;
    } else {
      state.dataForm.F_Type = '0';
      state.WorkOrderId = '';
    }
    // 标记是否为编辑模式，编辑时禁用部分字段
    state.isEdit = !!data.id;
    openModal();
    nextTick(() => {
      getForm().resetFields();
      state.dataForm.tableFieldbb1084 = [];
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
        F_ReturnInNo: undefined,
        F_WarehouseId: [],
        F_ReturnInType: undefined,
        F_WorkOrderId: state.WorkOrderId,
        F_ReturnInDate: dayjs(new Date()).startOf(getDateTimeUnit('yyyy-MM-dd')).valueOf(),
        F_Type: '0',
        F_Method: state.dataForm.F_Method,
        F_Description: undefined,
        F_CreatorUserId: undefined,
        F_CreatorTime: undefined,
        tableFieldbb1084: [],
      };
      getF_WarehouseIdOptions();
      getF_ReturnInTypeOptions();
      getF_WorkOrderIdOptions();
      gettableFieldbb1084_F_WarehouseIDOptions();
      getF_TypeOptions();
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
      for (let i = 0; i < state.dataForm.tableFieldbb1084.length; i++) {
        const element = state.dataForm.tableFieldbb1084[i];
        element.jnpfId = buildUUID();
        // 编辑模式下转换仓库数据格式（后端返回的可能是 JSON 字符串或 List）
        element.F_WarehouseID = parseWarehouseIdList(element.F_WarehouseID);
      }
      // 编辑模式下获取商品详情（含 F_Unit_Ratio）用于数量层级显示
      await enrichTableRowsGoodsMeta(state.dataForm.tableFieldbb1084);
      // 根据 F_Num 填充数量层级
      for (const row of state.dataForm.tableFieldbb1084) {
        fillNumLevelsFromF_Num(row);
      }
      if (state.dataForm.F_Type == '0') {
        getF_WorkOrderIdOptions();
      } else if (state.dataForm.F_Type == '1') {
        getF_OutsourceIdOptions();
      }
      getF_WarehouseIdOptions();
      getF_ReturnInTypeOptions();
      gettableFieldbb1084_F_WarehouseIDOptions();
      getF_TypeOptions();
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
  function gettableFieldbb1084_F_WarehouseIDOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2021045201115680768', query).then(res => {
      let data = res.data;
      state.optionsObj.tableFieldbb1084_F_WarehouseIDOptions = Array.isArray(data) ? data : [];
    });
  }
  function getF_ReturnInTypeOptions() {
    getDictionaryDataSelector('2012074650393251840').then(res => {
      state.optionsObj.F_ReturnInTypeOptions = res.data.list;
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
  function removetableFieldbb1084Row(index, showConfirm = false) {
    if (showConfirm) {
      createConfirm({
        iconType: 'warning',
        title: '提示',
        content: '此操作将永久删除该数据, 是否继续?',
        onOk: () => {
          state.dataForm.tableFieldbb1084.splice(index, 1);
        },
      });
    } else {
      state.dataForm.tableFieldbb1084.splice(index, 1);
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

  function WorkOrderChange(_val, _option) {
    // 清空商品列表和选择器
    state.dataForm.tableFieldbb1084 = [];
    state.tableFieldbb1084innerActiveKey = [];
    state.tempGoodsSelector = [];

    if (!state.dataForm.F_WorkOrderId) {
      return;
    }

    // 根据工单类型更新商品接口ID
    if (state.dataForm.F_Type == '0') {
      state.goodsInterfaceId = '2036358647373762560'; // 加工单接口
    } else if (state.dataForm.F_Type == '1') {
      state.goodsInterfaceId = '2036631044618326016'; // 外协工单接口
    }
  }

  /** InfoByIds 等接口返回可能是 { data: [] } 或已解包数组、或 { data: { list: [] } } */
  function parseInfoByIdsResponse(resp: any): any[] {
    if (resp == null) return [];
    const inner = resp.data !== undefined ? resp.data : resp;
    if (Array.isArray(inner)) return inner;
    if (inner && typeof inner === 'object' && Array.isArray(inner.list)) return inner.list;
    return [];
  }

  /** 将接口里的 F_WarehouseIdList 转为级联 value（路径 id 数组）；支持数组或 JSON 字符串 */
  function parseWarehouseIdList(raw: any): string[] {
    if (raw == null || raw === '') return [];
    if (Array.isArray(raw)) return raw.map((x: any) => String(x));
    if (typeof raw === 'string') {
      const s = raw.trim();
      if (!s) return [];
      try {
        const parsed = JSON.parse(s);
        if (Array.isArray(parsed)) return parsed.map((x: any) => String(x));
      } catch {
        // ignore
      }
    }
    return [];
  }

  /** 解析选中行的商品ID */
  function resolveSubTableGoodsId(data: any): string | null {
    if (!data) return null;
    return data.F_Id ?? data.F_GoodsId ?? data.F_Code ?? data.id ?? null;
  }

  /** 标准化选择器返回的完整行数据 */
  function normalizePickerFullRows(selected: any, pickerRows?: any): any[] | null {
    if (pickerRows && Array.isArray(pickerRows) && pickerRows.length) return pickerRows;
    if (selected && Array.isArray(selected) && selected.length && typeof selected[0] === 'object') return selected;
    return null;
  }

  /** 判断数组元素是否已经是完整行对象 */
  function pickerItemsAreFullRows(arr: any[]) {
    if (!arr || !arr.length) return false;
    const first = arr[0];
    return first && typeof first === 'object' && (first.F_Id || first.F_GoodsId || first.F_GoodsName || first.F_Code);
  }

  function handleGoodsSelectorChange(selected: any, pickerRows?: any) {
    state.dataForm.tableFieldbb1084 = [];
    state.selectedtableFieldbb1084RowKeys = [];

    const selectionEmpty = selected == null || selected === '' || (Array.isArray(selected) && selected.length === 0);
    if (selectionEmpty) {
      state.tempGoodsSelector = [];
      return;
    }

    const buildAndPushRow = (data: any) => {
      let relationId: any = data;
      let fullObj: any = null;
      if (data && typeof data === 'object') {
        fullObj = data;
        relationId = resolveSubTableGoodsId(data) ?? relationId;
      }
      const codeVal = fullObj?.F_Code ?? fullObj?.f_Code ?? undefined;

      // 从 F_Unit_Ratio 中提取 F_Num 的默认值
      let defaultNum: number | undefined = undefined;
      const unitRatioRaw = fullObj?.F_Unit_Ratio;
      if (unitRatioRaw) {
        try {
          const arr = typeof unitRatioRaw === 'string' ? JSON.parse(unitRatioRaw) : unitRatioRaw;
          if (Array.isArray(arr) && arr.length > 0) {
            // 如果数组只有一个元素，取第一个的 qty；如果超过一个，取第二个的 qty
            const idx = arr.length === 1 ? 0 : 1;
            if (arr[idx] && arr[idx].qty != null) {
              defaultNum = Number(arr[idx].qty);
            }
          }
        } catch (e) {
          // 解析失败，忽略
        }
      }

      const newRow: any = {
        jnpfId: buildUUID(),
        F_CustomerId: relationId,
        _F_CustomerObj: fullObj,
        F_InventoryNum: fullObj?.F_Num != null && fullObj?.F_Num !== '' ? fullObj.F_Num : undefined,
        F_Specification: fullObj?.F_Specification ?? undefined,
        F_Encoding: codeVal,
        F_WarehouseID: parseWarehouseIdList(fullObj?.F_WarehouseIdList ?? fullObj?.f_WarehouseIdList),
        F_Unit_Ratio: fullObj?.F_Unit_Ratio ?? undefined,
        F_Num: defaultNum,
        F_Price: undefined,
        F_Description: undefined,
        F_CreatorUserId: undefined,
        F_CreatorTime: undefined,
      };
      // 根据单位换算设置数量层级
      fillNumLevelsFromF_Num(newRow);
      state.dataForm.tableFieldbb1084.push(newRow);
      state.tableFieldbb1084innerActiveKey.push(newRow.jnpfId);
    };

    const fullRowsFromPicker = normalizePickerFullRows(selected, pickerRows);
    if (fullRowsFromPicker?.length) {
      fullRowsFromPicker.forEach((row: any) => buildAndPushRow(row));
      state.tempGoodsSelector = [];
      return;
    }

    if (Array.isArray(selected)) {
      if (pickerItemsAreFullRows(selected)) {
        selected.forEach((row: any) => buildAndPushRow(row));
        state.tempGoodsSelector = [];
        return;
      }
      const codes = Array.from(new Set(selected.map((s: any) => String(s))));
      const query = {
        ids: codes,
        interfaceId: state.goodsInterfaceId,
        propsValue: 'F_Code',
        relationField: 'F_GoodsName',
        paramList: [],
      };
      getDataInterfaceDataInfoByIds(state.goodsInterfaceId, query)
        .then(resp => {
          const list = parseInfoByIdsResponse(resp);
          const map: Record<string, any> = {};
          list.forEach((item: any) => {
            if (item && item.F_Code != null) map[String(item.F_Code)] = item;
          });
          for (let i = 0; i < codes.length; i++) {
            const code = codes[i];
            buildAndPushRow(map[String(code)] || code);
          }
          state.tempGoodsSelector = [];
        })
        .catch(err => {
          console.error('getDataInterfaceDataInfoByIds error', err);
          codes.forEach((c: any) => buildAndPushRow(c));
          state.tempGoodsSelector = [];
        });
      return;
    }

    const single = selected;
    if (single && typeof single === 'object') {
      buildAndPushRow(single);
      state.tempGoodsSelector = [];
      return;
    }
    const code = String(single);
    const querySingle = {
      ids: [code],
      interfaceId: state.goodsInterfaceId,
      propsValue: 'F_Code',
      relationField: 'F_GoodsName',
      paramList: [],
    };
    getDataInterfaceDataInfoByIds(state.goodsInterfaceId, querySingle)
      .then(resp => {
        const list = parseInfoByIdsResponse(resp);
        buildAndPushRow(list[0] || code);
        state.tempGoodsSelector = [];
      })
      .catch(() => {
        buildAndPushRow(code);
        state.tempGoodsSelector = [];
      });
  }

  /** 从 F_Unit_Ratio JSON 字符串解析第二级单位 name（数组下标 1，如 箱/盒/瓶 → 「盒」） */
  function parseSecondUnitNameFromUnitRatio(fUnitRatio: any): string {
    if (fUnitRatio == null || fUnitRatio === '') return '';
    try {
      const str = typeof fUnitRatio === 'string' ? fUnitRatio : String(fUnitRatio);
      const arr = JSON.parse(str);
      if (Array.isArray(arr) && arr.length > 1 && arr[1]?.name != null) {
        return String(arr[1].name);
      }
    } catch (_e) {
      /* ignore */
    }
    return '';
  }

  /** 退料数量输入框后缀：优先行上缓存，其次商品详情 */
  function getRecordNumUnitSuffix(record: any): string {
    if (record?._F_Num_UnitSuffix) return record._F_Num_UnitSuffix;
    return parseSecondUnitNameFromUnitRatio(record?._F_CustomerObj?.F_Unit_Ratio);
  }

  // 解析 F_Unit_Ratio（与 aPuStockFg 一致）：两级单位 { level1: { name, qty }, level2: { name, qty } }
  function getUnitRatioLevels(record) {
    const raw = record?.F_Unit_Ratio || record?._F_CustomerObj?.F_Unit_Ratio;
    if (!raw) return null;
    try {
      const str = typeof raw === 'string' ? raw : String(raw);
      const arr = JSON.parse(str);
      if (!Array.isArray(arr)) return null;
      if (arr.length < 2) return { level1: arr[0], level2: null };
      return { level1: arr[0], level2: arr[1] };
    } catch (e) {
      return null;
    }
  }

  function handleNumLevel1Change(record) {
    const levels = getUnitRatioLevels(record);
    if (!levels) return;
    const qty1 = Number(record.F_NumLevel1) || 0;
    const ratio = Number(levels.level2?.qty) || 0;
    record.F_NumLevel2 = qty1 * ratio;
    record._F_NumLevel2Max = record.F_NumLevel2;
    record.F_Num = record.F_NumLevel2;
  }

  /** 二级数量（最小单位）编辑：不超过该行默认/换算上限，并与 F_Num 同步 */
  function handleNumLevel2Change(record) {
    const levels = getUnitRatioLevels(record);
    if (!levels) return;
    let v = Number(record.F_NumLevel2);
    if (Number.isNaN(v)) v = 0;
    const max = Number(record._F_NumLevel2Max);
    if (record._F_NumLevel2Max != null && !Number.isNaN(max) && v > max) {
      v = max;
      record.F_NumLevel2 = v;
    }
    record.F_Num = v;
  }

  /** 回显：F_Num 存二级数量（最小单位），据此拆成一级 + 二级展示 */
  function fillNumLevelsFromF_Num(row) {
    const levels = getUnitRatioLevels(row);
    if (!levels || row.F_Num == null || row.F_Num === '') return;
    const qty2 = Number(row.F_Num) || 0;
    const ratio = Number(levels.level2?.qty) || 0;
    row.F_NumLevel2 = qty2;
    row._F_NumLevel2Max = qty2;
    if (ratio > 0) {
      row.F_NumLevel1 = qty2 / ratio;
    } else {
      row.F_NumLevel1 = qty2;
    }
  }

  /** 按商品 id 拉取详情（含 F_Unit_Ratio），用于编辑模式下的数量层级显示 */
  async function enrichTableRowsGoodsMeta(rows: any[]) {
    if (!rows?.length) return;
    const ids = [
      ...new Set(
        rows
          .map(r => r.F_CustomerId)
          .filter(id => id != null && id !== '')
          .map((id: any) => String(id)),
      ),
    ];
    if (!ids.length) return;
    const interfaceId = '2008721559174385664';
    const query = {
      ids,
      interfaceId,
      propsValue: 'F_Id',
      relationField: 'F_GoodsName',
      paramList: [],
    };
    try {
      const resp = await getDataInterfaceDataInfoByIds(interfaceId, query);
      const list = (resp && resp.data) || [];
      const map: Record<string, any> = {};
      list.forEach((item: any) => {
        if (item && item.F_Id != null) map[String(item.F_Id)] = item;
      });
      rows.forEach((row: any) => {
        const id = row.F_CustomerId != null ? String(row.F_CustomerId) : '';
        const obj = id && map[id];
        if (obj) {
          row._F_CustomerObj = obj;
          row.F_Unit_Ratio = obj.F_Unit_Ratio ?? row.F_Unit_Ratio;
        }
      });
    } catch (e) {
      console.error('enrichTableRowsGoodsMeta', e);
    }
  }

  //工单类型
  function ProcessBtn(val, option) {
    if (val == '0') {
      getF_WorkOrderIdOptions();
      state.goodsInterfaceId = '2036358647373762560'; // 加工单接口
    } else if (val == '1') {
      getF_OutsourceIdOptions();
      state.goodsInterfaceId = '2036631044618326016'; // 外协工单接口
    }
    state.dataForm.F_WorkOrderId = undefined;
    state.dataForm.tableFieldbb1084 = [];
    state.tableFieldbb1084innerActiveKey = [];
    state.tempGoodsSelector = [];
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
