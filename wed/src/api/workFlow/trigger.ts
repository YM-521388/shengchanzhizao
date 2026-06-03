import { defHttp } from '@/utils/http/axios';

enum Api {
  Prefix = '/api/workflow/trigger/task',
}

// 流传记录的任务流转日志
export function getTaskLogList(data) {
  return defHttp.get({ url: Api.Prefix + '/List', data });
}
// 获取任务流程监控列表
export function getTaskMonitorList(data) {
  return defHttp.get({ url: Api.Prefix, data });
}
// 批量删除任务流程监控
export function delTaskMonitor(data) {
  return defHttp.delete({ url: Api.Prefix, data });
}
// 重试日志
export function retryTask(id) {
  return defHttp.post({ url: Api.Prefix + `/Retry/${id}` });
}
