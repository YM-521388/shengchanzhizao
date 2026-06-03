<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    defaultFullscreen
    :footer="null"
    :closable="false"
    :keyboard="false"
    class="jnpf-full-modal full-modal designer-modal"
    destroy-on-close>
    <template #title>
      <div class="jnpf-full-modal-header">
        <div class="header-title">
          <img src="@/assets/images/jnpf.png" class="header-logo" />
          <span class="header-dot"></span>
          <p class="header-txt" v-if="!activeStep">模板设计</p>
          <a-tooltip :title="dataForm.fullName" v-else>
            <p class="header-txt">{{ dataForm.fullName }}</p>
          </a-tooltip>
        </div>
        <a-steps v-model:current="activeStep" type="navigation" size="small" class="header-steps">
          <a-step title="基础设计" />
          <a-step title="模板设计" :disabled="activeStep <= 1" />
        </a-steps>
        <a-space class="options" :size="10">
          <ValidatePopover ref="validatePopoverRef" :errorList="errorList" @select="handleSelect" v-if="activeStep == 1" />
          <a-space-compact block>
            <a-button shape="round" @click="handlePrev" :disabled="activeStep <= 0 || btnLoading">{{ t('common.prev') }}</a-button>
            <a-button shape="round" @click="handleNext" :disabled="activeStep >= 1 || loading || btnLoading">{{ t('common.next') }} </a-button>
          </a-space-compact>
          <a-button shape="round" type="primary" @click="handleSubmit()" :disabled="activeStep <= 0 || loading" :loading="btnLoading">
            {{ t('common.saveText') }}
          </a-button>
          <a-button shape="round" @click="handleCancel()">{{ t('common.closeText') }}</a-button>
        </a-space>
      </div>
    </template>
    <a-row type="flex" justify="center" align="middle" class="basic-content" v-show="!activeStep">
      <a-col :span="12" :xxl="10" class="basic-form">
        <BasicForm @register="registerForm" />
      </a-col>
    </a-row>
    <FormGenerator ref="generatorRef" :conf="formData" :formInfo="dataForm" :dbType="dbType" v-if="activeStep == 1" />
  </BasicModal>
</template>
<script lang="ts" setup>
  import { getInfo, create, update } from '@/api/system/kit';
  import { ref, reactive, toRefs, unref } from 'vue';
  import { BasicModal, useModalInner } from '@/components/Modal';
  import { BasicForm, useForm } from '@/components/Form';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useI18n } from '@/hooks/web/useI18n';
  import { useGeneratorStore } from '@/store/modules/generator';
  import formValidate from '@/utils/formValidate';
  import { FormGenerator } from '@/components/FormGenerator';
  import { ValidatePopover } from '@/components/CommonModal';

  interface State {
    activeStep: number;
    loading: boolean;
    btnLoading: boolean;
    tables: any[];
    dataForm: Recordable;
    isReload: boolean;
    [prop: string]: any;
    errorList: any[];
  }
  interface ComType {
    getData: () => any;
    setVisible: (data) => any;
    activeFormItemById: (data) => any;
    setTabActiveKey: (data) => any;
  }

  const emit = defineEmits(['register', 'reload']);
  const [registerForm, { setFieldsValue, resetFields, validate, updateSchema }] = useForm({
    schemas: [
      {
        field: 'fullName',
        label: '模板名称',
        component: 'Input',
        componentProps: { placeholder: '请输入', maxlength: 100 },
        rules: [{ required: true, trigger: 'blur', message: '必填' }],
      },
      {
        field: 'enCode',
        label: '模板编码',
        component: 'Input',
        componentProps: { placeholder: '请输入', maxlength: 50 },
        rules: [
          { required: true, trigger: 'blur', message: '必填' },
          { validator: formValidate('enCode'), trigger: 'blur' },
        ],
      },
      {
        field: 'category',
        label: '模板分类',
        component: 'Select',
        componentProps: { placeholder: '请选择', showSearch: true },
        rules: [{ required: true, trigger: 'change', message: '必填' }],
      },
      {
        field: 'icon',
        label: '模板图标',
        component: 'IconPicker',
        rules: [{ required: true, trigger: 'change', message: '必填' }],
      },
      {
        field: 'sortCode',
        label: '模板排序',
        defaultValue: 0,
        component: 'InputNumber',
        componentProps: { min: 0, max: 999999 },
      },
      {
        field: 'enabledMark',
        label: '模板状态',
        component: 'Switch',
        defaultValue: 1,
      },
      {
        field: 'description',
        label: '模板说明',
        component: 'Textarea',
        componentProps: { placeholder: '请输入' },
      },
    ],
  });
  const [registerModal, { closeModal, changeLoading }] = useModalInner(init);
  const { createMessage } = useMessage();
  const generatorStore = useGeneratorStore();
  const { t } = useI18n();
  const validatePopoverRef = ref<Nullable<ComType>>(null);
  const state = reactive<State>({
    activeStep: 0,
    loading: false,
    btnLoading: false,
    tables: [],
    dataForm: {
      id: '',
      fullName: '',
      enCode: '',
      icon: '',
      sortCode: 0,
      enabledMark: 1,
      category: '',
      description: '',
    },
    formData: null,
    dbType: 'MySQL',
    isReload: false,
    errorList: [],
  });
  const generatorRef = ref<Nullable<ComType>>(null);
  const { activeStep, loading, btnLoading, dbType, formData, dataForm, errorList } = toRefs(state);

  function init(data) {
    state.isReload = false;
    state.activeStep = 0;
    state.loading = true;
    state.formData = null;
    state.errorList = [];
    updateSchema([{ field: 'category', componentProps: { options: data.categoryList } }]);
    changeLoading(true);
    resetFields();
    state.dataForm.id = data.id;
    if (state.dataForm.id) {
      getInfo(state.dataForm.id).then(res => {
        state.dataForm = res.data;
        setFieldsValue(state.dataForm);
        state.formData = state.dataForm.formData && JSON.parse(state.dataForm.formData);
        handleNext();
        state.loading = false;
        changeLoading(false);
      });
    } else {
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
      state.dataForm = { ...state.dataForm, ...values, isKit: true };
      generatorStore.setHasTable(false);
      generatorStore.setAllTable([]);
      generatorStore.setFormItemList([]);
      state.activeStep += 1;
    } else if (state.activeStep === 1) {
      (unref(generatorRef) as ComType)
        .getData()
        .then(res => {
          state.formData = res.formData;
          state.dataForm.formData = state.formData ? JSON.stringify(state.formData) : null;
          state.activeStep += 1;
        })
        .catch(err => {
          err.msg && createMessage.warning(err.msg);
        });
    }
  }
  async function handleSubmit() {
    if (state.activeStep < 1) {
      const values = await validate();
      if (!values) return;
      state.dataForm = { ...state.dataForm, ...values };
      handleRequest();
    } else {
      (unref(generatorRef) as ComType)
        .getData()
        .then(res => {
          state.errorList = res.errorList || [];
          if (state.errorList.length) {
            setTimeout(() => {
              unref(validatePopoverRef)?.setVisible(true);
            }, 10);
            return;
          }
          state.formData = res.formData;
          state.dataForm.formData = state.formData ? JSON.stringify(state.formData) : null;
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
  function handleSelect(id) {
    unref(generatorRef)?.setTabActiveKey('field');
    unref(generatorRef)?.activeFormItemById(id);
  }
</script>
