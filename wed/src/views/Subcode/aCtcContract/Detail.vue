<template>
  <BasicModal v-bind="$attrs" @register="registerModal" :title="title" width="80%" :minHeight="100" :showOkBtn="false" :canFullscreen="true">
    <template #insertFooter> </template>
    <a-row class="dynamic-form">
      <a-form :colon="false" layout="horizontal" labelAlign="right" :labelCol="{ style: { width: '100px' } }" :model="dataForm" ref="formRef">
        <a-row :gutter="15">
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_ContractCode')">
            <a-form-item name="F_ContractCode" :labelCol="{ style: { width: '100px' } }">
              <template #label>合同编号</template>
              <JnpfInput
                v-model:value="dataForm.F_ContractCode"
                :maskConfig="{
                  filler: '*',
                  maskType: 1,
                  prefixType: 1,
                  prefixLimit: 0,
                  prefixSpecifyChar: '',
                  suffixType: 1,
                  suffixLimit: 0,
                  suffixSpecifyChar: '',
                  ignoreChar: '',
                  useUnrealMask: false,
                  unrealMaskLength: 1,
                }"
                disabled
                detailed />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_CustomerId')">
            <a-form-item name="F_CustomerId" :labelCol="{ style: { width: '100px' } }">
              <template #label>客户</template>
              <p>{{ dataForm.F_CustomerId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_ContactId')">
            <a-form-item name="F_ContactId" :labelCol="{ style: { width: '100px' } }">
              <template #label>联系人</template>
              <p class="leading-32px">{{ dataForm.F_ContactId }}</p>
              <ExtraRelationInfo
                :extraOptions="state.extraOptions.F_ContactId"
                :data="state.extraData.F_ContactId"
                v-if="state.extraOptions.F_ContactId?.length && state.extraData.F_ContactId && JSON.stringify(state.extraData.F_ContactId) !== '{}'" />
            </a-form-item>
          </a-col>
          <!-- <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_AddressId')">
            <a-form-item name="F_AddressId" :labelCol="{ style: { width: '100px' } }">
              <template #label>客户地址ID</template>
              <p>{{ dataForm.F_AddressId }}</p>
            </a-form-item>
          </a-col> -->
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_CustomerCode')">
            <a-form-item name="F_CustomerCode" :labelCol="{ style: { width: '100px' } }">
              <template #label>客户合同号</template>
              <JnpfInput
                v-model:value="dataForm.F_CustomerCode"
                :maskConfig="{
                  filler: '*',
                  maskType: 1,
                  prefixType: 1,
                  prefixLimit: 0,
                  prefixSpecifyChar: '',
                  suffixType: 1,
                  suffixLimit: 0,
                  suffixSpecifyChar: '',
                  ignoreChar: '',
                  useUnrealMask: false,
                  unrealMaskLength: 1,
                }"
                disabled
                detailed />
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
                }"
                disabled
                detailed />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_PaymentCycle')">
            <a-form-item name="F_PaymentCycle" :labelCol="{ style: { width: '100px' } }">
              <template #label>付款周期（天）</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_PaymentCycle"
                placeholder="0"
                :controls="false"
                :style="{
                  width: '100%',
                }"
                disabled
                detailed />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_SaleTotal')">
            <a-form-item name="F_SaleTotal" :labelCol="{ style: { width: '100px' } }">
              <template #label>总数</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_SaleTotal"
                placeholder="请输入"
                :controls="false"
                :style="{
                  width: '100%',
                }"
                disabled
                detailed />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_ContractAmount')">
            <a-form-item name="F_ContractAmount" :labelCol="{ style: { width: '100px' } }">
              <template #label>合同金额</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_ContractAmount"
                placeholder="请输入"
                :controls="false"
                :style="{
                  width: '100%',
                }"
                disabled
                detailed />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_DeliveryDate')">
            <a-form-item name="F_DeliveryDate" :labelCol="{ style: { width: '100px' } }">
              <template #label>交货日期</template>
              <p>{{ dataForm.F_DeliveryDate }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_SalesmanId')">
            <a-form-item name="F_SalesmanId" :labelCol="{ style: { width: '100px' } }">
              <template #label>业务员</template>
              <p>{{ dataForm.F_SalesmanId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_IsTaxable')">
            <a-form-item name="F_IsTaxable" :labelCol="{ style: { width: '100px' } }">
              <template #label>是否含税</template>
              <p>{{ dataForm.F_IsTaxable }}</p>
            </a-form-item>
          </a-col>

          <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_Description')">
            <a-form-item name="F_Description" :labelCol="{ style: { width: '100px' } }">
              <template #label>备注</template>
              <p>{{ dataForm.F_Description }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="选择合同商品" :bordered="false" />
              <a-table
                :data-source="dataForm.tableFieldf4dfaf"
                :columns="tableFieldf4dfafColumns"
                size="small"
                :pagination="false"
                :scroll="{ x: 'max-content' }">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                  <template v-if="column.key === 'F_CustomerId'">
                    <p class="leading-32px">{{ record.F_CustomerId }}</p>
                    <ExtraRelationInfo
                      :extraOptions="state.extraOptions.F_CustomerId"
                      :data="state.extraData.F_CustomerId"
                      v-if="state.extraOptions.F_CustomerId?.length && state.extraData.F_CustomerId && JSON.stringify(state.extraData.F_CustomerId) !== '{}'" />
                  </template>
                  <template v-if="column.key === 'F_Price'">
                    <JnpfInputNumber
                      v-model:value="record.F_Price"
                      placeholder="请输入"
                      :controls="false"
                      :style="{
                        width: '100%',
                      }"
                      disabled
                      detailed />
                  </template>
                  <template v-if="column.key === 'F_SaleQty'">
                    <JnpfInputNumber
                      v-model:value="record.F_SaleQty"
                      placeholder="请输入"
                      :controls="true"
                      :style="{
                        width: '100%',
                      }"
                      disabled
                      detailed />
                  </template>
                  <template v-if="column.key === 'F_Discount'">
                    <JnpfInputNumber
                      v-model:value="record.F_Discount"
                      placeholder="请输入"
                      :min="0"
                      :max="100"
                      :controls="false"
                      :style="{
                        width: '100%',
                      }"
                      disabled
                      detailed />
                  </template>
                  <template v-if="column.key === 'F_DiscountAmount'">
                    <JnpfInputNumber
                      v-model:value="record.F_DiscountAmount"
                      placeholder="0"
                      :controls="false"
                      :style="{
                        width: '100%',
                      }"
                      disabled
                      detailed />
                  </template>
                  <template v-if="column.key === 'F_DiscountedAmount'">
                    <JnpfInputNumber
                      v-model:value="record.F_DiscountedAmount"
                      placeholder="0"
                      :controls="false"
                      :style="{
                        width: '100%',
                      }"
                      disabled
                      detailed />
                  </template>
                  <template v-if="column.key === 'F_Description'">
                    <JnpfInput
                      v-model:value="record.F_Description"
                      :maskConfig="{
                        filler: '*',
                        maskType: 1,
                        prefixType: 1,
                        prefixLimit: 0,
                        prefixSpecifyChar: '',
                        suffixType: 1,
                        suffixLimit: 0,
                        suffixSpecifyChar: '',
                        ignoreChar: '',
                        useUnrealMask: false,
                        unrealMaskLength: 1,
                      }"
                      disabled
                      detailed />
                  </template>
                  <template v-if="column.key === 'F_DoorPlateThickness'">
                    <JnpfInputNumber
                      v-model:value="record.F_DoorPlateThickness"
                      placeholder="请输入"
                      :controls="false"
                      :style="{
                        width: '100%',
                      }"
                      disabled
                      detailed />
                  </template>
                  <template v-if="column.key === 'F_BoxPlateThickness'">
                    <JnpfInputNumber
                      v-model:value="record.F_BoxPlateThickness"
                      placeholder="请输入"
                      :controls="false"
                      :style="{
                        width: '100%',
                      }"
                      disabled
                      detailed />
                  </template>
                  <template v-if="column.key === 'F_InstallOrBeam'">
                    <JnpfInput
                      v-model:value="record.F_InstallOrBeam"
                      :maskConfig="{
                        filler: '*',
                        maskType: 1,
                        prefixType: 1,
                        prefixLimit: 0,
                        prefixSpecifyChar: '',
                        suffixType: 1,
                        suffixLimit: 0,
                        suffixSpecifyChar: '',
                        ignoreChar: '',
                        useUnrealMask: false,
                        unrealMaskLength: 1,
                      }"
                      disabled
                      detailed />
                  </template>
                  <template v-if="column.key === 'F_LockSet'">
                    <JnpfInput
                      v-model:value="record.F_LockSet"
                      :maskConfig="{
                        filler: '*',
                        maskType: 1,
                        prefixType: 1,
                        prefixLimit: 0,
                        prefixSpecifyChar: '',
                        suffixType: 1,
                        suffixLimit: 0,
                        suffixSpecifyChar: '',
                        ignoreChar: '',
                        useUnrealMask: false,
                        unrealMaskLength: 1,
                      }"
                      disabled
                      detailed />
                  </template>
                  <template v-if="column.key === 'F_Hinge'">
                    <JnpfInput
                      v-model:value="record.F_Hinge"
                      :maskConfig="{
                        filler: '*',
                        maskType: 1,
                        prefixType: 1,
                        prefixLimit: 0,
                        prefixSpecifyChar: '',
                        suffixType: 1,
                        suffixLimit: 0,
                        suffixSpecifyChar: '',
                        ignoreChar: '',
                        useUnrealMask: false,
                        unrealMaskLength: 1,
                      }"
                      disabled
                      detailed />
                  </template>
                  <template v-if="column.key === 'F_Color'">
                    <JnpfInput
                      v-model:value="record.F_Color"
                      :maskConfig="{
                        filler: '*',
                        maskType: 1,
                        prefixType: 1,
                        prefixLimit: 0,
                        prefixSpecifyChar: '',
                        suffixType: 1,
                        suffixLimit: 0,
                        suffixSpecifyChar: '',
                        ignoreChar: '',
                        useUnrealMask: false,
                        unrealMaskLength: 1,
                      }"
                      disabled
                      detailed />
                  </template>
                  <template v-if="column.key === 'F_Material'">
                    <JnpfInput
                      v-model:value="record.F_Material"
                      :maskConfig="{
                        filler: '*',
                        maskType: 1,
                        prefixType: 1,
                        prefixLimit: 0,
                        prefixSpecifyChar: '',
                        suffixType: 1,
                        suffixLimit: 0,
                        suffixSpecifyChar: '',
                        ignoreChar: '',
                        useUnrealMask: false,
                        unrealMaskLength: 1,
                      }"
                      disabled
                      detailed />
                  </template>
                  <template v-if="column.key === 'F_F_AuditStatus'">
                    <JnpfInput
                      v-model:value="record.F_F_AuditStatus"
                      :maskConfig="{
                        filler: '*',
                        maskType: 1,
                        prefixType: 1,
                        prefixLimit: 0,
                        prefixSpecifyChar: '',
                        suffixType: 1,
                        suffixLimit: 0,
                        suffixSpecifyChar: '',
                        ignoreChar: '',
                        useUnrealMask: false,
                        unrealMaskLength: 1,
                      }"
                      disabled
                      detailed />
                  </template>
                  <template v-if="column.key === 'F_Files'">
                    <JnpfUploadFile
                      v-model:value="record.F_Files"
                      buttonText="点击上传"
                      pathType="defaultPath"
                      :fileSize="10"
                      sizeUnit="MB"
                      :limit="9"
                      disabled
                      detailed />
                  </template>
                  <template v-if="column.key === 'F_CreatorUserId'">
                    <p>{{ record.F_CreatorUserId }}</p>
                  </template>
                  <template v-if="column.key === 'F_CreatorTime'">
                    <p>{{ record.F_CreatorTime }}</p>
                  </template>
                </template>
              </a-table>
            </a-form-item>
          </a-col>
        </a-row>
      </a-form>
    </a-row>
    <a-row :gutter="15" style="margin-top: 16px">
      <a-col :span="24" class="ant-col-item">
        <a-form-item label="">
          <JnpfGroupTitle content="操作日志" :bordered="false" />
          <div class="log-list" style="padding: 8px 0">
            <template v-if="dataForm.logs && dataForm.logs.length">
              <div
                v-for="log in dataForm.logs"
                :key="log.id"
                class="log-item"
                style="border-top: 1px solid #f0f0f0; padding: 12px 0; display: flex; align-items: center">
                <div style="width: 180px; color: #8c8c8c; font-size: 12px">{{ log.F_CreatorTime }}</div>
                <div style="flex: 1; overflow: hidden">
                  <div style="display: flex; align-items: center; gap: 12px">
                    <div style="font-weight: 600; white-space: nowrap; margin-right: 8px">{{ log.F_Title }}</div>
                    <div style="color: #595959; white-space: nowrap; overflow: hidden; text-overflow: ellipsis">{{ log.F_Content }}</div>
                  </div>
                </div>
                <div style="color: #8c8c8c; min-width: 140px; text-align: right; margin-left: 12px">{{ log.F_CreatorUserId }}</div>
              </div>
            </template>
            <template v-else>
              <p class="leading-32px">暂无操作日志</p>
            </template>
          </div>
        </a-form-item>
      </a-col>
    </a-row>
  </BasicModal>
</template>
<script lang="ts" setup>
  import { getDetail } from './helper/api';
  import { getConfigData } from '@/api/onlineDev/visualDev';
  import { reactive, toRefs, nextTick, ref, computed, unref, toRaw } from 'vue';
  import { thousandsFormat } from '@/utils/jnpf';
  import { CaretRightOutlined } from '@ant-design/icons-vue';
  import { buildUUID } from '@/utils/uuid';
  import { useI18n } from '@/hooks/web/useI18n';
  import { getDataChange } from '@/api/onlineDev/visualDev';
  import { getDataInterfaceDataInfoByIds } from '@/api/systemData/dataInterface';
  import ExtraRelationInfo from '@/components/Jnpf/RelationForm/src/ExtraRelationInfo.vue';
  import { BasicModal, useModal } from '@/components/Modal';
  import { usePermission } from '@/hooks/web/usePermission';

  interface State {
    dataForm: any;
    interfaceRes: any;
    extraOptions: any;
    extraData: any;
    title: string;
    tableFieldf4dfafouterActiveKey: number[];
    tableFieldf4dfafinnerActiveKey: string[];
  }

  defineOptions({ name: 'Detail' });
  const [registerModal, { openModal, closeModal, setModalProps }] = useModal();
  const { t } = useI18n();
  const tableFieldf4dfafColumns: any[] = computed(() => {
    let list = [
      {
        title: '商品',
        dataIndex: 'F_CustomerId',
        key: 'F_CustomerId',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_CustomerId',
        fixed: false,
      },
      {
        title: '单价(元)',
        dataIndex: 'F_Price',
        key: 'F_Price',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_Price',
        fixed: false,
      },
      {
        title: '数量',
        dataIndex: 'F_SaleQty',
        key: 'F_SaleQty',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_SaleQty',
        fixed: false,
      },
      {
        title: '单位',
        dataIndex: 'F_UnitTwo',
        key: 'F_UnitTwo',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_UnitTwo',
        fixed: false,
      },
      {
        title: '折扣(%)',
        dataIndex: 'F_Discount',
        key: 'F_Discount',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_Discount',
        fixed: false,
      },
      {
        title: '优惠金额(元)',
        dataIndex: 'F_DiscountAmount',
        key: 'F_DiscountAmount',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_DiscountAmount',
        fixed: false,
      },
      {
        title: '折后金额(元)',
        dataIndex: 'F_DiscountedAmount',
        key: 'F_DiscountedAmount',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_DiscountedAmount',
        fixed: false,
      },
      {
        title: '门板厚度',
        dataIndex: 'F_DoorPlateThickness',
        key: 'F_DoorPlateThickness',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_DoorPlateThickness',
        fixed: false,
      },
      {
        title: '箱体板厚',
        dataIndex: 'F_BoxPlateThickness',
        key: 'F_BoxPlateThickness',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_BoxPlateThickness',
        fixed: false,
      },
      {
        title: '安装或安装梁',
        dataIndex: 'F_InstallOrBeam',
        key: 'F_InstallOrBeam',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_InstallOrBeam',
        fixed: false,
      },
      {
        title: '锁具',
        dataIndex: 'F_LockSet',
        key: 'F_LockSet',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_LockSet',
        fixed: false,
      },
      {
        title: '铰链',
        dataIndex: 'F_Hinge',
        key: 'F_Hinge',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_Hinge',
        fixed: false,
      },
      {
        title: '颜色',
        dataIndex: 'F_Color',
        key: 'F_Color',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_Color',
        fixed: false,
      },
      {
        title: '材质',
        dataIndex: 'F_Material',
        key: 'F_Material',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_Material',
        fixed: false,
      },
      {
        title: '附件',
        dataIndex: 'F_Files',
        key: 'F_Files',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_Files',
        fixed: false,
      },
      {
        title: '商品备注',
        dataIndex: 'F_Description',
        key: 'F_Description',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_Description',
        fixed: false,
      },
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
    const actionColumn = { title: t('component.table.action'), dataIndex: 'action', key: 'action', align: 'center', width: 50, fixed: 'right' };
    let columns = [noColumn, ...columnList];
    const leftFixedList = columns.filter(o => o.fixed === 'left');
    const rightFixedList = columns.filter(o => o.fixed === 'right');
    const noFixedList = columns.filter(o => o.fixed !== 'left' && o.fixed !== 'right');
    return [...leftFixedList, ...noFixedList, ...rightFixedList];
  });

  const relationDetailRef = ref<any>(null);
  const state = reactive<State>({
    dataForm: {},
    interfaceRes: {
      F_ContactId: [
        {
          defaultValue: '',
          field: 'Id',
          dataType: 'varchar',
          required: 0,
          fieldName: '',
          relationField: '2009181616060108800',
          jnpfKey: null,
          complexHeaderList: null,
          sourceType: 2,
          isChildren: false,
          IsMultiple: false,
        },
      ],
    },
    extraOptions: {
      F_ContactId: [],
    },
    extraData: {
      F_ContactId: {},
    },
    title: '详情',
    tableFieldf4dfafouterActiveKey: [0],
    tableFieldf4dfafinnerActiveKey: [],
  });
  const { title, dataForm } = toRefs(state);
  const { hasFormP } = usePermission();

  defineExpose({ init });

  function init(data) {
    state.dataForm.id = data.id;
    openModal();
    nextTick(() => {
      setTimeout(initData, 0);
    });
  }
  function initData() {
    changeLoading(true);
    if (state.dataForm.id) {
      getData(state.dataForm.id);
    } else {
      closeModal();
    }
  }
  function getData(id) {
    getDetail(id).then(res => {
      state.dataForm = res.data || {};

      for (let i = 0; i < state.dataForm.tableFieldf4dfaf.length; i++) {
        const element = state.dataForm.tableFieldf4dfaf[i];
        element.jnpfId = buildUUID();
        // 计算 折后金额 和 优惠金额
        const price = Number(element.F_Price) || 0;
        const qty = Number(element.F_SaleQty) || 0;
        const discountRaw = Number(element.F_Discount);

        const multiplier = isNaN(discountRaw) ? 1 : discountRaw / 100;
        const originalTotal = price * qty;
        const discountedTotal = originalTotal * multiplier;
        element.F_DiscountedAmount = Number(discountedTotal.toFixed(2));
        element.F_DiscountAmount = Number((originalTotal - discountedTotal).toFixed(2));
      }
      nextTick(() => {
        changeLoading(false);
        getF_ContactIdExtraInfo();
      });
    });
  }
  function setFormProps(data) {
    setModalProps(data);
  }
  function changeLoading(loading) {
    setFormProps({ loading });
  }
  function getParamList(key) {
    let templateJson: any[] = state.interfaceRes[key];
    if (!templateJson || !templateJson.length || !state.dataForm) return templateJson;
    for (let i = 0; i < templateJson.length; i++) {
      if (templateJson[i].relationField && templateJson[i].sourceType == 1) {
        templateJson[i].defaultValue = state.dataForm[templateJson[i].relationField + '_id'] || '';
      }
    }
    return templateJson;
  }

  function getF_ContactIdExtraInfo() {
    if (!state.dataForm.F_ContactId_id) return;
    const paramList = getParamList('F_ContactId');
    const query = {
      ids: [state.dataForm.F_ContactId_id],
      interfaceId: '2009459664370143232',
      propsValue: 'F_Id',
      relationField: 'F_ContactName',
      paramList,
    };
    getDataInterfaceDataInfoByIds('2009459664370143232', query).then(res => {
      const data = res.data && res.data.length ? res.data[0] : {};
      state.extraData.F_ContactId = data;
    });
  }
</script>
