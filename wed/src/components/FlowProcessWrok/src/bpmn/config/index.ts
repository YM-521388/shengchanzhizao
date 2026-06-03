import { jnpfApproverConfig } from './element/approver';
import { jnpfStartConfig } from './element/start';
import { jnpfEndConfig } from './element/end';
import { jnpfSubFlowConfig } from './element/subFlow';
import { jnpfLabelConfig } from './element/label';
import { jnpfExclusiveConfig } from './element/gateway/exclusive';
import { jnpfInclusiveConfig } from './element/gateway/inclusive';
import { jnpfParallelConfig } from './element/gateway/parallel';
import {
  bpmnTask,
  bpmnStart,
  bpmnEnd,
  bpmnTimer,
  bpmnSubFlow,
  bpmnLabel,
  bpmnInclusive,
  bpmnParallel,
  bpmnExclusive,
  typeStart,
  typeEnd,
  typeSubFlow,
  typeTimer,
  typeLabel,
  typeGateway,
  typeTask,
  bpmnSequenceFlow,
  bpmnGroup,
  typeGroup,
  bpmnTrigger,
  typeAddData,
  typeGetData,
  typeUpdateData,
  typeDelData,
  typeInterface,
  typeLaunchFlow,
  typeMessage,
  typeSchedule,
  bpmnAddData,
  bpmnGetData,
  bpmnUpdateData,
  bpmnDelData,
  bpmnInterface,
  bpmnLaunchFlow,
  bpmnMessage,
  bpmnSchedule,
  bpmnEvent,
  bpmnNotice,
  bpmnWebhook,
  bpmnTime,
  typeTrigger,
  typeEventTrigger,
  typeWebhookTrigger,
  typeTimeTrigger,
  typeNoticeTrigger,
  bpmnExecute,
  typeExecute,
  bpmnProcessing,
  bpmnChoose,
  typeExclusive,
  typeChoose,
  typeParallel,
  typeInclusion,
} from './variableName';
import { jnpfSequenceFlow } from './element/sequenceFlow';
import { jnpfGroupConfig } from './element/group';
import { jnpfTriggerConfig } from './element/trigger';
import { jnpfAddDataConfig } from './element/execute/addData';
import { jnpfGetDataConfig } from './element/execute/getData';
import { jnpfUpdateDataConfig } from './element/execute/updateData';
import { jnpfDelDataConfig } from './element/execute/delData';
import { jnpfInterfaceConfig } from './element/execute/interface';
import { jnpfLaunchConfig } from './element/execute/launch';
import { jnpfMessageConfig } from './element/execute/message';
import { jnpfScheduleConfig } from './element/execute/schedule';
import { jnpfWebhookConfig } from './element/trigger/webhook';
import { jnpfEventConfig } from './element/trigger/event';
import { jnpfTimeConfig } from './element/trigger/time';
import { jnpfNoticeConfig } from './element/trigger/notice';
import { jnpfProcessingConfig } from './element/processing';
import { jnpfChooseConfig } from './element/gateway/choose';

const hasLabelElements: any = ['bpmn:StartEvent', 'bpmn:EndEvent', 'bpmn:InclusiveGateway']; // 一开始就有label标签的元素类型
const BpmnBusinessObjectKey = {
  id: 'wnId',
};
const typeConfig: any = {
  [bpmnTask]: jnpfApproverConfig,
  [bpmnStart]: jnpfStartConfig,
  [bpmnEnd]: jnpfEndConfig,
  [bpmnSubFlow]: jnpfSubFlowConfig,
  [bpmnLabel]: jnpfLabelConfig,
  [bpmnInclusive]: jnpfInclusiveConfig,
  [bpmnParallel]: jnpfParallelConfig,
  [bpmnExclusive]: jnpfExclusiveConfig,
  [bpmnSequenceFlow]: jnpfSequenceFlow,
  [bpmnGroup]: jnpfGroupConfig,
  [bpmnTrigger]: jnpfTriggerConfig,
  [bpmnAddData]: jnpfAddDataConfig,
  [bpmnGetData]: jnpfGetDataConfig,
  [bpmnUpdateData]: jnpfUpdateDataConfig,
  [bpmnDelData]: jnpfDelDataConfig,
  [bpmnInterface]: jnpfInterfaceConfig,
  [bpmnLaunchFlow]: jnpfLaunchConfig,
  [bpmnMessage]: jnpfMessageConfig,
  [bpmnSchedule]: jnpfScheduleConfig,
  [bpmnEvent]: jnpfEventConfig,
  [bpmnTime]: jnpfTimeConfig,
  [bpmnNotice]: jnpfNoticeConfig,
  [bpmnWebhook]: jnpfWebhookConfig,
  [bpmnProcessing]: jnpfProcessingConfig,
  [bpmnChoose]: jnpfChooseConfig,
};
// 默认wnType值
const conversionWnType: any = {
  [bpmnStart]: typeStart,
  [bpmnEnd]: typeEnd,
  [bpmnTask]: typeTask,
  [bpmnSubFlow]: typeSubFlow,
  [bpmnTimer]: typeTimer,
  [bpmnLabel]: typeLabel,
  [bpmnInclusive]: typeInclusion,
  [bpmnParallel]: typeParallel,
  [bpmnExclusive]: typeExclusive,
  [bpmnGroup]: typeGroup,
};
// 任务节点类型
const changeTypeByTaskShape: any = {
  [typeAddData]: bpmnAddData,
  [typeGetData]: bpmnGetData,
  [typeUpdateData]: bpmnUpdateData,
  [typeDelData]: bpmnDelData,
  [typeInterface]: bpmnInterface,
  [typeLaunchFlow]: bpmnLaunchFlow,
  [typeMessage]: bpmnMessage,
  [typeSchedule]: bpmnSchedule,
};
// 判断是否为触发节点
const triggerTypeChange: any = {
  [bpmnTrigger]: typeTrigger,
  [bpmnEvent]: typeTrigger,
  [bpmnNotice]: typeTrigger,
  [bpmnTime]: typeTrigger,
  [bpmnWebhook]: typeTrigger,
};
const changeTypeByTrigger: any = {
  [typeEventTrigger]: bpmnEvent,
  [typeNoticeTrigger]: bpmnNotice,
  [typeTimeTrigger]: bpmnTime,
  [typeWebhookTrigger]: bpmnWebhook,
};
const hasGatewayType = new Set([typeInclusion, typeParallel, typeExclusive, typeChoose, typeGateway]);

export { typeConfig, BpmnBusinessObjectKey, hasLabelElements, conversionWnType, changeTypeByTaskShape, triggerTypeChange, changeTypeByTrigger, hasGatewayType };
