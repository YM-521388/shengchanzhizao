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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_ReturnInNo')">
            <a-form-item name="F_ReturnInNo" :labelCol="{ style: { width: '100px' } }">
              <template #label>销售退货单号</template>
              <JnpfInput
                v-model:value="dataForm.F_ReturnInNo"
                placeholder="请输入"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>

          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_ReturnInType')">
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_ContractId')">
            <a-form-item name="F_ContractId" :labelCol="{ style: { width: '100px' } }">
              <template #label>合同单号</template>
              <JnpfSelect
                v-model:value="dataForm.F_ContractId"
                @change="handleContractChange"
                placeholder="请选择"
                :options="optionsObj.F_ContractIdOptions"
                :fieldNames="optionsObj.F_ContractIdProps"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_ReturnInDate')">
            <a-form-item name="F_ReturnInDate" :labelCol="{ style: { width: '100px' } }">
              <template #label>退货日期</template>
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
                popupWidth="50%"
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
                :style="{
                  width: '100%',
                }"
                :extraOptions="optionsObj.F_ContactId" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_AddressId')">
            <a-form-item name="F_AddressId" :labelCol="{ style: { width: '100px' } }">
              <template #label>地址</template>
              <JnpfPopupSelect
                v-model:value="dataForm.F_AddressId"
                :formData="state.dataForm"
                placeholder="请选择"
                :templateJson="optionsObj.F_AddressIdTemplateJson"
                allowClear
                field="F_AddressId"
                interfaceId="2009527238009163776"
                :columnOptions="optionsObj.F_AddressIdOptions"
                relationField="F_Address"
                propsValue="F_Id"
                :pageSize="20"
                popupType="dialog"
                popupTitle="选择数据"
                popupWidth="800px"
                :style="{
                  width: '100%',
                }"
                :extraOptions="optionsObj.F_AddressId" />
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
          <a-col :span="12" class="ant-col-item">
            <a-form-item :labelCol="{ style: { width: '100px' } }">
              <template #label>批量入库仓库</template>
              <a-space>
                <JnpfCascader
                  v-model:value="batchWarehouseValue"
                  placeholder="请选择仓库"
                  :options="optionsObj.tableFieldcfb049_F_WarehouseIDOptions"
                  :fieldNames="optionsObj.tableFieldcfb049_F_WarehouseIDProps"
                  allowClear
                  showSearch
                  changeOnSelect
                  :style="{
                    width: '300px',
                  }" />
                <a-button type="primary" size="small" @click="handleBatchAssignWarehouse" :disabled="!state.selectedtableFieldcfb049RowKeys.length">
                  批量赋值
                </a-button>
                <span class="text-gray-400 text-xs" v-if="state.selectedtableFieldcfb049RowKeys.length">
                  (已选 {{ state.selectedtableFieldcfb049RowKeys.length }} 条)
                </span>
              </a-space>
            </a-form-item>
          </a-col>
          <!-- <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_CreatorUserId')">
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
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_CreatorTime')">
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
          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="商品" :bordered="false" />
              <a-table
                :data-source="dataForm.tableFieldcfb049"
                :columns="tableFieldcfb049Columns"
                size="small"
                rowKey="jnpfId"
                :pagination="false"
                :scroll="{ x: 'max-content' }"
                :rowSelection="gettableFieldcfb049RowSelection">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                  <template v-if="column.key === 'F_CustomerId'">
                    <JnpfPopupSelect
                      v-model:value="record.F_CustomerId"
                      placeholder="请选择"
                      :templateJson="optionsObj.tableFieldcfb049_F_CustomerIdTemplateJson"
                      allowClear
                      :disabled="true"
                      :field="'F_CustomerId' + index"
                      interfaceId="2008721559174385664"
                      :columnOptions="optionsObj.tableFieldcfb049_F_CustomerIdOptions"
                      relationField="F_GoodsName"
                      propsValue="F_Id"
                      :pageSize="20"
                      popupType="dialog"
                      popupTitle="选择数据"
                      popupWidth="800px"
                      :style="{
                        width: '100%',
                      }"
                      :extraOptions="optionsObj.tableFieldcfb049_F_CustomerId" />
                  </template>
                  <template v-if="column.key === 'F_Encoding'">
                    <div class="stock-encoding-group" :class="{ 'stock-encoding-group--prefixed': !!getGoodsNoPrefix(record) }">
                      <span v-if="getGoodsNoPrefix(record)" class="stock-encoding-prefix">{{ getGoodsNoPrefix(record) }}</span>
                      <div class="stock-encoding-input-wrap">
                        <JnpfInput
                          v-model:value="record._F_Encoding_Suffix"
                          placeholder="请输入"
                          allowClear
                          :style="{
                            width: '100%',
                          }"
                          :showCount="false"
                          @change="handleEncodingChange(record)" />
                      </div>
                    </div>
                  </template>
                  <template v-if="column.key === 'F_GoodsNo'">
                    <span class="display-text">{{ record.F_GoodsNo }}</span>
                  </template>
                  <template v-if="column.key === 'F_Specification'">
                    <span class="display-text">{{ record.F_Specification }}</span>
                  </template>
                  <template v-if="column.key === 'F_Num'">
                    <JnpfInputNumber
                      v-model:value="record.F_Num"
                      placeholder="请输入"
                      :controls="false"
                      :style="{
                        width: '100%',
                      }" />
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
                  <template v-if="column.key === 'F_Amount'">
                    <span>{{ thousandsFormat((record.F_Num || 0) * (record.F_Price || 0)) }}</span>
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
                      :options="optionsObj.tableFieldcfb049_F_WarehouseIDOptions"
                      :fieldNames="optionsObj.tableFieldcfb049_F_WarehouseIDProps"
                      :changeOnSelect="true"
                      allowClear
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
                  <template v-if="column.key === 'action'">
                    <a-space>
                      <!-- <a-button class="action-btn" type="link" @click="copytableFieldcfb049Row(index)" size="small">{{
                        t('common.copyText', '复制')
                      }}</a-button> -->
                      <a-button class="action-btn" type="link" color="error" @click="removetableFieldcfb049Row(index, true)" size="small">{{
                        t('common.delText', '删除')
                      }}</a-button>
                    </a-space>
                  </template>
                </template>
              </a-table>
              <a-space class="input-table-footer-btn">
                <!-- <a-button type="primary" preIcon="icon-ym icon-ym-btn-add" @click="addtableFieldcfb049Row">{{ t('common.add1Text', '添加') }}</a-button>
                <a-button type="danger" preIcon="icon-ym icon-ym-btn-clearn" @click="batchRemovetableFieldcfb049Row(true)">{{
                  t('common.batchDelText', '批量删除')
                }}</a-button> -->
              </a-space>
            </a-form-item>
          </a-col>
          <!-- <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="操作日志" :bordered="false" />
              <a-table
                :data-source="dataForm.tableField0c9c23"
                :columns="tableField0c9c23Columns"
                size="small"
                rowKey="jnpfId"
                :pagination="false"
                :scroll="{ x: 'max-content' }"
                :rowSelection="gettableField0c9c23RowSelection">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
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
                    <JnpfInput
                      v-model:value="record.F_Content"
                      placeholder="请输入"
                      allowClear
                      :style="{
                        width: '100%',
                      }"
                      :showCount="false" />
                  </template>
                  <template v-if="column.key === 'F_Reason'">
                    <JnpfTextarea
                      v-model:value="record.F_Reason"
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
                      <a-button class="action-btn" type="link" @click="copytableField0c9c23Row(index)" size="small">{{
                        t('common.copyText', '复制')
                      }}</a-button>
                      <a-button class="action-btn" type="link" color="error" @click="removetableField0c9c23Row(index, true)" size="small">{{
                        t('common.delText', '删除')
                      }}</a-button>
                    </a-space>
                  </template>
                </template>
              </a-table>
              <a-space class="input-table-footer-btn">
                <a-button type="primary" preIcon="icon-ym icon-ym-btn-add" @click="addtableField0c9c23Row">{{ t('common.add1Text', '添加') }}</a-button>
                <a-button type="danger" preIcon="icon-ym icon-ym-btn-clearn" @click="batchRemovetableField0c9c23Row(true)">{{
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
  import { create, update, getInfo, getContractItemsByContractId, getContractRelationById, getMaxSeq } from './helper/api';
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
    dataForm: any;
    dataRule: any;
    optionsObj: any;
    currVmodel: any;
    currTableConf: any;
    addTableConf: any;
    tableRows: any;
    tableFieldcfb049outerActiveKey: number[];
    tableFieldcfb049innerActiveKey: string[];
    tableFieldcfb049DefaultExpandAll: boolean;
    selectedtableFieldcfb049RowKeys: any[];
    tableField0c9c23outerActiveKey: number[];
    tableField0c9c23innerActiveKey: string[];
    tableField0c9c23DefaultExpandAll: boolean;
    selectedtableField0c9c23RowKeys: any[];
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
  const tableFieldcfb049Columns: any[] = computed(() => {
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
      {
        title: '销售单价(元)',
        dataIndex: 'F_Price',
        key: 'F_Price',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Price',
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
    // 强制显示这些字段（不受权限控制）
    const _alwaysShowTableFieldcfb049 = ['F_Amount', 'F_GoodsNo', 'F_Specification'];
    list = list.filter(o => _alwaysShowTableFieldcfb049.includes(o.formP) || hasFormP('tableFieldcfb049-' + o.formP));
    // 插入只读的销售金额列，不受权限控制，展示 F_Num * F_Price
    const amountColumn = {
      title: '销售金额(元)',
      dataIndex: 'F_Amount',
      key: 'F_Amount',
      tipLabel: '',
      required: false,
      align: 'left',
      span: '24',
      labelWidth: '',
      width: '',
      fixed: false,
      formP: 'F_Amount',
      isSystemControl: false,
    };
    const priceIndex = list.findIndex(o => o.dataIndex === 'F_Price');
    if (priceIndex >= 0) {
      list.splice(priceIndex + 1, 0, amountColumn);
    } else {
      list.push(amountColumn);
    }
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
  const gettableFieldcfb049RowSelection = computed(() => {
    const rowSelection = {
      selectedRowKeys: state.selectedtableFieldcfb049RowKeys,
      onChange: (selectedRowKeys: string[]) => {
        state.selectedtableFieldcfb049RowKeys = (selectedRowKeys || []).sort().reverse();
      },
    };
    return rowSelection;
  });
  const tableField0c9c23Columns: any[] = computed(() => {
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
    list = list.filter(o => hasFormP('tableField0c9c23-' + o.formP));
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
  const gettableField0c9c23RowSelection = computed(() => {
    const rowSelection = {
      selectedRowKeys: state.selectedtableField0c9c23RowKeys,
      onChange: (selectedRowKeys: string[]) => {
        state.selectedtableField0c9c23RowKeys = (selectedRowKeys || []).sort().reverse();
      },
    };
    return rowSelection;
  });
  const state = reactive<State>({
    dataForm: {
      id: '',
      F_ReturnInNo: undefined,
      F_WarehouseId: [],
      F_ReturnInType: undefined,
      F_ContractId: undefined,
      F_ReturnInDate: dayjs(new Date()).startOf(getDateTimeUnit('yyyy-MM-dd')).valueOf(),
      F_CustomerId: undefined,
      F_ContactId: undefined,
      F_AddressId: undefined,
      F_Description: undefined,
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
      tableFieldcfb049: [],
      tableField0c9c23: [],
    },
    tableFieldcfb049outerActiveKey: [0],
    tableFieldcfb049innerActiveKey: [],
    tableFieldcfb049DefaultExpandAll: true,
    selectedtableFieldcfb049RowKeys: [],
    tableField0c9c23outerActiveKey: [0],
    tableField0c9c23innerActiveKey: [],
    tableField0c9c23DefaultExpandAll: true,
    selectedtableField0c9c23RowKeys: [],
    batchWarehouseValue: undefined,
    dataRule: {
      F_WarehouseId: [
        {
          required: true,
          message: '请输入仓库',
          trigger: 'change',
        },
      ],
      F_ContractId: [
        {
          required: true,
          message: '请输入合同单号',
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
      F_ReturnInTypeProps: { label: 'fullName', value: 'id' },
      F_ContractIdProps: { label: 'F_ContractCode', value: 'F_Id' },
      F_CustomerIdOptions: [
        {
          value: 'F_CustomerName',
          label: '客户姓名',
        },
        {
          value: 'F_CustomerCode',
          label: '客户编号',
        },
      ],
      F_CustomerIdTemplateJson: [],
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
      // Use selected customer's F_Id (dataForm.F_CustomerId) as the relationField so
      // dependent popup selects will receive the chosen customer's id as the parameter.
      F_ContactIdTemplateJson: [
        {
          defaultValue: '',
          field: 'Id',
          dataType: 'varchar',
          required: 0,
          fieldName: '',
          // changed from fixed id to dynamic field key
          relationField: 'F_CustomerId',
          jnpfKey: null,
          complexHeaderList: null,
          // set to 1 so getParamList will pick up relationField and set defaultValue
          sourceType: 1,
          isChildren: false,
          IsMultiple: false,
        },
      ],
      F_AddressIdOptions: [
        {
          value: 'F_Address',
          label: '地址',
        },
      ],
      F_AddressIdTemplateJson: [
        {
          defaultValue: '',
          field: 'id',
          dataType: 'varchar',
          required: 0,
          fieldName: '',
          // changed from fixed id to dynamic field key
          relationField: 'F_CustomerId',
          jnpfKey: null,
          complexHeaderList: null,
          // set to 1 so getParamList will pick up relationField and set defaultValue
          sourceType: 1,
          isChildren: false,
          IsMultiple: false,
        },
      ],
      tableFieldcfb049_F_CustomerIdOptions: [
        {
          value: 'F_GoodsNo',
          label: '商品编号',
        },
        {
          value: 'F_GoodsName',
          label: '商品名称',
        },
        {
          value: 'F_Unit',
          label: '单位',
        },
        {
          value: 'F_SalePrice',
          label: '销售单价',
        },
        {
          value: 'F_Specification',
          label: '规格',
        },
        {
          value: 'F_CostPrice',
          label: '成本单价',
        },
        {
          value: 'F_Source',
          label: '商品来源',
        },
        {
          value: 'F_Remark',
          label: '备注',
        },
      ],
      tableFieldcfb049_F_CustomerIdTemplateJson: [],
      tableFieldcfb049_F_WarehouseIDOptions: [],
      tableFieldcfb049_F_WarehouseIDProps: {
        label: 'F_Name',
        value: 'F_Id',
        children: 'children',
      },
      F_CustomerId: [],
      F_ContactId: [],
      F_AddressId: [],
    },
    currVmodel: '',
    currTableConf: {},
    tableRows: {
      tableFieldcfb049: {
        enabledmark: undefined,
        F_CustomerId: undefined,
        F_Encoding: undefined,
        _F_Encoding_Suffix: undefined,
        _F_CustomerObj: undefined,
        F_Num: undefined,
        F_Price: undefined,
        F_WarehouseID: [],
        F_Description: undefined,
        F_CreatorUserId: undefined,
        F_CreatorTime: undefined,
      },
      tableField0c9c23: {
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

  const getPrevDisabled = computed(() => state.currIndex === 0);
  const getNextDisabled = computed(() => state.currIndex === state.allList.length - 1);
  const isEditMode = computed(() => !!state.dataForm.id);

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
      state.dataForm.tableFieldcfb049 = [];
      state.dataForm.tableField0c9c23 = [];
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
        F_ContractId: undefined,
        F_ReturnInDate: dayjs(new Date()).startOf(getDateTimeUnit('yyyy-MM-dd')).valueOf(),
        F_CustomerId: undefined,
        F_ContactId: undefined,
        F_AddressId: undefined,
        F_Description: undefined,
        F_CreatorUserId: undefined,
        F_CreatorTime: undefined,
        tableFieldcfb049: [],
        tableField0c9c23: [],
      };
      getF_WarehouseIdOptions();
      getF_ReturnInTypeOptions();
      getF_ContractIdOptions();
      gettableFieldcfb049_F_WarehouseIDOptions();
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
      for (let i = 0; i < state.dataForm.tableFieldcfb049.length; i++) {
        const element = state.dataForm.tableFieldcfb049[i];
        element.jnpfId = buildUUID();
        // 加载已有行时尝试填充展示字段（商品编号/规格）
        try {
          fillRowDisplayFields(element);
        } catch (e) {
          // ignore
        }
      }
      await enrichTableRowsGoodsMeta(state.dataForm.tableFieldcfb049);
      for (let i = 0; i < state.dataForm.tableFieldcfb049.length; i++) {
        const element = state.dataForm.tableFieldcfb049[i];
        if (element.F_Encoding) {
          const prefix = getGoodsNoPrefix(element);
          if (prefix && element.F_Encoding.startsWith(prefix)) {
            element._F_Encoding_Suffix = element.F_Encoding.substring(prefix.length);
          } else {
            element._F_Encoding_Suffix = element.F_Encoding;
          }
        } else {
          handleEncodingChange(element);
        }
      }
      for (let i = 0; i < state.dataForm.tableField0c9c23.length; i++) {
        const element = state.dataForm.tableField0c9c23[i];
        element.jnpfId = buildUUID();
      }
      getF_WarehouseIdOptions();
      getF_ReturnInTypeOptions();
      gettableFieldcfb049_F_WarehouseIDOptions();
      getF_ContractIdOptions();
      changeLoading(false);
    });
  }
  async function handleSubmit() {
    try {
      for (const row of state.dataForm.tableFieldcfb049 || []) {
        if (getGoodsNoPrefix(row) || row._F_Encoding_Suffix !== undefined) {
          handleEncodingChange(row);
        }
      }

      // 验证子表 F_WarehouseID 是否全部填写
      const tableFieldcfb049 = state.dataForm.tableFieldcfb049 || [];
      if (tableFieldcfb049.length > 0) {
        const emptyWarehouseRows = tableFieldcfb049.filter((row: any) => {
          const warehouseId = row.F_WarehouseID;
          return !warehouseId || (Array.isArray(warehouseId) && warehouseId.length === 0);
        });
        if (emptyWarehouseRows.length > 0) {
          createMessage.warning(`商品列表中有 ${emptyWarehouseRows.length} 条数据的入库仓库未填写`);
          return;
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
  function getF_ReturnInTypeOptions() {
    getDictionaryDataSelector('2012074650393251840').then(res => {
      state.optionsObj.F_ReturnInTypeOptions = res.data.list;
    });
  }
  function gettableFieldcfb049_F_WarehouseIDOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2021045201115680768', query).then(res => {
      let data = res.data;
      state.optionsObj.tableFieldcfb049_F_WarehouseIDOptions = Array.isArray(data) ? data : [];
    });
  }
  function getF_ContractIdOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2010889611072638976', query).then(res => {
      let data = res.data;
      state.optionsObj.F_ContractIdOptions = Array.isArray(data) ? data : [];
    });
  }

  // 合同选择变更后处理：调用后端接口获取合同明细并填充到子表，同时获取合同关联（客户/联系人/地址）并填充表单字段
  const goodsSeqCounter: Record<string, number> = {};

  function handleContractChange(selected: any) {
    // 先清空现有商品行
    state.dataForm.tableFieldcfb049 = [];
    state.selectedtableFieldcfb049RowKeys = [];

    // 清空关联字段（以防选择为空或响应为空）
    state.dataForm.F_CustomerId = undefined;
    state.dataForm.F_ContactId = undefined;
    state.dataForm.F_AddressId = undefined;

    if (!selected) {
      return;
    }

    // 获取合同明细项并填充子表
    getContractItemsByContractId(String(selected))
      .then(async res => {
        const list = (res && res.data) || [];
        if (!Array.isArray(list) || list.length === 0) return;
        list.forEach((item: any) => {
          const row: any = {
            jnpfId: buildUUID(),
            F_CustomerId: item.F_CustomerId ?? undefined,
            F_Encoding: undefined,
            _F_Encoding_Suffix: undefined,
            _F_CustomerObj: undefined,
            F_Num: item.F_SaleQty ?? undefined,
            F_Price: item.F_Price ?? undefined,
            F_Description: undefined,
            F_CreatorUserId: undefined,
            F_CreatorTime: undefined,
          };
          state.dataForm.tableFieldcfb049.push(row);
          // 尝试填充展示字段（商品编号/规格）
          try {
            fillRowDisplayFields(row);
          } catch (e) {
            // ignore
          }
        });
        await enrichTableRowsGoodsMeta(state.dataForm.tableFieldcfb049);
        // 自动获取最大序号并赋值
        await autoAssignMaxSeq(state.dataForm.tableFieldcfb049);
      })
      .catch(err => {
        console.error('getContractItemsByContractId error', err);
      });

    // 同时调用后端接口获取合同关联（客户/联系人/地址）并填写到表单
    getContractRelationById(String(selected))
      .then(res => {
        const data = (res && res.data) || {};
        // 只在返回值存在时赋值，保持未返回字段为 undefined
        if (data.F_CustomerId !== undefined) state.dataForm.F_CustomerId = data.F_CustomerId;
        if (data.F_ContactId !== undefined) state.dataForm.F_ContactId = data.F_ContactId;
        if (data.F_AddressId !== undefined) state.dataForm.F_AddressId = data.F_AddressId;
      })
      .catch(err => {
        console.error('getContractRelationById error', err);
      });
  }
  function addtableFieldcfb049Row() {
    let item = {
      jnpfId: buildUUID(),
      F_CustomerId: undefined,
      F_Encoding: undefined,
      _F_Encoding_Suffix: undefined,
      _F_CustomerObj: undefined,
      F_Num: undefined,
      F_Price: undefined,
      F_Description: undefined,
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
    };
    state.dataForm.tableFieldcfb049.push(item);
    state.tableFieldcfb049innerActiveKey.push(item.jnpfId);
  }
  function removetableFieldcfb049Row(index, showConfirm = false) {
    if (showConfirm) {
      createConfirm({
        iconType: 'warning',
        title: '提示',
        content: '此操作将永久删除该数据, 是否继续?',
        onOk: () => {
          state.dataForm.tableFieldcfb049.splice(index, 1);
        },
      });
    } else {
      state.dataForm.tableFieldcfb049.splice(index, 1);
    }
  }
  function copytableFieldcfb049Row(index) {
    let item = cloneDeep(state.dataForm.tableFieldcfb049[index]);
    let copyData = {};
    for (let i = 0; i < unref(tableFieldcfb049Columns).length; i++) {
      const cur = unref(tableFieldcfb049Columns)[i];
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
    state.dataForm.tableFieldcfb049.push(copyItem);
    state.tableFieldcfb049innerActiveKey.push(copyItem.jnpfId);
  }
  function batchRemovetableFieldcfb049Row(showConfirm = false) {
    if (!state.selectedtableFieldcfb049RowKeys.length) return createMessage.error('请选择一条数据');
    const handleBatchRemove = () => {
      state.dataForm.tableFieldcfb049 = state.dataForm.tableFieldcfb049.filter(o => !state.selectedtableFieldcfb049RowKeys.includes(o.jnpfId));
      nextTick(() => {
        state.selectedtableFieldcfb049RowKeys = [];
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
  function addtableField0c9c23Row() {
    let item = {
      jnpfId: buildUUID(),
      F_Type: undefined,
      F_Title: undefined,
      F_Content: undefined,
      F_Reason: undefined,
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
    };
    state.dataForm.tableField0c9c23.push(item);
    state.tableField0c9c23innerActiveKey.push(item.jnpfId);
  }
  function removetableField0c9c23Row(index, showConfirm = false) {
    if (showConfirm) {
      createConfirm({
        iconType: 'warning',
        title: '提示',
        content: '此操作将永久删除该数据, 是否继续?',
        onOk: () => {
          state.dataForm.tableField0c9c23.splice(index, 1);
        },
      });
    } else {
      state.dataForm.tableField0c9c23.splice(index, 1);
    }
  }
  function copytableField0c9c23Row(index) {
    let item = cloneDeep(state.dataForm.tableField0c9c23[index]);
    let copyData = {};
    for (let i = 0; i < unref(tableField0c9c23Columns).length; i++) {
      const cur = unref(tableField0c9c23Columns)[i];
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
    state.dataForm.tableField0c9c23.push(copyItem);
    state.tableField0c9c23innerActiveKey.push(copyItem.jnpfId);
  }
  function batchRemovetableField0c9c23Row(showConfirm = false) {
    if (!state.selectedtableField0c9c23RowKeys.length) return createMessage.error('请选择一条数据');
    const handleBatchRemove = () => {
      state.dataForm.tableField0c9c23 = state.dataForm.tableField0c9c23.filter(o => !state.selectedtableField0c9c23RowKeys.includes(o.jnpfId));
      nextTick(() => {
        state.selectedtableField0c9c23RowKeys = [];
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
    // 如果是向子表添加，尝试填充展示字段
    if (state.currVmodel === 'tableFieldcfb049') {
      try {
        const list = state.dataForm.tableFieldcfb049 || [];
        for (let i = 0; i < list.length; i++) {
          fillRowDisplayFields(list[i]);
        }
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
   * 子表商品选择后填充只读展示字段（商品编号、规格）
   * 支持 option（完整对象）或仅 id 的情况
   */
  function handleTableCustomerSelectChange(val: any, option: any, record: any) {
    try {
      if (!val) {
        record.F_GoodsNo = undefined;
        record.F_Specification = undefined;
        return;
      }
      let info = Array.isArray(option) ? option[0] || {} : option || {};
      // 若组件没有返回完整对象，尝试从 extraOptions 缓存中查找
      if (!info || Object.keys(info).length === 0) {
        const opts = state.optionsObj.tableFieldcfb049_F_CustomerIdOptions || [];
        const found = opts.find((o: any) => o.F_Id === val || o.id === val || o.value === val);
        if (found) info = found;
      }
      record.F_GoodsNo = info.F_GoodsNo ?? info.GoodsNo ?? info.F_GoodsName ?? record.F_GoodsNo;
      record.F_Specification = info.F_Specification ?? info.Specification ?? info.spec ?? record.F_Specification;
    } catch (e) {
      // ignore
    }
  }

  /**
   * 加载已有行时填充展示字段（优先使用缓存的 extraOptions，否则按 id 拉取）
   */
  function fillRowDisplayFields(record: any) {
    try {
      const id = record.F_CustomerId;
      if (!id) return;
      const opts = state.optionsObj.tableFieldcfb049_F_CustomerIdOptions || [];
      const found = opts.find((o: any) => o.F_Id === id || o.id === id || o.value === id);
      if (found && Object.keys(found).length) {
        record.F_GoodsNo = found.F_GoodsNo ?? found.GoodsNo ?? record.F_GoodsNo;
        record.F_Specification = found.F_Specification ?? found.Specification ?? record.F_Specification;
        return;
      }
      // 回退：按 id 请求接口获取完整数据（interfaceId 与弹窗一致）
      try {
        const query = { paramList: [{ defaultValue: id, field: 'Id', dataType: 'varchar' }] };
        getDataInterfaceRes('2008721559174385664', query).then((res: any) => {
          const data = Array.isArray(res.data) ? res.data[0] : res.data;
          if (!data) return;
          record.F_GoodsNo = data.F_GoodsNo ?? data.GoodsNo ?? record.F_GoodsNo;
          record.F_Specification = data.F_Specification ?? data.Specification ?? record.F_Specification;
        });
      } catch (e) {
        // ignore
      }
    } catch (e) {
      // ignore
    }
  }

  // 监控子表变化：如果存在关联 id 但展示字段缺失，则尝试自动填充（批量查询提升性能）
  watch(
    () => state.dataForm.tableFieldcfb049,
    newVal => {
      try {
        if (!Array.isArray(newVal)) return;
        populateMissingCustomerDisplayFields();
      } catch (e) {
        // swallow
      }
    },
    { deep: true },
  );

  /**
   * 批量填充缺失的展示字段（F_GoodsNo、F_Specification、F_Image）
   * 优先从缓存选项中查找，剩余的通过批量接口查询
   */
  async function populateMissingCustomerDisplayFields() {
    try {
      const rows = state.dataForm.tableFieldcfb049 || [];
      const idsToFetch: string[] = [];

      for (let i = 0; i < rows.length; i++) {
        const r = rows[i];
        if (!r) continue;
        const hasDisplay = r.F_GoodsNo || r.F_Specification || r.F_Image;
        if (!hasDisplay && r.F_CustomerId) {
          // 先尝试从缓存选项中获取
          const opts = state.optionsObj.tableFieldcfb049_F_CustomerIdOptions || [];
          const found = opts.find((o: any) => o.F_Id === r.F_CustomerId || o.id === r.F_CustomerId || o.value === r.F_CustomerId);
          if (found && Object.keys(found).length) {
            r._F_CustomerObj = found;
            r.F_GoodsNo = found.F_GoodsNo ?? found.GoodsNo ?? found.F_GoodsName ?? r.F_GoodsNo;
            r.F_Specification = found.F_Specification ?? found.Specification ?? r.F_Specification;
            syncEncodingSuffixFromStoredValue(r);
          } else {
            // 缓存中没有，收集 ID 批量查询
            const id = typeof r.F_CustomerId === 'object' ? r.F_CustomerId.F_Id ?? r.F_CustomerId.id ?? r.F_CustomerId.value : r.F_CustomerId;
            if (id) idsToFetch.push(String(id));
          }
        }
      }

      if (!idsToFetch.length) return;

      // 批量查询接口
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

      // 将查询结果填充到对应行
      for (let i = 0; i < rows.length; i++) {
        const r = rows[i];
        if (!r) continue;
        const hasDisplay = r.F_GoodsNo || r.F_Specification || r.F_Image;
        if (!hasDisplay && r.F_CustomerId) {
          const id = typeof r.F_CustomerId === 'object' ? r.F_CustomerId.F_Id ?? r.F_CustomerId.id ?? r.F_CustomerId.value : r.F_CustomerId;
          if (!id) continue;
          const data = map[String(id)];
          if (data) {
            r._F_CustomerObj = data;
            r.F_GoodsNo = data.F_GoodsNo ?? data.GoodsNo ?? data.F_GoodsName ?? data.name ?? r.F_GoodsNo;
            r.F_Specification = data.F_Specification ?? data.Specification ?? data.spec ?? r.F_Specification;
            syncEncodingSuffixFromStoredValue(r);
          }
        }
      }
    } catch (e) {
      // ignore batch fetch errors
    }
  }

  /** 解析 F_CategoryId：JSON 数组字符串拼接，如 "[\"C\",\"A1\"]" -> "CA1" */
  function parseCategoryId(categoryId: any) {
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

  /** 库存编码前缀：parseCategoryId(F_CategoryId) + F_GoodsNo */
  function getGoodsNoPrefix(record: any) {
    let categoryPrefix = '';
    let goodsNo = '';
    if (record?._F_CustomerObj) {
      categoryPrefix = parseCategoryId(record._F_CustomerObj.F_CategoryId);
      goodsNo = record._F_CustomerObj.F_GoodsNo || '';
    } else if (record?.F_CustomerId && typeof record.F_CustomerId === 'object') {
      categoryPrefix = parseCategoryId(record.F_CustomerId.F_CategoryId);
      goodsNo = record.F_CustomerId.F_GoodsNo || '';
    }
    return categoryPrefix + goodsNo;
  }

  /** 前缀 + 用户输入后缀 -> F_Encoding */
  function handleEncodingChange(record: any) {
    const prefix = getGoodsNoPrefix(record);
    const suffix = record._F_Encoding_Suffix || '';
    record.F_Encoding = prefix ? prefix + suffix : suffix;
  }

  /** 已有 F_Encoding 时按当前前缀拆出可编辑后缀 */
  function syncEncodingSuffixFromStoredValue(record: any) {
    if (!record.F_Encoding) {
      handleEncodingChange(record);
      return;
    }
    const prefix = getGoodsNoPrefix(record);
    if (prefix && record.F_Encoding.startsWith(prefix)) {
      record._F_Encoding_Suffix = record.F_Encoding.substring(prefix.length);
    } else {
      record._F_Encoding_Suffix = record.F_Encoding;
    }
  }

  /** 按商品 id 拉取 InfoByIds 详情（含 F_CategoryId、F_GoodsNo），用于前缀 */
  async function enrichTableRowsGoodsMeta(rows: any[]) {
    if (!rows?.length) return;
    const rawIds = rows.map(r => r.F_CustomerId).filter(id => id != null && id !== '');
    const ids = [...new Set(rawIds.map((id: any) => String(id)))];
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
        }
      });
    } catch (e) {
      console.error('enrichTableRowsGoodsMeta', e);
    }
  }

  /**
   * 自动获取商品最大序号并赋值给子表行的 _F_Encoding_Suffix
   * 逻辑：去重商品 ID -> 调 GetMaxSeq -> 用返回的 MaxSeq 作为首行后缀；同一商品多行依次递增
   */
  async function autoAssignMaxSeq(rows: any[]) {
    if (!rows?.length) return;

    const goodsIds = [...new Set(rows.map(r => r.F_CustomerId).filter(id => id != null && id !== '').map((id: any) => String(id)))];
    if (!goodsIds.length) return;

    try {
      const res = await getMaxSeq(goodsIds);
      const maxSeqMap: Record<string, number> = {};
      let list: any[] = [];
      if (Array.isArray(res)) {
        list = res;
      } else if (res && Array.isArray((res as any).data)) {
        list = (res as any).data;
      }

      list.forEach((item: any) => {
        if (item?.F_GoodsId != null && item.F_GoodsId !== '') {
          const gid = String(item.F_GoodsId);
          maxSeqMap[gid] = typeof item.MaxSeq === 'number' ? item.MaxSeq : Number(item.MaxSeq) || 0;
        }
      });

      // 每个商品首行后缀 = 后端返回的下一个可用序号；同一商品多行时依次递增
      Object.keys(goodsSeqCounter).forEach(key => delete goodsSeqCounter[key]);
      goodsIds.forEach(id => {
        const gid = String(id);
        goodsSeqCounter[gid] = maxSeqMap[gid] ?? 0;
      });

      rows.forEach(row => {
        const gid = row.F_CustomerId != null && row.F_CustomerId !== '' ? String(row.F_CustomerId) : '';
        if (gid && goodsSeqCounter[gid] !== undefined) {
          row._F_Encoding_Suffix = goodsSeqCounter[gid];
          handleEncodingChange(row);
          goodsSeqCounter[gid]++;
        }
      });
    } catch (e) {
      console.error('autoAssignMaxSeq error:', e);
    }
  }

  // 批量赋值仓库给选中的子表行
  function handleBatchAssignWarehouse() {
    if (!state.batchWarehouseValue) {
      createMessage.warning('请先选择入库仓库');
      return;
    }
    if (!state.selectedtableFieldcfb049RowKeys.length) {
      createMessage.warning('请先勾选要赋值的子表数据');
      return;
    }
    const selectedKeys = state.selectedtableFieldcfb049RowKeys;
    let assignCount = 0;
    for (const row of state.dataForm.tableFieldcfb049) {
      if (selectedKeys.includes(row.jnpfId)) {
        row.F_WarehouseID = [...state.batchWarehouseValue];
        assignCount++;
      }
    }
    createMessage.success(`已成功赋值 ${assignCount} 条数据`);
  }
</script>
<style scoped lang="less">
  /* 库存编码：前缀（分类+商品编号）+ 可编辑后缀 */
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
