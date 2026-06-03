<template>
  <a-alert message="请点击确认按钮，保存工序信息。" type="warning" show-icon style="margin-bottom: 20px;" />
  <a-form :colon="false" layout="horizontal" labelAlign="right" :labelCol="{ style: { width: '100px' } }"
    :model="dataForm" :rules="dataRule" ref="formRef">
    <a-row :gutter="15">
      <a-col :span="24" class="ant-col-item">
        <a-form-item name="F_RoutingNo" :labelCol="{ style: { width: '100px' } }">
          <template #label>工艺路线编号</template>
          <JnpfInput v-model:value="dataForm.F_RoutingNo" placeholder="请填写，忽略将自动生产" allowClear :style="{
            width: '100%',
          }" :showCount="false" disabled />
        </a-form-item>
      </a-col>
      <a-col :span="24" class="ant-col-item">
        <a-form-item name="F_RoutingName" :labelCol="{ style: { width: '100px' } }">
          <template #label>工艺路线名称</template>
          <JnpfInput v-model:value="dataForm.F_RoutingName" placeholder="请输入" allowClear :style="{
            width: '100%',
          }" :showCount="false" disabled />
        </a-form-item>
      </a-col>
      <a-col :span="24" class="ant-col-item">
        <a-form-item name="F_ResponsibleUserId" :labelCol="{ style: { width: '100px' } }">
          <template #label>负责人</template>
          <JnpfUserSelect v-model:value="dataForm.F_ResponsibleUserId" placeholder="请选择" selectType="all" allowClear
            :style="{
              width: '100%',
            }" @change="onApproversChange" @labelChange="onLabelChange" />
        </a-form-item>
      </a-col>

      <a-col :span="24" class="ant-col-item">
        <a-form-item name="F_ProcessId" :labelCol="{ style: { width: '100px' } }">
          <template #label>工序</template>
          <JnpfPopupSelect v-model:value="dataForm.F_ProcessId" placeholder=""
            :templateJson="optionsObj.Process_F_ProcessIdTemplateJson" allowClear field="GoodsList"
            interfaceId="2012092092830060544" :columnOptions="optionsObj.Process_F_ProcessIdOptions"
            relationField="F_ProcessName" propsValue="id" :pageSize="20" popupType="dialog" popupTitle="选择数据"
            popupWidth="1200px" hasPage @change="GoodsListBtn" :style="{
              width: '100%',
            }" :extraOptions="[]" />
        </a-form-item>
      </a-col>
      <a-col :span="24" class="ant-col-item">
        <a-form-item name="F_WagePrice" :labelCol="{ style: { width: '100px' } }">
          <template #label>工资单价</template>
          <JnpfInputNumber v-model:value="dataForm.F_WagePrice" placeholder="请输入" min="0" :precision="2"
            :controls="false" :style="{
              width: '100%',
            }" />
        </a-form-item>
      </a-col>
      <a-col :span="12" class="ant-col-item">
        <a-form-item name="F_StdHour" :labelCol="{ style: { width: '100px' } }">
          <template #label>标准工时</template>
          <JnpfInputNumber v-model:value="dataForm.F_StdHour" placeholder="请输入" addonAfter="小时" min="0"
            :controls="false" :style="{
              width: '100%',
            }" />
        </a-form-item>
      </a-col>
      <a-col :span="6" class="ant-col-item">
        <a-form-item name="F_StdMinute" :labelCol="{ style: { width: '100px' } }">
          <JnpfInputNumber v-model:value="dataForm.F_StdMinute" placeholder="请输入" addonAfter="分钟" min="0" max="59"
            :controls="false" :style="{
              width: '100%',
            }" />
        </a-form-item>
      </a-col>
      <a-col :span="6" class="ant-col-item">
        <a-form-item name="F_StdSecond" :labelCol="{ style: { width: '100px' } }">
          <JnpfInputNumber v-model:value="dataForm.F_StdSecond" placeholder="请输入" addonAfter="秒" min="0" max="59"
            :controls="false" :style="{
              width: '100%',
            }" />
        </a-form-item>
      </a-col>
      <a-col :span="24" class="ant-col-item">
        <a-form-item name="F_PriceType" :labelCol="{ style: { width: '100px' } }">
          <template #label>计价方式</template>
          <JnpfSelect v-model:value="dataForm.F_PriceType" placeholder="请选择" :options="optionsObj.F_PriceTypeOptions"
            :fieldNames="optionsObj.F_PriceTypeProps" allowClear showSearch :style="{
              width: '100%',
            }" />
        </a-form-item>
      </a-col>
      <a-col :span="24" class="ant-col-item">
        <a-form-item name="F_UnitRatio" :labelCol="{ style: { width: '100px' } }">
          <template #label>单位关系</template>
          <div style="display: flex; align-items: center; gap: 4px">
            <span>生产</span>
            <span>1个计划数，需要</span>
            <span style="display: inline-block; width: 20%">
              <JnpfInputNumber v-model:value="dataForm.F_UnitRatio" placeholder="" :controls="false" :style="{
                width: '100%',
              }" />
            </span>
            <span style="display: inline-block; width: 20%">
              <JnpfSelect v-model:value="dataForm.F_ReportUnit" placeholder=""
                :options="optionsObj.F_UnitRatioBaseOptions" :fieldNames="optionsObj.F_UnitRatioBaseProps" allowClear
                :style="{
                  width: '100%',
                }" />
            </span>
          </div>
        </a-form-item>
      </a-col>
      <a-col :span="24" class="ant-col-item">
        <a-form-item name="F_IsOutsource" :labelCol="{ style: { width: '100px' } }">
          <template #label>工序需外协</template>
          <JnpfSwitch @change="IsOutsourceBtn" v-model:value="dataForm.F_IsOutsource" />
        </a-form-item>
      </a-col>
      <a-col :span="24" class="ant-col-item">
        <a-form-item name="F_IsQc" :labelCol="{ style: { width: '100px' } }">
          <template #label>工序需质检</template>
          <JnpfSwitch @change="IsQcBtn" v-model:value="dataForm.F_IsQc" />
        </a-form-item>
      </a-col>
      <a-col :span="24" class="ant-col-item" v-if="state.dataForm.F_IsQc == 1">
        <a-form-item name="F_DefectHandle" :labelCol="{ style: { width: '140px' } }">
          <template #label>不良品需报废、返工</template>
          <JnpfSwitch v-model:value="dataForm.F_DefectHandle" />
        </a-form-item>
      </a-col>
      <a-col :span="24" class="ant-col-item">
        <a-form-item name="F_Files" :labelCol="{ style: { width: '100px' } }">
          <template #label>附件</template>
          <JnpfUploadFile v-model:value="dataForm.F_Files" buttonText="点击上传" pathType="selfPath" :isAccount="-1"
            folder="" :fileSize="10" sizeUnit="MB" :limit="3" :sortRule="['1', '2']" timeFormat="YYYYMMDD" />
        </a-form-item>
      </a-col>
      <a-col :span="24" class="ant-col-item">
        <a-form-item name="F_Description" :labelCol="{ style: { width: '100px' } }">
          <template #label>备注</template>
          <JnpfTextarea v-model:value="dataForm.F_Description" placeholder="请输入" :maxlength="500" allowClear :autoSize="{
            minRows: 3,
            maxRows: 3,
          }" :style="{
            width: '100%',
          }" :showCount="false" />
        </a-form-item>
      </a-col>
      <a-col :span="24" class="ant-col-item">
        <a-form-item :labelCol="{ style: { width: '100px' } }">
          <template #label>BOM</template>
          <JnpfSelect v-model:value="dataForm.F_BOMId" placeholder="" :options="optionsObj.F_BOMIdOptions"
            :fieldNames="optionsObj.F_BOMIdProps" allowClear showSearch @change="F_BomChange" :style="{
              width: '100%',
            }" />
        </a-form-item>
      </a-col>
      <a-col :span="24" class="ant-col-item">
        <a-form-item label="">
          <a-table :data-source="dataForm.tableField3b6f08" :columns="tableField3b6f08Columns" size="small"
            rowKey="jnpfId" :pagination="false" :scroll="{ x: 'max-content' }"
            :rowSelection="gettableField3b6f08RowSelection">
            <template #headerCell="{ column }">
              <span class="required-sign" v-if="column.required">*</span>{{ column.title }}
              <BasicHelp :text="column.tipLabel" v-if="column.tipLabel" />
            </template>
            <template #bodyCell="{ column, record, index }">
              <template v-if="column.key === 'F_GoodsId'">
                <JnpfSelect v-model:value="record.F_GoodsId" :options="optionsObj.tableField3b6f08_F_GoodsIdOptions"
                  :fieldNames="optionsObj.tableField3b6f08_F_GoodsIdProps" allowClear showSearch disabled :style="{
                    width: '100%',
                  }" />

              </template>
              <template v-if="column.key === 'F_Num'">
                <JnpfInputNumber v-model:value="record.F_Num" placeholder="请输入" :min="1" :controls="false" :style="{
                  width: '100%',
                }" />
              </template>
              <template v-if="column.key === 'F_UnitTwo'">
                <JnpfInput v-model:value="record.F_UnitTwo" placeholder="" :controls="false" disabled :style="{
                  width: '100%',
                }" />
              </template>
              <template v-if="column.key === 'F_StockInProcess'">
                <JnpfSelect v-model:value="record.F_StockInProcess" placeholder="请选择"
                  :options="optionsObj.tableField3b6f08_F_StockInProcessOptions"
                  :fieldNames="optionsObj.tableField3b6f08_F_StockInProcessProps" allowClear showSearch :style="{
                    width: '100%',
                  }" />
              </template>
              <template v-if="column.key === 'F_Description'">
                <JnpfInput v-model:value="record.F_Description" placeholder="请输入" allowClear :style="{
                  width: '100%',
                }" :showCount="false" />
              </template>
              <template v-if="column.key === 'F_CreatorTime'">
                <JnpfOpenData v-model:value="record.F_CreatorTime" type="currTime" readonly :style="{
                  width: '100%',
                }" />
              </template>
              <template v-if="column.key === 'action'">
                <a-space>
                  <a-button class="action-btn" type="link" color="error" @click="removetableField3b6f08Row(index, true)"
                    size="small">{{
                      t('common.delText', '删除')
                    }}</a-button>
                </a-space>
              </template>
            </template>
          </a-table>
        </a-form-item>
      </a-col>

    </a-row>
  </a-form>
</template>
<script lang="ts" setup>
import { create, update, getInfo } from '@/views/Subcode/aProdRouting/helper/api';
import { watch, reactive, toRefs, nextTick, ref, unref, computed, toRaw, inject, onMounted, provide } from 'vue';
import { useMessage } from '@/hooks/web/useMessage';
import { useI18n } from '@/hooks/web/useI18n';
import type { FormInstance } from 'ant-design-vue';
import { cloneDeep } from 'lodash-es';
import { buildUUID } from '@/utils/uuid';
import { getDictionaryDataSelector } from '@/api/systemData/dictionary';
import { getInfo as getWorkInfo } from '@/views/Subcode/aProdProcess/helper/api';
import { typeConfig } from '../../bpmn/config';
import { bpmnTask } from '../../bpmn/config/variableName/index';
import { getDataInterfaceRes } from '@/api/systemData/dataInterface';
import { getBOMGoodsList } from '@/views/Subcode/aBom/helper/api';
import { usePermission } from '@/hooks/web/usePermission';

import {
  typeOptions,
} from '../../helper/define';
defineOptions({ inheritAttrs: false });
const props = defineProps({
  value: { default: '' },
  title: { type: String, default: '' },
  placeholder: { type: String, default: '请选择' },
  disabled: { type: Boolean, default: false },
  allowClear: { type: Boolean, default: true },
  size: { type: String, default: 'default' },
  isStartForm: { type: Number, default: 0 },
  type: { type: String, default: '' },
  bpmn: { type: Object, default: null },
  formConf: { type: Object, default: null },
  updateBpmnProperties: { type: Function, default: () => { } },
});
const emit = defineEmits(['update:value', 'change']);

interface State {
  dataForm: any;
  dataRule: any;
  optionsObj: any;
  tempGoodsSelector?: any[];
  userLabel: string;
  selectedtableField3b6f08RowKeys: any[];
}
const { createMessage, createConfirm } = useMessage();
const { t } = useI18n();
const { hasFormP } = usePermission();

const formRef = ref<FormInstance>();
const state = reactive<State>({
  dataForm: {
    F_RoutingNo: undefined,
    F_RoutingName: undefined,
    F_CreatorUserId: undefined,
    F_CreatorTime: undefined,
    F_ResponsibleUserId: undefined,
    F_ProcessId: undefined,
    F_WagePrice: undefined,
    F_StdHour: undefined,
    F_StdMinute: undefined,
    F_StdSecond: undefined,
    F_PriceType: undefined,
    F_UnitRatio: undefined,
    F_ReportUnit: undefined,
    F_Files: undefined,
    F_Description: undefined,
    F_BOMId: undefined,
    F_IsOutsource: 0,
    F_IsQc: 0,
    F_DefectHandle: 0,
    tableField3b6f08: [] as any[],
  },
  dataRule: {
    F_RoutingName: [
      {
        required: true,
        message: '请在开始流程中输入工艺路线名称',
        trigger: 'blur',
      },
    ],
    F_ResponsibleUserId: [
      {
        required: true,
        message: '请选择负责人',
        trigger: 'change',
      },
    ],
    F_ProcessId: [
      {
        required: true,
        message: '请选择工序',
        trigger: 'change',
      },
    ],
  },
  userLabel: '',

  tempGoodsSelector: [],

  optionsObj: {
    F_PriceTypeProps: { label: 'fullName', value: 'enCode' },
    F_UnitRatioBaseProps: { label: 'fullName', value: 'enCode' },
    tableField3b6f08_F_GoodsIdOptions: [],
    tableField3b6f08_F_GoodsIdProps: { label: 'F_GoodsName', value: 'id' },
    F_UnitRatioBaseOptions: [],
    Process_F_ProcessIdTemplateJson: [],
    Process_F_ProcessIdOptions: [
      {
        value: 'F_ProcessName',
        label: '工序',
      },
      {
        value: 'F_ProcessCode',
        label: '工序编号',
      },
      {
        value: 'F_ProdUserIds',
        label: '生产人员',
      },
      {
        value: 'F_QcUserIds',
        label: '质检人员',
      },
      {
        value: 'F_DefectIds',
        label: '不良品项',
      },
      {
        value: 'F_TaskTimer',
        label: '生产任务计时',
      },
      {
        value: 'F_DefectHandleName',
        label: '需返工 / 报废',
      },
      {
        value: 'F_Description',
        label: '备注',
      },
    ],
    F_BOMIdOptions: [],
    F_BOMIdProps: { label: 'F_BomName', value: 'id' },
  },

  selectedtableField3b6f08RowKeys: [],
});

const tableField3b6f08Columns: any[] = computed(() => {
  let list = [
    {
      title: '商品',
      dataIndex: 'F_GoodsId',
      key: 'F_GoodsId',
      tipLabel: '',
      required: false,
      align: 'left',
      span: '24',
      labelWidth: '',
      width: '180px',
      fixed: false,
      formP: 'F_GoodsId',
      isSystemControl: false,
    },
    {
      title: '数量',
      dataIndex: 'F_Num',
      key: 'F_Num',
      tipLabel: '',
      required: false,
      align: 'left',
      span: '24',
      labelWidth: '',
      width: '100px',
      fixed: false,
      formP: 'F_Num',
      isSystemControl: false,
    },
    // {
    //   title: '单位',
    //   dataIndex: 'F_UnitTwo',
    //   key: 'F_UnitTwo',
    //   tipLabel: '',
    //   required: false,
    //   align: 'left',
    //   span: '24',
    //   labelWidth: '',
    //   width: '100px',
    //   fixed: false,
    //   formP: 'F_UnitTwo',
    //   isSystemControl: false,
    // },
    {
      title: '备注',
      dataIndex: 'F_Description',
      key: 'F_Description',
      tipLabel: '',
      required: false,
      align: 'left',
      span: '24',
      labelWidth: '',
      width: '',
      fixed: false,
      formP: 'F_Description',
      isSystemControl: false,
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
  const actionColumn = { title: t('component.table.action'), dataIndex: 'action', key: 'action', align: 'center', width: 100, fixed: 'right' };
  let columns = [...columnList, actionColumn];
  const leftFixedList = columns.filter(o => o.fixed === 'left');
  const rightFixedList = columns.filter(o => o.fixed === 'right');
  const noFixedList = columns.filter(o => o.fixed !== 'left' && o.fixed !== 'right');
  return [...leftFixedList, ...noFixedList, ...rightFixedList];
});
const gettableField3b6f08RowSelection = computed(() => {
  const rowSelection = {
    selectedRowKeys: state.selectedtableField3b6f08RowKeys,
    onChange: (selectedRowKeys: string[]) => {
      state.selectedtableField3b6f08RowKeys = (selectedRowKeys || []).sort().reverse();
    },
  };
  return rowSelection;
});
const { dataForm, dataRule, optionsObj, tempGoodsSelector } = toRefs(state);

watch(
  () => props.formConf,
  (val, oldVal) => {
    nextTick(() => {
      getForm().resetFields();
      if (val?.nodeId !== oldVal?.nodeId) {
        resetDataForm();
      }
      if (val?.formData && typeof val.formData === 'object') {
        assignFromSource(val.formData);
        getAProdRoutingFromData();
      }
      // 同步 parentId 到 dataForm（从 formConf 获取，优先于 formData）
      if (val?.parentId) {
        state.dataForm.parentId = val.parentId;
      }
    });
  },
  { immediate: true },
);
defineExpose({ handleSubmit });

onMounted(() => {
  getF_BOMIdOptions();

  setTimeout(initData, 0);
});
function initData() {
  getF_UnitRatioBaseOptions();
  getF_PriceTypeOptions();
  getAProdRoutingFromData();
  gettableField449e6c_F_GoodsIdOptions();

}

// 重置 state.dataForm 为初始值
function resetDataForm() {
  state.dataForm = {
    F_RoutingNo: undefined,
    F_RoutingName: undefined,
    F_CreatorUserId: undefined,
    F_CreatorTime: undefined,
    F_ResponsibleUserId: undefined,
    F_ProcessId: undefined,
    F_WagePrice: undefined,
    F_StdHour: undefined,
    F_StdMinute: undefined,
    F_StdSecond: undefined,
    F_PriceType: undefined,
    F_UnitRatio: undefined,
    F_ReportUnit: undefined,
    F_Files: undefined,
    F_Description: undefined,
    F_BOMId: undefined,
    F_IsOutsource: 0,
    F_IsQc: 0,
    F_DefectHandle: 0,
    tableField3b6f08: [] as any[],
  };
}
function getF_UnitRatioBaseOptions() {
  getDictionaryDataSelector('2008448689420505088').then(res => {
    state.optionsObj.F_UnitRatioBaseOptions = res.data.list;
  });
}
function getF_PriceTypeOptions() {
  getDictionaryDataSelector('2011642610875240448').then(res => {
    state.optionsObj.F_PriceTypeOptions = res.data.list;
  });
}
function getF_BOMIdOptions() {
  let templateJson = [
    {
      defaultValue: '',
      field: 'goodsId',
      dataType: 'varchar',
      required: 0,
      fieldName: '合同ID',
      relationField: 'F_GoodsId',
      jnpfKey: 'popupSelect',
      complexHeaderList: null,
      sourceType: 1,
      isChildren: false,
      IsMultiple: false,
    },
  ];
  let query = {
    paramList: getParamList(templateJson, state.dataForm),
  };
  getDataInterfaceRes('2013188646957617152', query).then(res => {
    let data = res.data;
    state.optionsObj.F_BOMIdOptions = Array.isArray(data) ? data : [];
  });
}
function gettableField449e6c_F_GoodsIdOptions() {
  let templateJson = [
    {
      defaultValue: '',
      field: 'contractId',
      dataType: 'varchar',
      required: 0,
      fieldName: '合同ID',
      relationField: '',
      jnpfKey: null,
      complexHeaderList: null,
      sourceType: 3,
      isChildren: false,
      IsMultiple: false,
    },
  ];
  let query = {
    paramList: getParamList(templateJson, state.dataForm),
  };
  getDataInterfaceRes('2012085692393459712', query).then(res => {
    let data = res.data;
    state.optionsObj.tableField3b6f08_F_GoodsIdOptions = Array.isArray(data) ? data : [];
  });
}
function getParamList(templateJson, formData, index?) {
  for (let i = 0; i < templateJson.length; i++) {
    if (templateJson[i].relationField && templateJson[i].sourceType == 1) {
      //区分是否子表
      if (templateJson[i].relationField.includes('-')) {
        let tableVModel = templateJson[i].relationField.split('-')[0];
        let childVModel = templateJson[i].relationField.split('-')[1];
        templateJson[i].defaultValue = (formData[tableVModel] && formData[tableVModel][index] && formData[tableVModel][index][childVModel]) || '';
      } else {
        templateJson[i].defaultValue = formData[templateJson[i].relationField] || '';
      }
    }
  }
  return templateJson;
}
function getForm() {
  const form = unref(formRef);
  if (!form) {
    throw new Error('form is null!');
  }
  return form;
}

// 辅助函数：从源对象中提取 state.dataForm 中定义的键赋值
function assignFromSource(source: any) {
  if (!source || typeof source !== 'object') return;
  Object.keys(state.dataForm).forEach(key => {
    if (source[key] !== undefined) {
      state.dataForm[key] = source[key];
    }
  });
  state.dataForm.F_IsQc = Number(state.dataForm.F_IsQc)
  state.dataForm.F_IsOutsource = Number(state.dataForm.F_IsOutsource)
  state.dataForm.F_DefectHandle = Number(state.dataForm.F_DefectHandle)

}
async function handleSubmit() {
  try {
    const values = await getForm()?.validate();
    if (!values) return;

    // 获取当前节点的 nodeId
    const currentNodeId = props.formConf?.nodeId;
    if (!currentNodeId) return;
    // 同步到 props.formConf
    props.formConf.F_RoutingNo = state.dataForm.F_RoutingNo;
    props.formConf.F_RoutingName = state.dataForm.F_RoutingName;
    // 确保 parentId 被保存到 formData
    if (props.formConf?.parentId) {
      state.dataForm.parentId = props.formConf.parentId;
    }
    props.formConf.formData = { ...state.dataForm };
    createMessage.success('确认成功！');
    console.log( props.formConf,' props.formConf')

  } catch (err) {
    console.error('handleSubmit error:', err);
  }
}

// 工序改变
function GoodsListBtn(val, option) {
  // 表单的标题
  props.formConf.nodeName = option.F_ProcessName ?? props.formConf.nodeName;
  // 更新流程节点的标题
  props.updateBpmnProperties('nodeName', props.formConf.nodeName)
  if (!val) return;
  getWorkInfoData(val);
}

function getAProdRoutingFromData() {
  const fromData = localStorage.getItem('getAProdRoutingFromData');
  if (!fromData) return;

  const allNodesData = JSON.parse(fromData);
  if (allNodesData.formData.F_RoutingNo !== undefined) {
    state.dataForm.F_RoutingNo = allNodesData.formData.F_RoutingNo;
  }
  if (allNodesData.formData.F_RoutingName !== undefined) {
    state.dataForm.F_RoutingName = allNodesData.formData.F_RoutingName;
  }
}

// 工序改变
function getWorkInfoData(val) {
  getWorkInfo(val).then(res => {
    let data = res.data || {};
    // 将 getWorkInfo 返回的数据合并到 state.dataForm
    Object.assign(state.dataForm, data);
  });
}

function removetableField3b6f08Row(index, showConfirm = false) {
  if (showConfirm) {
    createConfirm({
      iconType: 'warning',
      title: '提示',
      content: '此操作将永久删除该数据, 是否继续?',
      onOk: () => {
        state.dataForm.tableField3b6f08.splice(index, 1);
      },
    });
  } else {
    state.dataForm.tableField3b6f08.splice(index, 1);
  }
}

function onApproversChange(val) {
  // 添加空值检查：确保 formConf 存在
  if (!props.formConf) return;
  props.formConf.approvers = val;
  props.formConf.approversSortList = val
  return
  if (props.formConf.assigneeType != 6 || !val) return (props.formConf.approversSortList = []);
  if (!props.formConf.approversSortList?.length) return (props.formConf.approversSortList = val);
  const arr = props.formConf.approversSortList.filter(o => val.indexOf(o) !== -1);
  const updated = val.filter(o => props.formConf.approversSortList.indexOf(o) === -1);
  props.formConf.approversSortList = [...arr, ...updated];
}
function onLabelChange(val) {
  state.userLabel = val;
  // 添加空值检查：确保 formConf 存在
  if (!props.formConf) return;

  // 更新流程节点的负责人
  props.updateBpmnProperties('elementBodyName', getContent());
}
function getContent() {

  // 添加空值检查：确保 formConf 存在
  if (!props.formConf) return typeConfig[bpmnTask].renderer.bodyDefaultText;
  let content = typeConfig[bpmnTask].renderer.bodyDefaultText;
  if (props.formConf.assigneeType != 6) {
    content = typeOptions.find(o => o.id === props.formConf.assigneeType)?.fullName || typeConfig[bpmnTask].renderer.bodyDefaultText;
  } else {
    content = state.userLabel || typeConfig[bpmnTask].renderer.bodyDefaultText;
  }
  props.formConf.content = content;
  return content;
}
function F_BomChange(val, option) {
  getBOMGoodsList(val).then(res => {
    state.dataForm.tableField3b6f08 = [];

    let nextSort = 1;
    res.data.forEach(o => {
      const newRow = {
        jnpfId: buildUUID(),
        F_GoodsId: o.F_GoodsId,
        F_UnitTwo: o.F_UnitTwo,
        F_Num: o.F_Unit,
        F_Description: undefined,
        F_SortCode: nextSort, // ← 自动累加
      };
      state.dataForm.tableField3b6f08.push(newRow);
      nextSort++; // 下一行继续 +1
      // state.GoodsList.push(o.F_GoodsId);
    });
  });
}
function IsOutsourceBtn(val) {
  if (val == 1) {
    state.dataForm.F_IsQc = 0;
  }
}
function IsQcBtn(val) {
  if (val == 1) {
    state.dataForm.F_IsOutsource = 0;
  }
}
</script>
<style lang="less" scoped>
/deep/.ant-form-item {
  margin-bottom: 20px !important;
}
</style>
