<template>
  <Timeline class="record-time-list-container">
    <TimelineItem v-for="item in props.list">
      <template #dot>
        <span class="tag" :style="{ background: getTimeLineTagColor(item.nodeStatus) }"></span>
      </template>
      <span>{{ formatToDateTime(item.startTime, 'YYYY-MM-DD HH:mm') }}</span>
      <div class="time-item-container">
        <div class="time-node-name">
          <i :class="getNodeIcon(item.nodeType)" />
          <span class="node-name">{{ item.nodeName }}</span>
          <a-tag :color="getNodeStatusColor(item.nodeStatus)" :bordered="false" class="node-status">{{ getNodeStatusContent(item.nodeStatus) }}</a-tag>
        </div>
        <div class="time-node-approver" v-if="item.approver?.length">
          <div class="approver-container">
            <div class="approver-item" v-for="child in item.approver.slice(0, 4)">
              <a-avatar :size="24" :src="apiUrl + child.headIcon" />
              <a-tag class="node-handle-type" :color="getHexColor(getFlowStateColor(child.handleType))">{{ getFlowStateContent(child.handleType) }}</a-tag>
              <span class="user-name">{{ child.userName }}</span>
            </div>
          </div>
          <div class="approver-count" v-if="item.approverCount">{{ item.approverCount }}</div>
        </div>
        <div class="counter-sign" @click="handleShowRecordModal(item)" v-if="item.nodeType == 'approver' || item.nodeType == 'processing'">
          <span>{{ getCounterSignContent(item.counterSign) }}</span>
          <i class="icon-ym icon-ym-right" />
        </div>
        <div class="counter-sign" @click="handleShowTaskLogModal(item)" v-if="item.showTaskFlow">
          <span>任务流程</span>
          <i class="icon-ym icon-ym-right" />
        </div>
      </div>
    </TimelineItem>
  </Timeline>
  <RecordModal @register="registerRecord" />
  <TaskLogModal @register="registerTaskLog" />
</template>
<script lang="ts" setup>
  import { ref } from 'vue';
  import { Timeline, TimelineItem } from 'ant-design-vue';
  import { formatToDateTime } from '@/utils/dateUtil';
  import { flowNodeList } from '@/components/FlowProcess/src/helper/componentMap';
  import { useGlobSetting } from '@/hooks/setting';
  import { useDefineSetting } from '@/hooks/setting/useDefineSetting';
  import { useModal } from '@/components/Modal';
  import RecordModal from './modal/RecordModal.vue';
  import TaskLogModal from './modal/TaskLogModal.vue';

  const props: any = defineProps({
    list: { type: Array, default: [] },
    endTime: { type: Number, default: 0 },
    opType: { default: '' },
    taskId: { type: [String, Number], default: '' },
  });
  const globSetting = useGlobSetting();
  const apiUrl = ref(globSetting.apiUrl);
  const { getFlowStateContent, getFlowStateColor, getHexColor } = useDefineSetting();
  const [registerRecord, { openModal: openRecordModal }] = useModal();
  const [registerTaskLog, { openModal: openTaskLogModal }] = useModal();

  function getNodeStatusColor(status) {
    return status == 1 || status == 2 ? 'success' : status != 3 ? 'blue' : 'error';
  }
  function getTimeLineTagColor(status) {
    return status == 1 || status == 2 ? '#08AF28' : status != 3 ? '#0177FF' : '#ed6f6f';
  }
  function getNodeStatusContent(status) {
    const list = ['', '已提交', '已通过', '已拒绝', '审批中', '已退回', '已撤回', '等待中', '办理中'];
    return list[status] || '';
  }
  function getCounterSignContent(counterSign) {
    return counterSign == 0 ? '或签' : counterSign == 1 ? '会签' : '依次审批';
  }
  function getNodeIcon(nodeType) {
    const list = flowNodeList.filter(o => o.option.wnType == nodeType);
    return list[0]?.icon || 'icon-ym icon-ym-flow-node-start';
  }
  function handleShowRecordModal(item) {
    const title = `${item.nodeName}（${getCounterSignContent(item.counterSign)}）`;
    openRecordModal(true, { taskId: props.taskId, nodeId: item.nodeId, title });
  }
  function handleShowTaskLogModal(item) {
    openTaskLogModal(true, { taskId: props.taskId, nodeId: item.nodeId });
  }
</script>
<style lang="less">
  .record-time-list-container {
    padding: 24px 12px;
    overflow: auto;
    height: 100%;
    .tag {
      display: block;
      width: 10px;
      height: 10px;
      border-radius: 50%;
    }
    .time-item-container {
      margin-top: 8px;
      border-radius: 4px;
      background-color: @app-content-background;
      .time-node-name {
        height: 40px;
        display: flex;
        align-items: center;
        margin-left: 10px;
        i {
          margin-right: 4px;
          font-size: 12px;
        }
        .node-name {
          flex: 1;
          overflow: hidden;
          white-space: nowrap;
          text-overflow: ellipsis;
        }
        .node-status {
          border-radius: 10px;
          padding-inline: 10px;
        }
      }
      .time-node-approver {
        display: flex;
        .approver-container {
          display: flex;
          margin: 0 10px 10px;
          justify-content: flex-start;
          flex: 1;
          min-width: 0;
          .approver-item {
            width: 25%;
            display: flex;
            align-items: center;
            flex-direction: column;
            position: relative;

            .node-handle-type {
              z-index: 999;
              margin: -8px auto 0;
            }
            .user-name {
              width: 100%;
              text-align: center;
              overflow: hidden;
              white-space: nowrap;
              text-overflow: ellipsis;
            }
          }
        }
        .approver-count {
          width: 24px;
          height: 24px;
          margin-right: 10px;
          display: flex;
          align-items: center;
          justify-content: center;
          border-radius: 12px;
          background-color: @component-background;
        }
      }
      .counter-sign {
        height: 40px;
        margin: 0 16px;
        display: flex;
        justify-content: space-between;
        align-items: center;
        border-top: 1px solid @border-color-base;
        cursor: pointer;
        span {
          height: 20px;
          line-height: 20px;
          text-align: center;
          background: #e2e2e2;
          border-radius: 4px;
          padding: 0 12px;
        }
      }
    }
  }
</style>
