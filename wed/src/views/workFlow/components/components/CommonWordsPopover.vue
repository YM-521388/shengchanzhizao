<template>
  <a-popover placement="top" trigger="click" overlayClassName="commonWords-popover" v-model:open="popoverVisible" @openChange="openPopover">
    <a-button type="link" postIcon="icon-ym icon-ym-arrow-down" class="!px-0">{{ t('common.moreText') }}</a-button>
    <template #content>
      <div class="commonWords-content">
        <a-input-search :placeholder="t('common.enterKeyword')" allowClear v-model:value="keyword" />
        <a-table :data-source="getList" :columns="columns" size="small" :pagination="false" :showHeader="false" :loading="loading" :scroll="{ y: 220 }">
          <template #bodyCell="{ column, record }">
            <template v-if="column.key === 'commonWordsText'">
              <span class="cursor-pointer" @click="handleSubmit(record.commonWordsText)">{{ record.commonWordsText }}</span>
            </template>
          </template>
        </a-table>
      </div>
    </template>
  </a-popover>
</template>
<script lang="ts" setup>
  import { reactive, toRefs, computed } from 'vue';
  import { useI18n } from '@/hooks/web/useI18n';

  interface State {
    popoverVisible: boolean;
    loading: boolean;
    list: any[];
    keyword: string;
  }

  defineExpose({ closePopover });

  const props = defineProps({
    list: { type: Array, default: [] },
  });
  const { t } = useI18n();
  const emit = defineEmits(['confirm']);
  const columns: any[] = [{ title: '常用语', dataIndex: 'commonWordsText', key: 'commonWordsText', ellipsis: true }];
  const state = reactive<State>({
    popoverVisible: false,
    loading: false,
    list: [],
    keyword: '',
  });
  const { popoverVisible, loading, keyword } = toRefs(state);

  const getList = computed(() => (state.keyword ? props.list.filter((o: any) => o.commonWordsText.indexOf(state.keyword) !== -1) : props.list));

  function openPopover() {
    state.keyword = '';
  }
  function closePopover() {
    state.popoverVisible = false;
  }
  async function handleSubmit(commonWordsText) {
    emit('confirm', commonWordsText || '');
    state.popoverVisible = false;
  }
</script>
<style lang="less">
  .commonWords-popover {
    z-index: 1001 !important;
    .ant-popover-inner-content {
      padding: 10px;
      width: 300px;
      .commonWords-content {
        display: flex;
        flex-direction: column;
      }
    }
  }
</style>
