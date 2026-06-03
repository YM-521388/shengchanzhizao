<template>
  <HeaderContainer :formConf="formConf" />
  <a-form :colon="false" :model="formConf" class="config-content" layout="vertical">
    <template v-if="type != 2">
      <a-form-item label="实例标题">
        <jnpf-radio v-model:value="formConf.titleType" :options="titleTypeOptions" />
        <jnpf-input class="!mt-12px" v-model:value="formConf.defaultContent" readonly v-if="formConf.titleType == 0" />
        <a-auto-complete
          class="!mt-12px"
          v-model:value="formConf.titleContent"
          :options="titleContentOptions"
          placeholder="请输入"
          allowClear
          @select="handleSelect"
          v-else />
      </a-form-item>
      <a-form-item label="流程设置">
        <a-checkbox v-model:checked="formConf.hasSign">启用签名</a-checkbox>
        <div class="mt-10px">
          <a-checkbox v-model:checked="formConf.hasRevoke">允许撤销</a-checkbox>
        </div>
        <div class="mt-10px">
          <a-checkbox v-model:checked="formConf.hasComment" @change="onHasCommentChange">允许评论</a-checkbox>
        </div>
        <div class="mt-10px" v-if="formConf.hasComment">
          <a-checkbox v-model:checked="formConf.hasCommentDeletedTips">显示评论已被删除提示</a-checkbox>
        </div>
        <div class="mt-10px" v-if="appStore.getSysConfigInfo.flowSign">
          <a-checkbox v-model:checked="formConf.hasSignFor">审批任务是否签收</a-checkbox>
        </div>
        <div class="mt-10px">
          <a-checkbox v-model:checked="formConf.hasAloneConfigureForms" @change="onHasAloneConfigureFormsChange">允许审批节点独立配置表单</a-checkbox>
        </div>
        <div class="mt-10px">
          <a-checkbox v-model:checked="formConf.hasContinueAfterReject">
            拒绝后允许流程继续流转审批<BasicHelp :text="['允许时：审批拒绝继续流转，直至流程结束，', '以最后节点的审批为流程的最终审批结果。']" />
          </a-checkbox>
        </div>
        <div class="mt-10px">
          <a-checkbox v-model:checked="formConf.hasInitiatorPressOverdueNode">允许发起人对当前逾期节点进行催办</a-checkbox>
        </div>
        <div class="mt-10px">
          <a-checkbox v-model:checked="formConf.isShowConditions" @change="onShowConditions">允许连线显示条件</a-checkbox>
        </div>
        <template v-if="formConf.isShowConditions">
          <a-radio-group class="mt-10px" v-model:value="formConf.showNameType" @change="onShowConditions">
            <a-radio :value="0">显示连线名称</a-radio>
            <a-radio :value="1">显示条件内容</a-radio>
          </a-radio-group>
        </template>
      </a-form-item>
      <a-form-item label="审批人自动审批规则">
        <a-checkbox v-model:checked="formConf.autoSubmitConfig.adjacentNodeApproverRepeated">相邻节点审批人重复</a-checkbox>
        <div class="mt-10px">
          <a-checkbox v-model:checked="formConf.autoSubmitConfig.ApproverHasApproval">审批人审批过该流程</a-checkbox>
        </div>
        <div class="mt-10px">
          <a-checkbox v-model:checked="formConf.autoSubmitConfig.initiatorApproverRepeated">发起人与审批人重复</a-checkbox>
        </div>
      </a-form-item>
      <a-form-item label="流程撤回规则">
        <jnpf-select v-model:value="formConf.recallRule" :options="recallRuleOptions" />
      </a-form-item>
      <a-form-item label="异常处理">
        <template #label>异常处理<BasicHelp text="子流程发起节点人员异常时遵循该规则" /></template>
        <jnpf-select v-model:value="formConf.errorRule" :options="errorRuleOptions" @change="formConf.errorRuleUser = []" />
        <jnpf-user-select v-model:value="formConf.errorRuleUser" buttonType="button" multiple class="mt-10px" v-if="formConf.errorRule === 2" />
      </a-form-item>
      <a-form-item label="流程归档">
        <a-checkbox class="leading-32px" v-model:checked="formConf.fileConfig.on">开启归档（流程审批结束后自动归档）</a-checkbox>
        <template v-if="formConf.fileConfig.on">
          <div class="mt-10px">
            <a-button @click="handleFile">归档设置</a-button>
          </div>
          <div class="file-info" v-if="formConf.fileConfig.templateId">
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
        <jnpf-select v-model:value="formConf.msgUserType" :options="msgUserOptions" multiple />
      </a-form-item>
      <a-form-item v-if="formConf.msgUserType?.includes(3)">
        <div class="ant-form-item-label"><label class="ant-form-item-no-colon">选择用户</label></div>
        <jnpf-users-select v-model:value="formConf.msgUserIds" multiple />
      </a-form-item>
      <a-form-item>
        <template #label>执行失败<BasicHelp text="执行失败时发送通知！" /></template>
        <jnpf-select v-model:value="formConf.failMsgConfig.on" :options="noticeOptions" />
      </a-form-item>
      <a-form-item v-if="formConf?.failMsgConfig?.on === 1">
        <div class="ant-form-item-label"><label class="ant-form-item-no-colon">发送配置</label></div>
        <msg-modal
          :value="formConf.failMsgConfig.msgId"
          :title="formConf.failMsgConfig.msgName"
          messageSource="3"
          @change="(val, data) => onMsgChange(keyMap.failMsgConfig, val, data)" />
      </a-form-item>
      <a-form-item>
        <template #label>开始执行<BasicHelp text="开始执行时发送通知！" /></template>
        <jnpf-select v-model:value="formConf.startMsgConfig.on" :options="noticeOptions" />
      </a-form-item>
      <a-form-item v-if="formConf?.startMsgConfig?.on === 1">
        <div class="ant-form-item-label"><label class="ant-form-item-no-colon">发送配置</label></div>
        <msg-modal
          :value="formConf.startMsgConfig.msgId"
          :title="formConf.startMsgConfig.msgName"
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
    const type = props.formConf.fileConfig.permissionType;
    return type == 2 ? '流程发起人可查看' : type == 3 ? '最后节点审批人可查看' : '当前流程所有人可查看';
  });

  watch(
    () => props.formConf,
    () => {
      props.updateJnpfData(props.formConf);
    },
    { deep: true, immediate: true },
  );
  watch(
    () => props.formConf.titleContent,
    val => {
      state.temporaryContent = val;
    },
  );

  function handleSelect(value) {
    state.temporaryContent += '{' + value + '}';
    props.formConf.titleContent = state.temporaryContent;
  }
  function handleFile() {
    openFileModal(true, { fileConfig: props.formConf.fileConfig });
  }
  function onFileConfirm(data) {
    props.formConf.fileConfig = data || {};
  }
  function handleRemove() {
    props.formConf.fileConfig = {
      on: true,
      permissionType: 1,
      path: '',
      pathName: '',
      templateId: '',
    };
  }
  /**
   * 允许审批节点独立配置表单改变对审批节点表单清空
   */
  function onHasAloneConfigureFormsChange() {
    props.formConf.hasAloneConfigureForms == false && props.updateAllNodeFormOperates(true, true);
  }
  function onHasCommentChange() {
    if (props.formConf.hasComment == false) props.formConf.hasCommentDeletedTips = false;
  }
  function handleGlobalSettingModal() {
    openGlobalSettingModal(true, { globalParameterList: props.formConf.globalParameterList || [], approvalFieldList: props.formConf.approvalFieldList || [] });
  }
  function onGlobalSettingConfirm(data) {
    props.formConf.globalParameterList = data.globalParameterList || [];
    props.formConf.approvalFieldList = data.approvalFieldList || [];
  }
  function onMsgChange(key, val, row) {
    if (!val) {
      props.formConf[key].msgId = '';
      props.formConf[key].msgName = '';
      props.formConf[key].templateJson = [];
      return;
    }
    if (props.formConf[key].msgId === val) return;
    props.formConf[key].msgId = val;
    props.formConf[key].msgName = row.fullName;
    props.formConf[key].templateJson = row.templateJson || [];
  }
  function onShowConditions() {
    emit('updateConnectNameType', props.formConf);
  }
</script>
