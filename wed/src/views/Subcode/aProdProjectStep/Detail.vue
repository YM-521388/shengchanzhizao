<template>
  <BasicModal v-bind="$attrs" @register="registerModal" :title="title" width="600px" :minHeight="100" :showOkBtn="false">
    <template #insertFooter>
    </template>
    <a-row class="dynamic-form ">
      <a-form :colon="false" layout="horizontal" labelAlign="right"  :labelCol="{ style: { width: '100px' } }" :model="dataForm" ref="formRef">
        <a-row :gutter="15">
          <a-col :span="24" class="ant-col-item" >
            <a-form-item name="F_ProjcetItemId" :labelCol="{ style: { width: '100px' } }">
              <template #label>项目商品</template>
              <p>{{ dataForm.F_ProjcetItemId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" >
            <a-form-item name="F_ProcessId" :labelCol="{ style: { width: '100px' } }">
              <template #label>工序</template>
              <p>{{ dataForm.F_ProcessId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" >
            <a-form-item name="F_StartDate" :labelCol="{ style: { width: '100px' } }">
              <template #label>计划开始</template>
              <p>{{ dataForm.F_StartDate }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" >
            <a-form-item name="F_EndDate" :labelCol="{ style: { width: '100px' } }">
              <template #label>计划结束</template>
              <p>{{ dataForm.F_EndDate }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" >
            <a-form-item name="F_SortCode" :labelCol="{ style: { width: '100px' } }">
              <template #label>排序</template>
              <JnpfInputNumber v-model:value="dataForm.F_SortCode" placeholder='请输入' :controls="false" :style='{
  "width": "100%"
}' disabled detailed />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" >
            <a-form-item name="F_Description" :labelCol="{ style: { width: '100px' } }">
              <template #label>备注</template>
              <p>{{ dataForm.F_Description }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" v-show="false" >
            <a-form-item name="F_CreatorTime" :labelCol="{ style: { width: '100px' } }">
              <template #label>创建时间</template>
              <p>{{ dataForm.F_CreatorTime }}</p>
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
  import { reactive, toRefs, nextTick, ref, computed, unref, toRaw} from 'vue';
  import { thousandsFormat } from '@/utils/jnpf';
  import { CaretRightOutlined } from '@ant-design/icons-vue';
  import { buildUUID } from '@/utils/uuid';
  import { useI18n } from '@/hooks/web/useI18n';
  import { getDataChange } from '@/api/onlineDev/visualDev';
  import { getDataInterfaceDataInfoByIds } from '@/api/systemData/dataInterface';
  import ExtraRelationInfo from '@/components/Jnpf/RelationForm/src/ExtraRelationInfo.vue';
  import { BasicModal, useModal } from '@/components/Modal';

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
	interfaceRes:{
	},
	extraOptions:{
	},
	extraData:{
	},
    title: '详情',
  });
  const { title, dataForm } = toRefs(state);

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
    getDetail(id).then((res) => {
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
