<template>
  <HeaderContainer :formConf="formConf" />
  <a-form :colon="false" :model="formConf" class="config-content" layout="vertical">
    <template v-if="type != 2">
      <a-form-item label="实例标题">
        <jnpf-radio v-model:value="safeFormConf.titleType" :options="titleTypeOptions" />
        <jnpf-input class="!mt-12px" v-model:value="safeFormConf.defaultContent" readonly v-if="safeFormConf.titleType == 0" />
        <a-auto-complete
          class="!mt-12px"
          v-model:value="safeFormConf.titleContent"
          :options="titleContentOptions"
          placeholder="请输入"
          allowClear
          @select="handleSelect"
          v-else />
      </a-form-item>
      <a-form-item label="流程设置">
        <a-checkbox v-model:checked="safeFormConf.hasSign">启用签名</a-checkbox>
        <div class="mt-10px">
          <a-checkbox v-model:checked="safeFormConf.hasRevoke">允许撤销</a-checkbox>
        </div>
        <div class="mt-10px">
          <a-checkbox v-model:checked="safeFormConf.hasComment" @change="onHasCommentChange">允许评论</a-checkbox>
        </div>
        <div class="mt-10px" v-if="safeFormConf.hasComment">
          <a-checkbox v-model:checked="safeFormConf.hasCommentDeletedTips">显示评论已被删除提示</a-checkbox>
        </div>
        <div class="mt-10px" v-if="appStore.getSysConfigInfo.flowSign">
          <a-checkbox v-model:checked="safeFormConf.hasSignFor">审批任务是否签收</a-checkbox>
        </div>
        <div class="mt-10px">
          <a-checkbox v-model:checked="safeFormConf.hasAloneConfigureForms" @change="onHasAloneConfigureFormsChange">允许工序节点独立配置表单</a-checkbox>
        </div>
        <div class="mt-10px">
          <a-checkbox v-model:checked="safeFormConf.hasContinueAfterReject">
            拒绝后允许流程继续流转审批<BasicHelp :text="['允许时：审批拒绝继续流转，直至流程结束，', '以最后节点的审批为流程的最终审批结果。']" />
          </a-checkbox>
        </div>
        <div class="mt-10px">
          <a-checkbox v-model:checked="safeFormConf.hasInitiatorPressOverdueNode">允许发起人对当前逾期节点进行催办</a-checkbox>
        </div>
        <div class="mt-10px">
          <a-checkbox v-model:checked="safeFormConf.isShowConditions" @change="onShowConditions">允许连线显示条件</a-checkbox>
        </div>
        <template v-if="safeFormConf.isShowConditions">
          <a-radio-group class="mt-10px" v-model:value="safeFormConf.showNameType" @change="onShowConditions">
            <a-radio :value="0">显示连线名称</a-radio>
            <a-radio :value="1">显示条件内容</a-radio>
          </a-radio-group>
        </template>
      </a-form-item>
      <a-form-item label="审批人自动审批规则">
        <a-checkbox v-model:checked="safeFormConf.autoSubmitConfig.adjacentNodeApproverRepeated">相邻节点审批人重复</a-checkbox>
        <div class="mt-10px">
          <a-checkbox v-model:checked="safeFormConf.autoSubmitConfig.ApproverHasApproval">审批人审批过该流程</a-checkbox>
        </div>
        <div class="mt-10px">
          <a-checkbox v-model:checked="safeFormConf.autoSubmitConfig.initiatorApproverRepeated">发起人与审批人重复</a-checkbox>
        </div>
      </a-form-item>
      <a-form-item label="流程撤回规则">
        <jnpf-select v-model:value="safeFormConf.recallRule" :options="recallRuleOptions" />
      </a-form-item>
      <a-form-item label="异常处理">
        <template #label>异常处理<BasicHelp text="子流程发起节点人员异常时遵循该规则" /></template>
        <jnpf-select v-model:value="safeFormConf.errorRule" :options="errorRuleOptions" @change="safeFormConf.errorRuleUser = []" />
        <jnpf-user-select v-model:value="safeFormConf.errorRuleUser" buttonType="button" multiple class="mt-10px" v-if="safeFormConf.errorRule === 2" />
      </a-form-item>
      <a-form-item label="流程归档">
        <a-checkbox class="leading-32px" v-model:checked="safeFormConf.fileConfig.on">开启归档（流程审批结束后自动归档）</a-checkbox>
        <template v-if="safeFormConf.fileConfig.on">
          <div class="mt-10px">
            <a-button @click="handleFile">归档设置</a-button>
          </div>
          <div class="file-info" v-if="safeFormConf.fileConfig.templateId">
            <span> {{ getPermissionTitle }}</span>
            <i class="icon-ym icon-ym-delete" @click="handleRemove"></i>
          </div>
        </template>
      </a-form-item>
      <a-form-item label="全局设置">
        <a-button class="w-full" @click="handleGlobalSettingModal">设置</a-button>
      </a-form-item>
    </template>
    <template v-else>
      <a-form-item label="通知人">
        <jnpf-select v-model:value="safeFormConf.msgUserType" :options="msgUserOptions" multiple />
      </a-form-item>
      <a-form-item v-if="safeFormConf.msgUserType?.includes(3)">
        <div class="ant-form-item-label"><label class="ant-form-item-no-colon">选择用户</label></div>
        <jnpf-users-select v-model:value="safeFormConf.msgUserIds" multiple />
      </a-form-item>
      <a-form-item>
        <template #label>执行失败<BasicHelp text="执行失败时发送通知！" /></template>
        <jnpf-select v-model:value="safeFormConf.failMsgConfig.on" :options="noticeOptions" />
      </a-form-item>
      <a-form-item v-if="safeFormConf?.failMsgConfig?.on === 1">
        <div class="ant-form-item-label"><label class="ant-form-item-no-colon">发送配置</label></div>
        <msg-modal
          :value="safeFormConf.failMsgConfig.msgId"
          :title="safeFormConf.failMsgConfig.msgName"
          messageSource="3"
          @change="(val, data) => onMsgChange(keyMap.failMsgConfig, val, data)" />
      </a-form-item>
      <a-form-item>
        <template #label>开始执行<BasicHelp text="开始执行时发送通知！" /></template>
        <jnpf-select v-model:value="safeFormConf.startMsgConfig.on" :options="noticeOptions" />
      </a-form-item>
      <a-form-item v-if="safeFormConf?.startMsgConfig?.on === 1">
        <div class="ant-form-item-label"><label class="ant-form-item-no-colon">发送配置</label></div>
        <msg-modal
          :value="safeFormConf.startMsgConfig.msgId"
          :title="safeFormConf.startMsgConfig.msgName"
          messageSource="3"
          @change="(val, data) => onMsgChange(keyMap.startMsgConfig, val, data)" />
      </a-form-item>
    </template>
  </a-form>
  <FileModal @register="registerModal" @confirm="onFileConfirm" />
  <GlobalSettingModal @register="registerGlobalSettingModal" @confirm="onGlobalSettingConfirm" />
</template>
<script lang="ts" setup>
  import HeaderContainer from './components/HeaderContainer.vue';
  import FileModal from './components/FileModal.vue';
  import GlobalSettingModal from './components/GlobalSettingModal.vue';
  import { useModal } from '@/components/Modal';
  import { computed, reactive, watch } from 'vue';
  import { errorRuleOptions } from '../helper/define';
  import { useAppStore } from '@/store/modules/app';
  import MsgModal from '@/components/FlowProcess/src/propPanel/components/MsgModal.vue';

  // 确保 autoSubmitConfig 存在的安全访问
  const safeFormConf = computed(() => {
    const conf = props.formConf || {};
    if (!conf.autoSubmitConfig) {
      conf.autoSubmitConfig = {
        adjacentNodeApproverRepeated: false,
        ApproverHasApproval: false,
        initiatorApproverRepeated: false,
      };
    }
    // 确保 fileConfig 也存在
    if (!conf.fileConfig) {
      conf.fileConfig = {
        on: false,
        permissionType: 1,
        path: '',
        pathName: '',
        templateId: '',
      };
    }
    // 确保 failMsgConfig 和 startMsgConfig 也存在
    if (!conf.failMsgConfig) {
      conf.failMsgConfig = {
        on: 3,
        msgId: '',
        msgName: '',
        templateJson: [],
      };
    }
    if (!conf.startMsgConfig) {
      conf.startMsgConfig = {
        on: 3,
        msgId: '',
        msgName: '',
        templateJson: [],
      };
    }
    return conf;
  });

  interface State {
    temporaryContent: string;
  }

  const state = reactive<State>({
    temporaryContent: '',
  });
  const titleTypeOptions = [
    { id: 0, fullName: '默认' },
    { id: 1, fullName: '自定义' },
  ];
  const recallRuleOptions = [
    { fullName: '不允许撤回', id: 1 },
    { fullName: '发起节点允许撤回', id: 2 },
    { fullName: '所有节点允许撤回', id: 3 },
  ];
  const msgUserOptions = [
    { id: 1, fullName: '创建人' },
    { id: 2, fullName: '超级管理员' },
    { id: 3, fullName: '自定义' },
  ];
  const noticeOptions = [
    { id: 3, fullName: '默认' },
    { id: 1, fullName: '自定义' },
    { id: 0, fullName: '关闭' },
  ];
  const keyMap = {
    failMsgConfig: 'failMsgConfig',
    startMsgConfig: 'startMsgConfig',
  };
  const emit = defineEmits(['register', 'reload', 'updateConnectNameType']);
  defineOptions({ inheritAttrs: false });
  const appStore = useAppStore();
  const props = defineProps(['formConf', 'usedFormItems', 'updateJnpfData', 'updateAllNodeFormOperates', 'type']);
  const [registerModal, { openModal: openFileModal }] = useModal();
  const [registerGlobalSettingModal, { openModal: openGlobalSettingModal }] = useModal();

  const titleContentOptions = computed(() => {
    const sysOptions = [
      { id: '@flowFullName', fullName: '流程名称' },
      { id: '@flowFullCode', fullName: '流程编码' },
      { id: '@launchUserName', fullName: '发起用户名' },
      { id: '@launchTime', fullName: '发起时间' },
    ];
    const fields = props.usedFormItems.filter(o => ['input', 'inputNumber', 'textarea'].includes(o.__config__.jnpfKey));
    const options = [...sysOptions, ...fields].map(o => ({ value: o.id, label: o.id + '(' + o.fullName + ')' }));
    return options;
  });
  const getPermissionTitle = computed(() => {
    const type = safeFormConf.value.fileConfig.permissionType;
    return type == 2 ? '流程发起人可查看' : type == 3 ? '最后节点审批人可查看' : '当前流程所有人可查看';
  });

  watch(
    () => safeFormConf.value,
    () => {
      props.updateJnpfData(safeFormConf.value);
    },
    { deep: true, immediate: true },
  );
  watch(
    () => safeFormConf.value.titleContent,
    val => {
      state.temporaryContent = val;
    },
  );

  function handleSelect(value) {
    state.temporaryContent += '{' + value + '}';
    safeFormConf.value.titleContent = state.temporaryContent;
  }
  function handleFile() {
    openFileModal(true, { fileConfig: safeFormConf.value.fileConfig });
  }
  function onFileConfirm(data) {
    safeFormConf.value.fileConfig = data || {};
  }
  function handleRemove() {
    safeFormConf.value.fileConfig = {
      on: true,
      permissionType: 1,
      path: '',
      pathName: '',
      templateId: '',
    };
  }
  /**
   * 允许工序节点独立配置表单改变对工序节点表单清空
   */
  function onHasAloneConfigureFormsChange() {
    safeFormConf.value.hasAloneConfigureForms == false && props.updateAllNodeFormOperates(true, true);
  }
  function onHasCommentChange() {
    if (safeFormConf.value.hasComment == false) safeFormConf.value.hasCommentDeletedTips = false;
  }
  function handleGlobalSettingModal() {
    openGlobalSettingModal(true, { globalParameterList: safeFormConf.value.globalParameterList || [], approvalFieldList: safeFormConf.value.approvalFieldList || [] });
  }
  function onGlobalSettingConfirm(data) {
    safeFormConf.value.globalParameterList = data.globalParameterList || [];
    safeFormConf.value.approvalFieldList = data.approvalFieldList || [];
  }
  function onMsgChange(key, val, row) {
    if (!val) {
      safeFormConf.value[key].msgId = '';
      safeFormConf.value[key].msgName = '';
      safeFormConf.value[key].templateJson = [];
      return;
    }
    if (safeFormConf.value[key].msgId === val) return;
    safeFormConf.value[key].msgId = val;
    safeFormConf.value[key].msgName = row.fullName;
    safeFormConf.value[key].templateJson = row.templateJson || [];
  }
  function onShowConditions() {
    emit('updateConnectNameType', safeFormConf.value);
  }
</script>
