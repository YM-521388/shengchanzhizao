<template>
  <BasicModal v-bind="$attrs" @register="registerModal" :title="title" width="1200px" :minHeight="100" :showOkBtn="false" :canFullscreen="true">
    <template #insertFooter> </template>
    <a-row class="dynamic-form">
      <a-form :colon="false" layout="horizontal" labelAlign="right" :labelCol="{ style: { width: '100px' } }" :model="dataForm" ref="formRef">
        <a-row :gutter="15">
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_ProcessName')">
            <a-form-item name="F_ProcessName" :labelCol="{ style: { width: '100px' } }">
              <template #label>工序名：</template>
              <JnpfInput
                v-model:value="dataForm.F_ProcessName"
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
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_ProcessCode')">
            <a-form-item name="F_ProcessCode" :labelCol="{ style: { width: '100px' } }">
              <template #label>工序编号：</template>
              <JnpfInput
                v-model:value="dataForm.F_ProcessCode"
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
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_WorkshopId')">
            <a-form-item name="F_WorkshopId" :labelCol="{ style: { width: '100px' } }">
              <template #label>车间：</template>
              <p>{{ dataForm.F_WorkshopId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_Line')">
            <a-form-item name="F_Line" :labelCol="{ style: { width: '100px' } }">
              <template #label>产线：</template>
              <p>{{ dataForm.F_Line }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_ReportUnit')">
            <a-form-item name="F_ReportUnit" :labelCol="{ style: { width: '100px' } }">
              <template #label>报工单位：</template>
              <p>{{ dataForm.F_ReportUnit }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_UnitRatio')">
            <a-form-item name="F_UnitRatio" :labelCol="{ style: { width: '100px' } }">
              <template #label>单位关系：</template>
              <JnpfInput
                v-model:value="dataForm.F_UnitRatio"
                placeholder="请输入"
                :controls="false"
                :style="{
                  width: '100%',
                }"
                disabled
                detailed />
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_PriceType')">
            <a-form-item name="F_PriceType" :labelCol="{ style: { width: '100px' } }">
              <template #label>计价方式：</template>
              <p>{{ dataForm.F_PriceType }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_WagePrice')">
            <a-form-item name="F_WagePrice" :labelCol="{ style: { width: '100px' } }">
              <template #label>工资单价：</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_WagePrice"
                placeholder="请输入"
                :controls="false"
                :style="{
                  width: '100%',
                }"
                disabled
                detailed />
            </a-form-item>
          </a-col>
          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_StdHour')">
            <a-form-item name="F_StdHour" :labelCol="{ style: { width: '100px' } }">
              <template #label>标准工时：</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_StdHour"
                placeholder="请输入"
                addonAfter="小时"
                :controls="false"
                :style="{
                  width: '100%',
                }"
                disabled
                detailed />
            </a-form-item>
          </a-col>

          <a-col :span="6" class="ant-col-item" v-if="hasFormP('F_State')">
            <a-form-item name="F_State" :labelCol="{ style: { width: '100px' } }">
              <template #label>启用状态：</template>
              <p>{{ dataForm.F_State }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_Files')">
            <a-form-item name="F_Files" :labelCol="{ style: { width: '100px' } }">
              <template #label>附件：</template>
              <JnpfUploadFile
                v-model:value="dataForm.F_Files"
                buttonText="点击上传"
                pathType="selfPath"
                :isAccount="-1"
                folder=""
                :fileSize="10"
                sizeUnit="MB"
                :limit="3"
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

          <a-col :span="24" class="ant-col-item">
            <a-tabs v-model:activeKey="state.active1" type="" tabPosition="top" class="mb-20">
              <a-tab-pane tab="生产信息" key="1" forceRender>
                <a-row>
                  <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_MachineId')">
                    <a-form-item name="F_MachineId" :labelCol="{ style: { width: '100px' } }">
                      <template #label>机台：</template>
                      <p class="leading-32px">{{ dataForm.F_MachineId }}</p>
                      <ExtraRelationInfo
                        :extraOptions="state.extraOptions.F_MachineId"
                        :data="state.extraData.F_MachineId"
                        v-if="state.extraOptions.F_MachineId?.length && state.extraData.F_MachineId && JSON.stringify(state.extraData.F_MachineId) !== '{}'" />
                    </a-form-item>
                  </a-col>
                  <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_ProdUserIds')">
                    <a-form-item name="F_ProdUserIds" :labelCol="{ style: { width: '100px' } }">
                      <template #label>生产人员：</template>
                      <p>{{ dataForm.F_ProdUserIds }}</p>
                    </a-form-item>
                  </a-col>
                  <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_DefectIds')">
                    <a-form-item name="F_DefectIds" :labelCol="{ style: { width: '100px' } }">
                      <template #label>不良品项：</template>
                      <p>{{ dataForm.F_DefectIds }}</p>
                    </a-form-item>
                  </a-col>
                </a-row>
              </a-tab-pane>
              <a-tab-pane tab="质检/外协信息" key="2" forceRender>
                <a-row>
                  <!-- <a-col :span="24" class="ant-col-item">
                    <a-form-item :labelCol="{ style: { width: '100px' } }">
                      <JnpfText
                        content="开启配置后工序默认为外协工序，外协工序无法报工和质检。"
                        :textStyle="{
                          color: '#F20808',
                          'text-align': 'left',
                          'font-weight': 'bold',
                          'font-style': 'normal',
                          'text-decoration': 'none',
                          'line-height': 20,
                          'font-size': 14,
                        }" />
                    </a-form-item>
                  </a-col> -->
                  <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_IsOutsource')">
                    <a-form-item name="F_IsOutsource" :labelCol="{ style: { width: '100px' } }">
                      <template #label>工序需外协：</template>
                      <p>{{ dataForm.F_IsOutsource }}</p>
                    </a-form-item>
                  </a-col>
                  <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_IsQc')">
                    <a-form-item name="F_IsQc" :labelCol="{ style: { width: '100px' } }">
                      <template #label>工序需质检：</template>
                      <p>{{ dataForm.F_IsQc }}</p>
                    </a-form-item>
                  </a-col>
                  <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_DefectHandle')">
                    <a-form-item name="F_DefectHandle" :labelCol="{ style: { width: '100px' } }">
                      <template #label>不良品需报废、返工：</template>
                      <p>{{ dataForm.F_DefectHandle }}</p>
                    </a-form-item>
                  </a-col>
                  <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_QcUserIds')">
                    <a-form-item name="F_QcUserIds" :labelCol="{ style: { width: '100px' } }">
                      <template #label>质检人员：</template>
                      <p>{{ dataForm.F_QcUserIds }}</p>
                    </a-form-item>
                  </a-col>
                </a-row>
              </a-tab-pane>
              <!-- <a-tab-pane tab="工序打卡设置" key="2585b9" forceRender>
                <a-row>
                  <a-col :span="24" class="ant-col-item">
                    <a-form-item :labelCol="{ style: { width: '100px' } }">
                      <JnpfText
                        content="开启后可以通过打卡的方式记录生产的总用时。"
                        :textStyle="{
                          color: '#F20808',
                          'text-align': 'left',
                          'font-weight': 'bold',
                          'font-style': 'normal',
                          'text-decoration': 'none',
                          'line-height': 20,
                          'font-size': 14,
                        }" />
                    </a-form-item>
                  </a-col>
                  <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_TaskTimer')">
                    <a-form-item name="F_TaskTimer" :labelCol="{ style: { width: '100px' } }">
                      <template #label>生产任务计时：</template>
                      <p>{{ dataForm.F_TaskTimer }}</p>
                    </a-form-item>
                  </a-col>
                </a-row>
              </a-tab-pane> -->
            </a-tabs>
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
    active1: any;
  }

  defineOptions({ name: 'Detail' });
  const [registerModal, { openModal, closeModal, setModalProps }] = useModal();
  const { t } = useI18n();

  const relationDetailRef = ref<any>(null);
  const state = reactive<State>({
    dataForm: {},
    interfaceRes: {},
    extraOptions: {
      F_MachineId: [],
    },
    extraData: {
      F_MachineId: {},
    },
    title: '详情',
    active1: '1',
  });
  const { title, dataForm } = toRefs(state);
  const { hasFormP } = usePermission();

  defineExpose({ init });

  function init(data) {
    state.dataForm.id = data.id;
    state.active1 = '1';
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
      state.dataForm.F_UnitRatio = state.dataForm.F_UnitRatio + state.dataForm.F_ReportUnit;
      nextTick(() => {
        changeLoading(false);
        getF_MachineIdExtraInfo();
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

  function getF_MachineIdExtraInfo() {
    if (!state.dataForm.F_MachineId_id) return;
    const paramList = getParamList('F_MachineId');
    const query = {
      ids: [state.dataForm.F_MachineId_id],
      interfaceId: '2011729284707782656',
      propsValue: 'id',
      relationField: 'F_MachineName',
      paramList,
    };
    getDataInterfaceDataInfoByIds('2011729284707782656', query).then(res => {
      const data = res.data && res.data.length ? res.data[0] : {};
      state.extraData.F_MachineId = data;
    });
  }
</script>
