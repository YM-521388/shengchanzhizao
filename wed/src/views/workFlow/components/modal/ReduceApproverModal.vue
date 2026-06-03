<template>
  <BasicModal v-bind="$attrs" @register="registerModal" title="加签人员" :footer="null" class="transfer-modal member-modal">
    <div class="transfer__body">
      <div class="transfer-pane left-pane">
        <div class="transfer-pane__tool">
          <a-input-search :placeholder="t('common.enterKeyword')" allowClear v-model:value="listQuery.keyword" @search="handleSearch" />
        </div>
        <div class="transfer-pane__body transfer-pane__body-tab">
          <ScrollContainer v-loading="loading && listQuery.currentPage === 1" ref="infiniteBody">
            <div v-for="(item, index) in list" :key="item.id" class="selected-item selected-item-user">
              <div class="selected-item-main">
                <a-avatar :size="36" :src="apiUrl + item.headIcon" class="selected-item-headIcon" />
                <div class="selected-item-text">
                  <p class="name">{{ item.fullName }}</p>
                  <p class="organize" :title="item.organize">{{ item.organize }}</p>
                </div>
                <i class="icon-ym icon-ym-app-delete px-5px" @click.stop="handleReduceApprover(item.id, index)" />
              </div>
            </div>
            <jnpf-empty v-if="!list.length" />
          </ScrollContainer>
        </div>
      </div>
    </div>
  </BasicModal>
</template>
<script lang="ts" setup>
  import { getFreeApproverUserList, reduceApprover } from '@/api/workFlow/task';
  import { toRefs, ref, reactive, nextTick } from 'vue';
  import { BasicModal, useModalInner } from '@/components/Modal';
  import { useGlobSetting } from '@/hooks/setting';
  import { useI18n } from '@/hooks/web/useI18n';
  import { ScrollContainer, ScrollActionType } from '@/components/Container';
  import { useMessage } from '@/hooks/web/useMessage';

  interface State {
    list: any[];
    listQuery: any;
    loading: boolean;
    finish: boolean;
    total: number;
  }

  defineEmits(['register']);

  const { t } = useI18n();
  const globSetting = useGlobSetting();
  const { createMessage } = useMessage();
  const apiUrl = ref(globSetting.apiUrl);
  const [registerModal] = useModalInner(init);
  const infiniteBody = ref<Nullable<ScrollActionType>>(null);
  const state = reactive<State>({
    list: [],
    listQuery: {
      organizeId: '',
      currentPage: 1,
      pageSize: 100000,
      sort: 'desc',
      sidx: '',
      keyword: '',
      enabledMark: 1,
    },
    loading: false,
    finish: false,
    total: 0,
  });
  const { list, listQuery, loading } = toRefs(state);

  function init(data) {
    state.listQuery.id = data.id;
    state.listQuery.keyword = '';
    state.list = [];
    initData();
    nextTick(() => {
      bindScroll();
    });
  }
  function initData() {
    state.loading = true;
    getFreeApproverUserList(state.listQuery).then(res => {
      if (res.data.list.length < state.listQuery.pageSize) {
        state.finish = true;
      }
      state.list = [...state.list, ...res.data.list];
      state.total = res.data.pagination.total;
      state.loading = false;
    });
  }
  function bindScroll() {
    const bodyRef = infiniteBody.value;
    const vBody = bodyRef?.getScrollWrap();
    vBody?.addEventListener('scroll', () => {
      if (vBody.scrollHeight - vBody.clientHeight - vBody.scrollTop <= 200 && !state.loading && !state.finish) {
        state.listQuery.currentPage += 1;
        initData();
      }
    });
  }
  function handleSearch() {
    state.list = [];
    state.finish = false;
    state.listQuery.currentPage = 1;
    state.listQuery.pageSize = 20;
    initData();
  }
  function handleReduceApprover(id, index) {
    const query = { ids: [id] };
    reduceApprover(state.listQuery.id, query).then(res => {
      createMessage.success(res.msg);
      state.list.splice(index, 1);
    });
  }
</script>
