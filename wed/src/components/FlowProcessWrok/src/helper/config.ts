const defaultGlobalForm = {
  type: 'global',
  nodeId: 'global',
  allFormMap: {}, //所有节点表单字段
  formId: '', // 将发起节点的表单id存在全局属性中
  titleType: 0, //标题类型 0：默认  1：自定义
  defaultContent: '{发起用户名}的{流程名称}', //默认名称
  titleContent: '', //自定义名称
  hasSign: false, //启用签名
  hasRevoke: false, //允许撤销
  hasComment: true, //允许评论
  hasCommentDeletedTips: true, //显示评论已被删除提示
  hasSignFor: false, //审批任务是否签收
  hasAloneConfigureForms: false, //允许审批节点独立配置表单
  hasContinueAfterReject: false, //拒绝后允许流程继续流转审批
  hasInitiatorPressOverdueNode: true, //允许发起人对当前逾期节点进行催办
  //自动提交规则
  autoSubmitConfig: {
    adjacentNodeApproverRepeated: false, //相邻节点审批人重复
    ApproverHasApproval: false, //审批人审批过该流程
    initiatorApproverRepeated: false, //发起人与审批人重复
  },
  recallRule: 1, //流程撤回规则  1: 不允许撤回  2: 发起节点允许撤回  3:所有节点允许撤回
  errorRule: 1, // 异常处理规则  1：超级管理员  2：指定人员   3：上一节点审批人指定  4：默认审批通过  5：无法提交
  errorRuleUser: [], // 异常处理指定人员
  //流程归档配置
  fileConfig: {
    on: false, //开启归档
    permissionType: 1, //查看权限 1：当前流程所有人  2：流程发起人  3：最后节点审批人
    path: '', //归档路径
    templateId: '', //归档模板
  },
  globalParameterList: [], //全局参数
  approvalFieldList: [], //审批字段
  isShowConditions: true, // 显示线条条件
  showNameType: 0, // 0：显示名称 1：显示条件内容
  formData: {},
};
const defaultTaskGlobalForm = {
  type: 'global',
  nodeId: 'global',
  allFormMap: {}, //所有节点表单字段
  msgUserType: [1], // 通知人类型
  msgUserIds: [], // 通知人
  // 执行失败通知
  failMsgConfig: {
    on: 3,
    msgId: '',
    msgName: '',
    templateJson: [],
  },
  // 开始执行通知
  startMsgConfig: {
    on: 3,
    msgId: '',
    msgName: '',
    templateJson: [],
  },
};
const defaultStartForm = {
  type: 'start',
  nodeId: undefined,
  nodeName: '开始',
  formId: '', //流程表单id
  formName: '', //流程表单名称
  formOperates: [], //流程表单权限
  divideRule: 'inclusion', //分流规则
  //打印配置
  printConfig: {
    on: false, //开启打印
    printIds: [], //模板
    conditionType: 1, //打印条件 1：不限制  2：节点结束  3：流程结束  4：条件设置
    conditions: [], //条件设置
    matchLogic: 'and',
  },
  //限时设置配置
  timeLimitConfig: {
    on: 0, // 开启  0:关闭  1：自定义
    nodeLimit: 0, // 节点处理截止时间类型  1：接收时间  2：发起时间  3：表单变量
    duringDeal: 24, // 节点处理限定时长(时)
  },
  //提醒配置
  noticeConfig: {
    on: 0, // 开启   0:关闭  1：自定义
    firstOver: 1, // 第一次提醒时间(时)
    overTimeDuring: 2, // 提醒间隔(时)
    overNotice: true, // 提醒事务-提醒通知
    overEvent: false, // 提醒事件
    overEventTime: 5, // 提醒次数(次)
  },
  //超时设置
  overTimeConfig: {
    on: 0, // 开启   0:关闭  1：自定义
    firstOver: 0, // 第一次超时时间(时)
    overTimeDuring: 2, // 超时间隔(时)
    overNotice: true, // 超时事务-超时通知
    overAutoApprove: false, // 超时事务-超时自动审批
    overAutoApproveTime: 5, // 自动审批超时次数(次)
    overEvent: false, // 超时事件
    overEventTime: 5, // 超时事件超时次数(次)
  },
  // 流程待办通知
  waitMsgConfig: {
    on: 3, // 0:关闭  1:自定义  3：默认
    msgId: '',
    msgName: '',
    templateJson: [],
  },
  // 流程结束通知
  endMsgConfig: {
    on: 3,
    msgId: '',
    msgName: '',
    templateJson: [],
  },
  // 流程评论通知
  commentMsgConfig: {
    on: 3,
    msgId: '',
    msgName: '',
    templateJson: [],
  },
  // 节点同意通知
  approveMsgConfig: {
    on: 3,
    msgId: '',
    msgName: '',
    templateJson: [],
  },
  // 节点拒绝通知
  rejectMsgConfig: {
    on: 3,
    msgId: '',
    msgName: '',
    templateJson: [],
  },
  // 节点退回通知
  backMsgConfig: {
    on: 3,
    msgId: '',
    msgName: '',
    templateJson: [],
  },
  // copyMsgConfig通知
  copyMsgConfig: {
    on: 3,
    msgId: '',
    msgName: '',
    templateJson: [],
  },
  // 节点超时通知
  overTimeMsgConfig: {
    on: 3,
    msgId: '',
    msgName: '',
    templateJson: [],
  },
  // 节点提醒通知
  noticeMsgConfig: {
    on: 3,
    msgId: '',
    msgName: '',
    templateJson: [],
  },
  //发起事件配置
  initFuncConfig: {
    on: false,
    interfaceId: '',
    interfaceName: '',
    templateJson: [],
  },
  //结束事件配置
  endFuncConfig: {
    on: false,
    interfaceId: '',
    interfaceName: '',
    templateJson: [],
  },
  //测回事件配置
  flowRecallFuncConfig: {
    on: false,
    interfaceId: '',
    interfaceName: '',
    templateJson: [],
  },
};
const defaultApproverForm = {
  type: 'approver',
  nodeId: undefined,
  nodeName: '工序节点',
  formId: '', //流程表单id
  formName: '', //流程表单名称
  formOperates: [], //流程表单权限
  assignList: [], //数据传递
  assigneeType: 6, // 指定审批人
  approverType: 1, //直属主管审批人类型  1：发起人  2：上节点审批人
  managerApproverType: 1, //部门主管审批人类型  1：发起人  2：上节点审批人
  managerLevel: 1, //主管  1：直属  2：第2级主管  ....  10:第10级主管
  formFieldType: 1, // 表单字段审核方式的类型  1：用户 2：部门  3：岗位  4：角色  5：分组
  formField: '', //表单字段
  approverNodeId: '', //审批节点id
  prevNodeList: [],
  approvers: [], // 审批人集合
  approversSortList: [], // 审批人依次审批顺序
  extraRule: 1, // 审批人范围  1:无审批人范围  2:同一部门  3:同一岗位  4:发起人上级  5:发起人下属  6:同一公司
  counterSign: 0, //会签规则  0：或签  1：会签  2：依次审批
  // 会签流转配置
  counterSignConfig: {
    auditType: 1, // 1:按百分比  2:按人数
    auditRatio: 100,
    auditNum: 1,
    rejectType: 0,
    rejectRatio: 10,
    rejectNum: 1,
    calculateType: 1, //会签计算规则  1：实时计算  2：延后计算
  },
  circulateUser: [], // 抄送人集合
  extraCopyRule: 1, //抄送人范围
  isCustomCopy: false, //允许自选抄送人
  isInitiatorCopy: false, //抄送给流程发起人
  isFormFieldCopy: false, //抄送给表单变量
  copyFormFieldType: 1, //表单字段类型  1：用户 2：部门  3：岗位  4：角色  5：分组
  copyFormField: '', //表单字段
  hasSign: false, //手写签名
  hasFile: false, //启用签名
  hasApprovalField: false, //启用审批字段
  approvalField: [], //审批字段
  hasAuditBtn: true,
  auditBtnText: '同意',
  hasRejectBtn: true,
  rejectBtnText: '拒绝',
  hasBackBtn: false,
  backBtnText: '退回',
  hasFreeApproverBtn: false,
  freeApproverBtnText: '加签',
  hasReduceApproverBtn: false,
  reduceApproverBtnText: '减签',
  hasTransferBtn: false,
  transferBtnText: '转审',
  hasAssistBtn: false,
  assistBtnText: '协办',
  hasSaveAuditBtn: false,
  saveAuditBtnText: '暂存',
  backType: 1,
  backNodeCode: 0,
  customBtns: [], // 自定义按钮集合
  auxiliaryInfo: [], // 辅助信息
  //打印配置
  printConfig: {
    on: false, //开启打印
    printIds: [], //模板
    conditionType: 1, //打印条件 1：不限制  2：节点结束  3：流程结束  4：条件设置
    conditions: [], //条件设置
    matchLogic: 'and',
  },
  parameterList: [], //节点参数
  hasAutoApprover: false, // 自动同意规则,默认不启用
  autoAuditRule: {
    conditions: [], //条件设置
    matchLogic: 'and',
  },
  autoRejectRule: {
    conditions: [], //条件设置
    matchLogic: 'and',
  },
  divideRule: 'inclusion', //分流规则    inclusion: 根据条件多分支流转(包容网关)  exclusive:根据条件单分支流转（排它网关） parallel:所有分支都流转（并行网关）
  // 数据接口
  interfaceConfig: {
    interfaceId: '', // 接口id
    interfaceName: '', // 接口名称
    templateJson: [], // 模块json
  },
  // 节点同意通知
  approveMsgConfig: {
    on: 2,
    msgId: '',
    msgName: '',
    templateJson: [],
  },
  // 节点拒绝通知
  rejectMsgConfig: {
    on: 2,
    msgId: '',
    msgName: '',
    templateJson: [],
  },
  // 节点退回通知
  backMsgConfig: {
    on: 2,
    msgId: '',
    msgName: '',
    templateJson: [],
  },
  // copyMsgConfig通知
  copyMsgConfig: {
    on: 2,
    msgId: '',
    msgName: '',
    templateJson: [],
  },
  // 节点超时通知
  overTimeMsgConfig: {
    on: 2,
    msgId: '',
    msgName: '',
    templateJson: [],
  },
  // 节点提醒通知
  noticeMsgConfig: {
    on: 2,
    msgId: '',
    msgName: '',
    templateJson: [],
  },
  content: '', //内容
  //限时设置配置
  timeLimitConfig: {
    on: 2, // 开启    0:关闭   1：自定义  2:同步发起节点配置
    nodeLimit: 0, // 节点处理截止时间类型
    duringDeal: 24, // 节点处理限定时长(时)
    formField: '', // 请选择字段
  },
  //超时设置
  overTimeConfig: {
    on: 2, // 开启  0:关闭   1：自定义  2:同步发起节点配置
    firstOver: 0, // 第一次超时时间(时)
    overTimeDuring: 2, // 超时间隔(时)
    overNotice: true, // 超时事务-超时通知
    overAutoApprove: false, // 超时事务-超时自动审批
    overAutoApproveTime: 5, // 自动审批超时次数(次)
    overEvent: false, // 超时事件
    overEventTime: 5, // 超时事件超时次数(次)
    overAutoTransfer: false, // 超时事务-超时自动转审
    overAutoTransferTime: 2, // 自动转审超时次数(次)
    overTimeType: 6, // 转审人类型
    interfaceId: '', // 接口id
    interfaceName: '', // 接口名称
    templateJson: [], // 模块json
    reApprovers: '', // 转审人
    overTimeExtraRule: 2,
  },
  //提醒配置
  noticeConfig: {
    on: 2, // 开启    0:关闭   1：自定义  2:同步发起节点配置
    firstOver: 1, // 第一次提醒时间(时)
    overTimeDuring: 2, // 提醒间隔(时)
    overNotice: true, // 提醒事务-提醒通知
    overEvent: false, // 提醒事件
    overEventTime: 5, // 提醒次数(次)
  },
  // 同意事件配置
  approveFuncConfig: {
    on: false, // 开启
    interfaceId: '', // 接口id
    interfaceName: '', // 接口名称
    templateJson: [], // 接口参数
  },
  // 拒绝事件配置
  rejectFuncConfig: {
    on: false,
    interfaceId: '',
    interfaceName: '',
    templateJson: [],
  },
  // 退回事件配置
  backFuncConfig: {
    on: false,
    interfaceId: '',
    interfaceName: '',
    templateJson: [],
  },
  // 撤回事件配置
  recallFuncConfig: {
    on: false,
    interfaceId: '',
    interfaceName: '',
    templateJson: [],
  },
  // 超时事件配置
  overTimeFuncConfig: {
    on: false,
    interfaceId: '',
    interfaceName: '',
    templateJson: [],
  },
  // 提醒事件配置
  noticeFuncConfig: {
    on: false,
    interfaceId: '',
    interfaceName: '',
    templateJson: [],
  },
  //审核数据的表单
  formData: {},
};
const defaultProcessingForm = {
  type: 'processing',
  nodeId: undefined,
  nodeName: '办理节点',
  formId: '', //流程表单id
  formName: '', //流程表单名称
  formOperates: [], //流程表单权限
  assignList: [], //数据传递
  assigneeType: 6, // 指定审批人
  approverType: 1, //直属主管审批人类型  1：发起人  2：上节点审批人
  managerApproverType: 1, //部门主管审批人类型  1：发起人  2：上节点审批人
  managerLevel: 1, //主管  1：直属  2：第2级主管  ....  10:第10级主管
  formFieldType: 1, // 表单字段审核方式的类型  1：用户 2：部门  3：岗位  4：角色  5：分组
  formField: '', //表单字段
  approverNodeId: '', //审批节点id
  approvers: [], // 审批人集合
  approversSortList: [], // 审批人依次审批顺序
  extraRule: 1, // 审批人范围  1:无审批人范围  2:同一部门  3:同一岗位  4:发起人上级  5:发起人下属  6:同一公司
  counterSign: 0, //会签规则  0：或签  1：会签  2：依次审批
  // 会签流转配置
  counterSignConfig: {
    auditType: 1, // 1:按百分比  2:按人数
    auditRatio: 100,
    auditNum: 1,
    rejectType: 0,
    rejectRatio: 10,
    rejectNum: 1,
    calculateType: 1, //会签计算规则  1：实时计算  2：延后计算
  },
  circulateUser: [], // 抄送人集合
  extraCopyRule: 1, //抄送人范围
  isCustomCopy: false, //允许自选抄送人
  isInitiatorCopy: false, //抄送给流程发起人
  isFormFieldCopy: false, //抄送给表单变量
  copyFormFieldType: 1, //表单字段类型  1：用户 2：部门  3：岗位  4：角色  5：分组
  copyFormField: '', //表单字段
  hasSign: false, //手写签名
  hasFile: false, //启用签名
  hasApprovalField: false, //启用审批字段
  approvalField: [], //审批字段
  hasAuditBtn: true,
  auditBtnText: '确认办理',
  hasBackBtn: false,
  backBtnText: '退回',
  hasTransferBtn: false,
  transferBtnText: '转办',
  hasSaveAuditBtn: false,
  saveAuditBtnText: '暂存',
  backType: 1,
  backNodeCode: 0,
  customBtns: [], // 自定义按钮集合
  //打印配置
  printConfig: {
    on: false, //开启打印
    printIds: [], //模板
    conditionType: 1, //打印条件 1：不限制  2：节点结束  3：流程结束  4：条件设置
    conditions: [], //条件设置
    matchLogic: 'and',
  },
  parameterList: [], //节点参数
  hasAutoApprover: false, // 自动同意规则,默认不启用
  autoAuditRule: {
    conditions: [], //条件设置
    matchLogic: 'and',
  },
  autoRejectRule: {
    conditions: [], //条件设置
    matchLogic: 'and',
  },
  divideRule: 'inclusion', //分流规则    inclusion: 根据条件多分支流转(包容网关)  exclusive:根据条件单分支流转（排它网关） parallel:所有分支都流转（并行网关）
  // 数据接口
  interfaceConfig: {
    interfaceId: '', // 接口id
    interfaceName: '', // 接口名称
    templateJson: [], // 模块json
  },
  // 节点退回通知
  backMsgConfig: {
    on: 2,
    msgId: '',
    msgName: '',
    templateJson: [],
  },
  // copyMsgConfig通知
  copyMsgConfig: {
    on: 2,
    msgId: '',
    msgName: '',
    templateJson: [],
  },
  // 节点超时通知
  overTimeMsgConfig: {
    on: 2,
    msgId: '',
    msgName: '',
    templateJson: [],
  },
  // 节点提醒通知
  noticeMsgConfig: {
    on: 2,
    msgId: '',
    msgName: '',
    templateJson: [],
  },
  content: '', //内容
  //限时设置配置
  timeLimitConfig: {
    on: 2, // 开启    0:关闭   1：自定义  2:同步发起节点配置
    nodeLimit: 0, // 节点处理截止时间类型
    duringDeal: 24, // 节点处理限定时长(时)
    formField: '', // 请选择字段
  },
  //超时设置
  overTimeConfig: {
    on: 2, // 开启  0:关闭   1：自定义  2:同步发起节点配置
    firstOver: 0, // 第一次超时时间(时)
    overTimeDuring: 2, // 超时间隔(时)
    overNotice: true, // 超时事务-超时通知
    overAutoApprove: false, // 超时事务-超时自动审批
    overAutoApproveTime: 5, // 自动审批超时次数(次)
    overEvent: false, // 超时事件
    overEventTime: 5, // 超时事件超时次数(次)
    overAutoTransfer: false, // 超时事务-超时自动转审
    overAutoTransferTime: 2, // 自动转审超时次数(次)
    overTimeType: 6, // 转审人类型
    interfaceId: '', // 接口id
    interfaceName: '', // 接口名称
    templateJson: [], // 模块json
    reApprovers: '', // 转审人
    overTimeExtraRule: 2,
  },
  //提醒配置
  noticeConfig: {
    on: 2, // 开启    0:关闭   1：自定义  2:同步发起节点配置
    firstOver: 1, // 第一次提醒时间(时)
    overTimeDuring: 2, // 提醒间隔(时)
    overNotice: true, // 提醒事务-提醒通知
    overEvent: false, // 提醒事件
    overEventTime: 5, // 提醒次数(次)
  },

  // 超时事件配置
  overTimeFuncConfig: {
    on: false,
    interfaceId: '',
    interfaceName: '',
    templateJson: [],
  },
  // 提醒事件配置
  noticeFuncConfig: {
    on: false,
    interfaceId: '',
    interfaceName: '',
    templateJson: [],
  },
};
const defaultSubFlowForm = {
  type: 'subFlow',
  nodeId: undefined,
  nodeName: '子流程',
  isAsync: 0, //子流程同步  0:异步  1:同步
  flowId: '',
  flowName: '',
  assignList: [], //数据传递
  assigneeType: 6, // 指定审批人
  approverType: 1, //直属主管审批人类型  1：发起人  2：上节点审批人
  managerApproverType: 1, //部门主管审批人类型  1：发起人  2：上节点审批人
  managerLevel: 1, //主管  1：直属  2：第2级主管  ....  10:第10级主管
  formFieldType: 1, // 表单字段审核方式的类型  1：用户 2：部门  3：岗位  4：角色  5：分组
  formField: '', //表单字段
  approverNodeId: '', //审批节点id
  approvers: [], // 审批人集合
  approversSortList: [], // 审批人依次审批顺序
  extraRule: 1, // 审批人范围  1:无审批人范围  2:同一部门  3:同一岗位  4:发起人上级  5:发起人下属  6:同一公司
  content: '',
  subFlowLaunchPermission: 1, //子流程发起权限
  errorRule: 1, // 异常处理规则 1：超级管理员  2：指定人员   3：上一节点审批人指定  4：默认审批通过  5：无法提交
  errorRuleUser: [], // 指定人员处理异常
  autoSubmit: 0, //自动提交 0:否  1:是
  divideRule: 'inclusion', //分流规则
  // 数据接口
  interfaceConfig: {
    interfaceId: '', // 接口id
    interfaceName: '', // 接口名称
    templateJson: [], // 参数
  },
  //子流程发起通知
  launchMsgConfig: {
    on: 3,
    msgId: '',
    msgName: '',
    templateJson: [],
  },
  createRule: 0, //创建规则 0:同时创建  1:依次创建
};
const defaultConnectForm = {
  type: 'connect',
  nodeId: undefined,
  nodeName: '连接线',
  conditions: [],
  matchLogic: 'and',
};
//触发节点
const defaultTriggerForm = {
  type: 'trigger',
  nodeId: undefined,
  nodeName: '触发事件',
  content: '',
  isAsync: 1, //子流程同步  0:异步  1:同步
  formId: '', // 触发表单
  formName: '', // 触发表单名称
  triggerEvent: 1, // 触发事件 1-表单事件 2-审批事件 3-空白事件
  triggerFormEvent: 1, // 触发表单事件 1-新增 2-修改 3-删除
  actionList: [], //1-同意  2-拒绝  3-退回  4-加签  5-减签  6-转办  7-协办
  updateFieldList: [], //表单事件-修改数据-修改字段
  ruleList: [], //触发条件
  ruleMatchLogic: 'and', //条件规则匹配逻辑
};
//事件触发
const defaultEventTriggerForm = {
  type: 'eventTrigger',
  nodeId: undefined,
  nodeName: '事件触发',
  content: '',
  formId: '', // 触发表单
  formName: '', // 触发表单名称
  triggerEvent: 1, // 触发事件 1-表单事件 2-审批事件 3-空白事件
  triggerFormEvent: 1, // 触发表单事件 1-新增 2-修改 3-删除
  actionList: [], //1-同意  2-拒绝  3-退回  4-加签  5-减签  6-转办  7-协办
  updateFieldList: [], //表单事件-修改数据-修改字段
  ruleList: [], //触发条件
  ruleMatchLogic: 'and', //条件规则匹配逻辑
};
// 定时触发
const defaultTimeTriggerForm = {
  type: 'timeTrigger',
  nodeId: undefined,
  nodeName: '定时触发',
  content: '',
  startTime: null, // 触发开始时间
  cron: '', // cron表达式
  endTimeType: 1, // 触发结束时间类型
  endLimit: 1, // 触发次数
  endTime: null, // 触发指定时间
};
// 通知触发
const defaultNoticeTriggerForm = {
  type: 'noticeTrigger',
  nodeId: undefined,
  nodeName: '通知触发',
  content: '',
  noticeId: '',
  noticeName: null,
};
// Webhook通知触发
const defaultWebhookTriggerForm = {
  type: 'webhookTrigger',
  nodeId: undefined,
  nodeName: 'webhook触发',
  content: '',
  webhookUrl: '', // webhookUrl
  webhookGetFieldsUrl: '', // webhook获取接口字段Url
  webhookRandomStr: '', // webhook获取接口字段识别码
  formFieldList: [], // 表单/接口字段
};
//获取数据节点
const defaultGetDataForm = {
  type: 'getData',
  nodeId: undefined,
  nodeName: '获取数据',
  formType: 1, // 表单类型 1-从表单中获取 2-从流程中获取 3-从数据接口中获取 4-从子表
  formId: '', // 触发表单/接口id
  formName: '', // 触发表单/接口名称
  interfaceTemplateJson: [], // 接口参数
  ruleList: [], // 获取条件规则
  ruleMatchLogic: 'and', // 获取条件规则匹配逻辑
  formFieldLis: [], //表单字段
  cacheFormFieldList: [], //缓存表单字段
  sortList: [], //排序
};
//新增数据节点
const defaultAddDataForm = {
  type: 'addData',
  nodeId: undefined,
  nodeName: '新增数据',
  formId: '', // 表单id
  formName: '', // 表单名称
  transferList: [], // 字段设置
  dataSourceForm: '', //数据源
  ruleList: [], // 条件规则
  ruleMatchLogic: 'and', // 条件规则匹配逻辑
};
//更新数据节点
const defaultUpdateDataForm = {
  type: 'updateData',
  nodeId: undefined,
  nodeName: '更新数据',
  content: '', //内容
  formId: '', // 表单id
  formName: '', // 表单名称
  dataSourceForm: '', //数据源
  transferList: [], // 更新字段
  ruleList: [], // 更新条件规则
  ruleMatchLogic: 'and', // 更新条件规则
  unFoundRule: false, // 未找到数据时更新
};
//删除数据节点
const defaultDeleteDataForm = {
  type: 'deleteData',
  nodeId: undefined,
  nodeName: '删除数据',
  content: '', //内容
  deleteType: 0, //删除类型
  tableType: 0, //表类型  0-主表  1-子表
  formId: '', //表单
  formName: '', //表单名称
  subTable: '', //子表
  deleteCondition: 1, //删除条件  1-存在  2-不存在
  dataSourceForm: '', //数据源
  dataSourceFormName: '', //数据源名称
  ruleList: [], // 更新条件规则
  ruleMatchLogic: 'and', // 更新条件规则
};
//数据接口节点
const defaultDataInterfaceForm = {
  type: 'dataInterface',
  nodeId: undefined,
  nodeName: '数据接口',
  content: '', //内容
  dataSourceForm: '', //数据源
  formId: '', // 接口id
  formName: '', // 接口名称
  templateJson: [], // 接口参数
};
//消息通知节点
const defaultMessageForm = {
  type: 'message',
  nodeId: undefined,
  nodeName: '消息通知',
  content: '', //内容
  msgId: '',
  msgName: '',
  msgTemplateJson: [],
  msgUserIds: [], //接收人
  msgUserIdsSourceType: 2,
};
//发起审批节点
const defaultLaunchFlowForm = {
  type: 'launchFlow',
  nodeId: undefined,
  nodeName: '发起审批',
  content: '', //内容
  flowId: '', // 流程id
  flowName: '', // 流程名称
  transferList: [], // 字段设置
  initiator: [], //发起人
  dataSourceForm: '', //数据源
  formFieldList: [],
};
//创建日程节点
const defaultScheduleForm = {
  type: 'schedule',
  nodeId: undefined,
  nodeName: '创建日程',
  content: '', //内容
  title: '', //日程标题
  contents: '', //日程内容
  files: [], //日程附件
  allDay: 0, //日程全天
  startDay: '', //日程开始日期
  startTime: '', //日程开始时间
  duration: 0, //日程时长
  endDay: '', //日程结束日期
  endTime: '', //日程结束时间
  creatorUserId: '', //日程创建人
  toUserIds: [], //日程参与人
  color: '#188ae2', //日程标签颜色
  reminderTime: -2, //日程提醒时间
  reminderType: 1, //日程提醒方式
  send: '',
  repetition: 1,
  repeatTime: '',
  startDaySourceType: 1,
  endDaySourceType: 1,
  titleSourceType: 1,
  contentsSourceType: 1,
  creatorUserIdSourceType: 1,
  toUserIdsSourceType: 1,
};

export default {
  defaultGlobalForm,
  defaultTaskGlobalForm,
  defaultStartForm,
  defaultApproverForm,
  defaultProcessingForm,
  defaultSubFlowForm,
  defaultConnectForm,
  defaultTriggerForm,
  defaultEventTriggerForm,
  defaultTimeTriggerForm,
  defaultNoticeTriggerForm,
  defaultWebhookTriggerForm,
  defaultGetDataForm,
  defaultAddDataForm,
  defaultUpdateDataForm,
  defaultDeleteDataForm,
  defaultDataInterfaceForm,
  defaultMessageForm,
  defaultLaunchFlowForm,
  defaultScheduleForm,
};
