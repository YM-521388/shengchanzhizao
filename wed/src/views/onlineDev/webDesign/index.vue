<template>
  <div class="jnpf-content-wrapper">
    <div class="jnpf-content-wrapper-center">
      <div class="jnpf-content-wrapper-content">
        <BasicTable @register="registerTable">
          <template #tableTitle>
            <a-dropdown>
              <template #overlay>
                <a-menu @click="handleAdd">
                  <a-menu-item :key="item.id" v-for="item in webTypeOptions">{{ item.fullName }}</a-menu-item>
                </a-menu>
              </template>
              <a-button type="primary" preIcon="icon-ym icon-ym-btn-add">{{ t('common.addText') }}<DownOutlined /></a-button>
            </a-dropdown>
            <a-button type="primary" preIcon="icon-ym icon-ym-ai-form" @click="handleAiCreate">AI建表</a-button>
            <jnpf-upload-btn url="/api/visualdev/OnlineDev/Actions/Import" accept=".vdd" @on-success="reload"></jnpf-upload-btn>
          </template>
          <template #bodyCell="{ column, record }">
            <template v-if="column.key === 'isRelease'">
              <a-tag :color="record.isRelease == 1 ? 'success' : record.isRelease == 2 ? 'warning' : ''">
                {{ record.isRelease == 1 ? '已发布' : record.isRelease == 2 ? '已修改' : '未发布' }}
              </a-tag>
            </template>
            <template v-if="column.key === 'action'">
              <TableAction :actions="getTableActions(record)" :dropDownActions="getDropDownActions(record)" />
            </template>
          </template>
        </BasicTable>
      </div>
    </div>
    <Form @register="registerForm" @reload="reload" />
    <ViewForm @register="registerViewForm" @reload="reload" />
    <CreateMenuModal @register="registerCreateMenuModal" @reload="reload" />
    <ShortLinkModal @register="registerShortLink" @reload="reload" />
    <DownloadModal @register="registerDownloadModal" />
    <PreviewModal @register="registerPreview" />
    <AiCreateModal @register="registerAiCreateModal" @reload="handleAiReload" />
  </div>
</template>
<script lang="ts" setup>
  import { reactive, onMounted, ref } from 'vue';
  import { getVisualDevList, delVisualDev, copy, exportData, release } from '@/api/onlineDev/visualDev';
  import { BasicTable, useTable, TableAction, BasicColumn, ActionItem } from '@/components/Table';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useI18n } from '@/hooks/web/useI18n';
  import { useModal } from '@/components/Modal';
  import { useBaseStore } from '@/store/modules/base';
  import Form from './Form.vue';
  import ViewForm from './ViewForm.vue';
  import CreateMenuModal from './components/CreateMenuModal.vue';
  import ShortLinkModal from './components/ShortLinkModal.vue';
  import DownloadModal from './components/DownloadModal.vue';
  import AiCreateModal from './components/AiCreateModal.vue';
  import { downloadByUrl } from '@/utils/file/download';
  import { PreviewModal } from '@/components/CommonModal';
  import { DownOutlined } from '@ant-design/icons-vue';
  import { useGo } from '@/hooks/web/usePage';

  defineOptions({ name: 'onlineDev-webDesign' });

  const { createMessage } = useMessage();
  const baseStore = useBaseStore();
  const { t } = useI18n();
  const [registerForm, { openModal: openFormModal }] = useModal();
  const [registerViewForm, { openModal: openViewFormModal }] = useModal();
  const [registerCreateMenuModal, { openModal: openCreateMenuModal }] = useModal();
  const [registerShortLink, { openModal: openShortLinkModal }] = useModal();
  const [registerPreview, { openModal: openPreviewModal }] = useModal();
  const [registerDownloadModal, { openModal: openDownloadModal }] = useModal();
  const [registerAiCreateModal, { openModal: openAiCreateModal }] = useModal();

  const webTypeOptions: any[] = [
    { fullName: '表单', id: 2 },
    { fullName: '视图', id: 4 },
  ];
  const columns: BasicColumn[] = [
    { title: '名称', dataIndex: 'fullName', width: 200 },
    { title: '编码', dataIndex: 'enCode', width: 200 },
    { title: '分类', dataIndex: 'category', width: 150 },
    { title: '类型', dataIndex: 'webType', width: 100, align: 'center', customRender: ({ record }) => (record.webType == 4 ? '视图' : '表单') },
    { title: '创建人', dataIndex: 'creatorUser', width: 120 },
    { title: '创建时间', dataIndex: 'creatorTime', width: 150, format: 'date|YYYY-MM-DD HH:mm:ss' },
    { title: '最后修改时间', dataIndex: 'lastModifyTime', width: 150, format: 'date|YYYY-MM-DD HH:mm:ss' },
    { title: '排序', dataIndex: 'sortCode', width: 70, align: 'center' },
    { title: '状态', dataIndex: 'isRelease', width: 80, align: 'center' },
  ];
  const categoryList = ref<any[]>([]);
  const searchInfo = reactive({
    type: 1,
  });
  const go = useGo();
  const [registerTable, { reload, getForm }] = useTable({
    api: getVisualDevList,
    columns,
    searchInfo,
    useSearchForm: true,
    formConfig: {
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
        {
          field: 'category',
          label: '分类',
          component: 'Select',
          componentProps: {
            placeholder: '请选择',
            showSearch: true,
          },
        },
        {
          field: 'webType',
          label: '类型',
          component: 'Select',
          componentProps: {
            placeholder: '请选择',
            options: webTypeOptions,
          },
        },
        {
          field: 'isRelease',
          label: '状态',
          component: 'Select',
          componentProps: {
            placeholder: '请选择',
            options: [
              { fullName: '未发布', id: 0 },
              { fullName: '已发布', id: 1 },
              { fullName: '已修改', id: 2 },
            ],
          },
        },
      ],
    },
    actionColumn: {
      width: 220,
      title: '操作',
      dataIndex: 'action',
    },
  });
  function getTableActions(record): ActionItem[] {
    return [
      {
        label: t('common.editText'),
        onClick: addOrUpdateHandle.bind(null, record.id, record.webType),
      },
      {
        label: '发布',
        color: 'error',
        modelConfirm: {
          title: '发布确认',
          content: '发布表单会覆盖当前线上版本, 是否继续?',
          onOk: handleRelease.bind(null, record.id),
        },
      },
      {
        label: '代码生成',
        disabled: !record.isRelease,
        onClick: handleDownload.bind(null, record),
      },
    ];
  }
  function getDropDownActions(record): ActionItem[] {
    return [
      {
        label: '预览表单',
        onClick: handlePreview.bind(null, record.id, record.isRelease),
      },
      {
        label: '生成菜单',
        ifShow: !!record.isRelease,
        onClick: handleCreateMenu.bind(null, record.id),
      },
      {
        label: '复制表单',
        modelConfirm: {
          content: '您确定要复制该功能表单, 是否继续?',
          onOk: handleCopy.bind(null, record.id),
        },
      },
      {
        label: '导出表单',
        modelConfirm: {
          content: '您确定要导出该功能表单, 是否继续?',
          onOk: handleExport.bind(null, record.id),
        },
      },
      {
        label: '外链设置',
        ifShow: !!record.isRelease && record.webType != 4,
        onClick: openShortLink.bind(null, record.id),
      },
      {
        label: '数据管理',
        ifShow: !!record.isRelease && record.webType == 2,
        onClick: openFormData.bind(null, record.id),
      },
      {
        label: '删除表单',
        modelConfirm: {
          onOk: handleDelete.bind(null, record.id),
        },
      },
    ];
  }
  function addOrUpdateHandle(id = '', webType) {
    if (webType == 4) return openViewFormModal(true, { id, ...searchInfo, categoryList });
    openFormModal(true, { id, ...searchInfo, categoryList });
  }
  function handleDelete(id) {
    delVisualDev(id).then(res => {
      createMessage.success(res.msg);
      reload();
    });
  }
  function handleDownload(record) {
    openDownloadModal(true, { tables: record.tables, id: record.id, hasPackage: record.hasPackage, fullName: record.fullName, webType: record.webType });
  }
  // 发布
  function handleRelease(id) {
    release(id).then(res => {
      createMessage.success(res.msg);
      reload();
    });
  }
  // 生成菜单
  function handleCreateMenu(id) {
    openCreateMenuModal(true, { id });
  }
  function handlePreview(id, isRelease) {
    openPreviewModal(true, { type: 'webDesign', id, isRelease });
  }
  function openShortLink(id) {
    openShortLinkModal(true, { id });
  }
  function openFormData(id) {
    if (!id) return;
    go(`/dataManage?isDataManage=1&id=${id}`);
  }
  function handleCopy(id) {
    copy(id).then(res => {
      createMessage.success(res.msg);
      reload();
    });
  }
  function handleExport(id) {
    exportData(id).then(res => {
      downloadByUrl({ url: res.data.url });
    });
  }
  async function getOptions() {
    const res = await baseStore.getDictionaryData('businessType');
    categoryList.value = res as any[];
    getForm().updateSchema({ field: 'category', componentProps: { options: res } });
  }
  function handleAdd({ key }) {
    addOrUpdateHandle('', key);
  }
  function handleAiCreate() {
    openAiCreateModal(true, { categoryList });
  }
  function handleAiReload(id) {
    addOrUpdateHandle(id, 2);
    reload();
  }

  onMounted(() => {
    getOptions();
  });
</script>
