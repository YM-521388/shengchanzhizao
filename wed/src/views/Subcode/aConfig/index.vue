<template>
  <div class="a-config">
    <h2 class="page-title">基础设置</h2>
    <div class="card">
      <div class="row">
        <div class="left">
          <div class="name">商品重名检查</div>
          <div class="desc">商品是否需要重名检查</div>
        </div>
        <div class="right">
          <label class="switch">
            <input type="checkbox" v-model="option1" />
            <span class="slider"></span>
          </label>
        </div>
      </div>

      <div class="row">
        <div class="left">
          <div class="name">商品编号自动填写</div>
          <div class="desc">开启后，商品编号为空时会自动填写</div>
        </div>
        <div class="right">
          <label class="switch">
            <input type="checkbox" v-model="option2" />
            <span class="slider"></span>
          </label>
        </div>
      </div>

      <!-- <div class="row">
          <div class="left">
            <div class="name">扫描商品二维码查看商品信息需要登录</div>
            <div class="desc">开启后，用户登录系统才能查看扫描商品二维码页面</div>
          </div>
          <div class="right">
            <label class="switch">
              <input type="checkbox" v-model="option3" />
              <span class="slider"></span>
            </label>
          </div>
        </div> -->

      <div class="actions">
        <button class="btn-save" @click="save">保存</button>
      </div>
    </div>
  </div>
</template>

<script setup>
  import { ref, onMounted } from 'vue';
  import { onlineUtils } from '@/utils/jnpf';
  import { updateByFixedIds } from './helper/api';

  const option1 = ref(false);
  const option2 = ref(false);
  const option3 = ref(false);
  const configList = ref([]);

  function mapResponseToOptions(list) {
    if (!list || !Array.isArray(list)) return;
    // 使用后端返回的 F_Name 与页面文本匹配以确定对应项
    const NAME1 = '商品重名检查';
    const NAME2 = '商品编号自动填写';
    const NAME3 = '扫描商品二维码查看商品信息需要登录';
    const findByName = name => list.find(i => i.F_Name === name);
    const a = findByName(NAME1);
    const b = findByName(NAME2);
    const c = findByName(NAME3);
    option1.value = a ? a.F_Value === '开' : false;
    option2.value = b ? b.F_Value === '开' : false;
    option3.value = c ? c.F_Value === '开' : false;
    configList.value = list;
  }

  // 页面进入时请求后端获取三条配置（后端支持 POST 空数组 返回默认三条）
  onMounted(async () => {
    try {
      const res = await updateByFixedIds([]);
      const data = Array.isArray(res) ? res : res && res.data ? res.data : [];
      mapResponseToOptions(data);
    } catch (err) {
      console.error('获取配置失败', err);
    }
  });

  async function save() {
    // 构造要提交的 id-value 列表（格式 id-value）
    // 按 F_Name 查找到每一行对应的记录（更稳健，不依赖 F_Code）
    const NAME1 = '商品重名检查';
    const NAME2 = '商品编号自动填写';
    const NAME3 = '扫描商品二维码查看商品信息需要登录';
    const findByName = name => configList.value.find(i => i.F_Name === name);
    const a = findByName(NAME1);
    const b = findByName(NAME2);
    const c = findByName(NAME3);
    const payload = [];
    if (a) payload.push(`${a.id}-${option1.value ? '1' : '0'}`);
    if (b) payload.push(`${b.id}-${option2.value ? '1' : '0'}`);
    if (c) payload.push(`${c.id}-${option3.value ? '1' : '0'}`);

    try {
      const res = await updateByFixedIds(payload);
      // 更新成功后刷新页面显示（兼容后端包裹结构）
      const data = Array.isArray(res) ? res : res && res.data ? res.data : [];
      mapResponseToOptions(data);
      onlineUtils.toast('保存成功', 'success', 2000);
    } catch (err) {
      console.error('保存失败', err);
      onlineUtils.toast('保存失败', 'error', 2000);
    }
  }
</script>

<style scoped>
  .a-config {
    max-width: 820px;
    margin: 24px auto;
    font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial;
    color: #333;
  }
  .page-title {
    font-size: 18px;
    margin: 0 0 12px 6px;
  }
  .card {
    background: #fff;
    border-radius: 6px;
    padding: 18px;
    box-shadow: 0 1px 0 rgba(0, 0, 0, 0.04);
  }
  .row {
    display: flex;
    align-items: center;
    justify-content: space-between;
    padding: 14px 6px;
    border-bottom: 1px solid #f0f0f0;
  }
  .row:last-child {
    border-bottom: none;
  }
  .left {
    flex: 1;
  }
  .name {
    font-size: 14px;
    color: #333;
    margin-bottom: 6px;
  }
  .desc {
    font-size: 12px;
    color: #888;
  }
  .right {
    width: 120px;
    display: flex;
    justify-content: flex-end;
    align-items: center;
  }

  /* 自定义开关 */
  .switch {
    position: relative;
    display: inline-block;
    width: 46px;
    height: 26px;
  }
  .switch input {
    opacity: 0;
    width: 0;
    height: 0;
  }
  .slider {
    position: absolute;
    cursor: pointer;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background-color: #e6e6e6;
    border-radius: 26px;
    transition: 0.2s;
  }
  .slider:before {
    position: absolute;
    content: '';
    height: 20px;
    width: 20px;
    left: 3px;
    bottom: 3px;
    background-color: white;
    border-radius: 50%;
    box-shadow: 0 1px 2px rgba(0, 0, 0, 0.2);
    transition: 0.2s;
  }
  .switch input:checked + .slider {
    background-color: #13c18b;
  }
  .switch input:checked + .slider:before {
    transform: translateX(20px);
  }

  .actions {
    padding-top: 18px;
    display: flex;
    justify-content: flex-start;
  }
  .btn-save {
    background-color: #13c18b;
    color: #fff;
    border: none;
    padding: 8px 18px;
    border-radius: 4px;
    cursor: pointer;
    font-size: 14px;
  }
  .btn-save:hover {
    opacity: 0.95;
  }
</style>
