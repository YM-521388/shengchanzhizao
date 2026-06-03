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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_OutsourceNo')">
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_GoodsId')">
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
          <!-- <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_BOMId')">
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
          </a-col> -->
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_PlanNum')">
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_SupplierId')">
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_ContactPerson')">
            <a-form-item name="F_ContactPerson" :labelCol="{ style: { width: '100px' } }">
              <template #label>联系人</template>
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_ContactPhone')">
            <a-form-item name="F_ContactPhone" :labelCol="{ style: { width: '100px' } }">
              <template #label>联系人电话</template>
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_Region')">
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_DetailAddress')">
            <a-form-item name="F_DetailAddress" :labelCol="{ style: { width: '100px' } }">
              <template #label>详细地址</template>
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_Price')">
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_Priority')">
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_PlanStartDate')">
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_PlanEndDate')">
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_Files')">
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
          <!-- <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_State')">
            <a-form-item name="F_State" :labelCol="{ style: { width: '100px' } }">
              <template #label>状态</template>
              <JnpfSelect
                v-model:value="dataForm.F_State"
                placeholder="请选择"
                :options="optionsObj.F_StateOptions"
                :fieldNames="optionsObj.F_StateProps"
                allowClear
                :style="{
                  width: '100%',
                }" />
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

          <!-- <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="用料清单" :bordered="false" />
              <a-col :span="24" class="ant-col-item">
                <a-form-item name="GoodsList" :labelCol="{ style: { width: '100px' } }">
                  <template #label>选择用料</template>
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
                    hasPage
                    :style="{
                      width: '100%',
                    }"
                    :extraOptions="optionsObj.GoodsList" />
                </a-form-item>
              </a-col>
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
                      popupWidth="100%"
                      disabled
                      hasPage
                      :style="{
                        width: '100%',
                      }"
                      :extraOptions="optionsObj.tableField7a8044_F_GoodsId" />
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
                  <template v-if="column.key === 'F_Unit'">
                    <a-space>
                      <JnpfInputNumber
                        v-model:value="record.F_Unit"
                        placeholder="请输入"
                        :controls="false"
                        :style="{
                          width: '100%',
                          borderColor: record.F_Unit > record.F_InventoryNum ? '#ff4d4f' : undefined,
                        }"
                        @change="handleF_UnitChange(record)" />
                      <span>{{ record.F_NumUnit }}</span>
                    </a-space>
                    <div v-if="record.F_Unit > record.F_InventoryNum" style="color: #ff4d4f; font-size: 12px; margin-top: 4px">
                      用量不能大于库存数量 {{ record.F_InventoryNum }}
                    </div>
                  </template>
                  <template v-if="column.key === 'F_UnitTwo'">
                    <JnpfInput
                      v-model:value="record.F_UnitTwo"
                      :min="1"
                      :controls="false"
                      disabled
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
                      <a-button class="action-btn" type="link" color="error" @click="removetableField7a8044Row(index, true)" size="small">{{
                        t('common.delText', '删除')
                      }}</a-button>
                    </a-space>
                  </template>
                </template>
              </a-table>
              <a-space class="input-table-footer-btn">
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
  import { create, update, getInfo, getSupplierContactInfo, getBOMGoodsList } from './helper/api';
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
  import { getDictionaryDataSelector } from '@/api/systemData/dictionary';
  import { getDataInterfaceRes, getDataInterfaceDataInfoByIds } from '@/api/systemData/dataInterface';
  import dayjs from 'dayjs';
  import { cloneDeep } from 'lodash-es';
  import { buildUUID } from '@/utils/uuid';
  import { CaretRightOutlined } from '@ant-design/icons-vue';
  import { systemComponentsList } from '@/components/FormGenerator/src/helper/config';
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
        title: '单位用量',
        dataIndex: 'F_Unit',
        key: 'F_Unit',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '120px',
        fixed: false,
        formP: 'F_Unit',
        isSystemControl: false,
      },
      {
        title: '单位',
        dataIndex: 'F_UnitTwo',
        key: 'F_UnitTwo',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_UnitTwo',
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
    GoodsList: [],
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
      F_Priority: '1',
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
      F_Priority: [
        {
          required: true,
          message: '请选择优先级',
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
        {
          value: 'F_InventoryNum',
          label: '库存',
        },
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
          value: 'F_Specification',
          label: '商品规格',
        },
        {
          value: 'F_Type',
          label: '商品类型',
        },
        // {
        //   value: 'F_Unit',
        //   label: '单位',
        // },
        // {
        //   value: 'F_Specification',
        //   label: '规格',
        // },
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
      F_StateProps: { label: 'fullName', value: 'enCode' },
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
          value: 'F_InventoryNum',
          label: '库存',
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
    tempGoodsSelector: [],
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

  const { title, continueText, showContinueBtn, dataForm, submitType, dataRule, optionsObj, tempGoodsSelector } = toRefs(state);

  const getPrevDisabled = computed(() => state.currIndex === 0);
  const getNextDisabled = computed(() => state.currIndex === state.allList.length - 1);
  // 临时保存外部传入的预填数据（通过 defineExpose 的 init 调用）
  const prefillData = ref<any>(null);
  defineExpose({ init });

  function init(data) {
    // 保存预填数据（只保留我们关心的字段），供 initData/getData 合并使用
    const incoming = data || {};
    // 编辑模式下不需要预填数据，后端会返回完整数据
    if (!incoming.id) {
      prefillData.value = {
        F_GoodsId: incoming.F_GoodsId,
        F_PlanNum: incoming.F_PlanNum,
      };
    } else {
      prefillData.value = null;
    }
    state.submitType = 0;
    state.showContinueBtn = true;
    state.title = !incoming.id ? '新增外协工单' : '编辑';
    setFormProps({ continueLoading: false });
    state.dataForm.id = incoming.id;
    openModal();
    // 防御性处理 allList 可能为 undefined 的情况
    state.allList = Array.isArray(incoming.allList) ? incoming.allList : [];
    state.currIndex = state.allList.length && incoming.id ? state.allList.findIndex(item => item.id === incoming.id) : 0;
    nextTick(() => {
      getForm().resetFields();
      state.dataForm.tableField7a8044 = [];
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
        F_Priority: '1',
        F_PlanStartDate: undefined,
        F_PlanEndDate: undefined,
        F_Files: [],
        F_State: '1',
        F_Description: undefined,
        F_CreatorUserId: undefined,
        F_CreatorTime: undefined,
        tableField7a8044: [],
      };
      state.GoodsList = [];
      getF_SupplierIdOptions();
      getF_PriorityOptions();
      getF_StateOptions();
      if (getLeftTreeActiveInfo) state.dataForm = { ...state.dataForm, ...(getLeftTreeActiveInfo() || {}) };
      // 合并外部预填数据（如从生产计划传入的 F_GoodsId/F_PlanNum）
      if (prefillData.value) {
        state.dataForm = { ...state.dataForm, ...(prefillData.value || {}) };
        prefillData.value = null;
      }
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
      // for (let i = 0; i < state.dataForm.tableField7a8044.length; i++) {
      //   const element = state.dataForm.tableField7a8044[i];
      //   element.jnpfId = buildUUID();
      //   state.GoodsList.push(state.dataForm.tableField7a8044[i].F_GoodsId);
      // }
      // 如果外部传入预填数据（init 调用时提供），合并到获取到的数据中
      // 只合并有有效值的字段，避免覆盖后端返回的有效数据
      if (prefillData.value) {
        const filteredPrefill = {};
        for (const key in prefillData.value) {
          const val = prefillData.value[key];
          // 只保留非 undefined 的值
          if (val !== undefined) {
            filteredPrefill[key] = val;
          }
        }
        if (Object.keys(filteredPrefill).length > 0) {
          state.dataForm = { ...state.dataForm, ...filteredPrefill };
        }
        prefillData.value = null;
      }
      getF_SupplierIdOptions();
      getF_PriorityOptions();
      getF_StateOptions();
      changeLoading(false);
    });
  }
  async function handleSubmit() {
    try {
      const values = await getForm()?.validate();
      if (!values) return;

      // 验证单位用量不能大于库存数量
      // const invalidRows = state.dataForm.tableField7a8044.filter(item => item.F_Unit && item.F_InventoryNum && item.F_Unit > item.F_InventoryNum);
      // if (invalidRows.length > 0) {
      //   const errorMsg = invalidRows
      //     .map(item => {
      //       const goods = state.optionsObj.tableField7a8044_F_GoodsIdOptions.find((g: any) => g.F_Id === item.F_GoodsId);
      //       const goodsName = goods ? goods.F_GoodsName : item.F_GoodsId;
      //       return `${goodsName}：用量 ${item.F_Unit} 大于库存 ${item.F_InventoryNum}`;
      //     })
      //     .join('；');
      //   createMessage.error(`用料清单验证失败：${errorMsg}`);
      //   return false;
      // }

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
  function getF_StateOptions() {
    getDictionaryDataSelector('2013819135649255424').then(res => {
      state.optionsObj.F_StateOptions = res.data.list;
    });
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
  async function handleBOMChange(bomId) {
    if (bomId) {
      try {
        const res = await getBOMGoodsList(bomId);
        const goodsList = res.data || [];

        const tableData = [];
        const goodsIdList = [];
        const extraOptionsList = [];

        for (const item of goodsList) {
          const rowData = {
            jnpfId: buildUUID(),
            F_GoodsId: item.F_GoodsId,
            F_Specification: item?.F_Specification,
            F_GoodsNo: item.F_GoodsNo,
            F_Unit: item.F_Unit,
            F_InventoryNum: item.F_InventoryNum,
            F_Description: '',
            F_CreatorUserId: undefined,
            F_CreatorTime: undefined,
          };
          // 获取单位信息
          try {
            const unitRes = await getGoodsUnit(item.F_GoodsId);
            rowData.F_UnitTwo = unitRes?.data;
            rowData.F_NumUnit = unitRes?.data?.split('/').length > 1 ? unitRes.data.split('/')[1] : unitRes?.data?.split('/')[0];
          } catch {
            rowData.F_UnitTwo = '';
            rowData.F_NumUnit = '';
          }

          tableData.push(rowData);
          goodsIdList.push(item.F_GoodsId);
          // 关键：同步更新 extraOptions，组件才能根据 ID 显示标签名
          extraOptionsList.push({
            id: item.F_GoodsId,
            F_GoodsName: item.F_GoodsName,
            F_GoodsNo: item.F_GoodsNo,
            F_Specification: item.F_Specification,
          });
        }

        // 统一赋值，确保响应式更新
        state.dataForm.tableField7a8044 = tableData;
        state.GoodsList = goodsIdList;
        state.optionsObj.GoodsList = extraOptionsList;
      } catch {
        createMessage.error('获取BOM用料清单失败');
        state.dataForm.tableField7a8044 = [];
        state.GoodsList = [];
        state.optionsObj.GoodsList = [];
      }
    } else {
      state.dataForm.tableField7a8044 = [];
      state.GoodsList = [];
      state.optionsObj.GoodsList = [];
    }
  }

  function removetableField7a8044Row(index, showConfirm = false) {
    if (showConfirm) {
      createConfirm({
        iconType: 'warning',
        title: '提示',
        content: '此操作将永久删除该数据, 是否继续?',
        onOk: () => {
          state.dataForm.tableField7a8044.splice(index, 1);
          state.GoodsList.splice(index, 1);
        },
      });
    } else {
      state.dataForm.tableField7a8044.splice(index, 1);
      state.GoodsList.splice(index, 1);
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

  // 处理单位用量变化
  function handleF_UnitChange(record) {
    // 触发响应式更新,确保错误提示及时显示
    const index = state.dataForm.tableField7a8044.findIndex(item => item.jnpfId === record.jnpfId);
    if (index > -1) {
      state.dataForm.tableField7a8044[index] = { ...record };
    }
  }

  function GoodsListBtn(val, option) {
    /* 兜底保证是数组 */
    if (!Array.isArray(state.dataForm.tableField7a8044)) {
      state.dataForm.tableField7a8044 = [];
    }

    /* 1. 把 option 里的 id 做成 Set，方便比对 */
    const optionIdSet = new Set(option.map(o => o.id));

    /* 2. 删除本地已不在 option 里的行 */
    state.dataForm.tableField7a8044 = state.dataForm.tableField7a8044.filter(item => optionIdSet.has(item.F_GoodsId));

    /* 3. 把 option 里本地还没有的行加进来 */
    option.forEach(o => {
      const exist = state.dataForm.tableField7a8044.findIndex(item => item.F_GoodsId === o.id) !== -1;
      if (!exist) {
        getGoodsUnit(o.id).then(res => {
          const newRow = {
            jnpfId: buildUUID(),
            F_GoodsId: o.id,
            F_GoodsNo: o.F_GoodsNo,
            F_Specification: o.F_Specification,
            F_NumUnit: res.data?.split('/').length > 1 ? res.data?.split('/')[1] : res.data?.split('/')[0],
            F_UnitTwo: res?.data,
            F_InventoryNum: o.F_InventoryNum,
            F_Unit: 1,
            F_Description: undefined,
            F_CreatorTime: undefined,
          };
          state.dataForm.tableField7a8044.push(newRow);
          state.tableField7a8044innerActiveKey.push(newRow.jnpfId);
        });
      }
    });
  }
</script>
