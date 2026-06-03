<template>
  <BasicModal v-bind="$attrs" @register="registerModal" :title="title" width="70%" :minHeight="100" :showOkBtn="false" :canFullscreen="true">
    <template #insertFooter> </template>
    <a-row class="dynamic-form">
      <a-form :colon="false" layout="horizontal" labelAlign="right" :labelCol="{ style: { width: '100px' } }" :model="dataForm" ref="formRef">
        <a-row :gutter="15">
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_PlanNo')">
            <a-form-item name="F_PlanNo" :labelCol="{ style: { width: '100px' } }">
              <template #label>计划编号</template>
              <JnpfInput
                v-model:value="dataForm.F_PlanNo"
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_PlanName')">
            <a-form-item name="F_PlanName" :labelCol="{ style: { width: '100px' } }">
              <template #label>计划名称</template>
              <JnpfInput
                v-model:value="dataForm.F_PlanName"
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_SupplierId')">
            <a-form-item name="F_SupplierId" :labelCol="{ style: { width: '100px' } }">
              <template #label>供应商</template>
              <p>{{ dataForm.F_SupplierId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_Money')">
            <a-form-item name="F_Money" :labelCol="{ style: { width: '100px' } }">
              <template #label>金额</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_Money"
                placeholder="请输入"
                :controls="false"
                :style="{
                  width: '100%',
                }"
                disabled
                detailed />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_DeliveryDate')">
            <a-form-item name="F_DeliveryDate" :labelCol="{ style: { width: '100px' } }">
              <template #label>交货日期</template>
              <p>{{ dataForm.F_DeliveryDate }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_PurchaseNum')">
            <a-form-item name="F_PurchaseNum" :labelCol="{ style: { width: '100px' } }">
              <template #label>采购数量</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_PurchaseNum"
                placeholder="请输入"
                :controls="false"
                :style="{
                  width: '100%',
                }"
                disabled
                detailed />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_ResponsibleUserId')">
            <a-form-item name="F_ResponsibleUserId" :labelCol="{ style: { width: '100px' } }">
              <template #label>负责人</template>
              <p>{{ dataForm.F_ResponsibleUserId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_CheckState')">
            <a-form-item name="F_CheckState" :labelCol="{ style: { width: '100px' } }">
              <template #label>审核状态</template>
              <p>{{ dataForm.F_CheckState }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_CheckUserId')">
            <a-form-item name="F_CheckUserId" :labelCol="{ style: { width: '100px' } }">
              <template #label>审核人员</template>
              <p>{{ dataForm.F_CheckUserId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_CheckTime')">
            <a-form-item name="F_CheckTime" :labelCol="{ style: { width: '100px' } }">
              <template #label>审核时间</template>
              <p>{{ dataForm.F_CheckTime }}</p>
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
              <JnpfGroupTitle content="选择采购商品" :bordered="false" />
              <a-table
                :data-source="dataForm.tableFieldc87c94"
                :columns="tableFieldc87c94Columns"
                size="small"
                :pagination="false"
                :scroll="{ x: 'max-content' }">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                  <template v-if="column.key === 'F_GoodsName'">
                    <p class="leading-32px">{{ record.F_GoodsName }}</p>
                  </template>
                  <template v-if="column.key === 'F_GoodsNo'">
                    <p class="leading-32px">{{ record.F_GoodsNo }}</p>
                  </template>
                  <template v-if="column.key === 'F_Specification'">
                    <p class="leading-32px">{{ record.F_Specification }}</p>
                  </template>
                  <template v-if="column.key === 'F_Image'">
                    <template v-if="record.F_Image">
                      <img :src="record.F_Image" alt="image" style="max-width: 120px; max-height: 40px; object-fit: contain" />
                    </template>
                    <template v-else>
                      <p class="leading-32px">{{ record.F_Image }}</p>
                    </template>
                  </template>
                  <template v-if="column.key === 'F_SupplierId'">
                    <p>{{ record.F_SupplierId }}</p>
                  </template>
                  <template v-if="column.key === 'F_Num'">
                    <p class="leading-32px">{{ record.F_Num }}</p>
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
                  <template v-if="column.key === 'F_Amount'">
                    <p>{{ thousandsFormat(record.F_Amount || 0) }}</p>
                  </template>
                  <template v-if="column.key === 'F_Description'">
                    <p>{{ record.F_Description }}</p>
                  </template>
                </template>
              </a-table>
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="采购计划日志" :bordered="false" />
              <a-table
                :data-source="dataForm.tableFielde82301"
                :columns="tableFielde82301Columns"
                size="small"
                :pagination="{ pageSize: 20 }"
                :scroll="{ x: 'max-content' }">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
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
                    <p>{{ record.F_Content }}</p>
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
    tableFieldc87c94outerActiveKey: number[];
    tableFieldc87c94innerActiveKey: string[];
    tableFielde82301outerActiveKey: number[];
    tableFielde82301innerActiveKey: string[];
  }

  defineOptions({ name: 'Detail' });
  const [registerModal, { openModal, closeModal, setModalProps }] = useModal();
  const { t } = useI18n();
  const tableFieldc87c94Columns: any[] = computed(() => {
    let list = [
      {
        title: '商品',
        dataIndex: 'F_GoodsName',
        key: 'F_GoodsName',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_GoodsName',
        fixed: false,
      },
      {
        title: '商品编号',
        dataIndex: 'F_GoodsNo',
        key: 'F_GoodsNo',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_GoodsNo',
        fixed: false,
        isSystemControl: true,
      },
      {
        title: '规格',
        dataIndex: 'F_Specification',
        key: 'F_Specification',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_Specification',
        fixed: false,
        isSystemControl: true,
      },
      // {
      //   title: '商品图片',
      //   dataIndex: 'F_Image',
      //   key: 'F_Image',
      //   tipLabel: '',
      //   align: 'left',
      //   span: '24',
      //   labelWidth: '',
      //   width: '',
      //   formP: 'F_Image',
      //   fixed: false,
      //   isSystemControl: true,
      // },
      {
        title: '供应商',
        dataIndex: 'F_SupplierId',
        key: 'F_SupplierId',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_SupplierId',
        fixed: false,
      },
      {
        title: '数量',
        dataIndex: 'F_Num',
        key: 'F_Num',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_Num',
        fixed: false,
      },
      {
        title: '单价',
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
        title: '金额',
        dataIndex: 'F_Amount',
        key: 'F_Amount',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_Amount',
        fixed: false,
        isSystemControl: true,
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
    list = list.filter(o => hasFormP('Detail_CGSP-' + o.formP));
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
  const tableFielde82301Columns: any[] = computed(() => {
    let list = [
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
        title: '修改用户',
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
        title: '修改时间',
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
    // 过滤权限控制的字段
    list = list.filter(o => hasFormP('tableFielde82301-' + o.formP));
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
    tableFieldc87c94outerActiveKey: [0],
    tableFieldc87c94innerActiveKey: [],
    tableFielde82301outerActiveKey: [0],
    tableFielde82301innerActiveKey: [],
  });
  const { title, dataForm } = toRefs(state);
  const { hasFormP } = usePermission();

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

      // ensure each row has a jnpfId used as rowKey
      const rows = state.dataForm.tableFieldc87c94 || [];
      for (let i = 0; i < rows.length; i++) {
        const element = rows[i];
        element.jnpfId = buildUUID();
      }
      for (let i = 0; i < state.dataForm.tableFielde82301.length; i++) {
        const element = state.dataForm.tableFielde82301[i];
        element.jnpfId = buildUUID();
      }

      // calculate per-row amount and total money, collect ids to fetch display fields
      let totalMoney = 0;
      const idsToFetch: any[] = [];
      // 从字符串中提取数字（支持小数，如 "5箱子50盒" -> 5, "5.5箱子" -> 5.5）
      const extractNumber = (val: any): number => {
        if (val == null) return 0;
        if (typeof val === 'number') return val;
        const str = String(val);
        const match = str.match(/^-?\d+(\.\d+)?/);
        return match ? parseFloat(match[0]) : 0;
      };
      for (let i = 0; i < rows.length; i++) {
        const r = rows[i] || {};
        const num = extractNumber(r.F_Num);
        const price = Number(r.F_Price) || 0;
        r.F_Amount = num * price;
        totalMoney += r.F_Amount;
        // if display fields missing but have relation id, fetch them
        if (r.F_CustomerId && !(r.F_GoodsNo || r.F_Specification || r.F_Image)) idsToFetch.push(r.F_CustomerId);
      }
      state.dataForm.F_Money = totalMoney;

      // fetch display-only relation fields (商品编号/规格/图片) when missing
      if (idsToFetch.length) {
        const uniqueIds = Array.from(new Set(idsToFetch));
        const interfaceId = '2008721559174385664';
        const query = {
          ids: uniqueIds,
          interfaceId,
          propsValue: 'F_Id',
          relationField: 'F_GoodsName',
          paramList: [],
        };
        getDataInterfaceDataInfoByIds(interfaceId, query)
          .then(resp => {
            const list = resp.data || [];
            const idKey = 'F_Id';
            const map: Record<string, any> = {};
            list.forEach((item: any) => {
              if (item && item[idKey] != null) map[String(item[idKey])] = item;
            });
            for (let i = 0; i < rows.length; i++) {
              const r = rows[i];
              if (!r) continue;
              const found = map[String(r.F_CustomerId)];
              if (found) {
                r.F_GoodsNo = found.F_GoodsNo ?? found.GoodsNo ?? found.F_GoodsName ?? found.name ?? r.F_GoodsNo;
                r.F_Specification = found.F_Specification ?? found.Specification ?? found.spec ?? r.F_Specification;
                // image handling (prefer common fields)
                let imgVal = found.F_Image ?? found.Image ?? found.img ?? found.picture ?? found.fileUrl ?? found.url ?? found.imageUrl ?? null;
                try {
                  if (typeof imgVal === 'string' && imgVal.trim().startsWith('[')) {
                    const parsed = JSON.parse(imgVal);
                    if (Array.isArray(parsed) && parsed.length) {
                      const first = parsed[0];
                      imgVal = first.url ?? first.fileUrl ?? first.value ?? first.thumbUrl ?? first.name ?? imgVal;
                    }
                  } else if (Array.isArray(imgVal) && imgVal.length) {
                    const first = imgVal[0];
                    imgVal = first.url ?? first.fileUrl ?? first.value ?? first.thumbUrl ?? first.name ?? imgVal[0];
                  }
                } catch (e) {
                  // ignore parse errors
                }
                r.F_Image = imgVal || r.F_Image;
              }
            }
          })
          .catch(() => {
            // ignore errors - fallback is showing empty display fields
          });
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
