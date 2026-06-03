<template>
  <BasicModal v-bind="$attrs" @register="registerModal" :title="title" width="70%" :minHeight="100" :showOkBtn="false">
    <template #insertFooter> </template>
    <a-row class="dynamic-form">
      <a-form :colon="false" layout="horizontal" labelAlign="right" :labelCol="{ style: { width: '100px' } }" :model="dataForm" ref="formRef">
        <a-row :gutter="15">
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_TransferNo')">
            <a-form-item name="F_TransferNo" :labelCol="{ style: { width: '100px' } }">
              <template #label>调拨单号</template>
              <p>{{ dataForm.F_TransferNo }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_TransferDate')">
            <a-form-item name="F_TransferDate" :labelCol="{ style: { width: '100px' } }">
              <template #label>调拨日期：</template>
              <p>{{ dataForm.F_TransferDate }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_TransferUserId')">
            <a-form-item name="F_TransferUserId" :labelCol="{ style: { width: '100px' } }">
              <template #label>调拨人：</template>
              <p>{{ dataForm.F_TransferUserId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_FromWarehouseId')">
            <a-form-item name="F_FromWarehouseId" :labelCol="{ style: { width: '100px' } }">
              <template #label>发出仓库：</template>
              <p>{{ dataForm.F_FromWarehouseId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_ToWarehouseId')">
            <a-form-item name="F_ToWarehouseId" :labelCol="{ style: { width: '100px' } }">
              <template #label>收入仓库：</template>
              <p>{{ dataForm.F_ToWarehouseId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="16" class="ant-col-item" v-if="hasFormP('F_Description')">
            <a-form-item name="F_Description" :labelCol="{ style: { width: '100px' } }">
              <template #label>备注：</template>
              <p>{{ dataForm.F_Description }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle :content="t('商品')" :bordered="true" />
              <BasicTable :scroll="{ y: 400 }" @register="registerTable" @resizeColumn="handleResizeColumn">
                <template #bodyCell="{ column, record }"> </template>
              </BasicTable>
            </a-form-item>
          </a-col>

          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle :content="t('日志')" :bordered="true" />
              <BasicTable :scroll="{ y: 400 }" @register="registerLogTable" @resizeColumn="handleResizeColumn">
                <template #bodyCell="{ column, record }">
                  <!-- 只让“内容”列折行 -->
                  <template v-if="column.dataIndex === 'F_Content'">
                    <div style="white-space: normal; word-break: break-all">
                      {{ record.F_Content }}
                    </div>
                  </template></template
                >
              </BasicTable>
            </a-form-item>
          </a-col>
        </a-row>
      </a-form>
    </a-row>
  </BasicModal>
</template>
<script lang="ts" setup>
  import { getDetail, getLogList, getGoodsList } from './helper/api';
  import { getConfigData } from '@/api/onlineDev/visualDev';
  import { reactive, toRefs, nextTick, ref, computed, unref, toRaw } from 'vue';
  import { buildUUID } from '@/utils/uuid';
  import { useI18n } from '@/hooks/web/useI18n';
  import { BasicModal, useModal } from '@/components/Modal';
  import { usePermission } from '@/hooks/web/usePermission';

  import { BasicTable, useTable, TableAction, BasicColumn, FormProps, ActionItem } from '@/components/Table';
  let tableReload: Function = () => {};
  let tableReloadLog: Function = () => {};

  interface State {
    dataForm: any;
    interfaceRes: any;
    extraOptions: any;
    extraData: any;
    title: string;
  }

  defineOptions({ name: 'Detail' });
  const [registerModal, { openModal, closeModal, setModalProps }] = useModal();
  const { t } = useI18n();

  const relationDetailRef = ref<any>(null);
  const state = reactive<State>({
    dataForm: {},
    interfaceRes: {},
    extraOptions: {},
    extraData: {},
    title: '详情',
  });
  const { title, dataForm } = toRefs(state);
  const { hasFormP } = usePermission();

  defineExpose({ init });

  function init(data) {
    state.dataForm.id = data.id;
    openModal();
    /* 手动刷新表格 */
    tableReload();
    tableReloadLog();
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

  const columns: BasicColumn[] = [
    { title: '商品名', dataIndex: 'F_GoodsName', resizable: true, width: 150 },
    { title: '商品编号', dataIndex: 'F_GoodsNo', resizable: true, width: 150 },
    { title: '商品规格', dataIndex: 'F_Specification', resizable: true, width: 100 },
    // { title: '库存数量', dataIndex: 'F_InventoryNum', resizable: true, width: 100 },
    { title: '数量', dataIndex: 'F_Num', resizable: true, width: 100 },
    { title: '成本单价(元)', dataIndex: 'F_Price', resizable: true, width: 100 },
    { title: '销售单价(元)', dataIndex: 'F_SalesPrice', resizable: true, width: 100 },
    { title: '单位', dataIndex: 'F_UnitTwo', resizable: true, width: 100 },
    { title: '备注', dataIndex: 'F_Description', resizable: true },
  ];

  const [registerTable, { reload }] = useTable({
    api: getGoodsList,
    beforeFetch: data => {
      data.F_TransferId = state.dataForm.id;
      return data;
    },
    columns,
    useSearchForm: false,
    showTableSetting: false,
    showIndexColumn: false,
  });

  const columnsLog: BasicColumn[] = [
    { title: '时间', dataIndex: 'F_CreatorTime', resizable: true, width: 150 },
    { title: '人员', dataIndex: 'F_CreatorUserId', resizable: true, width: 150 },
    { title: '标题', dataIndex: 'F_Title', resizable: true, width: 150 },
    { title: '内容', dataIndex: 'F_Content', resizable: true },
  ];

  const [registerLogTable, { reload: reloadLog }] = useTable({
    api: getLogList,
    beforeFetch: data => {
      data.F_TransferId = state.dataForm.id;
      return data;
    },
    columns: columnsLog,
    useSearchForm: false,
    showTableSetting: false,
    showIndexColumn: false,
  });

  tableReload = reload;
  tableReloadLog = reloadLog;

  function handleResizeColumn(w, col) {
    col.width = w;
  }
</script>
