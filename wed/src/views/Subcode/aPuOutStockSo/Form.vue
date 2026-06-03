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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_StockOutNo')">
            <a-form-item name="F_StockOutNo" :labelCol="{ style: { width: '100px' } }">
              <template #label>销售出库单号</template>
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
              <JnpfCascader
                v-model:value="dataForm.F_WarehouseId"
                placeholder="请选择"
                :options="optionsObj.F_WarehouseIdOptions"
                :fieldNames="optionsObj.F_WarehouseIdProps"
                allowClear
                :disabled="!!state.dataForm.id"
                showSearch
                :changeOnSelect="true"
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_StockOutUserId')">
            <a-form-item name="F_StockOutUserId" :labelCol="{ style: { width: '100px' } }">
              <template #label>发货人</template>
              <JnpfUserSelect
                v-model:value="dataForm.F_StockOutUserId"
                placeholder="请选择"
                selectType="all"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_CustomerId')">
            <a-form-item name="F_CustomerId" :labelCol="{ style: { width: '100px' } }">
              <template #label>客户</template>
              <JnpfPopupSelect
                v-model:value="dataForm.F_CustomerId"
                placeholder="请选择"
                :templateJson="optionsObj.F_CustomerIdTemplateJson"
                allowClear
                field="F_CustomerId"
                interfaceId="2009458830353764352"
                :columnOptions="optionsObj.F_CustomerIdOptions"
                relationField="F_CustomerName"
                propsValue="F_Id"
                :pageSize="20"
                popupType="dialog"
                popupTitle="选择数据"
                popupWidth="800px"
                hasPage
                :style="{
                  width: '100%',
                }"
                :extraOptions="optionsObj.F_CustomerId" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_ContactId')">
            <a-form-item name="F_ContactId" :labelCol="{ style: { width: '100px' } }">
              <template #label>联系人</template>
              <JnpfSelect
                v-model:value="dataForm.F_ContactId"
                placeholder="请选择"
                :options="optionsObj.F_ContactIdOptions"
                :fieldNames="optionsObj.F_ContactIdProps"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_AddressId')">
            <a-form-item name="F_AddressId" :labelCol="{ style: { width: '100px' } }">
              <template #label>地址</template>
              <JnpfSelect
                v-model:value="dataForm.F_AddressId"
                placeholder="请选择"
                :options="optionsObj.F_AddressIdOptions"
                :fieldNames="optionsObj.F_AddressIdProps"
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
          <a-col :span="24" class="ant-col-item">
            <a-form-item :labelCol="{ style: { width: '100px' } }">
              <template #label>库存商品选择</template>
              <JnpfPopupSelect
                v-model:value="tempGoodsSelector"
                :formData="state.dataForm"
                placeholder="请先选择仓库，再进行商品选择"
                :templateJson="optionsObj.tableFielde48def_F_CustomerIdTemplateJson"
                allowClear
                :field="'tempGoodsSelector'"
                interfaceId="2034873228723359744"
                :columnOptions="optionsObj.tableFielde48def_F_CustomerIdOptions"
                relationField="F_GoodsName"
                propsValue="id"
                :pageSize="20"
                popupType="dialog"
                popupTitle="选择商品"
                popupWidth="70%"
                :disabled="state.dataForm.id"
                hasPage
                multiple
                :style="{
                  width: '100%',
                }"
                :extraOptions="optionsObj.tableFielde48def_F_CustomerId"
                @change="(ids, rows) => handleGoodsSelectorChange(ids, rows)" />
            </a-form-item>
          </a-col>

          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="商品列表" :bordered="false" />
              <a-table
                :data-source="dataForm.tableFielde48def"
                :columns="tableFielde48defColumns"
                size="small"
                rowKey="jnpfId"
                :pagination="listPagination"
                :scroll="{ x: 'max-content' }"
                :rowSelection="gettableFielde48defRowSelection">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                  <template v-if="column.key === 'F_CustomerId'">
                    <JnpfSelect
                      v-model:value="record.F_CustomerId"
                      :options="optionsObj.tableField449e6c_F_GoodsIdOptions"
                      :fieldNames="optionsObj.tableField449e6c_F_GoodsIdProps"
                      allowClear
                      showSearch
                      disabled
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_Code'">
                    <JnpfInput
                      v-model:value="record.F_Code"
                      placeholder="库存编码"
                      readonly
                      :disabled="true"
                      :allowClear="false"
                      :style="{
                        width: '100%',
                      }"
                      :showCount="false" />
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
                  <template v-if="column.key === 'F_SalesPrice'">
                    <JnpfInputNumber
                      v-model:value="record.F_SalesPrice"
                      placeholder="请输入"
                      :controls="false"
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_Type'">
                    <JnpfInput
                      v-model:value="record.F_Type"
                      placeholder="请输入"
                      allowClear
                      :style="{
                        width: '100%',
                      }"
                      :showCount="false" />
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
                      <!-- <a-button class="action-btn" type="link" @click="copytableFielde48defRow(index)" size="small">{{
                        t('common.copyText', '复制')
                      }}</a-button> -->
                      <a-button class="action-btn" type="link" color="error" @click="removetableFielde48defRow(index, true)" size="small">{{
                        t('common.delText', '删除')
                      }}</a-button>
                    </a-space>
                  </template>
                </template>
              </a-table>
              <a-space class="input-table-footer-btn">
                <!-- <a-button type="primary" preIcon="icon-ym icon-ym-btn-add" @click="addtableFielde48defRow">{{ t('common.add1Text', '添加') }}</a-button>
                <a-button type="danger" preIcon="icon-ym icon-ym-btn-clearn" @click="batchRemovetableFielde48defRow(true)">{{
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
  import { create, update, getInfo, getLinkData, getInventoryByWarehouse } from './helper/api';
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
    dataForm: any;
    dataRule: any;
    optionsObj: any;
    currVmodel: any;
    currTableConf: any;
    addTableConf: any;
    tableRows: any;
    tableFielde48defouterActiveKey: number[];
    tableFielde48definnerActiveKey: string[];
    tableFielde48defDefaultExpandAll: boolean;
    selectedtableFielde48defRowKeys: any[];
    tableField67a4d7outerActiveKey: number[];
    tableField67a4d7innerActiveKey: string[];
    tableField67a4d7DefaultExpandAll: boolean;
    selectedtableField67a4d7RowKeys: any[];
    title: string;
    continueText: string;
    allList: any[];
    currIndex: number;
    isContinue: boolean;
    submitType: number;
    showContinueBtn: boolean;
    tempGoodsSelector: any[];
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
  const tableFielde48defColumns: any[] = computed(() => {
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
        dataIndex: 'F_Code',
        key: 'F_Code',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Code',
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
        dataIndex: 'F_SalesPrice',
        key: 'F_SalesPrice',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '100px',
        fixed: false,
        formP: 'F_SalesPrice',
        isSystemControl: false,
      },
      {
        title: '类别',
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
    list = list.filter(o => hasFormP('tableFielde48def-' + o.formP));
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
  const gettableFielde48defRowSelection = computed(() => {
    const rowSelection = {
      selectedRowKeys: state.selectedtableFielde48defRowKeys,
      onChange: (selectedRowKeys: string[]) => {
        state.selectedtableFielde48defRowKeys = (selectedRowKeys || []).sort().reverse();
      },
    };
    return rowSelection;
  });
  const tableField67a4d7Columns: any[] = computed(() => {
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
    list = list.filter(o => hasFormP('tableField67a4d7-' + o.formP));
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
  const gettableField67a4d7RowSelection = computed(() => {
    const rowSelection = {
      selectedRowKeys: state.selectedtableField67a4d7RowKeys,
      onChange: (selectedRowKeys: string[]) => {
        state.selectedtableField67a4d7RowKeys = (selectedRowKeys || []).sort().reverse();
      },
    };
    return rowSelection;
  });
  const state = reactive<State>({
    listPageSize: 20,
    listCurrentPage: 1,
    dataForm: {
      id: '',
      F_StockOutNo: undefined,
      F_WarehouseId: [],
      F_StockOutType: undefined,
      F_StockOutDate: dayjs(new Date()).startOf(getDateTimeUnit('yyyy-MM-dd')).valueOf(),
      F_StockOutUserId: userInfo.userId ? userInfo.userId : '',
      F_CustomerId: undefined,
      F_ContactId: undefined,
      F_AddressId: undefined,
      F_Description: undefined,
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
      tableFielde48def: [],
      tableField67a4d7: [],
    },
    tableFielde48defouterActiveKey: [0],
    tableFielde48definnerActiveKey: [],
    tableFielde48defDefaultExpandAll: true,
    selectedtableFielde48defRowKeys: [],
    tableField67a4d7outerActiveKey: [0],
    tableField67a4d7innerActiveKey: [],
    tableField67a4d7DefaultExpandAll: true,
    selectedtableField67a4d7RowKeys: [],
    dataRule: {
      F_WarehouseId: [
        {
          required: true,
          message: '请输入仓库',
          trigger: 'change',
        },
      ],
      F_StockOutDate: [
        {
          required: true,
          message: '请输入出库日期',
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
      F_StockOutTypeProps: { label: 'fullName', value: 'enCode' },
      F_CustomerIdOptions: [
        {
          value: 'F_CustomerName',
          label: '客户姓名',
        },
        {
          value: 'F_CustomerCode',
          label: '客户编码',
        },
      ],
      F_CustomerIdTemplateJson: [],
      F_ContactIdProps: { label: 'F_ContactName', value: 'F_Id' },
      F_AddressIdProps: { label: 'F_Address', value: 'F_Id' },
      tableFielde48def_F_CustomerIdOptions: [
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
          value: 'F_Type',
          label: '商品类型',
        },
        {
          value: 'F_Num',
          label: '商品数量',
        },
      ],
      tableFielde48def_F_CustomerIdTemplateJson: [
        {
          defaultValue: '',
          field: 'warehouseId',
          dataType: 'varchar',
          required: 0,
          fieldName: '',
          // 使用表单中选中的仓库 ID 作为接口参数（动态绑定）
          relationField: '',
          jnpfKey: null,
          complexHeaderList: null,
          // sourceType=1 会让 getParamList 从 formData 中取值并填充 defaultValue
          sourceType: 2,
          isChildren: false,
          IsMultiple: false,
        },
      ],

      F_CustomerId: [],
      tableFielde48def_F_CustomerId: [],
      // 存放根据仓库获取的库存数据 [{ F_GoodsId, F_Num }]
      inventoryByWarehouse: [],
      tableField449e6c_F_GoodsIdOptions: [],
      tableField449e6c_F_GoodsIdProps: { label: 'F_GoodsName', value: 'id' },
    },
    currVmodel: '',
    currTableConf: {},
    tableRows: {
      tableFielde48def: {
        enabledmark: undefined,
        F_CustomerId: undefined,
        F_Code: undefined,
        F_Num: undefined,
        F_NumLevel1: undefined,
        F_NumLevel2: undefined,
        _F_NumLevel2Max: undefined,
        F_Unit_Ratio: undefined,
        F_Price: undefined,
        F_SalesPrice: undefined,
        F_Type: undefined,
        F_Description: undefined,
        F_CreatorUserId: undefined,
        F_CreatorTime: undefined,
      },
      tableField67a4d7: {
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
  });
  const { title, continueText, showContinueBtn, dataForm, submitType, dataRule, optionsObj, tempGoodsSelector } = toRefs(state);

  // 解析 F_Unit_Ratio（与 aPuStockFg 一致）：两级单位 { level1: { name, qty }, level2: { name, qty } }
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

  watch(
    () => state.dataForm.tableFielde48def,
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
      state.dataForm.tableFielde48def = [];
      state.dataForm.tableField67a4d7 = [];
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
        F_WarehouseId: [],
        F_StockOutType: undefined,
        F_StockOutDate: dayjs(new Date()).startOf(getDateTimeUnit('yyyy-MM-dd')).valueOf(),
        F_StockOutUserId: userInfo.userId ? userInfo.userId : '',
        F_CustomerId: undefined,
        F_ContactId: undefined,
        F_AddressId: undefined,
        F_Description: undefined,
        F_CreatorUserId: undefined,
        F_CreatorTime: undefined,
        tableFielde48def: [],
        tableField67a4d7: [],
      };
      getF_WarehouseIdOptions();
      getF_StockOutTypeOptions();
      getF_ContactIdOptions();
      getF_AddressIdOptions();
      gettableField449e6c_F_GoodsIdOptions();
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
      for (let i = 0; i < state.dataForm.tableFielde48def.length; i++) {
        const element = state.dataForm.tableFielde48def[i];
        element.jnpfId = buildUUID();
      }
      for (let i = 0; i < state.dataForm.tableField67a4d7.length; i++) {
        const element = state.dataForm.tableField67a4d7[i];
        element.jnpfId = buildUUID();
      }
      // 编辑模式下获取商品详情（含 F_Unit_Ratio）用于数量层级显示
      await enrichTableRowsGoodsMeta(state.dataForm.tableFielde48def);
      for (let i = 0; i < (state.dataForm.tableFielde48def || []).length; i++) {
        const r = state.dataForm.tableFielde48def[i];
        if (r) fillNumLevelsFromF_Num(r);
      }
      getF_WarehouseIdOptions();
      getF_StockOutTypeOptions();
      gettableField449e6c_F_GoodsIdOptions();
      changeLoading(false);
    });
  }

  /** 编辑模式下获取商品详情（含 F_Unit_Ratio）用于数量层级显示 */
  async function enrichTableRowsGoodsMeta(rows: any[]) {
    if (!Array.isArray(rows) || rows.length === 0) return;
    const idsToFetch: string[] = [];
    for (let i = 0; i < rows.length; i++) {
      const r = rows[i];
      if (r && r.F_CustomerId && !r.F_Unit_Ratio) {
        idsToFetch.push(r.F_CustomerId);
      }
    }
    if (idsToFetch.length === 0) return;
    const uniqueIds = Array.from(new Set(idsToFetch));
    const interfaceId = '2008721559174385664';
    const query = {
      ids: uniqueIds,
      interfaceId,
      propsValue: 'F_Id',
      relationField: 'F_GoodsName',
      paramList: [],
    };
    try {
      const resp = await getDataInterfaceDataInfoByIds(interfaceId, query);
      const list = resp.data || [];
      const map: Record<string, any> = {};
      list.forEach((item: any) => {
        if (item && item.F_Id != null) map[String(item.F_Id)] = item;
      });
      for (let i = 0; i < rows.length; i++) {
        const r = rows[i];
        if (!r) continue;
        const found = map[String(r.F_CustomerId)];
        if (found) {
          r.F_Unit_Ratio = found.F_Unit_Ratio ?? found.f_Unit_Ratio ?? r.F_Unit_Ratio;
        }
      }
    } catch (e) {
      // ignore
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
  // 监听仓库变化，拉取该仓库的库存信息，供批量选择时自动填充出库数量
  watch(
    () => state.dataForm.F_WarehouseId,
    async val => {
      // 将仓库 ID 转换为 relationField 格式（JSON 字符串数组）
      const warehouseIdArr = Array.isArray(val) ? val : val ? [val] : [];
      state.optionsObj.tableFielde48def_F_CustomerIdTemplateJson[0].relationField = JSON.stringify(warehouseIdArr);
      const warehouseId = val || '';
      if (!warehouseId) {
        state.optionsObj.inventoryByWarehouse = [];
        return;
      }
      try {
        const res = await getInventoryByWarehouse(warehouseId);
        const data = res && res.data ? res.data : res || [];
        state.optionsObj.inventoryByWarehouse = Array.isArray(data) ? data : [];
      } catch (e) {
        state.optionsObj.inventoryByWarehouse = [];
      }
    },
  );
  function gettableField449e6c_F_GoodsIdOptions() {
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
      state.optionsObj.tableField449e6c_F_GoodsIdOptions = Array.isArray(data) ? data : [];
    });
  }
  function getF_StockOutTypeOptions() {
    getDictionaryDataSelector('2013096194263355392').then(res => {
      state.optionsObj.F_StockOutTypeOptions = res.data.list;
    });
  }
  function getF_ContactIdOptions(customerId?: string) {
    const relationVal = customerId || state.dataForm.F_CustomerId || '';
    let templateJson = [
      {
        defaultValue: '',
        field: 'Id',
        dataType: 'varchar',
        required: 0,
        fieldName: '',
        relationField: relationVal,
        jnpfKey: null,
        complexHeaderList: null,
        sourceType: 2,
        isChildren: false,
        IsMultiple: false,
      },
    ];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2009459664370143232', query).then(res => {
      let data = res.data;
      state.optionsObj.F_ContactIdOptions = Array.isArray(data) ? data : [];
    });
  }
  function getF_AddressIdOptions(customerId?: string) {
    const relationVal = customerId || state.dataForm.F_CustomerId || '';
    let templateJson = [
      {
        defaultValue: '',
        field: 'id',
        dataType: 'varchar',
        required: 0,
        fieldName: '',
        relationField: relationVal,
        jnpfKey: null,
        complexHeaderList: null,
        sourceType: 2,
        isChildren: false,
        IsMultiple: false,
      },
    ];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2009527238009163776', query).then(res => {
      let data = res.data;
      state.optionsObj.F_AddressIdOptions = Array.isArray(data) ? data : [];
    });
  }

  // 监听客户选择变化，调用后端接口获取默认联系人与地址并设置下拉及选中值
  watch(
    () => state.dataForm.F_CustomerId,
    async val => {
      const customerId = val || '';
      if (!customerId) {
        state.optionsObj.F_ContactIdOptions = [];
        state.optionsObj.F_AddressIdOptions = [];
        state.dataForm.F_ContactId = undefined;
        state.dataForm.F_AddressId = undefined;
        return;
      }
      try {
        const res = await getLinkData(customerId);
        // defHttp 可能返回 { code,msg,data } 或直接返回 data，兼容两种情况
        const data = res && res.data ? res.data : res || {};
        // 设置下拉选项（保证能展示名称）
        if (data.contactId && data.contactName) {
          state.optionsObj.F_ContactIdOptions = [{ F_Id: data.contactId, F_ContactName: data.contactName, value: data.contactId, label: data.contactName }];
          state.dataForm.F_ContactId = data.contactId;
        } else {
          state.optionsObj.F_ContactIdOptions = [];
          state.dataForm.F_ContactId = undefined;
        }
        if (data.addressId && data.address) {
          state.optionsObj.F_AddressIdOptions = [{ F_Id: data.addressId, F_Address: data.address, value: data.addressId, label: data.address }];
          state.dataForm.F_AddressId = data.addressId;
        } else {
          state.optionsObj.F_AddressIdOptions = [];
          state.dataForm.F_AddressId = undefined;
        }
      } catch (error) {
        state.optionsObj.F_ContactIdOptions = [];
        state.optionsObj.F_AddressIdOptions = [];
      }
    },
  );

  function addtableFielde48defRow() {
    let item = {
      jnpfId: buildUUID(),
      F_CustomerId: undefined,
      F_Code: undefined,
      F_Num: undefined,
      F_NumLevel1: undefined,
      F_NumLevel2: undefined,
      _F_NumLevel2Max: undefined,
      F_Unit_Ratio: undefined,
      F_Price: undefined,
      F_SalesPrice: undefined,
      F_Type: undefined,
      F_Description: undefined,
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
    };
    state.dataForm.tableFielde48def.push(item);
    state.tableFielde48definnerActiveKey.push(item.jnpfId);
  }
  function removetableFielde48defRow(index, showConfirm = false) {
    const doRemove = () => {
      const removed = state.dataForm.tableFielde48def.splice(index, 1) || [];
      // 同步清除批量选择中已删除的商品
      if (removed.length) syncTempGoodsSelectorRemoveByRecords(removed);
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
  function copytableFielde48defRow(index) {
    let item = cloneDeep(state.dataForm.tableFielde48def[index]);
    let copyData = {};
    for (let i = 0; i < unref(tableFielde48defColumns).length; i++) {
      const cur = unref(tableFielde48defColumns)[i];
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
    state.dataForm.tableFielde48def.push(copyItem);
    state.tableFielde48definnerActiveKey.push(copyItem.jnpfId);
  }
  function batchRemovetableFielde48defRow(showConfirm = false) {
    if (!state.selectedtableFielde48defRowKeys.length) return createMessage.error('请选择一条数据');
    const handleBatchRemove = () => {
      const removedRows = state.dataForm.tableFielde48def.filter(o => state.selectedtableFielde48defRowKeys.includes(o.jnpfId));
      state.dataForm.tableFielde48def = state.dataForm.tableFielde48def.filter(o => !state.selectedtableFielde48defRowKeys.includes(o.jnpfId));
      // 清除批量选择中已删除的商品
      if (removedRows.length) syncTempGoodsSelectorRemoveByRecords(removedRows);
      nextTick(() => {
        state.selectedtableFielde48defRowKeys = [];
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
  /**
   * 从 tempGoodsSelector 中移除与已删除子表记录对应的选中项
   * 接受被删除记录数组，支持 tempGoodsSelector 中存放 id 或对象的情形
   */
  function syncTempGoodsSelectorRemoveByRecords(records: any[]) {
    try {
      if (!records || !records.length) return;
      const curr = state.tempGoodsSelector || [];
      if (!curr || !curr.length) return;
      // 收集被删除记录中的 id（尽量兼容不同字段名）
      const removedIds = records
        .map(r => r.F_CustomerId ?? r.F_GoodsId ?? r.F_Id ?? r.id ?? null)
        .filter(Boolean)
        .map(String);
      if (!removedIds.length) return;
      const newArr = (curr || []).filter((item: any) => {
        if (item == null) return false;
        if (typeof item === 'object') {
          const itemId = item.F_Id ?? item.FId ?? item.id ?? item.value ?? item.F_GoodsId ?? item.FGoodsId ?? null;
          return !itemId || !removedIds.includes(String(itemId));
        } else {
          return !removedIds.includes(String(item));
        }
      });
      state.tempGoodsSelector = newArr;
    } catch (e) {
      // ignore
    }
  }
  function addtableField67a4d7Row() {
    let item = {
      jnpfId: buildUUID(),
      F_Type: undefined,
      F_Title: undefined,
      F_Content: undefined,
      F_Reason: undefined,
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
    };
    state.dataForm.tableField67a4d7.push(item);
    state.tableField67a4d7innerActiveKey.push(item.jnpfId);
  }
  function removetableField67a4d7Row(index, showConfirm = false) {
    if (showConfirm) {
      createConfirm({
        iconType: 'warning',
        title: '提示',
        content: '此操作将永久删除该数据, 是否继续?',
        onOk: () => {
          state.dataForm.tableField67a4d7.splice(index, 1);
        },
      });
    } else {
      state.dataForm.tableField67a4d7.splice(index, 1);
    }
  }
  function copytableField67a4d7Row(index) {
    let item = cloneDeep(state.dataForm.tableField67a4d7[index]);
    let copyData = {};
    for (let i = 0; i < unref(tableField67a4d7Columns).length; i++) {
      const cur = unref(tableField67a4d7Columns)[i];
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
    state.dataForm.tableField67a4d7.push(copyItem);
    state.tableField67a4d7innerActiveKey.push(copyItem.jnpfId);
  }
  function batchRemovetableField67a4d7Row(showConfirm = false) {
    if (!state.selectedtableField67a4d7RowKeys.length) return createMessage.error('请选择一条数据');
    const handleBatchRemove = () => {
      state.dataForm.tableField67a4d7 = state.dataForm.tableField67a4d7.filter(o => !state.selectedtableField67a4d7RowKeys.includes(o.jnpfId));
      nextTick(() => {
        state.selectedtableField67a4d7RowKeys = [];
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

  /** InfoByIds 等接口返回可能是 { data: [] } 或已解包数组、或 { data: { list: [] } } */
  function parseInfoByIdsResponse(resp: any): any[] {
    if (resp == null) return [];
    const inner = resp.data !== undefined ? resp.data : resp;
    if (Array.isArray(inner)) return inner;
    if (inner && typeof inner === 'object' && Array.isArray(inner.list)) return inner.list;
    return [];
  }

  /**
   * 子表「商品」列提交值应对齐 a_goods_inventory.F_GoodsId。
   * 库存接口一行里 F_Id 为库存主键，F_GoodsId 为商品主键；二者不同，不能存 F_Id。
   * 仅商品表弹窗行时通常只有 F_Id（即商品 id），此时用 F_Id。
   */
  function resolveSubTableGoodsId(data: any): any {
    if (data == null || typeof data !== 'object') return data;
    if (data.F_GoodsId != null && data.F_GoodsId !== '') return data.F_GoodsId;
    return data.F_Id ?? data.FId ?? data.id ?? data.value;
  }

  /** 弹窗确定时 emit(change, ids, selectRow)：优先用第二参完整行，才能带上 F_Code */
  function normalizePickerFullRows(_selected: any, pickerRows: any): any[] | null {
    if (Array.isArray(pickerRows) && pickerRows.length) {
      const first = pickerRows[0];
      if (first && typeof first === 'object' && (first.F_Id != null || first.F_GoodsId != null || first.F_GoodsName)) {
        return pickerRows;
      }
    }
    if (pickerRows && typeof pickerRows === 'object' && !Array.isArray(pickerRows)) {
      if (pickerRows.F_Id != null || pickerRows.F_GoodsId != null) return [pickerRows];
    }
    return null;
  }

  function logTableFCodeDebug(source: string) {
    const rows = state.dataForm.tableFielde48def || [];
    // console.log(
    //   `[销售出库-商品子表 F_Code] 来源=${source}`,
    //   rows.map((r: any, i: number) => ({
    //     行: i + 1,
    //     F_CustomerId: r.F_CustomerId,
    //     F_Code: r.F_Code,
    //   })),
    // );
  }

  // 批量商品选择处理方法（参考 aPuStockPu 实现），并按仓库库存过滤
  function handleGoodsSelectorChange(selected: any, pickerRows?: any) {
    // 清空现有子表数据
    state.dataForm.tableFielde48def = [];
    state.selectedtableFielde48defRowKeys = [];

    const selectionEmpty = selected == null || selected === '' || (Array.isArray(selected) && selected.length === 0);
    if (selectionEmpty) {
      state.tempGoodsSelector = [];
      return;
    }

    const interfaceId = '2015036768293883904';

    // 仓库库存集合（后端返回的 F_GoodsId）
    const inventoryList = state.optionsObj.inventoryByWarehouse || [];
    const inventorySet = new Set((inventoryList || []).map((it: any) => String(it.F_GoodsId)));
    const shouldFilterByInventory = !!state.dataForm.F_WarehouseId && inventorySet.size > 0;
    const skipped: string[] = [];

    const buildAndPushRow = (data: any, _index: number) => {
      let relationId: any = data;
      let fullObj: any = null;
      if (data && typeof data === 'object') {
        fullObj = data;
        relationId = resolveSubTableGoodsId(data) ?? relationId;
      }
      // 如果需要按仓库库存过滤且当前商品不在库存中，则跳过
      if (shouldFilterByInventory && !inventorySet.has(String(relationId))) {
        const name = (fullObj && (fullObj.F_GoodsName || fullObj.F_GoodsNo)) || String(relationId);
        skipped.push(name);
        return;
      }

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
        F_CustomerId: fullObj.F_GoodsId,
        _F_CustomerObj: fullObj,
        F_Code: fullObj?.F_Code ?? fullObj?.f_Code ?? undefined,
        // display fields
        GoodsName: fullObj?.F_GoodsName ?? fullObj?.GoodsName ?? undefined,
        GoodsNo: fullObj?.F_GoodsNo ?? fullObj?.GoodsNo ?? undefined,
        F_Unit_Ratio: fullObj?.F_Unit_Ratio ?? fullObj?.f_Unit_Ratio ?? undefined,
        F_Num: defaultNum,
        F_NumLevel1: undefined,
        F_NumLevel2: undefined,
        F_Price: undefined,
        F_SalesPrice: undefined,
        F_Type: undefined,
        F_Description: undefined,
        F_CreatorUserId: undefined,
        F_CreatorTime: undefined,
      };
      // 填充展示字段（优先使用完整对象）
      handleTableCustomerSelectChange(fullObj || relationId, fullObj, newRow);
      // 若有仓库库存数据且当前行没有出库数量，则用库存数量填充（仅针对新增行）
      try {
        const invList = state.optionsObj.inventoryByWarehouse || [];
        const foundInv = (invList || []).find((it: any) => String(it.F_GoodsId) === String(relationId) || String(it.F_Id) === String(relationId));
        if ((newRow.F_Num === undefined || newRow.F_Num === null) && foundInv && foundInv.F_Num != null) {
          newRow.F_Num = foundInv.F_Num;
        }
        fillNumLevelsFromF_Num(newRow);
      } catch (e) {
        // ignore
      }
      state.dataForm.tableFielde48def.push(newRow);
    };

    const itemsAreObjects = (arr: any[]) => {
      if (!arr || !arr.length) return false;
      const first = arr[0];
      return first && typeof first === 'object' && (first.F_Id || first.F_GoodsName || first.F_GoodsNo);
    };

    const finishWithSkippedNotice = () => {
      if (skipped.length) {
        const listPreview = skipped.slice(0, 10).join(', ');
        createMessage.warning('以下商品不在所选仓库库存中，已被忽略：' + listPreview + (skipped.length > 10 ? '...' : ''));
      }
    };

    const fullRowsFromPicker = normalizePickerFullRows(selected, pickerRows);
    if (fullRowsFromPicker?.length) {
      fullRowsFromPicker.forEach((goods, _i) => buildAndPushRow(goods, _i));
      state.tempGoodsSelector = [];
      finishWithSkippedNotice();
      logTableFCodeDebug('弹窗第二参完整行');
      return;
    }

    if (Array.isArray(selected)) {
      if (itemsAreObjects(selected)) {
        selected.forEach((goods, _i) => buildAndPushRow(goods, _i));
        state.tempGoodsSelector = [];
        finishWithSkippedNotice();
        logTableFCodeDebug('selected 为对象数组');
      } else {
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
            const list = parseInfoByIdsResponse(resp);
            const map: Record<string, any> = {};
            list.forEach((item: any) => {
              if (item && item.F_Id != null) map[String(item.F_Id)] = item;
            });
            for (let i = 0; i < ids.length; i++) {
              const id = ids[i];
              const found = map[String(id)];
              buildAndPushRow(found || id, i);
            }
            state.tempGoodsSelector = [];
            finishWithSkippedNotice();
            logTableFCodeDebug('InfoByIds(ids)');
          })
          .catch(err => {
            console.error('getDataInterfaceDataInfoByIds error', err);
            try {
              createMessage.error(typeof err === 'object' ? JSON.stringify(err) : String(err));
            } catch (e) {
              // ignore
            }
            selected.forEach((goods, _i) => buildAndPushRow(goods, _i));
            state.tempGoodsSelector = [];
            finishWithSkippedNotice();
            logTableFCodeDebug('InfoByIds catch 回退');
          });
      }
    } else {
      const single = selected;
      if (single && typeof single === 'object') {
        buildAndPushRow(single, 0);
        state.tempGoodsSelector = [];
        finishWithSkippedNotice();
        logTableFCodeDebug('单选对象');
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
            const list = parseInfoByIdsResponse(resp);
            const found = list[0] || null;
            buildAndPushRow(found || id, 0);
            state.tempGoodsSelector = [];
            finishWithSkippedNotice();
            logTableFCodeDebug('InfoByIds 单 id');
          })
          .catch(err => {
            console.error('getDataInterfaceDataInfoByIds error', err);
            try {
              createMessage.error(typeof err === 'object' ? JSON.stringify(err) : String(err));
            } catch (e) {
              // ignore
            }
            buildAndPushRow(id, 0);
            state.tempGoodsSelector = [];
            finishWithSkippedNotice();
            logTableFCodeDebug('InfoByIds 单 id catch');
          });
      }
    }
  }

  // 子表商品选择后填充只读展示字段（与 aPuStockPu 对齐）
  function handleTableCustomerSelectChange(val: any, option: any, record: any) {
    try {
      if (!val) {
        record.F_Price = undefined;
        record.F_SalesPrice = undefined;
        record.F_Type = undefined;
        record.GoodsName = undefined;
        record.GoodsNo = undefined;
        record.F_Code = undefined;
        record.F_Unit_Ratio = undefined;
        record.F_NumLevel1 = undefined;
        record.F_NumLevel2 = undefined;
        return;
      }
      let info = Array.isArray(option) ? option[0] || {} : option || {};
      if (!info || Object.keys(info).length === 0) {
        const opts = state.optionsObj.tableFielde48def_F_CustomerIdOptions || [];
        const found = opts.find((o: any) => o.F_Id === val || o.id === val || o.value === val);
        if (found) info = found;
      }
      // 映射商品名称与编号，供表格显示
      record.GoodsName = info.F_GoodsName ?? info.GoodsName ?? record.GoodsName;
      record.GoodsNo = info.F_GoodsNo ?? info.GoodsNo ?? record.GoodsNo;
      record.F_Code = info.F_Code ?? info.f_Code ?? record.F_Code;
      record.F_Price = info.F_Price ?? info.F_CostPrice ?? record.F_Price;
      record.F_SalesPrice = info.F_SalesPrice ?? info.F_SalePrice ?? record.F_SalesPrice;
      record.F_Type = info.F_Type ?? info.F_Class ?? record.F_Type;
      record.F_Unit_Ratio = info.F_Unit_Ratio ?? info.f_Unit_Ratio ?? record.F_Unit_Ratio;
      fillNumLevelsFromF_Num(record);
    } catch (e) {
      // ignore
    }
  }

  // 加载已有行时填充展示字段（优先使用缓存的 extraOptions，否则按 id 调接口查询）
  function fillRowDisplayFields(record: any) {
    try {
      const id = record.F_CustomerId;
      if (!id) return;
      const opts = state.optionsObj.tableFielde48def_F_CustomerIdOptions || [];
      const found = opts.find((o: any) => o.F_Id === id || o.id === id || o.value === id);
      if (found && Object.keys(found).length) {
        record.F_Code = found.F_Code ?? found.f_Code ?? record.F_Code;
        record.F_Price = found.F_Price ?? found.F_CostPrice ?? record.F_Price;
        record.F_SalesPrice = found.F_SalesPrice ?? found.F_SalePrice ?? record.F_SalesPrice;
        record.F_Type = found.F_Type ?? found.F_Class ?? record.F_Type;
        record.F_Unit_Ratio = found.F_Unit_Ratio ?? found.f_Unit_Ratio ?? record.F_Unit_Ratio;
        fillNumLevelsFromF_Num(record);
        return;
      }
      const extras = state.optionsObj.tableFielde48def_F_CustomerId || [];
      const foundExtra = extras.find((o: any) => String(o.F_Id) === String(id) || String(o.F_GoodsId) === String(id));
      if (foundExtra && Object.keys(foundExtra).length) {
        record.F_Code = foundExtra.F_Code ?? foundExtra.f_Code ?? record.F_Code;
        record.F_Price = foundExtra.F_Price ?? foundExtra.F_CostPrice ?? record.F_Price;
        record.F_SalesPrice = foundExtra.F_SalesPrice ?? foundExtra.F_SalePrice ?? record.F_SalesPrice;
        record.F_Type = foundExtra.F_Type ?? foundExtra.F_Class ?? record.F_Type;
        record.F_Unit_Ratio = foundExtra.F_Unit_Ratio ?? foundExtra.f_Unit_Ratio ?? record.F_Unit_Ratio;
        fillNumLevelsFromF_Num(record);
        return;
      }
      try {
        const query = { paramList: [{ defaultValue: id, field: 'Id', dataType: 'varchar' }] };
        getDataInterfaceRes('2008721559174385664', query).then((res: any) => {
          const data = Array.isArray(res.data) ? res.data[0] : res.data;
          if (!data) return;
          record.F_Code = data.F_Code ?? data.f_Code ?? record.F_Code;
          record.F_Price = data.F_Price ?? data.F_CostPrice ?? record.F_Price;
          record.F_SalesPrice = data.F_SalesPrice ?? data.F_SalePrice ?? record.F_SalesPrice;
          record.F_Type = data.F_Type ?? data.F_Class ?? record.F_Type;
          record.F_Unit_Ratio = data.F_Unit_Ratio ?? data.f_Unit_Ratio ?? record.F_Unit_Ratio;
          fillNumLevelsFromF_Num(record);
        });
      } catch (e) {
        // ignore
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
