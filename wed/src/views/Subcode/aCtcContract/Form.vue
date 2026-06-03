<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    width="80%"
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_ContractCode')">
            <a-form-item name="F_ContractCode" :labelCol="{ style: { width: '100px' } }">
              <template #label>合同编号</template>
              <JnpfInput
                v-model:value="dataForm.F_ContractCode"
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
                showSearch
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_CustomerCode')">
            <a-form-item name="F_CustomerCode" :labelCol="{ style: { width: '100px' } }">
              <template #label>客户合同号</template>
              <JnpfInput
                v-model:value="dataForm.F_CustomerCode"
                placeholder="请填写"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_PrepayAmount')">
            <a-form-item name="F_PrepayAmount" :labelCol="{ style: { width: '100px' } }">
              <template #label>预付款</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_PrepayAmount"
                placeholder="0"
                :controls="false"
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_PaymentCycle')">
            <a-form-item name="F_PaymentCycle" :labelCol="{ style: { width: '100px' } }">
              <template #label>付款周期（天）</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_PaymentCycle"
                placeholder="请输入"
                :controls="false"
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_SaleTotal')">
            <a-form-item name="F_SaleTotal" :labelCol="{ style: { width: '100px' } }">
              <template #label>总数</template>
              <JnpfInputNumber
                :value="computedSaleTotal"
                placeholder="请输入"
                :controls="false"
                :disabled="true"
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_ContractAmount')">
            <a-form-item name="F_ContractAmount" :labelCol="{ style: { width: '100px' } }">
              <template #label>合同金额</template>
              <JnpfInputNumber
                :value="computedContractAmount"
                placeholder="请输入"
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_IsTaxable')">
            <a-form-item name="F_IsTaxable" :labelCol="{ style: { width: '100px' } }">
              <template #label>是否含税</template>
              <JnpfSwitch v-model:value="dataForm.F_IsTaxable" />
            </a-form-item>
          </a-col>
          <!-- <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_AuditStatus')">
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
              <JnpfGroupTitle content="选择合同商品" :bordered="false" />
              <a-table
                :data-source="dataForm.tableFieldf4dfaf"
                :columns="tableFieldf4dfafColumns"
                size="small"
                rowKey="jnpfId"
                :pagination="false"
                :scroll="{ x: 'max-content' }"
                :rowSelection="gettableFieldf4dfafRowSelection">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                  <template v-if="column.key === 'F_WorkOrderNo'">
                    <JnpfInput
                      v-model:value="record.F_WorkOrderNo"
                      placeholder="请填写，忽略将自动生成"
                      allowClear
                      :style="{
                        width: '100%',
                      }"
                      :showCount="false" />
                  </template>
                  <template v-if="column.key === 'F_CustomerId'">
                    <JnpfPopupSelect
                      v-model:value="record.F_CustomerId"
                      @change="(val, option) => handleTableCustomerSelectChange(val, option, record)"
                      placeholder="请选择"
                      :templateJson="optionsObj.tableFieldf4dfaf_F_CustomerIdTemplateJson"
                      allowClear
                      :field="'F_CustomerId' + index"
                      interfaceId="2008721559174385664"
                      :columnOptions="optionsObj.tableFieldf4dfaf_F_CustomerIdOptions"
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
                      :extraOptions="optionsObj.tableFieldf4dfaf_F_CustomerId" />
                  </template>
                  <template v-if="column.key === 'F_AmountAfterDiscount'">
                    <span class="jnpf-table-readonly">{{ record.F_AmountAfterDiscount }}</span>
                  </template>
                  <template v-if="column.key === 'F_DiscountAmount'">
                    <span class="jnpf-table-readonly">{{ record.F_DiscountAmount }}</span>
                  </template>
                  <template v-if="column.key === 'F_UnitTwo'">
                    <span>{{ record.F_UnitTwo }}</span>
                  </template>
                  <template v-if="column.key === 'F_GoodsNo'">
                    <span class="jnpf-table-readonly">{{ record.F_GoodsNo }}</span>
                  </template>
                  <template v-if="column.key === 'F_Specification'">
                    <span class="jnpf-table-readonly">{{ record.F_Specification }}</span>
                  </template>
                  <template v-if="column.key === 'F_PlanNum'">
                    <JnpfInputNumber
                      v-model:value="record.F_PlanNum"
                      placeholder="请输入"
                      :min="1"
                      :controls="false"
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_Category'">
                    <JnpfCascader
                      v-model:value="record.F_Category"
                      placeholder="请选择"
                      :options="optionsObj.tableFieldf4dfaf_F_CategoryOptions"
                      :fieldNames="optionsObj.tableFieldf4dfaf_F_CategoryProps"
                      allowClear
                      showSearch
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_RoutingId'">
                    <JnpfSelect
                      v-model:value="record.F_RoutingId"
                      placeholder="请选择"
                      :options="optionsObj.tableFieldf4dfaf_F_RoutingIdOptions"
                      :fieldNames="optionsObj.tableFieldf4dfaf_F_RoutingIdProps"
                      allowClear
                      showSearch
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_DeliveryDate'">
                    <JnpfDatePicker
                      v-model:value="record.F_DeliveryDate"
                      placeholder="请选择"
                      format="yyyy-MM-dd"
                      allowClear
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_Priority'">
                    <JnpfSelect
                      v-model:value="record.F_Priority"
                      placeholder="请选择"
                      :options="optionsObj.tableFieldf4dfaf_F_PriorityOptions"
                      :fieldNames="optionsObj.tableFieldf4dfaf_F_PriorityProps"
                      allowClear
                      showSearch
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_BomId'">
                    <JnpfSelect
                      v-model:value="record.F_BomId"
                      placeholder="请选择"
                      :options="record.tableFieldf4dfaf_F_BomIdOptions"
                      :fieldNames="optionsObj.tableFieldf4dfaf_F_BomIdProps"
                      allowClear
                      showSearch
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_DrawingFiles'">
                    <JnpfUploadFile
                      v-model:value="record.F_DrawingFiles"
                      buttonText="点击上传"
                      pathType="selfPath"
                      :isAccount="-1"
                      folder=""
                      :fileSize="10"
                      sizeUnit="MB"
                      :limit="9"
                      :sortRule="['1', '2']"
                      timeFormat="YYYYMMDD" />
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
                  <template v-if="column.key === 'F_SaleQty'">
                    <JnpfInputNumber
                      v-model:value="record.F_SaleQty"
                      placeholder="请输入"
                      :step="1.0"
                      :controls="true"
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
                  <template v-if="column.key === 'F_DoorPlateThickness'">
                    <JnpfSelect
                      v-model:value="record.F_DoorPlateThickness"
                      placeholder="请选择"
                      :options="optionsObj.tableFieldf4dfaf_F_DoorPlateThicknessOptions"
                      :fieldNames="optionsObj.tableFieldf4dfaf_F_DoorPlateThicknessProps"
                      allowClear
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_BoxPlateThickness'">
                    <JnpfSelect
                      v-model:value="record.F_BoxPlateThickness"
                      placeholder="请选择"
                      :options="optionsObj.tableFieldf4dfaf_F_BoxPlateThicknessOptions"
                      :fieldNames="optionsObj.tableFieldf4dfaf_F_BoxPlateThicknessProps"
                      allowClear
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_InstallOrBeam'">
                    <JnpfSelect
                      v-model:value="record.F_InstallOrBeam"
                      placeholder="请选择"
                      :options="optionsObj.tableFieldf4dfaf_F_InstallOrBeamOptions"
                      :fieldNames="optionsObj.tableFieldf4dfaf_F_InstallOrBeamProps"
                      allowClear
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_LockSet'">
                    <JnpfInput
                      v-model:value="record.F_LockSet"
                      placeholder="请输入"
                      allowClear
                      :style="{
                        width: '100%',
                      }"
                      :showCount="false" />
                  </template>
                  <template v-if="column.key === 'F_Hinge'">
                    <JnpfInput
                      v-model:value="record.F_Hinge"
                      placeholder="请输入"
                      allowClear
                      :style="{
                        width: '100%',
                      }"
                      :showCount="false" />
                  </template>
                  <template v-if="column.key === 'F_Color'">
                    <JnpfInput
                      v-model:value="record.F_Color"
                      placeholder="请输入"
                      allowClear
                      :style="{
                        width: '100%',
                      }"
                      :showCount="false" />
                  </template>
                  <template v-if="column.key === 'F_Material'">
                    <JnpfSelect
                      v-model:value="record.F_Material"
                      placeholder="请选择"
                      :options="optionsObj.tableFieldf4dfaf_F_MaterialOptions"
                      :fieldNames="optionsObj.tableFieldf4dfaf_F_MaterialProps"
                      allowClear
                      :style="{
                        width: '150px',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_F_AuditStatus'">
                    <JnpfInput
                      v-model:value="record.F_F_AuditStatus"
                      placeholder="请输入"
                      allowClear
                      :style="{
                        width: '100%',
                      }"
                      :showCount="false" />
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
                      <!-- <a-button class="action-btn" type="link" @click="copytableFieldf4dfafRow(index)" size="small">{{
                        t('common.copyText', '复制')
                      }}</a-button> -->
                      <!-- <a-button class="action-btn" type="link" @click="goodsHandle(record)" size="small">{{ t('用料清单') }}</a-button> -->
                      <a-button class="action-btn" type="link" color="error" @click="removetableFieldf4dfafRow(index, true)" size="small">{{
                        t('common.delText', '删除')
                      }}</a-button>
                    </a-space>
                  </template>
                </template>
              </a-table>
              <!-- <a-space class="input-table-footer-btn">
                <a-button type="primary" preIcon="icon-ym icon-ym-btn-add" @click="addtableFieldf4dfafRow">{{ t('common.add1Text', '添加') }}</a-button>
                <a-button type="danger" preIcon="icon-ym icon-ym-btn-clearn" @click="batchRemovetableFieldf4dfafRow(true)">{{
                  t('common.batchDelText', '批量删除')
                }}</a-button>
              </a-space> -->
            </a-form-item>
          </a-col>
        </a-row>
      </a-form>
    </a-row>
  </BasicModal>
  <SelectModal :config="state.currTableConf" :formData="state.dataForm" ref="selectModal" @select="addForSelect" />
  <GoodsForm ref="goodsFormRef" @goodsSubmit="onGoodsSubmit" />
</template>
<script lang="ts" setup>
  import { create, update, getInfo, getDefaultsByCustomer } from './helper/api';
  import { getGoodsUnit } from '@/views/Subcode/aGoods/helper/api';
  import { reactive, toRefs, nextTick, ref, unref, computed, toRaw, inject, watch } from 'vue';
  import { BasicModal, useModal } from '@/components/Modal';
  import SelectModal from '@/components/CommonModal/src/SelectModal.vue';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useI18n } from '@/hooks/web/useI18n';
  import { useUserStore } from '@/store/modules/user';
  import { getDictionaryDataSelector } from '@/api/systemData/dictionary';
  import type { FormInstance } from 'ant-design-vue';
  import { thousandsFormat } from '@/utils/jnpf';
  import { getTimeUnit, getDateTimeUnit } from '@/utils/jnpf';
  import { getDataInterfaceRes } from '@/api/systemData/dataInterface';
  import dayjs from 'dayjs';
  import { cloneDeep } from 'lodash-es';
  import { buildUUID } from '@/utils/uuid';
  import { CaretRightOutlined } from '@ant-design/icons-vue';
  import GoodsForm from '@/views/Subcode/aProdProject/GoodsForm.vue';
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
    tableFieldf4dfafouterActiveKey: number[];
    tableFieldf4dfafinnerActiveKey: string[];
    tableFieldf4dfafDefaultExpandAll: boolean;
    selectedtableFieldf4dfafRowKeys: any[];
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
  const goodsFormRef = ref<any>(null);
  const selectModal = ref(null);
  const tableFieldf4dfafColumns: any[] = computed(() => {
    let list = [
      // {
      //   title: '工单编号',
      //   dataIndex: 'F_WorkOrderNo',
      //   key: 'F_WorkOrderNo',
      //   tipLabel: '',
      //   required: false,
      //   align: 'left',
      //   span: '24',
      //   labelWidth: '',
      //   width: '',
      //   fixed: false,
      //   formP: 'F_WorkOrderNo',
      //   isSystemControl: false,
      // },
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
      // {
      //   title: '计划数',
      //   dataIndex: 'F_PlanNum',
      //   key: 'F_PlanNum',
      //   tipLabel: '',
      //   required: true,
      //   align: 'left',
      //   span: '24',
      //   labelWidth: '',
      //   width: '80px',
      //   fixed: false,
      //   formP: 'F_PlanNum',
      //   isSystemControl: false,
      // },
      // {
      //   title: '单位',
      //   dataIndex: 'F_UnitTwo',
      //   key: 'F_UnitTwo',
      //   tipLabel: '',
      //   required: false,
      //   align: 'left',
      //   span: '24',
      //   labelWidth: '',
      //   width: '',
      //   fixed: false,
      //   formP: 'F_UnitTwo',
      //   isSystemControl: false,
      // },
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
        title: '工艺路线',
        dataIndex: 'F_RoutingId',
        key: 'F_RoutingId',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '180px',
        fixed: false,
        formP: 'F_RoutingId',
        isSystemControl: false,
      },
      {
        title: '交货日期',
        dataIndex: 'F_DeliveryDate',
        key: 'F_DeliveryDate',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_DeliveryDate',
        isSystemControl: false,
      },
      {
        title: '优先级',
        dataIndex: 'F_Priority',
        key: 'F_Priority',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Priority',
        isSystemControl: false,
      },
      {
        title: 'BOM',
        dataIndex: 'F_BomId',
        key: 'F_BomId',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_BomId',
        isSystemControl: false,
      },
      {
        title: '图纸',
        dataIndex: 'F_DrawingFiles',
        key: 'F_DrawingFiles',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_DrawingFiles',
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
        title: '门板厚度',
        dataIndex: 'F_DoorPlateThickness',
        key: 'F_DoorPlateThickness',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_DoorPlateThickness',
        isSystemControl: false,
      },
      {
        title: '箱体板厚',
        dataIndex: 'F_BoxPlateThickness',
        key: 'F_BoxPlateThickness',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_BoxPlateThickness',
        isSystemControl: false,
      },
      {
        title: '安装或安装梁',
        dataIndex: 'F_InstallOrBeam',
        key: 'F_InstallOrBeam',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_InstallOrBeam',
        isSystemControl: false,
      },
      {
        title: '锁具',
        dataIndex: 'F_LockSet',
        key: 'F_LockSet',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_LockSet',
        isSystemControl: false,
      },
      {
        title: '铰链',
        dataIndex: 'F_Hinge',
        key: 'F_Hinge',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Hinge',
        isSystemControl: false,
      },
      {
        title: '颜色',
        dataIndex: 'F_Color',
        key: 'F_Color',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Color',
        isSystemControl: false,
      },
      {
        title: '材质',
        dataIndex: 'F_Material',
        key: 'F_Material',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Material',
        isSystemControl: false,
      },
      // {
      //   title: '审核状态',
      //   dataIndex: 'F_F_AuditStatus',
      //   key: 'F_F_AuditStatus',
      //   tipLabel: '',
      //   required: false,
      //   align: 'left',
      //   span: '24',
      //   labelWidth: '',
      //   width: '',
      //   fixed: false,
      //   formP: 'F_F_AuditStatus',
      //   isSystemControl: false,
      // },
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
    list = list.filter(o => hasFormP('tableFieldf4dfaf-' + o.formP));
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
  const gettableFieldf4dfafRowSelection = computed(() => {
    const rowSelection = {
      selectedRowKeys: state.selectedtableFieldf4dfafRowKeys,
      onChange: (selectedRowKeys: string[]) => {
        state.selectedtableFieldf4dfafRowKeys = (selectedRowKeys || []).sort().reverse();
      },
    };
    return rowSelection;
  });
  const state = reactive<State>({
    dataForm: {
      id: '',
      GoodsList: [],
      F_ContractCode: undefined,
      F_CustomerCode: undefined,
      F_CustomerId: undefined,
      F_ContactId: undefined,
      F_AddressId: undefined,
      F_PrepayAmount: undefined,
      F_PaymentCycle: undefined,
      F_SaleTotal: undefined,
      F_ContractAmount: undefined,
      F_DeliveryDate: undefined,
      F_SalesmanId: undefined,
      F_IsTaxable: 0,
      F_AuditStatus: undefined,
      F_Description: undefined,
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
      tableFieldf4dfaf: [],
    },
    tableFieldf4dfafouterActiveKey: [0],
    tableFieldf4dfafinnerActiveKey: [],
    tableFieldf4dfafDefaultExpandAll: true,
    selectedtableFieldf4dfafRowKeys: [],
    dataRule: {
      F_CustomerId: [
        {
          required: true,
          message: '请输入客户',
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
          relationField: '',
          jnpfKey: null,
          complexHeaderList: null,
          sourceType: 2,
          isChildren: false,
          IsMultiple: false,
        },
      ],
      F_AddressIdProps: { label: 'F_Address', value: 'F_Id' },
      tableFieldf4dfaf_F_CustomerIdOptions: [
        {
          value: 'F_GoodsNo',
          label: '编码',
        },
        {
          value: 'F_GoodsName',
          label: '名称',
        },
        // {
        //   value: 'F_Unit',
        //   label: '单位',
        // },
        // 成本单价与销售单价已移除，不在下拉映射中
        {
          value: 'F_Specification',
          label: '规格',
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
      tableFieldf4dfaf_F_CustomerIdTemplateJson: [],
      GoodsListOptions: [
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
        {
          value: 'F_Type',
          label: '商品类型',
        },
        {
          value: 'F_Remark',
          label: '备注',
        },
      ],
      GoodsListTemplateJson: [],
      GoodsList: [],
      tableFieldf4dfaf_F_PriorityOptions: [],
      tableFieldf4dfaf_F_PriorityProps: { label: 'fullName', value: 'enCode' },
      tableFieldf4dfaf_F_BomIdProps: { label: 'F_BomName', value: 'id' },
      tableFieldf4dfaf_F_CategoryOptions: [],
      tableFieldf4dfaf_F_CategoryProps: {
        label: 'F_Name',
        value: 'F_Id',
        children: 'children',
      },
      tableFieldf4dfaf_F_RoutingIdOptions: [],
      tableFieldf4dfaf_F_RoutingIdProps: { label: 'F_RoutingName', value: 'id' },
      tableFieldf4dfaf_F_DoorPlateThicknessOptions: [],
      tableFieldf4dfaf_F_DoorPlateThicknessProps: { label: 'fullName', value: 'enCode' },
      tableFieldf4dfaf_F_BoxPlateThicknessOptions: [],
      tableFieldf4dfaf_F_BoxPlateThicknessProps: { label: 'fullName', value: 'enCode' },
      tableFieldf4dfaf_F_InstallOrBeamOptions: [],
      tableFieldf4dfaf_F_InstallOrBeamProps: { label: 'fullName', value: 'enCode' },
      tableFieldf4dfaf_F_MaterialOptions: [],
      tableFieldf4dfaf_F_MaterialProps: { label: 'fullName', value: 'enCode' },
      F_ContactId: [],
    },
    currVmodel: '',
    currTableConf: {},
    tableRows: {
      tableFieldf4dfaf: {
        enabledmark: undefined,
        F_WorkOrderNo: undefined,
        F_GoodsNo: undefined,
        // F_CostPrice and F_SalePrice removed from row defaults
        F_AmountAfterDiscount: undefined,
        F_DiscountAmount: undefined,
        F_Unit: undefined,
        F_Specification: undefined,
        F_CustomerId: undefined,
        F_Price: undefined,
        F_SaleQty: undefined,
        F_PlanNum: 1,
        F_Discount: undefined,
        F_Description: undefined,
        F_DoorPlateThickness: undefined,
        F_BoxPlateThickness: undefined,
        F_InstallOrBeam: undefined,
        F_LockSet: undefined,
        F_Hinge: undefined,
        F_Color: undefined,
        F_Material: undefined,
        F_F_AuditStatus: undefined,
        F_Files: [],
        F_CreatorUserId: undefined,
        F_CreatorTime: undefined,
        F_Category: [],
        F_RoutingId: undefined,
        F_BomId: undefined,
        tableFieldf4dfaf_F_BomIdOptions: [],
        F_DrawingFiles: [],
        F_DeliveryDate: undefined,
        F_Priority: undefined,
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

  // 监听客户选择变化，更新联系人和地址的关联字段
  watch(
    () => state.dataForm.F_CustomerId,
    newCustomerId => {
      if (newCustomerId) {
        // 更新联系人模板的关联字段
        state.optionsObj.F_ContactIdTemplateJson[0].relationField = newCustomerId;
        // 清除当前选择的联系人和地址
        state.dataForm.F_ContactId = undefined;
        state.dataForm.F_AddressId = undefined;
        // 重新获取地址选项
        getF_AddressIdOptions();
      }
    },
  );

  // 当子表选择合同商品发生变化时，实时设置销售总数（禁用显示）
  watch(
    () => state.dataForm.tableFieldf4dfaf,
    list => {
      try {
        const len = Array.isArray(list) ? list.length : 0;
        state.dataForm.F_SaleTotal = len;
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

  const { title, continueText, showContinueBtn, dataForm, submitType, dataRule, optionsObj } = toRefs(state);
  const computedSaleTotal = computed(() => {
    try {
      const rows = Array.isArray(state.dataForm.tableFieldf4dfaf) ? state.dataForm.tableFieldf4dfaf : [];
      const sum = rows.reduce((acc: number, row: any) => {
        const v = typeof row.F_SaleQty !== 'undefined' && row.F_SaleQty !== null ? Number(row.F_SaleQty) || 0 : 0;
        return acc + (isNaN(v) ? 0 : v);
      }, 0);
      return sum;
    } catch {
      return 0;
    }
  });
  const computedContractAmount = computed(() => {
    try {
      const rows = Array.isArray(state.dataForm.tableFieldf4dfaf) ? state.dataForm.tableFieldf4dfaf : [];
      const sum = rows.reduce((acc: number, row: any) => {
        const v =
          typeof row.F_AmountAfterDiscount !== 'undefined' && row.F_AmountAfterDiscount !== null
            ? Number(row.F_AmountAfterDiscount)
            : (Number(row.F_Price) || 0) * (Number(row.F_SaleQty) || 0);
        return acc + (isNaN(v) ? 0 : v);
      }, 0);
      return Math.round(sum * 100) / 100;
    } catch {
      return 0;
    }
  });

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
      state.dataForm.tableFieldf4dfaf = [];
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
        F_ContractCode: undefined,
        F_CustomerCode: undefined,
        F_CustomerId: undefined,
        F_ContactId: undefined,
        F_AddressId: undefined,
        F_PrepayAmount: undefined,
        F_PaymentCycle: undefined,
        F_SaleTotal: undefined,
        F_ContractAmount: undefined,
        F_DeliveryDate: undefined,
        F_SalesmanId: undefined,
        F_IsTaxable: 0,
        F_AuditStatus: undefined,
        F_Description: undefined,
        F_CreatorUserId: undefined,
        F_CreatorTime: undefined,
        tableFieldf4dfaf: [],
      };
      state.dataForm.GoodsList = [];
      getF_CustomerIdOptions();
      if (getLeftTreeActiveInfo) state.dataForm = { ...state.dataForm, ...(getLeftTreeActiveInfo() || {}) };
      // 在设置完客户ID后获取地址选项
      getF_AddressIdOptions();
      gettableFieldf4dfaf_F_DoorPlateThicknessOptions();
      gettableFieldf4dfaf_F_BoxPlateThicknessOptions();
      gettableFieldf4dfaf_F_InstallOrBeamOptions();
      gettableFieldf4dfaf_F_MaterialOptions();
      gettableFieldf4dfaf_F_PriorityOptions();
      gettableFieldf4dfaf_F_CategoryOptions();
      gettableFieldf4dfaf_F_RoutingIdOptions();
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
      state.dataForm.GoodsList = [];
      // 给每行补 jnpfId，并尝试为只读展示字段填充数据（后端有时只返回关联 id）
      const rows = state.dataForm.tableFieldf4dfaf || [];
      for (let i = 0; i < rows.length; i++) {
        const element = rows[i];
        element.jnpfId = buildUUID();
        state.dataForm.GoodsList.push(element.F_CustomerId);
        gettableFieldf4dfaf_F_BomIdOptions(i);
      }
      // 如果存在客户ID，尝试从后端读取默认联系人与默认地址以填充选项，保证 F_ContactId / F_AddressId 能显示文本
      if (state.dataForm.F_CustomerId) {
        try {
          const defaultsRes = await getDefaultsByCustomer(state.dataForm.F_CustomerId);
          const payload = defaultsRes && defaultsRes.data ? defaultsRes.data : defaultsRes;
          const data = payload || {};
          // 联系人（用于 popupSelect 的 extraOptions）
          if (Array.isArray(data.contacts) && data.contacts.length) {
            const mappedContacts = data.contacts.map((c: any) => ({
              ...c,
              F_Id: c.F_Id || c.id,
              F_ContactName: c.F_ContactName || c.fullName || c.F_ContactName,
              id: c.F_Id || c.id,
              fullName: c.F_ContactName || c.fullName,
            }));
            state.optionsObj.F_ContactId = mappedContacts;
            // 保留后端已返回的 F_ContactId，如不存在则使用第一个默认联系人
            state.dataForm.F_ContactId = state.dataForm.F_ContactId || mappedContacts[0]?.F_Id || mappedContacts[0]?.id;
          } else {
            state.optionsObj.F_ContactId = [];
          }
          // 地址（用于普通 select 的 options）
          if (Array.isArray(data.addresses) && data.addresses.length) {
            const mappedAddrs = data.addresses.map((a: any) => ({
              ...a,
              F_Id: a.F_Id || a.id,
              F_Address: a.F_Address || a.address || a.F_Address,
              F_Region: a.F_Region || a.region || a.F_Region,
              id: a.F_Id || a.id,
            }));
            state.optionsObj.F_AddressIdOptions = mappedAddrs;
            state.dataForm.F_AddressId = state.dataForm.F_AddressId || mappedAddrs[0]?.F_Id || mappedAddrs[0]?.id;
          } else {
            state.optionsObj.F_AddressIdOptions = [];
          }
        } catch (e) {
          // ignore
        }
      }
      // 如果后端返回的子表只有引用 id，需要再请求接口获取商品详情以填充只读字段
      // 使用与 JnpfPopupSelect 相同的数据接口 id（和 popup 的接口保持一致）
      const productInterfaceId = '2008721559174385664';
      try {
        await Promise.all(
          rows.map(async (row: any) => {
            if (!row) return;
            // 如果已经有展示字段就跳过查询
            const needFetch = !row.F_GoodsNo && !row.F_Specification && !row.F_Unit && row.F_CustomerId;
            if (!needFetch) {
              // 还是触发一次金额重算以确保显示字段与金额一致
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
                // 如果需要，可同时填充价格类字段（但之前已移除成本/销售列）
                // row.F_SalePrice = info.F_SalePrice ?? row.F_SalePrice;
                // row.F_CostPrice = info.F_CostPrice ?? row.F_CostPrice;
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
      gettableFieldf4dfaf_F_DoorPlateThicknessOptions();
      gettableFieldf4dfaf_F_BoxPlateThicknessOptions();
      gettableFieldf4dfaf_F_InstallOrBeamOptions();
      gettableFieldf4dfaf_F_MaterialOptions();
      gettableFieldf4dfaf_F_PriorityOptions();
      gettableFieldf4dfaf_F_CategoryOptions();
      gettableFieldf4dfaf_F_RoutingIdOptions();
      // 如果有客户ID，更新关联字段
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
      // clone form data and strip display-only fields from table rows before submitting
      const payload: any = cloneDeep(state.dataForm);
      if (Array.isArray(payload.tableFieldf4dfaf)) {
        payload.tableFieldf4dfaf = payload.tableFieldf4dfaf.map((row: any) => {
          const { F_GoodsNo, F_Specification, F_AmountAfterDiscount, F_DiscountAmount, F_Unit, ...rest } = row || {};
          return rest;
        });
      }
      payload.F_ContractAmount = computedContractAmount.value;
      payload.F_SaleTotal = computedSaleTotal.value;
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
  function gettableFieldf4dfaf_F_DoorPlateThicknessOptions() {
    getDictionaryDataSelector('2013183327061807104').then(res => {
      state.optionsObj.tableFieldf4dfaf_F_DoorPlateThicknessOptions = res.data.list;
    });
  }
  function gettableFieldf4dfaf_F_BoxPlateThicknessOptions() {
    getDictionaryDataSelector('2013183327061807104').then(res => {
      state.optionsObj.tableFieldf4dfaf_F_BoxPlateThicknessOptions = res.data.list;
    });
  }
  function gettableFieldf4dfaf_F_InstallOrBeamOptions() {
    getDictionaryDataSelector('2013183327061807104').then(res => {
      state.optionsObj.tableFieldf4dfaf_F_InstallOrBeamOptions = res.data.list;
    });
  }
  function gettableFieldf4dfaf_F_MaterialOptions() {
    getDictionaryDataSelector('2016346179754921984').then(res => {
      state.optionsObj.tableFieldf4dfaf_F_MaterialOptions = res.data.list;
    });
  }
  function gettableFieldf4dfaf_F_PriorityOptions() {
    getDictionaryDataSelector('2013182853290004480').then(res => {
      state.optionsObj.tableFieldf4dfaf_F_PriorityOptions = res.data.list;
    });
  }
  function gettableFieldf4dfaf_F_CategoryOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2008414575283802112', query).then(res => {
      let data = res.data;
      state.optionsObj.tableFieldf4dfaf_F_CategoryOptions = Array.isArray(data) ? data : [];
    });
  }
  function gettableFieldf4dfaf_F_RoutingIdOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2013852230570086400', query).then(res => {
      let data = res.data;
      state.optionsObj.tableFieldf4dfaf_F_RoutingIdOptions = Array.isArray(data) ? data : [];
    });
  }
  function gettableFieldf4dfaf_F_BomIdOptions(i?) {
    let templateJson = [
      {
        defaultValue: '',
        field: 'goodsId',
        dataType: 'varchar',
        required: 0,
        fieldName: '商品ID',
        relationField: 'tableFieldf4dfaf-F_CustomerId',
        jnpfKey: 'select',
        complexHeaderList: null,
        sourceType: 1,
        isChildren: false,
        IsMultiple: false,
      },
    ];
    let query = {
      paramList: getParamList(templateJson, state.dataForm, i),
    };
    getDataInterfaceRes('2013188646957617152', query).then(res => {
      let data = res.data;
      state.dataForm.tableFieldf4dfaf[i].tableFieldf4dfaf_F_BomIdOptions = Array.isArray(data) ? data : [];
    });
  }
  function F_GoodsIdTableChange(i) {
    state.dataForm.tableFieldf4dfaf[i].F_BomId = undefined;
    gettableFieldf4dfaf_F_BomIdOptions(i);
  }
  function GoodsListBtn(val, option) {
    /* 兜底保证是数组 */
    if (!Array.isArray(state.dataForm.tableFieldf4dfaf)) {
      state.dataForm.tableFieldf4dfaf = [];
    }

    /* 1. 把 option 里的 id 做成 Set，方便比对 */
    const optionIdSet = new Set(option.map(o => o.id));

    /* 2. 删除本地已不在 option 里的行 */
    state.dataForm.tableFieldf4dfaf = state.dataForm.tableFieldf4dfaf.filter(item => optionIdSet.has(item.F_CustomerId));

    /* 3. 把 option 里本地还没有的行加进来 */
    option.forEach((o, i) => {
      const exist = state.dataForm.tableFieldf4dfaf.findIndex(item => item.F_CustomerId === o.id) !== -1;
      if (!exist) {
        const newRow = {
          jnpfId: buildUUID(),
          F_CustomerId: o.id,
          F_WorkOrderNo: undefined,
          F_Specification: o.F_Specification,
          F_GoodsNo: o.F_GoodsNo,
          F_DeliveryDate: state.dataForm.F_DeliveryDate || undefined,
          F_Priority: '1',
          F_BomId: undefined,
          tableFieldf4dfaf_F_BomIdOptions: [],
          F_DrawingFiles: [],
          F_DoorPlateThickness: undefined,
          F_BoxPlateThickness: undefined,
          F_InstallOrBeam: undefined,
          F_LockSet: undefined,
          F_Hinge: undefined,
          F_Color: undefined,
          F_Material: undefined,
          F_Category: JSON.parse(o.F_CategoryId) || [],
          F_RoutingId: undefined,
          F_Price: undefined,
          F_SaleQty: 1,
          F_PlanNum: undefined,
          F_Discount: undefined,
          F_Description: undefined,
          F_Files: [],
          F_AmountAfterDiscount: undefined,
          F_DiscountAmount: undefined,
        };
        state.dataForm.tableFieldf4dfaf.push(newRow);
        state.tableFieldf4dfafinnerActiveKey.push(newRow.jnpfId);
      }
      gettableFieldf4dfaf_F_BomIdOptions(i);
    });
  }

  // 用料清单
  function goodsHandle(record) {
    const data = {
      ...record,
      jnpfId: record.jnpfId,
    };
    goodsFormRef.value?.init(data);
  }

  // 监听用料清单 submit 事件
  function onGoodsSubmit(returnRow) {
    const idx = state.dataForm.tableFieldf4dfaf.findIndex(item => item.jnpfId === returnRow.jnpfId);
    if (idx > -1) {
      state.dataForm.tableFieldf4dfaf[idx].ProjectGoods = returnRow.ProjectGoods;
    }
  }

  function addtableFieldf4dfafRow() {
    let item = {
      jnpfId: buildUUID(),
      F_WorkOrderNo: undefined,
      F_GoodsNo: undefined,
      F_AmountAfterDiscount: undefined,
      F_DiscountAmount: undefined,
      F_Unit: undefined,
      F_Specification: undefined,
      F_CustomerId: undefined,
      F_Price: undefined,
      F_SaleQty: 1,
      F_PlanNum: undefined,
      F_Discount: undefined,
      F_Description: undefined,
      F_DoorPlateThickness: undefined,
      F_BoxPlateThickness: undefined,
      F_InstallOrBeam: undefined,
      F_LockSet: undefined,
      F_Hinge: undefined,
      F_Color: undefined,
      F_Material: undefined,
      F_F_AuditStatus: undefined,
      F_Files: [],
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
      F_Category: [],
      F_RoutingId: undefined,
      F_BomId: undefined,
      tableFieldf4dfaf_F_BomIdOptions: [],
      F_DrawingFiles: [],
      F_DeliveryDate: state.dataForm.F_DeliveryDate || undefined,
      F_Priority: undefined,
    };
    state.dataForm.tableFieldf4dfaf.push(item);
    state.tableFieldf4dfafinnerActiveKey.push(item.jnpfId);
  }
  function removetableFieldf4dfafRow(index, showConfirm = false) {
    if (showConfirm) {
      createConfirm({
        iconType: 'warning',
        title: '提示',
        content: '此操作将永久删除该数据, 是否继续?',
        onOk: () => {
          state.dataForm.tableFieldf4dfaf.splice(index, 1);
          state.dataForm.GoodsList.splice(index, 1);
        },
      });
    } else {
      state.dataForm.tableFieldf4dfaf.splice(index, 1);
      state.dataForm.GoodsList.splice(index, 1);
    }
  }
  function copytableFieldf4dfafRow(index) {
    let item = cloneDeep(state.dataForm.tableFieldf4dfaf[index]);
    let copyData = {};
    for (let i = 0; i < unref(tableFieldf4dfafColumns).length; i++) {
      const cur = unref(tableFieldf4dfafColumns)[i];
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
    state.dataForm.tableFieldf4dfaf.push(copyItem);
    state.tableFieldf4dfafinnerActiveKey.push(copyItem.jnpfId);
  }
  function batchRemovetableFieldf4dfafRow(showConfirm = false) {
    if (!state.selectedtableFieldf4dfafRowKeys.length) return createMessage.error('请选择一条数据');
    const handleBatchRemove = () => {
      state.dataForm.tableFieldf4dfaf = state.dataForm.tableFieldf4dfaf.filter(o => !state.selectedtableFieldf4dfafRowKeys.includes(o.jnpfId));
      nextTick(() => {
        state.selectedtableFieldf4dfafRowKeys = [];
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
  // 处理子表中商品选择：把选择数据的编号/规则/来源映射到只读展示字段
  function handleTableCustomerSelectChange(val: any, option: any, record: any) {
    try {
      if (!val) {
        record.F_GoodsNo = undefined;
        record.F_Specification = undefined;
        record.F_AmountAfterDiscount = undefined;
        record.F_Unit = undefined;
      } else {
        // option 可能为数组或单对象，取第一个对象字段
        const info = Array.isArray(option) ? option[0] || {} : option || {};
        record.F_GoodsNo = info.F_GoodsNo;
        record.F_Specification = info.F_Specification;

        getGoodsUnit(val).then(res => {
          record.F_UnitTwo = res.data;
        });
        // 计算折后金额
        recalcRowAmount(record);
      }
    } catch (e) {
      // ignore
    }
  }
  function recalcRowAmount(record: any) {
    try {
      const qty = Number(record.F_SaleQty) || 0;
      const discountInput = Number(record.F_Discount) || 100;

      const discountMultiplier = Number.isFinite(discountInput) && !Number.isNaN(discountInput) ? discountInput / 100 : 1;
      const unitPrice = Number(record.F_Price) || 0;
      const originalAmount = unitPrice * qty;
      const amountAfter = originalAmount * discountMultiplier;
      record.F_AmountAfterDiscount = Number.isFinite(amountAfter) ? parseFloat(amountAfter.toFixed(2)) : 0;
      // 优惠金额 = 原价 - 折后金额
      const discountAmount = originalAmount - record.F_AmountAfterDiscount;
      record.F_DiscountAmount = Number.isFinite(discountAmount) ? parseFloat(discountAmount.toFixed(2)) : 0;
    } catch (e) {
      // ignore
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
