<template>
  <BasicModal v-bind="$attrs" @register="registerModal" width="1200px" :minHeight="100" okText="确定" cancelText="取消" @ok="handleSubmit" :closeFunc="onClose">
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
          <a-col :span="24" class="ant-col-item">
            <a-row>
              <a-col :span="24" class="ant-col-item">
                <a-form-item name="GoodsList" :labelCol="{ style: { width: '100px' } }">
                  <template #label>选择物料</template>
                  <JnpfPopupMultipleSelect
                    v-model:value="state.GoodsList"
                    placeholder="请选择"
                    :templateJson="optionsObj.GoodsListTemplateJson"
                    allowClear
                    field="GoodsList"
                    interfaceId="2012085692393459712"
                    :columnOptions="optionsObj.GoodsListOptions"
                    relationField="F_GoodsName"
                    propsValue="id"
                    :pageSize="20"
                    popupType="dialog"
                    popupTitle="选择数据"
                    popupWidth="1000px"
                    @change="GoodsListBtn"
                    :style="{
                      width: '100%',
                    }"
                    :extraOptions="optionsObj.GoodsList" />
                </a-form-item>
              </a-col>

              <a-col :span="24" class="ant-col-item">
                <a-form-item label="">
                  <a-table
                    :data-source="dataForm.tableField0bb944"
                    :columns="tableField0bb944Columns"
                    size="small"
                    rowKey="jnpfId"
                    :pagination="false"
                    :scroll="{ x: 'max-content' }"
                    :rowSelection="gettableField0bb944RowSelection">
                    <template #headerCell="{ column }">
                      <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                    /></template>
                    <template #bodyCell="{ column, record, index }">
                      <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                      <template v-if="column.key === 'F_GoodsId'">
                        <JnpfSelect
                          v-model:value="record.F_GoodsId"
                          :options="optionsObj.tableField0bb944_F_GoodsIdOptions"
                          :fieldNames="optionsObj.tableField0bb944_F_GoodsIdProps"
                          allowClear
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
                        <JnpfInputNumber
                          v-model:value="record.F_Num"
                          :min="1"
                          :controls="false"
                          :style="{
                            width: '100%',
                            borderColor: record.F_Num > record.F_InventoryNum ? '#ff4d4f' : undefined,
                          }"
                          @change="handleF_NumChange(record)" />
                        <div v-if="record.F_Num > record.F_InventoryNum" style="color: #ff4d4f; font-size: 12px; margin-top: 4px">
                          数量不能大于库存数量 {{ record.F_InventoryNum }}
                        </div>
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
                      <template v-if="column.key === 'F_Description'">
                        <JnpfInput
                          v-model:value="record.F_Description"
                          allowClear
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
                          <!-- <a-button class="action-btn" type="link" @click="copytableField0bb944Row(index)" size="small">{{
                                t('common.copyText', '复制')
                              }}</a-button> -->
                          <a-button class="action-btn" type="link" color="error" @click="removetableField0bb944Row(index, true)" size="small">{{
                            t('common.delText', '删除')
                          }}</a-button>
                        </a-space>
                      </template>
                    </template>
                  </a-table>
                  <a-space class="input-table-footer-btn">
                    <!-- <a-button type="primary" preIcon="icon-ym icon-ym-btn-add" @click="addtableField0bb944Row">{{ t('common.add1Text', '添加') }}</a-button> -->
                    <!-- <a-button type="danger" preIcon="icon-ym icon-ym-btn-clearn" @click="batchRemovetableField0bb944Row(true)">{{
                          t('common.batchDelText', '批量删除')
                        }}</a-button> -->
                  </a-space>
                </a-form-item>
              </a-col>
            </a-row>
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
  import dayjs from 'dayjs';
  import { cloneDeep } from 'lodash-es';
  import { buildUUID } from '@/utils/uuid';
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
    tableField0bb944outerActiveKey: number[];
    tableField0bb944innerActiveKey: string[];
    tableField0bb944DefaultExpandAll: boolean;
    selectedtableField0bb944RowKeys: any[];
    title: string;
    continueText: string;
    allList: any[];
    currIndex: number;
    isContinue: boolean;
    submitType: number;
    showContinueBtn: boolean;
  }

  const emit = defineEmits(['reload', 'submit']);
  const getLeftTreeActiveInfo: (() => any) | null = inject('getLeftTreeActiveInfo', null);
  const userStore = useUserStore();
  const userInfo = userStore.getUserInfo;
  const { createMessage, createConfirm } = useMessage();
  const { t } = useI18n();
  const [registerModal, { openModal, setModalProps }] = useModal();
  const formRef = ref<FormInstance>();
  const selectModal = ref(null);
  const tableField0bb944Columns: any[] = computed(() => {
    let list = [
      {
        title: '商品名',
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
    let columns = [noColumn, ...columnList, actionColumn];
    const leftFixedList = columns.filter(o => o.fixed === 'left');
    const rightFixedList = columns.filter(o => o.fixed === 'right');
    const noFixedList = columns.filter(o => o.fixed !== 'left' && o.fixed !== 'right');
    return [...leftFixedList, ...noFixedList, ...rightFixedList];
  });
  const gettableField0bb944RowSelection = computed(() => {
    const rowSelection = {
      selectedRowKeys: state.selectedtableField0bb944RowKeys,
      onChange: (selectedRowKeys: string[]) => {
        state.selectedtableField0bb944RowKeys = (selectedRowKeys || []).sort().reverse();
      },
    };
    return rowSelection;
  });
  const state = reactive<State>({
    GoodsList: [],
    dataForm: {
      id: '',
      jnpfId: '',
      F_ProcessId: undefined,
      F_WagePrice: undefined,
      F_StdHour: 0,
      F_StdMinute: 0,
      F_StdSecond: 0,
      F_PriceType: '0',
      F_UnitRatioProd: undefined,
      F_UnitRatioReport: undefined,
      F_Files: [],
      F_Description: undefined,
      F_CreatorTime: undefined,
      F_IsOutsource: 0,
      F_IsQc: 0,
      F_DefectHandle: 0,
      F_TaskTimer: 0,
      F_UnitRatioBase: undefined,
      tableField0bb944: [],
    },
    tableField0bb944outerActiveKey: [0],
    tableField0bb944innerActiveKey: [],
    tableField0bb944DefaultExpandAll: true,
    selectedtableField0bb944RowKeys: [],
    dataRule: {
      F_ProcessId: [
        {
          required: true,
          message: '请输入工序',
          trigger: 'blur',
        },
      ],
      F_PriceType: [
        {
          required: true,
          message: '请输入计价方式',
          trigger: 'change',
        },
      ],
    },
    optionsObj: {
      F_PriceTypeProps: { label: 'fullName', value: 'enCode' },
      F_UnitRatioBaseProps: { label: 'fullName', value: 'enCode' },
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
        // {
        //   value: 'F_CategoryId',
        //   label: '商品类别',
        // },
        {
          value: 'F_Unit',
          label: '单位',
        },
        {
          value: 'F_InventoryNum',
          label: '库存',
        },
        {
          value: 'F_InspectRule',
          label: '检验规范',
        },
        {
          value: 'F_Remark',
          label: '备注',
        },
      ],
      GoodsListTemplateJson: [],
      GoodsList: [],
      tableField0bb944_F_GoodsIdOptions: [],
      tableField0bb944_F_GoodsIdProps: { label: 'F_GoodsName', value: 'id' },
    },
    currVmodel: '',
    currTableConf: {},
    tableRows: {
      tableField0bb944: {
        enabledmark: undefined,
        F_GoodsId: undefined,
        F_Specification: undefined,
        F_Unit: undefined,
        F_Num: undefined,
        F_Description: undefined,
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
    state.title = '物料清单';
    setFormProps({ continueLoading: false });
    // 直接赋值，别再 reset
    state.dataForm = { ...data };
    state.dataForm.F_IsOutsource = Number(state.dataForm.F_IsOutsource);
    state.dataForm.F_IsQc = Number(state.dataForm.F_IsQc);
    state.dataForm.F_DefectHandle = Number(state.dataForm.F_DefectHandle);
    state.dataForm.F_TaskTimer = Number(state.dataForm.F_TaskTimer);
    state.GoodsList = [];
    if (!Array.isArray(state.dataForm.tableField0bb944)) {
      state.dataForm.tableField0bb944 = [];
    }
    for (var i = 0; i < state.dataForm.tableField0bb944.length; i++) {
      state.GoodsList.push(state.dataForm.tableField0bb944[i].F_GoodsId);
    }
    openModal();
    initData();
  }
  function initData() {
    changeLoading(true);
    getF_PriceTypeOptions();
    getF_UnitRatioBaseOptions();
    gettableField0bb944_F_GoodsIdOptions();
    changeLoading(false);
  }
  function getForm() {
    const form = unref(formRef);
    if (!form) {
      throw new Error('form is null!');
    }
    return form;
  }
  async function handleSubmit() {
    try {
      const values = await getForm()?.validate();
      if (!values) return;

      // 验证数量不能大于库存数量
      const invalidRows = state.dataForm.tableField0bb944.filter(item => item.F_Num && item.F_InventoryNum && item.F_Num > item.F_InventoryNum);
      if (invalidRows.length > 0) {
        const errorMsg = invalidRows
          .map(item => {
            const goods = optionsObj.tableField0bb944_F_GoodsIdOptions.find(g => g.id === item.F_GoodsId);
            const goodsName = goods ? goods.F_GoodsName : item.F_GoodsId;
            return `${goodsName}：数量 ${item.F_Num} 大于库存 ${item.F_InventoryNum}`;
          })
          .join('；');
        createMessage.error(`物料清单验证失败：${errorMsg}`);
        return;
      }

      setFormProps({ confirmLoading: true });
      /* 这里原来是空的，现在把数据抛出去 */
      emit('submit', toRaw(state.dataForm));

      setFormProps({ confirmLoading: false });

      setFormProps({ open: false });
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

  // 处理数量变化
  function handleF_NumChange(record) {
    // 触发响应式更新,确保错误提示及时显示
    const index = state.dataForm.tableField0bb944.findIndex(item => item.jnpfId === record.jnpfId);
    if (index > -1) {
      state.dataForm.tableField0bb944[index] = { ...record };
    }
  }
  function addForSelect(data) {
    for (let i = 0; i < data.length; i++) {
      let item = { ...state.tableRows[state.currVmodel], ...data[i], jnpfId: buildUUID() };
      state.dataForm[state.currVmodel].push(cloneDeep(item));
      if (state[state.currVmodel + 'DefaultExpandAll']) state[state.currVmodel + 'innerActiveKey'].push(item.jnpfId);
    }
  }

  function gettableField0bb944_F_GoodsIdOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2012085692393459712', query).then(res => {
      let data = res.data;
      state.optionsObj.tableField0bb944_F_GoodsIdOptions = Array.isArray(data) ? data : [];
    });
  }
  function removetableField0bb944Row(index, showConfirm = false) {
    if (showConfirm) {
      createConfirm({
        iconType: 'warning',
        title: '提示',
        content: '此操作将永久删除该数据, 是否继续?',
        onOk: () => {
          state.dataForm.tableField0bb944.splice(index, 1);
          state.GoodsList.splice(index, 1);
        },
      });
    } else {
      state.dataForm.tableField0bb944.splice(index, 1);
      state.GoodsList.splice(index, 1);
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
    if (!Array.isArray(state.dataForm.tableField0bb944)) {
      state.dataForm.tableField0bb944 = [];
    }

    /* 1. 把 option 里的 id 做成 Set，方便比对 */
    const optionIdSet = new Set(option.map(o => o.id));

    /* 2. 删除本地已不在 option 里的行 */
    state.dataForm.tableField0bb944 = state.dataForm.tableField0bb944.filter(item => optionIdSet.has(item.F_GoodsId));

    /* 3. 把 option 里本地还没有的行加进来 */
    option.forEach(o => {
      const exist = state.dataForm.tableField0bb944.findIndex(item => item.F_GoodsId === o.id) !== -1;
      if (!exist) {
        const newRow = {
          jnpfId: buildUUID(),
          F_GoodsId: o.id,
          F_Specification: o.F_Specification,
          F_Unit: o.F_Unit,
          F_InventoryNum: o.F_InventoryNum,
          F_Num: 1,
          F_Description: undefined,
          F_CreatorTime: undefined,
        };
        state.dataForm.tableField0bb944.push(newRow);
        state.tableField0bb944innerActiveKey.push(newRow.jnpfId);
      }
    });
  }
</script>
