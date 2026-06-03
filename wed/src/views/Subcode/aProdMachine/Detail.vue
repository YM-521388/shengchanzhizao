<template>
  <BasicModal v-bind="$attrs" @register="registerModal" :title="title" width="1200px" :minHeight="100" :showOkBtn="false" :canFullscreen="true">
    <template #insertFooter> </template>
    <a-row class="dynamic-form">
      <a-form :colon="false" layout="horizontal" labelAlign="right" :labelCol="{ style: { width: '100px' } }" :model="dataForm" ref="formRef">
        <a-row :gutter="15">
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_Image')">
            <a-form-item name="F_Image" :labelCol="{ style: { width: '100px' } }">
              <template #label>机台图片：</template>
              <JnpfUploadImg v-model:value="dataForm.F_Image" pathType="selfPath" :isAccount="-1" folder="" sizeUnit="MB" :fileSize="10" disabled detailed />
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_MachineCode')">
            <a-form-item name="F_MachineCode" :labelCol="{ style: { width: '100px' } }">
              <template #label>机台编号：</template>
              <JnpfInput
                v-model:value="dataForm.F_MachineCode"
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
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_MachineName')">
            <a-form-item name="F_MachineName" :labelCol="{ style: { width: '100px' } }">
              <template #label>机台名称：</template>
              <JnpfInput
                v-model:value="dataForm.F_MachineName"
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
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_MachineSpec')">
            <a-form-item name="F_MachineSpec" :labelCol="{ style: { width: '100px' } }">
              <template #label>机台规格：</template>
              <JnpfInput
                v-model:value="dataForm.F_MachineSpec"
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
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_MachineStatus')">
            <a-form-item name="F_MachineStatus" :labelCol="{ style: { width: '100px' } }">
              <template #label>机台状态：</template>
              <p>{{ dataForm.F_MachineStatus }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_MachineType')">
            <a-form-item name="F_MachineType" :labelCol="{ style: { width: '100px' } }">
              <template #label>机台类别：</template>
              <p>{{ dataForm.F_MachineType }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_WorkshopId')">
            <a-form-item name="F_WorkshopId" :labelCol="{ style: { width: '100px' } }">
              <template #label>车间：</template>
              <p>{{ dataForm.F_WorkshopId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_LineId')">
            <a-form-item name="F_LineId" :labelCol="{ style: { width: '100px' } }">
              <template #label>产线：</template>
              <p>{{ dataForm.F_LineId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_DailyRunHours')">
            <a-form-item name="F_DailyRunHours" :labelCol="{ style: { width: '100px' } }">
              <template #label>单日运行时长：</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_DailyRunHours"
                placeholder="请输入"
                addonAfter="小时/天"
                :controls="false"
                :style="{
                  width: '100%',
                }"
                disabled
                detailed />
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_State')">
            <a-form-item name="F_State" :labelCol="{ style: { width: '100px' } }">
              <template #label>启用状态：</template>
              <p>{{ dataForm.F_State }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_StdHour')">
            <a-form-item name="F_StdHour" :labelCol="{ style: { width: '100px' } }">
              <template #label>标准工时：</template>
              <p>{{ (dataForm.F_StdHour ?? 0) + '小时' + (dataForm.F_StdMinute ?? 0) + '分钟' + (dataForm.F_StdSecond ?? 0) + '秒' }}</p>
            </a-form-item>
          </a-col>

          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_StdOutput')">
            <a-form-item name="F_StdOutput" :labelCol="{ style: { width: '100px' } }">
              <template #label>标准产出：</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_StdOutput"
                placeholder="请输入"
                :controls="false"
                :style="{
                  width: '100%',
                }"
                disabled
                detailed />
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item">
            <a-form-item name="F_StdOutput" :labelCol="{ style: { width: '100px' } }">
              <template #label>标准效率<BasicHelp text="标准效率是单位时间内按照标准操作流程生产出的产品数量。标准效率 = 标准产出 / 标准工时。" /> </template>

              <p>{{
                (dataForm.F_StdOutput ?? 0) +
                '/' +
                (dataForm.F_StdHour ?? 0) +
                '小时' +
                (dataForm.F_StdMinute ?? 0) +
                '分钟' +
                (dataForm.F_StdSecond ?? 0) +
                '秒'
              }}</p>
            </a-form-item>
          </a-col>

          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="生产工序" :bordered="true" />
              <BasicTable :scroll="{ y: 400 }" @register="registerTable" @resizeColumn="handleResizeColumn">
                <template #bodyCell="{ column, record }">
                  <template v-if="column.key === 'F_IsOutsource'">
                    <a-tag :color="record.F_IsOutsource == '1' ? 'success' : 'error'">
                      {{ record.F_IsOutsource == '1' ? '是' : '否' }}
                    </a-tag>
                  </template>
                  <template v-if="column.key === 'F_IsQc'">
                    <a-tag :color="record.F_IsQc == '1' ? 'success' : 'error'">
                      {{ record.F_IsQc == '1' ? '是' : '否' }}
                    </a-tag>
                  </template>
                  <template v-if="column.key === 'F_DefectHandle'">
                    <a-tag :color="record.F_DefectHandle == '1' ? 'success' : 'error'">
                      {{ record.F_DefectHandle == '1' ? '是' : '否' }}
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
  import { getProcessList } from '@/views/Subcode/aProdProcess/helper/api';
  import { BasicTable, useTable, TableAction, BasicColumn, FormProps, ActionItem } from '@/components/Table';
  import { getDetail } from './helper/api';
  import { reactive, toRefs, nextTick, ref, computed, unref, toRaw } from 'vue';
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
  }

  defineOptions({ name: 'Detail' });
  const [registerModal, { openModal, closeModal, setModalProps }] = useModal();
  const { t } = useI18n();
  let tableReload: Function = () => {};

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
      // state.dataForm.ProcessList[0].F_UnitRatio = state.dataForm.ProcessList[0].F_UnitRatio + state.dataForm.ProcessList[0].F_ReportUnit;
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
  const allColumns: BasicColumn[] = [
    { title: '工序', dataIndex: 'F_ProcessName', resizable: true },
    { title: '工序编号', dataIndex: 'F_ProcessCode', resizable: true },
    { title: '车间', dataIndex: 'F_WorkshopId', resizable: true },
    { title: '产线', dataIndex: 'F_Line', resizable: true },
    { title: '计价方式', dataIndex: 'F_PriceType', resizable: true },
    { title: '工资单价', dataIndex: 'F_WagePrice', align: 'left', resizable: true },
    { title: '机台', dataIndex: 'F_MachineId', resizable: true, width: 260 },
    { title: '生产人员', dataIndex: 'F_ProdUserIds', align: 'left', resizable: true, width: 260 },
    { title: '质检人员', dataIndex: 'F_QcUserIds', align: 'left', resizable: true, width: 260 },
    { title: '不良品项', dataIndex: 'F_DefectIds', align: 'left', resizable: true, width: 260 },
    { title: '报工单位', dataIndex: 'F_ReportUnit', align: 'left', resizable: true },
    { title: '单位关系', dataIndex: 'F_UnitRatio', align: 'left', resizable: true, width: 180 },
    { title: '标准工时', dataIndex: 'F_StdHour', align: 'left', resizable: true },
    { title: '需质检', dataIndex: 'F_IsQc', align: 'left', resizable: true },
    { title: '需返工 / 报废', dataIndex: 'F_DefectHandle', align: 'left', resizable: true },
    { title: '需外协', dataIndex: 'F_IsOutsource', align: 'left', resizable: true },
    { title: '备注', dataIndex: 'F_Description', align: 'left', resizable: true, width: 260 },
    // { title: t('zapp.fy283', '获取时间'), dataIndex: 'F_UtcTime', width: 150, format: 'date|YYYY-MM-DD HH:mm:ss', resizable: true },
  ];

  const columns = computed(() => allColumns.filter(col => typeof col.dataIndex === 'string' && hasFormP(('gridtable_process-' + col.dataIndex) as string)));
  const [registerTable, { reload, getForm, getFetchParams }] = useTable({
    api: getProcessList,
    beforeFetch: data => {
      data.machineId = state.dataForm.id;
      return data;
    },
    columns,
    useSearchForm: false,
    showTableSetting: false,
  });
  tableReload = reload;

  function handleResizeColumn(w, col) {
    col.width = w;
  }
</script>
