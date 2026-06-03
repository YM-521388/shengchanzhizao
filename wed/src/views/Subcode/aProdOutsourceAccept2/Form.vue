<template>
  <BasicModal v-bind="$attrs" @register="registerModal" width="30%" :minHeight="100" okText="确定" cancelText="取消" @ok="handleSubmit" :closeFunc="onClose">
    <template #title>
      <a-space :size="10">
        <div class="text-16px font-medium">{{ title }}</div>
        <a-space-compact size="small" block v-if="dataForm.id">
          <!-- <a-tooltip :title="t('common.prevRecord')">
            <a-button size="small" :disabled="getPrevDisabled" @click="handlePrev">
              <i class="icon-ym icon-ym-caret-left text-10px"></i>
            </a-button>
          </a-tooltip>
          <a-tooltip :title="t('common.nextRecord')">
            <a-button size="small" :disabled="getNextDisabled" @click="handleNext">
              <i class="icon-ym icon-ym-caret-right text-10px"></i>
            </a-button>
          </a-tooltip> -->
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
          <!-- <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_OutsourceId')">
            <a-form-item name="F_OutsourceId" :labelCol="{ style: { width: '100px' } }">
              <template #label>外协工单</template>
              <JnpfPopupSelect
                v-model:value="dataForm.F_OutsourceId"
                placeholder="请选择"
                :templateJson="optionsObj.F_OutsourceIdTemplateJson"
                allowClear
                field="F_OutsourceId"
                interfaceId="2014180492177444864"
                :columnOptions="optionsObj.F_OutsourceIdOptions"
                relationField="F_OutsourceNo"
                propsValue="F_Id"
                :pageSize="20"
                popupType="dialog"
                popupTitle="选择数据"
                popupWidth="70%"
                disabled
                :style="{
                  width: '100%',
                }"
                :extraOptions="optionsObj.F_OutsourceId" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_PassNum')">
            <a-form-item name="F_PassNum" :labelCol="{ style: { width: '100px' } }">
              <template #label>合格数量</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_PassNum"
                placeholder="请输入"
                :controls="false"
                disabled
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_FailNum')">
            <a-form-item name="F_FailNum" :labelCol="{ style: { width: '100px' } }">
              <template #label>不合格数量</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_FailNum"
                placeholder="请输入"
                :controls="false"
                disabled
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col> -->
          <!-- <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_OutsourceType')">
            <a-form-item name="F_OutsourceType" :labelCol="{ style: { width: '100px' } }">
              <template #label>外协类型</template>
              <JnpfInput
                v-model:value="dataForm.F_OutsourceType"
                placeholder="请输入"
                allowClear
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col> -->
          <a-col :span="24" class="ant-col-item">
            <a-form-item name="F_SettleUnitPrice" :labelCol="{ style: { width: '100px' } }">
              <template #label>结算金额</template>
              <JnpfInputNumber
                v-model:value="dataForm.F_SettleUnitPrice"
                :precision="2"
                placeholder="请输入"
                :controls="false"
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <!-- <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_SettleStatus')">
            <a-form-item name="F_SettleStatus" :labelCol="{ style: { width: '100px' } }" :rules="[{ required: true, message: '请选择结算状态' }]">
              <template #label>结算状态</template>
              <JnpfRadio
                v-model:value="dataForm.F_SettleStatus"
                :options="optionsObj.F_SettleStatusOptions"
                :fieldNames="optionsObj.F_SettleStatusProps"
                direction="horizontal"
                optionType="default"
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_SettleUserId')">
            <a-form-item name="F_SettleUserId" :labelCol="{ style: { width: '100px' } }">
              <template #label>结算人</template>
              <JnpfUserSelect
                v-model:value="dataForm.F_SettleUserId"
                placeholder="请选择"
                selectType="all"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col>
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_SettleTime')">
            <a-form-item name="F_SettleTime" :labelCol="{ style: { width: '100px' } }">
              <template #label>结算时间</template>
              <JnpfDatePicker
                v-model:value="dataForm.F_SettleTime"
                placeholder="请选择"
                format="yyyy-MM-dd"
                allowClear
                :style="{
                  width: '100%',
                }" />
            </a-form-item>
          </a-col> -->
          <a-col :span="8" class="ant-col-item" v-if="hasFormP('F_SettleFiles')">
            <a-form-item name="F_SettleFiles" :labelCol="{ style: { width: '100px' } }">
              <template #label>结算附件</template>
              <JnpfUploadFile
                v-model:value="dataForm.F_SettleFiles"
                buttonText="点击上传"
                pathType="defaultPath"
                :fileSize="10"
                sizeUnit="MB"
                :limit="9"
                :sortRule="[]"
                timeFormat="YYYY" />
            </a-form-item>
          </a-col>
          <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_SettleDesc')">
            <a-form-item name="F_SettleDesc" :labelCol="{ style: { width: '100px' } }">
              <template #label>结算备注</template>
              <JnpfTextarea
                v-model:value="dataForm.F_SettleDesc"
                placeholder="请输入"
                allowClear
                :autoSize="{
                  minRows: 4,
                  maxRows: 4,
                }"
                :style="{
                  width: '100%',
                }"
                :showCount="false" />
            </a-form-item>
          </a-col>
          <!-- <a-col :span="24" class="ant-col-item" v-if="hasFormP('F_CreatorUserId')">
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
          </a-col> -->
          <!-- <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="选择验收内容" :bordered="false" />
              <a-table
                :data-source="dataForm.tableField561a64"
                :columns="tableField561a64Columns"
                size="small"
                rowKey="jnpfId"
                :pagination="false"
                :scroll="{ x: 'max-content' }"
                :rowSelection="gettableField561a64RowSelection">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                  <template v-if="column.key === 'F_AcceptId'">
                    <JnpfPopupSelect
                      v-model:value="record.F_AcceptId"
                      placeholder="请选择"
                      :templateJson="optionsObj.tableField561a64_F_AcceptIdTemplateJson"
                      allowClear
                      :field="'F_AcceptId' + index"
                      interfaceId="2014209857888063488"
                      :columnOptions="optionsObj.tableField561a64_F_AcceptIdOptions"
                      relationField="F_Title"
                      propsValue="F_Id"
                      :pageSize="20"
                      popupType="dialog"
                      popupTitle="选择数据"
                      popupWidth="60%"
                      :style="{
                        width: '100%',
                      }"
                      :extraOptions="optionsObj.tableField561a64_F_AcceptId" />
                  </template>
                  <template v-if="column.key === 'F_SortCode'">
                    <JnpfInputNumber
                      v-model:value="record.F_SortCode"
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
                      <a-button class="action-btn" type="link" @click="copytableField561a64Row(index)" size="small">{{
                        t('common.copyText', '复制')
                      }}</a-button>
                      <a-button class="action-btn" type="link" color="error" @click="removetableField561a64Row(index, true)" size="small">{{
                        t('common.delText', '删除')
                      }}</a-button>
                    </a-space>
                  </template>
                </template>
              </a-table>
              <a-space class="input-table-footer-btn">
                <a-button type="primary" preIcon="icon-ym icon-ym-btn-add" @click="addtableField561a64Row">{{ t('common.add1Text', '添加') }}</a-button>
                <a-button type="danger" preIcon="icon-ym icon-ym-btn-clearn" @click="batchRemovetableField561a64Row(true)">{{
                  t('common.batchDelText', '批量删除')
                }}</a-button>
              </a-space>
            </a-form-item>
          </a-col> -->
          <!-- <a-col :span="24" class="ant-col-item">
            <a-form-item label="">
              <JnpfGroupTitle content="操作日志" :bordered="false" />
              <a-table
                :data-source="dataForm.tableField4b440e"
                :columns="tableField4b440eColumns"
                size="small"
                rowKey="jnpfId"
                :pagination="false"
                :scroll="{ x: 'max-content' }"
                :rowSelection="gettableField4b440eRowSelection">
                <template #headerCell="{ column }">
                  <span class="required-sign" v-if="column.required">*</span>{{ column.title }}<BasicHelp :text="column.tipLabel" v-if="column.tipLabel"
                /></template>
                <template #bodyCell="{ column, record, index }">
                  <template v-if="column.key === 'index'">{{ index + 1 }}</template>
                  <template v-if="column.key === 'F_Title'">
                    <JnpfInput
                      v-model:value="record.F_Title"
                      placeholder="请输入"
                      allowClear
                      :style="{
                        width: '100%',
                      }"
                      :showCount="false" />
                  </template>
                  <template v-if="column.key === 'F_Content'">
                    <JnpfTextarea
                      v-model:value="record.F_Content"
                      placeholder="请输入"
                      allowClear
                      :autoSize="{
                        minRows: 4,
                        maxRows: 4,
                      }"
                      :style="{
                        width: '100%',
                      }"
                      :showCount="false" />
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
                      <a-button class="action-btn" type="link" @click="copytableField4b440eRow(index)" size="small">{{
                        t('common.copyText', '复制')
                      }}</a-button>
                      <a-button class="action-btn" type="link" color="error" @click="removetableField4b440eRow(index, true)" size="small">{{
                        t('common.delText', '删除')
                      }}</a-button>
                    </a-space>
                  </template>
                </template>
              </a-table>
              <a-space class="input-table-footer-btn">
                <a-button type="primary" preIcon="icon-ym icon-ym-btn-add" @click="addtableField4b440eRow">{{ t('common.add1Text', '添加') }}</a-button>
                <a-button type="danger" preIcon="icon-ym icon-ym-btn-clearn" @click="batchRemovetableField4b440eRow(true)">{{
                  t('common.batchDelText', '批量删除')
                }}</a-button>
              </a-space>
            </a-form-item>
          </a-col> -->
        </a-row>
      </a-form>
    </a-row>
  </BasicModal>
  <SelectModal :config="state.currTableConf" :formData="state.dataForm" ref="selectModal" @select="addForSelect" />
</template>
<script lang="ts" setup>
  import { create, update, getInfo, updateSettlement } from './helper/api';
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
    tableField561a64outerActiveKey: number[];
    tableField561a64innerActiveKey: string[];
    tableField561a64DefaultExpandAll: boolean;
    selectedtableField561a64RowKeys: any[];
    tableField4b440eouterActiveKey: number[];
    tableField4b440einnerActiveKey: string[];
    tableField4b440eDefaultExpandAll: boolean;
    selectedtableField4b440eRowKeys: any[];
    title: string;
    continueText: string;
    allList: any[];
    currIndex: number;
    isContinue: boolean;
    submitType: number;
    showContinueBtn: boolean;
    defaultSettle: boolean;
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
  const tableField561a64Columns: any[] = computed(() => {
    let list = [
      {
        title: '验收内容',
        dataIndex: 'F_AcceptId',
        key: 'F_AcceptId',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_AcceptId',
        isSystemControl: false,
      },
      {
        title: '排序',
        dataIndex: 'F_SortCode',
        key: 'F_SortCode',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_SortCode',
        isSystemControl: false,
      },
      {
        title: '创建用户',
        dataIndex: 'F_CreatorUserId',
        key: 'F_CreatorUserId',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_CreatorUserId',
        isSystemControl: true,
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
    const noColumn = { title: t('component.table.index'), dataIndex: 'index', key: 'index', align: 'center', width: 50, fixed: 'left' };
    const actionColumn = { title: t('component.table.action'), dataIndex: 'action', key: 'action', align: 'center', width: 100, fixed: 'right' };
    let columns = [noColumn, ...columnList, actionColumn];
    const leftFixedList = columns.filter(o => o.fixed === 'left');
    const rightFixedList = columns.filter(o => o.fixed === 'right');
    const noFixedList = columns.filter(o => o.fixed !== 'left' && o.fixed !== 'right');
    return [...leftFixedList, ...noFixedList, ...rightFixedList];
  });
  const gettableField561a64RowSelection = computed(() => {
    const rowSelection = {
      selectedRowKeys: state.selectedtableField561a64RowKeys,
      onChange: (selectedRowKeys: string[]) => {
        state.selectedtableField561a64RowKeys = (selectedRowKeys || []).sort().reverse();
      },
    };
    return rowSelection;
  });
  const tableField4b440eColumns: any[] = computed(() => {
    let list = [
      {
        title: '标题',
        dataIndex: 'F_Title',
        key: 'F_Title',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Title',
        isSystemControl: false,
      },
      {
        title: '内容',
        dataIndex: 'F_Content',
        key: 'F_Content',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_Content',
        isSystemControl: false,
      },
      {
        title: '创建用户',
        dataIndex: 'F_CreatorUserId',
        key: 'F_CreatorUserId',
        tipLabel: '',
        required: false,
        align: 'left',
        span: '24',
        labelWidth: '',
        width: '',
        fixed: false,
        formP: 'F_CreatorUserId',
        isSystemControl: true,
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
    const noColumn = { title: t('component.table.index'), dataIndex: 'index', key: 'index', align: 'center', width: 50, fixed: 'left' };
    const actionColumn = { title: t('component.table.action'), dataIndex: 'action', key: 'action', align: 'center', width: 100, fixed: 'right' };
    let columns = [noColumn, ...columnList, actionColumn];
    const leftFixedList = columns.filter(o => o.fixed === 'left');
    const rightFixedList = columns.filter(o => o.fixed === 'right');
    const noFixedList = columns.filter(o => o.fixed !== 'left' && o.fixed !== 'right');
    return [...leftFixedList, ...noFixedList, ...rightFixedList];
  });
  const gettableField4b440eRowSelection = computed(() => {
    const rowSelection = {
      selectedRowKeys: state.selectedtableField4b440eRowKeys,
      onChange: (selectedRowKeys: string[]) => {
        state.selectedtableField4b440eRowKeys = (selectedRowKeys || []).sort().reverse();
      },
    };
    return rowSelection;
  });
  const state = reactive<State>({
    dataForm: {
      id: '',
      F_OutsourceId: undefined,
      F_PassNum: undefined,
      F_FailNum: undefined,
      F_OutsourceType: undefined,
      F_SettleStatus: '1',
      F_SettleUnitPrice: undefined,
      F_SettleUserId: '',
      F_SettleTime: undefined,
      F_SettleFiles: [],
      F_SettleDesc: undefined,
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
      tableField561a64: [],
      tableField4b440e: [],
    },
    tableField561a64outerActiveKey: [0],
    tableField561a64innerActiveKey: [],
    tableField561a64DefaultExpandAll: true,
    selectedtableField561a64RowKeys: [],
    tableField4b440eouterActiveKey: [0],
    tableField4b440einnerActiveKey: [],
    tableField4b440eDefaultExpandAll: true,
    selectedtableField4b440eRowKeys: [],
    dataRule: {
      F_OutsourceId: [
        {
          required: true,
          message: '请输入外协工单',
          trigger: 'change',
        },
      ],
      F_PassNum: [
        {
          required: true,
          message: '请输入合格数量',
          trigger: ['blur', 'change'],
        },
      ],
      F_FailNum: [
        {
          required: true,
          message: '请输入不合格数量',
          trigger: ['blur', 'change'],
        },
      ],
    },
    optionsObj: {
      F_OutsourceIdOptions: [
        {
          value: 'F_OutsourceNo',
          label: '外协工单编码',
        },
        {
          value: 'F_GoodsNo',
          label: '商品编码',
        },
        {
          value: 'F_GoodsName',
          label: '商品名称',
        },
        {
          value: 'F_SupplierName',
          label: '供应商姓名',
        },
        {
          value: 'F_ContactPerson',
          label: '联系人',
        },
        {
          value: 'F_ContactPhone',
          label: '联系人电话',
        },
        {
          value: 'F_DetailAddress',
          label: '详细地址',
        },
        {
          value: 'F_Price',
          label: '外协单价',
        },
        {
          value: 'F_DeliveryDate',
          label: '交货日期',
        },
        {
          value: 'F_State',
          label: '状态',
        },
      ],
      F_OutsourceIdTemplateJson: [],
      F_SettleStatusProps: { label: 'fullName', value: 'enCode' },
      tableField561a64_F_AcceptIdOptions: [],
      tableField561a64_F_AcceptIdTemplateJson: [],
      F_OutsourceId: [],
    },
    currVmodel: '',
    currTableConf: {},
    tableRows: {
      tableField561a64: {
        enabledmark: undefined,
        F_AcceptId: undefined,
        F_SortCode: undefined,
        F_CreatorUserId: undefined,
        F_CreatorTime: undefined,
      },
      tableField4b440e: {
        enabledmark: undefined,
        F_Title: undefined,
        F_Content: undefined,
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
    defaultSettle: false,
  });
  const { title, continueText, showContinueBtn, dataForm, submitType, dataRule, optionsObj } = toRefs(state);

  const getPrevDisabled = computed(() => state.currIndex === 0);
  const getNextDisabled = computed(() => state.currIndex === state.allList.length - 1);

  defineExpose({ init });

  function init(data) {
    state.submitType = 0;
    state.showContinueBtn = true;
    state.defaultSettle = !!data.settle;
    state.title = !data.id ? '新增' : '编辑';
    state.continueText = !data.id ? '确定并新增' : '确定并继续';
    setFormProps({ continueLoading: false });
    state.dataForm.id = data.id;
    openModal();
    state.allList = data.allList;
    state.currIndex = state.allList.length && data.id ? state.allList.findIndex(item => item.id === data.id) : 0;
    nextTick(() => {
      getForm().resetFields();
      state.dataForm.tableField561a64 = [];
      state.dataForm.tableField4b440e = [];
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
        F_OutsourceId: undefined,
        F_PassNum: undefined,
        F_FailNum: undefined,
        F_OutsourceType: undefined,
        F_SettleStatus: '1',
        F_SettleUnitPrice: undefined,
        F_SettleUserId: userInfo.userId ? userInfo.userId : '',
        F_SettleTime: undefined,
        F_SettleFiles: [],
        F_SettleDesc: undefined,
        F_CreatorUserId: undefined,
        F_CreatorTime: undefined,
        tableField561a64: [],
        tableField4b440e: [],
      };
      getF_SettleStatusOptions();
      if (state.defaultSettle) {
        // state.dataForm.F_SettleStatus = '1';
        // state.dataForm.F_SettleTime = dayjs(new Date()).startOf(getDateTimeUnit('yyyy-MM-dd')).valueOf();
      }
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
      for (let i = 0; i < state.dataForm.tableField561a64.length; i++) {
        const element = state.dataForm.tableField561a64[i];
        element.jnpfId = buildUUID();
      }
      for (let i = 0; i < state.dataForm.tableField4b440e.length; i++) {
        const element = state.dataForm.tableField4b440e[i];
        element.jnpfId = buildUUID();
      }
      getF_SettleStatusOptions();
      if (state.defaultSettle) {
        state.dataForm.F_SettleStatus = '1';
        state.dataForm.F_SettleTime = dayjs(new Date()).startOf(getDateTimeUnit('yyyy-MM-dd')).valueOf();
      }
      changeLoading(false);
    });
  }
  async function handleSubmit() {
    try {
      const values = await getForm()?.validate();
      if (!values) return;
      setFormProps({ confirmLoading: true });
      const formMethod = state.dataForm.id ? updateSettlement : create;
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
  function getF_SettleStatusOptions() {
    getDictionaryDataSelector('2014214471169478656').then(res => {
      state.optionsObj.F_SettleStatusOptions = res.data.list;
    });
  }
  function addtableField561a64Row() {
    let item = {
      jnpfId: buildUUID(),
      F_AcceptId: undefined,
      F_SortCode: undefined,
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
    };
    state.dataForm.tableField561a64.push(item);
    state.tableField561a64innerActiveKey.push(item.jnpfId);
  }
  function removetableField561a64Row(index, showConfirm = false) {
    if (showConfirm) {
      createConfirm({
        iconType: 'warning',
        title: '提示',
        content: '此操作将永久删除该数据, 是否继续?',
        onOk: () => {
          state.dataForm.tableField561a64.splice(index, 1);
        },
      });
    } else {
      state.dataForm.tableField561a64.splice(index, 1);
    }
  }
  function copytableField561a64Row(index) {
    let item = cloneDeep(state.dataForm.tableField561a64[index]);
    let copyData = {};
    for (let i = 0; i < unref(tableField561a64Columns).length; i++) {
      const cur = unref(tableField561a64Columns)[i];
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
    state.dataForm.tableField561a64.push(copyItem);
    state.tableField561a64innerActiveKey.push(copyItem.jnpfId);
  }
  function batchRemovetableField561a64Row(showConfirm = false) {
    if (!state.selectedtableField561a64RowKeys.length) return createMessage.error('请选择一条数据');
    const handleBatchRemove = () => {
      state.dataForm.tableField561a64 = state.dataForm.tableField561a64.filter(o => !state.selectedtableField561a64RowKeys.includes(o.jnpfId));
      nextTick(() => {
        state.selectedtableField561a64RowKeys = [];
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
  function addtableField4b440eRow() {
    let item = {
      jnpfId: buildUUID(),
      F_Title: undefined,
      F_Content: undefined,
      F_CreatorUserId: undefined,
      F_CreatorTime: undefined,
    };
    state.dataForm.tableField4b440e.push(item);
    state.tableField4b440einnerActiveKey.push(item.jnpfId);
  }
  function removetableField4b440eRow(index, showConfirm = false) {
    if (showConfirm) {
      createConfirm({
        iconType: 'warning',
        title: '提示',
        content: '此操作将永久删除该数据, 是否继续?',
        onOk: () => {
          state.dataForm.tableField4b440e.splice(index, 1);
        },
      });
    } else {
      state.dataForm.tableField4b440e.splice(index, 1);
    }
  }
  function copytableField4b440eRow(index) {
    let item = cloneDeep(state.dataForm.tableField4b440e[index]);
    let copyData = {};
    for (let i = 0; i < unref(tableField4b440eColumns).length; i++) {
      const cur = unref(tableField4b440eColumns)[i];
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
    state.dataForm.tableField4b440e.push(copyItem);
    state.tableField4b440einnerActiveKey.push(copyItem.jnpfId);
  }
  function batchRemovetableField4b440eRow(showConfirm = false) {
    if (!state.selectedtableField4b440eRowKeys.length) return createMessage.error('请选择一条数据');
    const handleBatchRemove = () => {
      state.dataForm.tableField4b440e = state.dataForm.tableField4b440e.filter(o => !state.selectedtableField4b440eRowKeys.includes(o.jnpfId));
      nextTick(() => {
        state.selectedtableField4b440eRowKeys = [];
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
