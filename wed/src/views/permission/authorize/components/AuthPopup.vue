<template>
  <BasicPopup v-bind="$attrs" @register="registerPopup" class="full-popup" :title="title" destroyOnClose :closeFunc="onClose">
    <div class="auth-container">
      <div class="left-container">
        <BasicLeftTree title="权限类型" ref="leftTreeRef" :treeData="treeData" :showSearch="false" @select="handleTreeSelect" :showToolbar="false" />
      </div>
      <div class="main-container" v-if="type">
        <MenuAuth v-bind="getBindValues" v-if="type === 'Menu'" />
        <CommonAuth v-bind="getBindValues" v-else />
      </div>
    </div>
  </BasicPopup>
</template>
<script lang="ts" setup>
  import { BasicPopup, usePopupInner } from '@/components/Popup';
  import { reactive, ref, toRefs, unref, computed } from 'vue';
  import { BasicLeftTree, TreeActionType } from '@/components/Tree';
  import MenuAuth from './MenuAuth.vue';
  import CommonAuth from './CommonAuth.vue';

  interface State {
    title: string;
    id: string;
    type: string;
    key: number;
  }

  const treeData: any = [
    { id: 'Menu', fullName: '应用' },
    { id: 'Portal', fullName: '门户' },
    { id: 'Flow', fullName: '流程' },
    { id: 'Print', fullName: '打印模板' },
  ];
  const [registerPopup] = usePopupInner(init);
  const leftTreeRef = ref<Nullable<TreeActionType>>(null);
  const state = reactive<State>({
    title: '',
    id: '',
    type: 'Menu',
    key: 0,
  });
  const { title, type } = toRefs(state);

  const getBindValues = computed(() => ({ id: state.id, type: state.type, key: state.key }));

  function init(data) {
    state.id = data.id;
    state.title = data.fullName;
    state.type = 'Menu';
    const leftTree = unref(leftTreeRef);
    leftTree?.setSelectedKeys([state.type]);
    state.key = +new Date();
  }
  function handleTreeSelect(type) {
    state.type = type;
    state.key = +new Date();
  }
  async function onClose() {
    state.type = '';
    return true;
  }
</script>
<style lang="less">
  .auth-container {
    height: 100%;
    display: flex;
    .left-container {
      width: 220px;
      background-color: @component-background;
      flex-shrink: 0;
      height: 100%;
      display: flex;
      flex-direction: column;
      overflow: hidden;
      border-right: 1px solid @border-color-base;
    }

    .main-container {
      height: 100%;
      flex: 1;
      .auth-main-container {
        .tool-container {
          height: 50px;
          border-bottom: 1px solid @border-color-base;
          display: flex;
          align-items: center;
          justify-content: space-between;
          padding: 0 10px;
          .tool-container-title {
            font-size: 16px;
          }
        }
        height: 100%;
        display: flex;
        flex-direction: column;
        .ant-steps {
          flex-shrink: 0;
        }
        .main {
          flex: 1;
          padding: 20px;
          overflow: hidden;
          .no-data {
            height: 100%;
            display: flex;
            align-items: center;
            justify-content: center;
          }
        }
      }
    }
  }
</style>
