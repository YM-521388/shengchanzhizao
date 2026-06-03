<template>
  <div class="common-container">
    <a-select v-model:value="innerValue" v-bind="getSelectBindValue" :options="options" @change="onChange" @click="openSelectModal" />
    <a-modal
      v-model:open="visible"
      :title="popupTitle"
      :width="1000"
      class="common-container-modal"
      @ok="handleSubmit"
      @cancel="handleCancel"
      :maskClosable="false">
      <template #closeIcon>
        <ModalClose :canFullscreen="false" @cancel="handleCancel" />
      </template>
      <div class="jnpf-content-wrapper">
        <div class="jnpf-content-wrapper-left">
          <BasicLeftTree ref="leftTreeRef" :showSearch="false" :treeData="treeData" :loading="treeLoading" @select="handleTreeSelect" />
        </div>
        <div class="jnpf-content-wrapper-center">
          <div class="jnpf-content-wrapper-content">
            <div class="jnpf-common-search-box jnpf-common-search-box-modal">
              <a-form :colon="false" labelAlign="right" :model="state.listQuery" ref="formElRef">
                <a-row :gutter="10">
                  <a-col :span="8">
                    <a-form-item :label="t('common.keyword')" name="keyword">
                      <a-input v-model:value="state.listQuery.keyword" :placeholder="t('common.enterKeyword')" allowClear @pressEnter="handleSearch" />
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
            </div>
            <a-table :data-source="state.list" v-bind="getTableBindValues" @change="handleTableChange" ref="tableElRef"></a-table>
          </div>
        </div>
      </div>
    </a-modal>
  </div>
</template>

<script lang="ts" setup>
  import { getFlowSelector, getFlowEngineListByIds } from '@/api/workFlow/template';
  import { Form, Modal as AModal } from 'ant-design-vue';
  import { reactive, ref, unref, watch, computed, nextTick } from 'vue';
  import ModalClose from '@/components/Modal/src/components/ModalClose.vue';
  import { BasicLeftTree, TreeActionType } from '@/components/Tree';
  import { BasicColumn } from '@/components/Table';
  import { useI18n } from '@/hooks/web/useI18n';
  import { useBaseStore } from '@/store/modules/base';
  import { cloneDeep, pick } from 'lodash-es';
  import { useMessage } from '@/hooks/web/useMessage';
  import type { FormInstance } from 'ant-design-vue';

  interface State {
    list: any[];
    listQuery: any;
    loading: boolean;
    total: number;
    selectedRowKeys: any[];
    selectedRows: any[];
  }

  defineOptions({ inheritAttrs: false });
  const props = defineProps({
    value: { default: [] },
    popupTitle: { type: String, default: '选择流程' },
    placeholder: { type: String, default: '请选择' },
    disabled: { type: Boolean, default: false },
    allowClear: { type: Boolean, default: true },
    multiple: { type: Boolean, default: true },
    size: { type: String, default: 'default' },
    entrustType: { type: Number, default: 0 },
    toUserId: { type: Array, default: [] },
  });
  const state = reactive<State>({
    list: [],
    listQuery: {
      keyword: '',
      currentPage: 1,
      pageSize: 20,
    },
    loading: false,
    total: 0,
    selectedRowKeys: [],
    selectedRows: [],
  });
  const emit = defineEmits(['update:value', 'change']);
  const formItemContext = Form.useInjectFormItemContext();
  const { t } = useI18n();
  const baseStore = useBaseStore();
  const { createMessage } = useMessage();
  const innerValue = ref<string | string[] | undefined>(undefined);
  const visible = ref(false);
  const options = ref<any[]>([]);
  const columns: BasicColumn[] = [
    { title: '流程名称', dataIndex: 'fullName' },
    { title: '流程编码', dataIndex: 'enCode' },
  ];
  const searchInfo = reactive({
    category: '',
    isDelegate: 1,
    isAuthority: 1,
    delegateUser: '',
  });
  const leftTreeRef = ref<Nullable<TreeActionType>>(null);
  const treeLoading = ref(false);
  const treeData = ref<any[]>([]);
  const selectRow = ref<any>(null);
  const formElRef = ref<FormInstance>();
  const tableElRef = ref<any>(null);
  const scrollHeight = window.innerHeight * 0.7 - 52 - 39 - 56;

  const getSelectBindValue = computed(() => {
    return {
      ...pick(props, ['disabled', 'size', 'allowClear', 'placeholder']),
      fieldNames: { label: 'fullName', value: 'id' },
      open: false,
      mode: props.multiple ? 'multiple' : '',
      showSearch: false,
      showArrow: true,
    };
  });
  const getRowSelection = computed<any>(() => ({
    type: props.multiple ? 'checkbox' : 'radio',
    selectedRowKeys: state.selectedRowKeys,
    onChange: setSelectedRowKeys,
  }));
  const getTableBindValues = computed(() => {
    return {
      columns,
      rowSelection: unref(getRowSelection),
      loading: state.loading,
      size: 'small',
      rowKey: 'id',
      scroll: {
        y: scrollHeight,
      },
      pagination: {
        current: state.listQuery.currentPage,
        pageSize: state.listQuery.pageSize,
        size: 'small',
        defaultPageSize: state.listQuery.pageSize,
        showTotal: total => t('component.table.total', { total }),
        showSizeChanger: true,
        pageSizeOptions: ['20', '50', '80', '100'],
        showQuickJumper: true,
        total: state.total,
      },
      class: 'jnpf-basic-table',
    };
  });

  watch(
    () => props.value,
    () => {
      setValue();
    },
    { immediate: true },
  );

  function setValue() {
    if (!props.value || !props.value.length) {
      innerValue.value = [];
      options.value = [];
      selectRow.value = null;
      state.selectedRowKeys = [];
      state.selectedRows = [];
      return;
    }
    const ids = props.value as any[];
    getFlowEngineListByIds(ids).then(res => {
      const selectedList: any[] = res.data;
      const innerIds = selectedList.map(o => o.id);
      innerValue.value = innerIds;
      options.value = cloneDeep(selectedList);
      selectRow.value = cloneDeep(selectedList);
      state.selectedRowKeys = innerIds;
      state.selectedRows = cloneDeep(selectedList);
    });
  }
  function onChange(val, option) {
    if (!val || !val.length) {
      options.value = [];
      emit('update:value', []);
      emit('change', '', []);
      state.selectedRowKeys = [];
      state.selectedRows = [];
    } else {
      options.value = option;
      emit('update:value', val);
      emit('change', '', options.value);
    }
    formItemContext.onFieldChange();
  }
  async function openSelectModal() {
    if (props.disabled) return;
    if (!props.toUserId?.length) return createMessage.error(props.entrustType === 0 ? '请先选择受委托人' : '请先选择代理人');
    visible.value = true;
    treeLoading.value = true;
    const firstItem = { id: '', encode: 'all', fullName: '全部流程' };
    const res = (await baseStore.getDictionaryData('businessType')) as any[];
    treeData.value = [firstItem, ...res];
    searchInfo.category = treeData.value[0].id;
    searchInfo.isAuthority = props.entrustType === 0 ? 1 : 0;
    const leftTree = unref(leftTreeRef);
    leftTree?.setSelectedKeys([searchInfo.category]);
    treeLoading.value = false;
    state.loading = true;
    handleReset();
    const tableEl = tableElRef.value?.$el;
    let bodyEl = tableEl.querySelector('.ant-table-body');
    bodyEl!.style.height = `${scrollHeight}px`;
  }
  function initData() {
    state.loading = true;
    const query = {
      ...state.listQuery,
      ...unref(searchInfo),
      isLaunch: 1,
    };
    getFlowSelector(query)
      .then(res => {
        state.list = res.data.list || [];
        state.total = res.data.pagination.total;
        state.loading = false;
      })
      .catch(() => {
        state.loading = false;
      });
  }
  function handleTreeSelect(id) {
    if (searchInfo.category === id) return;
    searchInfo.category = id;
    nextTick(() => initData());
  }
  function handleCancel() {
    visible.value = false;
  }
  function getForm() {
    const form = unref(formElRef);
    if (!form) {
      throw new Error('form is null!');
    }
    return form;
  }
  function handleSearch() {
    state.listQuery.currentPage = 1;
    state.listQuery.pageSize = state.listQuery.pageSize;
    updateSelectRow();
    nextTick(() => initData());
  }
  function handleReset() {
    getForm().resetFields();
    state.listQuery.keyword = '';
    handleSearch();
  }
  function setSelectedRowKeys(selectedRowKeys, selectedRows) {
    state.selectedRowKeys = selectedRowKeys;
    state.selectedRows = selectedRows;
  }
  function handleTableChange(pagination) {
    state.listQuery.currentPage = pagination.current;
    state.listQuery.pageSize = pagination.pageSize;
    updateSelectRow();
    nextTick(() => initData());
  }
  function updateSelectRow() {
    if (!selectRow.value) selectRow.value = [];
    const newSelectRow = state.selectedRows;
    for (let i = 0; i < newSelectRow.length; i++) {
      const item = newSelectRow[i];
      if (!selectRow.value.some(o => o.id === item.id)) selectRow.value.push(item);
    }
    selectRow.value = selectRow.value.filter(o => !(state.list.some(l => l.id === o.id) && !newSelectRow.some(l => l.id === o.id)));
  }
  function handleSubmit() {
    updateSelectRow();
    options.value = selectRow.value;
    innerValue.value = unref(selectRow).map(o => o.id);
    emit('update:value', unref(innerValue));
    emit('change', unref(innerValue), unref(selectRow));
    formItemContext.onFieldChange();
    handleCancel();
  }
</script>
