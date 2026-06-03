<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    width="1300px"
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_PlanNo')">
            <a-form-item name="F_PlanNo" :labelCol="{ style: { width: '100px' } }">
              <template #label>计划编号</template>
              <JnpfInput
                v-model:value="dataForm.F_PlanNo"
                placeholder="请填写，忽略将自动生成"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_PlanName')">
            <a-form-item name="F_PlanName" :labelCol="{ style: { width: '100px' } }">
              <template #label>计划名称</template>
              <JnpfInput
                v-model:value="dataForm.F_PlanName"
                placeholder="请输入"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_DeliveryDate')">
            <a-form-item name="F_DeliveryDate" :labelCol="{ style: { width: '100px' } }">
              <template #label>交货日期</template>
              <JnpfDatePicker
                v-model:value="dataForm.F_DeliveryDate"
                placeholder="请选择"
                format="yyyy-MM-dd"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_Description')">
            <a-form-item name="F_Description" :labelCol="{ style: { width: '100px' } }">
              <template #label>备注</template>
              <JnpfTextarea
                v-model:value="dataForm.F_Description"
                placeholder="请输入"
                :maxlength="500"
                allowClear
                :autoSize="{
                  minRows: 2,
                  maxRows: 2,
                }"
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>

          <a-col :span="24" class="ant-col-item">
            <a-form-item name="GoodsList" :labelCol="{ style: { width: '100px' } }">
              <template #label>选择商品</template>
              <JnpfPopupMultipleSelect
                v-model:value="state.GoodsList"
                placeholder="请选择"
                :templateJson="optionsObj.GoodsListTemplateJson"
                allowClear
                field="GoodsList"
                interfaceId="2029803158963884032"
                :columnOptions="optionsObj.GoodsListOptions"
                relationField="F_GoodsName"
                propsValue="id"
                :pageSize="20"
                popupType="dialog"
                popupTitle="选择数据"
                popupWidth="1000px"
                @change="GoodsListBtn"
                hasPage
                :style="{
                  width: '100%',
                }"
                :extraOptions="optionsObj.GoodsList" />
            </a-form-item>
          </a-col>

          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="商品" :bordered="false" />
              <a-table
                :data-source="dataForm.tableField2152f8"
                :columns="tableField2152f8Columns"
                size="small"
                rowKey="jnpfId"
                :pagination="listPagination"
                :scroll="{ x: 'max-content' }"
                :rowSelection="gettableField2152f8RowSelection">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                  <template v-if="column.key === 'F_GoodsId'">
                    <JnpfSelect
                      v-model:value="record.F_GoodsId"
                      @change="F_GoodsIdTableChange(index)"
                      placeholder=""
                      :options="optionsObj.tableField2152f8_F_GoodsIdOptions"
                      :fieldNames="optionsObj.tableField2152f8_F_GoodsIdProps"
                      allowClear
                      showSearch
                      disabled
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_GoodsNo'">
                    <JnpfInput
                      v-model:value="record.F_GoodsNo"
                      :min="1"
                      :controls="false"
                      disabled
                      :style="{
                        width: '100%',
                      }" />
                  </template>

                  <template v-if="column.key === 'F_Specification'">
                    <JnpfInput
                      v-model:value="record.F_Specification"
                      :min="1"
                      :controls="false"
                      disabled
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_Num'">
                    <a-space>
                      <JnpfInputNumber
                        v-model:value="record.F_Num"
                        placeholder=""
                        :min="1"
                        :controls="false"
                        :style="{
                          width: '100%',
                        }" />
                      <span>{{ record.F_NumUnit }}</span>
                    </a-space>
                  </template>
                  <template v-if="column.key === 'F_BOMId'">
                    <JnpfSelect
                      v-model:value="record.F_BOMId"
                      placeholder=""
                      :options="record.tableField2152f8_F_BOMIdOptions"
                      :fieldNames="optionsObj.tableField2152f8_F_BOMIdProps"
                      allowClear
                      showSearch
                      :style="{
                        width: '100%',
                      }" />
                  </template>

                  <template v-if="column.key === 'F_Unit'">
                    <JnpfInput
                      v-model:value="record.F_Unit"
                      :min="1"
                      :controls="false"
                      disabled
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_Remark'">
                    <JnpfInput
                      v-model:value="record.F_Remark"
                      allowClear
                      disabled
                      :style="{
                        width: '100%',
                      }"
                      :showCount="false" />
                  </template>
                  <template v-if="column.key === 'action'">
                    <a-space>
                      <!-- <a-button class="action-btn" type="link" @click="copytableField2152f8Row(index)" size="small">{{
                        t('common.copyText', '复制')
                      }}</a-button> -->
                      <a-button class="action-btn" type="link" color="error" @click="removetableField2152f8Row(index, true)" size="small">{{
                        t('common.delText', '删除')
                      }}</a-button>
                    </a-space>
                  </template>
                </template>
              </a-table>
              <a-space class="input-table-footer-btn">
                <!-- <a-button type="primary" preIcon="icon-ym icon-ym-btn-add" @click="addtableField2152f8Row">{{ t('common.add1Text', '添加') }}</a-button>
                <a-button type="danger" preIcon="icon-ym icon-ym-btn-clearn" @click="batchRemovetableField2152f8Row(true)">{{
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
</template>
<script lang="ts" setup>
  import { create, update, getInfo } from './helper/api';
  import { getGoodsUnit } from '@/views/Subcode/aGoods/helper/api';
  import { reactive, toRefs, nextTick, ref, unref, computed, toRaw, inject } from 'vue';
  import { BasicModal, useModal } from '@/components/Modal';
  import SelectModal from '@/components/CommonModal/src/SelectModal.vue';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useI18n } from '@/hooks/web/useI18n';
  import { useUserStore } from '@/store/modules/user';
  import type { FormInstance } from 'ant-design-vue';
  import { thousandsFormat } from '@/utils/jnpf';
  import { getTimeUnit, getDateTimeUnit } from '@/utils/jnpf';
  import { getDataInterfaceRes } from '@/api/systemData/dataInterface';
  import dayjs from 'dayjs';
  import { cloneDeep } from 'lodash-es';
  import { buildUUID } from '@/utils/uuid';
  import { CaretRightOutlined } from '@ant-design/icons-vue';
  import { systemComponentsList } from '@/components/FormGenerator/src/helper/config';
  import { usePermission } from '@/hooks/web/usePermission';

  interface State {
    listPageSize: number;
    listCurrentPage: number;
    GoodsList: any[];
    dataForm: any;
    dataRule: any;
    optionsObj: any;
    currVmodel: any;
    currTableConf: any;
    addTableConf: any;
    tableRows: any;
    tableField2152f8outerActiveKey: number[];
    tableField2152f8innerActiveKey: string[];
    tableField2152f8DefaultExpandAll: boolean;
    selectedtableField2152f8RowKeys: any[];
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
  const tableField2152f8Columns: any[] = computed(() => {
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
        width: '',
        fixed: false,
        formP: 'F_GoodsId',
        isSystemControl: false,
      },
      {
        title: '商品编号',
        dataIndex: 'F_GoodsNo',
        key: 'F_GoodsNo',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_GoodsNo',
        isSystemControl: false,
      },
      {
        title: '规格',
        dataIndex: 'F_Specification',
        key: 'F_Specification',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Specification',
        isSystemControl: false,
      },
      {
        title: '计划数量',
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
      {
        title: '单位',
        dataIndex: 'F_Unit',
        key: 'F_Unit',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Unit',
        isSystemControl: false,
      },
      {
        title: '商品BOM',
        dataIndex: 'F_BOMId',
        key: 'F_BOMId',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '130px',
        fixed: false,
        formP: 'F_BOMId',
        isSystemControl: false,
      },
      {
        title: '备注',
        dataIndex: 'F_Remark',
        key: 'F_Remark',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Remark',
        isSystemControl: false,
      },
    ];
    list = list.filter(o => hasFormP('SPLB-' + o.formP));
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
  const gettableField2152f8RowSelection = computed(() => {
    const rowSelection = {
      selectedRowKeys: state.selectedtableField2152f8RowKeys,
      onChange: (selectedRowKeys: string[]) => {
        state.selectedtableField2152f8RowKeys = (selectedRowKeys || []).sort().reverse();
      },
    };
    return rowSelection;
  });
  const state = reactive<State>({
    listPageSize: 20,
    listCurrentPage: 1,
    GoodsList: [],
    dataForm: {
      id: '',
      F_PlanNo: undefined,
      F_PlanName: undefined,
      F_DeliveryDate: undefined,
      F_Description: undefined,
      F_State: undefined,
      F_CreatorTime: undefined,
      F_CreatorUserId: undefined,
      tableField2152f8: [],
    },
    tableField2152f8outerActiveKey: [0],
    tableField2152f8innerActiveKey: [],
    tableField2152f8DefaultExpandAll: true,
    selectedtableField2152f8RowKeys: [],
    dataRule: {
      F_PlanName: [
        {
          required: true,
          message: '请输入计划名称',
          trigger: 'blur',
        },
      ],
    },
    optionsObj: {
      GoodsListOptions: [
        {
          value: 'F_GoodsName',
          label: '商品名',
        },
        {
          value: 'F_GoodsNo',
          label: '商品编号',
        },
        {
          value: 'F_Specification',
          label: '商品规格',
        },
        {
          value: 'F_Type',
          label: '商品类型',
        },
        // {
        //   value: 'F_CategoryId',
        //   label: '商品类别',
        // },
        // {
        //   value: 'F_Unit',
        //   label: '单位',
        // },
        // {
        //   value: 'F_InspectRule',
        //   label: '检验规范',
        // },
        {
          value: 'F_Remark',
          label: '备注',
        },
      ],
      GoodsListTemplateJson: [],
      GoodsList: [],
      F_StateOptions: [
        { fullName: '已分析', id: '1' },
        { fullName: '未分析', id: '0' },
      ],
      F_StateProps: { label: 'fullName', value: 'id' },
      tableField2152f8_F_GoodsIdOptions: [],
      tableField2152f8_F_GoodsIdProps: { label: 'F_GoodsName', value: 'id' },
      tableField2152f8_F_BOMIdProps: { label: 'F_BomName', value: 'id' },
    },
    currVmodel: '',
    currTableConf: {},
    tableRows: {
      tableField2152f8: {
        enabledmark: undefined,
        F_GoodsId: undefined,
        F_Num: undefined,
        F_BOMId: undefined,
        tableField2152f8_F_BOMIdOptions: [],
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

  const getPrevDisabled = computed(() => state.currIndex === 0);
  const getNextDisabled = computed(() => state.currIndex === state.allList.length - 1);

  defineExpose({ init });

  function init(data) {
    state.submitType = 0;
    state.showContinueBtn = true;
    state.title = !data.id ? '新增' : '编辑';
    setFormProps({ continueLoading: false });
    state.dataForm.id = data.id;
    openModal();
    nextTick(() => {
      getForm().resetFields();
      state.dataForm.tableField2152f8 = [];
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
        F_PlanNo: undefined,
        F_PlanName: undefined,
        F_DeliveryDate: undefined,
        F_Description: undefined,
        F_State: undefined,
        F_CreatorTime: undefined,
        F_CreatorUserId: undefined,
        tableField2152f8: [],
      };
      state.GoodsList = [];
      gettableField2152f8_F_GoodsIdOptions();
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
      for (let i = 0; i < state.dataForm.tableField2152f8.length; i++) {
        const element = state.dataForm.tableField2152f8[i];
        element.jnpfId = buildUUID();

        state.GoodsList = [];
        for (let i = 0; i < state.dataForm.tableField2152f8.length; i++) {
          const element = state.dataForm.tableField2152f8[i];
          element.jnpfId = buildUUID();
          state.GoodsList.push(state.dataForm.tableField2152f8[i].F_GoodsId);
          gettableField2152f8_F_BOMIdOptions(i);
        }
      }
      gettableField2152f8_F_GoodsIdOptions();
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
  function F_GoodsIdTableChange(i) {
    state.dataForm.tableField2152f8[i].F_BOMId = undefined;
    gettableField2152f8_F_BOMIdOptions(i);
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
  function gettableField2152f8_F_GoodsIdOptions() {
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
    getDataInterfaceRes('2029803158963884032', query).then(res => {
      let data = res.data;
      state.optionsObj.tableField2152f8_F_GoodsIdOptions = Array.isArray(data) ? data : [];
    });
  }
  function gettableField2152f8_F_BOMIdOptions(i?) {
    let templateJson = [
      {
        defaultValue: '',
        field: 'goodsId',
        dataType: 'varchar',
        required: 0,
        fieldName: '合同ID',
        relationField: 'tableField2152f8-F_GoodsId',
        jnpfKey: 'select',
        complexHeaderList: null,
        sourceType: 1,
        isChildren: false,
        IsMultiple: false,
      },
    ];
    let query = {
      paramList: getParamList(templateJson, state.dataForm, i),
    };
    getDataInterfaceRes('2013188646957617152', query).then(res => {
      let data = res.data;
      state.dataForm.tableField2152f8[i].tableField2152f8_F_BOMIdOptions = Array.isArray(data) ? data : [];
    });
  }
  function removetableField2152f8Row(index, showConfirm = false) {
    if (showConfirm) {
      createConfirm({
        iconType: 'warning',
        title: '提示',
        content: '此操作将永久删除该数据, 是否继续?',
        onOk: () => {
          state.dataForm.tableField2152f8.splice(index, 1);
          state.GoodsList.splice(index, 1);
        },
      });
    } else {
      state.dataForm.tableField2152f8.splice(index, 1);
      state.GoodsList.splice(index, 1);
    }
  }
  function copytableField2152f8Row(index) {
    let item = cloneDeep(state.dataForm.tableField2152f8[index]);
    let copyData = {};
    for (let i = 0; i < unref(tableField2152f8Columns).length; i++) {
      const cur = unref(tableField2152f8Columns)[i];
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
    state.dataForm.tableField2152f8.push(copyItem);
    state.tableField2152f8innerActiveKey.push(copyItem.jnpfId);
  }
  function batchRemovetableField2152f8Row(showConfirm = false) {
    if (!state.selectedtableField2152f8RowKeys.length) return createMessage.error('请选择一条数据');
    const handleBatchRemove = () => {
      state.dataForm.tableField2152f8 = state.dataForm.tableField2152f8.filter(o => !state.selectedtableField2152f8RowKeys.includes(o.jnpfId));
      nextTick(() => {
        state.selectedtableField2152f8RowKeys = [];
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
    /* 兜底保证是数组 */
    if (!Array.isArray(state.dataForm.tableField2152f8)) {
      state.dataForm.tableField2152f8 = [];
    }

    /* 1. 把 option 里的 id 做成 Set，方便比对 */
    const optionIdSet = new Set(option.map(o => o.id));

    /* 2. 删除本地已不在 option 里的行 */
    state.dataForm.tableField2152f8 = state.dataForm.tableField2152f8.filter(item => optionIdSet.has(item.F_GoodsId));

    /* 3. 把 option 里本地还没有的行加进来 */
    option.forEach((o, i) => {
      const exist = state.dataForm.tableField2152f8.findIndex(item => item.F_GoodsId === o.id) !== -1;
      if (!exist) {
        getGoodsUnit(o.id).then(res => {
          const newRow = {
            jnpfId: buildUUID(),
            F_GoodsId: o.id,
            F_GoodsNo: o.F_GoodsNo,
            F_Specification: o.F_Specification,
            F_Unit: res?.data,
            F_Num: undefined,
            F_NumUnit: res.data?.split('/').length > 1 ? res.data?.split('/')[1] : res.data?.split('/')[0],
            F_BOMId: undefined,
            tableField2152f8_F_BOMIdOptions: [],
            F_Remark: o.F_Remark,
            F_CreatorTime: undefined,
          };
          state.dataForm.tableField2152f8.push(newRow);
          state.tableField2152f8innerActiveKey.push(newRow.jnpfId);
        });
      }
      gettableField2152f8_F_BOMIdOptions(i);
    });
  }
  const listPagination = computed(() => ({
    current: state.listCurrentPage,
    pageSize: state.listPageSize,
    size: 'small',
    showSizeChanger: true,
    pageSizeOptions: [20, 50, 100],
    showTotal: (total: number) => `共 ${total} 条`,
    onChange: (page: number) => {
      state.listCurrentPage = page;
    },
    onShowSizeChange: (_current: number, size: number) => {
      state.listPageSize = size;
      state.listCurrentPage = 1; // 切换页大小时回到第1页
    },
  }));
</script>
<!-- 全局样式：分页下拉菜单宽度 -->
<style lang="less">
  /* 分页下拉框触发器和弹出菜单都加宽 */
  .ant-pagination-options-size-changer {
    min-width: 100px;
    .ant-select {
      width: 100%;
    }
  }
</style>
