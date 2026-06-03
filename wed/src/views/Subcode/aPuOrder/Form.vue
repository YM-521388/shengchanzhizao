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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_OrderNo')">
            <a-form-item name="F_OrderNo" :labelCol="{ style: { width: '100px' } }">
              <template #label>采购单号</template>
              <JnpfInput
                v-model:value="dataForm.F_OrderNo"
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_Money')">
            <a-form-item name="F_Money" :labelCol="{ style: { width: '100px' } }">
              <template #label>商品金额</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_Money"
                placeholder="请输入"
                :controls="false"
                disabled
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_PurchaseNum')">
            <a-form-item name="F_PurchaseNum" :labelCol="{ style: { width: '100px' } }">
              <template #label>采购数量</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_PurchaseNum"
                placeholder="请输入"
                :controls="false"
                disabled
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>

          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_ProdPlanId')">
            <a-form-item name="F_ProdPlanId" :labelCol="{ style: { width: '100px' } }">
              <template #label>采购计划</template>
              <JnpfPopupSelect
                v-model:value="dataForm.F_ProdPlanId"
                placeholder="请选择"
                :templateJson="optionsObj.F_ProdPlanIdTemplateJson"
                allowClear
                field="F_ProdPlanId"
                interfaceId="2011711502578487296"
                :columnOptions="optionsObj.F_ProdPlanIdOptions"
                relationField="F_PlanName"
                propsValue="F_Id"
                :pageSize="20"
                popupType="dialog"
                popupTitle="选择数据"
                popupWidth="60%"
                hasPage
                :style="{
                  width: '100%',
                }"
                :extraOptions="optionsObj.F_ProdPlanId"
                @change="handleProdPlanChange" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_UserId')">
            <a-form-item name="F_UserId" :labelCol="{ style: { width: '100px' } }">
              <template #label>采购人</template>
              <JnpfUserSelect
                v-model:value="dataForm.F_UserId"
                placeholder="请选择"
                selectType="all"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_DeliveryDate')">
            <a-form-item name="F_DeliveryDate" :labelCol="{ style: { width: '100px' } }">
              <template #label>交货日期</template>
              <JnpfDatePicker
                v-model:value="dataForm.F_DeliveryDate"
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
              <JnpfGroupTitle content="采购单商品" :bordered="false" />
              <a-col :span="24" class="ant-col-item">
                <a-form-item :labelCol="{ style: { width: '100px' } }">
                  <template #label>商品选择</template>
                  <JnpfPopupSelect
                    v-model:value="tempGoodsSelector"
                    placeholder="请选择商品"
                    :templateJson="optionsObj.tableFieldf01abd_F_CustomerIdTemplateJson"
                    allowClear
                    :field="'tempGoodsSelector'"
                    interfaceId="2008721559174385664"
                    :columnOptions="optionsObj.tableFieldf01abd_F_CustomerIdOptions"
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
              <a-table
                :data-source="dataForm.tableFieldf01abd"
                :columns="tableFieldf01abdColumns"
                size="small"
                rowKey="jnpfId"
                :pagination="listPagination"
                :scroll="{ x: 'max-content' }"
                :rowSelection="gettableFieldf01abdRowSelection">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                  <template v-if="column.key === 'F_CustomerId'">
                    <JnpfPopupSelect
                      v-model:value="record.F_CustomerId"
                      placeholder="请选择"
                      :templateJson="optionsObj.tableFieldf01abd_F_CustomerIdTemplateJson"
                      allowClear
                      :field="'F_CustomerId' + index"
                      interfaceId="2008721559174385664"
                      :columnOptions="optionsObj.tableFieldf01abd_F_CustomerIdOptions"
                      relationField="F_GoodsName"
                      propsValue="F_Id"
                      :pageSize="20"
                      disabled
                      popupType="dialog"
                      popupTitle="选择数据"
                      popupWidth="70%"
                      :style="{
                        width: '100%',
                      }"
                      :extraOptions="optionsObj.tableFieldf01abd_F_CustomerId" />
                  </template>
                  <template v-if="column.key === 'F_SupplierId'">
                    <JnpfSelect
                      v-model:value="record.F_SupplierId"
                      placeholder="请选择"
                      :options="optionsObj.F_SupplierIdOptions"
                      :fieldNames="optionsObj.F_SupplierIdProps"
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_Num'">
                    <template v-if="getUnitRatioLevels(record)?.level2?.name">
                      <a-space>
                        <JnpfInputNumber
                          v-model:value="record.F_NumLevel1"
                          placeholder="请输入"
                          :controls="false"
                          :style="{ width: '80px' }"
                          @change="handleNumLevelChange(record)" />
                        <span>{{ getUnitRatioLevels(record)?.level1?.name }}</span
                        >/
                        <JnpfInputNumber v-model:value="record.F_NumLevel2" placeholder="自动计算" :controls="false" disabled :style="{ width: '80px' }" />
                        <span>{{ getUnitRatioLevels(record)?.level2?.name }}</span>
                      </a-space>
                    </template>
                    <template v-else>
                      <a-space>
                        <JnpfInputNumber
                          v-model:value="record.F_NumLevel1"
                          placeholder="请输入"
                          :controls="false"
                          :style="{ width: '80px' }"
                          @change="handleNumLevelChange(record)" />
                        <span>{{ getUnitRatioLevels(record)?.level1?.name }}</span>
                      </a-space>
                    </template>
                  </template>
                  <template v-if="column.key === 'F_Price'">
                    <JnpfInputNumber
                      v-model:value="record.F_Price"
                      placeholder="请输入"
                      :controls="false"
                      @change="recalcTableRow(record)"
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_Amount'">
                    <p>{{
                      ((Number(record.F_Price) || 0) * (getUnitRatioLevels(record) ? Number(record.F_NumLevel1) || 0 : Number(record.F_Num) || 0)).toFixed(2)
                    }}</p>
                  </template>
                  <template v-if="column.key === 'F_Discount'">
                    <JnpfInputNumber
                      v-model:value="record.F_Discount"
                      placeholder="请输入"
                      :controls="false"
                      :min="0"
                      :max="100"
                      @change="recalcTableRow(record)"
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_DiscountMoney'">
                    <JnpfInputNumber
                      v-model:value="record.F_DiscountMoney"
                      placeholder="请输入"
                      :controls="false"
                      :min="0"
                      @change="onDiscountMoneyChange(record)"
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_DiscountedAmount'">
                    <p>{{ (Number(record.F_DiscountedAmount) || 0).toFixed(2) }}</p>
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
                      <!-- <a-button class="action-btn" type="link" @click="copytableFieldf01abdRow(index)" size="small">{{
                        t('common.copyText', '复制')
                      }}</a-button> -->
                      <a-button class="action-btn" type="link" color="error" @click="removetableFieldf01abdRow(index, true)" size="small">{{
                        t('common.delText', '删除')
                      }}</a-button>
                    </a-space>
                  </template>
                </template>
              </a-table>
              <a-space class="input-table-footer-btn">
                <!-- <a-button type="primary" preIcon="icon-ym icon-ym-btn-add" @click="addtableFieldf01abdRow">{{ t('common.add1Text', '添加') }}</a-button>
                <a-button type="danger" preIcon="icon-ym icon-ym-btn-clearn" @click="batchRemovetableFieldf01abdRow(true)">{{
                  t('common.batchDelText', '批量删除')
                }}</a-button> -->
              </a-space>
            </a-form-item>
          </a-col>
          <!-- <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="采购单日志" :bordered="false" />
              <a-table
                :data-source="dataForm.tableField8b2a57"
                :columns="tableField8b2a57Columns"
                size="small"
                rowKey="jnpfId"
                :pagination="false"
                :scroll="{ x: 'max-content' }"
                :rowSelection="gettableField8b2a57RowSelection">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                  <template v-if="column.key === 'F_Title'">
                    <JnpfInput
                      v-model:value="record.F_Title"
                      placeholder="请输入"
                      allowClear
                      :style="{
                        width: '100%',
                      }"
                      :showCount="false" />
                  </template>
                  <template v-if="column.key === 'F_Content'">
                    <JnpfTextarea
                      v-model:value="record.F_Content"
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
                      <a-button class="action-btn" type="link" @click="copytableField8b2a57Row(index)" size="small">{{
                        t('common.copyText', '复制')
                      }}</a-button>
                      <a-button class="action-btn" type="link" color="error" @click="removetableField8b2a57Row(index, true)" size="small">{{
                        t('common.delText', '删除')
                      }}</a-button>
                    </a-space>
                  </template>
                </template>
              </a-table>
              <a-space class="input-table-footer-btn">
                <a-button type="primary" preIcon="icon-ym icon-ym-btn-add" @click="addtableField8b2a57Row">{{ t('common.add1Text', '添加') }}</a-button>
                <a-button type="danger" preIcon="icon-ym icon-ym-btn-clearn" @click="batchRemovetableField8b2a57Row(true)">{{
                  t('common.batchDelText', '批量删除')
                }}</a-button>
              </a-space>
            </a-form-item>
          </a-col> -->
        </a-row>
      </a-form>
    </a-row>
  </BasicModal>
  <SelectModal :config="state.currTableConf" :formData="state.dataForm" ref="selectModal" @select="addForSelect" />
</template>
<script lang="ts" setup>
  import { create, update, getInfo, getProdPlanItems } from './helper/api';
  import { reactive, toRefs, nextTick, ref, unref, computed, toRaw, inject, watch } from 'vue';
  import { BasicModal, useModal } from '@/components/Modal';
  import SelectModal from '@/components/CommonModal/src/SelectModal.vue';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useI18n } from '@/hooks/web/useI18n';
  import { useUserStore } from '@/store/modules/user';
  import type { FormInstance } from 'ant-design-vue';
  import { thousandsFormat } from '@/utils/jnpf';
  import { getTimeUnit, getDateTimeUnit } from '@/utils/jnpf';
  import { getDataInterfaceRes, getDataInterfaceDataInfoByIds } from '@/api/systemData/dataInterface';
  import dayjs from 'dayjs';
  import { cloneDeep } from 'lodash-es';
  import { buildUUID } from '@/utils/uuid';
  import { CaretRightOutlined } from '@ant-design/icons-vue';
  import { systemComponentsList } from '@/components/FormGenerator/src/helper/config';
  import { usePermission } from '@/hooks/web/usePermission';
  import { useGlobSetting } from '@/hooks/setting';

  interface State {
    dataForm: any;
    dataRule: any;
    optionsObj: any;
    tempGoodsSelector?: any[];
    currVmodel: any;
    currTableConf: any;
    addTableConf: any;
    tableRows: any;
    tableFieldf01abdouterActiveKey: number[];
    tableFieldf01abdinnerActiveKey: string[];
    tableFieldf01abdDefaultExpandAll: boolean;
    selectedtableFieldf01abdRowKeys: any[];
    tableField8b2a57outerActiveKey: number[];
    tableField8b2a57innerActiveKey: string[];
    tableField8b2a57DefaultExpandAll: boolean;
    selectedtableField8b2a57RowKeys: any[];
    listPageSize: number;
    listCurrentPage: number;
    title: string;
    continueText: string;
    allList: any[];
    currIndex: number;
    isContinue: boolean;
    submitType: number;
    showContinueBtn: boolean;
    _incomingPrefill?: any;
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
  const tableFieldf01abdColumns: any[] = computed(() => {
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
        width: '150px',
        fixed: false,
        formP: 'F_CustomerId',
        isSystemControl: false,
      },
      {
        title: '商品编号',
        dataIndex: 'F_GoodsNo',
        key: 'F_GoodsNo',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_GoodsNo',
        isSystemControl: true,
      },
      {
        title: '规格',
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
        isSystemControl: true,
      },
      {
        title: '供应商',
        dataIndex: 'F_SupplierId',
        key: 'F_SupplierId',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '150px',
        fixed: false,
        formP: 'F_SupplierId',
        isSystemControl: false,
      },
      {
        title: '数量',
        dataIndex: 'F_Num',
        key: 'F_Num',
        tipLabel: '',
        required: true,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Num',
        isSystemControl: false,
      },

      {
        title: '单价(元)',
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
        title: '金额(元)',
        dataIndex: 'F_Amount',
        key: 'F_Amount',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '150px',
        fixed: false,
        formP: 'F_Amount',
        isSystemControl: false,
      },
      {
        title: '折扣(%)',
        dataIndex: 'F_Discount',
        key: 'F_Discount',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '100px',
        fixed: false,
        formP: 'F_Discount',
        isSystemControl: false,
      },
      {
        title: '优惠金额(元)',
        dataIndex: 'F_DiscountMoney',
        key: 'F_DiscountMoney',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_DiscountMoney',
        isSystemControl: false,
      },
      {
        title: '优惠后金额(元)',
        dataIndex: 'F_DiscountedAmount',
        key: 'F_DiscountedAmount',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_DiscountedAmount',
        isSystemControl: false,
        readonly: true,
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
    list = list.filter(o => hasFormP('tableFieldf01abd-' + o.formP));

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
  const gettableFieldf01abdRowSelection = computed(() => {
    const rowSelection = {
      selectedRowKeys: state.selectedtableFieldf01abdRowKeys,
      onChange: (selectedRowKeys: string[]) => {
        state.selectedtableFieldf01abdRowKeys = (selectedRowKeys || []).sort().reverse();
      },
    };
    return rowSelection;
  });
  const state = reactive<State>({
    dataForm: {
      id: '',
      F_OrderNo: undefined,
      F_SupplierId: undefined,
      F_Money: 0,
      F_PurchaseNum: 0,
      F_ProdPlanId: undefined,
      F_UserId: undefined,
      F_DeliveryDate: undefined,
      F_Description: undefined,
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
      tableFieldf01abd: [],
      tableField8b2a57: [],
    },
    tempGoodsSelector: [],
    tableFieldf01abdouterActiveKey: [0],
    tableFieldf01abdinnerActiveKey: [],
    tableFieldf01abdDefaultExpandAll: true,
    selectedtableFieldf01abdRowKeys: [],
    tableField8b2a57outerActiveKey: [0],
    tableField8b2a57innerActiveKey: [],
    tableField8b2a57DefaultExpandAll: true,
    selectedtableField8b2a57RowKeys: [],
    listPageSize: 20,
    listCurrentPage: 1,
    dataRule: {},
    optionsObj: {
      F_SupplierIdProps: { label: 'F_SupplierName', value: 'F_Id' },
      F_ProdPlanIdOptions: [
        {
          value: 'F_PlanNo',
          label: '计划编号',
        },
        {
          value: 'F_PlanName',
          label: '计划名称',
        },
        {
          value: 'F_SupplierId',
          label: '供应商',
        },
        {
          value: 'F_Money',
          label: '金额',
        },
        {
          value: 'F_PurchaseNum',
          label: '采购数量',
        },
      ],
      F_ProdPlanIdTemplateJson: [],
      tableFieldf01abd_F_CustomerIdOptions: [
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
        // {
        //   value: 'F_Unit',
        //   label: '单位',
        // },
        // {
        //   value: 'F_Specification',
        //   label: '规格',
        // },
        // {
        //   value: 'F_Image',
        //   label: '图片',
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
      tableFieldf01abd_F_CustomerIdTemplateJson: [],
      F_ProdPlanId: [],
    },
    currVmodel: '',
    currTableConf: {},
    tableRows: {
      tableFieldf01abd: {
        enabledmark: undefined,
        F_CustomerId: undefined,
        F_SupplierId: undefined,
        F_Num: undefined,
        F_NumLevel1: undefined,
        F_NumLevel2: undefined,
        F_Unit_Ratio: undefined,
        F_Price: undefined,
        F_Discount: undefined,
        F_DiscountMoney: undefined,
        F_Description: undefined,
        F_CreatorUserId: undefined,
        F_CreatorTime: undefined,
      },
      tableField8b2a57: {
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
    _incomingPrefill: null,
  });
  const { title, continueText, showContinueBtn, dataForm, submitType, dataRule, optionsObj, tempGoodsSelector } = toRefs(state);
  const globSetting = useGlobSetting();
  const apiUrl = globSetting.apiUrl;

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

  // 一级数量（如箱子）改变时，按比例计算二级数量（如盒）：二级 = 一级 * level2.qty
  function handleNumLevelChange(record) {
    const levels = getUnitRatioLevels(record);
    if (!levels) return;
    const qty1 = Number(record.F_NumLevel1) || 0;
    const ratio = Number(levels.level2?.qty) || 0;
    record.F_NumLevel2 = qty1 * ratio;
    record.F_Num = record.F_NumLevel1;
    recalcTableRow(record);
  }

  // 回显/回填时：F_Num 存第一框（一级数量），据此填充 F_NumLevel1 并计算 F_NumLevel2（与采购计划一致）
  function fillNumLevelsFromF_Num(row) {
    const levels = getUnitRatioLevels(row);
    if (!levels || row.F_Num == null || row.F_Num === '') return;
    const qty1 = Number(row.F_Num) || 0;
    const ratio = Number(levels.level2?.qty) || 0;
    row.F_NumLevel1 = qty1;
    row.F_NumLevel2 = qty1 * ratio;
  }

  // 监听主表供应商变化：同步到子表行
  watch(
    () => state.dataForm.F_SupplierId,
    newVal => {
      if (newVal && Array.isArray(state.dataForm.tableFieldf01abd)) {
        state.dataForm.tableFieldf01abd.forEach(row => {
          row.F_SupplierId = newVal;
        });
      }
    },
  );

  const getPrevDisabled = computed(() => state.currIndex === 0);
  const getNextDisabled = computed(() => state.currIndex === state.allList.length - 1);

  defineExpose({ init });

  function init(data) {
    state.submitType = 0;
    state.showContinueBtn = true;
    state.title = !data.id ? '新增采购单' : '编辑';
    state.continueText = !data.id ? '确定并新增' : '确定并继续';
    setFormProps({ continueLoading: false });
    state.dataForm.id = data.id;
    openModal();
    state.allList = data.allList;
    state.currIndex = state.allList.length && data.id ? state.allList.findIndex(item => item.id === data.id) : 0;
    nextTick(() => {
      getForm().resetFields();
      state.dataForm.tableFieldf01abd = [];
      state.dataForm.tableField8b2a57 = [];
      // 清空顶部商品选择器（确保每次打开新增表单时不保留上次选择）
      state.tempGoodsSelector = [];
      // 清空子表已选行
      state.selectedtableFieldf01abdRowKeys = [];
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
        F_OrderNo: undefined,
        F_SupplierId: undefined,
        F_Money: 0,
        F_PurchaseNum: 0,
        F_ProdPlanId: undefined,
        F_UserId: undefined,
        F_DeliveryDate: undefined,
        F_Description: undefined,
        F_CreatorUserId: undefined,
        F_CreatorTime: undefined,
        tableFieldf01abd: [],
        tableField8b2a57: [],
      };
      getF_SupplierIdOptions();
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
      for (let i = 0; i < state.dataForm.tableFieldf01abd.length; i++) {
        const element = state.dataForm.tableFieldf01abd[i];
        element.jnpfId = buildUUID();
        // 先根据 F_Num（一级数量）与 F_Unit_Ratio 填充两级输入框，再重算金额
        fillNumLevelsFromF_Num(element);
        recalcTableRow(element);
      }
      for (let i = 0; i < state.dataForm.tableField8b2a57.length; i++) {
        const element = state.dataForm.tableField8b2a57[i];
        element.jnpfId = buildUUID();
      }
      // 同步主表金额（子表优惠后金额之和）
      syncMoneySum();
      getF_SupplierIdOptions();
      // 将现有子表行的关联 id 同步到顶部选择器，保持 selector 与子表一致
      try {
        const remainingIds = (state.dataForm.tableFieldf01abd || [])
          .map((r: any) => {
            // 优先使用缓存的完整对象
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
      // Ensure display-only fields (商品编号/规格/图片) are populated for existing rows.
      populateMissingCustomerDisplayFields();
      changeLoading(false);
    });
  }
  /**
   * 重新计算子表单行金额、优惠后金额和优惠金额
   * 规则：
   *  - F_Amount = F_Price * F_Num
   *  - F_DiscountedAmount = F_Amount * F_Discount
   *  - F_DiscountMoney = F_Amount - F_DiscountedAmount
   */
  function recalcTableRow(record) {
    if (!record) return;
    const levels = getUnitRatioLevels(record);
    // 有两级单位时：F_Num 存第一框（一级数量）；二级数量仅展示/换算
    if (levels && record.F_NumLevel1 != null && record.F_NumLevel1 !== '') {
      const qty1 = Number(record.F_NumLevel1) || 0;
      const ratio = Number(levels.level2?.qty) || 0;
      record.F_NumLevel2 = qty1 * ratio;
      record.F_Num = record.F_NumLevel1;
    }
    const price = Number(record.F_Price) || 0;
    const num = levels ? Number(record.F_NumLevel1) || 0 : Number(record.F_Num) || 0;
    const amount = price * num;
    record.F_Amount = Number(amount.toFixed(2));
    // 采购计划回填等未填折扣时：优惠金额保持为空，优惠后金额=金额；若用户填了折扣则从折扣反算
    const discountInput = record.F_Discount != null && record.F_Discount !== '';
    let discountMoney;
    if (discountInput) {
      const discount = Number(record.F_Discount) || 0;
      const discountFactor = discount / 100;
      discountMoney = amount - amount * discountFactor;
    } else {
      discountMoney = Number(record.F_DiscountMoney) || 0;
    }
    if (discountMoney < 0) discountMoney = 0;
    if (discountMoney > amount) discountMoney = amount;
    record.F_DiscountMoney = discountMoney === 0 ? undefined : Number(discountMoney.toFixed(2));
    record.F_DiscountedAmount = Number((amount - discountMoney).toFixed(2));
    // 同步主表金额（子表优惠后金额之和）
    syncMoneySum();
    // 同步采购数量为子表数量之和（当编辑行的数量时也要更新总采购数）
    syncPurchaseNum();
  }
  /**
   * 当人工修改 优惠金额(F_DiscountMoney) 时，同步计算 优惠后金额 和 折扣 字段
   */
  function onDiscountMoneyChange(record) {
    if (!record) return;
    const rawVal = Number(record.F_DiscountMoney) || 0;
    // 计算金额优先尝试使用已存在的 F_Amount，否则用单价*数量（有双单位时用一级数量）
    const levels = getUnitRatioLevels(record);
    const amount = Number(record.F_Amount) || (Number(record.F_Price) || 0) * (levels ? Number(record.F_NumLevel1) || 0 : Number(record.F_Num) || 0);
    let val = rawVal;
    if (val < 0) val = 0;
    if (val > amount) val = amount;
    record.F_DiscountMoney = Number(val.toFixed(2));
    const discountedAmount = amount - record.F_DiscountMoney;
    record.F_DiscountedAmount = Number(discountedAmount.toFixed(2));

    if (amount > 0) {
      record.F_Discount = Number(((discountedAmount / amount) * 100).toFixed(2));
    } else {
      record.F_Discount = 0;
    }
    // 同步主表金额（子表优惠后金额之和）
    syncMoneySum();
  }
  /**
   * 将子表 `tableFieldf01abd` 的 `F_Amount` 求和并写入主表 `F_Money`
   */
  function syncMoneySum() {
    try {
      const list = state.dataForm.tableFieldf01abd || [];
      let sum = 0;
      for (let i = 0; i < list.length; i++) {
        const v = Number(list[i].F_Amount) || 0;
        sum += v;
      }
      // 保持两位小数
      state.dataForm.F_Money = Number(sum.toFixed(2));
    } catch (e) {
      // 容错，不应阻塞业务
    }
  }
  async function handleSubmit() {
    try {
      const values = await getForm()?.validate();
      if (!values) return;
      // 验证子表数量不能为空
      const rows = state.dataForm.tableFieldf01abd || [];
      for (let i = 0; i < rows.length; i++) {
        if (rows[i].F_NumLevel1 == null || rows[i].F_NumLevel1 === '') {
          createMessage.warning(`第${i + 1}行数量不能为空`);
          return;
        }
      }
      setFormProps({ confirmLoading: true });
      // clone and strip system-controlled display fields from rows before submit
      const payload = cloneDeep(state.dataForm);
      try {
        const cols: any[] = unref(tableFieldf01abdColumns) || [];
        if (Array.isArray(payload.tableFieldf01abd)) {
          payload.tableFieldf01abd = payload.tableFieldf01abd.map((row: any) => {
            const newRow: any = { ...row };
            for (let i = 0; i < cols.length; i++) {
              const cur = cols[i];
              if (cur.jnpfKey === 'complexHeader' && Array.isArray(cur.children)) {
                for (let j = 0; j < cur.children.length; j++) {
                  const child = cur.children[j];
                  if (child.isSystemControl) delete newRow[child.key];
                }
              } else {
                if (cur.isSystemControl) delete newRow[cur.key];
              }
            }
            return newRow;
          });
        }
      } catch (e) {
        // ignore transform errors and fall back to raw payload
      }

      const formMethod = payload.id ? update : create;
      formMethod(payload)
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
  /**
   * 生产计划选择变化：从后端获取该计划的物料与数量并填充到子表
   * @param selectedProdPlanId
   */
  function handleProdPlanChange(val, selectedProdPlanId) {
    // 清空原有子表
    state.dataForm.tableFieldf01abd = [];
    state.selectedtableFieldf01abdRowKeys = [];
    state.tempGoodsSelector = [];
    if (!selectedProdPlanId) {
      syncPurchaseNum();
      syncMoneySum();
      return;
    }
    // 调用后端接口获取物料列表
    getProdPlanItems(selectedProdPlanId.F_Id)
      .then(resp => {
        const list = (resp && resp.data) || resp || [];
        const ids: string[] = [];
        list.forEach((it: any) => {
          const newRow: any = {
            jnpfId: buildUUID(),
            F_CustomerId: it.F_GoodsId ?? it.F_GoodsId,
            F_SupplierId: state.dataForm.F_SupplierId || it.F_SupplierId,
            F_Num: it.F_Num ?? 0,
            F_NumLevel1: undefined,
            F_NumLevel2: undefined,
            F_Unit_Ratio: undefined,
            F_Price: it.F_Price ?? 0,
            F_Discount: undefined,
            F_DiscountMoney: undefined,
            F_Description: undefined,
            F_CreatorUserId: undefined,
            F_CreatorTime: undefined,
            _F_CustomerObj: null,
          };
          state.dataForm.tableFieldf01abd.push(newRow);
          fillNumLevelsFromF_Num(newRow);
          recalcTableRow(newRow);
          if (newRow.F_CustomerId) ids.push(newRow.F_CustomerId);
        });
        // 更新顶部选择器
        state.tempGoodsSelector = ids;
        // 填充子表展示字段（商品编号/规格/图片）
        populateMissingCustomerDisplayFields();
        syncPurchaseNum();
        syncMoneySum();
      })
      .catch(err => {
        console.error('getProdPlanItems error', err);
        try {
          createMessage.error(typeof err === 'object' ? JSON.stringify(err) : String(err));
        } catch (e) {
          // ignore
        }
        syncPurchaseNum();
        syncMoneySum();
      });
  }
  function removetableFieldf01abdRow(index, showConfirm = false) {
    const doRemove = () => {
      state.dataForm.tableFieldf01abd.splice(index, 1);
      // 同步采购数量为子表行数
      syncPurchaseNum();
      // 同步主表金额
      syncMoneySum();
      // 同步顶部选择器，保留剩余行的 relation id
      try {
        const remainingIds = (state.dataForm.tableFieldf01abd || [])
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
  // 将主表采购数量同步为子表数量之和（有双单位时按一级数量汇总）
  function syncPurchaseNum() {
    try {
      const list = state.dataForm.tableFieldf01abd || [];
      let sum = 0;
      for (let i = 0; i < list.length; i++) {
        const row = list[i];
        const levels = getUnitRatioLevels(row);
        const v = levels ? Number(row.F_NumLevel1) || 0 : Number(row.F_Num) || 0;
        sum += v;
      }
      state.dataForm.F_PurchaseNum = sum;
    } catch (e) {
      // ignore
    }
  }
  function addForSelect(data) {
    for (let i = 0; i < data.length; i++) {
      let item = { ...state.tableRows[state.currVmodel], ...data[i], jnpfId: buildUUID() };
      state.dataForm[state.currVmodel].push(cloneDeep(item));
      if (state[state.currVmodel + 'DefaultExpandAll']) state[state.currVmodel + 'innerActiveKey'].push(item.jnpfId);
    }
    // 如果是向采购商品子表添加，更新采购数量
    if (state.currVmodel === 'tableFieldf01abd') {
      syncPurchaseNum();
      // 同步主表金额（子表优惠后金额之和）
      syncMoneySum();
      // 同步顶部批量选择器的值以保持与子表一致
      try {
        const remainingIds = (state.dataForm.tableFieldf01abd || [])
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

  /**
   * 顶部商品选择器变更处理：把选择的商品生成子表行
   * selected 可能是对象或数组（multiple 情况），也可能是 id 或 id 数组
   */
  function handleGoodsSelectorChange(selected: any, option) {
    if (!selected) {
      /* 兜底 */
      if (!Array.isArray(state.dataForm.tableFieldf01abd)) {
        state.dataForm.tableFieldf01abd = [];
      }
      const optionIdSet = new Set(option.map(o => o.F_Id));
      /* 1. 删掉已取消勾选的行 */
      state.dataForm.tableFieldf01abd = state.dataForm.tableFieldf01abd.filter(item => optionIdSet.has(item.F_CustomerId));
      state.tempGoodsSelector = state.tempGoodsSelector.filter(item => optionIdSet.has(item));
      syncPurchaseNum();
      syncMoneySum();
      return;
    }

    // 清空现有子表数据
    // state.dataForm.tableFieldf01abd = [];
    // state.selectedtableFieldf01abdRowKeys = [];

    const interfaceId = '2008721559174385664';

    const buildAndPushRow = (data: any, index: number) => {
      let relationId: any = data;
      let fullObj: any = null;
      if (data && typeof data === 'object') {
        fullObj = data;
        relationId = data.F_Id ?? data.FId ?? data.id ?? data.value ?? relationId;
      }
      const exist = state.dataForm.tableFieldf01abd.some(item => item.F_CustomerId === data.F_Id);
      if (!exist) {
        const newRow: any = {
          jnpfId: buildUUID(),
          F_CustomerId: relationId,
          F_SupplierId: state.dataForm.F_SupplierId || undefined,
          F_Num: undefined,
          F_NumLevel1: undefined,
          F_NumLevel2: undefined,
          F_Unit_Ratio: undefined,
          F_Price: undefined,
          F_Discount: undefined,
          F_DiscountMoney: undefined,
          F_Description: undefined,
          F_CreatorUserId: undefined,
          F_CreatorTime: undefined,
          _F_CustomerObj: fullObj,
        };
        // 如果拿到完整对象，尝试填充展示字段（例如商品编号/规格/图片）
        if (fullObj) {
          handleCustomerSelectChange(fullObj, newRow);
        }
        state.dataForm.tableFieldf01abd.push(newRow);
        // 初始化计算
        recalcTableRow(newRow);
      }
    };

    const itemsAreObjects = (arr: any[]) => {
      if (!arr || !arr.length) return false;
      const first = arr[0];
      return first && typeof first === 'object' && (first.F_Id || first.F_GoodsName || first.F_GoodsNo);
    };

    if (Array.isArray(selected)) {
      if (itemsAreObjects(selected)) {
        selected.forEach((goods, index) => buildAndPushRow(goods, index));
        state.tempGoodsSelector = selected.slice();
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
            const list = (resp && resp.data) || [];
            const map: Record<string, any> = {};
            list.forEach((item: any) => {
              if (item && item.F_Id != null) map[String(item.F_Id)] = item;
            });
            for (let i = 0; i < ids.length; i++) {
              const id = ids[i];
              const found = map[String(id)];
              buildAndPushRow(found || id, i);
            }
            state.tempGoodsSelector = ids.slice();
            syncPurchaseNum();
            syncMoneySum();
          })
          .catch(err => {
            console.error('getDataInterfaceDataInfoByIds error', err);
            try {
              createMessage.error(typeof err === 'object' ? JSON.stringify(err) : String(err));
            } catch (e) {
              // ignore
            }
            // 回退使用原 id
            selected.forEach((goods, index) => buildAndPushRow(goods, index));
            state.tempGoodsSelector = selected.slice();
            syncPurchaseNum();
            syncMoneySum();
          });
        return;
      }
    } else {
      const single = selected;
      if (single && typeof single === 'object') {
        buildAndPushRow(single, 0);
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
            const found = (resp && resp.data && resp.data[0]) || null;
            buildAndPushRow(found || id, 0);
            state.tempGoodsSelector = [found || id];
            syncPurchaseNum();
            syncMoneySum();
          })
          .catch(err => {
            console.error('getDataInterfaceDataInfoByIds error', err);
            try {
              createMessage.error(typeof err === 'object' ? JSON.stringify(err) : String(err));
            } catch (e) {
              // ignore
            }
            buildAndPushRow(id, 0);
            state.tempGoodsSelector = [id];
            syncPurchaseNum();
            syncMoneySum();
          });
        return;
      }
    }
    // 如果已经同步添加完行，则同步数值
    syncPurchaseNum();
    syncMoneySum();
  }

  /**
   * Map fields from a fetched/selected product object into a table row's display fields.
   * Keeps mapping logic centralized so both selector and load-time code can reuse it.
   */
  function handleCustomerSelectChange(selected: any, record: any) {
    const data = Array.isArray(selected) ? selected[0] : selected;
    if (!data) {
      record.F_GoodsNo = undefined;
      record.F_Specification = undefined;
      record.F_Image = undefined;
      record.F_Unit_Ratio = undefined;
      record.F_NumLevel1 = undefined;
      record.F_NumLevel2 = undefined;
      return;
    }
    record.F_GoodsNo = data.F_GoodsNo ?? data.GoodsNo ?? data.F_GoodsName ?? data.name ?? record.F_GoodsNo;
    record.F_Specification = data.F_Specification ?? data.Specification ?? data.spec ?? record.F_Specification;
    record.F_Unit_Ratio = data.F_Unit_Ratio ?? record.F_Unit_Ratio;
    let imgVal = data.F_Image ?? data.Image ?? data.img ?? data.picture ?? data.fileUrl ?? data.url ?? data.imageUrl ?? null;
    try {
      if (typeof imgVal === 'string' && imgVal.trim().startsWith('[')) {
        const parsed = JSON.parse(imgVal);
        if (Array.isArray(parsed) && parsed.length) {
          const first = parsed[0];
          imgVal = first.url ?? first.fileUrl ?? first.value ?? first.thumbUrl ?? first.name ?? imgVal;
        }
      } else if (Array.isArray(imgVal) && imgVal.length) {
        const first = imgVal[0];
        imgVal = first.url ?? first.fileUrl ?? first.value ?? first.thumbUrl ?? first.name ?? imgVal[0];
      }
    } catch (e) {
      // ignore parse errors
    }
    if (!imgVal) {
      record.F_Image = undefined;
    } else {
      if (/^https?:\/\//i.test(String(imgVal))) {
        record.F_Image = String(imgVal);
      } else {
        const s = String(imgVal);
        record.F_Image = s.startsWith(apiUrl) ? s : apiUrl.replace(/\/$/, '') + (s.startsWith('/') ? '' : '/') + s;
      }
    }
  }

  /**
   * Populate missing display fields (F_GoodsNo, F_Specification, F_Image) for rows that only have F_CustomerId.
   * This will batch-fetch data from the data interface and map results back into rows.
   */
  async function populateMissingCustomerDisplayFields() {
    try {
      const rows = state.dataForm.tableFieldf01abd || [];
      const idsToFetch: string[] = [];
      for (let i = 0; i < rows.length; i++) {
        const r = rows[i];
        if (!r) continue;
        const hasDisplay = r.F_GoodsNo || r.F_Specification || r.F_Image;
        if (!hasDisplay && r.F_CustomerId) {
          if (typeof r.F_CustomerId === 'object') {
            const id = r.F_CustomerId.F_Id ?? r.F_CustomerId.id ?? r.F_CustomerId.value;
            if (id) idsToFetch.push(String(id));
          } else {
            idsToFetch.push(String(r.F_CustomerId));
          }
        }
      }
      if (!idsToFetch.length) return;
      const uniqueIds = Array.from(new Set(idsToFetch));
      const interfaceId = '2008721559174385664';
      const query = {
        ids: uniqueIds,
        interfaceId,
        propsValue: 'F_Id',
        relationField: 'F_GoodsName',
        paramList: [],
      };
      const resp = await getDataInterfaceDataInfoByIds(interfaceId, query);
      const list = (resp && resp.data) || [];
      const map: Record<string, any> = {};
      list.forEach((item: any) => {
        if (item && item.F_Id != null) map[String(item.F_Id)] = item;
      });
      for (let i = 0; i < rows.length; i++) {
        const r = rows[i];
        if (!r) continue;
        const id =
          r && r.F_CustomerId && typeof r.F_CustomerId === 'object' ? r.F_CustomerId.F_Id ?? r.F_CustomerId.id ?? r.F_CustomerId.value : r.F_CustomerId;
        if (id) {
          const found = map[String(id)];
          if (found) handleCustomerSelectChange(found, r);
          fillNumLevelsFromF_Num(r);
          recalcTableRow(r);
        }
      }
    } catch (e) {
      // ignore errors to avoid blocking UI
    }
  }
  const listPagination = computed(() => ({
    current: state.listCurrentPage,
    pageSize: state.listPageSize,
    size: 'small',
    showSizeChanger: true,
    pageSizeOptions: [20, 50, 100],
    showTotal: (total: number) => `共 ${total} 条`,
    onShowSizeChange: (_current: number, size: number) => {
      state.listPageSize = size;
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
