import { defHttp } from '@/utils/http/axios';

enum Api {
  Prefix = '/api/system/DataInterface',
}
// 获取数量
export function getNum() {
  return defHttp.post({ url: Api.Prefix + '/2017064114857316352/Actions/Preview' });
}
// 获取员工产能趋势
export function getProductTrend() {
  return defHttp.post({ url: Api.Prefix + '/2017076375495774208/Actions/Preview' });
}
// 不良品项分布
export function getDistr() {
  return defHttp.post({ url: Api.Prefix + '/2017053716754075648/Actions/Preview' });
}
// 人员不良品率
export function getDefectRate() {
  return defHttp.post({ url: Api.Prefix + '/2017054144778604544/Actions/Preview' });
}
// 实时报工数据
export function getRealtimeReport() {
  return defHttp.post({ url: Api.Prefix + '/2017075983546454016/Actions/Preview' });
}
// 人员生产统计
export function getProductStatistics() {
  return defHttp.post({ url: Api.Prefix + '/2017085670912299008/Actions/Preview' });
}
// 工单执行进度
export function getWorkOrder() {
  return defHttp.post({ url: Api.Prefix + '/2016811346536042496/Actions/Preview' });
}
