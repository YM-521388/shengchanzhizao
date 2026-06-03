import { defHttp } from '@/utils/http/axios';

enum Api {
  Prefix = '/api/example/AProdTask',
}

// 获取生产人员、质检人员列表
export function getUserCon(data) {
  return defHttp.post({ url: Api.Prefix + `/UserCon/` + data });
}
// 获取任务物料列表
export function getTaskItemList(data) {
  return defHttp.post({ url: Api.Prefix + `/ItemList`, data });
}
// 获取列表
export function getTaskList(data) {
  return defHttp.post({ url: Api.Prefix + `/TaskList`, data });
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

// 修改状态
export function handleCheck(id, state) {
  return defHttp.get({ url: Api.Prefix + `/Check/` + id + `/` + state });
}

// 暂停原因
export function reasonUpdate(data) {
  return defHttp.put({ url: Api.Prefix + `/ReasonUpdate/` + data.id, data });
}
