<template>
  <BasicModal v-bind="$attrs" @register="registerModal"  width="600px" :minHeight="100" okText="确定" cancelText="取消" @ok="handleSubmit" :closeFunc="onClose">
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
    <a-row class="dynamic-form ">
      <a-form :colon="false" layout="horizontal" labelAlign="right"  :labelCol="{ style: { width: '100px' } }" :model="dataForm" :rules="dataRule" ref="formRef">
        <a-row :gutter="15">
          <a-col :span="24" class="ant-col-item" >
            <a-form-item name="F_ProjcetItemId" :labelCol="{ style: { width: '100px' } }">
              <template #label>项目商品</template>
              <JnpfSelect v-model:value="dataForm.F_ProjcetItemId" placeholder='请选择' :options="optionsObj.F_ProjcetItemIdOptions" :fieldNames="optionsObj.F_ProjcetItemIdProps" allowClear showSearch :style='{
  "width": "100%"
}'  />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" >
            <a-form-item name="F_ProcessId" :labelCol="{ style: { width: '100px' } }">
              <template #label>工序</template>
              <JnpfSelect v-model:value="dataForm.F_ProcessId" placeholder='请选择' :options="optionsObj.F_ProcessIdOptions" :fieldNames="optionsObj.F_ProcessIdProps" allowClear showSearch :style='{
  "width": "100%"
}'  />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" >
            <a-form-item name="F_StartDate" :labelCol="{ style: { width: '100px' } }">
              <template #label>计划开始</template>
              <JnpfDatePicker v-model:value="dataForm.F_StartDate" placeholder='请选择' format="yyyy-MM-dd" allowClear :style='{
  "width": "100%"
}'  />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" >
            <a-form-item name="F_EndDate" :labelCol="{ style: { width: '100px' } }">
              <template #label>计划结束</template>
              <JnpfDatePicker v-model:value="dataForm.F_EndDate" placeholder='请选择' format="yyyy-MM-dd" allowClear :style='{
  "width": "100%"
}'  />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" >
            <a-form-item name="F_SortCode" :labelCol="{ style: { width: '100px' } }">
              <template #label>排序</template>
              <JnpfInputNumber v-model:value="dataForm.F_SortCode" placeholder='请输入' :controls="false" :style='{
  "width": "100%"
}'  />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" >
            <a-form-item name="F_Description" :labelCol="{ style: { width: '100px' } }">
              <template #label>备注</template>
              <JnpfTextarea v-model:value="dataForm.F_Description" placeholder='请输入' allowClear :autoSize='{
  "minRows": 4,
  "maxRows": 4
}' :style='{
  "width": "100%"
}'  :showCount='false'  />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" v-show="false" >
            <a-form-item name="F_CreatorTime" :labelCol="{ style: { width: '100px' } }">
              <template #label>创建时间</template>
              <JnpfOpenData v-model:value="dataForm.F_CreatorTime" type="currTime" readonly :style='{
  "width": "100%"
}'  />
            </a-form-item>
          </a-col>
        </a-row>
      </a-form>
    </a-row>
  </BasicModal>
    <SelectModal :config="state.currTableConf" :formData="state.dataForm" ref="selectModal" @select="addForSelect"/>
</template>
<script lang="ts" setup>
  import { create, update, getInfo } from './helper/api';
  import { reactive, toRefs, nextTick, ref, unref, computed, toRaw, inject } from 'vue';
  import { BasicModal, useModal } from '@/components/Modal';
  import SelectModal from '@/components/CommonModal/src/SelectModal.vue';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useI18n } from '@/hooks/web/useI18n';
  import { useUserStore } from '@/store/modules/user';
  import type { FormInstance } from 'ant-design-vue';
  import { thousandsFormat } from '@/utils/jnpf';
  import { getTimeUnit, getDateTimeUnit } from '@/utils/jnpf';
  import { getDataInterfaceRes } from '@/api/systemData/dataInterface';
  import dayjs from 'dayjs';
  import { cloneDeep } from 'lodash-es';
  import { buildUUID } from '@/utils/uuid';
  import { CaretRightOutlined } from '@ant-design/icons-vue';
  import { systemComponentsList } from '@/components/FormGenerator/src/helper/config';

  interface State {
    dataForm: any;
    dataRule: any;
    optionsObj: any;
	currVmodel:any;
	currTableConf:any;
	addTableConf: any;
	tableRows: any;
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
  const [registerModal, { openModal, setModalProps }] = useModal();
  const formRef = ref<FormInstance>();
  const selectModal = ref(null);
  const state = reactive<State>({
    dataForm: {
      id: '',
      F_ProjcetItemId: undefined,
      F_ProcessId: undefined,
      F_StartDate: undefined,
      F_EndDate: undefined,
      F_SortCode: undefined,
      F_Description: undefined,
      F_CreatorTime: undefined,
    },
    dataRule: {
    },
    optionsObj:{
      F_ProjcetItemIdProps: {'label':'F_GoodsName','value':'id'},
      F_ProcessIdProps: {'label':'F_ProcessName','value':'id'},
    },
    currVmodel: '',
    currTableConf:{},
    tableRows: {
},
    addTableConf:{
    },
    title: '',
    continueText: '',
    allList: [],
    currIndex: 0,
    isContinue: false,
    submitType: 0,
    showContinueBtn: true,
  });
  const { title, continueText, showContinueBtn, dataForm,submitType,dataRule, optionsObj } = toRefs(state);

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
    state.currIndex = state.allList.length && data.id ? state.allList.findIndex((item) => item.id === data.id) : 0;
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
      state.dataForm={
        F_ProjcetItemId: undefined,
        F_ProcessId: undefined,
        F_StartDate: undefined,
        F_EndDate: undefined,
        F_SortCode: undefined,
        F_Description: undefined,
        F_CreatorTime: undefined,
      };
      getF_ProjcetItemIdOptions();
      getF_ProcessIdOptions();
      if (getLeftTreeActiveInfo) state.dataForm = {...state.dataForm, ...(getLeftTreeActiveInfo() || {}) };
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
    getInfo(id).then((res) => {
      state.dataForm = res.data || {};
      getF_ProjcetItemIdOptions();
      getF_ProcessIdOptions();
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
        .then((res) => {
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
  function getF_ProjcetItemIdOptions() {
    let templateJson = [
  {
    "defaultValue": "",
    "field": "contractId",
    "dataType": "varchar",
    "required": 0,
    "fieldName": "合同ID",
    "relationField": "",
    "jnpfKey": null,
    "complexHeaderList": null,
    "sourceType": 3,
    "isChildren": false,
    "IsMultiple": false
  }
]
    let query = {
      paramList: getParamList(templateJson, state.dataForm)
    }
    getDataInterfaceRes('2012085692393459712', query).then(res => {
      let data = res.data;
      state.optionsObj.F_ProjcetItemIdOptions = Array.isArray(data) ? data : [];
    });
  }
  function getF_ProcessIdOptions() {
    let templateJson = []
    let query = {
      paramList: getParamList(templateJson, state.dataForm)
    }
    getDataInterfaceRes('2012092092830060544', query).then(res => {
      let data = res.data;
      state.optionsObj.F_ProcessIdOptions = Array.isArray(data) ? data : [];
    });
  }
  function openSelectDialog(key,value) {
    state.currTableConf = state.addTableConf[key+'List'+value]
    state.currVmodel = key
    nextTick(() => {
      (selectModal.value as any)?.openSelectModal();
    })
  }
  function addForSelect(data) {
    for (let i = 0; i < data.length; i++) {
      let item = {...state.tableRows[state.currVmodel],...data[i],jnpfId:buildUUID()}
      state.dataForm[state.currVmodel].push(cloneDeep(item));
      if(state[state.currVmodel+'DefaultExpandAll']) state[state.currVmodel+'innerActiveKey'].push(item.jnpfId);
    }
  }
  function getParamList(templateJson, formData, index?) {
    for (let i = 0; i < templateJson.length; i++) {
      if (templateJson[i].relationField && templateJson[i].sourceType == 1) {
        //区分是否子表
        if (templateJson[i].relationField.includes('-')) {
          let tableVModel = templateJson[i].relationField.split('-')[0]
          let childVModel = templateJson[i].relationField.split('-')[1]
          templateJson[i].defaultValue = formData[tableVModel] && formData[tableVModel][index] && formData[tableVModel][index][childVModel] || ''
        } else {
          templateJson[i].defaultValue = formData[templateJson[i].relationField] || ''
        }
      }
    }
    return templateJson
  }
</script>
