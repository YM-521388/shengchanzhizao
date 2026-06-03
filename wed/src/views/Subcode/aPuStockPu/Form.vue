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
        <a-space-compact size="small" block v-if="dataForm.id">
          <!-- <a-tooltip :title="t('common.prevRecord')">
            <a-button size="small" :disabled="getPrevDisabled" @click="handlePrev">
              <i class="icon-ym icon-ym-caret-left text-10px"></i>
            </a-button>
          </a-tooltip>
          <a-tooltip :title="t('common.nextRecord')">
            <a-button size="small" :disabled="getNextDisabled" @click="handleNext">
              <i class="icon-ym icon-ym-caret-right text-10px"></i>
            </a-button>
          </a-tooltip> -->
        </a-space-compact>
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
              <template #label>采购入库单号</template>
              <JnpfInput
                v-model:value="dataForm.F_StockInNo"
                placeholder="请填写,忽略将自动生成"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_StockInUserId')">
            <a-form-item name="F_StockInUserId" :labelCol="{ style: { width: '100px' } }">
              <template #label>收货人</template>
              <JnpfUserSelect
                v-model:value="dataForm.F_StockInUserId"
                placeholder="请选择"
                selectType="all"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_pu_Orderld" :labelCol="{ style: { width: '100px' } }">
              <template #label>采购单</template>
              <JnpfSelect
                v-model:value="dataForm.F_pu_Orderld"
                placeholder="请选择"
                :options="optionsObj.F_pu_OrderldOptions"
                :fieldNames="optionsObj.F_pu_OrderldProps"
                allowClear
                showSearch
                :disabled="!!state.dataForm.id"
                @change="handleOrderChange"
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
          <a-col :span="24" class="ant-col-item" v-if="!state.dataForm.id">
            <a-form-item :labelCol="{ style: { width: '100px' } }">
              <template #label>批量入库仓库</template>
              <a-space>
                <JnpfCascader
                  v-model:value="batchWarehouseValue"
                  placeholder="请选择仓库"
                  :options="optionsObj.tableField4bd139_F_WarehouseIDOptions"
                  :fieldNames="optionsObj.tableField4bd139_F_WarehouseIDProps"
                  allowClear
                  showSearch
                  changeOnSelect
                  :style="{
                    width: '300px',
                  }" />
                <a-button type="primary" size="small" @click="handleBatchAssignWarehouse" :disabled="!state.selectedtableField4bd139RowKeys.length">
                  批量赋值
                </a-button>
                <span class="text-gray-400 text-xs" v-if="state.selectedtableField4bd139RowKeys.length">
                  (已选 {{ state.selectedtableField4bd139RowKeys.length }} 条)
                </span>
              </a-space>
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="采购入库商品" :bordered="false" />

              <a-table
                :data-source="dataForm.tableField4bd139"
                :columns="tableField4bd139Columns"
                size="small"
                rowKey="jnpfId"
                :pagination="tableField4bd139Pagination"
                :scroll="{ x: 'max-content' }"
                :rowSelection="gettableField4bd139RowSelection">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                  <template v-if="column.key === 'F_CustomerId'">
                    <JnpfPopupSelect
                      v-model:value="record.F_CustomerId"
                      placeholder="请选择"
                      :templateJson="optionsObj.tableField4bd139_F_CustomerIdTemplateJson"
                      allowClear
                      :field="'F_CustomerId' + index"
                      interfaceId="2027641675890954240"
                      :columnOptions="optionsObj.tableField4bd139_F_CustomerIdOptions"
                      relationField="GoodsName"
                      propsValue="GoodsId"
                      :pageSize="20"
                      popupType="dialog"
                      popupTitle="选择数据"
                      popupWidth="70%"
                      hasPage
                      disabled
                      @change="(_, selected) => handleCustomerSelectChange(selected, record, index)"
                      :style="{
                        width: '100%',
                      }"
                      :extraOptions="optionsObj.tableField4bd139_F_CustomerId" />
                  </template>
                  <template v-if="column.key === 'OrderNo'">
                    <span>{{ record.OrderNo }}</span>
                  </template>
                  <template v-if="column.key === 'SupplierName'">
                    <span>{{ record.SupplierName }}</span>
                  </template>
                  <template v-if="column.key === 'GoodsName'">
                    <span>{{ record.GoodsName }}</span>
                  </template>
                  <template v-if="column.key === 'GoodsNo'">
                    <span>{{ record.GoodsNo }}</span>
                  </template>
                  <!-- <template v-if="column.key === 'Specification'">
                    <span>{{ record.Specification }}</span>
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
                      :controls="false"
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <!-- <template v-if="column.key === 'F_CostAmount'">
                    <span>{{ thousandsFormat(calculateRowCostAmount(record)) }}</span>
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
                  <template v-if="column.key === 'F_WarehouseID'">
                    <JnpfCascader
                      v-model:value="record.F_WarehouseID"
                      placeholder="请选择"
                      :disabled="!!state.dataForm.id"
                      :options="optionsObj.tableField4bd139_F_WarehouseIDOptions"
                      :fieldNames="optionsObj.tableField4bd139_F_WarehouseIDProps"
                      :changeOnSelect="true"
                      allowClear
                      showSearch
                      :style="{
                        width: '100%',
                      }" />
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
                      <!-- <a-button class="action-btn" type="link" @click="copytableField4bd139Row(index)" size="small">{{
                        t('common.copyText', '复制')
                      }}</a-button> -->
                      <a-button class="action-btn" type="link" color="error" @click="removetableField4bd139Row(index, true)" size="small">{{
                        t('common.delText', '删除')
                      }}</a-button>
                    </a-space>
                  </template>
                </template>
              </a-table>
              <a-space class="input-table-footer-btn">
                <!-- <a-button type="primary" preIcon="icon-ym icon-ym-btn-add" @click="addtableField4bd139Row">{{ t('common.add1Text', '添加') }}</a-button>
                <a-button type="danger" preIcon="icon-ym icon-ym-btn-clearn" @click="batchRemovetableField4bd139Row(true)">{{
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
  import { getMaxCode } from '@/views/Subcode/aGoodsInventoryQr/helper/api';
  import { reactive, toRefs, nextTick, ref, unref, computed, watch, inject } from 'vue';
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
  import { getOrderItems } from './helper/api';
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
    currVmodel: any;
    currTableConf: any;
    addTableConf: any;
    tableRows: any;
    tableField4bd139outerActiveKey: number[];
    tableField4bd139innerActiveKey: string[];
    tableField4bd139DefaultExpandAll: boolean;
    selectedtableField4bd139RowKeys: any[];
    tableField4bd139PageSize: number;
    tableField4bd139CurrentPage: number;
    tableFieldca527douterActiveKey: number[];
    tableFieldca527dinnerActiveKey: string[];
    tableFieldca527dDefaultExpandAll: boolean;
    selectedtableFieldca527dRowKeys: any[];
    title: string;
    continueText: string;
    allList: any[];
    currIndex: number;
    isContinue: boolean;
    submitType: number;
    showContinueBtn: boolean;
    batchWarehouseValue: any;
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
  const tableField4bd139Columns: any[] = computed(() => {
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
        title: '商品编号',
        dataIndex: 'GoodsNo',
        key: 'GoodsNo',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'GoodsNo',
        isSystemControl: true,
      },
      // {
      //   title: '规格',
      //   dataIndex: 'Specification',
      //   key: 'Specification',
      //   tipLabel: '',
      //   required: false,
      //   align: 'left',
      //   span: '24',
      //   labelWidth: '',
      //   width: '',
      //   fixed: false,
      //   formP: 'Specification',
      //   isSystemControl: true,
      // },
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
        title: '数量',
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
      // {
      //   title: '成本金额(元)',
      //   dataIndex: 'F_CostAmount',
      //   key: 'F_CostAmount',
      //   tipLabel: '',
      //   required: false,
      //   align: 'left',
      //   span: '24',
      //   labelWidth: '',
      //   width: '',
      //   fixed: false,
      //   formP: 'F_CostAmount',
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
    list = list.filter(o => hasFormP('tableField4bd139-' + o.formP));
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
  const tableField4bd139Pagination = computed(() => ({
    current: state.tableField4bd139CurrentPage,
    pageSize: state.tableField4bd139PageSize,
    size: 'small',
    showSizeChanger: true,
    pageSizeOptions: [20, 50, 100],
    showTotal: (total: number) => `共 ${total} 条`,
    onChange: (page: number) => {
      state.tableField4bd139CurrentPage = page;
    },
    onShowSizeChange: (_current: number, size: number) => {
      state.tableField4bd139PageSize = size;
      state.tableField4bd139CurrentPage = 1; // 切换页大小时回到第1页
    },
  }));

  const gettableField4bd139RowSelection = computed(() => {
    const rowSelection = {
      selectedRowKeys: state.selectedtableField4bd139RowKeys,
      preserveSelectedRowKeys: true,
      onChange: (selectedRowKeys: string[]) => {
        state.selectedtableField4bd139RowKeys = (selectedRowKeys || []).sort().reverse();
      },
    };
    return rowSelection;
  });
  const tableFieldca527dColumns: any[] = computed(() => {
    let list = [
      {
        title: '类型',
        dataIndex: 'F_Type',
        key: 'F_Type',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Type',
        isSystemControl: false,
      },
      {
        title: '标题',
        dataIndex: 'F_Title',
        key: 'F_Title',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Title',
        isSystemControl: false,
      },
      {
        title: '内容',
        dataIndex: 'F_Content',
        key: 'F_Content',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Content',
        isSystemControl: false,
      },
      {
        title: '修改原因',
        dataIndex: 'F_Reason',
        key: 'F_Reason',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Reason',
        isSystemControl: false,
      },
      {
        title: '创建人员',
        dataIndex: 'F_CreatorUserId',
        key: 'F_CreatorUserId',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_CreatorUserId',
        isSystemControl: true,
      },
      {
        title: '创建时间',
        dataIndex: 'F_CreatorTime',
        key: 'F_CreatorTime',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_CreatorTime',
        isSystemControl: true,
      },
    ];
    list = list.filter(o => hasFormP('tableFieldca527d-' + o.formP));
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
  const gettableFieldca527dRowSelection = computed(() => {
    const rowSelection = {
      selectedRowKeys: state.selectedtableFieldca527dRowKeys,
      onChange: (selectedRowKeys: string[]) => {
        state.selectedtableFieldca527dRowKeys = (selectedRowKeys || []).sort().reverse();
      },
    };
    return rowSelection;
  });
  const state = reactive<State>({
    dataForm: {
      id: '',
      F_StockInNo: undefined,
      F_WarehouseId: [],
      F_StockInType: undefined,
      F_StockInDate: dayjs(new Date()).startOf(getDateTimeUnit('yyyy-MM-dd')).valueOf(),
      F_StockInUserId: userInfo.userId ? userInfo.userId : '',
      F_pu_Orderld: undefined,
      F_Description: undefined,
      tableField4bd139: [],
      tableFieldca527d: [],
    },
    batchWarehouseValue: undefined,
    tableField4bd139outerActiveKey: [0],
    tableField4bd139innerActiveKey: [],
    tableField4bd139DefaultExpandAll: true,
    selectedtableField4bd139RowKeys: [],
    tableField4bd139PageSize: 20,
    tableField4bd139CurrentPage: 1,
    tableFieldca527douterActiveKey: [0],
    tableFieldca527dinnerActiveKey: [],
    tableFieldca527dDefaultExpandAll: true,
    selectedtableFieldca527dRowKeys: [],
    dataRule: {
      F_StockInDate: [
        {
          required: true,
          message: '请输入入库日期',
          trigger: 'change',
        },
      ],
      F_StockInUserId: [
        {
          required: true,
          message: '请输入收货人',
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
      F_pu_Orderld: [
        {
          required: true,
          message: '请输入采购单',
          trigger: 'change',
        },
      ],
    },
    optionsObj: {
      F_WarehouseIdProps: {
        label: 'F_Name',
        value: 'F_Id',
        children: 'children',
      },
      F_StockInTypeProps: { label: 'fullName', value: 'enCode' },
      F_pu_OrderldProps: { label: 'F_OrderNo', value: 'F_Id' },
      tableField4bd139_F_CustomerIdOptions: [
        {
          value: 'OrderNo',
          label: '采购单号',
        },
        {
          value: 'GoodsName',
          label: '商品名称',
        },
        {
          value: 'GoodsNo',
          label: '商品编号',
        },
        // {
        //   value: 'Specification',
        //   label: '单位',
        // },
        {
          value: 'F_Num',
          label: '数量',
        },
        {
          value: 'F_Price',
          label: '单价',
        },
      ],
      tableField4bd139_F_WarehouseIDOptions: [],
      tableField4bd139_F_WarehouseIDProps: {
        label: 'F_Name',
        value: 'F_Id',
        children: 'children',
      },
      tableField4bd139_F_CustomerIdTemplateJson: computed(() => [
        {
          defaultValue: '',
          field: 'id',
          dataType: 'varchar',
          required: 0,
          fieldName: '',
          relationField: state.dataForm.F_pu_Orderld || '',
          jnpfKey: null,
          complexHeaderList: null,
          sourceType: 2,
          isChildren: false,
          IsMultiple: false,
        },
      ]),
    },
    currVmodel: '',
    currTableConf: {},
    tableRows: {
      tableField4bd139: {
        enabledmark: undefined,
        F_CustomerId: undefined,
        OrderNo: undefined,
        SupplierName: undefined,
        GoodsName: undefined,
        GoodsNo: undefined,
        Specification: undefined,
        F_Encoding: undefined,
        _F_Encoding_Suffix: undefined,
        F_WarehouseID: [],
        F_Unit_Ratio: undefined,
        F_NumLevel1: undefined,
        F_NumLevel2: undefined,
        _F_NumLevel2Max: undefined,
        F_Num: undefined,
        F_Price: undefined,
        F_Description: undefined,
        F_CreatorUserId: undefined,
        F_CreatorTime: undefined,
      },
      tableFieldca527d: {
        enabledmark: undefined,
        F_Type: undefined,
        F_Title: undefined,
        F_Content: undefined,
        F_Reason: undefined,
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
  const { title, continueText, showContinueBtn, dataForm, submitType, dataRule, optionsObj, batchWarehouseValue } = toRefs(state);

  // 解析 F_Unit_Ratio（与采购计划 Form 一致）：两级单位 { level1: { name, qty }, level2: { name, qty } }
  function getUnitRatioLevels(record) {
    const raw = record?.F_Unit_Ratio;
    if (!raw) return null;
    try {
      const str = typeof raw === 'string' ? raw : String(raw);
      const arr = JSON.parse(str);
      if (!Array.isArray(arr) || arr.length < 1) return null;
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

  /** 回显：F_Num 与提交一致，存二级数量（最小单位），据此拆成一级 + 二级展示 */
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

  /** 成本金额：有两级单位时按换算后一级当量 × 单价（二级可改小时与数量一致） */
  function calculateRowCostAmount(record) {
    const levels = getUnitRatioLevels(record);
    let num = 0;
    if (levels != null) {
      const ratio = Number(levels.level2?.qty) || 0;
      if (ratio > 0) {
        num = (Number(record.F_NumLevel2) || 0) / ratio;
      } else {
        num = Number(record.F_NumLevel1) || 0;
      }
    } else {
      num = Number(record.F_Num) || 0;
    }
    const price = Number(record.F_Price) || 0;
    return num * price;
  }

  /** 解析 F_CategoryId：JSON 数组字符串拼接，如 "[\"C\", \"A1\"]" -> "CA1" */
  function parseCategoryId(categoryId) {
    if (!categoryId) return '';
    try {
      const str = typeof categoryId === 'string' ? categoryId : String(categoryId);
      const arr = JSON.parse(str);
      if (Array.isArray(arr)) return arr.join('');
      return '';
    } catch (e) {
      return '';
    }
  }

  /** 库存编码前缀：分类解析值 + GoodsNo（与 InfoByIds / 商品接口字段一致） */
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
      goodsNo = record.GoodsNo || '';
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
    const rows = state.dataForm.tableField4bd139 || [];
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

  function handleCustomerSelectChange(selected: any, record: any, rowIndex: number) {
    handleOrderItemChange(selected, record, rowIndex);
    fillNumLevelsFromF_Num(record);
    handleEncodingChange(record);
  }

  watch(
    () => state.dataForm.tableField4bd139,
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
      state.dataForm.tableField4bd139 = [];
      state.dataForm.tableFieldca527d = [];
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
        F_WarehouseId: [],
        F_StockInType: undefined,
        F_StockInDate: dayjs(new Date()).startOf(getDateTimeUnit('yyyy-MM-dd')).valueOf(),
        F_StockInUserId: userInfo.userId ? userInfo.userId : '',
        F_pu_Orderld: undefined,
        F_Description: undefined,
        tableField4bd139: [],
        tableFieldca527d: [],
      };
      getF_WarehouseIdOptions();
      getF_StockInTypeOptions();
      getF_pu_OrderldOptions();
      gettableField4bd139_F_WarehouseIDOptions();
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
      for (let i = 0; i < state.dataForm.tableField4bd139.length; i++) {
        const element = state.dataForm.tableField4bd139[i];
        element.jnpfId = buildUUID();
      }
      for (let i = 0; i < state.dataForm.tableFieldca527d.length; i++) {
        const element = state.dataForm.tableFieldca527d[i];
        element.jnpfId = buildUUID();
      }
      // 填充子表商品编号和规格
      try {
        const rows = state.dataForm.tableField4bd139 || [];
        const idsToFetch: any[] = [];
        for (let i = 0; i < rows.length; i++) {
          const r = rows[i];
          if (!r) continue;
          // 如果关联字段已经是对象，直接使用
          if (r.F_CustomerId && typeof r.F_CustomerId === 'object') {
            handleOrderItemChange(r.F_CustomerId, r, i);
            fillNumLevelsFromF_Num(r);
            continue;
          }
          // 如果显示字段已经存在，跳过
          if (r.GoodsNo || r.Specification) continue;
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
                  handleOrderItemChange(found, r, i);
                  fillNumLevelsFromF_Num(r);
                }
              }
              refreshEncodingSuffixForTableRows();
            })
            .catch(() => {
              // ignore errors
            });
        }
      } catch (e) {
        // swallow errors
      }

      getF_WarehouseIdOptions();
      getF_StockInTypeOptions();
      getF_pu_OrderldOptions();
      gettableField4bd139_F_WarehouseIDOptions();
      changeLoading(false);
    });
  }
  async function handleSubmit() {
    try {
      const values = await getForm()?.validate();
      if (!values) return;
      // 提交前：子表数量、库存编码（前缀+可编辑后缀）
      for (const row of state.dataForm.tableField4bd139 || []) {
        if (row.F_Unit_Ratio && row.F_NumLevel2 !== undefined && row.F_NumLevel2 !== null) {
          row.F_Num = row.F_NumLevel2;
        }
        if (row._F_Encoding_Suffix !== undefined || getGoodsNoPrefix(row)) {
          handleEncodingChange(row);
        }
      }

      // 验证子表 F_WarehouseID 是否全部填写
      const tableField4bd139 = state.dataForm.tableField4bd139 || [];
      if (tableField4bd139.length > 0) {
        const emptyWarehouseRows = tableField4bd139.filter((row: any) => {
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
        const rows = state.dataForm.tableField4bd139 || [];
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
        const rows = state.dataForm.tableField4bd139 || [];
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
  // function getF_WarehouseIdOptions() {
  //   let templateJson = [];
  //   let query = {
  //     paramList: getParamList(templateJson, state.dataForm),
  //   };
  //   getDataInterfaceRes('2012073572784279552', query).then(res => {
  //     let data = res.data;
  //     state.optionsObj.F_WarehouseIdOptions = Array.isArray(data) ? data : [];
  //   });
  // }

  function gettableField4bd139_F_WarehouseIDOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2021045201115680768', query).then(res => {
      let data = res.data;
      state.optionsObj.tableField4bd139_F_WarehouseIDOptions = Array.isArray(data) ? data : [];
    });
  }

  function getF_pu_OrderldOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2011984543933927424', query).then(res => {
      let data = res.data;
      state.optionsObj.F_pu_OrderldOptions = Array.isArray(data) ? data : [];
    });
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

  function getF_StockInTypeOptions() {
    getDictionaryDataSelector('2012074650393251840').then(res => {
      state.optionsObj.F_StockInTypeOptions = res.data.list;
    });
  }
  function addtableField4bd139Row() {
    let item = {
      jnpfId: buildUUID(),
      F_CustomerId: undefined,
      OrderNo: undefined,
      SupplierName: undefined,
      GoodsName: undefined,
      GoodsNo: undefined,
      Specification: undefined,
      F_Encoding: undefined,
      _F_Encoding_Suffix: undefined,
      F_CategoryId: undefined,
      _F_CustomerObj: undefined,
      F_WarehouseID: [],
      F_Unit_Ratio: undefined,
      F_NumLevel1: undefined,
      F_NumLevel2: undefined,
      _F_NumLevel2Max: undefined,
      F_Num: undefined,
      F_Price: undefined,
      F_Description: undefined,
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
      _quantityDisabled: undefined,
    };
    state.dataForm.tableField4bd139.push(item);
    state.tableField4bd139innerActiveKey.push(item.jnpfId);
  }
  function removetableField4bd139Row(index, showConfirm = false) {
    const doRemove = () => {
      state.dataForm.tableField4bd139.splice(index, 1);
    };

    if (showConfirm) {
      createConfirm({
        iconType: 'warning',
        title: '提示',
        content: '此操作将永久删除该数据, 是否继续?',
        onOk: doRemove,
      });
    } else {
      doRemove();
    }
  }
  function copytableField4bd139Row(index) {
    let item = cloneDeep(state.dataForm.tableField4bd139[index]);
    let copyData = {};
    for (let i = 0; i < unref(tableField4bd139Columns).length; i++) {
      const cur = unref(tableField4bd139Columns)[i];
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
    const copyItem: any = { ...copyData, jnpfId: buildUUID() };
    copyItem._F_CustomerObj = item._F_CustomerObj;
    copyItem.F_CategoryId = item.F_CategoryId;
    copyItem._F_Encoding_Suffix = item._F_Encoding_Suffix;
    state.dataForm.tableField4bd139.push(copyItem);
    state.tableField4bd139innerActiveKey.push(copyItem.jnpfId);
    handleEncodingChange(copyItem);
  }
  function batchRemovetableField4bd139Row(showConfirm = false) {
    if (!state.selectedtableField4bd139RowKeys.length) return createMessage.error('请选择一条数据');
    const handleBatchRemove = () => {
      state.dataForm.tableField4bd139 = state.dataForm.tableField4bd139.filter(o => !state.selectedtableField4bd139RowKeys.includes(o.jnpfId));
      nextTick(() => {
        state.selectedtableField4bd139RowKeys = [];
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
  function addtableFieldca527dRow() {
    let item = {
      jnpfId: buildUUID(),
      F_Type: undefined,
      F_Title: undefined,
      F_Content: undefined,
      F_Reason: undefined,
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
    };
    state.dataForm.tableFieldca527d.push(item);
    state.tableFieldca527dinnerActiveKey.push(item.jnpfId);
  }
  function removetableFieldca527dRow(index, showConfirm = false) {
    if (showConfirm) {
      createConfirm({
        iconType: 'warning',
        title: '提示',
        content: '此操作将永久删除该数据, 是否继续?',
        onOk: () => {
          state.dataForm.tableFieldca527d.splice(index, 1);
        },
      });
    } else {
      state.dataForm.tableFieldca527d.splice(index, 1);
    }
  }
  function copytableFieldca527dRow(index) {
    let item = cloneDeep(state.dataForm.tableFieldca527d[index]);
    let copyData = {};
    for (let i = 0; i < unref(tableFieldca527dColumns).length; i++) {
      const cur = unref(tableFieldca527dColumns)[i];
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
    state.dataForm.tableFieldca527d.push(copyItem);
    state.tableFieldca527dinnerActiveKey.push(copyItem.jnpfId);
  }
  function batchRemovetableFieldca527dRow(showConfirm = false) {
    if (!state.selectedtableFieldca527dRowKeys.length) return createMessage.error('请选择一条数据');
    const handleBatchRemove = () => {
      state.dataForm.tableFieldca527d = state.dataForm.tableFieldca527d.filter(o => !state.selectedtableFieldca527dRowKeys.includes(o.jnpfId));
      nextTick(() => {
        state.selectedtableFieldca527dRowKeys = [];
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

  // 采购单变化时获取商品列表：每行对应采购单子表一行；带单位换算时展示「一级+二级」且数量由采购单带出、不可改（与计划单展示一致，一级框在带入时禁用）
  async function handleOrderChange(orderId) {
    if (!orderId) {
      // 清空商品列表
      state.dataForm.tableField4bd139 = [];
      return;
    }

    try {
      // 获取采购单商品列表
      const res = await getOrderItems(orderId);
      const items = res.data || [];

      if (!items.length) {
        state.dataForm.tableField4bd139 = [];
        return;
      }

      // 批量获取所有商品的详细信息（与编辑回显逻辑一致，确保每行商品编号、规格、库存编码前缀正确）
      const idsToFetch: string[] = [];
      for (const item of items) {
        const goodsId = item.F_CustomerId || item.GoodsId;
        if (goodsId) idsToFetch.push(String(goodsId));
      }
      const uniqueIds = Array.from(new Set(idsToFetch));
      const goodsMap: Record<string, any> = {};
      if (uniqueIds.length) {
        const interfaceId = '2008721559174385664';
        const query = {
          ids: uniqueIds,
          interfaceId,
          propsValue: 'F_Id',
          relationField: 'F_GoodsName',
          paramList: [],
        };
        const resp = await getDataInterfaceDataInfoByIds(interfaceId, query);
        const list = resp.data || [];
        const idKey = 'F_Id';
        list.forEach((obj: any) => {
          if (obj && obj[idKey] != null) goodsMap[String(obj[idKey])] = obj;
        });
      }

      // 收集所有要创建的行数据，按 GoodsNo 分组
      const allNewRows: any[] = [];
      for (const item of items) {
        const goodsId = item.F_CustomerId || item.GoodsId;
        const goodsInfo = goodsId ? goodsMap[String(goodsId)] : null;

        const orderQty = Math.max(1, parseInt(String(item.F_Num), 10) || 0);
        const price = item.F_Price ?? 0;
        // 生成基础行数据（含单位换算），商品编号、规格、_F_CustomerObj 必须来自 goodsInfo 以跟随每行商品
        const baseRow: any = {
          F_CustomerId: item.F_CustomerId || item.GoodsId || undefined,
          OrderNo: item.F_OrderNo || item.OrderNo || '',
          SupplierName: item.SupplierName || '',
          GoodsName: item.GoodsName ?? goodsInfo?.F_GoodsName ?? goodsInfo?.GoodsName ?? '',
          GoodsNo: item.GoodsNo ?? goodsInfo?.GoodsNo ?? goodsInfo?.F_GoodsNo ?? '',
          F_CategoryId: goodsInfo?.F_CategoryId ?? item.F_CategoryId,
          _F_CustomerObj: goodsInfo ? { ...goodsInfo } : undefined,
          Specification: item.Specification ?? goodsInfo?.Specification ?? goodsInfo?.F_Specification ?? '',
          F_Unit_Ratio: goodsInfo?.F_Unit_Ratio ?? item.F_Unit_Ratio,
          F_Price: price,
          F_Description: undefined,
          F_Encoding: undefined,
          _F_Encoding_Suffix: undefined,
          F_WarehouseID: [],
          F_CreatorUserId: undefined,
          F_CreatorTime: undefined,
          _quantityDisabled: true,
        };

        // 按采购单数量拆成多条，每条最大单位数量为 1
        for (let i = 0; i < orderQty; i++) {
          const row: any = {
            ...baseRow,
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
          allNewRows.push(row);
        }
      }

      // 按 GoodsNo 分组
      const goodsNoMap = new Map<string, any[]>();
      allNewRows.forEach(row => {
        const goodsNo = row.GoodsNo;
        if (!goodsNoMap.has(goodsNo)) {
          goodsNoMap.set(goodsNo, []);
        }
        goodsNoMap.get(goodsNo)!.push(row);
      });

      // 对每个 GoodsNo 调用一次 getMaxCode
      const goodsNoCodeMap = new Map<string, number>();
      const promises: Promise<void>[] = [];

      for (const [goodsNo, rows] of goodsNoMap) {
        // 获取该组的 F_CustomerId（所有 row 的 F_CustomerId 相同）
        const customerId = rows[0]?.F_CustomerId;
        if (customerId) {
          const promise = getMaxCode(customerId).then(res => {
            const baseCode = res?.data || 0;
            goodsNoCodeMap.set(goodsNo, Number(baseCode));
          });
          promises.push(promise);
        }
      }

      // 等待所有 getMaxCode 调用完成
      await Promise.all(promises);

      // 设置编码后缀
      const goodsNoCounter = new Map<string, number>();
      const tableItems: any[] = [];

      for (const row of allNewRows) {
        const goodsNo = row.GoodsNo;
        const baseCode = goodsNoCodeMap.get(goodsNo) || 0;

        // 获取当前计数器
        if (!goodsNoCounter.has(goodsNo)) {
          goodsNoCounter.set(goodsNo, baseCode);
        }
        const currentCode = goodsNoCounter.get(goodsNo)!;
        goodsNoCounter.set(goodsNo, currentCode + 1);

        row._F_Encoding_Suffix = String(currentCode);
        tableItems.push(row);
      }

      state.dataForm.tableField4bd139 = tableItems;
    } catch (e) {
      console.error('获取采购单商品失败:', e);
    }
  }

  // 填充商品展示字段（根据商品ID获取商品名称）
  function fillGoodsDisplayFields(record: any) {
    try {
      const goodsId = record.F_CustomerId;
      if (!goodsId) return;

      // 通过商品数据接口获取商品信息
      const query = {
        paramList: [{ defaultValue: goodsId, field: 'F_Id', dataType: 'varchar' }],
      };
      getDataInterfaceRes('2008721559174385664', query).then((res: any) => {
        const data = Array.isArray(res.data) ? res.data[0] : res.data;
        if (!data) return;
        record._F_CustomerObj = data;
        record.F_CategoryId = data.F_CategoryId;
        // 只填充商品名称，不覆盖已有的编号和规格（编号和规格已由后端返回）
        record.GoodsName = data.F_GoodsName ?? data.GoodsName;
        record.F_Unit_Ratio = data.F_Unit_Ratio ?? record.F_Unit_Ratio;
        fillNumLevelsFromF_Num(record);
        handleEncodingChange(record);
      });
    } catch (e) {
      // ignore
    }
  }

  // 编辑时填充子表商品编号和规格
  function handleOrderItemChange(selected: any, record: any, _rowIndex: number) {
    const data = Array.isArray(selected) ? selected[0] : selected;
    if (!data) {
      record.GoodsNo = undefined;
      record.Specification = undefined;
      record.GoodsName = undefined;
      record.F_Unit_Ratio = undefined;
      record.F_NumLevel1 = undefined;
      record.F_NumLevel2 = undefined;
      record._F_NumLevel2Max = undefined;
      record.F_CategoryId = undefined;
      record._F_CustomerObj = undefined;
      record._F_Encoding_Suffix = undefined;
      record.F_Encoding = undefined;
      return;
    }
    record._F_CustomerObj = data;
    record.F_CategoryId = data.F_CategoryId;
    // 填充商品编号
    record.GoodsNo = data.GoodsNo ?? data.F_GoodsNo ?? data.F_GoodsName ?? data.name ?? record.GoodsNo;
    // 填充规格
    record.Specification = data.Specification ?? data.F_Specification ?? data.spec ?? record.Specification;
    // 填充商品名称
    record.GoodsName = data.F_GoodsName ?? data.GoodsName ?? data.name ?? record.GoodsName;
    record.F_Unit_Ratio = data.F_Unit_Ratio ?? record.F_Unit_Ratio;
  }

  // 批量赋值仓库给选中的子表行
  function handleBatchAssignWarehouse() {
    if (!state.batchWarehouseValue) {
      createMessage.warning('请先选择入库仓库');
      return;
    }
    if (!state.selectedtableField4bd139RowKeys.length) {
      createMessage.warning('请先勾选要赋值的子表数据');
      return;
    }
    const selectedKeys = state.selectedtableField4bd139RowKeys;
    let assignCount = 0;
    for (const row of state.dataForm.tableField4bd139) {
      if (selectedKeys.includes(row.jnpfId)) {
        row.F_WarehouseID = [...state.batchWarehouseValue];
        assignCount++;
      }
    }
    createMessage.success(`已成功赋值 ${assignCount} 条数据`);
  }
</script>
<style scoped lang="less">
  /* 库存编码：前缀按内容宽度，剩余空间给可编辑输入框 */
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
