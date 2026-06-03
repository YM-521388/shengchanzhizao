<template>
  <div class="center-container-top">
    <div class="button-utils" v-if="flowState == 0 && (props.type === 0 || props.type === 2)">
      <a-tooltip placement="bottom" title="框选">
        <a-button type="text" @click="handleBoxSelect">
          <i class="icon-ym icon-ym-flow-selection" />
        </a-button>
      </a-tooltip>
      <a-tooltip placement="bottom" title="移动">
        <a-button type="text" @click="handleMove">
          <i class="icon-ym icon-ym-flow-move" />
        </a-button>
      </a-tooltip>
      <template v-if="props.type === 0">
        <a-divider type="vertical" />
        <a-tooltip placement="bottom" v-for="(item, index) in flowNodeList" :key="index" :title="item.fullName">
          <a-button
            type="text"
            @click="handleSelect($event, item.type, { ...item.option, shapeType: 'click' })"
            @mousedown="handleSelect($event, item.type, { ...item.option, shapeType: 'mouseDown' })">
            <i :class="item.icon" />
          </a-button>
        </a-tooltip>
      </template>
    </div>
    <div class="version-tip" v-else-if="flowState != 0">
      <InfoCircleFilled class="icon" />
      {{ flowState == 1 ? '启用中' : '已归档' }}的版本不可修改，请
      <a-button type="link" class="px-2px" @click="handleAddVersion">添加新版本</a-button>
    </div>
    <div v-else></div>
    <div class="button-utils right-button-utils">
      <template v-if="(props.type === 0 || props.type === 2) && flowState == 0">
        <a-tooltip placement="bottom" title="撤销">
          <a-button type="text" @click="handleUndo" :disabled="!canUndo">
            <i class="icon-ym icon-ym-undo" />
          </a-button>
        </a-tooltip>
        <a-tooltip placement="bottom" title="重做">
          <a-button type="text" @click="handleRedo" :disabled="!canRedo">
            <i class="icon-ym icon-ym-redo" />
          </a-button>
        </a-tooltip>
        <a-popover trigger="click" placement="bottom" overlayClassName="jnpf-common-history-popover" arrow-point-at-center destroyTooltipOnHide>
          <template #content>
            <div class="history-popover-content">
              <div class="title"><i class="icon-ym icon-ym-flow-history" />历史记录</div>
              <ScrollContainer class="contain">
                <div
                  v-if="stackList.length"
                  class="item"
                  v-for="(item, index) in stackList"
                  :key="index"
                  @click="handleClickStack(item)"
                  :class="{ 'current-item': item.stackId == item.id, 'past-item': item.stackId < item.id }">
                  <i :class="item.icon" />
                  {{ item.fullName }}
                </div>
                <jnpf-empty class="my-85px" v-else />
              </ScrollContainer>
            </div>
          </template>
          <a-tooltip placement="bottom" title="历史记录">
            <a-button type="text">
              <i class="icon-ym icon-ym-flow-history" />
            </a-button>
          </a-tooltip>
        </a-popover>
        <a-divider type="vertical" />
        <a-tooltip placement="bottom" title="横向美化">
          <a-button type="text" @click="handleAutoLayoutByHorizontal">
            <i class="icon-ym icon-ym-beautify-horizontal" />
          </a-button>
        </a-tooltip>
        <a-tooltip placement="bottom" title="纵向美化">
          <a-button type="text" @click="handleAutoLayoutByVertical">
            <i class="icon-ym icon-ym-beautify-vertical" />
          </a-button>
        </a-tooltip>
        <a-divider type="vertical" />
        <a-dropdown>
          <template #overlay>
            <a-menu @click="handleAlign">
              <a-menu-item key="left">
                <a-tooltip placement="right" title="左对齐">
                  <i class="icon-ym icon-ym-flow-align-left" />
                </a-tooltip>
              </a-menu-item>
              <a-menu-item key="center">
                <a-tooltip placement="right" title="水平居中">
                  <i class="icon-ym icon-ym-flow-align-center" />
                </a-tooltip>
              </a-menu-item>
              <a-menu-item key="right">
                <a-tooltip placement="right" title="右对齐">
                  <i class="icon-ym icon-ym-flow-align-right" />
                </a-tooltip>
              </a-menu-item>
            </a-menu>
          </template>
          <a-button type="text">
            <i class="icon-ym icon-ym-flow-align-left" />
          </a-button>
        </a-dropdown>
        <a-dropdown>
          <template #overlay>
            <a-menu @click="handleAlign">
              <a-menu-item key="top">
                <a-tooltip placement="right" title="顶部对齐">
                  <i class="icon-ym icon-ym-flow-align-top" />
                </a-tooltip>
              </a-menu-item>
              <a-menu-item key="middle">
                <a-tooltip placement="right" title="垂直居中">
                  <i class="icon-ym icon-ym-flow-align-middle" />
                </a-tooltip>
              </a-menu-item>
              <a-menu-item key="bottom">
                <a-tooltip placement="right" title="底部对齐">
                  <i class="icon-ym icon-ym-flow-align-bottom" />
                </a-tooltip>
              </a-menu-item>
            </a-menu>
          </template>
          <a-button type="text">
            <i class="icon-ym icon-ym-flow-align-top" />
          </a-button>
        </a-dropdown>
        <a-divider type="vertical" />
      </template>
      <a-dropdown>
        <template #overlay>
          <a-menu @click="handleZoomClick">
            <a-menu-item v-for="item in zoomList" :key="item.id">{{ item.fullName }}</a-menu-item>
          </a-menu>
        </template>
        <a-button type="text" class="w-60px">{{ Math.floor(defaultZoom * 100) + '%' }} <i class="icon-ym icon-ym-unfold !text-10px ml-4px" /></a-button>
      </a-dropdown>
      <a-tooltip placement="bottom" title="缩小视图">
        <a-button type="text" @click="handleZoom(0)" :disabled="defaultZoom < 0.6">
          <i class="icon-ym icon-ym-flow-minus" />
        </a-button>
      </a-tooltip>
      <a-tooltip placement="bottom" title="放大视图">
        <a-button type="text" @click="handleZoom(1)" :disabled="defaultZoom > 2">
          <i class="icon-ym icon-ym-flow-plus" />
        </a-button>
      </a-tooltip>
      <a-divider type="vertical" />
      <a-tooltip placement="bottom" title="导航器">
        <a-button type="text" @click="handleLocation">
          <i class="icon-ym icon-ym-flow-focus" />
        </a-button>
      </a-tooltip>
      <a-tooltip placement="bottom" title="鸟瞰图">
        <a-button type="text" @click="handleShowMiniMap">
          <i class="icon-ym icon-ym-flow-aerial-view" />
        </a-button>
      </a-tooltip>
      <template v-if="flowState == 0">
        <a-divider type="vertical" />
        <a-popover v-if="type !== 1" trigger="click" placement="bottom" overlayClassName="jnpf-flow-common-popover" arrow-point-at-center destroyTooltipOnHide>
          <template #content>
            <div class="jnpf-shortcut-key-popover">
              <div class="left">
                <div class="item-contain" v-for="(item, index) in leftShortcutKeyList" :key="index">
                  <div class="title">{{ item.title }}</div>
                  <div class="items" v-for="(child, cIndex) in item.children" :key="cIndex">
                    <div class="items-title">{{ child.title }}</div>
                    <div class="items-keys" v-for="(sub, sIndex) in child.children" :key="sIndex">
                      <div>{{ sub.title }}</div>
                    </div>
                  </div>
                </div>
              </div>
              <a-divider type="vertical" class="h-auto mx-16px"></a-divider>
              <div class="right">
                <template v-for="item in rightShortcutKeyList" :key="item.title">
                  <div class="item-contain">
                    <div class="title">{{ item.title }}</div>
                    <div class="items" v-for="(child, cIndex) in item.children" :key="cIndex">
                      <div class="items-title">{{ child.title }}</div>
                      <div class="items-keys" v-for="(sub, sIndex) in child.children" :key="sIndex">
                        <div>{{ sub.title }}</div>
                      </div>
                    </div>
                  </div>
                </template>
              </div>
            </div>
          </template>
          <a-tooltip placement="bottom" title="热键">
            <a-button type="text">
              <i class="icon-ym icon-ym-flow-shortcut" />
            </a-button>
          </a-tooltip>
        </a-popover>
        <a-popover v-else trigger="click" placement="bottom" overlayClassName="jnpf-flow-common-popover" arrow-point-at-center destroyTooltipOnHide>
          <template #content>
            <div class="jnpf-shortcut-key-popover">
              <div class="left">
                <div class="item-contain" v-for="(item, index) in shortcutKeyList" :key="index">
                  <div class="title">{{ item.title }}</div>
                  <div class="items" v-for="(child, cIndex) in item.children" :key="cIndex">
                    <div class="items-title">{{ child.title }}</div>
                    <div class="items-keys" v-for="(sub, sIndex) in child.children" :key="sIndex">
                      <div>{{ sub.title }}</div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </template>
          <a-tooltip placement="bottom" title="热键">
            <a-button type="text">
              <i class="icon-ym icon-ym-flow-shortcut" />
            </a-button>
          </a-tooltip>
        </a-popover>
      </template>
      <!-- <a-tooltip placement="bottom" title="帮助">
        <a-button type="text" @click="handleShowHelp">
          <i class="icon-ym icon-ym-tip" />
        </a-button>
      </a-tooltip> -->
    </div>
  </div>
</template>
<script lang="ts" setup>
  import { reactive, toRefs } from 'vue';
  import { flowNodeList } from '../../helper/componentMap';
  import { useMessage } from '@/hooks/web/useMessage';
  import { bpmnEnd, bpmnGroup, bpmnSequenceFlow, bpmnStart, bpmnTask, typeProcessing, typeSubFlow, typeTrigger } from '../../bpmn/config/variableName';
  import { BPMNTreeBuilder } from '../../bpmn/utils/constructTreeUtil';
  import { typeConfig } from '../../bpmn/config';
  import { DEFAULT_CONNECT, DEFAULT_DISTANCE } from '../../bpmn/config/constants';
  import { NodeUtils } from '../../bpmn/utils/nodeUtil';
  import { InfoCircleFilled } from '@ant-design/icons-vue';
  import { ScrollContainer } from '@/components/Container';

  interface State {
    defaultZoom: number;
    canUndo: boolean;
    canRedo: boolean;
    stackList: any[];
  }

  const emit = defineEmits(['addVersion']);
  const props = defineProps(['bpmn', 'flowState', 'isPreview', 'type']);
  defineExpose({ updateCanRedoAndUndo });
  const { createMessage } = useMessage();
  const state = reactive<State>({
    defaultZoom: 1,
    canUndo: false,
    canRedo: false,
    stackList: [],
  });
  const { defaultZoom, canUndo, canRedo, stackList } = toRefs(state);
  const zoomList = [
    { fullName: '150%', id: '1.5' },
    { fullName: '120%', id: '1.2' },
    { fullName: '100%', id: '1' },
    { fullName: '80%', id: '0.8' },
    { fullName: '50%', id: '0.5' },
  ];
  const leftShortcutKeyList: any[] = [
    {
      title: '选中节点/连接线时',
      children: [
        { title: '删除', children: [{ title: 'Delete' }] },
        { title: '全选', children: [{ title: 'Ctrl' }, { title: 'A' }] },
        { title: '复制', children: [{ title: 'Ctrl' }, { title: 'C' }] },
        { title: '粘贴', children: [{ title: 'Ctrl' }, { title: 'V' }] },
      ],
    },
    {
      title: '节点框选',
      children: [{ title: '框选', children: [{ title: 'L' }] }],
    },
  ];
  const rightShortcutKeyList: any[] = [
    {
      title: '撤销/重做',
      children: [
        { title: '撤销', children: [{ title: 'Ctrl' }, { title: 'Z' }] },
        { title: '重做', children: [{ title: 'Ctrl' }, { title: 'Y' }] },
      ],
    },
    {
      title: '缩放画布',
      children: [
        { title: '放大', children: [{ title: 'Ctrl' }, { title: '+' }] },
        { title: '缩小', children: [{ title: 'Ctrl' }, { title: '-' }] },
        { title: '自适应', children: [{ title: 'Ctrl' }, { title: '0' }] },
      ],
    },
  ];
  const shortcutKeyList: any[] = [
    {
      title: '选中节点/连接线时',
      type: 0,
      children: [{ title: '删除', children: [{ title: 'Delete' }] }],
    },
    {
      title: '缩放画布',
      children: [
        { title: '放大', children: [{ title: 'Ctrl' }, { title: '+' }] },
        { title: '缩小', children: [{ title: 'Ctrl' }, { title: '-' }] },
        { title: '自适应', children: [{ title: 'Ctrl' }, { title: '0' }] },
      ],
    },
  ];
  const commandMap = {
    'shape.create': '新增节点',
    'elements.move': '移动节点',
    'elements.delete': '删除节点',
    'connection.create': '新增连接线',
    'connection.updateWaypoints': '移动连接线',
    'connection.reconnect': '重连连接线',
    'beautification.horizontal': '横向美化',
    'beautification.vertical': '纵向美化',
  };

  /**
   * 画布框选
   */
  function handleBoxSelect(event) {
    const lassoTool = props.bpmn.get('lassoTool');
    lassoTool.activateSelection(event);
  }
  /**
   * 画布移动
   */
  function handleMove(event) {
    const handTool = props.bpmn.get('handTool');
    handTool.activateHand(event);
  }
  /**
   * 选择bpmn节点
   * @param type  节点类型
   * @param options  节点配置
   */
  function handleSelect(event: any, type: any, options: any = {}) {
    if (type === typeSubFlow || type === typeTrigger || type === typeProcessing) type = 'bpmn:UserTask';
    const bpmnFactory = props.bpmn.get('bpmnFactory');
    const businessObject = bpmnFactory.create(type);
    const ElementFactory = props.bpmn.get('elementFactory');
    const create = props.bpmn.get('create');
    const shape = ElementFactory.createShape(Object.assign({ type: type, businessObject }, options));
    create.start(event, shape);
  }
  /**
   * 撤销
   */
  function handleUndo() {
    props.bpmn.get('commandStack')?.undo();
    // 记录所有
  }
  /**
   * 重做
   */
  function handleRedo() {
    props.bpmn.get('commandStack').redo();
  }
  /**
   * 更新canUndo、canRedo、stackList
   */
  function updateCanRedoAndUndo() {
    const commandStack = props.bpmn?.get('commandStack');
    state.canUndo = commandStack?.canUndo() || false;
    state.canRedo = commandStack?.canRedo() || false;
    initStackList(commandStack);
  }
  /**
   * 初始化记录
   */
  function initStackList(commandStack) {
    state.stackList = [];
    const list = commandStack._stack || [];
    const stackId = list[commandStack._stackIdx]?.id || 0;
    for (let i = 0; i < list.length; i++) {
      const item = list[i];
      const parentItem = i > 0 ? list[i - 1] : undefined;
      const fullName = commandMap[item.type || item.command] || item.command;
      const icon = 'icon-ym icon-ym-flow-node-approve';
      const newItem = { ...item, fullName, stackId, icon };
      if (item?.command != 'label.create') {
        if (!parentItem) state.stackList.push(newItem);
        else if (parentItem.id != item.id) state.stackList.push(newItem);
      }
    }
  }
  /**
   * 点击历史记录回到当前记录
   * @param item  点击的行数据
   */
  function handleClickStack(item) {
    const targetId = item.id;
    const commandStack = props.bpmn?.get('commandStack');
    const currentId = item.stackId;
    if (targetId < 0 || targetId >= commandStack._stack.length) return;
    const steps = currentId - targetId; // 计算需要撤销或重做的次数
    if (steps > 0) {
      for (let i = 0; i < steps; i++) {
        if (commandStack.canUndo() && getSteps(currentId - (i + 1))) commandStack.undo();
      }
    } else if (steps < 0) {
      for (let i = 0; i < -steps; i++) {
        if (commandStack.canRedo() && getSteps(currentId + (i + 1))) commandStack.redo();
      }
    }
  }
  /**
   * 查询历史记录中是否存在可以撤回的记录
   * @param id
   */
  function getSteps(id) {
    const list = state.stackList.filter(o => o.id == id);
    return !!list.length;
  }
  /**
   * 纵向美化
   */
  function handleAutoLayoutByVertical() {
    if (NodeUtils.getEndlessLoop(props.bpmn)?.length > 0) {
      createMessage.warning('流程存在闭环，无法进行美化操作');
      return;
    }
    props.bpmn.get('canvas').zoom(1);
    const commandStack = props.bpmn?.get('commandStack');
    props.bpmn?.get('jnpfData').setValue('layout', { value: 'vertical' });
    commandStack.execute('vertical.action.same.id.start', { some: 'context' });
    let elementRegistry = props.bpmn.get('elementRegistry');
    let modeling = props.bpmn.get('modeling');
    getCanvasSize('vertical');
    let treeBuilder = new BPMNTreeBuilder(elementRegistry.getAll()); // 实例化工具类
    let visited: any = new Map(); // 用于防止重复访问
    let bpmnTree = treeBuilder.constructTree(); // 构建树状数据结构
    treeBuilder.calculateVirtualWidth(bpmnTree, elementRegistry); // 计算虚拟宽度
    treeBuilder.bpmnTraverseTreeBFS(
      bpmnTree,
      node => {
        visited.set(node.id, node);
        let confluenceElement = elementRegistry.get(node.id);
        let connectMap = treeBuilder._connectMap;
        let incoming: any = [];
        confluenceElement.incoming.map((connect: any) => {
          if (!connectMap.has(connect.id)) {
            incoming.push(connect);
          }
        });
        if (incoming?.length > 1) {
          let x = 0;
          incoming.map((connect: any) => {
            x += visited.get(connect.source.id)?.offset?.x ? visited.get(connect.source.id).offset.x : connect.source.x || 0;
          });
          x = x / (incoming.length || 1);
          if (confluenceElement.width < typeConfig[bpmnTask].renderer.attr.width) x += (typeConfig[bpmnTask].renderer.attr.width - confluenceElement.width) / 2;
          let offset = {
            x: x,
            y: node.offset.y,
          };
          node.offset = offset;
        }
        node?.offset && node.y != node.offset.y && visited.set(node.id, node);
      },
      'vertical',
    );
    let list = Array.from(visited.values());
    // 碰撞算法
    treeBuilder.handleCollisionsByLevel(list, 'vertical');
    treeBuilder.formatCanvas(list, modeling, elementRegistry);
    let shape: any;
    let currentItem: any;
    let currentHeight: any;
    list.map((item: any) => {
      let element = elementRegistry.get(item.id);
      let height = 0;
      if (element.type != bpmnStart) height = typeConfig[bpmnTask].renderer.attr.height - typeConfig[bpmnStart].renderer.attr.height || 0;
      if (element.type === bpmnEnd) {
        shape = element;
        currentItem = item;
        currentHeight = height;
      }
      modeling.moveShape(element, {
        x: 0,
        y: (item.level + 1) * typeConfig[bpmnTask].renderer.attr.height + DEFAULT_DISTANCE * item.level - item.y - height, // 默认所有元素高度一致 元素总高度+线条总高度。
      });
    });

    treeBuilder.resizeGroupShape(elementRegistry.getAll(), props.bpmn);
    // 处理结束节点（如果有分组 且分支坐标大于结束坐标 则单独处理结束节点坐标）
    let y = -Infinity;
    let hasGroupELements = false;
    elementRegistry.getAll().map((element: any) => {
      if (element.type === bpmnGroup) {
        if (element.y + element.height + DEFAULT_CONNECT - 15 > y) y = element.y + element.height + DEFAULT_CONNECT - 15;
        hasGroupELements = true;
      }
    });
    if (shape) {
      let oldY = (currentItem.level + 1) * typeConfig[bpmnTask].renderer.attr.height + DEFAULT_DISTANCE * currentItem.level;
      modeling.moveShape(shape, {
        x: 0,
        y: hasGroupELements && y > oldY ? y - shape.y : oldY - shape.y - currentHeight,
      });
      elementRegistry.getAll().map((item: any) => {
        item.type === bpmnSequenceFlow && treeBuilder.updateConnectionWaypoints(item, modeling, 'vertical');
      });
    }

    commandStack.execute('vertical.action.same.id.end', { some: 'context' });
    handleViewBox();
    handleLocation();
  }
  /**
   *  横向美化
   */
  function handleAutoLayoutByHorizontal() {
    if (NodeUtils.getEndlessLoop(props.bpmn)?.length > 0) {
      createMessage.warning('流程存在闭环，无法进行美化操作');
      return;
    }
    props.bpmn.get('canvas').zoom(1);
    const commandStack = props.bpmn?.get('commandStack');
    props.bpmn?.get('jnpfData').setValue('layout', { value: 'horizontal' });
    commandStack.execute('horizontal.action.same.id.start', { some: 'context' });
    let elementRegistry = props.bpmn.get('elementRegistry');
    let modeling = props.bpmn.get('modeling');
    getCanvasSize('horizontal');
    let treeBuilder = new BPMNTreeBuilder(elementRegistry.getAll()); // 实例化工具类
    let visited: any = new Map(); // 用于防止重复访问
    let bpmnTree = treeBuilder.constructTree(); // 构建树状数据结构
    treeBuilder.calculateVirtualHeight(bpmnTree, elementRegistry); // 计算虚拟高度
    treeBuilder.bpmnTraverseTreeBFS(
      bpmnTree,
      node => {
        visited.set(node.id, node);
        let confluenceElement = elementRegistry.get(node.id);
        let connectMap = treeBuilder._connectMap;
        let incoming: any = [];
        confluenceElement.incoming.map((connect: any) => {
          if (!connectMap.has(connect.id)) {
            incoming.push(connect);
          }
        });
        if (incoming?.length > 1) {
          let y = 0;
          incoming.map((connect: any) => {
            y += visited.get(connect.source.id)?.offset?.y ? visited.get(connect.source.id).offset.y : connect.source.y || 0;
          });
          y = y / (incoming.length || 1);
          if (confluenceElement.height < typeConfig[bpmnTask].renderer.attr.height)
            y += (typeConfig[bpmnTask].renderer.attr.height - confluenceElement.height) / 2;
          let offset = {
            x: node.offset.x,
            y: y,
          };
          node.offset = offset;
        }
        node?.offset && node.x != node.offset.x && visited.set(node.id, node);
      },
      'horizontal',
    );
    let list = Array.from(visited.values());
    // 碰撞算法
    treeBuilder.handleCollisionsByLevel(list, 'horizontal');
    treeBuilder.formatCanvasHorizontal(list, modeling, elementRegistry);
    let shape: any;
    let currentItem: any;
    let currentWidth: any;
    list.map((item: any) => {
      let element = elementRegistry.get(item.id);
      let width = 0;
      if (element.type != bpmnStart) width = typeConfig[bpmnTask].renderer.attr.width - typeConfig[bpmnStart].renderer.attr.width || 0;
      if (element.type === bpmnEnd) {
        shape = element;
        currentItem = item;
        currentWidth = width;
      }
      modeling.moveShape(element, {
        y: 0,
        x: (item.level + 1) * typeConfig[bpmnTask].renderer.attr.width + 160 * item.level - item.x - width,
      });
    });
    treeBuilder.resizeGroupShape(elementRegistry.getAll(), props.bpmn);
    // 处理结束节点（如果有分组 且分支坐标大于结束坐标 则单独处理结束节点坐标）
    let x = -Infinity;
    let hasGroupELements = false;
    elementRegistry.getAll().map((element: any) => {
      if (element.type === bpmnGroup) {
        if (element.x + element.width + DEFAULT_CONNECT - 25 > x) x = element.x + element.width + DEFAULT_CONNECT - 25;
        hasGroupELements = true;
      }
    });

    if (shape) {
      let level = currentItem?.level || 0;
      let oldX = (level + 1) * typeConfig[bpmnTask].renderer.attr.width + 160 * level;
      modeling.moveShape(shape, {
        y: 0,
        x: hasGroupELements && x > oldX ? x - shape.x : oldX - shape.x - currentWidth,
      });
    }
    elementRegistry.getAll().map((item: any) => {
      item.type === bpmnSequenceFlow && treeBuilder.updateConnectionWaypoints(item, modeling, 'horizontal');
    });
    commandStack.execute('horizontal.action.same.id.end', { some: 'context' });
    handleViewBox();
    handleLocation();
  }
  /**
   * 元素对齐
   */
  function handleAlign(e) {
    const Align = props.bpmn.get('alignElements');
    const Selection = props.bpmn.get('selection');
    const SelectedElements = Selection.get();
    if (!SelectedElements || SelectedElements.length <= 1) return createMessage.error('请先选择要对齐的元素');
    Align.trigger(SelectedElements, e.key);
  }
  /**
   * 画布放大/缩小
   * @param isPlus  1放大 0缩小
   */
  function handleZoom(isPlus) {
    state.defaultZoom = Math.floor(state.defaultZoom * 100 + (isPlus ? 10 : -10)) / 100;
    props.bpmn.get('canvas').zoom(state.defaultZoom);
  }

  function handleZoomClick(e) {
    state.defaultZoom = e.key;
    props.bpmn.get('canvas').zoom(state.defaultZoom);
  }
  /**
   * 导航器定位到初始位置 (居中显示)
   */
  function handleLocation() {
    var canvas = props.bpmn.get('canvas');
    canvas.zoom('fit-viewport', 'auto');
    state.defaultZoom = 1;
  }
  /**
   * 显示鸟瞰图（小地图）
   */
  function handleShowMiniMap() {
    props.bpmn.get('minimap').toggle();
  }
  /**
   * 帮助
   */
  // function handleShowHelp() {
  //   createMessage.error('帮助');
  // }
  function handleAddVersion() {
    emit('addVersion');
  }

  function getCanvasSize(type: 'horizontal' | 'vertical') {
    let elementRegistry = props.bpmn.get('elementRegistry');
    let canvas = props.bpmn.get('canvas');
    let modeling = props.bpmn.get('modeling');
    let size = canvas.getSize();
    let startElement = elementRegistry.getAll().filter(item => item.type === 'bpmn:StartEvent');
    startElement.map((item: any) => {
      let x = type === 'horizontal' ? size.width * 0.05 : size.width * 0.5;
      let y = type === 'horizontal' ? size.height * 0.5 : size.height * 0.05;
      modeling.moveShape(item, {
        y: y - item.y,
        x: x - item.x,
      });
    });
  }
  function handleViewBox() {
    let canvas = props.bpmn.get('canvas');
    let viewbox = canvas.viewbox();
    viewbox.x = 0;
    viewbox.y = 0;
    canvas.viewbox(viewbox);
    canvas.zoom(1);
  }
</script>
