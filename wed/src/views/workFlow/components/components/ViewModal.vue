<template>
  <div class="common-container">
    <a-modal
      v-model:open="visible"
      title="查看数据"
      :width="800"
      class="common-container-modal"
      :wrapClassName="getWrapClassName"
      @cancel="handleCancel"
      :footer="null"
      :maskClosable="false">
      <template #closeIcon>
        <ModalClose :fullScreen="fullScreenRef" @cancel="handleCancel" @fullscreen="handleFullScreen" />
      </template>
      <div class="jnpf-common-search-box jnpf-common-search-box-modal">
        <a-form :colon="false" labelAlign="right" :model="listQuery" ref="formElRef" :class="getFormClass">
          <a-row :gutter="10">
            <a-col :span="8">
              <a-form-item :label="t('common.keyword')" name="keyword">
                <a-input v-model:value="listQuery.keyword" :placeholder="t('common.enterKeyword')" allowClear @pressEnter="handleSearch" />
              </a-form-item>
            </a-col>
            <a-col :span="8">
              <a-form-item label=" ">
                <a-button type="primary" class="mr-2" @click="handleSearch">{{ t('common.queryText') }}</a-button>
                <a-button @click="handleReset">{{ t('common.resetText') }}</a-button>
              </a-form-item>
            </a-col>
          </a-row>
        </a-form>
        <div class="jnpf-common-search-box-right">
          <a-tooltip placement="top">
            <template #title>
              <span>{{ t('common.redo') }}</span>
            </template>
            <RedoOutlined class="jnpf-common-search-box-right-icon" @click="initData" />
          </a-tooltip>
        </div>
      </div>
      <a-table :data-source="list" v-bind="getTableBindValues" @change="handleTableChange" ref="tableElRef">
        <template #bodyCell="{ column, record }">
          <template v-if="column.dataIndex !== 'index'">{{ record[column.dataIndex] }}</template>
        </template>
      </a-table>
    </a-modal>
  </div>
</template>

<script lang="ts" setup>
  import { getDataInterfaceDataSelect } from '@/api/systemData/dataInterface';
  import { Modal as AModal } from 'ant-design-vue';
  import { ref, unref, computed, nextTick, reactive, toRefs, watch } from 'vue';
  import ModalClose from '@/components/Modal/src/components/ModalClose.vue';
  import { useI18n } from '@/hooks/web/useI18n';
  import { useDesign } from '@/hooks/web/useDesign';
  import { RedoOutlined } from '@ant-design/icons-vue';
  import type { FormInstance } from 'ant-design-vue';
  import { useFullScreen } from '@/components/Modal/src/hooks/useModalFullScreen';

  interface State {
    list: any[];
    listQuery: any;
    loading: boolean;
    total: number;
  }

  defineOptions({ name: 'SelectModal', inheritAttrs: false });
  const props = defineProps({
    config: {
      type: Object,
      default: () => {},
    },
    formData: Object,
  });
  const emit = defineEmits(['select']);
  const { t } = useI18n();
  const { prefixCls: formPrefixCls } = useDesign('basic-form');
  const { prefixCls: tablePrefixCls } = useDesign('basic-table');
  const { handleFullScreen, getWrapClassName, fullScreenRef, resetFullScreen } = useFullScreen();
  const visible = ref(false);
  const formElRef = ref<FormInstance>();
  const tableElRef = ref<any>(null);
  const indexColumn = { width: 50, title: '序号', dataIndex: 'index', key: 'index', align: 'center', customRender: ({ index }) => index + 1 };
  const state = reactive<State>({
    list: [],
    listQuery: {
      keyword: '',
      currentPage: 1,
      pageSize: 20,
    },
    loading: false,
    total: 0,
  });
  const { listQuery, list } = toRefs(state);

  const getFormClass = computed(() => {
    return [formPrefixCls, `${formPrefixCls}--compact`, 'search-form'];
  });
  const getColumns = computed<any[]>(() => {
    const columns = (props.config.columnOptions as any)
      .filter(o => o.ifShow || o.ifShow === undefined)
      .map(o => ({ title: o.label, dataIndex: o.value, ellipsis: true, width: o.width || 100 }));
    return [indexColumn, ...columns];
  });
  const searchInfo = computed(() => {
    const paramList = getParamList();
    const columnOptions = (props.config.columnOptions as any).map(o => o.value).join(',');
    const info: any = {
      interfaceId: props.config.interfaceId,
      columnOptions,
      paramList,
    };

    return info;
  });
  const getPagination = computed<any>(() => {
    return {
      current: state.listQuery.currentPage,
      pageSize: state.listQuery.pageSize,
      size: 'small',
      defaultPageSize: 20,
      showTotal: total => t('component.table.total', { total }),
      showSizeChanger: true,
      pageSizeOptions: ['20', '50', '80', '100'],
      showQuickJumper: true,
      total: state.total,
    };
  });
  const getScrollY = computed(() => {
    const scale = unref(fullScreenRef) ? 0.9 : 0.7;
    let height = window.innerHeight * scale - 52 - 38;
    height -= 44;
    return height;
  });
  const getTableBindValues = computed(() => {
    return {
      columns: unref(getColumns),
      pagination: unref(getPagination),
      size: 'small',
      loading: state.loading,
      rowKey: record => record,
      scroll: {
        y: unref(getScrollY),
      },
      class: unref(tablePrefixCls),
    };
  });

  defineExpose({ openViewModal });

  watch(
    () => fullScreenRef.value,
    () => {
      nextTick(() => setTableHeight());
    },
  );

  function getForm() {
    const form = unref(formElRef);
    if (!form) {
      throw new Error('form is null!');
    }
    return form;
  }
  function openViewModal() {
    visible.value = true;
    setTimeout(() => {
      nextTick(() => {
        handleReset();
        setTableHeight();
        resetFullScreen();
        state.list = [];
        state.total = 0;
        const tableEl = tableElRef.value?.$el;
        let bodyEl = tableEl.querySelector('.ant-table-body');
        bodyEl!.style.height = `${unref(getScrollY)}px`;
      });
    }, 50);
  }
  function handleCancel() {
    visible.value = false;
  }

  function getParamList() {
    let templateJson: any[] = props.config.templateJson;
    if (!props.formData) return templateJson;
    for (let i = 0; i < templateJson.length; i++) {
      const e = templateJson[i];
      const data = props.formData;
      if (e.sourceType == 1) {
        e.defaultValue = data[e.relationField] || data[e.relationField] == 0 || data[e.relationField] == false ? data[e.relationField] : '';
      }
    }
    return templateJson;
  }
  function handleSearch() {
    state.listQuery.currentPage = 1;
    state.listQuery.pageSize = 20;
    initData();
  }
  function handleReset() {
    getForm().resetFields();
    state.listQuery.keyword = '';
    handleSearch();
  }
  function initData() {
    if (!props.config.interfaceId) return;
    state.loading = true;
    const query = {
      ...state.listQuery,
      ...unref(searchInfo),
    };
    getDataInterfaceDataSelect(query)
      .then(res => {
        state.list = res.data.list;
        state.total = res.data.pagination.total;
        state.loading = false;
      })
      .catch(() => {
        state.loading = false;
      });
  }
  function handleTableChange(pagination) {
    state.listQuery.currentPage = pagination.current;
    state.listQuery.pageSize = pagination.pageSize;
    initData();
  }
  function setTableHeight() {
    const tableEl = tableElRef.value?.$el;
    let bodyEl = tableEl.querySelector('.ant-table-body');
    bodyEl!.style.height = `${unref(getScrollY)}px`;
  }
</script>
