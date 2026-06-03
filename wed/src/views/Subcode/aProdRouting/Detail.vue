<template>
  <BasicModal v-bind="$attrs" @register="registerModal" :title="title" width="1000px" :minHeight="100" :showOkBtn="false">
    <template #insertFooter>
    </template>
    <a-row class="dynamic-form ">
      <a-form :colon="false" layout="horizontal" labelAlign="right"  :labelCol="{ style: { width: '100px' } }" :model="dataForm" ref="formRef">
        <a-row :gutter="15">
          <a-col :span="24" class="ant-col-item" >
            <a-form-item name="F_RoutingNo" :labelCol="{ style: { width: '100px' } }">
              <template #label>工艺路线编号</template>
              <JnpfInput v-model:value="dataForm.F_RoutingNo" :maskConfig='{
  "filler": "*",
  "maskType": 1,
  "prefixType": 1,
  "prefixLimit": 0,
  "prefixSpecifyChar": "",
  "suffixType": 1,
  "suffixLimit": 0,
  "suffixSpecifyChar": "",
  "ignoreChar": "",
  "useUnrealMask": false,
  "unrealMaskLength": 1
}' disabled detailed />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" >
            <a-form-item name="F_RoutingName" :labelCol="{ style: { width: '100px' } }">
              <template #label>工艺路线名称</template>
              <JnpfInput v-model:value="dataForm.F_RoutingName" :maskConfig='{
  "filler": "*",
  "maskType": 1,
  "prefixType": 1,
  "prefixLimit": 0,
  "prefixSpecifyChar": "",
  "suffixType": 1,
  "suffixLimit": 0,
  "suffixSpecifyChar": "",
  "ignoreChar": "",
  "useUnrealMask": false,
  "unrealMaskLength": 1
}' disabled detailed />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" v-show="false" >
            <a-form-item name="F_CreatorUserId" :labelCol="{ style: { width: '100px' } }">
              <template #label>创建用户</template>
              <p>{{ dataForm.F_CreatorUserId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" v-show="false" >
            <a-form-item name="F_CreatorTime" :labelCol="{ style: { width: '100px' } }">
              <template #label>创建时间</template>
              <p>{{ dataForm.F_CreatorTime }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="工序" :bordered="false" />
              <a-table :data-source="dataForm.tableField7b5631" :columns="tableField7b5631Columns" size="small" :pagination="false" :scroll="{ x: 'max-content' }" >
                <template #headerCell="{ column }"> <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel" /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                  <template v-if="column.key === 'F_ProcessId'">
                    <p class="leading-32px">{{ record.F_ProcessId }}</p>
	                <ExtraRelationInfo :extraOptions="state.extraOptions.F_ProcessId" :data="state.extraData.F_ProcessId" v-if="state.extraOptions.F_ProcessId?.length && state.extraData.F_ProcessId && JSON.stringify(state.extraData.F_ProcessId) !== '{}'" />
                  </template>
                  <template v-if="column.key === 'F_TaskTimer'">
                    <p>{{ record.F_TaskTimer }}</p>
                  </template>
                  <template v-if="column.key === 'F_SortCode'">
                    <JnpfInputNumber v-model:value="record.F_SortCode" placeholder='请输入' :controls="false" :style='{
  "width": "100%"
}' disabled detailed />
                  </template>
                  <template v-if="column.key === 'F_CreatorTime'">
                    <p>{{ record.F_CreatorTime }}</p>
                  </template>
                  <template v-if="column.key === 'F_WagePrice'">
                    <JnpfInputNumber v-model:value="record.F_WagePrice" placeholder='请输入' :controls="false" :style='{
  "width": "100%"
}' disabled detailed />
                  </template>
                  <template v-if="column.key === 'F_StdHour'">
                    <JnpfInputNumber v-model:value="record.F_StdHour" placeholder='请输入' :controls="false" :style='{
  "width": "100%"
}' disabled detailed />
                  </template>
                  <template v-if="column.key === 'F_StdMinute'">
                    <JnpfInputNumber v-model:value="record.F_StdMinute" placeholder='请输入' :controls="false" :style='{
  "width": "100%"
}' disabled detailed />
                  </template>
                  <template v-if="column.key === 'F_StdSecond'">
                    <JnpfInputNumber v-model:value="record.F_StdSecond" placeholder='请输入' :controls="false" :style='{
  "width": "100%"
}' disabled detailed />
                  </template>
                  <template v-if="column.key === 'F_PriceType'">
                    <p>{{ record.F_PriceType }}</p>
                  </template>
                  <template v-if="column.key === 'F_UnitRatioProd'">
                    <JnpfInputNumber v-model:value="record.F_UnitRatioProd" placeholder='请输入' :controls="false" :style='{
  "width": "100%"
}' disabled detailed />
                  </template>
                  <template v-if="column.key === 'F_UnitRatioReport'">
                    <JnpfInputNumber v-model:value="record.F_UnitRatioReport" placeholder='请输入' :controls="false" :style='{
  "width": "100%"
}' disabled detailed />
                  </template>
                  <template v-if="column.key === 'F_UnitRatioBase'">
                    <p>{{ record.F_UnitRatioBase }}</p>
                  </template>
                  <template v-if="column.key === 'F_Description'">
                    <JnpfInput v-model:value="record.F_Description" :maskConfig='{
  "filler": "*",
  "maskType": 1,
  "prefixType": 1,
  "prefixLimit": 0,
  "prefixSpecifyChar": "",
  "suffixType": 1,
  "suffixLimit": 0,
  "suffixSpecifyChar": "",
  "ignoreChar": "",
  "useUnrealMask": false,
  "unrealMaskLength": 1
}' disabled detailed />
                  </template>
                  <template v-if="column.key === 'F_Files'">
                    <JnpfUploadFile v-model:value="record.F_Files" buttonText='点击上传' pathType="defaultPath" :fileSize="10" sizeUnit="MB" :limit="9" disabled detailed />
                  </template>
                  <template v-if="column.key === 'F_IsOutsource'">
                    <p>{{ record.F_IsOutsource }}</p>
                  </template>
                  <template v-if="column.key === 'F_IsQc'">
                    <p>{{ record.F_IsQc }}</p>
                  </template>
                  <template v-if="column.key === 'F_DefectHandle'">
                    <p>{{ record.F_DefectHandle }}</p>
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
  import { reactive, toRefs, nextTick, ref, computed, unref, toRaw} from 'vue';
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
    tableField7b5631outerActiveKey: number[];
    tableField7b5631innerActiveKey: string[];
  }

  defineOptions({ name: 'Detail' });
  const [registerModal, { openModal, closeModal, setModalProps }] = useModal();
  const { t } = useI18n();
  const tableField7b5631Columns: any[] = computed(() => {
    let list = [
      {
        title: "工序",
        dataIndex: 'F_ProcessId',
        key: 'F_ProcessId',
        tipLabel:  "" ,
        align:'left',
		span:'24',
		labelWidth:'',
		width:'',
		formP: 'F_ProcessId',
        fixed:false
      },
      {
        title: "生产任务计时",
        dataIndex: 'F_TaskTimer',
        key: 'F_TaskTimer',
        tipLabel:  "" ,
        align:'left',
		span:'24',
		labelWidth:'',
		width:'',
		formP: 'F_TaskTimer',
        fixed:false
      },
      {
        required: true,
        title: "排序",
        dataIndex: 'F_SortCode',
        key: 'F_SortCode',
        tipLabel:  "排序不可重复" ,
        align:'left',
		span:'24',
		labelWidth:'',
		width:'',
		formP: 'F_SortCode',
        fixed:false
      },
      {
        title: "工资单价",
        dataIndex: 'F_WagePrice',
        key: 'F_WagePrice',
        tipLabel:  "" ,
        align:'left',
		span:'24',
		labelWidth:'',
		width:'',
		formP: 'F_WagePrice',
        fixed:false
      },
      {
        title: "标准工时(时)",
        dataIndex: 'F_StdHour',
        key: 'F_StdHour',
        tipLabel:  "" ,
        align:'left',
		span:'24',
		labelWidth:'',
		width:'',
		formP: 'F_StdHour',
        fixed:false
      },
      {
        title: "标准工时(分)",
        dataIndex: 'F_StdMinute',
        key: 'F_StdMinute',
        tipLabel:  "" ,
        align:'left',
		span:'24',
		labelWidth:'',
		width:'',
		formP: 'F_StdMinute',
        fixed:false
      },
      {
        title: "标准工时(秒)",
        dataIndex: 'F_StdSecond',
        key: 'F_StdSecond',
        tipLabel:  "" ,
        align:'left',
		span:'24',
		labelWidth:'',
		width:'',
		formP: 'F_StdSecond',
        fixed:false
      },
      {
        title: "计价方式",
        dataIndex: 'F_PriceType',
        key: 'F_PriceType',
        tipLabel:  "" ,
        align:'left',
		span:'24',
		labelWidth:'',
		width:'',
		formP: 'F_PriceType',
        fixed:false
      },
      {
        title: "单位关系(生产)",
        dataIndex: 'F_UnitRatioProd',
        key: 'F_UnitRatioProd',
        tipLabel:  "" ,
        align:'left',
		span:'24',
		labelWidth:'',
		width:'',
		formP: 'F_UnitRatioProd',
        fixed:false
      },
      {
        title: "单位关系(报工)",
        dataIndex: 'F_UnitRatioReport',
        key: 'F_UnitRatioReport',
        tipLabel:  "" ,
        align:'left',
		span:'24',
		labelWidth:'',
		width:'',
		formP: 'F_UnitRatioReport',
        fixed:false
      },
      {
        title: "单位关系(单位)",
        dataIndex: 'F_UnitRatioBase',
        key: 'F_UnitRatioBase',
        tipLabel:  "" ,
        align:'left',
		span:'24',
		labelWidth:'',
		width:'',
		formP: 'F_UnitRatioBase',
        fixed:false
      },
      {
        title: "备注",
        dataIndex: 'F_Description',
        key: 'F_Description',
        tipLabel:  "" ,
        align:'left',
		span:'24',
		labelWidth:'',
		width:'',
		formP: 'F_Description',
        fixed:false
      },
      {
        title: "附件",
        dataIndex: 'F_Files',
        key: 'F_Files',
        tipLabel:  "" ,
        align:'left',
		span:'24',
		labelWidth:'',
		width:'',
		formP: 'F_Files',
        fixed:false
      },
      {
        title: "工序需外协",
        dataIndex: 'F_IsOutsource',
        key: 'F_IsOutsource',
        tipLabel:  "" ,
        align:'left',
		span:'24',
		labelWidth:'',
		width:'',
		formP: 'F_IsOutsource',
        fixed:false
      },
      {
        title: "工序需质检",
        dataIndex: 'F_IsQc',
        key: 'F_IsQc',
        tipLabel:  "" ,
        align:'left',
		span:'24',
		labelWidth:'',
		width:'',
		formP: 'F_IsQc',
        fixed:false
      },
      {
        title: "不良品需报废、返工",
        dataIndex: 'F_DefectHandle',
        key: 'F_DefectHandle',
        tipLabel:  "" ,
        align:'left',
		span:'24',
		labelWidth:'',
		width:'',
		formP: 'F_DefectHandle',
        fixed:false
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
    const noColumn = { title: t('component.table.index'), dataIndex: 'index', key: 'index', align: 'center', width: 50, fixed: 'left' }
    const actionColumn={ title: t('component.table.action'), dataIndex: 'action', key: 'action', align: 'center', width: 50, fixed: 'right' }
	let columns = [noColumn, ...columnList, actionColumn];
    const leftFixedList = columns.filter(o => o.fixed === 'left');
    const rightFixedList = columns.filter(o => o.fixed === 'right');
    const noFixedList = columns.filter(o => o.fixed !== 'left' && o.fixed !== 'right');
    return [...leftFixedList, ...noFixedList, ...rightFixedList];
  });

  const relationDetailRef = ref<any>(null);
  const state = reactive<State>({
    dataForm: {},
	interfaceRes:{
	},
	extraOptions:{
	},
	extraData:{
	},
    title: '详情',
    tableField7b5631outerActiveKey: [0],
    tableField7b5631innerActiveKey: [],
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
    getDetail(id).then((res) => {
      state.dataForm = res.data || {};

    for (let i = 0; i < state.dataForm.tableField7b5631.length; i++) {
        const element = state.dataForm.tableField7b5631[i];
        element.jnpfId = buildUUID()
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
