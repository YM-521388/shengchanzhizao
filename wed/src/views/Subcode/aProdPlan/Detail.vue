<template>
  <BasicModal v-bind="$attrs" @register="registerModal" :title="title" width="1300px" :minHeight="100" :showOkBtn="false" :canFullscreen="true">
    <template #insertFooter> </template>
    <a-row class="dynamic-form">
      <a-form :colon="false" layout="horizontal" labelAlign="right" :labelCol="{ style: { width: '100px' } }" :model="dataForm" ref="formRef">
        <a-row :gutter="15">
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_PlanNo')">
            <a-form-item name="F_PlanNo" :labelCol="{ style: { width: '100px' } }">
              <template #label>计划编号：</template>
              <JnpfInput
                v-model:value="dataForm.F_PlanNo"
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_PlanName')">
            <a-form-item name="F_PlanName" :labelCol="{ style: { width: '100px' } }">
              <template #label>计划名称：</template>
              <JnpfInput
                v-model:value="dataForm.F_PlanName"
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_DeliveryDate')">
            <a-form-item name="F_DeliveryDate" :labelCol="{ style: { width: '100px' } }">
              <template #label>交货日期：</template>
              <p>{{ dataForm.F_DeliveryDate }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_Description')">
            <a-form-item name="F_Description" :labelCol="{ style: { width: '100px' } }">
              <template #label>备注：</template>
              <p>{{ dataForm.F_Description }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <BasicTable v-bind="getTableBindValue" :scroll="{ y: 400 }" @register="registerTable" @resizeColumn="handleResizeColumn">
                <template #bodyCell="{ column, record }">
                  <template v-if="column.key === 'action' && !record.top">
                    <TableAction :actions="getTableActions(record)" :dropDownActions="getDropDownActions(record)" />
                  </template>
                </template>
              </BasicTable>
            </a-form-item>
          </a-col>
        </a-row>
      </a-form>
    </a-row>
  </BasicModal>
  <ProcessingForm ref="processingRef" @reload="reload" />
  <ProdOutsourceForm ref="outsourceRef" @reload="reload" />
  <APuOrderForm ref="aPuOrderRef" @reload="reload" />
</template>
<script lang="ts" setup>
  import { getDetail } from './helper/api';
  import ProcessingForm from '@/views/Subcode/aProdProcessing/Form.vue';
  import ProdOutsourceForm from '@/views/Subcode/aProdOutsource/Form.vue';
  import APuOrderForm from '@/views/Subcode/aPuOrder/Form.vue';
  import { reactive, toRefs, nextTick, ref, computed } from 'vue';
  import { useI18n } from '@/hooks/web/useI18n';
  import { BasicModal, useModal } from '@/components/Modal';
  import { BasicTable, useTable, TableAction, BasicColumn, ActionItem } from '@/components/Table';
  import { getPlanItemList } from '@/views/Subcode/aProdPlanItem/helper/api';
  import { usePermission } from '@/hooks/web/usePermission';
  let tableReload: Function = () => {};

  interface State {
    dataForm: any;
    interfaceRes: any;
    extraOptions: any;
    extraData: any;
    title: string;
  }

  defineOptions({ name: 'Detail' });
  const { hasFormP } = usePermission();
  const [registerModal, { openModal, closeModal, setModalProps }] = useModal();
  const { t } = useI18n();
  const processingRef = ref<any>(null);
  const outsourceRef = ref<any>(null);
  const aPuOrderRef = ref<any>(null);

  // relationDetailRef unused by this component
  const state = reactive<State>({
    dataForm: {},
    interfaceRes: {},
    extraOptions: {},
    extraData: {},
    title: '详情',
  });
  const { title, dataForm } = toRefs(state);

  defineExpose({ init });

  function init(data) {
    state.dataForm.id = data.id;
    openModal();
    /* 手动刷新表格 */
    tableReload();
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
      nextTick(() => {
        changeLoading(false);
      });
    });
  }
  function setFormProps(data) {
    setModalProps(data);
  }
  function changeLoading(loading) {
    setFormProps({ loading });
  }

  function getTableActions(record): ActionItem[] {
    return [];
  }
  //把按钮放在更多中
  function getDropDownActions(record): ActionItem[] {
    return [
      {
        label: '创建加工单',
        onClick: onProcessingAction.bind(null, record),
        auth: 'btn_add_processing',
      },
      // {
      //   label: '创建外协工单',
      //   onClick: onOutsourceAction.bind(null, record),
      // },
      {
        label: '创建采购单',
        onClick: onPurchaseAction.bind(null, record),
        auth: 'btn_add_order',
      },
    ];
  }
  function onProcessingAction(record) {
    // 不带工作流；传入 menuId 以便弹窗内 hasFormP 使用加工单模型的权限
    const data = {
      id: '',
      F_GoodsId: record.F_GoodsById,
      F_Num: record.F_Num,
      fullName: '创建加工单',
      menuId: '2029121606537842688', // 加工单(aProdProcessing) 的 menuId
    };
    processingRef.value?.init(data);
  }
  function onOutsourceAction(record) {
    const data = {
      id: '',
      F_GoodsId: record.F_GoodsById,
      F_PlanNum: record.F_Num,
      fullName: '创建外协工单',
    };
    outsourceRef.value?.init(data);
  }
  function onPurchaseAction(record) {
    // 打开采购单表单并预填生产计划 ID（注意：Form.init 支持 prefill 字段）
    aPuOrderRef.value?.init({ id: '', allList: [], prefill: { F_ProdPlanId: state.dataForm.id } });
  }
  const getTableBindValue = computed(() => {
    const defaultSortConfig = [];
    const sortField = defaultSortConfig.map(o => (o.sort === 'desc' ? '-' : '') + o.field);
    const data: any = {
      pagination: { pageSize: 20 }, //有分页
      ellipsis: true,
      defSort: { sidx: sortField.join(',') },
      bordered: true,
      actionColumn: {
        width: 90,
        title: t('component.table.action'),
        dataIndex: 'action',
      },
    };
    return data;
  });
  const allColumns: BasicColumn[] = [
    { title: '商品名', dataIndex: 'F_GoodsId', resizable: true },
    { title: '商品编号', dataIndex: 'F_GoodsNo', resizable: true },
    { title: '规格', dataIndex: 'F_Specification', resizable: true },
    { title: '计划数量', dataIndex: 'F_NumUnit', resizable: true },
    { title: '单位', dataIndex: 'F_Unit', resizable: true },
    { title: '商品BOM', dataIndex: 'F_BOMId', resizable: true },
    { title: '备注', dataIndex: 'F_Description', resizable: true },
  ];
  const columns = computed(() => allColumns.filter(col => typeof col.dataIndex === 'string' && hasFormP(('SPLB-' + col.dataIndex) as string)));

  const [registerTable, { reload }] = useTable({
    api: getPlanItemList,
    beforeFetch: data => {
      data.F_ProdPlanId = state.dataForm.id;
      return data;
    },
    columns,
    useSearchForm: false,
    showTableSetting: false,
    showIndexColumn: true,
  });

  tableReload = reload;
  function handleResizeColumn(w, col) {
    col.width = w;
  }
</script>
