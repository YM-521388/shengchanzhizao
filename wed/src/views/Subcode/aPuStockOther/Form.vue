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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_StockInNo')">
            <a-form-item name="F_StockInNo" :labelCol="{ style: { width: '100px' } }">
              <template #label>其他入库单号</template>
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_SupplierId')">
            <a-form-item name="F_SupplierId" :labelCol="{ style: { width: '100px' } }">
              <template #label>供应商</template>
              <JnpfSelect
                v-model:value="dataForm.F_SupplierId"
                placeholder="请选择"
                :options="optionsObj.F_SupplierIdOptions"
                :fieldNames="optionsObj.F_SupplierIdProps"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <!-- <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_WarehouseId')">
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
              <template #label>库存商品选择</template>
              <JnpfPopupSelect
                v-model:value="tempGoodsSelector"
                placeholder="请选择商品"
                :templateJson="optionsObj.tableFieldecf5cb_F_CustomerIdTemplateJson"
                allowClear
                :field="'tempGoodsSelector'"
                interfaceId="2008721559174385664"
                :columnOptions="optionsObj.tableFieldecf5cb_F_CustomerIdOptions"
                relationField="F_GoodsName"
                propsValue="F_Id"
                :pageSize="20"
                popupType="dialog"
                popupTitle="选择商品"
                popupWidth="70%"
                hasPage
                multiple
                :style="{
                  width: '100%',
                }"
                @change="handleGoodsSelectorChange" />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="!state.dataForm.id">
            <a-form-item :labelCol="{ style: { width: '100px' } }">
              <template #label>批量入库仓库</template>
              <a-space>
                <JnpfCascader
                  v-model:value="batchWarehouseValue"
                  placeholder="请选择仓库"
                  :options="optionsObj.tableFieldecf5cb_F_WarehouseIDOptions"
                  :fieldNames="optionsObj.tableFieldecf5cb_F_WarehouseIDProps"
                  allowClear
                  showSearch
                  changeOnSelect
                  :style="{
                    width: '300px',
                  }" />
                <a-button type="primary" size="small" @click="handleBatchAssignWarehouse" :disabled="!state.selectedtableFieldecf5cbRowKeys.length">
                  批量赋值
                </a-button>
                <span class="text-gray-400 text-xs" v-if="state.selectedtableFieldecf5cbRowKeys.length">
                  (已选 {{ state.selectedtableFieldecf5cbRowKeys.length }} 条)
                </span>
              </a-space>
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="入库商品列表" :bordered="false" />
              <a-table
                :data-source="dataForm.tableFieldecf5cb"
                :columns="tableFieldecf5cbColumns"
                size="small"
                rowKey="jnpfId"
                :pagination="listPagination"
                :scroll="{ x: 'max-content' }"
                :rowSelection="gettableFieldecf5cbRowSelection">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                  <template v-if="column.key === 'F_CustomerId'">
                    <JnpfPopupSelect
                      v-model:value="record.F_CustomerId"
                      placeholder=""
                      :templateJson="optionsObj.tableFieldecf5cb_F_CustomerIdTemplateJson"
                      allowClear
                      :field="'F_CustomerId' + index"
                      interfaceId="2008721559174385664"
                      :columnOptions="optionsObj.tableFieldecf5cb_F_CustomerIdOptions"
                      relationField="F_GoodsName"
                      propsValue="F_Id"
                      :pageSize="20"
                      popupType="dialog"
                      popupTitle="选择数据"
                      popupWidth="70%"
                      hasPage
                      disabled
                      :style="{
                        width: '100%',
                      }"
                      :extraOptions="optionsObj.tableFieldecf5cb_F_CustomerId"
                      @change="(val, obj) => handleGoodsChange(record, val, obj)" />
                  </template>
                  <template v-if="column.key === 'F_Num'">
                    <template v-if="getUnitRatioLevels(record)?.level2?.name">
                      <a-space>
                        <JnpfInputNumber
                          v-model:value="record.F_NumLevel1"
                          placeholder="1"
                          :controls="false"
                          disabled
                          :style="{ width: '80px' }"
                          @change="handleNumLevel1Change(record)" />
                        <span>{{ getUnitRatioLevels(record)?.level1?.name }}</span>
                        <JnpfInputNumber
                          v-model:value="record.F_NumLevel2"
                          placeholder="请输入"
                          :controls="false"
                          :disabled="!!state.dataForm.id"
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
                          :style="{ width: '80px' }" />
                        <span>{{ getUnitRatioLevels(record)?.level1?.name }}</span>
                      </a-space>
                    </template>
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
                            width: '100px',
                          }"
                          :showCount="false"
                          @change="handleEncodingChange(record)" />
                      </div>
                    </div>
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
                  <template v-if="column.key === 'F_Sales_Price'">
                    <JnpfInputNumber
                      v-model:value="record.F_Sales_Price"
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
                      :options="optionsObj.tableFieldecf5cb_F_WarehouseIDOptions"
                      :fieldNames="optionsObj.tableFieldecf5cb_F_WarehouseIDProps"
                      :changeOnSelect="true"
                      allowClear
                      :disabled="!!state.dataForm.id"
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
                      <!-- <a-button class="action-btn" type="link" @click="copytableFieldecf5cbRow(index)" size="small">{{
                        t('common.copyText', '复制')
                      }}</a-button> -->
                      <a-button class="action-btn" type="link" color="error" @click="removetableFieldecf5cbRow(index, true)" size="small">{{
                        t('common.delText', '删除')
                      }}</a-button>
                    </a-space>
                  </template>
                </template>
              </a-table>
              <a-space class="input-table-footer-btn">
                <!-- <a-button type="primary" preIcon="icon-ym icon-ym-btn-add" @click="addtableFieldecf5cbRow">{{ t('common.add1Text', '添加') }}</a-button>
                <a-button type="danger" preIcon="icon-ym icon-ym-btn-clearn" @click="batchRemovetableFieldecf5cbRow(true)">{{
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
  <!-- 商品数量设置弹窗 -->
  <BasicModal
    v-bind="$attrs"
    @register="registerQuantityModal"
    width="600px"
    :minHeight="100"
    okText="确定"
    cancelText="取消"
    @ok="handleQuantityConfirm"
    title="设置商品数量">
    <a-table :data-source="state.pendingGoodsList" :columns="quantityModalColumns" size="small" :pagination="false" :scroll="{ y: 400 }">
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'goodsName'">
          <span>{{ record.goodsName || '未知商品' }}</span>
        </template>
        <template v-if="column.key === 'quantity'">
          <JnpfInputNumber
            v-model:value="record.quantity"
            placeholder="请输入数量"
            :min="1"
            :controls="true"
            :style="{
              width: '100%',
            }" />
        </template>
      </template>
    </a-table>
  </BasicModal>
</template>
<script lang="ts" setup>
  import { create, update, getInfo } from './helper/api';
  import { getMaxCode } from '@/views/Subcode/aGoodsInventoryQr/helper/api';
  import { reactive, toRefs, nextTick, ref, unref, computed, watch, toRaw, inject } from 'vue';
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
    dataForm: any;
    dataRule: any;
    optionsObj: any;
    currVmodel: any;
    currTableConf: any;
    addTableConf: any;
    tableRows: any;
    tableFieldecf5cbouterActiveKey: number[];
    tableFieldecf5cbinnerActiveKey: string[];
    tableFieldecf5cbDefaultExpandAll: boolean;
    selectedtableFieldecf5cbRowKeys: any[];
    tableFielde1e6d9outerActiveKey: number[];
    tableFielde1e6d9innerActiveKey: string[];
    tableFielde1e6d9DefaultExpandAll: boolean;
    selectedtableFielde1e6d9RowKeys: any[];
    title: string;
    continueText: string;
    allList: any[];
    currIndex: number;
    isContinue: boolean;
    submitType: number;
    showContinueBtn: boolean;
    tempGoodsSelector: any[];
    pendingGoodsList: Array<{ goodsId: any; goodsName: string; goodsObj: any; quantity: number }>;
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
  const [registerQuantityModal, { openModal: openQuantityModal, setModalProps: setQuantityModalProps }] = useModal();
  const formRef = ref<FormInstance>();
  const selectModal = ref(null);
  const tableFieldecf5cbColumns: any[] = computed(() => {
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
      {
        title: '销售单价(元)',
        dataIndex: 'F_Sales_Price',
        key: 'F_Sales_Price',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '100px',
        fixed: false,
        formP: 'F_Sales_Price',
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
    list = list.filter(o => hasFormP('tableFieldecf5cb-' + o.formP));
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
  const gettableFieldecf5cbRowSelection = computed(() => {
    const rowSelection = {
      selectedRowKeys: state.selectedtableFieldecf5cbRowKeys,
      onChange: (selectedRowKeys: string[]) => {
        state.selectedtableFieldecf5cbRowKeys = (selectedRowKeys || []).sort().reverse();
      },
    };
    return rowSelection;
  });
  const tableFielde1e6d9Columns: any[] = computed(() => {
    let list = [
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
    list = list.filter(o => hasFormP('tableFielde1e6d9-' + o.formP));
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

  const gettableFielde1e6d9RowSelection = computed(() => {
    const rowSelection = {
      selectedRowKeys: state.selectedtableFielde1e6d9RowKeys,
      onChange: (selectedRowKeys: string[]) => {
        state.selectedtableFielde1e6d9RowKeys = (selectedRowKeys || []).sort().reverse();
      },
    };
    return rowSelection;
  });
  const state = reactive<State>({
    listPageSize: 20,
    listCurrentPage: 1,
    dataForm: {
      id: '',
      F_StockInNo: undefined,
      F_SupplierId: undefined,
      F_WarehouseId: [],
      F_StockInType: undefined,
      F_StockInDate: dayjs(new Date()).startOf(getDateTimeUnit('yyyy-MM-dd')).valueOf(),
      F_StockInUserId: userInfo.userId ? userInfo.userId : '',
      F_Description: undefined,
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
      tableFieldecf5cb: [],
      tableFielde1e6d9: [],
    },
    tableFieldecf5cbouterActiveKey: [0],
    tableFieldecf5cbinnerActiveKey: [],
    tableFieldecf5cbDefaultExpandAll: true,
    selectedtableFieldecf5cbRowKeys: [],
    tableFielde1e6d9outerActiveKey: [0],
    tableFielde1e6d9innerActiveKey: [],
    tableFielde1e6d9DefaultExpandAll: true,
    selectedtableFielde1e6d9RowKeys: [],
    dataRule: {
      F_WarehouseId: [
        {
          required: true,
          message: '请输入入库仓库',
          trigger: 'change',
        },
      ],
      F_StockInDate: [
        {
          required: true,
          message: '请输入入库日期',
          trigger: 'change',
        },
      ],
    },
    optionsObj: {
      F_SupplierIdProps: { label: 'F_SupplierName', value: 'F_Id' },
      F_WarehouseIdProps: {
        label: 'F_Name',
        value: 'F_Id',
        children: 'children',
      },
      F_StockInTypeProps: { label: 'fullName', value: 'enCode' },
      tableFieldecf5cb_F_CustomerIdOptions: [
        {
          value: 'F_GoodsName',
          label: '商品名称',
        },
        {
          value: 'F_GoodsNo',
          label: '商品编号',
        },
        {
          value: 'F_Type',
          label: '商品类型',
        },
        // {
        //   value: 'F_Unit',
        //   label: '单位',
        // },
        {
          value: 'F_Specification',
          label: '规格',
        },
        // {
        //   value: 'F_Image',
        //   label: '商品图片',
        // },
        {
          value: 'F_Source',
          label: '商品来源',
        },
        {
          value: 'F_Remark',
          label: '备注',
        },
      ],
      tableFieldecf5cb_F_CustomerIdTemplateJson: [],
      tableFieldecf5cb_F_WarehouseIDOptions: [],
      tableFieldecf5cb_F_WarehouseIDProps: {
        label: 'F_Name',
        value: 'F_Id',
        children: 'children',
      },
    },
    currVmodel: '',
    currTableConf: {},
    tableRows: {
      tableFieldecf5cb: {
        enabledmark: undefined,
        F_CustomerId: undefined,
        F_Encoding: undefined,
        F_Num: undefined,
        F_NumLevel1: undefined,
        F_NumLevel2: undefined,
        F_Unit_Ratio: undefined,
        F_Price: undefined,
        F_WarehouseID: [],
        F_Sales_Price: undefined,
        F_Description: undefined,
        F_CreatorUserId: undefined,
        F_CreatorTime: undefined,
      },
      tableFielde1e6d9: {
        enabledmark: undefined,
        F_Title: undefined,
        F_Content: undefined,
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
    tempGoodsSelector: [],
    pendingGoodsList: [],
    batchWarehouseValue: undefined,
  });
  const { title, continueText, showContinueBtn, dataForm, submitType, dataRule, optionsObj, tempGoodsSelector, batchWarehouseValue } = toRefs(state);

  const getPrevDisabled = computed(() => state.currIndex === 0);
  const getNextDisabled = computed(() => state.currIndex === state.allList.length - 1);

  watch(
    () => state.dataForm.tableFieldecf5cb,
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
      state.dataForm.tableFieldecf5cb = [];
      state.dataForm.tableFielde1e6d9 = [];
      state.tempGoodsSelector = [];
      state.batchWarehouseValue = undefined;
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
        F_SupplierId: undefined,
        F_WarehouseId: [],
        F_StockInType: undefined,
        F_StockInDate: dayjs(new Date()).startOf(getDateTimeUnit('yyyy-MM-dd')).valueOf(),
        F_StockInUserId: userInfo.userId ? userInfo.userId : '',
        F_Description: undefined,
        F_CreatorUserId: undefined,
        F_CreatorTime: undefined,
        tableFieldecf5cb: [],
        tableFielde1e6d9: [],
      };
      state.tempGoodsSelector = [];
      state.batchWarehouseValue = undefined;
      getF_SupplierIdOptions();
      getF_WarehouseIdOptions();
      getF_StockInTypeOptions();
      gettableFieldecf5cb_F_WarehouseIDOptions();
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
      for (let i = 0; i < state.dataForm.tableFieldecf5cb.length; i++) {
        const element = state.dataForm.tableFieldecf5cb[i];
        element.jnpfId = buildUUID();
        // 如果有单位比例，初始化数量级别
        if (element.F_Unit_Ratio) {
          fillNumLevelsFromF_Num(element);
        }
      }
      for (let i = 0; i < state.dataForm.tableFielde1e6d9.length; i++) {
        const element = state.dataForm.tableFielde1e6d9[i];
        element.jnpfId = buildUUID();
      }

      // 填充子表商品信息（用于计算库存编码前缀）
      try {
        const rows = state.dataForm.tableFieldecf5cb || [];
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

      // 同步顶部批量商品选择器的值：从已加载的子表中提取关联 id（兼容多种保存格式）
      try {
        const selectedIds = (state.dataForm.tableFieldecf5cb || [])
          .map((r: any) => {
            // 优先使用完整对象中常见 id 字段
            if (r && r._F_CustomerObj && (r._F_CustomerObj.F_Id || r._F_CustomerObj.id)) {
              return r._F_CustomerObj.F_Id ?? r._F_CustomerObj.id;
            }
            // 如果 F_CustomerId 是对象，尝试取常见 id 字段
            if (r && r.F_CustomerId && typeof r.F_CustomerId === 'object') {
              return r.F_CustomerId.F_Id ?? r.F_CustomerId.id ?? r.F_CustomerId.value;
            }
            // 否则直接返回字段值（可能是原始 id 或已解析的字符串）
            return r && r.F_CustomerId ? r.F_CustomerId : undefined;
          })
          .filter((v: any) => v !== undefined && v !== null);
        // 将提取到的 id 列表赋给顶部选择器（组件支持 id 数组或对象数组）
        state.tempGoodsSelector = selectedIds;
      } catch (e) {
        // ignore
      }
      getF_SupplierIdOptions();
      getF_WarehouseIdOptions();
      getF_StockInTypeOptions();
      gettableFieldecf5cb_F_WarehouseIDOptions();
      changeLoading(false);
    });
  }
  async function handleSubmit() {
    try {
      // 提交前：处理子表数据
      for (const row of state.dataForm.tableFieldecf5cb) {
        // 将 F_NumLevel2 的值同步到 F_Num（用于后端存储）
        if (row.F_Unit_Ratio && row.F_NumLevel2 !== undefined && row.F_NumLevel2 !== null) {
          row.F_Num = row.F_NumLevel2;
        }
        // 确保库存编码已正确拼接（前缀 + 用户输入）
        if (row._F_Encoding_Suffix !== undefined) {
          handleEncodingChange(row);
        }
      }

      // 验证子表 F_WarehouseID 是否全部填写
      const tableFieldecf5cb = state.dataForm.tableFieldecf5cb || [];
      if (tableFieldecf5cb.length > 0) {
        const emptyWarehouseRows = tableFieldecf5cb.filter((row: any) => {
          const warehouseId = row.F_WarehouseID;
          // 检查是否为空：undefined、null、空数组
          return !warehouseId || (Array.isArray(warehouseId) && warehouseId.length === 0);
        });
        if (emptyWarehouseRows.length > 0) {
          createMessage.warning(`入库商品列表中有 ${emptyWarehouseRows.length} 条数据的入库仓库未填写`);
          return;
        }
      }

      // 验证子表库存编码：后缀不能为空
      {
        const rows = state.dataForm.tableFieldecf5cb || [];
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
        const rows = state.dataForm.tableFieldecf5cb || [];
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
            createMessage.warning(`库存编码 "${code}" 重复，请检查后重试`);
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
  function getF_SupplierIdOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2008457397298925568', query).then(res => {
      let data = res.data;
      state.optionsObj.F_SupplierIdOptions = Array.isArray(data) ? data : [];
    });
  }
  function gettableFieldecf5cb_F_WarehouseIDOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2021045201115680768', query).then(res => {
      let data = res.data;
      state.optionsObj.tableFieldecf5cb_F_WarehouseIDOptions = Array.isArray(data) ? data : [];
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
  // 解析 F_Unit_Ratio，只取前两级，返回 { level1: { name, qty }, level2: { name, qty } } 或 null
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

  // 获取商品的 F_GoodsNo 作为前缀（前面拼接 F_CategoryId 解析后的值）
  function getGoodsNoPrefix(record) {
    let categoryPrefix = '';
    let goodsNo = '';

    // 获取 F_CategoryId 和 F_GoodsNo
    if (record?._F_CustomerObj) {
      categoryPrefix = parseCategoryId(record._F_CustomerObj.F_CategoryId);
      goodsNo = record._F_CustomerObj.F_GoodsNo || '';
    } else if (record?.F_CustomerId && typeof record.F_CustomerId === 'object') {
      categoryPrefix = parseCategoryId(record.F_CustomerId.F_CategoryId);
      goodsNo = record.F_CustomerId.F_GoodsNo || '';
    }

    // 拼接：F_CategoryId解析后的值 + F_GoodsNo
    return categoryPrefix + goodsNo;
  }

  // 解析 F_CategoryId：将 JSON 字符串数组解析并拼接（例如 "[\"C\", \"A1\"]" -> "CA1"）
  function parseCategoryId(categoryId) {
    if (!categoryId) return '';
    try {
      // 如果已经是字符串，尝试解析为 JSON
      const str = typeof categoryId === 'string' ? categoryId : String(categoryId);
      const arr = JSON.parse(str);
      if (Array.isArray(arr)) {
        // 将数组元素拼接成字符串
        return arr.join('');
      }
      return '';
    } catch (e) {
      // 解析失败，返回空字符串
      return '';
    }
  }

  // 处理库存编码输入变化：将前缀和用户输入拼接后存储到 F_Encoding
  function handleEncodingChange(record) {
    const prefix = getGoodsNoPrefix(record);
    const suffix = record._F_Encoding_Suffix || '';
    // 拼接前缀和用户输入
    record.F_Encoding = prefix ? prefix + suffix : suffix;
  }

  /** 回显：完整 F_Encoding 拆成前缀 + 可编辑后缀 */
  function refreshEncodingSuffixForTableRows() {
    const rows = state.dataForm.tableFieldecf5cb || [];
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

  // 处理商品选择变化：保存完整对象并更新编码
  function handleGoodsChange(record, val, obj) {
    // 保存完整商品对象
    if (val && obj && typeof obj === 'object') {
      record._F_CustomerObj = obj;
    } else {
      // 清空商品时，清空相关对象
      record._F_CustomerObj = undefined;
    }
    // 更新 F_Encoding（拼接前缀和后缀）
    handleEncodingChange(record);
  }

  // 一级数量（如箱子）改变时，按比例计算二级数量（如盒）：二级 = 一级 * level2.qty
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

  // 初始化数量级别：新增时计算默认值，回显时使用 F_Num 的值
  // F_NumLevel1 固定为1（因为总是1箱）
  function fillNumLevelsFromF_Num(row) {
    const levels = getUnitRatioLevels(row);
    if (!levels) return;
    // 如果 F_Num 有值（回显情况），使用 F_Num 的值；否则（新增情况）根据比例计算默认值
    if (row.F_Num !== undefined && row.F_Num !== null && row.F_Num !== '') {
      const qty2 = Number(row.F_Num) || 0;
      const ratio = Number(levels.level2?.qty) || 0;
      // 回显：F_Num 存的是 F_NumLevel2 的值
      row.F_NumLevel2 = qty2;
      row._F_NumLevel2Max = qty2;
      if (ratio > 0) {
        row.F_NumLevel1 = qty2 / ratio;
      } else {
        row.F_NumLevel1 = qty2;
      }
    } else {
      // 新增：根据比例计算默认值 = 1 * level2.qty
      row.F_NumLevel1 = 1;
      const ratio = Number(levels.level2?.qty) || 0;
      row.F_NumLevel2 = 1 * ratio;
      row._F_NumLevel2Max = row.F_NumLevel2;
      row.F_Num = row.F_NumLevel2;
    }
  }

  function getF_StockInTypeOptions() {
    getDictionaryDataSelector('2012074650393251840').then(res => {
      state.optionsObj.F_StockInTypeOptions = res.data.list;
    });
  }
  function addtableFieldecf5cbRow() {
    let item = {
      jnpfId: buildUUID(),
      F_CustomerId: undefined,
      F_Num: undefined,
      F_NumLevel1: undefined,
      F_NumLevel2: undefined,
      F_Unit_Ratio: undefined,
      F_Encoding: undefined,
      _F_Encoding_Suffix: undefined,
      F_Price: undefined,
      F_Sales_Price: undefined,
      F_Description: undefined,
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
    };
    state.dataForm.tableFieldecf5cb.push(item);
    state.tableFieldecf5cbinnerActiveKey.push(item.jnpfId);
  }
  function removetableFieldecf5cbRow(index, showConfirm = false) {
    const doRemove = () => {
      state.dataForm.tableFieldecf5cb.splice(index, 1);
      // 同步更新顶部选择器：留下的行对应的 relation id 列表
      try {
        const remainingIds = (state.dataForm.tableFieldecf5cb || [])
          .map((r: any) => {
            // 优先使用我们保存的完整对象
            if (r && r._F_CustomerObj && (r._F_CustomerObj.F_Id || r._F_CustomerObj.id)) {
              return r._F_CustomerObj.F_Id ?? r._F_CustomerObj.id;
            }
            // 如果 F_CustomerId 是对象，尝试取常见 id 字段
            if (r && r.F_CustomerId && typeof r.F_CustomerId === 'object') {
              return r.F_CustomerId.F_Id ?? r.F_CustomerId.id ?? r.F_CustomerId.value;
            }
            return r && r.F_CustomerId ? r.F_CustomerId : undefined;
          })
          .filter((v: any) => v !== undefined && v !== null);
        state.tempGoodsSelector = remainingIds;
      } catch (e) {
        // ignore
      }
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
  function copytableFieldecf5cbRow(index) {
    let item = cloneDeep(state.dataForm.tableFieldecf5cb[index]);
    let copyData = {};
    for (let i = 0; i < unref(tableFieldecf5cbColumns).length; i++) {
      const cur = unref(tableFieldecf5cbColumns)[i];
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
    state.dataForm.tableFieldecf5cb.push(copyItem);
    state.tableFieldecf5cbinnerActiveKey.push(copyItem.jnpfId);
  }
  function batchRemovetableFieldecf5cbRow(showConfirm = false) {
    if (!state.selectedtableFieldecf5cbRowKeys.length) return createMessage.error('请选择一条数据');
    const handleBatchRemove = () => {
      state.dataForm.tableFieldecf5cb = state.dataForm.tableFieldecf5cb.filter(o => !state.selectedtableFieldecf5cbRowKeys.includes(o.jnpfId));
      nextTick(() => {
        state.selectedtableFieldecf5cbRowKeys = [];
        // 更新顶部选择器值，保留剩余行的 relation id
        try {
          const remainingIds = (state.dataForm.tableFieldecf5cb || [])
            .map((r: any) => {
              if (r && r._F_CustomerObj && (r._F_CustomerObj.F_Id || r._F_CustomerObj.id)) {
                return r._F_CustomerObj.F_Id ?? r._F_CustomerObj.id;
              }
              if (r && r.F_CustomerId && typeof r.F_CustomerId === 'object') {
                return r.F_CustomerId.F_Id ?? r.F_CustomerId.id ?? r.F_CustomerId.value;
              }
              return r && r.F_CustomerId ? r.F_CustomerId : undefined;
            })
            .filter((v: any) => v !== undefined && v !== null);
          state.tempGoodsSelector = remainingIds;
        } catch (e) {
          // ignore
        }
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
  function addtableFielde1e6d9Row() {
    let item = {
      jnpfId: buildUUID(),
      F_Title: undefined,
      F_Content: undefined,
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
    };
    state.dataForm.tableFielde1e6d9.push(item);
    state.tableFielde1e6d9innerActiveKey.push(item.jnpfId);
  }
  function removetableFielde1e6d9Row(index, showConfirm = false) {
    if (showConfirm) {
      createConfirm({
        iconType: 'warning',
        title: '提示',
        content: '此操作将永久删除该数据, 是否继续?',
        onOk: () => {
          state.dataForm.tableFielde1e6d9.splice(index, 1);
        },
      });
    } else {
      state.dataForm.tableFielde1e6d9.splice(index, 1);
    }
  }
  function copytableFielde1e6d9Row(index) {
    let item = cloneDeep(state.dataForm.tableFielde1e6d9[index]);
    let copyData = {};
    for (let i = 0; i < unref(tableFielde1e6d9Columns).length; i++) {
      const cur = unref(tableFielde1e6d9Columns)[i];
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
    state.dataForm.tableFielde1e6d9.push(copyItem);
    state.tableFielde1e6d9innerActiveKey.push(copyItem.jnpfId);
  }
  function batchRemovetableFielde1e6d9Row(showConfirm = false) {
    if (!state.selectedtableFielde1e6d9RowKeys.length) return createMessage.error('请选择一条数据');
    const handleBatchRemove = () => {
      state.dataForm.tableFielde1e6d9 = state.dataForm.tableFielde1e6d9.filter(o => !state.selectedtableFielde1e6d9RowKeys.includes(o.jnpfId));
      nextTick(() => {
        state.selectedtableFielde1e6d9RowKeys = [];
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
      // 如果有单位比例，初始化数量级别
      if (item.F_Unit_Ratio && state.currVmodel === 'tableFieldecf5cb') {
        fillNumLevelsFromF_Num(item);
      }
      state.dataForm[state.currVmodel].push(cloneDeep(item));
      if (state[state.currVmodel + 'DefaultExpandAll']) state[state.currVmodel + 'innerActiveKey'].push(item.jnpfId);
    }
  }
  // 数量设置弹窗的列定义
  const quantityModalColumns = [
    {
      title: '商品名称',
      dataIndex: 'goodsName',
      key: 'goodsName',
      width: '60%',
    },
    {
      title: '数量',
      dataIndex: 'quantity',
      key: 'quantity',
      width: '40%',
    },
  ];

  // 批量商品选择处理方法 - 修改为打开数量设置弹窗
  function handleGoodsSelectorChange(selected: any) {
    if (!selected) {
      state.tempGoodsSelector = [];
      state.pendingGoodsList = [];
      return;
    }

    const interfaceId = '2008721559174385664';

    // Helper to detect whether selection items are objects (already contain fields)
    const itemsAreObjects = (arr: any[]) => {
      if (!arr || !arr.length) return false;
      const first = arr[0];
      return first && typeof first === 'object' && (first.F_Id || first.F_GoodsName || first.F_GoodsNo);
    };

    const processGoodsData = (goodsList: any[]) => {
      // 辅助函数：从子表行中提取商品ID
      const getGoodsIdFromRow = (row: any): string | null => {
        if (!row) return null;
        // 优先使用保存的完整对象
        if (row._F_CustomerObj && (row._F_CustomerObj.F_Id || row._F_CustomerObj.id)) {
          return String(row._F_CustomerObj.F_Id ?? row._F_CustomerObj.id);
        }
        // 如果 F_CustomerId 是对象，尝试取常见 id 字段
        if (row.F_CustomerId && typeof row.F_CustomerId === 'object') {
          const id = row.F_CustomerId.F_Id ?? row.F_CustomerId.id ?? row.F_CustomerId.value;
          return id ? String(id) : null;
        }
        // 否则直接返回字段值（可能是原始 id）
        return row.F_CustomerId ? String(row.F_CustomerId) : null;
      };

      // 准备待处理的商品列表，统计子表中已有的数量
      state.pendingGoodsList = goodsList.map((goods: any) => {
        let goodsId: any = goods;
        let goodsName = '未知商品';
        let goodsObj: any = null;

        if (goods && typeof goods === 'object') {
          goodsObj = goods;
          goodsId = goods.F_Id ?? goods.FId ?? goods.id ?? goods.value ?? goodsId;
          goodsName = goods.F_GoodsName ?? goods.FGoodsName ?? goods.goodsName ?? goods.name ?? goodsName;
        }

        // 统计子表中该商品已有的数量
        const targetGoodsId = String(goodsId);
        let existingQuantity = 0;
        if (state.dataForm.tableFieldecf5cb && Array.isArray(state.dataForm.tableFieldecf5cb)) {
          existingQuantity = state.dataForm.tableFieldecf5cb.filter((row: any) => {
            const rowGoodsId = getGoodsIdFromRow(row);
            return rowGoodsId && rowGoodsId === targetGoodsId;
          }).length;
        }

        return {
          goodsId,
          goodsName,
          goodsObj,
          quantity: existingQuantity > 0 ? existingQuantity : 1, // 如果子表中已有，使用已有数量，否则默认为1
        };
      });

      // 打开数量设置弹窗
      openQuantityModal();
    };

    if (Array.isArray(selected)) {
      // 多选：可能是对象数组（完整数据）或 id 数组（需要拉取）
      if (itemsAreObjects(selected)) {
        processGoodsData(selected);
        state.tempGoodsSelector = selected.slice();
      } else {
        // selected 是 id 数组，按 id 拉取完整数据
        const ids = Array.from(new Set(selected.map((s: any) => String(s))));
        const query = {
          ids,
          interfaceId,
          propsValue: 'F_Id',
          relationField: 'F_GoodsName',
          paramList: [],
        };
        getDataInterfaceDataInfoByIds(interfaceId, query)
          .then(resp => {
            const list = (resp && resp.data) || [];
            const map: Record<string, any> = {};
            list.forEach((item: any) => {
              if (item && item.F_Id != null) map[String(item.F_Id)] = item;
            });
            // 按原始顺序构建商品列表
            const goodsList = ids.map((id: string) => map[String(id)] || { F_Id: id, F_GoodsName: `商品ID: ${id}` });
            processGoodsData(goodsList);
            state.tempGoodsSelector = ids.slice();
          })
          .catch(err => {
            console.error('getDataInterfaceDataInfoByIds error', err);
            try {
              createMessage.error(typeof err === 'object' ? JSON.stringify(err) : String(err));
            } catch (e) {
              // ignore
            }
            // 拉取失败时，使用 id 作为商品信息
            const goodsList = ids.map((id: string) => ({ F_Id: id, F_GoodsName: `商品ID: ${id}` }));
            processGoodsData(goodsList);
            state.tempGoodsSelector = selected.slice();
          });
      }
    } else {
      // 单选：可能是对象或单个 id
      const single = selected;
      if (single && typeof single === 'object') {
        processGoodsData([single]);
        state.tempGoodsSelector = [single];
      } else {
        const id = String(single);
        const query = {
          ids: [id],
          interfaceId,
          propsValue: 'F_Id',
          relationField: 'F_GoodsName',
          paramList: [],
        };
        getDataInterfaceDataInfoByIds(interfaceId, query)
          .then(resp => {
            const found = (resp && resp.data && resp.data[0]) || { F_Id: id, F_GoodsName: `商品ID: ${id}` };
            processGoodsData([found]);
            state.tempGoodsSelector = [found];
          })
          .catch(err => {
            console.error('getDataInterfaceDataInfoByIds error', err);
            try {
              createMessage.error(typeof err === 'object' ? JSON.stringify(err) : String(err));
            } catch (e) {
              // ignore
            }
            processGoodsData([{ F_Id: id, F_GoodsName: `商品ID: ${id}` }]);
            state.tempGoodsSelector = [id];
          });
      }
    }
  }

  // 数量设置弹窗确认处理
  async function handleQuantityConfirm() {
    if (!state.pendingGoodsList || state.pendingGoodsList.length === 0) {
      createMessage.warning('请至少选择一个商品');
      return;
    }

    // 验证数量
    const invalidItems = state.pendingGoodsList.filter(item => !item.quantity || item.quantity < 1);
    if (invalidItems.length > 0) {
      createMessage.warning('请为所有商品设置有效的数量（至少为1）');
      return;
    }

    // 辅助函数：从子表行中提取商品ID
    const getGoodsIdFromRow = (row: any): string | null => {
      if (!row) return null;
      // 优先使用保存的完整对象
      if (row._F_CustomerObj && (row._F_CustomerObj.F_Id || row._F_CustomerObj.id)) {
        return String(row._F_CustomerObj.F_Id ?? row._F_CustomerObj.id);
      }
      // 如果 F_CustomerId 是对象，尝试取常见 id 字段
      if (row.F_CustomerId && typeof row.F_CustomerId === 'object') {
        const id = row.F_CustomerId.F_Id ?? row.F_CustomerId.id ?? row.F_CustomerId.value;
        return id ? String(id) : null;
      }
      // 否则直接返回字段值（可能是原始 id）
      return row.F_CustomerId ? String(row.F_CustomerId) : null;
    };

    // 收集所有需要新增的行数据
    const allNewRows: any[] = [];

    // 根据数量添加商品行到子表
    state.pendingGoodsList.forEach((item: any) => {
      const targetQuantity = item.quantity || 1;
      // 确定关联ID和完整对象
      let relationId: any = item.goodsId;
      let fullObj: any = item.goodsObj;

      if (item.goodsObj && typeof item.goodsObj === 'object') {
        fullObj = item.goodsObj;
        relationId = item.goodsObj.F_Id ?? item.goodsObj.FId ?? item.goodsObj.id ?? item.goodsObj.value ?? relationId;
      }

      // 检查子表中该商品已有的数量
      const targetGoodsId = String(relationId);
      let existingQuantity = 0;
      if (state.dataForm.tableFieldecf5cb && Array.isArray(state.dataForm.tableFieldecf5cb)) {
        existingQuantity = state.dataForm.tableFieldecf5cb.filter((row: any) => {
          const rowGoodsId = getGoodsIdFromRow(row);
          return rowGoodsId && rowGoodsId === targetGoodsId;
        }).length;
      }

      // 计算需要新增的数量 = 目标数量 - 已有数量
      const needToAdd = targetQuantity - existingQuantity;

      // 如果不需要新增，跳过
      if (needToAdd <= 0) {
        return;
      }

      // 只新增差额部分
      for (let i = 0; i < needToAdd; i++) {
        allNewRows.push({
          item,
          relationId,
          fullObj,
        });
      }
    });

    // 按 relationId 分组
    const relationIdMap = new Map<string, any[]>();
    allNewRows.forEach(row => {
      const key = String(row.relationId);
      if (!relationIdMap.has(key)) {
        relationIdMap.set(key, []);
      }
      relationIdMap.get(key)!.push(row);
    });

    // 对每个 relationId 调用一次 getMaxCode
    const relationIdCodeMap = new Map<string, number>();
    const promises: Promise<void>[] = [];

    for (const [key, rows] of relationIdMap) {
      // 获取该组的 goodsId（所有 item 的 goodsId 相同）
      const goodsId = rows[0]?.item?.goodsId;
      if (goodsId) {
        const promise = getMaxCode(goodsId).then(res => {
          const baseCode = res?.data || 0;
          relationIdCodeMap.set(key, Number(baseCode));
        });
        promises.push(promise);
      }
    }

    // 等待所有 getMaxCode 调用完成
    await Promise.all(promises);

    // 设置编码后缀并创建行
    const relationIdCounter = new Map<string, number>();

    for (const row of allNewRows) {
      const { item, relationId, fullObj } = row;
      const key = String(relationId);
      const baseCode = relationIdCodeMap.get(key) || 0;

      // 获取当前计数器
      if (!relationIdCounter.has(key)) {
        relationIdCounter.set(key, baseCode);
      }
      const currentCode = relationIdCounter.get(key)!;
      relationIdCounter.set(key, currentCode + 1);

      const newRow: any = {
        jnpfId: buildUUID(),
        F_CustomerId: relationId,
        _F_CustomerObj: fullObj,
        F_Num: undefined,
        F_NumLevel1: undefined,
        F_NumLevel2: undefined,
        F_Unit_Ratio: fullObj?.F_Unit_Ratio ?? undefined,
        F_Encoding: undefined,
        _F_Encoding_Suffix: String(currentCode),
        F_Price: undefined,
        F_Sales_Price: undefined,
        F_Description: undefined,
        F_CreatorUserId: undefined,
        F_CreatorTime: undefined,
      };
      // 如果有单位比例，初始化数量级别
      if (newRow.F_Unit_Ratio) {
        fillNumLevelsFromF_Num(newRow);
      }
      state.dataForm.tableFieldecf5cb.push(newRow);
    }

    // 更新顶部选择器的值
    const selectedIds = state.pendingGoodsList
      .map((item: any) => {
        if (item.goodsObj && typeof item.goodsObj === 'object') {
          return item.goodsObj.F_Id ?? item.goodsObj.FId ?? item.goodsObj.id ?? item.goodsObj.value;
        }
        return item.goodsId;
      })
      .filter((v: any) => v !== undefined && v !== null);
    state.tempGoodsSelector = selectedIds;

    // 关闭弹窗并清空待处理列表
    setQuantityModalProps({ open: false });
    state.pendingGoodsList = [];
  }

  // 批量赋值仓库给选中的子表行
  function handleBatchAssignWarehouse() {
    if (!state.batchWarehouseValue) {
      createMessage.warning('请先选择入库仓库');
      return;
    }
    if (!state.selectedtableFieldecf5cbRowKeys.length) {
      createMessage.warning('请先勾选要赋值的子表数据');
      return;
    }
    const selectedKeys = state.selectedtableFieldecf5cbRowKeys;
    let assignCount = 0;
    for (const row of state.dataForm.tableFieldecf5cb) {
      if (selectedKeys.includes(row.jnpfId)) {
        row.F_WarehouseID = [...state.batchWarehouseValue];
        assignCount++;
      }
    }
    createMessage.success(`已成功赋值 ${assignCount} 条数据`);
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
      state.listCurrentPage = 1;
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
