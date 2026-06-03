<template>
  <BasicModal v-bind="$attrs" @register="registerModal" :title="title" width="70%" :minHeight="100" :showOkBtn="false">
    <template #insertFooter>
    </template>
    <a-row class="dynamic-form ">
      <a-form :colon="false" layout="horizontal" labelAlign="right"  :labelCol="{ style: { width: '100px' } }" :model="dataForm" ref="formRef">
        <a-row :gutter="15">
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_OutsourceId')" >
            <a-form-item name="F_OutsourceId" :labelCol="{ style: { width: '100px' } }">
              <template #label>外协工单</template>
              <p class="leading-32px">{{ dataForm.F_OutsourceId }}</p>
	          <ExtraRelationInfo :extraOptions="state.extraOptions.F_OutsourceId" :data="state.extraData.F_OutsourceId" v-if="state.extraOptions.F_OutsourceId?.length && state.extraData.F_OutsourceId && JSON.stringify(state.extraData.F_OutsourceId) !== '{}'" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_PassNum')" >
            <a-form-item name="F_PassNum" :labelCol="{ style: { width: '100px' } }">
              <template #label>合格数量</template>
              <JnpfInputNumber v-model:value="dataForm.F_PassNum" placeholder='请输入' :controls="false" :style='{
  "width": "100%"
}' disabled detailed />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_FailNum')" >
            <a-form-item name="F_FailNum" :labelCol="{ style: { width: '100px' } }">
              <template #label>不合格数量</template>
              <JnpfInputNumber v-model:value="dataForm.F_FailNum" placeholder='请输入' :controls="false" :style='{
  "width": "100%"
}' disabled detailed />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_OutsourceType')" >
            <a-form-item name="F_OutsourceType" :labelCol="{ style: { width: '100px' } }">
              <template #label>外协类型</template>
              <JnpfInput v-model:value="dataForm.F_OutsourceType" :maskConfig='{
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_SettleStatus')" >
            <a-form-item name="F_SettleStatus" :labelCol="{ style: { width: '100px' } }">
              <template #label>结算状态</template>
              <p>{{ dataForm.F_SettleStatus }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_SettleUnitPrice')" >
            <a-form-item name="F_SettleUnitPrice" :labelCol="{ style: { width: '100px' } }">
              <template #label>结算单价</template>
              <JnpfInput v-model:value="dataForm.F_SettleUnitPrice" :maskConfig='{
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_SettleUserId')" >
            <a-form-item name="F_SettleUserId" :labelCol="{ style: { width: '100px' } }">
              <template #label>结算人</template>
              <p>{{ dataForm.F_SettleUserId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_SettleTime')" >
            <a-form-item name="F_SettleTime" :labelCol="{ style: { width: '100px' } }">
              <template #label>结算时间</template>
              <p>{{ dataForm.F_SettleTime }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_SettleFiles')" >
            <a-form-item name="F_SettleFiles" :labelCol="{ style: { width: '100px' } }">
              <template #label>结算附件</template>
              <JnpfUploadFile v-model:value="dataForm.F_SettleFiles" buttonText='点击上传' pathType="defaultPath" :fileSize="10" sizeUnit="MB" :limit="9" disabled detailed />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_SettleDesc')" >
            <a-form-item name="F_SettleDesc" :labelCol="{ style: { width: '100px' } }">
              <template #label>结算备注</template>
              <p>{{ dataForm.F_SettleDesc }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_CreatorUserId')" >
            <a-form-item name="F_CreatorUserId" :labelCol="{ style: { width: '100px' } }">
              <template #label>创建用户</template>
              <p>{{ dataForm.F_CreatorUserId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_CreatorTime')" >
            <a-form-item name="F_CreatorTime" :labelCol="{ style: { width: '100px' } }">
              <template #label>创建时间</template>
              <p>{{ dataForm.F_CreatorTime }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="选择验收内容" :bordered="false" />
              <a-table :data-source="dataForm.tableField561a64" :columns="tableField561a64Columns" size="small" :pagination="false" :scroll="{ x: 'max-content' }" >
                <template #headerCell="{ column }"> <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel" /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                  <template v-if="column.key === 'F_AcceptId'">
                    <p class="leading-32px">{{ record.F_AcceptId }}</p>
	                <ExtraRelationInfo :extraOptions="state.extraOptions.F_AcceptId" :data="state.extraData.F_AcceptId" v-if="state.extraOptions.F_AcceptId?.length && state.extraData.F_AcceptId && JSON.stringify(state.extraData.F_AcceptId) !== '{}'" />
                  </template>
                  <template v-if="column.key === 'F_SortCode'">
                    <JnpfInputNumber v-model:value="record.F_SortCode" placeholder='请输入' :controls="false" :style='{
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
          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="操作日志" :bordered="false" />
              <a-table :data-source="dataForm.tableField4b440e" :columns="tableField4b440eColumns" size="small" :pagination="false" :scroll="{ x: 'max-content' }" >
                <template #headerCell="{ column }"> <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel" /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                  <template v-if="column.key === 'F_Title'">
                    <JnpfInput v-model:value="record.F_Title" :maskConfig='{
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
                  <template v-if="column.key === 'F_Content'">
                    <p>{{ record.F_Content }}</p>
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
    tableField561a64outerActiveKey: number[];
    tableField561a64innerActiveKey: string[];
    tableField4b440eouterActiveKey: number[];
    tableField4b440einnerActiveKey: string[];
  }

  defineOptions({ name: 'Detail' });
  const [registerModal, { openModal, closeModal, setModalProps }] = useModal();
  const { t } = useI18n();
  const tableField561a64Columns: any[] = computed(() => {
    let list = [
      {
        title: "验收内容",
        dataIndex: 'F_AcceptId',
        key: 'F_AcceptId',
        tipLabel:  "" ,
        align:'left',
		span:'24',
		labelWidth:'',
		width:'',
		formP: 'F_AcceptId',
        fixed:false
      },
      {
        title: "排序",
        dataIndex: 'F_SortCode',
        key: 'F_SortCode',
        tipLabel:  "" ,
        align:'left',
		span:'24',
		labelWidth:'',
		width:'',
		formP: 'F_SortCode',
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
	list = list.filter(o => hasFormP('tableField561a64-' + o.formP));
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
  const tableField4b440eColumns: any[] = computed(() => {
    let list = [
      {
        title: "标题",
        dataIndex: 'F_Title',
        key: 'F_Title',
        tipLabel:  "" ,
        align:'left',
		span:'24',
		labelWidth:'',
		width:'',
		formP: 'F_Title',
        fixed:false
      },
      {
        title: "内容",
        dataIndex: 'F_Content',
        key: 'F_Content',
        tipLabel:  "" ,
        align:'left',
		span:'24',
		labelWidth:'',
		width:'',
		formP: 'F_Content',
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
	list = list.filter(o => hasFormP('tableField4b440e-' + o.formP));
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
      F_OutsourceId: [],
	},
	extraOptions:{
      F_OutsourceId: [],
	},
	extraData:{
      F_OutsourceId: {},
	},
    title: '详情',
    tableField561a64outerActiveKey: [0],
    tableField561a64innerActiveKey: [],
    tableField4b440eouterActiveKey: [0],
    tableField4b440einnerActiveKey: [],
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

    for (let i = 0; i < state.dataForm.tableField561a64.length; i++) {
        const element = state.dataForm.tableField561a64[i];
        element.jnpfId = buildUUID()
    }
    for (let i = 0; i < state.dataForm.tableField4b440e.length; i++) {
        const element = state.dataForm.tableField4b440e[i];
        element.jnpfId = buildUUID()
    }
      nextTick(() => {
        changeLoading(false);
		getF_OutsourceIdExtraInfo();
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

	function getF_OutsourceIdExtraInfo() {
	    if(!state.dataForm.F_OutsourceId_id) return;
        const paramList = getParamList('F_OutsourceId');
        const query = {
            ids: [state.dataForm.F_OutsourceId_id],
            interfaceId: '2014180492177444864',
            propsValue:"F_Id" ,
            relationField:"F_OutsourceNo" ,
            paramList,
        };
        getDataInterfaceDataInfoByIds('2014180492177444864', query).then(res => {
            const data = res.data && res.data.length ? res.data[0] : {};
            state.extraData.F_OutsourceId = data;
        });
    }
</script>
