<template>
  <BasicModal v-bind="$attrs" @register="registerModal" title="任务流程" destroyOnClose class="jnpf-task-log-modal" :footer="null">
    <div class="record-container">
      <div class="record-item" v-for="item in state.list" v-if="state.list?.length">
        <div class="record-item-top">
          <div class="handle-time">触发时间：{{ formatToDateTime(item.startTime) }}</div>
          <div class="async">
            <a-tag :color="item.isAsync == 1 ? 'error' : 'blue'">{{ item.isAsync == 1 ? '异步' : '同步' }}</a-tag>
          </div>
        </div>
        <div class="record-item-bottom">
          <div class="item" v-for="child in item.recordList">
            <div class="node-name">{{ child.nodeName }}</div>
            <div class="task-flow-status">
              <i class="icon icon-ym icon-ym-success" v-if="child.status === 0" />
              <i class="icon icon-ym icon-ym-fail" v-else />{{ child.status === 0 ? '已完成' : '异常' }}
            </div>
            <div class="w-56px">
              <a-button type="link" @click="handleShowErrorModal(child)" v-if="child.status !== 0">查看异常</a-button>
            </div>
          </div>
        </div>
      </div>
      <jnpf-empty v-if="!state.list?.length && !state.loading" />
    </div>
  </BasicModal>
  <LogErrorModal @register="registerLogErrorModal" />
</template>
<script lang="ts" setup>
  import { reactive } from 'vue';
  import { BasicModal, useModalInner } from '@/components/Modal';
  import { getTaskLogList } from '@/api/workFlow/trigger';
  import { formatToDateTime } from '@/utils/dateUtil';
  import { useModal } from '@/components/Modal';
  import LogErrorModal from './LogErrorModal.vue';

  interface State {
    list: any[];
    loading: boolean;
  }

  const state = reactive<State>({
    list: [],
    loading: false,
  });
  const [registerModal, { changeLoading }] = useModalInner(init);
  const [registerLogErrorModal, { openModal: openLogErrorModal }] = useModal();

  function init(data) {
    changeLoading(true);
    state.loading = true;
    state.list = [];
    getTaskLogList({ taskId: data.taskId, nodeCode: data.nodeId }).then(res => {
      state.list = res.data || [];
      changeLoading(false);
      state.loading = false;
    });
  }
  function handleShowErrorModal(item) {
    openLogErrorModal(true, { errorTip: item.errorTip, errorData: item.errorData });
  }
</script>
<style lang="less">
  .jnpf-task-log-modal {
    .ant-modal-body {
      & > .scrollbar {
        padding: 0 20px;
      }
    }
    .record-container {
      height: 600px;
      padding-top: 20px;
      .record-item {
        display: flex;
        flex-direction: column;
        border-bottom: 1px solid @border-color-base1;
        margin-bottom: 12px;
        padding-bottom: 12px;
        .record-item-top {
          display: flex;
          margin-bottom: 5px;
          .handle-time {
            color: @text-color-secondary;
            font-size: 14px;
            flex: 1;
          }
        }
        .record-item-bottom {
          margin: 4px 0;
          padding: 0 15px;
          border-radius: 4px;
          background-color: @app-content-background;
          .item {
            height: 38px;
            display: flex;
            align-items: center;
            border-bottom: 1px solid @border-color-base;
            &:last-child {
              border-bottom: unset;
            }
          }
          .node-name {
            flex: 1;
            overflow: hidden;
            white-space: nowrap;
            text-overflow: ellipsis;
            padding-right: 10px;
          }
          .ant-btn-link {
            padding: unset;
          }
        }
      }
    }
  }
</style>
