<template>
  <BasicModal v-bind="$attrs" @register="registerModal" :title="title" width="70%" :minHeight="100" :showOkBtn="false" :canFullscreen="true">
    <template #insertFooter> </template>
    <a-row class="dynamic-form">
      <a-form :colon="false" layout="horizontal" labelAlign="right" :labelCol="{ style: { width: '100px' } }" :model="dataForm" ref="formRef">
        <a-row :gutter="15">
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('SCTL_ReturnInNo')">
            <a-form-item name="F_ReturnInNo" :labelCol="{ style: { width: '100px' } }">
              <template #label>生产退料单号：</template>
              <JnpfInput
                v-model:value="dataForm.F_ReturnInNo"
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

          <a-col :span="8" class="ant-col-item" v-if="hasFormP('SCTL_ReturnInType')">
            <a-form-item name="F_ReturnInType" :labelCol="{ style: { width: '100px' } }">
              <template #label>入库类型：</template>
              <p>{{ dataForm.F_ReturnInType }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('SCTL_WorkOrderId')">
            <a-form-item name="F_WorkOrderId" :labelCol="{ style: { width: '100px' } }">
              <template #label>加工单号：</template>
              <p>{{ dataForm.F_WorkOrderId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('SCTL_ReturnInDate')">
            <a-form-item name="F_ReturnInDate" :labelCol="{ style: { width: '100px' } }">
              <template #label>退料日期：</template>
              <p>{{ dataForm.F_ReturnInDate }}</p>
            </a-form-item>
          </a-col>
          <!-- <a-col :span="8" class="ant-col-item" v-if="hasFormP('SCTL_Type')">
            <a-form-item name="F_Type" :labelCol="{ style: { width: '100px' } }">
              <template #label>工单类型：</template>
              <JnpfInput
                v-model:value="dataForm.F_Type"
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
          <!-- <a-col :span="8" class="ant-col-item" v-if="hasFormP('SCTL_Method')">
            <a-form-item name="F_Method" :labelCol="{ style: { width: '100px' } }">
              <template #label>操作方式：</template>
              <JnpfInput
                v-model:value="dataForm.F_Method"
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
          <a-col :span="16" class="ant-col-item" v-if="hasFormP('SCTL_Description')">
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
  import { thousandsFormat } from '@/utils/jnpf';
  import { CaretRightOutlined } from '@ant-design/icons-vue';
  import { buildUUID } from '@/utils/uuid';
  import { useI18n } from '@/hooks/web/useI18n';
  import { getDataChange } from '@/api/onlineDev/visualDev';
  import { getDataInterfaceDataInfoByIds } from '@/api/systemData/dataInterface';
  import ExtraRelationInfo from '@/components/Jnpf/RelationForm/src/ExtraRelationInfo.vue';
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

  const allColumns: BasicColumn[] = [
    { title: '商品名', dataIndex: 'F_GoodsName', resizable: true, width: 150 },
    { title: '商品编号', dataIndex: 'F_GoodsNo', resizable: true, width: 150 },
    { title: '商品规格', dataIndex: 'F_Specification', resizable: true, width: 100 },
    { title: '退料数量', dataIndex: 'F_NumInfo', resizable: true, width: 100 },
    { title: '成本单价(元)', dataIndex: 'F_Price', resizable: true, width: 100 },
    { title: '入库仓库', dataIndex: 'F_WarehouseId', resizable: true, width: 240 },
    // { title: '单位', dataIndex: 'F_UnitTwo', resizable: true, width: 100 },
    { title: '备注', dataIndex: 'F_Description', resizable: true },
  ];

  const columns = computed(() => allColumns.filter(col => typeof col.dataIndex === 'string' && hasFormP(('Detail_Goods-' + col.dataIndex) as string)));
  const [registerTable, { reload }] = useTable({
    api: getGoodsList,
    beforeFetch: data => {
      data.F_ReturnInPRDId = state.dataForm.id;
      return data;
    },
    columns,
    useSearchForm: false,
    showTableSetting: false,
    showIndexColumn: false,
  });

  const allColumnsLog: BasicColumn[] = [
    { title: '时间', dataIndex: 'F_CreatorTime', resizable: true, width: 150 },
    { title: '人员', dataIndex: 'F_CreatorUserId', resizable: true, width: 150 },
    { title: '标题', dataIndex: 'F_Title', resizable: true, width: 150 },
    { title: '内容', dataIndex: 'F_Content', resizable: true },
  ];

  const columnsLog = computed(() =>
    allColumnsLog.filter(col => typeof col.dataIndex === 'string' && hasFormP(('tableFieldceb7bd-' + col.dataIndex) as string)),
  );
  const [registerLogTable, { reload: reloadLog }] = useTable({
    api: getLogList,
    beforeFetch: data => {
      data.F_ReturnInPRDId = state.dataForm.id;
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
