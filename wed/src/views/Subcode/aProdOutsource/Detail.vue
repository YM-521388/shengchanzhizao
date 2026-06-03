<template>
  <BasicModal v-bind="$attrs" @register="registerModal" :title="title" width="70%" :minHeight="100" :showOkBtn="false" :canFullscreen="true">
    <template #insertFooter> </template>
    <a-row class="dynamic-form">
      <a-form :colon="false" layout="horizontal" labelAlign="right" :labelCol="{ style: { width: '100px' } }" :model="dataForm" ref="formRef">
        <a-row :gutter="15">
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_OutsourceNo')">
            <a-form-item name="F_OutsourceNo" :labelCol="{ style: { width: '100px' } }">
              <template #label>外协工单号：</template>
              <JnpfInput
                v-model:value="dataForm.F_OutsourceNo"
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
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_GoodsId')">
            <a-form-item name="F_GoodsId" :labelCol="{ style: { width: '100px' } }">
              <template #label>商品：</template>
              <p class="leading-32px">{{ dataForm.F_GoodsId }}</p>
              <ExtraRelationInfo
                :extraOptions="state.extraOptions.F_GoodsId"
                :data="state.extraData.F_GoodsId"
                v-if="state.extraOptions.F_GoodsId?.length && state.extraData.F_GoodsId && JSON.stringify(state.extraData.F_GoodsId) !== '{}'" />
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_PlanNum')">
            <a-form-item name="F_PlanNum" :labelCol="{ style: { width: '100px' } }">
              <template #label>计划数：</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_PlanNum"
                placeholder="请输入"
                :controls="true"
                :style="{
                  width: '100%',
                }"
                disabled
                detailed />
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_BOMId')">
            <a-form-item name="F_BOMId" :labelCol="{ style: { width: '100px' } }">
              <template #label>BOM：</template>
              <p class="leading-32px">{{ dataForm.F_BOMId }}</p>
              <ExtraRelationInfo
                :extraOptions="state.extraOptions.F_BOMId"
                :data="state.extraData.F_BOMId"
                v-if="state.extraOptions.F_BOMId?.length && state.extraData.F_BOMId && JSON.stringify(state.extraData.F_BOMId) !== '{}'" />
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_SupplierId')">
            <a-form-item name="F_SupplierId" :labelCol="{ style: { width: '100px' } }">
              <template #label>供应商：</template>
              <p>{{ dataForm.F_SupplierId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_ContactPerson')">
            <a-form-item name="F_ContactPerson" :labelCol="{ style: { width: '100px' } }">
              <template #label>联系人：</template>
              <JnpfInput
                v-model:value="dataForm.F_ContactPerson"
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
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_ContactPhone')">
            <a-form-item name="F_ContactPhone" :labelCol="{ style: { width: '100px' } }">
              <template #label>联系人电话：</template>
              <JnpfInput
                v-model:value="dataForm.F_ContactPhone"
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
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_Region')">
            <a-form-item name="F_Region" :labelCol="{ style: { width: '100px' } }">
              <template #label>供应商地区：</template>
              <p>{{ dataForm.F_Region }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_DetailAddress')">
            <a-form-item name="F_DetailAddress" :labelCol="{ style: { width: '100px' } }">
              <template #label>详细地址：</template>
              <JnpfInput
                v-model:value="dataForm.F_DetailAddress"
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
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_Price')">
            <a-form-item name="F_Price" :labelCol="{ style: { width: '100px' } }">
              <template #label>外协单价：</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_Price"
                placeholder="请输入"
                :controls="false"
                :style="{
                  width: '100%',
                }"
                disabled
                detailed />
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_DeliveryDate')">
            <a-form-item name="F_DeliveryDate" :labelCol="{ style: { width: '100px' } }">
              <template #label>交货日期：</template>
              <p>{{ dataForm.F_DeliveryDate }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_Priority')">
            <a-form-item name="F_Priority" :labelCol="{ style: { width: '100px' } }">
              <template #label>优先级：</template>
              <p>{{ dataForm.F_Priority }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_PlanStartDate')">
            <a-form-item name="F_PlanStartDate" :labelCol="{ style: { width: '100px' } }">
              <template #label>计划开始：</template>
              <p>{{ dataForm.F_PlanStartDate }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_PlanEndDate')">
            <a-form-item name="F_PlanEndDate" :labelCol="{ style: { width: '100px' } }">
              <template #label>计划结束：</template>
              <p>{{ dataForm.F_PlanEndDate }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_Files')">
            <a-form-item name="F_Files" :labelCol="{ style: { width: '100px' } }">
              <template #label>图纸：</template>
              <JnpfUploadFile
                v-model:value="dataForm.F_Files"
                buttonText="点击上传"
                pathType="defaultPath"
                :fileSize="10"
                sizeUnit="MB"
                :limit="9"
                disabled
                detailed />
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_State')">
            <a-form-item name="F_State" :labelCol="{ style: { width: '100px' } }">
              <template #label>状态：</template>
              <JnpfInput
                v-model:value="dataForm.F_State"
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_PauseReason')">
            <a-form-item name="F_PauseReason" :labelCol="{ style: { width: '100px' } }">
              <template #label>暂停原因：</template>
              <p>{{ dataForm.F_PauseReason }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_CancelReason')">
            <a-form-item name="F_CancelReason" :labelCol="{ style: { width: '100px' } }">
              <template #label>取消原因：</template>
              <p>{{ dataForm.F_CancelReason }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_Description')">
            <a-form-item name="F_Description" :labelCol="{ style: { width: '100px' } }">
              <template #label>备注：</template>
              <p>{{ dataForm.F_Description }}</p>
            </a-form-item>
          </a-col>

          <!-- <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle :content="t('用料清单')" :bordered="true" />
              <BasicTable :scroll="{ y: 400 }" @register="registerTable" @resizeColumn="handleResizeColumn">
                <template #bodyCell="{ column, record }"> </template>
              </BasicTable>
            </a-form-item>
          </a-col> -->
        </a-row>
      </a-form>
    </a-row>
  </BasicModal>
</template>
<script lang="ts" setup>
  import { getDetail, getGoodsList } from './helper/api';
  import { reactive, toRefs, nextTick, ref, computed, unref, toRaw } from 'vue';
  import { useI18n } from '@/hooks/web/useI18n';
  import { getDataInterfaceDataInfoByIds } from '@/api/systemData/dataInterface';
  import ExtraRelationInfo from '@/components/Jnpf/RelationForm/src/ExtraRelationInfo.vue';
  import { BasicModal, useModal } from '@/components/Modal';
  import { usePermission } from '@/hooks/web/usePermission';
  import { BasicTable, useTable, TableAction, BasicColumn, FormProps, ActionItem } from '@/components/Table';
  let tableReload: Function = () => {};

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

  const state = reactive<State>({
    dataForm: {},
    interfaceRes: {
      F_GoodsId: [],
      F_BOMId: [],
    },
    extraOptions: {
      F_GoodsId: [],
      F_BOMId: [],
    },
    extraData: {
      F_GoodsId: {},
      F_BOMId: {},
    },
    title: '详情',
  });
  const { title, dataForm } = toRefs(state);
  const { hasFormP } = usePermission();

  defineExpose({ init });

  function init(data) {
    state.dataForm.id = data.id;
    openModal();
    /* 手动刷新表格 */
    tableReload();
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
        getF_GoodsIdExtraInfo();
        getF_BOMIdExtraInfo();
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

  function getF_GoodsIdExtraInfo() {
    if (!state.dataForm.F_GoodsId_id) return;
    const paramList = getParamList('F_GoodsId');
    const query = {
      ids: [state.dataForm.F_GoodsId_id],
      interfaceId: '2008721559174385664',
      propsValue: 'F_Id',
      relationField: 'F_GoodsName',
      paramList,
    };
    getDataInterfaceDataInfoByIds('2008721559174385664', query).then(res => {
      const data = res.data && res.data.length ? res.data[0] : {};
      state.extraData.F_GoodsId = data;
    });
  }
  function getF_BOMIdExtraInfo() {
    if (!state.dataForm.F_BOMId_id) return;
    const paramList = getParamList('F_BOMId');
    const query = {
      ids: [state.dataForm.F_BOMId_id],
      interfaceId: '2008713719516893184',
      propsValue: 'F_Id',
      relationField: 'F_ParentId',
      paramList,
    };
    getDataInterfaceDataInfoByIds('2008713719516893184', query).then(res => {
      const data = res.data && res.data.length ? res.data[0] : {};
      state.extraData.F_BOMId = data;
    });
  }

  const allColumns: BasicColumn[] = [
    { title: '商品名', dataIndex: 'F_GoodsName', resizable: true, width: 150 },
    { title: '商品编号', dataIndex: 'F_GoodsNo', resizable: true, width: 150 },
    { title: '商品规格', dataIndex: 'F_Specification', resizable: true, width: 100 },
    { title: '单位用量', dataIndex: 'F_NumUnit', resizable: true, width: 100 },
    { title: '单位', dataIndex: 'F_UnitTwo', resizable: true, width: 100 },
    { title: '备注', dataIndex: 'F_Description', resizable: true },
  ];

  const columns = computed(() => allColumns.filter(col => typeof col.dataIndex === 'string' && hasFormP(('Detail_YLQD-' + col.dataIndex) as string)));
  // const [registerTable, { reload }] = useTable({
  //   api: getGoodsList,
  //   beforeFetch: data => {
  //     data.F_OutsourceId = state.dataForm.id;
  //     return data;
  //   },
  //   columns,
  //   useSearchForm: false,
  //   showTableSetting: false,
  //   showIndexColumn: false,
  // });

  // tableReload = reload;

  function handleResizeColumn(w, col) {
    col.width = w;
  }
</script>
