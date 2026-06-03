<template>
  <div class="customer-setup">
    <div style="display: flex">
      <h2>基础设置</h2>

      <HelpDoc docId="10010"
    /></div>
    <div class="card">
      <div class="row">
        <div class="label">
          <div class="title">客户重名检查</div>
          <div class="hint">开启后，重名客户将无法录入到系统</div>
        </div>
        <div class="control">
          <input type="checkbox" v-model="settings.checkDuplicateName" />
        </div>
      </div>

      <div class="row">
        <div class="label">
          <div class="title">客户手机号不允许重复</div>
          <div class="hint">开启后，重复的手机号码将无法录入到系统</div>
        </div>
        <div class="control">
          <input type="checkbox" v-model="settings.checkPhoneDuplicate" />
        </div>
      </div>

      <!-- <div class="row">
        <div class="label">
          <div class="title">报价单/合同订单支持自动计算</div>
          <div class="hint">开启后，报价金额/合同金额不支持手动填写，系统将自动计算商品的折后金额进行填充</div>
        </div>
        <div class="control">
          <input type="checkbox" v-model="settings.autoCalcQuote" />
        </div>
      </div> -->

      <!-- <div class="row">
        <div class="label">
          <div class="title">合同明细金额计算公式</div>
          <div class="hint">计算公式</div>
        </div>
        <div class="control">
          <input type="text" v-model="settings.contractFormula" class="full-input" />
          <div class="small-note">小数位数</div>
          <input type="number" v-model.number="settings.calcPrecision" class="small-input" />
        </div>
      </div> -->

      <!-- <div class="row">
        <div class="label">
          <div class="title">合同报价单明细金额计算公式</div>
          <div class="hint">计算公式</div>
        </div>
        <div class="control">
          <input type="text" v-model="settings.quoteDetailFormula" class="full-input" />
          <div class="small-note">小数位数</div>
          <input type="number" v-model.number="settings.quotePrecision" class="small-input" />
        </div>
      </div> -->

      <!-- <div class="row">
        <div class="label">
          <div class="title">手机号码全文检索</div>
          <div class="hint">开启后将无法按联系人手机号模糊搜索客户，只能通过手机号全文搜索客户</div>
        </div>
        <div class="control">
          <input type="checkbox" v-model="settings.phoneFullText" />
        </div>
      </div> -->

      <!-- <div class="row">
        <div class="label">
          <div class="title">合同审核设置</div>
          <div class="hint">合同审核通过后，将给以下人员发送催审消息</div>
        </div>
        <div class="control">
          <input type="text" v-model="settings.contractApproval" placeholder="输入人员或部门" class="full-input" />
        </div>
      </div> -->

      <!-- <div class="row">
        <div class="label">
          <div class="title">合同收款设置</div>
          <div class="hint">开启后，合同收款总额不能超过合同金额；关闭后，收款总金额不做限制。</div>
        </div>
        <div class="control">
          <input type="checkbox" v-model="settings.contractCollectLimit" />
        </div>
      </div> -->

      <!-- <div class="row">
        <div class="label">
          <div class="title">报价单分享字段设置</div>
          <div class="hint">选择显示用户展示的报价单明细字段，最多展示8个字段。</div>
        </div>
        <div class="control">
          <input type="text" v-model="settings.quoteFields" placeholder="示例：名称、型号、数量" class="full-input" />
        </div>
      </div> -->
      <!-- 
      <div class="row">
        <div class="label">
          <div class="title">合同订单即将逾期提醒</div>
          <div class="hint"
            >到合同订单交货日期前 <input type="number" v-model.number="settings.orderDueRemindDays" class="tiny-input" /> 天提醒创建人（每天早上9点提醒）</div
          >
        </div>
        <div class="control"></div>
      </div> -->

      <div class="actions">
        <button class="save-btn" @click="save">保存</button>
      </div>
    </div>
  </div>
</template>

<script setup>
  import { reactive, onMounted } from 'vue';
  import { defHttp } from '@/utils/http/axios';
  import { useMessage } from '@/hooks/web/useMessage';
  import HelpDoc from '@/components/HelpDoc/index.vue';

  // Known backend config IDs (from your screenshot/data)
  const ID_DUPLICATE_NAME = '2009104993759662080';
  const ID_PHONE_DUP = '2009105040731672576';

  const settings = reactive({
    checkDuplicateName: false,
    checkPhoneDuplicate: false,
    autoCalcQuote: false,
    contractFormula: '(${合同明细.单价}*${合同明细.销售数量})',
    calcPrecision: 8,
    quoteDetailFormula: '(${合同报价明细.单价}*${合同报价明细.数量})',
    quotePrecision: 8,
    phoneFullText: false,
    contractApproval: '',
    contractCollectLimit: false,
    quoteFields: '',
    orderDueRemindDays: 0,
  });

  // helpers to convert between backend F_Value and boolean
  function fValueToBool(value) {
    const v = value == null ? '' : String(value).trim();
    return v === '开' || v === 'true' || v === '1';
  }

  const backendConfigs = reactive({
    list: [],
  });
  const { createMessage } = useMessage();

  // Load backend configs and map known IDs to local settings
  onMounted(async () => {
    try {
      const res = await defHttp.post({ url: '/api/example/AConfig/customer', data: [] });
      // response shape may be { code,msg,data:[] } or an array directly
      const listFromRes = Array.isArray(res) ? res : res && Array.isArray(res.data) ? res.data : [];
      backendConfigs.list = listFromRes;
      // eslint-disable-next-line no-console

      const findById = id => (backendConfigs.list || []).find(x => String(x.id) === String(id));

      const duplicateNameItem = findById(ID_DUPLICATE_NAME);
      if (duplicateNameItem && duplicateNameItem.F_Value !== undefined) {
        settings.checkDuplicateName = fValueToBool(duplicateNameItem.F_Value);
      }

      const phoneDupItem = findById(ID_PHONE_DUP);
      if (phoneDupItem && phoneDupItem.F_Value !== undefined) {
        settings.checkPhoneDuplicate = fValueToBool(phoneDupItem.F_Value);
      }
    } catch (error) {
      // eslint-disable-next-line no-console
      console.error('failed to load customer configs', error);
    }
  });

  // Save mapped values back to backend (convert booleans to "id-value" strings)
  async function save() {
    try {
      // backend expects List<string> where each entry is "id" or "id-value"
      // value should be numeric string: "1" = 开, "0" = 关
      const payload = [`${ID_DUPLICATE_NAME}-${settings.checkDuplicateName ? '1' : '0'}`, `${ID_PHONE_DUP}-${settings.checkPhoneDuplicate ? '1' : '0'}`];

      const res = await defHttp.post({ url: '/api/example/AConfig/customer', data: payload });
      // normalize response to array
      const listFromRes = Array.isArray(res) ? res : res && Array.isArray(res.data) ? res.data : [];
      backendConfigs.list = listFromRes;

      // remap returned values back to UI (in case backend changed them)
      const findById = id => (backendConfigs.list || []).find(x => String(x.id) === String(id));
      const duplicateNameItem = findById(ID_DUPLICATE_NAME);
      if (duplicateNameItem && duplicateNameItem.F_Value !== undefined) {
        settings.checkDuplicateName = fValueToBool(duplicateNameItem.F_Value);
      }
      const phoneDupItem = findById(ID_PHONE_DUP);
      if (phoneDupItem && phoneDupItem.F_Value !== undefined) {
        settings.checkPhoneDuplicate = fValueToBool(phoneDupItem.F_Value);
      }

      createMessage.success('保存成功');
    } catch (err) {
      // eslint-disable-next-line no-console
      console.error('failed to save customer configs', err);
      createMessage.error('保存失败，请查看控制台');
    }
  }
</script>

<style scoped>
  .customer-setup {
    padding: 24px;
    font-family: 'Helvetica Neue', Arial, sans-serif;
    background: #f7f7f7;
  }
  h2 {
    font-size: 18px;
    margin-bottom: 16px;
    color: #333;
  }
  .card {
    background: #fff;
    border: 1px solid #ececec;
    padding: 16px;
    border-radius: 4px;
  }
  .row {
    display: flex;
    align-items: flex-start;
    padding: 12px 0;
    border-bottom: 1px solid #f0f0f0;
  }
  .row:last-child {
    border-bottom: none;
  }
  .label {
    width: 360px;
  }
  .title {
    font-weight: 600;
    color: #333;
    margin-bottom: 6px;
  }
  .hint {
    color: #888;
    font-size: 13px;
  }
  .control {
    flex: 1;
    display: flex;
    align-items: center;
    gap: 12px;
  }
  .full-input {
    width: 60%;
    padding: 6px;
    border: 1px solid #ddd;
    border-radius: 4px;
  }
  .small-input {
    width: 80px;
    padding: 6px;
    border: 1px solid #ddd;
    border-radius: 4px;
  }
  .tiny-input {
    width: 60px;
    padding: 4px;
    border: 1px solid #ddd;
    border-radius: 4px;
  }
  .small-note {
    font-size: 12px;
    color: #999;
    margin-left: 8px;
    margin-right: 8px;
  }
  .actions {
    display: flex;
    justify-content: flex-start;
    padding-top: 12px;
  }
  .save-btn {
    background: #1890ff;
    color: #fff;
    padding: 8px 18px;
    border: none;
    border-radius: 4px;
    cursor: pointer;
  }
  input[type='checkbox'] {
    width: 16px;
    height: 16px;
    accent-color: #1890ff;
  }
</style>
