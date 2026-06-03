import { defHttp } from '@/utils/http/axios';

enum Api {
  Prefix = '/api/example/AProdRouting',
}

// 获取列表
export function getList(data) {
  return defHttp.post({ url: Api.Prefix + `/List`, data });
}
// 获取
export function getInfo(id) {
  return defHttp.get({ url: Api.Prefix + `/` + id });
}

// 新建
export function create(data) {
  return defHttp.post({ url: Api.Prefix, data });
}

// 修改
export function update(data) {
  return defHttp.put({ url: Api.Prefix + `/` + data.id, data });
}

// 删除
export function del(id) {
  return defHttp.delete({ url: Api.Prefix + `/` + id });
}
// 批量删除数据
export function batchDelete(ids) {
  return defHttp.post({ url: Api.Prefix + `/batchRemove`, data: ids });
}
// 详情
export function getDetail(id) {
  return defHttp.get({ url: Api.Prefix + `/Detail/` + id });
}

// 获取-流程
export function getFlowInfo(id) {
  return defHttp.get({ url: Api.Prefix + `/FlowInfo/` + id });
}

// 新建-流程
export function flowCreate(data) {
  return defHttp.post({ url: Api.Prefix + `/FlowCreate`, data });
}

// 修改-流程
export function flowUpdate(data) {
  return defHttp.put({ url: Api.Prefix + `/FlowUpdate/` + data.id, data });
}
