<template>
  <BasicPopup v-bind="$attrs" @register="registerPopup" destroyOnClose class="full-popup basic-flow-parser">
    <template #title>
      <a-space :size="10">
        <div class="header-title">{{ getTitle }}</div>
        <template v-if="!loading || getTitle">
          <a-dropdown :disabled="config.opType != '-1'">
            <template #overlay>
              <a-menu @click="handleUrgentClick">
                <a-menu-item :key="item.id" v-for="item in flowUrgentList">
                  <div class="flow-urgent-value" :class="'urgent' + item.id">{{ item.fullName }}</div>
                </a-menu-item>
              </a-menu>
            </template>
            <div class="flow-urgent-value" :class="'urgent' + state.flowUrgent">{{ getUrgentName }}</div>
          </a-dropdown>
        </template>
      </a-space>
    </template>
    <div class="jnpf-common-form-wrapper">
      <div class="jnpf-common-form-wrapper__main">
        <a-tabs
          v-model:activeKey="state.activeKey"
          class="flow-parser-tabs"
          :class="{ 'no-head-margin': state.activeKey == '3' }"
          size="small"
          v-loading="loading">
          <a-tab-pane key="2" tab="流程信息" class="!overflow-hidden">
            <a-tabs v-model:activeKey="state.subFlowTab" class="flow-parser-tabs" type="card" @change="setCurrFlow">
              <a-tab-pane v-for="(item, index) in state.subFlowList" :key="index" :tab="item.taskInfo.fullName" class="!overflow-hidden">
                <FlowProcessMain :flowInfo="item.flowInfo" :nodeList="state.nodeList" :isPreview="true" />
              </a-tab-pane>
            </a-tabs>
          </a-tab-pane>
          <a-tab-pane key="3" tab="流转记录" class="!p-0" v-if="config.opType != '-1'">
            <RecordList :list="state.recordList" :opType="config.opType" @viewDetail="viewNodeDetail" v-if="state.activeKey == '3'" />
          </a-tab-pane>
        </a-tabs>
      </div>
      <FormExtraPanel :taskId="state.config.id" v-if="config.opType != '-1' && state.hasComment" :key="formExtraPanelKey" />
    </div>
  </BasicPopup>
  <NodeFormParser @register="registerNodeFormParser" />
</template>
<script lang="ts" setup>
  import { computed, reactive, toRefs } from 'vue';
  import { BasicPopup, usePopupInner } from '@/components/Popup';
  import { usePopup } from '@/components/Popup';
  import { getSubFlowInfo } from '@/api/workFlow/task';
  import FlowProcessMain from '@/components/FlowProcess/src/Main.vue';
  import RecordList from './RecordList.vue';
  import NodeFormParser from './NodeFormParser.vue';
  import FormExtraPanel from '@/components/FormExtraPanel/index.vue';
  import { cloneDeep } from 'lodash-es';

  interface State {
    config: any;
    properties: any;
    loading: boolean;
    flowUrgent: any;
    activeKey: any;
    taskInfo: any;
    flowInfo: any;
    nodeList: any[];
    recordList: any[];
    thisStep: string;
    currentView: any;
    formData: any;
    eventType: string;
    candidateType: number;
    hasComment: boolean;
    subFlowList: any[];
    currFlow: any;
    subFlowTab: number;
    formExtraPanelKey: number;
  }

  defineEmits(['register']);
  const [registerPopup] = usePopupInner(init);
  const [registerNodeFormParser, { openPopup: openNodeFormParserPopup }] = usePopup();

  const flowUrgentList = [
    { id: 1, fullName: '普通' },
    { id: 2, fullName: '重要' },
    { id: 3, fullName: '紧急' },
  ];
  const state = reactive<State>({
    config: {},
    properties: {},
    loading: false,
    flowUrgent: 1,
    activeKey: ' 1',
    taskInfo: {},
    flowInfo: {},
    nodeList: [],
    recordList: [],
    thisStep: '',
    currentView: null,
    formData: {},
    eventType: '',
    candidateType: 1,
    hasComment: false,
    subFlowList: [],
    currFlow: {},
    subFlowTab: 0,
    formExtraPanelKey: 0,
  });
  const { config, loading, formExtraPanelKey } = toRefs(state);

  const getTitle = computed(() => {
    const fullName = state.config.fullName;
    if ([2, 3, 4].includes(state.config.opType)) return fullName;
    return state.thisStep ? fullName + '/' + state.thisStep : fullName;
  });
  const getUrgentName = computed(() => {
    const list = flowUrgentList.filter(o => o.id === state.flowUrgent);
    if (!list.length) return '普通';
    return list[0].fullName || '普通';
  });

  function init(data) {
    state.hasComment = false;
    state.loading = true;
    state.config = data;
    state.activeKey = '2';
    state.subFlowList = [];
    /**
     * opType
     * -1 - 我发起的新建/编辑
     * 0 - 我发起的详情
     * 1 - 待办事宜
     * 2 - 已办事宜
     * 3 - 抄送事宜
     * 4 - 流程监控
     */
    getBeforeInfo(state.config);
  }
  function getBeforeInfo(config) {
    getSubFlowInfo(config.nodeCode, config.taskId)
      .then(res => {
        state.subFlowList = res.data || [];
        state.subFlowTab = 0;
        setCurrFlow(0);
        state.loading = false;
      })
      .catch(() => {
        state.loading = false;
      });
  }
  function setCurrFlow(key) {
    state.currFlow = cloneDeep(state.subFlowList[key]);
    state.flowInfo = state.currFlow.flowInfo;
    state.taskInfo = state.currFlow.taskInfo || {};
    state.nodeList = state.currFlow.nodeList || [];
    const fullName = state.config.opType == '-1' ? state.flowInfo.fullName : state.taskInfo.fullName;
    state.config.fullName = fullName;
    state.config.flowId = state.taskInfo.flowId;
    state.config.status = state.taskInfo.status;
    state.config.id = state.taskInfo.id;
    state.thisStep = state.taskInfo.currentNodeName || '';
    state.flowUrgent = state.taskInfo.flowUrgent || 1;
    state.hasComment = state.flowInfo.flowNodes.global.hasComment;
    state.properties = state.currFlow.nodeProperties || {};
    state.recordList = (state.currFlow.recordList || []).reverse();
    state.formExtraPanelKey = +new Date();
  }
  function handleUrgentClick({ key }) {
    state.flowUrgent = key;
  }
  // 流程监控查看各节点表单详情
  function viewNodeDetail(data) {
    openNodeFormParserPopup(true, { ...data, flowId: state.config.flowId });
  }
</script>
<style lang="less">
  @import './style/index.less';
</style>
