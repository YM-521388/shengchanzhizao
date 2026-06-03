<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    width="1400px"
    :minHeight="100"
    okText="确定"
    cancelText="取消"
    @ok="handleSubmit"
    :closeFunc="onClose"
    :canFullscreen="true">
    <template #title>
      <a-space :size="10">
        <div class="text-16px font-medium">{{ title }}</div>
      </a-space>
    </template>
    <template #insertToolbar>
      <div class="float-left mt-5px">
        <JnpfCheckboxSingle v-model:value="submitType" :label="continueText" />
      </div>
    </template>
    <a-row class="dynamic-form">
      <a-form :colon="false" layout="horizontal" labelAlign="right" :labelCol="{ style: { width: '100px' } }" :model="dataForm" :rules="dataRule" ref="formRef">
        <a-row :gutter="15">
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_ProjectNo')">
            <a-form-item name="F_ProjectNo" :labelCol="{ style: { width: '100px' } }">
              <template #label>项目编号</template>
              <JnpfInput
                v-model:value="dataForm.F_ProjectNo"
                placeholder="请填写，忽略将自动生成"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_ProjectName')">
            <a-form-item name="F_ProjectName" :labelCol="{ style: { width: '100px' } }">
              <template #label>项目名称</template>
              <JnpfInput
                v-model:value="dataForm.F_ProjectName"
                placeholder="请输入"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_ProjectType')">
            <a-form-item name="F_ProjectType" :labelCol="{ style: { width: '100px' } }">
              <template #label>项目类别</template>
              <JnpfSelect
                v-model:value="dataForm.F_ProjectType"
                placeholder="请选择"
                :options="optionsObj.F_ProjectTypeOptions"
                :fieldNames="optionsObj.F_ProjectTypeProps"
                @change="ProjectTypeBtn"
                allowClear
                showSearch
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_ContractId') && dataForm.F_ProjectType == '1'">
            <a-form-item name="F_ContractId" :labelCol="{ style: { width: '100px' } }">
              <template #label>合同</template>
              <JnpfSelect
                v-model:value="dataForm.F_ContractId"
                placeholder="请选择"
                :options="optionsObj.F_ContractIdOptions"
                :fieldNames="optionsObj.F_ContractIdProps"
                allowClear
                showSearch
                @change="ContractBtn"
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>

          <!-- <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_Status" :labelCol="{ style: { width: '100px' } }">
              <template #label>项目状态</template>
              <JnpfSelect
                v-model:value="dataForm.F_Status"
                placeholder="请选择"
                :options="optionsObj.F_StatusOptions"
                :fieldNames="optionsObj.F_StatusProps"
                allowClear
                showSearch
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col> -->

          <a-col :span="24" class="ant-col-item">
            <a-form-item name="GoodsList" :labelCol="{ style: { width: '100px' } }">
              <template #label>选择商品</template>
              <JnpfPopupMultipleSelect
                v-model:value="dataForm.GoodsList"
                placeholder="请选择"
                :formData="state.dataForm"
                :templateJson="optionsObj.GoodsListTemplateJson"
                allowClear
                field="GoodsList"
                interfaceId="2029803158963884032"
                :columnOptions="optionsObj.GoodsListOptions"
                relationField="F_GoodsName"
                propsValue="id"
                :pageSize="20"
                popupType="dialog"
                popupTitle="选择数据"
                popupWidth="1000px"
                @change="GoodsListBtn"
                hasPage
                :style="{
                  width: '100%',
                }"
                :extraOptions="optionsObj.GoodsList" />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="商品" :bordered="false" />
              <a-table
                :data-source="dataForm.tableField751ecb"
                :columns="tableField751ecbColumns"
                size="small"
                rowKey="jnpfId"
                :pagination="listPagination"
                :scroll="{ x: 'max-content' }"
                :rowSelection="gettableField751ecbRowSelection">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                  <template v-if="column.key === 'F_WorkOrderNo'">
                    <JnpfInput
                      v-model:value="record.F_WorkOrderNo"
                      placeholder="请填写，忽略将自动生成"
                      allowClear
                      :disabled="state.dataForm.id"
                      :style="{
                        width: '100%',
                      }"
                      :showCount="false" />
                  </template>
                  <template v-if="column.key === 'F_GoodsId'">
                    <JnpfSelect
                      v-model:value="record.F_GoodsId"
                      @change="F_GoodsIdTableChange(index)"
                      :options="optionsObj.tableField751ecb_F_GoodsIdOptions"
                      :fieldNames="optionsObj.tableField751ecb_F_GoodsIdProps"
                      allowClear
                      disabled
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_PlanNum'">
                    <JnpfInputNumber
                      v-model:value="record.F_PlanNum"
                      placeholder="请输入"
                      :disabled="state.dataForm.id"
                      :min="1"
                      :controls="false"
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_DeliveryDate'">
                    <JnpfDatePicker
                      v-model:value="record.F_DeliveryDate"
                      placeholder="请选择"
                      format="yyyy-MM-dd"
                      allowClear
                      :disabled="state.dataForm.id"
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_Priority'">
                    <JnpfSelect
                      v-model:value="record.F_Priority"
                      placeholder="请选择"
                      :options="optionsObj.tableField751ecb_F_PriorityOptions"
                      :fieldNames="optionsObj.tableField751ecb_F_PriorityProps"
                      allowClear
                      showSearch
                      :disabled="state.dataForm.id"
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_BomId'">
                    <JnpfSelect
                      v-model:value="record.F_BomId"
                      placeholder="请选择"
                      :options="record.tableField751ecb_F_BomIdOptions"
                      :fieldNames="optionsObj.tableField751ecb_F_BomIdProps"
                      :disabled="state.dataForm.id"
                      allowClear
                      showSearch
                      @change="F_BomIdTableChange(record)"
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_DrawingFiles'">
                    <JnpfUploadFile
                      v-model:value="record.F_DrawingFiles"
                      buttonText="点击上传"
                      pathType="selfPath"
                      :disabled="state.dataForm.id"
                      :isAccount="-1"
                      folder=""
                      :fileSize="10"
                      sizeUnit="MB"
                      :limit="9"
                      :sortRule="['1', '2']"
                      timeFormat="YYYYMMDD" />
                  </template>
                  <template v-if="column.key === 'F_DoorPlateThickness'">
                    <JnpfSelect
                      v-model:value="record.F_DoorPlateThickness"
                      placeholder="请选择"
                      :options="optionsObj.tableField751ecb_F_DoorPlateThicknessOptions"
                      :fieldNames="optionsObj.tableField751ecb_F_DoorPlateThicknessProps"
                      allowClear
                      showSearch
                      :disabled="state.dataForm.id"
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_BoxPlateThickness'">
                    <JnpfSelect
                      v-model:value="record.F_BoxPlateThickness"
                      placeholder="请选择"
                      :options="optionsObj.tableField751ecb_F_BoxPlateThicknessOptions"
                      :fieldNames="optionsObj.tableField751ecb_F_BoxPlateThicknessProps"
                      allowClear
                      showSearch
                      :disabled="state.dataForm.id"
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_InstallPlateOrBeam'">
                    <JnpfSelect
                      v-model:value="record.F_InstallPlateOrBeam"
                      placeholder="请选择"
                      :options="optionsObj.tableField751ecb_F_InstallPlateOrBeamOptions"
                      :fieldNames="optionsObj.tableField751ecb_F_InstallPlateOrBeamProps"
                      allowClear
                      showSearch
                      :disabled="state.dataForm.id"
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_CustomerName'">
                    <JnpfSelect
                      v-model:value="record.F_CustomerName"
                      placeholder="请选择"
                      :options="optionsObj.tableField751ecb_F_CustomerNameOptions"
                      :fieldNames="optionsObj.tableField751ecb_F_CustomerNameProps"
                      allowClear
                      showSearch
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_LockSet'">
                    <JnpfInput
                      v-model:value="record.F_LockSet"
                      placeholder="请输入"
                      allowClear
                      :disabled="state.dataForm.id"
                      :style="{
                        width: '100%',
                      }"
                      :showCount="false" />
                  </template>
                  <template v-if="column.key === 'F_Hinge'">
                    <JnpfInput
                      v-model:value="record.F_Hinge"
                      placeholder="请输入"
                      allowClear
                      :disabled="state.dataForm.id"
                      :style="{
                        width: '100%',
                      }"
                      :showCount="false" />
                  </template>
                  <template v-if="column.key === 'F_BomImages'">
                    <JnpfUploadImg
                      v-model:value="record.F_BomImages"
                      pathType="selfPath"
                      :isAccount="-1"
                      folder=""
                      sizeUnit="MB"
                      :fileSize="10"
                      :sortRule="['1', '2']"
                      :disabled="state.dataForm.id"
                      timeFormat="YYYYMMDD"
                      showType="button"
                      buttonText="点击上传" />
                  </template>
                  <template v-if="column.key === 'F_Color'">
                    <JnpfInput
                      v-model:value="record.F_Color"
                      placeholder="请输入"
                      allowClear
                      :disabled="state.dataForm.id"
                      :style="{
                        width: '100%',
                      }"
                      :showCount="false" />
                  </template>
                  <template v-if="column.key === 'F_Category'">
                    <JnpfCascader
                      v-model:value="record.F_Category"
                      placeholder="请选择"
                      :options="optionsObj.tableField751ecb_F_CategoryOptions"
                      :fieldNames="optionsObj.tableField751ecb_F_CategoryProps"
                      :disabled="state.dataForm.id"
                      allowClear
                      showSearch
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_RoutingId'">
                    <JnpfSelect
                      v-model:value="record.F_RoutingId"
                      placeholder="请选择"
                      :options="optionsObj.tableField751ecb_F_RoutingIdOptions"
                      :fieldNames="optionsObj.tableField751ecb_F_RoutingIdProps"
                      allowClear
                      showSearch
                      :disabled="state.dataForm.id"
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_ParentId'">
                    <JnpfInput
                      v-model:value="record.F_ParentId"
                      placeholder="请输入"
                      allowClear
                      :disabled="state.dataForm.id"
                      :style="{
                        width: '100%',
                      }"
                      :showCount="false" />
                  </template>
                  <template v-if="column.key === 'F_CreatorTime'">
                    <JnpfOpenData
                      v-model:value="record.F_CreatorTime"
                      type="currTime"
                      readonly
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'action'">
                    <a-space>
                      <!-- <a-button class="action-btn" type="link" @click="copytableField751ecbRow(index)" size="small">{{
                        t('common.copyText', '复制')
                      }}</a-button> -->
                      <!-- <a-button class="action-btn" type="link" @click="routingHandle(record)" size="small">{{ t('工序流程') }}</a-button> -->
                      <a-button class="action-btn" type="link" @click="goodsHandle(record)" size="small">{{ t('用料清单') }}</a-button>
                      <a-button
                        class="action-btn"
                        type="link"
                        color="error"
                        v-if="!state.dataForm.id"
                        @click="removetableField751ecbRow(index, true)"
                        size="small"
                        >{{ t('common.delText', '删除') }}</a-button
                      >
                    </a-space>
                  </template>
                </template>
              </a-table>
              <a-space class="input-table-footer-btn">
                <!-- <a-button type="primary" preIcon="icon-ym icon-ym-btn-add" @click="addtableField751ecbRow">{{ t('common.add1Text', '添加') }}</a-button>
                <a-button type="danger" preIcon="icon-ym icon-ym-btn-clearn" @click="batchRemovetableField751ecbRow(true)">{{
                  t('common.batchDelText', '批量删除')
                }}</a-button> -->
              </a-space>
            </a-form-item>
          </a-col>
        </a-row>
      </a-form>
    </a-row>
  </BasicModal>
  <SelectModal :config="state.currTableConf" :formData="state.dataForm" ref="selectModal" @select="addForSelect" />
  <RoutingForm ref="routingFormRef" @routingSubmit="onProcessSubmit" />
  <GoodsForm ref="goodsFormRef" @goodsSubmit="onGoodsSubmit" />
</template>
<script lang="ts" setup>
  import { create, update, getInfo, createTwo } from './helper/api';
  import { getBOMGoodsList } from '@/views/Subcode/aBom/helper/api';
  import { reactive, toRefs, nextTick, ref, unref, computed, toRaw, inject } from 'vue';
  import { BasicModal, useModal } from '@/components/Modal';
  import SelectModal from '@/components/CommonModal/src/SelectModal.vue';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useI18n } from '@/hooks/web/useI18n';
  import { useUserStore } from '@/store/modules/user';
  import type { FormInstance } from 'ant-design-vue';
  import { getDictionaryDataSelector } from '@/api/systemData/dictionary';
  import { getDataInterfaceRes } from '@/api/systemData/dataInterface';
  import dayjs from 'dayjs';
  import { cloneDeep } from 'lodash-es';
  import { buildUUID } from '@/utils/uuid';
  import { usePermission } from '@/hooks/web/usePermission';
  import RoutingForm from '@/views/Subcode/aProdProject/RoutingForm.vue';
  import GoodsForm from '@/views/Subcode/aProdProject/GoodsForm.vue';

  interface State {
    // GoodsList: any[];listPageSize: number;
    listCurrentPage: number;
    customerId: string;
    dataForm: any;
    dataRule: any;
    optionsObj: any;
    currVmodel: any;
    currTableConf: any;
    addTableConf: any;
    tableRows: any;
    tableField751ecbouterActiveKey: number[];
    tableField751ecbinnerActiveKey: string[];
    tableField751ecbDefaultExpandAll: boolean;
    selectedtableField751ecbRowKeys: any[];
    title: string;
    continueText: string;
    allList: any[];
    currIndex: number;
    isContinue: boolean;
    submitType: number;
    showContinueBtn: boolean;
  }

  const emit = defineEmits(['reload']);
  const { hasFormP } = usePermission();
  const getLeftTreeActiveInfo: (() => any) | null = inject('getLeftTreeActiveInfo', null);
  const userStore = useUserStore();
  const userInfo = userStore.getUserInfo;
  const { createMessage, createConfirm } = useMessage();
  const { t } = useI18n();
  const [registerModal, { openModal, setModalProps }] = useModal();
  const formRef = ref<FormInstance>();
  const routingFormRef = ref<any>(null);
  const goodsFormRef = ref<any>(null);
  const selectModal = ref(null);
  const tableField751ecbColumns: any[] = computed(() => {
    let list = [
      {
        title: '工单编号',
        dataIndex: 'F_WorkOrderNo',
        key: 'F_WorkOrderNo',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_WorkOrderNo',
        isSystemControl: false,
      },
      {
        title: '商品',
        dataIndex: 'F_GoodsId',
        key: 'F_GoodsId',
        tipLabel: '',
        required: true,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_GoodsId',
        isSystemControl: false,
      },
      {
        title: '规格',
        dataIndex: 'F_Specification',
        key: 'F_Specification',
        tipLabel: '',
        required: true,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Specification',
        isSystemControl: false,
      },
      {
        title: '计划数',
        dataIndex: 'F_PlanNum',
        key: 'F_PlanNum',
        tipLabel: '',
        required: true,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '80px',
        fixed: false,
        formP: 'F_PlanNum',
        isSystemControl: false,
      },
      {
        title: '类别',
        dataIndex: 'F_Category',
        key: 'F_Category',
        tipLabel: '',
        required: true,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Category',
        isSystemControl: false,
      },
      {
        title: '工艺路线',
        dataIndex: 'F_RoutingId',
        key: 'F_RoutingId',
        tipLabel: '',
        required: true,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '260px',
        fixed: false,
        formP: 'F_RoutingId',
        isSystemControl: false,
      },
      {
        title: '交货日期',
        dataIndex: 'F_DeliveryDate',
        key: 'F_DeliveryDate',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_DeliveryDate',
        isSystemControl: false,
      },
      {
        title: '优先级',
        dataIndex: 'F_Priority',
        key: 'F_Priority',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Priority',
        isSystemControl: false,
      },
      {
        title: 'BOM',
        dataIndex: 'F_BomId',
        key: 'F_BomId',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '260px',
        fixed: false,
        formP: 'F_BomId',
        isSystemControl: false,
      },
      {
        title: '图纸',
        dataIndex: 'F_DrawingFiles',
        key: 'F_DrawingFiles',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_DrawingFiles',
        isSystemControl: false,
      },
      {
        title: '门板厚度',
        dataIndex: 'F_DoorPlateThickness',
        key: 'F_DoorPlateThickness',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_DoorPlateThickness',
        isSystemControl: false,
      },
      {
        title: '箱体板厚',
        dataIndex: 'F_BoxPlateThickness',
        key: 'F_BoxPlateThickness',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_BoxPlateThickness',
        isSystemControl: false,
      },
      {
        title: '安装板或安装梁',
        dataIndex: 'F_InstallPlateOrBeam',
        key: 'F_InstallPlateOrBeam',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_InstallPlateOrBeam',
        isSystemControl: false,
      },
      {
        title: '客户',
        dataIndex: 'F_CustomerName',
        key: 'F_CustomerName',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '150px',
        fixed: false,
        formP: 'F_CustomerName',
        isSystemControl: false,
      },
      {
        title: '锁具',
        dataIndex: 'F_LockSet',
        key: 'F_LockSet',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_LockSet',
        isSystemControl: false,
      },
      {
        title: '铰链',
        dataIndex: 'F_Hinge',
        key: 'F_Hinge',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Hinge',
        isSystemControl: false,
      },
      {
        title: 'BOM图片',
        dataIndex: 'F_BomImages',
        key: 'F_BomImages',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_BomImages',
        isSystemControl: false,
      },
      {
        title: '颜色',
        dataIndex: 'F_Color',
        key: 'F_Color',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Color',
        isSystemControl: false,
      },
    ];
    list = list.filter(o => hasFormP('tableField751ecb-' + o.formP));
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
    const actionColumn = { title: t('component.table.action'), dataIndex: 'action', key: 'action', align: 'center', width: 100, fixed: 'right' };
    let columns = [noColumn, ...columnList, actionColumn];
    const leftFixedList = columns.filter(o => o.fixed === 'left');
    const rightFixedList = columns.filter(o => o.fixed === 'right');
    const noFixedList = columns.filter(o => o.fixed !== 'left' && o.fixed !== 'right');
    return [...leftFixedList, ...noFixedList, ...rightFixedList];
  });
  const gettableField751ecbRowSelection = computed(() => {
    const rowSelection = {
      selectedRowKeys: state.selectedtableField751ecbRowKeys,
      onChange: (selectedRowKeys: string[]) => {
        state.selectedtableField751ecbRowKeys = (selectedRowKeys || []).sort().reverse();
      },
    };
    return rowSelection;
  });
  const state = reactive<State>({
    listPageSize: 20,
    listCurrentPage: 1,
    customerId: '',
    dataForm: {
      GoodsList: [],
      id: '',
      F_ProjectNo: undefined,
      F_ProjectName: undefined,
      F_ContractId: undefined,
      F_ProjectType: '0',
      F_Status: undefined,
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
      F_UpdateUserId: undefined,
      F_UpdateTime: undefined,
      tableField751ecb: [],
    },
    tableField751ecbouterActiveKey: [0],
    tableField751ecbinnerActiveKey: [],
    tableField751ecbDefaultExpandAll: true,
    selectedtableField751ecbRowKeys: [],
    dataRule: {
      F_ProjectName: [
        {
          required: true,
          message: '请输入项目名称',
          trigger: 'blur',
        },
      ],
      F_ContractId: [
        {
          required: true,
          message: '请输入合同',
          trigger: 'change',
        },
      ],
    },
    optionsObj: {
      tableField751ecb_F_CustomerNameOptions: [],
      tableField751ecb_F_CustomerNameProps: { label: 'F_CustomerName', value: 'F_Id' },
      tableField751ecb_F_RoutingIdOptions: [],
      tableField751ecb_F_RoutingIdProps: { label: 'F_RoutingName', value: 'id' },
      GoodsListOptions: [
        {
          value: 'F_GoodsName',
          label: '商品名',
        },
        {
          value: 'F_GoodsNo',
          label: '商品编号',
        },
        {
          value: 'F_Specification',
          label: '商品规格',
        },
        {
          value: 'F_Type',
          label: '商品类型',
        },
        // {
        //   value: 'F_CategoryId',
        //   label: '商品类别',
        // },
        // {
        //   value: 'F_Unit',
        //   label: '单位',
        // },
        // {
        //   value: 'F_InspectRule',
        //   label: '检验规范',
        // },
        {
          value: 'F_Remark',
          label: '备注',
        },
      ],
      GoodsListTemplateJson: [
        {
          defaultValue: '',
          field: 'contractId',
          dataType: 'varchar',
          required: 0,
          fieldName: '合同',
          relationField: 'F_ContractId',
          jnpfKey: 'select',
          complexHeaderList: null,
          sourceType: 1,
          isChildren: false,
          IsMultiple: false,
        },
      ],
      GoodsList: [],
      F_ContractIdProps: { label: 'F_ContractCode', value: 'F_Id' },
      F_ProjectTypeProps: { label: 'fullName', value: 'enCode' },
      F_StatusProps: { label: 'fullName', value: 'enCode' },
      tableField751ecb_F_GoodsIdOptions: [],
      tableField751ecb_F_GoodsIdProps: { label: 'F_GoodsName', value: 'id' },
      tableField751ecb_F_PriorityOptions: [],
      tableField751ecb_F_PriorityProps: { label: 'fullName', value: 'enCode' },
      tableField751ecb_F_BomIdProps: { label: 'F_BomName', value: 'id' },
      tableField751ecb_F_DoorPlateThicknessOptions: [],
      tableField751ecb_F_DoorPlateThicknessProps: { label: 'fullName', value: 'enCode' },
      tableField751ecb_F_BoxPlateThicknessOptions: [],
      tableField751ecb_F_BoxPlateThicknessProps: { label: 'fullName', value: 'enCode' },
      tableField751ecb_F_InstallPlateOrBeamOptions: [],
      tableField751ecb_F_InstallPlateOrBeamProps: { label: 'fullName', value: 'enCode' },
      tableField751ecb_F_CategoryOptions: [],
      tableField751ecb_F_CategoryProps: {
        label: 'F_Name',
        value: 'F_Id',
        children: 'children',
      },
    },
    currVmodel: '',
    currTableConf: {},
    tableRows: {
      tableField751ecb: {
        enabledmark: undefined,
        F_WorkOrderNo: undefined,
        F_GoodsId: undefined,
        F_PlanNum: 1,
        F_DeliveryDate: undefined,
        F_Priority: undefined,
        F_BomId: undefined,
        tableField751ecb_F_BomIdOptions: [],
        F_DrawingFiles: [],
        F_DoorPlateThickness: undefined,
        F_BoxPlateThickness: undefined,
        F_InstallPlateOrBeam: undefined,
        F_CustomerName: undefined,
        F_LockSet: undefined,
        F_Hinge: undefined,
        F_BomImages: [],
        F_Color: undefined,
        F_Category: [],
        F_RoutingId: undefined,
        F_ParentId: undefined,
        F_CreatorTime: undefined,
        Process: [],
        ProjectGoods: [],
      },
    },
    addTableConf: {},
    title: '',
    continueText: '',
    allList: [],
    currIndex: 0,
    isContinue: false,
    submitType: 0,
    showContinueBtn: true,
  });
  const { title, continueText, showContinueBtn, dataForm, submitType, dataRule, optionsObj } = toRefs(state);

  defineExpose({ init });

  function init(data) {
    state.submitType = 0;
    state.showContinueBtn = true;
    state.title = !data.id ? '新增' : '编辑';
    setFormProps({ continueLoading: false });
    state.dataForm.id = data.id;
    openModal();
    nextTick(() => {
      getForm().resetFields();
      state.dataForm.tableField751ecb = [];
      setTimeout(initData, 0);
    });
  }
  function initData() {
    changeLoading(true);
    if (state.dataForm.id) {
      getData(state.dataForm.id);
    } else {
      // 设置默认值
      state.dataForm = {
        F_ProjectNo: undefined,
        F_ProjectName: undefined,
        F_ContractId: undefined,
        F_ProjectType: '0',
        F_Status: undefined,
        F_CreatorUserId: undefined,
        F_CreatorTime: undefined,
        F_UpdateUserId: undefined,
        F_UpdateTime: undefined,
        tableField751ecb: [],
      };
      state.dataForm.GoodsList = [];
      gettableField751ecb_F_CustomerNameOptions();
      getF_ProjectTypeOptions();
      getF_StatusOptions();
      gettableField751ecb_F_GoodsIdOptions();
      gettableField751ecb_F_PriorityOptions();
      gettableField751ecb_F_DoorPlateThicknessOptions();
      gettableField751ecb_F_BoxPlateThicknessOptions();
      gettableField751ecb_F_InstallPlateOrBeamOptions();
      gettableField751ecb_F_CategoryOptions();
      gettableField751ecb_F_RoutingIdOptions();
      getF_ContractIdOptions();
      if (getLeftTreeActiveInfo) state.dataForm = { ...state.dataForm, ...(getLeftTreeActiveInfo() || {}) };
      changeLoading(false);
    }
  }

  // 工序流程
  function routingHandle(record) {
    // 不带工作流
    const data = {
      ...record,
      jnpfId: record.jnpfId,
    };
    routingFormRef.value?.init(data);
  }

  // 用料清单
  function goodsHandle(record) {
    // 不带工作流
    const data = {
      ...record,
      jnpfId: record.jnpfId,
    };
    goodsFormRef.value?.init(data);
  }

  // 监听 submit 事件
  function onProcessSubmit(returnRow) {
    // returnRow 就是 StepForm 里 emit 出来的整行数据
    const idx = state.dataForm.tableField751ecb.findIndex(item => item.jnpfId === returnRow.jnpfId);
    if (idx > -1) {
      // 整行替换，保持响应式
      state.dataForm.tableField751ecb[idx].Process = returnRow.Process;
    }
  }
  // 监听 submit 事件
  function onGoodsSubmit(returnRow) {
    // returnRow 就是 StepForm 里 emit 出来的整行数据
    const idx = state.dataForm.tableField751ecb.findIndex(item => item.jnpfId === returnRow.jnpfId);
    if (idx > -1) {
      // 整行替换，保持响应式
      state.dataForm.tableField751ecb[idx].ProjectGoods = returnRow.ProjectGoods;
    }
  }

  function getForm() {
    const form = unref(formRef);
    if (!form) {
      throw new Error('form is null!');
    }
    return form;
  }
  function getData(id) {
    getInfo(id).then(res => {
      state.dataForm = res.data || {};

      state.dataForm.GoodsList = [];
      for (let i = 0; i < state.dataForm.tableField751ecb.length; i++) {
        const element = state.dataForm.tableField751ecb[i];
        element.jnpfId = buildUUID();
        state.dataForm.GoodsList.push(state.dataForm.tableField751ecb[i].F_GoodsId);
        gettableField751ecb_F_BomIdOptions(i);
      }
      gettableField751ecb_F_CustomerNameOptions();
      getF_ProjectTypeOptions();
      getF_StatusOptions();
      gettableField751ecb_F_GoodsIdOptions();
      gettableField751ecb_F_PriorityOptions();
      gettableField751ecb_F_DoorPlateThicknessOptions();
      gettableField751ecb_F_BoxPlateThicknessOptions();
      gettableField751ecb_F_InstallPlateOrBeamOptions();
      gettableField751ecb_F_CategoryOptions();
      gettableField751ecb_F_RoutingIdOptions();
      getF_ContractIdOptions();
      changeLoading(false);
    });
  }
  async function handleSubmit() {
    try {
      const values = await getForm()?.validate();
      if (!values) return;
      if (!tableField751ecbExist()) return;
      setFormProps({ confirmLoading: true });
      var formMethod = create;
      if (state.dataForm.F_ProjectType == '2') {
        formMethod = state.dataForm.id ? update : create;
      } else {
        formMethod = state.dataForm.id ? update : createTwo;
      }
      formMethod(state.dataForm)
        .then(res => {
          createMessage.success(res.msg);
          setFormProps({ confirmLoading: false });
          if (state.submitType == 1) {
            initData();
            state.isContinue = true;
          } else {
            setFormProps({ open: false });
            emit('reload');
          }
        })
        .catch(() => {
          setFormProps({ confirmLoading: false });
        });
    } catch (_) {}
  }
  function handlePrev() {
    state.currIndex--;
    handleGetNewInfo();
  }
  function handleNext() {
    state.currIndex++;
    handleGetNewInfo();
  }
  function handleGetNewInfo() {
    changeLoading(true);
    getForm().resetFields();
    const id = state.allList[state.currIndex].id;
    getData(id);
  }
  function gettableField751ecb_F_CustomerNameOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2009458830353764352', query).then(res => {
      let data = res.data;
      state.optionsObj.tableField751ecb_F_CustomerNameOptions = Array.isArray(data) ? data : [];
    });
  }
  function F_GoodsIdTableChange(i) {
    state.dataForm.tableField751ecb[i].F_BomId = undefined;
    gettableField751ecb_F_BomIdOptions(i);
  }
  function setFormProps(data) {
    setModalProps(data);
  }
  function changeLoading(loading) {
    setModalProps({ loading });
  }
  async function onClose() {
    if (state.isContinue) emit('reload');
    return true;
  }
  function getF_ContractIdOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2010889611072638976', query).then(res => {
      let data = res.data;
      state.optionsObj.F_ContractIdOptions = Array.isArray(data) ? data : [];
    });
  }
  function getF_ProjectTypeOptions() {
    getDictionaryDataSelector('2013172950349516800').then(res => {
      state.optionsObj.F_ProjectTypeOptions = res.data.list;
    });
  }
  function getF_StatusOptions() {
    getDictionaryDataSelector('2013173013452820480').then(res => {
      state.optionsObj.F_StatusOptions = res.data.list;
    });
  }
  function gettableField751ecb_F_GoodsIdOptions() {
    let templateJson = [
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
    ];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2029803158963884032', query).then(res => {
      let data = res.data;
      state.optionsObj.tableField751ecb_F_GoodsIdOptions = Array.isArray(data) ? data : [];
    });
  }
  function gettableField751ecb_F_PriorityOptions() {
    getDictionaryDataSelector('2013182853290004480').then(res => {
      state.optionsObj.tableField751ecb_F_PriorityOptions = res.data.list;
    });
  }
  function gettableField751ecb_F_BomIdOptions(i?) {
    let templateJson = [
      {
        defaultValue: '',
        field: 'goodsId',
        dataType: 'varchar',
        required: 0,
        fieldName: '合同ID',
        relationField: 'tableField751ecb-F_GoodsId',
        jnpfKey: 'select',
        complexHeaderList: null,
        sourceType: 1,
        isChildren: false,
        IsMultiple: false,
      },
    ];
    let query = {
      paramList: getParamList(templateJson, state.dataForm, i),
    };
    getDataInterfaceRes('2013188646957617152', query).then(res => {
      let data = res.data;
      state.dataForm.tableField751ecb[i].tableField751ecb_F_BomIdOptions = Array.isArray(data) ? data : [];
    });
  }
  function gettableField751ecb_F_RoutingIdOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2013852230570086400', query).then(res => {
      let data = res.data;
      state.optionsObj.tableField751ecb_F_RoutingIdOptions = Array.isArray(data) ? data : [];
    });
  }
  function gettableField751ecb_F_DoorPlateThicknessOptions() {
    getDictionaryDataSelector('2013183327061807104').then(res => {
      state.optionsObj.tableField751ecb_F_DoorPlateThicknessOptions = res.data.list;
    });
  }
  function gettableField751ecb_F_BoxPlateThicknessOptions() {
    getDictionaryDataSelector('2013183327061807104').then(res => {
      state.optionsObj.tableField751ecb_F_BoxPlateThicknessOptions = res.data.list;
    });
  }
  function gettableField751ecb_F_InstallPlateOrBeamOptions() {
    getDictionaryDataSelector('2013183327061807104').then(res => {
      state.optionsObj.tableField751ecb_F_InstallPlateOrBeamOptions = res.data.list;
    });
  }
  function gettableField751ecb_F_CategoryOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2008414575283802112', query).then(res => {
      let data = res.data;
      state.optionsObj.tableField751ecb_F_CategoryOptions = Array.isArray(data) ? data : [];
    });
  }
  function removetableField751ecbRow(index, showConfirm = false) {
    if (showConfirm) {
      createConfirm({
        iconType: 'warning',
        title: '提示',
        content: '此操作将永久删除该数据, 是否继续?',
        onOk: () => {
          state.dataForm.tableField751ecb.splice(index, 1);
          state.dataForm.GoodsList.splice(index, 1);
        },
      });
    } else {
      state.dataForm.tableField751ecb.splice(index, 1);
      state.dataForm.GoodsList.splice(index, 1);
    }
  }
  function tableField751ecbExist() {
    let isOk = true;
    for (let i = 0; i < state.dataForm.tableField751ecb.length; i++) {
      const e = state.dataForm.tableField751ecb[i];
      if (!e.F_GoodsId) {
        createMessage.error('商品' + t('sys.validate.textRequiredSuffix'));
        isOk = false;
        break;
      }
      if (!e.F_PlanNum) {
        createMessage.error('计划数' + t('sys.validate.textRequiredSuffix'));
        isOk = false;
        break;
      }
      // if (!e.F_DoorPlateThickness) {
      //   createMessage.error('门板厚度' + t('sys.validate.textRequiredSuffix'));
      //   isOk = false;
      //   break;
      // }
      // if (!e.F_BoxPlateThickness) {
      //   createMessage.error('箱体板厚' + t('sys.validate.textRequiredSuffix'));
      //   isOk = false;
      //   break;
      // }
      // if (!e.F_InstallPlateOrBeam) {
      //   createMessage.error('安装板或安装梁' + t('sys.validate.textRequiredSuffix'));
      //   isOk = false;
      //   break;
      // }
      // if (!e.F_CustomerName) {
      //   createMessage.error('客户' + t('sys.validate.textRequiredSuffix'));
      //   isOk = false;
      //   break;
      // }
      // if (!e.F_LockSet) {
      //   createMessage.error('锁具' + t('sys.validate.textRequiredSuffix'));
      //   isOk = false;
      //   break;
      // }
      // if (!e.F_Hinge) {
      //   createMessage.error('铰链' + t('sys.validate.textRequiredSuffix'));
      //   isOk = false;
      //   break;
      // }
      // if (!e.F_Color) {
      //   createMessage.error('颜色' + t('sys.validate.textRequiredSuffix'));
      //   isOk = false;
      //   break;
      // }
      if (!e.F_Category.length) {
        createMessage.error('类别' + t('sys.validate.textRequiredSuffix'));
        isOk = false;
        break;
      }
      if (!e.F_RoutingId) {
        createMessage.error('工艺路线' + t('sys.validate.textRequiredSuffix'));
        isOk = false;
        break;
      }
    }
    return isOk;
  }
  function addForSelect(data) {
    for (let i = 0; i < data.length; i++) {
      let item = { ...state.tableRows[state.currVmodel], ...data[i], jnpfId: buildUUID() };
      state.dataForm[state.currVmodel].push(cloneDeep(item));
      if (state[state.currVmodel + 'DefaultExpandAll']) state[state.currVmodel + 'innerActiveKey'].push(item.jnpfId);
    }
  }
  function getParamList(templateJson, formData, index?) {
    for (let i = 0; i < templateJson.length; i++) {
      if (templateJson[i].relationField && templateJson[i].sourceType == 1) {
        //区分是否子表
        if (templateJson[i].relationField.includes('-')) {
          let tableVModel = templateJson[i].relationField.split('-')[0];
          let childVModel = templateJson[i].relationField.split('-')[1];
          templateJson[i].defaultValue = (formData[tableVModel] && formData[tableVModel][index] && formData[tableVModel][index][childVModel]) || '';
        } else {
          templateJson[i].defaultValue = formData[templateJson[i].relationField] || '';
        }
      }
    }
    return templateJson;
  }

  function GoodsListBtn(val, option) {
    /* 兜底保证是数组 */
    if (!Array.isArray(state.dataForm.tableField751ecb)) {
      state.dataForm.tableField751ecb = [];
    }

    /* 1. 把 option 里的 id 做成 Set，方便比对 */
    const optionIdSet = new Set(option.map(o => o.id));

    /* 2. 删除本地已不在 option 里的行 */
    state.dataForm.tableField751ecb = state.dataForm.tableField751ecb.filter(item => optionIdSet.has(item.F_GoodsId));

    /* 3. 把 option 里本地还没有的行加进来 */
    option.forEach((o, i) => {
      const exist = state.dataForm.tableField751ecb.findIndex(item => item.F_GoodsId === o.id) !== -1;
      if (!exist) {
        const newRow = {
          jnpfId: buildUUID(),
          F_GoodsId: o.id,
          F_WorkOrderNo: undefined,
          F_Specification: o.F_Specification,
          F_RoutingId: o.F_RoutingId,
          F_PlanNum: o.F_PlanNum ?? 1,
          F_DeliveryDate: o.F_DeliveryDate ?? undefined,
          F_Priority: o.F_SaleQty ?? '1',
          F_BomId: o.F_BomId ?? undefined,
          F_DrawingFiles: JSON.parse(o.F_DrawingFiles) ?? [],
          tableField751ecb_F_BomIdOptions: [],
          F_DoorPlateThickness: undefined,
          F_BoxPlateThickness: undefined,
          F_InstallPlateOrBeam: undefined,
          F_CustomerName: state.customerId ?? '',
          F_LockSet: undefined,
          F_Hinge: undefined,
          F_BomImages: [],
          F_Color: undefined,
          F_Category: JSON.parse(o.F_Category) ?? JSON.parse(o.F_CategoryId) ?? [],
          ProjectGoods: [],
          F_CreatorTime: undefined,
        };
        state.dataForm.tableField751ecb.push(newRow);
        state.tableField751ecbinnerActiveKey.push(newRow.jnpfId);
      }
      gettableField751ecb_F_BomIdOptions(i);
    });
  }
  function ContractBtn(val, option) {
    state.customerId = option.F_CustomerId;
    state.dataForm.GoodsList = [];
    state.dataForm.tableField751ecb = [];
  }
  function ProjectTypeBtn(val, option) {
    if (val != 1) {
      state.dataForm.GoodsList = [];
      state.dataForm.tableField751ecb = [];
      state.dataForm.F_ContractId = undefined;
    } else {
      state.customerId = '';
    }
  }
  function F_BomIdTableChange(record) {
    if (record.F_BomId == undefined || record.F_BomId == '') {
      const idx = state.dataForm.tableField751ecb.findIndex(item => item.jnpfId === record.jnpfId);
      state.dataForm.tableField751ecb[idx].ProjectGoods = [];
    }
    getBOMGoodsList(record.F_BomId).then(res => {
      const idx = state.dataForm.tableField751ecb.findIndex(item => item.jnpfId === record.jnpfId);
      if (idx > -1) {
        if (!Array.isArray(state.dataForm.tableField751ecb[idx].ProjectGoods)) {
          state.dataForm.tableField751ecb[idx].ProjectGoods = [];
        }
        let nextSort = 1;
        for (var i = 0; i < res.data.length; i++) {
          const newRow = {
            jnpfId: buildUUID(),
            F_GoodsId: res.data[i]?.F_GoodsId,
            F_GoodsNo: res.data[i]?.F_GoodsNo,
            F_InventoryNum: res.data[i]?.F_InventoryNum,
            F_NumUnit: res.data[i]?.F_NumUnit,
            F_ProdUnit: 1,
            F_SortCode: nextSort,
            F_Specification: res.data[i]?.F_Specification,
            F_Unit: res.data[i]?.F_Unit,
            F_UnitTwo: res.data[i]?.F_UnitTwo,
            F_StockInProcess: undefined,
            F_FeedProcess: undefined,
            F_Description: undefined,
            F_CreatorTime: undefined,
          };
          state.dataForm.tableField751ecb[idx].ProjectGoods.push(newRow);
          nextSort++;
        }
      }
    });
  }
  const listPagination = computed(() => ({
    current: state.listCurrentPage,
    pageSize: state.listPageSize,
    size: 'small',
    showSizeChanger: true,
    pageSizeOptions: [20, 50, 100],
    showTotal: (total: number) => `共 ${total} 条`,
    onChange: (page: number) => {
      state.listCurrentPage = page;
    },
    onShowSizeChange: (_current: number, size: number) => {
      state.listPageSize = size;
      state.listCurrentPage = 1; // 切换页大小时回到第1页
    },
  }));
</script>
<!-- 全局样式：分页下拉菜单宽度 -->
<style lang="less">
  /* 分页下拉框触发器和弹出菜单都加宽 */
  .ant-pagination-options-size-changer {
    min-width: 100px;
    .ant-select {
      width: 100%;
    }
  }
</style>
