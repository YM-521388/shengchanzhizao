<template>
  <BasicModal v-bind="$attrs" @register="registerModal" :title="title" width="70%" :minHeight="100" :showOkBtn="false" :canFullscreen="true">
    <template #insertFooter> </template>
    <a-row class="dynamic-form">
      <a-form :colon="false" layout="horizontal" labelAlign="right" :labelCol="{ style: { width: '100px' } }" :model="dataForm" ref="formRef">
        <a-row :gutter="15">
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_OrderNo')">
            <a-form-item name="F_OrderNo" :labelCol="{ style: { width: '100px' } }">
              <template #label>采购单号</template>
              <JnpfInput
                v-model:value="dataForm.F_OrderNo"
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_SupplierId')">
            <a-form-item name="F_SupplierId" :labelCol="{ style: { width: '100px' } }">
              <template #label>供应商</template>
              <p>{{ dataForm.F_SupplierId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_Money')">
            <a-form-item name="F_Money" :labelCol="{ style: { width: '100px' } }">
              <template #label>商品金额</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_Money"
                placeholder="请输入"
                :controls="false"
                :style="{
                  width: '100%',
                }"
                disabled
                detailed />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_PurchaseNum')">
            <a-form-item name="F_PurchaseNum" :labelCol="{ style: { width: '100px' } }">
              <template #label>采购数量</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_PurchaseNum"
                placeholder="请输入"
                :controls="false"
                :style="{
                  width: '100%',
                }"
                disabled
                detailed />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_ProdPlanId')">
            <a-form-item name="F_ProdPlanId" :labelCol="{ style: { width: '100px' } }">
              <template #label>采购计划</template>
              <p class="leading-32px">{{ dataForm.F_ProdPlanId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_UserId')">
            <a-form-item name="F_UserId" :labelCol="{ style: { width: '100px' } }">
              <template #label>采购人</template>
              <p>{{ dataForm.F_UserId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_DeliveryDate')">
            <a-form-item name="F_DeliveryDate" :labelCol="{ style: { width: '100px' } }">
              <template #label>交货日期</template>
              <p>{{ dataForm.F_DeliveryDate }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_CheckState')">
            <a-form-item name="F_CheckState" :labelCol="{ style: { width: '100px' } }">
              <template #label>审核状态</template>
              <p>{{ dataForm.F_CheckState }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_CheckUserId')">
            <a-form-item name="F_CheckUserId" :labelCol="{ style: { width: '100px' } }">
              <template #label>审核人员</template>
              <p>{{ dataForm.F_CheckUserId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_CheckTime')">
            <a-form-item name="F_CheckTime" :labelCol="{ style: { width: '100px' } }">
              <template #label>审核时间</template>
              <p>{{ dataForm.F_CheckTime }}</p>
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
              <JnpfGroupTitle content="采购单商品" :bordered="false" />
              <a-table
                :data-source="dataForm.tableFieldf01abd"
                :columns="tableFieldf01abdColumns"
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
                  <template v-if="column.key === 'F_SupplierId'">
                    <p>{{ record.F_SupplierId }}</p>
                  </template>
                  <template v-if="column.key === 'F_NumInfo'">
                    <p>{{ record.F_NumInfo }}</p>
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
                  <template v-if="column.key === 'F_DiscountMoney'">
                    <JnpfInputNumber
                      v-model:value="record.F_DiscountMoney"
                      placeholder="请输入"
                      :controls="false"
                      :style="{
                        width: '100%',
                      }"
                      disabled
                      detailed />
                  </template>
                  <template v-if="column.key === 'F_Description'">
                    <p>{{ record.F_Description }}</p>
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
          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="采购单日志" :bordered="false" />
              <a-table
                :data-source="dataForm.tableField8b2a57"
                :columns="tableField8b2a57Columns"
                size="small"
                :pagination="false"
                :scroll="{ x: 'max-content' }">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                  <template v-if="column.key === 'F_Title'">
                    <JnpfInput
                      v-model:value="record.F_Title"
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
                  <template v-if="column.key === 'F_Content'">
                    <p>{{ record.F_Content }}</p>
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
    tableFieldf01abdouterActiveKey: number[];
    tableFieldf01abdinnerActiveKey: string[];
    tableField8b2a57outerActiveKey: number[];
    tableField8b2a57innerActiveKey: string[];
  }

  defineOptions({ name: 'Detail' });
  const [registerModal, { openModal, closeModal, setModalProps }] = useModal();
  const { t } = useI18n();
  const tableFieldf01abdColumns: any[] = computed(() => {
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
        title: '供应商',
        dataIndex: 'F_SupplierId',
        key: 'F_SupplierId',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_SupplierId',
        fixed: false,
      },
      {
        title: '数量',
        dataIndex: 'F_NumInfo',
        key: 'F_NumInfo',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_NumInfo',
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
        dataIndex: 'F_DiscountMoney',
        key: 'F_DiscountMoney',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_DiscountMoney',
        fixed: false,
      },
      {
        title: '备注',
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
    list = list.filter(o => hasFormP('Detail_CGD-' + o.formP));
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
  const tableField8b2a57Columns: any[] = computed(() => {
    let list = [
      {
        title: '标题',
        dataIndex: 'F_Title',
        key: 'F_Title',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_Title',
        fixed: false,
      },
      {
        title: '内容',
        dataIndex: 'F_Content',
        key: 'F_Content',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_Content',
        fixed: false,
      },
      {
        title: '修改用户',
        dataIndex: 'F_CreatorUserId',
        key: 'F_CreatorUserId',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_CreatorUserId',
        fixed: false,
      },
      {
        title: '修改时间',
        dataIndex: 'F_CreatorTime',
        key: 'F_CreatorTime',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_CreatorTime',
        fixed: false,
      },
    ];
    list = list.filter(o => hasFormP('tableField8b2a57-' + o.formP));

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
      F_ProdPlanId: [],
    },
    extraOptions: {
      F_ProdPlanId: [],
    },
    extraData: {
      F_ProdPlanId: {},
    },
    title: '详情',
    tableFieldf01abdouterActiveKey: [0],
    tableFieldf01abdinnerActiveKey: [],
    tableField8b2a57outerActiveKey: [0],
    tableField8b2a57innerActiveKey: [],
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

      for (let i = 0; i < state.dataForm.tableFieldf01abd.length; i++) {
        const element = state.dataForm.tableFieldf01abd[i];
        element.jnpfId = buildUUID();
      }
      for (let i = 0; i < state.dataForm.tableField8b2a57.length; i++) {
        const element = state.dataForm.tableField8b2a57[i];
        element.jnpfId = buildUUID();
      }
      nextTick(() => {
        changeLoading(false);
        getF_ProdPlanIdExtraInfo();
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

  function getF_ProdPlanIdExtraInfo() {
    if (!state.dataForm.F_ProdPlanId_id) return;
    const paramList = getParamList('F_ProdPlanId');
    const query = {
      ids: [state.dataForm.F_ProdPlanId_id],
      interfaceId: '2011711502578487296',
      propsValue: 'F_Id',
      relationField: 'F_PlanNo',
      paramList,
    };
    getDataInterfaceDataInfoByIds('2011711502578487296', query).then(res => {
      const data = res.data && res.data.length ? res.data[0] : {};
      state.extraData.F_ProdPlanId = data;
    });
  }
</script>
