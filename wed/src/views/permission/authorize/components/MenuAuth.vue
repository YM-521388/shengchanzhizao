<template>
  <div class="auth-main-container">
    <div class="tool-container">
      <div class="tool-container-title">应用权限</div>
      <a-space :size="10">
        <a-button type="text" :disabled="loading" @click="handleExpandAll(true)">展开全部</a-button>
        <a-button type="text" :disabled="loading" @click="handleExpandAll(false)">折叠全部</a-button>
        <a-button type="text" :disabled="loading" @click="handleCheckAll(true)">全部勾选</a-button>
        <a-button type="text" :disabled="loading" @click="handleCheckAll(false)">取消全选</a-button>
        <a-divider type="vertical" class="!h-7"></a-divider>
        <a-button @click="handlePrev" :disabled="activeStep <= 0 || loading">{{ t('common.prev') }}</a-button>
        <a-button @click="handleNext" :disabled="activeStep >= maxStep || loading">{{ t('common.next') }} </a-button>
        <a-button type="primary" @click="handleSubmit()" :disabled="activeStep < maxStep || loading" :loading="btnLoading">{{ t('common.saveText') }}</a-button>
      </a-space>
    </div>
    <a-steps v-model:current="activeStep" type="navigation" size="small">
      <a-step title="应用权限" disabled />
      <a-step title="菜单权限" disabled />
      <a-step title="按钮权限" disabled />
      <a-step title="列表权限" disabled />
      <a-step title="表单权限" disabled />
      <a-step title="数据权限" disabled />
    </a-steps>
    <div class="main">
      <BasicTree
        :treeData="treeData"
        ref="treeRef"
        checkable
        blockNode
        clickRowToExpand
        defaultExpandAll
        :fieldNames="activeStep == 6 ? { key: 'onlyId' } : { key: 'id' }"
        :loading="loading"
        @check="onCheck"
        :key="key"
        class="overflow-auto" />
    </div>
  </div>
</template>
<script lang="ts" setup>
  import { getAuthorizeValues, updateAuthorizeList, updateBatchAuthorize } from '@/api/permission/authorize';
  import { getRoleSelectorByPermission } from '@/api/permission/role';
  import { BasicTree, TreeActionType } from '@/components/Tree';
  import { reactive, toRefs, ref, unref, nextTick, onMounted } from 'vue';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useI18n } from '@/hooks/web/useI18n';

  interface State {
    title: string;
    activeStep: number;
    maxStep: number;
    treeData: any[];
    loading: boolean;
    objectId: string;
    dataForm: any;
    params: any;
    systemIdsAuthorizeTree: any[];
    moduleAuthorizeTree: any[];
    buttonAuthorizeTree: any[];
    columnAuthorizeTree: any[];
    formAuthorizeTree: any[];
    resourceAuthorizeTree: any[];
    systemIdsAllData: any[];
    moduleAllData: any[];
    buttonAllData: any[];
    columnAllData: any[];
    formAllData: any[];
    resourceAllData: any[];
    moduleIdsTemp: any[];
    systemIds: any[];
    key: number;
    btnLoading: boolean;
  }

  const props = defineProps({
    id: { type: String, default: '' },
    type: { type: String, default: '' },
  });

  const { createMessage } = useMessage();
  const { t } = useI18n();
  const treeRef = ref<Nullable<TreeActionType>>(null);
  const state = reactive<State>({
    title: '',
    activeStep: 0,
    maxStep: 5,
    treeData: [],
    loading: false,
    objectId: '',
    dataForm: {
      objectType: '',
      module: [],
      systemIds: [],
      button: [],
      column: [],
      form: [],
      resource: [],
      roleIds: [],
    },
    params: {
      type: 'system',
      moduleIds: '',
    },
    systemIdsAuthorizeTree: [],
    moduleAuthorizeTree: [],
    buttonAuthorizeTree: [],
    columnAuthorizeTree: [],
    formAuthorizeTree: [],
    resourceAuthorizeTree: [],
    systemIdsAllData: [],
    moduleAllData: [],
    buttonAllData: [],
    columnAllData: [],
    formAllData: [],
    resourceAllData: [],
    moduleIdsTemp: [],
    systemIds: [],
    key: 0,
    btnLoading: false,
  });
  const { activeStep, maxStep, treeData, loading, key, btnLoading } = toRefs(state);

  function init() {
    resetState();
    state.objectId = props.id;
    state.dataForm.objectType = 'Batch';
    state.maxStep = 5;
    state.activeStep = 0;
    if (!props.id) return;
    getAuthorizeList();
  }
  function resetState() {
    state.activeStep = 0;
    state.btnLoading = false;
    state.dataForm = {
      objectType: '',
      module: [],
      systemIds: [],
      button: [],
      column: [],
      form: [],
      resource: [],
      roleIds: [],
    };
    state.params = {
      type: 'system',
      moduleIds: '',
    };
    state.moduleIdsTemp = [];
    state.systemIds = [];
  }
  function getAuthorizeList() {
    state.loading = true;
    state.treeData = [];
    getTree().setCheckedKeys([]);
    getAuthorizeValues(state.objectId, state.params).then(res => {
      state.key = +new Date();
      const key = getKey();
      state[key + 'AuthorizeTree'] = res.data.list || [];
      state[key + 'AllData'] = res.data.all || [];
      state.treeData = state[key + 'AuthorizeTree'];
      const parentKeys = getTree().getParentKeys(state.treeData, true);
      let dataIds = [...new Set([...state.dataForm[key], ...res.data.ids, ...state.moduleIdsTemp])];
      dataIds = dataIds.filter(o => state[key + 'AllData'].includes(o));
      setDataFormValue(dataIds);
      const checkedKeys = dataIds.filter(o => !parentKeys.includes(o));
      nextTick(() => {
        getTree().setCheckedKeys(checkedKeys);
        state.loading = false;
      });
    });
  }
  function getRoleList() {
    state.loading = true;
    state.treeData = [];
    getTree().setCheckedKeys([]);
    getRoleSelectorByPermission().then(res => {
      state.treeData = res.data.list || [];
      getTree().setCheckedKeys(state.dataForm.roleIds);
      state.loading = false;
    });
  }
  function getKey() {
    if (state.activeStep === 0) return 'systemIds';
    if (state.activeStep === 1) return 'module';
    if (state.activeStep === 2) return 'button';
    if (state.activeStep === 3) return 'column';
    if (state.activeStep === 4) return 'form';
    if (state.activeStep === 5) return 'resource';
    if (state.activeStep === 6) return 'roleIds';
    return 'systemIds';
  }
  function onCheck(checkedKeys, e) {
    const halfCheckedKeys = state.activeStep === 6 ? [] : e.halfCheckedKeys;
    let dataIds = [...checkedKeys, ...halfCheckedKeys];
    if (state.activeStep === 6 && dataIds.length) {
      const checkedNodes = getTree().getCheckedNodes();
      dataIds = checkedNodes.map(o => o.id);
    }
    setDataFormValue(dataIds);
  }
  function setDataFormValue(dataIds) {
    const key = getKey();
    state.dataForm[key] = dataIds;
    if (state.activeStep === 0 || state.activeStep === 1) state.moduleIdsTemp = state.dataForm[key];
    if (state.activeStep === 0) state.systemIds = state.moduleIdsTemp;
  }
  function handlePrev() {
    state.activeStep -= 1;
    handleInitData();
  }
  function handleNext() {
    state.activeStep += 1;
    handleInitData();
  }
  function handleInitData() {
    if (state.activeStep === 6) return getRoleList();
    const key = getKey();
    state.params.type = key === 'systemIds' ? 'system' : key;
    state.params.moduleIds = key === 'module' ? state.systemIds.toString() : state.moduleIdsTemp.toString();
    getAuthorizeList();
  }
  function handleCheckAll(boo) {
    getTree().checkAll(boo);
    nextTick(() => {
      let checkedKeys: any[] = [];
      if (state.activeStep === 6) {
        const checkedNodes = getTree().getCheckedNodes();
        checkedKeys = checkedNodes.map(o => o.id);
      } else {
        checkedKeys = getTree().getCheckedKeys() as any[];
      }
      setDataFormValue(checkedKeys);
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
    if (state.maxStep === 6) {
      updateBatchAuthorize(state.dataForm)
        .then(res => {
          createMessage.success(res.msg);
          state.btnLoading = false;
          init();
        })
        .catch(() => {
          state.btnLoading = false;
        });
    } else {
      updateAuthorizeList(state.objectId, state.dataForm)
        .then(res => {
          createMessage.success(res.msg);
          state.btnLoading = false;
        })
        .catch(() => {
          state.btnLoading = false;
        });
    }
  }

  onMounted(() => {
    init();
  });
</script>
