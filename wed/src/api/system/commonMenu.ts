import { defHttp } from '@/utils/http/axios';

enum Api {
  Prefix = '/api/system/MenuData',
}

// 获取常用菜单列表
export function getCommonMenuList(data) {
  return defHttp.get({ url: Api.Prefix, data });
}
// 设为常用菜单
export function create(id) {
  return defHttp.post({ url: Api.Prefix + '/' + id });
}
// 删除常用菜单
export function delCommonMenu(id) {
  return defHttp.delete({ url: Api.Prefix + '/' + id });
}
