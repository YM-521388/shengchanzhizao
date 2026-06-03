import { defHttp } from '@/utils/http/axios';

enum Api {
  Prefix = '/api/example/AProdProcessing',
}

// 获取某个 加工单及商品信息.
export function getProcessingIdList(id) {
  return defHttp.post({ url: Api.Prefix + `/ProcessingIdList/` + id });
}
// 获取加工单用料清单
export function getBomItemList(data) {
  return defHttp.post({ url: Api.Prefix + `/BomItemList`, data });
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

// 暂停、取消原因
export function reasonUpdate(data, type) {
  return defHttp.put({ url: Api.Prefix + `/ReasonUpdate/` + data.id + '/' + type, data });
}

// 获取加工单 生产计划信息.
export function getFlowInfo(id) {
  return defHttp.get({ url: Api.Prefix + `/FlowInfo/` + id });
}
