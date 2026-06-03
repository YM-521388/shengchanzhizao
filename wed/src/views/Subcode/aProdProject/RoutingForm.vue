<template>
  <BasicModal v-bind="$attrs" @register="registerModal" width="1300px" :minHeight="100" okText="确定" cancelText="取消" @ok="handleSubmit" :closeFunc="onClose">
    <template #title>
      <a-space :size="10">
        <div class="text-16px font-medium">{{ title }}</div>
        <a-space-compact size="small" block v-if="dataForm.id">
          <a-tooltip :title="t('common.prevRecord')">
            <a-button size="small" :disabled="getPrevDisabled" @click="handlePrev">
              <i class="icon-ym icon-ym-caret-left text-10px"></i>
            </a-button>
          </a-tooltip>
          <a-tooltip :title="t('common.nextRecord')">
            <a-button size="small" :disabled="getNextDisabled" @click="handleNext">
              <i class="icon-ym icon-ym-caret-right text-10px"></i>
            </a-button>
          </a-tooltip>
        </a-space-compact>
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
                :data-source="dataForm.Process"
                :columns="ProcessColumns"
                size="small"
                rowKey="jnpfId"
                :pagination="false"
                :scroll="{ x: 'max-content' }"
                :rowSelection="getProcessRowSelection">
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

                  <template v-if="column.key === 'F_StartDate'">
                    <JnpfDatePicker
                      v-model:value="record.F_StartDate"
                      placeholder="请选择"
                      format="yyyy-MM-dd"
                      allowClear
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_EndDate'">
                    <JnpfDatePicker
                      v-model:value="record.F_EndDate"
                      placeholder="请选择"
                      format="yyyy-MM-dd"
                      allowClear
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_SortCode'">
                    <JnpfInputNumber
                      v-model:value="record.F_SortCode"
                      placeholder="排序不可重复"
                      :min="1"
                      :precision="0"
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
                      <!-- <a-button class="action-btn" type="link" @click="copyProcessRow(index)" size="small">{{
                        t('common.copyText', '复制')
                      }}</a-button> -->
                      <a-button class="action-btn" type="link" @click="goodsHandle(record)" size="small">{{ t('物料清单') }}</a-button>
                      <a-button class="action-btn" type="link" color="error" @click="removeProcessRow(index, true)" size="small">{{
                        t('common.delText', '删除')
                      }}</a-button>
                    </a-space>
                  </template>
                </template>
              </a-table>
              <a-space class="input-table-footer-btn">
                <!-- <a-button type="danger" preIcon="icon-ym icon-ym-btn-clearn" @click="batchRemoveProcessRow(true)">{{
                  t('common.batchDelText', '批量删除')
                }}</a-button> -->
              </a-space>
            </a-form-item>
          </a-col>
        </a-row>
      </a-form>
    </a-row>
  </BasicModal>
  <SelectModal :config="state.currTableConf" :formData="state.dataForm" ref="selectModal" @select="addForSelect" />
  <RoutingGoodsForm ref="goodsFormRef" @submit="onStepSubmit" />
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
  import RoutingGoodsForm from '@/views/Subcode/aProdProject/RoutingGoodsForm.vue';
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
    ProcessouterActiveKey: number[];
    ProcessinnerActiveKey: string[];
    ProcessDefaultExpandAll: boolean;
    selectedProcessRowKeys: any[];
    title: string;
    continueText: string;
    allList: any[];
    currIndex: number;
    isContinue: boolean;
    submitType: number;
    showContinueBtn: boolean;
  }

  const emit = defineEmits(['reload', 'routingSubmit']);
  const getLeftTreeActiveInfo: (() => any) | null = inject('getLeftTreeActiveInfo', null);
  const userStore = useUserStore();
  const userInfo = userStore.getUserInfo;
  const { createMessage, createConfirm } = useMessage();
  const { t } = useI18n();
  const [registerModal, { openModal, setModalProps }] = useModal();

  const goodsFormRef = ref<any>(null);
  const formRef = ref<FormInstance>();
  const selectModal = ref(null);
  const ProcessColumns: any[] = computed(() => {
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
      {
        title: '计划开始',
        dataIndex: 'F_StartDate',
        key: 'F_StartDate',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_StartDate',
        isSystemControl: false,
      },
      {
        title: '计划结束',
        dataIndex: 'F_EndDate',
        key: 'F_EndDate',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_EndDate',
        isSystemControl: false,
      },

      {
        title: '排序',
        dataIndex: 'F_SortCode',
        key: 'F_SortCode',
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
  const getProcessRowSelection = computed(() => {
    const rowSelection = {
      selectedRowKeys: state.selectedProcessRowKeys,
      onChange: (selectedRowKeys: string[]) => {
        state.selectedProcessRowKeys = (selectedRowKeys || []).sort().reverse();
      },
    };
    return rowSelection;
  });
  const state = reactive<State>({
    GoodsList: [],
    dataForm: {
      id: '',
      jnpfId: '',
      Process: [],
    },
    ProcessouterActiveKey: [0],
    ProcessinnerActiveKey: [],
    ProcessDefaultExpandAll: true,
    selectedProcessRowKeys: [],
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
    },
    currVmodel: '',
    currTableConf: {},
    tableRows: {
      Process: {
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

  // 物料清单
  function goodsHandle(record) {
    // 不带工作流
    const data = {
      ...record,
      jnpfId: record.jnpfId,
    };
    goodsFormRef.value?.init(data);
  }

  // 监听 submit 事件
  function onStepSubmit(returnRow) {
    // returnRow 就是 StepForm 里 emit 出来的整行数据
    const idx = state.dataForm.Process.findIndex(item => item.jnpfId === returnRow.jnpfId);
    if (idx > -1) {
      // 整行替换，保持响应式
      state.dataForm.Process[idx].tableField3b6f08 = returnRow.tableField3b6f08;
    }
  }

  function init(data) {
    state.submitType = 0;
    state.showContinueBtn = true;
    state.title = '编辑商品工序';
    setFormProps({ continueLoading: false });
    // 直接赋值，别再 reset
    state.dataForm = { ...data };
    state.GoodsList = [];
    if (!Array.isArray(state.dataForm.Process)) {
      state.dataForm.Process = [];
    }
    for (var i = 0; i < state.dataForm.Process.length; i++) {
      state.GoodsList.push(state.dataForm.Process[i].F_ProcessId);
    }
    openModal();
    initData();
  }
  function initData() {
    changeLoading(true);
    getF_ProcessIdOptions();
    if (getLeftTreeActiveInfo) state.dataForm = { ...state.dataForm, ...(getLeftTreeActiveInfo() || {}) };
    changeLoading(false);
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
      for (let i = 0; i < state.dataForm.Process.length; i++) {
        const element = state.dataForm.Process[i];
        element.jnpfId = buildUUID();
      }
      getF_ProcessIdOptions();
      changeLoading(false);
    });
  }
  async function handleSubmit() {
    try {
      const values = await getForm()?.validate();
      if (!values) return;
      if (!ProcessExist()) return;
      const sortCodes = state.dataForm.Process.map(m => m.F_SortCode);
      const duplicates = sortCodes.filter((code, idx, arr) => arr.indexOf(code) !== idx);
      if (duplicates.length) {
        createMessage.error(`排序号重复：${[...new Set(duplicates)].join('、')}`);
        return false; // 阻断后续提交
      }

      /* 如果你希望新增行当下就提示: 新增/删除完成后立即校验 */
      // const sortCodes = state.dataForm.Process.map(m => m.F_SortCode);
      // const hasRepeat = sortCodes.length !== new Set(sortCodes).size;
      // if (hasRepeat) {
      //   createMessage.error('排序号出现重复，请手动调整！');
      // }
      setFormProps({ confirmLoading: true });
      emit('routingSubmit', toRaw(state.dataForm));
      setFormProps({ confirmLoading: false });

      setFormProps({ open: false });
    } catch (e) {
      console.log('⑥ catch 异常', e); // ← 任何异常都会直接进这里
    }
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
  function removeProcessRow(index, showConfirm = false) {
    if (showConfirm) {
      createConfirm({
        iconType: 'warning',
        title: '提示',
        content: '此操作将永久删除该数据, 是否继续?',
        onOk: () => {
          state.dataForm.Process.splice(index, 1);
          state.GoodsList.splice(index, 1);
        },
      });
    } else {
      state.dataForm.Process.splice(index, 1);
      state.GoodsList.splice(index, 1);
    }
  }
  function ProcessExist() {
    let isOk = true;
    for (let i = 0; i < state.dataForm.Process.length; i++) {
      const e = state.dataForm.Process[i];
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

  function GoodsListBtn(val, option) {
    /* 兜底 */
    if (!Array.isArray(state.dataForm.Process)) {
      state.dataForm.Process = [];
    }
    const optionIdSet = new Set(option.map(o => o.id));

    /* 1. 删掉已取消勾选的行 */
    state.dataForm.Process = state.dataForm.Process.filter(item => optionIdSet.has(item.F_ProcessId));

    /* 2. 计算当前最大序号（0 表示还没有任何行） */
    const maxSort = state.dataForm.Process.reduce((max, item) => {
      return item.F_SortCode > max ? item.F_SortCode : max;
    }, 0);

    /* 3. 追加本地还没有的行，序号依次累加 */
    let nextSort = maxSort + 1;
    option.forEach(o => {
      const exist = state.dataForm.Process.some(item => item.F_ProcessId === o.id);
      if (!exist) {
        const newRow = {
          jnpfId: buildUUID(),
          F_ProcessId: o.id,
          F_StartDate: undefined,
          F_EndDate: undefined,
          F_Description: undefined,
          F_SortCode: nextSort, // ← 自动累加
        };
        state.dataForm.Process.push(newRow);
        state.ProcessinnerActiveKey.push(newRow.jnpfId);
        nextSort++; // 下一行继续 +1
      }
    });
  }
</script>
