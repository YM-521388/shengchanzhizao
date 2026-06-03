<template>
  <BasicModal v-bind="$attrs" @register="registerModal" width="70%" :minHeight="100" okText="确定" cancelText="取消" @ok="handleSubmit" :closeFunc="onClose">
    <template #title>
      <a-space :size="10">
        <div class="text-16px font-medium">{{ title }}</div>
        <a-space-compact size="small" block v-if="dataForm.id">
          <a-tooltip :title="t('common.prevRecord')">
            <a-button size="small" :disabled="getPrevDisabled" @click="handlePrev">
              <i class="icon-ym icon-ym-caret-left text-10px"></i>
            </a-button>
          </a-tooltip>
          <a-tooltip :title="t('common.nextRecord')">
            <a-button size="small" :disabled="getNextDisabled" @click="handleNext">
              <i class="icon-ym icon-ym-caret-right text-10px"></i>
            </a-button>
          </a-tooltip>
        </a-space-compact>
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
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_CustomerName')">
            <a-form-item name="F_CustomerName" :labelCol="{ style: { width: '100px' } }">
              <template #label>名称</template>
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
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_CustomerCode')">
            <a-form-item name="F_CustomerCode" :labelCol="{ style: { width: '100px' } }">
              <template #label>编号</template>
              <JnpfInput
                v-model:value="dataForm.F_CustomerCode"
                placeholder="请填写，忽略当自动生成"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>
          <!-- <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_QRCode')">
            <a-form-item name="F_QRCode" :labelCol="{ style: { width: '100px' } }">
              <template #label>二维码</template>
              <JnpfInput
                v-model:value="dataForm.F_QRCode"
                placeholder="请输入"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col> -->
          <!-- <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_IsPublic')">
            <a-form-item name="F_IsPublic" :labelCol="{ style: { width: '100px' } }">
              <template #label>公海客户</template>
              <JnpfInput
                v-model:value="dataForm.F_IsPublic"
                placeholder="请输入"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col> -->
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_CustomerTags')">
            <a-form-item name="F_CustomerTags" :labelCol="{ style: { width: '100px' } }">
              <template #label>客户标签</template>
              <JnpfSelect
                v-model:value="dataForm.F_CustomerTags"
                placeholder="请选择"
                :options="optionsObj.F_CustomerTagsOptions"
                :fieldNames="optionsObj.F_CustomerTagsProps"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_ShareUsers')">
            <a-form-item name="F_ShareUsers" :labelCol="{ style: { width: '100px' } }">
              <template #label>客户共享</template>
              <JnpfUserSelect
                v-model:value="dataForm.F_ShareUsers"
                placeholder="请选择"
                selectType="all"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <!-- <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_IsFollow')">
            <a-form-item name="F_IsFollow" :labelCol="{ style: { width: '100px' } }">
              <template #label>关注</template>
              <JnpfInput
                v-model:value="dataForm.F_IsFollow"
                placeholder="请输入"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col> -->
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_State')">
            <a-form-item name="F_State" :labelCol="{ style: { width: '100px' } }">
              <template #label>状态</template>
              <JnpfSwitch v-model:value="dataForm.F_State" />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="客户地址" :bordered="false" />
              <a-table
                :data-source="dataForm.tableField6eed25"
                :columns="tableField6eed25Columns"
                size="small"
                rowKey="jnpfId"
                :pagination="false"
                :scroll="{ x: 'max-content' }"
                :rowSelection="gettableField6eed25RowSelection">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                  <template v-if="column.key === 'F_Region'">
                    <JnpfAreaSelect
                      v-model:value="record.F_Region"
                      placeholder="请选择"
                      allowClear
                      :level="2"
                      :style="{
                        width: '100%',
                      }" />
                  </template>
                  <template v-if="column.key === 'F_Address'">
                    <JnpfInput
                      v-model:value="record.F_Address"
                      placeholder="请输入"
                      allowClear
                      :style="{
                        width: '100%',
                      }"
                      :showCount="false" />
                  </template>
                  <template v-if="column.key === 'F_IsDefault'">
                    <JnpfSwitch v-model:value="record.F_IsDefault" />
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
                      <a-button class="action-btn" type="link" @click="copytableField6eed25Row(index)" size="small">{{
                        t('common.copyText', '复制')
                      }}</a-button>
                      <a-button class="action-btn" type="link" color="error" @click="removetableField6eed25Row(index, true)" size="small">{{
                        t('common.delText', '删除')
                      }}</a-button>
                    </a-space>
                  </template>
                </template>
              </a-table>
              <a-space class="input-table-footer-btn">
                <a-button type="primary" preIcon="icon-ym icon-ym-btn-add" @click="addtableField6eed25Row">{{ t('common.add1Text', '添加') }}</a-button>
                <a-button type="danger" preIcon="icon-ym icon-ym-btn-clearn" @click="batchRemovetableField6eed25Row(true)">{{
                  t('common.batchDelText', '批量删除')
                }}</a-button>
              </a-space>
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="客户联系人" :bordered="false" />
              <a-table
                :data-source="dataForm.tableFieldddabab"
                :columns="tableFieldddababColumns"
                size="small"
                rowKey="jnpfId"
                :pagination="false"
                :scroll="{ x: 'max-content' }"
                :rowSelection="gettableFieldddababRowSelection">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                  <template v-if="column.key === 'F_ContactName'">
                    <JnpfInput
                      v-model:value="record.F_ContactName"
                      placeholder="请输入"
                      allowClear
                      :style="{
                        width: '100%',
                      }"
                      :showCount="false" />
                  </template>
                  <template v-if="column.key === 'F_ContactPhone'">
                    <JnpfInput
                      v-model:value="record.F_ContactPhone"
                      placeholder="请输入"
                      allowClear
                      :style="{
                        width: '100%',
                      }"
                      :showCount="false" />
                  </template>
                  <template v-if="column.key === 'F_IsDefault'">
                    <JnpfSwitch v-model:value="record.F_IsDefault" />
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
                      <a-button class="action-btn" type="link" @click="copytableFieldddababRow(index)" size="small">{{
                        t('common.copyText', '复制')
                      }}</a-button>
                      <a-button class="action-btn" type="link" color="error" @click="removetableFieldddababRow(index, true)" size="small">{{
                        t('common.delText', '删除')
                      }}</a-button>
                    </a-space>
                  </template>
                </template>
              </a-table>
              <a-space class="input-table-footer-btn">
                <a-button type="primary" preIcon="icon-ym icon-ym-btn-add" @click="addtableFieldddababRow">{{ t('common.add1Text', '添加') }}</a-button>
                <a-button type="danger" preIcon="icon-ym icon-ym-btn-clearn" @click="batchRemovetableFieldddababRow(true)">{{
                  t('common.batchDelText', '批量删除')
                }}</a-button>
              </a-space>
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_Description')">
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
                :showCount="false" />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_Files')">
            <a-form-item name="F_Files" :labelCol="{ style: { width: '100px' } }">
              <template #label>附件</template>
              <JnpfUploadFile
                v-model:value="dataForm.F_Files"
                buttonText="点击上传"
                pathType="defaultPath"
                :fileSize="10"
                sizeUnit="MB"
                :limit="9"
                :sortRule="[]"
                timeFormat="YYYY" />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_RequisitionUserId')">
            <a-form-item name="F_RequisitionUserId" :labelCol="{ style: { width: '100px' } }">
              <template #label>领用人员</template>
              <JnpfUserSelect
                v-model:value="dataForm.F_RequisitionUserId"
                placeholder="请选择"
                selectType="all"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_RequisitionTime')">
            <a-form-item name="F_RequisitionTime" :labelCol="{ style: { width: '100px' } }">
              <template #label>领用时间</template>
              <JnpfDatePicker
                v-model:value="dataForm.F_RequisitionTime"
                placeholder="请选择"
                format="yyyy-MM-dd"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="12" class="ant-col-item" v-if="hasFormP('F_CreatorUserId')">
            <a-form-item name="F_CreatorUserId" :labelCol="{ style: { width: '100px' } }">
              <template #label>创建人员</template>
              <JnpfOpenData
                v-model:value="dataForm.F_CreatorUserId"
                type="currUser"
                readonly
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_CreatorTime')">
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
        </a-row>
      </a-form>
    </a-row>
  </BasicModal>
  <SelectModal :config="state.currTableConf" :formData="state.dataForm" ref="selectModal" @select="addForSelect" />
</template>
<script lang="ts" setup>
  import { create, update, getInfo } from './helper/api';
  import { reactive, toRefs, nextTick, ref, unref, computed, toRaw, inject } from 'vue';
  import { BasicModal, useModal } from '@/components/Modal';
  import SelectModal from '@/components/CommonModal/src/SelectModal.vue';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useI18n } from '@/hooks/web/useI18n';
  import { useUserStore } from '@/store/modules/user';
  import type { FormInstance } from 'ant-design-vue';
  import { thousandsFormat } from '@/utils/jnpf';
  import { getTimeUnit, getDateTimeUnit } from '@/utils/jnpf';
  import { getDictionaryDataSelector } from '@/api/systemData/dictionary';
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
    tableField6eed25outerActiveKey: number[];
    tableField6eed25innerActiveKey: string[];
    tableField6eed25DefaultExpandAll: boolean;
    selectedtableField6eed25RowKeys: any[];
    tableFieldddababouterActiveKey: number[];
    tableFieldddababinnerActiveKey: string[];
    tableFieldddababDefaultExpandAll: boolean;
    selectedtableFieldddababRowKeys: any[];
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
  const { hasFormP } = usePermission();
  const [registerModal, { openModal, setModalProps }] = useModal();
  const formRef = ref<FormInstance>();
  const selectModal = ref(null);
  const tableField6eed25Columns: any[] = computed(() => {
    let list = [
      {
        title: '地区',
        dataIndex: 'F_Region',
        key: 'F_Region',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Region',
        isSystemControl: false,
      },
      {
        title: '地址',
        dataIndex: 'F_Address',
        key: 'F_Address',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Address',
        isSystemControl: false,
      },
      {
        title: '是否默认',
        dataIndex: 'F_IsDefault',
        key: 'F_IsDefault',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_IsDefault',
        isSystemControl: false,
      },
      {
        title: '创建时间',
        dataIndex: 'F_CreatorTime',
        key: 'F_CreatorTime',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_CreatorTime',
        isSystemControl: true,
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
    const actionColumn = { title: t('component.table.action'), dataIndex: 'action', key: 'action', align: 'center', width: 100, fixed: 'right' };
    let columns = [noColumn, ...columnList, actionColumn];
    const leftFixedList = columns.filter(o => o.fixed === 'left');
    const rightFixedList = columns.filter(o => o.fixed === 'right');
    const noFixedList = columns.filter(o => o.fixed !== 'left' && o.fixed !== 'right');
    return [...leftFixedList, ...noFixedList, ...rightFixedList];
  });
  const gettableField6eed25RowSelection = computed(() => {
    const rowSelection = {
      selectedRowKeys: state.selectedtableField6eed25RowKeys,
      onChange: (selectedRowKeys: string[]) => {
        state.selectedtableField6eed25RowKeys = (selectedRowKeys || []).sort().reverse();
      },
    };
    return rowSelection;
  });
  const tableFieldddababColumns: any[] = computed(() => {
    let list = [
      {
        title: '联系人',
        dataIndex: 'F_ContactName',
        key: 'F_ContactName',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_ContactName',
        isSystemControl: false,
      },
      {
        title: '联系人电话',
        dataIndex: 'F_ContactPhone',
        key: 'F_ContactPhone',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_ContactPhone',
        isSystemControl: false,
      },
      {
        title: '是否默认',
        dataIndex: 'F_IsDefault',
        key: 'F_IsDefault',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_IsDefault',
        isSystemControl: false,
      },
      {
        title: '创建时间',
        dataIndex: 'F_CreatorTime',
        key: 'F_CreatorTime',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_CreatorTime',
        isSystemControl: true,
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
    const actionColumn = { title: t('component.table.action'), dataIndex: 'action', key: 'action', align: 'center', width: 100, fixed: 'right' };
    let columns = [noColumn, ...columnList, actionColumn];
    const leftFixedList = columns.filter(o => o.fixed === 'left');
    const rightFixedList = columns.filter(o => o.fixed === 'right');
    const noFixedList = columns.filter(o => o.fixed !== 'left' && o.fixed !== 'right');
    return [...leftFixedList, ...noFixedList, ...rightFixedList];
  });
  const gettableFieldddababRowSelection = computed(() => {
    const rowSelection = {
      selectedRowKeys: state.selectedtableFieldddababRowKeys,
      onChange: (selectedRowKeys: string[]) => {
        state.selectedtableFieldddababRowKeys = (selectedRowKeys || []).sort().reverse();
      },
    };
    return rowSelection;
  });
  const state = reactive<State>({
    dataForm: {
      id: '',
      F_CustomerName: undefined,
      F_CustomerCode: undefined,
      F_QRCode: undefined,
      F_IsPublic: undefined,
      F_CustomerTags: undefined,
      F_ShareUsers: undefined,
      F_IsFollow: undefined,
      F_State: 0,
      tableField6eed25: [],
      tableFieldddabab: [],
      F_Description: undefined,
      F_Files: [],
      F_RequisitionUserId: undefined,
      F_RequisitionTime: undefined,
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
    },
    tableField6eed25outerActiveKey: [0],
    tableField6eed25innerActiveKey: [],
    tableField6eed25DefaultExpandAll: true,
    selectedtableField6eed25RowKeys: [],
    tableFieldddababouterActiveKey: [0],
    tableFieldddababinnerActiveKey: [],
    tableFieldddababDefaultExpandAll: true,
    selectedtableFieldddababRowKeys: [],
    dataRule: {
      F_CustomerName: [{ required: true, message: '请输入名称', trigger: 'blur' }],
    },
    optionsObj: {
      F_CustomerTagsProps: { label: 'fullName', value: 'id' },
    },
    currVmodel: '',
    currTableConf: {},
    tableRows: {
      tableField6eed25: {
        enabledmark: undefined,
        F_Region: [],
        F_Address: undefined,
        F_IsDefault: 0,
        F_CreatorTime: undefined,
      },
      tableFieldddabab: {
        enabledmark: undefined,
        F_ContactName: undefined,
        F_ContactPhone: undefined,
        F_IsDefault: 0,
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
    state.continueText = !data.id ? '确定并新增' : '确定并继续';
    setFormProps({ continueLoading: false });
    state.dataForm.id = data.id;
    openModal();
    state.allList = data.allList;
    state.currIndex = state.allList.length && data.id ? state.allList.findIndex(item => item.id === data.id) : 0;
    nextTick(() => {
      getForm().resetFields();
      state.dataForm.tableField6eed25 = [];
      state.dataForm.tableFieldddabab = [];
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
        F_CustomerName: undefined,
        F_CustomerCode: undefined,
        F_QRCode: undefined,
        F_IsPublic: undefined,
        F_CustomerTags: undefined,
        F_ShareUsers: undefined,
        F_IsFollow: undefined,
        F_State: 0,
        tableField6eed25: [],
        tableFieldddabab: [],
        F_Description: undefined,
        F_Files: [],
        F_RequisitionUserId: undefined,
        F_RequisitionTime: undefined,
        F_CreatorUserId: undefined,
        F_CreatorTime: undefined,
      };
      getF_CustomerTagsOptions();
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
      for (let i = 0; i < state.dataForm.tableField6eed25.length; i++) {
        const element = state.dataForm.tableField6eed25[i];
        element.jnpfId = buildUUID();
      }
      for (let i = 0; i < state.dataForm.tableFieldddabab.length; i++) {
        const element = state.dataForm.tableFieldddabab[i];
        element.jnpfId = buildUUID();
      }
      getF_CustomerTagsOptions();
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
  function getF_CustomerTagsOptions() {
    getDictionaryDataSelector('2009094067723571200').then(res => {
      state.optionsObj.F_CustomerTagsOptions = res.data.list;
    });
  }
  function addtableField6eed25Row() {
    let item = {
      jnpfId: buildUUID(),
      F_Region: [],
      F_Address: undefined,
      F_IsDefault: 0,
      F_CreatorTime: undefined,
    };
    state.dataForm.tableField6eed25.push(item);
    state.tableField6eed25innerActiveKey.push(item.jnpfId);
  }
  function removetableField6eed25Row(index, showConfirm = false) {
    if (showConfirm) {
      createConfirm({
        iconType: 'warning',
        title: '提示',
        content: '此操作将永久删除该数据, 是否继续?',
        onOk: () => {
          state.dataForm.tableField6eed25.splice(index, 1);
        },
      });
    } else {
      state.dataForm.tableField6eed25.splice(index, 1);
    }
  }
  function copytableField6eed25Row(index) {
    let item = cloneDeep(state.dataForm.tableField6eed25[index]);
    let copyData = {};
    for (let i = 0; i < unref(tableField6eed25Columns).length; i++) {
      const cur = unref(tableField6eed25Columns)[i];
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
    state.dataForm.tableField6eed25.push(copyItem);
    state.tableField6eed25innerActiveKey.push(copyItem.jnpfId);
  }
  function batchRemovetableField6eed25Row(showConfirm = false) {
    if (!state.selectedtableField6eed25RowKeys.length) return createMessage.error('请选择一条数据');
    const handleBatchRemove = () => {
      state.dataForm.tableField6eed25 = state.dataForm.tableField6eed25.filter(o => !state.selectedtableField6eed25RowKeys.includes(o.jnpfId));
      nextTick(() => {
        state.selectedtableField6eed25RowKeys = [];
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
  function addtableFieldddababRow() {
    let item = {
      jnpfId: buildUUID(),
      F_ContactName: undefined,
      F_ContactPhone: undefined,
      F_IsDefault: 0,
      F_CreatorTime: undefined,
    };
    state.dataForm.tableFieldddabab.push(item);
    state.tableFieldddababinnerActiveKey.push(item.jnpfId);
  }
  function removetableFieldddababRow(index, showConfirm = false) {
    if (showConfirm) {
      createConfirm({
        iconType: 'warning',
        title: '提示',
        content: '此操作将永久删除该数据, 是否继续?',
        onOk: () => {
          state.dataForm.tableFieldddabab.splice(index, 1);
        },
      });
    } else {
      state.dataForm.tableFieldddabab.splice(index, 1);
    }
  }
  function copytableFieldddababRow(index) {
    let item = cloneDeep(state.dataForm.tableFieldddabab[index]);
    let copyData = {};
    for (let i = 0; i < unref(tableFieldddababColumns).length; i++) {
      const cur = unref(tableFieldddababColumns)[i];
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
    state.dataForm.tableFieldddabab.push(copyItem);
    state.tableFieldddababinnerActiveKey.push(copyItem.jnpfId);
  }
  function batchRemovetableFieldddababRow(showConfirm = false) {
    if (!state.selectedtableFieldddababRowKeys.length) return createMessage.error('请选择一条数据');
    const handleBatchRemove = () => {
      state.dataForm.tableFieldddabab = state.dataForm.tableFieldddabab.filter(o => !state.selectedtableFieldddababRowKeys.includes(o.jnpfId));
      nextTick(() => {
        state.selectedtableFieldddababRowKeys = [];
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
