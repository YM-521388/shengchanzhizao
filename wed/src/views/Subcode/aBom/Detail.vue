<template>
  <BasicModal v-bind="$attrs" @register="registerModal" :title="title" width="80%" :minHeight="100" :showOkBtn="false" :canFullscreen="true">
    <template #insertFooter> </template>
    <a-row class="dynamic-form">
      <a-form :colon="false" layout="horizontal" labelAlign="right" :labelCol="{ style: { width: '100px' } }" :model="dataForm" ref="formRef">
        <a-row :gutter="15">
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_BomCode')">
            <a-form-item name="F_BomCode" :labelCol="{ style: { width: '100px' } }">
              <template #label>BOM编号：</template>
              <JnpfInput
                v-model:value="dataForm.F_BomCode"
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_BomName')">
            <a-form-item name="F_BomName" :labelCol="{ style: { width: '100px' } }">
              <template #label>BOM名称：</template>
              <JnpfInput
                v-model:value="dataForm.F_BomName"
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_CategoryId')">
            <a-form-item name="F_CategoryId" :labelCol="{ style: { width: '100px' } }">
              <template #label>类别：</template>
              <p>{{ dataForm.F_CategoryId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_GoodsId')">
            <a-form-item name="F_GoodsId" :labelCol="{ style: { width: '100px' } }">
              <template #label>商品：</template>
              <p class="leading-32px">{{ dataForm.F_GoodsId }}</p>
              <ExtraRelationInfo
                :extraOptions="state.extraOptions.F_GoodsId"
                :data="state.extraData.F_GoodsId"
                v-if="state.extraOptions.F_GoodsId?.length && state.extraData.F_GoodsId && JSON.stringify(state.extraData.F_GoodsId) !== '{}'" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_IsDefault')">
            <a-form-item name="F_IsDefault" :labelCol="{ style: { width: '100px' } }">
              <template #label>默认BOM：</template>
              <p>{{ dataForm.F_IsDefault }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_State')">
            <a-form-item name="F_State" :labelCol="{ style: { width: '100px' } }">
              <template #label>备注：</template>
              <p>{{ dataForm.F_State }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="增加商品" :bordered="false" />
              <a-table
                :data-source="dataForm.tableFieldd2a80d"
                :columns="tableFieldd2a80dColumns"
                size="small"
                :pagination="false"
                :scroll="{ x: 'max-content' }">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ record.displayIndex || index + 1 }}</template>
                  <template v-if="column.key === 'F_GoodsId'">
                    <p class="leading-32px">{{ record.F_GoodsId }}</p>
                    <ExtraRelationInfo
                      :extraOptions="state.extraOptions.F_GoodsId"
                      :data="state.extraData.F_GoodsId"
                      v-if="state.extraOptions.F_GoodsId?.length && state.extraData.F_GoodsId && JSON.stringify(state.extraData.F_GoodsId) !== '{}'" />
                  </template>
                  <template v-if="column.key === 'F_GoodsNo'">
                    <p>{{ record.F_GoodsNo }}</p>
                  </template>
                  <template v-if="column.key === 'F_Specification'">
                    <p>{{ record.F_Specification }}</p>
                  </template>
                  <template v-if="column.key === 'F_Source'">
                    <p>{{ record.F_Source }}</p>
                  </template>
                  <template v-if="column.key === 'F_Process'">
                    <p>{{ record.F_Process }}</p>
                  </template>
                  <template v-if="column.key === 'F_Image'">
                    <div style="min-width: 80px">
                      <JnpfUploadImg v-model:value="record.F_Image" pathType="defaultPath" sizeUnit="MB" :fileSize="10" disabled detailed />
                    </div>
                  </template>
                  <template v-if="column.key === 'F_Num'">
                    <JnpfInputNumber
                      v-model:value="record.F_NumUnit"
                      placeholder="请输入"
                      :controls="false"
                      :style="{
                        width: '100%',
                      }"
                      disabled
                      detailed />
                  </template>
                  <template v-if="column.key === 'F_Unitsp'">
                    <p>{{ record.F_Unitsp }}</p>
                  </template>
                  <template v-if="column.key === 'F_SalePrice'">
                    <p>{{ record.F_SalePrice }}</p>
                  </template>
                  <template v-if="column.key === 'F_CostPrice'">
                    <p>{{ record.F_CostPrice }}</p>
                  </template>
                  <template v-if="column.key === 'F_CostAmount'">
                    <p>{{ record.F_CostAmount }}</p>
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
          <!-- 操作日志展示（水平行） -->
          <a-col :span="24" v-if="state.fullLogs && state.fullLogs.length && hasFormP('Detail_Log')">
            <a-form-item label="">
              <a-divider>操作日志</a-divider>
              <div style="display: flex; flex-direction: column; gap: 12px">
                <div
                  v-for="(log, idx) in pagedLogs"
                  :key="log.id"
                  style="display: flex; align-items: flex-start; gap: 16px; padding: 8px 0; border-bottom: 1px solid #f0f0f0">
                  <div style="flex: 0 0 180px; color: rgba(0, 0, 0, 0.85); font-size: 12px; font-weight: 500">
                    {{ log.F_CreatorTime }}
                  </div>
                  <div style="flex: 1; display: flex; gap: 12px; align-items: center">
                    <div>{{ log.F_Content }}</div>
                    <div v-if="log.F_Reason" style="color: rgba(0, 0, 0, 0.85)"><strong>修改内容：</strong>{{ log.F_Reason }}</div>
                  </div>
                  <div style="flex: 0 0 200px">
                    <strong>{{ log.F_CreatorUserId }}</strong>
                  </div>
                </div>
                <div style="display: flex; justify-content: flex-end; padding-top: 8px">
                  <a-pagination
                    :current="state.logsCurrentPage"
                    :pageSize="state.logsPageSize"
                    :total="state.fullLogs.length"
                    :showSizeChanger="false"
                    @change="
                      page => {
                        state.logsCurrentPage = page;
                      }
                    " />
                </div>
              </div>
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" v-show="false" v-if="hasFormP('F_CreatorUserId')">
            <a-form-item name="F_CreatorUserId" :labelCol="{ style: { width: '100px' } }">
              <template #label>创建人员</template>
              <p>{{ dataForm.F_CreatorUserId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" v-show="false" v-if="hasFormP('F_CreatorTime')">
            <a-form-item name="F_CreatorTime" :labelCol="{ style: { width: '100px' } }">
              <template #label>创建时间</template>
              <p>{{ dataForm.F_CreatorTime }}</p>
            </a-form-item>
          </a-col>
        </a-row>
      </a-form>
    </a-row>
  </BasicModal>
</template>
<script lang="ts" setup>
  import { getDetail, getLogs } from './helper/api';
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
    tableFieldd2a80douterActiveKey: number[];
    tableFieldd2a80dinnerActiveKey: string[];
    // 全部日志列表与分页状态
    fullLogs: any[];
    logsCurrentPage: number;
    logsPageSize: number;
  }

  defineOptions({ name: 'Detail' });
  const [registerModal, { openModal, closeModal, setModalProps }] = useModal();
  const { t } = useI18n();
  const tableFieldd2a80dColumns: any[] = computed(() => {
    let list = [
      {
        title: '商品名称',
        dataIndex: 'F_GoodsId',
        key: 'F_GoodsId',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_GoodsId',
        fixed: false,
      },
      {
        title: '编码',
        dataIndex: 'F_GoodsNo',
        key: 'F_GoodsNo',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_GoodsNo',
        fixed: false,
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
      },
      {
        title: '产品来源',
        dataIndex: 'F_Source',
        key: 'F_Source',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_Source',
        fixed: false,
      },
      {
        title: '投料工序',
        dataIndex: 'F_Process',
        key: 'F_Process',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_Process',
        fixed: false,
      },
      {
        title: '商品图片',
        dataIndex: 'F_Image',
        key: 'F_Image',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_Image',
        fixed: false,
      },
      {
        title: '单位用量',
        dataIndex: 'F_NumUnit',
        key: 'F_NumUnit',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_NumUnit',
        fixed: false,
      },
      {
        title: '单位',
        dataIndex: 'F_Unitsp',
        key: 'F_Unitsp',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_Unitsp',
        fixed: false,
      },
      {
        title: '销售单价',
        dataIndex: 'F_SalePrice',
        key: 'F_SalePrice',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_SalePrice',
        fixed: false,
      },
      {
        title: '成本单价',
        dataIndex: 'F_CostPrice',
        key: 'F_CostPrice',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_CostPrice',
        fixed: false,
      },
      {
        title: '成本金额（元）',
        dataIndex: 'F_CostAmount',
        key: 'F_CostAmount',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_CostAmount',
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

    list = list.filter(o => hasFormP('Detail_BOM-' + o.formP));
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
    let columns = [noColumn, ...columnList, actionColumn];
    const leftFixedList = columns.filter(o => o.fixed === 'left');
    const rightFixedList = columns.filter(o => o.fixed === 'right');
    const noFixedList = columns.filter(o => o.fixed !== 'left' && o.fixed !== 'right');
    return [...leftFixedList, ...noFixedList, ...rightFixedList];
  });

  const relationDetailRef = ref<any>(null);
  const state = reactive<State>({
    dataForm: {},
    interfaceRes: {
      F_GoodsId: [],
    },
    extraOptions: {
      F_GoodsId: [],
    },
    extraData: {
      F_GoodsId: {},
    },
    title: '详情',
    tableFieldd2a80douterActiveKey: [0],
    tableFieldd2a80dinnerActiveKey: [],
    fullLogs: [],
    logsCurrentPage: 1,
    logsPageSize: 5,
  });
  const { title, dataForm } = toRefs(state);
  // 分页计算当前页日志
  const pagedLogs = computed(() => {
    const list = state.fullLogs || [];
    const start = (state.logsCurrentPage - 1) * state.logsPageSize;
    return list.slice(start, start + state.logsPageSize);
  });
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

      for (let i = 0; i < state.dataForm.tableFieldd2a80d.length; i++) {
        const element = state.dataForm.tableFieldd2a80d[i];
        element.jnpfId = buildUUID();
        // normalize F_Image to parsed array for JnpfUploadImg component
        try {
          if (element.F_Image && typeof element.F_Image === 'string') {
            const s = element.F_Image;
            if ((s.startsWith('[') && s.endsWith(']')) || (s.startsWith('{') && s.endsWith('}'))) {
              element.F_Image = JSON.parse(s);
            }
          }
        } catch (e) {
          // ignore parse errors
        }
      }
      // compute display-only cost amount per row
      try {
        for (let i = 0; i < state.dataForm.tableFieldd2a80d.length; i++) {
          const row = state.dataForm.tableFieldd2a80d[i];
          row.F_CostAmount = (Number(row.F_Unit) || 0) * (Number(row.F_CostPrice) || 0);
        }
      } catch (e) {
        // ignore
      }
      // compute hierarchical display indices for rows (root -> children)
      try {
        computeDisplayIndices(state.dataForm.tableFieldd2a80d);
      } catch (e) {
        // ignore
      }
      nextTick(() => {
        changeLoading(false);
        getF_GoodsIdExtraInfo();
      });
      // 获取操作日志（保存完整列表用于前端分页）
      try {
        getLogs(id).then(logRes => {
          state.fullLogs = logRes.data || logRes || [];
          state.logsCurrentPage = 1;
        });
      } catch (e) {
        // ignore
      }
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

  function getF_GoodsIdExtraInfo() {
    if (!state.dataForm.F_GoodsId_id) return;
    const paramList = getParamList('F_GoodsId');
    const query = {
      ids: [state.dataForm.F_GoodsId_id],
      interfaceId: '2008721559174385664',
      propsValue: 'F_Id',
      relationField: 'F_GoodsName',
      paramList,
    };
    getDataInterfaceDataInfoByIds('2008721559174385664', query).then(res => {
      const data = res.data && res.data.length ? res.data[0] : {};
      state.extraData.F_GoodsId = data;
    });
  }

  // 计算表格行的层级序号（保留原始顺序）
  function computeDisplayIndices(rows: any[]) {
    try {
      if (!Array.isArray(rows)) return;
      const idToRow = new Map<string, any>();
      for (const r of rows) {
        if (r && r.F_Id != null) idToRow.set(String(r.F_Id), r);
      }
      const parentToChildren = new Map<string, any[]>();
      const roots: any[] = [];
      for (const r of rows) {
        const rawParent = r && (r.F_BomId === null || r.F_BomId === undefined || r.F_BomId === '' ? null : r.F_BomId);
        const parentId = rawParent != null ? String(rawParent) : null;
        if (parentId && idToRow.has(parentId)) {
          if (!parentToChildren.has(parentId)) parentToChildren.set(parentId, []);
          parentToChildren.get(parentId)!.push(r);
        } else {
          roots.push(r);
        }
      }
      function assign(prefix: string, nodes: any[]) {
        for (let i = 0; i < nodes.length; i++) {
          const node = nodes[i];
          const idx = i + 1;
          const label = prefix ? `${prefix}.${idx}` : `${idx}`;
          node.displayIndex = label;
          const children = parentToChildren.get(String(node.F_Id));
          if (children && children.length) assign(label, children);
        }
      }
      assign('', roots);
    } catch (e) {
      // ignore
    }
  }

  function getFirstImageUrl(record) {
    try {
      const imgField = record && record.F_Image;
      if (!imgField) return '';
      let arr: any = imgField;
      if (typeof imgField === 'string') {
        // try parse possibly double-encoded JSON
        try {
          arr = JSON.parse(imgField);
        } catch (_e) {
          // try unescape then parse
          try {
            arr = JSON.parse(imgField.replace(/\\+/g, ''));
          } catch (_e2) {
            arr = imgField;
          }
        }
      }
      if (Array.isArray(arr) && arr.length) {
        const first = arr[0];
        // prefer explicit url fields
        const candidate = first.url || first.fileUrl || first.fileId || first.fileIdValue || first.src || '';
        if (!candidate) return '';
        // if candidate looks like a fileId (no protocol and no slash), build download URL
        if (typeof candidate === 'string' && !candidate.startsWith('http') && candidate.indexOf('/') === -1) {
          return `${window.location.origin}/api/File/Download?fileId=${candidate}`;
        }
        if (typeof candidate === 'string' && candidate.startsWith('/')) return `${window.location.origin}${candidate}`;
        return candidate;
      }
      return '';
    } catch (e) {
      return '';
    }
  }
</script>
