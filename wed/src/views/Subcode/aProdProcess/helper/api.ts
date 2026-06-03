import { defHttp } from '@/utils/http/axios';

enum Api {
  Prefix = '/api/example/AProdProcess',
}

/** 级联返回数组、空数组拼进 URL 会变成 .../id/ 导致 404；未选仓库用 0 占位，后端查不到库存则数量为 0 */
function resolveWarehouseIdForPath(warehouseId: unknown): string {
  if (warehouseId == null || warehouseId === '') return '0';
  if (Array.isArray(warehouseId)) {
    return warehouseId.length ? String(warehouseId[warehouseId.length - 1]) : '0';
  }
  return String(warehouseId);
}

// 根据加工单和仓库获取用料清单
export function GetProcessingGoodsList(id, warehouseId) {
  const wid = resolveWarehouseIdForPath(warehouseId);
  return defHttp.post({ url: Api.Prefix + `/ProcessingGoodsList/` + id + '/' + wid });
}

// 获取机台下工序信息
export function getProcessList(data) {
  return defHttp.post({ url: Api.Prefix + `/ProcessList`, data });
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
