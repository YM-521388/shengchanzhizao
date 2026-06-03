<template>
  <div class="jnpf-content-wrapper">
    <div class="jnpf-content-wrapper-center">
      <div class="jnpf-content-wrapper-content">
        <BasicTable @register="registerTable">
          <template #tableTitle>
            <a-dropdown>
              <template #overlay>
                <a-menu @click="handleAdd">
                  <a-menu-item :key="0">标准流程</a-menu-item>
                  <a-menu-item :key="1">简单流程</a-menu-item>
                  <a-menu-item :key="2">任务流程</a-menu-item>
                </a-menu>
              </template>
              <a-button type="primary" preIcon="icon-ym icon-ym-btn-add">{{ t('common.addText') }}<DownOutlined /></a-button>
            </a-dropdown>
            <jnpf-upload-btn url="/api/workflow/template/Actions/Import" accept=".ffe" @on-success="reload"></jnpf-upload-btn>
          </template>
          <template #bodyCell="{ column, record }">
            <template v-if="column.key === 'status'">
              <a-tag :color="record.status === 0 ? '' : record.status == 1 ? 'success' : 'error'">
                {{ record.status === 0 ? '未上架' : record.status === 1 ? '已上架' : '已下架' }}
              </a-tag>
            </template>
            <template v-if="column.key === 'enabledMark'">
              <a-tag :color="record.enabledMark == 1 ? 'success' : ''">{{ record.enabledMark == 1 ? '已发布' : '未发布' }}</a-tag>
            </template>
            <template v-if="column.key === 'action'">
              <TableAction :actions="getTableActions(record)" :dropDownActions="getDropDownActions(record)" />
            </template>
          </template>
        </BasicTable>
      </div>
    </div>
    <Form @register="registerForm" @reload="reload" @design="handleDesign" />
    <DesignForm @register="registerDesignForm" @reload="reload" />
    <AuthModal @register="registerAuth" />
  </div>
</template>
<script lang="ts" setup>
  import { ref, onMounted, createVNode } from 'vue';
  import { getList, delFlow, copy, exportData, changeType, upDownSelf } from '@/api/workFlow/template';
  import { BasicTable, useTable, TableAction, BasicColumn, ActionItem } from '@/components/Table';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useI18n } from '@/hooks/web/useI18n';
  import { useModal } from '@/components/Modal';
  import { downloadByUrl } from '@/utils/file/download';
  import { useBaseStore } from '@/store/modules/base';
  import { DownOutlined } from '@ant-design/icons-vue';
  import Form from './Form.vue';
  import DesignForm from './DesignForm.vue';
  import AuthModal from './AuthModal.vue';
  import { Radio, RadioGroup, Form as vueForm, FormItem } from 'ant-design-vue';

  defineOptions({ name: 'workFlow-flowEngine' });

  const { createMessage } = useMessage();
  const baseStore = useBaseStore();
  const { t } = useI18n();
  const [registerForm, { openModal: openFormModal }] = useModal();
  const [registerDesignForm, { openModal: openDesignFormModal }] = useModal();
  const [registerAuth, { openModal: openAuthModal }] = useModal();
  const { createConfirm } = useMessage();

  const columns: BasicColumn[] = [
    { title: '流程名称', dataIndex: 'fullName' },
    { title: '流程编码', dataIndex: 'enCode', width: 200 },
    { title: '流程分类', dataIndex: 'category', width: 150 },
    { title: '流程类型', dataIndex: 'type', width: 100, align: 'center', customRender: ({ record }) => getFlowTypeName(record.type) },
    { title: '创建人', dataIndex: 'creatorUser', width: 120 },
    { title: '创建时间', dataIndex: 'creatorTime', width: 150, format: 'date|YYYY-MM-DD HH:mm:ss' },
    { title: '排序', dataIndex: 'sortCode', width: 70, align: 'center' },
    { title: '发布状态', dataIndex: 'enabledMark', width: 100, align: 'center' },
    { title: '上架状态', dataIndex: 'status', width: 100, align: 'center' },
  ];
  const categoryList = ref<any[]>([]);
  const [registerTable, { reload, getForm }] = useTable({
    api: getList,
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
          label: '流程分类',
          component: 'Select',
          componentProps: {
            placeholder: '请选择',
            showSearch: true,
          },
        },
        {
          field: 'type',
          label: '流程类型',
          component: 'Select',
          componentProps: {
            placeholder: '请选择',
            options: [
              { id: 0, fullName: '标准流程' },
              { id: 1, fullName: '简单流程' },
              { id: 2, fullName: '任务流程' },
            ],
          },
        },
      ],
    },
    actionColumn: {
      width: 180,
      title: '操作',
      dataIndex: 'action',
    },
  });
  function getTableActions(record): ActionItem[] {
    return [
      {
        label: t('common.editText'),
        onClick: addOrUpdateHandle.bind(null, record.id, record.type),
      },
      {
        label: record.status != 1 ? '上架' : '下架',
        color: 'error',
        disabled: record.enabledMark === 0,
        onClick: handleUpOrDown.bind(null, record),
      },
      {
        label: '设计',
        onClick: handleDesign.bind(null, record),
      },
    ];
  }
  function getDropDownActions(record): ActionItem[] {
    return [
      {
        label: '复制',
        ifShow: !!record.enabledMark,
        modelConfirm: {
          content: '您确定要复制该流程, 是否继续?',
          onOk: handleCopy.bind(null, record.id),
        },
      },
      {
        label: '导出',
        ifShow: !!record.enabledMark,
        modelConfirm: {
          content: '您确定要导出该流程, 是否继续?',
          onOk: handleExport.bind(null, record.id),
        },
      },
      {
        label: '切换',
        ifShow: record.type === 1,
        modelConfirm: {
          content: '您确定要切换为标准流程, 是否继续?',
          onOk: handleChangeType.bind(null, record.id),
        },
      },
      {
        label: '权限',
        ifShow: record.visibleType == 2,
        onClick: handlePermission.bind(null, record.id),
      },
      {
        label: t('common.delText'),
        modelConfirm: {
          content: t('common.delTip'),
          onOk: handleDelete.bind(null, record.id),
        },
      },
    ];
  }
  function addOrUpdateHandle(id = '', type) {
    openFormModal(true, { id, type, categoryList });
  }
  function handleDesign(record) {
    openDesignFormModal(true, { id: record.id, fullName: record.fullName });
  }
  function handleDelete(id) {
    delFlow(id).then(res => {
      createMessage.success(res.msg);
      reload();
    });
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
  function handleChangeType(id) {
    changeType(id).then(res => {
      createMessage.success(res.msg);
      reload();
    });
  }
  function handlePermission(id) {
    openAuthModal(true, { id });
  }
  function handleAdd(e) {
    addOrUpdateHandle('', e.key);
  }
  async function getOptions() {
    const res = await baseStore.getDictionaryData('businessType');
    categoryList.value = res as any[];
    getForm().updateSchema({ field: 'category', componentProps: { options: res } });
  }
  function getFlowTypeName(type) {
    if (type === 0) return '标准流程';
    if (type === 1) return '简单流程';
    if (type === 2) return '任务流程';
    return '';
  }
  function handleUpOrDown(record) {
    record.status != 1 ? handleUpFlow(record) : handleDownFlow(record);
  }
  function handleUpFlow(data) {
    createConfirm({
      iconType: 'warning',
      title: t('common.tipTitle'),
      content: '此操作将上架该流程，是否继续？',
      onOk: onOk,
    });
    function onOk() {
      upDownSelf({ id: data.id, isUp: 0 }).then(res => {
        createMessage.success(res.msg);
        reload();
      });
    }
  }
  function handleDownFlow(data) {
    const formData = ref({
      isHidden: 0, // 默认选中的值
    });
    const formInstance: any = ref(null);
    createConfirm({
      iconType: 'warning',
      title: t('common.tipTitle'),
      content: () => {
        return createVNode(
          vueForm,
          {
            model: formData.value,
            ref: instance => (formInstance.value = instance),
            layout: 'vertical',
            class: 'downFlow-confirm',
          },
          {
            default: () => [
              createVNode(
                FormItem,
                {
                  label: '此操作将下架该流程，下架后未审批完成的流程',
                  name: 'isHidden',
                  class: 'downFlow-label',
                },
                {
                  default: () =>
                    createVNode(
                      RadioGroup,
                      {
                        value: formData.value.isHidden,
                        'onUpdate:value': value => (formData.value.isHidden = value),
                      },
                      {
                        default: () => [
                          createVNode(Radio, { value: 0 }, { default: () => '继续审批' }),
                          createVNode(Radio, { value: 1 }, { default: () => '隐藏审批数据' }),
                        ],
                      },
                    ),
                },
              ),
            ],
          },
        );
      },
      onOk: onOk,
    });
    function onOk() {
      return formInstance.value
        ?.validate()
        .then(() => {
          upDownSelf({ id: data.id, isUp: 1, ...formData.value }).then(res => {
            createMessage.success(res.msg);
            reload();
          });
        })
        .catch(() => {
          return Promise.reject(); // 阻止 Modal 关闭
        });
    }
  }

  onMounted(() => {
    getOptions();
  });
</script>
