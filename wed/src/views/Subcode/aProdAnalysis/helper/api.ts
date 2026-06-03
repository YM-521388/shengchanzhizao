import { defHttp } from '@/utils/http/axios';

enum Api {
  Prefix = '/api/example/AProdAnalysis',
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

// 根据生产计划ID获取BOM物料信息
export function getBomMaterials(prodPlanId) {
  return defHttp.get({ url: Api.Prefix + `/GetBomMaterials/` + prodPlanId });
}

// 根据分析ID获取物料分析商品信息并按商品来源分类
export function getAnalysisItemsBySource(analysisId) {
  return defHttp.get({ url: Api.Prefix + `/GetAnalysisItemsBySource/` + analysisId });
}
