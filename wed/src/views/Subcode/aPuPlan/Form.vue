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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_SupplierId')">
            <a-form-item name="F_SupplierId" :labelCol="{ style: { width: '100px' } }">
              <template #label>供应商</template>
              <JnpfSelect
                v-model:value="dataForm.F_SupplierId"
                placeholder="请选择"
                :options="optionsObj.F_SupplierIdOptions"
                :fieldNames="optionsObj.F_SupplierIdProps"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_Money')">
            <a-form-item name="F_Money" :labelCol="{ style: { width: '100px' } }">
              <template #label>金额</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_Money"
                placeholder="请输入"
                :controls="false"
                disabled
                :style="{
                  width: '100%',
                }" />
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_PurchaseNum')">
            <a-form-item name="F_PurchaseNum" :labelCol="{ style: { width: '100px' } }">
              <template #label>采购数量</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_PurchaseNum"
                placeholder="请输入"
                :controls="false"
                disabled
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

          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="选择采购商品" :bordered="false" />
              <a-col :span="24" class="ant-col-item">
                <a-form-item :labelCol="{ style: { width: '100px' } }">
                  <template #label>商品选择</template>
                  <JnpfPopupSelect
                    v-model:value="tempGoodsSelector"
                    placeholder="请选择商品"
                    :templateJson="optionsObj.tableFieldc87c94_F_CustomerIdTemplateJson"
                    allowClear
                    :field="'tempGoodsSelector'"
                    interfaceId="2008721559174385664"
                    :columnOptions="optionsObj.tableFieldc87c94_F_CustomerIdOptions"
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
                    @change="handleGoodsSelectorChange" />
                </a-form-item>
              </a-col>
              <a-table
                :data-source="dataForm.tableFieldc87c94"
                :columns="tableFieldc87c94Columns"
                size="small"
                rowKey="jnpfId"
                :pagination="listPagination"
                :scroll="{ x: 'max-content' }"
                :rowSelection="gettableFieldc87c94RowSelection">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                  <template v-if="column.key === 'F_CustomerId'">
                    <JnpfPopupSelect
                      v-model:value="record.F_CustomerId"
                      @change="(_, selected) => handleCustomerSelectChange(selected, record, index)"
                      placeholder="请选择"
                      :templateJson="optionsObj.tableFieldc87c94_F_CustomerIdTemplateJson"
                      allowClear
                      :field="'F_CustomerId' + index"
                      interfaceId="2008721559174385664"
                      :columnOptions="optionsObj.tableFieldc87c94_F_CustomerIdOptions"
                      relationField="F_GoodsName"
                      propsValue="F_Id"
                      :pageSize="20"
                      popupType="dialog"
                      popupTitle="选择数据"
                      popupWidth="70%"
                      hasPage
                      disabled
                      :style="{
                        width: '100%',
                      }"
                      :extraOptions="optionsObj.tableFieldc87c94_F_CustomerId" />
                  </template>
                  <template v-if="column.key === 'F_GoodsNo'">
                    <span class="display-text">{{ record.F_GoodsNo }}</span>
                  </template>
                  <template v-if="column.key === 'F_Specification'">
                    <span class="display-text">{{ record.F_Specification }}</span>
                  </template>
                  <template v-if="column.key === 'F_Image'">
                    <template v-if="record.F_Image">
                      <img :src="record.F_Image" alt="image" style="max-width: 120px; max-height: 40px; object-fit: contain" />
                    </template>
                    <template v-else>
                      <span class="display-text">{{ record.F_Image }}</span>
                    </template>
                  </template>
                  <template v-if="column.key === 'F_SupplierId'">
                    <JnpfSelect
                      v-model:value="record.F_SupplierId"
                      placeholder="请选择"
                      :options="optionsObj.F_SupplierIdOptions"
                      :fieldNames="optionsObj.F_SupplierIdProps"
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_Num'">
                    <template v-if="getUnitRatioLevels(record)?.level2?.name">
                      <a-space>
                        <JnpfInputNumber
                          v-model:value="record.F_NumLevel1"
                          placeholder="请输入"
                          :controls="false"
                          :style="{ width: '80px' }"
                          @change="handleNumLevel1Change(record)" />
                        <span>{{ getUnitRatioLevels(record)?.level1?.name }}</span
                        >/
                        <JnpfInputNumber v-model:value="record.F_NumLevel2" placeholder="自动计算" :controls="false" disabled :style="{ width: '80px' }" />
                        <span>{{ getUnitRatioLevels(record)?.level2?.name }}</span>
                      </a-space>
                    </template>
                    <template v-else>
                      <a-space>
                        <JnpfInputNumber
                          v-model:value="record.F_NumLevel1"
                          placeholder="请输入"
                          :controls="false"
                          :style="{ width: '80px' }"
                          @change="handleNumLevel1Change(record)" />
                        <span>{{ getUnitRatioLevels(record)?.level1?.name }}</span>
                      </a-space>
                    </template>
                  </template>
                  <template v-if="column.key === 'F_Price'">
                    <JnpfTextarea
                      v-model:value="record.F_Price"
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
                  <template v-if="column.key === 'F_Amount'">
                    <span class="display-text">{{ thousandsFormat(calculateRowAmount(record)) }}</span>
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
                      <!-- <a-button class="action-btn" type="link" @click="copytableFieldc87c94Row(index)" size="small">{{
                        t('common.copyText', '复制')
                      }}</a-button> -->
                      <a-button class="action-btn" type="link" color="error" @click="removetableFieldc87c94Row(index, true)" size="small">{{
                        t('common.delText', '删除')
                      }}</a-button>
                    </a-space>
                  </template>
                </template>
              </a-table>
              <!-- <a-space class="input-table-footer-btn">
                <a-button type="primary" preIcon="icon-ym icon-ym-btn-add" @click="addtableFieldc87c94Row">{{ t('common.add1Text', '添加') }}</a-button>
                <a-button type="danger" preIcon="icon-ym icon-ym-btn-clearn" @click="batchRemovetableFieldc87c94Row(true)">{{
                  t('common.batchDelText', '批量删除')
                }}</a-button>
              </a-space> -->
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
  import { reactive, toRefs, nextTick, ref, unref, computed, toRaw, inject, watch } from 'vue';
  import { BasicModal, useModal } from '@/components/Modal';
  import SelectModal from '@/components/CommonModal/src/SelectModal.vue';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useI18n } from '@/hooks/web/useI18n';
  import { useUserStore } from '@/store/modules/user';
  import type { FormInstance } from 'ant-design-vue';
  import { thousandsFormat } from '@/utils/jnpf';
  import { getTimeUnit, getDateTimeUnit } from '@/utils/jnpf';
  import { getDataInterfaceRes, getDataInterfaceDataInfoByIds } from '@/api/systemData/dataInterface';
  import dayjs from 'dayjs';
  import { cloneDeep } from 'lodash-es';
  import { buildUUID } from '@/utils/uuid';
  import { CaretRightOutlined } from '@ant-design/icons-vue';
  import { systemComponentsList } from '@/components/FormGenerator/src/helper/config';
  import { usePermission } from '@/hooks/web/usePermission';
  import { useGlobSetting } from '@/hooks/setting';

  interface State {
    dataForm: any;
    dataRule: any;
    optionsObj: any;
    currVmodel: any;
    currTableConf: any;
    addTableConf: any;
    tableRows: any;
    tableFieldc87c94outerActiveKey: number[];
    tableFieldc87c94innerActiveKey: string[];
    tableFieldc87c94DefaultExpandAll: boolean;
    selectedtableFieldc87c94RowKeys: any[];
    tableFielde82301outerActiveKey: number[];
    tableFielde82301innerActiveKey: string[];
    tableFielde82301DefaultExpandAll: boolean;
    selectedtableFielde82301RowKeys: any[];
    listPageSize: number;
    listCurrentPage: number;
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
  const tableFieldc87c94Columns: any[] = computed(() => {
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
        width: '150px',
        fixed: false,
        formP: 'F_CustomerId',
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
        isSystemControl: true,
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
        isSystemControl: true,
      },
      // {
      //   title: '商品图片',
      //   dataIndex: 'F_Image',
      //   key: 'F_Image',
      //   tipLabel: '',
      //   required: false,
      //   align: 'left',
      //   span: '24',
      //   labelWidth: '',
      //   width: '',
      //   fixed: false,
      //   formP: 'F_Image',
      //   isSystemControl: true,
      // },
      {
        title: '供应商',
        dataIndex: 'F_SupplierId',
        key: 'F_SupplierId',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '150px',
        fixed: false,
        formP: 'F_SupplierId',
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
        title: '单价(元)',
        dataIndex: 'F_Price',
        key: 'F_Price',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '100px',
        fixed: false,
        formP: 'F_Price',
        isSystemControl: false,
      },
      {
        title: '金额(元)',
        dataIndex: 'F_Amount',
        key: 'F_Amount',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Amount',
        isSystemControl: true,
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
    list = list.filter(o => hasFormP('tableFieldc87c94-' + o.formP));
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
  const gettableFieldc87c94RowSelection = computed(() => {
    const rowSelection = {
      selectedRowKeys: state.selectedtableFieldc87c94RowKeys,
      onChange: (selectedRowKeys: string[]) => {
        state.selectedtableFieldc87c94RowKeys = (selectedRowKeys || []).sort().reverse();
      },
    };
    return rowSelection;
  });
  const tableFielde82301Columns: any[] = computed(() => {
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
    ];
    list = list.filter(o => hasFormP('tableFielde82301-' + o.formP));
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
  const gettableFielde82301RowSelection = computed(() => {
    const rowSelection = {
      selectedRowKeys: state.selectedtableFielde82301RowKeys,
      onChange: (selectedRowKeys: string[]) => {
        state.selectedtableFielde82301RowKeys = (selectedRowKeys || []).sort().reverse();
      },
    };
    return rowSelection;
  });
  const state = reactive<State>({
    dataForm: {
      id: '',
      F_PlanNo: undefined,
      F_PlanName: undefined,
      F_SupplierId: undefined,
      F_Money: 0,
      F_DeliveryDate: undefined,
      F_PurchaseNum: 0,
      F_Description: undefined,
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
      tableFieldc87c94: [],
      tableFielde82301: [],
    },
    tableFieldc87c94outerActiveKey: [0],
    tableFieldc87c94innerActiveKey: [],
    tableFieldc87c94DefaultExpandAll: true,
    selectedtableFieldc87c94RowKeys: [],
    tableFielde82301outerActiveKey: [0],
    tableFielde82301innerActiveKey: [],
    tableFielde82301DefaultExpandAll: true,
    selectedtableFielde82301RowKeys: [],
    listPageSize: 20,
    listCurrentPage: 1,

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
      F_SupplierIdProps: { label: 'F_SupplierName', value: 'F_Id' },
      tableFieldc87c94_F_CustomerIdOptions: [
        {
          value: 'F_GoodsName',
          label: '商品名称',
        },
        {
          value: 'F_GoodsNo',
          label: '商品编码',
        },
        // {
        //   value: 'F_Image',
        //   label: '商品图片',
        // },
        // {
        //   value: 'F_Unit',
        //   label: '单位',
        // },
        {
          value: 'F_Specification',
          label: '规格',
        },
        {
          value: 'F_Remark',
          label: '备注',
        },
        {
          value: 'F_Source',
          label: '商品来源',
        },
      ],
      tableFieldc87c94_F_CustomerIdTemplateJson: [],
    },
    currVmodel: '',
    currTableConf: {},
    tableRows: {
      tableFieldc87c94: {
        enabledmark: undefined,
        F_CustomerId: undefined,
        F_GoodsNo: undefined,
        F_Specification: undefined,
        F_Image: undefined,
        F_SupplierId: undefined,
        F_Num: undefined,
        F_NumLevel1: undefined,
        F_NumLevel2: undefined,
        F_Unit_Ratio: undefined,
        F_Price: undefined,
        F_PriceBag: undefined,
        F_PriceUnit: undefined,
        F_Amount: undefined,
        F_Description: undefined,
        F_CreatorUserId: undefined,
        F_CreatorTime: undefined,
      },
      tableFielde82301: {
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
    tempGoodsSelector: [],
  });
  const { title, continueText, showContinueBtn, dataForm, submitType, dataRule, optionsObj, tempGoodsSelector } = toRefs(state);
  const globSetting = useGlobSetting();
  const apiUrl = globSetting.apiUrl;

  // 袋价改变时，自动计算个价（袋价 * 12）
  function handlePriceBagChange(record) {
    const priceBag = Number(record.F_PriceBag) || 0;
    record.F_PriceUnit = priceBag * 12;
  }

  // 解析 F_Unit_Ratio，只取前两级，返回 { level1: { name, qty }, level2: { name, qty } } 或 null
  function getUnitRatioLevels(record) {
    const raw = record?.F_Unit_Ratio;
    if (!raw) return null;
    try {
      const str = typeof raw === 'string' ? raw : String(raw);
      const arr = JSON.parse(str);
      if (!Array.isArray(arr)) return null;
      if (arr.length < 2) return { level1: arr[0], level2: null };
      return { level1: arr[0], level2: arr[1] };
    } catch (e) {
      return null;
    }
  }

  // 一级数量（如箱子）改变时，按比例计算二级数量（如盒）：二级 = 一级 * level2.qty
  function handleNumLevel1Change(record) {
    const levels = getUnitRatioLevels(record);
    if (!levels) return;
    const qty1 = Number(record.F_NumLevel1) || 0;
    const ratio = Number(levels.level2?.qty) || 0;
    record.F_NumLevel2 = qty1 * ratio;
    record.F_Num = record.F_NumLevel1;
  }

  // 回显时：F_Num 存的是第一框（一级数量），据此填充 F_NumLevel1 并计算 F_NumLevel2
  function fillNumLevelsFromF_Num(row) {
    const levels = getUnitRatioLevels(row);
    if (!levels || row.F_Num == null || row.F_Num === '') return;
    const qty1 = Number(row.F_Num) || 0;
    const ratio = Number(levels.level2?.qty) || 0;
    row.F_NumLevel1 = qty1;
    row.F_NumLevel2 = qty1 * ratio;
  }

  // 计算单行金额：有两级单位时用一级数量（如箱）* 单价；否则用数量 * 单价
  function calculateRowAmount(record) {
    const levels = getUnitRatioLevels(record);
    const num = levels != null ? Number(record.F_NumLevel1) || 0 : Number(record.F_Num) || 0;
    const price = Number(record.F_Price) || 0;
    return num * price;
  }

  // 监听子表变化：有两级单位时采购数量按一级数量汇总；金额按一级数量*单价；F_Num 仍存二级数量（如盒）供库存等使用
  watch(
    () => state.dataForm.tableFieldc87c94,
    newVal => {
      let purchaseNum = 0;
      let total = 0;
      if (Array.isArray(newVal)) {
        for (let i = 0; i < newVal.length; i++) {
          const row = newVal[i] || {};
          const levels = getUnitRatioLevels(row);
          if (levels && row.F_NumLevel1 != null && row.F_NumLevel1 !== '') {
            if (levels.level2?.qty) {
              const qty1 = Number(row.F_NumLevel1) || 1;
              const ratio = Number(levels.level2?.qty) || 1;
              row.F_NumLevel2 = qty1 * ratio;
              row.F_Num = row.F_NumLevel1;
            } else {
              row.F_Num = row.F_NumLevel1;
            }
          }
          const amount = calculateRowAmount(row);
          // 将计算结果写回行上的 F_Amount（用于展示）
          row.F_Amount = amount;
          total += amount;
          purchaseNum += levels ? Number(row.F_NumLevel1) || 0 : Number(row.F_Num) || 0;
        }
      }
      // 写入表单顶级数量字段（采购数量：子表数量之和）
      state.dataForm.F_PurchaseNum = purchaseNum;
      // 写入表单顶级金额字段（数值型）
      state.dataForm.F_Money = total;
    },
    { deep: true },
  );

  // 监听主表供应商变化：同步到子表行
  watch(
    () => state.dataForm.F_SupplierId,
    newVal => {
      if (newVal && Array.isArray(state.dataForm.tableFieldc87c94)) {
        state.dataForm.tableFieldc87c94.forEach(row => {
          row.F_SupplierId = newVal;
        });
      }
    },
  );

  const getPrevDisabled = computed(() => state.currIndex === 0);
  const getNextDisabled = computed(() => state.currIndex === state.allList.length - 1);

  defineExpose({ init });

  function init(data) {
    state.submitType = 0;
    state.showContinueBtn = true;
    state.title = !data.id ? '新增' : '编辑';
    state.continueText = !data.id ? '确定并新增' : '确定并继续';
    setFormProps({ continueLoading: false });
    state.dataForm.id = data.id;
    openModal();
    state.allList = data.allList;
    state.currIndex = state.allList.length && data.id ? state.allList.findIndex(item => item.id === data.id) : 0;
    nextTick(() => {
      getForm().resetFields();
      state.dataForm.tableFieldc87c94 = [];
      state.dataForm.tableFielde82301 = [];
      state.tempGoodsSelector = [];
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
        F_SupplierId: undefined,
        F_Money: 0,
        F_DeliveryDate: undefined,
        F_PurchaseNum: 0,
        F_Description: undefined,
        F_CreatorUserId: undefined,
        F_CreatorTime: undefined,
        tableFieldc87c94: [],
        tableFielde82301: [],
      };
      state.tempGoodsSelector = [];
      getF_SupplierIdOptions();
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

      // ensure each row has a jnpfId used as rowKey
      for (let i = 0; i < (state.dataForm.tableFieldc87c94 || []).length; i++) {
        const element = state.dataForm.tableFieldc87c94[i];
        element.jnpfId = buildUUID();
      }
      for (let i = 0; i < (state.dataForm.tableFielde82301 || []).length; i++) {
        const element = state.dataForm.tableFielde82301[i];
        element.jnpfId = buildUUID();
      }

      // Populate display-only fields (商品编号/规格/图片) for existing rows.
      // Saved rows may only contain the relation id (F_CustomerId) because
      // system-controlled fields were removed before persist — here we fetch
      // the extra info from the configured data interface and inject it into rows
      // so the readonly columns show correctly.
      try {
        const rows = state.dataForm.tableFieldc87c94 || [];
        const idsToFetch: any[] = [];
        for (let i = 0; i < rows.length; i++) {
          const r = rows[i];
          if (!r) continue;
          // If the relation already returned an object, use it directly.
          if (r.F_CustomerId && typeof r.F_CustomerId === 'object') {
            handleCustomerSelectChange(r.F_CustomerId, r, i);
            fillNumLevelsFromF_Num(r);
            continue;
          }
          // If display fields are already present, skip.
          if (r.F_GoodsNo || r.F_Specification || r.F_Image) continue;
          if (r.F_CustomerId) idsToFetch.push(r.F_CustomerId);
        }
        if (idsToFetch.length) {
          const uniqueIds = Array.from(new Set(idsToFetch));
          const interfaceId = '2008721559174385664';
          const query = {
            ids: uniqueIds,
            interfaceId,
            propsValue: 'F_Id',
            relationField: 'F_GoodsName',
            paramList: [],
          };
          getDataInterfaceDataInfoByIds(interfaceId, query)
            .then(resp => {
              const list = resp.data || [];
              const idKey = 'F_Id';
              const map: Record<string, any> = {};
              list.forEach((item: any) => {
                if (item && item[idKey] != null) map[String(item[idKey])] = item;
              });
              for (let i = 0; i < rows.length; i++) {
                const r = rows[i];
                if (!r) continue;
                const found = map[String(r.F_CustomerId)];
                if (found) {
                  // reuse existing helper to map fields into the row
                  handleCustomerSelectChange(found, r, i);
                  fillNumLevelsFromF_Num(r);
                }
              }
            })
            .catch(() => {
              // ignore errors - fallback is showing empty display fields
            });
        }
      } catch (e) {
        // swallow any unexpected errors here to avoid blocking UI
      }

      // 将现有子表行的关联 id 同步到顶部选择器，保持 selector 与子表一致
      try {
        const remainingIds = (state.dataForm.tableFieldc87c94 || [])
          .map((r: any) => {
            // 优先使用缓存的完整对象
            if (r && r._F_CustomerObj && (r._F_CustomerObj.F_Id || r._F_CustomerObj.id)) {
              return r._F_CustomerObj.F_Id ?? r._F_CustomerObj.id;
            }
            // 如果 F_CustomerId 是对象，尝试取常见 id 字段
            if (r && r.F_CustomerId && typeof r.F_CustomerId === 'object') {
              return r.F_CustomerId.F_Id ?? r.F_CustomerId.id ?? r.F_CustomerId.value;
            }
            return r && r.F_CustomerId ? r.F_CustomerId : undefined;
          })
          .filter((v: any) => v !== undefined && v !== null);
        state.tempGoodsSelector = remainingIds;
      } catch (e) {
        // ignore
      }

      getF_SupplierIdOptions();
      changeLoading(false);
    });
  }
  async function handleSubmit() {
    try {
      const values = await getForm()?.validate();
      if (!values) return;
      // 验证子表数量不能为空
      const rows = state.dataForm.tableFieldc87c94 || [];
      for (let i = 0; i < rows.length; i++) {
        if (rows[i].F_NumLevel1 == null || rows[i].F_NumLevel1 === '') {
          createMessage.warning(`第${i + 1}行数量不能为空`);
          return;
        }
      }
      setFormProps({ confirmLoading: true });
      // clone dataForm and remove system-controlled fields from tableFieldc87c94 before submit
      const payload = cloneDeep(state.dataForm);
      try {
        const cols: any[] = unref(tableFieldc87c94Columns) || [];
        if (Array.isArray(payload.tableFieldc87c94)) {
          payload.tableFieldc87c94 = payload.tableFieldc87c94.map((row: any) => {
            const newRow: any = { ...row };
            for (let i = 0; i < cols.length; i++) {
              const cur = cols[i];
              if (cur.jnpfKey === 'complexHeader' && Array.isArray(cur.children)) {
                for (let j = 0; j < cur.children.length; j++) {
                  const child = cur.children[j];
                  if (child.isSystemControl) delete newRow[child.key];
                }
              } else {
                if (cur.isSystemControl) delete newRow[cur.key];
              }
            }
            return newRow;
          });
        }
      } catch (e) {
        // if anything goes wrong here, fall back to original dataForm
      }

      const formMethod = payload.id ? update : create;
      formMethod(payload)
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
  /**
   * 当在弹窗中选择商品后，把返回的数据字段赋值到表格行的展示字段上
   * selected 可能是对象或数组（multiple 情况），record 为当前行对象
   */
  function handleGoodsSelectorChange(selected: any, option) {
    if (!selected) {
      /* 兜底 */
      if (!Array.isArray(state.dataForm.tableFieldc87c94)) {
        state.dataForm.tableFieldc87c94 = [];
      }
      const optionIdSet = new Set(option.map(o => o.F_Id));
      /* 1. 删掉已取消勾选的行 */
      state.dataForm.tableFieldc87c94 = state.dataForm.tableFieldc87c94.filter(item => optionIdSet.has(item.F_CustomerId));
      state.tempGoodsSelector = state.tempGoodsSelector.filter(item => optionIdSet.has(item));
      return;
    }

    // 清空现有子表数据
    // state.dataForm.tableFieldc87c94 = [];
    // state.selectedtableFieldc87c94RowKeys = [];

    const interfaceId = '2008721559174385664';

    const buildAndPushRow = (data: any, index: number) => {
      // Determine the id value to bind to the selector (component expects id/primitive)
      let relationId: any = data;
      let fullObj: any = null;
      if (data && typeof data === 'object') {
        fullObj = data;
        relationId = data.F_Id ?? data.FId ?? data.id ?? data.value ?? relationId;
      }
      const exist = state.dataForm.tableFieldc87c94.some(item => item.F_CustomerId === data.F_Id);
      if (!exist) {
        const newRow: any = {
          jnpfId: buildUUID(),
          // Bind selector value to the id (primitive) so the popup select shows correctly
          F_CustomerId: relationId,
          // keep the fetched object for filling display fields / future use
          _F_CustomerObj: fullObj,
          F_GoodsNo: undefined,
          F_Specification: undefined,
          F_Image: undefined,
          F_SupplierId: state.dataForm.F_SupplierId || undefined,
          F_Num: undefined,
          F_NumLevel1: undefined,
          F_NumLevel2: undefined,
          F_Unit_Ratio: undefined,
          F_Price: undefined,
          F_PriceBag: undefined,
          F_PriceUnit: undefined,
          F_Amount: undefined,
          F_Description: undefined,
          F_CreatorUserId: undefined,
          F_CreatorTime: undefined,
        };
        // 使用现有的方法来填充商品信息（传入完整对象优先）
        handleCustomerSelectChange(fullObj || relationId, newRow, index);
        state.dataForm.tableFieldc87c94.push(newRow);
      }
    };

    // Helper to detect whether selection items are objects (already contain fields)
    const itemsAreObjects = (arr: any[]) => {
      if (!arr || !arr.length) return false;
      const first = arr[0];
      return first && typeof first === 'object' && (first.F_Id || first.F_GoodsName || first.F_GoodsNo);
    };

    if (Array.isArray(selected)) {
      // 多选：可能是对象数组（完整数据）或 id 数组（需要拉取）
      if (itemsAreObjects(selected)) {
        selected.forEach((goods, index) => buildAndPushRow(goods, index));
        state.tempGoodsSelector = selected.slice();
      } else {
        // selected 是 id 数组，按 id 拉取完整数据后再填充行
        const ids = Array.from(new Set(selected.map((s: any) => String(s))));
        const query = {
          ids,
          interfaceId,
          propsValue: 'F_Id',
          relationField: 'F_GoodsName',
          paramList: [],
        };
        getDataInterfaceDataInfoByIds(interfaceId, query)
          .then(resp => {
            const list = (resp && resp.data) || [];
            const map: Record<string, any> = {};
            list.forEach((item: any) => {
              if (item && item.F_Id != null) map[String(item.F_Id)] = item;
            });
            for (let i = 0; i < ids.length; i++) {
              const id = ids[i];
              const found = map[String(id)];
              buildAndPushRow(found || id, i);
            }
            state.tempGoodsSelector = ids.slice();
          })
          .catch(err => {
            // 拉取失败则回退为用原 id 填充（至少保留关联关系）
            console.error('getDataInterfaceDataInfoByIds error', err);
            try {
              createMessage.error(typeof err === 'object' ? JSON.stringify(err) : String(err));
            } catch (e) {
              // ignore
            }
            selected.forEach((goods, index) => buildAndPushRow(goods, index));
            state.tempGoodsSelector = selected.slice();
          });
      }
    } else {
      // 单选：可能是对象或单个 id
      const single = selected;
      if (single && typeof single === 'object') {
        buildAndPushRow(single, 0);
        state.tempGoodsSelector = [single];
      } else {
        const id = String(single);
        const query = {
          ids: [id],
          interfaceId,
          propsValue: 'F_Id',
          relationField: 'F_GoodsName',
          paramList: [],
        };
        getDataInterfaceDataInfoByIds(interfaceId, query)
          .then(resp => {
            const found = (resp && resp.data && resp.data[0]) || null;
            buildAndPushRow(found || id, 0);
            state.tempGoodsSelector = [found || id];
          })
          .catch(err => {
            console.error('getDataInterfaceDataInfoByIds error', err);
            try {
              createMessage.error(typeof err === 'object' ? JSON.stringify(err) : String(err));
            } catch (e) {
              // ignore
            }
            buildAndPushRow(id, 0);
            state.tempGoodsSelector = [id];
          });
      }
    }
  }

  function handleCustomerSelectChange(selected: any, record: any, rowIndex: number) {
    const data = Array.isArray(selected) ? selected[0] : selected;
    if (!data) {
      record.F_GoodsNo = undefined;
      record.F_Specification = undefined;
      record.F_Image = undefined;
      record.F_Unit_Ratio = undefined;
      return;
    }
    // 尝试使用常见字段名进行赋值，若后端字段不同可根据实际字段调整
    record.F_GoodsNo = data.F_GoodsNo ?? data.GoodsNo ?? data.F_GoodsName ?? data.name ?? record.F_GoodsNo;
    record.F_Specification = data.F_Specification ?? data.Specification ?? data.spec ?? record.F_Specification;
    record.F_Unit_Ratio = data.F_Unit_Ratio ?? record.F_Unit_Ratio;
    // 处理图片字段：支持直接url、相对路径、数组或JSON字符串
    let imgVal = data.F_Image ?? data.Image ?? data.img ?? data.picture ?? data.fileUrl ?? data.url ?? data.imageUrl ?? null;
    try {
      if (typeof imgVal === 'string' && imgVal.trim().startsWith('[')) {
        const parsed = JSON.parse(imgVal);
        if (Array.isArray(parsed) && parsed.length) {
          const first = parsed[0];
          imgVal = first.url ?? first.fileUrl ?? first.value ?? first.thumbUrl ?? first.name ?? imgVal;
        }
      } else if (Array.isArray(imgVal) && imgVal.length) {
        const first = imgVal[0];
        imgVal = first.url ?? first.fileUrl ?? first.value ?? first.thumbUrl ?? first.name ?? imgVal[0];
      }
    } catch (e) {
      // ignore parse errors
    }
    if (!imgVal) {
      record.F_Image = undefined;
    } else {
      // 如果已经是完整地址就直接用，否则加上 apiUrl 前缀
      if (/^https?:\/\//i.test(String(imgVal))) {
        record.F_Image = String(imgVal);
      } else {
        // 防止重复拼接
        const s = String(imgVal);
        record.F_Image = s.startsWith(apiUrl) ? s : apiUrl.replace(/\/$/, '') + (s.startsWith('/') ? '' : '/') + s;
      }
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
  function getF_SupplierIdOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2008457397298925568', query).then(res => {
      let data = res.data;
      state.optionsObj.F_SupplierIdOptions = Array.isArray(data) ? data : [];
    });
  }
  function removetableFieldc87c94Row(index, showConfirm = false) {
    const doRemove = () => {
      state.dataForm.tableFieldc87c94.splice(index, 1);
      // 同步更新顶部选择器：留下的行对应的 relation id 列表
      try {
        const remainingIds = (state.dataForm.tableFieldc87c94 || [])
          .map((r: any) => {
            // 优先使用我们保存的完整对象
            if (r && r._F_CustomerObj && (r._F_CustomerObj.F_Id || r._F_CustomerObj.id)) {
              return r._F_CustomerObj.F_Id ?? r._F_CustomerObj.id;
            }
            // 如果 F_CustomerId 是对象，尝试取常见 id 字段
            if (r && r.F_CustomerId && typeof r.F_CustomerId === 'object') {
              return r.F_CustomerId.F_Id ?? r.F_CustomerId.id ?? r.F_CustomerId.value;
            }
            return r && r.F_CustomerId ? r.F_CustomerId : undefined;
          })
          .filter((v: any) => v !== undefined && v !== null);
        state.tempGoodsSelector = remainingIds;
      } catch (e) {
        // ignore
      }
    };

    if (showConfirm) {
      createConfirm({
        iconType: 'warning',
        title: '提示',
        content: '此操作将永久删除该数据, 是否继续?',
        onOk: doRemove,
      });
    } else {
      doRemove();
    }
  }
  function addForSelect(data) {
    for (let i = 0; i < data.length; i++) {
      let item = { ...state.tableRows[state.currVmodel], ...data[i], jnpfId: buildUUID() };
      state.dataForm[state.currVmodel].push(cloneDeep(item));
      if (state[state.currVmodel + 'DefaultExpandAll']) state[state.currVmodel + 'innerActiveKey'].push(item.jnpfId);
    }
    // 如果是向采购计划子表添加，更新顶部批量选择器的值以保持两者同步
    if (state.currVmodel === 'tableFieldc87c94') {
      try {
        const remainingIds = (state.dataForm.tableFieldc87c94 || [])
          .map((r: any) => {
            if (r && r._F_CustomerObj && (r._F_CustomerObj.F_Id || r._F_CustomerObj.id)) {
              return r._F_CustomerObj.F_Id ?? r._F_CustomerObj.id;
            }
            if (r && r.F_CustomerId && typeof r.F_CustomerId === 'object') {
              return r.F_CustomerId.F_Id ?? r.F_CustomerId.id ?? r.F_CustomerId.value;
            }
            return r && r.F_CustomerId ? r.F_CustomerId : undefined;
          })
          .filter((v: any) => v !== undefined && v !== null);
        state.tempGoodsSelector = remainingIds;
      } catch (e) {
        // ignore
      }
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
  const listPagination = computed(() => ({
    current: state.listCurrentPage,
    pageSize: state.listPageSize,
    size: 'small',
    showSizeChanger: true,
    pageSizeOptions: [20, 50, 100],
    showTotal: (total: number) => `共 ${total} 条`,
    onShowSizeChange: (_current: number, size: number) => {
      state.listPageSize = size;
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
