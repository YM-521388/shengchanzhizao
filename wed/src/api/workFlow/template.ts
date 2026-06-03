import { defHttp } from '@/utils/http/axios';

enum Api {
  Prefix = '/api/workflow/template',
  CommentPrefix = '/api/workflow/comment',
  WebhookPrefix = '/api/workflow/Hooks',
}

// 获取流程列表(分页)
export function getList(data) {
  return defHttp.get({ url: Api.Prefix, data });
}
// 通过表单id获取流程列表
export function getFlowList(id) {
  return defHttp.get({ url: Api.Prefix + `/${id}/FlowList` });
}
// 新建流程基本信息
export function create(data) {
  return defHttp.post({ url: Api.Prefix, data });
}
// 修改流程基本信息
export function update(data) {
  return defHttp.put({ url: Api.Prefix + '/' + data.id, data });
}
// 获取流程基本信息
export function getInfo(id) {
  return defHttp.get({ url: Api.Prefix + '/' + id });
}
// 删除流程引擎
export function delFlow(id) {
  return defHttp.delete({ url: Api.Prefix + '/' + id });
}
// 复制流程引擎
export function copy(id) {
  return defHttp.post({ url: Api.Prefix + `/${id}/Actions/Copy` });
}
// 导出
export function exportData(id) {
  return defHttp.get({ url: Api.Prefix + `/${id}/Actions/Export` });
}
// 简单流程切换为标准流程
export function changeType(id) {
  return defHttp.put({ url: Api.Prefix + `/${id}/UpdateType` });
}
// 修改流程
export function saveFlow(data) {
  return defHttp.post({ url: Api.Prefix + '/Save', data });
}
// 获取流程引擎
export function getFlowInfo(id) {
  return defHttp.get({ url: Api.Prefix + '/Info/' + id });
}
// 获取流程引擎版本
export function getVersionList(id) {
  return defHttp.get({ url: Api.Prefix + '/Version/' + id });
}
// 删除流程版本
export function delVersion(id) {
  return defHttp.delete({ url: Api.Prefix + '/Info/' + id });
}
// 新增流程版本
export function copyVersion(id) {
  return defHttp.post({ url: Api.Prefix + `/Info/${id}` });
}
// 列表流程列表(分页)
export function getFlowSelector(data) {
  return defHttp.get({ url: Api.Prefix + `/Selector`, data });
}
// 获取流程发起节点表单信息(小流程id)
export function getFlowFormInfo(id) {
  return defHttp.get({ url: Api.Prefix + `/${id}/FormInfo` });
}
// 获取流程发起节点表单信息(大流程id)
export function getFlowStartFormInfo(id) {
  return defHttp.get({ url: Api.Prefix + `/StartForm/${id}` });
}
// 获取流程版本Id和发起节点表单id(大流程id)
export function getFlowStartFormId(id) {
  return defHttp.get({ url: Api.Prefix + `/StartFormId/${id}` });
}
// 所属流程列表(树形) formType 1-系统表单 2-自定义表单
export function getTreeList(formType = '') {
  return defHttp.get({ url: Api.Prefix + '/TreeList', data: { formType } });
}
// 委托 通过list<flowId>获取流程引擎列表
export function getFlowEngineListByIds(data) {
  return defHttp.post({ url: Api.Prefix + `/GetFlowList`, data });
}
// 设置常用流程
export function setCommonFlow(id) {
  return defHttp.post({ url: Api.Prefix + `/SetCommonFlow/${id}` });
}
// 子流程带权限的可选用户
export function getSubFlowUserList(id, data) {
  return defHttp.get({ url: Api.Prefix + `/${id}/SubFlowUserList`, data });
}
// 获取流程评论列表
export function getCommentList(data) {
  return defHttp.get({ url: Api.CommentPrefix, data });
}
// 新建流程评论
export function createComment(data) {
  return defHttp.post({ url: Api.CommentPrefix, data });
}
// 删除流程评论
export function delComment(id) {
  return defHttp.delete({ url: Api.CommentPrefix + '/' + id });
}
// webhookUrl
// 获取webhookUrl
export function getWebhookUrl(id) {
  return defHttp.get({ url: Api.WebhookPrefix + `/getUrl`, data: { id } });
}
// 获取webhook字段
export function getWebhookParams(randomStr) {
  return defHttp.get({ url: Api.WebhookPrefix + `/getParams/${randomStr}` });
}
// webhook开启接收请求
export function webhookStart(id, randomStr) {
  return defHttp.get({ url: Api.WebhookPrefix + `/${id}/start/${randomStr}` });
}
// 上架
export function upDownSelf(data) {
  return defHttp.put({ url: Api.Prefix + `/${data.id}/UpDownShelf`, data });
}
