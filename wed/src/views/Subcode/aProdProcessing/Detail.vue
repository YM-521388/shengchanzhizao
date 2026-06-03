<template>
  <BasicModal v-bind="$attrs" @register="registerModal" :title="title" width="1300px" :minHeight="100" :showOkBtn="false" :canFullscreen="true">
    <template #insertFooter> </template>
    <a-row class="dynamic-form">
      <a-form :colon="false" layout="horizontal" labelAlign="right" :labelCol="{ style: { width: '100px' } }" :model="dataForm" ref="formRef">
        <a-row :gutter="15">
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_ProcessNo')">
            <a-form-item name="F_ProcessNo" :labelCol="{ style: { width: '100px' } }">
              <template #label>加工单号：</template>
              <JnpfInput
                v-model:value="dataForm.F_ProcessNo"
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
          <!-- <a-col :span="6" class="ant-col-item">
            <a-form-item name="F_GoodsId" :labelCol="{ style: { width: '100px' } }">
              <template #label>商品：</template>
              <p class="leading-32px">{{ dataForm.F_GoodsId }}</p>
              <ExtraRelationInfo
                :extraOptions="state.extraOptions.F_GoodsId"
                :data="state.extraData.F_GoodsId"
                v-if="state.extraOptions.F_GoodsId?.length && state.extraData.F_GoodsId && JSON.stringify(state.extraData.F_GoodsId) !== '{}'" />
            </a-form-item>
          </a-col> -->
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_BOMId')">
            <a-form-item name="F_BOMId" :labelCol="{ style: { width: '100px' } }">
              <template #label>BOM：</template>
              <p>{{ dataForm.F_BOMId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_PlanQty')">
            <a-form-item name="F_PlanQty" :labelCol="{ style: { width: '100px' } }">
              <template #label>计划数：</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_PlanQty"
                placeholder="请输入"
                :controls="false"
                :style="{
                  width: '100%',
                }"
                disabled
                detailed />
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_DeliveryDate')">
            <a-form-item name="F_DeliveryDate" :labelCol="{ style: { width: '100px' } }">
              <template #label>交货日期：</template>
              <p>{{ dataForm.F_DeliveryDate }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_Priority')">
            <a-form-item name="F_Priority" :labelCol="{ style: { width: '100px' } }">
              <template #label>优先级：</template>
              <p>{{ dataForm.F_Priority }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_PlanStartDate')">
            <a-form-item name="F_PlanStartDate" :labelCol="{ style: { width: '100px' } }">
              <template #label>计划开始：</template>
              <p>{{ dataForm.F_PlanStartDate }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_PlanEndDate')">
            <a-form-item name="F_PlanEndDate" :labelCol="{ style: { width: '100px' } }">
              <template #label>计划结束：</template>
              <p>{{ dataForm.F_PlanEndDate }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_DrawingFile')">
            <a-form-item name="F_DrawingFile" :labelCol="{ style: { width: '100px' } }">
              <template #label>图纸：</template>
              <JnpfUploadFile
                v-model:value="dataForm.F_DrawingFile"
                buttonText="点击上传"
                pathType="selfPath"
                :isAccount="-1"
                folder=""
                :fileSize="10"
                sizeUnit="MB"
                :limit="9"
                disabled
                detailed />
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_CustomerName')">
            <a-form-item name="F_CustomerName" :labelCol="{ style: { width: '100px' } }">
              <template #label>客户：</template>
              <p>{{ dataForm.F_CustomerName }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_ContractId')">
            <a-form-item name="F_ContractId" :labelCol="{ style: { width: '100px' } }">
              <template #label>合同：</template>
              <p>{{ dataForm.F_ContractId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_DoorPlateThickness')">
            <a-form-item name="F_DoorPlateThickness" :labelCol="{ style: { width: '100px' } }">
              <template #label>门板厚度：</template>
              <p>{{ dataForm.F_DoorPlateThickness }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_BoxPlateThickness')">
            <a-form-item name="F_BoxPlateThickness" :labelCol="{ style: { width: '100px' } }">
              <template #label>箱体板厚：</template>
              <p>{{ dataForm.F_BoxPlateThickness }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_InstallOrBeam')">
            <a-form-item name="F_InstallOrBeam" :labelCol="{ style: { width: '120px' } }">
              <template #label>安装板或安装梁：</template>
              <p>{{ dataForm.F_InstallOrBeam }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_LockSet')">
            <a-form-item name="F_LockSet" :labelCol="{ style: { width: '100px' } }">
              <template #label>锁具：</template>
              <JnpfInput
                v-model:value="dataForm.F_LockSet"
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
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_Hinge')">
            <a-form-item name="F_Hinge" :labelCol="{ style: { width: '100px' } }">
              <template #label>铰链：</template>
              <JnpfInput
                v-model:value="dataForm.F_Hinge"
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
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_Color')">
            <a-form-item name="F_Color" :labelCol="{ style: { width: '100px' } }">
              <template #label>颜色：</template>
              <JnpfInput
                v-model:value="dataForm.F_Color"
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
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_Type')">
            <a-form-item name="F_Type" :labelCol="{ style: { width: '100px' } }">
              <template #label>类别：</template>
              <p>{{ dataForm.F_Type }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_BOMImages')">
            <a-form-item name="F_BOMImages" :labelCol="{ style: { width: '100px' } }">
              <template #label>BOM图片：</template>
              <JnpfUploadImg
                v-model:value="dataForm.F_BOMImages"
                pathType="selfPath"
                :isAccount="-1"
                folder=""
                sizeUnit="MB"
                :fileSize="10"
                disabled
                detailed />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_Description')">
            <a-form-item name="F_Description" :labelCol="{ style: { width: '100px' } }">
              <template #label>备注：</template>
              <p>{{ dataForm.F_Description }}</p>
            </a-form-item>
          </a-col>
          <!-- <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle :content="t('工序流程')" :bordered="true" />
              <BasicTable v-bind="getTableBindValue" :scroll="{ y: 400 }" @register="registerTable" @resizeColumn="handleResizeColumn">
                <template #bodyCell="{ column, record }">
                  <template v-if="column.key === 'action' && !record.top">
                    <TableAction :actions="getTableActions(record)" />
                  </template>
                </template>
              </BasicTable>
            </a-form-item>
          </a-col> -->

          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle :content="t('用料清单')" :bordered="true" />
              <BasicTable :scroll="{ y: 400 }" @register="registerGoodTable" @resizeColumn="handleResizeColumn">
                <template #bodyCell="{ column, record }"> </template>
              </BasicTable>
            </a-form-item>
          </a-col>

          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle :content="t('日志')" :bordered="true" />
              <BasicTable :scroll="{ y: 400 }" @register="registerLogTable" @resizeColumn="handleResizeColumn">
                <template #bodyCell="{ column, record }">
                  <!-- 只让“内容”列折行 -->
                  <template v-if="column.dataIndex === 'F_Content'">
                    <div style="white-space: normal; word-break: break-all">
                      {{ record.F_Content }}
                    </div>
                  </template></template
                >
              </BasicTable>
            </a-form-item>
          </a-col>
        </a-row>
      </a-form>
    </a-row>
  </BasicModal>
  <ItemRoutingDetail ref="detailRef" />
</template>
<script lang="ts" setup>
  import { getDetail, getBomItemList } from './helper/api';
  import ItemRoutingDetail from './ItemRoutingDetail.vue';
  import { reactive, toRefs, nextTick, ref, computed, unref, toRaw } from 'vue';
  import { buildUUID } from '@/utils/uuid';
  import { useI18n } from '@/hooks/web/useI18n';
  import { getDataInterfaceDataInfoByIds } from '@/api/systemData/dataInterface';
  import ExtraRelationInfo from '@/components/Jnpf/RelationForm/src/ExtraRelationInfo.vue';
  import { BasicModal, useModal } from '@/components/Modal';
  import { getTaskList } from '@/views/SubCode/aProdTask/helper/api';
  import { getLogList } from '@/views/SubCode/aProdProcessingLog/helper/api';
  import { BasicTable, useTable, TableAction, BasicColumn, FormProps, ActionItem } from '@/components/Table';
  import { usePermission } from '@/hooks/web/usePermission';
  let tableReload: Function = () => {};
  let tableReloadGood: Function = () => {};
  let tableReloadLog: Function = () => {};

  interface State {
    dataForm: any;
    interfaceRes: any;
    extraOptions: any;
    extraData: any;
    title: string;
    tableField0c720eouterActiveKey: number[];
    tableField0c720einnerActiveKey: string[];
    tableField449e6couterActiveKey: number[];
    tableField449e6cinnerActiveKey: string[];
  }

  defineOptions({ name: 'Detail' });
  const { hasFormP } = usePermission();
  const [registerModal, { openModal, closeModal, setModalProps }] = useModal();
  const { t } = useI18n();
  const detailRef = ref<any>(null);
  const state = reactive<State>({
    dataForm: {},
    interfaceRes: {
      F_GoodsId: [
        {
          defaultValue: '',
          field: 'contractId',
          dataType: 'varchar',
          required: 0,
          fieldName: '合同ID',
          relationField: '',
          jnpfKey: null,
          complexHeaderList: null,
          sourceType: 3,
          isChildren: false,
          IsMultiple: false,
        },
      ],
    },
    extraOptions: {
      F_GoodsId: [],
    },
    extraData: {
      F_GoodsId: {},
    },
    title: '详情',
    tableField0c720eouterActiveKey: [0],
    tableField0c720einnerActiveKey: [],
    tableField449e6couterActiveKey: [0],
    tableField449e6cinnerActiveKey: [],
  });
  const { title, dataForm } = toRefs(state);

  defineExpose({ init });

  function init(data) {
    state.dataForm.id = data.id;
    openModal();
    /* 手动刷新表格 */
    tableReload();
    tableReloadGood();
    tableReloadLog();
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

      nextTick(() => {
        changeLoading(false);
        getF_GoodsIdExtraInfo();
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

  function getF_GoodsIdExtraInfo() {
    if (!state.dataForm.F_GoodsId_id) return;
    const paramList = getParamList('F_GoodsId');
    const query = {
      ids: [state.dataForm.F_GoodsId_id],
      interfaceId: '2012085692393459712',
      propsValue: 'id',
      relationField: 'F_GoodsName',
      paramList,
    };
    getDataInterfaceDataInfoByIds('2012085692393459712', query).then(res => {
      const data = res.data && res.data.length ? res.data[0] : {};
      state.extraData.F_GoodsId = data;
    });
  }

  const columns: BasicColumn[] = [
    { title: '排序', dataIndex: 'F_SortCode', resizable: true, width: 100 },
    { title: '工序', dataIndex: 'F_ProcessId', resizable: true },
    { title: '计划开始', dataIndex: 'F_PlanStartDate', format: 'date|YYYY-MM-DD', resizable: true },
    { title: '计划结束', dataIndex: 'F_PlanEndDate', format: 'date|YYYY-MM-DD', resizable: true },
    { title: '备注', dataIndex: 'F_Description', resizable: true },
  ];

  const [registerTable, { reload }] = useTable({
    api: getTaskList,
    beforeFetch: data => {
      data.F_ProcessingId = state.dataForm.id;
      return data;
    },
    columns,
    useSearchForm: false,
    showTableSetting: false,
    showIndexColumn: false,
  });

  function getTableActions(record): ActionItem[] {
    return [
      {
        label: t('查看物料清单'),
        onClick: goDetail.bind(null, record),
      },
    ];
  }

  // 查看详情
  function goDetail(record) {
    // 不带流程
    const data = {
      id: record.id,
    };
    detailRef.value?.init(data);
  }

  const getTableBindValue = computed(() => {
    const defaultSortConfig = [];
    const sortField = defaultSortConfig.map(o => (o.sort === 'desc' ? '-' : '') + o.field);
    const data: any = {
      pagination: { pageSize: 20 }, //有分页
      ellipsis: true,
      defSort: { sidx: sortField.join(',') },
      bordered: true,
      actionColumn: {
        width: 110,
        title: t('component.table.action'),
        dataIndex: 'action',
      },
    };
    return data;
  });

  const allColumnsGood: BasicColumn[] = [
    { title: '排序', dataIndex: 'F_SortCode', resizable: true, width: 100 },
    { title: '商品名', dataIndex: 'F_GoodsNo', resizable: true },
    { title: '单位用量', dataIndex: 'F_NumUnit', resizable: true, width: 100 },
    { title: '单位', dataIndex: 'F_UnitTwo', resizable: true, width: 100 },
    // { title: '入库工序', dataIndex: 'F_StockInProcess', resizable: true },
    { title: '备注', dataIndex: 'F_Description', resizable: true },
  ];

  const columnsGood = computed(() =>
    allColumnsGood.filter(col => typeof col.dataIndex === 'string' && hasFormP(('tableField449e6c-' + col.dataIndex) as string)),
  );

  const [registerGoodTable, { reload: reloadGood }] = useTable({
    api: getBomItemList,
    beforeFetch: data => {
      data.F_ProcessingId = state.dataForm.id;
      return data;
    },
    columns: columnsGood,
    useSearchForm: false,
    showTableSetting: false,
    showIndexColumn: false,
  });

  const allColumnsLog: BasicColumn[] = [
    { title: '时间', dataIndex: 'F_CreatorTime', resizable: true, width: 150 },
    { title: '人员', dataIndex: 'F_CreatorUserId', resizable: true, width: 150 },
    { title: '标题', dataIndex: 'F_Title', resizable: true, width: 150 },
    { title: '内容', dataIndex: 'F_Content', resizable: true },
  ];

  const columnsLog = computed(() => allColumnsLog.filter(col => typeof col.dataIndex === 'string' && hasFormP(('allColumnsLog-' + col.dataIndex) as string)));
  const [registerLogTable, { reload: reloadLog }] = useTable({
    api: getLogList,
    beforeFetch: data => {
      data.F_ProcessingId = state.dataForm.id;
      return data;
    },
    columns: columnsLog,
    useSearchForm: false,
    showTableSetting: false,
    showIndexColumn: false,
  });

  tableReload = reload;
  tableReloadGood = reloadGood;
  tableReloadLog = reloadLog;

  function handleResizeColumn(w, col) {
    col.width = w;
  }
</script>
