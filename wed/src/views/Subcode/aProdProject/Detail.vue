<template>
  <BasicModal v-bind="$attrs" @register="registerModal" :title="title" width="1300px" :minHeight="100" :showOkBtn="false" :canFullscreen="true">
    <template #insertFooter> </template>
    <a-row class="dynamic-form">
      <a-form :colon="false" layout="horizontal" labelAlign="right" :labelCol="{ style: { width: '100px' } }" :model="dataForm" ref="formRef">
        <a-row :gutter="15">
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_ProjectNo')">
            <a-form-item name="F_ProjectNo" :labelCol="{ style: { width: '100px' } }">
              <template #label>项目编号：</template>
              <JnpfInput
                v-model:value="dataForm.F_ProjectNo"
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
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_ProjectName')">
            <a-form-item name="F_ProjectName" :labelCol="{ style: { width: '100px' } }">
              <template #label>项目名称：</template>
              <JnpfInput
                v-model:value="dataForm.F_ProjectName"
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
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_ProjectType')">
            <a-form-item name="F_ProjectType" :labelCol="{ style: { width: '100px' } }">
              <template #label>项目类别：</template>
              <p>{{ dataForm.F_ProjectTypeName }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_ContractId') && dataForm.F_ProjectType == '1'">
            <a-form-item name="F_ContractId" :labelCol="{ style: { width: '100px' } }">
              <template #label>合同：</template>
              <p class="leading-32px">{{ dataForm.F_ContractId }}</p>
              <ExtraRelationInfo
                :extraOptions="state.extraOptions.F_ContractId"
                :data="state.extraData.F_ContractId"
                v-if="state.extraOptions.F_ContractId?.length && state.extraData.F_ContractId && JSON.stringify(state.extraData.F_ContractId) !== '{}'" />
            </a-form-item>
          </a-col>

          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle :content="t('商品')" :bordered="true" />
              <BasicTable v-bind="getTableBindValue" :scroll="{ y: 400 }" @register="registerTable" @resizeColumn="handleResizeColumn">
                <template #bodyCell="{ column, record }">
                  <template v-if="column.key === 'F_Priority'">
                    <a-tag :color="record.F_Priority == '正常' ? 'success' : record.F_Priority == '延期' ? 'warning' : 'error'">
                      {{ record.F_Priority }}
                    </a-tag>
                  </template>
                  <template v-if="column.key === 'action' && !record.top">
                    <TableAction :actions="getTableActions(record)" />
                  </template>
                </template>
              </BasicTable>
            </a-form-item>
          </a-col>
        </a-row>
      </a-form>
    </a-row>
  </BasicModal>
  <ItemDetail ref="detailRef" />
</template>
<script lang="ts" setup>
  import { getDetail } from './helper/api';
  import ItemDetail from './ItemDetail.vue';
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
  import { getItemList } from '@/views/SubCode/aProdProject/helper/api';
  import { BasicTable, useTable, TableAction, BasicColumn, FormProps, ActionItem } from '@/components/Table';
  import { usePermission } from '@/hooks/web/usePermission';
  import { useUserStore } from '@/store/modules/user';
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
  const userStore = useUserStore();
  const userInfo = userStore.getUserInfo;
  const detailRef = ref<any>(null);

  const relationDetailRef = ref<any>(null);
  const state = reactive<State>({
    dataForm: {},
    interfaceRes: {
      F_ContractId: [
        {
          defaultValue: '',
          field: 'contractId',
          dataType: 'varchar',
          required: 0,
          fieldName: '合同ID',
          relationField: '66666',
          jnpfKey: null,
          complexHeaderList: null,
          sourceType: 2,
          isChildren: false,
          IsMultiple: false,
        },
      ],
    },
    extraOptions: {
      F_ContractId: [],
    },
    extraData: {
      F_ContractId: {},
    },
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
        getF_ContractIdExtraInfo();
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

  function getF_ContractIdExtraInfo() {
    if (!state.dataForm.F_ContractId_id) return;
    const paramList = getParamList('F_ContractId');
    const query = {
      ids: [state.dataForm.F_ContractId_id],
      interfaceId: '2012085692393459712',
      propsValue: 'id',
      relationField: 'F_GoodsName',
      paramList,
    };
    getDataInterfaceDataInfoByIds('2012085692393459712', query).then(res => {
      const data = res.data && res.data.length ? res.data[0] : {};
      state.extraData.F_ContractId = data;
    });
  }

  function getTableActions(record): ActionItem[] {
    return [
      {
        label: t('查看'),
        onClick: goDetail.bind(null, record),
      },
    ];
  }

  // 查看详情
  function goDetail(record) {
    // 不带流程
    const data = {
      id: record.id,
    };
    detailRef.value?.init(data);
  }

  const allColumns: BasicColumn[] = [
    { title: '工单编号', dataIndex: 'F_WorkOrderNo', resizable: true },
    { title: '商品', dataIndex: 'F_GoodsId', resizable: true },
    { title: '计划数', dataIndex: 'F_PlanNum', resizable: true },
    { title: '交货日期', dataIndex: 'F_DeliveryDate', format: 'date|YYYY-MM-DD', resizable: true },
    { title: '优先级', dataIndex: 'F_Priority', resizable: true },
    { title: 'BOM', dataIndex: 'F_BomId', align: 'left', resizable: true },
    { title: '门板厚度', dataIndex: 'F_DoorPlateThickness', resizable: true },
    { title: '箱体板厚', dataIndex: 'F_BoxPlateThickness', align: 'left', resizable: true },
    { title: '安装板或安装梁', dataIndex: 'F_InstallPlateOrBeam', align: 'left', resizable: true },
    { title: '客户', dataIndex: 'F_CustomerName', align: 'left', resizable: true },
    { title: '锁具', dataIndex: 'F_LockSet', align: 'left', resizable: true },
    { title: '铰链', dataIndex: 'F_Hinge', align: 'left', resizable: true },
    { title: '颜色', dataIndex: 'F_Color', align: 'left', resizable: true },
    { title: '类别', dataIndex: 'F_Category', align: 'left', resizable: true },
  ];

  const columns = computed(() => allColumns.filter(col => typeof col.dataIndex === 'string' && hasFormP(('tableField751ecb-' + col.dataIndex) as string)));

  const [registerTable, { reload, getForm, getFetchParams }] = useTable({
    api: getItemList,
    beforeFetch: data => {
      data.F_ProjectId = state.dataForm.id;
      return data;
    },
    columns,
    useSearchForm: false,
    showTableSetting: false,
  });

  tableReload = reload;

  const getTableBindValue = computed(() => {
    const defaultSortConfig = [];
    const sortField = defaultSortConfig.map(o => (o.sort === 'desc' ? '-' : '') + o.field);
    const data: any = {
      pagination: { pageSize: 20 }, //有分页
      ellipsis: true,
      defSort: { sidx: sortField.join(',') },
      bordered: true,
      actionColumn: {
        width: 80,
        title: t('component.table.action'),
        dataIndex: 'action',
      },
    };
    return data;
  });

  function handleResizeColumn(w, col) {
    col.width = w;
  }
</script>
