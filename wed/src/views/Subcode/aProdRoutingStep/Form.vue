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
          <a-col :span="24" class="ant-col-item">
            <a-tabs v-model:activeKey="state.active01" type="card" tabPosition="top" class="mb-20">
              <a-tab-pane tab="报工/质检信息" key="01" forceRender>
                <a-row>
                  <a-col :span="6" class="ant-col-item">
                    <a-form-item name="F_WagePrice" :labelCol="{ style: { width: '100px' } }">
                      <template #label>工资单价</template>
                      <JnpfInputNumber
                        v-model:value="dataForm.F_WagePrice"
                        placeholder="请输入"
                        min="0"
                        :precision="2"
                        :controls="false"
                        :style="{
                          width: '100%',
                        }" />
                    </a-form-item>
                  </a-col>
                  <a-col :span="5" class="ant-col-item">
                    <a-form-item name="F_StdHour" :labelCol="{ style: { width: '100px' } }">
                      <template #label>标准工时</template>
                      <JnpfInputNumber
                        v-model:value="dataForm.F_StdHour"
                        placeholder="请输入"
                        addonAfter="小时"
                        min="0"
                        :controls="false"
                        :style="{
                          width: '100%',
                        }" />
                    </a-form-item>
                  </a-col>
                  <a-col :span="3" class="ant-col-item">
                    <a-form-item name="F_StdMinute" :labelCol="{ style: { width: '100px' } }">
                      <JnpfInputNumber
                        v-model:value="dataForm.F_StdMinute"
                        placeholder="请输入"
                        addonAfter="分钟"
                        min="0"
                        max="59"
                        :controls="false"
                        :style="{
                          width: '100%',
                        }" />
                    </a-form-item>
                  </a-col>
                  <a-col :span="3" class="ant-col-item">
                    <a-form-item name="F_StdSecond" :labelCol="{ style: { width: '100px' } }">
                      <JnpfInputNumber
                        v-model:value="dataForm.F_StdSecond"
                        placeholder="请输入"
                        addonAfter="秒"
                        min="0"
                        max="59"
                        :controls="false"
                        :style="{
                          width: '100%',
                        }" />
                    </a-form-item>
                  </a-col>
                  <a-col :span="6" class="ant-col-item">
                    <a-form-item name="F_PriceType" :labelCol="{ style: { width: '100px' } }">
                      <template #label>计价方式</template>
                      <JnpfSelect
                        v-model:value="dataForm.F_PriceType"
                        placeholder="请选择"
                        :options="optionsObj.F_PriceTypeOptions"
                        :fieldNames="optionsObj.F_PriceTypeProps"
                        allowClear
                        showSearch
                        :style="{
                          width: '100%',
                        }" />
                    </a-form-item>
                  </a-col>
                  <a-col :span="12" class="ant-col-item">
                    <a-form-item name="F_UnitRatio" :labelCol="{ style: { width: '100px' } }">
                      <template #label>单位关系</template>
                      <div style="display: flex; align-items: center; gap: 4px">
                        <span>生产</span>
                        <span style="display: inline-block; width: 20%">
                          <JnpfInputNumber
                            v-model:value="dataForm.F_UnitRatioProd"
                            placeholder=""
                            :controls="false"
                            :style="{
                              width: '100%',
                            }" />
                        </span>
                        <span>个计划数，需要</span>
                        <span style="display: inline-block; width: 20%">
                          <JnpfInputNumber
                            v-model:value="dataForm.F_UnitRatioReport"
                            placeholder=""
                            :controls="false"
                            :style="{
                              width: '100%',
                            }" />
                        </span>
                        <span style="display: inline-block; width: 20%">
                          <JnpfSelect
                            v-model:value="dataForm.F_UnitRatioBase"
                            placeholder=""
                            :options="optionsObj.F_UnitRatioBaseOptions"
                            :fieldNames="optionsObj.F_UnitRatioBaseProps"
                            allowClear
                            :style="{
                              width: '100%',
                            }"
                        /></span>
                      </div>
                    </a-form-item>
                  </a-col>
                  <a-col :span="12" class="ant-col-item">
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
                        :limit="3"
                        :sortRule="['1', '2']"
                        timeFormat="YYYYMMDD" />
                    </a-form-item>
                  </a-col>
                  <a-col :span="24" class="ant-col-item">
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
                    <a-tabs v-model:activeKey="state.active1" type="" tabPosition="top" class="mb-20">
                      <a-tab-pane tab="质检/外协信息" key="2" forceRender>
                        <a-row>
                          <!-- <a-col :span="24" class="ant-col-item">
                            <a-form-item :labelCol="{ style: { width: '100px' } }">
                              <JnpfText
                                content="开启配置后工序默认为外协工序，外协工序无法报工和质检。"
                                :textStyle="{
                                  color: '#F20808',
                                  'text-align': 'left',
                                  'font-weight': 'bold',
                                  'font-style': 'normal',
                                  'text-decoration': 'none',
                                  'line-height': 20,
                                  'font-size': 14,
                                }" />
                            </a-form-item>
                          </a-col> -->
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
                        </a-row>
                      </a-tab-pane>
                      <!-- <a-tab-pane tab="工序打卡设置" key="2585b9" forceRender>
                        <a-row>
                          <a-col :span="24" class="ant-col-item">
                            <a-form-item :labelCol="{ style: { width: '100px' } }">
                              <JnpfText
                                content="开启后可以通过打卡的方式记录生产的总用时。"
                                :textStyle="{
                                  color: '#F20808',
                                  'text-align': 'left',
                                  'font-weight': 'bold',
                                  'font-style': 'normal',
                                  'text-decoration': 'none',
                                  'line-height': 20,
                                  'font-size': 14,
                                }" />
                            </a-form-item>
                          </a-col>
                          <a-col :span="24" class="ant-col-item">
                            <a-form-item name="F_TaskTimer" :labelCol="{ style: { width: '100px' } }">
                              <template #label>生产任务计时</template>
                              <JnpfSwitch v-model:value="dataForm.F_TaskTimer" />
                            </a-form-item>
                          </a-col>
                        </a-row>
                      </a-tab-pane> -->
                    </a-tabs>
                  </a-col>
                </a-row>
              </a-tab-pane>
              <a-tab-pane tab="物料信息" key="02" forceRender>
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
                        :data-source="dataForm.tableField3b6f08"
                        :columns="tableField3b6f08Columns"
                        size="small"
                        rowKey="jnpfId"
                        :pagination="false"
                        :scroll="{ x: 'max-content' }"
                        :rowSelection="gettableField3b6f08RowSelection">
                        <template #headerCell="{ column }">
                          <span class="required-sign" v-if="column.required">*</span>{{ column.title
                          }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                        /></template>
                        <template #bodyCell="{ column, record, index }">
                          <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                          <template v-if="column.key === 'F_GoodsId'">
                            <JnpfSelect
                              v-model:value="record.F_GoodsId"
                              :options="optionsObj.tableField3b6f08_F_GoodsIdOptions"
                              :fieldNames="optionsObj.tableField3b6f08_F_GoodsIdProps"
                              allowClear
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
                              <!-- <a-button class="action-btn" type="link" @click="copytableField3b6f08Row(index)" size="small">{{
                                t('common.copyText', '复制')
                              }}</a-button> -->
                              <a-button class="action-btn" type="link" color="error" @click="removetableField3b6f08Row(index, true)" size="small">{{
                                t('common.delText', '删除')
                              }}</a-button>
                            </a-space>
                          </template>
                        </template>
                      </a-table>
                      <a-space class="input-table-footer-btn">
                        <!-- <a-button type="primary" preIcon="icon-ym icon-ym-btn-add" @click="addtableField3b6f08Row">{{ t('common.add1Text', '添加') }}</a-button> -->
                        <!-- <a-button type="danger" preIcon="icon-ym icon-ym-btn-clearn" @click="batchRemovetableField3b6f08Row(true)">{{
                          t('common.batchDelText', '批量删除')
                        }}</a-button> -->
                      </a-space>
                    </a-form-item>
                  </a-col>
                </a-row>
              </a-tab-pane>
            </a-tabs>
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
    active1: any;
    active01: any;
    currVmodel: any;
    currTableConf: any;
    addTableConf: any;
    tableRows: any;
    tableField3b6f08outerActiveKey: number[];
    tableField3b6f08innerActiveKey: string[];
    tableField3b6f08DefaultExpandAll: boolean;
    selectedtableField3b6f08RowKeys: any[];
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
  const tableField3b6f08Columns: any[] = computed(() => {
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
  const gettableField3b6f08RowSelection = computed(() => {
    const rowSelection = {
      selectedRowKeys: state.selectedtableField3b6f08RowKeys,
      onChange: (selectedRowKeys: string[]) => {
        state.selectedtableField3b6f08RowKeys = (selectedRowKeys || []).sort().reverse();
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
      tableField3b6f08: [],
    },
    tableField3b6f08outerActiveKey: [0],
    tableField3b6f08innerActiveKey: [],
    tableField3b6f08DefaultExpandAll: true,
    selectedtableField3b6f08RowKeys: [],
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
    active1: '2',
    active01: '01',
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
      tableField3b6f08_F_GoodsIdOptions: [],
      tableField3b6f08_F_GoodsIdProps: { label: 'F_GoodsName', value: 'id' },
    },
    currVmodel: '',
    currTableConf: {},
    tableRows: {
      tableField3b6f08: {
        enabledmark: undefined,
        F_GoodsId: undefined,
        F_GoodsNo: undefined,
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
    state.title = '修改工序 -- ' + data.F_ProcessName;
    setFormProps({ continueLoading: false });
    // 直接赋值，别再 reset
    state.dataForm = { ...data };
    state.dataForm.F_IsOutsource = Number(state.dataForm.F_IsOutsource);
    state.dataForm.F_IsQc = Number(state.dataForm.F_IsQc);
    state.dataForm.F_DefectHandle = Number(state.dataForm.F_DefectHandle);
    state.dataForm.F_TaskTimer = Number(state.dataForm.F_TaskTimer);
    state.GoodsList = [];
    for (var i = 0; i < state.dataForm.tableField3b6f08.length; i++) {
      state.dataForm.tableField3b6f08[i].jnpfId = buildUUID();
      state.GoodsList.push(state.dataForm.tableField3b6f08[i].F_GoodsId);
    }
    state.active1 = '2';
    state.active01 = '01';
    openModal();
    initData();
  }
  function initData() {
    changeLoading(true);
    getF_PriceTypeOptions();
    getF_UnitRatioBaseOptions();
    gettableField3b6f08_F_GoodsIdOptions();
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
      const invalidRows = state.dataForm.tableField3b6f08.filter(item => item.F_Num && item.F_InventoryNum && item.F_Num > item.F_InventoryNum);
      if (invalidRows.length > 0) {
        const errorMsg = invalidRows
          .map(item => {
            const goods = state.optionsObj.tableField3b6f08_F_GoodsIdOptions.find((g: any) => g.id === item.F_GoodsId);
            const goodsName = goods ? goods.F_GoodsName : item.F_GoodsId;
            return `${goodsName}：数量 ${item.F_Num} 大于库存 ${item.F_InventoryNum}`;
          })
          .join('；');
        createMessage.error(`物料清单验证失败：${errorMsg}`);
        return false;
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
  function addForSelect(data) {
    for (let i = 0; i < data.length; i++) {
      let item = { ...state.tableRows[state.currVmodel], ...data[i], jnpfId: buildUUID() };
      state.dataForm[state.currVmodel].push(cloneDeep(item));
      if (state[state.currVmodel + 'DefaultExpandAll']) state[state.currVmodel + 'innerActiveKey'].push(item.jnpfId);
    }
  }

  function gettableField3b6f08_F_GoodsIdOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2012085692393459712', query).then(res => {
      let data = res.data;
      state.optionsObj.tableField3b6f08_F_GoodsIdOptions = Array.isArray(data) ? data : [];
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
          state.GoodsList.splice(index, 1);
        },
      });
    } else {
      state.dataForm.tableField3b6f08.splice(index, 1);
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

  // 处理数量变化
  function handleF_NumChange(record) {
    // 触发响应式更新,确保错误提示及时显示
    const index = state.dataForm.tableField3b6f08.findIndex(item => item.jnpfId === record.jnpfId);
    if (index > -1) {
      // 只更新当前行,不使用 record 的直接引用
      state.dataForm.tableField3b6f08[index] = {
        ...state.dataForm.tableField3b6f08[index],
        F_Num: record.F_Num,
      };
    }
  }
  function GoodsListBtn(val, option) {
    /* 兜底保证是数组 */
    if (!Array.isArray(state.dataForm.tableField3b6f08)) {
      state.dataForm.tableField3b6f08 = [];
    }

    /* 1. 把 option 里的 id 做成 Set，方便比对 */
    const optionIdSet = new Set(option.map(o => o.id));

    /* 2. 删除本地已不在 option 里的行 */
    state.dataForm.tableField3b6f08 = state.dataForm.tableField3b6f08.filter(item => optionIdSet.has(item.F_GoodsId));

    /* 3. 把 option 里本地还没有的行加进来，已存在的行保留用户编辑的值 */
    option.forEach(o => {
      const existIndex = state.dataForm.tableField3b6f08.findIndex(item => item.F_GoodsId === o.id);
      if (existIndex === -1) {
        // 新增行
        const newRow = {
          jnpfId: buildUUID(),
          F_GoodsId: o.id,
          F_GoodsNo: o.F_GoodsNo,
          F_Specification: o.F_Specification,
          F_Unit: o.F_Unit,
          F_InventoryNum: o.F_InventoryNum,
          F_Num: 1,
          F_Description: undefined,
          F_CreatorTime: undefined,
        };
        state.dataForm.tableField3b6f08.push(newRow);
        state.tableField3b6f08innerActiveKey.push(newRow.jnpfId);
      } else {
        // 已存在的行，只更新基础信息，保留用户编辑的 F_Num 和 F_Description
        state.dataForm.tableField3b6f08[existIndex] = {
          ...state.dataForm.tableField3b6f08[existIndex],
          F_GoodsNo: o.F_GoodsNo,
          F_Specification: o.F_Specification,
          F_Unit: o.F_Unit,
          F_InventoryNum: o.F_InventoryNum,
        };
      }
    });
  }
</script>
