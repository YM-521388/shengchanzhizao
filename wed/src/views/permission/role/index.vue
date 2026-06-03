<template>
  <div class="jnpf-content-wrapper">
    <div class="jnpf-content-wrapper-center">
      <div class="jnpf-content-wrapper-content">
        <BasicTable @register="registerTable">
          <template #tableTitle>
            <a-button type="primary" preIcon="icon-ym icon-ym-btn-add" @click="addOrUpdateHandle()">{{ t('common.addText') }}</a-button>
            <a-button type="link" @click="handleExport"><i class="icon-ym icon-ym-btn-download button-preIcon"></i>{{ t('common.exportText') }}</a-button>
            <a-button type="link" @click="handleImport"><i class="icon-ym icon-ym-btn-upload button-preIcon"></i>{{ t('common.importText') }}</a-button>
          </template>
          <template #bodyCell="{ column, record }">
            <template v-if="column.key === 'enabledMark'">
              <a-tag :color="record.enabledMark == 1 ? 'success' : 'error'">{{ record.enabledMark == 1 ? '启用' : '禁用' }}</a-tag>
            </template>
            <template v-if="column.key === 'action'">
              <TableAction :actions="getTableActions(record)" :dropDownActions="getDropDownActions(record)" />
            </template>
          </template>
        </BasicTable>
      </div>
    </div>
    <Form @register="registerForm" @reload="reload" />
    <Member @register="registerMember" />
    <GlobalMember @register="registerGlobalMember" />
    <PermissionPopup @register="registerPermission" />
    <ExportModal @register="registerExportModal" />
    <ImportModal @register="registerImportModal" @reload="reload" />
  </div>
</template>
<script lang="ts" setup>
  import { getRoleList, delRole } from '@/api/permission/role';
  import { BasicTable, useTable, TableAction, BasicColumn, FormProps, ActionItem } from '@/components/Table';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useI18n } from '@/hooks/web/useI18n';
  import { useModal } from '@/components/Modal';
  import { usePopup } from '@/components/Popup';
  import { useOrganizeStore } from '@/store/modules/organize';
  import Form from './Form.vue';
  import Member from './Member.vue';
  import GlobalMember from './GlobalMember.vue';
  import PermissionPopup from '../organize/Permission.vue';
  import ImportModal from '@/views/permission/common/components/ImportModal.vue';
  import ExportModal from '@/views/permission/common/components/ExportModal.vue';

  defineOptions({ name: 'permission-role' });

  const { createMessage } = useMessage();
  const { t } = useI18n();
  const organizeStore = useOrganizeStore();
  const [registerMember, { openModal: openMemberModal }] = useModal();
  const [registerGlobalMember, { openModal: openGlobalMemberModal }] = useModal();
  const [registerForm, { openModal: openFormModal }] = useModal();
  const [registerPermission, { openPopup: openPermissionPopup }] = usePopup();
  const [registerImportModal, { openModal: openImportModal }] = useModal();
  const [registerExportModal, { openModal: openExportModal }] = useModal();

  const columns: BasicColumn[] = [
    { title: '角色名称', dataIndex: 'fullName', width: 200 },
    { title: '角色编码', dataIndex: 'enCode', width: 150 },
    { title: '角色类型', dataIndex: 'type', width: 90, align: 'center' },
    { title: '所属组织', dataIndex: 'organizeInfo' },
    { title: '创建时间', dataIndex: 'creatorTime', width: 150, format: 'date|YYYY-MM-DD HH:mm:ss' },
    { title: '排序', dataIndex: 'sortCode', width: 70, align: 'center' },
    { title: '状态', dataIndex: 'enabledMark', width: 70, align: 'center' },
  ];
  const [registerTable, { reload, getFetchParams }] = useTable({
    api: getRoleList,
    beforeFetch: data => {
      data.organizeId = data.organizeTree?.length ? data.organizeTree[data.organizeTree.length - 1] : '';
      delete data.organizeTree;
      return data;
    },
    columns,
    useSearchForm: true,
    formConfig: getFormConfig(),
    actionColumn: {
      width: 150,
      title: '操作',
      dataIndex: 'action',
    },
  });

  function getFormConfig(): Partial<FormProps> {
    return {
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
          field: 'organizeTree',
          label: '所属组织',
          component: 'OrganizeSelect',
          componentProps: {
            placeholder: '请选择',
            auth: true,
          },
        },
        {
          field: 'type',
          label: '角色类型',
          component: 'Select',
          componentProps: {
            placeholder: '请选择',
            options: [
              { fullName: '全局', id: 1 },
              { fullName: '组织', id: 0 },
            ],
          },
        },
        {
          field: 'enabledMark',
          label: '状态',
          component: 'Select',
          componentProps: {
            placeholder: '请选择',
            options: [
              { fullName: '启用', id: 1 },
              { fullName: '禁用', id: 0 },
            ],
          },
        },
      ],
    };
  }
  function getTableActions(record): ActionItem[] {
    return [
      {
        label: t('common.editText'),
        onClick: addOrUpdateHandle.bind(null, record.id),
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
  function getDropDownActions(record): ActionItem[] {
    if (!record.enabledMark) return [];
    return [
      {
        label: '角色成员',
        onClick: viewMember.bind(null, record),
      },
      {
        label: '查看权限',
        onClick: handlePermission.bind(null, record.id, record.fullName),
      },
    ];
  }
  function handlePermission(id, fullName) {
    openPermissionPopup(true, { id, fullName, objectType: 'role' });
  }
  function addOrUpdateHandle(id = '') {
    openFormModal(true, { id });
  }
  function handleDelete(id) {
    delRole(id).then(res => {
      createMessage.success(res.msg);
      organizeStore.resetState();
      reload();
    });
  }
  function viewMember(record) {
    const { id, fullName, type } = record;
    type === '全局' ? openGlobalMemberModal(true, { id, fullName, type: 'Role' }) : openMemberModal(true, { id, fullName });
  }
  function handleImport() {
    openImportModal(true, { type: 'role' });
  }
  function handleExport() {
    const listQuery = {
      ...getFetchParams(),
      organizeId: getFetchParams().organizeId || '',
      keyword: getFetchParams().keyword || '',
    };
    openExportModal(true, { listQuery, type: 'role' });
  }
</script>
