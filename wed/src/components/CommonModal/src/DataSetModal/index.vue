<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    defaultFullscreen
    :footer="null"
    :closable="false"
    :keyboard="false"
    destroyOnClose
    class="jnpf-full-modal full-modal designer-modal dataSet-modal">
    <template #title>
      <div class="jnpf-full-modal-header">
        <div class="header-title">数据集名称：<a-input v-model:value="dataForm.fullName" placeholder="请输入" class="!w-200px"></a-input></div>
        <a-steps v-model:current="dataForm.type" :initial="1" type="navigation" size="small" @change="handleStep" class="header-steps tab-steps">
          <a-step title="SQL语句" />
          <a-step title="配置式" />
          <a-step title="数据接口" />
        </a-steps>
        <a-space class="options" :size="10">
          <a-button type="primary" @click="handleSubmit()" :disabled="btnLoading">{{ t('common.okText') }}</a-button>
          <a-button @click="closeModal" :disabled="btnLoading">{{ t('common.cancelText') }}</a-button>
        </a-space>
      </div>
    </template>
    <div class="dataSet-modal-container" v-if="dataForm.type == 2">
      <span class="label">数据连接</span>
      <jnpf-select
        class="dataSet-modal-select"
        v-model:value="dataForm.dbLinkId"
        showSearch
        :options="dbOptions"
        placeholder="选择数据库"
        @change="onDbLinkChange"
        :fieldNames="{ options: 'children' }" />
    </div>
    <div class="dataSet-modal-main">
      <div class="left-pane" v-if="dataForm.type == 1">
        <jnpf-select
          class="!w-full"
          v-model:value="dataForm.dbLinkId"
          showSearch
          :options="dbOptions"
          placeholder="选择数据库"
          @change="onDbLinkChange"
          :fieldNames="{ options: 'children' }" />
        <div class="left-pane-box">
          <InputSearch class="search-box" :placeholder="t('common.enterKeyword')" allowClear v-model:value="keyword" @search="handleSearchTable" />
          <ScrollContainer ref="infiniteBody">
            <BasicTree class="tree-box remove-active-tree" ref="leftTreeRef" v-bind="getTreeBindValue" />
          </ScrollContainer>
        </div>
      </div>
      <div class="middle-pane">
        <a-tabs class="jnpf-content-wrapper-tabs">
          <template #rightExtra v-if="dataForm.type == 1">
            <a-dropdown>
              <a-button type="link">系统变量<DownOutlined /></a-button>
              <template #overlay>
                <a-menu>
                  <a-menu-item disabled>当前系统变量仅支持内部接口引用</a-menu-item>
                  <a-menu-divider />
                  <a-menu-item v-for="(item, index) in getSysVariableList" :key="index" @click="handleSysNodeClick(item.id)">
                    <span>{{ item.id }}</span>
                    <span style="float: right; color: #8492a6; padding-left: 10px">{{ item.fullName }}</span>
                  </a-menu-item>
                </a-menu>
              </template>
            </a-dropdown>
          </template>
        </a-tabs>
        <template v-if="dataForm.type == 1">
          <MonacoEditor class="h-full" ref="sqlEditorRef" language="sql" v-model="dataForm.dataConfigJson" />
        </template>
        <template v-else-if="dataForm.type == 2">
          <div class="config-tree-box">
            <div class="add-btn-box" v-if="!visualConfigJson.length">
              <a-button size="large" preIcon="icon-ym icon-ym-btn-add" @click="handleAdd()">新增数据库表</a-button>
            </div>
            <BasicTree
              class="tree-box remove-active-tree"
              ref="tableConfigTreeRef"
              :treeData="visualConfigJson"
              :fieldNames="fieldNames"
              v-model:selectedKeys="selectedKeys"
              defaultExpandAll
              v-else>
              <template #title="item">
                <div class="jnpf-tree__title" @click="handleEdit(item.table)">
                  <i class="mr-6px" :class="getIconClassName(item)" :title="getIconTitle(item)"></i>
                  {{ item.tableName }}
                  <span class="jnpf-tree__action">
                    <PlusOutlined class="ml-10px" title="添加" @click.stop="handleAdd(item.table)" v-if="getAddBtnShow(item.parentTable)" />
                    <DeleteOutlined class="ml-6px" title="删除" @click.stop="handleDelete(item.table)" />
                  </span>
                </div>
              </template>
            </BasicTree>
          </div>
        </template>
        <template v-else-if="dataForm.type == 3">
          <div class="interface-content">
            <a-form-item label="数据接口" :colon="false">
              <interface-modal
                class="interface-modal"
                :value="dataForm.interfaceId"
                :title="dataForm.treePropsName"
                popupTitle="数据接口"
                :sourceType="1"
                @change="onPropsUrlChange" />
            </a-form-item>
            <div class="interface-box">
              <a-table
                :data-source="dataForm.parameterJson"
                :columns="templateJsonColumns"
                size="small"
                :pagination="false"
                v-if="dataForm.parameterJson?.length">
                <template #bodyCell="{ column, record }">
                  <template v-if="column.key === 'field'">
                    <span class="required-sign">{{ record.required ? '*' : '' }}</span>
                    {{ record.field }}{{ record.fieldName ? '(' + record.fieldName + ')' : '' }}
                  </template>
                  <template v-if="column.key === 'sourceType'">
                    <jnpf-select
                      class="!w-100%"
                      v-model:value="record.sourceType"
                      :options="getSourceTypeOptions(record.required)"
                      @change="onSourceTypeChange(record)" />
                  </template>
                  <template v-if="column.key === 'relationField'">
                    <a-input v-if="record.sourceType == 2" v-model:value="record.relationField" placeholder="请输入" class="!w-100%" />
                    <jnpf-select v-else-if="record.sourceType == 4" class="!w-100%" v-model:value="record.relationField" :options="getSysVariableList" />
                  </template>
                </template>
              </a-table>
            </div>
          </div>
        </template>
        <div class="preview-box" v-if="dataForm.type != 1">
          <a-tabs v-model:activeKey="previewActiveKey" class="jnpf-content-wrapper-tabs">
            <a-tab-pane :key="1" tab="数据预览"></a-tab-pane>
            <a-tab-pane :key="2" tab="SQL预览" v-if="dataForm.type == 2"></a-tab-pane>
            <template #rightExtra>
              <a-button v-if="dataForm.type == 2" type="link" preIcon="icon-ym icon-ym-filter" @click="openFilterConfig">筛选设置</a-button>
              <a-button type="link" preIcon="icon-ym icon-ym-Refresh" @click="handleGetPreviewData">刷新数据</a-button>
            </template>
          </a-tabs>
          <div class="preview-content" v-loading="previewLoading" v-show="previewActiveKey === 1">
            <div class="preview-tip">预览数据仅显示20条数据</div>
            <BasicTable
              :data-source="state.previewData"
              :columns="state.previewColumns"
              size="small"
              :pagination="false"
              :showTableSetting="false"
              v-if="!!state.previewColumns.length" />
          </div>
          <div class="preview-content" v-loading="previewLoading" v-show="previewActiveKey === 2">
            <a-textarea v-model:value="state.previewSqlText" readonly class="preview-content-sql" />
            <a-tooltip placement="bottom" :title="t('common.copyText')">
              <CopyOutlined class="copy-btn" @click="handleCopySql" v-if="state.previewSqlText" />
            </a-tooltip>
          </div>
        </div>
      </div>
    </div>
    <TableConfigDrawer v-bind="getTableConfigBind" @register="registerTableConfigDrawer" @confirm="onTreeNodeConfirm" @close="onTreeNodeClose" />
    <FilterConfigDrawer :sysVariableList="getSysVariableList" @register="registerFilterConfigDrawer" @confirm="onFilterConfigConfirm" />
  </BasicModal>
</template>
<script lang="ts" setup>
  import { getDataSourceSelector } from '@/api/systemData/dataSource';
  import { getDataModelList, getDataModelFieldList } from '@/api/systemData/dataModel';
  import { getFields, getPreviewData } from '@/api/system/dataSet';
  import { reactive, toRefs, ref, unref, computed, nextTick } from 'vue';
  import { PlusOutlined, DeleteOutlined, DownOutlined, CopyOutlined } from '@ant-design/icons-vue';
  import { BasicModal, useModalInner } from '@/components/Modal';
  import { useDrawer } from '@/components/Drawer';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useI18n } from '@/hooks/web/useI18n';
  import { cloneDeep } from 'lodash-es';
  import { BasicTree, TreeActionType } from '@/components/Tree';
  import { MonacoEditor } from '@/components/CodeEditor';
  import { InputSearch } from 'ant-design-vue';
  import { buildBitUUID } from '@/utils/uuid';
  import TableConfigDrawer from './TableConfigDrawer.vue';
  import FilterConfigDrawer from './FilterConfigDrawer.vue';
  import { useCopyToClipboard } from '@/hooks/web/useCopyToClipboard';
  import { BasicTable } from '@/components/Table';
  import { InterfaceModal } from '@/components/CommonModal';
  import { ScrollContainer, ScrollActionType } from '@/components/Container';

  interface State {
    dataForm: any;
    dataSetList: any[];
    parameterJson: any[];
    fieldJson: any[];
    dbOptions: any[];
    treeLoading: boolean;
    btnLoading: boolean;
    keyword: string;
    treeData: any[];
    isEdit: boolean;
    visualConfigJson: any[];
    filterConfigJson: any;
    previewActiveKey: number;
    currentNode: string;
    previewSqlText: string;
    previewData: any[];
    previewColumns: any[];
    previewLoading: boolean;
    allOptions: any[];
    selectedKeys: any[];
    pagination: any;
    finish: boolean;
  }

  const props = defineProps({
    type: { type: String, default: '' },
  });

  const relationFieldOptions = [
    { fullName: '自定义', id: 2 },
    { fullName: '系统参数', id: 4 },
    { fullName: '为空', id: 3 },
  ];

  const templateJsonColumns = [
    { width: 50, title: '序号', align: 'center', customRender: ({ index }) => index + 1 },
    { title: '参数名称', dataIndex: 'field', key: 'field', width: 135 },
    { title: '参数来源', dataIndex: 'sourceType', key: 'sourceType', width: 220 },
    { title: '参数值', dataIndex: 'relationField', key: 'relationField', width: 220 },
  ];
  const fieldNames = { key: 'table' };
  const defaultDataForm = {
    jnpfId: '',
    id: '',
    fullName: '',
    dbLinkId: '0',
    dataConfigJson: '',
    visualConfigJson: '',
    filterConfigJson: '',
    parameterJson: '',
    fieldJson: '',
    objectType: props.type === 'print' ? 'printVersion' : '',
    type: 1,
  };
  const defaultFilterConfigJson = {
    ruleList: [],
    matchLogic: 'and',
  };
  const state = reactive<State>({
    dataForm: {
      jnpfId: '',
      id: '',
      fullName: '',
      dbLinkId: '0',
      dataConfigJson: '',
      visualConfigJson: '',
      filterConfigJson: '',
      parameterJson: '',
      fieldJson: '',
      objectType: '',
      type: 1,
      interfaceId: '',
      treePropsName: '',
      templateJson: [],
    },
    dataSetList: [],
    parameterJson: [],
    fieldJson: [],
    dbOptions: [],
    treeLoading: true,
    btnLoading: false,
    keyword: '',
    treeData: [],
    isEdit: false,
    visualConfigJson: [],
    filterConfigJson: {},
    previewActiveKey: 1,
    currentNode: '',
    previewSqlText: '',
    previewData: [],
    previewColumns: [],
    previewLoading: false,
    allOptions: [],
    selectedKeys: [],
    pagination: {
      currentPage: 1,
      pageSize: 30,
    },
    finish: false,
  });
  const { dataForm, parameterJson, dbOptions, keyword, treeData, btnLoading, visualConfigJson, previewActiveKey, previewLoading, selectedKeys } = toRefs(state);
  const { t } = useI18n();
  const { createMessage, createConfirm } = useMessage();
  const emit = defineEmits(['register', 'confirm']);
  const sqlEditorRef = ref(null);
  const leftTreeRef = ref(null);
  const tableConfigTreeRef = ref<Nullable<TreeActionType>>(null);
  const [registerModal, { closeModal, changeLoading }] = useModalInner(init);
  const [registerTableConfigDrawer, { openDrawer: openTableConfigDrawer }] = useDrawer();
  const [registerFilterConfigDrawer, { openDrawer: openFilterConfigDrawer }] = useDrawer();
  const infiniteBody = ref<Nullable<ScrollActionType>>(null);

  const getSysVariableList = computed(() => {
    let list = [
      { id: '@userId', fullName: '当前用户' },
      { id: '@userAndSubordinates', fullName: '当前用户及下属' },
      { id: '@organizeId', fullName: '当前组织' },
      { id: '@organizationAndSuborganization', fullName: '当前组织及子组织' },
      { id: '@branchManageOrganize', fullName: '当前分管组织' },
    ];
    if (props.type == 'print') list.unshift({ id: '@formId', fullName: '当前表单ID' });
    if (state.dataForm.type === 1) {
      const extraList = [
        { id: '@lotSnowID', fullName: '批量生成不同雪花ID' },
        { id: '@snowFlakeID', fullName: '系统生成雪花ID' },
      ];
      list.unshift(...extraList);
    }
    return list;
  });
  const getTreeBindValue = computed(() => {
    const key = +new Date();
    const data: any = {
      defaultExpandAll: false,
      treeData: state.treeData,
      loading: state.treeLoading,
      loadData: onLoadData,
      onSelect: handleTreeSelect,
      key,
    };
    return data;
  });
  const getTableConfigBind = computed(() => ({
    linkId: state.dataForm.dbLinkId,
    sysVariableList: unref(getSysVariableList),
    getTableConfigTree: getTableConfigTree,
  }));

  function init(data) {
    state.btnLoading = false;
    state.previewActiveKey = 1;
    handleClearPreviewData();
    state.keyword = '';
    state.isEdit = !!data.data;
    state.dataSetList = cloneDeep(data.list);
    const dataForm = data.data ? cloneDeep(data.data) : cloneDeep(defaultDataForm);
    state.dataForm = dataForm;
    if (!state.isEdit) {
      const id = 'DataSet' + buildBitUUID();
      state.dataForm.jnpfId = id;
      state.dataForm.fullName = id;
    }
    state.dataForm.parameterJson = dataForm.parameterJson ? JSON.parse(dataForm.parameterJson) : [];
    state.fieldJson = dataForm.fieldJson ? JSON.parse(dataForm.fieldJson) : [];
    state.visualConfigJson = dataForm.visualConfigJson ? JSON.parse(dataForm.visualConfigJson) : [];
    state.filterConfigJson = dataForm.filterConfigJson ? JSON.parse(dataForm.filterConfigJson) : cloneDeep(defaultFilterConfigJson);
    changeLoading(true);
    getDataSourceSelector().then(res => {
      let list = res.data.list || [];
      list = list.filter(o => o.children && o.children.length);
      if (list[0]?.children?.length) list[0] = list[0].children[0];
      delete list[0].children;
      state.dbOptions = list;
      changeLoading(false);
      state.finish = false;
      state.treeData = [];
      state.pagination.currentPage = 1;
      getTableList();
      nextTick(() => {
        bindScroll();
      });
    });
  }
  function getSourceTypeOptions(isRequired) {
    return isRequired ? relationFieldOptions.filter(o => o.id != 3) : relationFieldOptions;
  }
  function bindScroll() {
    const bodyRef = infiniteBody.value;
    const vBody = bodyRef?.getScrollWrap();
    vBody?.addEventListener('scroll', () => {
      if (vBody.scrollHeight - vBody.clientHeight - vBody.scrollTop <= 200 && !state.treeLoading && !state.finish) {
        state.pagination.currentPage += 1;
        getTableList();
      }
    });
  }
  function onPropsUrlChange(val, row) {
    if (!val) {
      state.dataForm.interfaceId = '';
      state.dataForm.treePropsName = '';
      state.dataForm.parameterJson = [];
      return;
    }
    if (state.dataForm.interfaceId === val) return;
    state.dataForm.interfaceId = val;
    state.dataForm.treePropsName = row.fullName;
    state.dataForm.parameterJson = row.parameterJson ? JSON.parse(row.parameterJson).map(o => ({ ...o, relationField: '', sourceType: 2 })) : [];
  }
  function onSourceTypeChange(record) {
    record.relationField = record.sourceType == 4 ? unref(getSysVariableList)[0]?.id : '';
  }
  function onDbLinkChange() {
    treeData.value = [];
    state.pagination.currentPage = 1;
    state.finish = false;
    getTableList(true);
  }
  function getTableList(isClean = false) {
    state.treeLoading = true;
    const query = {
      linkId: state.dataForm.dbLinkId,
      keyword: state.keyword,
      pageSize: state.pagination.pageSize,
      currentPage: state.pagination.currentPage,
    };
    getDataModelList(query)
      .then(res => {
        state.treeLoading = false;
        if (res.data.list.length < state.pagination.pageSize) state.finish = true;
        state.treeData = [...state.treeData, ...res.data.list];
        state.treeData = state.treeData.map(o => ({
          ...o,
          fullName: o.tableName ? o.table + '(' + o.tableName + ')' : o.table,
          isLeaf: false,
          id: o.table,
          icon: o.type == 1 ? 'icon-ym icon-ym-view' : 'icon-ym icon-ym-generator-tableGrid',
        }));
        if (isClean) {
          state.dataForm.dataConfigJson = '';
          state.visualConfigJson = [];
          state.filterConfigJson = cloneDeep(defaultFilterConfigJson);
          handleClearPreviewData();
        }
      })
      .catch(() => {
        state.treeLoading = false;
        state.treeData = [];
      });
  }
  function onLoadData(node) {
    return new Promise((resolve: (value?: unknown) => void) => {
      getDataModelFieldList(state.dataForm.dbLinkId, node.dataRef.table).then(res => {
        const data = res.data.list.map(o => ({
          ...o,
          isLeaf: true,
          fullName: o.fieldName ? o.field + '(' + o.fieldName + ')' : o.field,
          id: node.dataRef.table + '-' + o.field,
        }));
        getTree().updateNodeByKey(node.eventKey, { children: data, isLeaf: !data.length });
        resolve();
      });
    });
  }
  function handleSearchTable() {
    state.pagination.currentPage = 1;
    getTableList();
    treeData.value = [];
    handleClearPreviewData();
  }
  function handleSysNodeClick(data) {
    (sqlEditorRef.value as any)?.insert(data);
  }
  function handleItemClick(item) {
    item.field && (sqlEditorRef.value as any)?.insert('{' + item.field + '}');
  }
  function handleTreeSelect(keys) {
    const selectedNode: any = getTree()?.getSelectedNode(keys[0]);
    const content = selectedNode.isLeaf ? selectedNode.field : selectedNode.table;
    (sqlEditorRef.value as any)?.insert(content);
  }
  function getTree() {
    const tree = unref(leftTreeRef);
    if (!tree) {
      throw new Error('tree is null!');
    }
    return tree as any;
  }
  // 更新字段
  function updateFieldList(data) {
    state.btnLoading = true;
    changeLoading(true);
    getFields(data)
      .then(res => {
        data.children = res.data.map(o => ({ ...o, jnpfId: data.fullName + '.' + o.id }));
        changeLoading(false);
        state.btnLoading = false;
        updateDataSetList(data);
      })
      .catch(() => {
        changeLoading(false);
        state.btnLoading = false;
      });
  }
  function getTableConfigTree() {
    const tree = unref(tableConfigTreeRef);
    if (!tree) {
      throw new Error('tree is null!');
    }
    return tree;
  }
  // 删除数据库表
  function handleDelete(table) {
    createConfirm({
      iconType: 'warning',
      title: t('common.tipTitle'),
      content: `确定删除数据库表?`,
      onOk: () => {
        getTableConfigTree().deleteNodeByKey(table);
        handleClearPreviewData();
      },
    });
  }
  function getAddBtnShow(parentTable) {
    if (!parentTable) return true;
    return state.visualConfigJson.some(o => o.table === parentTable);
  }
  function getIconClassName(item) {
    if (!item.parentTable) return 'icon-ym icon-ym-generator-tableGrid';
    if (item.relationConfig.type === 1) return 'icon-ym icon-ym-left-join';
    if (item.relationConfig.type === 2) return 'icon-ym icon-ym-right-join';
    if (item.relationConfig.type === 3) return 'icon-ym icon-ym-inner-join';
    if (item.relationConfig.type === 4) return 'icon-ym icon-ym-full-join';
    return 'icon-ym icon-ym-generator-tableGrid';
  }
  function getIconTitle(item) {
    if (!item.parentTable) return '';
    if (item.relationConfig.type === 1) return '左连接';
    if (item.relationConfig.type === 2) return '右连接';
    if (item.relationConfig.type === 3) return '内连接';
    if (item.relationConfig.type === 4) return '全连接';
    return '';
  }
  // 新增数据库表
  function handleAdd(table = '') {
    state.currentNode = table;
    if (state.currentNode) {
      const node: any = getTableConfigTree().getSelectedNode(state.currentNode);
      if (node?.children?.length >= 2) {
        createMessage.warning('最多只能添加两个子表');
        return;
      }
    }
    openTableConfigDrawer(true, { parentTable: table });
  }
  function handleEdit(table) {
    state.currentNode = table;
    const node: any = getTableConfigTree().getSelectedNode(state.currentNode);
    openTableConfigDrawer(true, { parentTable: node.parentTable, data: node });
  }
  function onTreeNodeClose() {
    handleClearPreviewData();
  }
  function onTreeNodeConfirm(data, isEdit, isDelChild) {
    if (!isEdit) {
      if (!state.currentNode) return state.visualConfigJson.push(data);
      const node = getTableConfigTree().getSelectedNode(state.currentNode);
      node?.children?.push(data);
      handleClearPreviewData();
      return;
    }
    if (isDelChild) data.children = [];
    getTableConfigTree().updateNodeByKey(state.currentNode, data);
    handleClearPreviewData();
  }
  function openFilterConfig() {
    if (!state.visualConfigJson.length) return createMessage.error('请先配置数据库表');
    openFilterConfigDrawer(true, { data: state.filterConfigJson, visualConfigJson: state.visualConfigJson });
  }
  function onFilterConfigConfirm(data) {
    state.filterConfigJson = data;
    handleClearPreviewData();
  }
  // 获取预览数据
  function handleGetPreviewData() {
    if (!state.visualConfigJson.length && state.dataForm.type == 1) return createMessage.error('请先配置数据库表');
    if (!state.dataForm.interfaceId && state.dataForm.type == 3) return createMessage.error('请选择数据接口');
    state.previewLoading = true;
    const data = {
      ...state.dataForm,
      parameterJson: state.dataForm.parameterJson.length ? JSON.stringify(state.dataForm.parameterJson) : '',
      fieldJson: state.fieldJson.length ? JSON.stringify(state.fieldJson) : '',
      visualConfigJson: state.visualConfigJson.length ? JSON.stringify(state.visualConfigJson) : '',
      filterConfigJson: state.filterConfigJson ? JSON.stringify(state.filterConfigJson) : '',
    };
    getPreviewData(data)
      .then(res => {
        state.previewData = res.data.previewData;
        let previewColumns = res.data.previewColumns.map(o => ({ title: o.title, dataIndex: o.title, key: o.title }));
        state.previewColumns = [...previewColumns];
        state.previewSqlText = res.data.previewSqlText;
        state.previewLoading = false;
      })
      .catch(() => {
        handleClearPreviewData();
      });
  }
  // 清空预览数据
  function handleClearPreviewData() {
    state.previewLoading = false;
    state.previewData = [];
    state.previewColumns = [];
    state.previewSqlText = '';
    state.dataForm.interfaceId = '';
    state.dataForm.propsName = '';
    state.dataForm.parameterJson = [];
    state.selectedKeys = [];
  }
  function handleStep() {
    treeData.value = [];
    state.visualConfigJson = [];
    state.pagination.currentPage = 1;
    state.finish = false;
    handleClearPreviewData();
    if (state.dataForm.type == 1) {
      getTableList();
      nextTick(() => {
        bindScroll();
      });
    }
  }
  function handleCopySql() {
    if (!state.previewSqlText) return;
    const { isSuccessRef } = useCopyToClipboard(state.previewSqlText);
    unref(isSuccessRef) && createMessage.success('复制成功');
  }
  // 更新数据集列表
  function updateDataSetList(data) {
    if (!state.isEdit) {
      state.dataSetList.push(data);
    } else {
      for (let i = 0; i < state.dataSetList.length; i++) {
        if (state.dataSetList[i].jnpfId === data.jnpfId) {
          state.dataSetList[i] = data;
          break;
        }
      }
    }
    emit('confirm', state.dataSetList);
    closeModal();
  }
  function handleSubmit() {
    if (!state.dataForm.fullName) {
      createMessage.error('请输入数据集名称');
      return;
    }
    const reg = /(^_([A-Za-z0-9]_?)*$)|(^[A-Za-z](_?[A-Za-z0-9])*_?$)/;
    if (!reg.test(state.dataForm.fullName)) {
      createMessage.error('数据集名称只能包含字母、数字、下划线，并且以字母开头');
      return;
    }
    let boo = false;
    for (let i = 0; i < state.dataSetList.length; i++) {
      if (state.dataForm.jnpfId !== state.dataSetList[i].jnpfId && state.dataForm.fullName === state.dataSetList[i].fullName) {
        boo = true;
        break;
      }
    }
    if (boo) return createMessage.error('数据集名称已存在');
    if (state.dataForm.type === 1 && !state.dataForm.dataConfigJson) return createMessage.error('请输入SQL语句');
    if (state.dataForm.type === 2 && !state.visualConfigJson.length) return createMessage.error('请配置数据库表');
    if (!state.dataForm.interfaceId && state.dataForm.type == 3) return createMessage.error('请选择数据接口');
    if (state.dataForm.type == 3 && !exist()) return;
    const data = {
      ...state.dataForm,
      parameterJson: state.dataForm.parameterJson.length ? JSON.stringify(state.dataForm.parameterJson) : '',
      fieldJson: state.fieldJson.length ? JSON.stringify(state.fieldJson) : '',
      visualConfigJson: state.visualConfigJson.length ? JSON.stringify(state.visualConfigJson) : '',
      filterConfigJson: state.filterConfigJson ? JSON.stringify(state.filterConfigJson) : '',
    };
    updateFieldList(data);
  }
  function exist() {
    let isOk = true;
    for (let i = 0; i < state.dataForm.parameterJson?.length; i++) {
      const e = state.dataForm.parameterJson[i];
      if (e.sourceType == 2 && !e.relationField) {
        createMessage.error(`参数${e.field}的参数值不能为空`);
        isOk = false;
        break;
      }
      if (!!e.required && e.sourceType == 2 && !e.relationField) {
        createMessage.error(`参数${e.field}的参数值不能为空`);
        isOk = false;
        break;
      }
    }
    return isOk;
  }
</script>
<style lang="less">
  .dataSet-modal {
    .dataSet-modal-container {
      display: flex;
      align-items: center;
      margin-bottom: 8px;
      border-radius: 4px;
      width: 100%;
      height: 50px;
      border: 1px solid @border-color-base;
      background-color: @component-background;
      .label {
        margin-left: 10px;
        font-size: 14px;
        line-height: 22px;
        width: 70px;
      }
      .dataSet-modal-select {
        width: 400px;
        border-radius: 3px;
        margin: 8px 0;
      }
    }

    .dataSet-modal-main {
      height: 100%;
      overflow: hidden;
      display: flex;
      justify-content: space-between;
      overflow: hidden;
      .left-pane {
        flex-shrink: 0;
        width: 350px;
        margin-right: 10px;
        .left-pane-box {
          margin-top: 8px;
          border-radius: 4px;
          height: calc(100% - 40px);
          border: 1px solid @border-color-base;
          background-color: @component-background;
          overflow: hidden;
          .search-box {
            padding: 10px;
          }
          & > .scroll-container {
            height: calc(100% - 52px) !important;
          }
          .tree-box {
            overflow: auto;
            overflow-x: hidden;
          }
        }
      }
      .middle-pane {
        background-color: @component-background;
        border: 1px solid @border-color-base;
        border-radius: 4px;
        flex: 1;
        display: flex;
        flex-direction: column;
        overflow: hidden;
        .system-text {
          text-align: end;
        }
        .title {
          border-top: 1px solid @border-color-base;
        }
        .title-box {
          height: 36px;
          line-height: 36px;
          display: flex;
          justify-content: space-between;
          color: @text-color-label;
          font-size: 14px;
          padding: 0 10px;
          flex-shrink: 0;
          border-bottom: 1px solid @border-color-base;
        }
        .tabs-box {
          overflow: unset;
          :deep(.ant-tabs-tab:first-child) {
            margin-left: 20px;
          }
        }
        .table-actions {
          flex-shrink: 0;
          border-top: 1px dashed @border-color-base;
          text-align: center;
        }
        .top-box {
          display: flex;
          .main-box {
            flex: 1;
            margin-right: 18px;
          }
        }
        .jnpf-content-wrapper-tabs {
          flex-shrink: 0;
          .ant-btn-link {
            padding-left: 0;
            padding-right: 0;
            margin-left: 20px;
          }
        }
        .config-tree-box {
          flex: 1;
          min-height: 200px;
          .jnpf-tree .ant-tree .ant-tree-node-content-wrapper,
          .jnpf-tree .ant-tree .ant-tree-switcher,
          .jnpf-tree__title {
            line-height: 50px;
            height: 50px;
          }
          .add-btn-box {
            height: 100%;
            display: flex;
            align-items: center;
            justify-content: center;
          }
        }
        .preview-box {
          flex-shrink: 0;
          border-top: 1px solid @border-color-base1;
          .preview-content {
            position: relative;
            height: 380px;
            .preview-tip {
              color: @text-color-label;
              text-align: right;
              line-height: 40px;
              padding: 0 10px;
            }
            .copy-btn {
              position: absolute;
              right: 15px;
              top: 15px;
              color: @text-color-label;
              cursor: pointer;
              font-size: 18px;
            }
            .preview-content-sql {
              height: 100%;
              border-radius: 0;
              border: unset !important;
              box-shadow: unset !important;
              padding: 10px 20px;
            }
          }
        }
        .interface-content {
          padding: 20px;
          .ant-select.ant-select-in-form-item {
            width: 400px;
          }
          .interface-box {
            flex: 1;
            min-height: 310px;
          }
        }
      }
      .right-pane {
        width: 350px;
        flex-shrink: 0;
        display: flex;
        flex-direction: column;
        height: calc(100% + 9px);
        overflow: hidden;
        margin-left: 10px;
        .right-pane-btn {
          flex-shrink: 0;
        }
        .field-table-box {
          background-color: @component-background;
        }
      }
    }
  }
</style>
