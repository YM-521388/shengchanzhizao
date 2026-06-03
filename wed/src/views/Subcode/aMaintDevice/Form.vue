<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    width="70%"
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
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_DeviceImages')">
            <a-form-item name="F_DeviceImages" :labelCol="{ style: { width: '100px' } }">
              <template #label>设备图片</template>
              <JnpfUploadImg
                v-model:value="dataForm.F_DeviceImages"
                pathType="selfPath"
                :isAccount="-1"
                folder=""
                sizeUnit="MB"
                :fileSize="10"
                :sortRule="['1', '2']"
                timeFormat="YYYYMMDD"
                showType="card"
                buttonText="点击上传" />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_DeviceNo')">
            <a-form-item name="F_DeviceNo" :labelCol="{ style: { width: '100px' } }">
              <template #label>设备编号</template>
              <JnpfInput
                v-model:value="dataForm.F_DeviceNo"
                placeholder="请填写，忽略将自动生成"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_DeviceName')">
            <a-form-item name="F_DeviceName" :labelCol="{ style: { width: '100px' } }">
              <template #label>设备名称</template>
              <JnpfInput
                v-model:value="dataForm.F_DeviceName"
                placeholder="请输入"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_DeviceSpec')">
            <a-form-item name="F_DeviceSpec" :labelCol="{ style: { width: '100px' } }">
              <template #label>设备规格</template>
              <JnpfInput
                v-model:value="dataForm.F_DeviceSpec"
                placeholder="请输入"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_DeviceStatus')">
            <a-form-item name="F_DeviceStatus" :labelCol="{ style: { width: '100px' } }">
              <template #label>设备状态</template>
              <JnpfSelect
                v-model:value="dataForm.F_DeviceStatus"
                placeholder="请选择"
                :options="optionsObj.F_DeviceStatusOptions"
                :fieldNames="optionsObj.F_DeviceStatusProps"
                allowClear
                showSearch
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_DeviceType')">
            <a-form-item name="F_DeviceType" :labelCol="{ style: { width: '100px' } }">
              <template #label>设备类别</template>
              <JnpfCascader
                v-model:value="dataForm.F_DeviceType"
                placeholder="请选择"
                :options="optionsObj.F_DeviceTypeOptions"
                :fieldNames="optionsObj.F_DeviceTypeProps"
                allowClear
                showSearch
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_GroupId')">
            <a-form-item name="F_GroupId" :labelCol="{ style: { width: '100px' } }">
              <template #label>分组</template>
              <JnpfGroupSelect
                v-model:value="dataForm.F_GroupId"
                placeholder="请选择"
                selectType="all"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_DeviceUsersId')">
            <a-form-item name="F_DeviceUsersId" :labelCol="{ style: { width: '100px' } }">
              <template #label>设备负责人</template>
              <JnpfUserSelect
                v-model:value="dataForm.F_DeviceUsersId"
                placeholder="请选择"
                selectType="all"
                multiple
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_WorkshopId')">
            <a-form-item name="F_WorkshopId" :labelCol="{ style: { width: '100px' } }">
              <template #label>车间</template>
              <JnpfCascader
                v-model:value="dataForm.F_WorkshopId"
                placeholder="请选择"
                :options="optionsObj.F_WorkshopIdOptions"
                :fieldNames="optionsObj.F_WorkshopIdProps"
                allowClear
                showSearch
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_LineId')">
            <a-form-item name="F_LineId" :labelCol="{ style: { width: '100px' } }">
              <template #label>产线</template>
              <JnpfSelect
                v-model:value="dataForm.F_LineId"
                placeholder="请选择"
                :options="optionsObj.F_LineIdOptions"
                :fieldNames="optionsObj.F_LineIdProps"
                allowClear
                showSearch
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_EnabledMark')">
            <a-form-item name="F_EnabledMark" :labelCol="{ style: { width: '100px' } }">
              <template #label>启用状态</template>
              <JnpfSwitch v-model:value="dataForm.F_EnabledMark" />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="物料清单" :bordered="false" />
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
              <a-table
                :data-source="dataForm.tableField72c841"
                :columns="tableField72c841Columns"
                size="small"
                rowKey="jnpfId"
                :pagination="false"
                :scroll="{ x: 'max-content' }"
                :rowSelection="gettableField72c841RowSelection">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                  <template v-if="column.key === 'F_GoodsId'">
                    <JnpfSelect
                      v-model:value="record.F_GoodsId"
                      placeholder=""
                      :options="optionsObj.tableField72c841_F_GoodsIdOptions"
                      :fieldNames="optionsObj.tableField72c841_F_GoodsIdProps"
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
                      placeholder="请输入"
                      :min="1"
                      :precision="0"
                      :controls="false"
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
                  <template v-if="column.key === 'F_InventoryNum'">
                    <JnpfInput
                      v-model:value="record.F_InventoryNum"
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
                      placeholder="请输入"
                      :maxlength="500"
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
                      <a-button class="action-btn" type="link" color="error" @click="removetableField72c841Row(index, true)" size="small">{{
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
  import { thousandsFormat } from '@/utils/jnpf';
  import { getTimeUnit, getDateTimeUnit } from '@/utils/jnpf';
  import { getDictionaryDataSelector } from '@/api/systemData/dictionary';
  import { getDataInterfaceRes } from '@/api/systemData/dataInterface';
  import dayjs from 'dayjs';
  import { cloneDeep } from 'lodash-es';
  import { buildUUID } from '@/utils/uuid';
  import { usePermission } from '@/hooks/web/usePermission';

  interface State {
    GoodsList: any[];
    dataForm: any;
    dataRule: any;
    optionsObj: any;
    currVmodel: any;
    currTableConf: any;
    addTableConf: any;
    tableRows: any;
    tableField72c841outerActiveKey: number[];
    tableField72c841innerActiveKey: string[];
    tableField72c841DefaultExpandAll: boolean;
    selectedtableField72c841RowKeys: any[];
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
  const tableField72c841Columns: any[] = computed(() => {
    let list = [
      {
        title: '物料',
        dataIndex: 'F_GoodsId',
        key: 'F_GoodsId',
        tipLabel: '',
        required: true,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_GoodsId',
        isSystemControl: false,
      },
      {
        title: '物料规格',
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
        required: true,
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
        title: '当前库存',
        dataIndex: 'F_InventoryNum',
        key: 'F_InventoryNum',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_InventoryNum',
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
    list = list.filter(o => hasFormP('tableField72c841-' + o.formP));
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
  const gettableField72c841RowSelection = computed(() => {
    const rowSelection = {
      selectedRowKeys: state.selectedtableField72c841RowKeys,
      onChange: (selectedRowKeys: string[]) => {
        state.selectedtableField72c841RowKeys = (selectedRowKeys || []).sort().reverse();
      },
    };
    return rowSelection;
  });
  const state = reactive<State>({
    GoodsList: [],
    dataForm: {
      id: '',
      F_DeviceNo: undefined,
      F_DeviceName: undefined,
      F_DeviceSpec: undefined,
      F_DeviceStatus: undefined,
      F_DeviceType: [],
      F_GroupId: '2016346530658783232',
      F_DeviceUsersId: [],
      F_WorkshopId: [],
      F_LineId: undefined,
      F_DeviceQrCode: undefined,
      F_EnabledMark: 1,
      F_DeviceImages: [],
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
      tableField72c841: [],
    },
    tableField72c841outerActiveKey: [0],
    tableField72c841innerActiveKey: [],
    tableField72c841DefaultExpandAll: true,
    selectedtableField72c841RowKeys: [],
    dataRule: {
      F_DeviceName: [
        {
          required: true,
          message: '请输入设备名称',
          trigger: 'blur',
        },
      ],
      F_DeviceStatus: [
        {
          required: true,
          message: '请输入设备状态',
          trigger: 'change',
        },
      ],
      F_DeviceType: [
        {
          required: true,
          message: '请输入设备类别',
          trigger: 'change',
        },
      ],
      F_GroupId: [
        {
          required: true,
          message: '请输入分组',
          trigger: 'change',
        },
      ],
      F_DeviceUsersId: [
        {
          required: true,
          message: '请输入设备负责人',
          trigger: 'change',
        },
      ],
      F_EnabledMark: [
        {
          required: true,
          message: '请输入启用状态',
          trigger: 'change',
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
        // {
        //   value: 'F_CategoryId',
        //   label: '商品类别',
        // },
        {
          value: 'F_Unit',
          label: '单位',
        },
        {
          value: 'F_InspectRule',
          label: '检验规范',
        },
        {
          value: 'F_InventoryNum',
          label: '库存',
        },
        {
          value: 'F_Remark',
          label: '备注',
        },
      ],
      GoodsListTemplateJson: [],
      GoodsList: [],
      F_DeviceStatusProps: { label: 'fullName', value: 'enCode' },
      F_DeviceTypeProps: {
        label: 'F_Name',
        value: 'F_Id',
        children: 'children',
      },
      F_WorkshopIdProps: {
        label: 'F_Name',
        value: 'F_Id',
        children: 'children',
      },
      F_LineIdProps: { label: 'fullName', value: 'enCode' },
      tableField72c841_F_GoodsIdOptions: [],
      tableField72c841_F_GoodsIdProps: { label: 'F_GoodsName', value: 'id' },
    },
    currVmodel: '',
    currTableConf: {},
    tableRows: {
      tableField72c841: {
        enabledmark: undefined,
        F_GoodsId: undefined,
        F_Num: 1,
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
    state.title = !data.id ? '新增' : '编辑';
    setFormProps({ continueLoading: false });
    state.dataForm.id = data.id;
    openModal();
    nextTick(() => {
      getForm().resetFields();
      state.dataForm.tableField72c841 = [];
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
        F_DeviceNo: undefined,
        F_DeviceName: undefined,
        F_DeviceSpec: undefined,
        F_DeviceStatus: undefined,
        F_DeviceType: [],
        F_GroupId: '2016346530658783232',
        F_DeviceUsersId: [],
        F_WorkshopId: [],
        F_LineId: undefined,
        F_DeviceQrCode: undefined,
        F_EnabledMark: 1,
        F_DeviceImages: [],
        F_CreatorUserId: undefined,
        F_CreatorTime: undefined,
        tableField72c841: [],
      };
      state.GoodsList = [];
      getF_DeviceStatusOptions();
      getF_DeviceTypeOptions();
      getF_WorkshopIdOptions();
      getF_LineIdOptions();
      gettableField72c841_F_GoodsIdOptions();
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
      state.GoodsList = [];
      for (let i = 0; i < state.dataForm.tableField72c841.length; i++) {
        const element = state.dataForm.tableField72c841[i];
        element.jnpfId = buildUUID();
        state.GoodsList.push(state.dataForm.tableField72c841[i].F_GoodsId);
      }
      getF_DeviceStatusOptions();
      getF_DeviceTypeOptions();
      getF_WorkshopIdOptions();
      getF_LineIdOptions();
      gettableField72c841_F_GoodsIdOptions();
      changeLoading(false);
    });
  }
  async function handleSubmit() {
    try {
      const values = await getForm()?.validate();
      if (!values) return;
      if (!tableField72c841Exist()) return;
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
  function getF_DeviceStatusOptions() {
    getDictionaryDataSelector('2011620395194650624').then(res => {
      state.optionsObj.F_DeviceStatusOptions = res.data.list;
    });
  }
  function getF_DeviceTypeOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2011621811959238656', query).then(res => {
      let data = res.data;
      state.optionsObj.F_DeviceTypeOptions = Array.isArray(data) ? data : [];
    });
  }
  function getF_WorkshopIdOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2011621894347952128', query).then(res => {
      let data = res.data;
      state.optionsObj.F_WorkshopIdOptions = Array.isArray(data) ? data : [];
    });
  }
  function getF_LineIdOptions() {
    getDictionaryDataSelector('2011623836151320576').then(res => {
      state.optionsObj.F_LineIdOptions = res.data.list;
    });
  }
  function gettableField72c841_F_GoodsIdOptions() {
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
      state.optionsObj.tableField72c841_F_GoodsIdOptions = Array.isArray(data) ? data : [];
    });
  }
  function removetableField72c841Row(index, showConfirm = false) {
    if (showConfirm) {
      createConfirm({
        iconType: 'warning',
        title: '提示',
        content: '此操作将永久删除该数据, 是否继续?',
        onOk: () => {
          state.dataForm.tableField72c841.splice(index, 1);
          state.GoodsList.splice(index, 1);
        },
      });
    } else {
      state.dataForm.tableField72c841.splice(index, 1);
      state.GoodsList.splice(index, 1);
    }
  }
  function tableField72c841Exist() {
    let isOk = true;
    for (let i = 0; i < state.dataForm.tableField72c841.length; i++) {
      const e = state.dataForm.tableField72c841[i];
      if (!e.F_GoodsId) {
        createMessage.error('物料' + t('sys.validate.textRequiredSuffix'));
        isOk = false;
        break;
      }
      if (!e.F_Num) {
        createMessage.error('数量' + t('sys.validate.textRequiredSuffix'));
        isOk = false;
        break;
      }
    }
    return isOk;
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
    if (!Array.isArray(state.dataForm.tableField72c841)) {
      state.dataForm.tableField72c841 = [];
    }

    /* 1. 把 option 里的 id 做成 Set，方便比对 */
    const optionIdSet = new Set(option.map(o => o.id));

    /* 2. 删除本地已不在 option 里的行 */
    state.dataForm.tableField72c841 = state.dataForm.tableField72c841.filter(item => optionIdSet.has(item.F_GoodsId));
    /* 3. 把 option 里本地还没有的行加进来 */
    option.forEach(o => {
      const exist = state.dataForm.tableField72c841.findIndex(item => item.F_GoodsId === o.id) !== -1;
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
        state.dataForm.tableField72c841.push(newRow);
        state.tableField72c841innerActiveKey.push(newRow.jnpfId);
      }
    });
  }
</script>
