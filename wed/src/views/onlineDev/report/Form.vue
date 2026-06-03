<template>
  <BasicModal v-bind="$attrs" @register="registerModal" :title="dataForm.id ? t('common.editText') : t('common.addText')" @ok="handleSubmit()">
    <BasicForm @register="registerForm" />
    <template #appendFooter>
      <a-button type="primary" :loading="btnLoading" @click="handleSubmit(1)">确定并设计</a-button>
    </template>
  </BasicModal>
</template>
<script lang="ts" setup>
  import { toRefs, reactive } from 'vue';
  import { BasicModal, useModalInner } from '@/components/Modal';
  import { BasicForm, useForm } from '@/components/Form';
  import formValidate from '@/utils/formValidate';
  import { getReportInfo, updateReport, createReport } from '@/api/onlineDev/report';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useI18n } from '@/hooks/web/useI18n';

  interface State {
    dataForm: any;
    btnLoading: boolean;
  }

  const state = reactive<State>({
    dataForm: {},
    btnLoading: false,
  });
  const { dataForm, btnLoading } = toRefs(state);
  const { createMessage } = useMessage();
  const { t } = useI18n();
  const emit = defineEmits(['register', 'reload', 'design']);
  const [registerModal, { closeModal, changeLoading, changeOkLoading }] = useModalInner(init);
  const [registerForm, { setFieldsValue, resetFields, validate, updateSchema }] = useForm({
    schemas: [
      {
        field: 'fullName',
        label: '名称',
        component: 'Input',
        componentProps: { placeholder: '请输入', maxlength: 100 },
        rules: [{ required: true, trigger: 'blur', message: '必填' }],
      },
      {
        field: 'enCode',
        label: '编码',
        component: 'Input',
        componentProps: { placeholder: '请输入', maxlength: 50 },
        rules: [
          { required: true, trigger: 'blur', message: '必填' },
          { validator: formValidate('enCode'), trigger: 'blur' },
        ],
      },
      {
        field: 'category',
        label: '分类',
        component: 'Select',
        componentProps: { placeholder: '请选择', showSearch: true },
        rules: [{ required: true, trigger: 'change', message: '必填' }],
      },
      {
        field: 'sortCode',
        label: '排序',
        defaultValue: 0,
        component: 'InputNumber',
        componentProps: { min: 0, max: 999999 },
      },
      {
        field: 'description',
        label: '说明 ',
        component: 'Textarea',
        componentProps: { placeholder: '请输入' },
      },
    ],
  });

  function init(data) {
    resetFields();
    state.dataForm.id = data.id || '';
    updateSchema([{ field: 'category', componentProps: { options: data.categoryList || [] } }]);
    if (state.dataForm.id) {
      changeLoading(true);
      getReportInfo(state.dataForm.id)
        .then(res => {
          state.dataForm = res.data;
          setFieldsValue(state.dataForm);
          changeLoading(false);
        })
        .catch(() => {
          changeLoading(false);
        });
    }
  }
  async function handleSubmit(type = 0) {
    const values = await validate();
    if (!values) return;
    type === 1 ? (state.btnLoading = true) : changeOkLoading(true);
    const query = { ...values, id: state.dataForm.id };
    const formMethod = state.dataForm.id ? updateReport : createReport;
    formMethod(query)
      .then(res => {
        createMessage.success(res.msg);
        changeOkLoading(false);
        state.btnLoading = false;
        closeModal();
        emit('reload');
        if (!state.dataForm.id) state.dataForm.id = res.data;
        if (type == 1) emit('design', { ...values, id: state.dataForm.id });
      })
      .catch(() => {
        changeOkLoading(false);
        state.btnLoading = false;
      });
  }
</script>
