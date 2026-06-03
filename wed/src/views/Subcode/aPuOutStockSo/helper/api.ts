import { defHttp } from '@/utils/http/axios';

enum Api {
  Prefix = '/api/example/APuOutStockSo',
}

// 获取列表
export function getList(data) {
  return defHttp.post({ url: Api.Prefix + `/List`, data });
}
// 获取
export function getInfo(id) {
  return defHttp.get({ url: Api.Prefix + `/` + id });
}
// 获取联系与地址联动数据
export function getLinkData(customerId) {
  return defHttp.get({ url: Api.Prefix + `/LinkData/` + customerId });
}

// 根据仓库 id 获取库存商品的商品 id 与数量
export function getInventoryByWarehouse(warehouseId) {
  return defHttp.get({ url: Api.Prefix + `/InventoryByWarehouse/` + warehouseId });
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
