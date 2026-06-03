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
        <BasicTable @register="registerTable" v-bind="getTableBindValue" ref="tableRef" @resizeColumn="handleResizeColumn">
          <template #toolbar> </template>
          <template #bodyCell="{ column, record }">
            <template v-if="column.dataIndex === 'alertStatus'">
              <a-tag :color="getAlertColor(record.enCode)">{{ record.alertStatus }}</a-tag>
            </template>
            <template v-if="column.key === 'action'">
              <TableAction :actions="getTableActions(record)" />
            </template>
          </template>
        </BasicTable>
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
  import { getAlertList } from './helper/api';
  import { getDictionaryDataSelector } from '@/api/systemData/dictionary';
  import { ref, reactive, onMounted, toRefs, computed, unref, nextTick } from 'vue';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useI18n } from '@/hooks/web/useI18n';
  import { useUserStore } from '@/store/modules/user';
  import { usePermission } from '@/hooks/web/usePermission';
  import { BasicForm, useForm } from '@/components/Form';
  import { BasicTable, useTable, TableActionType, SorterResult, TableAction, ActionItem } from '@/components/Table';
  import { ReloadOutlined } from '@ant-design/icons-vue';
  import { useRoute } from 'vue-router';
  import { cloneDeep } from 'lodash-es';
  import columnList from './helper/columnList';
  import searchList from './helper/searchList';
  import { noGroupList } from '@/components/FormGenerator/src/helper/config';

  const route = useRoute();
  const { createMessage } = useMessage();
  const { t } = useI18n();
  const userStore = useUserStore();
  const { hasBtnP } = usePermission();

  interface State {
    complexColumns: any[];
    childColumnList: any[];
    expandObj: any;
    exportList: any[];
    columns: any[];
    searchSchemas: any[];
    columnList: any[];
  }

  const state = reactive<State>({
    complexColumns: [], // 复杂表头
    childColumnList: [],
    expandObj: {},
    exportList: [],
    columnList: [],
    columns: [],
    searchSchemas: [],
  });
  const { searchSchemas } = toRefs(state);
  const tableRef = ref<Nullable<TableActionType>>(null);
  const defaultSearchInfo = {
    menuId: route.meta.modelId as string,
    moduleId: '2014598496543444992',
    superQueryJson: '',
  };
  const searchInfo: any = reactive({
    ...cloneDeep(defaultSearchInfo),
  });
  const [registerSearchForm, { updateSchema, resetFields, submit: searchFormSubmit, setFieldsValue }] = useForm({
    baseColProps: { span: 6 },
    showActionButtonGroup: true,
    showAdvancedButton: true,
    compact: true,
  });
  const [registerTable, { reload, setLoading, redoHeight, clearSelectedRowKeys }] = useTable({
    api: getAlertList,
    immediate: false,
    clickToRowSelect: false,
    tableSetting: { setting: true },
    afterFetch: data => {
      return data || [];
    },
  });

  // 根据告警编码获取标签颜色
  function getAlertColor(enCode) {
    if (enCode === 'LowStock') return 'red'; // 库存不足 - 红色
    if (enCode === 'OverStock') return 'orange'; // 库存过剩 - 橙色
    return 'default';
  }

  const getColumns = computed(() => {
    return state.columns;
  });
  const getSearchList = computed(() => {
    return cloneDeep(state.searchSchemas);
  });
  const getTableBindValue = computed(() => {
    const data: any = {
      pagination: { pageSize: 20 },
      searchInfo: unref(searchInfo),
      ellipsis: true,
      columns: state.columns,
      bordered: true,
      actionColumn: {
        width: 150,
        title: t('component.table.action'),
        dataIndex: 'action',
      },
    };
    return data;
  });

  function getTableActions(record): ActionItem[] {
    return [
      {
        label: t('common.detailText', '详情'),
        onClick: () => {},
        auth: 'btn_detail',
      },
    ];
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
    };
    Object.keys(searchInfo).map(key => {
      delete searchInfo[key];
    });
    for (let [key, value] of Object.entries(obj)) {
      searchInfo[key.replaceAll('-', '_')] = value;
    }
    reload({ page: 1 });
  }
  function handleRefresh() {
    reload();
  }
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
  function initColumns() {
    state.columns = columnList.map(o => ({
      ...o,
      title: o.label,
      dataIndex: o.prop,
      align: o.align,
      fixed: o.fixed == 'none' ? false : o.fixed,
      sorter: false,
      width: o.width || 100,
      resizable: true,
    }));
  }
  // 加载字典数据更新搜索选项
  async function loadDictionaryOptions() {
    try {
      const res = await getDictionaryDataSelector('2044625620159303680');
      const alertTypeSchema = state.searchSchemas.find(item => item.field === 'alertType');
      // 处理字典数据返回格式（可能是 { list: [...] } 或直接是数组）
      const dataList = res?.list || res;
      if (alertTypeSchema && alertTypeSchema.componentProps && dataList && dataList.length) {
        alertTypeSchema.componentProps.options = dataList.map(item => ({
          value: item.enCode,
          label: item.fullName,
        }));
      }
    } catch (error) {
      console.error('加载字典数据失败', error);
    }
  }

  function initSearchSchemas() {
    state.searchSchemas = searchList;
    loadDictionaryOptions();
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
      resizable: true,
    }));
    columns = getComplexColumns(columns);
    state.columns = columns.filter(o => o.prop.indexOf('-') < 0);
    getChildComplexColumns(columns);
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
  async function init() {
    searchInfo.menuId = route.meta.modelId as string;
    setLoading(true);
    state.columnList = columnList;
    initSearchSchemas();
    getColumnList();
    nextTick(() => {
      handleSearchSubmit({});
    });
  }

  onMounted(() => {
    init();
  });
</script>
