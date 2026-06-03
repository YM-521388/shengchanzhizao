<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    defaultFullscreen
    :footer="null"
    :closable="false"
    :keyboard="false"
    class="jnpf-full-modal full-modal designer-modal">
    <template #title>
      <div class="jnpf-full-modal-header">
        <div class="header-title !w-auto">
          <img src="@/assets/images/jnpf.png" class="header-logo" />
          <span class="header-dot"></span>
          <a-tooltip :title="dataForm.fullName">
            <p class="header-txt">{{ dataForm.fullName }}</p>
          </a-tooltip>
          <a-popover
            v-model:open="versionVisible"
            trigger="click"
            placement="bottom"
            overlayClassName="jnpf-version-popover"
            arrow-point-at-center
            destroyTooltipOnHide
            v-if="currentVersion?.id">
            <template #content>
              <div class="w-250px">
                <div class="version-list">
                  <div v-for="item in versionList" class="version-item" @click="handleChangeVersion(item)">
                    <span class="version-badge" :style="{ background: getVersionColor(item.state) }"></span>
                    <span class="version-name">{{ item.fullName }}</span>
                    <span class="version-state" :style="{ background: getVersionColor(item.state) }">{{ getVersionState(item.state) }}</span>
                    <div class="version-delete">
                      <i class="icon-ym icon-ym-app-delete" @click.stop="handleDelVersion(item.id)" v-if="!item.state && versionList.length > 1" />
                    </div>
                  </div>
                </div>
                <div class="add-btn" @click="handleAddVersion">
                  <a-button type="link" preIcon="icon-ym icon-ym-btn-add">新增报表版本</a-button>
                </div>
              </div>
            </template>
            <a-button type="text" class="current-version">
              <span class="version-badge" :style="{ background: getVersionColor(currentVersion.state) }"></span>{{ currentVersion?.fullName }}
              <i class="icon-ym icon-ym-unfold ml-5px font text-10px" />
            </a-button>
          </a-popover>
          <div class="version-tip ml-0" v-if="currentVersion?.state != 0">
            <InfoCircleFilled class="icon" />
            {{ currentVersion?.state == 1 ? '启用中' : '已归档' }}的版本不可修改，请
            <a-button type="link" class="px-2px" @click="handleAddVersion">添加新版本</a-button>
          </div>
        </div>
        <a-steps v-model:current="activeKey" type="navigation" size="small" class="header-steps tab-steps">
          <a-step title="报表建模" />
          <a-step title="报表设置" />
        </a-steps>
        <a-space class="options" :size="10">
          <a-button shape="round" type="primary" @click="handleSubmit(1)" v-if="currentVersion?.state !== 1" :loading="btnLoading" :disabled="saveBtnLoading">
            {{ currentVersion?.state == 2 ? '启用' : '发布' }}
          </a-button>
          <a-button shape="round" type="primary" @click="handleSubmit()" :loading="saveBtnLoading" :disabled="btnLoading" v-if="!currentVersion?.state">
            {{ t('common.saveText') }}
          </a-button>
          <a-button shape="round" @click="handleCancel()">{{ t('common.closeText') }}</a-button>
        </a-space>
      </div>
    </template>

    <div class="h-full" :class="`${prefixCls}`">
      <Design
        ref="reportDesign"
        v-bind="getBindValue"
        @closeVersionPopover="handleCloseVersionPopover"
        @changeModalLoading="changeModalLoading"
        v-if="!loading" />
      <div v-if="activeKey == 1" class="reportDesign">
        <a-row type="flex" justify="center" align="middle" class="h-full">
          <a-col :span="12" :xxl="10" class="configuration-contain">
            <a-form :model="dataForm" :colon="false">
              <a-form-item>
                <template #label>允许导出<BasicHelp text="禁用后，【设计预览】和【发布到菜单中的报表】导出按钮不显示" /></template>
                <a-switch v-model:checked="allowExport" :checkedValue="1" :unCheckedValue="0" />
              </a-form-item>
              <a-form-item>
                <template #label>允许打印<BasicHelp text="禁用后，【设计预览】和【发布到菜单中的报表】打印按钮不显示" /></template>
                <a-switch v-model:checked="allowPrint" :checkedValue="1" :unCheckedValue="0" />
              </a-form-item>
            </a-form>
          </a-col>
        </a-row>
      </div>
    </div>
  </BasicModal>
</template>
<script lang="ts" setup>
  import { reactive, toRefs, ref, unref, computed } from 'vue';
  import { getVersionInfo, getVersionList, saveVersion, copyVersion, delVersion } from '@/api/onlineDev/report';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useDesign } from '@/hooks/web/useDesign';
  import { useI18n } from '@/hooks/web/useI18n';
  import { BasicModal, useModalInner } from '@/components/Modal';
  import Design from './design/index.vue';
  import { InfoCircleFilled } from '@ant-design/icons-vue';

  interface State {
    dataForm: any;
    btnLoading: boolean;
    saveBtnLoading: boolean;
    loading: boolean;
    isReload: boolean;
    versionVisible: boolean;
    versionList: any[];
    currentVersion: any;
    key: number;
    activeKey: number;
    allowExport: number;
    allowPrint: number;
  }
  const state = reactive<State>({
    dataForm: {},
    btnLoading: false,
    saveBtnLoading: false,
    loading: true,
    isReload: false,
    versionVisible: false,
    versionList: [],
    currentVersion: {},
    key: 0,
    activeKey: 0,
    allowExport: 1,
    allowPrint: 1,
  });

  const { dataForm, btnLoading, saveBtnLoading, loading, versionVisible, versionList, currentVersion, activeKey, allowExport, allowPrint } = toRefs(state);
  const { t } = useI18n();
  const reportDesign = ref();
  const { createMessage } = useMessage();
  const emit = defineEmits(['register', 'reload']);
  const { prefixCls } = useDesign('basic-report-designer');
  const [registerModal, { closeModal, changeLoading }] = useModalInner(init);

  const getBindValue = computed(() => ({
    dataSetList: state.dataForm.dataSetList || [],
    convertConfig: state.dataForm.convertConfig ? JSON.parse(state.dataForm.convertConfig) : [],
    versionState: state.currentVersion?.state,
    snapshot: state.dataForm.snapshot,
    customs: state.dataForm.customs,
    queryList: state.dataForm.queryList,
    cells: state.dataForm.cells,
    versionId: state.currentVersion?.id,
    sortList: state.dataForm.sortList,
    key: state.key,
  }));

  function init(data) {
    state.isReload = false;
    state.dataForm.id = data.id || '';
    state.dataForm.fullName = data.fullName || '';
    state.loading = true;
    state.activeKey = 0;
    changeLoading(true);
    initData();
  }

  function initData(update = true) {
    getVersionList(state.dataForm.id)
      .then(res => {
        state.versionList = res.data || [];
        if (!update) return;
        state.currentVersion = state.versionList[0] || {};
        initReportData();
      })
      .catch(() => {
        state.loading = false;
        changeLoading(false);
      });
  }
  function initReportData() {
    if (!state.currentVersion.id) return;
    changeLoading(true);
    getVersionInfo(state.currentVersion.id)
      .then(res => {
        state.dataForm = { ...state.dataForm, ...res.data };
        state.allowExport = res.data.allowExport || 0;
        state.allowPrint = res.data.allowPrint || 0;
        state.loading = false;
        changeLoading(false);
        state.key = +new Date();
      })
      .catch(() => {
        state.loading = false;
        changeLoading(false);
      });
  }
  function getVersionColor(state) {
    return state == 0 ? 'rgba(247,171,51,0.9)' : state == 1 ? 'rgba(75, 222, 0, 0.9)' : 'rgba(152,158,178,0.9)';
  }
  function getVersionState(state) {
    return state == 0 ? '设计中' : state == 1 ? '启用中' : '已归档';
  }
  function handleAddVersion() {
    copyVersion(state.currentVersion.id).then(res => {
      const currentId = res.data;
      getVersionList(state.dataForm.id).then(res => {
        state.versionList = res.data || [];
        if (currentId) {
          const list = state.versionList.filter(o => o.id == currentId);
          state.currentVersion = list.length ? list[0] : state.versionList[0];
        }
        initReportData();
      });
    });
  }
  function handleChangeVersion(item) {
    if (state.currentVersion.id == item.id) return;
    state.versionVisible = false;
    state.currentVersion = item;
    initReportData();
  }
  function handleDelVersion(id) {
    delVersion(id).then(res => {
      createMessage.success(res.msg);
      initData(state.currentVersion.id === id);
    });
  }
  function handleSubmit(type = 0) {
    (unref(reportDesign) as any)
      .getData()
      .then((reportRes: any) => {
        const query = {
          ...state.dataForm,
          ...reportRes,
          type,
          allowExport: state.allowExport,
          allowPrint: state.allowPrint,
        };
        type == 1 ? (state.btnLoading = true) : (state.saveBtnLoading = true);
        saveVersion(query)
          .then(res => {
            createMessage.success(res.msg);
            state.saveBtnLoading = false;
            state.btnLoading = false;
            state.isReload = true;
            state.dataForm = { ...state.dataForm, ...reportRes };
            if (!type) return;
            state.currentVersion.state = 1;
            initData(false);
          })
          .catch(() => {
            state.saveBtnLoading = false;
            state.btnLoading = false;
          });
      })
      .catch(err => {
        err.msg && createMessage.warning(err.msg);
      })
      .finally(() => {
        changeLoading(false);
      });
  }

  async function handleCancel() {
    closeModal();
    (unref(reportDesign) as any)?.handleDisposeUnit();
    if (state.isReload) emit('reload');
  }
  function handleCloseVersionPopover() {
    state.versionVisible = false;
  }
  function changeModalLoading(loading) {
    changeLoading(loading);
  }
</script>
<style lang="less" scoped>
  .reportDesign {
    z-index: 100000;
    height: 100%;
    padding: 10px;
    position: absolute;
    width: 100%;
    top: 0px;
    left: 0px;
    background: @app-base-background;
  }

  .configuration-contain {
    height: 100%;
    background-color: @component-background;
    padding: 20px;
    border-radius: 8px;
    overflow-y: auto;
    .title {
      font-weight: 600;
      margin-bottom: 10px;
    }
  }
</style>
