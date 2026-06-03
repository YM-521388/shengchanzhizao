<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    width="40%"
    :minHeight="100"
    okText="确定"
    cancelText="取消"
    @ok="handleSubmit"
    :closeFunc="onClose"
    :canFullscreen="true">
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
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_ContractId') || isInvoiceOpen || leftTreeContractId">
            <a-form-item name="F_ContractId" :labelCol="{ style: { width: '100px' } }">
              <template #label>合同</template>
              <JnpfPopupSelect
                v-model:value="dataForm.F_ContractId"
                placeholder="请选择"
                :templateJson="optionsObj.F_ContractIdTemplateJson"
                :disabled="disableContract"
                allowClear
                field="F_ContractId"
                interfaceId="2010889611072638976"
                :columnOptions="optionsObj.F_ContractIdOptions"
                relationField="F_ContractCode"
                propsValue="F_Id"
                :pageSize="20"
                popupType="dialog"
                popupTitle="选择数据"
                popupWidth="1000px"
                :style="{
                  width: '100%',
                }"
                :extraOptions="optionsObj.F_ContractId" />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_Money') || isInvoiceOpen || leftTreeContractId">
            <a-form-item name="F_Money" :labelCol="{ style: { width: '100px' } }">
              <template #label>开票金额(元)</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_Money"
                placeholder="请输入"
                :controls="false"
                :disabled="disableMoney"
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <!-- <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_Status')">
            <a-form-item name="F_Status" :labelCol="{ style: { width: '100px' } }">
              <template #label>开票状态</template>
              <JnpfInput
                v-model:value="dataForm.F_Status"
                placeholder="请输入"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col> -->
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_ApplyFiles') || isInvoiceOpen || leftTreeContractId">
            <a-form-item name="F_ApplyFiles" :labelCol="{ style: { width: '100px' } }">
              <template #label>申请附件</template>
              <JnpfUploadFile
                v-model:value="dataForm.F_ApplyFiles"
                buttonText="点击上传"
                pathType="defaultPath"
                :fileSize="10"
                sizeUnit="MB"
                :limit="9"
                :sortRule="[]"
                timeFormat="YYYY" />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_ApplyRemark') || isInvoiceOpen || leftTreeContractId">
            <a-form-item name="F_ApplyRemark" :labelCol="{ style: { width: '100px' } }">
              <template #label>申请备注</template>
              <JnpfTextarea
                v-model:value="dataForm.F_ApplyRemark"
                placeholder="请输入"
                allowClear
                :autoSize="{
                  minRows: 2,
                  maxRows: 4,
                }"
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_InvoiceFiles') && isInvoiceOpen">
            <a-form-item name="F_InvoiceFiles" :labelCol="{ style: { width: '100px' } }">
              <template #label>开票附件</template>
              <JnpfUploadFile
                v-model:value="dataForm.F_InvoiceFiles"
                buttonText="点击上传"
                pathType="defaultPath"
                :fileSize="10"
                sizeUnit="MB"
                :limit="9"
                :sortRule="[]"
                timeFormat="YYYY" />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_InvoiceRemark') && isInvoiceOpen">
            <a-form-item name="F_InvoiceRemark" :labelCol="{ style: { width: '100px' } }">
              <template #label>开票备注</template>
              <JnpfTextarea
                v-model:value="dataForm.F_InvoiceRemark"
                placeholder="请输入"
                allowClear
                :autoSize="{
                  minRows: 2,
                  maxRows: 4,
                }"
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_ApplyUserId') && !isInvoiceOpen">
            <a-form-item name="F_ApplyUserId" :labelCol="{ style: { width: '100px' } }">
              <template #label>申请人员</template>
              <JnpfUserSelect
                v-model:value="dataForm.F_ApplyUserId"
                placeholder="请选择"
                selectType="all"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_ApplyTime') && !isInvoiceOpen">
            <a-form-item name="F_ApplyTime" :labelCol="{ style: { width: '100px' } }">
              <template #label>申请时间</template>
              <JnpfDatePicker
                v-model:value="dataForm.F_ApplyTime"
                placeholder="请选择"
                format="yyyy-MM-dd"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_InvoiceUserId') && !isInvoiceOpen">
            <a-form-item name="F_InvoiceUserId" :labelCol="{ style: { width: '100px' } }">
              <template #label>开票人员</template>
              <JnpfUserSelect
                v-model:value="dataForm.F_InvoiceUserId"
                placeholder="请选择"
                selectType="all"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_InvoiceTime') && !isInvoiceOpen">
            <a-form-item name="F_InvoiceTime" :labelCol="{ style: { width: '100px' } }">
              <template #label>开票时间</template>
              <JnpfDatePicker
                v-model:value="dataForm.F_InvoiceTime"
                placeholder="请选择"
                format="yyyy-MM-dd"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <!-- <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="操作日志" :bordered="false" />
              <a-table
                :data-source="dataForm.tableField2cca74"
                :columns="tableField2cca74Columns"
                size="small"
                rowKey="jnpfId"
                :pagination="false"
                :scroll="{ x: 'max-content' }"
                :rowSelection="gettableField2cca74RowSelection">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                  <template v-if="column.key === 'F_Title'">
                    <JnpfInput
                      v-model:value="record.F_Title"
                      placeholder="请输入"
                      allowClear
                      :style="{
                        width: '100%',
                      }"
                      :showCount="false" />
                  </template>
                  <template v-if="column.key === 'F_Content'">
                    <JnpfInput
                      v-model:value="record.F_Content"
                      placeholder="请输入"
                      allowClear
                      :style="{
                        width: '100%',
                      }"
                      :showCount="false" />
                  </template>
                  <template v-if="column.key === 'F_CreatorUserId'">
                    <JnpfOpenData
                      v-model:value="record.F_CreatorUserId"
                      type="currUser"
                      readonly
                      :style="{
                        width: '100%',
                      }" />
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
                      <a-button class="action-btn" type="link" @click="copytableField2cca74Row(index)" size="small">{{
                        t('common.copyText', '复制')
                      }}</a-button>
                      <a-button class="action-btn" type="link" color="error" @click="removetableField2cca74Row(index, true)" size="small">{{
                        t('common.delText', '删除')
                      }}</a-button>
                    </a-space>
                  </template>
                </template>
              </a-table>
              <a-space class="input-table-footer-btn">
                <a-button type="primary" preIcon="icon-ym icon-ym-btn-add" @click="addtableField2cca74Row">{{ t('common.add1Text', '添加') }}</a-button>
                <a-button type="danger" preIcon="icon-ym icon-ym-btn-clearn" @click="batchRemovetableField2cca74Row(true)">{{
                  t('common.batchDelText', '批量删除')
                }}</a-button>
              </a-space>
            </a-form-item>
          </a-col> -->
        </a-row>
      </a-form>
    </a-row>
    <template #centerFooter>
      <a-button v-if="showInvoiceCompletedBtn" type="primary" @click="handleInvoiceCompleted">{{ '开票完成' }}</a-button>
    </template>
  </BasicModal>
  <SelectModal :config="state.currTableConf" :formData="state.dataForm" ref="selectModal" @select="addForSelect" />
</template>
<script lang="ts" setup>
  import { create, update, getInfo, calculateContractAmount } from './helper/api';
  import { reactive, toRefs, nextTick, ref, unref, computed, inject } from 'vue';
  import { BasicModal, useModal } from '@/components/Modal';
  import SelectModal from '@/components/CommonModal/src/SelectModal.vue';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useI18n } from '@/hooks/web/useI18n';
  import { useUserStore } from '@/store/modules/user';
  import type { FormInstance } from 'ant-design-vue';
  import { cloneDeep } from 'lodash-es';
  import { buildUUID } from '@/utils/uuid';
  import { usePermission } from '@/hooks/web/usePermission';
  import { watch } from 'vue';
  import dayjs from 'dayjs';
  import { getDateTimeUnit } from '@/utils/jnpf';

  interface State {
    dataForm: any;
    dataRule: any;
    optionsObj: any;
    currVmodel: any;
    currTableConf: any;
    addTableConf: any;
    tableRows: any;
    tableField2cca74outerActiveKey: number[];
    tableField2cca74innerActiveKey: string[];
    tableField2cca74DefaultExpandAll: boolean;
    selectedtableField2cca74RowKeys: any[];
    title: string;
    continueText: string;
    allList: any[];
    currIndex: number;
    isContinue: boolean;
    submitType: number;
    showContinueBtn: boolean;
    showInvoiceCompletedBtn: boolean;
    // 是否通过“开票”按钮打开（用于设置状态并禁止编辑某些字段）
    isInvoiceOpen: boolean;
    // 禁止编辑合同字段
    disableContract: boolean;
    // 禁止编辑金额字段
    disableMoney: boolean;
  }

  const emit = defineEmits(['reload']);
  const getLeftTreeActiveInfo: (() => any) | null = inject('getLeftTreeActiveInfo', null);
  const leftTreeContractId = computed(() => {
    try {
      return getLeftTreeActiveInfo ? getLeftTreeActiveInfo().F_ContractId : undefined;
    } catch (e) {
      return undefined;
    }
  });
  const userStore = useUserStore();
  const userInfo = userStore.getUserInfo;
  const { createMessage, createConfirm } = useMessage();
  const { t } = useI18n();
  const { hasFormP } = usePermission();
  const [registerModal, { openModal, setModalProps }] = useModal();
  const formRef = ref<FormInstance>();
  const selectModal = ref(null);
  const tableField2cca74Columns: any[] = computed(() => {
    let list = [
      {
        title: '标题',
        dataIndex: 'F_Title',
        key: 'F_Title',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Title',
        isSystemControl: false,
      },
      {
        title: '内容',
        dataIndex: 'F_Content',
        key: 'F_Content',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Content',
        isSystemControl: false,
      },
      {
        title: '创建人员',
        dataIndex: 'F_CreatorUserId',
        key: 'F_CreatorUserId',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_CreatorUserId',
        isSystemControl: true,
      },
      {
        title: '创建时间',
        dataIndex: 'F_CreatorTime',
        key: 'F_CreatorTime',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_CreatorTime',
        isSystemControl: true,
      },
    ];
    list = list.filter(o => hasFormP('tableField2cca74-' + o.formP));
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
    let columns = [noColumn, ...columnList, actionColumn];
    const leftFixedList = columns.filter(o => o.fixed === 'left');
    const rightFixedList = columns.filter(o => o.fixed === 'right');
    const noFixedList = columns.filter(o => o.fixed !== 'left' && o.fixed !== 'right');
    return [...leftFixedList, ...noFixedList, ...rightFixedList];
  });
  const gettableField2cca74RowSelection = computed(() => {
    const rowSelection = {
      selectedRowKeys: state.selectedtableField2cca74RowKeys,
      onChange: (selectedRowKeys: string[]) => {
        state.selectedtableField2cca74RowKeys = (selectedRowKeys || []).sort().reverse();
      },
    };
    return rowSelection;
  });
  const state = reactive<State>({
    dataForm: {
      id: '',
      F_ContractId: undefined,
      F_Money: undefined,
      F_Status: '0',
      F_ApplyFiles: [],
      F_ApplyRemark: undefined,
      F_InvoiceFiles: [],
      F_InvoiceRemark: undefined,
      F_ApplyUserId: undefined,
      F_ApplyTime: undefined,
      F_InvoiceUserId: undefined,
      F_InvoiceTime: undefined,
      tableField2cca74: [],
    },
    tableField2cca74outerActiveKey: [0],
    tableField2cca74innerActiveKey: [],
    tableField2cca74DefaultExpandAll: true,
    selectedtableField2cca74RowKeys: [],
    dataRule: {},
    optionsObj: {
      F_ContractIdOptions: [
        {
          value: 'F_ContractCode',
          label: '编号',
        },
        {
          value: 'F_PrepayAmount',
          label: '预付款',
        },
        {
          value: 'F_SaleTotal',
          label: '销售总数',
        },
        {
          value: 'F_ContractAmount',
          label: '合同金额',
        },
        {
          value: 'F_IsTaxable',
          label: '是否含税',
        },
        {
          value: 'F_AuditStatus',
          label: '审核状态',
        },
        {
          value: 'F_Description',
          label: '备注',
        },
        {
          value: 'F_CreatorTime',
          label: '参加时间',
        },
      ],
      F_ContractIdTemplateJson: [],
      F_ContractId: [],
    },
    currVmodel: '',
    currTableConf: {},
    tableRows: {
      tableField2cca74: {
        enabledmark: undefined,
        F_Title: undefined,
        F_Content: undefined,
        F_CreatorUserId: undefined,
        F_CreatorTime: undefined,
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
    // 是否显示“开票完成”按钮（只有从列表编辑打开时显示）
    showInvoiceCompletedBtn: false,
    isInvoiceOpen: false,
    disableContract: false,
    disableMoney: false,
  });
  const { title, continueText, dataForm, submitType, dataRule, optionsObj, showInvoiceCompletedBtn, disableContract, disableMoney, isInvoiceOpen } =
    toRefs(state);

  const getPrevDisabled = computed(() => state.currIndex === 0);
  const getNextDisabled = computed(() => state.currIndex === state.allList.length - 1);

  // 监听合同选择变化，自动计算合同金额
  watch(
    () => state.dataForm.F_ContractId,
    async newContractId => {
      if (newContractId) {
        try {
          const res = await calculateContractAmount(newContractId);
          if (res.data !== undefined && res.data !== null) {
            state.dataForm.F_Money = res.data;
          }
        } catch (error) {
          console.error('计算合同金额失败:', error);
        }
      } else {
        // 清空合同时也清空金额
        state.dataForm.F_Money = undefined;
      }
    },
  );

  defineExpose({ init });

  function init(data) {
    state.submitType = 0;
    state.showContinueBtn = true;
    state.title = !data.id ? '新增' : '编辑';
    setFormProps({ continueLoading: false });
    state.dataForm.id = data.id;
    // 根据调用方传入标记决定是否显示“开票完成”按钮
    // 如果从合同列表打开（openedFromContractList），不要显示该按钮
    state.showInvoiceCompletedBtn = !!data.openedFromListEdit && !data.openedFromContractList;
    // 如果是通过流程“开票”按钮打开，设置为流程打开的只读行为（合同和金额都只读）
    state.isInvoiceOpen = !!data.openedFromInvoice;
    if (data.openedFromInvoice) {
      state.disableContract = true;
      state.disableMoney = true;
    }
    // 如果是从合同列表打开，仅禁用合同字段并尝试使用传入的合同 id 填充
    if (data.openedFromContractList) {
      state.disableContract = true;
      state.disableMoney = false;
      if (data.F_ContractId) state.dataForm.F_ContractId = data.F_ContractId;
    }
    openModal();
    nextTick(() => {
      getForm().resetFields();
      state.dataForm.tableField2cca74 = [];
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
        F_ContractId: undefined,
        F_Money: undefined,
        F_Status: '0',
        F_ApplyFiles: [],
        F_ApplyRemark: undefined,
        F_InvoiceFiles: [],
        F_InvoiceRemark: undefined,
        F_ApplyUserId: userInfo.userId ? userInfo.userId : '',
        F_ApplyTime: dayjs(new Date()).startOf(getDateTimeUnit('yyyy-MM-dd')).valueOf(),
        F_InvoiceUserId: undefined,
        F_InvoiceTime: undefined,
        tableField2cca74: [],
      };
      // 如果通过“开票”打开，直接将状态设为“开票完成”
      if (state.isInvoiceOpen) {
        state.dataForm.F_Status = '1';
        // 记录当前登录用户 id 到开票人字段，并记录开票时间（取当天起始时间戳）
        state.dataForm.F_InvoiceUserId = userInfo.userId ? userInfo.userId : '';
        state.dataForm.F_InvoiceTime = dayjs(new Date()).startOf(getDateTimeUnit('yyyy-MM-dd')).valueOf();
      }
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
      for (let i = 0; i < state.dataForm.tableField2cca74.length; i++) {
        const element = state.dataForm.tableField2cca74[i];
        element.jnpfId = buildUUID();
      }
      // 如果通过流程“开票”打开，覆盖状态并禁止编辑合同/金额字段
      if (state.isInvoiceOpen) {
        state.dataForm.F_Status = '1';
        state.disableContract = true;
        state.disableMoney = true;
        // 覆盖开票人和开票时间为当前登录用户与当前日期（按天起始时间）
        state.dataForm.F_InvoiceUserId = userInfo.userId ? userInfo.userId : '';
        state.dataForm.F_InvoiceTime = dayjs(new Date()).startOf(getDateTimeUnit('yyyy-MM-dd')).valueOf();
      }
      // 如果通过 provide 注入了合同 id（来自列表），优先使用并禁用合同字段
      if (leftTreeContractId.value) {
        state.dataForm.F_ContractId = leftTreeContractId.value;
        state.disableContract = true;
      }
      changeLoading(false);
    });
  }
  async function handleSubmit() {
    try {
      const values = await getForm()?.validate();
      if (!values) return;
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
  // 开票完成：设置状态并提交（复用 handleSubmit）
  function handleInvoiceCompleted() {
    state.dataForm.F_Status = '1';
    handleSubmit();
  }
  function addtableField2cca74Row() {
    let item = {
      jnpfId: buildUUID(),
      F_Title: undefined,
      F_Content: undefined,
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
    };
    state.dataForm.tableField2cca74.push(item);
    state.tableField2cca74innerActiveKey.push(item.jnpfId);
  }
  function removetableField2cca74Row(index, showConfirm = false) {
    if (showConfirm) {
      createConfirm({
        iconType: 'warning',
        title: '提示',
        content: '此操作将永久删除该数据, 是否继续?',
        onOk: () => {
          state.dataForm.tableField2cca74.splice(index, 1);
        },
      });
    } else {
      state.dataForm.tableField2cca74.splice(index, 1);
    }
  }
  function copytableField2cca74Row(index) {
    let item = cloneDeep(state.dataForm.tableField2cca74[index]);
    let copyData = {};
    for (let i = 0; i < unref(tableField2cca74Columns).length; i++) {
      const cur = unref(tableField2cca74Columns)[i];
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
    state.dataForm.tableField2cca74.push(copyItem);
    state.tableField2cca74innerActiveKey.push(copyItem.jnpfId);
  }
  function batchRemovetableField2cca74Row(showConfirm = false) {
    if (!state.selectedtableField2cca74RowKeys.length) return createMessage.error('请选择一条数据');
    const handleBatchRemove = () => {
      state.dataForm.tableField2cca74 = state.dataForm.tableField2cca74.filter(o => !state.selectedtableField2cca74RowKeys.includes(o.jnpfId));
      nextTick(() => {
        state.selectedtableField2cca74RowKeys = [];
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
</script>
