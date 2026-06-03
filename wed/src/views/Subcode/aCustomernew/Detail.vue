<template>
  <BasicModal v-bind="$attrs" @register="registerModal" :title="title" width="70%" :minHeight="100" :showOkBtn="false" :canFullscreen="true">
    <template #insertFooter> </template>
    <a-row class="dynamic-form">
      <a-form :colon="false" layout="horizontal" labelAlign="right" :labelCol="{ style: { width: '100px' } }" :model="dataForm" ref="formRef">
        <a-row :gutter="15">
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_CustomerName')">
            <a-form-item name="F_CustomerName" :labelCol="{ style: { width: '100px' } }">
              <template #label>名称：</template>
              <JnpfInput
                v-model:value="dataForm.F_CustomerName"
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_CustomerCode')">
            <a-form-item name="F_CustomerCode" :labelCol="{ style: { width: '100px' } }">
              <template #label>编号：</template>
              <JnpfInput
                v-model:value="dataForm.F_CustomerCode"
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
          <!-- <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_QRCode')">
            <a-form-item name="F_QRCode" :labelCol="{ style: { width: '100px' } }">
              <template #label>二维码：</template>
              <JnpfInput
                v-model:value="dataForm.F_QRCode"
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
          </a-col> -->
          <!-- <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_IsPublic')">
            <a-form-item name="F_IsPublic" :labelCol="{ style: { width: '100px' } }">
              <template #label>公海客户</template>
              <JnpfInput
                v-model:value="dataForm.F_IsPublic"
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
          </a-col> -->
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_CustomerTags')">
            <a-form-item name="F_CustomerTags" :labelCol="{ style: { width: '100px' } }">
              <template #label>客户标签：</template>
              <p>{{ dataForm.F_CustomerTags }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_ShareUsers')">
            <a-form-item name="F_ShareUsers" :labelCol="{ style: { width: '100px' } }">
              <template #label>客户共享：</template>
              <p>{{ dataForm.F_ShareUsers }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_IsFollow')">
            <a-form-item name="F_IsFollow" :labelCol="{ style: { width: '100px' } }">
              <template #label>关注：</template>
              <JnpfInput
                v-model:value="dataForm.F_IsFollow"
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
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_State')">
            <a-form-item name="F_State" :labelCol="{ style: { width: '100px' } }">
              <template #label>状态：</template>
              <p>{{ dataForm.F_State }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_RequisitionUserId')">
            <a-form-item name="F_RequisitionUserId" :labelCol="{ style: { width: '100px' } }">
              <template #label>领用人员：</template>
              <p>{{ dataForm.F_RequisitionUserId }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_RequisitionTime')">
            <a-form-item name="F_RequisitionTime" :labelCol="{ style: { width: '100px' } }">
              <template #label>领用时间：</template>
              <p>{{ dataForm.F_RequisitionTime }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_Description')">
            <a-form-item name="F_Description" :labelCol="{ style: { width: '100px' } }">
              <template #label>备注：</template>
              <p>{{ dataForm.F_Description }}</p>
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_Files')">
            <a-form-item name="F_Files" :labelCol="{ style: { width: '100px' } }">
              <template #label>附件：</template>
              <JnpfUploadFile
                v-model:value="dataForm.F_Files"
                buttonText="点击上传"
                pathType="defaultPath"
                :fileSize="10"
                sizeUnit="MB"
                :limit="9"
                disabled
                detailed />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="客户地址" :bordered="false" />
              <a-table
                :data-source="dataForm.tableField6eed25"
                :columns="tableField6eed25Columns"
                size="small"
                :pagination="false"
                :scroll="{ x: 'max-content' }">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                  <template v-if="column.key === 'F_Region'">
                    <p>{{ record.F_Region }}</p>
                  </template>
                  <template v-if="column.key === 'F_Address'">
                    <JnpfInput
                      v-model:value="record.F_Address"
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
                  <template v-if="column.key === 'F_IsDefault'">
                    <p>{{ record.F_IsDefault }}</p>
                  </template>
                  <template v-if="column.key === 'F_CreatorTime'">
                    <p>{{ record.F_CreatorTime }}</p>
                  </template>
                </template>
              </a-table>
            </a-form-item>
          </a-col>
          <!-- 操作日志（已移动到页面底部） -->
          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="客户联系人" :bordered="false" />
              <a-table
                :data-source="dataForm.tableFieldddabab"
                :columns="tableFieldddababColumns"
                size="small"
                :pagination="false"
                :scroll="{ x: 'max-content' }">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                  <template v-if="column.key === 'F_ContactName'">
                    <JnpfInput
                      v-model:value="record.F_ContactName"
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
                  <template v-if="column.key === 'F_ContactPhone'">
                    <JnpfInput
                      v-model:value="record.F_ContactPhone"
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
                  <template v-if="column.key === 'F_IsDefault'">
                    <p>{{ record.F_IsDefault }}</p>
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
    <!-- 操作日志（页面底部） -->
    <a-row style="margin-top: 24px" v-if="hasFormP('Logs')">
      <a-col :span="24" class="ant-col-item">
        <a-form-item label="">
          <div style="text-align: center; margin: 8px 0 16px 0; font-weight: 600; font-size: 14px">操作日志</div>
          <div v-if="state.logs && state.logs.length">
            <template v-for="(log, idx) in state.logs" :key="log.id || idx">
              <a-row style="padding: 12px 0">
                <a-col :span="6">
                  <p>{{ log.F_CreatorTime }}</p>
                </a-col>
                <a-col :span="12">
                  <p>{{ log.F_Title }} {{ log.F_Content }}</p>
                </a-col>
                <a-col :span="6" style="text-align: right">
                  <p>{{ log.F_CreatorUserId }}</p>
                </a-col>
              </a-row>
              <a-divider />
            </template>
          </div>
          <div v-else style="text-align: center; padding: 24px 0; color: #999"> 暂无操作日志 </div>
        </a-form-item>
      </a-col>
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
    logs: any[];
    tableField6eed25outerActiveKey: number[];
    tableField6eed25innerActiveKey: string[];
    tableFieldddababouterActiveKey: number[];
    tableFieldddababinnerActiveKey: string[];
  }

  defineOptions({ name: 'Detail' });
  const [registerModal, { openModal, closeModal, setModalProps }] = useModal();
  const { t } = useI18n();
  const tableField6eed25Columns: any[] = computed(() => {
    let list = [
      {
        title: '地区',
        dataIndex: 'F_Region',
        key: 'F_Region',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_Region',
        fixed: false,
      },
      {
        title: '地址',
        dataIndex: 'F_Address',
        key: 'F_Address',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_Address',
        fixed: false,
      },
      {
        title: '是否默认',
        dataIndex: 'F_IsDefault',
        key: 'F_IsDefault',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_IsDefault',
        fixed: false,
      },
      {
        title: '创建时间',
        dataIndex: 'F_CreatorTime',
        key: 'F_CreatorTime',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_CreatorTime',
        fixed: false,
      },
    ];
    list = list.filter(o => hasFormP('tableField6eed25-' + o.formP));
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
    let columns = [noColumn, ...columnList, actionColumn];
    const leftFixedList = columns.filter(o => o.fixed === 'left');
    const rightFixedList = columns.filter(o => o.fixed === 'right');
    const noFixedList = columns.filter(o => o.fixed !== 'left' && o.fixed !== 'right');
    return [...leftFixedList, ...noFixedList, ...rightFixedList];
  });
  const tableFieldddababColumns: any[] = computed(() => {
    let list = [
      {
        title: '联系人',
        dataIndex: 'F_ContactName',
        key: 'F_ContactName',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_ContactName',
        fixed: false,
      },
      {
        title: '联系人电话',
        dataIndex: 'F_ContactPhone',
        key: 'F_ContactPhone',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_ContactPhone',
        fixed: false,
      },
      {
        title: '是否默认',
        dataIndex: 'F_IsDefault',
        key: 'F_IsDefault',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_IsDefault',
        fixed: false,
      },
      {
        title: '创建时间',
        dataIndex: 'F_CreatorTime',
        key: 'F_CreatorTime',
        tipLabel: '',
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        formP: 'F_CreatorTime',
        fixed: false,
      },
    ];
    list = list.filter(o => hasFormP('tableFieldddabab-' + o.formP));
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
    let columns = [noColumn, ...columnList, actionColumn];
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
    logs: [],
    tableField6eed25outerActiveKey: [0],
    tableField6eed25innerActiveKey: [],
    tableFieldddababouterActiveKey: [0],
    tableFieldddababinnerActiveKey: [],
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
    getDetail(id)
      .then(res => {
        const payload = res.data || {};
        // backend may return { detail, logs } — prefer detail when present
        state.dataForm = payload.detail || payload;
        // attach logs separately (frontend can use if needed)
        state['logs'] = payload.logs || [];

        // ensure child table arrays exist to avoid undefined.length errors
        state.dataForm.tableField6eed25 = state.dataForm.tableField6eed25 || [];
        state.dataForm.tableFieldddabab = state.dataForm.tableFieldddabab || [];

        for (let i = 0; i < state.dataForm.tableField6eed25.length; i++) {
          const element = state.dataForm.tableField6eed25[i];
          element.jnpfId = buildUUID();
        }
        for (let i = 0; i < state.dataForm.tableFieldddabab.length; i++) {
          const element = state.dataForm.tableFieldddabab[i];
          element.jnpfId = buildUUID();
        }
        nextTick(() => {
          changeLoading(false);
        });
      })
      .catch(() => {
        // ensure loader is hidden on error
        changeLoading(false);
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
