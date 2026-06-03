<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    width="80%"
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_BomCode')">
            <a-form-item name="F_BomCode" :labelCol="{ style: { width: '100px' } }">
              <template #label>BOM编号</template>
              <JnpfInput
                v-model:value="dataForm.F_BomCode"
                placeholder="请输入"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_BomName')">
            <a-form-item name="F_BomName" :labelCol="{ style: { width: '100px' } }">
              <template #label>BOM名称</template>
              <JnpfInput
                v-model:value="dataForm.F_BomName"
                placeholder="请输入"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_CategoryId')">
            <a-form-item name="F_CategoryId" :labelCol="{ style: { width: '100px' } }">
              <template #label>类别</template>
              <JnpfCascader
                v-model:value="dataForm.F_CategoryId"
                placeholder="请选择"
                :options="optionsObj.F_CategoryIdOptions"
                :fieldNames="optionsObj.F_CategoryIdProps"
                allowClear
                :style="{
                  width: '100%',
                }" />
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
                interfaceId="2029803158963884032"
                :columnOptions="optionsObj.F_GoodsIdOptions"
                relationField="F_GoodsName"
                propsValue="id"
                :pageSize="20"
                popupType="dialog"
                popupTitle="选择数据"
                popupWidth="70%"
                hasPage
                :style="{
                  width: '100%',
                }"
                :extraOptions="optionsObj.F_GoodsId" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_IsDefault')">
            <a-form-item name="F_IsDefault" :labelCol="{ style: { width: '100px' } }">
              <template #label>默认BOM</template>
              <JnpfSwitch v-model:value="dataForm.F_IsDefault" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_State')">
            <a-form-item name="F_State" :labelCol="{ style: { width: '100px' } }">
              <template #label>备注</template>
              <JnpfTextarea
                v-model:value="dataForm.F_State"
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
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="dataForm.id">
            <a-form-item name="F_Reason" :labelCol="{ style: { width: '100px' } }">
              <template #label><span class="required-sign" v-if="dataForm.id">*</span>修改内容</template>
              <JnpfTextarea
                v-model:value="dataForm.F_Reason"
                placeholder="请输入修改内容（编辑时必填）"
                allowClear
                :autoSize="{
                  minRows: 3,
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
              <JnpfGroupTitle content="增加商品" :bordered="false" />
              <div class="mb-15px">
                <JnpfPopupSelect
                  v-model:value="tempGoodsSelector"
                  placeholder="请选择商品（可多选）"
                  :templateJson="optionsObj.tempGoodsSelectorTemplateJson"
                  allowClear
                  :field="'tempGoodsSelector'"
                  interfaceId="2030840782923108352"
                  :columnOptions="optionsObj.tempGoodsSelectorOptions"
                  relationField="F_GoodsName"
                  propsValue="id"
                  :pageSize="20"
                  popupType="dialog"
                  popupTitle="选择商品"
                  popupWidth="75%"
                  hasPage
                  multiple
                  :style="{
                    width: '100%',
                  }"
                  @change="handleGoodsSelectorChange" />
              </div>
              <a-table
                :data-source="dataForm.tableFieldd2a80d"
                :columns="tableFieldd2a80dColumns"
                size="small"
                rowKey="jnpfId"
                :pagination="listPagination"
                :scroll="{ x: 'max-content' }"
                :rowSelection="gettableFieldd2a80dRowSelection">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ formatRowIndex(record, index) }}</template>
                  <template v-if="column.key === 'F_GoodsId'">
                    <JnpfPopupSelect
                      v-model:value="record.F_GoodsId"
                      placeholder="请选择"
                      :templateJson="optionsObj.tableFieldd2a80d_F_GoodsIdTemplateJson"
                      allowClear
                      :field="'F_GoodsId' + index"
                      interfaceId="2008721559174385664"
                      :columnOptions="optionsObj.tableFieldd2a80d_F_GoodsIdOptions"
                      relationField="F_GoodsName"
                      propsValue="F_Id"
                      :pageSize="20"
                      popupType="dialog"
                      popupTitle="选择数据"
                      popupWidth="70%"
                      hasPage
                      :style="{
                        width: '100%',
                      }"
                      :extraOptions="optionsObj.tableFieldd2a80d_F_GoodsId"
                      @change="(val, selectData) => handleGoodsSelect(record, val, selectData)" />
                  </template>
                  <template v-if="column.key === 'F_GoodsNo'">
                    <div>{{ record.F_GoodsNo }}</div>
                  </template>
                  <template v-if="column.key === 'F_Specification'">
                    <div>{{ record.F_Specification }}</div>
                  </template>
                  <template v-if="column.key === 'F_Source'">
                    <div>{{ record.F_Source }}</div>
                  </template>
                  <template v-if="column.key === 'F_Unitsp'">
                    <div>{{ record.F_Unitsp }}</div>
                  </template>
                  <template v-if="column.key === 'F_SalePrice'">
                    <div>{{ record.F_SalePrice }}</div>
                  </template>
                  <template v-if="column.key === 'F_CostPrice'">
                    <div>{{ record.F_CostPrice }}</div>
                  </template>
                  <template v-if="column.key === 'F_CostAmount'">
                    <div>{{ record.F_CostAmount }}</div>
                  </template>
                  <template v-if="column.key === 'F_Process'">
                    <JnpfSelect
                      v-model:value="record.F_Process"
                      placeholder="请选择"
                      :options="optionsObj.tableFieldd2a80d_F_ProcessOptions"
                      :fieldNames="optionsObj.tableFieldd2a80d_F_ProcessProps"
                      allowClear
                      showSearch
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_Unit'">
                    <a-space>
                      <JnpfInputNumber
                        v-model:value="record.F_Unit"
                        placeholder="请输入"
                        :min="1"
                        :controls="false"
                        :style="{
                          width: '100%',
                          borderColor: record.F_InventoryNum != null && record.F_Unit > record.F_InventoryNum ? '#ff4d4f' : undefined,
                        }"
                        @change="handleF_UnitChange(record)" />
                      <span>{{ record.F_NumUnit }}</span>
                    </a-space>
                    <div v-if="record.F_InventoryNum != null && record.F_Unit > record.F_InventoryNum" style="color: #ff4d4f; font-size: 12px; margin-top: 4px">
                      用量不能大于库存数量 {{ record.F_InventoryNum }}
                    </div>
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
                      <!-- <a-button class="action-btn" type="link" @click="addChildRow(index)" size="small">添加下级</a-button> -->
                      <!-- <a-button class="action-btn" type="link" @click="copytableFieldd2a80dRow(index)" size="small">{{
                        t('common.copyText', '复制')
                      }}</a-button> -->
                      <a-button class="action-btn" type="link" color="error" @click="removetableFieldd2a80dRow(index, true)" size="small">{{
                        t('common.delText', '删除')
                      }}</a-button>
                    </a-space>
                  </template>
                </template>
              </a-table>
              <a-space class="input-table-footer-btn">
                <!-- <a-button type="primary" preIcon="icon-ym icon-ym-btn-add" @click="addtableFieldd2a80dRow">{{ t('common.add1Text', '添加') }}</a-button>
                <a-button type="danger" preIcon="icon-ym icon-ym-btn-clearn" @click="batchRemovetableFieldd2a80dRow(true)">{{
                  t('common.batchDelText', '批量删除')
                }}</a-button> -->
              </a-space>
            </a-form-item>
          </a-col>
          <!-- 操作日志已移至详情页显示 -->
          <a-col :span="24" class="ant-col-item" v-show="false" v-if="hasFormP('F_CreatorUserId')">
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
          <a-col :span="24" class="ant-col-item" v-show="false" v-if="hasFormP('F_CreatorTime')">
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
          </a-col>
        </a-row>
      </a-form>
    </a-row>
  </BasicModal>
  <!-- 修改原因字段将内嵌在表单中（见备注之后） -->
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
  import { getDictionaryDataSelector } from '@/api/systemData/dictionary';
  import { getDataInterfaceRes, getDataInterfaceDataInfoByIds } from '@/api/systemData/dataInterface';
  import dayjs from 'dayjs';
  import { cloneDeep } from 'lodash-es';
  import { buildUUID } from '@/utils/uuid';
  import { CaretRightOutlined } from '@ant-design/icons-vue';
  import { systemComponentsList } from '@/components/FormGenerator/src/helper/config';
  import { usePermission } from '@/hooks/web/usePermission';

  interface State {
    listPageSize: number;
    listCurrentPage: number;
    dataForm: any;
    dataRule: any;
    optionsObj: any;
    currVmodel: any;
    currTableConf: any;
    addTableConf: any;
    tableRows: any;
    tableFieldd2a80douterActiveKey: number[];
    tableFieldd2a80dinnerActiveKey: string[];
    tableFieldd2a80dDefaultExpandAll: boolean;
    selectedtableFieldd2a80dRowKeys: any[];
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
  const /* reason modal removed */ _unused1 = null;
  const tableFieldd2a80dColumns: any[] = computed(() => {
    let list = [
      {
        title: '商品名称',
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
        title: '编码',
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
        title: '商品来源',
        dataIndex: 'F_Source',
        key: 'F_Source',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Source',
        isSystemControl: false,
      },
      {
        title: '投料工序',
        dataIndex: 'F_Process',
        key: 'F_Process',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '40',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Process',
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
        dataIndex: 'F_Unitsp',
        key: 'F_Unitsp',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Unitsp',
        isSystemControl: false,
      },
      {
        title: '销售单价',
        dataIndex: 'F_SalePrice',
        key: 'F_SalePrice',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_SalePrice',
        isSystemControl: false,
      },
      {
        title: '成本单价',
        dataIndex: 'F_CostPrice',
        key: 'F_CostPrice',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_CostPrice',
        isSystemControl: false,
      },
      {
        title: '成本金额（元）',
        dataIndex: 'F_CostAmount',
        key: 'F_CostAmount',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_CostAmount',
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

    list = list.filter(o => hasFormP('tableFieldd2a80d-' + o.formP));
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
  const gettableFieldd2a80dRowSelection = computed(() => {
    const rowSelection = {
      selectedRowKeys: state.selectedtableFieldd2a80dRowKeys,
      onChange: (selectedRowKeys: string[]) => {
        state.selectedtableFieldd2a80dRowKeys = (selectedRowKeys || []).sort().reverse();
      },
    };
    return rowSelection;
  });
  const state = reactive<State>({
    listPageSize: 20,
    listCurrentPage: 1,
    dataForm: {
      id: '',
      F_BomCode: undefined,
      F_BomName: undefined,
      F_CategoryId: [],
      F_GoodsId: undefined,
      F_IsDefault: 0,
      F_State: undefined,
      tableFieldd2a80d: [],
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
    },
    tableFieldd2a80douterActiveKey: [0],
    tableFieldd2a80dinnerActiveKey: [],
    tableFieldd2a80dDefaultExpandAll: true,
    selectedtableFieldd2a80dRowKeys: [],
    dataRule: {
      F_BomName: [
        {
          required: true,
          message: '请输入BOM名称',
          trigger: 'blur',
        },
      ],
      F_Reason: [
        {
          validator: (_rule: any, value: any) => {
            if (state.dataForm.id) {
              if (!value || !String(value).trim()) return Promise.reject('请输入修改内容');
            }
            return Promise.resolve();
          },
          trigger: 'blur',
        },
      ],
    },
    optionsObj: {
      F_CategoryIdProps: {
        label: 'F_Name',
        value: 'F_Id',
        children: 'children',
      },
      tempGoodsSelectorOptions: [
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
          label: '规格',
        },
        {
          value: 'F_Type',
          label: '商品类型',
        },
        {
          value: 'F_Source',
          label: '商品来源',
        },
        // {
        //   value: 'F_Unit',
        //   label: '单位',
        // },
        {
          value: 'F_SalePrice',
          label: '销售单价',
        },
        {
          value: 'F_CostPrice',
          label: '成本单价',
        },
        {
          value: 'F_Remark',
          label: '备注',
        },
      ],
      tempGoodsSelectorTemplateJson: [],
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
          label: '规格',
        },
        // {
        //   value: 'F_CategoryId',
        //   label: '类型',
        // },
        {
          value: 'F_Type',
          label: '商品类型',
        },
        // {
        //   value: 'F_Unit',
        //   label: '单位',
        // },
        {
          value: 'F_SalePrice',
          label: '销售单价',
        },
        {
          value: 'F_CostPrice',
          label: '成本单价',
        },
        // {
        //   value: 'F_Image',
        //   label: '商品图片',
        // },
        {
          value: 'F_Source',
          label: '商品来源',
        },
        {
          value: 'F_Remark',
          label: '备注',
        },
      ],
      F_GoodsIdTemplateJson: [],
      tableFieldd2a80d_F_GoodsIdOptions: [
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
          label: '规格',
        },
        // {
        //   value: 'F_CategoryId',
        //   label: '类型',
        // },
        // {
        //   value: 'F_Unit',
        //   label: '单位',
        // },
        {
          value: 'F_Type',
          label: '商品类型',
        },
        {
          value: 'F_Source',
          label: '商品来源',
        },
        {
          value: 'F_SalePrice',
          label: '销售单价',
        },
        {
          value: 'F_CostPrice',
          label: '成本单价',
        },
        // {
        //   value: 'F_Image',
        //   label: '商品图片',
        // },
        {
          value: 'F_Remark',
          label: '备注',
        },
      ],
      tableFieldd2a80d_F_GoodsIdTemplateJson: [],
      tableFieldd2a80d_F_ProcessOptions: [],
      tableFieldd2a80d_F_ProcessProps: { label: 'fullName', value: 'enCode' },
      F_GoodsId: [],
    },
    currVmodel: '',
    currTableConf: {},
    tableRows: {
      tableFieldd2a80d: {
        enabledmark: undefined,
        F_GoodsId: undefined,
        F_GoodsNo: undefined,
        F_Specification: undefined,
        F_Source: undefined,
        F_CostAmount: undefined,
        F_Process: undefined,
        F_InventoryNum: undefined,
        F_NumUnit: undefined,
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
  const { title, continueText, showContinueBtn, dataForm, submitType, dataRule, optionsObj } = toRefs(state);

  // 实时计算当前子表中的商品ID列表，用于双向关联
  const tempGoodsSelector = computed({
    get: () => {
      const goodsIds = state.dataForm.tableFieldd2a80d.map((row: any) => row.F_GoodsId).filter((id: any) => !!id);
      return goodsIds.length > 0 ? goodsIds : undefined;
    },
    set: val => {
      // 当选择器的值被外部改变时（如用户在弹窗中取消选中）
      // 同步更新子表：移除不在新值中的商品
      if (val === undefined) {
        // 清空所有商品
        state.dataForm.tableFieldd2a80d = [];
      } else {
        const newGoodsIds = Array.isArray(val) ? val : [val];
        // 过滤掉不在新值中的行
        state.dataForm.tableFieldd2a80d = state.dataForm.tableFieldd2a80d.filter((row: any) => newGoodsIds.includes(row.F_GoodsId));
      }
    },
  });

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
      state.dataForm.tableFieldd2a80d = [];
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
        F_BomCode: undefined,
        F_BomName: undefined,
        F_CategoryId: [],
        F_GoodsId: undefined,
        F_IsDefault: 1,
        F_State: undefined,
        tableFieldd2a80d: [],
        F_CreatorUserId: undefined,
        F_CreatorTime: undefined,
      };
      getF_CategoryIdOptions();
      gettableFieldd2a80d_F_ProcessOptions();
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
      for (let i = 0; i < state.dataForm.tableFieldd2a80d.length; i++) {
        const element = state.dataForm.tableFieldd2a80d[i];
        element.jnpfId = buildUUID();
      }
      // If backend returned F_BomId referencing a parent row's F_Id, copy it to F_ParentId
      // so the front-end hierarchical helpers (formatRowIndex, addChildRow, etc.) work as expected.
      try {
        const rows = state.dataForm.tableFieldd2a80d || [];
        for (let i = 0; i < rows.length; i++) {
          const row = rows[i];
          if (row && row.F_BomId) {
            // set F_ParentId to the parent's row id (backend stores parent in F_BomId)
            row.F_ParentId = row.F_BomId;
          }
        }
      } catch (e) {
        // ignore mapping errors
      }
      getF_CategoryIdOptions();
      gettableFieldd2a80d_F_ProcessOptions();
      // 操作日志改到详情页显示，Form 不再加载日志
      // 请求并填充子表行的商品额外信息（编码、规则）
      nextTick(() => {
        fillTableGoodsExtraInfo().finally(() => {
          changeLoading(false);
        });
      });
    });
  }

  async function fillTableGoodsExtraInfo() {
    try {
      const rows = state.dataForm.tableFieldd2a80d || [];
      const ids = Array.from(new Set(rows.map((r: any) => r.F_GoodsId).filter((v: any) => !!v)));
      if (!ids.length) return;
      const query: any = {
        ids,
        interfaceId: '2008721559174385664',
        propsValue: 'F_Id',
        relationField: 'F_GoodsName',
        paramList: [],
      };
      const res = await getDataInterfaceDataInfoByIds('2008721559174385664', query);
      const list = (res && res.data) || [];
      for (let i = 0; i < rows.length; i++) {
        const row = rows[i];
        if (!row.F_GoodsId) continue;
        const info = list.find((it: any) => it.F_Id == row.F_GoodsId || it.F_Id == row.F_GoodsId + '');
        if (info) {
          row.F_GoodsNo = info.F_GoodsNo ?? row.F_GoodsNo;
          row.F_Specification = info.F_Specification ?? row.F_Specification;
          // populate display-only price fields from backend info
          row.F_SalePrice = info.F_SalePrice ?? row.F_SalePrice;
          row.F_CostPrice = info.F_CostPrice ?? row.F_CostPrice;
          // populate 商品来源 from backend info
          row.F_Source = info.F_Source ?? row.F_Source;
          // populate 商品单位 from backend info (display-only)
          row.F_Unitsp = info.F_Unit ?? row.F_Unitsp;
          getGoodsUnit(row.F_GoodsId).then(res => {
            row.F_NumUnit = parseNumUnitFromGoodsUnit(res?.data) ?? row.F_NumUnit;
          });
          // compute cost amount = 单位用量 * 成本单价
          row.F_CostAmount = safeMultiply(row.F_Unit, row.F_CostPrice, 4);
        }
      }
    } catch (e) {
      // ignore
    }
  }
  async function handleSubmit() {
    try {
      const values = await getForm()?.validate();
      if (!values) return;
      // If editing (has id), require F_Reason to be filled in the form
      if (state.dataForm.id) {
        if (!state.dataForm.F_Reason || !String(state.dataForm.F_Reason).trim()) {
          createMessage.error('请输入修改内容');
          return;
        }
      }
      // create a sanitized payload so F_GoodsNo and F_Specification are not submitted
      setFormProps({ confirmLoading: true });
      const formMethod = state.dataForm.id ? update : create;
      const payload = cloneDeep(state.dataForm);
      // For each subtable row, compute and store its displayed sequence (序号) into F_BomId
      // so the backend receives the row's index label (e.g. "1", "1.1").
      if (payload.tableFieldd2a80d && Array.isArray(payload.tableFieldd2a80d)) {
        payload.tableFieldd2a80d.forEach((r: any, i: number) => {
          try {
            // Use the existing formatRowIndex helper (which builds labels like "1" / "1.1")
            // formatRowIndex reads from state.dataForm which mirrors the current rows order,
            // so calling it here yields the displayed sequence for the row at index i.
            r.F_BomId = formatRowIndex(r, i);
          } catch (err) {
            // fallback: use numeric position (1-based) if anything goes wrong
            r.F_BomId = String(i + 1);
          }
          if (r && typeof r === 'object') {
            // Ensure numeric fields are numbers for backend DTO (decimal?)
            if (r.F_Unit !== undefined && r.F_Unit !== null && r.F_Unit !== '') {
              const unitNum = Number(r.F_Unit);
              r.F_Unit = Number.isFinite(unitNum) ? unitNum : null;
            }
            // Remove front-end only fields before submit
            delete r.jnpfId;
            delete r.F_ParentId;
            // Remove display-only fields before submit
            delete r.F_GoodsNo;
            delete r.F_Specification;
            delete r.F_Source;
            delete r.F_CostAmount;
            delete r.F_Unitsp;
            delete r.F_SalePrice;
            delete r.F_CostPrice;
            delete r.F_NumUnit;
            delete r.F_InventoryNum;
          }
        });
      }
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
        .catch((err: any) => {
          setFormProps({ confirmLoading: false });
          createMessage.error(err?.message || '保存失败');
        });
    } catch (err: any) {
      // ant-design-vue validate 会自己标红字段，这里避免完全静默
      if (!err?.errorFields) {
        createMessage.error(err?.message || '保存失败');
      }
      setFormProps({ confirmLoading: false });
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
  function getF_CategoryIdOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2008713719516893184', query).then(res => {
      let data = res.data;
      state.optionsObj.F_CategoryIdOptions = Array.isArray(data) ? data : [];
    });
  }
  function gettableFieldd2a80d_F_ProcessOptions() {
    getDictionaryDataSelector('2008728907364306944').then(res => {
      state.optionsObj.tableFieldd2a80d_F_ProcessOptions = res.data.list;
    });
  }
  function addtableFieldd2a80dRow() {
    let item = {
      jnpfId: buildUUID(),
      F_GoodsId: undefined,
      F_Source: undefined,
      F_CostAmount: undefined,
      F_Unitsp: undefined,
      F_Process: undefined,
      F_InventoryNum: undefined,
      F_NumUnit: undefined,
      F_Unit: undefined,
      F_Description: undefined,
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
    };
    state.dataForm.tableFieldd2a80d.push(item);
    state.tableFieldd2a80dinnerActiveKey.push(item.jnpfId);
  }
  function removetableFieldd2a80dRow(index, showConfirm = false) {
    if (showConfirm) {
      createConfirm({
        iconType: 'warning',
        title: '提示',
        content: '此操作将永久删除该数据, 是否继续?',
        onOk: () => {
          state.dataForm.tableFieldd2a80d.splice(index, 1);
        },
      });
    } else {
      state.dataForm.tableFieldd2a80d.splice(index, 1);
    }
  }
  function copytableFieldd2a80dRow(index) {
    let item = cloneDeep(state.dataForm.tableFieldd2a80d[index]);
    let copyData = {};
    for (let i = 0; i < unref(tableFieldd2a80dColumns).length; i++) {
      const cur = unref(tableFieldd2a80dColumns)[i];
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
    state.dataForm.tableFieldd2a80d.push(copyItem);
    state.tableFieldd2a80dinnerActiveKey.push(copyItem.jnpfId);
  }
  /**
   * Add a child row immediately after the clicked row.
   * The new row's F_ParentId is set to the parent's id (F_Id if exists, otherwise parent's jnpfId).
   */
  function addChildRow(index) {
    const parent = state.dataForm.tableFieldd2a80d[index];
    if (!parent) return;
    const parentId = parent.F_Id ?? parent.jnpfId;
    const item = {
      jnpfId: buildUUID(),
      F_GoodsId: undefined,
      F_Source: undefined,
      F_CostAmount: undefined,
      F_Unitsp: undefined,
      F_Process: undefined,
      F_InventoryNum: undefined,
      F_NumUnit: undefined,
      F_Unit: undefined,
      F_Description: undefined,
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
      F_ParentId: parentId,
    };
    // insert after the parent's last descendant so the flat list keeps hierarchical order
    const rows = state.dataForm.tableFieldd2a80d;
    let insertionIndex = index + 1;
    for (let i = index + 1; i < rows.length; i++) {
      const current = rows[i];
      let curParentId = current.F_ParentId;
      let isDescendant = false;
      // walk up the ancestor chain to see if `parentId` is an ancestor of `current`
      while (curParentId) {
        if (curParentId == parentId) {
          isDescendant = true;
          break;
        }
        const parentRowIndex = rows.findIndex(r => (r.F_Id ?? r.jnpfId) == curParentId);
        if (parentRowIndex === -1) break;
        curParentId = rows[parentRowIndex].F_ParentId;
      }
      if (isDescendant) {
        insertionIndex = i + 1;
      } else {
        break;
      }
    }
    rows.splice(insertionIndex, 0, item);
    state.tableFieldd2a80dinnerActiveKey.push(item.jnpfId);
  }

  /**
   * Format the row index for display. Parent rows show "1", child rows show "1.1", "1.2", etc.
   */
  function formatRowIndex(record, index) {
    try {
      const rows = state.dataForm.tableFieldd2a80d || [];
      function buildLabel(i) {
        const r = rows[i];
        if (!r || !r.F_ParentId) {
          // top-level position: count top-level rows up to and including i
          const topPos = rows.slice(0, i + 1).filter(rr => !rr.F_ParentId).length;
          return String(topPos);
        }
        const parentId = r.F_ParentId;
        const parentIndex = rows.findIndex(rr => (rr.F_Id ?? rr.jnpfId) == parentId);
        if (parentIndex === -1) return String(i + 1);
        const parentLabel = buildLabel(parentIndex);
        const siblingsBefore = rows.slice(parentIndex + 1, i).filter(rr => rr.F_ParentId == parentId).length;
        return `${parentLabel}.${siblingsBefore + 1}`;
      }
      return buildLabel(index);
    } catch (e) {
      return String(index + 1);
    }
  }
  function batchRemovetableFieldd2a80dRow(showConfirm = false) {
    if (!state.selectedtableFieldd2a80dRowKeys.length) return createMessage.error('请选择一条数据');
    const handleBatchRemove = () => {
      state.dataForm.tableFieldd2a80d = state.dataForm.tableFieldd2a80d.filter(o => !state.selectedtableFieldd2a80dRowKeys.includes(o.jnpfId));
      nextTick(() => {
        state.selectedtableFieldd2a80dRowKeys = [];
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
  function handleGoodsSelectorChange(val, selectData) {
    console.log('222', val, selectData);
    // When multiple products are selected, add them to the table
    if (!val || !selectData || !selectData.length) {
      // Clear or do nothing when deselected
      return;
    }
    // Get existing goods ids in the table to avoid duplicates
    const existingGoodsIds = state.dataForm.tableFieldd2a80d.map((row: any) => row.F_GoodsId).filter(Boolean);
    // Add each selected product to the table
    selectData.forEach((item: any) => {
      // 兼容老接口(返回 F_Id) 和新接口(返回 id)
      const itemId = item.F_Id || item.id;
      if (!itemId) return;
      if (!existingGoodsIds.includes(itemId)) {
        const unitQty = 1;
        const costPrice = Number(item.F_CostPrice) || 0;
        getGoodsUnit(itemId).then(res => {
          const newRow = {
            jnpfId: buildUUID(),
            F_GoodsId: itemId,
            F_GoodsNo: item.F_GoodsNo,
            F_Specification: item.F_Specification,
            F_Source: item.F_Source,
            F_CostPrice: item.F_CostPrice,
            F_SalePrice: item.F_SalePrice,
            // F_Unit is 单位用量 (decimal) - must be numeric for backend
            F_Unit: unitQty,
            // F_Unitsp is display-only 商品单位 (e.g. "PCS.")
            F_Unitsp: res?.data,
            F_NumUnit: parseNumUnitFromGoodsUnit(res?.data),
            F_CostAmount: safeMultiply(unitQty, costPrice, 4),
            F_Process: undefined,
            F_Description: undefined,
            F_CreatorUserId: undefined,
            F_CreatorTime: undefined,
          };
          state.dataForm.tableFieldd2a80d.push(newRow);
        });
      }
    });
  }
  function handleGoodsSelect(record, val, selectData) {
    // When a product is selected, populate its code and specification into the row.
    try {
      record.F_GoodsNo = selectData?.F_GoodsNo ?? selectData?.F_GoodsNo ?? record.F_GoodsNo;
      record.F_Specification = selectData?.F_Specification ?? selectData?.F_Specification ?? record.F_Specification;
      // fill display-only prices when selecting product
      record.F_SalePrice = selectData?.F_SalePrice ?? record.F_SalePrice;
      record.F_CostPrice = selectData?.F_CostPrice ?? record.F_CostPrice;
      // fill 商品来源 when selecting product
      record.F_Source = selectData?.F_Source ?? record.F_Source;
      getGoodsUnit(record.F_GoodsId).then(res => {
        record.F_NumUnit = parseNumUnitFromGoodsUnit(res?.data) ?? record.F_NumUnit;
      });
      // compute cost amount after selection
      record.F_CostAmount = safeMultiply(record.F_Unit, record.F_CostPrice, 4);
    } catch (e) {
      // ignore
    }
  }

  /** 与生产加工单一致：从商品单位字符串解析「数量旁展示」的单位（如 "箱/个" → "个"） */
  function parseNumUnitFromGoodsUnit(unitStr: string | undefined) {
    if (unitStr == null || unitStr === '') return undefined;
    const parts = String(unitStr).split('/');
    return parts.length > 1 ? parts[1] : parts[0];
  }

  /** 与生产加工单 handleF_UnitChange 一致：触发行替换以更新校验样式，并同步成本金额 */
  function handleF_UnitChange(record) {
    recomputeRowCostAmount(record);
    const index = state.dataForm.tableFieldd2a80d.findIndex(item => item.jnpfId === record.jnpfId);
    if (index > -1) {
      state.dataForm.tableFieldd2a80d[index] = { ...record };
    }
  }

  function recomputeRowCostAmount(record) {
    try {
      record.F_CostAmount = safeMultiply(record.F_Unit, record.F_CostPrice, 4);
    } catch (e) {
      // ignore
    }
  }
  /**
   * 安全乘法 - 解决 JavaScript 浮点数精度问题
   * @param a 乘数
   * @param b 被乘数
   * @param precision 保留小数位数，默认4位
   * @returns 乘积
   */
  function safeMultiply(a: any, b: any, precision: number = 4): number {
    const numA = Number(a) || 0;
    const numB = Number(b) || 0;
    const result = numA * numB;
    return parseFloat(result.toFixed(precision));
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
