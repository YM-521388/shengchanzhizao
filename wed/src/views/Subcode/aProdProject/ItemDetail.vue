<template>
  <BasicModal v-bind="$attrs" @register="registerModal" :title="title" width="1200px" :minHeight="100" :showOkBtn="false" :canFullscreen="true">
    <template #insertFooter> </template>
    <a-row class="dynamic-form">
      <a-form :colon="false" layout="horizontal" labelAlign="right" :labelCol="{ style: { width: '100px' } }" :model="dataForm" ref="formRef">
        <a-row :gutter="15">
          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <BasicTable :scroll="{ y: 400 }" @register="registerGoodTable" @resizeColumn="handleResizeColumn">
                <template #bodyCell="{ column, record }"> </template>
              </BasicTable>
            </a-form-item>
          </a-col>
        </a-row>
      </a-form>
    </a-row>
    <ItemRoutingDetail ref="detailRef" />
  </BasicModal>
</template>
<script lang="ts" setup>
  import { getDetail } from './helper/api';
  import ItemRoutingDetail from './ItemRoutingDetail.vue';
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
  import { getGoodList } from '@/views/SubCode/aProdProjectGood/helper/api';
  import { BasicTable, useTable, TableAction, BasicColumn, FormProps, ActionItem } from '@/components/Table';
  let tableReload: Function = () => {};
  let tableReloadGood: Function = () => {};

  interface State {
    dataForm: any;
    interfaceRes: any;
    extraOptions: any;
    extraData: any;
    title: string;
    tableField751ecbouterActiveKey: number[];
    tableField751ecbinnerActiveKey: string[];
  }

  defineOptions({ name: 'Detail' });
  const [registerModal, { openModal, closeModal, setModalProps }] = useModal();
  const { t } = useI18n();
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
    title: '查看--用料清单',
    tableField751ecbouterActiveKey: [0],
    tableField751ecbinnerActiveKey: [],
  });
  const { title, dataForm } = toRefs(state);

  defineExpose({ init });

  function init(data) {
    state.dataForm.id = data.id;
    openModal();
    /* 手动刷新表格 */
    tableReload();
    tableReloadGood();
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

      for (let i = 0; i < state.dataForm.tableField751ecb.length; i++) {
        const element = state.dataForm.tableField751ecb[i];
        element.jnpfId = buildUUID();
      }
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

  // 查看详情
  function goDetail(record) {
    // 不带流程
    const data = {
      id: record.id,
    };
    detailRef.value?.init(data);
  }

  const columnsGood: BasicColumn[] = [
    { title: '排序', dataIndex: 'F_SortCode', resizable: true },
    { title: '商品名', dataIndex: 'F_GoodsId', resizable: true },
    { title: '商品编号', dataIndex: 'F_GoodsNo', resizable: true },
    { title: '规格', dataIndex: 'F_Specification', resizable: true },
    // { title: '投料工序', dataIndex: 'F_FeedProcess', resizable: true },
    // { title: '入库工序', dataIndex: 'F_StockInProcess', align: 'left', resizable: true },
    { title: '单位用量', dataIndex: 'F_NumUnit', resizable: true },
    // { title: '生产用量', dataIndex: 'F_ProdUnit', align: 'left', resizable: true },
    { title: '单位', dataIndex: 'F_UnitTwo', align: 'left', resizable: true },
    { title: '备注', dataIndex: 'F_Description', align: 'left', resizable: true },
  ];

  const [registerGoodTable, { reload: reloadGood }] = useTable({
    api: getGoodList,
    beforeFetch: data => {
      data.F_ProjectItemId = state.dataForm.id;
      return data;
    },
    columns: columnsGood,
    useSearchForm: false,
    showTableSetting: false,
    showIndexColumn: false,
  });

  tableReloadGood = reloadGood;

  function handleResizeColumn(w, col) {
    col.width = w;
  }
</script>
