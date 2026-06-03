<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    defaultFullscreen
    :footer="null"
    :closable="false"
    :keyboard="false"
    class="jnpf-full-modal full-modal designer-modal">
    <template #title>
      <div class="jnpf-full-modal-header">
        <div class="header-title">
          <img src="@/assets/images/jnpf.png" class="header-logo" />
          <span class="header-dot"></span>
          <p class="header-txt" v-if="!activeStep">表单设计</p>
          <a-tooltip :title="dataForm.fullName" v-else>
            <p class="header-txt">{{ dataForm.fullName }}</p>
          </a-tooltip>
        </div>
        <a-steps v-model:current="activeStep" type="navigation" size="small" class="header-steps tab-steps">
          <a-step title="基础设置" />
          <a-step title="表单设计" disabled />
        </a-steps>
        <a-space class="options" :size="10">
          <a-space-compact block>
            <a-button shape="round" @click="handlePrev" :disabled="activeStep <= 0 || btnLoading">{{ t('common.prev') }}</a-button>
            <a-button shape="round" @click="handleNext" :disabled="activeStep >= 1 || loading || btnLoading">{{ t('common.next') }} </a-button>
          </a-space-compact>
          <a-button shape="round" type="primary" @click="handleSubmit()" :disabled="loading" :loading="btnLoading">{{ t('common.saveText') }}</a-button>
          <a-button shape="round" @click="handleCancel()">{{ t('common.closeText') }}</a-button>
        </a-space>
      </div>
    </template>
    <a-row type="flex" justify="center" align="middle" class="basic-content" v-show="!activeStep">
      <a-col :span="12" :xxl="10" class="basic-form">
        <BasicForm @register="registerForm" />
      </a-col>
    </a-row>
    <template v-if="activeStep == 1">
      <a-row type="flex" justify="center" align="middle" class="basic-content">
        <a-col :span="12" :xxl="10" class="basic-form">
          <FieldForm ref="fieldFormRef" :conf="formData" />
        </a-col>
      </a-row>
    </template>
  </BasicModal>
</template>
<script lang="ts" setup>
  import { getInfo, create, update } from '@/api/onlineDev/visualDev';
  import { ref, reactive, toRefs, unref } from 'vue';
  import { BasicModal, useModalInner } from '@/components/Modal';
  import { BasicForm, useForm } from '@/components/Form';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useI18n } from '@/hooks/web/useI18n';
  import formValidate from '@/utils/formValidate';
  import FieldForm from './FieldForm.vue';

  interface State {
    activeStep: number;
    loading: boolean;
    btnLoading: boolean;
    dataForm: Recordable;
    formData: any;
    isReload: boolean;
  }
  interface ComType {
    getData: () => any;
  }

  const emit = defineEmits(['register', 'reload']);
  const [registerForm, { setFieldsValue, resetFields, validate }] = useForm({
    schemas: [
      {
        field: 'fullName',
        label: '表单名称',
        component: 'Input',
        componentProps: { placeholder: '请输入', maxlength: 50 },
        rules: [{ required: true, trigger: 'blur', message: '必填' }],
      },
      {
        field: 'enCode',
        label: '表单编码',
        component: 'Input',
        componentProps: { placeholder: '请输入', maxlength: 50 },
        rules: [
          { required: true, trigger: 'blur', message: '必填' },
          { validator: formValidate('enCode'), trigger: 'blur' },
        ],
      },
      {
        field: 'type',
        label: '表单类型',
        component: 'Select',
        componentProps: { options: [{ id: 2, fullName: '系统导入' }], disabled: true, showArrow: false },
        rules: [{ required: true, trigger: 'change', message: '必填', type: 'number' }],
      },
      {
        field: 'urlAddress',
        label: 'Web地址',
        helpMessage: 'Web地址配置为前端物理地址',
        component: 'Input',
        componentProps: { placeholder: '请输入', addonBefore: '@/views/' },
      },
      {
        field: 'appUrlAddress',
        label: 'App地址',
        helpMessage: 'APP地址配置为前端物理地址，需与代码同步更新',
        component: 'Input',
        componentProps: { placeholder: '请输入' },
      },
      {
        field: 'interfaceUrl',
        label: '接口地址',
        helpMessage: ['后端接口请求地址', ' 系统将会请求地址中的saveData(post方法), getData(get方法)', '接口例：/api/example/UserController'],
        component: 'Input',
        componentProps: { placeholder: '请输入' },
      },
      {
        field: 'sortCode',
        label: '表单排序',
        defaultValue: 0,
        component: 'InputNumber',
        componentProps: { min: 0, max: 999999 },
      },
      {
        field: 'description',
        label: '表单说明',
        component: 'Textarea',
        componentProps: { placeholder: '请输入' },
      },
    ],
  });
  const [registerModal, { closeModal, changeLoading }] = useModalInner(init);
  const { createMessage } = useMessage();
  const { t } = useI18n();
  const state = reactive<State>({
    activeStep: 0,
    loading: false,
    btnLoading: false,
    dataForm: {
      id: '',
      fullName: '',
      enCode: '',
      type: 2,
      webType: 1,
      dbLinkId: '0',
      sortCode: 0,
      state: 1,
      category: '',
      description: '',
      tables: '',
      urlAddress: '',
      appUrlAddress: '',
      interfaceUrl: '',
    },
    formData: null,
    isReload: false,
  });
  const fieldFormRef = ref<Nullable<ComType>>(null);
  const { activeStep, loading, btnLoading, formData, dataForm } = toRefs(state);

  function init(data) {
    state.isReload = false;
    state.activeStep = 0;
    state.loading = true;
    state.formData = null;
    changeLoading(true);
    resetFields();
    state.dataForm.id = data.id || '';
    if (state.dataForm.id) {
      getInfo(state.dataForm.id).then(res => {
        state.dataForm = res.data;
        setFieldsValue(state.dataForm);
        state.formData = state.dataForm.formData && JSON.parse(state.dataForm.formData);
        state.loading = false;
        changeLoading(false);
      });
    } else {
      setFieldsValue({ type: 2 });
      state.loading = false;
      changeLoading(false);
    }
  }
  function handlePrev() {
    state.activeStep -= 1;
  }
  async function handleNext() {
    if (state.activeStep < 1) {
      const values = await validate();
      if (!values) return;
      state.dataForm = { ...state.dataForm, ...values };
      state.activeStep += 1;
    }
  }
  async function handleSubmit() {
    if (state.activeStep < 1) {
      const values = await validate();
      if (!values) return;
      state.dataForm = { ...state.dataForm, ...values };
      handleRequest();
    } else {
      (unref(fieldFormRef) as ComType)
        .getData()
        .then(res => {
          state.formData = res.formData;
          handleRequest();
        })
        .catch(err => {
          err.msg && createMessage.warning(err.msg);
        });
    }
  }
  function handleRequest() {
    state.btnLoading = true;
    const query = {
      ...state.dataForm,
      formData: state.formData ? JSON.stringify(state.formData) : null,
    };
    const formMethod = state.dataForm.id ? update : create;
    formMethod(query)
      .then(res => {
        createMessage.success(res.msg);
        state.btnLoading = false;
        state.isReload = true;
        if (!state.dataForm.id) state.dataForm.id = res.data;
      })
      .catch(() => {
        state.btnLoading = false;
      });
  }
  function handleCancel() {
    closeModal();
    if (state.isReload) emit('reload');
  }
</script>
