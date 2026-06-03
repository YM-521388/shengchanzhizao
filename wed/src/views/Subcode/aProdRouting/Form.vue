<template>
  <BasicModal v-bind="$attrs" @register="registerModal" width="1400px" :minHeight="100" okText="确定" cancelText="取消" @ok="handleSubmit" :closeFunc="onClose">
    <template #title>
      <a-space :size="10">
        <div class="text-16px font-medium">{{ title }}</div>
      </a-space>
    </template>
    <template #insertToolbar>
      <div class="float-left mt-5px">
        <JnpfCheckboxSingle v-model:value="submitType" :label="continueText" />
      </div>
    </template>
    <a-row class="dynamic-form">
      <a-form :colon="false" layout="horizontal" labelAlign="right" :labelCol="{ style: { width: '100px' } }" :model="dataForm" :rules="dataRule" ref="formRef">
        <a-row :gutter="15">
          <a-col :span="12" class="ant-col-item">
            <a-form-item name="F_RoutingNo" :labelCol="{ style: { width: '100px' } }">
              <template #label>工艺路线编号</template>
              <JnpfInput
                v-model:value="dataForm.F_RoutingNo"
                placeholder="请填写，忽略将自动生产"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item">
            <a-form-item name="F_RoutingName" :labelCol="{ style: { width: '100px' } }">
              <template #label>工艺路线名称</template>
              <JnpfInput
                v-model:value="dataForm.F_RoutingName"
                placeholder="请输入"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>

          <a-col :span="24" class="ant-col-item">
            <a-form-item name="GoodsList" :labelCol="{ style: { width: '100px' } }">
              <template #label>选择工序</template>
              <JnpfPopupMultipleSelect
                v-model:value="state.GoodsList"
                placeholder=""
                :templateJson="optionsObj.Process_F_ProcessIdTemplateJson"
                allowClear
                field="GoodsList"
                interfaceId="2012092092830060544"
                :columnOptions="optionsObj.Process_F_ProcessIdOptions"
                relationField="F_ProcessName"
                propsValue="id"
                :pageSize="20"
                popupType="dialog"
                popupTitle="选择数据"
                popupWidth="1200px"
                hasPage
                @change="GoodsListBtn"
                :style="{
                  width: '100%',
                }"
                :extraOptions="optionsObj.GoodsList" />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="工序" :bordered="false" />
              <a-table
                :data-source="dataForm.tableField7b5631"
                :columns="tableField7b5631Columns"
                size="small"
                rowKey="jnpfId"
                :pagination="false"
                :scroll="{ x: 'max-content' }"
                :rowSelection="gettableField7b5631RowSelection">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                /></template>
                <template #bodyCell="{ column, record, index }">
                  <!-- <template v-if="column.key === 'index'">{{ index + 1 }}</template> -->
                  <template v-if="column.key === 'F_ProcessId'">
                    <JnpfSelect
                      v-model:value="record.F_ProcessId"
                      placeholder=""
                      :options="optionsObj.F_ProcessIdOptions"
                      :fieldNames="optionsObj.F_ProcessIdProps"
                      allowClear
                      showSearch
                      disabled
                      :style="{
                        width: '100%',
                      }" />
                  </template>

                  <!-- <template v-if="column.key === 'F_ProcessCode'">
                    <p>{{ record.F_ProcessCode }}</p>
                  </template>
                  <template v-if="column.key === 'F_DefectIds'">
                    <p>{{ record.F_DefectIds }}</p>
                  </template>
                  <template v-if="column.key === 'F_DefectHandle'">
                    <p>{{ record.F_DefectHandle }}</p>
                  </template>
                  <template v-if="column.key === 'F_TaskTimer'">
                    <p>{{ record.F_TaskTimer }}</p>
                  </template> -->

                  <template v-if="column.key === 'F_SortCode'">
                    <JnpfInputNumber
                      v-model:value="record.F_SortCode"
                      placeholder="排序不可重复"
                      :min="1"
                      :controls="false"
                      :disabled="!record.F_ProcessId"
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_Description'">
                    <JnpfInput
                      v-model:value="record.F_Description"
                      placeholder=""
                      allowClear
                      :disabled="!record.F_ProcessId"
                      :style="{
                        width: '100%',
                      }"
                      :showCount="false" />
                  </template>
                  <template v-if="column.key === 'F_CreatorTime'">
                    <JnpfOpenData
                      v-model:value="record.F_CreatorTime"
                      type="currTime"
                      readonly
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'action'">
                    <a-space>
                      <!-- <a-button class="action-btn" type="link" @click="copytableField7b5631Row(index)" size="small">{{
                        t('common.copyText', '复制')
                      }}</a-button> -->
                      <a-button class="action-btn" type="link" @click="updateHandle(record)" size="small" :disabled="!record.F_ProcessId">{{
                        t('修改')
                      }}</a-button>
                      <a-button class="action-btn" type="link" color="error" @click="removetableField7b5631Row(index, true)" size="small">{{
                        t('common.delText', '删除')
                      }}</a-button>
                    </a-space>
                  </template>
                </template>
              </a-table>
              <a-space class="input-table-footer-btn">
                <!-- <a-button type="primary" preIcon="icon-ym icon-ym-btn-add" @click="addtableField7b5631Row">{{ t('common.add1Text', '添加') }}</a-button> -->
                <!-- <a-button type="danger" preIcon="icon-ym icon-ym-btn-clearn" @click="batchRemovetableField7b5631Row(true)">{{
                  t('common.batchDelText', '批量删除') 
                }}</a-button>-->
              </a-space>
            </a-form-item>
          </a-col>
        </a-row>
      </a-form>
    </a-row>
  </BasicModal>
  <SelectModal :config="state.currTableConf" :formData="state.dataForm" ref="selectModal" @select="addForSelect" />
  <StepForm ref="stepFormRef" @submit="onStepSubmit" />
</template>
<script lang="ts" setup>
  import { create, update, getInfo } from './helper/api';
  import { reactive, toRefs, nextTick, ref, unref, computed, toRaw, inject } from 'vue';
  import { BasicModal, useModal } from '@/components/Modal';
  import SelectModal from '@/components/CommonModal/src/SelectModal.vue';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useI18n } from '@/hooks/web/useI18n';
  import { useUserStore } from '@/store/modules/user';
  import type { FormInstance } from 'ant-design-vue';
  import dayjs from 'dayjs';
  import { cloneDeep } from 'lodash-es';
  import { buildUUID } from '@/utils/uuid';
  import StepForm from '@/views/Subcode/aProdRoutingStep/Form.vue';
  import { getDataInterfaceRes } from '@/api/systemData/dataInterface';

  interface State {
    GoodsList: any[];
    dataForm: any;
    dataRule: any;
    optionsObj: any;
    currVmodel: any;
    currTableConf: any;
    addTableConf: any;
    tableRows: any;
    tableField7b5631outerActiveKey: number[];
    tableField7b5631innerActiveKey: string[];
    tableField7b5631DefaultExpandAll: boolean;
    selectedtableField7b5631RowKeys: any[];
    title: string;
    continueText: string;
    allList: any[];
    currIndex: number;
    isContinue: boolean;
    submitType: number;
    showContinueBtn: boolean;
  }

  const emit = defineEmits(['reload']);
  const getLeftTreeActiveInfo: (() => any) | null = inject('getLeftTreeActiveInfo', null);
  const userStore = useUserStore();
  const userInfo = userStore.getUserInfo;
  const { createMessage, createConfirm } = useMessage();
  const { t } = useI18n();
  const [registerModal, { openModal, setModalProps }] = useModal();

  const stepFormRef = ref<any>(null);
  const formRef = ref<FormInstance>();
  const selectModal = ref(null);
  const tableField7b5631Columns: any[] = computed(() => {
    let list = [
      {
        title: '工序',
        dataIndex: 'F_ProcessId',
        key: 'F_ProcessId',
        tipLabel: '',
        required: true,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '280px',
        fixed: false,
        formP: 'F_ProcessId',
        isSystemControl: false,
      },
      // {
      //   title: '工序编号',
      //   dataIndex: 'F_ProcessCode',
      //   key: 'F_ProcessCode',
      //   tipLabel: '',
      //   required: true,
      //   align: 'left',
      //   span: '24',
      //   labelWidth: '',
      //   width: '',
      //   fixed: false,
      //   formP: 'F_ProcessId',
      //   isSystemControl: false,
      // },
      // {
      //   title: '不良品项',
      //   dataIndex: 'F_DefectIds',
      //   key: 'F_DefectIds',
      //   tipLabel: '',
      //   required: true,
      //   align: 'left',
      //   span: '24',
      //   labelWidth: '',
      //   width: '',
      //   fixed: false,
      //   formP: 'F_ProcessId',
      //   isSystemControl: false,
      // },
      // {
      //   title: '需返工 / 报废',
      //   dataIndex: 'F_DefectHandleName',
      //   key: 'F_DefectHandleName',
      //   tipLabel: '',
      //   required: true,
      //   align: 'left',
      //   span: '24',
      //   labelWidth: '',
      //   width: '',
      //   fixed: false,
      //   formP: 'F_ProcessId',
      //   isSystemControl: false,
      // },
      // {
      //   title: '生产任务计时',
      //   dataIndex: 'F_TaskTimer',
      //   key: 'F_TaskTimer',
      //   tipLabel: '',
      //   required: false,
      //   align: 'left',
      //   span: '24',
      //   labelWidth: '',
      //   width: '',
      //   fixed: false,
      //   formP: 'F_TaskTimer',
      //   isSystemControl: false,
      // },
      {
        title: '排序',
        dataIndex: 'F_SortCode',
        key: 'F_SortCode',
        tipLabel: '排序不可重复',
        required: true,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '150px',
        fixed: false,
        formP: 'F_SortCode',
        isSystemControl: false,
      },
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
  const gettableField7b5631RowSelection = computed(() => {
    const rowSelection = {
      selectedRowKeys: state.selectedtableField7b5631RowKeys,
      onChange: (selectedRowKeys: string[]) => {
        state.selectedtableField7b5631RowKeys = (selectedRowKeys || []).sort().reverse();
      },
    };
    return rowSelection;
  });
  const state = reactive<State>({
    GoodsList: [],
    dataForm: {
      id: '',
      F_RoutingNo: undefined,
      F_RoutingName: undefined,
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
      tableField7b5631: [],
    },
    tableField7b5631outerActiveKey: [0],
    tableField7b5631innerActiveKey: [],
    tableField7b5631DefaultExpandAll: true,
    selectedtableField7b5631RowKeys: [],
    dataRule: {
      F_RoutingName: [
        {
          required: true,
          message: '请输入工艺路线名称',
          trigger: 'blur',
        },
      ],
    },
    optionsObj: {
      F_ProcessIdProps: { label: 'F_ProcessName', value: 'id' },
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
      Process_F_ProcessIdTemplateJson: [],
      tableField7b5631_F_ProcessIdOptions: [
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
      tableField7b5631_F_ProcessIdTemplateJson: [],
    },
    currVmodel: '',
    currTableConf: {},
    tableRows: {
      tableField7b5631: {
        enabledmark: undefined,
        F_ProcessId: undefined,
        F_TaskTimer: undefined,
        F_ProcessName: undefined,
        F_ProcessCode: undefined,
        F_DefectIds: undefined,
        F_DefectHandle: undefined,
        F_SortCode: undefined,
        F_Description: undefined,
        F_CreatorTime: undefined,
        F_WagePrice: undefined,
        F_StdHour: 0,
        F_StdMinute: 0,
        F_StdSecond: 0,
        F_PriceType: '0',
        F_UnitRatioProd: undefined,
        F_UnitRatioReport: undefined,
        F_Files: [],
        F_IsOutsource: 0,
        F_IsQc: 0,
        F_UnitRatioBase: undefined,
        tableField3b6f08: [],
      },
    },
    addTableConf: {},
    title: '',
    continueText: '',
    allList: [],
    currIndex: 0,
    isContinue: false,
    submitType: 0,
    showContinueBtn: true,
  });
  const { title, continueText, showContinueBtn, dataForm, submitType, dataRule, optionsObj } = toRefs(state);

  const getPrevDisabled = computed(() => state.currIndex === 0);
  const getNextDisabled = computed(() => state.currIndex === state.allList.length - 1);

  defineExpose({ init });

  // 修改工序
  function updateHandle(record) {
    // 不带工作流
    const data = {
      ...record,
      jnpfId: record.jnpfId,
    };
    stepFormRef.value?.init(data);
  }
  // 监听 submit 事件
  function onStepSubmit(returnRow) {
    // returnRow 就是 StepForm 里 emit 出来的整行数据
    const idx = state.dataForm.tableField7b5631.findIndex(item => item.jnpfId === returnRow.jnpfId);
    if (idx > -1) {
      // 整行替换，保持响应式
      state.dataForm.tableField7b5631[idx].F_WagePrice = returnRow.F_WagePrice;
      state.dataForm.tableField7b5631[idx].F_UnitRatioReport = returnRow.F_UnitRatioReport;
      state.dataForm.tableField7b5631[idx].F_UnitRatioProd = returnRow.F_UnitRatioProd;
      state.dataForm.tableField7b5631[idx].F_UnitRatioBase = returnRow.F_UnitRatioBase;
      state.dataForm.tableField7b5631[idx].F_StdSecond = returnRow.F_StdSecond;
      state.dataForm.tableField7b5631[idx].F_StdMinute = returnRow.F_StdMinute;
      state.dataForm.tableField7b5631[idx].F_StdHour = returnRow.F_StdHour;
      state.dataForm.tableField7b5631[idx].F_PriceType = returnRow.F_PriceType;
      state.dataForm.tableField7b5631[idx].F_Files = returnRow.F_Files;
      state.dataForm.tableField7b5631[idx].F_Description = returnRow.F_Description;
      state.dataForm.tableField7b5631[idx].F_IsOutsource = returnRow.F_IsOutsource;
      state.dataForm.tableField7b5631[idx].F_IsQc = returnRow.F_IsQc;
      state.dataForm.tableField7b5631[idx].F_DefectHandle = returnRow.F_DefectHandle;
      state.dataForm.tableField7b5631[idx].F_TaskTimer = returnRow.F_TaskTimer;
      state.dataForm.tableField7b5631[idx].tableField3b6f08 = returnRow.tableField3b6f08;
    }
  }

  function init(data) {
    state.submitType = 0;
    state.showContinueBtn = true;
    state.title = !data.id ? '新增' : '编辑';
    setFormProps({ continueLoading: false });
    state.dataForm.id = data.id;
    openModal();
    nextTick(() => {
      getForm().resetFields();
      state.dataForm.tableField7b5631 = [];
      setTimeout(initData, 0);
    });
  }
  function initData() {
    changeLoading(true);
    if (state.dataForm.id) {
      getData(state.dataForm.id);
    } else {
      // 设置默认值
      state.dataForm = {
        F_RoutingNo: undefined,
        F_RoutingName: undefined,
        F_CreatorUserId: undefined,
        F_CreatorTime: undefined,
        tableField7b5631: [],
      };
      state.GoodsList = [];
      getF_ProcessIdOptions();
      if (getLeftTreeActiveInfo) state.dataForm = { ...state.dataForm, ...(getLeftTreeActiveInfo() || {}) };
      changeLoading(false);
    }
  }
  function getForm() {
    const form = unref(formRef);
    if (!form) {
      throw new Error('form is null!');
    }
    return form;
  }
  function getData(id) {
    getInfo(id).then(res => {
      state.dataForm = res.data || {};
      for (let i = 0; i < state.dataForm.tableField7b5631.length; i++) {
        const element = state.dataForm.tableField7b5631[i];
        element.jnpfId = buildUUID();
        state.GoodsList = [];
        for (let i = 0; i < state.dataForm.tableField7b5631.length; i++) {
          const element = state.dataForm.tableField7b5631[i];
          element.jnpfId = buildUUID();
          state.GoodsList.push(state.dataForm.tableField7b5631[i].F_ProcessId);
        }
        getF_ProcessIdOptions();
      }
      changeLoading(false);
    });
  }
  async function handleSubmit() {
    try {
      const values = await getForm()?.validate();
      if (!values) return;
      if (!tableField7b5631Exist()) return;
      const sortCodes = state.dataForm.tableField7b5631.map(m => m.F_SortCode);
      const duplicates = sortCodes.filter((code, idx, arr) => arr.indexOf(code) !== idx);
      if (duplicates.length) {
        createMessage.error(`排序号重复：${[...new Set(duplicates)].join('、')}`);
        return false; // 阻断后续提交
      }

      setFormProps({ confirmLoading: true });
      const formMethod = state.dataForm.id ? update : create;
      formMethod(state.dataForm)
        .then(res => {
          createMessage.success(res.msg);
          setFormProps({ confirmLoading: false });
          if (state.submitType == 1) {
            initData();
            state.isContinue = true;
          } else {
            setFormProps({ open: false });
            emit('reload');
          }
        })
        .catch(() => {
          setFormProps({ confirmLoading: false });
        });
    } catch (_) {}
  }
  function handlePrev() {
    state.currIndex--;
    handleGetNewInfo();
  }
  function handleNext() {
    state.currIndex++;
    handleGetNewInfo();
  }
  function handleGetNewInfo() {
    changeLoading(true);
    getForm().resetFields();
    const id = state.allList[state.currIndex].id;
    getData(id);
  }
  function setFormProps(data) {
    setModalProps(data);
  }
  function changeLoading(loading) {
    setModalProps({ loading });
  }
  async function onClose() {
    if (state.isContinue) emit('reload');
    return true;
  }
  function getF_ProcessIdOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2012092092830060544', query).then(res => {
      let data = res.data;
      state.optionsObj.F_ProcessIdOptions = Array.isArray(data) ? data : [];
    });
  }
  function addtableField7b5631Row() {
    let item = {
      jnpfId: buildUUID(),
      F_ProcessId: undefined,
      F_TaskTimer: undefined,
      F_ProcessName: undefined,
      F_ProcessCode: undefined,
      F_DefectIds: undefined,
      F_DefectHandle: undefined,
      F_SortCode: undefined,
      F_Description: undefined,
      F_CreatorTime: undefined,
      F_WagePrice: undefined,
      F_StdHour: 0,
      F_StdMinute: 0,
      F_StdSecond: 0,
      F_PriceType: '0',
      F_UnitRatioProd: undefined,
      F_UnitRatioReport: undefined,
      F_Files: [],
      F_IsOutsource: 0,
      F_IsQc: 0,
      F_UnitRatioBase: undefined,
      tableField3b6f08: [],
    };
    state.dataForm.tableField7b5631.push(item);
    state.tableField7b5631innerActiveKey.push(item.jnpfId);
  }
  function removetableField7b5631Row(index, showConfirm = false) {
    if (showConfirm) {
      createConfirm({
        iconType: 'warning',
        title: '提示',
        content: '此操作将永久删除该数据, 是否继续?',
        onOk: () => {
          state.dataForm.tableField7b5631.splice(index, 1);
          state.GoodsList.splice(index, 1);
        },
      });
    } else {
      state.dataForm.tableField7b5631.splice(index, 1);
      state.GoodsList.splice(index, 1);
    }
  }
  function copytableField7b5631Row(index) {
    let item = cloneDeep(state.dataForm.tableField7b5631[index]);
    let copyData = {};
    for (let i = 0; i < unref(tableField7b5631Columns).length; i++) {
      const cur = unref(tableField7b5631Columns)[i];
      if (cur.key != 'index' && cur.key != 'action') {
        if (cur.children?.length && cur.jnpfKey == 'complexHeader') {
          for (let j = 0; j < cur.children.length; j++) {
            copyData[cur.children[j].key] = cur.isSystemControl ? null : item[cur.children[j].key];
          }
        } else {
          copyData[cur.key] = cur.isSystemControl ? null : item[cur.key];
        }
      }
    }
    const copyItem = { ...copyData, jnpfId: buildUUID() };
    state.dataForm.tableField7b5631.push(copyItem);
    state.tableField7b5631innerActiveKey.push(copyItem.jnpfId);
  }
  function batchRemovetableField7b5631Row(showConfirm = false) {
    if (!state.selectedtableField7b5631RowKeys.length) return createMessage.error('请选择一条数据');
    const handleBatchRemove = () => {
      state.dataForm.tableField7b5631 = state.dataForm.tableField7b5631.filter(o => !state.selectedtableField7b5631RowKeys.includes(o.jnpfId));
      nextTick(() => {
        state.selectedtableField7b5631RowKeys = [];
      });
    };
    if (!showConfirm) return handleBatchRemove();
    createConfirm({
      iconType: 'warning',
      title: '提示',
      content: '此操作将永久删除该数据, 是否继续?',
      onOk: handleBatchRemove,
    });
  }
  function tableField7b5631Exist() {
    let isOk = true;
    for (let i = 0; i < state.dataForm.tableField7b5631.length; i++) {
      const e = state.dataForm.tableField7b5631[i];
      if (!e.F_ProcessId) {
        createMessage.error('工序' + t('sys.validate.textRequiredSuffix'));
        isOk = false;
        break;
      }
      if (!e.F_SortCode) {
        createMessage.error('排序' + t('sys.validate.textRequiredSuffix'));
        isOk = false;
        break;
      }
    }
    return isOk;
  }
  function openSelectDialog(key, value) {
    state.currTableConf = state.addTableConf[key + 'List' + value];
    state.currVmodel = key;
    nextTick(() => {
      (selectModal.value as any)?.openSelectModal();
    });
  }
  function addForSelect(data) {
    for (let i = 0; i < data.length; i++) {
      let item = { ...state.tableRows[state.currVmodel], ...data[i], jnpfId: buildUUID() };
      state.dataForm[state.currVmodel].push(cloneDeep(item));
      if (state[state.currVmodel + 'DefaultExpandAll']) state[state.currVmodel + 'innerActiveKey'].push(item.jnpfId);
    }
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

  function ProcessIdBtn(val, option) {
    var num = 0;
    var indexs = 0;
    // 处理选择变化
    for (var i = 0; i < state.dataForm.tableField7b5631.length; i++) {
      if (option.id == state.dataForm.tableField7b5631[i].F_ProcessId) {
        state.dataForm.tableField7b5631[i].F_ProcessName = option.F_ProcessName;
        state.dataForm.tableField7b5631[i].F_ProcessCode = option.F_ProcessCode;
        state.dataForm.tableField7b5631[i].F_DefectIds = option.F_DefectIds;
        state.dataForm.tableField7b5631[i].F_IsOutsource = option.F_IsOutsource;
        state.dataForm.tableField7b5631[i].F_IsQc = option.F_IsQc;
        state.dataForm.tableField7b5631[i].F_DefectHandle = option.F_DefectHandle;
        state.dataForm.tableField7b5631[i].F_TaskTimer = option.F_TaskTimer;
        num += 1;
        indexs = i;
      }
    }
    if (num > 1) {
      removetableField7b5631Row(indexs, false);
      createMessage.error('项目已存在!');
    }
  }

  function GoodsListBtn(val, option) {
    /* 兜底 */
    if (!Array.isArray(state.dataForm.tableField7b5631)) {
      state.dataForm.tableField7b5631 = [];
    }
    const optionIdSet = new Set(option.map(o => o.id));

    /* 1. 删掉已取消勾选的行 */
    state.dataForm.tableField7b5631 = state.dataForm.tableField7b5631.filter(item => optionIdSet.has(item.F_ProcessId));

    /* 2. 计算当前最大序号（0 表示还没有任何行） */
    const maxSort = state.dataForm.tableField7b5631.reduce((max, item) => {
      return item.F_SortCode > max ? item.F_SortCode : max;
    }, 0);

    /* 3. 追加本地还没有的行，序号依次累加 */
    let nextSort = maxSort + 1;
    option.forEach(o => {
      const exist = state.dataForm.tableField7b5631.some(item => item.F_ProcessId === o.id);
      if (!exist) {
        const newRow = {
          jnpfId: buildUUID(),
          F_ProcessId: o.id,
          F_SortCode: nextSort, // ← 自动累加
        };
        state.dataForm.tableField7b5631.push(newRow);
        state.tableField7b5631innerActiveKey.push(newRow.jnpfId);
        nextSort++; // 下一行继续 +1
      }
    });
  }
</script>
