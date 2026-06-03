<template>
  <BasicModal v-bind="$attrs" @register="registerModal" :title="title" width="70%" :minHeight="100" :showOkBtn="false" :canFullscreen="true">
    <template #insertFooter> </template>
    <a-row class="dynamic-form">
      <a-form :colon="false" layout="horizontal" labelAlign="right" :labelCol="{ style: { width: '100px' } }" :model="dataForm" ref="formRef">
        <a-row :gutter="15">
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_QuoteCode')">
            <a-form-item name="F_QuoteCode" :labelCol="{ style: { width: '100px' } }">
              <template #label>报价单号</template>
              <JnpfInput
                v-model:value="dataForm.F_QuoteCode"
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_AddressId')">
            <a-form-item name="F_AddressId" :labelCol="{ style: { width: '100px' } }">
              <template #label>客户地址</template>
              <p>{{ dataForm.F_AddressId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_QuoteAmount')">
            <a-form-item name="F_QuoteAmount" :labelCol="{ style: { width: '100px' } }">
              <template #label>报价金额(元)</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_QuoteAmount"
                placeholder="请输入"
                :controls="false"
                :style="{
                  width: '100%',
                }"
                disabled
                detailed />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_GoodsTotal')">
            <a-form-item name="F_GoodsTotal" :labelCol="{ style: { width: '100px' } }">
              <template #label>商品总数</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_GoodsTotal"
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_QuoteDate')">
            <a-form-item name="F_QuoteDate" :labelCol="{ style: { width: '100px' } }">
              <template #label>报价日期</template>
              <p>{{ dataForm.F_QuoteDate }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_SalesmanId')">
            <a-form-item name="F_SalesmanId" :labelCol="{ style: { width: '100px' } }">
              <template #label>业务员</template>
              <p>{{ dataForm.F_SalesmanId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_QuoteStatus')">
            <a-form-item name="F_QuoteStatus" :labelCol="{ style: { width: '100px' } }">
              <template #label>报价状态</template>
              <JnpfInput
                v-model:value="dataForm.F_QuoteStatus"
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
          <!-- <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_AuditStatus')">
            <a-form-item name="F_AuditStatus" :labelCol="{ style: { width: '100px' } }">
              <template #label>审核状态</template>
              <JnpfInput
                v-model:value="dataForm.F_AuditStatus"
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
          </a-col> -->
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_Description')">
            <a-form-item name="F_Description" :labelCol="{ style: { width: '100px' } }">
              <template #label>备注</template>
              <p>{{ dataForm.F_Description }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="选择报价单商品" :bordered="false" />
              <a-table
                :data-source="dataForm.tableFielddc29f7"
                :columns="tableFielddc29f7Columns"
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
                      :controls="false"
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
                      placeholder="请输入"
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
          <!-- <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_CreatorUserId')">
            <a-form-item name="F_CreatorUserId" :labelCol="{ style: { width: '100px' } }">
              <template #label>创建人员</template>
              <p>{{ dataForm.F_CreatorUserId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_CreatorTime')">
            <a-form-item name="F_CreatorTime" :labelCol="{ style: { width: '100px' } }">
              <template #label>创建时间</template>
              <p>{{ dataForm.F_CreatorTime }}</p>
            </a-form-item>
          </a-col> -->
        </a-row>
      </a-form>
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
    tableFielddc29f7outerActiveKey: number[];
    tableFielddc29f7innerActiveKey: string[];
  }

  defineOptions({ name: 'Detail' });
  const [registerModal, { openModal, closeModal, setModalProps }] = useModal();
  const { t } = useI18n();
  const tableFielddc29f7Columns: any[] = computed(() => {
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
      // {
      //   title: '创建人员',
      //   dataIndex: 'F_CreatorUserId',
      //   key: 'F_CreatorUserId',
      //   tipLabel: '',
      //   align: 'left',
      //   span: '24',
      //   labelWidth: '',
      //   width: '',
      //   formP: 'F_CreatorUserId',
      //   fixed: false,
      // },
      // {
      //   title: '创建时间',
      //   dataIndex: 'F_CreatorTime',
      //   key: 'F_CreatorTime',
      //   tipLabel: '',
      //   align: 'left',
      //   span: '24',
      //   labelWidth: '',
      //   width: '',
      //   formP: 'F_CreatorTime',
      //   fixed: false,
      // },
    ];
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
    tableFielddc29f7outerActiveKey: [0],
    tableFielddc29f7innerActiveKey: [],
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

      for (let i = 0; i < state.dataForm.tableFielddc29f7.length; i++) {
        const element = state.dataForm.tableFielddc29f7[i];
        element.jnpfId = buildUUID();
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
      propsValue: 'id',
      relationField: 'fullName',
      paramList,
    };
    getDataInterfaceDataInfoByIds('2009459664370143232', query).then(res => {
      const data = res.data && res.data.length ? res.data[0] : {};
      state.extraData.F_ContactId = data;
    });
  }
</script>
