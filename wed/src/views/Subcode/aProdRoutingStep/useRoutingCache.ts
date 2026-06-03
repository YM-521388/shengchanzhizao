import { reactive, readonly } from 'vue';
import { buildUUID } from '@/utils/uuid';

export interface RoutingNode {
  jnpfId: string; // 前端行唯一键
  F_ProcessId: string; // 工序主键
  F_ProcessCode: string;
  F_ProcessName: string;
  F_DefectIds?: string;
  F_DefectHandle?: string;
  F_TaskTimer?: number;
  F_SortCode: number;
  F_Description?: string;
  F_CreatorTime?: string;
}

/* 缓存对象 */
const cache = reactive<{
  list: RoutingNode[]; // 真正给后端的数组
}>({
  list: [],
});

/* 供子组件调用的方法 */
export function useRoutingCache() {
  function reset(list: RoutingNode[] = []) {
    cache.list = list.map(item => ({ ...item }));
  }
  /* 新增 or 修改（按 jnpfId 匹配） */
  function upsert(node: RoutingNode) {
    const idx = cache.list.findIndex(r => r.jnpfId === node.jnpfId);
    if (idx > -1) cache.list.splice(idx, 1, node);
    else cache.list.push(node);
  }
  /* 删除 */
  function remove(jnpfId: string) {
    const idx = cache.list.findIndex(r => r.jnpfId === jnpfId);
    if (idx > -1) cache.list.splice(idx, 1);
  }
  return {
    list: readonly(cache.list),
    reset,
    upsert,
    remove,
  };
}
