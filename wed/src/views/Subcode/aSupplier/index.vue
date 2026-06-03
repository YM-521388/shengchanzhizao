<template>
  <div class="jnpf-content-wrapper">
    <div class="jnpf-content-wrapper-center">
      <div class="jnpf-content-wrapper-search-box" v-if="getSearchList.length">
        <BasicForm
          @register="registerSearchForm"
          :schemas="getSearchList"
          @advanced-change="redoHeight"
          @submit="handleSearchSubmit"
          @reset="handleSearchReset"
          class="search-form">
        </BasicForm>
      </div>
      <div class="jnpf-content-wrapper-content bg-white">
        <BasicTable
          @register="registerTable"
          v-bind="getTableBindValue"
          ref="tableRef"
          @columns-change="handleColumnChange"
          @resize-column="handleResizeColumn">
          <template #tableTitle>
            <a-button type="primary" preIcon="icon-ym icon-ym-btn-add" @click="addHandle()" v-auth="'btn_add'">{{ t('common.add2Text', '新增') }}</a-button>
            <a-button
              type="link"
              preIcon="icon-ym icon-ym-btn-download"
              @click="openExportModal(true, { columnList: state.exportList, selectIds: getSelectRowKeys() })"
              v-auth="'btn_download'"
              >{{ t('common.exportText', '导出') }}</a-button
            >
          </template>
          <!-- 有高级查询：开始 -->
          <!-- <template #toolbar>
            <a-tooltip placement="top">
              <template #title>
                <span>{{ t('common.superQuery') }}</span>
              </template>
              <filter-outlined @click="openSuperQuery(true, { columnOptions: superQueryJson })" />
            </a-tooltip>
          </template> -->
          <!-- 有高级查询：结束 -->
          <template #toolbarAfter>
            <ViewList :menuId="searchInfo.menuId" :viewList="viewList" @itemClick="handleViewClick" @reload="initViewList" />
            <ViewSetting :menuId="searchInfo.menuId" :viewList="viewList" :currentView="currentView" @reload="initViewList" />
            <HelpDoc docId="10062" />
          </template>
          <template #bodyCell="{ column, record }">
            <template v-if="!(record.top || column.id?.includes('-'))">
              <template v-if="column.jnpfKey === 'sign'">
                <jnpf-sign v-model:value="record[column.dataIndex]" detailed />
              </template>
              <template v-if="column.jnpfKey === 'signature'">
                <jnpf-signature v-model:value="record[column.dataIndex]" detailed />
              </template>
              <template v-if="column.jnpfKey === 'rate'">
                <jnpf-rate v-model:value="record[column.dataIndex]" :count="column.count" :allowHalf="column.allowHalf" disabled />
              </template>
              <template v-if="column.jnpfKey === 'slider'">
                <jnpf-slider v-model:value="record[column.dataIndex]" :min="column.min" :max="column.max" :step="column.step" disabled />
              </template>
              <template v-if="column.jnpfKey === 'uploadImg'">
                <jnpf-upload-img v-model:value="record[column.dataIndex]" disabled detailed simple v-if="record[column.dataIndex]?.length" />
              </template>
              <template v-if="column.jnpfKey === 'uploadFile'">
                <jnpf-upload-file v-model:value="record[column.dataIndex]" disabled detailed simple v-if="record[column.dataIndex]?.length" />
              </template>
              <template v-if="column.jnpfKey === 'input'">
                <jnpf-input v-model:value="record[column.dataIndex]" :useMask="column.useMask" :maskConfig="column.maskConfig" :showOverflow="true" detailed />
              </template>
              <template v-if="column.key === 'F_StartUsing'">
                <a-tag :color="record.F_StartUsing == '开' ? 'success' : 'error'">
                  {{ record.F_StartUsing == '开' ? '启用' : '禁用' }}
                </a-tag>
              </template>
            </template>
            <template v-if="column.key === 'action' && !record.top">
              <TableAction :actions="getTableActions(record)" />
            </template>
          </template>
        </BasicTable>
      </div>
    </div>
    <Form ref="formRef" @reload="reload" />
    <Detail ref="detailRef" />
    <ExportModal @register="registerExportModal" @download="handleDownload" />
    <SuperQueryModal @register="registerSuperQueryModal" @superQuery="handleSuperQuery" />
  </div>
</template>

<script lang="ts" setup>
  import { getList, del, exportData, batchDelete } from './helper/api';
  import { getViewList } from '@/api/onlineDev/visualDev';
  import { getDictionaryDataSelector } from '@/api/systemData/dictionary';
  import { getDataInterfaceRes } from '@/api/systemData/dataInterface';
  import { getOrgByOrganizeCondition, getDepartmentSelectAsyncList } from '@/api/permission/organize';
  import { ref, reactive, onMounted, toRefs, computed, unref, nextTick, provide } from 'vue';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useI18n } from '@/hooks/web/useI18n';
  import { useOrganizeStore } from '@/store/modules/organize';
  import { useUserStore } from '@/store/modules/user';
  import { useModal } from '@/components/Modal';
  import { BasicForm, useForm } from '@/components/Form';
  import { BasicTable, useTable, TableAction, ActionItem, TableActionType, SorterResult } from '@/components/Table';
  import Form from './Form.vue';
  import Detail from './Detail.vue';
  import { ExportModal, ImportModal, SuperQueryModal } from '@/components/CommonModal';
  import { downloadByUrl } from '@/utils/file/download';
  import { useRoute, useRouter } from 'vue-router';
  import { useTabs } from '@/hooks/web/useTabs';
  import { getFlowStartFormId } from '@/api/workFlow/template';
  import { FilterOutlined } from '@ant-design/icons-vue';
  import { getSearchFormSchemas } from '@/components/FormGenerator/src/helper/transform';
  import { cloneDeep } from 'lodash-es';
  import columnList from './helper/columnList';
  import searchList from './helper/searchList';
  import superQueryJson from './helper/superQueryJson';
  import { dyOptionsList } from '@/components/FormGenerator/src/helper/config';
  import { thousandsFormat, getParamList } from '@/utils/jnpf';
  import { usePermission } from '@/hooks/web/usePermission';
  import { noGroupList } from '@/components/FormGenerator/src/helper/config';
  import ViewSetting from '@/views/common/dynamicModel/list/components/ViewSetting.vue';
  import ViewList from '@/views/common/dynamicModel/list/components/ViewList.vue';
  import HelpDoc from '@/components/HelpDoc/index.vue';

  interface State {
    config: any;
    columnList: any[];
    printListOptions: any[];
    columnBtnsList: any[];
    customBtnsList: any[];
    treeActiveId: string;
    treeActiveNodePath: any;
    columns: any[];
    complexColumns: any[];
    childColumnList: any[];
    exportList: any[];
    cacheList: any[];
    currFlow: any;
    isCustomCopy: boolean;
    candidateType: number;
    currRow: any;
    workFlowFormData: any;
    expandObj: any;
    columnSettingList: any[];
    searchSchemas: any[];
    treeRelationObj: any;
    treeQueryJson: any;
    leftTreeActiveInfo: any;
    viewList: any[];
    currentView: any;
  }

  const router = useRouter();
  const { close } = useTabs();
  const route = useRoute();
  const { hasBtnP } = usePermission();
  const { createMessage, createConfirm } = useMessage();
  const { t } = useI18n();
  const organizeStore = useOrganizeStore();
  const userStore = useUserStore();
  const userInfo = userStore.getUserInfo;

  const [registerExportModal, { openModal: openExportModal, closeModal: closeExportModal, setModalProps: setExportModalProps }] = useModal();
  const [registerImportModal, { openModal: openImportModal }] = useModal();
  const [registerSuperQueryModal, { openModal: openSuperQuery }] = useModal();
  const formRef = ref<any>(null);
  const tableRef = ref<Nullable<TableActionType>>(null);
  const detailRef = ref<any>(null);
  const state = reactive<State>({
    config: {},
    columnList: [],
    printListOptions: [],
    columnBtnsList: [],
    customBtnsList: [],
    treeActiveId: '',
    treeActiveNodePath: [],
    columns: [],
    complexColumns: [], // 复杂表头
    childColumnList: [],
    exportList: [],
    cacheList: [],
    currFlow: {},
    isCustomCopy: false,
    candidateType: 1,
    currRow: {},
    workFlowFormData: {},
    expandObj: {},
    columnSettingList: [],
    searchSchemas: [],
    treeRelationObj: null,
    treeQueryJson: {},
    leftTreeActiveInfo: {},
    viewList: [],
    currentView: {},
  });
  const { childColumnList, searchSchemas, viewList, currentView } = toRefs(state);
  const defaultSearchInfo = {
    menuId: route.meta.modelId as string,
    moduleId: '2008453570059440128',
    superQueryJson: '',
  };
  const searchInfo: any = reactive({
    ...cloneDeep(defaultSearchInfo),
  });
  const [registerSearchForm, { updateSchema, resetFields, submit: searchFormSubmit }] = useForm({
    baseColProps: { span: 6 },
    showActionButtonGroup: true,
    showAdvancedButton: true,
    compact: true,
  });
  const [registerChildTable] = useTable({
    pagination: false,
    canResize: false,
    showTableSetting: false,
  });
  const [registerTable, { reload, setLoading, getFetchParams, getSelectRows, getSelectRowKeys, redoHeight, clearSelectedRowKeys }] = useTable({
    api: getList,
    immediate: false,
    clickToRowSelect: false,
    tableSetting: { setting: false },
    afterFetch: data => {
      const list = data.map(o => ({
        ...o,
        ...state.expandObj,
      }));
      state.cacheList = cloneDeep(list);
      return list;
    },
  });

  provide('getLeftTreeActiveInfo', () => state.leftTreeActiveInfo);

  const getColumns = computed(() => {
    const columns: any[] = state.complexColumns;
    return setListValue(state.currentView?.columnList, columns, 'prop');
  });
  const getSearchList = computed(() => {
    const searchSchemas = cloneDeep(state.searchSchemas).map(o => ({ ...o, show: true }));
    return setListValue(state.currentView?.searchList, searchSchemas, 'field');
  });
  const getHasBatchBtn = computed(() => {
    let btnsList: any[] = [];
    btnsList.push('download');
    btnsList = btnsList.filter(o => hasBtnP('btn_' + o));
    return !!btnsList.length;
  });
  const getTableBindValue = computed(() => {
    let columns = unref(getColumns);
    const defaultSortConfig = [];
    const sortField = defaultSortConfig.map(o => (o.sort === 'desc' ? '-' : '') + o.field);
    const data: any = {
      pagination: { pageSize: 20 }, //有分页
      searchInfo: unref(searchInfo),
      ellipsis: true,
      defSort: { sidx: sortField.join(',') },
      sortFn: (sortInfo: SorterResult | SorterResult[]) => {
        if (Array.isArray(sortInfo)) {
          const sortList = sortInfo.map(o => (o.order === 'descend' ? '-' : '') + o.field);
          return { sidx: sortList.join(',') };
        } else {
          const { field, order } = sortInfo;
          if (field && order) {
            // 排序字段
            return { sidx: (order === 'descend' ? '-' : '') + field };
          } else {
            return {};
          }
        }
      },
      columns,
      bordered: true,
      actionColumn: {
        width: 150,
        title: t('component.table.action'),
        dataIndex: 'action',
      },
    };
    if (unref(getHasBatchBtn)) {
      const rowSelection: any = { type: 'checkbox' };
      data.rowSelection = rowSelection;
    }
    return data;
  });
  function getTableActions(record): ActionItem[] {
    return [
      {
        label: t('common.editText', '编辑'),
        onClick: updateHandle.bind(null, record),
        auth: 'btn_edit',
      },
      {
        label: t('common.delText', '删除'),
        color: 'error',
        modelConfirm: {
          onOk: handleDelete.bind(null, record.id),
        },
        auth: 'btn_remove',
      },
      {
        label: t('common.detailText', '详情'),
        onClick: goDetail.bind(null, record),
        auth: 'btn_detail',
      },
    ];
  }
  // 新增
  function addHandle() {
    const data = {
      id: '',
      menuId: searchInfo.menuId,
      allList: state.cacheList,
    };
    formRef.value?.init(data);
  }
  // 导出
  function handleDownload(data) {
    data.selectKey = data.selectKey.join(',');
    data.selectIds = data.selectIds.join(',');
    let query = { ...getFetchParams(), ...data };
    exportData(query)
      .then(res => {
        setExportModalProps({ confirmLoading: false });
        if (!res.data.url) return;
        downloadByUrl({ url: res.data.url });
        closeExportModal();
      })
      .catch(() => {
        setExportModalProps({ confirmLoading: false });
      });
  }
  // 编辑
  function updateHandle(record) {
    // 不带工作流
    const data = {
      id: record.id,
      menuId: searchInfo.menuId,
      allList: state.cacheList,
    };
    formRef.value?.init(data);
  }
  // 查看详情
  function goDetail(record) {
    // 不带流程
    const data = {
      id: record.id,
    };
    detailRef.value?.init(data);
  }
  function handleDelete(id) {
    const query = { ids: [id] };
    batchDelete(query).then(res => {
      createMessage.success(res.msg);
      clearSelectedRowKeys();
      reload();
    });
  }
  async function init() {
    state.config = {};
    searchInfo.menuId = route.meta.modelId as string;

    state.columnList = columnList;
    getSearchSchemas();

    setLoading(true);
    getColumnList();
    await initViewList();
    nextTick(() => {
      unref(getSearchList).length ? searchFormSubmit() : handleSearchSubmit({});
    });
  }
  function getSearchSchemas() {
    const schemas = getSearchFormSchemas(searchList);
    state.searchSchemas = schemas;
    schemas.forEach(cur => {
      const config = cur.__config__;
      if (dyOptionsList.includes(config.jnpfKey)) {
        if (config.dataType === 'dictionary') {
          if (!config.dictionaryType) return;
          getDictionaryDataSelector(config.dictionaryType).then(res => {
            updateSchema([{ field: cur.field, componentProps: { options: res.data.list } }]);
          });
        }
        if (config.dataType === 'dynamic') {
          if (!config.propsUrl) return;
          const query = { paramList: getParamList(config.templateJson) || [] };
          getDataInterfaceRes(config.propsUrl, query).then(res => {
            const data = Array.isArray(res.data) ? res.data : [];
            updateSchema([{ field: cur.field, componentProps: { options: data } }]);
          });
        }
      }
      cur.defaultValue = cur.value;
    });
  }
  function getColumnList() {
    // 过滤权限
    let columnList: any[] = [];
    const permissionList = userStore.getPermissionList;
    const list = permissionList.filter(o => o.modelId === searchInfo.menuId);
    const perColumnList = list[0] && list[0].column ? list[0].column : [];
    for (let i = 0; i < state.columnList.length; i++) {
      inner: for (let j = 0; j < perColumnList.length; j++) {
        if (state.columnList[i].prop === perColumnList[j].enCode) {
          columnList.push(state.columnList[i]);
          break inner;
        }
      }
    }

    state.exportList = columnList.filter(o => !noGroupList.includes(o.__config__.jnpfKey));
    let columns = columnList.map(o => ({
      ...o,
      title: o.labelI18nCode ? t(o.labelI18nCode, o.label) : o.label,
      dataIndex: o.prop,
      align: o.align,
      fixed: o.fixed == 'none' ? false : o.fixed,
      sorter: o.sortable ? { multiple: 1 } : o.sortable,
      width: o.width || 100,
      // 允许列宽拖拽
      resizable: true,
    }));
    columns = getComplexColumns(columns);
    state.columns = columns.filter(o => o.prop.indexOf('-') < 0);
    getChildComplexColumns(columns);
  }
  // 列宽拖拽处理：把新的宽度写回到 state.columns 以触发视图更新
  function handleResizeColumn(w, col) {
    try {
      if (col) col.width = w;
      state.columns = state.columns.map(c => {
        if (c.prop === col?.prop || c.dataIndex === col?.dataIndex) {
          return { ...c, width: w };
        }
        return c;
      });
    } catch (e) {
      // ignore
    }
  }
  function getComplexColumns(columns) {
    let complexHeaderList: any[] = [];
    if (!complexHeaderList.length) return columns;
    let childColumns: any[] = [];
    let firstChildColumns: string[] = [];
    for (let i = 0; i < complexHeaderList.length; i++) {
      const e = complexHeaderList[i];
      e.label = e.fullName;
      e.labelI18nCode = e.fullNameI18nCode;
      e.title = e.fullNameI18nCode ? t(e.fullNameI18nCode, e.fullName) : e.fullName;
      e.align = e.align;
      e.dataIndex = e.id;
      e.prop = e.id;
      e.children = [];
      e.jnpfKey = 'complexHeader';
      if (e.childColumns?.length) {
        childColumns.push(...e.childColumns);
        for (let k = 0; k < e.childColumns.length; k++) {
          const item = e.childColumns[k];
          for (let j = 0; j < columns.length; j++) {
            const o = columns[j];
            if (o.prop == item && o.fixed !== 'left' && o.fixed !== 'right') e.children.push({ ...o });
          }
        }
      }
      if (e.children.length) firstChildColumns.push(e.children[0].prop);
    }
    complexHeaderList = complexHeaderList.filter(o => o.children.length);
    let list: any[] = [];
    for (let i = 0; i < columns.length; i++) {
      const e = columns[i];
      if (!childColumns.includes(e.prop)) {
        list.push(e);
      } else {
        if (firstChildColumns.includes(e.prop)) {
          const item = complexHeaderList.find(o => o.childColumns.includes(e.prop));
          list.push(item);
        }
      }
    }
    return list;
  }
  function getChildComplexColumns(columnList) {
    let list: any[] = [];
    for (let i = 0; i < columnList.length; i++) {
      const e = columnList[i];
      if (!e.prop.includes('-')) {
        list.push(e);
      } else {
        let prop = e.prop.split('-')[0];
        let vModel = e.prop.split('-')[1];
        let label = e.label.split('-')[0];
        let childLabel = e.label.replace(label + '-', '');
        if (e.fullNameI18nCode && Array.isArray(e.fullNameI18nCode) && e.fullNameI18nCode[0]) label = t(e.fullNameI18nCode[0], label);
        let newItem = {
          align: 'center',
          jnpfKey: 'table',
          prop,
          label,
          title: label,
          dataIndex: prop,
          children: [],
        };
        e.dataIndex = vModel;
        e.title = e.labelI18nCode ? t(e.labelI18nCode, childLabel) : childLabel;
        if (!state.expandObj.hasOwnProperty(`${prop}Expand`)) state.expandObj[`${prop}Expand`] = false;
        if (!list.some(o => o.prop === prop)) list.push(newItem);
        for (let i = 0; i < list.length; i++) {
          if (list[i].prop === prop) {
            list[i].children.push(e);
            break;
          }
        }
      }
    }
    // 行内分组展示
    getMergeList(list);
    state.complexColumns = list;
    state.childColumnList = list.filter(o => o.jnpfKey === 'table');
    // 子表分组展示宽度取100
    for (let i = 0; i < state.childColumnList.length; i++) {
      const e = state.childColumnList[i];
      if (e.children?.length) e.children = e.children.map(o => ({ ...o, width: 100 }));
    }
  }
  function getMergeList(list) {
    list.forEach(item => {
      if (item.jnpfKey === 'table' && item.children && item.children.length) {
        item.children.forEach((child, index) => {
          if (index == 0) {
            child.customCell = () => ({
              rowspan: 1,
              colspan: item.children.length,
              class: 'child-table-box',
            });
          } else {
            child.customCell = () => ({
              rowspan: 0,
              colspan: 0,
            });
          }
        });
      }
    });
  }
  function handleColumnChange(data) {
    state.columnSettingList = data;
  }
  // 高级查询
  function handleSuperQuery(superQueryJson) {
    if (!superQueryJson) {
      searchInfo.superQueryJson = '';
      reload({ page: 1 });
      return;
    }
    let queryJsonObj = JSON.parse(superQueryJson);
    searchInfo.superQueryJson = JSON.stringify(queryJsonObj);
    reload({ page: 1 });
  }
  function handleSearchReset() {
    searchFormSubmit();
  }
  function handleSearchSubmit(data) {
    clearSelectedRowKeys();
    let obj = {
      ...defaultSearchInfo,
      superQueryJson: searchInfo.superQueryJson,
      ...data,
      ...(state.treeQueryJson || {}),
    };
    Object.keys(searchInfo).map(key => {
      delete searchInfo[key];
    });
    for (let [key, value] of Object.entries(obj)) {
      searchInfo[key.replaceAll('-', '_')] = value;
    }
    reload({ page: 1 });
  }
  async function initViewList(currentId = '') {
    const query = {
      menuId: searchInfo.menuId,
    };
    await getViewList(query).then(res => {
      const columns: any[] = state.complexColumns;
      const searchList: any[] = state.searchSchemas.map(o => ({ label: o.label, id: o.field, show: o.show }));
      const columnList: any[] = columns.map(o => ({ label: o.label, id: o.prop, show: true, fixed: o.fixed || 'none', labelI18nCode: o.labelI18nCode }));
      state.viewList = (res.data || []).map(o => {
        if (o.type == 0) return { ...o, searchList, columnList };
        return { ...o, searchList: o.searchList ? JSON.parse(o.searchList) : [], columnList: o.columnList ? JSON.parse(o.columnList) : [] };
      });
      if (currentId) {
        state.currentView = state.viewList.filter(o => o.id === currentId)[0] || state.viewList[0];
      } else {
        state.currentView = state.viewList.filter(o => o.status === 1)[0] || state.viewList[0];
      }
    });
  }
  function handleViewClick(item) {
    state.currentView = item;
  }
  function setListValue(data: any[] = [], defaultData: any[] = [], key) {
    let list: any[] = [];
    for (let i = 0; i < data.length; i++) {
      for (let j = 0; j < defaultData.length; j++) {
        if (data[i].show && data[i].id == defaultData[j][key]) list.push(defaultData[j]);
      }
    }
    return list;
  }
  onMounted(() => {
    init();
  });
</script>
