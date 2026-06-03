<template>
  <div :class="`${prefixCls}`" v-loading="loading">
    <process-main ref="bpmnContainer" v-bind="getBindValue" @addVersion="handleAddVersion" />
    <div class="right-container" :class="{ open: isAdvanced }">
      <a-tooltip placement="left" :title="isAdvanced ? t('component.form.fold') : t('component.form.unfold')" :key="isAdvanced">
        <div class="toggle-btn" @click="isAdvanced = !isAdvanced">
          <DoubleRightOutlined v-if="isAdvanced" />
          <DoubleLeftOutlined v-else />
        </div>
      </a-tooltip>
      <PropPanel v-bind="getBindValue" />
    </div>
  </div>
</template>
<script lang="ts" setup>
  import { reactive, toRefs, computed, unref, ref, provide } from 'vue';
  import { useDesign } from '@/hooks/web/useDesign';
  import { validate } from './bpmn/utils/bpmnLintUtil';
  import { NodeUtils } from './bpmn/utils/nodeUtil';
  import {
    bpmnSequenceFlow,
    bpmnSubFlow,
    typeChoose,
    typeConfluence,
    typeConnect,
    typeExclusive,
    typeGateway,
    typeInclusion,
    typeParallel,
    typeProcessing,
  } from './bpmn/config/variableName';
  import { DoubleLeftOutlined, DoubleRightOutlined } from '@ant-design/icons-vue';
  import { useI18n } from '@/hooks/web/useI18n';
  import ProcessMain from './Main.vue';
  import PropPanel from './propPanel/index.vue';
  import { hasGatewayType, triggerTypeChange } from './bpmn/config';

  interface State {
    loading: boolean;
    flowList: any[];
    activeConf: any;
    defaultData: any;
    formFieldList: any;
    errorList: any[];
    isAdvanced: boolean;
  }
  provide('bpmn', () => unref(getBpmn));
  defineExpose({ getData, handleSelectNode, destroyBpmn });
  const emit = defineEmits(['addVersion']);
  const props = defineProps(['flowInfo', 'flowState', 'versionList', 'type' /* 0：bpmn 1：自研*/]);
  const { prefixCls } = useDesign('basic-process');
  const bpmnContainer: any = ref(null);
  const state = reactive<State>({
    loading: false,
    flowList: [],
    activeConf: {},
    defaultData: {},
    formFieldList: [],
    errorList: [],
    isAdvanced: true,
  });
  const { loading, isAdvanced } = toRefs(state);
  const { t } = useI18n();

  const getBpmn = computed(() => unref(bpmnContainer)?.getBpmn());
  const getElement = computed(() => unref(bpmnContainer)?.getElement());
  const getBindValue = computed(() => ({ ...props, element: unref(getElement), bpmn: unref(getBpmn) }));

  async function getData() {
    handleVerificationConnect(); // 校验所有的连线是否存在bpmn2:conditionExpression 标签
    return new Promise(resolve => {
      state.errorList = [];
      let root = unref(getBpmn).get('canvas').getRootElement();
      let registry = unref(getBpmn).get('elementRegistry');
      // 对元素的校验
      const res = validate(registry, root, props);
      if (res) {
        const element = res.element;
        // 没有节点
        if (!element) setErrorList(res.message);
        // 节点验证
        if (element?.length) element.map((item: any) => setErrorList(item.error));
      }
      // 流程自定义校验
      let saveXMLPromise = unref(getBpmn).saveXML({ format: true });
      saveXMLPromise.then((data: any) => {
        if (data) {
          let jnpfData = unref(getBpmn).get('jnpfData');
          let node: any = {};
          let allElement: any[] = unref(getBpmn)
            .get('elementRegistry')
            .filter((o: any) => o.type != 'bpmn:Process')
            .map((o: any) => o.id);
          Object.keys(jnpfData.data).map(key => {
            const item = jnpfData.data[key];
            //全局属性规则验证
            if (key === 'global') {
              if (props.type != 2) {
                if (item.errorRule == 2 && !item.errorRuleUser.length) setErrorList('请选择异常处理人', 'global', '全局属性');
                if (item.fileConfig.on && !item.fileConfig.templateId) setErrorList('请选择归档模板', 'global', '全局属性');
                let connectList = unref(getBpmn)
                  .get('elementRegistry')
                  .getAll()
                  ?.filter(element => element.type === bpmnSequenceFlow);
                if (props.type === 1)
                  connectList = connectList.filter(element => !hasGatewayType.has(element.target?.wnType) && element.source?.wnType != typeConfluence); // 简流过滤xml自动删除掉的网关对应的线条。
                item.connectList = connectList.map(obj => obj.id);
              }
              node[key] = item;
            }
            if (allElement.includes(key)) {
              const title = item.nodeName || '' + key;
              const ignoreList = [typeGateway, typeConfluence, typeInclusion, typeExclusive, typeParallel, typeChoose, typeConnect];
              if (item?.type && !item.nodeName && !ignoreList.includes(item?.type)) {
                setErrorList('节点名称不能为空', key, title);
              }
              if (item.divideRule === 'choose' && props.type === 0) {
                let element = unref(getBpmn).get('elementRegistry').get(item.nodeId);
                let hasSubFlowChildren = element.outgoing.some(connect => connect.target.wnType === bpmnSubFlow);
                if (hasSubFlowChildren) setErrorList('分流规则-选择分支配置不能选择子流程', key, title);
              }
              if (item.type == 'start' && props.type != 2) {
                if (!item.formId) setErrorList('请选择流程表单', key, title);
                if (item.printConfig?.on) {
                  if (!item.printConfig?.printIds?.length) setErrorList('请选择打印模板', key, title);
                  if (item.printConfig?.conditionType == 4 && !item.printConfig?.conditions?.length) setErrorList('请设置打印条件', key, title);
                }
              } else if (item.type == 'approver' || item.type == typeProcessing) {
                if (item.assigneeType == 6 && !item?.approvers?.length) setErrorList('请设置审批人', key, title);
                if (item.assigneeType == 4 && !item.formField) setErrorList('请选择表单字段', key, title);
                if (item.assigneeType == 5 && !item.approverNodeId) setErrorList('请选择节点', key, title);
                if (item.assigneeType == 9 && !item.interfaceConfig?.interfaceId) setErrorList('请选择数据接口', key, title);
                if (
                  item.overTimeConfig?.on === 1 &&
                  item.overTimeConfig?.overAutoTransfer &&
                  item.overTimeConfig?.overTimeType == 6 &&
                  !item?.overTimeConfig.reApprovers
                )
                  setErrorList('请设置转审人', key, title + '【超时设置】');
                if (
                  item.overTimeConfig?.on === 1 &&
                  item.overTimeConfig?.overAutoTransfer &&
                  item.overTimeConfig?.overTimeType == 9 &&
                  !item.overTimeConfig?.interfaceId
                )
                  setErrorList('请选择超时数据接口', key, title + '【超时设置】');
                if (item.hasAutoApprover) {
                  if (!item.autoAuditRule?.conditions?.length) setErrorList('请选择同意规则配置', key, title);
                  if (!item.autoRejectRule?.conditions?.length) setErrorList('请选择拒绝规则配置', key, title);
                }
                if (item.printConfig?.on) {
                  if (!item.printConfig?.printIds?.length) setErrorList('请选择打印模板', key, title);
                  if (item.printConfig?.conditionType == 4 && !item.printConfig?.conditions?.length) setErrorList('请设置打印条件', key, title);
                }
                if (item.hasApprovalField && !item.approvalField?.length) setErrorList('请选择拓展字段', key, title);
                // 超时转审人不能为空
              } else if (item.type == 'subFlow') {
                if ((item.assigneeType == 6 || item.subFlowLaunchPermission == 2) && !item?.approvers?.length) setErrorList('请设置审批人', key, title);
                if (item.assigneeType == 4 && !item.formField && item.subFlowLaunchPermission == 1) setErrorList('请选择表单字段', key, title);
                if (item.assigneeType == 5 && !item.approverNodeId) setErrorList('请选择节点', key, title);
                if (item.assigneeType == 9 && !item.interfaceConfig?.interfaceId) setErrorList('请选择数据接口', key, title);
                if (!item.flowId) setErrorList('请选择子流程表单', key, title);
              } else if (item.type == 'connect') {
                conditionsExist(item.conditions, key, title);
              } else if (item.type == 'trigger' || item.type == 'eventTrigger') {
                if (!item.formId) setErrorList('请选择触发表单', key, title);
                if (item.triggerEvent == 2 && !item.actionList?.length) setErrorList('请选择审批动作', key, title);
                conditionsExist(item.ruleList, key, title);
              } else if (item.type == 'timeTrigger') {
                if (!item.startTime) setErrorList('请选择触发开始时间', key, title);
                if (!item.cron) setErrorList('请设置Cron表达式', key, title);
                if (item.endTimeType == 2 && !item.endTime) setErrorList('请选择触发结束时间', key, title);
              } else if (item.type == 'noticeTrigger') {
                if (!item.noticeId) setErrorList('请选择通知模板', key, title);
              } else if (item.type == 'webhookTrigger') {
                if (!item.webhookUrl) setErrorList('webhook URL未生成', key, title);
                if (!item.formFieldList?.length) setErrorList('请设置接口字段', key, title);
                formFieldListExist(item.formFieldList, key, title);
              } else if (item.type == 'getData') {
                const msg = item.formType == 1 ? '请选择表单' : item.formType == 2 ? '请选择流程' : item.formType == 3 ? '请选择数据接口' : '请选择主表';
                if (!item.formId) setErrorList(msg, key, title);
                if (item.formType == 4 && !item.subTable) setErrorList('请选择子表', key, title);
                conditionsExist(item.ruleList, key, title);
                sortListExist(item.sortList || [], key, title);
                item.formType == 3 && templateJsonExist(item.interfaceTemplateJson || [], key, title);
              } else if (item.type == 'addData' || item.type == 'updateData') {
                if (!item.formId) setErrorList('请选择目标表单', key, title);
                transferListExist(item.transferList, key, title);
                if (!item.ruleList?.length && item.type == 'updateData') setErrorList('请设置更新条件', key, title);
                conditionsExist(item.ruleList, key, title);
              } else if (item.type == 'deleteData') {
                if (item.deleteType === 0) {
                  if (!item.formId) setErrorList(item.tableType === 0 ? '请选择表单' : '请选择主表', key, title);
                  if (!item.subTable && item.tableType === 1) setErrorList('请选择子表', key, title);
                  conditionsExist(item.ruleList, key, title);
                } else {
                  if (!item.formId) setErrorList('请选择表单', key, title);
                  if (!item.dataSourceForm) setErrorList('请选择数据节点', key, title);
                  if (!item.ruleList?.length) setErrorList('请设置数据校验', key, title);
                  conditionsExist(item.ruleList, key, title);
                }
              } else if (item.type == 'dataInterface') {
                if (!item.formId) setErrorList('请选择执行数据接口', key, title);
                templateJsonExist(item.templateJson || [], key, title);
              } else if (item.type == 'message') {
                if (!item.msgUserIds?.length) setErrorList('请选择接收人', key, title);
                if (!item.msgId) setErrorList('请选择发送配置', key, title);
              } else if (item.type == 'launchFlow') {
                if (!item.flowId) setErrorList('请选择发起流程', key, title);
                transferListExist(item.transferList, key, title);
                if (!item.initiator?.length) setErrorList('请选择发起人', key, title);
              } else if (item.type == 'schedule') {
                if (!item.title) setErrorList('标题不能为空', key, title);
                if (!item.creatorUserId) setErrorList('创建人不能为空', key, title);
                if (!item.startDay || (!item.allDay && !item.startTime)) setErrorList('开始时间不能为空', key, title);
                if ((item.allDay && !item.endDay) || (!item.allDay && item.duration == -1 && !item.endTime)) setErrorList('结束时间不能为空', key, title);
                if (item.reminderTime !== -2 && item.reminderType == 2 && !item.send) setErrorList('发送配置不能为空', key, title);
              } else if (hasGatewayType.has(item.type)) {
                let element = unref(getBpmn).get('elementRegistry').get(item.nodeId);
                item.type = element.wnType;
              }
              // 校验分流规则是选择分支，则必须有两个以上分支
              if (item.divideRule === typeChoose && props.type === 0) {
                let element = unref(getBpmn).get('elementRegistry').get(item.nodeId);
                let newOutgoing: any = [];
                element.outgoing.forEach(connect => {
                  if (!triggerTypeChange[connect.target.wnType]) {
                    newOutgoing.push(connect);
                  }
                });
                if (newOutgoing.length < 2) setErrorList('选择分支节点至少设置2条分支', key, title);
              }
              node[key] = item;
            }
          });
          let query = {};
          if (props.type === 1) node = handleGatewayTypeSettings(node);
          const flowXml = props.type != 1 ? handleAutoCreateGateWay(data, jnpfData) : encodeURIComponent(data.xml);
          if (!state.errorList?.length) query = { flowXml, flowNodes: node };
          resolve({ errorList: state.errorList, data: query });
        }
      });
    });
  }
  /**
   * 设置验证的数组errorList
   * @param errorInfo 异常消息
   * @param id 节点id
   * @param title 节点名称+节点id
   */
  function setErrorList(errorInfo, id = '', title = '流程建模') {
    state.errorList.push({ errorInfo, id, title });
  }

  /**
   * 选中节点
   * @param elementId 节点id
   */
  function handleSelectNode(elementId) {
    const elementRegistry: any = unref(getBpmn).get('elementRegistry');
    const selection: any = unref(getBpmn).get('selection');
    let element = elementRegistry.get(elementId);
    selection.select(element);
  }
  /**
   * 自动生成网关
   * @param data 节点id
   */
  function handleAutoCreateGateWay(data: any, jnpfData: any) {
    let elementRegistry = unref(getBpmn).get('elementRegistry');
    return NodeUtils.autoCreateGateWay(data.xml, elementRegistry, jnpfData);
  }
  /**
   * 校验连线
   * @param xml
   */
  function handleVerificationConnect() {
    let bpmn = unref(getBpmn);
    return NodeUtils.verificationConnect(bpmn);
  }
  function handleGatewayTypeSettings(node: any) {
    return NodeUtils.gatewayTypeSettings(unref(getBpmn), node);
  }
  function handleAddVersion() {
    emit('addVersion');
  }
  //条件组校验
  function conditionsExist(conditions, key, title) {
    if (conditions?.length) {
      outer: for (let i = 0; i < conditions.length; i++) {
        const e = conditions[i];
        for (let j = 0; j < e.groups.length; j++) {
          const child = e.groups[j];
          if (!child.field || !child.symbol) {
            setErrorList('请完善条件组数据', key, title);
            break outer;
          }
        }
      }
    }
  }
  // 字段验证
  function formFieldListExist(list, key, title) {
    for (let i = 0; i < list.length; i++) {
      const e = list[i];
      if (!e.id) setErrorList(`第${i + 1}行字段不能为空`, key, title);
      if (!e.fullName) setErrorList(`第${i + 1}行字段说明不能为空`, key, title);
    }
  }
  // 排序字段验证
  function sortListExist(list, key, title) {
    for (let i = 0; i < list.length; i++) {
      const e = list[i];
      if (!e.field) setErrorList(`第${i + 1}行排序字段不能为空`, key, title);
    }
  }
  // 字段验证
  function transferListExist(list, key, title) {
    if (!list?.length) return setErrorList('请至少设置一个字段', key, title);
    for (let i = 0; i < list.length; i++) {
      const e = list[i];
      if (!e.targetField) setErrorList(`第${i + 1}行目标表单字段不能为空`, key, title);
      if (!e.sourceValue) setErrorList(`第${i + 1}行数据源字段不能为空`, key, title);
    }
  }
  // 参数设置验证
  function templateJsonExist(list, key, title) {
    for (let i = 0; i < list.length; i++) {
      const e = list[i];
      if (e.sourceType == 1 && !e.relationField) setErrorList(`参数${e.field}字段不能为空`, key, title);
      if (!!e.required && e.sourceType == 2 && !e.relationField) setErrorList(`参数${e.field}不能为空`, key, title);
    }
  }
  function destroyBpmn() {
    unref(getBpmn)?.destroy();
  }
</script>
