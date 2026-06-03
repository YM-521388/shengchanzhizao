import { defHttp } from '@/utils/http/axios';

enum Api {
  Prefix = '/api/example/APuReturnPrd',
}

// 获取商品列表
export function getGoodsList(data) {
  return defHttp.post({ url: Api.Prefix + `/GoodsList`, data });
}
// 获取Log列表
export function getLogList(data) {
  return defHttp.post({ url: Api.Prefix + `/LogList`, data });
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
// 根据加工ID获取商品成本价列表
export function getCostsByProcessing(processingId) {
  return defHttp.get({ url: Api.Prefix + `/CostsByProcessing/` + processingId });
}

// 获取商品的最大序号（来自 AGoodsInventoryQr 服务）
export function getMaxSeq(goodsIds: string[]) {
  return defHttp.post({ url: '/api/example/AGoodsInventoryQr/GetMaxSeq', data: { goodsIds } });
}
