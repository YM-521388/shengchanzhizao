<template>
  <div class="jnpf-bpmnContainer" ref="bpmnContainer"></div>
</template>
<script lang="ts" setup>
  import { reactive, onMounted, markRaw, ref, unref, computed, nextTick, onUnmounted } from 'vue';
  import { defaultXml, simpleDefaultXml } from './helper/defaultXml';
  import {
    bpmnStart,
    bpmnSequenceFlow,
    bpmnGateway,
    bpmnLabel,
    bpmnTask,
    bpmnEnd,
    bpmnInclusive,
    bpmnExclusive,
    bpmnGroup,
    typeConfluence,
    typeStart,
    typeTask,
    typeSubFlow,
    typeEnd,
    typeTrigger,
    bpmnTrigger,
    typeCondition,
    bpmnParallel,
    typeLabel,
    typeChoose,
    typeInclusion,
    typeParallel,
    typeProcessing,
  } from './config/variableName';
  import { getExternalLabelMid } from 'bpmn-js/lib/util/LabelUtil';
  import { NodeUtils } from './utils/nodeUtil';
  import { changeTypeByTaskShape, changeTypeByTrigger, conversionWnType, hasGatewayType, triggerTypeChange, typeConfig } from './config';
  import BpmnModeler from './modeler';
  import SimpleModeler from './simpleModeler/modeler';
  import TaskModeler from './taskModeler/modeler';
  import PreviewModeler from './previewModeler/index';
  import customTranslate from './translate';
  import AutoPlaceModule from 'bpmn-js/lib/features/auto-place';
  import minimapModule from 'diagram-js-minimap';
  import 'bpmn-js/dist/assets/diagram-js.css';
  import 'bpmn-js/dist/assets/bpmn-font/css/bpmn-embedded.css';
  import 'diagram-js-minimap/assets/diagram-js-minimap.css';
  import { BPMNTreeBuilder } from './utils/constructTreeUtil';
  import { DEFAULT_DISTANCE } from './config/constants';
  import { is } from 'bpmn-js/lib/util/ModelUtil';
  import type { PropType } from 'vue';
  import { createVNode } from 'vue';
  import { ExclamationCircleOutlined } from '@ant-design/icons-vue';
  import { Modal, message as Message } from 'ant-design-vue';

  interface State {
    jnpfData: any;
    element: any;
    bpmn: any;
  }

  defineOptions({ name: 'JnpfBpmn' });
  const props = defineProps({
    flowNodes: {
      type: Object,
      default: () => ({}),
    },
    flowXml: {
      type: String,
      default: '',
    },
    nodeList: {
      type: Array as PropType<any[]>,
      default: () => [],
    },
    disabled: {
      type: Boolean,
      default: true,
    },
    type: {
      type: Number,
      default: 0,
    },
    isPreview: {
      type: Boolean,
      default: false,
    },
    lineKeyList: {
      type: Array as PropType<any[]>,
      default: undefined,
    },
    openPreview: {
      type: Boolean,
      default: false,
    },
  });
  const emit = defineEmits(['viewSubFlow']);

  defineExpose({ getElement, getBpmn });

  const state = reactive<State>({
    jnpfData: {},
    element: {},
    bpmn: null,
  });
  const customTranslateModule = {
    translate: ['value', customTranslate],
  };
  const bpmnContainer = ref(null);
  let elementClickId: any = null;
  let nodeMap: any = new Map();
  if (props.nodeList?.length > 0) {
    for (let i = 0; i < props.nodeList?.length; i++) {
      nodeMap.set(props.nodeList[i].nodeCode, props.nodeList[i]);
    }
  }
  // 禁用画布操作处理
  const getAdditionalModules = computed(() => {
    if (!props.disabled) {
      if (props.type === 1) return { bendpoints: ['value', ''], move: ['value', ''] };
      return { labelEditingProvider: ['value', ''] };
    }
    const data = {
      bendpoints: ['value', ''], // 禁止拖动线
      contextPadProvider: ['value', ''], // 禁止点击节点出现contextPad
      labelEditingProvider: ['value', ''], // 禁止双击节点出现label编辑框
      move: ['value', ''], // 禁止节点拖动
    };
    return data;
  });

  // 初始化bpmn模拟器
  function initBpmn() {
    const container: any = unref(bpmnContainer);
    let modelerMap = [BpmnModeler, SimpleModeler, TaskModeler];
    let Modeler = props?.openPreview ? PreviewModeler : modelerMap[props.type]; // 查看流程用单独的modeler，否则会出现数据污染。
    state.bpmn = markRaw(
      new Modeler({
        container,
        // 添加控制板
        propertiesPanel: {
          parent: '#properties',
        },
        //键盘快捷键
        keyboard: {
          bindTo: document,
        },
        additionalModules: [
          AutoPlaceModule,
          minimapModule, //小地图
          customTranslateModule, // 翻译
          unref(getAdditionalModules),
        ],
        flowInfo: {
          flowNodes: props.flowNodes,
          nodeList: nodeMap,
          isPreview: props.isPreview,
          lineKeyList: props.lineKeyList,
        },
        type: props.type,
      }),
    );
    state.jnpfData = state.bpmn.get('jnpfData');
    state.jnpfData.setValue('global', {});
    for (let key in props.flowNodes) {
      state.jnpfData.setValue(key, props.flowNodes[key]);
    }
    const modeling: any = state.bpmn.get('modeling');
    handleInitListeners(state.bpmn, modeling);
    const xml = props.flowXml ? getRealXml(props.flowXml) : props.type === 1 ? simpleDefaultXml : defaultXml;
    let startBound = getBound('startEvent');
    let endBound = getBound('endEvent');
    if (startBound?.x && endBound?.x) {
      if (Number(endBound.y) - Number(startBound.y) > Number(endBound.x) - Number(startBound.x))
        state.bpmn?.get('jnpfData').setValue('layout', { value: 'vertical' });
    }
    state.bpmn.importXML(xml).then(() => {
      handleFlowState(modeling);
    });
    if (props.isPreview || props.disabled) state.bpmn.get('keyboard').unbind();
  }
  function handleInitListeners(bpmn, modeling) {
    const eventBus: any = bpmn.get('eventBus');
    // 监听新增元素
    bpmn.on('shape.added', ({ element }) => {
      handleShapeAdded(element);
    });
    bpmn.on('create.end', ({ shape }) => {
      handleShapeCreateEnd(shape, modeling);
    });
    // 监听点击事件
    eventBus.on('element.click', e => {
      e.originalEvent.preventDefault();
      e.originalEvent.stopPropagation();
      let element = e.element;
      handleElementClick(element);
    });
    eventBus.on('custom.message', e => {
      e.messageType === 'warning' && Message.warning(e?.context || '');
      e.messageType === 'success' && Message.success(e?.context || '');
    });
    eventBus.on('resize.start', function (event) {
      const context = event.context;
      const shape = context.shape;
      // 禁用 Group元素的大小手动拖动
      if (shape.businessObject.$type === 'bpmn:Group') event.preventDefault();
    });
    if (props.isPreview) return;
    // 监听节点选中
    bpmn.on('selection.changed', e => {
      handleSelectionChanged(e);
    });
    // 监听新建线条
    bpmn.on('connection.added', e => {
      handleConnectionAdded(e, modeling);
    });
    // 监听移除事件
    bpmn.on('shape.removed', ({ element }) => {
      handleShapeRemoved(element);
    });
    // 监听移动事件
    eventBus.on('drag.cleanup', e => {
      handleElementMoveEnd(e);
    });
    bpmn.on('copyPaste.copyElement', ({ element }) => {
      if (element.type === bpmnStart) {
        Message.warning('流程发起节点不能复制');
        return false;
      }
      if (element.type === bpmnEnd) {
        Message.warning('流程结束节点不能复制');
        return false;
      }
      if (element.wnType === typeTrigger || changeTypeByTrigger[element.wnType] || changeTypeByTaskShape[element.wnType]) {
        Message.warning('流程任务节点不能复制');
        return false;
      }
    });
    // 监听事件，自定义删除规则
    eventBus.on('commandStack.canExecute', e => {
      handleCommandStackCanExecute(e, modeling);
    });
    eventBus.on('commandStack.elements.move.postExecuted', e => {
      handleCommandMovePostExecuted(e, modeling);
    });
    eventBus.on('commandStack.connection.reconnect.postExecuted', e => {
      handleCommandReconnectPostExecuted(e, modeling);
    });
  }
  function handleShapeAdded(element) {
    element.offset = 0;
    if (element.type === bpmnLabel || element.type === bpmnGroup || !element.id) return;
    let data: any = {
      nodeId: element.id,
      type: getWnType(element),
    };
    if (getNodeName(data.type)) data.nodeName = getNodeName(data.type);
    if (element.wnName) data.nodeName = element.wnName;
    if (data.type === typeTask) data.assigneeType = 6;
    let currentData = state.jnpfData.getValue(element.id);
    element.wnType = data.type;
    if (currentData) {
      state.jnpfData.setValue(element.id, { ...currentData });
    } else {
      //全局属性开启签名后，新增的节点默认开启
      if (element.wnType == 'approver' || element.wnType === typeProcessing) data.hasSign = state.jnpfData.getValue('global', 'hasSign') || false;
      state.jnpfData.setValue(element.id, { ...data });
    }
    state.element = element;
  }
  function handleSelectionChanged(e) {
    nextTick(() => {
      let selection = state.bpmn?.get('selection');
      if (e.newSelection.length === 0) return (state.element = null);
      e.newSelection.forEach(element => {
        // 禁止选中条件
        if (props.type === 0) {
          if (element?.type === typeLabel) selection.deselect(element);
          // 判断source的分流规则选择 如果是 并行和选择分支类型无法被选中
          if (element.type === bpmnSequenceFlow) {
            let jnpfData = state.bpmn?.get('jnpfData');
            let sourceData = jnpfData.getValue(element.source?.id);
            let divideRule = sourceData.divideRule;
            if (divideRule === typeParallel || divideRule === typeChoose) {
              return (state.element = undefined);
            }
          }
        }
        // 禁止选中group标签
        if (element?.type === bpmnGroup) selection.deselect(element);
        if (element && props.type === 1) {
          if (
            element.type === bpmnSequenceFlow &&
            !((element.source?.type === bpmnInclusive && element.source?.wnType === typeInclusion) || element.source?.type === bpmnExclusive)
          )
            return (state.element = undefined);
          if (element.type === bpmnGateway || element.source?.wnType === typeConfluence) return (state.element = undefined);
          if (element.source?.wnType === typeConfluence && element.target?.wnType === typeConfluence) return (state.element = undefined);
        }
        if (element && props.type === 2 && (element.wnType == typeStart || element.type === bpmnSequenceFlow)) return (state.element = undefined);
        const list = [bpmnGroup, bpmnLabel, bpmnParallel, bpmnInclusive, bpmnExclusive];
        if (element && (list.includes(element.type) || changeTypeByTaskShape[element?.target?.wnType] || element?.target?.wnType === typeTrigger))
          return (state.element = undefined);
        state.element = element;
      });
    });
  }
  function handleConnectionAdded(e, modeling) {
    nextTick(() => {
      handleLabelPosition(e, modeling);
    });
  }
  function handleShapeRemoved(element) {
    if (state.jnpfData.getValue(element.id)) delete state.jnpfData.data[element.id];
  }
  function handleElementMoveEnd(e) {
    let elementRegistry = state.bpmn.get('elementRegistry');
    let treeBuilder = new BPMNTreeBuilder(elementRegistry.getAll()); // 实例化工具类
    if (e?.context?.shape) {
      treeBuilder.resizeGroupShape([e?.context?.shape], state.bpmn);
    } else treeBuilder.resizeGroupShape(e?.context?.shapes || [], state.bpmn);
  }
  function handleElementClick(element) {
    if (element.wnType == 'subFlow' && props.isPreview && nodeMap.get(element.id)?.type) return emit('viewSubFlow', element.id);
    // 清除上一次选中的元素颜色
    if (elementClickId) {
      let marker = document.querySelector<HTMLElement>('marker');
      let oldElementNode = document.querySelector<HTMLElement>(`g.djs-element[data-element-id=${elementClickId.id}] .djs-visual `)!;
      let oldPath = oldElementNode?.querySelector('path');
      if (oldPath) oldPath.setAttribute('style', `stroke: #A2B9D5;marker-end:url(#${marker?.id}); visibility: visible;`);
    }
    if (element.type == bpmnSequenceFlow && !props.isPreview) {
      let elementNode = document.querySelector<HTMLElement>(`g.djs-element[data-element-id=${element.id}] .djs-visual `)!;
      if (elementNode) {
        let path = elementNode.querySelector('path');
        if (
          (path &&
            !((element.source.type === bpmnInclusive && element.source.wnType === typeInclusion) || element.source.type === bpmnExclusive) &&
            props.type === 1) ||
          (element.source?.type === bpmnInclusive && element.target?.type === bpmnInclusive) ||
          element.source?.wnType === typeConfluence
        ) {
        } else if (path) {
          elementClickId = element; // 记录上一次点击的元素 取消点击后需要清除选中颜色。
          path.setAttribute('style', `stroke: rgb(24,131,255);marker-end:url(#${'bpmnSequenceFlowActiveId'}); visibility: visible;`);
        }
      }
    }
    // 禁用Group元素的拖动
    let dragging = state.bpmn.get('dragging');
    if (element.type === bpmnGroup || element.type === bpmnLabel) dragging.setOptions({ manual: true });
    else dragging.setOptions({ manual: false });
  }
  /**
   * autoLayoutDel 获取偏移坐标的子元素 如遇到坐标重新计算偏移坐标
   * （如果下一个元素不是网关则入栈遍历获取到相同偏移量的组别进行统一偏移）
   */
  function handleDeleteGateway(gateway: any, delElement: any, type: 'gateway' | 'task' | 'group') {
    let elementRegistry: any = state.bpmn.get('elementRegistry');
    let modeling: any = state.bpmn.get('modeling');
    let allElements = elementRegistry.getAll();
    let newGateway: any = [];
    let treeBuilder = new BPMNTreeBuilder(allElements);
    let groupH: number = 0;
    if (type === 'gateway') {
      let confluenceElement = elementRegistry.get(gateway.id + '_confluence');
      let sourceList = NodeUtils.getLastElementList(confluenceElement, allElements).filter(i => i.id != delElement.id);
      let targetList = NodeUtils.getNextElementList(confluenceElement, allElements);
      let elementList = treeBuilder.getElementsByGateway(gateway); // 通过分流元素获取所有的分流 合流网关内部的所有元素
      modeling.removeElements([gateway, confluenceElement]);
      modeling.connect(sourceList[0], targetList[0]);
      modeling.moveElements(elementList, { x: 0, y: -(DEFAULT_DISTANCE + typeConfig[bpmnGateway].renderer.attr.height) });
      autoLayoutDel(targetList[0]);
    } else if (type === 'task') {
      let h = delElement.y - (gateway.y + DEFAULT_DISTANCE + gateway.height);
      autoLayoutDel(delElement, [], h);
    } else if (type === 'group') {
      let h = delElement.y - (gateway.y + DEFAULT_DISTANCE + gateway.height);
      autoLayoutDel(delElement, [], h);
    }
    while (newGateway.length > 0) {
      let childrenMaxY: number = -Infinity,
        h: number = 0;
      let gateway: any = newGateway.shift();
      // 如果是合流网关 需要获取到所有的分流到合流网关内的所有元素 如果有触发节点
      if (gateway.wnType === typeConfluence) {
        let elementList = treeBuilder.getElementsByGateway(elementRegistry.get(gateway.id.replace('_confluence', '')));
        elementList.map((item: any) => {
          if (item.y > childrenMaxY) {
            childrenMaxY = Number(item.y) || 0;
          }
        });
      } else {
        let lastElementList: any = NodeUtils.getLastElementList(gateway, elementRegistry.getAll());
        lastElementList.map((item: any) => {
          if (item.y > childrenMaxY) {
            childrenMaxY = Number(item.y) || 0;
          }
          // 如果该元素的下一个元素内有任务节点 则更新childrenMaxY
          let nextList = NodeUtils.getNextElementList(item, elementRegistry.getAll())?.filter(
            element => element.wnType === typeTrigger && element.y > childrenMaxY,
          );
          nextList.map(nextElement => {
            // 获取到该分组内的所有元素
            let customGroupId = nextElement.businessObject.$attrs?.customGroupId;
            let groupList = elementRegistry.getAll()?.filter(element => element.businessObject.$attrs?.customGroupId === customGroupId);
            groupList.map((element: any) => {
              if (element.y > childrenMaxY) {
                childrenMaxY = Number(element.y) || 0;
              }
            });
          });
        });
      }
      if (gateway.y - childrenMaxY != DEFAULT_DISTANCE) h = gateway.y - (childrenMaxY + DEFAULT_DISTANCE + typeConfig[bpmnTask].renderer.attr.height);
      let nextList = NodeUtils.getNextElementList(gateway, elementRegistry.getAll());
      autoLayoutDel(nextList[0], [gateway], groupH ? groupH : h);
    }
    function autoLayoutDel(element: any, oldList?: any[], currentHeight?: number) {
      let queue: any = [element];
      let childrenMaxY: number = -Infinity,
        h: number = 0,
        groupList: any = [];
      while (queue.length > 0) {
        let current: any = queue.shift();
        if (currentHeight) {
          groupList = oldList || [];
          h = currentHeight;
        } else {
          let lastElementList: any = NodeUtils.getLastElementList(current, elementRegistry.getAll());
          lastElementList.map((item: any) => {
            if (item.y > childrenMaxY) childrenMaxY = Number(item.y) || 0;
          });
          if (!h && current.y - childrenMaxY != DEFAULT_DISTANCE) h = current.y - (childrenMaxY + DEFAULT_DISTANCE + typeConfig[bpmnTask].renderer.attr.height);
        }
        if (hasGatewayType.has(current.wnType)) {
          groupList.push(current);
          let l = treeBuilder.getElementsByGateway(current) || [];
          l.map((item: any) => {
            groupList.push(elementRegistry.get(item.id));
          });
          let confluenceElement = elementRegistry.get(current.id + '_confluence');
          groupList.push(confluenceElement);
          if (confluenceElement.outgoing && confluenceElement.outgoing.length > 0) {
            confluenceElement.outgoing.map((item: any) => {
              let nextElement = item.target;
              queue.push(nextElement);
            });
          }
        } else if (current.wnType === typeConfluence) {
          let gatewayElement = state.bpmn.get('elementRegistry').get(current.id.replace('_confluence', ''));
          let elementObj = treeBuilder.onComputerMaxElementH(state.bpmn, current, gatewayElement, groupList);
          newGateway = elementObj?.list || [];
          groupH = elementObj?.h || 0;
          continue;
        } else {
          groupList.push(current);
          if (current.outgoing && current.outgoing.length > 0) {
            current.outgoing.map((item: any) => {
              let nextElement = item.target;
              queue.push(nextElement);
            });
          }
        }
      }
      // 获取所有的分流合流元素
      if (groupList && groupList.length > 0) {
        let list: any = [];
        groupList.map((item: any) => {
          let element = elementRegistry.get(item.id);
          list.push(element);
        });
        // 如果分流到合流内的最大y值也在移动的list内 则需要将最大值的出线方向元素全部进行偏移。
        modeling.moveElements(list, { x: 0, y: -h });
      }
    }
  }
  // 删除后自动排序
  function onAutoPosition() {
    let elementRegistry: any = state.bpmn.get('elementRegistry');
    let modeling: any = state.bpmn.get('modeling');
    let allElements = elementRegistry.getAll();
    let treeBuilder = new BPMNTreeBuilder(allElements); // 实例化工具类
    let bpmnTree = treeBuilder.constructTree(); // 构建树状数据结构
    let visited: any = new Map(); // 用于防止重复访问
    treeBuilder.calculateVirtualWidth(bpmnTree, elementRegistry); // 计算虚拟宽度
    let shapeList: any = []; // 修改触发节点旁的连接线坐标
    let confluenceMap: any = new Map();
    treeBuilder.traverseTreeBFS(bpmnTree, node => {
      node?.offset && node.x != node.offset.x && visited.set(node.id, node);
      if (node?.children?.length > 0) {
        let hasTrigger = node.children.some(o => o.wnType === typeTrigger);
        let hasConfluence = node.children.some(o => o.wnType === typeConfluence);
        let shape = elementRegistry.get(node.id);
        let confluence: any;
        if (shape.outgoing?.length) {
          shape.outgoing.map((connect: any) => {
            if (connect.target.wnType === typeTrigger) hasTrigger = true;
            if (connect.target.wnType === typeConfluence) {
              confluence = connect.target;
              hasConfluence = true;
            }
          });
        }
        if (hasTrigger && hasConfluence) shapeList.push({ shape: elementRegistry.get(node.id), treeShape: node, confluence: confluence });
        if (node.wnType === typeConfluence || shape.wnType === typeConfluence) confluenceMap.set(node.id, node);
      }
    });
    treeBuilder.formatCanvas(Array.from(visited.values()), modeling, elementRegistry);
    shapeList.map(({ shape, treeShape, confluence }) => {
      let confluenceElement = confluenceMap.get(confluence.id);
      let x = (treeShape.offset?.x ? treeShape.offset.x : treeShape.x) + treeShape.width / 2 - treeShape.virtualWidth / 2 + 120;
      let newWaypoints = [
        { x: shape.x, y: shape.y + shape.height / 2 },
        { x: x, y: shape.y + shape.height / 2 },
        { x: x, y: confluenceElement.y },
        { x: confluenceElement.x, y: confluenceElement.y },
      ];
      let connect = shape.outgoing[0];
      if (shape.outgoing?.length) connect = shape.outgoing.find(connect => connect.target.wnType != typeTrigger);
      // 线条的source节点的出线元素未发生过偏移 （list中不包含）则不偏移。
      if (shape.outgoing.length > 0) {
        let hasConnect = false;
        if (connect.target?.wnType === typeConfluence && connect.source?.outgoing?.length > 1) hasConnect = true;
        hasConnect && modeling.updateWaypoints(connect, newWaypoints);
      }
    });
  }
  async function handleCommandStackCanExecute(e, modeling) {
    let elementRegistry: any = state.bpmn.get('elementRegistry');
    let hasConfirm = false;
    if (e.command == 'elements.delete') {
      e.defaultPrevented = true;
      const elements = e.context.elements;
      const hasStartElement = elements.some(o => o.type == bpmnStart);
      hasStartElement && Message.warning('流程发起节点不能删除');
      elements.some(o => hasGatewayType.has(o.wnType)) && Message.warning('流程网关节点不能删除');
      let newElements = elements.filter(o => {
        if (props.type === 1) return o.type != bpmnStart && o.type != bpmnLabel && o.type != bpmnSequenceFlow && !hasGatewayType.has(o.wnType);
        return o.type != bpmnStart;
      });
      let hasGroup = false;
      if (props.type === 1) {
        if (newElements?.length === 1) {
          let element = newElements[0];
          let incoming = element.incoming;
          let outgoing = element.outgoing;
          let source = element.incoming[0]?.source;
          let target: any;
          let oldTargetList: any = [];
          if (element.outgoing?.length) {
            element.outgoing.some((connect: any) => {
              if (connect.target.wnType != typeTrigger) {
                target = connect.target;
                return;
              }
            });
          }
          // 简流内删除分组内元素 触发元素
          if (element.wnType === bpmnTrigger) {
            let list: any = [].concat(elements);
            let groupId = element.businessObject.$attrs.customGroupId;
            elementRegistry.forEach(e => {
              if ((groupId && e.businessObject.$attrs.customGroupId === groupId) || e.id === groupId) {
                if (elements.some(o => o.id != e.id)) {
                  hasConfirm = true;
                  list.push(e);
                }
              }
            });
            if (hasConfirm) {
              await onCreateConfirm().then(() => {
                modeling.removeElements(list);
              });
            } else modeling.removeElements(list);
            hasGroup = true;
            hasConfirm = true;
            let confluence = getConfluence(source);
            confluence && handleDeleteGateway(source, confluence, 'group');
          }
          // 执行元素
          if (changeTypeByTaskShape[element.wnType]) {
            newElements?.map((element: any) => {
              if (element.outgoing.length > 1) {
                element.outgoing.map((connect: any) => {
                  let target = connect.target;
                  if (target) {
                    modeling.connect(source, target);
                    oldTargetList.push(target);
                  }
                });
              }
            });
            modeling.removeElements(newElements);
            hasGroup = true;
            let triggerElement = elementRegistry
              .getAll()
              .find(item => item.wnType === typeTrigger && item.businessObject.$attrs?.customGroupId === source.businessObject.$attrs?.customGroupId);
            let element = triggerElement.incoming[0].source; //
            let confluence = getConfluence(element);
            hasConfirm = true;
            confluence && handleDeleteGateway(source, confluence, 'group');
          }
          if (element.type === bpmnTask && source && target && element.wnType != typeTrigger) {
            if (source.type === bpmnStart && target.type === bpmnEnd) {
              Message.warning('请确保至少包含一个任务节点');
              return false;
            }
            if (hasGatewayType.has(incoming[0]?.source?.wnType) && hasGatewayType.has(outgoing[0]?.target?.wnType)) {
              Message.warning('由于存在分支，无法删除该元素');
              return false;
            }
            if (source.wnType === typeChoose && target.wnType === typeSubFlow) {
              Message.warning('选择分支不能直接连线子流程，无法删除该元素');
              return false;
            }
            if (source.wnType === typeConfluence && hasGatewayType.has(target.wnType)) {
              Message.warning('无法删除该节点，前后节点不能同时存在分支');
              return false;
            }
            // 如果有触发节点 还需要删除所有的触发分组内的所有元素。
            let hasGroup = false;
            outgoing.map(e => {
              if (e.target.wnType === typeTrigger) {
                let treeBuilder = new BPMNTreeBuilder(elementRegistry.getAll()); // 实例化工具类
                let list: any = treeBuilder.getGroupElementById(e.target.businessObject?.$attrs?.customGroupId, state.bpmn) || [];
                newElements = newElements.concat(list);
                hasConfirm = true;
                if (list.length > 0) hasGroup = true;
              }
            });
            let x = outgoing[0];
            if (outgoing.length > 0)
              outgoing.map(connect => {
                if (connect.target?.wnType != bpmnTrigger) x = connect;
              });
            if (hasGatewayType.has(incoming[0]?.source?.wnType) && x?.target?.wnType === typeConfluence) {
              let sourceElement = incoming[0]?.source;
              let length = sourceElement?.outgoing?.length || 0;
              if (length >= 2) {
                modeling.removeConnection(incoming[0]);
                // 如果有分组元素 则需要重新计算合流节点
                if (hasGroup) {
                  handleDeleteGateway(source, target, 'task');
                }
              }
              if (length <= 2) handleDeleteGateway(sourceElement, element, 'gateway');
              modeling.removeElements(newElements);
            } else {
              modeling.removeElements(newElements);
              if (target.incoming?.length === 0) modeling.connect(source, target);
              if (source.outgoing?.length === 0) modeling.connect(source, target);
              if (source.outgoing?.length > 0) {
                // 如果都是触发节点 则也需要连线
                let isConnect = true;
                source.outgoing.map(connect => {
                  if (connect.target.wnType != typeTrigger) isConnect = false;
                });
                isConnect && modeling.connect(source, target);
              }
              if (oldTargetList.length) {
                oldTargetList.map((target: any) => {
                  handleDeleteGateway(source, target, 'task');
                });
              } else {
                handleDeleteGateway(source, target, 'task');
              }
            }
          }
        }
        onAutoPosition();
        if (props.type === 1 || hasGroup) {
          let treeBuilder = new BPMNTreeBuilder(elementRegistry.getAll()); // 实例化工具类
          treeBuilder.resizeGroupShape(elementRegistry.getAll(), state.bpmn);
        }
        return;
      } else {
        elements.map((element: any) => {
          let source = element.incoming[0]?.source;
          let target = element.outgoing[0]?.target;
          if (source?.type === bpmnStart && target?.type === bpmnEnd) {
            newElements.push(element.outgoing[0]);
            newElements.push(element.incoming[0]);
            modeling.removeElements([newElements]);
            return false;
          }
        });
        if (elements?.length === 1 && elements[0]?.type === bpmnGroup) return false;
      }
      elements.map((element: any) => {
        let groupId = element.businessObject.$attrs.customGroupId;
        if (element.wnType === typeTrigger) {
          elementRegistry.forEach(e => {
            if ((groupId && e.businessObject.$attrs.customGroupId === groupId) || e.id === groupId) {
              if (newElements.some(o => o.id != e.id)) {
                hasConfirm = true;
                newElements.push(e);
              }
            }
          });
        }
        // 删除触发节点 默认删除所有的执行节点
        if (changeTypeByTrigger[element.wnType]) {
          elementRegistry.forEach(e => {
            if (changeTypeByTrigger[e.wnType] || changeTypeByTaskShape[e.wnType]) {
              hasConfirm = true;
              newElements.push(e);
              if (e.incoming.length) newElements = elements.concat(e.incoming.filter(connect => connect.source.type === bpmnStart) || []);
            }
          });
        }
        // 删除执行节点，判断上一个节点及下一个节点是否是开始节点连线结束节点 是的话删除多余线条
        if (changeTypeByTaskShape[element.wnType]) {
          if (element.outgoing.length) {
            element.outgoing.map(connect => {
              if (connect.target.type === bpmnEnd) {
                element.incoming.map(incoming => {
                  newElements.push(incoming);
                });
                newElements.push(connect);
              }
            });
          }
        }
      });
      nextTick(() => {
        if (hasConfirm) {
          onCreateConfirm().then(() => onRemoveElements(newElements, modeling));
        } else onRemoveElements(newElements, modeling);
      });
    }
    if (e.command == 'shape.delete') {
      let element = e.context.shape;
      let source = element.incoming[0]?.source;
      let target = element.outgoing[0]?.target;
      if (source?.type === bpmnStart && target?.type === bpmnEnd) {
        modeling.removeElements([element.outgoing[0], element.incoming[0], element]);
        return;
      }
      let elements: any = [];
      elements.push(element);
      elements.map((element: any) => {
        let groupId = element.businessObject.$attrs.customGroupId;
        if (element.wnType === typeTrigger) {
          elementRegistry.forEach(e => {
            if ((groupId && e.businessObject.$attrs.customGroupId === groupId) || e.id === groupId) {
              if (elements.some(o => o.id != e.id)) {
                hasConfirm = true;
                elements.push(e);
              }
            }
          });
        }
        // 删除触发节点 默认删除所有的执行节点
        if (changeTypeByTrigger[element.wnType]) {
          elementRegistry.forEach(e => {
            if (changeTypeByTrigger[e.wnType] || changeTypeByTaskShape[e.wnType]) {
              hasConfirm = true;
              elements.push(e);
              if (e.incoming.length) elements = elements.concat(e.incoming.filter(connect => connect.source.type === bpmnStart) || []);
            }
          });
        }
        // 删除执行节点，判断上一个节点及下一个节点是否是开始节点连线结束节点 是的话删除多余线条
        if (changeTypeByTaskShape[element.wnType]) {
          if (element.outgoing.length) {
            element.outgoing.map(connect => {
              if (connect.target.type === bpmnEnd) {
                element.incoming.map(incoming => {
                  elements.push(incoming);
                });
              }
            });
          }
        }
      });
      nextTick(() => {
        if (hasConfirm) {
          onCreateConfirm().then(() => onRemoveElements(elements, modeling));
        } else onRemoveElements(elements, modeling);
      });
    }
  }
  function handleCommandMovePostExecuted(e, modeling) {
    let elementRegistry: any = state.bpmn.get('elementRegistry');
    if (e.command === 'elements.move' && props.type === 1) {
      let allConnections = e.context.closure.allConnections;
      allConnections &&
        Object.values(allConnections).map((connection: any) => {
          if (connection.label) {
            // 只适应简流
            let newConnect = elementRegistry.get(connection.id);
            if (connection.label.wnType === typeCondition) {
              let labelCenter = NodeUtils.updateLabelWaypoints(connection, elementRegistry, state?.jnpfData);
              let label = elementRegistry.get(connection.label.id);
              label.x = labelCenter.x;
              label.y = labelCenter.y;
              modeling.updateProperties(label, {});
            } else {
              modeling.moveElements([connection.label], {
                x: 0,
                y: newConnect.source.y + newConnect.source.height + 48 - newConnect.label.y,
              });
            }
          }
        });
    }
  }
  function handleCommandReconnectPostExecuted(e: any, modeling: any) {
    if (props.type === 1 && e?.context?.connection && e?.context?.newSource) {
      let connection = e.context.connection;
      let newSource = e.context.newSource;
      if (connection.label) {
        let x = newSource.x + newSource.width / 2 - 14 - connection.labels[0]?.x;
        // 如果source有多个出线 则取target坐标计算 反之取source
        if (newSource.outgoing?.length > 1) {
          x = connection.target.x + connection.target.width / 2 - 14 - connection.labels[0].x;
          // connection 如果是合流网关则不适用
          if (connection.target.wnType === typeConfluence)
            x = newSource.outgoing.find(connect => connect.id === connection.id)?.waypoints[0].x - 14 - connection.labels[0].x;
        }
        modeling.moveElements([connection.label], {
          x: x,
          y: newSource.y + newSource.height + 40 - connection.labels[0].y,
        });
      }
    }
    if (e.command === 'connection.reconnect' && e.context?.newSource?.id === e.context?.newTarget?.id) {
      modeling.removeConnection(e.context.connection);
    }
  }
  function getRealXml(xml) {
    return new XMLSerializer().serializeToString(NodeUtils.autoDelGateWay(xml, props.type, nodeMap, props.isPreview));
  }
  /**
   * 获取wnType 新建时取element中的wnType,编辑时取数据中的wnType
   * @param element  节点
   */
  function getWnType(element) {
    const type = state.jnpfData.getValue(element.id, 'type');
    return element.wnType ? element.wnType : type ? type : conversionWnType[element.type];
  }
  function getNodeName(type) {
    if (type == typeStart) return '流程发起';
    if (type == typeTask) return '审批节点';
    if (type == typeSubFlow) return '子流程';
    if (type == typeEnd) return '流程结束';
  }
  function getElement() {
    return state.element;
  }
  function getBpmn() {
    return state.bpmn;
  }
  function handleFlowState(modeling) {
    if (!props.isPreview) return;
    let flowNodes = props.flowNodes;
    let connectList = state.bpmn
      .get('elementRegistry')
      .getAll()
      .filter((element: any) => is(element, 'bpmn:SequenceFlow') && element?.type != 'label');
    connectList.map((connect: any) => {
      let source = connect.source;
      let target = connect.target;
      if (nodeMap && nodeMap.has(source?.id) && nodeMap.has(target?.id)) {
        if (nodeMap.get(source.id)?.type === '0' && nodeMap.get(target.id)?.type === '0') {
          let waypoints: any = connect.waypoints || [];
          modeling?.updateProperties(connect, {});
          modeling.updateWaypoints(connect, waypoints);
        }
      }
      // 流转图的条件显示。
      if (props.isPreview) {
        let labelCenter = getExternalLabelMid(connect);
        let elementRegistry = state.bpmn.get('elementRegistry');
        let existingElement = elementRegistry.get(connect.businessObject.id + '_label');
        if (flowNodes[connect.id]?.conditions) {
          let text = NodeUtils.getConditionsContent(flowNodes[connect.id]?.conditions, flowNodes[connect.id]?.matchLogic);
          labelCenter = NodeUtils.updateLabelWaypoints(connect, elementRegistry, state?.jnpfData, props.type);
          if (text?.trim()) {
            if (existingElement) {
              existingElement.text = text;
              existingElement.x = labelCenter.x;
              existingElement.y = labelCenter.y;
              modeling.updateProperties(existingElement, {});
            } else {
              if (connect.parent) {
                let labelElement = modeling.createLabel(connect, labelCenter, {
                  id: connect.businessObject.id + '_label',
                  businessObject: connect.businessObject,
                  wnType: 'condition',
                  text: text,
                  di: connect.di,
                });
                labelCenter = NodeUtils.updateLabelWaypoints(connect, elementRegistry, state?.jnpfData, props.type);
                labelElement.text = text;
                labelElement.x = labelCenter.x;
                labelElement.y = labelCenter.y;
                modeling.updateProperties(labelElement, {});
              }
            }
          }
        } else {
          existingElement && modeling.removeElements([existingElement]);
        }
      }
    });
  }
  // 迭代获取最近的合流节点 如果下一个节点时分流节点 则获取到对应的合流节点继续获取下一个节点
  function getConfluence(shape: any) {
    let targetList = NodeUtils.getNextElementList(shape, state.bpmn.get('elementRegistry').getAll());
    let confluence: any;
    targetList.map((element: any) => {
      if (element.wnType === typeConfluence) {
        confluence = element;
        return;
      }
      if (element.wnType != typeTrigger) confluence = getConfluence(element);
      if (hasGatewayType.has(element.wnType)) confluence = getConfluence(state.bpmn.get('elementRegistry').get(element.id + '_confluence'));
    });
    return confluence;
  }
  function onRemoveElements(elements, modeling) {
    modeling.removeElements(elements);
    if (elements.some(o => o.wnType === typeTrigger || changeTypeByTaskShape[o.wnType])) {
      let elementRegistry = state.bpmn.get('elementRegistry');
      let treeBuilder = new BPMNTreeBuilder(elementRegistry.getAll()); // 实例化工具类
      treeBuilder.resizeGroupShape(elements || [], state.bpmn);
    }
  }
  function onCreateConfirm() {
    return new Promise<boolean>((resolve, reject) => {
      Modal.confirm({
        icon: createVNode(ExclamationCircleOutlined),
        title: '是否确认删除该节点',
        content: '删除该节点后会导致相应的执行节点同步删除，确认是否继续删除？',
        onOk() {
          return resolve(true);
        },
        onCancel() {
          return reject(false);
        },
      });
    });
  }
  // 重新生成label标签位置。
  function handleLabelPosition(e, modeling) {
    const { element } = e;
    let labelCenter = getExternalLabelMid(element);
    let existingElement = state.bpmn.get('elementRegistry').get(element.businessObject.id + '_label');
    // 简单流程
    if (props.type === 1) {
      // 条件label显示。
      let data = state?.jnpfData?.getValue(element.id);
      if (element.businessObject?.conditionExpression && data) {
        let text = NodeUtils.getConditionsContent(data.conditions, data.matchLogic);
        labelCenter.y = element.target.y - 30;
        labelCenter.x = element.target.x + element.target.width / 2 - 20;
        if (text?.trim()) {
          if (existingElement) modeling.removeElements([existingElement]);
          labelCenter = NodeUtils.getNewLabelWaypoints(element, state.bpmn.get('elementRegistry'), state?.jnpfData, props.type);
          let labelElement = modeling.createLabel(element, labelCenter, {
            id: element.businessObject.id + '_label',
            businessObject: element.businessObject,
            wnType: 'condition',
            text: text,
            di: element.di,
            y: 0,
          });
          labelElement.x = labelCenter.x;
          labelElement.y = labelCenter.y;
          modeling.updateProperties(labelElement, {});
        }
      } else {
        if (!hasGatewayType.has(element.source.wnType)) {
          if (element.target.wnType === typeConfluence) {
            labelCenter = {
              x: labelCenter.x - 28,
              y: element.target.y - (element.target.y - element.source.y - element.source.height) / 2 - 8,
            };
          } else {
            labelCenter = {
              x: element.target.x + element.target.width / 2 - typeConfig[bpmnLabel].renderer.attr.width / 2,
              y: element.target.y - (element.target.y - element.source.y - element.source.height) / 2 - 8,
            };
          }
          element.businessObject.x = 10;
          // 说明不是分流网关 不显示其样式
          labelCenter.y = element.source.y + element.source.height + 50;
          if (!existingElement && triggerTypeChange[element.target?.wnType]) {
            if (!triggerTypeChange[element.target.wnType]) {
              labelCenter = NodeUtils.getNewLabelWaypoints(element, state.bpmn.get('elementRegistry'), state?.jnpfData);
              let labelElement = modeling.createLabel(element, labelCenter, {
                id: element.id + '_label',
                businessObject: element.businessObject,
                text: 'label',
                di: element.di,
              });
              labelElement.x = labelCenter.x;
              labelElement.y = labelCenter.y;
              modeling.updateProperties(labelElement, {});
            }
          } else {
            let x = element.target.x + element.target.width / 2 - 14;
            if (element.target.wnType === typeConfluence) x = element.source.x + element.source.width / 2 - 14;
            if (element.source.outgoing?.length > 1) {
              x = labelCenter.x;
            }
            if (!triggerTypeChange[element.target.wnType]) {
              modeling.updateLabel(element, 'Label2', {
                ...labelCenter,
                // x,
                width: 128, // 标签的宽度
                height: 28,
              });
            }
          }
        }
      }
    }
    // 标准流程
    if (props.type === 0) {
      // 条件label
      if (element.businessObject?.conditionExpression) {
        if (state?.jnpfData?.getValue(element.id)) {
          let data = state?.jnpfData?.getValue(element.id);
          let text = NodeUtils.getConditionsContent(data.conditions, data.matchLogic);
          if (text?.trim()) {
            let elementRegistry = state.bpmn.get('elementRegistry');
            if (existingElement) {
              let labelCenter = NodeUtils.updateLabelWaypoints(element, elementRegistry, state?.jnpfData);
              existingElement.text = text;
              existingElement.x = labelCenter.x;
              existingElement.y = labelCenter.y;
              modeling.updateProperties(existingElement, {});
            } else {
              let labelCenter = getExternalLabelMid(element);
              labelCenter = NodeUtils.getNewLabelWaypoints(element, elementRegistry, state?.jnpfData, props.type);
              modeling.createLabel(element, labelCenter, {
                id: element.businessObject.id + '_label',
                businessObject: element.businessObject,
                text: text,
                wnType: 'condition',
                di: element.di,
                y: 0,
              });
              let connectLabel = elementRegistry.get(element.businessObject.id + '_label');
              if (Object.keys(labelCenter).length > 0) {
                connectLabel.x = labelCenter.x;
                connectLabel.y = labelCenter.y;
              }
              modeling.updateProperties(connectLabel, {});
            }
          }
        } else {
          if (existingElement) modeling.removeElements([existingElement]);
        }
      } else {
        if (existingElement) {
          if (state?.jnpfData?.getValue(element.id)) {
            let data = state?.jnpfData?.getValue(element.id);
            let text = NodeUtils.getConditionsContent(data.conditions, data.matchLogic);
            modeling.updateLabel(element, text, {
              ...labelCenter,
              wnType: 'condition',
              // x,
              width: 128, // 标签的宽度
              height: 28,
            });
          }
        }
      }
    }
  }
  // 获取开始及结束坐标。
  function getBound(type) {
    let parser = new DOMParser();
    let xmlDoc = parser.parseFromString(decodeURIComponent(props.flowXml), 'text/xml');
    let event = xmlDoc.getElementsByTagNameNS('http://www.omg.org/spec/BPMN/20100524/MODEL', type)[0];
    if (event) {
      let id = event?.getAttribute('id') + '_di';
      let shape = xmlDoc.querySelector(`#${id}`)?.getElementsByTagName('dc:Bounds');
      let bounds: any = {};
      if (shape)
        for (let i = 0; i < shape.length; i++) {
          bounds = shape[i]
            ? {
                x: shape[i].getAttribute('x'),
                y: shape[i].getAttribute('y'),
              }
            : {};
        }
      return bounds;
    }
    return {};
  }
  function handleShapeCreateEnd(shape, modeling) {
    if (shape && shape.wnType === typeTrigger) {
      let groupShapes = modeling.createShape(
        {
          type: 'bpmn:Group',
        },
        { x: shape.x - 25, y: shape.y - 15, width: 250, height: 118 },
        shape.parent,
      );
      modeling.updateProperties(shape, {
        customGroupId: groupShapes.id,
      });
    }
  }

  onMounted(() => {
    initBpmn();
  });

  onUnmounted(() => {
    state.bpmn?.destroy();
  });
</script>
<style lang="less">
  @import url('./style/index.less');
</style>
