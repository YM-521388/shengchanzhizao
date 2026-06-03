import { changeTypeByTaskShape, changeTypeByTrigger, typeConfig } from '../../config';
import { append as svgAppend, create as svgCreate } from 'tiny-svg';
import { bpmnEnd, bpmnGroup, bpmnStart } from '../../config/variableName';

/**
 * svg重画bpmn节点
 */
export default (parentNode: any, element: any, jnpfFlowInfo: any) => {
  let data = jnpfFlowInfo?.flowNodes[element.id];
  let nodeMap = jnpfFlowInfo?.nodeList;
  let isPreview = jnpfFlowInfo?.isPreview;
  let type = element.type; // 获取到类型;
  if (typeConfig && typeConfig[type]) {
    if (type === bpmnGroup) return null;
    if (changeTypeByTrigger[element.wnType || data?.type]) type = changeTypeByTrigger[element.wnType || data?.type];
    if (changeTypeByTaskShape[element.wnType || data?.type]) type = changeTypeByTaskShape[element.wnType || data?.type];
    let { renderer } = typeConfig[type];
    let { icon, iconColor, rendererName, background, titleColor, attr, bodyDefaultText } = renderer;
    element['width'] = attr.width;
    element['height'] = attr.height;
    let nodeName = element.nodeName != null ? element.nodeName : data?.nodeName != null ? data.nodeName : rendererName;
    let nodeContent = element.elementBodyName || nodeMap.get(element.id)?.userName || data?.content || bodyDefaultText;
    if (element.elementBodyName === '') nodeContent = bodyDefaultText;
    // 只有任务节点才根据F_TaskStatusCode显示状态颜色，开始/结束节点保持默认
    // F_TaskStatusCode: 0=未开始, 1=进行中, 2=已完成, 3=已暂停, 空值默认为未开始
    if (isPreview && (changeTypeByTaskShape[element.wnType || data?.type] || changeTypeByTrigger[element.wnType || data?.type])) {
      const taskStatusCode = data?.F_TaskStatusCode;
      // 转换为字符串进行比较，兼容数字和字符串类型
      const status = String(taskStatusCode || '');
      if (status === '1') {
        // 进行中 - 蓝色
        titleColor = 'linear-gradient(90deg, #C0EDF8 0%, #A6DEF8 100%)';
        iconColor = '#1eaceb';
      } else if (status === '2') {
        // 已完成 - 绿色
        titleColor = 'linear-gradient(90deg, #AEEFC2 0%, #4ED587 100%)';
        iconColor = '#25a210';
      } else if (status === '3') {
        // 已暂停 - 红色
        titleColor = 'linear-gradient(90deg, #FDC9D1 0%, #E03845 100%)';
        iconColor = '#E03845';
      } else {
        // 未开始(0) 或 空值 默认 - 灰色
        titleColor = 'linear-gradient(90deg, #CED1D5 0%, #CBCBCC 100%)';
        iconColor = '#4c4c58';
      }
    }
    let foreignObject: any = svgCreate('foreignObject', {
      width: attr.width,
      height: attr.height,
      class: type === bpmnStart || type === bpmnEnd ? 'begin-or-end-node' : 'task-node',
    });
    // 开始节点
    if (type === bpmnStart) {
      foreignObject.innerHTML = `
      <div class="node-container start-node-container" style="background:${background}" >
        <div class='node-top-container'>
          <i class="${icon}" style="color:${iconColor}"></i>
          <span title=${nodeName}>${nodeName}</span>
        </div>
      </div>`;
      svgAppend(parentNode, foreignObject);
      return parentNode;
    }
    // 任务节点
    if (changeTypeByTaskShape[element.wnType || data?.type] || changeTypeByTrigger[element.wnType || data?.type]) {
      foreignObject.innerHTML = `
      <div class="node-container" style="background:${background}" >
        <div class='node-top-container' style="background:${titleColor}">
          <i class="${icon}" style="color:${iconColor}"></i>
          <span>${nodeName}</span>
        </div>
        <div class='node-bottom-container'>
          <span>${nodeContent}</span>
        </div>
      </div>`;
      svgAppend(parentNode, foreignObject);
      return parentNode;
    }
    // 结束节点
    if (type === bpmnEnd) {
      foreignObject.innerHTML = `
      <div class="node-container end-node-container" style="background:${background}" >
        <div class='node-top-container'>
          <i class="${icon}" style="color:${iconColor}"></i>
          <span>${nodeName}</span>
        </div>
      </div>`;
      svgAppend(parentNode, foreignObject);
      return parentNode;
    }
  }
};
