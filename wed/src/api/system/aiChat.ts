import { defHttp } from '@/utils/http/axios';

enum Api {
  Prefix = '/api/system/Aichat',
}

// ai发送对话
export function send(data) {
  return defHttp.post({ url: Api.Prefix + '/send', data });
}
//ai会话列表
export function historyList() {
  return defHttp.get({ url: Api.Prefix + '/history/list' });
}
//ai会话记录
export function historyGet(id) {
  return defHttp.get({ url: Api.Prefix + `/history/get/${id}` });
}
//保存历史记录
export function historySave(data) {
  return defHttp.post({ url: Api.Prefix + `/history/save`, data });
}
//删除历史记录
export function historyDelete(id) {
  return defHttp.delete({ url: Api.Prefix + `/history/delete/${id}` });
}
