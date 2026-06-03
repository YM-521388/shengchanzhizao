<template>
  <div class="jnpf-content-wrapper document-wrapper bg-white">
    <a-tabs v-model:activeKey="activeKey" tab-position="left" class="common-left-tabs">
      <a-tab-pane v-for="tab in leftList" :key="tab.id">
        <template #tab><i :class="tab.icon"></i>{{ tab.fullName }}</template>
      </a-tab-pane>
    </a-tabs>
    <div class="document-container">
      <Breadcrumb class="!mb-10px">
        <BreadcrumbItem v-if="levelList.length > 1" @click="handleJump(levelList[levelList.length - 2], levelList.length - 2)">
          <a>返回上一级</a>
        </BreadcrumbItem>
        <BreadcrumbItem v-for="(item, i) in levelList" :key="i">
          <span v-if="i + 1 >= levelList.length">{{ item.fullName }}</span>
          <a v-else @click="handleJump(item, i)">{{ item.fullName }}</a>
        </BreadcrumbItem>
      </Breadcrumb>
      <div class="jnpf-common-search-box">
        <BasicForm class="search-form" @register="registerForm" @submit="handleSubmit" @reset="handleReset"></BasicForm>
        <div class="jnpf-common-search-box-right">
          <template v-if="!selectedRowKeys.length">
            <template v-if="activeKey === 'all'">
              <a-button preIcon="icon-ym icon-ym-add-folder" @click="addFolder()">新建文件夹</a-button>
              <a-button type="primary" preIcon="icon-ym icon-ym-upload1" @click="uploadFile()" class="ml-10px">上传文件</a-button>
            </template>
            <a-tooltip>
              <template #title>{{ showMode === 1 ? '缩略模式' : '列表模式' }}</template>
              <i class="mode-icon icon-ym icon-ym-thumb-mode" @click="toggleShowMode(2)" v-show="showMode === 1"></i>
              <i class="mode-icon icon-ym icon-ym-tile-mode" @click="toggleShowMode(1)" v-show="showMode === 2"></i>
            </a-tooltip>
          </template>
          <template v-else>
            <a-space-compact block>
              <template v-if="activeKey === 'all'">
                <a-button preIcon="icon-ym icon-ym-app-share" @click="handleShare">共享</a-button>
                <a-button preIcon="icon-ym icon-ym-app-download" @click="handleDownload">下载</a-button>
                <a-button preIcon="icon-ym icon-ym-app-delete" @click="handleDelete">删除</a-button>
                <a-button preIcon="icon-ym icon-ym-app-rename" @click="handleRename" v-if="selectedRowKeys.length === 1">重命名</a-button>
                <a-button preIcon="icon-ym icon-ym-app-move" @click="handleMoveTo">移动</a-button>
              </template>
              <template v-if="activeKey === 'shareOut'">
                <a-button preIcon="icon-ym icon-ym-share-cancel" @click="handleUnshare" v-if="levelList.length <= 1">取消共享</a-button>
              </template>
              <template v-if="activeKey === 'shareTome'">
                <a-button preIcon="icon-ym icon-ym-app-download" @click="handleDownload">下载</a-button>
              </template>
              <template v-if="activeKey === 'trash'">
                <a-button preIcon="icon-ym icon-ym-recovery" @click="handleRecovery">还原</a-button>
                <a-button preIcon="icon-ym icon-ym-app-delete" @click="handleTrashDel">删除</a-button>
              </template>
            </a-space-compact>
          </template>
        </div>
      </div>
      <BasicTable @register="registerTable" v-bind="getTableBindValue" v-show="showMode === 1">
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'fullName'">
            <span class="document-fileName" :class="{ 'link-fullName': record.type }" @click="onRecordClick(record)">
              <img :src="getRecordImg(record.fileExtension)" class="file-img" />
              {{ record.fullName }}
            </span>
          </template>
          <template v-if="column.key === 'isShare'">
            <span v-if="record.isShare" title="共享文件"><i class="icon-ym icon-ym-share-filled i-default"></i></span>
            <span v-else></span>
          </template>
          <template v-if="column.key === 'fileSize'">
            {{ toFileSize(record.fileSize) }}
          </template>
          <template v-if="column.key === 'action'">
            <a-button type="link" size="small" @click="handleSingleShare(record.id)" class="!px-0" v-if="levelList.length <= 1">共享</a-button>
          </template>
        </template>
      </BasicTable>
      <div class="document-list-header" v-show="showMode === 2">
        <a-checkbox :indeterminate="isIndeterminate" v-model:checked="checkAll" :disabled="!list.length" @change="handleCheckAllChange">全选</a-checkbox>
      </div>
      <a-checkbox-group v-model:value="selectedRowKeys" class="document-list" @change="handleCheckedChange" v-show="showMode === 2">
        <ScrollContainer v-loading="loading">
          <div class="document-list-main">
            <div
              class="document-item"
              :class="{ active: selectedRowKeys.includes(record.id) }"
              v-for="record in list"
              :key="record.id"
              @click="onRecordClick(record)">
              <img :src="getRecordImg(record.fileExtension)" class="document-item-img" />
              <p class="document-item-title" :title="record.fullName">{{ record.fullName }}</p>
              <div class="check-icon" @click.stop>
                <a-checkbox :value="record.id"></a-checkbox>
              </div>
            </div>
          </div>
          <jnpf-empty v-if="!list.length" />
        </ScrollContainer>
      </a-checkbox-group>
    </div>
    <FileUploader ref="fileUploaderRef" :parentId="searchInfo.parentId" @fileSuccess="initData" />
    <Form @register="registerFormModal" @reload="initData" />
    <UserBox @register="registerUserBox" @reload="initData" />
    <FolderTree @register="registerFolderTree" @reload="initData" />
    <Preview ref="filePreviewRef" />
  </div>
</template>

<script lang="ts" setup>
  import { reactive, toRefs, watch, onMounted, ref, computed } from 'vue';
  import { Breadcrumb, BreadcrumbItem } from 'ant-design-vue';
  import { BasicForm, useForm } from '@/components/Form';
  import { BasicTable, useTable } from '@/components/Table';
  import {
    getAllList,
    download,
    batchDeleteDocument,
    getShareOutList,
    shareCancel,
    getShareTomeList,
    getTrashList,
    trashRecovery,
    trashDelete,
  } from '@/api/workFlow/document';
  import { useI18n } from '@/hooks/web/useI18n';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useGlobSetting } from '@/hooks/setting';
  import { toFileSize } from '@/utils/jnpf';
  import { downloadByUrl } from '@/utils/file/download';
  import { useModal } from '@/components/Modal';
  import { createImgPreview } from '@/components/Preview/index';
  import FileUploader from './FileUploader.vue';
  import Form from './Form.vue';
  import UserBox from './UserBox.vue';
  import FolderTree from './FolderTree.vue';
  import Preview from './Preview.vue';
  import { ScrollContainer } from '@/components/Container';
  import folderImg from '@/assets/images/document/folder.png';
  import wordImg from '@/assets/images/document/word.png';
  import excelImg from '@/assets/images/document/excel.png';
  import pptImg from '@/assets/images/document/ppt.png';
  import pdfImg from '@/assets/images/document/pdf.png';
  import rarImg from '@/assets/images/document/rar.png';
  import txtImg from '@/assets/images/document/txt.png';
  import codeImg from '@/assets/images/document/code.png';
  import imageImg from '@/assets/images/document/image.png';
  import audioImg from '@/assets/images/document/audio.png';
  import blankImg from '@/assets/images/document/blank.png';

  interface State {
    activeKey: string;
    levelList: any[];
    showMode: number;
    selectedRowKeys: string[];
    searchInfo: any;
    list: any[];
    loading: boolean;
    isIndeterminate: boolean;
    checkAll: boolean;
  }

  defineOptions({ name: 'workFlow-document' });

  const leftList = [
    { id: 'all', fullName: '我的文档', icon: 'icon-ym icon-ym-folder' },
    { id: 'shareOut', fullName: '我的共享', icon: 'icon-ym icon-ym-share-filled' },
    { id: 'shareTome', fullName: '共享给我', icon: 'icon-ym icon-ym-shareMe-filled' },
    { id: 'trash', fullName: '回收站', icon: 'icon-ym icon-ym-trash-filled' },
  ];
  const allColumns = [
    { title: '文件名', dataIndex: 'fullName' },
    { title: '', dataIndex: 'isShare', width: 35 },
    { title: '大小', dataIndex: 'fileSize', width: 90 },
    { title: '创建日期', dataIndex: 'creatorTime', width: 150, format: 'date|YYYY-MM-DD HH:mm:ss' },
  ];
  const shareOutColumns = [
    { title: '文件名', dataIndex: 'fullName' },
    { title: '大小', dataIndex: 'fileSize', width: 90 },
    { title: '共享日期', dataIndex: 'shareTime', width: 150, format: 'date|YYYY-MM-DD HH:mm:ss' },
    { title: '操作', dataIndex: 'action', width: 50 },
  ];
  const shareTomeColumns = [
    { title: '文件名', dataIndex: 'fullName' },
    { title: '大小', dataIndex: 'fileSize', width: 90 },
    { title: '共享日期', dataIndex: 'shareTime', width: 150, format: 'date|YYYY-MM-DD HH:mm:ss' },
    { title: '共享人员', dataIndex: 'creatorUserId', width: 120 },
  ];
  const trashColumns = [
    { title: '文件名', dataIndex: 'fullName' },
    { title: '大小', dataIndex: 'fileSize', width: 90 },
    { title: '删除日期', dataIndex: 'deleteTime', width: 150, format: 'date|YYYY-MM-DD HH:mm:ss' },
  ];

  const wordTypeList = ['doc', 'docx'];
  const excelTypeList = ['xls', 'xlsx'];
  const pptTypeList = ['ppt', 'pptx'];
  const pdfTypeList = ['pdf'];
  const zipTypeList = ['rar', 'zip', 'arj', 'z', '7z'];
  const txtTypeList = ['txt', 'log'];
  const codeTypeList = ['html', 'cs', 'xml'];
  const imgTypeList = ['png', 'jpg', 'jpeg', 'bmp', 'gif'];
  const videoTypeList = ['avi', 'wmv', 'mpg', 'mpeg', 'mov', 'rm', 'ram', 'swf', 'flv', 'mp4', 'mp3', 'wma', 'avi', 'rm', 'rmvb', 'flv', 'mpg', 'mkv'];
  const previewTypeList = [...wordTypeList, ...excelTypeList, ...pptTypeList, ...pdfTypeList];
  const { t } = useI18n();
  const { createMessage, createConfirm } = useMessage();
  const globSetting = useGlobSetting();
  const apiUrl = ref(globSetting.apiUrl);
  const fileUploaderRef = ref<any>(null);
  const filePreviewRef = ref<any>(null);
  const state = reactive<State>({
    activeKey: 'all',
    levelList: [],
    showMode: 1, // 1 列表模式 2 缩略模式
    selectedRowKeys: [],
    searchInfo: {
      keyword: '',
      parentId: '0',
    },
    list: [],
    loading: false,
    isIndeterminate: false,
    checkAll: false,
  });
  const { activeKey, levelList, showMode, selectedRowKeys, searchInfo, list, loading, isIndeterminate, checkAll } = toRefs(state);

  const [registerFormModal, { openModal: openFormModal }] = useModal();
  const [registerUserBox, { openModal: openUserBox }] = useModal();
  const [registerFolderTree, { openModal: openFolderTree }] = useModal();
  const [registerForm, { resetFields }] = useForm({
    baseColProps: { span: 8 },
    showActionButtonGroup: true,
    showAdvancedButton: true,
    compact: true,
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
    ],
  });
  const [registerTable, { clearSelectedRowKeys }] = useTable({
    rowSelection: { type: 'checkbox' },
    clickToRowSelect: false,
    showTableSetting: false,
    pagination: false,
    immediate: false,
    resizeHeightOffset: 10,
  });

  const getTableBindValue = computed(() => ({ loading: state.loading, onSelectionChange, dataSource: state.list, columns: getColumns() }));

  watch(
    () => state.activeKey,
    () => init(),
  );

  function getApi() {
    if (state.activeKey === 'shareOut') return getShareOutList;
    if (state.activeKey === 'shareTome') return getShareTomeList;
    if (state.activeKey === 'trash') return getTrashList;
    return getAllList;
  }
  function getColumns() {
    if (state.activeKey === 'shareOut') return shareOutColumns;
    if (state.activeKey === 'shareTome') return shareTomeColumns;
    if (state.activeKey === 'trash') return trashColumns;
    return allColumns;
  }
  function handleJump(item, i) {
    state.searchInfo.parentId = item.id;
    state.levelList = state.levelList.slice(0, i + 1);
    handleReset();
  }
  function handleSubmit(values) {
    state.searchInfo.keyword = values?.keyword || '';
    if (state.searchInfo.keyword) resetBreadcrumb();
    initData();
  }
  function handleReset() {
    state.searchInfo.keyword = '';
    initData();
  }
  // 上传文件
  function uploadFile() {
    fileUploaderRef.value?.openUploader();
  }
  function addFolder() {
    openFormModal(true, { parentId: state.searchInfo.parentId, id: '' });
  }
  function handleRename() {
    openFormModal(true, { parentId: state.searchInfo.parentId, id: state.selectedRowKeys[0] });
  }
  function onRecordClick(record) {
    if (state.activeKey === 'trash') return;
    record.type ? handlePreview(record) : openFolder(record);
  }
  // 预览
  function handlePreview(record) {
    // 图片预览
    if (imgTypeList.includes(record.fileExtension)) {
      const imageList = [apiUrl.value + record.uploaderUrl];
      createImgPreview({ imageList: imageList });
      return;
    }
    if (previewTypeList.includes(record.fileExtension)) {
      // 文件预览
      const file = {
        name: record.fullName,
        fileId: record.filePath,
        fileVersionId: null,
        url: record.uploaderUrl,
      };
      filePreviewRef.value?.init(file);
    }
  }
  // 批量共享
  function handleShare() {
    openUserBox(true, { ids: state.selectedRowKeys, isBatch: true });
  }
  // 单个共享
  function handleSingleShare(id) {
    if (!id) return;
    openUserBox(true, { ids: [id], isBatch: false });
  }
  // 移动到
  function handleMoveTo() {
    openFolderTree(true, { ids: state.selectedRowKeys, parentId: state.searchInfo.parentId });
  }
  function openFolder(record) {
    state.searchInfo.parentId = record.id;
    state.levelList.push(record);
    state.selectedRowKeys = [];
    handleReset();
  }
  function getRecordImg(ext) {
    if (!ext) return folderImg;
    if (ext) ext = ext.replace('.', '');
    if (wordTypeList.includes(ext)) return wordImg;
    if (excelTypeList.includes(ext)) return excelImg;
    if (pptTypeList.includes(ext)) return pptImg;
    if (pdfTypeList.includes(ext)) return pdfImg;
    if (zipTypeList.includes(ext)) return rarImg;
    if (txtTypeList.includes(ext)) return txtImg;
    if (codeTypeList.includes(ext)) return codeImg;
    if (imgTypeList.includes(ext)) return imageImg;
    if (videoTypeList.includes(ext)) return audioImg;
    return blankImg;
  }
  // 下载
  function handleDownload() {
    download(state.selectedRowKeys).then(res => {
      downloadByUrl({ url: res.data.url, fileName: res.data.name });
    });
  }
  // 删除
  function handleDelete() {
    createConfirm({
      iconType: 'warning',
      title: t('common.tipTitle'),
      content: '您确定要把所选文件放入回收站, 是否继续?',
      onOk: () => {
        batchDeleteDocument(state.selectedRowKeys).then(res => {
          createMessage.success(res.msg).then(() => {
            initData();
          });
        });
      },
    });
  }
  // 取消共享
  function handleUnshare() {
    createConfirm({
      iconType: 'warning',
      title: t('common.tipTitle'),
      content: '您确定要取消共享, 是否继续?',
      onOk: () => {
        shareCancel(state.selectedRowKeys).then(res => {
          createMessage.success(res.msg).then(() => {
            initData();
          });
        });
      },
    });
  }
  // 还原
  function handleRecovery() {
    createConfirm({
      iconType: 'warning',
      title: t('common.tipTitle'),
      content: '您确定要还原选中的文件?',
      onOk: () => {
        trashRecovery(state.selectedRowKeys).then(res => {
          createMessage.success(res.msg).then(() => {
            initData();
          });
        });
      },
    });
  }
  // 彻底删除
  function handleTrashDel() {
    createConfirm({
      iconType: 'warning',
      title: '提示',
      content: '文件删除后将无法恢复，您确定要彻底删除所选文件吗?',
      onOk: () => {
        trashDelete(state.selectedRowKeys).then(res => {
          createMessage.success(res.msg).then(() => {
            initData();
          });
        });
      },
    });
  }
  // 切换展示模式
  function toggleShowMode(type) {
    state.showMode = type;
    handleCheckedChange(state.selectedRowKeys);
  }
  // 缩略模式下全选
  function handleCheckAllChange(e) {
    const val = e.target.checked;
    state.selectedRowKeys = val ? state.list.map(o => o.id) : [];
    state.isIndeterminate = false;
  }
  // 缩略模式下单选
  function handleCheckedChange(val) {
    const checkedCount = val.length;
    state.checkAll = !!checkedCount && checkedCount === state.list.length;
    state.isIndeterminate = !!checkedCount && checkedCount < state.list.length;
  }
  function onSelectionChange({ keys }) {
    state.selectedRowKeys = keys;
  }
  function initData() {
    state.loading = true;
    state.list = [];
    state.selectedRowKeys = [];
    clearSelectedRowKeys();
    handleCheckedChange(state.selectedRowKeys);
    getApi()(state.searchInfo)
      .then(res => {
        state.list = res.data.list || [];
        state.loading = false;
      })
      .catch(() => {
        state.list = [];
        state.loading = false;
      });
  }
  function resetBreadcrumb() {
    const activeItem = leftList.find(o => o.id === state.activeKey);
    state.levelList = [{ id: '0', fullName: activeItem?.fullName }];
    state.searchInfo.parentId = '0';
  }
  function init() {
    resetBreadcrumb();
    resetFields();
  }

  onMounted(() => {
    init();
  });
</script>

<style lang="less">
  .document-wrapper {
    :deep(.ant-table-container),
    .ant-table-container {
      .ant-table-cell::before {
        display: none !important;
      }
    }
    .document-container {
      flex: 1;
      padding-top: 20px;
      height: 100%;
      overflow: hidden;
      display: flex;
      flex-direction: column;
      .icon-ym {
        font-size: 14px;
      }
    }
    .jnpf-common-search-box {
      margin-bottom: 10px;
      .jnpf-common-search-box-right {
        top: 10px;
        display: flex;
        align-items: center;
        .mode-icon {
          margin-left: 10px;
          font-size: 18px;
          line-height: 32px;
          cursor: pointer;
          &:hover {
            color: @primary-color;
          }
        }
      }
    }
    .document-list-header {
      margin-top: -10px;
      margin-right: 10px;
      line-height: 40px;
      flex-shrink: 0;
      border-bottom: 1px solid @border-color-base1;
    }
    .document-list {
      flex: 1;
      width: 100%;
      overflow: hidden;
      padding-bottom: 10px;
      .document-list-main {
        padding: 20px 10px 0 0;
        height: 100%;
        display: flex;
        justify-content: flex-start;
        align-content: flex-start;
        flex-wrap: wrap;
      }
      .document-item {
        width: 100px;
        height: 100px;
        border-radius: var(--border-radius);
        overflow: hidden;
        margin: 0 20px 40px;
        padding: 5px;
        cursor: pointer;
        position: relative;
        &:hover {
          background-color: @app-content-background;
          .check-icon {
            display: block;
          }
        }
        &.active {
          .check-icon {
            display: block;
          }
        }
        .document-item-img {
          width: 60px;
          height: 60px;
          margin: 0 auto 6px;
        }
        .document-item-title {
          color: @text-color-label;
          font-size: 14px;
          text-align: center;
          overflow: hidden;
          text-overflow: ellipsis;
          white-space: nowrap;
        }
        .check-icon {
          position: absolute;
          top: 2px;
          right: 4px;
          display: none;
        }
      }
    }
    .document-fileName {
      cursor: pointer;
      &.link-fullName {
        &:hover {
          color: @primary-color;
        }
      }
      .file-img {
        width: 16px;
        height: 16px;
        display: inline-block;
        vertical-align: -3px;
      }
    }
  }
</style>
