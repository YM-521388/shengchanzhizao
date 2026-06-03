import { defHttp } from '@/utils/http/axios';

enum Api {
  Prefix = '/api/workflow/monitor',
}

// 获取列表
export function getFlowMonitorList(data) {
  return defHttp.get({ url: Api.Prefix, data });
}
// 批量删除流程监控
export function delMonitorList(data) {
  return defHttp.delete({ url: Api.Prefix, data });
}
// 事件日志
export function getLogList(data) {
  return defHttp.get({ url: Api.Prefix + `/${data.id}/EventLog` });
}
// 终止审核
export function cancel(id, data) {
  return defHttp.post({ url: Api.Prefix + `/Cancel/${id}`, data });
}
// 流程监控指派
export function assign(id, data) {
  return defHttp.post({ url: Api.Prefix + `/Assign/${id}`, data });
}
// 流程复活和变更提交
export function resurgence(id, data) {
  return defHttp.post({ url: Api.Prefix + `/Activate/${id}`, data });
}
// 获取暂停类型
export function getPauseType(id) {
  return defHttp.get({ url: Api.Prefix + `/AnySubFlow/${id}` });
}
// 暂停流程
export function pause(id, data) {
  return defHttp.post({ url: Api.Prefix + `/Pause/${id}`, data });
}
// 恢复流程
export function reboot(id) {
  return defHttp.post({ url: Api.Prefix + `/Reboot/${id}` });
}
