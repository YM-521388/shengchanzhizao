<template>
  <BasicModal v-bind="$attrs" @register="registerModal" width="1300px" :minHeight="100" okText="确定" cancelText="取消" @ok="handleSubmit" :closeFunc="onClose">
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
          <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_ProcessNo" :labelCol="{ style: { width: '100px' } }">
              <template #label>加工单号</template>
              <JnpfInput
                v-model:value="dataForm.F_ProcessNo"
                placeholder="请填写，忽略将自动生成"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_GoodsId" :labelCol="{ style: { width: '100px' } }">
              <template #label>商品</template>
              <JnpfPopupSelect
                v-model:value="dataForm.F_GoodsId"
                @change="F_GoodsIdChange"
                placeholder=""
                :templateJson="optionsObj.F_GoodsIdTemplateJson"
                allowClear
                field="F_GoodsId"
                interfaceId="2012085692393459712"
                :columnOptions="optionsObj.F_GoodsIdOptions"
                relationField="F_GoodsName"
                propsValue="id"
                :pageSize="20"
                :disabled="dataForm.id"
                popupType="dialog"
                popupTitle="选择数据"
                popupWidth="1000px"
                hasPage
                :style="{
                  width: '100%',
                }"
                :extraOptions="optionsObj.F_GoodsId" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_BOMId" :labelCol="{ style: { width: '100px' } }">
              <template #label>BOM</template>
              <JnpfSelect
                v-model:value="dataForm.F_BOMId"
                placeholder=""
                :options="optionsObj.F_BOMIdOptions"
                :fieldNames="optionsObj.F_BOMIdProps"
                allowClear
                showSearch
                @change="F_BomChange"
                :disabled="!dataForm.F_GoodsId"
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_PlanQty" :labelCol="{ style: { width: '100px' } }">
              <template #label>计划数</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_PlanQty"
                placeholder="请输入"
                :min="1"
                :controls="false"
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_DeliveryDate" :labelCol="{ style: { width: '100px' } }">
              <template #label>交货日期</template>
              <JnpfDatePicker
                v-model:value="dataForm.F_DeliveryDate"
                placeholder="请选择"
                format="yyyy-MM-dd"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_Priority" :labelCol="{ style: { width: '100px' } }">
              <template #label>优先级</template>
              <JnpfSelect
                v-model:value="dataForm.F_Priority"
                placeholder="请选择"
                :options="optionsObj.F_PriorityOptions"
                :fieldNames="optionsObj.F_PriorityProps"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_PlanStartDate" :labelCol="{ style: { width: '100px' } }">
              <template #label>计划开始</template>
              <JnpfDatePicker
                v-model:value="dataForm.F_PlanStartDate"
                placeholder="请选择"
                format="yyyy-MM-dd"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_PlanEndDate" :labelCol="{ style: { width: '100px' } }">
              <template #label>计划结束</template>
              <JnpfDatePicker
                v-model:value="dataForm.F_PlanEndDate"
                placeholder="请选择"
                format="yyyy-MM-dd"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_DrawingFile" :labelCol="{ style: { width: '100px' } }">
              <template #label>图纸</template>
              <JnpfUploadFile
                v-model:value="dataForm.F_DrawingFile"
                buttonText="点击上传"
                pathType="selfPath"
                :isAccount="-1"
                folder=""
                :fileSize="10"
                sizeUnit="MB"
                :limit="9"
                :sortRule="['1', '2']"
                timeFormat="YYYYMMDD" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_CustomerName" :labelCol="{ style: { width: '100px' } }">
              <template #label>客户</template>
              <JnpfInput
                v-model:value="dataForm.F_CustomerName"
                placeholder="请输入"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_DoorPlateThickness" :labelCol="{ style: { width: '100px' } }">
              <template #label>门板厚度</template>
              <JnpfSelect
                v-model:value="dataForm.F_DoorPlateThickness"
                placeholder="请选择"
                :options="optionsObj.F_DoorPlateThicknessOptions"
                :fieldNames="optionsObj.F_DoorPlateThicknessProps"
                allowClear
                showSearch
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_BoxPlateThickness" :labelCol="{ style: { width: '100px' } }">
              <template #label>箱体板厚</template>
              <JnpfSelect
                v-model:value="dataForm.F_BoxPlateThickness"
                placeholder="请选择"
                :options="optionsObj.F_BoxPlateThicknessOptions"
                :fieldNames="optionsObj.F_BoxPlateThicknessProps"
                allowClear
                showSearch
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_InstallOrBeam" :labelCol="{ style: { width: '120px' } }">
              <template #label>安装板或安装梁</template>
              <JnpfSelect
                v-model:value="dataForm.F_InstallOrBeam"
                placeholder="请选择"
                :options="optionsObj.F_InstallOrBeamOptions"
                :fieldNames="optionsObj.F_InstallOrBeamProps"
                allowClear
                showSearch
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_LockSet" :labelCol="{ style: { width: '100px' } }">
              <template #label>锁具</template>
              <JnpfInput
                v-model:value="dataForm.F_LockSet"
                placeholder="请输入"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_Hinge" :labelCol="{ style: { width: '100px' } }">
              <template #label>铰链</template>
              <JnpfInput
                v-model:value="dataForm.F_Hinge"
                placeholder="请输入"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_Color" :labelCol="{ style: { width: '100px' } }">
              <template #label>颜色</template>
              <JnpfInput
                v-model:value="dataForm.F_Color"
                placeholder="请输入"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_Type" :labelCol="{ style: { width: '100px' } }">
              <template #label>类别</template>
              <JnpfCascader
                v-model:value="dataForm.F_Type"
                placeholder="请选择"
                :options="optionsObj.F_TypeOptions"
                :fieldNames="optionsObj.F_TypeProps"
                allowClear
                showSearch
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_BOMImages" :labelCol="{ style: { width: '100px' } }">
              <template #label>BOM图片</template>
              <JnpfUploadImg
                v-model:value="dataForm.F_BOMImages"
                pathType="selfPath"
                :isAccount="-1"
                folder=""
                sizeUnit="MB"
                :fileSize="10"
                :sortRule="['1', '2']"
                timeFormat="YYYYMMDD"
                showType="button"
                buttonText="点击上传" />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item">
            <a-form-item name="F_Description" :labelCol="{ style: { width: '100px' } }">
              <template #label>备注</template>
              <JnpfTextarea
                v-model:value="dataForm.F_Description"
                placeholder="请输入"
                :maxlength="500"
                allowClear
                :autoSize="{
                  minRows: 2,
                  maxRows: 2,
                }"
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-show="false">
            <a-form-item name="F_ContractId" :labelCol="{ style: { width: '100px' } }">
              <template #label>合同</template>
              <JnpfSelect
                v-model:value="dataForm.F_ContractId"
                placeholder="请选择"
                :options="optionsObj.F_ContractIdOptions"
                :fieldNames="optionsObj.F_ContractIdProps"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-show="false">
            <a-form-item name="F_ProjectId" :labelCol="{ style: { width: '100px' } }">
              <template #label>项目</template>
              <JnpfSelect
                v-model:value="dataForm.F_ProjectId"
                placeholder="请选择"
                :options="optionsObj.F_ProjectIdOptions"
                :fieldNames="optionsObj.F_ProjectIdProps"
                allowClear
                showSearch
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-show="false">
            <a-form-item name="F_ProdPlanId" :labelCol="{ style: { width: '100px' } }">
              <template #label>生产计划</template>
              <JnpfSelect
                v-model:value="dataForm.F_ProdPlanId"
                placeholder="请选择"
                :options="optionsObj.F_ProdPlanIdOptions"
                :fieldNames="optionsObj.F_ProdPlanIdProps"
                allowClear
                showSearch
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-show="false">
            <a-form-item name="F_State" :labelCol="{ style: { width: '100px' } }">
              <template #label>状态</template>
              <JnpfSelect
                v-model:value="dataForm.F_State"
                placeholder="请选择"
                :options="optionsObj.F_StateOptions"
                :fieldNames="optionsObj.F_StateProps"
                allowClear
                showSearch
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-show="false">
            <a-form-item name="F_SequenceNo" :labelCol="{ style: { width: '100px' } }">
              <template #label>排单序号</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_SequenceNo"
                placeholder="请输入"
                :min="1"
                :controls="false"
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>

          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="生产任务" :bordered="false" />
              <a-row :gutter="15">
                <a-col :span="18" class="ant-col-item">
                  <a-form-item name="ProcessList" :labelCol="{ style: { width: '100px' } }">
                    <template #label>选择工序</template>
                    <JnpfPopupMultipleSelect
                      v-model:value="state.ProcessList"
                      placeholder=""
                      :templateJson="optionsObj.Process_F_ProcessIdTemplateJson"
                      allowClear
                      field="ProcessList"
                      interfaceId="2012092092830060544"
                      :columnOptions="optionsObj.Process_F_ProcessIdOptions"
                      relationField="F_ProcessName"
                      propsValue="id"
                      :pageSize="20"
                      popupType="dialog"
                      popupTitle="选择数据"
                      popupWidth="1200px"
                      hasPage
                      @change="ProcessListBtn"
                      :style="{
                        width: '100%',
                      }"
                      :extraOptions="optionsObj.ProcessList" />
                  </a-form-item>
                </a-col>
                <a-col :span="6" class="ant-col-item">
                  <a-form-item name="RoutingStr" :labelCol="{ style: { width: '130px' } }">
                    <template #label>从工艺路线中添加</template>
                    <JnpfSelect
                      v-model:value="state.RoutingStr"
                      placeholder=""
                      :options="optionsObj.RoutingStrOptions"
                      :fieldNames="optionsObj.RoutingStrProps"
                      allowClear
                      showSearch
                      @change="RoutingStrBtn"
                      :style="{
                        width: '100%',
                      }" />
                  </a-form-item>
                </a-col>
              </a-row>
              <a-table
                :data-source="dataForm.tableField0c720e"
                :columns="tableField0c720eColumns"
                size="small"
                rowKey="jnpfId"
                :pagination="false"
                :scroll="{ x: 'max-content' }"
                :rowSelection="gettableField0c720eRowSelection">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                  <template v-if="column.key === 'F_SortCode'">
                    <JnpfInputNumber
                      v-model:value="record.F_SortCode"
                      placeholder="排序不可重复"
                      :min="1"
                      :controls="false"
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_ProcessId'">
                    <JnpfSelect
                      v-model:value="record.F_ProcessId"
                      :options="optionsObj.tableField0c720e_F_ProcessIdOptions"
                      :fieldNames="optionsObj.tableField0c720e_F_ProcessIdProps"
                      allowClear
                      placeholder=""
                      showSearch
                      disabled
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_PlanStartDate'">
                    <JnpfDatePicker
                      v-model:value="record.F_PlanStartDate"
                      placeholder="请选择"
                      format="yyyy-MM-dd"
                      allowClear
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_PlanEndDate'">
                    <JnpfDatePicker
                      v-model:value="record.F_PlanEndDate"
                      placeholder="请选择"
                      format="yyyy-MM-dd"
                      allowClear
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_Description'">
                    <JnpfInput
                      v-model:value="record.F_Description"
                      placeholder="请输入"
                      :maxlength="500"
                      allowClear
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
                      <a-button class="action-btn" type="link" @click="goodsHandle(record)" size="small">{{ t('物料清单') }}</a-button>
                      <a-button class="action-btn" type="link" color="error" @click="removetableField0c720eRow(index, true)" size="small">{{
                        t('common.delText', '删除')
                      }}</a-button>
                    </a-space>
                  </template>
                </template>
              </a-table>
              <a-space class="input-table-footer-btn"> </a-space>
            </a-form-item>
          </a-col>

          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="用料清单" :bordered="false" helpMessage="选择BOM后将拉取数据填入用料清单" />
              <a-col :span="24" class="ant-col-item">
                <a-form-item name="GoodsList" :labelCol="{ style: { width: '100px' } }">
                  <template #label>添加用料</template>
                  <JnpfPopupMultipleSelect
                    v-model:value="state.GoodsList"
                    placeholder="请选择"
                    :templateJson="optionsObj.GoodsListTemplateJson"
                    allowClear
                    field="GoodsList"
                    interfaceId="2012085692393459712"
                    :columnOptions="optionsObj.GoodsListOptions"
                    relationField="F_GoodsName"
                    propsValue="id"
                    :pageSize="20"
                    popupType="dialog"
                    popupTitle="选择数据"
                    popupWidth="1000px"
                    @change="GoodsListBtn"
                    :style="{
                      width: '100%',
                    }"
                    :extraOptions="optionsObj.GoodsList" />
                </a-form-item>
              </a-col>
              <a-table
                :data-source="dataForm.tableField449e6c"
                :columns="tableField449e6cColumns"
                size="small"
                rowKey="jnpfId"
                :pagination="false"
                :scroll="{ x: 'max-content' }"
                :rowSelection="gettableField449e6cRowSelection">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                  <template v-if="column.key === 'F_SortCode'">
                    <JnpfInputNumber
                      v-model:value="record.F_SortCode"
                      placeholder="排序不可重复"
                      :min="1"
                      :controls="false"
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_GoodsId'">
                    <JnpfSelect
                      v-model:value="record.F_GoodsId"
                      :options="optionsObj.tableField449e6c_F_GoodsIdOptions"
                      :fieldNames="optionsObj.tableField449e6c_F_GoodsIdProps"
                      allowClear
                      showSearch
                      disabled
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_Unit'">
                    <JnpfInputNumber
                      v-model:value="record.F_Unit"
                      placeholder="请输入"
                      :min="1"
                      :controls="false"
                      :style="{
                        width: '100%',
                        borderColor: record.F_Unit > record.F_InventoryNum ? '#ff4d4f' : undefined,
                      }"
                      @change="handleF_UnitChange(record)" />
                    <div v-if="record.F_Unit > record.F_InventoryNum" style="color: #ff4d4f; font-size: 12px; margin-top: 4px">
                      用量不能大于库存数量 {{ record.F_InventoryNum }}
                    </div>
                  </template>
                  <template v-if="column.key === 'F_UnitTwo'">
                    <JnpfInput
                      v-model:value="record.F_UnitTwo"
                      placeholder=""
                      :controls="false"
                      disabled
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_StockInProcess'">
                    <JnpfSelect
                      v-model:value="record.F_StockInProcess"
                      placeholder="请选择"
                      :options="optionsObj.tableField449e6c_F_StockInProcessOptions"
                      :fieldNames="optionsObj.tableField449e6c_F_StockInProcessProps"
                      allowClear
                      showSearch
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_Description'">
                    <JnpfInput
                      v-model:value="record.F_Description"
                      placeholder="请输入"
                      allowClear
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
                      <a-button class="action-btn" type="link" color="error" @click="removetableField449e6cRow(index, true)" size="small">{{
                        t('common.delText', '删除')
                      }}</a-button>
                    </a-space>
                  </template>
                </template>
              </a-table>
              <a-space class="input-table-footer-btn"> </a-space>
            </a-form-item>
          </a-col>
        </a-row>
      </a-form>
    </a-row>
  </BasicModal>
  <SelectModal :config="state.currTableConf" :formData="state.dataForm" ref="selectModal" @select="addForSelect" />
  <GoodsForm ref="goodsFormRef" @submit="onSubmit" />
</template>
<script lang="ts" setup>
  import { create, update, getInfo } from './helper/api';
  import { getBOMGoodsList } from '@/views/Subcode/aBom/helper/api';
  import { getRoutingStepList } from '@/views/Subcode/aProdRoutingStep/helper/api';
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
  import GoodsForm from '@/views/Subcode/aProdProcessing/GoodsForm.vue';

  interface State {
    RoutingStr: string;
    GoodsList: any[];
    ProcessList: any[];
    dataForm: any;
    dataRule: any;
    optionsObj: any;
    currVmodel: any;
    currTableConf: any;
    addTableConf: any;
    tableRows: any;
    tableField0c720eouterActiveKey: number[];
    tableField0c720einnerActiveKey: string[];
    tableField0c720eDefaultExpandAll: boolean;
    selectedtableField0c720eRowKeys: any[];
    tableField449e6couterActiveKey: number[];
    tableField449e6cinnerActiveKey: string[];
    tableField449e6cDefaultExpandAll: boolean;
    selectedtableField449e6cRowKeys: any[];
    title: string;
    continueText: string;
    allList: any[];
    currIndex: number;
    isContinue: boolean;
    submitType: number;
    showContinueBtn: boolean;
  }

  const emit = defineEmits(['reload']);
  const getLeftTreeActiveInfo: (() => any) | null = inject('getLeftTreeActiveInfo', null);
  const userStore = useUserStore();
  const userInfo = userStore.getUserInfo;
  const { createMessage, createConfirm } = useMessage();
  const { t } = useI18n();
  const [registerModal, { openModal, setModalProps }] = useModal();
  const formRef = ref<FormInstance>();
  const goodsFormRef = ref<any>(null);
  const selectModal = ref(null);
  const tableField0c720eColumns: any[] = computed(() => {
    let list = [
      {
        title: '排序',
        dataIndex: 'F_SortCode',
        key: 'F_SortCode',
        tipLabel: '',
        required: true,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '80px',
        fixed: false,
        formP: 'F_SortCode',
        isSystemControl: false,
      },
      {
        title: '工序',
        dataIndex: 'F_ProcessId',
        key: 'F_ProcessId',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '120px',
        fixed: false,
        formP: 'F_ProcessId',
        isSystemControl: false,
      },
      {
        title: '计划开始',
        dataIndex: 'F_PlanStartDate',
        key: 'F_PlanStartDate',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '150px',
        fixed: false,
        formP: 'F_PlanStartDate',
        isSystemControl: false,
      },
      {
        title: '计划结束',
        dataIndex: 'F_PlanEndDate',
        key: 'F_PlanEndDate',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '150px',
        fixed: false,
        formP: 'F_PlanEndDate',
        isSystemControl: false,
      },
      {
        title: '备注',
        dataIndex: 'F_Description',
        key: 'F_Description',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Description',
        isSystemControl: false,
      },
    ];
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
  const gettableField0c720eRowSelection = computed(() => {
    const rowSelection = {
      selectedRowKeys: state.selectedtableField0c720eRowKeys,
      onChange: (selectedRowKeys: string[]) => {
        state.selectedtableField0c720eRowKeys = (selectedRowKeys || []).sort().reverse();
      },
    };
    return rowSelection;
  });
  const tableField449e6cColumns: any[] = computed(() => {
    let list = [
      {
        title: '排序',
        dataIndex: 'F_SortCode',
        key: 'F_SortCode',
        tipLabel: '',
        required: true,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '80px',
        fixed: false,
        formP: 'F_SortCode',
        isSystemControl: false,
      },
      {
        title: '商品',
        dataIndex: 'F_GoodsId',
        key: 'F_GoodsId',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '180px',
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
        width: '100px',
        fixed: false,
        formP: 'F_Unit',
        isSystemControl: false,
      },
      {
        title: '单位',
        dataIndex: 'F_UnitTwo',
        key: 'F_UnitTwo',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '100px',
        fixed: false,
        formP: 'F_UnitTwo',
        isSystemControl: false,
      },
      {
        title: '入库工序',
        dataIndex: 'F_StockInProcess',
        key: 'F_StockInProcess',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '150px',
        fixed: false,
        formP: 'F_StockInProcess',
        isSystemControl: false,
      },
      {
        title: '备注',
        dataIndex: 'F_Description',
        key: 'F_Description',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Description',
        isSystemControl: false,
      },
    ];
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
  const gettableField449e6cRowSelection = computed(() => {
    const rowSelection = {
      selectedRowKeys: state.selectedtableField449e6cRowKeys,
      onChange: (selectedRowKeys: string[]) => {
        state.selectedtableField449e6cRowKeys = (selectedRowKeys || []).sort().reverse();
      },
    };
    return rowSelection;
  });
  const state = reactive<State>({
    RoutingStr: '',
    GoodsList: [],
    ProcessList: [],
    dataForm: {
      id: '',
      F_ProcessNo: undefined,
      F_GoodsId: undefined,
      F_BOMId: undefined,
      F_PlanQty: undefined,
      F_DeliveryDate: undefined,
      F_Priority: undefined,
      F_PlanStartDate: undefined,
      F_PlanEndDate: undefined,
      F_DrawingFile: [],
      F_CustomerName: undefined,
      F_DoorPlateThickness: undefined,
      F_BoxPlateThickness: undefined,
      F_InstallOrBeam: undefined,
      F_LockSet: undefined,
      F_Hinge: undefined,
      F_Color: undefined,
      F_Type: [],
      F_BOMImages: [],
      F_Description: undefined,
      F_ContractId: undefined,
      F_ProjectId: undefined,
      F_ProdPlanId: undefined,
      F_State: undefined,
      F_SequenceNo: undefined,
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
      tableField0c720e: [],
      tableField449e6c: [],
    },
    tableField0c720eouterActiveKey: [0],
    tableField0c720einnerActiveKey: [],
    tableField0c720eDefaultExpandAll: true,
    selectedtableField0c720eRowKeys: [],
    tableField449e6couterActiveKey: [0],
    tableField449e6cinnerActiveKey: [],
    tableField449e6cDefaultExpandAll: true,
    selectedtableField449e6cRowKeys: [],
    dataRule: {
      F_GoodsId: [
        {
          required: true,
          message: '请输入商品',
          trigger: 'change',
        },
      ],
      F_PlanQty: [
        {
          required: true,
          message: '请输入计划数',
          trigger: ['blur', 'change'],
        },
      ],
      F_CustomerName: [
        {
          required: true,
          message: '请输入客户',
          trigger: 'blur',
        },
      ],
      F_DoorPlateThickness: [
        {
          required: true,
          message: '请输入门板厚度',
          trigger: 'change',
        },
      ],
      F_BoxPlateThickness: [
        {
          required: true,
          message: '请输入箱体板厚',
          trigger: 'change',
        },
      ],
      F_InstallOrBeam: [
        {
          required: true,
          message: '请输入安装板或安装梁',
          trigger: 'change',
        },
      ],
      F_LockSet: [
        {
          required: true,
          message: '请输入锁具',
          trigger: 'blur',
        },
      ],
      F_Hinge: [
        {
          required: true,
          message: '请输入铰链',
          trigger: 'blur',
        },
      ],
      F_Color: [
        {
          required: true,
          message: '请输入颜色',
          trigger: 'blur',
        },
      ],
      F_Type: [
        {
          required: true,
          message: '请输入类别',
          trigger: 'change',
        },
      ],
    },
    optionsObj: {
      RoutingStrProps: { label: 'F_RoutingName', value: 'id' },
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
        // {
        //   value: 'F_CategoryId',
        //   label: '商品类别',
        // },
        {
          value: 'F_Unit',
          label: '单位',
        },
        {
          value: 'F_InventoryNum',
          label: '库存',
        },
        {
          value: 'F_InspectRule',
          label: '检验规范',
        },
        {
          value: 'F_Remark',
          label: '备注',
        },
      ],
      GoodsListTemplateJson: [],
      GoodsList: [],
      Process_F_ProcessIdOptions: [
        {
          value: 'F_ProcessName',
          label: '工序',
        },
        {
          value: 'F_ProcessCode',
          label: '工序编号',
        },
        {
          value: 'F_ProdUserIds',
          label: '生产人员',
        },
        {
          value: 'F_QcUserIds',
          label: '质检人员',
        },
        {
          value: 'F_DefectIds',
          label: '不良品项',
        },
        {
          value: 'F_TaskTimer',
          label: '生产任务计时',
        },
        {
          value: 'F_DefectHandleName',
          label: '需返工 / 报废',
        },
        {
          value: 'F_Description',
          label: '备注',
        },
      ],
      Process_F_ProcessIdTemplateJson: [],
      F_GoodsIdOptions: [
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
        // {
        //   value: 'F_CategoryId',
        //   label: '商品类别',
        // },
        {
          value: 'F_Unit',
          label: '单位',
        },
        {
          value: 'F_InspectRule',
          label: '检验规范',
        },
        {
          value: 'F_Remark',
          label: '备注',
        },
      ],
      F_GoodsIdTemplateJson: [
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
      F_BOMIdProps: { label: 'F_BomName', value: 'id' },
      F_PriorityProps: { label: 'fullName', value: 'enCode' },
      F_DoorPlateThicknessProps: { label: 'fullName', value: 'enCode' },
      F_BoxPlateThicknessProps: { label: 'fullName', value: 'enCode' },
      F_InstallOrBeamProps: { label: 'fullName', value: 'enCode' },
      F_TypeProps: {
        label: 'F_Name',
        value: 'F_Id',
        children: 'children',
      },
      F_ContractIdProps: { label: 'F_GoodsName', value: 'id' },
      F_ProjectIdProps: { label: 'F_ProjectName', value: 'id' },
      F_ProdPlanIdProps: { label: 'F_PlanName', value: 'id' },
      F_StateProps: { label: 'fullName', value: 'enCode' },
      tableField0c720e_F_ProcessIdOptions: [],
      tableField0c720e_F_ProcessIdProps: { label: 'F_ProcessName', value: 'id' },
      tableField449e6c_F_GoodsIdOptions: [],
      tableField449e6c_F_GoodsIdProps: { label: 'F_GoodsName', value: 'id' },
      tableField449e6c_F_StockInProcessOptions: [],
      tableField449e6c_F_StockInProcessProps: { label: 'F_ProcessName', value: 'id' },
      F_GoodsId: [],
    },
    currVmodel: '',
    currTableConf: {},
    tableRows: {
      tableField0c720e: {
        enabledmark: undefined,
        F_SortCode: undefined,
        F_ProcessId: undefined,
        F_PlanStartDate: undefined,
        F_PlanEndDate: undefined,
        F_Description: undefined,
        F_CreatorTime: undefined,
        tableField3b6f08: [],
      },
      tableField449e6c: {
        enabledmark: undefined,
        F_SortCode: undefined,
        F_GoodsId: undefined,
        F_Unit: undefined,
        F_StockInProcess: undefined,
        F_Description: undefined,
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
  });
  const { title, continueText, showContinueBtn, dataForm, submitType, dataRule, optionsObj } = toRefs(state);

  const getPrevDisabled = computed(() => state.currIndex === 0);
  const getNextDisabled = computed(() => state.currIndex === state.allList.length - 1);

  defineExpose({ init });

  function init(data) {
    state.submitType = 0;
    state.showContinueBtn = true;
    state.title = !data.id ? '新增' : '编辑';
    setFormProps({ continueLoading: false });
    state.dataForm.id = data.id;
    if (data.F_GoodsId) {
      state.dataForm.F_GoodsId = data.F_GoodsId;
      state.dataForm.F_PlanQty = data.F_Num;
      state.dataForm.F_BOMId = undefined;
      getF_BOMIdOptions();
      state.title = data.fullName;
    } else {
      state.dataForm.F_GoodsId = undefined;
      state.dataForm.F_PlanQty = undefined;
    }
    openModal();
    nextTick(() => {
      getForm().resetFields();
      state.dataForm.tableField0c720e = [];
      state.dataForm.tableField449e6c = [];
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
        F_ProcessNo: undefined,
        F_GoodsId: state.dataForm.F_GoodsId,
        F_BOMId: undefined,
        F_PlanQty: state.dataForm.F_PlanQty,
        F_DeliveryDate: undefined,
        F_Priority: undefined,
        F_PlanStartDate: undefined,
        F_PlanEndDate: undefined,
        F_DrawingFile: [],
        F_CustomerName: undefined,
        F_DoorPlateThickness: undefined,
        F_BoxPlateThickness: undefined,
        F_InstallOrBeam: undefined,
        F_LockSet: undefined,
        F_Hinge: undefined,
        F_Color: undefined,
        F_Type: [],
        F_BOMImages: [],
        F_Description: undefined,
        F_ContractId: undefined,
        F_ProjectId: undefined,
        F_ProdPlanId: undefined,
        F_State: undefined,
        F_SequenceNo: undefined,
        F_CreatorUserId: undefined,
        F_CreatorTime: undefined,
        tableField0c720e: [],
        tableField449e6c: [],
      };
      state.GoodsList = [];
      state.ProcessList = [];
      state.RoutingStr = '';
      getRoutingStrOptions();
      getF_BOMIdOptions();
      getF_PriorityOptions();
      getF_DoorPlateThicknessOptions();
      getF_BoxPlateThicknessOptions();
      getF_InstallOrBeamOptions();
      getF_TypeOptions();
      getF_ContractIdOptions();
      getF_ProjectIdOptions();
      getF_ProdPlanIdOptions();
      getF_StateOptions();
      gettableField0c720e_F_ProcessIdOptions();
      gettableField449e6c_F_GoodsIdOptions();
      gettableField449e6c_F_StockInProcessOptions();
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

  // 物料清单
  function goodsHandle(record) {
    // 不带工作流
    const data = {
      ...record,
      jnpfId: record.jnpfId,
    };
    goodsFormRef.value?.init(data);
  }
  // 监听 submit 事件
  function onSubmit(returnRow) {
    // returnRow 就是 StepForm 里 emit 出来的整行数据
    const idx = state.dataForm.tableField0c720e.findIndex(item => item.jnpfId === returnRow.jnpfId);
    if (idx > -1) {
      // 整行替换，保持响应式
      state.dataForm.tableField0c720e[idx].tableField0bb944 = returnRow.tableField0bb944;
    }
  }

  function getData(id) {
    getInfo(id).then(res => {
      state.dataForm = res.data || {};
      state.RoutingStr = '';
      state.ProcessList = [];
      state.GoodsList = [];
      for (let i = 0; i < state.dataForm.tableField0c720e.length; i++) {
        const element = state.dataForm.tableField0c720e[i];
        element.jnpfId = buildUUID();
        state.ProcessList.push(state.dataForm.tableField0c720e[i].F_ProcessId);
      }
      for (let i = 0; i < state.dataForm.tableField449e6c.length; i++) {
        const element = state.dataForm.tableField449e6c[i];
        element.jnpfId = buildUUID();
        state.GoodsList.push(state.dataForm.tableField449e6c[i].F_GoodsId);
      }
      getRoutingStrOptions();
      getF_BOMIdOptions();
      getF_PriorityOptions();
      getF_DoorPlateThicknessOptions();
      getF_BoxPlateThicknessOptions();
      getF_InstallOrBeamOptions();
      getF_TypeOptions();
      getF_ContractIdOptions();
      getF_ProjectIdOptions();
      getF_ProdPlanIdOptions();
      getF_StateOptions();
      gettableField0c720e_F_ProcessIdOptions();
      gettableField449e6c_F_GoodsIdOptions();
      gettableField449e6c_F_StockInProcessOptions();
      changeLoading(false);
    });
  }
  async function handleSubmit() {
    try {
      const values = await getForm()?.validate();
      if (!values) return;
      if (!tableField0c720eExist()) return;
      if (!tableField449e6cExist()) return;

      // 验证单位用量不能大于库存数量
      const invalidRows = state.dataForm.tableField449e6c.filter(item => item.F_Unit && item.F_InventoryNum && item.F_Unit > item.F_InventoryNum);
      if (invalidRows.length > 0) {
        const errorMsg = invalidRows
          .map(item => {
            const goods = optionsObj.tableField449e6c_F_GoodsIdOptions.find(g => g.id === item.F_GoodsId);
            const goodsName = goods ? goods.F_GoodsName : item.F_GoodsId;
            return `${goodsName}：用量 ${item.F_Unit} 大于库存 ${item.F_InventoryNum}`;
          })
          .join('；');
        createMessage.error(`用料清单验证失败：${errorMsg}`);
        return false;
      }

      setFormProps({ confirmLoading: true });

      const sortCodes = state.dataForm.tableField0c720e.map(m => m.F_SortCode);
      const duplicates = sortCodes.filter((code, idx, arr) => arr.indexOf(code) !== idx);
      if (duplicates.length) {
        createMessage.error(`生产任务--排序号重复：${[...new Set(duplicates)].join('、')}`);
        return false; // 阻断后续提交
      }

      const sortCodesGood = state.dataForm.tableField449e6c.map(m => m.F_SortCode);
      const duplicatesGood = sortCodesGood.filter((code, idx, arr) => arr.indexOf(code) !== idx);
      if (duplicatesGood.length) {
        createMessage.error(`用料清单--排序号重复：${[...new Set(duplicatesGood)].join('、')}`);
        return false; // 阻断后续提交
      }

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
  function getRoutingStrOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2013852230570086400', query).then(res => {
      let data = res.data;
      state.optionsObj.RoutingStrOptions = Array.isArray(data) ? data : [];
    });
  }
  function getF_BOMIdOptions() {
    let templateJson = [
      {
        defaultValue: '',
        field: 'goodsId',
        dataType: 'varchar',
        required: 0,
        fieldName: '合同ID',
        relationField: 'F_GoodsId',
        jnpfKey: 'popupSelect',
        complexHeaderList: null,
        sourceType: 1,
        isChildren: false,
        IsMultiple: false,
      },
    ];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2013188646957617152', query).then(res => {
      let data = res.data;
      state.optionsObj.F_BOMIdOptions = Array.isArray(data) ? data : [];
    });
  }
  function getF_PriorityOptions() {
    getDictionaryDataSelector('2013182853290004480').then(res => {
      state.optionsObj.F_PriorityOptions = res.data.list;
    });
  }
  function getF_DoorPlateThicknessOptions() {
    getDictionaryDataSelector('2013183327061807104').then(res => {
      state.optionsObj.F_DoorPlateThicknessOptions = res.data.list;
    });
  }
  function getF_BoxPlateThicknessOptions() {
    getDictionaryDataSelector('2013183327061807104').then(res => {
      state.optionsObj.F_BoxPlateThicknessOptions = res.data.list;
    });
  }
  function getF_InstallOrBeamOptions() {
    getDictionaryDataSelector('2013183327061807104').then(res => {
      state.optionsObj.F_InstallOrBeamOptions = res.data.list;
    });
  }
  function getF_TypeOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2008414575283802112', query).then(res => {
      let data = res.data;
      state.optionsObj.F_TypeOptions = Array.isArray(data) ? data : [];
    });
  }
  function getF_ContractIdOptions() {
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
    getDataInterfaceRes('2012085692393459712', query).then(res => {
      let data = res.data;
      state.optionsObj.F_ContractIdOptions = Array.isArray(data) ? data : [];
    });
  }
  function getF_ProjectIdOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2013822233163730944', query).then(res => {
      let data = res.data;
      state.optionsObj.F_ProjectIdOptions = Array.isArray(data) ? data : [];
    });
  }
  function getF_ProdPlanIdOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2013822515994038272', query).then(res => {
      let data = res.data;
      state.optionsObj.F_ProdPlanIdOptions = Array.isArray(data) ? data : [];
    });
  }
  function getF_StateOptions() {
    getDictionaryDataSelector('2013819135649255424').then(res => {
      state.optionsObj.F_StateOptions = res.data.list;
    });
  }
  function gettableField0c720e_F_ProcessIdOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2012092092830060544', query).then(res => {
      let data = res.data;
      state.optionsObj.tableField0c720e_F_ProcessIdOptions = Array.isArray(data) ? data : [];
    });
  }
  function gettableField449e6c_F_GoodsIdOptions() {
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
    getDataInterfaceRes('2012085692393459712', query).then(res => {
      let data = res.data;
      state.optionsObj.tableField449e6c_F_GoodsIdOptions = Array.isArray(data) ? data : [];
    });
  }
  function gettableField449e6c_F_StockInProcessOptions() {
    let templateJson = [];
    let query = {
      paramList: getParamList(templateJson, state.dataForm),
    };
    getDataInterfaceRes('2012092092830060544', query).then(res => {
      let data = res.data;
      state.optionsObj.tableField449e6c_F_StockInProcessOptions = Array.isArray(data) ? data : [];
    });
  }
  function removetableField0c720eRow(index, showConfirm = false) {
    if (showConfirm) {
      createConfirm({
        iconType: 'warning',
        title: '提示',
        content: '此操作将永久删除该数据, 是否继续?',
        onOk: () => {
          state.dataForm.tableField0c720e.splice(index, 1);
          state.ProcessList.splice(index, 1);
        },
      });
    } else {
      state.dataForm.tableField0c720e.splice(index, 1);
      state.ProcessList.splice(index, 1);
    }
  }
  function tableField0c720eExist() {
    let isOk = true;
    for (let i = 0; i < state.dataForm.tableField0c720e.length; i++) {
      const e = state.dataForm.tableField0c720e[i];
      if (!e.F_SortCode) {
        createMessage.error('排序' + t('sys.validate.textRequiredSuffix'));
        isOk = false;
        break;
      }
    }
    return isOk;
  }
  function removetableField449e6cRow(index, showConfirm = false) {
    if (showConfirm) {
      createConfirm({
        iconType: 'warning',
        title: '提示',
        content: '此操作将永久删除该数据, 是否继续?',
        onOk: () => {
          state.dataForm.tableField449e6c.splice(index, 1);
          state.GoodsList.splice(index, 1);
        },
      });
    } else {
      state.dataForm.tableField449e6c.splice(index, 1);
      state.GoodsList.splice(index, 1);
    }
  }
  function tableField449e6cExist() {
    let isOk = true;
    for (let i = 0; i < state.dataForm.tableField449e6c.length; i++) {
      const e = state.dataForm.tableField449e6c[i];
      if (!e.F_SortCode) {
        createMessage.error('排序' + t('sys.validate.textRequiredSuffix'));
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

  function ProcessListBtn(val, option) {
    /* 兜底 */
    if (!Array.isArray(state.dataForm.tableField0c720e)) {
      state.dataForm.tableField0c720e = [];
    }
    const optionIdSet = new Set(option.map(o => o.id));

    /* 1. 删掉已取消勾选的行 */
    state.dataForm.tableField0c720e = state.dataForm.tableField0c720e.filter(item => optionIdSet.has(item.F_ProcessId));

    /* 2. 计算当前最大序号（0 表示还没有任何行） */
    const maxSort = state.dataForm.tableField0c720e.reduce((max, item) => {
      return item.F_SortCode > max ? item.F_SortCode : max;
    }, 0);

    /* 3. 追加本地还没有的行，序号依次累加 */
    let nextSort = maxSort + 1;
    option.forEach(o => {
      const exist = state.dataForm.tableField0c720e.some(item => item.F_ProcessId === o.id);
      if (!exist) {
        const newRow = {
          jnpfId: buildUUID(),
          F_ProcessId: o.id,
          F_PlanStartDate: undefined,
          F_PlanEndDate: undefined,
          F_Description: undefined,
          F_CreatorTime: undefined,
          F_SortCode: nextSort, // ← 自动累加
        };
        state.dataForm.tableField0c720e.push(newRow);
        state.tableField0c720einnerActiveKey.push(newRow.jnpfId);
        nextSort++; // 下一行继续 +1
      }
    });
  }

  function GoodsListBtn(val, option) {
    /* 兜底 */
    if (!Array.isArray(state.dataForm.tableField449e6c)) {
      state.dataForm.tableField449e6c = [];
    }
    const optionIdSet = new Set(option.map(o => o.id));

    /* 1. 删掉已取消勾选的行 */
    state.dataForm.tableField449e6c = state.dataForm.tableField449e6c.filter(item => optionIdSet.has(item.F_GoodsId));

    /* 2. 计算当前最大序号（0 表示还没有任何行） */
    const maxSort = state.dataForm.tableField449e6c.reduce((max, item) => {
      return item.F_SortCode > max ? item.F_SortCode : max;
    }, 0);

    /* 3. 追加本地还没有的行，序号依次累加 */
    let nextSort = maxSort + 1;
    option.forEach(o => {
      const exist = state.dataForm.tableField449e6c.some(item => item.F_GoodsId === o.id);
      if (!exist) {
        const newRow = {
          jnpfId: buildUUID(),
          F_GoodsId: o.id,
          F_UnitTwo: o.F_Unit,
          F_Unit: 1,
          F_InventoryNum: o.F_InventoryNum,
          F_StockInProcess: undefined,
          F_Description: undefined,
          F_CreatorTime: undefined,
          F_SortCode: nextSort, // ← 自动累加
        };
        state.dataForm.tableField449e6c.push(newRow);
        state.tableField449e6cinnerActiveKey.push(newRow.jnpfId);
        nextSort++; // 下一行继续 +1
      }
    });
  }

  function F_GoodsIdChange(val, option) {
    state.dataForm.F_BOMId = undefined;
    getF_BOMIdOptions();
  }

  // 处理单位用量变化
  function handleF_UnitChange(record) {
    // 触发响应式更新,确保错误提示及时显示
    const index = state.dataForm.tableField449e6c.findIndex(item => item.jnpfId === record.jnpfId);
    if (index > -1) {
      state.dataForm.tableField449e6c[index] = { ...record };
    }
  }
  function F_BomChange(val, option) {
    getBOMGoodsList(val).then(res => {
      state.dataForm.tableField449e6c = [];
      state.GoodsList = [];

      let nextSort = 1;
      res.data.forEach(o => {
        const newRow = {
          jnpfId: buildUUID(),
          F_GoodsId: o.F_GoodsId,
          F_UnitTwo: o.F_UnitTwo,
          F_Unit: o.F_Unit,
          F_InventoryNum: o.F_InventoryNum,
          F_StockInProcess: undefined,
          F_Description: undefined,
          F_CreatorTime: undefined,
          F_SortCode: nextSort, // ← 自动累加
        };
        state.dataForm.tableField449e6c.push(newRow);
        state.tableField449e6cinnerActiveKey.push(newRow.jnpfId);
        nextSort++; // 下一行继续 +1
        state.GoodsList.push(o.F_GoodsId);
      });
    });
  }
  function RoutingStrBtn(val, option) {
    if (val != '' && val != null) {
      getRoutingStepList(val).then(res => {
        state.dataForm.tableField0c720e = [];
        state.ProcessList = [];

        let nextSort = 1;
        res.data.forEach(o => {
          const newRow = {
            jnpfId: buildUUID(),
            F_ProcessId: o.F_ProcessId,
            F_PlanStartDate: undefined,
            F_PlanEndDate: undefined,
            F_Description: undefined,
            F_CreatorTime: undefined,
            F_SortCode: nextSort, // ← 自动累加
          };
          state.dataForm.tableField0c720e.push(newRow);
          state.tableField0c720einnerActiveKey.push(newRow.jnpfId);
          nextSort++; // 下一行继续 +1
          state.ProcessList.push(o.F_ProcessId);
        });
      });
    }
  }
</script>
