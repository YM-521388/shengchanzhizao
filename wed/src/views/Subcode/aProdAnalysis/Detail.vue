<template>
  <BasicModal v-bind="$attrs" @register="registerModal" :title="title" width="80%" :minHeight="100" :showOkBtn="false">
    <template #insertFooter>
    </template>
    <a-row class="dynamic-form ">
      <a-form :colon="false" layout="horizontal" labelAlign="right"  :labelCol="{ style: { width: '100px' } }" :model="dataForm" ref="formRef">
        <a-row :gutter="15">
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_ProdPlanId')" >
            <a-form-item name="F_ProdPlanId" :labelCol="{ style: { width: '100px' } }">
              <template #label>生产计划</template>
              <p class="leading-32px">{{ dataForm.F_ProdPlanId }}</p>
	          <ExtraRelationInfo :extraOptions="state.extraOptions.F_ProdPlanId" :data="state.extraData.F_ProdPlanId" v-if="state.extraOptions.F_ProdPlanId?.length && state.extraData.F_ProdPlanId && JSON.stringify(state.extraData.F_ProdPlanId) !== '{}'" />
            </a-form-item>
          </a-col>
          <a-col :span="16" class="ant-col-item" v-if="hasFormP('F_Description')" >
            <a-form-item name="F_Description" :labelCol="{ style: { width: '100px' } }">
              <template #label>备注</template>
              <p>{{ dataForm.F_Description }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="24">
            <table class="table-grid-box" :style="{'--borderType':'solid','--borderColor':'#E2E0E0','--borderWidth':'1px'}">
              <tbody>
                <tr>
                  <td colspan="1" rowspan="1">
	                  <a-row>
                    <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_ConsiderMainStock')" >
                      <a-form-item name="F_ConsiderMainStock" :labelCol="{ style: { width: '100px' } }">
                        <template #label>是否考虑主商品库存</template>
                        <p>{{ dataForm.F_ConsiderMainStock }}</p>
                      </a-form-item>
                    </a-col>
                      </a-row>
                  </td>
                  <td colspan="1" rowspan="1">
	                  <a-row>
                    <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_ConsiderMaterialStock')" >
                      <a-form-item name="F_ConsiderMaterialStock" :labelCol="{ style: { width: '100px' } }">
                        <template #label>是否考虑物料库存</template>
                        <p>{{ dataForm.F_ConsiderMaterialStock }}</p>
                      </a-form-item>
                    </a-col>
                      </a-row>
                  </td>
                </tr>
                <tr>
                  <td colspan="1" rowspan="1">
	                  <a-row>
                    <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_ConsiderMaterialOccupy')" >
                      <a-form-item name="F_ConsiderMaterialOccupy" :labelCol="{ style: { width: '100px' } }">
                        <template #label>是否考虑物料占用</template>
                        <p>{{ dataForm.F_ConsiderMaterialOccupy }}</p>
                      </a-form-item>
                    </a-col>
                      </a-row>
                  </td>
                  <td colspan="1" rowspan="1">
	                  <a-row>
                    <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_RoundUpQty')" >
                      <a-form-item name="F_RoundUpQty" :labelCol="{ style: { width: '100px' } }">
                        <template #label>是否考虑数量向上取整</template>
                        <p>{{ dataForm.F_RoundUpQty }}</p>
                      </a-form-item>
                    </a-col>
                      </a-row>
                  </td>
                </tr>
                <tr>
                  <td colspan="1" rowspan="1">
	                  <a-row>
                    <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_ConsiderWipGoods')" >
                      <a-form-item name="F_ConsiderWipGoods" :labelCol="{ style: { width: '100px' } }">
                        <template #label>是否考虑在制商品</template>
                        <p>{{ dataForm.F_ConsiderWipGoods }}</p>
                      </a-form-item>
                    </a-col>
                      </a-row>
                  </td>
                  <td colspan="1" rowspan="1">
	                  <a-row>
                    <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_ConsiderTransitGoods')" >
                      <a-form-item name="F_ConsiderTransitGoods" :labelCol="{ style: { width: '100px' } }">
                        <template #label>是否考虑在途商品</template>
                        <p>{{ dataForm.F_ConsiderTransitGoods }}</p>
                      </a-form-item>
                    </a-col>
                      </a-row>
                  </td>
                </tr>
                <tr>
                  <td colspan="1" rowspan="1">
	                  <a-row>
                    <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_OccupyAllowOtherOut')" >
                      <a-form-item name="F_OccupyAllowOtherOut" :labelCol="{ style: { width: '100px' } }">
                        <template #label>占用物料能否被其他单据出库</template>
                        <p>{{ dataForm.F_OccupyAllowOtherOut }}</p>
                      </a-form-item>
                    </a-col>
                      </a-row>
                  </td>
                  <td colspan="1" rowspan="1">
	                  <a-row>
                      </a-row>
                  </td>
                </tr>
              </tbody>
            </table>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_AnalysisUserId')" >
            <a-form-item name="F_AnalysisUserId" :labelCol="{ style: { width: '100px' } }">
              <template #label>分析人</template>
              <p>{{ dataForm.F_AnalysisUserId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_AnalysisTime')" >
            <a-form-item name="F_AnalysisTime" :labelCol="{ style: { width: '100px' } }">
              <template #label>分析时间</template>
              <p>{{ dataForm.F_AnalysisTime }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_State')" >
            <a-form-item name="F_State" :labelCol="{ style: { width: '100px' } }">
              <template #label>状态</template>
              <JnpfInput v-model:value="dataForm.F_State" :maskConfig='{
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
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_CreatorTime')" >
            <a-form-item name="F_CreatorTime" :labelCol="{ style: { width: '100px' } }">
              <template #label>创建时间</template>
              <p>{{ dataForm.F_CreatorTime }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_CreatorUserId')" >
            <a-form-item name="F_CreatorUserId" :labelCol="{ style: { width: '100px' } }">
              <template #label>创建用户</template>
              <p>{{ dataForm.F_CreatorUserId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="物料分析商品列表信息" :bordered="false" />
              <a-table :data-source="dataForm.tableField4d57ab" :columns="tableField4d57abColumns" size="small" :pagination="false" :scroll="{ x: 'max-content' }" >
                <template #headerCell="{ column }"> <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel" /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                  <template v-if="column.key === 'F_ParentId'">
                    <JnpfInput v-model:value="record.F_ParentId" :maskConfig='{
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
                  <template v-if="column.key === 'F_GoodsId'">
                    <p class="leading-32px">{{ record.F_GoodsId }}</p>
	                <ExtraRelationInfo :extraOptions="state.extraOptions.F_GoodsId" :data="state.extraData.F_GoodsId" v-if="state.extraOptions.F_GoodsId?.length && state.extraData.F_GoodsId && JSON.stringify(state.extraData.F_GoodsId) !== '{}'" />
                  </template>
                  <template v-if="column.key === 'F_Unit'">
                    <JnpfInputNumber v-model:value="record.F_Unit" placeholder='请输入' :controls="false" :style='{
  "width": "100%"
}' disabled detailed />
                  </template>
                  <template v-if="column.key === 'F_DemandQty'">
                    <JnpfInputNumber v-model:value="record.F_DemandQty" placeholder='请输入' :controls="false" :style='{
  "width": "100%"
}' disabled detailed />
                  </template>
                  <template v-if="column.key === 'F_AvailableStock'">
                    <JnpfInputNumber v-model:value="record.F_AvailableStock" placeholder='请输入' :controls="false" :style='{
  "width": "100%"
}' disabled detailed />
                  </template>
                  <template v-if="column.key === 'F_WipStock'">
                    <JnpfInputNumber v-model:value="record.F_WipStock" placeholder='请输入' :controls="false" :style='{
  "width": "100%"
}' disabled detailed />
                  </template>
                  <template v-if="column.key === 'F_TransitStock'">
                    <JnpfInputNumber v-model:value="record.F_TransitStock" placeholder='请输入' :controls="false" :style='{
  "width": "100%"
}' disabled detailed />
                  </template>
                  <template v-if="column.key === 'F_ShouldSelfMake'">
                    <JnpfInputNumber v-model:value="record.F_ShouldSelfMake" placeholder='请输入' :controls="false" :style='{
  "width": "100%"
}' disabled detailed />
                  </template>
                  <template v-if="column.key === 'F_ShouldOutsource'">
                    <JnpfInputNumber v-model:value="record.F_ShouldOutsource" placeholder='请输入' :controls="false" :style='{
  "width": "100%"
}' disabled detailed />
                  </template>
                  <template v-if="column.key === 'F_ShouldPurchase'">
                    <JnpfInputNumber v-model:value="record.F_ShouldPurchase" placeholder='请输入' :controls="false" :style='{
  "width": "100%"
}' disabled detailed />
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
  import { reactive, toRefs, nextTick, ref, computed, unref, toRaw} from 'vue';
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
    tableField4d57abouterActiveKey: number[];
    tableField4d57abinnerActiveKey: string[];
  }

  defineOptions({ name: 'Detail' });
  const [registerModal, { openModal, closeModal, setModalProps }] = useModal();
  const { t } = useI18n();
  const tableField4d57abColumns: any[] = computed(() => {
    let list = [
      {
        title: "商品上级",
        dataIndex: 'F_ParentId',
        key: 'F_ParentId',
        tipLabel:  "" ,
        align:'left',
		span:'24',
		labelWidth:'',
		width:'',
		formP: 'F_ParentId',
        fixed:false
      },
      {
        title: "商品",
        dataIndex: 'F_GoodsId',
        key: 'F_GoodsId',
        tipLabel:  "" ,
        align:'left',
		span:'24',
		labelWidth:'',
		width:'',
		formP: 'F_GoodsId',
        fixed:false
      },
      {
        title: "单位用量",
        dataIndex: 'F_Unit',
        key: 'F_Unit',
        tipLabel:  "" ,
        align:'left',
		span:'24',
		labelWidth:'',
		width:'',
		formP: 'F_Unit',
        fixed:false
      },
      {
        title: "需求",
        dataIndex: 'F_DemandQty',
        key: 'F_DemandQty',
        tipLabel:  "" ,
        align:'left',
		span:'24',
		labelWidth:'',
		width:'',
		formP: 'F_DemandQty',
        fixed:false
      },
      {
        title: "可用库存",
        dataIndex: 'F_AvailableStock',
        key: 'F_AvailableStock',
        tipLabel:  "" ,
        align:'left',
		span:'24',
		labelWidth:'',
		width:'',
		formP: 'F_AvailableStock',
        fixed:false
      },
      {
        title: "在制库存",
        dataIndex: 'F_WipStock',
        key: 'F_WipStock',
        tipLabel:  "" ,
        align:'left',
		span:'24',
		labelWidth:'',
		width:'',
		formP: 'F_WipStock',
        fixed:false
      },
      {
        title: "在途库存",
        dataIndex: 'F_TransitStock',
        key: 'F_TransitStock',
        tipLabel:  "" ,
        align:'left',
		span:'24',
		labelWidth:'',
		width:'',
		formP: 'F_TransitStock',
        fixed:false
      },
      {
        title: "应自产",
        dataIndex: 'F_ShouldSelfMake',
        key: 'F_ShouldSelfMake',
        tipLabel:  "" ,
        align:'left',
		span:'24',
		labelWidth:'',
		width:'',
		formP: 'F_ShouldSelfMake',
        fixed:false
      },
      {
        title: "应外协",
        dataIndex: 'F_ShouldOutsource',
        key: 'F_ShouldOutsource',
        tipLabel:  "" ,
        align:'left',
		span:'24',
		labelWidth:'',
		width:'',
		formP: 'F_ShouldOutsource',
        fixed:false
      },
      {
        title: "应采购",
        dataIndex: 'F_ShouldPurchase',
        key: 'F_ShouldPurchase',
        tipLabel:  "" ,
        align:'left',
		span:'24',
		labelWidth:'',
		width:'',
		formP: 'F_ShouldPurchase',
        fixed:false
      },
      {
        title: "创建用户",
        dataIndex: 'F_CreatorUserId',
        key: 'F_CreatorUserId',
        tipLabel:  "" ,
        align:'left',
		span:'24',
		labelWidth:'',
		width:'',
		formP: 'F_CreatorUserId',
        fixed:false
      },
      {
        title: "创建时间",
        dataIndex: 'F_CreatorTime',
        key: 'F_CreatorTime',
        tipLabel:  "" ,
        align:'left',
		span:'24',
		labelWidth:'',
		width:'',
		formP: 'F_CreatorTime',
        fixed:false
      },
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
      F_ProdPlanId: [],
	},
	extraOptions:{
      F_ProdPlanId: [],
	},
	extraData:{
      F_ProdPlanId: {},
	},
    title: '详情',
    tableField4d57abouterActiveKey: [0],
    tableField4d57abinnerActiveKey: [],
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
    getDetail(id).then((res) => {
      state.dataForm = res.data || {};

    for (let i = 0; i < state.dataForm.tableField4d57ab.length; i++) {
        const element = state.dataForm.tableField4d57ab[i];
        element.jnpfId = buildUUID()
    }
      nextTick(() => {
        changeLoading(false);
		getF_ProdPlanIdExtraInfo();
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

	function getF_ProdPlanIdExtraInfo() {
	    if(!state.dataForm.F_ProdPlanId_id) return;
        const paramList = getParamList('F_ProdPlanId');
        const query = {
            ids: [state.dataForm.F_ProdPlanId_id],
            interfaceId: '2014512148058869760',
            propsValue:"F_Id" ,
            relationField:"F_PlanNo" ,
            paramList,
        };
        getDataInterfaceDataInfoByIds('2014512148058869760', query).then(res => {
            const data = res.data && res.data.length ? res.data[0] : {};
            state.extraData.F_ProdPlanId = data;
        });
    }
</script>
