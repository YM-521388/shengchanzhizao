<template>
  <BasicModal v-bind="$attrs" @register="registerModal" :title="title" width="50%" :minHeight="100" :showOkBtn="false" :canFullscreen="true">
    <template #insertFooter> </template>
    <a-row class="dynamic-form">
      <a-form :colon="false" layout="horizontal" labelAlign="right" :labelCol="{ style: { width: '100px' } }" :model="dataForm" ref="formRef">
        <a-row :gutter="15">
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_CustomerId')">
            <a-form-item name="F_CustomerId" :labelCol="{ style: { width: '100px' } }">
              <template #label>客户：</template>
              <p>{{ dataForm.F_CustomerId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_ContractId')">
            <a-form-item name="F_ContractId" :labelCol="{ style: { width: '100px' } }">
              <template #label>合同：</template>
              <p class="leading-32px">{{ dataForm.F_ContractId }}</p>
              <ExtraRelationInfo
                :extraOptions="state.extraOptions.F_ContractId"
                :data="state.extraData.F_ContractId"
                v-if="state.extraOptions.F_ContractId?.length && state.extraData.F_ContractId && JSON.stringify(state.extraData.F_ContractId) !== '{}'" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_ExpendDate')">
            <a-form-item name="F_ExpendDate" :labelCol="{ style: { width: '100px' } }">
              <template #label>支出日期：</template>
              <p>{{ dataForm.F_ExpendDate }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_ExpendType')">
            <a-form-item name="F_ExpendType" :labelCol="{ style: { width: '100px' } }">
              <template #label>支出类别：</template>
              <p>{{ dataForm.F_ExpendType }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_Money')">
            <a-form-item name="F_Money" :labelCol="{ style: { width: '100px' } }">
              <template #label>金额：</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_Money"
                placeholder="请输入"
                :controls="false"
                :style="{
                  width: '100%',
                }"
                disabled
                detailed />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_SettleStatus')">
            <a-form-item name="F_SettleStatus" :labelCol="{ style: { width: '100px' } }">
              <template #label>结算状态：</template>
              <JnpfInput
                v-model:value="dataForm.F_SettleStatus"
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
          <!-- <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_AuditStatus')" >
            <a-form-item name="F_AuditStatus" :labelCol="{ style: { width: '100px' } }">
              <template #label>审核状态</template>
              <JnpfInput v-model:value="dataForm.F_AuditStatus" :maskConfig='{
  "filler": "*",
  "maskType": 1,
  "prefixType": 1,
  "prefixLimit": 0,
  "prefixSpecifyChar": "",
  "suffixType": 1,
  "suffixLimit": 0,
  "suffixSpecifyChar": "",
  "ignoreChar": "",
  "useUnrealMask": false,
  "unrealMaskLength": 1
}' disabled detailed />
            </a-form-item>
          </a-col> -->
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_Files')">
            <a-form-item name="F_Files" :labelCol="{ style: { width: '100px' } }">
              <template #label>附件：</template>
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
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_Description')">
            <a-form-item name="F_Description" :labelCol="{ style: { width: '100px' } }">
              <template #label>备注：</template>
              <p>{{ dataForm.F_Description }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_SettleFiles')">
            <a-form-item name="F_SettleFiles" :labelCol="{ style: { width: '100px' } }">
              <template #label>结算附件：</template>
              <JnpfUploadFile
                v-model:value="dataForm.F_SettleFiles"
                buttonText="点击上传"
                pathType="defaultPath"
                :fileSize="10"
                sizeUnit="MB"
                :limit="9"
                disabled
                detailed />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_SettleDesc')">
            <a-form-item name="F_SettleDesc" :labelCol="{ style: { width: '100px' } }">
              <template #label>结算备注：</template>
              <p>{{ dataForm.F_SettleDesc }}</p>
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
    interfaceRes: {
      F_ContractId: [
        {
          defaultValue: '',
          field: 'id',
          dataType: 'varchar',
          required: 0,
          fieldName: '',
          relationField: '2009181616060108800',
          jnpfKey: null,
          complexHeaderList: null,
          sourceType: 2,
          isChildren: false,
          IsMultiple: false,
        },
      ],
    },
    extraOptions: {
      F_ContractId: [],
    },
    extraData: {
      F_ContractId: {},
    },
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
        getF_ContractIdExtraInfo();
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

  function getF_ContractIdExtraInfo() {
    if (!state.dataForm.F_ContractId_id) return;
    const paramList = getParamList('F_ContractId');
    const query = {
      ids: [state.dataForm.F_ContractId_id],
      interfaceId: '2010894701401608192',
      propsValue: 'F_Id',
      relationField: 'F_ContractCode',
      paramList,
    };
    getDataInterfaceDataInfoByIds('2010894701401608192', query).then(res => {
      const data = res.data && res.data.length ? res.data[0] : {};
      state.extraData.F_ContractId = data;
    });
  }
</script>
