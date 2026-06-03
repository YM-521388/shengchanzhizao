<template>
  <div class="flow-list-container">
    <a-tabs v-model:activeKey="category" tab-position="left" class="common-left-tabs flow-left-tabs">
      <a-tab-pane key="" tab="全部模板"></a-tab-pane>
      <a-tab-pane :key="item.id" :tab="item.fullName" v-for="item in categoryList"></a-tab-pane>
    </a-tabs>
    <div class="flow-list">
      <div class="jnpf-common-search-box">
        <BasicForm class="search-form" @register="registerForm" @submit="handleSubmit" @reset="handleReset"></BasicForm>
      </div>
      <div class="list">
        <ScrollContainer v-loading="loading && listQuery.currentPage === 1" ref="infiniteBody">
          <div class="px-10px pt-10px" v-if="list.length">
            <a-row :gutter="20">
              <a-col :span="6" v-for="(item, i) in list" :key="i" class="item" @click="handleClick(item)">
                <a-card hoverable>
                  <div class="item-icon" :style="{ backgroundColor: item.iconBackground || '#008cff' }">
                    <i :class="item.icon" />
                  </div>
                  <span class="item-title">{{ item.fullName }}</span>
                </a-card>
              </a-col>
            </a-row>
          </div>
          <jnpf-empty v-if="!list.length" />
        </ScrollContainer>
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
  import { reactive, toRefs, onMounted, watch, ref, nextTick } from 'vue';
  import { getPrintWorkSelector } from '@/api/system/printDev';
  import { BasicForm, useForm } from '@/components/Form';
  import { useI18n } from '@/hooks/web/useI18n';
  import { useBaseStore } from '@/store/modules/base';
  import { ScrollContainer, ScrollActionType } from '@/components/Container';

  interface State {
    [prop: string]: any;
  }

  const emit = defineEmits(['select']);
  const state = reactive<State>({
    category: '',
    categoryList: [],
    list: [],
    listQuery: {
      currentPage: 1,
      pageSize: 50,
      sort: 'desc',
      sidx: '',
    },
    keyword: '',
    loading: false,
    finish: false,
    total: 0,
    flowList: [],
    userList: [],
    activeFlow: {},
  });
  const { category, categoryList, list, listQuery, loading } = toRefs(state);
  const baseStore = useBaseStore();
  const { t } = useI18n();

  const [registerForm, { resetFields }] = useForm({
    baseColProps: { span: 6 },
    showActionButtonGroup: true,
    showAdvancedButton: true,
    compact: true,
    schemas: [
      {
        field: 'keyword',
        label: t('common.keyword'),
        component: 'Input',
        componentProps: {
          placeholder: t('common.enterKeyword'),
          submitOnPressEnter: true,
        },
      },
    ],
  });
  const infiniteBody = ref<Nullable<ScrollActionType>>(null);

  watch(
    () => state.category,
    () => {
      resetFields();
    },
  );

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
  function initData() {
    state.loading = true;
    let query = {
      ...state.listQuery,
      keyword: state.keyword,
      category: state.category,
    };
    getPrintWorkSelector(query).then(res => {
      if (res.data.list.length < state.listQuery.pageSize) {
        state.finish = true;
      }
      state.list = [...state.list, ...res.data.list];
      state.total = res.data.pagination.total;
      state.loading = false;
    });
  }
  async function getDictionaryData() {
    const res = await baseStore.getDictionaryData('businessType');
    state.categoryList = res as any[];
    initData();
    nextTick(() => {
      bindScroll();
    });
  }
  function handleSubmit(values) {
    if (state.loading) return;
    state.keyword = values?.keyword || '';
    search();
  }
  function handleReset() {
    if (state.loading) return;
    state.keyword = '';
    search();
  }
  function search() {
    state.list = [];
    state.finish = false;
    state.listQuery = {
      currentPage: 1,
      pageSize: 50,
      sort: 'desc',
      sidx: '',
    };
    initData();
  }
  function handleClick(item) {
    emit('select', item);
  }

  onMounted(() => {
    getDictionaryData();
  });
</script>

<style lang="less" scoped>
  .flow-list-container {
    height: 100%;
    width: 100%;
    display: flex;
    .flow-list {
      padding: 10px 0;
      flex: 1;
      height: 100%;
      display: flex;
      flex-direction: column;
      overflow: hidden;
      .list {
        flex: 1;
        min-height: 0;
        .item {
          margin-bottom: 20px;
          cursor: pointer;
          &:nth-last-child(1),
          &:nth-last-child(2),
          &:nth-last-child(3),
          &:nth-last-child(4) {
            margin-bottom: 0;
          }
          :deep(.ant-card) {
            border-radius: 8px;
            .ant-card-body {
              display: flex;
              align-items: center;
              padding: 20px 15px 20px 20px;

              &:hover {
                .item-star i {
                  display: block;
                }
              }
            }
          }
          .item-icon {
            width: 56px;
            height: 56px;
            border-radius: 8px;
            text-align: center;
            background-color: #ccc;
            display: inline-block;
            margin-right: 20px;
            i {
              text-align: center;
              font-size: 40px;
              color: #fff;
              line-height: 56px;
            }
          }
          .item-title {
            flex: 1;
            min-width: 0;
            overflow: hidden;
            text-overflow: ellipsis;
            display: -webkit-box;
            -webkit-line-clamp: 2;
            -webkit-box-orient: vertical;
            word-break: break-all;
            font-size: 14px;
          }
          .item-star {
            width: 32px;
            margin-top: -36px;
            padding: 6px;
            text-align: right;
            i {
              display: none;
              color: #c0c4cc;
              font-size: 14px;
            }
            .common-flow {
              color: #efae32;
              display: block;
            }
          }
        }
      }
      .ant-empty {
        margin-top: 60px;
      }
    }
  }
</style>
