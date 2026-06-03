<template>
  <div :class="`${prefixCls}`" v-loading="loading">
    <process-main ref="bpmnContainer" v-bind="getBindValue" @addVersion="handleAddVersion" />
    <div class="right-container" :class="{ open: isAdvanced }">
      <a-tooltip placement="left" :title="isAdvanced ? t('component.form.fold') : t('component.form.unfold')"
        :key="isAdvanced">
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
import { reactive, toRefs, computed, unref, ref, provide, watch } from 'vue';
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
  isAdvanced: false,
});
const { loading, isAdvanced } = toRefs(state);
const { t } = useI18n();

const getBpmn = computed(() => unref(bpmnContainer)?.getBpmn());
const getElement = computed(() => unref(bpmnContainer)?.getElement());
const getBindValue = computed(() => ({ ...props, element: unref(getElement), bpmn: unref(getBpmn) }));

// 监听选中节点变化，控制右侧面板显示
watch(getElement, newElement => {
  if (newElement) {
    // 如果是结束节点或连线，不显示右侧面板
    const isEndNode = NodeUtils.isEndNode({ type: newElement.wnType });
    const isConnection = newElement.type === bpmnSequenceFlow;
    state.isAdvanced = !isEndNode && !isConnection;
  } else {
    state.isAdvanced = false;
  }
});

async function getData() {
  handleVerificationConnect(); // 校验所有的连线是否存在bpmn2:conditionExpression 标签
  return new Promise(resolve => {
    state.errorList = [];
    let root = unref(getBpmn).get('canvas').getRootElement();
    let registry = unref(getBpmn).get('elementRegistry');
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

        let hasEndNode = false;
        let hasApproverNode = false;

        Object.keys(jnpfData.data).map(key => {
          const item = jnpfData.data[key];
          if (allElement.includes(key)) {
            const title = item.nodeName || '' + key;

            // 检查是否有结束节点
            if (item.type === 'end') {
              hasEndNode = true;
            }

            // 检查是否有工序节点
            if (item.type === 'approver' || item.type === typeProcessing) {
              hasApproverNode = true;
              // 校验工序节点的审批人
              // if (!item.approvers || item.approvers.length === 0) {
              //   setErrorList('工序节点必须设置审批人', key, title);
              // }
            }

            // 根据节点类型过滤提交到后端的字段
            if (item.type === 'start') {
              // 开始节点只保留必要字段: nodeId, type, nodeName
              node[key] = {
                nodeId: item.nodeId,
                type: item.type,
                nodeName: item.nodeName,
              };
            } else if (item.type === 'end') {
              // 结束节点只保留必要字段: nodeId, type, nodeName
              node[key] = {
                nodeId: item.nodeId,
                type: item.type,
                nodeName: item.nodeName,
              };
            } else if (item.type === 'approver' || item.type === typeProcessing) {
              // 工序节点只保留必要字段: nodeId, type, nodeName, approvers
              node[key] = {
                nodeId: item.nodeId,
                type: item.type,
                nodeName: item.nodeName,
                content: item.content||'负责人',
                approvers: item.approvers || [],
                F_RoutingNo:item.F_RoutingNo || '',//编号
                F_RoutingName:item.F_RoutingName || '',//名称
                formData: item.formData || {},//表单
                parentId:item.parentId,//父级nodeId
              };
            } else if (item.type === 'connect') {
              // 连线只保留必要字段: nodeId, type
              node[key] = {
                nodeId: item.nodeId,
                type: item.type,
              };
            } else {
              node[key] = item;
            }
          }
        });

        // 校验必须有结束节点
        if (!hasEndNode) {
          setErrorList('必须有结束节点', 'global', '路线');
        }

        // 校验必须有工序节点
        if (!hasApproverNode) {
          setErrorList('必须有工序节点', 'global', '路线');
        }
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
