<template>
  <BasicModal v-bind="$attrs" @register="registerModal" width="70%" :minHeight="100" okText="确定" cancelText="取消" @ok="handleSubmit" :closeFunc="onClose">
    <template #title>
      <a-space :size="10">
        <div class="text-16px font-medium">{{ title }}</div>
        <a-space-compact size="small" block v-if="dataForm.id">
          <!-- <a-tooltip :title="t('common.prevRecord')">
            <a-button size="small" :disabled="getPrevDisabled" @click="handlePrev">
              <i class="icon-ym icon-ym-caret-left text-10px"></i>
            </a-button>
          </a-tooltip>
          <a-tooltip :title="t('common.nextRecord')">
            <a-button size="small" :disabled="getNextDisabled" @click="handleNext">
              <i class="icon-ym icon-ym-caret-right text-10px"></i>
            </a-button>
          </a-tooltip> -->
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_StockOutNo')">
            <a-form-item name="F_StockOutNo" :labelCol="{ style: { width: '100px' } }">
              <template #label>其他出库单号</template>
              <JnpfInput
                v-model:value="dataForm.F_StockOutNo"
                placeholder="请填写，忽略将自动生成"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_WarehouseId')">
            <a-form-item name="F_WarehouseId" :labelCol="{ style: { width: '100px' } }">
              <template #label>仓库</template>
              <JnpfCascader
                v-model:value="dataForm.F_WarehouseId"
                placeholder="请选择"
                :options="optionsObj.F_WarehouseIdOptions"
                :fieldNames="optionsObj.F_WarehouseIdProps"
                allowClear
                :disabled="!!state.dataForm.id"
                showSearch
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_StockOutType')">
            <a-form-item name="F_StockOutType" :labelCol="{ style: { width: '100px' } }">
              <template #label>出库类型</template>
              <JnpfSelect
                v-model:value="dataForm.F_StockOutType"
                placeholder="请选择"
                :options="optionsObj.F_StockOutTypeOptions"
                :fieldNames="optionsObj.F_StockOutTypeProps"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_StockOutDate')">
            <a-form-item name="F_StockOutDate" :labelCol="{ style: { width: '100px' } }">
              <template #label>出库日期</template>
              <JnpfDatePicker
                v-model:value="dataForm.F_StockOutDate"
                placeholder="请选择"
                format="yyyy-MM-dd"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_StockOutUserId')">
            <a-form-item name="F_StockOutUserId" :labelCol="{ style: { width: '100px' } }">
              <template #label>发货人</template>
              <JnpfUserSelect
                v-model:value="dataForm.F_StockOutUserId"
                placeholder="请选择"
                selectType="all"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_ProcessNo')">
            <a-form-item name="F_ProcessNo" :labelCol="{ style: { width: '100px' } }">
              <template #label>加工单号</template>
              <JnpfSelect
                v-model:value="dataForm.F_ProcessNo"
                placeholder="请选择"
                :options="optionsObj.F_ProcessNoOptions"
                :fieldNames="optionsObj.F_ProcessNoProps"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <!-- <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_CreatorUserId')">
            <a-form-item name="F_CreatorUserId" :labelCol="{ style: { width: '100px' } }">
              <template #label>创建人员</template>
              <JnpfOpenData
                v-model:value="dataForm.F_CreatorUserId"
                type="currUser"
                readonly
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_CreatorTime')">
            <a-form-item name="F_CreatorTime" :labelCol="{ style: { width: '100px' } }">
              <template #label>创建时间</template>
              <JnpfOpenData
                v-model:value="dataForm.F_CreatorTime"
                type="currTime"
                readonly
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col> -->
          <a-col :span="24" class="ant-col-item">
            <a-form-item :labelCol="{ style: { width: '100px' } }">
              <template #label>批量商品选择</template>
              <JnpfPopupSelect
                v-model:value="tempGoodsSelector"
                :formData="state.dataForm"
                placeholder="选择对应仓库后，再选择商品"
                :templateJson="customerTemplateJson"
                allowClear
                :field="'tempGoodsSelector'"
                interfaceId="2015036768293883904"
                :columnOptions="optionsObj.tableField44e07d_F_CustomerIdOptions"
                relationField="F_GoodsName"
                propsValue="F_Id"
                :pageSize="20"
                popupType="dialog"
                popupTitle="选择商品"
                popupWidth="70%"
                hasPage
                multiple
                :style="{
                  width: '100%',
                }"
                :extraOptions="optionsObj.tableField44e07d_F_CustomerId"
                @change="handleGoodsSelectorChange" />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="商品管理" :bordered="false" />
              <a-table
                :data-source="dataForm.tableField44e07d"
                :columns="tableField44e07dColumns"
                size="small"
                rowKey="jnpfId"
                :pagination="false"
                :scroll="{ x: 'max-content' }"
                :rowSelection="gettableField44e07dRowSelection">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                  <template v-if="column.key === 'F_CustomerId'">
                    <JnpfPopupSelect
                      v-model:value="record.F_CustomerId"
                      :formData="state.dataForm"
                      :rowIndex="index"
                      placeholder="请选择"
                      :templateJson="customerTemplateJson"
                      allowClear
                      :field="'F_CustomerId' + index"
                      interfaceId="2015036768293883904"
                      :columnOptions="optionsObj.tableField44e07d_F_CustomerIdOptions"
                      relationField="F_GoodsName"
                      propsValue="F_GoodsId"
                      :pageSize="20"
                      popupType="dialog"
                      popupTitle="选择数据"
                      popupWidth="60%"
                      hasPage
                      :style="{
                        width: '100%',
                      }"
                      :extraOptions="optionsObj.tableField44e07d_F_CustomerId"
                      :disabled="true" />
                  </template>
                  <template v-if="column.key === 'F_Num'">
                    <JnpfInputNumber
                      v-model:value="record.F_Num"
                      placeholder="请输入"
                      :step="1.0"
                      :controls="true"
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_Price'">
                    <JnpfInputNumber
                      v-model:value="record.F_Price"
                      placeholder="请输入"
                      :controls="false"
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_Description'">
                    <JnpfTextarea
                      v-model:value="record.F_Description"
                      placeholder="请输入"
                      allowClear
                      :autoSize="{
                        minRows: 1,
                        maxRows: 4,
                      }"
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
                      <!-- <a-button class="action-btn" type="link" @click="copytableField44e07dRow(index)" size="small">{{
                        t('common.copyText', '复制')
                      }}</a-button> -->
                      <a-button class="action-btn" type="link" color="error" @click="removetableField44e07dRow(index, true)" size="small">{{
                        t('common.delText', '删除')
                      }}</a-button>
                    </a-space>
                  </template>
                </template>
              </a-table>
              <a-space class="input-table-footer-btn">
                <!-- <a-button type="primary" preIcon="icon-ym icon-ym-btn-add" @click="addtableField44e07dRow">{{ t('common.add1Text', '添加') }}</a-button>
                <a-button type="danger" preIcon="icon-ym icon-ym-btn-clearn" @click="batchRemovetableField44e07dRow(true)">{{
                  t('common.batchDelText', '批量删除')
                }}</a-button> -->
              </a-space>
            </a-form-item>
          </a-col>
          <!-- <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="设计子表" :bordered="false" />
              <a-table
                :data-source="dataForm.tableField211772"
                :columns="tableField211772Columns"
                size="small"
                rowKey="jnpfId"
                :pagination="false"
                :scroll="{ x: 'max-content' }"
                :rowSelection="gettableField211772RowSelection">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                  <template v-if="column.key === 'F_Type'">
                    <JnpfInput
                      v-model:value="record.F_Type"
                      placeholder="请输入"
                      allowClear
                      :style="{
                        width: '100%',
                      }"
                      :showCount="false" />
                  </template>
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
                  <template v-if="column.key === 'F_Reason'">
                    <JnpfTextarea
                      v-model:value="record.F_Reason"
                      placeholder="请输入"
                      allowClear
                      :autoSize="{
                        minRows: 1,
                        maxRows: 4,
                      }"
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
                      <a-button class="action-btn" type="link" @click="copytableField211772Row(index)" size="small">{{
                        t('common.copyText', '复制')
                      }}</a-button>
                      <a-button class="action-btn" type="link" color="error" @click="removetableField211772Row(index, true)" size="small">{{
                        t('common.delText', '删除')
                      }}</a-button>
                    </a-space>
                  </template>
                </template>
              </a-table>
              <a-space class="input-table-footer-btn">
                <a-button type="primary" preIcon="icon-ym icon-ym-btn-add" @click="addtableField211772Row">{{ t('common.add1Text', '添加') }}</a-button>
                <a-button type="danger" preIcon="icon-ym icon-ym-btn-clearn" @click="batchRemovetableField211772Row(true)">{{
                  t('common.batchDelText', '批量删除')
                }}</a-button>
              </a-space>
            </a-form-item>
          </a-col> -->
        </a-row>
      </a-form>
    </a-row>
  </BasicModal>
  <SelectModal :config="state.currTableConf" :formData="state.dataForm" ref="selectModal" @select="addForSelect" />
</template>
<script lang="ts" setup>
  import { create, update, getInfo } from './helper/api';
  import { reactive, toRefs, nextTick, ref, unref, computed, toRaw, inject, watch } from 'vue';
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
  import { CaretRightOutlined } from '@ant-design/icons-vue';
  import { systemComponentsList } from '@/components/FormGenerator/src/helper/config';
  import { usePermission } from '@/hooks/web/usePermission';

  interface State {
    dataForm: any;
    dataRule: any;
    optionsObj: any;
    currVmodel: any;
    currTableConf: any;
    addTableConf: any;
    tableRows: any;
    tableField44e07douterActiveKey: number[];
    tableField44e07dinnerActiveKey: string[];
    tableField44e07dDefaultExpandAll: boolean;
    selectedtableField44e07dRowKeys: any[];
    tableField211772outerActiveKey: number[];
    tableField211772innerActiveKey: string[];
    tableField211772DefaultExpandAll: boolean;
    selectedtableField211772RowKeys: any[];
    title: string;
    continueText: string;
    allList: any[];
    currIndex: number;
    isContinue: boolean;
    submitType: number;
    showContinueBtn: boolean;
    tempGoodsSelector: any[];
  }

  const emit = defineEmits(['reload']);
  const getLeftTreeActiveInfo: (() => any) | null = inject('getLeftTreeActiveInfo', null);
  const userStore = useUserStore();
  const userInfo = userStore.getUserInfo;
  const { createMessage, createConfirm } = useMessage();
  const { t } = useI18n();
  const { hasFormP } = usePermission();
  const [registerModal, { openModal, setModalProps }] = useModal();
  const formRef = ref<FormInstance>();
  const selectModal = ref(null);
  const tableField44e07dColumns: any[] = computed(() => {
    let list = [
      {
        title: '商品',
        dataIndex: 'F_CustomerId',
        key: 'F_CustomerId',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_CustomerId',
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
        title: '成本单价(元)',
        dataIndex: 'F_Price',
        key: 'F_Price',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Price',
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
      // {
      //   title: '创建人员',
      //   dataIndex: 'F_CreatorUserId',
      //   key: 'F_CreatorUserId',
      //   tipLabel: '',
      //   required: false,
      //   align: 'left',
      //   span: '24',
      //   labelWidth: '',
      //   width: '',
      //   fixed: false,
      //   formP: 'F_CreatorUserId',
      //   isSystemControl: true,
      // },
      // {
      //   title: '创建时间',
      //   dataIndex: 'F_CreatorTime',
      //   key: 'F_CreatorTime',
      //   tipLabel: '',
      //   required: false,
      //   align: 'left',
      //   span: '24',
      //   labelWidth: '',
      //   width: '',
      //   fixed: false,
      //   formP: 'F_CreatorTime',
      //   isSystemControl: true,
      // },
    ];
    list = list.filter(o => hasFormP('tableField44e07d-' + o.formP));
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
  const gettableField44e07dRowSelection = computed(() => {
    const rowSelection = {
      selectedRowKeys: state.selectedtableField44e07dRowKeys,
      onChange: (selectedRowKeys: string[]) => {
        state.selectedtableField44e07dRowKeys = (selectedRowKeys || []).sort().reverse();
      },
    };
    return rowSelection;
  });
  const tableField211772Columns: any[] = computed(() => {
    let list = [
      {
        title: '类型',
        dataIndex: 'F_Type',
        key: 'F_Type',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Type',
        isSystemControl: false,
      },
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
        title: '修改原因',
        dataIndex: 'F_Reason',
        key: 'F_Reason',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Reason',
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
    list = list.filter(o => hasFormP('tableField211772-' + o.formP));
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
  const gettableField211772RowSelection = computed(() => {
    const rowSelection = {
      selectedRowKeys: state.selectedtableField211772RowKeys,
      onChange: (selectedRowKeys: string[]) => {
        state.selectedtableField211772RowKeys = (selectedRowKeys || []).sort().reverse();
      },
    };
    return rowSelection;
  });
  const state = reactive<State>({
    dataForm: {
      id: '',
      F_StockOutNo: undefined,
      F_WarehouseId: [],
      F_StockOutType: undefined,
      F_StockOutDate: dayjs(new Date()).startOf(getDateTimeUnit('yyyy-MM-dd')).valueOf(),
      F_StockOutUserId: userInfo.userId ? userInfo.userId : '',
      F_ProcessNo: undefined,
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
      tableField44e07d: [],
      tableField211772: [],
    },
    tableField44e07douterActiveKey: [0],
    tableField44e07dinnerActiveKey: [],
    tableField44e07dDefaultExpandAll: true,
    selectedtableField44e07dRowKeys: [],
    tableField211772outerActiveKey: [0],
    tableField211772innerActiveKey: [],
    tableField211772DefaultExpandAll: true,
    selectedtableField211772RowKeys: [],
    dataRule: {
      F_StockOutDate: [
        {
          required: true,
          message: '请输入出库日期',
          trigger: 'change',
        },
      ],
      F_WarehouseId: [
        {
          required: true,
          message: '请选择仓库',
          trigger: 'change',
        },
      ],
    },
    optionsObj: {
      F_WarehouseIdProps: {
        label: 'F_Name',
        value: 'F_Id',
        children: 'children',
      },
      F_StockOutTypeProps: { label: 'fullName', value: 'enCode' },
      F_ProcessNoProps: { label: 'F_ProcessNo', value: 'F_Id' },
      tableField44e07d_F_CustomerIdOptions: [
        {
          value: 'F_GoodsName',
          label: '商品',
        },
        {
          value: 'F_GoodsNo',
          label: '商品编码',
        },
        {
          value: 'F_Type',
          label: '商品类型',
        },
        {
          value: 'F_Num',
          label: '数量',
        },
      ],
      tableField44e07d_F_CustomerIdTemplateJson: [
        {
          defaultValue: '',
          field: 'id',
          dataType: 'varchar',
          required: 0,
          fieldName: '',
          relationField: '',
          jnpfKey: null,
          complexHeaderList: null,
          sourceType: 2,
          isChildren: false,
          IsMultiple: false,
        },
      ],
    },
    currVmodel: '',
    currTableConf: {},
    tableRows: {
      tableField44e07d: {
        enabledmark: undefined,
        F_CustomerId: undefined,
        F_Num: undefined,
        F_Price: undefined,
        F_Description: undefined,
        F_CreatorUserId: undefined,
        F_CreatorTime: undefined,
      },
      tableField211772: {
        enabledmark: undefined,
        F_Type: undefined,
        F_Title: undefined,
        F_Content: undefined,
        F_Reason: undefined,
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
    tempGoodsSelector: [],
  });
  const { title, continueText, showContinueBtn, dataForm, submitType, dataRule, optionsObj, tempGoodsSelector } = toRefs(state);

  /** 仓库级联选择的值 → id 字符串列表（来自 JnpfCascader 多选路径） */
  function getWarehouseIdParamForInterface(): string[] {
    const w = state.dataForm.F_WarehouseId as unknown;
    if (w == null || w === '') return [];
    if (Array.isArray(w)) return w.map(v => String(v));
    return [String(w)];
  }

  /**
   * 数据接口联动参数：必须是 JSON 数组文本（双引号），如 '["id1","id2"]'。
   * 传 JS 数组时后端 GetDatainterfaceParameter 会按数组分支二次序列化，导致与 SQL/API 约定不一致。
   */
  function getWarehouseIdParamJsonForInterface(): string {
    return JSON.stringify(getWarehouseIdParamForInterface());
  }

  const customerTemplateJson = computed(() => {
    const arr = cloneDeep(state.optionsObj.tableField44e07d_F_CustomerIdTemplateJson || []);
    if (arr && arr.length) {
      arr.forEach(item => {
        item.relationField = getWarehouseIdParamJsonForInterface();
      });
    }
    return arr;
  });

  // 批量商品选择后的处理（简化实现：把所选商品作为子表行添加）
  function handleGoodsSelectorChange(selected: any) {
    // 清空现有子表数据
    state.dataForm.tableField44e07d = [];
    state.selectedtableField44e07dRowKeys = [];

    if (!selected) {
      state.tempGoodsSelector = [];
      return;
    }

    const buildAndPushRow = (data: any) => {
      let relationId: any = data;
      let fullObj: any = null;
      if (data && typeof data === 'object') {
        fullObj = data;
        relationId = data.F_Id ?? data.FId ?? data.id ?? data.value ?? relationId;
      }
      const newRow: any = {
        jnpfId: buildUUID(),
        F_CustomerId: relationId,
        _F_CustomerObj: fullObj,
        F_Num: undefined,
        F_Price: undefined,
        F_Description: undefined,
        F_CreatorUserId: undefined,
        F_CreatorTime: undefined,
      };
      state.dataForm.tableField44e07d.push(newRow);
    };

    if (Array.isArray(selected)) {
      selected.forEach(item => buildAndPushRow(item));
      state.tempGoodsSelector = selected.slice();
    } else {
      buildAndPushRow(selected);
      state.tempGoodsSelector = [selected];
    }
  }

  /**
   * 根据子表中被删除行的 relationId（即 F_CustomerId）清理 tempGoodsSelector 中对应的已选项
   * 支持 tempGoodsSelector 中为对象或原始值的情况
   */
  function removeFromTempGoodsSelectorByRelationIds(relationIds: any[]) {
    if (!relationIds || !relationIds.length) return;
    const filterFn = (item: any) => {
      let id = item;
      if (item && typeof item === 'object') {
        id = item.F_Id ?? item.FId ?? item.id ?? item.value ?? id;
      }
      return !relationIds.includes(id);
    };
    state.tempGoodsSelector = (state.tempGoodsSelector || []).filter(filterFn);
  }

  // 监听子表变化，自动同步批量选择显示的值（避免删除子表后选择框仍显示已删除项）
  watch(
    () => state.dataForm.tableField44e07d,
    (list: any[]) => {
      try {
        const newSel = (list || []).map(r => r._F_CustomerObj ?? r.F_CustomerId ?? r.F_GoodsId ?? r.F_Id ?? r.id ?? r.value ?? null).filter(Boolean);
        state.tempGoodsSelector = newSel;
      } catch (e) {
        // ignore
      }
    },
    { deep: true },
  );

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
      state.dataForm.tableField44e07d = [];
      state.dataForm.tableField211772 = [];
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
        F_StockOutNo: undefined,
        F_WarehouseId: [],
        F_StockOutType: undefined,
        F_StockOutDate: dayjs(new Date()).startOf(getDateTimeUnit('yyyy-MM-dd')).valueOf(),
        F_StockOutUserId: userInfo.userId ? userInfo.userId : '',
        F_ProcessNo: undefined,
        F_CreatorUserId: undefined,
        F_CreatorTime: undefined,
        tableField44e07d: [],
        tableField211772: [],
      };
      getF_WarehouseIdOptions();
      getF_StockOutTypeOptions();
      getF_ProcessNoOptions();
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
      for (let i = 0; i < state.dataForm.tableField44e07d.length; i++) {
        const element = state.dataForm.tableField44e07d[i];
        element.jnpfId = buildUUID();
      }
      for (let i = 0; i < state.dataForm.tableField211772.length; i++) {
        const element = state.dataForm.tableField211772[i];
        element.jnpfId = buildUUID();
      }
      getF_WarehouseIdOptions();
      getF_StockOutTypeOptions();
      getF_ProcessNoOptions();
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
  function getF_WarehouseIdOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2021045201115680768', query).then(res => {
      let data = res.data;
      state.optionsObj.F_WarehouseIdOptions = Array.isArray(data) ? data : [];
    });
  }
  function getF_ProcessNoOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2014981107073814528', query).then(res => {
      let data = res.data;
      state.optionsObj.F_ProcessNoOptions = Array.isArray(data) ? data : [];
    });
  }
  function getF_StockOutTypeOptions() {
    getDictionaryDataSelector('2013096194263355392').then(res => {
      state.optionsObj.F_StockOutTypeOptions = res.data.list;
    });
  }
  function addtableField44e07dRow() {
    let item = {
      jnpfId: buildUUID(),
      F_CustomerId: undefined,
      F_Num: undefined,
      F_Price: undefined,
      F_Description: undefined,
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
    };
    state.dataForm.tableField44e07d.push(item);
    state.tableField44e07dinnerActiveKey.push(item.jnpfId);
  }
  function removetableField44e07dRow(index, showConfirm = false) {
    if (showConfirm) {
      createConfirm({
        iconType: 'warning',
        title: '提示',
        content: '此操作将永久删除该数据, 是否继续?',
        onOk: () => {
          const removed = state.dataForm.tableField44e07d[index];
          state.dataForm.tableField44e07d.splice(index, 1);
          if (removed) removeFromTempGoodsSelectorByRelationIds([removed.F_CustomerId]);
        },
      });
    } else {
      const removed = state.dataForm.tableField44e07d[index];
      state.dataForm.tableField44e07d.splice(index, 1);
      if (removed) removeFromTempGoodsSelectorByRelationIds([removed.F_CustomerId]);
    }
  }
  function copytableField44e07dRow(index) {
    let item = cloneDeep(state.dataForm.tableField44e07d[index]);
    let copyData = {};
    for (let i = 0; i < unref(tableField44e07dColumns).length; i++) {
      const cur = unref(tableField44e07dColumns)[i];
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
    state.dataForm.tableField44e07d.push(copyItem);
    state.tableField44e07dinnerActiveKey.push(copyItem.jnpfId);
  }
  function batchRemovetableField44e07dRow(showConfirm = false) {
    if (!state.selectedtableField44e07dRowKeys.length) return createMessage.error('请选择一条数据');
    const handleBatchRemove = () => {
      const removedRows = state.dataForm.tableField44e07d.filter(o => state.selectedtableField44e07dRowKeys.includes(o.jnpfId));
      state.dataForm.tableField44e07d = state.dataForm.tableField44e07d.filter(o => !state.selectedtableField44e07dRowKeys.includes(o.jnpfId));
      nextTick(() => {
        state.selectedtableField44e07dRowKeys = [];
      });
      const relationIds = removedRows.map(r => r.F_CustomerId).filter(id => id !== undefined && id !== null);
      if (relationIds.length) removeFromTempGoodsSelectorByRelationIds(relationIds);
    };
    if (!showConfirm) return handleBatchRemove();
    createConfirm({
      iconType: 'warning',
      title: '提示',
      content: '此操作将永久删除该数据, 是否继续?',
      onOk: handleBatchRemove,
    });
  }
  function addtableField211772Row() {
    let item = {
      jnpfId: buildUUID(),
      F_Type: undefined,
      F_Title: undefined,
      F_Content: undefined,
      F_Reason: undefined,
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
    };
    state.dataForm.tableField211772.push(item);
    state.tableField211772innerActiveKey.push(item.jnpfId);
  }
  function removetableField211772Row(index, showConfirm = false) {
    if (showConfirm) {
      createConfirm({
        iconType: 'warning',
        title: '提示',
        content: '此操作将永久删除该数据, 是否继续?',
        onOk: () => {
          state.dataForm.tableField211772.splice(index, 1);
        },
      });
    } else {
      state.dataForm.tableField211772.splice(index, 1);
    }
  }
  function copytableField211772Row(index) {
    let item = cloneDeep(state.dataForm.tableField211772[index]);
    let copyData = {};
    for (let i = 0; i < unref(tableField211772Columns).length; i++) {
      const cur = unref(tableField211772Columns)[i];
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
    state.dataForm.tableField211772.push(copyItem);
    state.tableField211772innerActiveKey.push(copyItem.jnpfId);
  }
  function batchRemovetableField211772Row(showConfirm = false) {
    if (!state.selectedtableField211772RowKeys.length) return createMessage.error('请选择一条数据');
    const handleBatchRemove = () => {
      state.dataForm.tableField211772 = state.dataForm.tableField211772.filter(o => !state.selectedtableField211772RowKeys.includes(o.jnpfId));
      nextTick(() => {
        state.selectedtableField211772RowKeys = [];
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
</script>
