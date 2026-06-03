<template>
  <BasicModal v-bind="$attrs" @register="registerModal" width="80%" :minHeight="100" okText="确定" cancelText="取消" @ok="handleSubmit" :closeFunc="onClose">
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_ProdPlanId')">
            <a-form-item name="F_ProdPlanId" :labelCol="{ style: { width: '100px' } }">
              <template #label>生产计划</template>
              <JnpfPopupSelect
                v-model:value="dataForm.F_ProdPlanId"
                placeholder="请选择"
                :templateJson="optionsObj.F_ProdPlanIdTemplateJson"
                allowClear
                field="F_ProdPlanId"
                interfaceId="2014512148058869760"
                :columnOptions="optionsObj.F_ProdPlanIdOptions"
                relationField="F_PlanNo"
                propsValue="F_Id"
                :pageSize="20"
                popupType="dialog"
                popupTitle="选择数据"
                popupWidth="50%"
                :disabled="state.dataForm.id"
                hasPage
                :style="{
                  width: '100%',
                }"
                :extraOptions="optionsObj.F_ProdPlanId"
                @change="handleProdPlanChange" />
            </a-form-item>
          </a-col>
          <a-col :span="16" class="ant-col-item" v-if="hasFormP('F_Description')">
            <a-form-item name="F_Description" :labelCol="{ style: { width: '100px' } }">
              <template #label>备注</template>
              <JnpfTextarea
                v-model:value="dataForm.F_Description"
                placeholder="请输入"
                allowClear
                :autoSize="{
                  minRows: 1,
                  maxRows: 4,
                }"
                :style="{
                  width: '100%',
                }"
                :showCount="true" />
            </a-form-item>
          </a-col>
          <!-- <a-col :span="24">
            <table class="table-grid-box" :style="{ '--borderType': 'solid', '--borderColor': '#E2E0E0', '--borderWidth': '1px' }">
              <tbody>
                <tr>
                  <td colspan="1" rowspan="1" :style="{ '--backgroundColor': '' }">
                    <a-row>
                      <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_ConsiderMainStock')">
                        <a-form-item name="F_ConsiderMainStock" :labelCol="{ style: { width: '130px' } }">
                          <template #label>是否考虑主商品库存</template>
                          <JnpfRadio
                            v-model:value="dataForm.F_ConsiderMainStock"
                            :options="optionsObj.F_ConsiderMainStockOptions"
                            :fieldNames="optionsObj.F_ConsiderMainStockProps"
                            direction="horizontal"
                            optionType="default"
                            :style="{
                              width: '100%',
                            }" />
                        </a-form-item>
                      </a-col>
                    </a-row>
                  </td>
                  <td colspan="1" rowspan="1" :style="{ '--backgroundColor': '' }">
                    <a-row>
                      <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_ConsiderMaterialStock')">
                        <a-form-item name="F_ConsiderMaterialStock" :labelCol="{ style: { width: '130px' } }">
                          <template #label>是否考虑物料库存</template>
                          <JnpfRadio
                            v-model:value="dataForm.F_ConsiderMaterialStock"
                            :options="optionsObj.F_ConsiderMaterialStockOptions"
                            :fieldNames="optionsObj.F_ConsiderMaterialStockProps"
                            direction="horizontal"
                            optionType="default"
                            :style="{
                              width: '100%',
                            }" />
                        </a-form-item>
                      </a-col>
                    </a-row>
                  </td>
                </tr>
                <tr>
                  <td colspan="1" rowspan="1" :style="{ '--backgroundColor': '' }">
                    <a-row>
                      <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_ConsiderMaterialOccupy')">
                        <a-form-item name="F_ConsiderMaterialOccupy" :labelCol="{ style: { width: '130px' } }">
                          <template #label>是否考虑物料占用</template>
                          <JnpfRadio
                            v-model:value="dataForm.F_ConsiderMaterialOccupy"
                            :options="optionsObj.F_ConsiderMaterialOccupyOptions"
                            :fieldNames="optionsObj.F_ConsiderMaterialOccupyProps"
                            direction="horizontal"
                            optionType="default"
                            :style="{
                              width: '100%',
                            }" />
                        </a-form-item>
                      </a-col>
                    </a-row>
                  </td>
                  <td colspan="1" rowspan="1" :style="{ '--backgroundColor': '' }">
                    <a-row>
                      <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_RoundUpQty')">
                        <a-form-item name="F_RoundUpQty" :labelCol="{ style: { width: '150px' } }">
                          <template #label>是否考虑数量向上取整</template>
                          <JnpfRadio
                            v-model:value="dataForm.F_RoundUpQty"
                            :options="optionsObj.F_RoundUpQtyOptions"
                            :fieldNames="optionsObj.F_RoundUpQtyProps"
                            direction="horizontal"
                            optionType="default"
                            :style="{
                              width: '100%',
                            }" />
                        </a-form-item>
                      </a-col>
                    </a-row>
                  </td>
                </tr>
                <tr>
                  <td colspan="1" rowspan="1" :style="{ '--backgroundColor': '' }">
                    <a-row>
                      <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_ConsiderWipGoods')">
                        <a-form-item name="F_ConsiderWipGoods" :labelCol="{ style: { width: '130px' } }">
                          <template #label>是否考虑在制商品</template>
                          <JnpfRadio
                            v-model:value="dataForm.F_ConsiderWipGoods"
                            :options="optionsObj.F_ConsiderWipGoodsOptions"
                            :fieldNames="optionsObj.F_ConsiderWipGoodsProps"
                            direction="horizontal"
                            optionType="default"
                            :style="{
                              width: '100%',
                            }" />
                        </a-form-item>
                      </a-col>
                    </a-row>
                  </td>
                  <td colspan="1" rowspan="1" :style="{ '--backgroundColor': '' }">
                    <a-row>
                      <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_ConsiderTransitGoods')">
                        <a-form-item name="F_ConsiderTransitGoods" :labelCol="{ style: { width: '130px' } }">
                          <template #label>是否考虑在途商品</template>
                          <JnpfRadio
                            v-model:value="dataForm.F_ConsiderTransitGoods"
                            :options="optionsObj.F_ConsiderTransitGoodsOptions"
                            :fieldNames="optionsObj.F_ConsiderTransitGoodsProps"
                            direction="horizontal"
                            optionType="default"
                            :style="{
                              width: '100%',
                            }" />
                        </a-form-item>
                      </a-col>
                    </a-row>
                  </td>
                </tr>
                <tr>
                  <td colspan="1" rowspan="1" :style="{ '--backgroundColor': '' }">
                    <a-row>
                      <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_OccupyAllowOtherOut')">
                        <a-form-item name="F_OccupyAllowOtherOut" :labelCol="{ style: { width: '200px' } }">
                          <template #label>占用物料能否被其他单据出库</template>
                          <JnpfRadio
                            v-model:value="dataForm.F_OccupyAllowOtherOut"
                            :options="optionsObj.F_OccupyAllowOtherOutOptions"
                            :fieldNames="optionsObj.F_OccupyAllowOtherOutProps"
                            direction="horizontal"
                            optionType="default"
                            :style="{
                              width: '100%',
                            }" />
                        </a-form-item>
                      </a-col>
                    </a-row>
                  </td>
                  <td colspan="1" rowspan="1" :style="{ '--backgroundColor': '' }">
                    <a-row> </a-row>
                  </td>
                </tr>
              </tbody>
            </table>
          </a-col> -->
          <!-- <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_AnalysisUserId')">
            <a-form-item name="F_AnalysisUserId" :labelCol="{ style: { width: '100px' } }">
              <template #label>分析人</template>
              <JnpfUserSelect
                v-model:value="dataForm.F_AnalysisUserId"
                placeholder="请选择"
                selectType="all"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_AnalysisTime')">
            <a-form-item name="F_AnalysisTime" :labelCol="{ style: { width: '100px' } }">
              <template #label>分析时间</template>
              <JnpfDatePicker
                v-model:value="dataForm.F_AnalysisTime"
                placeholder="请选择"
                format="yyyy-MM-dd"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_State')">
            <a-form-item name="F_State" :labelCol="{ style: { width: '100px' } }">
              <template #label>状态</template>
              <JnpfInput
                v-model:value="dataForm.F_State"
                placeholder="请输入"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col> -->
          <!-- <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_CreatorTime')">
            <a-form-item name="F_CreatorTime" :labelCol="{ style: { width: '100px' } }">
              <template #label>创建时间</template>
              <JnpfOpenData
                v-model:value="dataForm.F_CreatorTime"
                type="currTime"
                readonly
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_CreatorUserId')">
            <a-form-item name="F_CreatorUserId" :labelCol="{ style: { width: '100px' } }">
              <template #label>创建用户</template>
              <JnpfOpenData
                v-model:value="dataForm.F_CreatorUserId"
                type="currUser"
                readonly
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col> -->
          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="物料分析商品列表信息" :bordered="false" />
              <a-table
                :data-source="dataForm.tableField4d57ab"
                :columns="tableField4d57abColumns"
                size="small"
                rowKey="jnpfId"
                :pagination="false"
                :scroll="{ x: 'max-content' }"
                :rowSelection="gettableField4d57abRowSelection">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                  <template v-if="column.key === 'F_ParentId'">
                    <JnpfInput
                      v-model:value="record.F_ParentId"
                      placeholder="请输入"
                      readonly
                      :style="{
                        width: '100%',
                        backgroundColor: '#f5f5f5',
                        cursor: 'not-allowed',
                      }"
                      :showCount="false" />
                  </template>
                  <template v-if="column.key === 'F_GoodsId'">
                    <JnpfPopupSelect
                      v-model:value="record.F_GoodsId"
                      placeholder="请选择"
                      :templateJson="optionsObj.tableField4d57ab_F_GoodsIdTemplateJson"
                      allowClear
                      :field="'F_GoodsId' + index"
                      interfaceId="2008721559174385664"
                      :columnOptions="optionsObj.tableField4d57ab_F_GoodsIdOptions"
                      relationField="F_GoodsName"
                      propsValue="F_Id"
                      :pageSize="20"
                      popupType="dialog"
                      popupTitle="选择数据"
                      popupWidth="60%"
                      hasPage
                      :style="{
                        width: '100%',
                      }"
                      :extraOptions="optionsObj.tableField4d57ab_F_GoodsId" />
                  </template>
                  <template v-if="column.key === 'F_Unit'">
                    <JnpfInputNumber
                      v-model:value="record.F_Unit"
                      placeholder="请输入"
                      :controls="false"
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_DemandQty'">
                    <JnpfInputNumber
                      v-model:value="record.F_DemandQty"
                      placeholder="请输入"
                      :controls="false"
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_AvailableStock'">
                    <JnpfInputNumber
                      v-model:value="record.F_AvailableStock"
                      placeholder="请输入"
                      :controls="false"
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_WipStock'">
                    <JnpfInputNumber
                      v-model:value="record.F_WipStock"
                      placeholder="请输入"
                      :controls="false"
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_TransitStock'">
                    <JnpfInputNumber
                      v-model:value="record.F_TransitStock"
                      placeholder="请输入"
                      :controls="false"
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_ShouldSelfMake'">
                    <JnpfInputNumber
                      v-model:value="record.F_ShouldSelfMake"
                      placeholder="请输入"
                      :controls="false"
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_ShouldOutsource'">
                    <JnpfInputNumber
                      v-model:value="record.F_ShouldOutsource"
                      placeholder="请输入"
                      :controls="false"
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_ShouldPurchase'">
                    <JnpfInputNumber
                      v-model:value="record.F_ShouldPurchase"
                      placeholder="请输入"
                      :controls="false"
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_CreatorUserId'">
                    <JnpfOpenData
                      v-model:value="record.F_CreatorUserId"
                      type="currUser"
                      readonly
                      :style="{
                        width: '100%',
                      }" />
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
                      <a-button class="action-btn" type="link" @click="copytableField4d57abRow(index)" size="small">{{
                        t('common.copyText', '复制')
                      }}</a-button>
                      <a-button class="action-btn" type="link" color="error" @click="removetableField4d57abRow(index, true)" size="small">{{
                        t('common.delText', '删除')
                      }}</a-button>
                    </a-space>
                  </template>
                </template>
              </a-table>
              <a-space class="input-table-footer-btn">
                <a-button type="primary" preIcon="icon-ym icon-ym-btn-add" @click="addtableField4d57abRow">{{ t('common.add1Text', '添加') }}</a-button>
                <a-button type="danger" preIcon="icon-ym icon-ym-btn-clearn" @click="batchRemovetableField4d57abRow(true)">{{
                  t('common.batchDelText', '批量删除')
                }}</a-button>
              </a-space>
            </a-form-item>
          </a-col>
        </a-row>
      </a-form>
    </a-row>
  </BasicModal>
  <SelectModal :config="state.currTableConf" :formData="state.dataForm" ref="selectModal" @select="addForSelect" />
</template>
<script lang="ts" setup>
  import { create, update, getInfo, getBomMaterials } from './helper/api';
  import { reactive, toRefs, nextTick, ref, unref, computed, toRaw, inject } from 'vue';
  import { BasicModal, useModal } from '@/components/Modal';
  import SelectModal from '@/components/CommonModal/src/SelectModal.vue';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useI18n } from '@/hooks/web/useI18n';
  import { useUserStore } from '@/store/modules/user';
  import type { FormInstance } from 'ant-design-vue';
  import { thousandsFormat } from '@/utils/jnpf';
  import { getTimeUnit, getDateTimeUnit } from '@/utils/jnpf';
  import dayjs from 'dayjs';
  import { cloneDeep } from 'lodash-es';
  import { buildUUID } from '@/utils/uuid';
  import { CaretRightOutlined } from '@ant-design/icons-vue';
  import { systemComponentsList } from '@/components/FormGenerator/src/helper/config';
  import { usePermission } from '@/hooks/web/usePermission';

  interface State {
    dataForm: any;
    dataRule: any;
    optionsObj: any;
    currVmodel: any;
    currTableConf: any;
    addTableConf: any;
    tableRows: any;
    tableField4d57abouterActiveKey: number[];
    tableField4d57abinnerActiveKey: string[];
    tableField4d57abDefaultExpandAll: boolean;
    selectedtableField4d57abRowKeys: any[];
    title: string;
    continueText: string;
    allList: any[];
    currIndex: number;
    isContinue: boolean;
    submitType: number;
    showContinueBtn: boolean;
    bomMaterials: any[]; // 存储BOM物料信息
  }

  const emit = defineEmits(['reload']);
  const getLeftTreeActiveInfo: (() => any) | null = inject('getLeftTreeActiveInfo', null);
  const userStore = useUserStore();
  const userInfo = userStore.getUserInfo;
  const { createMessage, createConfirm } = useMessage();
  const { t } = useI18n();
  const { hasFormP } = usePermission();
  const [registerModal, { openModal, setModalProps }] = useModal();
  const formRef = ref<FormInstance>();
  const selectModal = ref(null);
  const tableField4d57abColumns: any[] = computed(() => {
    let list = [
      // {
      //   title: '商品上级',
      //   dataIndex: 'F_ParentId',
      //   key: 'F_ParentId',
      //   tipLabel: '',
      //   required: false,
      //   align: 'left',
      //   span: '24',
      //   labelWidth: '',
      //   width: '',
      //   fixed: false,
      //   formP: 'F_ParentId',
      //   isSystemControl: false,
      // },
      {
        title: '商品',
        dataIndex: 'F_GoodsId',
        key: 'F_GoodsId',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_GoodsId',
        isSystemControl: false,
      },
      {
        title: '单位用量',
        dataIndex: 'F_Unit',
        key: 'F_Unit',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Unit',
        isSystemControl: false,
      },
      {
        title: '需求',
        dataIndex: 'F_DemandQty',
        key: 'F_DemandQty',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_DemandQty',
        isSystemControl: false,
      },
      {
        title: '可用库存',
        dataIndex: 'F_AvailableStock',
        key: 'F_AvailableStock',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_AvailableStock',
        isSystemControl: false,
      },
      {
        title: '在制库存',
        dataIndex: 'F_WipStock',
        key: 'F_WipStock',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_WipStock',
        isSystemControl: false,
      },
      {
        title: '在途库存',
        dataIndex: 'F_TransitStock',
        key: 'F_TransitStock',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_TransitStock',
        isSystemControl: false,
      },
      {
        title: '应自产',
        dataIndex: 'F_ShouldSelfMake',
        key: 'F_ShouldSelfMake',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_ShouldSelfMake',
        isSystemControl: false,
      },
      {
        title: '应外协',
        dataIndex: 'F_ShouldOutsource',
        key: 'F_ShouldOutsource',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_ShouldOutsource',
        isSystemControl: false,
      },
      {
        title: '应采购',
        dataIndex: 'F_ShouldPurchase',
        key: 'F_ShouldPurchase',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_ShouldPurchase',
        isSystemControl: false,
      },
      // {
      //   title: '创建用户',
      //   dataIndex: 'F_CreatorUserId',
      //   key: 'F_CreatorUserId',
      //   tipLabel: '',
      //   required: false,
      //   align: 'left',
      //   span: '24',
      //   labelWidth: '',
      //   width: '',
      //   fixed: false,
      //   formP: 'F_CreatorUserId',
      //   isSystemControl: true,
      // },
      // {
      //   title: '创建时间',
      //   dataIndex: 'F_CreatorTime',
      //   key: 'F_CreatorTime',
      //   tipLabel: '',
      //   required: false,
      //   align: 'left',
      //   span: '24',
      //   labelWidth: '',
      //   width: '',
      //   fixed: false,
      //   formP: 'F_CreatorTime',
      //   isSystemControl: true,
      // },
    ];
    list = list.filter(o => hasFormP('tableField4d57ab-' + o.formP));
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
  const gettableField4d57abRowSelection = computed(() => {
    const rowSelection = {
      selectedRowKeys: state.selectedtableField4d57abRowKeys,
      onChange: (selectedRowKeys: string[]) => {
        state.selectedtableField4d57abRowKeys = (selectedRowKeys || []).sort().reverse();
      },
    };
    return rowSelection;
  });
  const state = reactive<State>({
    dataForm: {
      id: '',
      F_ProdPlanId: undefined,
      F_Description: undefined,
      F_ConsiderMainStock: undefined,
      F_ConsiderMaterialStock: undefined,
      F_ConsiderMaterialOccupy: undefined,
      F_RoundUpQty: undefined,
      F_ConsiderWipGoods: undefined,
      F_ConsiderTransitGoods: undefined,
      F_OccupyAllowOtherOut: undefined,
      F_AnalysisUserId: userInfo.userId ? userInfo.userId : '',
      F_AnalysisTime: undefined,
      F_State: undefined,
      F_CreatorTime: undefined,
      F_CreatorUserId: undefined,
      tableField4d57ab: [],
    },
    tableField4d57abouterActiveKey: [0],
    tableField4d57abinnerActiveKey: [],
    tableField4d57abDefaultExpandAll: true,
    selectedtableField4d57abRowKeys: [],
    dataRule: {
      F_ProdPlanId: [
        {
          required: true,
          message: '请选择生产计划',
          trigger: 'change',
        },
      ],
    },
    optionsObj: {
      F_ProdPlanIdOptions: [
        {
          value: 'F_PlanNo',
          label: '计划编号',
        },
        {
          value: 'F_PlanName',
          label: '计划名称',
        },
        {
          value: 'F_DeliveryDate',
          label: '交货日期',
        },
        {
          value: 'F_Description',
          label: '备注',
        },
      ],
      F_ProdPlanIdTemplateJson: [],
      F_ConsiderMainStockOptions: [
        { fullName: '是', id: '1' },
        { fullName: '否', id: '2' },
      ],
      F_ConsiderMainStockProps: { label: 'fullName', value: 'id' },
      F_ConsiderMaterialStockOptions: [
        { fullName: '是', id: '1' },
        { fullName: '否', id: '2' },
      ],
      F_ConsiderMaterialStockProps: { label: 'fullName', value: 'id' },
      F_ConsiderMaterialOccupyOptions: [
        { fullName: '是', id: '1' },
        { fullName: '否', id: '2' },
      ],
      F_ConsiderMaterialOccupyProps: { label: 'fullName', value: 'id' },
      F_RoundUpQtyOptions: [
        { fullName: '是', id: '1' },
        { fullName: '否', id: '2' },
      ],
      F_RoundUpQtyProps: { label: 'fullName', value: 'id' },
      F_ConsiderWipGoodsOptions: [
        { fullName: '是', id: '1' },
        { fullName: '否', id: '2' },
      ],
      F_ConsiderWipGoodsProps: { label: 'fullName', value: 'id' },
      F_ConsiderTransitGoodsOptions: [
        { fullName: '是', id: '1' },
        { fullName: '否', id: '2' },
      ],
      F_ConsiderTransitGoodsProps: { label: 'fullName', value: 'id' },
      F_OccupyAllowOtherOutOptions: [
        { fullName: '是', id: '1' },
        { fullName: '否', id: '2' },
      ],
      F_OccupyAllowOtherOutProps: { label: 'fullName', value: 'id' },
      tableField4d57ab_F_GoodsIdOptions: [
        {
          value: 'F_GoodsName',
          label: '商品名称',
        },
        {
          value: 'F_GoodsNo',
          label: '商品编号',
        },
        {
          value: 'F_Unit',
          label: '单位',
        },
        {
          value: 'F_SalePrice',
          label: '销售单价',
        },
        {
          value: 'F_CostPrice',
          label: '成本单价',
        },
        {
          value: 'F_Source',
          label: '商品来源',
        },
        {
          value: 'F_Remark',
          label: '备注',
        },
      ],
      tableField4d57ab_F_GoodsIdTemplateJson: [],
      F_ProdPlanId: [],
    },
    currVmodel: '',
    currTableConf: {},
    tableRows: {
      tableField4d57ab: {
        enabledmark: undefined,
        F_ParentId: undefined,
        F_GoodsId: undefined,
        F_Unit: undefined,
        F_DemandQty: undefined,
        F_AvailableStock: undefined,
        F_WipStock: undefined,
        F_TransitStock: undefined,
        F_ShouldSelfMake: undefined,
        F_ShouldOutsource: undefined,
        F_ShouldPurchase: undefined,
        F_CreatorUserId: undefined,
        F_CreatorTime: undefined,
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
    bomMaterials: [], // 存储BOM物料信息
  });
  const { title, continueText, showContinueBtn, dataForm, submitType, dataRule, optionsObj } = toRefs(state);

  const getPrevDisabled = computed(() => state.currIndex === 0);
  const getNextDisabled = computed(() => state.currIndex === state.allList.length - 1);

  defineExpose({ init });

  function init(data) {
    state.submitType = 0;
    state.showContinueBtn = true;
    state.title = !data.id ? '新增' : '编辑';
    state.continueText = !data.id ? '确定并新增' : '确定并继续';
    setFormProps({ continueLoading: false });
    state.dataForm.id = data.id;
    openModal();
    state.allList = data.allList;
    state.currIndex = state.allList.length && data.id ? state.allList.findIndex(item => item.id === data.id) : 0;
    nextTick(() => {
      getForm().resetFields();
      state.dataForm.tableField4d57ab = [];
      setTimeout(initData, 0);
    });
  }

  // 处理生产计划选择变化
  async function handleProdPlanChange(value) {
    if (value) {
      try {
        const res = await getBomMaterials(value);
        // 将返回的BOM物料信息存储到状态中
        state.bomMaterials = res.data || [];

        // 如果后端返回一条或多条数据，则为每条数据向子表增加一行
        if (state.bomMaterials && state.bomMaterials.length > 0) {
          for (let i = 0; i < state.bomMaterials.length; i++) {
            const mat = state.bomMaterials[i];
            // 使用 tableRows 中的模板对象作为初始值（浅拷贝）
            const newItem = { ...state.tableRows.tableField4d57ab };
            newItem.jnpfId = buildUUID();
            // 将后端返回字段赋值到子表对应字段
            newItem.F_GoodsId = mat.F_GoodsId ?? newItem.F_GoodsId;
            // 将后端 F_BomId 赋值到子表的 F_ParentId（商品上级）
            newItem.F_ParentId = mat.F_BomId ?? newItem.F_ParentId;
            // 如果返回了单位，用于赋值
            if (mat.F_Unit !== undefined) newItem.F_Unit = mat.F_Unit;
            // 推入子表数据并展开
            state.dataForm.tableField4d57ab.push(newItem);
            state.tableField4d57abinnerActiveKey.push(newItem.jnpfId);
          }
        }

        // 可以在这里根据BOM物料信息更新界面或进行其他业务逻辑
      } catch (error) {
        console.error('获取BOM物料信息失败:', error);
        createMessage.error('获取BOM物料信息失败');
        state.bomMaterials = [];
      }
    } else {
      // 清空数据当没有选择生产计划时
      state.bomMaterials = [];
    }
  }
  function initData() {
    changeLoading(true);
    if (state.dataForm.id) {
      getData(state.dataForm.id);
    } else {
      // 设置默认值
      state.dataForm = {
        F_ProdPlanId: undefined,
        F_Description: undefined,
        F_ConsiderMainStock: undefined,
        F_ConsiderMaterialStock: undefined,
        F_ConsiderMaterialOccupy: undefined,
        F_RoundUpQty: undefined,
        F_ConsiderWipGoods: undefined,
        F_ConsiderTransitGoods: undefined,
        F_OccupyAllowOtherOut: undefined,
        F_AnalysisUserId: userInfo.userId ? userInfo.userId : '',
        F_AnalysisTime: undefined,
        F_State: undefined,
        F_CreatorTime: undefined,
        F_CreatorUserId: undefined,
        tableField4d57ab: [],
      };
      if (getLeftTreeActiveInfo) state.dataForm = { ...state.dataForm, ...(getLeftTreeActiveInfo() || {}) };
      changeLoading(false);
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
      for (let i = 0; i < state.dataForm.tableField4d57ab.length; i++) {
        const element = state.dataForm.tableField4d57ab[i];
        element.jnpfId = buildUUID();
      }
      changeLoading(false);
    });
  }
  async function handleSubmit() {
    try {
      const values = await getForm()?.validate();
      if (!values) return;
      setFormProps({ confirmLoading: true });
      const formMethod = state.dataForm.id ? update : create;
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
  function addtableField4d57abRow() {
    let item = {
      jnpfId: buildUUID(),
      F_ParentId: undefined,
      F_GoodsId: undefined,
      F_Unit: undefined,
      F_DemandQty: undefined,
      F_AvailableStock: undefined,
      F_WipStock: undefined,
      F_TransitStock: undefined,
      F_ShouldSelfMake: undefined,
      F_ShouldOutsource: undefined,
      F_ShouldPurchase: undefined,
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
    };
    state.dataForm.tableField4d57ab.push(item);
    state.tableField4d57abinnerActiveKey.push(item.jnpfId);
  }
  function removetableField4d57abRow(index, showConfirm = false) {
    if (showConfirm) {
      createConfirm({
        iconType: 'warning',
        title: '提示',
        content: '此操作将永久删除该数据, 是否继续?',
        onOk: () => {
          state.dataForm.tableField4d57ab.splice(index, 1);
        },
      });
    } else {
      state.dataForm.tableField4d57ab.splice(index, 1);
    }
  }
  function copytableField4d57abRow(index) {
    let item = cloneDeep(state.dataForm.tableField4d57ab[index]);
    let copyData = {};
    for (let i = 0; i < unref(tableField4d57abColumns).length; i++) {
      const cur = unref(tableField4d57abColumns)[i];
      if (cur.key != 'index' && cur.key != 'action') {
        if (cur.children?.length && cur.jnpfKey == 'complexHeader') {
          for (let j = 0; j < cur.children.length; j++) {
            copyData[cur.children[j].key] = cur.isSystemControl ? null : item[cur.children[j].key];
          }
        } else {
          copyData[cur.key] = cur.isSystemControl ? null : item[cur.key];
        }
      }
    }
    const copyItem = { ...copyData, jnpfId: buildUUID() };
    state.dataForm.tableField4d57ab.push(copyItem);
    state.tableField4d57abinnerActiveKey.push(copyItem.jnpfId);
  }
  function batchRemovetableField4d57abRow(showConfirm = false) {
    if (!state.selectedtableField4d57abRowKeys.length) return createMessage.error('请选择一条数据');
    const handleBatchRemove = () => {
      state.dataForm.tableField4d57ab = state.dataForm.tableField4d57ab.filter(o => !state.selectedtableField4d57abRowKeys.includes(o.jnpfId));
      nextTick(() => {
        state.selectedtableField4d57abRowKeys = [];
      });
    };
    if (!showConfirm) return handleBatchRemove();
    createConfirm({
      iconType: 'warning',
      title: '提示',
      content: '此操作将永久删除该数据, 是否继续?',
      onOk: handleBatchRemove,
    });
  }
  function openSelectDialog(key, value) {
    state.currTableConf = state.addTableConf[key + 'List' + value];
    state.currVmodel = key;
    nextTick(() => {
      (selectModal.value as any)?.openSelectModal();
    });
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
</script>
