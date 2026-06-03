<template>
  <BasicModal v-bind="$attrs" @register="registerModal" title="工作交接" :width="800" class="work-handover-modal" destroyOnClose @ok="handleSubmit">
    <BasicForm @register="registerForm">
      <template #content>
        <div class="tabs-box">
          <a-tabs v-model:activeKey="activeKey">
            <a-tab-pane v-for="item in tabList" :key="item.id" :tab="item.fullName" />
          </a-tabs>
          <ScrollContainer class="px-20px">
            <div class="pb-10px">
              <JnpfAlert :message="getTips" type="warning" />
            </div>
            <BasicTree ref="treeRef" checkable defaultExpandAll :loading="loading" :key="key" :treeData="getTreeData" @check="handleTreeCheck" />
          </ScrollContainer>
        </div>
      </template>
    </BasicForm>
  </BasicModal>
</template>
<script lang="ts" setup>
  import { reactive, toRefs, ref, computed, watch, unref, nextTick } from 'vue';
  import { BasicModal, useModalInner } from '@/components/Modal';
  import { useMessage } from '@/hooks/web/useMessage';
  import { BasicForm, useForm } from '@/components/Form';
  import { BasicTree, TreeActionType } from '@/components/Tree';
  import { ScrollContainer } from '@/components/Container';
  import { workHandover, getWorkByUser } from '@/api/permission/user';

  interface State {
    activeKey: string;
    userInfo: any;
    loading: boolean;
    key: number;
    flowTaskList: any[];
    flowList: any[];
    permissionList: any[];
    flowTaskIds: any[];
    flowIds: any[];
    permissionIds: any[];
  }

  const emit = defineEmits(['register', 'reload']);
  const { createMessage } = useMessage();
  const state = reactive<State>({
    activeKey: '',
    userInfo: {},
    loading: false,
    key: +new Date(),
    flowTaskList: [],
    flowList: [],
    permissionList: [],
    flowTaskIds: [],
    flowIds: [],
    permissionIds: [],
  });
  const { activeKey, key, loading } = toRefs(state);
  const treeRef = ref<Nullable<TreeActionType>>(null);
  const [registerModal, { closeModal, changeOkLoading }] = useModalInner(init);
  const [registerForm, { setFieldsValue, resetFields, validate }] = useForm({
    labelWidth: 100,
    schemas: [
      {
        field: 'transferUser',
        label: '工作移交人',
        component: 'Input',
        componentProps: { disabled: true },
      },
      {
        field: 'handoverUser',
        label: '工作交接人',
        component: 'UserSelect',
        componentProps: { placeholder: '请选择', onChange: onHandoverUserChange },
        rules: [{ required: true, trigger: 'change', message: '必填' }],
      },
      {
        field: 'content',
        label: '交接内容',
        component: 'Select',
        slot: 'content',
      },
    ],
  });
  const tabList = [
    { fullName: '流程列表', id: 'flow' },
    { fullName: '流程实例', id: 'flowTask' },
    { fullName: '权限组', id: 'permission' },
  ];

  const getTips = computed(() => {
    if (state.activeKey === 'flowTask') return '转交内容：包含移交人未处理的待签、待办、在办列表数据';
    if (state.activeKey === 'flow') return '转交内容：审批节点、抄送人是移交人的流程';
    return '转交内容：移交人的权限组';
  });
  const getTreeData = computed(() => state[state.activeKey + 'List'] || []);

  watch(
    () => state.activeKey,
    () => {
      setTreeCheckedKeys();
    },
  );

  function init(data) {
    resetFields();
    state.flowTaskList = [];
    state.flowList = [];
    state.permissionList = [];
    state.flowTaskIds = [];
    state.flowIds = [];
    state.permissionIds = [];
    state.userInfo = data.userInfo || '';
    state.activeKey = 'flow';
    setFieldsValue({ transferUser: state.userInfo.realName + '/' + state.userInfo.account });
    state.loading = true;
    getWorkByUser({ fromId: state.userInfo.id })
      .then(res => {
        state.loading = false;
        state.key = +new Date();
        state.flowTaskList = res.data.flowTask || [];
        state.flowList = res.data.flow || [];
        state.permissionList = res.data.permission || [];
      })
      .catch(() => {
        state.loading = false;
      });
  }
  function getTree() {
    const tree = unref(treeRef);
    if (!tree) throw new Error('tree is null!');
    return tree;
  }
  function handleTreeCheck() {
    const childrenIds: any[] = (getTree().getCheckedKeys() as any[]) || [];
    const parentIds: any[] = (getTree().getHalfCheckedKeys() as any[]) || [];
    const ids = [...parentIds, ...childrenIds];
    state[state.activeKey + 'Ids'] = ids;
  }
  function setTreeCheckedKeys() {
    state.key = +new Date();
    nextTick(() => {
      const list = getTree().getParentKeys();
      const keys = state[state.activeKey + 'Ids'].filter(id => !list.includes(id));
      getTree().setCheckedKeys(keys);
    });
  }
  function onHandoverUserChange(val, item) {
    if (val == state.userInfo.id || item.account === 'admin') {
      const mes = val == state.userInfo.id ? '工作交接无法转交给本人' : '工作交接无法转交给admin';
      createMessage.error(mes);
      setFieldsValue({ handoverUser: '' });
    }
  }
  async function handleSubmit() {
    const values = await validate();
    if (!values) return;
    if (!state.flowTaskIds.length && !state.flowIds.length && !state.permissionIds.length) return createMessage.warning('请先选择交接内容');
    const query = {
      flowTaskList: state.flowTaskIds,
      flowList: state.flowIds,
      permissionList: state.permissionIds,
      fromId: state.userInfo.id,
      toId: values.handoverUser,
    };
    changeOkLoading(true);
    workHandover(query)
      .then(res => {
        changeOkLoading(false);
        createMessage.success(res.msg);
        emit('reload');
        closeModal();
      })
      .catch(() => {
        changeOkLoading(false);
      });
  }
</script>
<style lang="less">
  .work-handover-modal {
    .tabs-box {
      border: 1px solid @border-color-base;
      border-radius: 2px;

      .ant-tabs-tab:first-child {
        margin-left: 20px;
      }

      .scroll-container .jnpf-tree {
        height: 350px;
        overflow: auto;
      }
    }
  }
</style>
