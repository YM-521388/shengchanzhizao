<template>
  <BasicModal v-bind="$attrs" @register="registerModal" :title="title" width="1100px" :minHeight="100" :showOkBtn="false" :canFullscreen="true">
    <template #insertFooter> </template>
    <a-row class="dynamic-form">
      <a-form :colon="false" layout="horizontal" labelAlign="right" :labelCol="{ style: { width: '100px' } }" :model="dataForm" ref="formRef">
        <a-row :gutter="15">
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('BGJL_ProdUserId')">
            <a-form-item name="F_ProdUserId" :labelCol="{ style: { width: '100px' } }">
              <template #label>生产人员</template>
              <p>{{ dataForm.F_ProdUserId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('BGJL_GoodQty')">
            <a-form-item name="F_GoodQty" :labelCol="{ style: { width: '100px' } }">
              <template #label>良品数</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_GoodQty"
                placeholder="请输入"
                :controls="false"
                :style="{
                  width: '100%',
                }"
                disabled
                detailed />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('BGJL_GoodNotQty')">
            <a-form-item name="F_GoodNotQty" :labelCol="{ style: { width: '100px' } }">
              <template #label>不良品数</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_GoodNotQty"
                placeholder="请输入"
                :controls="false"
                :style="{
                  width: '100%',
                }"
                disabled
                detailed />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('BGJL_StartTime')">
            <a-form-item name="F_StartTime" :labelCol="{ style: { width: '100px' } }">
              <template #label>开始时间</template>
              <p>{{ dataForm.F_StartTime }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('BGJL_EndTime')">
            <a-form-item name="F_EndTime" :labelCol="{ style: { width: '100px' } }">
              <template #label>结束时间</template>
              <p>{{ dataForm.F_EndTime }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('BGJL_WagePrice')">
            <a-form-item name="F_WagePrice" :labelCol="{ style: { width: '100px' } }">
              <template #label>工资单价</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_WagePrice"
                placeholder="请输入"
                :controls="false"
                :style="{
                  width: '100%',
                }"
                disabled
                detailed />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('BGJL_Files')">
            <a-form-item name="F_Files" :labelCol="{ style: { width: '100px' } }">
              <template #label>附件</template>
              <JnpfUploadFile
                v-model:value="dataForm.F_Files"
                buttonText="点击上传"
                pathType="selfPath"
                :isAccount="-1"
                folder=""
                :fileSize="10"
                sizeUnit="MB"
                :limit="9"
                disabled
                detailed />
            </a-form-item>
          </a-col>
          <a-col :span="16" class="ant-col-item" v-if="hasFormP('BGJL_Description')">
            <a-form-item name="F_Description" :labelCol="{ style: { width: '100px' } }">
              <template #label>备注</template>
              <p>{{ dataForm.F_Description }}</p>
            </a-form-item>
          </a-col>

          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle :content="t('不良品项信息')" :bordered="true" />
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
  import { getDetail, getDefectList } from './helper/api';
  import { getConfigData } from '@/api/onlineDev/visualDev';
  import { reactive, toRefs, nextTick, ref, computed, unref, toRaw } from 'vue';
  import { buildUUID } from '@/utils/uuid';
  import { useI18n } from '@/hooks/web/useI18n';
  import { BasicModal, useModal } from '@/components/Modal';
  import { getLogList } from '@/views/SubCode/aProdReportLog/helper/api';
  import { BasicTable, useTable, TableAction, BasicColumn, FormProps, ActionItem } from '@/components/Table';
  import { usePermission } from '@/hooks/web/usePermission';
  let tableReload: Function = () => {};
  let tableReloadLog: Function = () => {};

  interface State {
    dataForm: any;
    interfaceRes: any;
    extraOptions: any;
    extraData: any;
    title: string;
    tableField579169outerActiveKey: number[];
    tableField579169innerActiveKey: string[];
  }

  defineOptions({ name: 'Detail' });
  const { hasFormP } = usePermission();
  const [registerModal, { openModal, closeModal, setModalProps }] = useModal();
  const { t } = useI18n();
  const state = reactive<State>({
    dataForm: {},
    interfaceRes: {},
    extraOptions: {},
    extraData: {},
    title: '详情',
    tableField579169outerActiveKey: [0],
    tableField579169innerActiveKey: [],
  });
  const { title, dataForm } = toRefs(state);

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
    { title: '不良品项', dataIndex: 'F_DefectId', resizable: true },
    { title: '数量', dataIndex: 'F_Num', resizable: true },
  ];

  const columns = computed(() => allColumns.filter(col => typeof col.dataIndex === 'string' && hasFormP(('tableField579169-' + col.dataIndex) as string)));
  const [registerTable, { reload }] = useTable({
    api: getDefectList,
    beforeFetch: data => {
      data.F_ReportId = state.dataForm.id;
      return data;
    },
    columns: columns,
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

  const columnsLog = computed(() => allColumnsLog.filter(col => typeof col.dataIndex === 'string' && hasFormP(('BGJL_LOG-' + col.dataIndex) as string)));
  const [registerLogTable, { reload: reloadLog }] = useTable({
    api: getLogList,
    beforeFetch: data => {
      data.F_ReportId = state.dataForm.id;
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
