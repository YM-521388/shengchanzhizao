<template>
  <div class="prod-task-container" v-if="state.show">
    <div class="header-container">{{ title }}</div>
    <div class="content-container">
      <a-tabs v-model:activeKey="activeKey">
        <template #tabBarExtraContent>
          <div class="button-container">
            <template v-for="(action, index) in getActions" :key="`${index}-${action.label}`">
              <PopConfirmButton v-bind="action">
                <i :class="[action.icon, { 'mr-5px': !!action.label }]" v-if="action.icon"></i>
                <template v-if="action.label">{{ action.label }}</template>
              </PopConfirmButton>
            </template>
            <Dropdown :trigger="['hover']" :dropMenuList="getDropdownList">
              <slot name="more"></slot>
              <a-button type="link" size="small" v-if="!$slots.more">
                {{ t('common.moreText') }}
                <DownOutlined class="icon-more" />
              </a-button>
            </Dropdown>
          </div>
        </template>
        <a-tab-pane key="1" tab="工序信息">
          <a-form
            :colon="false"
            layout="horizontal"
            labelAlign="right"
            :labelCol="{ style: { width: '100px' } }"
            :model="workForm"
            :rules="workaRule"
            ref="workFormRef">
            <a-row :gutter="15">
              <a-col :span="12" class="ant-col-item">
                <a-form-item name="F_ResponsibleUserId" :labelCol="{ style: { width: '100px' } }">
                  <template #label>负责人</template>
                  <JnpfUserSelect
                    v-model:value="workForm.F_ResponsibleUserId"
                    placeholder=""
                    selectType="all"
                    allowClear
                    :style="{
                      width: '100%',
                    }"
                    disabled />
                </a-form-item>
              </a-col>

              <a-col :span="12" class="ant-col-item">
                <a-form-item name="F_ProcessId" :labelCol="{ style: { width: '100px' } }">
                  <template #label>工序</template>
                  <JnpfPopupSelect
                    v-model:value="workForm.F_ProcessId"
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
                    :style="{
                      width: '100%',
                    }"
                    :extraOptions="[]"
                    disabled />
                </a-form-item>
              </a-col>
              <a-col :span="12" class="ant-col-item">
                <a-form-item name="F_WagePrice" :labelCol="{ style: { width: '100px' } }">
                  <template #label>工资单价</template>
                  <JnpfInputNumber
                    v-model:value="workForm.F_WagePrice"
                    placeholder=""
                    min="0"
                    :precision="2"
                    :controls="false"
                    :style="{
                      width: '100%',
                    }"
                    disabled />
                </a-form-item>
              </a-col>

              <a-col :span="12" class="ant-col-item">
                <a-form-item name="F_PriceType" :labelCol="{ style: { width: '100px' } }">
                  <template #label>计价方式</template>
                  <JnpfSelect
                    v-model:value="workForm.F_PriceType"
                    placeholder=""
                    :options="optionsObj.F_PriceTypeOptions"
                    :fieldNames="optionsObj.F_PriceTypeProps"
                    allowClear
                    showSearch
                    :style="{
                      width: '100%',
                    }"
                    disabled />
                </a-form-item>
              </a-col>
              <a-col :span="12" class="ant-col-item">
                <a-form-item name="F_StdHour" :labelCol="{ style: { width: '100px' } }">
                  <template #label>标准工时</template>
                  <JnpfInputNumber
                    v-model:value="workForm.F_StdHour"
                    placeholder=""
                    addonAfter="小时"
                    min="0"
                    :controls="false"
                    :style="{
                      width: '100%',
                    }"
                    disabled />
                </a-form-item>
              </a-col>
              <a-col :span="6" class="ant-col-item">
                <a-form-item name="F_StdMinute" :labelCol="{ style: { width: '100px' } }">
                  <JnpfInputNumber
                    v-model:value="workForm.F_StdMinute"
                    placeholder=""
                    addonAfter="分钟"
                    min="0"
                    max="59"
                    :controls="false"
                    :style="{
                      width: '100%',
                    }"
                    disabled />
                </a-form-item>
              </a-col>
              <a-col :span="6" class="ant-col-item">
                <a-form-item name="F_StdSecond" :labelCol="{ style: { width: '100px' } }">
                  <JnpfInputNumber
                    v-model:value="workForm.F_StdSecond"
                    placeholder=""
                    addonAfter="秒"
                    min="0"
                    max="59"
                    :controls="false"
                    :style="{
                      width: '100%',
                    }"
                    disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24" class="ant-col-item">
                <a-form-item name="F_UnitRatio" :labelCol="{ style: { width: '100px' } }">
                  <template #label>单位关系</template>
                  <div style="display: flex; align-items: center; gap: 4px">
                    <span>生产</span>
                    <span>1个计划数，需要</span>
                    <span style="display: inline-block; width: 20%">
                      <JnpfInputNumber
                        v-model:value="workForm.F_UnitRatio"
                        placeholder=""
                        :controls="false"
                        :style="{
                          width: '100%',
                        }"
                        disabled />
                    </span>
                    <span style="display: inline-block; width: 20%">
                      <JnpfSelect
                        v-model:value="workForm.F_ReportUnit"
                        placeholder=""
                        :options="optionsObj.F_UnitRatioBaseOptions"
                        :fieldNames="optionsObj.F_UnitRatioBaseProps"
                        allowClear
                        :style="{
                          width: '100%',
                        }"
                        disabled />
                    </span>
                  </div>
                </a-form-item>
              </a-col>
              <a-col :span="12" class="ant-col-item">
                <a-form-item name="F_IsOutsource" :labelCol="{ style: { width: '100px' } }">
                  <template #label>工序需外协</template>
                  <JnpfSwitch @change="IsOutsourceBtn" v-model:value="workForm.F_IsOutsource" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="12" class="ant-col-item">
                <a-form-item name="F_IsQc" :labelCol="{ style: { width: '100px' } }">
                  <template #label>工序需质检</template>
                  <JnpfSwitch @change="IsQcBtn" v-model:value="workForm.F_IsQc" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="12" class="ant-col-item" v-if="state.workForm.F_IsQc == 1">
                <a-form-item name="F_DefectHandle" :labelCol="{ style: { width: '140px' } }">
                  <template #label>不良品需报废、返工</template>
                  <JnpfSwitch v-model:value="workForm.F_DefectHandle" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24" class="ant-col-item">
                <a-form-item name="F_Files" :labelCol="{ style: { width: '100px' } }">
                  <template #label>附件</template>
                  <JnpfUploadFile
                    v-model:value="workForm.F_Files"
                    buttonText="点击上传"
                    pathType="selfPath"
                    :isAccount="-1"
                    folder=""
                    :fileSize="10"
                    sizeUnit="MB"
                    :limit="3"
                    :sortRule="['1', '2']"
                    timeFormat="YYYYMMDD"
                    disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24" class="ant-col-item">
                <a-form-item name="F_Description" :labelCol="{ style: { width: '100px' } }">
                  <template #label>备注</template>
                  <JnpfTextarea
                    v-model:value="workForm.F_Description"
                    placeholder=""
                    :maxlength="500"
                    allowClear
                    :autoSize="{
                      minRows: 3,
                      maxRows: 3,
                    }"
                    :style="{
                      width: '100%',
                    }"
                    :showCount="false"
                    disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24" class="ant-col-item">
                <a-form-item :labelCol="{ style: { width: '100px' } }">
                  <template #label>BOM</template>
                  <JnpfSelect
                    v-model:value="workForm.F_BOMId"
                    placeholder=""
                    :options="optionsObj.F_BOMIdOptions"
                    :fieldNames="optionsObj.F_BOMIdProps"
                    allowClear
                    showSearch
                    :style="{
                      width: '100%',
                    }"
                    disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24" class="ant-col-item">
                <a-form-item label="">
                  <a-table
                    :data-source="workForm.tableField0bb944"
                    :columns="tableField0bb944Columns"
                    size="small"
                    rowKey="jnpfId"
                    :pagination="false"
                    :scroll="{ x: 'max-content' }"
                    :rowSelection="gettableField0bb944RowSelection">
                    <template #headerCell="{ column }">
                      <span class="required-sign" v-if="column.required">*</span>{{ column.title }}
                      <BasicHelp :text="column.tipLabel" v-if="column.tipLabel" />
                    </template>
                    <template #bodyCell="{ column, record, index }">
                      <template v-if="column.key === 'F_GoodsId'">
                        <JnpfSelect
                          v-model:value="record.F_GoodsId"
                          :options="optionsObj.tableField0bb944_F_GoodsIdOptions"
                          :fieldNames="optionsObj.tableField0bb944_F_GoodsIdProps"
                          allowClear
                          showSearch
                          disabled
                          :style="{
                            width: '100%',
                          }" />
                      </template>
                      <template v-if="column.key === 'F_Num'">
                        <JnpfInputNumber
                          v-model:value="record.F_Num"
                          placeholder=""
                          :min="1"
                          :controls="false"
                          :style="{
                            width: '100%',
                          }"
                          disabled />
                      </template>
                      <template v-if="column.key === 'F_UnitTwo'">
                        <JnpfInput
                          v-model:value="record.F_UnitTwo"
                          placeholder=""
                          :controls="false"
                          disabled
                          :style="{
                            width: '100%',
                          }" />
                      </template>
                      <template v-if="column.key === 'F_Description'">
                        <JnpfInput
                          v-model:value="record.F_Description"
                          placeholder=""
                          allowClear
                          :style="{
                            width: '100%',
                          }"
                          :showCount="false"
                          disabled />
                      </template>
                    </template>
                  </a-table>
                </a-form-item>
              </a-col>
            </a-row>
          </a-form>
        </a-tab-pane>
        <a-tab-pane key="2" tab="生产任务" force-render>
          <!-- 生产任务 -->
          <a-form
            :colon="false"
            layout="horizontal"
            labelAlign="right"
            :labelCol="{ style: { width: '100px' } }"
            :model="dataForm"
            :rules="dataRule"
            ref="dataFormRef"
            class="task-form">
            <a-row :gutter="15">
              <a-col :span="12" class="ant-col-item">
                <a-form-item name="F_PlanStartDate" :labelCol="{ style: { width: '100px' } }">
                  <template #label>计划开始</template>
                  <JnpfDatePicker
                    v-model:value="dataForm.F_PlanStartDate"
                    placeholder="请选择"
                    format="yyyy-MM-dd"
                    allowClear
                    :style="{
                      width: '100%',
                    }" />
                </a-form-item>
              </a-col>
              <a-col :span="12" class="ant-col-item">
                <a-form-item name="F_PlanEndDate" :labelCol="{ style: { width: '100px' } }">
                  <template #label>计划结束</template>
                  <JnpfDatePicker
                    v-model:value="dataForm.F_PlanEndDate"
                    placeholder="请选择"
                    format="yyyy-MM-dd"
                    allowClear
                    :style="{
                      width: '100%',
                    }" />
                </a-form-item>
              </a-col>
              <a-col :span="12" class="ant-col-item">
                <a-form-item name="F_ProdDispatch" :labelCol="{ style: { width: '100px' } }">
                  <template #label>生产派工</template>
                  <JnpfSelect
                    v-model:value="dataForm.F_ProdDispatch"
                    placeholder="请选择"
                    :options="optionsObj.F_ProdDispatchOptions"
                    :fieldNames="optionsObj.F_ProdDispatchProps"
                    allowClear
                    showSearch
                    multiple
                    :style="{
                      width: '100%',
                    }" />
                </a-form-item>
              </a-col>
              <a-col :span="12" class="ant-col-item">
                <a-form-item name="F_QcDispatch" :labelCol="{ style: { width: '100px' } }">
                  <template #label>质检派工</template>
                  <JnpfSelect
                    v-model:value="dataForm.F_QcDispatch"
                    placeholder="请选择"
                    :options="optionsObj.F_QcDispatchOptions"
                    :fieldNames="optionsObj.F_QcDispatchProps"
                    allowClear
                    showSearch
                    multiple
                    :style="{
                      width: '100%',
                    }" />
                </a-form-item>
              </a-col>
              <a-col :span="12" class="ant-col-item">
                <a-form-item name="F_ProdQty" :labelCol="{ style: { width: '100px' } }">
                  <template #label>生产数量</template>
                  <JnpfInputNumber
                    v-model:value="dataForm.F_ProdQty"
                    placeholder=""
                    :min="1"
                    :controls="false"
                    :style="{
                      width: '100%',
                    }" />
                </a-form-item>
              </a-col>
              <a-col :span="24" class="ant-col-item">
                <a-form-item name="F_Description" :labelCol="{ style: { width: '100px' } }">
                  <template #label>备注</template>
                  <JnpfTextarea
                    v-model:value="dataForm.F_Description"
                    placeholder=""
                    allowClear
                    :autoSize="{
                      minRows: 4,
                      maxRows: 4,
                    }"
                    :style="{
                      width: '100%',
                    }"
                    :showCount="false" />
                </a-form-item>
              </a-col>
            </a-row>
          </a-form>
        </a-tab-pane>
      </a-tabs>
      <template v-if="activeKey === '2' && dataForm.F_TaskStatusCode">
        <div class="submit-btn">
          <a-button type="primary" @click="handleSubmit" :loading="state.loading">保存</a-button>
        </div>
      </template>
      <CheckForm ref="checkRef" @reload="reload" />
      <ReportForm ref="reportRef" @reload="reload" />
    </div>
  </div>
</template>
<script lang="ts" setup>
  import { reactive, toRefs, ref, unref, computed, toRaw, inject, onMounted } from 'vue';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useI18n } from '@/hooks/web/useI18n';
  import { usePermission } from '@/hooks/web/usePermission';

  import { cloneDeep } from 'lodash-es';
  import type { FormInstance } from 'ant-design-vue';
  import { DownOutlined } from '@ant-design/icons-vue';
  import { isBoolean, isFunction } from '@/utils/is';

  import { Dropdown } from '@/components/Dropdown';
  import { BasicModal, useModal } from '@/components/Modal';
  import ReportForm from '@/views/Subcode/aProdReport/Form.vue';
  import CheckForm from '@/views/Subcode/aProdTask/CheckForm.vue';
  import { PopConfirmButton, ModelConfirmButton } from '@/components/Button';

  import { getDictionaryDataSelector } from '@/api/systemData/dictionary';
  import { getDataInterfaceRes } from '@/api/systemData/dataInterface';
  import { getInfo, getUserCon, getList, del, batchDelete, handleCheck, update, create } from '@/views/Subcode/aProdTask/helper/api';

  const { t } = useI18n();
  const { hasFormP } = usePermission();
  const { createMessage, createConfirm } = useMessage();
  const refreshPage = inject<() => void>('refreshPage', () => {});
  const [registerModal, { openModal, setModalProps }] = useModal();

  const props = defineProps(['flowInfo', 'flowState', 'versionList', 'type', 'isAdvanced']);

  defineExpose({ init, hideModal });

  interface State {
    loading: boolean;
    show: boolean;
    title: string;
    workForm: any;
    workaRule: any;
    dataForm: any;
    dataRule: any;
    optionsObj: any;
    F_TaskStatusCode: any;
    activeKey: string;
    tempGoodsSelector?: any[];
    selectedtableField0bb944RowKeys: any[];
  }

  const dataFormRef = ref<FormInstance>();
  const checkRef = ref<any>(null);
  const reportRef = ref<any>(null);
  const { hasBtnP } = usePermission();

  const state = reactive<State>({
    loading: false,
    show: false,
    title: '',
    workForm: {
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
      tableField0bb944: [] as any[],
    },
    workaRule: {
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
    dataForm: {
      id: '',
      F_ProcessingId: undefined,
      F_ProcessId: undefined,
      F_PlanStartDate: undefined,
      F_PlanEndDate: undefined,
      F_ProdDispatch: [],
      F_QcDispatch: [],
      F_ProdQty: undefined,
      F_SortCode: undefined,
      F_Description: undefined,
      F_TaskStatus: undefined,
      F_TaskType: undefined,
      F_CreatorTime: undefined,
      tableField0bb944: [],
    },
    dataRule: {},
    activeKey: '1',
    tempGoodsSelector: [],
    selectedtableField0bb944RowKeys: [],

    optionsObj: {
      F_PriceTypeProps: { label: 'fullName', value: 'enCode' },
      F_UnitRatioBaseProps: { label: 'fullName', value: 'enCode' },
      tableField0bb944_F_GoodsIdOptions: [],
      tableField0bb944_F_GoodsIdProps: { label: 'F_GoodsName', value: 'id' },
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
      F_ProdDispatchOptions: [],
      F_QcDispatchProps: { label: 'fullName', value: 'id' },
      F_QcDispatchOptions: [],
    },
    F_TaskStatusCode: '',
  });

  const { workForm, workaRule, optionsObj, title, dataForm, dataRule, activeKey } = toRefs(state);

  const tableField0bb944Columns: any[] = computed(() => {
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
    let columns = [...columnList];
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
  function isIfShow(action: ActionItem): boolean {
    const ifShow = action.ifShow;

    let isIfShow = true;

    if (isBoolean(ifShow)) {
      isIfShow = ifShow;
    }
    if (isFunction(ifShow)) {
      isIfShow = ifShow(action);
    }
    return isIfShow;
  }
  const getActions = computed(() => {
    return (toRaw(getTableActions()) || [])
      .filter(action => {
        // return hasBtnP(action.auth) && isIfShow(action);
        return isIfShow(action);
      })
      .map(action => {
        const { popConfirm } = action;
        return {
          type: 'link',
          size: 'small',
          ...action,
          ...(popConfirm || {}),
          onConfirm: popConfirm?.confirm,
          onCancel: popConfirm?.cancel,
          enable: !!popConfirm,
        };
      });
  });
  const getDropdownList = computed((): any[] => {
    const list = (toRaw(getDropDownActions()) || []).filter(action => {
      // return hasBtnP(action.auth) && isIfShow(action);
      return isIfShow(action);
    });

    return list.map((action, index) => {
      const { label, popConfirm } = action;
      return {
        ...action,
        ...popConfirm,
        onConfirm: popConfirm?.confirm,
        onCancel: popConfirm?.cancel,
        text: label,
      };
    });
  });
  onMounted(() => {
    getF_BOMIdOptions();

    setTimeout(initData, 0);
  });

  function init(formConf) {
    state.show = true;
    state.title = formConf.nodeName;
    state.dataForm.id = formConf.id;
    state.F_TaskStatusCode = formConf.F_TaskStatusCode;
    state.dataForm.F_TaskStatusCode = formConf.F_TaskStatusCode;

    getInfoData();
    getUserConData();
    resetworkForm();
    if (formConf?.formData && typeof formConf.formData === 'object') {
      assignFromSource(formConf.formData);
    }
  }
  function hideModal() {
    state.show = false;
  }

  // 重置 state.workForm 为初始值
  function resetworkForm() {
    state.workForm = {
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
      tableField0bb944: [] as any[],
    };
  }

  function initData() {
    getF_UnitRatioBaseOptions();
    getF_PriceTypeOptions();
    getAProdRoutingFromData();
    gettableField449e6c_F_GoodsIdOptions();
  }

  // ===============工艺路线===============start
  // 辅助函数：从源对象中提取 state.workForm 中定义的键赋值
  function assignFromSource(source: any) {
    if (!source || typeof source !== 'object') return;
    Object.keys(state.workForm).forEach(key => {
      if (source[key] !== undefined) {
        state.workForm[key] = source[key];
      }
    });
    state.workForm.F_IsQc = Number(state.workForm.F_IsQc);
    state.workForm.F_IsOutsource = Number(state.workForm.F_IsOutsource);
    state.workForm.F_DefectHandle = Number(state.workForm.F_DefectHandle);
  }

  // 单位关系的单位
  function getF_UnitRatioBaseOptions() {
    getDictionaryDataSelector('2008448689420505088').then(res => {
      state.optionsObj.F_UnitRatioBaseOptions = res.data.list;
    });
  }
  // 计价方式
  function getF_PriceTypeOptions() {
    getDictionaryDataSelector('2011642610875240448').then(res => {
      state.optionsObj.F_PriceTypeOptions = res.data.list;
    });
  }
  // BOM
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
      paramList: getParamList(templateJson, state.workForm),
    };
    getDataInterfaceRes('2013188646957617152', query).then(res => {
      let data = res.data;
      state.optionsObj.F_BOMIdOptions = Array.isArray(data) ? data : [];
    });
  }
  // 商品
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
      paramList: getParamList(templateJson, state.workForm),
    };
    getDataInterfaceRes('2012085692393459712', query).then(res => {
      let data = res.data;
      state.optionsObj.tableField0bb944_F_GoodsIdOptions = Array.isArray(data) ? data : [];
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

  function getAProdRoutingFromData() {
    const fromData = localStorage.getItem('getAProdRoutingFromData');
    if (!fromData) return;

    const allNodesData = JSON.parse(fromData);
    if (allNodesData.formData.F_RoutingNo !== undefined) {
      state.workForm.F_RoutingNo = allNodesData.formData.F_RoutingNo;
    }
    if (allNodesData.formData.F_RoutingName !== undefined) {
      state.workForm.F_RoutingName = allNodesData.formData.F_RoutingName;
    }
  }

  function IsOutsourceBtn(val) {
    if (val == 1) {
      state.workForm.F_IsQc = 0;
    }
  }
  function IsQcBtn(val) {
    if (val == 1) {
      state.workForm.F_IsOutsource = 0;
    }
  }

  // ===============工艺路线===============end

  // ===============生产任务===============start
  function getForm() {
    const form = unref(dataFormRef);
    if (!form) {
      throw new Error('form is null!');
    }
    return form;
  }
  // 生产派单
  function getUserConData() {
    getUserCon(state.dataForm.id).then(res => {
      state.optionsObj.F_ProdDispatchOptions = res.data.prodUserList || [];
      state.optionsObj.F_QcDispatchOptions = res.data.qcUserList || [];
    });
  }
  async function handleSubmit() {
    try {
      const values = await getForm()?.validate();
      if (!values) return;
      state.loading = true;
      update(state.dataForm)
        .then(res => {
          createMessage.success(res.msg);
          state.loading = false;
          setFormProps({ confirmLoading: false });
          setFormProps({ open: false });
        })
        .catch(() => {
          setFormProps({ confirmLoading: false });
          state.loading = false;
        });
    } catch (_) {}
  }
  function setFormProps(data) {
    setModalProps(data);
  }
  function changeLoading(loading) {
    setModalProps({ loading });
  }
  function getTableActions() {
    let record = state.dataForm;

    return [
      {
        label: t('报工'),
        ifShow: record.F_TaskStatusCode == '1',
        onClick: reportHandle.bind(null, state.dataForm),
        // auth: 'btn_report',
      },
      {
        label: t('开始工序'),
        ifShow: record.F_TaskStatusCode == '0',
        onClick: handleState.bind(null, record.id, '1'),
        // auth: 'btn_startProcess',
      },
    ];
  }
  // 更多按钮
  function getDropDownActions() {
    let record = state.dataForm;
    return [
      {
        label: '未开始',
        ifShow: record.F_TaskStatusCode != '3' && record.F_TaskStatusCode != '0',
        onClick: handleState.bind(null, record.id, '0'),
        // auth: 'btn_notStart',
      },
      {
        label: '进行中',
        ifShow: record.F_TaskStatusCode != '3' && record.F_TaskStatusCode != '1',
        onClick: handleState.bind(null, record.id, '1'),
        // auth: 'btn_inProgress',
      },
      {
        label: '已完成',
        ifShow: record.F_TaskStatusCode != '3' && record.F_TaskStatusCode != '2',
        onClick: handleState.bind(null, record.id, '2'),
        // auth: 'btn_completed',
      },
      {
        label: '暂停',
        ifShow: record.F_TaskStatusCode == '1',
        onClick: handleState.bind(null, record.id, '3'),
        // auth: 'btn_pause',
      },
      {
        label: '取消暂停',
        ifShow: record.F_TaskStatusCode == '3',
        onClick: handleState.bind(null, record.id, '4'),
        // auth: 'btn_cancelPause',
      },
    ];
  }
  //修改状态
  function handleState(id, state) {
    if (state == '3') {
      const data = {
        id: id,
        fullName: '暂停原因',
      };
      checkRef.value?.init(data);
    } else {
      createConfirm({
        iconType: 'warning',
        title: t('common.sureText', '确定要修改状态吗？'),
        onOk: () => {
          handleCheck(id, state).then(res => {
            createMessage.success(res.msg);
            refreshPage();
          });
        },
      });
      return;
    }
  }
  // 报工
  function reportHandle(record) {
    const data = {
      id: '',
      F_TaskId: record.id,
      F_ProcessId: record.F_ProcessId,
      F_ProcessName: record.F_ProcessName || state.title,
      F_ProdUserByIds: record.F_ProdUserByIds || null,
      F_ProdQty: record.F_ProdQty,
    };
    reportRef.value?.init(data);
  }

  function reload() {
    refreshPage();
  }
  function getInfoData() {
    getInfo(state.dataForm.id).then(res => {
      state.dataForm = res.data || {};
      state.dataForm.F_TaskStatusCode = state.F_TaskStatusCode;
      changeLoading(false);
    });
  }

  // ===============生产任务===============end
</script>
<style lang="less" scoped>
  .prod-task-container {
    width: 500px;
    flex-shrink: 0;
    background-color: #ffffff;
    transition: all 0.3s;
    display: flex;
    flex-direction: column;
    border-radius: 8px;
    position: absolute;
    right: 0;
    z-index: 199;
    top: 0;
    box-shadow: 0px 20px 20px rgba(0, 0, 0, 0.1);
    overflow: hidden;
    height: 100%;

    .header-container {
      border-bottom: 1px solid #f0f0f0;
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 0 16px;
      font-weight: 600;
      background-color: #f1f4f8;
      border-radius: 8px 8px 0 0;
      height: 50px;
    }

    .content-container {
      height: calc(100% - 50px);
      overflow: auto;
      padding: 15px;

      .task-form {
        height: 100%;
      }

      .button-container {
        display: flex;
        align-items: center;
        gap: 8px;
      }

      .submit-btn {
        width: 100%;
        display: flex;
        justify-content: center;
        // bottom: 30%;

        position: absolute;
      }
    }
  }
</style>
