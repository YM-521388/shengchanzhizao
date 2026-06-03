import { defHttp } from '@/utils/http/axios';

enum Api {
  Prefix = '/api/system/Schedule',
}

// 获取日程安排列表
export function getScheduleList(data) {
  return defHttp.get({ url: Api.Prefix, data });
}
// 新建日程安排
export function createSchedule(data) {
  return defHttp.post({ url: Api.Prefix, data });
}
// 更新日程安排
export function updateSchedule(data, type) {
  return defHttp.put({ url: Api.Prefix + `/${data.id}/${type}`, data });
}
// 删除日程安排
export function delSchedule(id, type) {
  return defHttp.delete({ url: Api.Prefix + `/${id}/${type}` });
}
// 获取日程安排信息
export function getScheduleInfo(id) {
  return defHttp.get({ url: Api.Prefix + `/${id}` });
}
// 查看日程详情
export function getScheduleDetail(groupId, id) {
  return defHttp.get({ url: Api.Prefix + `/detail?groupId=${groupId}&id=${id}` });
}
