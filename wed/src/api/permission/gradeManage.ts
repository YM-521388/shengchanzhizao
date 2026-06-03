import { defHttp } from '@/utils/http/axios';

enum Api {
  Prefix = '/api/permission/organizeAdminIsTrator',
}

// 获取分级管理员列表
export function getGradeManageList(data) {
  return defHttp.get({ url: Api.Prefix, data });
}
// 获取分级管理员下拉框列表
export function getSelectorOrgList(userId) {
  return defHttp.get({ url: Api.Prefix + `/Selector?userId=${userId}` });
}
// 获取分级管理员下拉框列表(异步)
export function getSelectorAsyncOrgList(id = '0', userId) {
  return defHttp.get({ url: Api.Prefix + `/SelectAsyncList/${id || '0'}?userId=${userId}` });
}
// 获取分级管理(除了组织信息)
export function getInfo(userId) {
  return defHttp.get({ url: Api.Prefix + `/organizeSelector?userId=${userId}` });
}
// 新建分级管理员
export function create(data) {
  return defHttp.post({ url: Api.Prefix, data });
}
// 删除分级管理员
export function delGradeManage(id) {
  return defHttp.delete({ url: Api.Prefix + '/' + id });
}
