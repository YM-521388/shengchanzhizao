<template>
  <BasicModal v-bind="$attrs" @register="registerModal" :title="title" width="60%" :minHeight="100" :showOkBtn="false" :canFullscreen="true">
    <template #insertFooter> </template>
    <a-row class="dynamic-form">
      <a-form :colon="false" layout="horizontal" labelAlign="right" :labelCol="{ style: { width: '100px' } }" :model="dataForm" ref="formRef">
        <a-row :gutter="15">
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_DeviceImages')">
            <a-form-item name="F_DeviceImages" :labelCol="{ style: { width: '100px' } }">
              <template #label>设备图片：</template>
              <JnpfUploadImg
                v-model:value="dataForm.F_DeviceImages"
                pathType="selfPath"
                :isAccount="-1"
                folder=""
                sizeUnit="MB"
                :fileSize="10"
                disabled
                detailed />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_DeviceNo')">
            <a-form-item name="F_DeviceNo" :labelCol="{ style: { width: '100px' } }">
              <template #label>设备编号：</template>
              <JnpfInput
                v-model:value="dataForm.F_DeviceNo"
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_DeviceName')">
            <a-form-item name="F_DeviceName" :labelCol="{ style: { width: '100px' } }">
              <template #label>设备名称：</template>
              <JnpfInput
                v-model:value="dataForm.F_DeviceName"
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_DeviceSpec')">
            <a-form-item name="F_DeviceSpec" :labelCol="{ style: { width: '100px' } }">
              <template #label>设备规格：</template>
              <JnpfInput
                v-model:value="dataForm.F_DeviceSpec"
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_DeviceStatus')">
            <a-form-item name="F_DeviceStatus" :labelCol="{ style: { width: '100px' } }">
              <template #label>设备状态：</template>
              <p>{{ dataForm.F_DeviceStatus }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_DeviceType')">
            <a-form-item name="F_DeviceType" :labelCol="{ style: { width: '100px' } }">
              <template #label>设备类别：</template>
              <p>{{ dataForm.F_DeviceType }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_GroupId')">
            <a-form-item name="F_GroupId" :labelCol="{ style: { width: '100px' } }">
              <template #label>分组：</template>
              <p>{{ dataForm.F_GroupId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_DeviceUsersId')">
            <a-form-item name="F_DeviceUsersId" :labelCol="{ style: { width: '100px' } }">
              <template #label>设备负责人：</template>
              <p>{{ dataForm.F_DeviceUsersId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_WorkshopId')">
            <a-form-item name="F_WorkshopId" :labelCol="{ style: { width: '100px' } }">
              <template #label>车间：</template>
              <p>{{ dataForm.F_WorkshopId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_LineId')">
            <a-form-item name="F_LineId" :labelCol="{ style: { width: '100px' } }">
              <template #label>产线：</template>
              <p>{{ dataForm.F_LineId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_DeviceQrCode')">
            <a-form-item name="F_DeviceQrCode" :labelCol="{ style: { width: '100px' } }">
              <template #label>设备二维码：</template>
              <JnpfInput
                v-model:value="dataForm.F_DeviceQrCode"
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_EnabledMark')">
            <a-form-item name="F_EnabledMark" :labelCol="{ style: { width: '100px' } }">
              <template #label>启用状态：</template>
              <p>{{ dataForm.F_EnabledMark }}</p>
            </a-form-item>
          </a-col>

          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle :content="t('物料')" :bordered="true" />
              <BasicTable :scroll="{ y: 400 }" @register="registerTable" @resizeColumn="handleResizeColumn">
                <template #bodyCell="{ column, record }"> </template>
              </BasicTable>
            </a-form-item>
          </a-col>
        </a-row>
      </a-form>
    </a-row>
  </BasicModal>
</template>
<script lang="ts" setup>
  import { getDetail, getGoodsList } from './helper/api';
  import { getConfigData } from '@/api/onlineDev/visualDev';
  import { reactive, toRefs, nextTick, ref, computed, unref, toRaw } from 'vue';
  import { thousandsFormat } from '@/utils/jnpf';
  import { CaretRightOutlined } from '@ant-design/icons-vue';
  import { buildUUID } from '@/utils/uuid';
  import { useI18n } from '@/hooks/web/useI18n';
  import { BasicModal, useModal } from '@/components/Modal';
  import { BasicTable, useTable, TableAction, BasicColumn, FormProps, ActionItem } from '@/components/Table';
  import { usePermission } from '@/hooks/web/usePermission';
  let tableReload: Function = () => {};

  interface State {
    dataForm: any;
    interfaceRes: any;
    extraOptions: any;
    extraData: any;
    title: string;
    tableField72c841outerActiveKey: number[];
    tableField72c841innerActiveKey: string[];
  }

  defineOptions({ name: 'Detail' });
  const { hasFormP } = usePermission();
  const [registerModal, { openModal, closeModal, setModalProps }] = useModal();
  const { t } = useI18n();

  const relationDetailRef = ref<any>(null);
  const state = reactive<State>({
    dataForm: {},
    interfaceRes: {},
    extraOptions: {},
    extraData: {},
    title: '详情',
    tableField72c841outerActiveKey: [0],
    tableField72c841innerActiveKey: [],
  });
  const { title, dataForm } = toRefs(state);

  defineExpose({ init });

  function init(data) {
    state.dataForm.id = data.id;
    openModal();
    /* 手动刷新表格 */
    tableReload();
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
      });
    });
  }
  function setFormProps(data) {
    setModalProps(data);
  }
  function changeLoading(loading) {
    setFormProps({ loading });
  }
  const allColumns: BasicColumn[] = [
    { title: '商品名', dataIndex: 'F_GoodsName', resizable: true, width: 150 },
    { title: '商品编号', dataIndex: 'F_GoodsNo', resizable: true, width: 150 },
    { title: '商品规格', dataIndex: 'F_Specification', resizable: true, width: 100 },
    { title: '数量', dataIndex: 'F_Num', resizable: true, width: 100 },
    { title: '库存数量', dataIndex: 'F_InventoryNum', resizable: true, width: 100 },
    { title: '单位', dataIndex: 'F_Unit', resizable: true, width: 100 },
    { title: '备注', dataIndex: 'F_Description', resizable: true },
  ];

  const columns = computed(() => allColumns.filter(col => typeof col.dataIndex === 'string' && hasFormP(('Detal_SBDA-' + col.dataIndex) as string)));
  const [registerTable, { reload }] = useTable({
    api: getGoodsList,
    beforeFetch: data => {
      data.F_DeviceId = state.dataForm.id;
      return data;
    },
    columns,
    useSearchForm: false,
    showTableSetting: false,
    showIndexColumn: false,
  });

  tableReload = reload;

  function handleResizeColumn(w, col) {
    col.width = w;
  }
</script>
