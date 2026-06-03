<template>
  <BasicModal v-bind="$attrs" @register="registerModal" :title="title" width="1200px" :minHeight="100" :showOkBtn="false">
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

          <a-col :span="24" class="ant-col-item" v-if="hasFormP('gridtable_process')">
            <a-form-item label="">
              <JnpfGroupTitle content="生产工序" :bordered="false" />
              <a-table
                :data-source="dataForm.ProcessList"
                :columns="gridtable_process"
                size="small"
                :pagination="false"
                :scroll="{ x: 'max-content' }"
                @resizeColumn="handleResizeColumn">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                  <template v-if="column.key === 'F_ProcessName'">
                    <p>{{ record.F_ProcessName }}</p>
                  </template>
                  <template v-if="column.key === 'F_ProcessCode'">
                    <p>{{ record.F_ProcessCode }}</p>
                  </template>
                  <template v-if="column.key === 'F_WorkshopId'">
                    <p>{{ record.F_WorkshopId }}</p>
                  </template>
                  <template v-if="column.key === 'F_Line'">
                    <p>{{ record.F_Line }}</p>
                  </template>
                  <template v-if="column.key === 'F_WagePrice'">
                    <p>{{ record.F_WagePrice }}</p>
                  </template>
                  <template v-if="column.key === 'F_MachineId'">
                    <p>{{ record.F_MachineId }}</p>
                  </template>
                  <template v-if="column.key === 'F_ProdUserIds'">
                    <p>{{ record.F_ProdUserIds }}</p>
                  </template>
                  <template v-if="column.key === 'F_QcUserIds'">
                    <p>{{ record.F_QcUserIds }}</p>
                  </template>
                  <template v-if="column.key === 'F_DefectIds'">
                    <p>{{ record.F_DefectIds }}</p>
                  </template>
                  <template v-if="column.key === 'F_ReportUnit'">
                    <p>{{ record.F_ReportUnit }}</p>
                  </template>
                  <template v-if="column.key === 'F_UnitRatio'">
                    <p>{{ record.F_UnitRatio }}</p>
                  </template>
                  <template v-if="column.key === 'F_StdHour'">
                    <p>{{ record.F_StdHour }}</p>
                  </template>
                  <template v-if="column.key === 'F_IsQc'">
                    <p>{{ record.F_IsQc }}</p>
                  </template>
                  <template v-if="column.key === 'F_DefectHandle'">
                    <p>{{ record.F_DefectHandle }}</p>
                  </template>
                  <template v-if="column.key === 'F_IsOutsource'">
                    <p>{{ record.F_IsOutsource }}</p>
                  </template>
                  <template v-if="column.key === 'F_Description'">
                    <p>{{ record.F_Description }}</p>
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
      state.dataForm.ProcessList[0].F_UnitRatio = state.dataForm.ProcessList[0].F_UnitRatio + state.dataForm.ProcessList[0].F_ReportUnit;
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

  const gridtable_process: any[] = computed(() => {
    const list = [
      {
        title: '工序',
        dataIndex: 'F_ProcessName',
        key: 'F_ProcessName',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
      },
      {
        title: '工序编号',
        dataIndex: 'F_ProcessCode',
        key: 'F_ProcessCode',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
      },

      {
        title: '车间',
        dataIndex: 'F_WorkshopId',
        key: 'F_WorkshopId',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
      },
      {
        title: '产线',
        dataIndex: 'F_Line',
        key: 'F_Line',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
      },
      {
        title: '计价方式',
        dataIndex: 'F_PriceType',
        key: 'F_PriceType',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
      },
      {
        title: '工资单价',
        dataIndex: 'F_WagePrice',
        key: 'F_WagePrice',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
      },
      {
        title: '机台',
        dataIndex: 'F_MachineId',
        key: 'F_MachineId',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
      },
      {
        title: '生产人员',
        dataIndex: 'F_ProdUserIds',
        key: 'F_ProdUserIds',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
      },
      {
        title: '质检人员',
        dataIndex: 'F_QcUserIds',
        key: 'F_QcUserIds',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
      },
      {
        title: '不良品项',
        dataIndex: 'F_DefectIds',
        key: 'F_DefectIds',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
      },
      {
        title: '报工单位',
        dataIndex: 'F_ReportUnit',
        key: 'F_ReportUnit',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
      },
      {
        title: '单位关系',
        dataIndex: 'F_UnitRatio',
        key: 'F_UnitRatio',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
      },
      {
        title: '标准工时',
        dataIndex: 'F_StdHour',
        key: 'F_StdHour',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
      },
      {
        title: '需质检',
        dataIndex: 'F_IsQc',
        key: 'F_IsQc',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
      },
      {
        title: '需返工 / 报废',
        dataIndex: 'F_DefectHandle',
        key: 'F_DefectHandle',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
      },
      {
        title: '需外协',
        dataIndex: 'F_IsOutsource',
        key: 'F_IsOutsource',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
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
        fixed: false,
      },
    ];
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

  function handleResizeColumn(w, col) {
    col.width = w;
  }
</script>
