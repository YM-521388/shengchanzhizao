import { defHttp } from '@/utils/http/axios';

enum Api {
  Prefix = '/api/visualdev/Base',
  GenPrefix = '/api/visualdev/Generater',
  OnlinePrefix = '/api/visualdev/OnlineDev',
  LogPrefix = '/api/visualdev/OnlineLog',
  PersonalPrefix = '/api/visualdev/personal',
  AiPrefix = '/api/visualdev/ai/form',
}

// 获取功能列表
export function getVisualDevList(data) {
  return defHttp.get({ url: Api.Prefix, data });
}
// 获取功能列表（集成助手用）
export function getVisualDevFormList(data) {
  return defHttp.get({ url: Api.Prefix + '/list', data });
}
// 新建功能
export function create(data) {
  return defHttp.post({ url: Api.Prefix, data });
}
// 修改功能
export function update(data) {
  return defHttp.put({ url: Api.Prefix + '/' + data.id, data });
}
// 获取功能
export function getInfo(id) {
  return defHttp.get({ url: Api.Prefix + '/' + id });
}
// 删除功能
export function delVisualDev(id) {
  return defHttp.delete({ url: Api.Prefix + '/' + id });
}
// 复制功能
export function copy(id) {
  return defHttp.post({ url: Api.Prefix + `/${id}/Actions/Copy` });
}
// 发布
export function release(id) {
  return defHttp.post({ url: Api.Prefix + `/${id}/Actions/Release` });
}
// 获取已发布菜单
export function getReleaseMenu(id) {
  return defHttp.get({ url: Api.Prefix + `/${id}/getReleaseMenu` });
}
// 生成菜单
export function createMenu(id, data) {
  return defHttp.post({ url: Api.Prefix + `/${id}/Actions/Module`, data });
}
// 获取表单主表属性列表
export function getFormDataFields(id, filterType = 0) {
  return defHttp.get({ url: Api.Prefix + `/${id}/FormDataFields?filterType=${filterType}` });
}
// 获取表单主表数据下拉框
export function getFieldDataSelect(data) {
  return defHttp.get({ url: Api.Prefix + `/${data.modelId}/FieldDataSelect`, data });
}
// 获取功能下拉框列表
export function getVisualDevSelector(data) {
  return defHttp.get({ url: Api.Prefix + `/Selector`, data });
}
// 回滚模板
export function rollbackTemplate(id) {
  return defHttp.get({ url: Api.Prefix + `/${id}/Actions/RollbackTemplate` });
}

// 代码下载
export function downloadCode(id, data) {
  return defHttp.post({ url: Api.GenPrefix + `/${id}/Actions/DownloadCode`, data });
}
// 代码预览
export function codePreview(id, data) {
  return defHttp.post({ url: Api.GenPrefix + `/${id}/Actions/CodePreview`, data });
}
// 获取代码生成命名规范
export function getAliasInfo(id) {
  return defHttp.get({ url: Api.GenPrefix + `/${id}/Alias/Info` });
}
// 更新代码生成命名规范
export function saveAlias(id, data) {
  return defHttp.post({ url: Api.GenPrefix + `/${id}/Alias/Save`, data });
}

// 获取列表表单配置JSON
export function getConfigData(modelId, data = {}) {
  return defHttp.get({ url: Api.OnlinePrefix + `/${modelId}/Config`, data });
}
// 通过菜单id获取列表表单配置JSON
export function getConfigDataByMenuId(data = {}) {
  return defHttp.get({ url: Api.OnlinePrefix + `/Config`, data }, { errorMessageMode: 'none' });
}
// 获取表单配置JSON
export function getFormData(modelId) {
  return defHttp.get({ url: Api.OnlinePrefix + `/${modelId}/FormData` });
}
// 获取数据列表
export function getModelList(data) {
  return defHttp.post({ url: Api.OnlinePrefix + `/${data.modelId}/List`, data });
}
// 新建数据
export function createModel(modelId, data) {
  return defHttp.post({ url: Api.OnlinePrefix + `/${modelId}`, data });
}
// 修改数据
export function updateModel(modelId, data) {
  return defHttp.put({ url: Api.OnlinePrefix + `/${modelId}/${data.id}`, data });
}
// 获取数据信息
export function getModelInfo(modelId, id, menuId = '') {
  return defHttp.get({ url: Api.OnlinePrefix + `/${modelId}/${id}`, data: { menuId } });
}
// 获取修改记录列表
export function getDataLogList(data) {
  return defHttp.get({ url: Api.LogPrefix, data });
}
// 删除数据
export function delModel(modelId, id) {
  return defHttp.delete({ url: Api.OnlinePrefix + `/${modelId}/${id}` });
}
// 批量删除数据
export function batchDelete(modelId, data) {
  return defHttp.post({ url: Api.OnlinePrefix + `/batchDelete/${modelId}`, data });
}
// 获取数据详情
export function getDataChange(modelId, data) {
  return defHttp.post({ url: Api.OnlinePrefix + `/${modelId}/DataChange`, data });
}
// 导出数据
export function exportModel(modelId, data) {
  return defHttp.post({ url: Api.OnlinePrefix + `/${modelId}/Actions/ExportData`, data });
}
// 下载模板
export function getTemplate(modelId) {
  return defHttp.get({ url: Api.OnlinePrefix + `/${modelId}/Template` });
}
// 导出模板
export function exportData(modelId) {
  return defHttp.post({ url: Api.OnlinePrefix + `/${modelId}/Actions/Export` });
}
// 自定义按钮发起流程
export function launchFlow(modelId, data) {
  return defHttp.post({ url: Api.OnlinePrefix + `/${modelId}/actionLaunchFlow`, data });
}

// 获取视图列表
export function getViewList(data) {
  return defHttp.get({ url: Api.PersonalPrefix, data });
}
// 删除视图
export function delView(id, menuId) {
  return defHttp.delete({ url: Api.PersonalPrefix + `/${id}?menuId=${menuId}` });
}
// 新建视图
export function createView(data) {
  return defHttp.post({ url: Api.PersonalPrefix, data });
}
// 修改视图
export function updateView(data) {
  return defHttp.put({ url: Api.PersonalPrefix + '/' + data.id, data });
}
// 设置默认视图
export function setDefaultView(id, menuId) {
  return defHttp.put({ url: Api.PersonalPrefix + `/${id}/setDefault?menuId=${menuId}` });
}

// 获取ai字段
export function getAiInfo(data) {
  return defHttp.post({ url: Api.AiPrefix, data });
}
