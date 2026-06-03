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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_QuoteCode')">
            <a-form-item name="F_QuoteCode" :labelCol="{ style: { width: '100px' } }">
              <template #label>报价单号</template>
              <JnpfInput
                v-model:value="dataForm.F_QuoteCode"
                placeholder="请填写，忽略将自动生成"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_CustomerId')">
            <a-form-item name="F_CustomerId" :labelCol="{ style: { width: '100px' } }">
              <template #label>客户</template>
              <JnpfSelect
                v-model:value="dataForm.F_CustomerId"
                @change="handleCustomerSelectChange"
                placeholder="请选择"
                :options="optionsObj.F_CustomerIdOptions"
                :fieldNames="optionsObj.F_CustomerIdProps"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_ContactId')">
            <a-form-item name="F_ContactId" :labelCol="{ style: { width: '100px' } }">
              <template #label>联系人</template>
              <JnpfPopupSelect
                v-model:value="dataForm.F_ContactId"
                :formData="state.dataForm"
                placeholder="请选择"
                :templateJson="optionsObj.F_ContactIdTemplateJson"
                allowClear
                field="F_ContactId"
                interfaceId="2009459664370143232"
                :columnOptions="optionsObj.F_ContactIdOptions"
                relationField="F_ContactName"
                propsValue="F_Id"
                :pageSize="20"
                popupType="dialog"
                popupTitle="选择数据"
                popupWidth="800px"
                hasPage
                :style="{
                  width: '100%',
                }"
                :extraOptions="optionsObj.F_ContactId" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_AddressId')">
            <a-form-item name="F_AddressId" :labelCol="{ style: { width: '100px' } }">
              <template #label>客户地址</template>
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_QuoteAmount')">
            <a-form-item name="F_QuoteAmount" :labelCol="{ style: { width: '100px' } }">
              <template #label>报价金额(元)</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_QuoteAmount"
                placeholder="请输入"
                :controls="false"
                :disabled="true"
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_GoodsTotal')">
            <a-form-item name="F_GoodsTotal" :labelCol="{ style: { width: '100px' } }">
              <template #label>商品总数</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_GoodsTotal"
                placeholder="请输入"
                :min="0"
                :controls="false"
                :disabled="true"
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_QuoteDate')">
            <a-form-item name="F_QuoteDate" :labelCol="{ style: { width: '100px' } }">
              <template #label>报价日期</template>
              <JnpfDatePicker
                v-model:value="dataForm.F_QuoteDate"
                placeholder="请选择"
                format="yyyy-MM-dd"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_SalesmanId')">
            <a-form-item name="F_SalesmanId" :labelCol="{ style: { width: '100px' } }">
              <template #label>业务员</template>
              <JnpfUserSelect
                v-model:value="dataForm.F_SalesmanId"
                placeholder="请选择"
                selectType="all"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <!-- <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_QuoteStatus')">
            <a-form-item name="F_QuoteStatus" :labelCol="{ style: { width: '100px' } }">
              <template #label>报价状态</template>
              <JnpfInput
                v-model:value="dataForm.F_QuoteStatus"
                placeholder="请输入"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col> -->
          <!-- <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_AuditStatus')">
            <a-form-item name="F_AuditStatus" :labelCol="{ style: { width: '100px' } }">
              <template #label>审核状态</template>
              <JnpfInput
                v-model:value="dataForm.F_AuditStatus"
                placeholder="请输入"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col> -->
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
            <a-form-item name="GoodsList" :labelCol="{ style: { width: '100px' } }">
              <template #label>选择商品</template>
              <JnpfPopupMultipleSelect
                v-model:value="dataForm.GoodsList"
                placeholder="请选择"
                :formData="state.dataForm"
                :templateJson="optionsObj.GoodsListTemplateJson"
                allowClear
                field="GoodsList"
                interfaceId="2029803158963884032"
                :columnOptions="optionsObj.GoodsListOptions"
                relationField="F_GoodsName"
                propsValue="id"
                :pageSize="20"
                popupType="dialog"
                popupTitle="选择数据"
                popupWidth="1000px"
                @change="GoodsListBtn"
                hasPage
                :style="{
                  width: '100%',
                }"
                :extraOptions="optionsObj.GoodsList" />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="选择报价单商品" :bordered="false" />
              <a-table
                :data-source="dataForm.tableFielddc29f7"
                :columns="tableFielddc29f7Columns"
                size="small"
                rowKey="jnpfId"
                :pagination="false"
                :scroll="{ x: 'max-content' }"
                :rowSelection="gettableFielddc29f7RowSelection">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                  <template v-if="column.key === 'F_CustomerId'">
                    <JnpfPopupSelect
                      v-model:value="record.F_CustomerId"
                      placeholder="请选择"
                      :templateJson="optionsObj.tableFielddc29f7_F_CustomerIdTemplateJson"
                      allowClear
                      @change="(val, row) => handleChildCustomerSelectChange(record, val, row)"
                      :field="'F_CustomerId' + index"
                      interfaceId="2008721559174385664"
                      :columnOptions="optionsObj.tableFielddc29f7_F_CustomerIdOptions"
                      relationField="F_GoodsName"
                      propsValue="F_Id"
                      :pageSize="20"
                      disabled
                      popupType="dialog"
                      popupTitle="选择数据"
                      popupWidth="800px"
                      :style="{
                        width: '100%',
                      }"
                      :extraOptions="optionsObj.tableFielddc29f7_F_CustomerId" />
                  </template>
                  <template v-if="column.key === 'F_GoodsNo'">
                    <span class="jnpf-table-readonly">{{ record.F_GoodsNo }}</span>
                  </template>
                  <template v-if="column.key === 'F_Specification'">
                    <span class="jnpf-table-readonly">{{ record.F_Specification }}</span>
                  </template>
                  <template v-if="column.key === 'F_SaleQty'">
                    <JnpfInputNumber
                      v-model:value="record.F_SaleQty"
                      placeholder="请输入"
                      :min="1"
                      :controls="false"
                      @change="() => recalcRowAmount(record)"
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_Category'">
                    <JnpfCascader
                      v-model:value="record.F_Category"
                      placeholder="请选择"
                      :options="optionsObj.tableFielddc29f7_F_CategoryOptions"
                      :fieldNames="optionsObj.tableFielddc29f7_F_CategoryProps"
                      allowClear
                      showSearch
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_Price'">
                    <JnpfInputNumber
                      v-model:value="record.F_Price"
                      placeholder="请输入"
                      :controls="false"
                      @change="() => recalcRowAmount(record)"
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_Discount'">
                    <JnpfInputNumber
                      v-model:value="record.F_Discount"
                      placeholder="请输入"
                      :controls="false"
                      :min="0"
                      :max="100"
                      @change="() => recalcRowAmount(record)"
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_DiscountAmount'">
                    <span class="jnpf-table-readonly">{{ record.F_DiscountAmount }}</span>
                  </template>
                  <template v-if="column.key === 'F_AmountAfterDiscount'">
                    <span class="jnpf-table-readonly">{{ record.F_AmountAfterDiscount }}</span>
                  </template>
                  <template v-if="column.key === 'F_Files'">
                    <JnpfUploadFile
                      v-model:value="record.F_Files"
                      buttonText="点击上传"
                      pathType="defaultPath"
                      :fileSize="10"
                      sizeUnit="MB"
                      :limit="9"
                      :sortRule="[]"
                      timeFormat="YYYY" />
                  </template>
                  <template v-if="column.key === 'F_Description'">
                    <JnpfInput
                      v-model:value="record.F_Description"
                      placeholder="请输入"
                      allowClear
                      :style="{
                        width: '100%',
                      }"
                      :showCount="false" />
                  </template>
                  <template v-if="column.key === 'action'">
                    <a-space>
                      <a-button class="action-btn" type="link" color="error" @click="removetableFielddc29f7Row(index, true)" size="small">{{
                        t('common.delText', '删除')
                      }}</a-button>
                    </a-space>
                  </template>
                </template>
              </a-table>
            </a-form-item>
          </a-col>
          <!-- <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_CreatorUserId')">
            <a-form-item name="F_CreatorUserId" :labelCol="{ style: { width: '100px' } }">
              <template #label>创建人员</template>
              <JnpfOpenData
                v-model:value="dataForm.F_CreatorUserId"
                type="currUser"
                readonly
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_CreatorTime')">
            <a-form-item name="F_CreatorTime" :labelCol="{ style: { width: '100px' } }">
              <template #label>创建时间</template>
              <JnpfOpenData
                v-model:value="dataForm.F_CreatorTime"
                type="currTime"
                readonly
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col> -->
        </a-row>
      </a-form>
    </a-row>
  </BasicModal>
  <SelectModal :config="state.currTableConf" :formData="state.dataForm" ref="selectModal" @select="addForSelect" />
</template>
<script lang="ts" setup>
  import { create, update, getInfo, getDefaultsByCustomer } from './helper/api';


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
    dataForm: any;
    dataRule: any;
    optionsObj: any;
    currVmodel: any;
    currTableConf: any;
    addTableConf: any;
    tableRows: any;
    tableFielddc29f7outerActiveKey: number[];
    tableFielddc29f7innerActiveKey: string[];
    tableFielddc29f7DefaultExpandAll: boolean;
    selectedtableFielddc29f7RowKeys: any[];
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
  const tableFielddc29f7Columns: any[] = computed(() => {
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
        title: '编号',
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
        isSystemControl: false,
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
        isSystemControl: false,
      },
      {
        title: '数量',
        dataIndex: 'F_SaleQty',
        key: 'F_SaleQty',
        tipLabel: '',
        required: true,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '100px',
        fixed: false,
        formP: 'F_SaleQty',
        isSystemControl: false,
      },
      {
        title: '类别',
        dataIndex: 'F_Category',
        key: 'F_Category',
        tipLabel: '',
        required: true,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Category',
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
        dataIndex: 'F_DiscountAmount',
        key: 'F_DiscountAmount',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_DiscountAmount',
        isSystemControl: false,
      },
      {
        title: '折后金额(元)',
        dataIndex: 'F_AmountAfterDiscount',
        key: 'F_AmountAfterDiscount',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_AmountAfterDiscount',
        isSystemControl: false,
      },
      {
        title: '附件',
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
      {
        title: '附件',
        dataIndex: 'F_Files',
        key: 'F_Files',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Files',
        isSystemControl: false,
      },
      {
        title: '商品备注',
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
    // 保留系统字段始终显示，其他字段按权限过滤
    list = list.filter(o => hasFormP('tableFielddc29f7-' + o.formP));
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
  const gettableFielddc29f7RowSelection = computed(() => {
    const rowSelection = {
      selectedRowKeys: state.selectedtableFielddc29f7RowKeys,
      onChange: (selectedRowKeys: string[]) => {
        state.selectedtableFielddc29f7RowKeys = (selectedRowKeys || []).sort().reverse();
      },
    };
    return rowSelection;
  });
  const state = reactive<State>({
    dataForm: {
      id: '',
      GoodsList: [],
      F_QuoteCode: undefined,
      F_CustomerId: undefined,
      F_ContactId: undefined,
      F_AddressId: undefined,
      F_QuoteAmount: undefined,
      F_GoodsTotal: undefined,
      F_DeliveryDate: undefined,
      F_QuoteDate: undefined,
      F_SalesmanId: userInfo.userId ? userInfo.userId : '',
      F_QuoteStatus: undefined,
      F_AuditStatus: undefined,
      F_Description: undefined,
      tableFielddc29f7: [],
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
    },
    tableFielddc29f7outerActiveKey: [0],
    tableFielddc29f7innerActiveKey: [],
    tableFielddc29f7DefaultExpandAll: true,
    selectedtableFielddc29f7RowKeys: [],
    dataRule: {
      F_CustomerId: [
        {
          required: true,
          message: '请输入客户',
          trigger: 'change',
        },
      ],
      F_SalesmanId: [
        {
          required: true,
          message: '请输入业务员',
          trigger: 'change',
        },
      ],
    },
    optionsObj: {
      F_CustomerIdProps: { label: 'F_CustomerName', value: 'F_Id' },
      F_ContactIdOptions: [
        {
          value: 'F_ContactName',
          label: '姓名',
        },
        {
          value: 'F_ContactPhone',
          label: '电话',
        },
      ],
      F_ContactIdTemplateJson: [
        {
          defaultValue: '',
          field: 'Id',
          dataType: 'varchar',
          required: 0,
          fieldName: '',
          // relationField will be set dynamically based on selected customer (F_CustomerId)
          relationField: '',
          jnpfKey: null,
          complexHeaderList: null,
          sourceType: 2,
          isChildren: false,
          IsMultiple: false,
        },
      ],
      F_AddressIdProps: { label: 'F_Address', value: 'F_Id' },
      tableFielddc29f7_F_CustomerIdOptions: [
        {
          value: 'F_GoodsNo',
          label: '编号',
        },
        {
          value: 'F_GoodsName',
          label: '商品名称',
        },
        // {
        //   value: 'F_Unit',
        //   label: '单位',
        // },
        {
          value: 'F_Specification',
          label: '规则',
        },
        {
          value: 'F_SalePrice',
          label: '销售单价',
        },
        {
          value: 'F_CostPrice',
          label: '成本单价',
        },
      ],
      tableFielddc29f7_F_CustomerIdTemplateJson: [],
      GoodsListOptions: [
        { value: 'F_GoodsName', label: '商品名' },
        { value: 'F_GoodsNo', label: '商品编号' },
        { value: 'F_Specification', label: '商品规格' },
        { value: 'F_Type', label: '商品类型' },
        { value: 'F_Remark', label: '备注' },
      ],
      GoodsListTemplateJson: [],
      GoodsList: [],
      tableFielddc29f7_F_CategoryOptions: [],
      tableFielddc29f7_F_CategoryProps: {
        label: 'F_Name',
        value: 'F_Id',
        children: 'children',
      },
      F_ContactId: [],
    },
    currVmodel: '',
    currTableConf: {},
    tableRows: {
      tableFielddc29f7: {
        enabledmark: undefined,
        F_CustomerId: undefined,
        F_GoodsNo: undefined,
        F_Specification: undefined,
        F_Price: undefined,
        F_SaleQty: 1,
        F_Category: [],
        F_Discount: undefined,
        F_DiscountAmount: undefined,
        F_AmountAfterDiscount: undefined,
        F_Description: undefined,
        F_Files: [],
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
      state.dataForm.tableFielddc29f7 = [];
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
        GoodsList: [],
        F_QuoteCode: undefined,
        F_CustomerId: undefined,
        F_ContactId: undefined,
        F_AddressId: undefined,
        F_QuoteAmount: undefined,
        F_GoodsTotal: undefined,
        F_DeliveryDate: undefined,
        F_QuoteDate: undefined,
        F_SalesmanId: userInfo.userId ? userInfo.userId : '',
        F_QuoteStatus: '0',
        F_AuditStatus: undefined,
        F_Description: undefined,
        tableFielddc29f7: [],
        F_CreatorUserId: undefined,
        F_CreatorTime: undefined,
      };
      getF_CustomerIdOptions();
      getF_AddressIdOptions();
      gettableFielddc29f7_F_CategoryOptions();
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
      // 确保子表存在
      state.dataForm.tableFielddc29f7 = Array.isArray(state.dataForm.tableFielddc29f7) ? state.dataForm.tableFielddc29f7 : [];
      for (let i = 0; i < state.dataForm.tableFielddc29f7.length; i++) {
        const element = state.dataForm.tableFielddc29f7[i];
        // 唯一 id（用于前端行操作）
        element.jnpfId = buildUUID();

        // 构建大小写不敏感的字段映射，方便兼容后端不同命名
        const lowerMap: Record<string, any> = {};
        for (const k in element) {
          if (Object.prototype.hasOwnProperty.call(element, k)) {
            lowerMap[k.toLowerCase()] = element[k];
          }
        }

        // 解析附件/图片类字段：后端可能返回 F_Files 或 F_Image 等
        try {
          if (
            (element.F_Files && typeof element.F_Files === 'string') ||
            (!element.F_Files && lowerMap['f_files'] && typeof lowerMap['f_files'] === 'string')
          ) {
            const raw = element.F_Files && typeof element.F_Files === 'string' ? element.F_Files : lowerMap['f_files'];
            element.F_Files = JSON.parse(raw);
          } else if (!element.F_Files && lowerMap['f_image']) {
            const raw = lowerMap['f_image'];
            element.F_Files = typeof raw === 'string' ? JSON.parse(raw) : raw;
          }
        } catch (e) {
          element.F_Files = element.F_Files || [];
        }
        if (!element.F_Files) element.F_Files = [];

        // 计算展示金额（如果后端已返回则保留）
        const originalAmount = (Number(element.F_Price) || 0) * (Number(element.F_SaleQty) || 0);
        // 计算折后金额：如果 F_Discount 存在，按 折扣/100 计算；否则按原价
        const factor = element.F_Discount ? Number(element.F_Discount) / 100 : 1;
        element.F_AmountAfterDiscount =
          typeof element.F_AmountAfterDiscount !== 'undefined' ? element.F_AmountAfterDiscount : Math.round(originalAmount * factor * 100) / 100;
        // 计算优惠金额 = 原价 - 折后金额
        element.F_DiscountAmount =
          typeof element.F_DiscountAmount !== 'undefined'
            ? element.F_DiscountAmount
            : Math.round((originalAmount - (element.F_AmountAfterDiscount || 0)) * 100) / 100;
      }
      // 如果后端返回的子表只有关联 id，但缺少展示字段，尝试按商品接口逐条获取详情以填充展示字段
      const rows = state.dataForm.tableFielddc29f7 || [];
      const productInterfaceId = '2008721559174385664';
      try {
        await Promise.all(
          rows.map(async (row: any) => {
            if (!row) return;
            const needFetch = !row.F_GoodsNo && !row.F_Specification && row.F_CustomerId;
            if (!needFetch) {
              // 仍然触发金额重算以确保展示一致
              recalcRowAmount(row);
              return;
            }
            try {
              const templateJson = [
                {
                  defaultValue: row.F_CustomerId || '',
                  field: 'id',
                  dataType: 'varchar',
                  required: 0,
                  fieldName: '',
                  relationField: '',
                  jnpfKey: null,
                  complexHeaderList: null,
                  sourceType: 2,
                  isChildren: false,
                  IsMultiple: false,
                },
              ];
              const query = { paramList: getParamList(templateJson, state.dataForm) };
              const infoRes = await getDataInterfaceRes(productInterfaceId, query);
              const infoData = infoRes && infoRes.data ? infoRes.data : infoRes;
              const info = Array.isArray(infoData) ? infoData[0] : infoData;
              if (info) {
                row.F_GoodsNo = info.F_GoodsNo ?? row.F_GoodsNo;
                row.F_Specification = info.F_Specification ?? row.F_Specification;
                row.F_Unit = info.F_Unit ?? row.F_Unit;
                recalcRowAmount(row);
              }
            } catch (e) {
              // ignore single row fetch errors
            }
          }),
        );
      } catch (e) {
        // ignore overall fetch errors
      }
      getF_CustomerIdOptions();
      gettableFielddc29f7_F_CategoryOptions();
      // 如果有客户ID，更新关联字段并刷新地址选项
      if (state.dataForm.F_CustomerId) {
        state.optionsObj.F_ContactIdTemplateJson[0].relationField = state.dataForm.F_CustomerId;
        getF_AddressIdOptions();
      }
      changeLoading(false);
    });
  }
  async function handleSubmit() {
    try {
      const values = await getForm()?.validate();
      if (!values) return;
      setFormProps({ confirmLoading: true });
      const formMethod = state.dataForm.id ? update : create;
      // 复制一份数据并移除只作展示的字段，避免它们参与提交
      const payload = JSON.parse(JSON.stringify(state.dataForm));
      if (Array.isArray(payload.tableFielddc29f7)) {
        payload.tableFielddc29f7 = payload.tableFielddc29f7.map((row: any) => {
          const { F_GoodsNo, F_Specification, F_AmountAfterDiscount, F_DiscountAmount, F_Unit, ...rest } = row || {};
          return rest;
        });
      }
      delete payload.GoodsList;
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
  function getF_CustomerIdOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2009458830353764352', query).then(res => {
      let data = res.data;
      state.optionsObj.F_CustomerIdOptions = Array.isArray(data) ? data : [];
    });
  }
  function getF_AddressIdOptions() {
    let templateJson = [
      {
        defaultValue: '',
        field: 'id',
        dataType: 'varchar',
        required: 0,
        fieldName: '',
        // use the same relationField as the contact template (customer id)
        relationField: state.dataForm.F_CustomerId || '',
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
  // 子表内选择商品时，填充展示字段（商品编号、单位、规则），只用于页面展示
  function handleChildCustomerSelectChange(record: any, val: any, selectedData: any) {
    try {
      const id = record.F_CustomerId;
      if (!id) {
        record.F_GoodsNo = undefined;
        record.F_Specification = undefined;
        return;
      }
      record.F_GoodsNo = selectedData.F_GoodsNo;
      record.F_Specification = selectedData.F_Specification;
      recalcRowAmount(record);
    } catch (e) {
      // ignore
    }
  }
  // 监听客户选择，动态设置联系人组件使用的 relationField（用于请求参数）
  watch(
    () => state.dataForm.F_CustomerId,
    val => {
      try {
        if (!state.optionsObj.F_ContactIdTemplateJson || !state.optionsObj.F_ContactIdTemplateJson.length) {
          state.optionsObj.F_ContactIdTemplateJson = [
            {
              defaultValue: '',
              field: 'Id',
              dataType: 'varchar',
              required: 0,
              fieldName: '',
              relationField: val || '',
              jnpfKey: null,
              complexHeaderList: null,
              sourceType: 2,
              isChildren: false,
              IsMultiple: false,
            },
          ];
        } else {
          state.optionsObj.F_ContactIdTemplateJson[0].relationField = val || '';
        }
        // 同步刷新地址联动查询的 relationField 并重新加载地址选项
        try {
          state.optionsObj.F_AddressIdProps; // noop to avoid unused warning, actual call below
          getF_AddressIdOptions();
        } catch (e) {
          // ignore
        }
      } catch (e) {
        // ignore
      }
    },
  );

  // 批量选择商品回调（参考合同 GoodsListBtn）
  function GoodsListBtn(val, option) {
    if (!Array.isArray(state.dataForm.tableFielddc29f7)) {
      state.dataForm.tableFielddc29f7 = [];
    }
    const optionIdSet = new Set(option.map(o => o.id));
    // 删除已取消勾选的行
    state.dataForm.tableFielddc29f7 = state.dataForm.tableFielddc29f7.filter(item => optionIdSet.has(item.F_CustomerId));
    // 追加新选中的行
    option.forEach(o => {
      const exist = state.dataForm.tableFielddc29f7.findIndex(item => item.F_CustomerId === o.id) !== -1;
      if (!exist) {
        const newRow = {
          jnpfId: buildUUID(),
          F_CustomerId: o.id,
          F_GoodsNo: o.F_GoodsNo,
          F_Specification: o.F_Specification,
          F_Category: o.F_CategoryId ? JSON.parse(o.F_CategoryId) : [],
          F_Price: undefined,
          F_SaleQty: 1,
          F_Discount: undefined,
          F_DiscountAmount: undefined,
          F_AmountAfterDiscount: undefined,
          F_Description: undefined,
          F_Files: [],
        };
        state.dataForm.tableFielddc29f7.push(newRow);
        state.tableFielddc29f7innerActiveKey.push(newRow.jnpfId);
      }
    });
  }

  // 重新计算单行金额（参考合同 recalcRowAmount）
  function recalcRowAmount(record: any) {
    try {
      const qty = Number(record.F_SaleQty) || 0;
      const discountInput = Number(record.F_Discount) || 100;
      const discountMultiplier = Number.isFinite(discountInput) && !Number.isNaN(discountInput) ? discountInput / 100 : 1;
      const unitPrice = Number(record.F_Price) || 0;
      const originalAmount = unitPrice * qty;
      const amountAfter = originalAmount * discountMultiplier;
      record.F_AmountAfterDiscount = Number.isFinite(amountAfter) ? parseFloat(amountAfter.toFixed(2)) : 0;
      const discountAmount = originalAmount - record.F_AmountAfterDiscount;
      record.F_DiscountAmount = Number.isFinite(discountAmount) ? parseFloat(discountAmount.toFixed(2)) : 0;
    } catch (e) {
      // ignore
    }
  }

  function gettableFielddc29f7_F_CategoryOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2008414575283802112', query).then(res => {
      let data = res.data;
      state.optionsObj.tableFielddc29f7_F_CategoryOptions = Array.isArray(data) ? data : [];
    });
  }

  // 当子表选择报价单商品发生变化时，自动设置商品总数（禁用显示）
  watch(
    () => state.dataForm.tableFielddc29f7,
    list => {
      try {
        // 计算销售数量总和（F_SaleQty），保留两位小数
        const totalSaleQty = (Array.isArray(list) ? list : []).reduce((acc: number, row: any) => {
          const qty = Number(row.F_SaleQty) || 0;
          return acc + (isNaN(qty) ? 0 : qty);
        }, 0);
        state.dataForm.F_GoodsTotal = Math.round(totalSaleQty * 100) / 100;
        // 计算子表折后金额合计并赋值到报价金额（F_QuoteAmount）
        try {
          const sum = (Array.isArray(list) ? list : []).reduce((acc: number, row: any) => {
            const amount =
              typeof row.F_AmountAfterDiscount !== 'undefined' && row.F_AmountAfterDiscount !== null
                ? Number(row.F_AmountAfterDiscount)
                : (Number(row.F_Price) || 0) * (Number(row.F_SaleQty) || 0);
            return acc + (isNaN(amount) ? 0 : amount);
          }, 0);
          state.dataForm.F_QuoteAmount = Math.round(sum * 100) / 100;
        } catch (e) {
          // ignore
        }
      } catch (e) {
        // ignore
      }
    },
    { deep: true, immediate: true },
  );

  // 仅当用户在界面上选择客户时调用：请求后端返回默认联系人/地址并赋值到表单
  function handleCustomerSelectChange(val: any) {
    try {
      // relationField 已由 watch 保证，这里确保地址选项即时刷新
      getF_AddressIdOptions();
      if (val) {
        getDefaultsByCustomer(val).then(async (res: any) => {
          // defHttp usually returns { code, msg, data }, accept either shape
          const payload = res && res.data ? res.data : res;
          const data = payload || {};
          // 联系人：用后端返回的数组覆盖 extraOptions，然后在下一 tick 设置 v-model
          if (Array.isArray(data.contacts) && data.contacts.length) {
            const mappedContacts = data.contacts.map((c: any) => ({
              ...c,
              F_Id: c.F_Id || c.id,
              F_ContactName: c.F_ContactName || c.fullName || c.F_ContactName,
              id: c.F_Id || c.id,
              fullName: c.F_ContactName || c.fullName,
            }));
            state.optionsObj.F_ContactId = mappedContacts;
            await nextTick();
            state.dataForm.F_ContactId = mappedContacts[0].F_Id || mappedContacts[0].id;
          } else {
            state.optionsObj.F_ContactId = [];
            state.dataForm.F_ContactId = undefined;
          }
          // 地址：直接替换地址下拉数据，再在下一 tick 设置 v-model
          if (Array.isArray(data.addresses) && data.addresses.length) {
            const mappedAddrs = data.addresses.map((a: any) => ({
              ...a,
              F_Id: a.F_Id || a.id,
              F_Address: a.F_Address || a.address || a.F_Address,
              F_Region: a.F_Region || a.region || a.F_Region,
              id: a.F_Id || a.id,
            }));
            state.optionsObj.F_AddressIdOptions = mappedAddrs;
            await nextTick();
            state.dataForm.F_AddressId = mappedAddrs[0].F_Id || mappedAddrs[0].id;
          } else {
            state.optionsObj.F_AddressIdOptions = [];
            state.dataForm.F_AddressId = undefined;
          }
        });
      } else {
        state.optionsObj.F_ContactId = [];
        state.optionsObj.F_AddressIdOptions = [];
        state.dataForm.F_ContactId = undefined;
        state.dataForm.F_AddressId = undefined;
      }
    } catch (e) {
      // ignore
    }
  }

  function addtableFielddc29f7Row() {
    let item = {
      jnpfId: buildUUID(),
      F_CustomerId: undefined,
      F_GoodsNo: undefined,
      F_Specification: undefined,
      F_Price: undefined,
      F_SaleQty: 1,
      F_Category: [],
      F_Discount: undefined,
      F_DiscountAmount: undefined,
      F_AmountAfterDiscount: undefined,
      F_Description: undefined,
      F_Files: [],
    };
    state.dataForm.tableFielddc29f7.push(item);
    state.tableFielddc29f7innerActiveKey.push(item.jnpfId);
  }
  function removetableFielddc29f7Row(index, showConfirm = false) {
    if (showConfirm) {
      createConfirm({
        iconType: 'warning',
        title: '提示',
        content: '此操作将永久删除该数据, 是否继续?',
        onOk: () => {
          state.dataForm.tableFielddc29f7.splice(index, 1);
        },
      });
    } else {
      state.dataForm.tableFielddc29f7.splice(index, 1);
    }
  }
  function copytableFielddc29f7Row(index) {
    let item = cloneDeep(state.dataForm.tableFielddc29f7[index]);
    let copyData = {};
    for (let i = 0; i < unref(tableFielddc29f7Columns).length; i++) {
      const cur = unref(tableFielddc29f7Columns)[i];
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
    // 计算金额展示值
    recalcRowAmount(copyItem);
    state.dataForm.tableFielddc29f7.push(copyItem);
    state.tableFielddc29f7innerActiveKey.push(copyItem.jnpfId);
  }
  function batchRemovetableFielddc29f7Row(showConfirm = false) {
    if (!state.selectedtableFielddc29f7RowKeys.length) return createMessage.error('请选择一条数据');
    const handleBatchRemove = () => {
      state.dataForm.tableFielddc29f7 = state.dataForm.tableFielddc29f7.filter(o => !state.selectedtableFielddc29f7RowKeys.includes(o.jnpfId));
      nextTick(() => {
        state.selectedtableFielddc29f7RowKeys = [];
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
      // 将选择项中的展示字段填充到行（只做展示，不参与提交）
      if (data[i].F_GoodsNo) item.F_GoodsNo = data[i].F_GoodsNo;
      if (data[i].F_Specification) item.F_Specification = data[i].F_Specification;
      // 计算金额展示值
      recalcRowAmount(item);
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
</script>
