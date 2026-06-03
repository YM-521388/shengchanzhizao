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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_ReturnOutNo')">
            <a-form-item name="F_ReturnOutNo" :labelCol="{ style: { width: '100px' } }">
              <template #label>采购退货单号</template>
              <JnpfInput
                v-model:value="dataForm.F_ReturnOutNo"
                placeholder="请填写，忽略将自动生成"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>

          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_ReturnOutType')">
            <a-form-item name="F_ReturnOutType" :labelCol="{ style: { width: '100px' } }">
              <template #label>出库类型</template>
              <JnpfSelect
                v-model:value="dataForm.F_ReturnOutType"
                placeholder="请选择"
                :options="optionsObj.F_ReturnOutTypeOptions"
                :fieldNames="optionsObj.F_ReturnOutTypeProps"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_OrderId')">
            <a-form-item name="F_OrderId" :labelCol="{ style: { width: '100px' } }">
              <template #label>采购单号</template>
              <JnpfPopupSelect
                v-model:value="dataForm.F_OrderId"
                placeholder="请选择"
                :templateJson="optionsObj.F_OrderIdTemplateJson"
                allowClear
                field="F_OrderId"
                interfaceId="2011984543933927424"
                :columnOptions="optionsObj.F_OrderIdOptions"
                relationField="F_OrderNo"
                propsValue="F_Id"
                :pageSize="20"
                popupType="dialog"
                popupTitle="选择数据"
                popupWidth="70%"
                hasPage
                :disabled="!!dataForm.id"
                :style="{
                  width: '100%',
                }"
                :extraOptions="optionsObj.F_OrderId" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_ReturnOutDate')">
            <a-form-item name="F_ReturnOutDate" :labelCol="{ style: { width: '100px' } }">
              <template #label>退货日期</template>
              <JnpfDatePicker
                v-model:value="dataForm.F_ReturnOutDate"
                placeholder="请选择"
                format="yyyy-MM-dd"
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
                  minRows: 1,
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
              <JnpfGroupTitle content="商品" :bordered="false" />
              <a-row :gutter="15">
                <a-col :span="16" class="ant-col-item">
                  <a-form-item :labelCol="{ style: { width: '100px' } }">
                    <template #label>库存商品选择</template>
                    <JnpfPopupSelect
                      v-model:value="tempGoodsSelector"
                      :formData="state.dataForm"
                      placeholder="请先选择采购单号，再进行商品选择"
                      :templateJson="optionsObj.tableFieldf5c8ef_F_CustomerIdTemplateJson"
                      allowClear
                      :field="'tempGoodsSelector'"
                      :interfaceId="state.goodsInterfaceId"
                      :columnOptions="optionsObj.tableFieldf5c8ef_F_CustomerIdOptions"
                      relationField="F_GoodsName"
                      propsValue="F_Code"
                      :pageSize="20"
                      :disabled="state.dataForm.id"
                      popupType="dialog"
                      popupTitle="选择数据"
                      popupWidth="50%"
                      hasPage
                      multiple
                      :style="{
                        width: '100%',
                      }"
                      :extraOptions="optionsObj.tableFieldf5c8ef_F_CustomerId"
                      @change="(ids, rows) => handleGoodsSelectorChange(ids, rows)" />
                  </a-form-item>
                </a-col>
                <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_WarehouseId')">
                  <a-form-item name="F_WarehouseId" :labelCol="{ style: { width: '100px' } }">
                    <template #label>仓库</template>
                    <JnpfCascader
                      v-model:value="dataForm.F_WarehouseId"
                      placeholder="请选择"
                      :options="optionsObj.F_WarehouseIdOptions"
                      :fieldNames="optionsObj.F_WarehouseIdProps"
                      allowClear
                      :disabled="!!state.dataForm.id"
                      :changeOnSelect="true"
                      showSearch
                      :style="{
                        width: '100%',
                      }" />
                  </a-form-item>
                </a-col>
              </a-row>

              <a-table
                :data-source="dataForm.tableFieldf5c8ef"
                :columns="tableFieldf5c8efColumns"
                size="small"
                rowKey="jnpfId"
                :pagination="listPagination"
                :scroll="{ x: 'max-content' }"
                :rowSelection="gettableFieldf5c8efRowSelection">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                  <template v-if="column.key === 'F_CustomerId'">
                    <JnpfPopupSelect
                      v-model:value="record.F_CustomerId"
                      :formData="state.dataForm"
                      :rowIndex="index"
                      placeholder="请选择"
                      :templateJson="optionsObj.tableFieldf5c8ef_F_CustomerIdTemplateJson"
                      allowClear
                      disabled
                      :field="'F_CustomerId' + index"
                      interfaceId="2036693599021830144"
                      :columnOptions="optionsObj.tableFieldf5c8ef_F_CustomerIdOptions"
                      relationField="F_GoodsName"
                      propsValue="F_GoodsId"
                      :pageSize="20"
                      popupType="dialog"
                      popupTitle="选择数据"
                      popupWidth="50%"
                      hasPage
                      :style="{
                        width: '100%',
                      }"
                      :extraOptions="optionsObj.tableFieldf5c8ef_F_CustomerId" />
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
                  <template v-if="column.key === 'F_InventoryNum'">
                    <span>{{ record.F_InventoryNum ?? '-' }}</span>
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
                      placeholder=""
                      :options="optionsObj.tableFieldf5c8ef_F_WarehouseIDOptions"
                      :fieldNames="optionsObj.tableFieldf5c8ef_F_WarehouseIDProps"
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
                  <template v-if="column.key === 'action'">
                    <a-space>
                      <!-- <a-button class="action-btn" type="link" @click="copytableFieldf5c8efRow(index)" size="small">{{
                        t('common.copyText', '复制')
                      }}</a-button> -->
                      <a-button class="action-btn" type="link" color="error" @click="removetableFieldf5c8efRow(index, true)" size="small">{{
                        t('common.delText', '删除')
                      }}</a-button>
                    </a-space>
                  </template>
                </template>
              </a-table>
              <a-space class="input-table-footer-btn">
                <!-- <a-button type="primary" preIcon="icon-ym icon-ym-btn-add" @click="addtableFieldf5c8efRow">{{ t('common.add1Text', '添加') }}</a-button>
                <a-button type="danger" preIcon="icon-ym icon-ym-btn-clearn" @click="batchRemovetableFieldf5c8efRow(true)">{{
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
  import { getOrderItems } from './helper/api';
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
  import { findPath } from '@/utils/helper/treeHelper';

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
    tableFieldf5c8efouterActiveKey: number[];
    tableFieldf5c8efinnerActiveKey: string[];
    tableFieldf5c8efDefaultExpandAll: boolean;
    selectedtableFieldf5c8efRowKeys: any[];
    tableField3ff267outerActiveKey: number[];
    tableField3ff267innerActiveKey: string[];
    tableField3ff267DefaultExpandAll: boolean;
    selectedtableField3ff267RowKeys: any[];
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
  const tableFieldf5c8efColumns: any[] = computed(() => {
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
        noPermission: true,
      },
      {
        title: '退货数量',
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
      // {
      //   title: '成本单价(元)',
      //   dataIndex: 'F_Price',
      //   key: 'F_Price',
      //   tipLabel: '',
      //   required: false,
      //   align: 'left',
      //   span: '24',
      //   labelWidth: '',
      //   width: '',
      //   fixed: false,
      //   formP: 'F_Price',
      //   isSystemControl: false,
      // },
      {
        title: '入库仓库',
        dataIndex: 'F_WarehouseID',
        key: 'F_WarehouseID',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
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
    ];
    list = list.filter(o => hasFormP('tableFieldf5c8ef-' + o.formP));
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
  const gettableFieldf5c8efRowSelection = computed(() => {
    const rowSelection = {
      selectedRowKeys: state.selectedtableFieldf5c8efRowKeys,
      onChange: (selectedRowKeys: string[]) => {
        state.selectedtableFieldf5c8efRowKeys = (selectedRowKeys || []).sort().reverse();
      },
    };
    return rowSelection;
  });
  const tableField3ff267Columns: any[] = computed(() => {
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
    list = list.filter(o => hasFormP('tableField3ff267-' + o.formP));
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
  const gettableField3ff267RowSelection = computed(() => {
    const rowSelection = {
      selectedRowKeys: state.selectedtableField3ff267RowKeys,
      onChange: (selectedRowKeys: string[]) => {
        state.selectedtableField3ff267RowKeys = (selectedRowKeys || []).sort().reverse();
      },
    };
    return rowSelection;
  });
  const state = reactive<State>({
    listPageSize: 20,
    listCurrentPage: 1,
    dataForm: {
      id: '',
      F_ReturnOutNo: undefined,
      F_WarehouseId: [],
      F_ReturnOutType: undefined,
      F_OrderId: undefined,
      F_ReturnOutDate: dayjs(new Date()).startOf(getDateTimeUnit('yyyy-MM-dd')).valueOf(),
      F_Description: undefined,
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
      tableFieldf5c8ef: [],
      tableField3ff267: [],
    },
    tableFieldf5c8efouterActiveKey: [0],
    tableFieldf5c8efinnerActiveKey: [],
    tableFieldf5c8efDefaultExpandAll: true,
    selectedtableFieldf5c8efRowKeys: [],
    tableField3ff267outerActiveKey: [0],
    tableField3ff267innerActiveKey: [],
    tableField3ff267DefaultExpandAll: true,
    selectedtableField3ff267RowKeys: [],
    dataRule: {
      // F_WarehouseId: [
      //   {
      //     required: true,
      //     message: '请输入仓库',
      //     trigger: 'change',
      //   },
      // ],
      F_OrderId: [
        {
          required: true,
          message: '请输入采购单号',
          trigger: 'change',
        },
      ],
      F_ReturnOutDate: [
        {
          required: true,
          message: '请输入退货日期',
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
      F_ReturnOutTypeProps: { label: 'fullName', value: 'enCode' },
      F_OrderIdOptions: [
        {
          value: 'F_OrderNo',
          label: '采购编号',
        },
        {
          value: 'F_SupplierId',
          label: '供应商',
        },
        {
          value: 'F_ProdPlanId',
          label: '生产计划',
        },
        {
          value: 'F_Money',
          label: '商品金额',
        },
        {
          value: 'F_PurchaseNum',
          label: '采购数量',
        },
        {
          value: 'F_DeliveryDate',
          label: '交货日期',
        },
        {
          value: 'F_Description',
          label: '备注',
        },
      ],
      F_OrderIdTemplateJson: [],
      tableFieldf5c8ef_F_CustomerIdOptions: [
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
          label: '储存仓库',
        },
      ],
      tableFieldf5c8ef_F_CustomerIdTemplateJson: [
        {
          defaultValue: '',
          field: 'F_OrderId',
          dataType: 'varchar',
          required: 0,
          fieldName: '采购单单号',
          relationField: 'F_OrderId',
          jnpfKey: null,
          complexHeaderList: null,
          sourceType: 2,
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
      tableFieldf5c8ef_F_WarehouseIDOptions: [],
      tableFieldf5c8ef_F_WarehouseIDProps: {
        label: 'F_Name',
        value: 'F_Id',
        children: 'children',
      },
      F_OrderId: [],
    },
    currVmodel: '',
    currTableConf: {},
    tableRows: {
      tableFieldf5c8ef: {
        enabledmark: undefined,
        F_CustomerId: undefined,
        F_InventoryNum: undefined,
        F_Encoding: undefined,
        F_Num: undefined,
        F_NumLevel1: undefined,
        F_NumLevel2: undefined,
        _F_NumLevel2Max: undefined,
        F_Unit_Ratio: undefined,
        F_Price: undefined,
        F_WarehouseID: [],
        F_Description: undefined,
        F_CreatorUserId: undefined,
        F_CreatorTime: undefined,
      },
      tableField3ff267: {
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
    tempGoodsSelector: [],
    goodsInterfaceId: '2036693599021830144',
  });
  const { title, continueText, showContinueBtn, dataForm, submitType, dataRule, optionsObj, tempGoodsSelector } = toRefs(state);

  const getPrevDisabled = computed(() => state.currIndex === 0);
  const getNextDisabled = computed(() => state.currIndex === state.allList.length - 1);

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
        return [String(parsed)];
      } catch {
        return [s];
      }
    }
    return [String(raw)];
  }

  function resolveSubTableGoodsId(data: any): any {
    if (data == null || typeof data !== 'object') return data;
    if (data.F_GoodsId != null && data.F_GoodsId !== '') return data.F_GoodsId;
    return data.F_Id ?? data.FId ?? data.id ?? data.value;
  }

  /** 弹窗确定时 emit(change, value, selectRow)：优先用第二参完整行，才能带上 F_Code / F_GoodsName */
  function normalizePickerFullRows(_selected: any, pickerRows: any): any[] | null {
    if (Array.isArray(pickerRows) && pickerRows.length) {
      const first = pickerRows[0];
      if (first && typeof first === 'object' && (first.F_Id != null || first.F_GoodsId != null || first.F_GoodsName || first.F_Code != null)) {
        return pickerRows;
      }
    }
    if (pickerRows && typeof pickerRows === 'object' && !Array.isArray(pickerRows)) {
      if (pickerRows.F_Id != null || pickerRows.F_GoodsId != null || pickerRows.F_Code != null) return [pickerRows];
    }
    return null;
  }

  function pickerItemsAreFullRows(arr: any[]) {
    if (!arr || !arr.length) return false;
    const first = arr[0];
    return first && typeof first === 'object' && (first.F_Id || first.F_GoodsId || first.F_GoodsName || first.F_Code);
  }

  function handleGoodsSelectorChange(selected: any, pickerRows?: any) {
    state.dataForm.tableFieldf5c8ef = [];
    state.selectedtableFieldf5c8efRowKeys = [];

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
      state.dataForm.tableFieldf5c8ef.push(newRow);
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
      const ids = Array.from(new Set(selected.map((s: any) => String(s))));
      const query = {
        ids: ids,
        interfaceId: state.goodsInterfaceId,
        propsValue: 'F_GoodsId',
        relationField: 'F_GoodsName',
        paramList: [],
      };
      getDataInterfaceDataInfoByIds(state.goodsInterfaceId, query)
        .then(resp => {
          const list = parseInfoByIdsResponse(resp);
          const map: Record<string, any> = {};
          list.forEach((item: any) => {
            if (item && item.F_GoodsId != null) map[String(item.F_GoodsId)] = item;
          });
          for (let i = 0; i < ids.length; i++) {
            const id = ids[i];
            buildAndPushRow(map[String(id)] || id);
          }
          state.tempGoodsSelector = [];
        })
        .catch(err => {
          console.error('getDataInterfaceDataInfoByIds error', err);
          try {
            createMessage.error(typeof err === 'object' ? JSON.stringify(err) : String(err));
          } catch (e) {
            // ignore
          }
          ids.forEach((c: any) => buildAndPushRow(c));
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
    const id = String(single);
    const querySingle = {
      ids: [id],
      interfaceId: state.goodsInterfaceId,
      propsValue: 'F_GoodsId',
      relationField: 'F_GoodsName',
      paramList: [],
    };
    getDataInterfaceDataInfoByIds(state.goodsInterfaceId, querySingle)
      .then(resp => {
        const list = parseInfoByIdsResponse(resp);
        buildAndPushRow(list[0] || id);
        state.tempGoodsSelector = [];
      })
      .catch(() => {
        buildAndPushRow(id);
        state.tempGoodsSelector = [];
      });
  }

  /** 批量选择按 F_GoodsId 区分；删除子表行时从 temp 中移除 */
  function removeFromTempGoodsSelectorByIds(ids: any[]) {
    if (!ids || !ids.length) return;
    const keySet = new Set(ids.map(c => String(c)));
    const filterFn = (item: any) => {
      let id: any = item;
      if (item && typeof item === 'object') {
        id = item.F_GoodsId ?? item.F_Id ?? null;
      }
      return id == null || id === '' || !keySet.has(String(id));
    };
    state.tempGoodsSelector = (state.tempGoodsSelector || []).filter(filterFn);
  }
  // 监听
  watch(
    () => state.dataForm.tableFieldf5c8ef,
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

  // 监听仓库变化，动态更新商品弹窗接口参数中的仓库字段
  watch(
    () => state.dataForm.F_WarehouseId,
    val => {
      const warehouseIdArr = Array.isArray(val) ? val : val ? [val] : [];
      const target = state.optionsObj.tableFieldf5c8ef_F_CustomerIdTemplateJson.find((t: any) => t.field === 'F_WarehouseId');
      if (target) {
        target.relationField = warehouseIdArr.length ? JSON.stringify(warehouseIdArr) : '';
      }
    },
    { immediate: true },
  );

  // 监听采购单变化，动态更新商品弹窗接口参数中的采购单字段
  watch(
    () => state.dataForm.F_OrderId,
    val => {
      const orderId = val || '';
      const target = state.optionsObj.tableFieldf5c8ef_F_CustomerIdTemplateJson.find((t: any) => t.field === 'F_OrderId');
      if (target) {
        target.relationField = orderId;
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
    openModal();
    nextTick(() => {
      getForm().resetFields();
      state.dataForm.tableFieldf5c8ef = [];
      state.dataForm.tableField3ff267 = [];
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
        F_ReturnOutNo: undefined,
        F_WarehouseId: [],
        F_ReturnOutType: undefined,
        F_OrderId: undefined,
        F_ReturnOutDate: dayjs(new Date()).startOf(getDateTimeUnit('yyyy-MM-dd')).valueOf(),
        F_Description: undefined,
        F_CreatorUserId: undefined,
        F_CreatorTime: undefined,
        tableFieldf5c8ef: [],
        tableField3ff267: [],
      };
      getF_WarehouseIdOptions();
      getF_ReturnOutTypeOptions();
      gettableFieldf5c8ef_F_WarehouseIDOptions();
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
      // 先获取仓库选项，再转换 ID 为路径
      await gettableFieldf5c8ef_F_WarehouseIDOptions();
      for (let i = 0; i < state.dataForm.tableFieldf5c8ef.length; i++) {
        const element = state.dataForm.tableFieldf5c8ef[i];
        element.jnpfId = buildUUID();
        // 编辑模式下将仓库 ID 转换为完整路径（级联选择器需要路径数组）
        element.F_WarehouseID = resolveWarehousePath(element.F_WarehouseID);
      }
      for (let i = 0; i < state.dataForm.tableField3ff267.length; i++) {
        const element = state.dataForm.tableField3ff267[i];
        element.jnpfId = buildUUID();
      }
      // 编辑模式下获取商品详情（含 F_Unit_Ratio）用于数量层级显示
      await enrichTableRowsGoodsMeta(state.dataForm.tableFieldf5c8ef);
      // 根据 F_Num 填充数量层级
      for (const row of state.dataForm.tableFieldf5c8ef) {
        fillNumLevelsFromF_Num(row);
      }
      getF_WarehouseIdOptions();
      getF_ReturnOutTypeOptions();
      changeLoading(false);
    });
  }

  /** 根据仓库 ID 查找完整路径（级联选择器需要路径数组） */
  function resolveWarehousePath(rawId: any): string[] {
    if (rawId == null || rawId === '') return [];
    // 如果已经是数组且长度大于1，可能是完整路径
    if (Array.isArray(rawId)) {
      if (rawId.length > 1) return rawId.map((x: any) => String(x));
      // 单元素数组，需要查找路径
      rawId = rawId[0];
    }
    const idStr = String(rawId);
    const options = state.optionsObj.tableFieldf5c8ef_F_WarehouseIDOptions || [];
    const path = findPath(options, (node: any) => node.F_Id === idStr, { children: 'children' });
    if (path && path.length) {
      return path.map((n: any) => n.F_Id);
    }
    // 找不到路径，返回原 ID
    return [idStr];
  }

  // 解析 F_Unit_Ratio（与 aPuReturnPrd 一致）：两级单位 { level1: { name, qty }, level2: { name, qty } }
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

  /** 编辑模式下获取商品详情（含 F_Unit_Ratio）用于数量层级显示 */
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
  function getF_ReturnOutTypeOptions() {
    getDictionaryDataSelector('2013096194263355392').then(res => {
      state.optionsObj.F_ReturnOutTypeOptions = res.data.list;
    });
  }
  function gettableFieldf5c8ef_F_WarehouseIDOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    return getDataInterfaceRes('2021045201115680768', query).then(res => {
      let data = res.data;
      state.optionsObj.tableFieldf5c8ef_F_WarehouseIDOptions = Array.isArray(data) ? data : [];
    });
  }
  function addtableFieldf5c8efRow() {
    let item = {
      jnpfId: buildUUID(),
      F_CustomerId: undefined,
      F_InventoryNum: undefined,
      F_Encoding: undefined,
      F_Num: undefined,
      F_Price: undefined,
      F_Description: undefined,
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
    };
    state.dataForm.tableFieldf5c8ef.push(item);
    state.tableFieldf5c8efinnerActiveKey.push(item.jnpfId);
  }
  function removetableFieldf5c8efRow(index, showConfirm = false) {
    if (showConfirm) {
      createConfirm({
        iconType: 'warning',
        title: '提示',
        content: '此操作将永久删除该数据, 是否继续?',
        onOk: () => {
          state.dataForm.tableFieldf5c8ef.splice(index, 1);
        },
      });
    } else {
      state.dataForm.tableFieldf5c8ef.splice(index, 1);
    }
  }
  function copytableFieldf5c8efRow(index) {
    let item = cloneDeep(state.dataForm.tableFieldf5c8ef[index]);
    let copyData = {};
    for (let i = 0; i < unref(tableFieldf5c8efColumns).length; i++) {
      const cur = unref(tableFieldf5c8efColumns)[i];
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
    state.dataForm.tableFieldf5c8ef.push(copyItem);
    state.tableFieldf5c8efinnerActiveKey.push(copyItem.jnpfId);
  }
  function batchRemovetableFieldf5c8efRow(showConfirm = false) {
    if (!state.selectedtableFieldf5c8efRowKeys.length) return createMessage.error('请选择一条数据');
    const handleBatchRemove = () => {
      state.dataForm.tableFieldf5c8ef = state.dataForm.tableFieldf5c8ef.filter(o => !state.selectedtableFieldf5c8efRowKeys.includes(o.jnpfId));
      nextTick(() => {
        state.selectedtableFieldf5c8efRowKeys = [];
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
  function addtableField3ff267Row() {
    let item = {
      jnpfId: buildUUID(),
      F_Type: undefined,
      F_Title: undefined,
      F_Content: undefined,
      F_Reason: undefined,
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
    };
    state.dataForm.tableField3ff267.push(item);
    state.tableField3ff267innerActiveKey.push(item.jnpfId);
  }
  function removetableField3ff267Row(index, showConfirm = false) {
    if (showConfirm) {
      createConfirm({
        iconType: 'warning',
        title: '提示',
        content: '此操作将永久删除该数据, 是否继续?',
        onOk: () => {
          state.dataForm.tableField3ff267.splice(index, 1);
        },
      });
    } else {
      state.dataForm.tableField3ff267.splice(index, 1);
    }
  }
  function copytableField3ff267Row(index) {
    let item = cloneDeep(state.dataForm.tableField3ff267[index]);
    let copyData = {};
    for (let i = 0; i < unref(tableField3ff267Columns).length; i++) {
      const cur = unref(tableField3ff267Columns)[i];
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
    state.dataForm.tableField3ff267.push(copyItem);
    state.tableField3ff267innerActiveKey.push(copyItem.jnpfId);
  }
  function batchRemovetableField3ff267Row(showConfirm = false) {
    if (!state.selectedtableField3ff267RowKeys.length) return createMessage.error('请选择一条数据');
    const handleBatchRemove = () => {
      state.dataForm.tableField3ff267 = state.dataForm.tableField3ff267.filter(o => !state.selectedtableField3ff267RowKeys.includes(o.jnpfId));
      nextTick(() => {
        state.selectedtableField3ff267RowKeys = [];
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

  // 监听采购单选择，查询后端订单子项并填充子表（F_CustomerId, F_Num）
  async function handleOrderSelectChange(val) {
    try {
      // 清空子表
      state.dataForm.tableFieldf5c8ef = [];
      if (!val) return;
      const res = await getOrderItems(val);
      const list = (res && res.data) || res || [];
      if (!Array.isArray(list) || !list.length) return;
      for (let i = 0; i < list.length; i++) {
        const item = list[i];
        const row = {
          jnpfId: buildUUID(),
          F_CustomerId: item.F_CustomerId ?? item.F_CustomerId,
          F_InventoryNum: item.F_InventoryNum ?? item.F_InventoryNum,
          F_Encoding: item.F_Encoding,
          F_Num: item.F_Num ?? item.F_Num,
          F_Price: undefined,
          F_Description: undefined,
          F_CreatorUserId: undefined,
          F_CreatorTime: undefined,
        };
        state.dataForm.tableFieldf5c8ef.push(row);
        state.tableFieldf5c8efinnerActiveKey.push(row.jnpfId);
      }
    } catch (e) {
      // ignore
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
