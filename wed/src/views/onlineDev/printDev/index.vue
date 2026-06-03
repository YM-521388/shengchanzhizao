<template>
  <div class="jnpf-content-wrapper">
    <div class="jnpf-content-wrapper-center">
      <div class="jnpf-content-wrapper-content">
        <BasicTable @register="registerTable">
          <template #tableTitle>
            <a-button type="primary" preIcon="icon-ym icon-ym-btn-add" @click="addOrUpdateHandle()">{{ t('common.addText') }}</a-button>
            <jnpf-upload-btn url="/api/system/printDev/Actions/Import" accept=".bp" @on-success="reload"></jnpf-upload-btn>
          </template>
          <template #bodyCell="{ column, record }">
            <template v-if="column.key === 'state'">
              <a-tag :color="record.state == 1 ? 'success' : ''">{{ record.state == 1 ? '已发布' : '未发布' }}</a-tag>
            </template>
            <template v-if="column.key === 'action'">
              <TableAction :actions="getTableActions(record)" :dropDownActions="getDropDownActions(record)" />
            </template>
          </template>
        </BasicTable>
      </div>
    </div>
    <Form @register="registerForm" @reload="reload" @design="handleDesign" />
    <PrintDesign @register="registerPrintDesign" @reload="reload" />
    <Preview @register="registerPreview" />
  </div>
</template>
<script lang="ts" setup>
  import { ref, onMounted } from 'vue';
  import { BasicTable, useTable, TableAction, BasicColumn, ActionItem } from '@/components/Table';
  import { getPrintDevList, delPrintDev, copy, exportData } from '@/api/system/printDev';
  import { useModal } from '@/components/Modal';
  import { useI18n } from '@/hooks/web/useI18n';
  import { useMessage } from '@/hooks/web/useMessage';
  import { downloadByUrl } from '@/utils/file/download';
  import { useBaseStore } from '@/store/modules/base';
  import PrintDesign from '@/components/PrintDesign/index.vue';
  import Preview from '@/components/PrintDesign/Preview.vue';
  import Form from './Form.vue';

  defineOptions({ name: 'onlineDev-printDev' });

  const { t } = useI18n();
  const baseStore = useBaseStore();
  const { createMessage } = useMessage();
  const categoryList = ref<any[]>([]);
  const columns: BasicColumn[] = [
    { title: '名称', dataIndex: 'fullName' },
    { title: '编码', dataIndex: 'enCode', width: 200 },
    { title: '分类', dataIndex: 'category', width: 150 },
    { title: '创建人', dataIndex: 'creatorUser', width: 120 },
    { title: '创建时间', dataIndex: 'creatorTime', width: 150, format: 'date|YYYY-MM-DD HH:mm:ss' },
    { title: '最后修改时间', dataIndex: 'lastModifyTime', width: 150, format: 'date|YYYY-MM-DD HH:mm:ss' },
    { title: '排序', dataIndex: 'sortCode', width: 70, align: 'center' },
    { title: '状态', dataIndex: 'state', width: 80, align: 'center' },
  ];
  const [registerTable, { reload, getForm }] = useTable({
    api: getPrintDevList,
    columns,
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
          },
        },
        {
          field: 'state',
          label: '状态',
          component: 'Select',
          componentProps: {
            placeholder: '请选择',
            options: [
              { fullName: '未发布', id: 0 },
              { fullName: '已发布', id: 1 },
            ],
          },
        },
      ],
    },
    actionColumn: {
      width: 150,
      title: '操作',
      dataIndex: 'action',
    },
  });
  const [registerForm, { openModal: openFormModal }] = useModal();
  const [registerPrintDesign, { openModal: openPrintDesign }] = useModal();
  const [registerPreview, { openModal: openPreviewModal }] = useModal();

  function getTableActions(record): ActionItem[] {
    return [
      {
        label: t('common.editText'),
        onClick: addOrUpdateHandle.bind(null, record.id),
      },
      {
        label: '设计',
        color: 'error',
        onClick: handleDesign.bind(null, record),
      },
    ];
  }
  function getDropDownActions(record): ActionItem[] {
    return [
      {
        label: t('common.previewText'),
        ifShow: !!record.state,
        onClick: handlePreview.bind(null, record.id, record.fullName),
      },
      {
        label: t('common.copyText'),
        ifShow: !!record.state,
        modelConfirm: {
          content: '您确定要复制该打印模板, 是否继续?',
          onOk: handleCopy.bind(null, record.id),
        },
      },
      {
        label: t('common.exportText'),
        ifShow: !!record.state,
        modelConfirm: {
          content: '您确定要导出该打印模板, 是否继续?',
          onOk: handleExport.bind(null, record.id),
        },
      },
      {
        label: t('common.delText'),
        color: 'error',
        modelConfirm: {
          onOk: handleDelete.bind(null, record.id),
        },
      },
    ];
  }
  function addOrUpdateHandle(id = '') {
    openFormModal(true, { id, categoryList: categoryList.value });
  }
  function handleExport(id) {
    exportData(id).then(res => {
      downloadByUrl({ url: res.data.url });
    });
  }
  function handleCopy(id) {
    copy(id).then(res => {
      createMessage.success(res.msg);
      reload();
    });
  }
  function handleDelete(id) {
    delPrintDev(id).then(res => {
      createMessage.success(res.msg);
      reload();
    });
  }
  function handleDesign(record) {
    openPrintDesign(true, record);
  }
  function handlePreview(id, fullName) {
    openPreviewModal(true, { id, fullName });
  }
  async function getOptions() {
    categoryList.value = (await baseStore.getDictionaryData('businessType')) as any[];
    getForm().updateSchema({ field: 'category', componentProps: { options: categoryList.value } });
  }

  onMounted(() => {
    getOptions();
  });
</script>
