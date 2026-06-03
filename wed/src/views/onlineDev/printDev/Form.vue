<template>
  <BasicModal v-bind="$attrs" @register="registerModal" :title="dataForm.id ? t('common.editText') : t('common.addText')" @ok="handleSubmit()">
    <BasicForm @register="registerForm">
      <template #icon="{ model, field }">
        <div class="flex">
          <div class="flex-1 mr-10px">
            <jnpf-icon-picker v-model:value="model[field]" placeholder="请选择" />
          </div>
          <a-form-item-rest>
            <jnpf-color-picker v-model:value="state.iconBackground" size="small" :predefine="predefineList" name="iconBackground" />
          </a-form-item-rest>
        </div>
      </template>
    </BasicForm>
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
  import { getPrintDevInfo, updatePrintDev, createPrintDev } from '@/api/system/printDev';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useI18n } from '@/hooks/web/useI18n';

  interface State {
    dataForm: any;
    btnLoading: boolean;
    iconBackground: string;
  }

  const predefineList = ['#008cff', '#35b8e0', '#00cc88', '#ff9d00', '#ff4d4d', '#5b69bc', '#ff8acc', '#3b3e47', '#282828'];
  const state = reactive<State>({
    dataForm: {},
    btnLoading: false,
    iconBackground: '',
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
        field: 'commonUse',
        label: '通用',
        helpMessage: '启用后，将该打印设计设为通用打印模板',
        defaultValue: 0,
        component: 'Switch',
      },
      {
        ifShow: ({ values }) => values.commonUse === 1,
        field: 'visibleType',
        label: '发布范围',
        defaultValue: 1,
        component: 'Radio',
        componentProps: {
          options: [
            { fullName: '公开', id: 1 },
            { fullName: '权限设置', id: 2 },
          ],
        },
      },
      {
        ifShow: ({ values }) => values.commonUse === 1,
        field: 'icon',
        label: '图标',
        slot: 'icon',
        component: 'IconPicker',
        rules: [{ required: true, trigger: 'change', message: '必填' }],
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
    state.iconBackground = '#008cff';
    state.dataForm.id = data.id || '';
    updateSchema([{ field: 'category', componentProps: { options: data.categoryList || [] } }]);
    if (state.dataForm.id) {
      changeLoading(true);
      getPrintDevInfo(state.dataForm.id)
        .then(res => {
          state.dataForm = res.data;
          state.iconBackground = state.dataForm.iconBackground || '#008cff';
          state.dataForm.visibleType = state.dataForm.visibleType || 1;
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
    const query = { ...values, iconBackground: state.iconBackground, id: state.dataForm.id };
    const formMethod = state.dataForm.id ? updatePrintDev : createPrintDev;
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
