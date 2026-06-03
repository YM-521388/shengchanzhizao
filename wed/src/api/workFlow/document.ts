import { defHttp } from '@/utils/http/axios';
import { ContentTypeEnum } from '@/enums/httpEnum';

enum Api {
  Prefix = '/api/extend/Document',
}

// 获取知识管理列表（全部文档）
export function getAllList(data) {
  return defHttp.get({ url: Api.Prefix, data });
}
// 添加文件夹
export function create(data) {
  return defHttp.post({ url: Api.Prefix, data });
}
// 修改文件名/文件夹名
export function update(data) {
  return defHttp.put({ url: Api.Prefix + '/' + data.id, data });
}
// 批量删除文件/文件夹
export function batchDeleteDocument(ids) {
  return defHttp.post({ url: Api.Prefix + `/BatchDelete`, data: { ids } });
}
// 获取知识管理列表（文件夹树）
export function getFolderTree(ids?) {
  return defHttp.post({ url: Api.Prefix + `/FolderTree`, data: { ids } });
}
// 获取文件/文件夹信息
export function getInfo(id) {
  return defHttp.get({ url: Api.Prefix + `/${id}` });
}
// 批量移动文件/文件夹
export function moveTo(toId, ids) {
  return defHttp.put({ url: Api.Prefix + `/Actions/MoveTo/${toId}`, data: { ids } });
}
// 批量取消分享文件/文件夹
export function shareCancel(ids) {
  return defHttp.post({ url: Api.Prefix + `/Actions/CancelShare`, data: { ids } });
}
// 批量分享文件/文件夹
export function createShare(ids, userIds) {
  return defHttp.post({ url: Api.Prefix + `/Actions/Share`, data: { ids, userIds } });
}
// 单个分享文件/文件夹
export function createSingleShare(id, userIds) {
  return defHttp.post({ url: Api.Prefix + `/Actions/ShareAdjustment/${id}`, data: { userIds } });
}
// 知识管理（我的共享列表）
export function getShareOutList(data) {
  return defHttp.get({ url: Api.Prefix + '/Share', data });
}
// 获取知识管理列表（共享给我）
export function getShareTomeList(data) {
  return defHttp.get({ url: Api.Prefix + '/ShareTome', data });
}
// 获取知识管理列表（共享人员）
export function getShareUserList(documentId) {
  return defHttp.get({ url: Api.Prefix + `/ShareUser/${documentId}` });
}
// 获取知识管理列表（回收站）
export function getTrashList(data) {
  return defHttp.get({ url: Api.Prefix + '/Trash', data });
}
// 回收站（批量彻底删除）
export function trashDelete(ids) {
  return defHttp.post({ url: Api.Prefix + `/Trash`, data: { ids } });
}
// 回收站（批量还原文件）
export function trashRecovery(ids) {
  return defHttp.post({ url: Api.Prefix + `/Trash/Actions/Recovery`, data: { ids } });
}
// 批量下载文件
export function download(ids) {
  return defHttp.post({ url: Api.Prefix + `/PackDownload`, data: { ids } });
}
// 分片组装
export function documentMerge(data) {
  return defHttp.post({ url: Api.Prefix + `/merge`, data, headers: { 'Content-Type': ContentTypeEnum.FORM_URLENCODED } });
}
// 上传Blob（流程归档）
export function uploadBlob(data) {
  return defHttp.post({ url: Api.Prefix + `/UploadBlob`, data, headers: { 'Content-Type': ContentTypeEnum.FORM_DATA } }, { errorMessageMode: 'none' });
}
