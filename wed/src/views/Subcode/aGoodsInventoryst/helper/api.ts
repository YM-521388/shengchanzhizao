import { defHttp } from '@/utils/http/axios';

enum Api {
  Prefix = '/api/example/AGoodsInventory',
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
