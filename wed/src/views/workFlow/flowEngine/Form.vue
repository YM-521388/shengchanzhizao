<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    :title="state.dataForm.id ? t('common.editText') : t('common.addText')"
    :width="600"
    @ok="handleSubmit"
    destroyOnClose>
    <BasicForm @register="registerForm">
      <template #icon="{ model, field }">
        <div class="flex">
          <div class="flex-1 mr-10px">
            <jnpf-icon-picker v-model:value="model[field]" placeholder="请选择" />
          </div>
          <a-form-item-rest>
            <jnpf-color-picker
              v-model:value="state.iconBackground"
              optionType="button"
              button-style="solid"
              size="small"
              :predefine="predefineList"
              name="iconBackground" />
          </a-form-item-rest>
        </div>
      </template>
    </BasicForm>
    <template #appendFooter>
      <a-button type="primary" @click="handleSubmit(1)" :loading="state.btnLoading">确定并设计</a-button>
    </template>
  </BasicModal>
</template>
<script lang="ts" setup>
  import { reactive } from 'vue';
  import { BasicModal, useModalInner } from '@/components/Modal';
  import { useI18n } from '@/hooks/web/useI18n';
  import formValidate from '@/utils/formValidate';
  import { BasicForm, useForm } from '@/components/Form';
  import { getInfo, create, update } from '@/api/workFlow/template';
  import { useMessage } from '@/hooks/web/useMessage';

  interface State {
    iconBackground: string;
    dataForm: any;
    btnLoading: boolean;
    showType: number;
    type: number;
  }

  const state = reactive<State>({
    iconBackground: '#008cff',
    dataForm: {},
    btnLoading: false,
    showType: 0,
    type: 0,
  });
  const emit = defineEmits(['register', 'reload', 'design']);
  const { t } = useI18n();
  const { createMessage } = useMessage();
  const predefineList = ['#008cff', '#35b8e0', '#00cc88', '#ff9d00', '#ff4d4d', '#5b69bc', '#ff8acc', '#3b3e47', '#282828'];
  const [registerModal, { closeModal, changeLoading, changeOkLoading }] = useModalInner(init);
  const [registerForm, { setFieldsValue, validate, updateSchema }] = useForm({
    schemas: [
      {
        field: 'fullName',
        label: '流程名称',
        component: 'Input',
        componentProps: { placeholder: '请输入', maxlength: 50 },
        rules: [{ required: true, trigger: 'blur', message: '必填' }],
      },
      {
        field: 'enCode',
        label: '流程编码',
        component: 'Input',
        componentProps: { placeholder: '请输入', maxlength: 50 },
        rules: [
          { required: true, trigger: 'blur', message: '必填' },
          { validator: formValidate('enCode'), trigger: 'blur' },
        ],
      },
      {
        field: 'category',
        label: '流程分类',
        component: 'Select',
        componentProps: { placeholder: '请选择', showSearch: true },
        rules: [{ required: true, trigger: 'change', message: '必填' }],
      },
      {
        field: 'icon',
        label: '流程图标',
        slot: 'icon',
        component: 'IconPicker',
        rules: [{ required: true, trigger: 'change', message: '必填' }],
      },
      {
        field: 'showType',
        label: '显示位置',
        defaultValue: 0,
        ifShow: () => state.type != 2,
        component: 'Radio',
        componentProps: {
          optionType: 'button',
          buttonStyle: 'solid',
          options: [
            { fullName: '都显示', id: 0 },
            { fullName: '仅发起流程显示', id: 1 },
            { fullName: '仅菜单显示', id: 2 },
          ],
        },
      },
      {
        field: 'sortCode',
        label: '流程排序',
        defaultValue: 0,
        component: 'InputNumber',
        componentProps: { min: 0, max: 999999 },
      },
      {
        field: 'description',
        label: '流程说明',
        component: 'Textarea',
        componentProps: { placeholder: '请输入' },
      },
    ],
  });

  function init(data) {
    state.type = data.type;
    state.iconBackground = '#008cff';
    updateSchema([{ field: 'category', componentProps: { options: data.categoryList || [] } }]);
    state.dataForm.id = data.id;
    if (state.dataForm.id) {
      getInfo(state.dataForm.id).then(res => {
        state.dataForm = res.data;
        state.iconBackground = state.dataForm.iconBackground || '#008cff';
        setFieldsValue(state.dataForm);
        changeLoading(false);
      });
    } else {
      state.dataForm.type = data.type || 0;
      changeLoading(false);
    }
  }
  async function handleSubmit(type?) {
    const values = await validate();
    if (!values) return;
    changeOkLoading(true);
    state.btnLoading = true;
    const query = {
      ...values,
      iconBackground: state.iconBackground,
      id: state.dataForm.id,
      type: state.dataForm.type,
    };
    const formMethod = state.dataForm.id ? update : create;
    formMethod(query)
      .then(res => {
        createMessage.success(res.msg);
        changeOkLoading(false);
        state.btnLoading = false;
        closeModal();
        emit('reload');
        if (type === 1) emit('design', { id: state.dataForm.id || res.data, fullName: state.dataForm.fullName });
      })
      .catch(() => {
        changeOkLoading(false);
        state.btnLoading = false;
      });
  }
</script>
