<template>
  <BasicModal v-bind="$attrs" @register="registerModal" width="70%" :minHeight="100" okText="确定" cancelText="取消" @ok="handleSubmit" :closeFunc="onClose">
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
          <!-- 后端 source1 映射为候选项，用户可选择添加到用料清单并填充表单 -->
          <a-col :span="24" v-if="initialTableRows && initialTableRows.length">
            <div style="margin-bottom:12px;font-weight:500;color:#333">从物料分析中选择要外协的商品</div>
            <div v-for="(row, idx) in initialTableRows" :key="row.jnpfId || idx" style="display:flex;align-items:center;gap:12px;padding:8px 0;border-bottom:1px dashed #f0f0f0">
              <div style="flex:1">
                <div style="font-size:12px;color:#666">商品</div>
                <div>{{ row.F_GoodsName || row.F_GoodsId || '-' }}</div>
              </div>
              <div style="width:220px">
                <div style="font-size:12px;color:#666">计划数/单位用量</div>
                <div>{{ row.F_Unit ?? '-' }}</div>
              </div>
              <div style="width:120px;text-align:right">
                <a-button size="small" type="primary" @click="fillOnly(row)">选择</a-button>
              </div>
            </div>
          </a-col>
          <a-col :span="8" class="ant-col-item" >
            <a-form-item name="F_OutsourceNo" :labelCol="{ style: { width: '100px' } }">
              <template #label>外协工单号</template>
              <JnpfInput
                v-model:value="dataForm.F_OutsourceNo"
                placeholder="请填写，忽略将自动生成"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" >
            <a-form-item name="F_GoodsId" :labelCol="{ style: { width: '100px' } }">
              <template #label>商品</template>
              <JnpfPopupSelect
                v-model:value="dataForm.F_GoodsId"
                placeholder="请选择"
                :templateJson="optionsObj.F_GoodsIdTemplateJson"
                allowClear
                field="F_GoodsId"
                interfaceId="2008721559174385664"
                :columnOptions="optionsObj.F_GoodsIdOptions"
                relationField="F_GoodsName"
                propsValue="F_Id"
                :pageSize="20"
                popupType="dialog"
                popupTitle="选择数据"
                popupWidth="60%"
                hasPage
                :style="{
                  width: '100%',
                }"
                :extraOptions="optionsObj.F_GoodsId" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_PlanNum" :labelCol="{ style: { width: '100px' } }">
              <template #label>计划数</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_PlanNum"
                placeholder="请输入"
                :step="1.0"
                :controls="true"
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" >
            <a-form-item name="F_BOMId" :labelCol="{ style: { width: '100px' } }">
              <template #label>BOM</template>
              <JnpfPopupSelect
                v-model:value="dataForm.F_BOMId"
                :formData="state.dataForm"
                placeholder="请选择"
                :templateJson="dynamicF_BOMIdTemplateJson"
                allowClear
                field="F_BOMId"
                interfaceId="2013188646957617152"
                :columnOptions="optionsObj.F_BOMIdOptions"
                relationField="F_BomCode"
                propsValue="id"
                :pageSize="20"
                popupType="dialog"
                popupTitle="选择数据"
                popupWidth="800px"
                hasPage
                :disabled="!dataForm.F_GoodsId"
                @change="handleBOMChange"
                :style="{
                  width: '100%',
                }"
                :extraOptions="optionsObj.F_BOMId" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" >
            <a-form-item name="F_SupplierId" :labelCol="{ style: { width: '100px' } }">
              <template #label>供应商</template>
              <JnpfSelect
                v-model:value="dataForm.F_SupplierId"
                placeholder="请选择"
                :options="optionsObj.F_SupplierIdOptions"
                :fieldNames="optionsObj.F_SupplierIdProps"
                allowClear
                showSearch
                @change="handleSupplierChange"
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" >
            <a-form-item name="F_ContactPerson" :labelCol="{ style: { width: '100px' } }">
              <template #label>供应商联系人</template>
              <JnpfInput
                v-model:value="dataForm.F_ContactPerson"
                placeholder="请输入"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" >
            <a-form-item name="F_ContactPhone" :labelCol="{ style: { width: '150px' } }">
              <template #label>供应商联系人电话</template>
              <JnpfInput
                v-model:value="dataForm.F_ContactPhone"
                placeholder="请输入"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" >
            <a-form-item name="F_Region" :labelCol="{ style: { width: '100px' } }">
              <template #label>供应商地区</template>
              <JnpfAreaSelect
                v-model:value="dataForm.F_Region"
                placeholder="请选择"
                allowClear
                :level="2"
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" >
            <a-form-item name="F_DetailAddress" :labelCol="{ style: { width: '100px' } }">
              <template #label>供应商详细地址</template>
              <JnpfInput
                v-model:value="dataForm.F_DetailAddress"
                placeholder="请输入"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" >
            <a-form-item name="F_Price" :labelCol="{ style: { width: '100px' } }">
              <template #label>外协单价</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_Price"
                placeholder="请输入"
                :controls="false"
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" >
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
          <a-col :span="8" class="ant-col-item" >
            <a-form-item name="F_Priority" :labelCol="{ style: { width: '100px' } }">
              <template #label>优先级</template>
              <JnpfSelect
                v-model:value="dataForm.F_Priority"
                placeholder="请选择"
                :options="optionsObj.F_PriorityOptions"
                :fieldNames="optionsObj.F_PriorityProps"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" >
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
          <a-col :span="8" class="ant-col-item" >
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
          <a-col :span="8" class="ant-col-item" >
            <a-form-item name="F_Files" :labelCol="{ style: { width: '100px' } }">
              <template #label>图纸</template>
              <JnpfUploadFile
                v-model:value="dataForm.F_Files"
                buttonText="点击上传"
                pathType="defaultPath"
                :fileSize="10"
                sizeUnit="MB"
                :limit="9"
                :sortRule="['2']"
                timeFormat="YYYYMMDD" />
            </a-form-item>
          </a-col>
          <!-- <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_State')">
            <a-form-item name="F_State" :labelCol="{ style: { width: '100px' } }">
              <template #label>状态</template>
              <JnpfInput
                v-model:value="dataForm.F_State"
                placeholder="请输入"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col> -->
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_Description')">
            <a-form-item name="F_Description" :labelCol="{ style: { width: '100px' } }">
              <template #label>描述</template>
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
          <!-- <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_CreatorUserId')">
            <a-form-item name="F_CreatorUserId" :labelCol="{ style: { width: '100px' } }">
              <template #label>创建用户</template>
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
          <!-- <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="用料清单" :bordered="false" />
              <a-table
                :data-source="dataForm.tableField7a8044"
                :columns="tableField7a8044Columns"
                size="small"
                rowKey="jnpfId"
                :pagination="false"
                :scroll="{ x: 'max-content' }"
                :rowSelection="gettableField7a8044RowSelection">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}
                  <BasicHelp :text="column.tipLabel" v-if="column.tipLabel" />
                </template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                  <template v-if="column.key === 'F_GoodsId'">
                    <JnpfPopupSelect
                      v-model:value="record.F_GoodsId"
                      placeholder="请选择"
                      :templateJson="optionsObj.tableField7a8044_F_GoodsIdTemplateJson"
                      allowClear
                      :field="'F_GoodsId' + index"
                      interfaceId="2008721559174385664"
                      :columnOptions="optionsObj.tableField7a8044_F_GoodsIdOptions"
                      relationField="F_GoodsName"
                      propsValue="F_Id"
                      :pageSize="20"
                      popupType="dialog"
                      popupTitle="选择数据"
                      popupWidth="60%"
                      hasPage
                      :style="{
                        width: '100%',
                      }"
                      :extraOptions="optionsObj.tableField7a8044_F_GoodsId" />
                  </template>
                  <template v-if="column.key === 'F_Unit'">
                    <JnpfInputNumber
                      v-model:value="record.F_Unit"
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
                      <a-button class="action-btn" type="link" @click="copytableField7a8044Row(index)" size="small">{{
                        t('common.copyText', '复制')
                      }}</a-button>
                      <a-button class="action-btn" type="link" color="error" @click="removetableField7a8044Row(index, true)" size="small">{{
                        t('common.delText', '删除')
                      }}</a-button>
                    </a-space>
                  </template>
                </template>
              </a-table>
              <a-space class="input-table-footer-btn">
                <a-button type="primary" preIcon="icon-ym icon-ym-btn-add" @click="addtableField7a8044Row">{{ t('common.add1Text', '添加') }}</a-button>
                <a-button type="danger" preIcon="icon-ym icon-ym-btn-clearn" @click="batchRemovetableField7a8044Row(true)">{{
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
  import { getInfo } from './helper/api';
  import { create as createOutsource, update as updateOutsource, getSupplierContactInfo, getBOMGoodsList } from '../aProdOutsource/helper/api';
  import { getInfo as getGoodsInfo } from '../aGoods/helper/api';
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
    tableField7a8044outerActiveKey: number[];
    tableField7a8044innerActiveKey: string[];
    tableField7a8044DefaultExpandAll: boolean;
    selectedtableField7a8044RowKeys: any[];
    title: string;
    continueText: string;
    allList: any[];
    currIndex: number;
    isContinue: boolean;
    submitType: number;
    showContinueBtn: boolean;
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
  const initialTableRows = ref<any[]>([]);
  const tableField7a8044Columns: any[] = computed(() => {
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
        title: '单位用量',
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
      // {
      //   title: '创建用户',
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
    list = list.filter(o => hasFormP('tableField7a8044-' + o.formP));
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
  const gettableField7a8044RowSelection = computed(() => {
    const rowSelection = {
      selectedRowKeys: state.selectedtableField7a8044RowKeys,
      onChange: (selectedRowKeys: string[]) => {
        state.selectedtableField7a8044RowKeys = (selectedRowKeys || []).sort().reverse();
      },
    };
    return rowSelection;
  });
  const state = reactive<State>({
    dataForm: {
      id: '',
      F_OutsourceNo: undefined,
      F_GoodsId: undefined,
      F_PlanNum: undefined,
      F_BOMId: undefined,
      F_SupplierId: undefined,
      F_ContactPerson: undefined,
      F_ContactPhone: undefined,
      F_Region: [],
      F_DetailAddress: undefined,
      F_Price: undefined,
      F_DeliveryDate: undefined,
      F_Priority: undefined,
      F_PlanStartDate: undefined,
      F_PlanEndDate: undefined,
      F_Files: [],
      F_State: undefined,
      F_Description: undefined,
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
      tableField7a8044: [],
    },
    tableField7a8044outerActiveKey: [0],
    tableField7a8044innerActiveKey: [],
    tableField7a8044DefaultExpandAll: true,
    selectedtableField7a8044RowKeys: [],
    dataRule: {
      F_GoodsId: [
        {
          required: true,
          message: '请输入商品',
          trigger: 'change',
        },
      ],
      F_PlanNum: [
        {
          required: true,
          message: '请输入计划数',
          trigger: ['blur', 'change'],
        },
      ],
      F_ContactPhone: [
        {
          pattern: /^1[3456789]\d{9}$|^0\d{2,3}-?\d{7,8}$/,
          message: t('sys.validate.phone', '请输入正确的联系方式'),
          trigger: 'blur',
        },
      ],
     
    },
    optionsObj: {
      F_GoodsIdOptions: [
        {
          value: 'F_GoodsNo',
          label: '商品编号',
        },
        {
          value: 'F_GoodsName',
          label: '商品名称',
        },
        {
          value: 'F_Unit',
          label: '单位',
        },
        {
          value: 'F_Specification',
          label: '规格',
        },
        {
          value: 'F_SalePrice',
          label: '销售单价',
        },
        {
          value: 'F_CostPrice',
          label: '成本单价',
        },
      ],
      F_GoodsIdTemplateJson: [],
      F_BOMIdOptions: [
        {
          value: 'F_BomCode',
          label: 'BOM编号',
        },
        {
          value: 'F_BomName',
          label: 'BOM名称',
        },
      ],
      F_BOMIdTemplateJson: [
        {
          defaultValue: '',
          field: 'goodsId',
          dataType: 'varchar',
          required: 0,
          fieldName: '合同ID',
          relationField: '2008839754363310080',
          jnpfKey: null,
          complexHeaderList: null,
          sourceType: 2,
          isChildren: false,
          IsMultiple: false,
        },
      ],
      F_SupplierIdProps: { label: 'F_SupplierName', value: 'F_Id' },
      F_PriorityProps: { label: 'fullName', value: 'enCode' },
      tableField7a8044_F_GoodsIdOptions: [
        {
          value: 'F_GoodsNo',
          label: '商品编号',
        },
        {
          value: 'F_GoodsName',
          label: '商品名称',
        },
        {
          value: 'F_Unit',
          label: '单位',
        },
        {
          value: 'F_Specification',
          label: '规格',
        },
        {
          value: 'F_SalePrice',
          label: '销售单价',
        },
        {
          value: 'F_CostPrice',
          label: '成本单价',
        },
      ],
      tableField7a8044_F_GoodsIdTemplateJson: [],
      F_GoodsId: [],
      F_BOMId: [],
    },
    currVmodel: '',
    currTableConf: {},
    tableRows: {
      tableField7a8044: {
        enabledmark: undefined,
        F_GoodsId: undefined,
        F_Unit: undefined,
        F_Description: undefined,
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
  });

  // 动态生成 F_BOMIdTemplateJson，relationField 绑定到 F_GoodsId
  const dynamicF_BOMIdTemplateJson = computed(() => [
    {
      defaultValue: '',
      field: 'goodsId',
      dataType: 'varchar',
      required: 0,
      fieldName: '合同ID',
      relationField: dataForm.value.F_GoodsId || '',
      jnpfKey: null,
      complexHeaderList: null,
      sourceType: 2,
      isChildren: false,
      IsMultiple: false,
    },
  ]);

  const { title, continueText, showContinueBtn, dataForm, submitType, dataRule, optionsObj } = toRefs(state);

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
    // 记录外部传入的初始表格行（如果有），并确保每行含有 jnpfId
    initialTableRows.value = (data.tableField7a8044 || []).map((r: any) => ({
      ...r,
      jnpfId: r.jnpfId || buildUUID(),
      F_GoodsName: r.F_GoodsName || undefined,
    }));
    // 异步加载商品名称以替换商品ID显示
    enrichInitialRowsWithGoodsName();
    openModal();
    state.allList = data.allList;
    state.currIndex = state.allList.length && data.id ? state.allList.findIndex(item => item.id === data.id) : 0;
    nextTick(() => {
      getForm().resetFields();
      // 不在此处覆盖表格数据，等待 initData 根据 initialTableRows 处理
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
        F_OutsourceNo: undefined,
        F_GoodsId: undefined,
        F_PlanNum: undefined,
        F_BOMId: undefined,
        F_SupplierId: undefined,
        F_ContactPerson: undefined,
        F_ContactPhone: undefined,
        F_Region: [],
        F_DetailAddress: undefined,
        F_Price: undefined,
        F_DeliveryDate: undefined,
        F_Priority: undefined,
        F_PlanStartDate: undefined,
        F_PlanEndDate: undefined,
        F_Files: [],
        F_State: "生产中",
        F_Description: undefined,
        F_CreatorUserId: undefined,
        F_CreatorTime: undefined,
        tableField7a8044: initialTableRows.value && initialTableRows.value.length ? initialTableRows.value : [],
      };
      getF_SupplierIdOptions();
      getF_PriorityOptions();
      // 如果只有一行数据，则将该行的商品与单位放到表单顶层字段（便于快速新建外协单）
      if (initialTableRows.value && initialTableRows.value.length === 1) {
        const row = initialTableRows.value[0];
        state.dataForm.F_GoodsId = row.F_GoodsId;
        state.dataForm.F_PlanNum = row.F_Unit;
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
      for (let i = 0; i < state.dataForm.tableField7a8044.length; i++) {
        const element = state.dataForm.tableField7a8044[i];
        element.jnpfId = buildUUID();
      }
      getF_SupplierIdOptions();
      getF_PriorityOptions();
      changeLoading(false);
    });
  }
  async function handleSubmit() {
    try {
      const values = await getForm()?.validate();
      if (!values) return;
      setFormProps({ confirmLoading: true });
      const formMethod = state.dataForm.id ? updateOutsource : createOutsource;
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
  function getF_PriorityOptions() {
    getDictionaryDataSelector('2013182853290004480').then(res => {
      state.optionsObj.F_PriorityOptions = res.data.list;
    });
  }
  // 将 initialTableRows 中的商品ID转换为商品名称（异步）
  async function enrichInitialRowsWithGoodsName() {
    if (!initialTableRows.value || !initialTableRows.value.length) return;
    for (let i = 0; i < initialTableRows.value.length; i++) {
      const row = initialTableRows.value[i];
      if (row && row.F_GoodsId) {
        try {
          const res = await getGoodsInfo(row.F_GoodsId);
          const goods = res.data;
          row.F_GoodsName = goods?.F_GoodsName || row.F_GoodsName || row.F_GoodsId;
        } catch (e) {
          row.F_GoodsName = row.F_GoodsName || row.F_GoodsId;
        }
      } else {
        row.F_GoodsName = row.F_GoodsName || row.F_GoodsId || '';
      }
    }
  }
  function handleSupplierChange(supplierId) {
    if (supplierId) {
      getSupplierContactInfo(supplierId)
        .then(res => {
          const contactInfo = res.data;
          state.dataForm.F_ContactPerson = contactInfo.F_ContactPerson;
          state.dataForm.F_ContactPhone = contactInfo.F_ContactPhone;
          // 兼容后端返回 array / json-string / 用 "/" 拼接 的三种情况
          let regionValue = [];
          if (Array.isArray(contactInfo.F_Region)) {
            regionValue = contactInfo.F_Region;
          } else if (typeof contactInfo.F_Region === 'string') {
            try {
              const parsed = JSON.parse(contactInfo.F_Region);
              if (Array.isArray(parsed)) {
                regionValue = parsed;
              } else if (contactInfo.F_Region.includes('/')) {
                regionValue = contactInfo.F_Region.split('/');
              } else if (parsed !== null && parsed !== undefined) {
                regionValue = [String(parsed)];
              } else {
                regionValue = [];
              }
            } catch (e) {
              regionValue = contactInfo.F_Region ? contactInfo.F_Region.split('/') : [];
            }
          } else {
            regionValue = [];
          }
          state.dataForm.F_Region = regionValue;
          state.dataForm.F_DetailAddress = contactInfo.F_DetailAddress;
        })
        .catch(() => {
          // 如果获取失败，清空相关字段
          state.dataForm.F_ContactPerson = undefined;
          state.dataForm.F_ContactPhone = undefined;
          state.dataForm.F_Region = [];
          state.dataForm.F_DetailAddress = undefined;
        });
    } else {
      // 如果清空选择，也清空相关字段
      state.dataForm.F_ContactPerson = undefined;
      state.dataForm.F_ContactPhone = undefined;
      state.dataForm.F_Region = [];
      state.dataForm.F_DetailAddress = undefined;
    }
  }

  // 处理BOM选择变化
  function handleBOMChange(bomId) {
    if (bomId) {
      // 调用后端API获取BOM用料清单
      getBOMGoodsList(bomId)
        .then(res => {
          const goodsList = res.data || [];
          // 清空现有用料清单
          state.dataForm.tableField7a8044 = [];
          // 将BOM用料清单填充到表格中
          goodsList.forEach(item => {
            const rowData = {
              jnpfId: buildUUID(),
              F_GoodsId: item.F_GoodsId,
              F_Unit: item.F_Unit,
              F_Description: '',
              F_CreatorUserId: undefined,
              F_CreatorTime: undefined,
            };
            state.dataForm.tableField7a8044.push(rowData);
          });
        })
        .catch(() => {
          createMessage.error('获取BOM用料清单失败');
          state.dataForm.tableField7a8044 = [];
        });
    } else {
      // 如果清空BOM选择，清空用料清单
      state.dataForm.tableField7a8044 = [];
    }
  }

  function addtableField7a8044Row() {
    let item = {
      jnpfId: buildUUID(),
      F_GoodsId: undefined,
      F_Unit: undefined,
      F_Description: undefined,
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
    };
    state.dataForm.tableField7a8044.push(item);
    state.tableField7a8044innerActiveKey.push(item.jnpfId);
  }
  // （已移除：添加并填充功能，保留仅填充选择按钮）

  // 仅填充顶层字段，不添加行
  function fillOnly(item) {
    if (!item) return;
    state.dataForm.F_GoodsId = item.F_GoodsId;
    state.dataForm.F_PlanNum = item.F_Unit;
    createMessage.success('已填充表单字段');
  }
  function removetableField7a8044Row(index, showConfirm = false) {
    if (showConfirm) {
      createConfirm({
        iconType: 'warning',
        title: '提示',
        content: '此操作将永久删除该数据, 是否继续?',
        onOk: () => {
          state.dataForm.tableField7a8044.splice(index, 1);
        },
      });
    } else {
      state.dataForm.tableField7a8044.splice(index, 1);
    }
  }
  function copytableField7a8044Row(index) {
    let item = cloneDeep(state.dataForm.tableField7a8044[index]);
    let copyData = {};
    for (let i = 0; i < unref(tableField7a8044Columns).length; i++) {
      const cur = unref(tableField7a8044Columns)[i];
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
    state.dataForm.tableField7a8044.push(copyItem);
    state.tableField7a8044innerActiveKey.push(copyItem.jnpfId);
  }
  function batchRemovetableField7a8044Row(showConfirm = false) {
    if (!state.selectedtableField7a8044RowKeys.length) return createMessage.error('请选择一条数据');
    const handleBatchRemove = () => {
      state.dataForm.tableField7a8044 = state.dataForm.tableField7a8044.filter(o => !state.selectedtableField7a8044RowKeys.includes(o.jnpfId));
      nextTick(() => {
        state.selectedtableField7a8044RowKeys = [];
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
