<template>
  <BasicModal v-bind="$attrs" @register="registerModal" :title="title" width="60%" :minHeight="100" :showOkBtn="false" :canFullscreen="true">
    <template #insertFooter> </template>
    <a-row class="dynamic-form">
      <a-form :colon="false" layout="horizontal" labelAlign="right" :labelCol="{ style: { width: '100px' } }" :model="dataForm" ref="formRef">
        <a-row :gutter="15">
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('WXGD_RepairNo')">
            <a-form-item name="F_RepairNo" :labelCol="{ style: { width: '100px' } }">
              <template #label>工单编号：</template>
              <JnpfInput
                v-model:value="dataForm.F_RepairNo"
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
          <!-- <a-col :span="6" class="ant-col-item">
            <a-form-item name="F_GroupId" :labelCol="{ style: { width: '100px' } }">
              <template #label>分组：</template>
              <p>{{ dataForm.F_GroupId }}</p>
            </a-form-item>
          </a-col> -->
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('WXGD_DispatchType')">
            <a-form-item name="F_DispatchType" :labelCol="{ style: { width: '100px' } }">
              <template #label>派单方式：</template>
              <p>{{ dataForm.F_DispatchType }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('WXGD_HandlerUserId')">
            <a-form-item name="F_HandlerUserId" :labelCol="{ style: { width: '100px' } }">
              <template #label>处理人：</template>
              <p>{{ dataForm.F_HandlerUserId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('WXGD_DeviceId')">
            <a-form-item name="F_DeviceId" :labelCol="{ style: { width: '100px' } }">
              <template #label>维修设备：</template>
              <p class="leading-32px">{{ dataForm.F_DeviceId }}</p>
              <ExtraRelationInfo
                :extraOptions="state.extraOptions.F_DeviceId"
                :data="state.extraData.F_DeviceId"
                v-if="state.extraOptions.F_DeviceId?.length && state.extraData.F_DeviceId && JSON.stringify(state.extraData.F_DeviceId) !== '{}'" />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('WXGD_Description')">
            <a-form-item name="F_Description" :labelCol="{ style: { width: '100px' } }">
              <template #label>故障描述：</template>
              <p>{{ dataForm.F_Description }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('WXGD_ReceiveTime')">
            <a-form-item name="F_ReceiveTime" :labelCol="{ style: { width: '100px' } }">
              <template #label>接单时间：</template>
              <p>{{ dataForm.F_ReceiveTime }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('WXGD_HandleTime')">
            <a-form-item name="F_HandleTime" :labelCol="{ style: { width: '100px' } }">
              <template #label>处理时间：</template>
              <p>{{ dataForm.F_HandleTime }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('WXGD_State')">
            <a-form-item name="F_State" :labelCol="{ style: { width: '100px' } }">
              <template #label>状态：</template>
              <p>{{ dataForm.F_State }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('WXGD_AuditorUserId')">
            <a-form-item name="F_AuditorUserId" :labelCol="{ style: { width: '100px' } }">
              <template #label>审核人：</template>
              <p>{{ dataForm.F_AuditorUserId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('WXGD_AuditTime')">
            <a-form-item name="F_AuditTime" :labelCol="{ style: { width: '100px' } }">
              <template #label>审核时间：</template>
              <p>{{ dataForm.F_AuditTime }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('WXGD_IsSolved')">
            <a-form-item name="F_IsSolved" :labelCol="{ style: { width: '100px' } }">
              <template #label>是否解决问题：</template>
              <p>{{ dataForm.F_IsSolved }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('WXGD_CreatorUserId')">
            <a-form-item name="F_CreatorUserId" :labelCol="{ style: { width: '100px' } }">
              <template #label>报修人：</template>
              <p>{{ dataForm.F_CreatorUserId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('WXGD_CreatorTime')">
            <a-form-item name="F_CreatorTime" :labelCol="{ style: { width: '100px' } }">
              <template #label>报修时间：</template>
              <p>{{ dataForm.F_CreatorTime }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('WXGD_PauseReason')">
            <a-form-item name="F_PauseReason" :labelCol="{ style: { width: '100px' } }">
              <template #label>暂停原因：</template>
              <p>{{ dataForm.F_PauseReason }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('WXGD_CancelReason')">
            <a-form-item name="F_CancelReason" :labelCol="{ style: { width: '100px' } }">
              <template #label>取消原因：</template>
              <p>{{ dataForm.F_CancelReason }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('WXGD_AuditReason')">
            <a-form-item name="F_AuditReason" :labelCol="{ style: { width: '100px' } }">
              <template #label>审核备注：</template>
              <p>{{ dataForm.F_AuditReason }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('WXGD_HandleResult')">
            <a-form-item name="F_HandleResult" :labelCol="{ style: { width: '100px' } }">
              <template #label>处理结果：</template>
              <p>{{ dataForm.F_HandleResult }}</p>
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
  import { getDetail } from './helper/api';
  import { getLogList } from '@/views/Subcode/aMaintRepairLog/helper/api';
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
  import { BasicTable, useTable, TableAction, BasicColumn, FormProps, ActionItem } from '@/components/Table';
  import { usePermission } from '@/hooks/web/usePermission';
  let tableReloadLog: Function = () => {};

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

  const relationDetailRef = ref<any>(null);
  const state = reactive<State>({
    dataForm: {},
    interfaceRes: {
      F_DeviceId: [],
    },
    extraOptions: {
      F_DeviceId: [],
    },
    extraData: {
      F_DeviceId: {},
    },
    title: '详情',
  });
  const { title, dataForm } = toRefs(state);

  defineExpose({ init });

  function init(data) {
    state.dataForm.id = data.id;
    openModal();
    /* 手动刷新表格 */
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
        getF_DeviceIdExtraInfo();
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

  function getF_DeviceIdExtraInfo() {
    if (!state.dataForm.F_DeviceId_id) return;
    const paramList = getParamList('F_DeviceId');
    const query = {
      ids: [state.dataForm.F_DeviceId_id],
      interfaceId: '2016476904974061568',
      propsValue: 'id',
      relationField: 'F_DeviceName',
      paramList,
    };
    getDataInterfaceDataInfoByIds('2016476904974061568', query).then(res => {
      const data = res.data && res.data.length ? res.data[0] : {};
      state.extraData.F_DeviceId = data;
    });
  }

  const allColumnsLog: BasicColumn[] = [
    { title: '时间', dataIndex: 'F_CreatorTime', resizable: true, width: 150 },
    { title: '人员', dataIndex: 'F_CreatorUserId', resizable: true, width: 150 },
    { title: '标题', dataIndex: 'F_Title', resizable: true, width: 150 },
    { title: '内容', dataIndex: 'F_Content', resizable: true },
  ];

  const columnsLog = computed(() => allColumnsLog.filter(col => typeof col.dataIndex === 'string' && hasFormP(('Detail_log-' + col.dataIndex) as string)));
  const [registerLogTable, { reload: reloadLog }] = useTable({
    api: getLogList,
    beforeFetch: data => {
      data.F_RepairId = state.dataForm.id;
      return data;
    },
    columns: columnsLog,
    useSearchForm: false,
    showTableSetting: false,
    showIndexColumn: false,
  });

  tableReloadLog = reloadLog;

  function handleResizeColumn(w, col) {
    col.width = w;
  }
</script>
