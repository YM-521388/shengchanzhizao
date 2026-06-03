<template>
  <div class="form-extra-panel" :class="{ 'form-extra-panel-unfold': !isAdvanced }">
    <a-tooltip placement="left" :title="isAdvanced ? t('component.form.fold') : t('component.form.unfold')" :key="key">
      <div class="trigger-btn" @click="toggleAdvanced">
        <DoubleRightOutlined v-if="isAdvanced" />
        <DoubleLeftOutlined v-else />
      </div>
    </a-tooltip>
    <a-tabs
      v-model:activeKey="activeKey"
      :tabBarGutter="10"
      size="small"
      class="average-tabs flow-average-tabs"
      :class="{ 'average-tabs-single': getShowList.filter(Boolean).length === 1 }">
      <a-tab-pane :key="1" tab="流转" v-if="showRecord" />
      <a-tab-pane :key="2" tab="评论" v-if="showComment" />
      <a-tab-pane :key="3" tab="修改记录" v-if="showDataLog" />
      <a-tab-pane :key="4" tab="辅助信息" v-if="auxiliaryInfo.length" />
    </a-tabs>
    <div class="form-extra-panel-main">
      <RecordTimeList :taskId="taskId" :list="recordTimeList" v-if="activeKey == 1" />
      <CommentList :taskId="taskId" v-if="activeKey == 2" />
      <DataLogList :modelId="modelId" :formDataId="formDataId" v-if="activeKey == 3" />
      <AuxiliaryList :list="auxiliaryInfo" :formData="formData" v-if="activeKey == 4" />
    </div>
  </div>
</template>
<script lang="ts" setup>
  import { reactive, toRefs, onMounted, computed } from 'vue';
  import { DoubleLeftOutlined, DoubleRightOutlined } from '@ant-design/icons-vue';
  import { useI18n } from '@/hooks/web/useI18n';
  import RecordTimeList from './RecordTimeList.vue';
  import CommentList from '@/components/FormExtraPanel/CommentList.vue';
  import DataLogList from '@/components/FormExtraPanel/DataLogList.vue';
  import AuxiliaryList from './AuxiliaryList.vue';

  interface State {
    isAdvanced: boolean;
    activeKey: number;
    key: number;
  }

  const props = defineProps({
    showComment: { type: Boolean, default: false },
    showRecord: { type: Boolean, default: false },
    showDataLog: { type: Boolean, default: false },
    taskId: { type: [String, Number], default: '' },
    modelId: { type: String, default: '' },
    formDataId: { type: [String, Number], default: '' },
    recordTimeList: { type: Array, default: [] },
    auxiliaryInfo: { type: Array, default: [] },
    formData: { type: Object },
  });
  const state = reactive<State>({
    isAdvanced: true,
    activeKey: 1,
    key: +new Date(),
  });
  const { isAdvanced, activeKey, key } = toRefs(state);
  const { t } = useI18n();

  const getShowList = computed(() => [props.showComment, props.showRecord, props.showDataLog]);

  function toggleAdvanced() {
    state.isAdvanced = !state.isAdvanced;
    state.key = +new Date();
  }

  onMounted(() => {
    state.activeKey = props.showRecord ? 1 : 2;
  });
</script>
