import { defHttp } from '@/utils/http/axios';

enum Api {
  Prefix = '/api/permission/Organize',
  DepartmentPrefix = '/api/permission/Organize/Department',
}

// 获取组织/公司列表
export function getOrganizeList(data) {
  return defHttp.get({ url: Api.Prefix, data });
}
// 获取组织/公司列表异步
export function getOrganizeSyncList(data) {
  return defHttp.get({ url: Api.Prefix + `/AsyncList/${data.parentId}`, data });
}
// 获取组织/公司下拉框列表
export function getOrganizeSelector(id = '0') {
  return defHttp.get({ url: Api.Prefix + `/Selector/${id || '0'}` });
}
// 获取组织/公司下拉框列表异步
export function getOrganizeSelectAsyncList(id = '0') {
  return defHttp.get({ url: Api.Prefix + `/SelectAsyncList/${id || '0'}` });
}
// 获取组织/公司下拉框列表(带权限)
export function getOrganizeSelectorByAuth(id = '0') {
  return defHttp.get({ url: Api.Prefix + `/SelectorByAuth/${id || '0'}` });
}
// 获取组织/公司下拉框列表(带权限)异步
export function getOrganizeSelectorAsyncByAuth(id = '0', data) {
  return defHttp.get({ url: Api.Prefix + `/SelectAsyncByAuth/${id || '0'}`, data });
}
// 获取组织/公司树形
export function getOrganizeTree() {
  return defHttp.get({ url: Api.Prefix + `/Tree` });
}
// 新建组织/公司
export function create(data) {
  return defHttp.post({ url: Api.Prefix, data });
}
// 修改组织/公司
export function update(data) {
  return defHttp.put({ url: Api.Prefix + '/' + data.id, data });
}
// 获取组织/公司
export function getInfo(id) {
  return defHttp.get({ url: Api.Prefix + '/' + id });
}
// 删除组织/公司
export function delOrganize(id) {
  return defHttp.delete({ url: Api.Prefix + '/' + id });
}
// 获取选中组织、部门基本信息
export function getSelectedList(ids) {
  return defHttp.post({ url: Api.Prefix + `/SelectedList`, data: { ids } });
}
// 通过部门id获取部门树形
export function getOrgByOrganizeCondition(data) {
  return defHttp.post({ url: Api.Prefix + `/OrganizeCondition`, data });
}
// 导出组织Excel
export function exportExcel(data) {
  return defHttp.get({ url: Api.Prefix + `/ExportData`, data });
}
// 公司导入模板下载
export function templateDownload() {
  return defHttp.get({ url: Api.Prefix + `/TemplateDownload` });
}
// 公司导入
export function importData(data) {
  return defHttp.post({ url: Api.Prefix + `/ImportData`, data });
}
// 公司导入预览
export function importPreview(data) {
  return defHttp.get({ url: Api.Prefix + `/ImportPreview`, data });
}
// 公司导出错误数据
export function exportExceptionData(data) {
  return defHttp.post({ url: Api.Prefix + `/ExportExceptionData`, data });
}
// 获取部门下拉框列表(公司+部门)
export function getDepartmentSelector(id = '0') {
  return defHttp.get({ url: Api.DepartmentPrefix + `/Selector/${id || '0'}` });
}
// 获取部门下拉框列表(公司+部门)异步
export function getDepartmentSelectAsyncList(id = '0', data = {}) {
  return defHttp.get({ url: Api.DepartmentPrefix + `/SelectAsyncList/${id || '0'}`, data });
}
// 获取部门下拉框列表(公司+部门) ---带权限
export function getDepartmentSelectorByAuth(id = '0') {
  return defHttp.get({ url: Api.DepartmentPrefix + `/SelectorByAuth/${id || '0'}` });
}
// 获取部门下拉框列表(公司+部门) ---带权限异步
export function getDepartmentSelectorAsyncByAuth(id = '0', data) {
  return defHttp.get({ url: Api.DepartmentPrefix + `/SelectAsyncByAuth/${id || '0'}`, data });
}
// 新建组织/公司
export function createDepartment(data) {
  return defHttp.post({ url: Api.DepartmentPrefix, data });
}
// 修改部门
export function updateDepartment(data) {
  return defHttp.put({ url: Api.DepartmentPrefix + '/' + data.id, data });
}
// 获取部门
export function getDepartmentInfo(id) {
  return defHttp.get({ url: Api.DepartmentPrefix + '/' + id });
}
