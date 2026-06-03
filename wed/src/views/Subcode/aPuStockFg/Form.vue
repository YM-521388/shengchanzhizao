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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('CPRK_StockInNo')">
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('CPRK_StockInDate')">
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
          <!-- <a-col :span="8" class="ant-col-item" v-if="hasFormP('CPRK_WarehouseId')">
            <a-form-item name="F_WarehouseId" :labelCol="{ style: { width: '100px' } }">
              <template #label>入库仓库</template>
              <JnpfCascader
                v-model:value="dataForm.F_WarehouseId"
                placeholder="请选择"
                :options="optionsObj.F_WarehouseIdOptions"
                :fieldNames="optionsObj.F_WarehouseIdProps"
                allowClear
                 :disabled="!!state.dataForm.id"
                showSearch
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col> -->
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('CPRK_Type')">
            <a-form-item name="F_Type" :labelCol="{ style: { width: '100px' } }">
              <template #label>工单类型</template>
              <JnpfSelect
                v-model:value="dataForm.F_Type"
                placeholder="请选择"
                :options="optionsObj.F_TypeOptions"
                :fieldNames="optionsObj.F_TypeProps"
                :disabled="state.dataForm.id || state.WorkOrderId != ''"
                @change="ProcessBtn"
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('CPRK_StockInType')">
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
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('CPRK_Description')">
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
          <a-col :span="12" class="ant-col-item" v-if="dataForm.F_Type == '1'">
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
          <a-col :span="12" class="ant-col-item" v-if="dataForm.F_Type == '0'">
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
          <a-col :span="12" class="ant-col-item">
            <a-form-item :labelCol="{ style: { width: '100px' } }">
              <template #label>批量入库仓库</template>
              <a-space>
                <JnpfCascader
                  v-model:value="batchWarehouseValue"
                  placeholder="请选择仓库"
                  :options="optionsObj.tableFieldc43f9b_F_WarehouseIDOptions"
                  :fieldNames="optionsObj.tableFieldc43f9b_F_WarehouseIDProps"
                  allowClear
                  showSearch
                  :disabled="state.dataForm.id"
                  changeOnSelect
                  :style="{
                    width: '300px',
                  }" />
                <a-button type="primary" size="small" @click="handleBatchAssignWarehouse" :disabled="!state.selectedtableFieldc43f9bRowKeys.length">
                  批量赋值
                </a-button>
                <span class="text-gray-400 text-xs" v-if="state.selectedtableFieldc43f9bRowKeys.length">
                  (已选 {{ state.selectedtableFieldc43f9bRowKeys.length }} 条)
                </span>
              </a-space>
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="选择入库商品" :bordered="false" />
              <a-table
                :data-source="dataForm.tableFieldc43f9b"
                :columns="tableFieldc43f9bColumns"
                size="small"
                rowKey="jnpfId"
                :pagination="listPagination"
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
                  <template v-if="column.key === 'F_Encoding'">
                    <div class="stock-encoding-group" :class="{ 'stock-encoding-group--prefixed': !!getGoodsNoPrefix(record) }">
                      <span v-if="getGoodsNoPrefix(record)" class="stock-encoding-prefix">{{ getGoodsNoPrefix(record) }}</span>
                      <div class="stock-encoding-input-wrap">
                        <JnpfInput
                          v-model:value="record._F_Encoding_Suffix"
                          placeholder="请输入"
                          allowClear
                          :disabled="!!state.dataForm.id"
                          :style="{
                            width: '80px',
                          }"
                          :showCount="false"
                          @change="handleEncodingChange(record)" />
                      </div>
                    </div>
                  </template>
                  <!-- <template v-if="column.key === 'F_Specification'">
                    <JnpfInput
                      v-model:value="record.F_Specification"
                      placeholder=""
                      :controls="false"
                      disabled
                      :style="{
                        width: '100%',
                      }" />
                  </template> -->
                  <!-- <template v-if="column.key === 'F_InventoryNum'">
                    <JnpfInput
                      v-model:value="record.F_InventoryNum"
                      placeholder=""
                      :controls="false"
                      disabled
                      :style="{
                        width: '100%',
                      }" />
                  </template> -->
                  <!-- <template v-if="column.key === 'F_PlanQty'">
                    <JnpfInput
                      v-model:value="record.F_PlanQty"
                      placeholder=""
                      :controls="false"
                      disabled
                      :style="{
                        width: '100%',
                      }" />
                  </template> -->
                  <!-- <template v-if="column.key === 'F_YseNum'">
                    <JnpfInput
                      v-model:value="record.F_YseNum"
                      placeholder=""
                      :controls="false"
                      disabled
                      :style="{
                        width: '100%',
                      }" />
                  </template> -->
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
                  <template v-if="column.key === 'F_WarehouseID'">
                    <JnpfCascader
                      v-model:value="record.F_WarehouseID"
                      placeholder="请选择"
                      :options="optionsObj.tableFieldc43f9b_F_WarehouseIDOptions"
                      :fieldNames="optionsObj.tableFieldc43f9b_F_WarehouseIDProps"
                      :changeOnSelect="true"
                      allowClear
                      :disabled="state.dataForm.id"
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
  import { getProcessingIdList } from '@/views/Subcode/aProdProcessing/helper/api';
  import { getOutsourceIdList } from '@/views/Subcode/aProdOutsource/helper/api';
  import { getMaxCode } from '@/views/Subcode/aGoodsInventoryQr/helper/api';
  import { reactive, toRefs, nextTick, ref, unref, computed, watch, toRaw, inject } from 'vue';
  import { BasicModal, useModal } from '@/components/Modal';
  import SelectModal from '@/components/CommonModal/src/SelectModal.vue';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useI18n } from '@/hooks/web/useI18n';
  import { useUserStore } from '@/store/modules/user';
  import type { FormInstance } from 'ant-design-vue';
  import { getDictionaryDataSelector } from '@/api/systemData/dictionary';
  import { getDataInterfaceRes, getDataInterfaceDataInfoByIds } from '@/api/systemData/dataInterface';
  import dayjs from 'dayjs';
  import { cloneDeep } from 'lodash-es';
  import { buildUUID } from '@/utils/uuid';
  import { usePermission } from '@/hooks/web/usePermission';

  interface State {
    WorkOrderId: string;
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
    listPageSize: number;
    listCurrentPage: number;
    batchWarehouseValue: any;
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
        title: '库存编码',
        dataIndex: 'F_Encoding',
        key: 'F_Encoding',
        tipLabel: '',
        required: true,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Encoding',
        isSystemControl: false,
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
      // {
      //   title: '商品规格',
      //   dataIndex: 'F_Specification',
      //   key: 'F_Specification',
      //   tipLabel: '',
      //   required: false,
      //   align: 'left',
      //   span: '24',
      //   labelWidth: '',
      //   width: '150px',
      //   fixed: false,
      //   formP: 'F_Specification',
      //   isSystemControl: false,
      // },
      // {
      //   title: '库存数量',
      //   dataIndex: 'F_InventoryNum',
      //   key: 'F_InventoryNum',
      //   tipLabel: '',
      //   required: false,
      //   align: 'left',
      //   span: '24',
      //   labelWidth: '',
      //   width: '100px',
      //   fixed: false,
      //   formP: 'F_InventoryNum',
      //   isSystemControl: false,
      // },
      // {
      //   title: '计划数量',
      //   dataIndex: 'F_PlanQty',
      //   key: 'F_PlanQty',
      //   tipLabel: '',
      //   required: false,
      //   align: 'left',
      //   span: '24',
      //   labelWidth: '',
      //   width: '100px',
      //   fixed: false,
      //   formP: 'F_PlanQty',
      //   isSystemControl: false,
      // },
      // {
      //   title: '已入库数量',
      //   dataIndex: 'F_YseNum',
      //   key: 'F_YseNum',
      //   tipLabel: '',
      //   required: false,
      //   align: 'left',
      //   span: '24',
      //   labelWidth: '',
      //   width: '100px',
      //   fixed: false,
      //   formP: 'F_YseNum',
      //   isSystemControl: false,
      // },
      {
        title: '入库数量',
        dataIndex: 'F_Num',
        key: 'F_Num',
        tipLabel: '',
        required: true,
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
        width: '100px',
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
    list = list.filter(o => hasFormP('tableFieldc43f9b-' + o.formP));
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
    listPageSize: 20,
    listCurrentPage: 1,
    WorkOrderId: '',
    ProcessingList: [],
    OutsourceList: [],
    dataForm: {
      id: '',
      F_StockInNo: undefined,
      F_StockInDate: undefined,
      F_WarehouseId: [],
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
    batchWarehouseValue: undefined,
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
      F_WarehouseIdProps: {
        label: 'F_Name',
        value: 'F_Id',
        children: 'children',
      },
      F_TypeProps: { label: 'fullName', value: 'enCode' },
      F_StockInTypeProps: { label: 'fullName', value: 'enCode' },
      F_MethodProps: { label: 'fullName', value: 'enCode' },
      tableFieldc43f9b_F_WorkOrderIdOptions: [],
      tableFieldc43f9b_F_WorkOrderIdProps: { label: 'F_ProcessNo', value: 'id' },
      tableFieldc43f9b_F_CustomerIdOptions: [],
      tableFieldc43f9b_F_CustomerIdProps: { label: 'F_GoodsName', value: 'id' },
      tableFieldc43f9b_F_WarehouseIDOptions: [],
      tableFieldc43f9b_F_WarehouseIDProps: {
        label: 'F_Name',
        value: 'F_Id',
        children: 'children',
      },
    },
    currVmodel: '',
    currTableConf: {},
    tableRows: {
      tableFieldc43f9b: {
        enabledmark: undefined,
        F_WorkOrderId: undefined,
        F_Encoding: undefined,
        _F_Encoding_Suffix: undefined,
        F_CategoryId: undefined,
        F_GoodsNo: undefined,
        F_CustomerId: undefined,
        F_NumLevel1: undefined,
        F_NumLevel2: undefined,
        _F_NumLevel2Max: undefined,
        F_Num: undefined,
        F_Price: undefined,
        F_CostPrice: undefined,
        F_Unit_Ratio: undefined,
        F_WarehouseID: [],
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
  const { title, continueText, showContinueBtn, dataForm, submitType, dataRule, optionsObj, batchWarehouseValue } = toRefs(state);

  defineExpose({ init });

  function init(data) {
    state.submitType = 0;
    state.showContinueBtn = true;
    state.title = !data.id ? '新增' : '编辑';
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
        F_WarehouseId: [],
        F_Type: state.dataForm.F_Type,
        F_StockInType: undefined,
        F_Method: undefined,
        F_Description: undefined,
        F_CreatorUserId: undefined,
        F_CreatorTime: undefined,
        tableFieldc43f9b: [],
      };
      state.ProcessingList = [];
      state.OutsourceList = [];
      if (state.WorkOrderId) {
        if (state.dataForm.F_Type == '0') {
          gettableFieldc43f9b_F_WorkOrderIdOptions();
          state.ProcessingList.push(state.WorkOrderId);
          getProcessingIdList(state.WorkOrderId).then(async res => {
            const data = res.data[0];
            // 如果没有 F_Unit_Ratio，从商品接口获取
            let unitRatio = data.F_Unit_Ratio;
            let costPrice = data.F_CostPrice;
            if (!unitRatio && data.F_GoodsId) {
              try {
                const goodsRes = await getDataInterfaceDataInfoByIds('2008721559174385664', {
                  ids: [data.F_GoodsId],
                  interfaceId: '2008721559174385664',
                  propsValue: 'F_Id',
                  relationField: 'F_GoodsName',
                  paramList: [],
                });
                if (goodsRes.data?.[0]) {
                  const goodsInfo = goodsRes.data[0];
                  unitRatio = goodsInfo.F_Unit_Ratio;
                  costPrice = costPrice || goodsInfo.F_CostPrice;
                  data.F_CategoryId = data.F_CategoryId || goodsInfo.F_CategoryId;
                  data.F_GoodsNo = data.F_GoodsNo || goodsInfo.F_GoodsNo;
                }
              } catch (e) {
                console.error('获取商品信息失败', e);
              }
            }
            // 获取库存编码基础值
            let baseCode = 0;
            if (data.F_GoodsId) {
              try {
                const codeRes = await getMaxCode(data.F_GoodsId);
                baseCode = Number(codeRes?.data) || 0;
              } catch (e) {
                baseCode = 0;
              }
            }
            // 按计划数量循环创建多条记录
            const planQty = parseInt(data.F_PlanQty) || 1;
            for (let i = 0; i < planQty; i++) {
              const currentCode = baseCode + i + 1;
              const newRow: any = {
                jnpfId: buildUUID(),
                F_WorkOrderId: data.id,
                F_CustomerId: data.F_GoodsId,
                F_GoodsName: data.F_GoodsName,
                F_GoodsNo: data.F_GoodsNo,
                F_CategoryId: data.F_CategoryId,
                F_Specification: data.F_Specification,
                F_PlanQty: 1,
                F_InventoryNum: data.F_InventoryNum,
                F_YseNum: data.F_YseNum,
                F_Price: costPrice,
                F_Unit_Ratio: unitRatio,
                F_Description: undefined,
                F_CreatorTime: undefined,
                _F_Encoding_Suffix: String(currentCode),
              };
              // 计算入库数量
              const levels = getUnitRatioLevels(newRow);
              if (levels) {
                if (levels.level2?.qty) {
                  newRow.F_NumLevel1 = 1;
                  const ratio = Number(levels.level2.qty) || 0;
                  newRow.F_NumLevel2 = 1 * ratio;
                  newRow._F_NumLevel2Max = newRow.F_NumLevel2;
                  newRow.F_Num = newRow.F_NumLevel2;
                } else {
                  newRow.F_NumLevel1 = 1;
                  newRow.F_NumLevel2 = 1;
                  newRow._F_NumLevel2Max = newRow.F_NumLevel2;
                  newRow.F_Num = newRow.F_NumLevel2;
                }
              } else {
                newRow.F_Num = 1;
                newRow.F_NumLevel2 = 1;
              }
              state.dataForm.tableFieldc43f9b.push(newRow);
              state.tableFieldc43f9binnerActiveKey.push(newRow.jnpfId);
            }
            changeLoading(false);
          });
        } else if (state.dataForm.F_Type == '1') {
          gettableFieldc43f9b_F_OutsourceIdOptions();
          state.OutsourceList.push(state.WorkOrderId);
          getOutsourceIdList(state.WorkOrderId).then(async res => {
            const data = res.data[0];
            // 如果没有 F_Unit_Ratio，从商品接口获取
            let unitRatio = data.F_Unit_Ratio;
            let costPrice = data.F_CostPrice;
            if (!unitRatio && data.F_GoodsId) {
              try {
                const goodsRes = await getDataInterfaceDataInfoByIds('2008721559174385664', {
                  ids: [data.F_GoodsId],
                  interfaceId: '2008721559174385664',
                  propsValue: 'F_Id',
                  relationField: 'F_GoodsName',
                  paramList: [],
                });
                if (goodsRes.data?.[0]) {
                  const goodsInfo = goodsRes.data[0];
                  unitRatio = goodsInfo.F_Unit_Ratio;
                  costPrice = costPrice || goodsInfo.F_CostPrice;
                  data.F_CategoryId = data.F_CategoryId || goodsInfo.F_CategoryId;
                  data.F_GoodsNo = data.F_GoodsNo || goodsInfo.F_GoodsNo;
                }
              } catch (e) {
                console.error('获取商品信息失败', e);
              }
            }
            // 获取库存编码基础值
            let baseCode = 0;
            if (data.F_GoodsId) {
              try {
                const codeRes = await getMaxCode(data.F_GoodsId);
                baseCode = Number(codeRes?.data) || 0;
              } catch (e) {
                baseCode = 0;
              }
            }
            // 按计划数量循环创建多条记录
            const planQty = parseInt(data.F_PlanNum) || 1;
            for (let i = 0; i < planQty; i++) {
              const currentCode = baseCode + i + 1;
              const newRow: any = {
                jnpfId: buildUUID(),
                F_WorkOrderId: data.id,
                F_CustomerId: data.F_GoodsId,
                F_GoodsName: data.F_GoodsName,
                F_GoodsNo: data.F_GoodsNo,
                F_CategoryId: data.F_CategoryId,
                F_Specification: data.F_Specification,
                F_PlanQty: 1,
                F_InventoryNum: data.F_InventoryNum,
                F_YseNum: data.F_YseNum,
                F_Price: costPrice,
                F_Unit_Ratio: unitRatio,
                F_Description: undefined,
                F_CreatorTime: undefined,
                _F_Encoding_Suffix: String(currentCode),
              };
              // 计算入库数量
              const levels = getUnitRatioLevels(newRow);
              if (levels) {
                if (levels.level2?.qty) {
                  newRow.F_NumLevel1 = 1;
                  const ratio = Number(levels.level2.qty) || 0;
                  newRow.F_NumLevel2 = 1 * ratio;
                  newRow._F_NumLevel2Max = newRow.F_NumLevel2;
                  newRow.F_Num = newRow.F_NumLevel2;
                } else {
                  newRow.F_NumLevel1 = 1;
                  newRow.F_NumLevel2 = 1;
                  newRow._F_NumLevel2Max = newRow.F_NumLevel2;
                  newRow.F_Num = newRow.F_NumLevel2;
                }
              } else {
                newRow.F_Num = 1;
                newRow.F_NumLevel2 = 1;
              }
              state.dataForm.tableFieldc43f9b.push(newRow);
              state.tableFieldc43f9binnerActiveKey.push(newRow.jnpfId);
            }
            changeLoading(false);
          });
        }
      }
      getF_WarehouseIdOptions();
      getF_TypeOptions();
      getF_StockInTypeOptions();
      getF_MethodOptions();
      gettableFieldc43f9b_F_CustomerIdOptions();
      gettableFieldc43f9b_F_WarehouseIDOptions();
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
        // 回显入库数量：F_Num 拆分为一级 + 二级展示
        fillNumLevelsFromF_Num(element);
      }

      // 填充子表商品信息（用于计算库存编码前缀）
      try {
        const rows = state.dataForm.tableFieldc43f9b || [];
        const idsToFetch: any[] = [];
        for (let i = 0; i < rows.length; i++) {
          const r = rows[i];
          if (!r) continue;
          // 如果关联字段已经是对象，直接使用
          if (r.F_CustomerId && typeof r.F_CustomerId === 'object') {
            r._F_CustomerObj = r.F_CustomerId;
            continue;
          }
          if (r.F_CustomerId) idsToFetch.push(r.F_CustomerId);
        }
        // 已能算出前缀的行先拆一次；待 InfoByIds 返回后再拆一次
        refreshEncodingSuffixForTableRows();
        if (idsToFetch.length) {
          const uniqueIds = Array.from(new Set(idsToFetch));
          const interfaceId = '2008721559174385664';
          const query = {
            ids: uniqueIds,
            interfaceId,
            propsValue: 'F_Id',
            relationField: 'F_GoodsName',
            paramList: [],
          };
          getDataInterfaceDataInfoByIds(interfaceId, query)
            .then(resp => {
              const list = resp.data || [];
              const idKey = 'F_Id';
              const map: Record<string, any> = {};
              list.forEach((item: any) => {
                if (item && item[idKey] != null) map[String(item[idKey])] = item;
              });
              for (let i = 0; i < rows.length; i++) {
                const r = rows[i];
                if (!r) continue;
                const found = map[String(r.F_CustomerId)];
                if (found) {
                  r._F_CustomerObj = found;
                  r.F_CategoryId = found.F_CategoryId ?? r.F_CategoryId;
                  r.F_GoodsNo = found.F_GoodsNo ?? found.GoodsNo ?? r.F_GoodsNo;
                }
              }
              // 获取商品信息后重新计算编码前缀和后缀
              refreshEncodingSuffixForTableRows();
            })
            .catch(() => {
              // ignore errors
            });
        }
      } catch (e) {
        // swallow errors
      }

      if (state.dataForm.F_Type == '0') {
        gettableFieldc43f9b_F_WorkOrderIdOptions();
      } else if (state.dataForm.F_Type == '1') {
        gettableFieldc43f9b_F_OutsourceIdOptions();
      }
      getF_WarehouseIdOptions();
      getF_TypeOptions();
      getF_StockInTypeOptions();
      getF_MethodOptions();
      gettableFieldc43f9b_F_CustomerIdOptions();
      gettableFieldc43f9b_F_WarehouseIDOptions();
      changeLoading(false);
    });
  }
  async function handleSubmit() {
    try {
      for (const row of state.dataForm.tableFieldc43f9b || []) {
        if (row._F_Encoding_Suffix !== undefined || getGoodsNoPrefix(row)) {
          handleEncodingChange(row);
        }
      }

      // 验证子表入库仓库：是否全部填写
      const tableFieldc43f9b = state.dataForm.tableFieldc43f9b || [];
      if (tableFieldc43f9b.length > 0) {
        const emptyWarehouseRows = tableFieldc43f9b.filter((row: any) => {
          const warehouseId = row.F_WarehouseID;
          return !warehouseId || (Array.isArray(warehouseId) && warehouseId.length === 0);
        });
        if (emptyWarehouseRows.length > 0) {
          createMessage.warning(`商品列表中有 ${emptyWarehouseRows.length} 条数据的入库仓库未填写`);
          return;
        }
      }

      // 验证子表库存编码：后缀不能为空
      {
        const rows = state.dataForm.tableFieldc43f9b || [];
        for (let i = 0; i < rows.length; i++) {
          const row = rows[i];
          const suffix = row._F_Encoding_Suffix == null ? '' : String(row._F_Encoding_Suffix).trim();
          if (!suffix) {
            createMessage.warning(`商品列表第 ${i + 1} 行的库存编码后缀未填写，请填写后重试`);
            return;
          }
        }
      }

      // 验证子表库存编码：同前缀下，后缀不能重复
      {
        const rows = state.dataForm.tableFieldc43f9b || [];
        const fullCodes = rows
          .map(row => {
            const prefix = getGoodsNoPrefix(row);
            const suffix = row._F_Encoding_Suffix || '';
            return prefix + suffix;
          })
          .filter(code => code);
        const seen = new Set<string>();
        for (const code of fullCodes) {
          if (seen.has(code)) {
            createMessage.warning(`库存编码「${code}」存在重复，请修改后重试`);
            return;
          }
          seen.add(code);
        }
      }

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

  watch(
    () => state.dataForm.tableFieldc43f9b,
    newVal => {
      if (!Array.isArray(newVal)) return;
      for (let i = 0; i < newVal.length; i++) {
        const row = newVal[i] || {};
        const levels = getUnitRatioLevels(row);
        if (levels) {
          if (levels.level2?.qty) {
            // 每条最大单位数量固定为 1，二级数量自动换算
            row.F_NumLevel1 = 1;
            row.F_NumLevel2 = row.F_Num;
            // row._F_NumLevel2Max = row.F_NumLevel2;
            row.F_Num = row.F_Num;
          } else {
            // 每条最大单位数量固定为 1，二级数量自动换算
            row.F_NumLevel1 = 1;
            row.F_NumLevel2 = 1;
            // row._F_NumLevel2Max = row.F_NumLevel2;
            row.F_Num = row.F_NumLevel2;
          }
        } else {
          row.F_Num = row.F_Num;
          row.F_NumLevel1 = 1;
          row.F_NumLevel2 = 1;
        }
      }
    },
    { deep: true },
  );

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
    getDataInterfaceRes('2014969808717746176', query).then(res => {
      let data = res.data;
      state.optionsObj.tableFieldc43f9b_F_WorkOrderIdOptions = Array.isArray(data) ? data : [];
    });
  }
  function gettableFieldc43f9b_F_WarehouseIDOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2021045201115680768', query).then(res => {
      let data = res.data;
      state.optionsObj.tableFieldc43f9b_F_WarehouseIDOptions = Array.isArray(data) ? data : [];
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

  async function ProcessingBtn(val, option) {
    /* 兜底 */
    if (!Array.isArray(state.dataForm.tableFieldc43f9b)) {
      state.dataForm.tableFieldc43f9b = [];
    }
    const optionIdSet = new Set(option.map(o => o.id));

    /*  删掉已取消勾选的行 */
    state.dataForm.tableFieldc43f9b = state.dataForm.tableFieldc43f9b.filter(item => optionIdSet.has(item.F_WorkOrderId));

    // 收集新增的数据，按 F_GoodsNo 分组
    const newItems: any[] = [];
    option.forEach(o => {
      const exist = state.dataForm.tableFieldc43f9b.some(item => item.F_WorkOrderId === o.id);
      if (!exist) {
        const planQty = parseInt(o.F_PlanQty) || 1;
        // 根据计划数量创建多条记录
        for (let i = 0; i < planQty; i++) {
          newItems.push({
            ...o,
            _index: i, // 记录在同一 F_GoodsNo 下的顺序
          });
        }
      }
    });

    // 按 F_GoodsNo 分组
    const goodsNoMap = new Map<string, any[]>();
    newItems.forEach(item => {
      const goodsNo = item.F_GoodsNo;
      if (!goodsNoMap.has(goodsNo)) {
        goodsNoMap.set(goodsNo, []);
      }
      goodsNoMap.get(goodsNo)!.push(item);
    });

    // 对每个 F_GoodsNo 调用一次 getMaxCode
    const goodsNoCodeMap = new Map<string, number>();
    const promises: Promise<void>[] = [];

    for (const [goodsNo, items] of goodsNoMap) {
      // 获取该组的 F_GoodsId（所有 item 的 F_GoodsId 相同）
      const goodsId = items[0]?.F_GoodsId;
      if (goodsId) {
        const promise = getMaxCode(goodsId).then(res => {
          const baseCode = res?.data || 0;
          goodsNoCodeMap.set(goodsNo, Number(baseCode));
        });
        promises.push(promise);
      }
    }

    // 等待所有 getMaxCode 调用完成
    await Promise.all(promises);

    // 创建行数据
    const goodsNoCounter = new Map<string, number>();

    newItems.forEach(o => {
      const goodsNo = o.F_GoodsNo;
      const baseCode = goodsNoCodeMap.get(goodsNo) || 0;

      // 获取当前计数器
      if (!goodsNoCounter.has(goodsNo)) {
        goodsNoCounter.set(goodsNo, baseCode);
      }
      const currentCode = goodsNoCounter.get(goodsNo)!;
      goodsNoCounter.set(goodsNo, currentCode + 1);

      const newRow = {
        F_WorkOrderId: o.id,
        F_CustomerId: o.F_GoodsId,
        F_GoodsName: o.F_GoodsName,
        F_GoodsNo: o.F_GoodsNo,
        F_CategoryId: o.F_CategoryId,
        F_Specification: o.F_Specification,
        F_PlanQty: 1, // 每条记录的计划数量都设置为 1
        F_InventoryNum: o.F_InventoryNum,
        F_YseNum: o.F_YseNum,
        F_Num: undefined,
        F_Price: o.F_CostPrice,
        F_Unit_Ratio: o.F_Unit_Ratio,
        F_Description: undefined,
        F_CreatorTime: undefined,
        _F_Encoding_Suffix: String(currentCode), // 设置编码后缀
      };

      // 按采购单数量拆成多条，每条最大单位数量为 1
      const row: any = {
        ...newRow,
        jnpfId: buildUUID(),
      };
      const levels = getUnitRatioLevels(row);
      if (levels) {
        if (levels.level2?.qty) {
          // 每条最大单位数量固定为 1，二级数量自动换算
          row.F_NumLevel1 = 1;
          const ratio = Number(levels.level2?.qty) || 0;
          row.F_NumLevel2 = 1 * ratio;
          row._F_NumLevel2Max = row.F_NumLevel2;
          row.F_Num = row.F_NumLevel2;
        } else {
          // 每条最大单位数量固定为 1，二级数量自动换算
          row.F_NumLevel1 = 1;
          row.F_NumLevel2 = 1;
          row._F_NumLevel2Max = row.F_NumLevel2;
          row.F_Num = row.F_NumLevel2;
        }
      } else {
        row.F_Num = 1;
        row.F_NumLevel2 = 1;
      }
      state.dataForm.tableFieldc43f9b.push(row);
      state.tableFieldc43f9binnerActiveKey.push(row.jnpfId);
    });
  }

  async function OutsourceBtn(val, option) {
    /* 兜底 */
    if (!Array.isArray(state.dataForm.tableFieldc43f9b)) {
      state.dataForm.tableFieldc43f9b = [];
    }
    const optionIdSet = new Set(option.map(o => o.id));

    /*  删掉已取消勾选的行 */
    state.dataForm.tableFieldc43f9b = state.dataForm.tableFieldc43f9b.filter(item => optionIdSet.has(item.F_WorkOrderId));

    // 收集新增的数据，按 F_GoodsNo 分组
    const newItems: any[] = [];
    option.forEach(o => {
      const exist = state.dataForm.tableFieldc43f9b.some(item => item.F_WorkOrderId === o.id);
      if (!exist) {
        const planQty = parseInt(o.F_PlanNum) || 1;
        // 根据计划数量创建多条记录
        for (let i = 0; i < planQty; i++) {
          newItems.push({
            ...o,
            _index: i, // 记录在同一 F_GoodsNo 下的顺序
          });
        }
      }
    });

    // 按 F_GoodsNo 分组
    const goodsNoMap = new Map<string, any[]>();
    newItems.forEach(item => {
      const goodsNo = item.F_GoodsNo;
      if (!goodsNoMap.has(goodsNo)) {
        goodsNoMap.set(goodsNo, []);
      }
      goodsNoMap.get(goodsNo)!.push(item);
    });

    // 对每个 F_GoodsNo 调用一次 getMaxCode
    const goodsNoCodeMap = new Map<string, number>();
    const promises: Promise<void>[] = [];

    for (const [goodsNo, items] of goodsNoMap) {
      // 获取该组的 F_GoodsId（所有 item 的 F_GoodsId 相同）
      const goodsId = items[0]?.F_GoodsId;
      if (goodsId) {
        const promise = getMaxCode(goodsId).then(res => {
          const baseCode = res?.data || 0;
          goodsNoCodeMap.set(goodsNo, Number(baseCode));
        });
        promises.push(promise);
      }
    }

    // 等待所有 getMaxCode 调用完成
    await Promise.all(promises);

    // 创建行数据
    const goodsNoCounter = new Map<string, number>();

    newItems.forEach(o => {
      const goodsNo = o.F_GoodsNo;
      const baseCode = goodsNoCodeMap.get(goodsNo) || 0;

      // 获取当前计数器
      if (!goodsNoCounter.has(goodsNo)) {
        goodsNoCounter.set(goodsNo, baseCode);
      }
      const currentCode = goodsNoCounter.get(goodsNo)!;
      goodsNoCounter.set(goodsNo, currentCode + 1);

      const newRow = {
        F_WorkOrderId: o.id,
        F_CustomerId: o.F_GoodsId,
        F_GoodsName: o.F_GoodsName,
        F_GoodsNo: o.F_GoodsNo,
        F_CategoryId: o.F_CategoryId,
        F_Specification: o.F_Specification,
        F_PlanQty: o.F_PlanNum,
        F_InventoryNum: o.F_InventoryNum,
        F_YseNum: o.F_YseNum,
        F_Num: undefined,
        F_Price: o.F_CostPrice,
        F_Unit_Ratio: o.F_Unit_Ratio,
        F_Description: undefined,
        F_CreatorTime: undefined,
        _F_Encoding_Suffix: String(currentCode), // 设置编码后缀
      };

      // 按采购单数量拆成多条，每条最大单位数量为 1
      const row: any = {
        ...newRow,
        jnpfId: buildUUID(),
      };
      const levels = getUnitRatioLevels(row);
      if (levels) {
        if (levels.level2?.qty) {
          // 每条最大单位数量固定为 1，二级数量自动换算
          row.F_NumLevel1 = 1;
          const ratio = Number(levels.level2?.qty) || 0;
          row.F_NumLevel2 = 1 * ratio;
          row._F_NumLevel2Max = row.F_NumLevel2;
          row.F_Num = row.F_NumLevel2;
        } else {
          // 每条最大单位数量固定为 1，二级数量自动换算
          row.F_NumLevel1 = 1;
          row.F_NumLevel2 = 1;
          row._F_NumLevel2Max = row.F_NumLevel2;
          row.F_Num = row.F_NumLevel2;
        }
      } else {
        row.F_Num = 1;
        row.F_NumLevel2 = 1;
      }
      state.dataForm.tableFieldc43f9b.push(row);
      state.tableFieldc43f9binnerActiveKey.push(row.jnpfId);
    });
  }

  /** 解析 F_CategoryId：如 "[\"C\",\"A2\"]" -> "CA2" */
  function parseCategoryId(categoryId) {
    if (!categoryId) return '';
    try {
      const str = typeof categoryId === 'string' ? categoryId : String(categoryId);
      const arr = JSON.parse(str);
      if (Array.isArray(arr)) {
        return arr.join('');
      }
      return '';
    } catch (e) {
      return '';
    }
  }

  /** 前缀 = parse(F_CategoryId) + F_GoodsNo；兼容详情里 F_CustomerId 为完整对象 */
  function getGoodsNoPrefix(record) {
    let categoryPrefix = '';
    let goodsNo = '';
    if (record?._F_CustomerObj) {
      categoryPrefix = parseCategoryId(record._F_CustomerObj.F_CategoryId);
      goodsNo = record._F_CustomerObj.F_GoodsNo || record._F_CustomerObj.GoodsNo || '';
    } else if (record?.F_CustomerId && typeof record.F_CustomerId === 'object') {
      categoryPrefix = parseCategoryId(record.F_CustomerId.F_CategoryId);
      goodsNo = record.F_CustomerId.F_GoodsNo || record.F_CustomerId.GoodsNo || '';
    } else {
      categoryPrefix = parseCategoryId(record.F_CategoryId);
      goodsNo = record.F_GoodsNo || '';
    }
    return categoryPrefix + goodsNo;
  }

  function handleEncodingChange(record) {
    const prefix = getGoodsNoPrefix(record);
    const suffix = record._F_Encoding_Suffix || '';
    record.F_Encoding = prefix ? prefix + suffix : suffix;
  }

  /** 回显：完整 F_Encoding 拆成前缀 + 可编辑后缀 */
  function refreshEncodingSuffixForTableRows() {
    const rows = state.dataForm.tableFieldc43f9b || [];
    for (const r of rows) {
      if (!r?.F_Encoding) continue;
      const prefix = getGoodsNoPrefix(r);
      if (prefix && r.F_Encoding.startsWith(prefix)) {
        r._F_Encoding_Suffix = r.F_Encoding.substring(prefix.length);
      } else {
        r._F_Encoding_Suffix = r.F_Encoding;
      }
    }
  }

  // 批量赋值仓库给选中的子表行
  function handleBatchAssignWarehouse() {
    if (!state.batchWarehouseValue) {
      createMessage.warning('请先选择入库仓库');
      return;
    }
    if (!state.selectedtableFieldc43f9bRowKeys.length) {
      createMessage.warning('请先勾选要赋值的子表数据');
      return;
    }
    const selectedKeys = state.selectedtableFieldc43f9bRowKeys;
    let assignCount = 0;
    for (const row of state.dataForm.tableFieldc43f9b) {
      if (selectedKeys.includes(row.jnpfId)) {
        row.F_WarehouseID = [...state.batchWarehouseValue];
        assignCount++;
      }
    }
    createMessage.success(`已成功赋值 ${assignCount} 条数据`);
  }

  // 解析 F_Unit_Ratio（与采购计划 Form 一致）：两级单位 { level1: { name, qty }, level2: { name, qty } }
  function getUnitRatioLevels(record) {
    const raw = record?.F_Unit_Ratio;
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
<style scoped lang="less">
  .stock-encoding-group {
    display: flex;
    width: 100%;
    align-items: stretch;
    min-width: 0;
  }

  .stock-encoding-prefix {
    flex: 0 0 auto;
    box-sizing: border-box;
    padding: 4px 11px;
    line-height: 1.5715;
    font-size: 14px;
    background-color: #f5f5f5;
    color: #666;
    border: 1px solid #d9d9d9;
    border-right: none;
    border-radius: 2px 0 0 2px;
    white-space: nowrap;
  }

  .stock-encoding-input-wrap {
    flex: 1 1 0;
    min-width: 64px;
    min-height: 0;
  }

  .stock-encoding-group--prefixed .stock-encoding-input-wrap :deep(.ant-input),
  .stock-encoding-group--prefixed .stock-encoding-input-wrap :deep(.ant-input-affix-wrapper) {
    border-top-left-radius: 0;
    border-bottom-left-radius: 0;
  }
</style>
