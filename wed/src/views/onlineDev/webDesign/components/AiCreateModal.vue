<template>
  <BasicModal v-bind="getBindValue" @register="registerModal" @ok="handleSubmit" destroyOnClose>
    <div class="!pb-35px">
      <div class="text-tips">通过AI生成表单 ，根据文字描述智能生成推荐表单</div>
      <div class="ai-textarea">
        <jnpf-textarea v-model:value="content" placeholder="输入你的需求描述" :maxlength="100" />
        <div class="ai-button">
          <loading-outlined class="mr-5px" v-if="loading" />
          <i class="ym-custom ym-custom-send cursor-pointer" :class="{ 'icon-selected': content }" v-else @click="handleAiSubmit" />
        </div>
      </div>
    </div>
    <a-form :colon="false" :labelCol="{ style: { width: '77px' } }" :model="dataForm" :rules="rules" ref="formElRef" v-if="showForm">
      <div class="text-tips">表单已生成，请继续确认表单名称、编码及分类</div>
      <a-form-item label="表单名称" name="fullName">
        <jnpf-input v-model:value="dataForm.fullName" placeholder="请输入" :maxlength="100" />
      </a-form-item>
      <a-form-item label="表单编码" name="enCode">
        <jnpf-input v-model:value="dataForm.enCode" placeholder="请输入" :maxlength="50" />
      </a-form-item>
      <a-form-item label="表单分类" name="category">
        <jnpf-select v-model:value="dataForm.category" :options="categoryOptions" showSearch />
      </a-form-item>
    </a-form>
  </BasicModal>
</template>
<script lang="ts" setup>
  import { reactive, ref, toRefs, nextTick, computed, unref } from 'vue';
  import { getAiInfo, create } from '@/api/onlineDev/visualDev';
  import { BasicModal, useModalInner } from '@/components/Modal';
  import type { FormInstance } from 'ant-design-vue';
  import { LoadingOutlined } from '@ant-design/icons-vue';
  import { useAttrs } from '@/hooks/core/useAttrs';
  import formValidate from '@/utils/formValidate';
  import { buildAiFormData } from '@/components/FormGenerator/src/helper/aiUtils';
  import { omit } from 'lodash-es';
  import { useMessage } from '@/hooks/web/useMessage';

  interface State {
    dataForm: any;
    content: string;
    loading: boolean;
    categoryOptions: any[];
    showForm: boolean;
  }

  const emit = defineEmits(['register', 'reload']);
  const { createMessage } = useMessage();
  const [registerModal, { changeOkLoading, closeModal }] = useModalInner(init);
  const formElRef = ref<FormInstance>();
  const state = reactive<State>({
    dataForm: {},
    content: '',
    loading: false,
    categoryOptions: [],
    showForm: false,
  });
  const { dataForm, content, loading, categoryOptions, showForm } = toRefs(state);
  const attrs = useAttrs({ excludeDefaultKeys: false });
  const rules = {
    fullName: [{ required: true, message: '必填', trigger: 'blur' }],
    enCode: [
      { required: true, message: '必填', trigger: 'blur' },
      { validator: formValidate('enCode'), trigger: 'blur' },
    ],
    category: [{ required: true, message: '必填', trigger: 'change' }],
  };

  const getBindValue = computed(() => {
    const attr: any = {
      ...unref(attrs),
      title: 'AI建表',
      okButtonProps: {
        disabled: state.loading,
      },
    };
    if (!state.showForm) attr.footer = null;
    return attr;
  });

  function init(data) {
    state.categoryOptions = data.categoryList || [];
    resetData();
    nextTick(() => {
      formElRef.value?.clearValidate();
    });
  }
  function resetData() {
    state.content = '';
    state.loading = false;
    state.showForm = false;
    state.dataForm = {};
  }
  function handleAiSubmit() {
    if (!state.content) return;
    state.loading = true;
    getAiInfo({ keyword: state.content })
      .then(res => {
        state.showForm = true;
        state.loading = false;
        state.dataForm = { ...res.data, category: state.dataForm.category };
      })
      .catch(() => {
        state.loading = false;
      });
  }
  async function handleSubmit() {
    try {
      const values = await formElRef.value?.validate();
      if (!values) return;
      state.dataForm = { ...buildAiFormData(state.dataForm.aiModelList || []), ...omit(state.dataForm, ['aiModelList']) };
      changeOkLoading(true);
      create(state.dataForm)
        .then(res => {
          changeOkLoading(false);
          closeModal();
          createMessage.success(res.msg);
          emit('reload', res.data);
        })
        .catch(() => {
          changeOkLoading(false);
        });
    } catch (_) {}
  }
</script>
<style lang="less" scoped>
  .ai-textarea {
    position: relative;
    .ai-button {
      position: absolute;
      bottom: 3px;
      right: 8px;
    }
  }
  .icon-selected {
    color: @primary-color!important;
  }
  .text-tips {
    margin: 10px 20px 20px 0;
    color: @text-color-label;
  }
</style>
