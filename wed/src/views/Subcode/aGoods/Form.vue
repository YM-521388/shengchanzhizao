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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_GoodsNo')">
            <a-form-item name="F_GoodsNo" :labelCol="{ style: { width: '100px' } }">
              <template #label>商品编号</template>
              <JnpfInput
                v-model:value="dataForm.F_GoodsNo"
                placeholder="请输入，忽略将自动生成"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_GoodsName')">
            <a-form-item name="F_GoodsName" :labelCol="{ style: { width: '100px' } }">
              <template #label>商品名称</template>
              <JnpfInput
                v-model:value="dataForm.F_GoodsName"
                placeholder="请输入"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false"
                @blur="handleGoodsNameBlur"
                @input="handleGoodsNameInput" />
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
                :changeOnSelect="true"
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_Unit')">
            <a-form-item name="F_Unit" :labelCol="{ style: { width: '100px' } }">
              <template #label>单位</template>
              <JnpfCascader
                v-model:value="dataForm.F_Unit"
                placeholder="请选择"
                :options="optionsObj.F_UnitOptions"
                :fieldNames="optionsObj.F_UnitProps"
                allowClear
                showSearch
                :changeOnSelect="true"
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_Unit_Ratio')">
            <a-form-item name="F_Unit_Ratio" :labelCol="{ style: { width: '100px' } }">
              <template #label>单位比例</template>
              <template v-if="unitRatioList.length > 0">
                <div class="unit-ratio-table">
                  <table class="jnpf-unit-ratio-table">
                    <thead>
                      <tr>
                        <th>名称</th>
                        <th>数量</th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr v-for="(row, index) in unitRatioList" :key="row.id">
                        <td>{{ row.name }}</td>
                        <td>
                          <JnpfInputNumber
                            :value="row.qty"
                            placeholder="请输入"
                            :min="0"
                            :controls="true"
                            :disabled="index === 0"
                            :style="{ width: '100%' }"
                            @update:value="val => updateUnitRatioQty(index, val)" />
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>
              </template>
              <JnpfInput
                v-else
                v-model:value="dataForm.F_Unit_Ratio"
                placeholder="请先选择单位"
                allowClear
                disabled
                :style="{ width: '100%' }"
                :showCount="false" />
            </a-form-item>
          </a-col>

          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_Specification')">
            <a-form-item name="F_Specification" :labelCol="{ style: { width: '100px' } }">
              <template #label>规格</template>
              <JnpfInput
                v-model:value="dataForm.F_Specification"
                placeholder="请输入"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_Material')">
            <a-form-item name="F_Material" :labelCol="{ style: { width: '100px' } }">
              <template #label>材质</template>
              <JnpfInput
                v-model:value="dataForm.F_Material"
                placeholder="请输入"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_SalePrice')">
            <a-form-item name="F_SalePrice" :labelCol="{ style: { width: '100px' } }">
              <template #label>销售单价</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_SalePrice"
                placeholder="请输入"
                :step="1.0"
                :controls="true"
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_CostPrice')">
            <a-form-item name="F_CostPrice" :labelCol="{ style: { width: '100px' } }">
              <template #label>成本单价</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_CostPrice"
                placeholder="请输入"
                :controls="false"
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_CommissionFixed')">
            <a-form-item name="F_CommissionFixed" :labelCol="{ style: { width: '100px' } }">
              <template #label>售后佣金</template>
              <a-space size="small">
                <JnpfInputNumber
                  v-model:value="dataForm.F_CommissionFixedLeft"
                  placeholder="请输入"
                  :controls="false"
                  :style="{
                    width: '110px',
                  }"
                  @input="handleCommissionLeftInput" />
                <div class="line-item-unit">元</div>
                <JnpfInputNumber
                  v-model:value="dataForm.F_CommissionFixedRight"
                  placeholder=""
                  :controls="false"
                  :style="{
                    width: '110px',
                  }"
                  @input="handleCommissionRightInput" />
                <div class="line-item-unit">%</div>
              </a-space>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_Source')">
            <a-form-item name="F_Source" :labelCol="{ style: { width: '100px' } }">
              <template #label>商品来源</template>
              <JnpfSelect
                v-model:value="dataForm.F_Source"
                placeholder="请选择"
                :options="optionsObj.F_SourceOptions"
                :fieldNames="optionsObj.F_SourceProps"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_Type')">
            <a-form-item name="F_Type" :labelCol="{ style: { width: '100px' } }">
              <template #label>商品类型</template>
              <JnpfSelect
                v-model:value="dataForm.F_Type"
                placeholder="请选择"
                :options="optionsObj.F_TypeOptions"
                :fieldNames="optionsObj.F_TypeProps"
                allowClear
                showSearch
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_OutsourcePrice') && dataForm.F_Source === '1'">
            <a-form-item name="F_OutsourcePrice" :labelCol="{ style: { width: '100px' } }">
              <template #label>外协单价</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_OutsourcePrice"
                placeholder="请输入"
                :controls="false"
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_SupplierId') && dataForm.F_Source === '2'">
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_InspectRule')">
            <a-form-item name="F_InspectRule" :labelCol="{ style: { width: '100px' } }">
              <template #label>检验规范</template>
              <JnpfInput
                v-model:value="dataForm.F_InspectRule"
                placeholder="请输入"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_StockUpperLimit')">
            <a-form-item name="F_StockUpperLimit" :labelCol="{ style: { width: '150px' } }">
              <template #label>库存上限警告值<BasicHelp text="设置该商品在仓库中的默认库存上限的警告值。" /> </template>
              <JnpfInputNumber
                v-model:value="dataForm.F_StockUpperLimit"
                placeholder="请输入"
                :controls="false"
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_StockLowerLimit')">
            <a-form-item name="F_StockLowerLimit" :labelCol="{ style: { width: '150px' } }">
              <template #label>库存下限警告值<BasicHelp text="设置该商品在仓库中的默认库存下限的警告值。" /> </template>
              <JnpfInputNumber
                v-model:value="dataForm.F_StockLowerLimit"
                placeholder="请输入"
                :controls="false"
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_Encoding')">
            <a-form-item name="F_Encoding" :labelCol="{ style: { width: '100px' } }">
              <template #label>商品编码</template>
              <JnpfInput
                v-model:value="dataForm.F_Encoding"
                placeholder="请输入"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_Image')">
            <a-form-item name="F_Image" :labelCol="{ style: { width: '100px' } }">
              <template #label>商品图片</template>
              <JnpfUploadImg
                v-model:value="dataForm.F_Image"
                pathType="defaultPath"
                sizeUnit="MB"
                :fileSize="10"
                :sortRule="[]"
                timeFormat="YYYY"
                showType="card"
                buttonText="点击上传" />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_AttachmentUrl')">
            <a-form-item name="F_AttachmentUrl" :labelCol="{ style: { width: '100px' } }">
              <template #label>附件</template>
              <JnpfUploadFile
                v-model:value="dataForm.F_AttachmentUrl"
                buttonText="点击上传"
                pathType="defaultPath"
                :fileSize="10"
                sizeUnit="MB"
                :limit="9"
                :sortRule="[]"
                timeFormat="YYYY" />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_Remark')">
            <a-form-item name="F_Remark" :labelCol="{ style: { width: '100px' } }">
              <template #label>备注</template>
              <JnpfTextarea
                v-model:value="dataForm.F_Remark"
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
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_Intro')">
            <a-form-item name="F_Intro" :labelCol="{ style: { width: '100px' } }">
              <template #label>商品简介</template>
              <JnpfEditor
                v-model:value="dataForm.F_Intro"
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <!-- <a-col :span="24" class="ant-col-item" v-show="false" v-if="hasFormP('F_CreatorTime')">
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
          <a-col :span="24" class="ant-col-item" v-show="false" v-if="hasFormP('F_CreatorUserId')">
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
          </a-col> -->
        </a-row>
      </a-form>
    </a-row>
  </BasicModal>
  <SelectModal :config="state.currTableConf" :formData="state.dataForm" ref="selectModal" @select="addForSelect" />
</template>
<script lang="ts" setup>
  import { create, update, getInfo, validateGoodsName } from './helper/api';
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
    title: string;
    continueText: string;
    allList: any[];
    currIndex: number;
    isContinue: boolean;
    submitType: number;
    showContinueBtn: boolean;
    goodsNameValidating: boolean;
    goodsNameError: string;
    originalGoodsName?: string;
    commissionLastChanged?: string;
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
  /** 单位比例明细：根据所选单位动态生成，每项 { id, name, rate, qty }，提交前序列化为 JSON 存入 F_Unit_Ratio */
  const unitRatioList = ref<Array<{ id: string; name: string; rate: number; qty: number }>>([]);
  const state = reactive<State>({
    dataForm: {
      id: '',
      F_GoodsNo: undefined,
      F_GoodsName: undefined,
      F_CategoryId: [],
      F_Unit: [],
      F_Unit_Ratio: undefined,
      F_SalePrice: undefined,
      F_CostPrice: undefined,
      F_CommissionFixed: undefined,
      F_CommissionFixedLeft: undefined,
      F_CommissionFixedRight: undefined,
      F_Source: undefined,
      F_Type: undefined,
      F_Specification: undefined,
      F_OutsourcePrice: undefined,
      F_SupplierId: undefined,
      F_InspectRule: undefined,
      F_StockUpperLimit: undefined,
      F_StockLowerLimit: undefined,
      F_Encoding: undefined,
      F_Material: undefined,
      F_Image: [],
      F_AttachmentUrl: [],
      F_Remark: undefined,
      F_Intro: undefined,
      F_CreatorTime: undefined,
      F_CreatorUserId: undefined,
    },
    dataRule: {
      F_GoodsName: [
        {
          required: true,
          message: '请输入商品名称',
          trigger: 'blur',
        },
      ],
      F_Specification: [
        {
          required: true,
          message: '请输入商品规格',
          trigger: 'blur',
        },
      ],
      F_Unit: [
        {
          required: true,
          message: '请选择单位',
          trigger: 'blur',
        },
      ],
      F_CategoryId: [
        {
          required: true,
          message: '请选择类别',
          trigger: 'change',
          type: 'array',
          len: 0,
          validator: (rule: any, value: any) => {
            if (!value || value.length === 0) {
              return Promise.reject(rule.message);
            }
            return Promise.resolve();
          },
        },
      ],
      F_Type: [
        {
          required: true,
          message: '请选择商品类型',
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
      F_UnitProps: {
        label: 'F_Name',
        value: 'F_Id',
        children: 'children',
      },
      F_SourceProps: { label: 'fullName', value: 'enCode' },
      F_TypeProps: { label: 'fullName', value: 'enCode' },
      F_SupplierIdProps: { label: 'F_SupplierName', value: 'F_Id' },
    },
    currVmodel: '',
    currTableConf: {},
    tableRows: {},
    addTableConf: {},
    title: '',
    continueText: '',
    allList: [],
    currIndex: 0,
    isContinue: false,
    submitType: 0,
    showContinueBtn: true,
    goodsNameValidating: false,
    goodsNameError: '',
    commissionLastChanged: '',
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
        F_GoodsNo: undefined,
        F_GoodsName: undefined,
        F_CategoryId: [],
        F_Unit: [],
        F_Unit_Ratio: undefined,
        F_SalePrice: undefined,
        F_CostPrice: undefined,
        F_CommissionFixed: undefined,
        F_CommissionFixedLeft: undefined,
        F_CommissionFixedRight: undefined,
        F_Source: undefined,
        F_Type: undefined,
        F_Specification: undefined,
        F_OutsourcePrice: undefined,
        F_SupplierId: undefined,
        F_InspectRule: undefined,
        F_StockUpperLimit: undefined,
        F_StockLowerLimit: undefined,
        F_Encoding: undefined,
        F_Material: undefined,
        F_Image: [],
        F_AttachmentUrl: [],
        F_Remark: undefined,
        F_Intro: undefined,
        F_CreatorTime: undefined,
        F_CreatorUserId: undefined,
      };
      // 重置商品名称验证状态
      state.goodsNameValidating = false;
      state.goodsNameError = '';
      (state as any).originalGoodsName = '';
      // 确认按钮恢复可用
      setFormProps({ okButtonProps: { disabled: false } });
      getF_CategoryIdOptions();
      getF_UnitOptions();
      getF_SourceOptions();
      getF_TypeOptions();
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
      // 保存原始商品名称，用于编辑时的比较
      (state as any).originalGoodsName = state.dataForm.F_GoodsName || '';
      // 拆分售后佣金字段为左右两部分（数据库中以 "-" 分隔）
      try {
        const fixed = state.dataForm.F_CommissionFixed || '';
        if (typeof fixed === 'string' && fixed.indexOf('-') !== -1) {
          const parts = fixed.split('-');
          state.dataForm.F_CommissionFixedLeft = parts[0] !== undefined && parts[0] !== '' ? Number(parts[0]) : undefined;
          state.dataForm.F_CommissionFixedRight = parts[1] !== undefined && parts[1] !== '' ? Number(parts[1]) : undefined;
        } else {
          state.dataForm.F_CommissionFixedLeft = state.dataForm.F_CommissionFixed !== undefined ? state.dataForm.F_CommissionFixed : undefined;
          state.dataForm.F_CommissionFixedRight = undefined;
        }
      } catch (e) {
        state.dataForm.F_CommissionFixedLeft = undefined;
        state.dataForm.F_CommissionFixedRight = undefined;
      }
      getF_CategoryIdOptions();
      getF_UnitOptions();
      getF_SourceOptions();
      getF_TypeOptions();
      getF_SupplierIdOptions();
      changeLoading(false);
    });
  }
  async function handleSubmit() {
    try {
      const values = await getForm()?.validate();
      if (!values) return;

      // 检查商品名称是否已通过实时验证
      if (state.goodsNameError) {
        createMessage.error(state.goodsNameError);
        return;
      }

      // 只保存左侧售后佣金（元）
      try {
        state.dataForm.F_CommissionFixed = state.dataForm.F_CommissionFixedLeft ?? '';
      } catch (e) {
        // ignore
      }
      // 如果上限和下限都有值，校验上限必须大于下限，否则阻止提交
      try {
        const upperRaw = state.dataForm.F_StockUpperLimit;
        const lowerRaw = state.dataForm.F_StockLowerLimit;
        const hasUpper = upperRaw !== undefined && upperRaw !== null && upperRaw !== '';
        const hasLower = lowerRaw !== undefined && lowerRaw !== null && lowerRaw !== '';
        if (hasUpper && hasLower) {
          const upperNum = Number(upperRaw);
          const lowerNum = Number(lowerRaw);
          if (!isNaN(upperNum) && !isNaN(lowerNum) && upperNum <= lowerNum) {
            createMessage.error('库存上限必须大于库存下限');
            return;
          }
        }
      } catch (e) {
        // ignore validation errors and continue
      }
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
  function getF_CategoryIdOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2008414575283802112', query).then(res => {
      let data = res.data;
      state.optionsObj.F_CategoryIdOptions = Array.isArray(data) ? data : [];
    });
  }
  function getF_UnitOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2034507723852353536', query).then(res => {
      let data = res.data;
      state.optionsObj.F_UnitOptions = Array.isArray(data) ? data : [];
    });
  }

  /** 将单位选项树扁平为 id -> { id, name } 的映射 */
  function flattenUnitOptions(nodes: any[], labelKey = 'F_Name', valueKey = 'F_Id', childrenKey = 'children'): Map<string, { id: string; name: string }> {
    const map = new Map<string, { id: string; name: string }>();
    function walk(list: any[]) {
      if (!Array.isArray(list)) return;
      for (const n of list) {
        const id = n[valueKey];
        const name = n[labelKey] ?? '';
        if (id) map.set(id, { id, name });
        if (n[childrenKey]?.length) walk(n[childrenKey]);
      }
    }
    walk(nodes);
    return map;
  }

  /** 根据级联值（单位路径 ID 数组）解析出有序的 [{ id, name }] */
  function resolveUnitPath(unitIds: string[] | undefined, options: any[]): Array<{ id: string; name: string }> {
    if (!Array.isArray(unitIds) || unitIds.length === 0) return [];
    const flat = flattenUnitOptions(options || [], 'F_Name', 'F_Id', 'children');
    return unitIds.map(id => ({ id, name: flat.get(id)?.name ?? id })).filter(x => x.name);
  }

  /** 解析 F_Unit_Ratio JSON 得到 id -> qty 映射 */
  function parseUnitRatioJson(str: any): Map<string, number> {
    const map = new Map<string, number>();
    if (str == null || typeof str !== 'string' || !str.trim()) return map;
    try {
      const arr = JSON.parse(str);
      if (!Array.isArray(arr)) return map;
      for (const item of arr) {
        if (item && item.id != null) map.set(String(item.id), Number(item.qty) || 0);
      }
    } catch (_) {}
    return map;
  }

  /** 同步 unitRatioList 到 dataForm.F_Unit_Ratio（JSON 字符串） */
  function syncUnitRatioToForm() {
    const list = unitRatioList.value.map(({ id, name, rate, qty }) => ({ id, name, rate, qty }));
    state.dataForm.F_Unit_Ratio = list.length > 0 ? JSON.stringify(list) : undefined;
  }

  /** 用户修改某一行的数量 */
  function updateUnitRatioQty(index: number, val: number | undefined) {
    const list = [...unitRatioList.value];
    if (index >= 0 && index < list.length) {
      list[index] = { ...list[index], qty: val ?? 0 };
      unitRatioList.value = list;
      syncUnitRatioToForm();
    }
  }

  /** 当单位或选项变化时，重建单位比例列表并同步到表单 */
  watch(
    () => [state.dataForm.F_Unit, state.optionsObj.F_UnitOptions] as const,
    ([unitVal, options]) => {
      let ids: string[] = [];
      if (Array.isArray(unitVal)) ids = unitVal;
      else if (typeof unitVal === 'string' && unitVal.trim()) {
        try {
          const parsed = JSON.parse(unitVal);
          ids = Array.isArray(parsed) ? parsed : [];
        } catch (_) {}
      }
      const opts = Array.isArray(options) ? options : [];
      const path = resolveUnitPath(ids, opts);
      if (path.length === 0) {
        unitRatioList.value = [];
        state.dataForm.F_Unit_Ratio = undefined;
        return;
      }
      const existingQty = parseUnitRatioJson(state.dataForm.F_Unit_Ratio);
      const list = path.map((item, i) => ({
        id: item.id,
        name: item.name,
        rate: 1,
        qty: existingQty.has(item.id) ? existingQty.get(item.id)! : i === 0 ? 1 : 0,
      }));
      unitRatioList.value = list;
      syncUnitRatioToForm();
    },
    { immediate: true, deep: true },
  );
  function getF_SourceOptions() {
    getDictionaryDataSelector('2008448951216377856').then(res => {
      state.optionsObj.F_SourceOptions = res.data.list;
    });
  }
  function getF_TypeOptions() {
    getDictionaryDataSelector('2029481827470807040').then(res => {
      state.optionsObj.F_TypeOptions = res.data.list;
    });
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
  function openSelectDialog(key, value) {
    state.currTableConf = state.addTableConf[key + 'List' + value];
    state.currVmodel = key;
    nextTick(() => {
      (selectModal.value as any)?.openSelectModal();
    });
  }
  // 监听销售单价或左侧输入变化，自动计算右侧值：售后佣金(元) 占 销售单价 的百分比
  // 用于防止左右输入互相触发造成循环更新的短时标记（非响应式计时器）
  let commissionChangeTimer: number | null = null;

  function setCommissionChanged(side: 'left' | 'right') {
    state.commissionLastChanged = side;
    if (commissionChangeTimer) {
      clearTimeout(commissionChangeTimer);
      commissionChangeTimer = null;
    }
    // 200ms 后重置标记，允许再次由另侧触发计算
    commissionChangeTimer = window.setTimeout(() => {
      state.commissionLastChanged = '';
      commissionChangeTimer = null;
    }, 200);
  }

  function handleCommissionLeftInput() {
    setCommissionChanged('left');
  }

  function handleCommissionRightInput() {
    setCommissionChanged('right');
  }

  // 左侧变化 -> 更新右侧百分比
  watch(
    () => [state.dataForm.F_SalePrice, state.dataForm.F_CommissionFixedLeft],
    ([salePrice, left]) => {
      // 如果刚刚是右侧输入触发的，不再覆盖右侧
      if (state.commissionLastChanged === 'right') return;
      const leftNum = Number(left);
      const saleNum = Number(salePrice);
      if (!isNaN(leftNum) && !isNaN(saleNum) && saleNum !== 0) {
        const val = (leftNum / saleNum) * 100;
        state.dataForm.F_CommissionFixedRight = Math.round(val * 100) / 100;
      } else if (!isNaN(leftNum) && leftNum === 0 && !isNaN(saleNum)) {
        state.dataForm.F_CommissionFixedRight = 0;
      } else {
        state.dataForm.F_CommissionFixedRight = undefined;
      }
    },
    { immediate: true },
  );

  // 右侧变化（百分比） -> 更新左侧金额
  watch(
    () => [state.dataForm.F_SalePrice, state.dataForm.F_CommissionFixedRight],
    ([salePrice, right]) => {
      if (state.commissionLastChanged === 'left') return;
      const rightNum = Number(right);
      const saleNum = Number(salePrice);
      if (!isNaN(rightNum) && !isNaN(saleNum) && saleNum !== 0) {
        const leftVal = (rightNum / 100) * saleNum;
        state.dataForm.F_CommissionFixedLeft = Math.round(leftVal * 100) / 100;
      } else if (!isNaN(rightNum) && rightNum === 0 && !isNaN(saleNum)) {
        state.dataForm.F_CommissionFixedLeft = 0;
      } else {
        state.dataForm.F_CommissionFixedLeft = undefined;
      }
    },
    { immediate: true },
  );
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

  // 商品名称输入时清除错误状态
  function handleGoodsNameInput() {
    if (state.goodsNameError) {
      state.goodsNameError = '';
      // 恢复确认按钮
      setFormProps({ okButtonProps: { disabled: false } });
    }
  }

  // 商品名称失去焦点时验证
  async function handleGoodsNameBlur() {
    const goodsName = state.dataForm.F_GoodsName?.trim();
    if (!goodsName) {
      state.goodsNameError = '';
      setFormProps({ okButtonProps: { disabled: false } });
      return;
    }

    // 如果正在编辑且商品名称没有改变，不需要验证
    if (state.dataForm.id && goodsName === state.originalGoodsName) {
      state.goodsNameError = '';
      setFormProps({ okButtonProps: { disabled: false } });
      return;
    }

    state.goodsNameValidating = true;
    state.goodsNameError = '';
    // 验证期间禁用确认按钮
    setFormProps({ okButtonProps: { disabled: true } });

    try {
      await validateGoodsName({ F_GoodsName: goodsName });
      state.goodsNameError = '';
      // 验证通过，恢复确认按钮
      setFormProps({ okButtonProps: { disabled: false } });
    } catch (error: any) {
      if (error?.msg?.includes('商品名称已存在')) {
        state.goodsNameError = '商品名称已存在，请使用其他名称';
        // 名称重复，禁用确认按钮
        setFormProps({ okButtonProps: { disabled: true } });
      }
    } finally {
      state.goodsNameValidating = false;
    }
  }
</script>
<style scoped>
  .unit-ratio-table {
    width: 100%;
  }
  .jnpf-unit-ratio-table {
    width: 100%;
    border-collapse: collapse;
    border: 1px solid #d9d9d9;
    border-radius: 2px;
  }
  .jnpf-unit-ratio-table th,
  .jnpf-unit-ratio-table td {
    padding: 8px 12px;
    border: 1px solid #d9d9d9;
    text-align: left;
  }
  .jnpf-unit-ratio-table th {
    background: #fafafa;
    font-weight: 500;
  }
</style>
