<template>
  <BasicModal v-bind="$attrs" @register="registerModal" :title="title" width="1100px" :minHeight="100" :showOkBtn="false" :canFullscreen="true">
    <template #insertFooter> </template>
    <a-row class="dynamic-form">
      <a-form :colon="false" layout="horizontal" labelAlign="right" :labelCol="{ style: { width: '100px' } }" :model="dataForm" ref="formRef">
        <a-row :gutter="15">
          <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_ProcessingId" :labelCol="{ style: { width: '100px' } }">
              <template #label>加工单：</template>
              <p>{{ dataForm.F_ProcessingId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_ProcessName" :labelCol="{ style: { width: '100px' } }">
              <template #label>工序：</template>
              <p>{{ dataForm.F_ProcessName }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_ProcessCode" :labelCol="{ style: { width: '100px' } }">
              <template #label>工序编号：</template>
              <p>{{ dataForm.F_ProcessCode }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_IsQc" :labelCol="{ style: { width: '100px' } }">
              <template #label>是否需要质检：</template>
              <p>{{ dataForm.F_IsQc }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_WorkshopId" :labelCol="{ style: { width: '100px' } }">
              <template #label>车间：</template>
              <p>{{ dataForm.F_WorkshopId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_Line" :labelCol="{ style: { width: '100px' } }">
              <template #label>产线：</template>
              <p>{{ dataForm.F_Line }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_ReportUnit" :labelCol="{ style: { width: '100px' } }">
              <template #label>报工单位：</template>
              <p>{{ dataForm.F_ReportUnit }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_UnitRatio" :labelCol="{ style: { width: '100px' } }">
              <template #label>单位关系：</template>
              <p>{{ dataForm.F_UnitRatio }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_PriceType" :labelCol="{ style: { width: '100px' } }">
              <template #label>计价方式：</template>
              <p>{{ dataForm.F_PriceType }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_WagePrice" :labelCol="{ style: { width: '100px' } }">
              <template #label>工资单价：</template>
              <p>{{ dataForm.F_WagePrice }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_StdHour" :labelCol="{ style: { width: '100px' } }">
              <template #label>标准工时：</template>
              <p>{{ dataForm.F_StdHour }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_MachineId" :labelCol="{ style: { width: '100px' } }">
              <template #label>机台：</template>
              <p>{{ dataForm.F_MachineId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_PlanStartDate" :labelCol="{ style: { width: '100px' } }">
              <template #label>计划开始：</template>
              <p>{{ dataForm.F_PlanStartDate }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_PlanEndDate" :labelCol="{ style: { width: '100px' } }">
              <template #label>计划结束：</template>
              <p>{{ dataForm.F_PlanEndDate }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_TaskType" :labelCol="{ style: { width: '100px' } }">
              <template #label>任务类型：</template>
              <p>{{ dataForm.F_TaskType }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_ProdUserIds" :labelCol="{ style: { width: '100px' } }">
              <template #label>生产人员：</template>
              <p>{{ dataForm.F_ProdUserIds }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_ProdDispatch" :labelCol="{ style: { width: '100px' } }">
              <template #label>生产派工：</template>
              <p>{{ dataForm.F_ProdDispatch }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_QcUserIds" :labelCol="{ style: { width: '100px' } }">
              <template #label>质检人员：</template>
              <p>{{ dataForm.F_QcUserIds }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_QcDispatch" :labelCol="{ style: { width: '100px' } }">
              <template #label>质检派工：</template>
              <p>{{ dataForm.F_QcDispatch }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_DefectIds" :labelCol="{ style: { width: '100px' } }">
              <template #label>不良品项：</template>
              <p>{{ dataForm.F_DefectIds }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_ProdQty" :labelCol="{ style: { width: '100px' } }">
              <template #label>生产数量：</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_ProdQty"
                placeholder="请输入"
                :controls="false"
                :style="{
                  width: '100%',
                }"
                disabled
                detailed />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_TaskStatus" :labelCol="{ style: { width: '100px' } }">
              <template #label>任务状态：</template>
              <p>{{ dataForm.F_TaskStatus }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item">
            <a-form-item name="F_SortCode" :labelCol="{ style: { width: '100px' } }">
              <template #label>排序：</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_SortCode"
                placeholder="请输入"
                :controls="false"
                :style="{
                  width: '100%',
                }"
                disabled
                detailed />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item">
            <a-form-item name="F_Description" :labelCol="{ style: { width: '100px' } }">
              <template #label>备注：</template>
              <p>{{ dataForm.F_Description }}</p>
            </a-form-item>
          </a-col>

          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="物料清单" :bordered="false" />
              <a-table
                :data-source="dataForm.tableField0bb944"
                :columns="tableField0bb944Columns"
                size="small"
                :pagination="false"
                :scroll="{ x: 'max-content' }">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                  <template v-if="column.key === 'F_GoodsId'">
                    <p>{{ record.F_GoodsId }}</p>
                  </template>
                  <template v-if="column.key === 'F_GoodsNo'">
                    <p>{{ record.F_GoodsNo }}</p>
                  </template>
                  <template v-if="column.key === 'F_Specification'">
                    <p>{{ record.F_Specification }}</p>
                  </template>
                  <template v-if="column.key === 'F_Num'">
                    <JnpfInputNumber
                      v-model:value="record.F_Num"
                      placeholder="请输入"
                      :controls="false"
                      :style="{
                        width: '100%',
                      }"
                      disabled
                      detailed />
                  </template>
                  <template v-if="column.key === 'F_UnitTwo'">
                    <p>{{ record.F_UnitTwo }}</p>
                  </template>
                  <template v-if="column.key === 'F_Description'">
                    <JnpfInput
                      v-model:value="record.F_Description"
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

  interface State {
    dataForm: any;
    interfaceRes: any;
    extraOptions: any;
    extraData: any;
    title: string;
    tableField0bb944outerActiveKey: number[];
    tableField0bb944innerActiveKey: string[];
  }

  defineOptions({ name: 'Detail' });
  const [registerModal, { openModal, closeModal, setModalProps }] = useModal();
  const { t } = useI18n();
  const tableField0bb944Columns: any[] = computed(() => {
    let list = [
      {
        title: '商品',
        dataIndex: 'F_GoodsId',
        key: 'F_GoodsId',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '150px',
        formP: 'F_GoodsId',
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
        width: '150px',
        formP: 'F_GoodsNo',
        fixed: false,
      },
      {
        title: '商品规格',
        dataIndex: 'F_Specification',
        key: 'F_Specification',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '100px',
        formP: 'F_Specification',
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
        width: '80px',
        formP: 'F_Num',
        fixed: false,
      },
      {
        title: '单位',
        dataIndex: 'F_UnitTwo',
        key: 'F_UnitTwo',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '80px',
        formP: 'F_UnitTwo',
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
    tableField0bb944outerActiveKey: [0],
    tableField0bb944innerActiveKey: [],
  });
  const { title, dataForm } = toRefs(state);

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

      for (let i = 0; i < state.dataForm.tableField0bb944.length; i++) {
        const element = state.dataForm.tableField0bb944[i];
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
