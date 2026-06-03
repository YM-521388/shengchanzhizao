<template>
  <a-row class="dynamic-form">
    <a-form :colon="false" layout="horizontal" labelAlign="right" :labelCol="{ style: { width: '100px' } }"
      :model="dataForm" :rules="dataRule" ref="formRef">
      <a-row :gutter="15">
        <a-col :span="24" class="ant-col-item">
          <a-form-item name="F_RoutingNo" :labelCol="{ style: { width: '100px' } }">
            <template #label>工艺路线编号</template>
            <JnpfInput v-model:value="dataForm.F_RoutingNo" placeholder="请填写，忽略将自动生产" allowClear :style="{
              width: '100%',
            }" :showCount="false" @update:value="handleChange" />
          </a-form-item>
        </a-col>
        <a-col :span="24" class="ant-col-item">
          <a-form-item name="F_RoutingName" :labelCol="{ style: { width: '100px' } }">
            <template #label>工艺路线名称</template>
            <JnpfInput v-model:value="dataForm.F_RoutingName" placeholder="请输入" allowClear :style="{
              width: '100%',
            }" :showCount="false" @update:value="handleChange" />
          </a-form-item>
        </a-col>

      </a-row>
    </a-form>
  </a-row>
</template>
<script lang="ts" setup>
import { watch, reactive, toRefs, nextTick, ref, unref, computed, toRaw, inject, onMounted } from 'vue';
import { useMessage } from '@/hooks/web/useMessage';
import { useI18n } from '@/hooks/web/useI18n';
import { useUserStore } from '@/store/modules/user';
import type { FormInstance } from 'ant-design-vue';


const props = defineProps({
  value: { default: '' },
  title: { type: String, default: '' },
  placeholder: { type: String, default: '请选择' },
  disabled: { type: Boolean, default: false },
  allowClear: { type: Boolean, default: true },
  size: { type: String, default: 'default' },
  isStartForm: { type: Number, default: 0 },
  type: { type: String, default: '' },
  bpmn: { type: Object, default: null },
  formConf: { type: Object, default: null },
  updateBpmnProperties: { type: Function, default: () => { } },
});

interface State {
  dataForm: any;
  dataRule: any;
}

const emit = defineEmits(['change']);
const userStore = useUserStore();
const userInfo = userStore.getUserInfo;
// const { createMessage, createConfirm } = useMessage();
const { t } = useI18n();

const formRef = ref<FormInstance>();

const state = reactive<State>({
  dataForm: {
    F_RoutingNo: undefined,
    F_RoutingName: undefined,
    fullName: undefined,
  },
  dataRule: {
    F_RoutingName: [
      {
        required: true,
        message: '请输入工艺路线名称',
        trigger: 'blur',
      },
    ],
  },
});
const { dataForm, dataRule } = toRefs(state);


watch(
  () => props.value,
  val => {
    setValue();
  },
  { immediate: true },
);
onMounted(() => {
  getDataInfo()
})
function setValue() {
  // 只有当 props.value 为真时才从 localStorage 加载数据
  if (!props.value) return;
  getDataInfo()

}

function getDataInfo() {
  resetDataForm()
  let data = localStorage.getItem('getAProdRoutingFromData');
  if (data) {
    let fromData = JSON.parse(data);
    // 将 fromData 中的对应键赋值给 state.dataForm（只合并存在的键）
    if (fromData.formData && typeof fromData.formData === 'object') {
      Object.keys(fromData.formData).forEach(key => {
        if (key in state.dataForm) {
          state.dataForm[key] = fromData.formData[key];
        }
      });
      state.dataForm.fullName = state.dataForm.F_RoutingName;
    }
  }
}
// 重置 state.dataForm 为初始值
function resetDataForm() {
  state.dataForm = {
    F_RoutingNo: undefined,
    F_RoutingName: undefined,
     fullName: undefined,
  }
}

function getForm() {
  const form = unref(formRef);
  if (!form) {
    throw new Error('form is null!');
  }
  return form;
}

async function handleChange() {
  try {
    const values = await getForm()?.validate();
    if (!values) return;
    // let formId = props.value ? props.value : buildUUID();
    let formId = ''
    let data = {
      formId, fullName: state.dataForm.F_RoutingName, formData: state.dataForm
    }
    state.dataForm.fullName = state.dataForm.F_RoutingName
    emit('change', formId, state.dataForm);
    localStorage.setItem('getAProdRoutingFromData', JSON.stringify(data));
  } catch (err) { }
}
</script>
<style lang="less" scoped>
/deep/.ant-form-item {
  margin-bottom: 20px !important;
}
</style>
