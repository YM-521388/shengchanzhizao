<template>
  <HeaderContainer :formConf="formConf" @onNodeNameChange="updateBpmnProperties('nodeName', $event)" />
  <a-form :colon="false" :model="formConf" class="config-content" layout="vertical">
    <a-form-item label="接收人">
      <a-input-group compact>
        <jnpf-select v-model:value="formConf.msgUserIdsSourceType" :options="sourceTypeOptions" class="!w-100px" @change="formConf.msgUserIds = ''" />
        <div style="width: calc(100% - 100px)">
          <jnpf-select
            v-model:value="formConf.msgUserIds"
            :options="getUserDataSourceForm"
            :fieldNames="{ options: 'children' }"
            showSearch
            allowClear
            class="w-full"
            v-if="formConf.msgUserIdsSourceType == 1" />
          <jnpf-users-select v-model:value="formConf.msgUserIds" multiple v-else />
        </div>
      </a-input-group>
    </a-form-item>
    <a-form-item label="通知内容">
      <div class="ant-form-item-label"><label class="ant-form-item-no-colon">发送配置</label></div>
      <msg-modal :value="formConf.msgId" :title="formConf.msgName" messageSource="3" @change="(val, data) => onMsgChange(val, data)" />
      <div class="ant-form-item-label !mt-12px"><label class="ant-form-item-no-colon">参数设置</label></div>
      <a-table :data-source="formConf.msgTemplateJson" :columns="msgTemplateJsonColumns" size="small" :pagination="false">
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'msgTemplateName'">
            <p class="link-text !w-80px" @click="goMsgDetail(record.templateId)">{{ record.msgTemplateName }}</p>
          </template>
          <div v-for="(item, index) in record.paramJson" :key="index" :class="{ '!mt-8px': index > 0 }">
            <template v-if="column.key === 'paramJson'">
              <div class="parameter-box !w-80px leading-32px" :title="item.field + '(' + item.fieldName + ')'"> {{ item.field }}({{ item.fieldName }}) </div>
            </template>
            <template v-if="column.key === 'sourceType'">
              <jnpf-select class="!w-75px" v-model:value="item.sourceType" :options="sourceTypeOptions" @change="item.relationField = ''" />
            </template>
            <template v-if="column.key === 'field'">
              <jnpf-select
                v-model:value="item.relationField"
                :options="dataSourceForm"
                allowClear
                showSearch
                :fieldNames="{ options: 'children' }"
                optionLabelProp="label"
                class="!w-140px"
                @change="onRelationFieldChange($event, item)"
                v-if="item.sourceType == 1" />
              <jnpf-input v-model:value="item.relationField" placeholder="请输入" v-else />
            </template>
          </div>
        </template>
        <template #emptyText>
          <p class="leading-60px">暂无数据</p>
        </template>
      </a-table>
    </a-form-item>
  </a-form>
  <MsgTemplateDetail @register="registerDetail" />
</template>
<script lang="ts" setup>
  import { computed, watch } from 'vue';
  import { useModal } from '@/components/Modal';
  import HeaderContainer from './components/HeaderContainer.vue';
  import MsgModal from '@/components/FlowProcess/src/propPanel/components/MsgModal.vue';
  import MsgTemplateDetail from '@/components/FlowProcess/src/propPanel/components/MsgTemplateDetail.vue';
  import { cloneDeep } from 'lodash-es';

  defineOptions({ inheritAttrs: false });

  const props = defineProps(['formFieldsOptions', 'formConf', 'updateJnpfData', 'dataSourceForm', 'updateBpmnProperties']);
  const [registerDetail, { openModal: openDetailModal }] = useModal();
  const msgTemplateJsonColumns = [
    { title: '模板名称', dataIndex: 'msgTemplateName', key: 'msgTemplateName' },
    { title: '参数名称', dataIndex: 'paramJson', key: 'paramJson' },
    { title: '参数来源', dataIndex: 'sourceType', key: 'sourceType', width: 85 },
    { title: '参数值', dataIndex: 'field', key: 'field', width: 150 },
  ];
  const sourceTypeOptions = [
    { id: 1, fullName: '字段' },
    { id: 2, fullName: '自定义' },
  ];
  const allowList = [
    'organizeSelect',
    'depSelect',
    'posSelect',
    'userSelect',
    'usersSelect',
    'roleSelect',
    'groupSelect',
    'createUser',
    'modifyUser',
    'currOrganize',
    'currPosition',
  ];
  const getUserDataSourceForm = computed(() => filterDataSourceForm(allowList));

  watch(
    () => props.formConf,
    () => props.updateJnpfData(),
    { deep: true, immediate: true },
  );

  function onMsgChange(val, row) {
    if (!val) {
      props.formConf.msgId = '';
      props.formConf.msgName = '';
      props.formConf.msgTemplateJson = [];
      props.formConf.content = '';
      props.updateBpmnProperties('elementBodyName', '');
      return;
    }
    if (props.formConf.msgId === val) return;
    props.formConf.msgId = val;
    props.formConf.msgName = row.fullName;
    props.formConf.msgTemplateJson = (row.templateJson || []).map(o => ({ ...o, paramJson: (o.paramJson || []).map(e => ({ ...e, sourceType: 1 })) })) || [];
    props.formConf.content = `执行[${props.formConf.msgName}]的消息通知`;
    props.updateBpmnProperties('elementBodyName', props.formConf.content);
  }
  function onRelationFieldChange(val, row) {
    if (!val) return;
    let list = props.formFieldsOptions.filter(o => o.id === val);
    if (!list.length) return;
    let item = list[0];
    row.isSubTable = item.__config__ && item.__config__.isSubTable ? item.__config__.isSubTable : false;
  }
  function goMsgDetail(id) {
    openDetailModal(true, { id });
  }
  function filterDataSourceForm(notSupportList) {
    return cloneDeep(props.dataSourceForm)
      .map(item => ({
        ...item,
        children: item.children.filter(o => o?.__config__?.jnpfKey && notSupportList.includes(o.__config__.jnpfKey)),
      }))
      .filter(item => item.children?.length);
  }
</script>
