<template>
  <BasicModal v-bind="$attrs" @register="registerModal" :title="title" width="1200px" :minHeight="100" :showOkBtn="false">
    <template #insertFooter> </template>
    <a-row class="dynamic-form">
      <a-form :colon="false" layout="horizontal" labelAlign="right" :labelCol="{ style: { width: '100px' } }" :model="dataForm" ref="formRef">
        <a-row :gutter="15">
          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <BasicTable :scroll="{ y: 400 }" @register="registerTable" @resizeColumn="handleResizeColumn">
                <template #bodyCell="{ column, record }">
                  <template v-if="column.key === 'F_UserType'">
                    {{ record.F_UserType == '0' ? t('dectionary.fy92') : t('zapp.fy777') }}
                  </template>
                  <template v-if="column.key === 'F_LocationType'">
                    <a-tag :color="record.F_LocationType == '1' ? 'red' : 'cyan'">
                      {{ record.F_LocationType == '1' ? t('dectionary.fy1138') : t('dectionary.fy1137') }}
                    </a-tag>
                  </template>
                </template>
              </BasicTable>
            </a-form-item>
          </a-col>
        </a-row>
      </a-form>
    </a-row>
  </BasicModal>
</template>
<script lang="ts" setup>
  import { getDetail } from './helper/api';
  import { reactive, toRefs, nextTick, ref, computed, unref, toRaw } from 'vue';
  import { buildUUID } from '@/utils/uuid';
  import { useI18n } from '@/hooks/web/useI18n';
  import { getDataInterfaceDataInfoByIds } from '@/api/systemData/dataInterface';
  import { BasicModal, useModal } from '@/components/Modal';
  import { BasicTable, useTable, TableAction, BasicColumn, FormProps, ActionItem } from '@/components/Table';
  import { getRoutingGoodList } from '@/views/SubCode/aProdProjectStepItem/helper/api';
  let tableReload: Function = () => {};

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
    title: '查看--物料清单',
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

  const columns: BasicColumn[] = [
    { title: '商品名', dataIndex: 'F_GoodsId', resizable: true },
    { title: '商品编号', dataIndex: 'F_GoodsNo', resizable: true },
    { title: '规格', dataIndex: 'F_Specification', resizable: true },
    { title: '数量', dataIndex: 'F_Num', resizable: true },
    { title: '单位', dataIndex: 'F_Unit', resizable: true },
    { title: '备注', dataIndex: 'F_Description', resizable: true },
  ];

  const [registerTable, { reload }] = useTable({
    api: getRoutingGoodList,
    beforeFetch: data => {
      data.F_ProjectStepId = state.dataForm.id;
      return data;
    },
    columns,
    useSearchForm: false,
    showTableSetting: false,
    showIndexColumn: false,
  });

  tableReload = reload;
  function handleResizeColumn(w, col) {
    col.width = w;
  }
</script>
