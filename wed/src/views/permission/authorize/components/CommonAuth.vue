<template>
  <div class="auth-main-container">
    <div class="tool-container">
      <div class="tool-container-title">{{ title }}</div>
      <a-space :size="10">
        <a-button type="text" :disabled="loading" @click="handleExpandAll(true)">展开全部</a-button>
        <a-button type="text" :disabled="loading" @click="handleExpandAll(false)">折叠全部</a-button>
        <a-button type="text" :disabled="loading" @click="handleCheckAll(true)">全部勾选</a-button>
        <a-button type="text" :disabled="loading" @click="handleCheckAll(false)">取消全选</a-button>
        <a-divider type="vertical" class="!h-7"></a-divider>
        <a-button type="primary" @click="handleSubmit()" :disabled="loading" :loading="btnLoading">{{ t('common.saveText') }}</a-button>
      </a-space>
    </div>
    <a-alert
      message="请先授权应用权限，且该应用需添加门户（【系统管理】-【应用菜单】）"
      type="warning"
      showIcon
      class="!mx-20px !mt-20px"
      v-if="type === 'Portal'" />
    <div class="main" v-loading="loading">
      <BasicTree
        v-show="treeData.length"
        :treeData="treeData"
        ref="treeRef"
        checkable
        blockNode
        clickRowToExpand
        defaultExpandAll
        @check="onCheck"
        class="overflow-auto" />
      <div class="no-data" v-show="!treeData.length && !loading">
        <jnpf-empty />
      </div>
    </div>
  </div>
</template>
<script lang="ts" setup>
  import { getCommonAuthorizeValues, updateCommonAuthorizeList } from '@/api/permission/authorize';
  import { BasicTree, TreeActionType } from '@/components/Tree';
  import { reactive, toRefs, ref, unref, nextTick, onMounted } from 'vue';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useI18n } from '@/hooks/web/useI18n';

  interface State {
    title: string;
    loading: boolean;
    objectId: string;
    treeData: any[];
    selectedIds: any[];
    btnLoading: boolean;
  }

  const props = defineProps({
    id: { type: String, default: '' },
    type: { type: String, default: 'Portal' },
  });

  const { t } = useI18n();
  const { createMessage } = useMessage();
  const treeRef = ref<Nullable<TreeActionType>>(null);
  const state = reactive<State>({
    title: '',
    loading: false,
    objectId: '',
    selectedIds: [],
    treeData: [],
    btnLoading: false,
  });
  const { treeData, loading, btnLoading, title } = toRefs(state);

  function init() {
    state.objectId = props.id;
    state.btnLoading = false;
    getTitle();
    getAuthorizeList();
  }
  function getTitle() {
    let title = '';
    if (props.type === 'Portal') title = '门户权限';
    if (props.type === 'Flow') title = '流程权限';
    if (props.type === 'Print') title = '打印模板权限';
    state.title = title;
  }
  function getAuthorizeList() {
    state.loading = true;
    state.treeData = [];
    state.selectedIds = [];
    getTree().setCheckedKeys([]);
    if (!props.type || !state.objectId) return;
    getCommonAuthorizeValues(props.type, state.objectId)
      .then(res => {
        state.treeData = res.data.list || [];
        const parentKeys = getTree().getParentKeys(state.treeData, true);
        const dataIds = [...new Set([...state.selectedIds, ...res.data.ids])];
        state.selectedIds = dataIds;
        const checkedKeys = dataIds.filter(o => !parentKeys.includes(o));
        nextTick(() => {
          getTree().setCheckedKeys(checkedKeys);
          state.loading = false;
        });
      })
      .catch(() => {
        state.loading = false;
      });
  }
  function onCheck(checkedKeys, e) {
    let dataIds = [...checkedKeys, ...e.halfCheckedKeys];
    state.selectedIds = dataIds;
  }
  function handleCheckAll(boo) {
    getTree().checkAll(boo);
    nextTick(() => {
      let checkedKeys: any[] = getTree().getCheckedKeys() as any[];
      state.selectedIds = checkedKeys;
    });
  }
  function handleExpandAll(boo) {
    getTree().expandAll(boo);
  }
  function getTree() {
    const tree = unref(treeRef);
    if (!tree) throw new Error('tree is null!');
    return tree;
  }
  function handleSubmit() {
    state.btnLoading = true;
    updateCommonAuthorizeList(props.type, state.objectId, { ids: state.selectedIds })
      .then(res => {
        createMessage.success(res.msg);
        state.btnLoading = false;
      })
      .catch(() => {
        state.btnLoading = false;
      });
  }

  onMounted(() => {
    init();
  });
</script>
