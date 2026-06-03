<template>
  <BasicModal v-bind="$attrs" @register="registerModal" :title="title" width="60%" :minHeight="100" :showOkBtn="false" :canFullscreen="true">
    <template #insertFooter> </template>
    <a-row class="dynamic-form">
      <a-form :colon="false" layout="horizontal" labelAlign="right" :labelCol="{ style: { width: '100px' } }" :model="dataForm" ref="formRef">
        <a-row :gutter="15">
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_GoodsNo')">
            <a-form-item name="F_GoodsNo" :labelCol="{ style: { width: '100px' } }">
              <template #label>商品编号：</template>
              <JnpfInput
                v-model:value="dataForm.F_GoodsNo"
                :maskConfig="{
                  filler: '*',
                  maskType: 1,
                  prefixType: 1,
                  prefixLimit: 0,
                  prefixSpecifyChar: '',
                  suffixType: 1,
                  suffixLimit: 0,
                  suffixSpecifyChar: '',
                  ignoreChar: '',
                  useUnrealMask: false,
                  unrealMaskLength: 1,
                }"
                disabled
                detailed />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_GoodsName')">
            <a-form-item name="F_GoodsName" :labelCol="{ style: { width: '100px' } }">
              <template #label>商品名称：</template>
              <JnpfInput
                v-model:value="dataForm.F_GoodsName"
                :maskConfig="{
                  filler: '*',
                  maskType: 1,
                  prefixType: 1,
                  prefixLimit: 0,
                  prefixSpecifyChar: '',
                  suffixType: 1,
                  suffixLimit: 0,
                  suffixSpecifyChar: '',
                  ignoreChar: '',
                  useUnrealMask: false,
                  unrealMaskLength: 1,
                }"
                disabled
                detailed />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_CategoryId')">
            <a-form-item name="F_CategoryId" :labelCol="{ style: { width: '100px' } }">
              <template #label>类别：</template>
              <p>{{ dataForm.F_CategoryId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_Unit')">
            <a-form-item name="F_Unit" :labelCol="{ style: { width: '100px' } }">
              <template #label>单位：</template>
              <p>{{ dataForm.F_Unit }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_Unit_Ratio')">
            <a-form-item name="F_Unit_Ratio" :labelCol="{ style: { width: '100px' } }">
              <template #label>单位比例</template>
              <JnpfInput
                v-model:value="dataForm.F_Unit_Ratio"
                :maskConfig="{
                  filler: '*',
                  maskType: 1,
                  prefixType: 1,
                  prefixLimit: 0,
                  prefixSpecifyChar: '',
                  suffixType: 1,
                  suffixLimit: 0,
                  suffixSpecifyChar: '',
                  ignoreChar: '',
                  useUnrealMask: false,
                  unrealMaskLength: 1,
                }"
                disabled
                detailed />
            </a-form-item>
          </a-col>

          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_Specification')">
            <a-form-item name="F_Specification" :labelCol="{ style: { width: '100px' } }">
              <template #label>规格：</template>
              <JnpfInput
                v-model:value="dataForm.F_Specification"
                :maskConfig="{
                  filler: '*',
                  maskType: 1,
                  prefixType: 1,
                  prefixLimit: 0,
                  prefixSpecifyChar: '',
                  suffixType: 1,
                  suffixLimit: 0,
                  suffixSpecifyChar: '',
                  ignoreChar: '',
                  useUnrealMask: false,
                  unrealMaskLength: 1,
                }"
                disabled
                detailed />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_Material')">
            <a-form-item name="F_Material" :labelCol="{ style: { width: '100px' } }">
              <template #label>材质</template>
              <JnpfInput
                v-model:value="dataForm.F_Material"
                :maskConfig="{
                  filler: '*',
                  maskType: 1,
                  prefixType: 1,
                  prefixLimit: 0,
                  prefixSpecifyChar: '',
                  suffixType: 1,
                  suffixLimit: 0,
                  suffixSpecifyChar: '',
                  ignoreChar: '',
                  useUnrealMask: false,
                  unrealMaskLength: 1,
                }"
                disabled
                detailed />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_SalePrice')">
            <a-form-item name="F_SalePrice" :labelCol="{ style: { width: '100px' } }">
              <template #label>销售单价：</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_SalePrice"
                placeholder="请输入"
                :controls="true"
                :style="{
                  width: '100%',
                }"
                disabled
                detailed />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_CostPrice')">
            <a-form-item name="F_CostPrice" :labelCol="{ style: { width: '100px' } }">
              <template #label>成本单价：</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_CostPrice"
                placeholder="请输入"
                :controls="false"
                :style="{
                  width: '100%',
                }"
                disabled
                detailed />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_CommissionFixed')">
            <a-form-item name="F_CommissionFixed" :labelCol="{ style: { width: '100px' } }">
              <template #label>售后佣金：</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_CommissionFixed"
                placeholder="请输入"
                :controls="false"
                :style="{
                  width: '100%',
                }"
                disabled
                detailed />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_Source')">
            <a-form-item name="F_Source" :labelCol="{ style: { width: '100px' } }">
              <template #label>商品来源：</template>
              <p>{{ dataForm.F_Source }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_Type')">
            <a-form-item name="F_type" :labelCol="{ style: { width: '100px' } }">
              <template #label>商品类型</template>
              <p>{{ dataForm.F_type }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_OutsourcePrice')">
            <a-form-item name="F_OutsourcePrice" :labelCol="{ style: { width: '100px' } }">
              <template #label>外协单价：</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_OutsourcePrice"
                placeholder="请输入"
                :controls="false"
                :style="{
                  width: '100%',
                }"
                disabled
                detailed />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_SupplierId')">
            <a-form-item name="F_SupplierId" :labelCol="{ style: { width: '100px' } }">
              <template #label>供应商：</template>
              <p>{{ dataForm.F_SupplierId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_InspectRule')">
            <a-form-item name="F_InspectRule" :labelCol="{ style: { width: '100px' } }">
              <template #label>检验规范：</template>
              <JnpfInput
                v-model:value="dataForm.F_InspectRule"
                :maskConfig="{
                  filler: '*',
                  maskType: 1,
                  prefixType: 1,
                  prefixLimit: 0,
                  prefixSpecifyChar: '',
                  suffixType: 1,
                  suffixLimit: 0,
                  suffixSpecifyChar: '',
                  ignoreChar: '',
                  useUnrealMask: false,
                  unrealMaskLength: 1,
                }"
                disabled
                detailed />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_StockUpperLimit')">
            <a-form-item name="F_StockUpperLimit" :labelCol="{ style: { width: '130px' } }">
              <template #label>库存上限警告值<BasicHelp text="设置该商品在仓库中的默认库存上限的警告值。" />： </template>
              <JnpfInputNumber
                v-model:value="dataForm.F_StockUpperLimit"
                placeholder="请输入"
                :controls="false"
                :style="{
                  width: '100%',
                }"
                disabled
                detailed />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_StockLowerLimit')">
            <a-form-item name="F_StockLowerLimit" :labelCol="{ style: { width: '130px' } }">
              <template #label>库存下限警告值<BasicHelp text="设置该商品在仓库中的默认库存下限的警告值。" /> ：</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_StockLowerLimit"
                placeholder="请输入"
                :controls="false"
                :style="{
                  width: '100%',
                }"
                disabled
                detailed />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_Image')">
            <a-form-item name="F_Image" :labelCol="{ style: { width: '100px' } }">
              <template #label>商品图片：</template>
              <JnpfUploadImg v-model:value="dataForm.F_Image" pathType="defaultPath" sizeUnit="MB" :fileSize="10" disabled detailed />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_AttachmentUrl')">
            <a-form-item name="F_AttachmentUrl" :labelCol="{ style: { width: '100px' } }">
              <template #label>附件：</template>
              <JnpfUploadFile
                v-model:value="dataForm.F_AttachmentUrl"
                buttonText="点击上传"
                pathType="defaultPath"
                :fileSize="10"
                sizeUnit="MB"
                :limit="9"
                disabled
                detailed />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_Remark')">
            <a-form-item name="F_Remark" :labelCol="{ style: { width: '100px' } }">
              <template #label>备注：</template>
              <p>{{ dataForm.F_Remark }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_Intro')">
            <a-form-item name="F_Intro" :labelCol="{ style: { width: '100px' } }">
              <template #label>商品简介：</template>
              <div v-html="dataForm.F_Intro"></div>
            </a-form-item>
          </a-col>
        </a-row>
      </a-form>
    </a-row>
  </BasicModal>
</template>
<script lang="ts" setup>
  import { getDetail } from './helper/api';
  import { getConfigData } from '@/api/onlineDev/visualDev';
  import { reactive, toRefs, nextTick, ref, computed, unref, toRaw } from 'vue';
  import { thousandsFormat } from '@/utils/jnpf';
  import { CaretRightOutlined } from '@ant-design/icons-vue';
  import { buildUUID } from '@/utils/uuid';
  import { useI18n } from '@/hooks/web/useI18n';
  import { getDataChange } from '@/api/onlineDev/visualDev';
  import { getDataInterfaceDataInfoByIds } from '@/api/systemData/dataInterface';
  import ExtraRelationInfo from '@/components/Jnpf/RelationForm/src/ExtraRelationInfo.vue';
  import { BasicModal, useModal } from '@/components/Modal';
  import { usePermission } from '@/hooks/web/usePermission';

  interface State {
    dataForm: any;
    interfaceRes: any;
    extraOptions: any;
    extraData: any;
    title: string;
  }

  defineOptions({ name: 'Detail' });
  const [registerModal, { openModal, closeModal, setModalProps }] = useModal();
  const { t } = useI18n();

  const relationDetailRef = ref<any>(null);
  const state = reactive<State>({
    dataForm: {},
    interfaceRes: {},
    extraOptions: {},
    extraData: {},
    title: '详情',
  });
  const { title, dataForm } = toRefs(state);
  const { hasFormP } = usePermission();

  defineExpose({ init });

  function init(data) {
    state.dataForm.id = data.id;
    openModal();
    nextTick(() => {
      setTimeout(initData, 0);
    });
  }
  function initData() {
    changeLoading(true);
    if (state.dataForm.id) {
      getData(state.dataForm.id);
    } else {
      closeModal();
    }
  }
  function getData(id) {
    getDetail(id).then(res => {
      state.dataForm = res.data || {};

      nextTick(() => {
        changeLoading(false);
      });
    });
  }
  function setFormProps(data) {
    setModalProps(data);
  }
  function changeLoading(loading) {
    setFormProps({ loading });
  }
  function getParamList(key) {
    let templateJson: any[] = state.interfaceRes[key];
    if (!templateJson || !templateJson.length || !state.dataForm) return templateJson;
    for (let i = 0; i < templateJson.length; i++) {
      if (templateJson[i].relationField && templateJson[i].sourceType == 1) {
        templateJson[i].defaultValue = state.dataForm[templateJson[i].relationField + '_id'] || '';
      }
    }
    return templateJson;
  }
</script>
