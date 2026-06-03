<template>
  <HeaderContainer :formConf="formConf" @onNodeNameChange="onConnectNameChange(props.formConf?.nodeId, $event)" />
  <div class="condition-main overflow-auto p-15px">
    <div class="mb-10px" v-if="formConf.conditions?.length">
      <jnpf-radio v-model:value="formConf.matchLogic" :options="logicOptions" optionType="button" button-style="solid" />
    </div>
    <div class="condition-item" v-for="(item, index) in formConf.conditions" :key="index">
      <div class="condition-item-title">
        <div>条件组</div>
        <i class="icon-ym icon-ym-nav-close" @click="delGroup(index)"></i>
      </div>
      <div class="condition-item-content">
        <div class="condition-item-cap">
          以下条件全部执行：
          <jnpf-radio v-model:value="item.logic" :options="logicOptions" optionType="button" button-style="solid" size="small" />
        </div>
        <a-row :gutter="8" v-for="(child, childIndex) in item.groups" :key="index + childIndex" wrap class="mb-10px">
          <a-col :span="7" class="!flex items-center">
            <jnpf-select v-model:value="child.fieldType" :options="conditionTypeOptions" @change="onFieldTypeChange(child)" />
          </a-col>
          <a-col :span="9" class="!flex items-center">
            <jnpf-select
              v-model:value="child.field"
              :options="usedFormItems"
              allowClear
              showSearch
              :fieldNames="{ options: 'options1' }"
              class="!flex-1"
              @change="(val, data) => onFieldChange(child, val, data, index, childIndex)"
              v-if="child.fieldType === 1" />
            <a-button @click="editFormula(child, index, childIndex)" class="!flex-1" v-if="child.fieldType === 3">公式编辑</a-button>
          </a-col>
          <a-col :span="8">
            <jnpf-select class="w-full" v-model:value="child.symbol" :options="flowSymbolOptions" @change="(val, data) => onSymbolChange(child, val, data)" />
          </a-col>
          <a-col :span="7" class="mt-10px">
            <jnpf-select
              v-model:value="child.fieldValueType"
              :options="getConditionTypeOptions"
              :disabled="child.disabled"
              @change="onFieldValueTypeChange(child)" />
          </a-col>
          <a-col :span="16" class="!flex items-center mt-10px">
            <jnpf-select
              v-model:value="child.fieldValue"
              :options="usedFormItems"
              allowClear
              showSearch
              :fieldNames="{ options: 'options1' }"
              class="flex-1"
              @change="(val, data) => onFieldValueChange(child, val, data)"
              v-if="child.fieldValueType === 1" />
            <div class="flex-1 w-150px" v-if="child.fieldValueType === 2">
              <template v-if="child.jnpfKey === 'inputNumber'">
                <a-input-number v-model:value="child.fieldValue" placeholder="请输入" :precision="child.precision" :disabled="child.disabled" />
              </template>
              <template v-else-if="child.jnpfKey === 'calculate'">
                <a-input-number v-model:value="child.fieldValue" placeholder="请输入" :precision="2" :disabled="child.disabled" />
              </template>
              <template v-else-if="['rate', 'slider'].includes(child.jnpfKey)">
                <a-input-number v-model:value="child.fieldValue" placeholder="请输入" :disabled="child.disabled" />
              </template>
              <template v-else-if="child.jnpfKey === 'switch'">
                <jnpf-switch v-model:value="child.fieldValue" :disabled="child.disabled" />
              </template>
              <template v-else-if="child.jnpfKey === 'timePicker'">
                <jnpf-time-picker v-model:value="child.fieldValue" :format="child.format || 'HH:mm:ss'" allowClear :disabled="child.disabled" />
              </template>
              <template v-else-if="['datePicker', 'createTime', 'modifyTime'].includes(child.jnpfKey)">
                <jnpf-date-picker
                  v-model:value="child.fieldValue"
                  :format="child.format || 'YYYY-MM-DD HH:mm:ss'"
                  allowClear
                  :disabled="child.disabled"
                  @change="onConditionDateChange($event, child)" />
              </template>
              <template v-else-if="['organizeSelect', 'currOrganize'].includes(child.jnpfKey)">
                <jnpf-organize-select
                  v-model:value="child.fieldValue"
                  allowClear
                  :disabled="child.disabled"
                  @change="(val, data) => onConditionOrganizeChange(child, val, data)" />
              </template>
              <template v-else-if="['depSelect'].includes(child.jnpfKey)">
                <jnpf-dep-select
                  v-model:value="child.fieldValue"
                  allowClear
                  :disabled="child.disabled"
                  @change="(val, data) => onConditionObjChange(child, val, data)" />
              </template>
              <template v-else-if="child.jnpfKey === 'roleSelect'">
                <jnpf-role-select
                  v-model:value="child.fieldValue"
                  allowClear
                  :disabled="child.disabled"
                  @change="(val, data) => onConditionObjChange(child, val, data)" />
              </template>
              <template v-else-if="child.jnpfKey === 'groupSelect'">
                <jnpf-group-select
                  v-model:value="child.fieldValue"
                  allowClear
                  :disabled="child.disabled"
                  @change="(val, data) => onConditionObjChange(child, val, data)" />
              </template>
              <template v-else-if="['posSelect', 'currPosition'].includes(child.jnpfKey)">
                <jnpf-pos-select
                  v-model:value="child.fieldValue"
                  allowClear
                  :disabled="child.disabled"
                  @change="(val, data) => onConditionObjChange(child, val, data)" />
              </template>
              <template v-else-if="['userSelect', 'createUser', 'modifyUser'].includes(child.jnpfKey)">
                <jnpf-user-select
                  v-model:value="child.fieldValue"
                  hasSys
                  allowClear
                  :disabled="child.disabled"
                  @change="(val, data) => onConditionObjChange(child, val, data)" />
              </template>
              <template v-else-if="['usersSelect'].includes(child.jnpfKey)">
                <jnpf-users-select
                  v-model:value="child.fieldValue"
                  allowClear
                  :disabled="child.disabled"
                  @change="(val, data) => onConditionObjChange(child, val, data)" />
              </template>
              <template v-else-if="child.jnpfKey === 'areaSelect'">
                <jnpf-area-select
                  v-model:value="child.fieldValue"
                  :level="child.level"
                  allowClear
                  :disabled="child.disabled"
                  @change="(val, data) => onConditionListChange(child, val, data)" />
              </template>
              <template v-else>
                <a-input v-model:value="child.fieldValue" placeholder="请输入" allowClear :disabled="child.disabled" />
              </template>
            </div>
            <jnpf-select
              v-model:value="child.fieldValue"
              :options="getSystemFieldOptions"
              :fieldNames="{ label: 'label', options: 'options1' }"
              allowClear
              v-else-if="child.fieldValueType === 3" />
            <jnpf-select
              v-model:value="child.fieldValue"
              :options="getParameterList"
              :fieldNames="{ label: 'fieldName', value: 'fieldName', options: 'options1' }"
              allowClear
              v-else-if="child.fieldValueType === 4" />
          </a-col>
          <a-col :span="1" class="text-center mt-10px">
            <i class="icon-ym icon-ym-btn-clearn" @click="delItem(index, childIndex)" />
          </a-col>
        </a-row>
        <span class="link-text inline-block" @click="addItem(index)"><i class="icon-ym icon-ym-btn-add text-14px mr-4px"></i>添加条件</span>
      </div>
    </div>
    <span class="link-text inline-block" @click="addGroup()"><i class="icon-ym icon-ym-btn-add text-14px mr-4px"></i>添加条件组</span>
  </div>
  <FormulaModal @register="registerFormulaModal" @confirm="updateFormula" />
</template>

<script lang="ts" setup>
  import { watch, ref, computed, inject, unref } from 'vue';
  import { formatToDateTime } from '@/utils/dateUtil';
  import { cloneDeep } from 'lodash-es';
  import { useModal } from '@/components/Modal';
  import { systemFieldOptions, conditionTypeOptions, conditionTypeOptions1, symbolOptions, logicOptions } from '../helper/define';
  import HeaderContainer from './components/HeaderContainer.vue';
  import FormulaModal from './components/FormulaModal.vue';
  import { getExternalLabelMid } from 'bpmn-js/lib/util/LabelUtil';
  import { NodeUtils } from '../bpmn/utils/nodeUtil';

  defineOptions({ inheritAttrs: false });

  const [registerFormulaModal, { openModal: openFormulaModal }] = useModal();
  const props = defineProps(['type', 'formConf', 'usedFormItems', 'updateJnpfData', 'formFieldsOptions', 'sourceIsStart', 'onConnectNameChange']);
  const emptyChildItem = {
    fieldName: '',
    symbolName: '',
    fieldValue: undefined,
    fieldType: 1,
    fieldValueType: 2,
    fieldLabel: '',
    fieldValueJnpfKey: '',
    logicName: '并且',
    field: '',
    symbol: '',
    logic: '&&',
    jnpfKey: '',
    cellKey: +new Date(),
  };
  const emptyItem = { logic: 'and', groups: [emptyChildItem] };
  const activeIndex = ref(0);
  const activeChildIndex = ref(0);
  const flowSymbolOptions = [...symbolOptions, { id: 'null', fullName: '为空' }, { id: 'notNull', fullName: '不为空' }];
  const bpmn: (() => string | undefined) | undefined = inject('bpmn');
  const getBpmn = computed(() => (bpmn as () => any)());
  const getJnpfGlobalData = computed(() => {
    const jnpfData: any = unref(getBpmn).get('jnpfData');
    return jnpfData?.getValue('global') || {};
  });
  const getSystemFieldOptions = computed(() => systemFieldOptions.map(o => ({ ...o, label: o.fullName ? o.id + '(' + o.fullName + ')' : o.id })));
  const getParameterList = computed(() => unref(getJnpfGlobalData).globalParameterList || []);
  const getConditionTypeOptions = computed(() => (props.sourceIsStart ? conditionTypeOptions1.filter(o => o.id != 3) : conditionTypeOptions1));

  watch(
    () => props.formConf,
    () => props.updateJnpfData(),
    { deep: true, immediate: true },
  );

  watch(
    () => props.formConf,
    () => updateConnectLabel(),
    { deep: true, immediate: true },
  );
  // 修改线条label
  function updateConnectLabel() {
    let modeling = unref(getBpmn).get('modeling');
    let jnpfData = unref(getBpmn).get('jnpfData');
    let elementRegistry: any = unref(getBpmn).get('elementRegistry');
    let connect = elementRegistry.get(props.formConf?.nodeId);
    // 添加空值检查：如果连线元素不存在则直接返回
    if (!connect) return;
    let connectLabel = elementRegistry.get(props.formConf?.nodeId + '_label');
    let labelCenter = getExternalLabelMid(connect);
    let text = NodeUtils.getConditionsContent(props.formConf.conditions, props.formConf.matchLogic);
    labelCenter = NodeUtils.updateLabelWaypoints(connect, elementRegistry, jnpfData, props.type);
    if (connectLabel) {
      connectLabel.text = text;
      connectLabel.width = 128;
      connectLabel.height = 28;
      if (Object.keys(labelCenter).length > 0) {
        connectLabel.x = labelCenter.x;
        connectLabel.y = labelCenter.y;
      }
      connectLabel['nodeName'] = props.formConf.nodeName;
      modeling.updateProperties(connectLabel, {});
    } else {
      if (text?.trim()) {
        let existingElement = unref(getBpmn)
          .get('elementRegistry')
          .get(connect.id + '_label');
        if (!existingElement) {
          labelCenter = NodeUtils.updateLabelWaypoints(connect, elementRegistry, jnpfData, props.type);
          let labelElement = modeling.createLabel(connect, labelCenter, {
            id: connect.id + '_label',
            businessObject: connect.businessObject,
            text: '',
            wnType: 'condition',
            di: connect.di,
            width: 128,
            height: 0,
          });
          labelElement.x = labelCenter.x;
          labelElement.y = labelCenter.y;
          modeling.updateProperties(labelElement, {});
        }
      }
    }
    return null;
  }

  function addItem(index) {
    props.formConf.conditions[index].groups.push(cloneDeep(emptyChildItem));
  }
  function delItem(index, childIndex) {
    props.formConf.conditions[index].groups.splice(childIndex, 1);
    if (!props.formConf.conditions[index].groups.length) delGroup(index);
  }
  function addGroup() {
    props.formConf.conditions.push(cloneDeep(emptyItem));
  }
  function delGroup(index) {
    props.formConf.conditions.splice(index, 1);
  }
  function editFormula(item, index, childIndex) {
    activeIndex.value = index;
    activeChildIndex.value = childIndex;
    openFormulaModal(true, { value: item.field, fieldsOptions: props.formFieldsOptions });
  }
  function updateFormula(formula) {
    props.formConf.conditions[activeIndex.value].groups[activeChildIndex.value].field = formula;
    props.formConf.conditions[activeIndex.value].groups[activeChildIndex.value].fieldName = formula;
  }
  function onFieldTypeChange(item) {
    item.field = '';
    handleNull(item);
  }
  function onFieldChange(item, val, data, index, childIndex) {
    if (!val) return handleNull(item);
    item.fieldName = data.__config__.label;
    item.jnpfKey = data.__config__.jnpfKey;
    const newItem = cloneDeep(emptyChildItem);
    for (let key of Object.keys(newItem)) {
      newItem[key] = item[key];
    }
    item = { ...newItem, ...data, disabled: item.disabled };
    if (item.fieldValueType == 2) {
      item.fieldValue = undefined;
      item.fieldLabel = '';
      item.fieldValueJnpfKey = '';
    }
    props.formConf.conditions[index].groups[childIndex] = item;
  }
  function handleNull(item) {
    item.fieldName = '';
    item.jnpfKey = '';
    if (item.fieldValueType == 2) {
      item.fieldValue = undefined;
      item.fieldLabel = '';
      item.fieldValueJnpfKey = '';
    }
  }
  function onSymbolChange(item, val, data) {
    item.symbolName = val ? data.fullName : '';
    item.disabled = ['null', 'notNull'].includes(val);
    if (['null', 'notNull'].includes(val)) {
      item.fieldValueType = 2;
      item.fieldValue = '';
      item.fieldValueJnpfKey = '';
    }
  }
  function onFieldValueChange(item, val, data) {
    item.fieldLabel = val ? data.fullName : '';
    item.fieldValueJnpfKey = val ? data.__config__.jnpfKey : '';
  }
  function onFieldValueTypeChange(item) {
    item.fieldValue = '';
    item.fieldLabel = '';
    item.fieldValueJnpfKey = '';
  }
  function onConditionDateChange(val, item) {
    if (!val) return (item.fieldLabel = '');
    const format = item.format || 'YYYY-MM-DD HH:mm:ss';
    item.fieldLabel = formatToDateTime(val, format);
  }
  function onConditionListChange(item, val, data) {
    if (!val) return (item.fieldLabel = '');
    const labelList = data.map(o => o.fullName);
    item.fieldLabel = labelList.join('/');
  }
  function onConditionOrganizeChange(item, val, data) {
    if (!val) return (item.fieldLabel = '');
    item.fieldLabel = data.organize || '';
  }
  function onConditionObjChange(item, val, data) {
    if (!val) return (item.fieldLabel = '');
    item.fieldLabel = data.fullName || '';
  }
</script>
