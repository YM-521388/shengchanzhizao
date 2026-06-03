<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    width="1200px"
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
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('BGJL_ProdUserId')">
            <a-form-item name="F_ProdUserId" :labelCol="{ style: { width: '100px' } }">
              <template #label>生产人员</template>
              <JnpfUserSelect
                v-model:value="dataForm.F_ProdUserId"
                placeholder="请选择"
                selectType="all"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('BGJL_GoodQty')">
            <a-form-item name="F_GoodQty" :labelCol="{ style: { width: '100px' } }">
              <template #label>良品数</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_GoodQty"
                placeholder="请输入"
                :min="0"
                :controls="false"
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('BGJL_GoodNotQty')">
            <a-form-item name="F_GoodNotQty" :labelCol="{ style: { width: '100px' } }">
              <template #label>不良品数</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_GoodNotQty"
                placeholder="请输入"
                :min="0"
                :controls="false"
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('BGJL_StartTime')">
            <a-form-item name="F_StartTime" :labelCol="{ style: { width: '100px' } }">
              <template #label>开始时间</template>
              <JnpfDatePicker
                v-model:value="dataForm.F_StartTime"
                placeholder="请选择"
                format="yyyy-MM-dd HH:mm"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('BGJL_EndTime')">
            <a-form-item name="F_EndTime" :labelCol="{ style: { width: '100px' } }">
              <template #label>结束时间</template>
              <JnpfDatePicker
                v-model:value="dataForm.F_EndTime"
                placeholder="请选择"
                format="yyyy-MM-dd HH:mm"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8">
            <div class="duration-box" v-if="hasFormP('BGJL_BaoGongShiChang')">
              <span class="label">报工时长</span>
              <span class="value">{{ duration.h }}</span>
              <span class="unit">小时</span>
              <span class="value">{{ duration.m }}</span>
              <span class="unit">分钟</span>
            </div>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('BGJL_WagePrice') && dataForm.id">
            <a-form-item name="F_WagePrice" :labelCol="{ style: { width: '100px' } }">
              <template #label>工资单价</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_WagePrice"
                placeholder="请输入"
                :min="0"
                :precision="2"
                :controls="false"
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('BGJL_Files')">
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
                :sortRule="['1', '2']"
                timeFormat="YYYYMMDD" />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('BGJL_Description')">
            <a-form-item name="F_Description" :labelCol="{ style: { width: '100px' } }">
              <template #label>备注</template>
              <JnpfTextarea
                v-model:value="dataForm.F_Description"
                placeholder="请输入"
                :maxlength="500"
                allowClear
                :autoSize="{
                  minRows: 3,
                  maxRows: 3,
                }"
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>

          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <a-col :span="24" class="ant-col-item">
                <a-form-item name="DefectList" :labelCol="{ style: { width: '100px' } }">
                  <template #label>不良品项</template>
                  <JnpfSelect
                    v-model:value="state.DefectList"
                    placeholder="请选择"
                    :options="optionsObj.DefectListOptions"
                    :fieldNames="optionsObj.DefectListProps"
                    allowClear
                    showSearch
                    @change="DefectListBtn"
                    multiple
                    :style="{
                      width: '100%',
                    }" />
                </a-form-item>
              </a-col>
              <a-table
                :data-source="dataForm.tableField579169"
                :columns="tableField579169Columns"
                size="small"
                rowKey="jnpfId"
                :pagination="false"
                :scroll="{ x: 'max-content' }"
                :rowSelection="gettableField579169RowSelection">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                  <template v-if="column.key === 'F_DefectId'">
                    <JnpfSelect
                      v-model:value="record.F_DefectId"
                      :options="optionsObj.tableField579169_F_DefectIdOptions"
                      :fieldNames="optionsObj.tableField579169_F_DefectIdProps"
                      allowClear
                      disabled
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_Num'">
                    <JnpfInputNumber
                      v-model:value="record.F_Num"
                      placeholder="请输入"
                      :min="0"
                      :controls="false"
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
                      <a-button class="action-btn" type="link" color="error" @click="removetableField579169Row(index, true)" size="small">{{
                        t('common.delText', '删除')
                      }}</a-button>
                    </a-space>
                  </template>
                </template>
              </a-table>
              <a-space class="input-table-footer-btn"> </a-space>
            </a-form-item>
          </a-col>
        </a-row>
      </a-form>
    </a-row>
  </BasicModal>
  <SelectModal :config="state.currTableConf" :formData="state.dataForm" ref="selectModal" @select="addForSelect" />
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
  import { getDictionaryDataSelector } from '@/api/systemData/dictionary';
  import { getDataInterfaceRes } from '@/api/systemData/dataInterface';
  import dayjs from 'dayjs';
  import { cloneDeep } from 'lodash-es';
  import { buildUUID } from '@/utils/uuid';
  import { getTimeUnit, getDateTimeUnit } from '@/utils/jnpf';
  import { usePermission } from '@/hooks/web/usePermission';

  interface State {
    DefectList: any[];
    dataForm: any;
    dataRule: any;
    optionsObj: any;
    currVmodel: any;
    currTableConf: any;
    addTableConf: any;
    tableRows: any;
    tableField579169outerActiveKey: number[];
    tableField579169innerActiveKey: string[];
    tableField579169DefaultExpandAll: boolean;
    selectedtableField579169RowKeys: any[];
    title: string;
    continueText: string;
    allList: any[];
    currIndex: number;
    isContinue: boolean;
    submitType: number;
    showContinueBtn: boolean;
  }

  const emit = defineEmits(['reload']);
  const { hasFormP } = usePermission();
  const getLeftTreeActiveInfo: (() => any) | null = inject('getLeftTreeActiveInfo', null);
  const userStore = useUserStore();
  const userInfo = userStore.getUserInfo;
  const { createMessage, createConfirm } = useMessage();
  const { t } = useI18n();
  const [registerModal, { openModal, setModalProps }] = useModal();
  const formRef = ref<FormInstance>();
  const selectModal = ref(null);

  /* 计算属性 */
  const duration = computed(() => {
    const start = state.dataForm.F_StartTime;
    const end = state.dataForm.F_EndTime;
    if (!start || !end) return { h: 0, m: 0 };

    const diff = dayjs(end).diff(dayjs(start), 'minute'); // 总分钟
    if (diff <= 0) return { h: 0, m: 0 };

    const h = Math.floor(diff / 60);
    const m = diff % 60;
    return { h, m };
  });

  const tableField579169Columns: any[] = computed(() => {
    let list = [
      {
        title: '不良品项',
        dataIndex: 'F_DefectId',
        key: 'F_DefectId',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_DefectId',
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
        width: '',
        fixed: false,
        formP: 'F_Num',
        isSystemControl: false,
      },
    ];
    list = list.filter(o => hasFormP('tableField579169-' + o.formP));
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
  const gettableField579169RowSelection = computed(() => {
    const rowSelection = {
      selectedRowKeys: state.selectedtableField579169RowKeys,
      onChange: (selectedRowKeys: string[]) => {
        state.selectedtableField579169RowKeys = (selectedRowKeys || []).sort().reverse();
      },
    };
    return rowSelection;
  });
  const state = reactive<State>({
    DefectList: [],
    dataForm: {
      id: '',
      F_TaskId: '',
      F_ProcessId: '',
      F_ProdUserId: undefined,
      F_GoodQty: undefined,
      F_GoodNotQty: 0,
      F_StartTime: dayjs(new Date()).startOf(getDateTimeUnit('yyyy-MM-dd')).valueOf(),
      F_EndTime: dayjs(new Date()).startOf(getDateTimeUnit('yyyy-MM-dd')).valueOf(),
      F_WagePrice: 0,
      F_Files: [],
      F_Description: undefined,
      F_SettleStatus: '0',
      F_SettleTime: undefined,
      F_SettleUserId: undefined,
      F_State: '0',
      F_CreatorTime: undefined,
      tableField579169: [],
    },
    tableField579169outerActiveKey: [0],
    tableField579169innerActiveKey: [],
    tableField579169DefaultExpandAll: true,
    selectedtableField579169RowKeys: [],
    dataRule: {
      // F_ProdUserId: [
      //   {
      //     required: true,
      //     message: '请输入生产人员',
      //     trigger: 'change',
      //   },
      // ],
      F_StartTime: [
        {
          validator: (_rule, value) => {
            if (!value) return Promise.resolve();
            const end = state.dataForm.F_EndTime;
            if (end && dayjs(value).isAfter(dayjs(end))) {
              return Promise.reject('开始时间不能晚于结束时间');
            }
            return Promise.resolve();
          },
          trigger: 'change',
        },
      ],
      F_EndTime: [
        {
          validator: (_rule, value) => {
            if (!value) return Promise.resolve();
            const start = state.dataForm.F_StartTime;
            if (start && dayjs(value).isBefore(dayjs(start))) {
              return Promise.reject('结束时间不能早于开始时间');
            }
            return Promise.resolve();
          },
          trigger: 'change',
        },
      ],
    },
    optionsObj: {
      DefectListProps: { label: 'fullName', value: 'id' },
      F_SettleStatusProps: { label: 'fullName', value: 'enCode' },
      F_StateProps: { label: 'fullName', value: 'enCode' },
      tableField579169_F_DefectIdOptions: [],
      tableField579169_F_DefectIdProps: { label: 'F_Name', value: 'F_Id' },
    },
    currVmodel: '',
    currTableConf: {},
    tableRows: {
      tableField579169: {
        enabledmark: undefined,
        F_DefectId: undefined,
        F_Num: undefined,
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
  });
  const { title, continueText, showContinueBtn, dataForm, submitType, dataRule, optionsObj } = toRefs(state);

  defineExpose({ init });

  function init(data) {
    state.submitType = 0;
    state.showContinueBtn = true;
    state.title = '报工--' + data.F_ProcessName;
    setFormProps({ continueLoading: false });
    state.dataForm.id = data.id;
    if (!state.dataForm.id) {
      state.dataForm.F_TaskId = data.F_TaskId;
      state.dataForm.F_ProcessId = data.F_ProcessId;
      state.dataForm.F_GoodQty = data.F_ProdQty;
      state.dataForm.F_ProdUserId = JSON.parse(data.F_ProdUserByIds);
    }
    openModal();
    nextTick(() => {
      getForm().resetFields();
      state.dataForm.tableField579169 = [];
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
        F_TaskId: state.dataForm.F_TaskId,
        F_ProcessId: state.dataForm.F_ProcessId,
        F_ProdUserId: state.dataForm.F_ProdUserId,
        F_GoodQty: state.dataForm.F_GoodQty,
        F_GoodNotQty: 0,
        F_StartTime: dayjs(new Date()).startOf(getDateTimeUnit('yyyy-MM-dd')).valueOf(),
        F_EndTime: dayjs(new Date()).startOf(getDateTimeUnit('yyyy-MM-dd')).valueOf(),
        F_WagePrice: 0,
        F_Files: [],
        F_Description: undefined,
        F_SettleStatus: '0',
        F_SettleTime: undefined,
        F_SettleUserId: undefined,
        F_State: '0',
        F_CreatorTime: undefined,
        tableField579169: [],
      };
      (state.DefectList = []), getDefectListOptions();
      getF_SettleStatusOptions();
      getF_StateOptions();
      gettableField579169_F_DefectIdOptions();
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
      state.DefectList = [];
      for (let i = 0; i < state.dataForm.tableField579169.length; i++) {
        const element = state.dataForm.tableField579169[i];
        element.jnpfId = buildUUID();
        state.DefectList.push(state.dataForm.tableField579169[i].F_DefectId);
      }
      getDefectListOptions();
      getF_SettleStatusOptions();
      getF_StateOptions();
      gettableField579169_F_DefectIdOptions();
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

  function getDefectListOptions() {
    let templateJson = [
      {
        defaultValue: '',
        field: 'processId',
        dataType: 'varchar',
        required: 0,
        fieldName: '',
        relationField: 'F_ProcessId',
        jnpfKey: 'select',
        complexHeaderList: null,
        sourceType: 1,
        isChildren: false,
        IsMultiple: false,
      },
    ];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2014256316289257472', query).then(res => {
      let data = res.data;
      state.optionsObj.DefectListOptions = Array.isArray(data) ? data : [];
    });
  }
  function getF_SettleStatusOptions() {
    getDictionaryDataSelector('2014214471169478656').then(res => {
      state.optionsObj.F_SettleStatusOptions = res.data.list;
    });
  }
  function getF_StateOptions() {
    getDictionaryDataSelector('2014253420491444224').then(res => {
      state.optionsObj.F_StateOptions = res.data.list;
    });
  }
  function gettableField579169_F_DefectIdOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2011648481097289728', query).then(res => {
      let data = res.data;
      state.optionsObj.tableField579169_F_DefectIdOptions = Array.isArray(data) ? data : [];
    });
  }
  function removetableField579169Row(index, showConfirm = false) {
    if (showConfirm) {
      createConfirm({
        iconType: 'warning',
        title: '提示',
        content: '此操作将永久删除该数据, 是否继续?',
        onOk: () => {
          state.dataForm.tableField579169.splice(index, 1);
          state.DefectList.splice(index, 1);
        },
      });
    } else {
      state.dataForm.tableField579169.splice(index, 1);
      state.DefectList.splice(index, 1);
    }
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

  function DefectListBtn(val, option) {
    /* 兜底 */
    if (!Array.isArray(state.dataForm.tableField579169)) {
      state.dataForm.tableField579169 = [];
    }
    const optionIdSet = new Set(option.map(o => o.id));

    /* 1. 删掉已取消勾选的行 */
    state.dataForm.tableField579169 = state.dataForm.tableField579169.filter(item => optionIdSet.has(item.F_DefectId));

    option.forEach(o => {
      const exist = state.dataForm.tableField579169.some(item => item.F_DefectId === o.id);
      if (!exist) {
        const newRow = {
          jnpfId: buildUUID(),
          F_DefectId: o.id,
          F_Num: undefined,
          F_CreatorTime: undefined,
        };
        state.dataForm.tableField579169.push(newRow);
        state.tableField579169innerActiveKey.push(newRow.jnpfId);
      }
    });
  }
</script>
<style scoped>
  .duration-box {
    display: flex;
    align-items: center;
    gap: 6px;
    font-size: 16px;
    color: #262626;
    padding: 8px 12px;
    border-radius: 4px;
  }
  .label {
    margin-right: 8px;
    color: #8c8c8c;
  }
  .value {
    font-weight: 600;
    color: #1890ff;
  }
  .unit {
    font-size: 14px;
    color: #595959;
  }
</style>
