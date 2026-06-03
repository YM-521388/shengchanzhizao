import { reportHttp } from '@/utils/http/axios';
import { ContentTypeEnum } from '@/enums/httpEnum';

enum Api {
  Prefix = '/api/Report',
}

/**
 * univer报表
 */
// 获取报表模板列表(分页)
export function getReportList(data) {
  return reportHttp.get({ url: Api.Prefix, data });
}
// 新建报表模板
export function createReport(data) {
  return reportHttp.post({ url: Api.Prefix, data });
}
// 修改报表模板
export function updateReport(data) {
  return reportHttp.put({ url: Api.Prefix + '/' + data.id, data });
}
// 获取报表模板
export function getReportInfo(id) {
  return reportHttp.get({ url: Api.Prefix + '/' + id });
}
// 删除报表模板
export function delReport(id) {
  return reportHttp.delete({ url: Api.Prefix + '/' + id });
}
// 复制报表模板
export function copy(id) {
  return reportHttp.post({ url: Api.Prefix + `/${id}/Actions/Copy` });
}
// 列表导出报表
export function upload(url, data) {
  return reportHttp.post({ url: url, data, headers: { 'Content-Type': ContentTypeEnum.FORM_DATA } });
}
// 导出数据报表模板数据
export function exportData(id) {
  return reportHttp.get({ url: Api.Prefix + `/${id}/Actions/Export` });
}
// 获取数据和报表模板
export function getData(data) {
  return reportHttp.post({ url: Api.Prefix + `/Data`, data });
}
// 修改报表设计
export function saveVersion(data) {
  return reportHttp.post({ url: Api.Prefix + '/Save', data });
}
// 获取报表设计详情
export function getVersionInfo(id) {
  return reportHttp.get({ url: Api.Prefix + '/Info/' + id });
}
// 获取报表版本
export function getVersionList(id) {
  return reportHttp.get({ url: Api.Prefix + '/Version/' + id });
}
// 删除报表版本
export function delVersion(id) {
  return reportHttp.delete({ url: Api.Prefix + `/Info/${id}` });
}
// 新增报表版本
export function copyVersion(id) {
  return reportHttp.post({ url: Api.Prefix + `/Info/${id}` });
}
// 预览设计
export function getPreviewDesign(id, data?: any) {
  return reportHttp.post({ url: Api.Prefix + `/data/${id}/preview`, data });
}
// 预览设计(通过报表大id获取)
export function getPreviewTemplate(id, data?: any) {
  return reportHttp.post({ url: Api.Prefix + `/data/${id}/previewTemplate`, data });
}
// 菜单
export function getReportSelector() {
  return reportHttp.get({ url: Api.Prefix + '/Selector' });
}
// 上传图片
export function uploadFileImage(data) {
  return reportHttp.post({ url: Api.Prefix + '/data/upload/file', data, headers: { 'Content-Type': ContentTypeEnum.FORM_DATA } });
}
// 上传图片
export function uploadImg(data) {
  return reportHttp.post({ url: Api.Prefix + '/data/downImg', data });
}
// 上传excel
export function uploadFileExcel(data) {
  return reportHttp.post({ url: Api.Prefix + '/data/ImportExcel', data, headers: { 'Content-Type': ContentTypeEnum.FORM_DATA } });
}
// 下载excel
export function exportFileExcel(id, data) {
  return reportHttp.post({ url: Api.Prefix + `/data/${id}/DownExcel`, data });
}

// 获取已发布菜单
export function getReleaseMenu(id) {
  return reportHttp.get({ url: Api.Prefix + `/${id}/getReleaseMenu` });
}
// 生成菜单
export function createMenu(id, data) {
  return reportHttp.post({ url: Api.Prefix + `/${id}/Actions/Module`, data });
}

