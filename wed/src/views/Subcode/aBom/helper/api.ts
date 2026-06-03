import { defHttp } from '@/utils/http/axios';

enum Api {
  Prefix = '/api/example/ABom',
}


// 根据BOM获取用料清单
export function getBOMGoodsList(data) {
    return defHttp.post({ url: Api.Prefix + `/BOMGoodsList/` + data });
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
// 获取操作日志
export function getLogs(id) {
  return defHttp.get({ url: Api.Prefix + `/Logs/` + id });
}
// 下载模板
export function getTemplateDownload(data) {
  return defHttp.get({ url: Api.Prefix + `/TemplateDownload`, data });
}
