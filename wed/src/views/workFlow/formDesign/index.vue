<template>
  <div class="jnpf-content-wrapper">
    <div class="jnpf-content-wrapper-center">
      <div class="jnpf-content-wrapper-content">
        <BasicTable @register="registerTable">
          <template #headerTop>
            <a-alert message="系统表单用于代码生成或自主编码的流程表单导入到系统！" type="warning" show-icon />
          </template>
          <template #tableTitle>
            <a-button type="primary" preIcon="icon-ym icon-ym-btn-add" @click="addOrUpdateHandle()">{{ t('common.addText') }}</a-button>
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
    <PreviewModal @register="registerPreview" @previewPc="previewPc" />
    <Preview @register="registerPreviewPopup" />
  </div>
</template>
<script lang="ts" setup>
  import { ref, reactive } from 'vue';
  import { getVisualDevList, delVisualDev, copy, exportData, release } from '@/api/onlineDev/visualDev';
  import { BasicTable, useTable, TableAction, BasicColumn, ActionItem } from '@/components/Table';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useI18n } from '@/hooks/web/useI18n';
  import { useModal } from '@/components/Modal';
  import { usePopup } from '@/components/Popup';
  import Form from './Form.vue';
  import Preview from './Preview.vue';
  import { PreviewModal } from '@/components/CommonModal';
  import { downloadByUrl } from '@/utils/file/download';

  defineOptions({ name: 'formDesign' });

  const { createMessage } = useMessage();
  const { t } = useI18n();
  const [registerForm, { openModal: openFormModal }] = useModal();
  const [registerPreview, { openModal: openPreviewModal }] = useModal();
  const [registerPreviewPopup, { openPopup: openPreviewPopup }] = usePopup();
  const currRow = ref<any>({});

  const columns: BasicColumn[] = [
    { title: '表单名称', dataIndex: 'fullName', width: 200 },
    { title: '表单编码', dataIndex: 'enCode', width: 200 },
    { title: '表单类型', dataIndex: 'type', width: 100, align: 'center', customRender: () => '系统导入' },
    { title: '创建人', dataIndex: 'creatorUser', width: 120 },
    { title: '创建时间', dataIndex: 'creatorTime', width: 150, format: 'date|YYYY-MM-DD HH:mm:ss' },
    { title: '最后修改时间', dataIndex: 'lastModifyTime', width: 150, format: 'date|YYYY-MM-DD HH:mm:ss' },
    { title: '排序', dataIndex: 'sortCode', width: 70, align: 'center' },
    { title: '状态', dataIndex: 'isRelease', width: 80, align: 'center' },
  ];
  const searchInfo = reactive({
    type: 2,
  });
  const [registerTable, { reload }] = useTable({
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
      width: 150,
      title: '操作',
      dataIndex: 'action',
    },
  });

  function getTableActions(record): ActionItem[] {
    return [
      {
        label: t('common.editText'),
        onClick: addOrUpdateHandle.bind(null, record.id),
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
    ];
  }
  function getDropDownActions(record): ActionItem[] {
    return [
      {
        label: '预览表单',
        onClick: handlePreview.bind(null, record),
      },
      {
        label: '复制表单',
        modelConfirm: {
          content: '您确定要复制该表单, 是否继续?',
          onOk: handleCopy.bind(null, record.id),
        },
      },
      {
        label: '导出表单',
        modelConfirm: {
          content: '您确定要导出该表单, 是否继续?',
          onOk: handleExport.bind(null, record.id),
        },
      },
      {
        label: '删除表单',
        modelConfirm: {
          onOk: handleDelete.bind(null, record.id),
        },
      },
    ];
  }
  function addOrUpdateHandle(id = '') {
    openFormModal(true, { id, ...searchInfo });
  }
  function handleDelete(id) {
    delVisualDev(id).then(res => {
      createMessage.success(res.msg);
      reload();
    });
  }
  function handleRelease(id) {
    release(id).then(res => {
      createMessage.success(res.msg);
      reload();
    });
  }
  function handlePreview(record) {
    currRow.value = record;
    openPreviewModal(true, { type: 'flow', id: record.id, isRelease: record.isRelease });
  }
  function previewPc(query) {
    const data = {
      fullName: currRow.value.fullName,
      enCode: currRow.value.enCode,
      modelId: currRow.value.id,
      previewType: query.previewType,
    };
    openPreviewPopup(true, data);
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
</script>
