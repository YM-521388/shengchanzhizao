<template>
  <BasicModal v-bind="$attrs" @register="registerModal" :title="title" width="70%" :minHeight="100" :showOkBtn="false" :canFullscreen="true">
    <template #insertFooter> </template>
    <a-row class="dynamic-form">
      <a-form :colon="false" layout="horizontal" labelAlign="right" :labelCol="{ style: { width: '100px' } }" :model="dataForm" ref="formRef">
        <a-row :gutter="15">
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_StockInNo')">
            <a-form-item name="F_StockInNo" :labelCol="{ style: { width: '100px' } }">
              <template #label>采购入库单号</template>
              <JnpfInput
                v-model:value="dataForm.F_StockInNo"
                :maskConfig="{
                  filler: '*',
                  maskType: 1,
                  prefixType: 1,
                  prefixLimit: 0,
                  prefixSpecifyChar: '',
                  suffixType: 1,
                  suffixLimit: 0,
                  suffixSpecifyChar: '',
                  ignoreChar: '',
                  useUnrealMask: false,
                  unrealMaskLength: 1,
                }"
                disabled
                detailed />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_StockInType')">
            <a-form-item name="F_StockInType" :labelCol="{ style: { width: '100px' } }">
              <template #label>入库类型</template>
              <p>{{ dataForm.F_StockInType }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_StockInDate')">
            <a-form-item name="F_StockInDate" :labelCol="{ style: { width: '100px' } }">
              <template #label>入库日期</template>
              <p>{{ dataForm.F_StockInDate }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_StockInUserId')">
            <a-form-item name="F_StockInUserId" :labelCol="{ style: { width: '100px' } }">
              <template #label>收货人</template>
              <p>{{ dataForm.F_StockInUserId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_pu_Orderld')">
            <a-form-item name="F_pu_Orderld" :labelCol="{ style: { width: '100px' } }">
              <template #label>采购单</template>
              <p>{{ dataForm.F_pu_Orderld }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_Description')">
            <a-form-item name="F_Description" :labelCol="{ style: { width: '100px' } }">
              <template #label>备注</template>
              <p>{{ dataForm.F_Description }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="采购入库商品" :bordered="false" />
              <a-table
                :data-source="dataForm.tableField4bd139"
                :columns="tableField4bd139Columns"
                size="small"
                :pagination="false"
                :scroll="{ x: 'max-content' }">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                  <template v-if="column.key === 'F_CustomerId'">
                    <p class="leading-32px">{{ record.F_CustomerId }}</p>
                    <ExtraRelationInfo
                      :extraOptions="state.extraOptions.F_CustomerId"
                      :data="state.extraData.F_CustomerId"
                      v-if="state.extraOptions.F_CustomerId?.length && state.extraData.F_CustomerId && JSON.stringify(state.extraData.F_CustomerId) !== '{}'" />
                  </template>
                  <template v-if="column.key === 'F_NumInfo'">
                    <p>{{ record.F_NumInfo }}</p>
                  </template>
                  <template v-if="column.key === 'F_Price'">
                    <JnpfInputNumber
                      v-model:value="record.F_Price"
                      placeholder="请输入"
                      :controls="false"
                      :style="{
                        width: '100%',
                      }"
                      disabled
                      detailed />
                  </template>
                  <template v-if="column.key === 'F_WarehouseID'">
                    <p>{{ record.F_WarehouseID }}</p>
                  </template>
                  <template v-if="column.key === 'F_Description'">
                    <p>{{ record.F_Description }}</p>
                  </template>
                  <template v-if="column.key === 'F_CreatorUserId'">
                    <p>{{ record.F_CreatorUserId }}</p>
                  </template>
                  <template v-if="column.key === 'F_CreatorTime'">
                    <p>{{ record.F_CreatorTime }}</p>
                  </template>
                </template>
              </a-table>
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="采购入库日志" :bordered="false" />
              <a-table
                :data-source="dataForm.tableFieldca527d"
                :columns="tableFieldca527dColumns"
                size="small"
                :pagination="false"
                :scroll="{ x: 'max-content' }">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                  <template v-if="column.key === 'F_Type'">
                    <JnpfInput
                      v-model:value="record.F_Type"
                      :maskConfig="{
                        filler: '*',
                        maskType: 1,
                        prefixType: 1,
                        prefixLimit: 0,
                        prefixSpecifyChar: '',
                        suffixType: 1,
                        suffixLimit: 0,
                        suffixSpecifyChar: '',
                        ignoreChar: '',
                        useUnrealMask: false,
                        unrealMaskLength: 1,
                      }"
                      disabled
                      detailed />
                  </template>
                  <template v-if="column.key === 'F_Title'">
                    <JnpfInput
                      v-model:value="record.F_Title"
                      :maskConfig="{
                        filler: '*',
                        maskType: 1,
                        prefixType: 1,
                        prefixLimit: 0,
                        prefixSpecifyChar: '',
                        suffixType: 1,
                        suffixLimit: 0,
                        suffixSpecifyChar: '',
                        ignoreChar: '',
                        useUnrealMask: false,
                        unrealMaskLength: 1,
                      }"
                      disabled
                      detailed />
                  </template>
                  <template v-if="column.key === 'F_Content'">
                    <JnpfInput
                      v-model:value="record.F_Content"
                      :maskConfig="{
                        filler: '*',
                        maskType: 1,
                        prefixType: 1,
                        prefixLimit: 0,
                        prefixSpecifyChar: '',
                        suffixType: 1,
                        suffixLimit: 0,
                        suffixSpecifyChar: '',
                        ignoreChar: '',
                        useUnrealMask: false,
                        unrealMaskLength: 1,
                      }"
                      disabled
                      detailed />
                  </template>
                  <template v-if="column.key === 'F_Reason'">
                    <p>{{ record.F_Reason }}</p>
                  </template>
                  <!-- <template v-if="column.key === 'F_CreatorUserId'">
                    <p>{{ record.F_CreatorUserId }}</p>
                  </template>
                  <template v-if="column.key === 'F_CreatorTime'">
                    <p>{{ record.F_CreatorTime }}</p>
                  </template> -->
                </template>
              </a-table>
            </a-form-item>
          </a-col>
        </a-row>
      </a-form>
    </a-row>
  </BasicModal>
</template>
<script lang="ts" setup>
  import { getDetail } from './helper/api';
  import { getConfigData } from '@/api/onlineDev/visualDev';
  import { reactive, toRefs, nextTick, ref, computed, unref, toRaw } from 'vue';
  import { thousandsFormat } from '@/utils/jnpf';
  import { CaretRightOutlined } from '@ant-design/icons-vue';
  import { buildUUID } from '@/utils/uuid';
  import { useI18n } from '@/hooks/web/useI18n';
  import { getDataChange } from '@/api/onlineDev/visualDev';
  import { getDataInterfaceDataInfoByIds } from '@/api/systemData/dataInterface';
  import ExtraRelationInfo from '@/components/Jnpf/RelationForm/src/ExtraRelationInfo.vue';
  import { BasicModal, useModal } from '@/components/Modal';
  import { usePermission } from '@/hooks/web/usePermission';

  interface State {
    dataForm: any;
    interfaceRes: any;
    extraOptions: any;
    extraData: any;
    title: string;
    tableField4bd139outerActiveKey: number[];
    tableField4bd139innerActiveKey: string[];
    tableFieldca527douterActiveKey: number[];
    tableFieldca527dinnerActiveKey: string[];
  }

  defineOptions({ name: 'Detail' });
  const [registerModal, { openModal, closeModal, setModalProps }] = useModal();
  const { t } = useI18n();
  const tableField4bd139Columns: any[] = computed(() => {
    let list = [
      {
        title: '商品',
        dataIndex: 'F_CustomerId',
        key: 'F_CustomerId',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_CustomerId',
        fixed: false,
      },
      {
        title: '商品编码',
        dataIndex: 'F_Encoding',
        key: 'F_Encoding',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_Encoding',
        fixed: false,
      },
      {
        title: '数量',
        dataIndex: 'F_NumInfo',
        key: 'F_NumInfo',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_NumInfo',
        fixed: false,
      },
      {
        title: '成本单价(元)',
        dataIndex: 'F_Price',
        key: 'F_Price',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_Price',
        fixed: false,
      },
      {
        title: '入库仓库',
        dataIndex: 'F_WarehouseID',
        key: 'F_WarehouseID',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_WarehouseID',
        fixed: false,
      },
      {
        title: '备注',
        dataIndex: 'F_Description',
        key: 'F_Description',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_Description',
        fixed: false,
      },
    ];
    list = list.filter(o => hasFormP('Detail_CGRKD-' + o.formP));
    let columnList = list;
    let complexHeaderList: any[] = [];
    if (complexHeaderList.length) {
      let childColumns: any[] = [];
      let firstChildColumns: string[] = [];
      for (let i = 0; i < complexHeaderList.length; i++) {
        const e = complexHeaderList[i];
        e.title = e.fullName;
        e.align = e.align;
        e.children = [];
        e.jnpfKey = 'complexHeader';
        if (e.childColumns?.length) {
          childColumns.push(...e.childColumns);
          for (let k = 0; k < e.childColumns.length; k++) {
            const item = e.childColumns[k];
            for (let j = 0; j < list.length; j++) {
              const o = list[j];
              if (o.dataIndex == item && o.fixed !== 'left' && o.fixed !== 'right') e.children.push({ ...o });
            }
          }
        }
        if (e.children.length) firstChildColumns.push(e.children[0].dataIndex);
      }
      complexHeaderList = complexHeaderList.filter(o => o.children.length);
      let newList: any[] = [];
      for (let i = 0; i < list.length; i++) {
        const e = list[i];
        if (!childColumns.includes(e.dataIndex) || e.fixed === 'left' || e.fixed === 'right') {
          newList.push(e);
        } else {
          if (firstChildColumns.includes(e.dataIndex)) {
            const item = complexHeaderList.find(o => o.childColumns.includes(e.dataIndex));
            newList.push(item);
          }
        }
      }
      columnList = newList;
    }
    const noColumn = { title: t('component.table.index'), dataIndex: 'index', key: 'index', align: 'center', width: 50, fixed: 'left' };
    const actionColumn = { title: t('component.table.action'), dataIndex: 'action', key: 'action', align: 'center', width: 50, fixed: 'right' };
    let columns = [noColumn, ...columnList];
    const leftFixedList = columns.filter(o => o.fixed === 'left');
    const rightFixedList = columns.filter(o => o.fixed === 'right');
    const noFixedList = columns.filter(o => o.fixed !== 'left' && o.fixed !== 'right');
    return [...leftFixedList, ...noFixedList, ...rightFixedList];
  });
  const tableFieldca527dColumns: any[] = computed(() => {
    let list = [
      // {
      //   title: '类型',
      //   dataIndex: 'F_Type',
      //   key: 'F_Type',
      //   tipLabel: '',
      //   align: 'left',
      //   span: '24',
      //   labelWidth: '',
      //   width: '',
      //   formP: 'F_Type',
      //   fixed: false,
      // },
      {
        title: '标题',
        dataIndex: 'F_Title',
        key: 'F_Title',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_Title',
        fixed: false,
      },
      {
        title: '内容',
        dataIndex: 'F_Content',
        key: 'F_Content',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_Content',
        fixed: false,
      },
      {
        title: '修改原因',
        dataIndex: 'F_Reason',
        key: 'F_Reason',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_Reason',
        fixed: false,
      },
      {
        title: '创建人员',
        dataIndex: 'F_CreatorUserId',
        key: 'F_CreatorUserId',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_CreatorUserId',
        fixed: false,
      },
      {
        title: '创建时间',
        dataIndex: 'F_CreatorTime',
        key: 'F_CreatorTime',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_CreatorTime',
        fixed: false,
      },
    ];
    list = list.filter(o => hasFormP('tableFieldca527d-' + o.formP));
    let columnList = list;
    let complexHeaderList: any[] = [];
    if (complexHeaderList.length) {
      let childColumns: any[] = [];
      let firstChildColumns: string[] = [];
      for (let i = 0; i < complexHeaderList.length; i++) {
        const e = complexHeaderList[i];
        e.title = e.fullName;
        e.align = e.align;
        e.children = [];
        e.jnpfKey = 'complexHeader';
        if (e.childColumns?.length) {
          childColumns.push(...e.childColumns);
          for (let k = 0; k < e.childColumns.length; k++) {
            const item = e.childColumns[k];
            for (let j = 0; j < list.length; j++) {
              const o = list[j];
              if (o.dataIndex == item && o.fixed !== 'left' && o.fixed !== 'right') e.children.push({ ...o });
            }
          }
        }
        if (e.children.length) firstChildColumns.push(e.children[0].dataIndex);
      }
      complexHeaderList = complexHeaderList.filter(o => o.children.length);
      let newList: any[] = [];
      for (let i = 0; i < list.length; i++) {
        const e = list[i];
        if (!childColumns.includes(e.dataIndex) || e.fixed === 'left' || e.fixed === 'right') {
          newList.push(e);
        } else {
          if (firstChildColumns.includes(e.dataIndex)) {
            const item = complexHeaderList.find(o => o.childColumns.includes(e.dataIndex));
            newList.push(item);
          }
        }
      }
      columnList = newList;
    }
    const noColumn = { title: t('component.table.index'), dataIndex: 'index', key: 'index', align: 'center', width: 50, fixed: 'left' };
    const actionColumn = { title: t('component.table.action'), dataIndex: 'action', key: 'action', align: 'center', width: 50, fixed: 'right' };
    let columns = [noColumn, ...columnList];
    const leftFixedList = columns.filter(o => o.fixed === 'left');
    const rightFixedList = columns.filter(o => o.fixed === 'right');
    const noFixedList = columns.filter(o => o.fixed !== 'left' && o.fixed !== 'right');
    return [...leftFixedList, ...noFixedList, ...rightFixedList];
  });

  const relationDetailRef = ref<any>(null);
  const state = reactive<State>({
    dataForm: {},
    interfaceRes: {},
    extraOptions: {},
    extraData: {},
    title: '详情',
    tableField4bd139outerActiveKey: [0],
    tableField4bd139innerActiveKey: [],
    tableFieldca527douterActiveKey: [0],
    tableFieldca527dinnerActiveKey: [],
  });
  const { title, dataForm } = toRefs(state);
  const { hasFormP } = usePermission();

  /** 解析 F_Unit_Ratio 为数组（与 Form 中商品单位结构一致） */
  function parseUnitRatioArray(raw: unknown): { id?: string; name?: string; rate?: number; qty?: number }[] | null {
    if (raw == null || raw === '') return null;
    try {
      const str = typeof raw === 'string' ? raw : String(raw);
      const arr = JSON.parse(str);
      return Array.isArray(arr) && arr.length ? arr : null;
    } catch {
      return null;
    }
  }

  /**
   * 详情数量展示：F_Num 存的是第二级单位数量（如「盒」），按 F_Unit_Ratio 换算为「1箱子0盒0瓶」
   * 规则与 Form.vue 一致：第二项 qty 表示多少第二单位 = 1 第一单位；余数为第二单位个数；第三级名称占位为 0（整数第二单位时）
   */
  function formatNumByUnitRatio(record: any): string {
    const n = Number(record?.F_Num);
    if (Number.isNaN(n)) return '';
    const units = parseUnitRatioArray(record?.F_Unit_Ratio);
    if (!units || units.length < 2) return String(record.F_Num);

    const q1 = Number(units[1]?.qty) || 0;
    if (q1 <= 0) return String(record.F_Num);

    const name0 = units[0]?.name ?? '';
    const name1 = units[1]?.name ?? '';
    // 第一单位个数、第二单位余数（F_Num 以第二单位计）
    const count0 = Math.floor(n / q1);
    const count1 = n % q1;

    if (units.length >= 3) {
      const name2 = units[2]?.name ?? '';
      const count2 = 0;
      return `${count0}${name0}${count1}${name1}${count2}${name2}`;
    }
    return `${count0}${name0}${count1}${name1}`;
  }

  defineExpose({ init });

  function init(data) {
    state.dataForm.id = data.id;
    openModal();
    nextTick(() => {
      setTimeout(initData, 0);
    });
  }
  function initData() {
    changeLoading(true);
    if (state.dataForm.id) {
      getData(state.dataForm.id);
    } else {
      closeModal();
    }
  }
  function getData(id) {
    getDetail(id).then(res => {
      state.dataForm = res.data || {};

      for (let i = 0; i < state.dataForm.tableField4bd139.length; i++) {
        const element = state.dataForm.tableField4bd139[i];
        element.jnpfId = buildUUID();
      }
      for (let i = 0; i < state.dataForm.tableFieldca527d.length; i++) {
        const element = state.dataForm.tableFieldca527d[i];
        element.jnpfId = buildUUID();
      }
      nextTick(() => {
        changeLoading(false);
      });
    });
  }
  function setFormProps(data) {
    setModalProps(data);
  }
  function changeLoading(loading) {
    setFormProps({ loading });
  }
  function getParamList(key) {
    let templateJson: any[] = state.interfaceRes[key];
    if (!templateJson || !templateJson.length || !state.dataForm) return templateJson;
    for (let i = 0; i < templateJson.length; i++) {
      if (templateJson[i].relationField && templateJson[i].sourceType == 1) {
        templateJson[i].defaultValue = state.dataForm[templateJson[i].relationField + '_id'] || '';
      }
    }
    return templateJson;
  }
</script>
