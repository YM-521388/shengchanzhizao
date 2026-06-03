import { defHttp } from '@/utils/http/axios';

enum Api {
  Prefix = '/api/system/printDev',
}

// 获取打印模板列表(分页)
export function getPrintDevList(data) {
  return defHttp.get({ url: Api.Prefix, data });
}
// 获取打印模板列表下拉框
export function getPrintDevSelector() {
  return defHttp.get({ url: Api.Prefix + `/Selector` });
}
// 获取打印模板业务列表
export function getPrintWorkSelector(data) {
  return defHttp.get({ url: Api.Prefix + `/WorkSelector`, data });
}
// 获取打印模板列表下拉框
export function getPrintDevByIds(data) {
  return defHttp.post({ url: Api.Prefix + `/getListOptions`, data });
}
// 新建打印模板
export function createPrintDev(data) {
  return defHttp.post({ url: Api.Prefix, data });
}
// 修改打印模板
export function updatePrintDev(data) {
  return defHttp.put({ url: Api.Prefix + '/' + data.id, data });
}
// 获取打印模板
export function getPrintDevInfo(id) {
  return defHttp.get({ url: Api.Prefix + '/' + id });
}
// 删除打印模板
export function delPrintDev(id) {
  return defHttp.delete({ url: Api.Prefix + '/' + id });
}
// 复制打印模板
export function copy(id) {
  return defHttp.post({ url: Api.Prefix + `/${id}/Actions/Copy` });
}
// 导出数据打印模板数据
export function exportData(id) {
  return defHttp.get({ url: Api.Prefix + `/${id}/Actions/Export` });
}
// 获取打印预览
export function getPreviewInfo(id) {
  return defHttp.get({ url: Api.Prefix + `/${id}/Actions/Preview` });
}
// 批量打印获取数据和打印模板
export function getBatchData(data) {
  return defHttp.post({ url: Api.Prefix + `/BatchData`, data });
}
// 修改打印设计
export function saveVersion(data) {
  return defHttp.post({ url: Api.Prefix + '/Save', data });
}
// 获取打印设计详情
export function getVersionInfo(id) {
  return defHttp.get({ url: Api.Prefix + '/Info/' + id });
}
// 获取打印版本
export function getVersionList(id) {
  return defHttp.get({ url: Api.Prefix + '/Version/' + id });
}
// 删除打印版本
export function delVersion(id) {
  return defHttp.delete({ url: Api.Prefix + '/Info/' + id });
}
// 新增打印版本
export function copyVersion(id) {
  return defHttp.post({ url: Api.Prefix + `/Info/${id}` });
}
