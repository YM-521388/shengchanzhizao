<template>
  <BasicModal v-bind="$attrs" @register="registerModal" title="生成菜单" @ok="handleSubmit" class="jnpf-release-modal">
    <a-alert message="将该报表发布至应用菜单" type="warning" show-icon />
    <a-form class="release-main" :colon="false" :model="dataForm" :rules="rules" layout="vertical" ref="formElRef">
      <div class="release-item report-item-left">
        <a-form-item>
          <div class="top-item" :class="{ active: dataForm.pc === 1 }" @click="selectToggle('pc')">
            <i class="item-icon icon-ym icon-ym-pc"></i>
            <p class="item-title">桌面端</p>
            <div class="icon-checked">
              <check-outlined />
            </div>
          </div>
        </a-form-item>
        <a-form-item label="上级" name="pcModuleParentId" v-if="dataForm.pc">
          <JnpfTreeSelect
            v-model:value="pcModuleParentId"
            :options="treeData"
            treeCheckStrictly
            multiple
            :dropdownMatchSelectWidth="false"
            @change="onPcChange" />
        </a-form-item>
        <a-form-item label="已发布菜单路径" v-if="record.pcIsRelease">
          <div class="released">{{ record.pcReleaseName }}</div>
        </a-form-item>
      </div>
    </a-form>
  </BasicModal>
</template>
<script lang="ts" setup>
  import { getReleaseMenu, createMenu } from '@/api/onlineDev/report';
  import { BasicModal, useModalInner } from '@/components/Modal';
  import { ref, reactive, toRefs, computed } from 'vue';
  import type { FormInstance } from 'ant-design-vue';
  import { CheckOutlined } from '@ant-design/icons-vue';
  import { getMenuSelectorFilter } from '@/api/system/menu';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useI18n } from '@/hooks/web/useI18n';

  interface State {
    dataForm: any;
    record: any;
    treeData: any[];
    pcModuleParentId: any[];
  }

  const emit = defineEmits(['register', 'reload']);
  const { createMessage, createConfirm } = useMessage();
  const { t } = useI18n();
  const [registerModal, { changeOkLoading, closeModal }] = useModalInner(init);
  const formElRef = ref<FormInstance>();
  const state = reactive<State>({
    dataForm: {
      pc: 1,
      pcModuleParentId: [],
    },
    record: {},
    treeData: [],
    pcModuleParentId: [],
  });
  const { dataForm, record, treeData, pcModuleParentId } = toRefs(state);

  const rules = computed(() => {
    let rules: any = {
      pcModuleParentId: [],
    };
    if (!state.record.pcIsRelease) rules.pcModuleParentId = [{ required: true, message: '必填', trigger: 'change', type: 'array' }];
    return rules;
  });

  function init(data) {
    state.pcModuleParentId = [];
    getReleaseMenu(data.id).then(res => {
      state.record = res.data;
      const platformRelease = res.data.platformRelease ? JSON.parse(res.data.platformRelease) : {};
      state.dataForm = {
        pc: platformRelease.pc === 0 ? 0 : 1,
        pcModuleParentId: [],
      };
      formElRef.value?.clearValidate();
    });
    getMenuOptions(data.id);
  }
  function getMenuOptions(id) {
    getMenuSelectorFilter({ category: 'Web' }, id).then(res => {
      state.treeData = res.data.list;
    });
  }

  function onPcChange(data) {
    state.dataForm.pcModuleParentId = data.map(o => o.value);
  }
  function selectToggle(key) {
    state.dataForm[key] = state.dataForm[key] === 1 ? 0 : 1;
  }
  async function handleSubmit() {
    try {
      if (!state.dataForm.pc) return createMessage.error('请选择发布类型');
      const values = await formElRef.value?.validate();
      if (!values) return;
      const platform = { pc: state.dataForm.pc };
      const query = { ...state.dataForm, platformRelease: JSON.stringify(platform) };
      const handleRelease = () => {
        changeOkLoading(true);
        createMenu(state.record.id, query)
          .then(res => {
            changeOkLoading(false);
            createMessage.success(res.msg);
            emit('reload');
            closeModal();
          })
          .catch(() => {
            changeOkLoading(false);
          });
      };
      if (!state.record.isRelease) return handleRelease();
      createConfirm({
        iconType: 'warning',
        title: t('common.tipTitle'),
        content: '发布确定后会覆盖当前线上版本且进行菜单同步，是否继续?',
        onOk: handleRelease,
      });
    } catch (_) {}
  }
</script>

