import { defHttp } from '@/utils/http/axios';

enum Api {
  Prefix = '/api/system/Kit',
}

// 获取模板列表(带分页)
export function getKitList(data) {
  return defHttp.get({ url: Api.Prefix, data });
}
// 获取模板下拉框列表
export function getKitSelector() {
  return defHttp.get({ url: Api.Prefix + `/Selector` });
}
// 新建模板
export function create(data) {
  return defHttp.post({ url: Api.Prefix, data });
}
// 修改模板
export function update(data) {
  return defHttp.put({ url: Api.Prefix + '/' + data.id, data });
}
// 获取模板
export function getInfo(id) {
  return defHttp.get({ url: Api.Prefix + '/' + id });
}
// 删除模板
export function delKit(id) {
  return defHttp.delete({ url: Api.Prefix + '/' + id });
}
// 复制模板
export function copy(id) {
  return defHttp.post({ url: Api.Prefix + `/${id}/Actions/Copy` });
}
// 导出
export function exportTpl(id) {
  return defHttp.get({ url: Api.Prefix + `/${id}/Actions/Export` });
}
