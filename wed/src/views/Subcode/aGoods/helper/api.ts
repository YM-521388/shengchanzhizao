import { defHttp } from '@/utils/http/axios';

enum Api {
  Prefix = '/api/example/AGoods',
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
// 导出
export function exportData(data) {
  return defHttp.post({ url: Api.Prefix + `/Actions/Export`, data });
}
// 详情
export function getDetail(id) {
  return defHttp.get({ url: Api.Prefix + `/Detail/` + id });
}

// 验证商品名称
export function validateGoodsName(data) {
  return defHttp.post({ url: Api.Prefix + `/ValidateGoodsName`, data });
}

// 通过商品获取单位
export function getGoodsUnit(id) {
  return defHttp.post({ url: Api.Prefix + `/GoodsUnit/` + id });
}

// 通过商品获取单位和数量
export function getGoodsUnitNumWeb(id) {
  return defHttp.post({ url: Api.Prefix + `/GoodsUnitNumWeb/` + id });
}

// 下载导入示例模板
export function getUnitTemplateDownload() {
  return defHttp.get({ url: Api.Prefix + `/UnitTemplateDownload` });
}

// 导入商品单位
export function importUnit(data) {
  return defHttp.post(
    { url: Api.Prefix + `/ImportUnitGoods`, data, headers: { 'Content-Type': undefined } },
    { isTransformResponse: false }
  );
}
