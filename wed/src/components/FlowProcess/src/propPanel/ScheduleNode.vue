<template>
  <HeaderContainer :formConf="formConf" @onNodeNameChange="updateBpmnProperties('nodeName', $event)" />
  <a-form :colon="false" :model="formConf" class="config-content" layout="vertical">
    <a-form-item label="标题">
      <a-input-group compact>
        <jnpf-select v-model:value="formConf.titleSourceType" :options="sourceTypeOptions" class="!w-100px" @change="formConf.title = ''" />
        <div style="width: calc(100% - 100px)">
          <jnpf-select
            v-model:value="formConf.title"
            :options="dataSourceForm"
            :fieldNames="{ options: 'children' }"
            showSearch
            allowClear
            class="w-full"
            @change="onTitleChange"
            v-if="formConf.titleSourceType == 1" />
          <jnpf-input v-model:value="formConf.title" placeholder="请输入" @change="onTitleChange" v-else />
        </div>
      </a-input-group>
    </a-form-item>
    <a-form-item label="内容">
      <a-input-group compact>
        <jnpf-select v-model:value="formConf.contentsSourceType" :options="sourceTypeOptions" class="!w-100px" @change="formConf.contents = ''" />
        <div style="width: calc(100% - 100px)">
          <jnpf-select
            v-model:value="formConf.contents"
            :options="dataSourceForm"
            :fieldNames="{ options: 'children' }"
            showSearch
            allowClear
            class="w-full"
            v-if="formConf.contentsSourceType == 1" />
          <jnpf-input v-model:value="formConf.contents" placeholder="请输入" v-else />
        </div>
      </a-input-group>
    </a-form-item>
    <a-form-item label="附件">
      <jnpf-upload-file v-model:value="formConf.files" />
    </a-form-item>
    <a-form-item label="全天">
      <jnpf-switch v-model:value="formConf.allDay" @change="onAllDayChange" />
    </a-form-item>
    <a-form-item label="开始时间">
      <div class="countersign-cell">
        <a-input-group compact>
          <jnpf-select v-model:value="formConf.startDaySourceType" :options="sourceTypeOptions" class="!w-100px" @change="formConf.startDay = ''" />
          <div style="width: calc(100% - 100px)">
            <jnpf-select
              v-model:value="formConf.startDay"
              :options="getDateDataSourceForm"
              :fieldNames="{ options: 'children' }"
              showSearch
              allowClear
              class="w-full"
              v-if="formConf.startDaySourceType == 1" />
            <jnpf-date-picker v-model:value="formConf.startDay" format="YYYY-MM-DD" v-else />
          </div>
        </a-input-group>
        <jnpf-time-picker
          class="ml-5px !w-180px"
          v-model:value="formConf.startTime"
          format="HH:mm"
          :minuteStep="5"
          placeholder="请选择"
          v-if="!formConf.allDay" />
      </div>
    </a-form-item>
    <a-form-item label="结束时间" v-if="formConf.duration === -1 || !!formConf.allDay">
      <div class="countersign-cell">
        <a-input-group compact>
          <jnpf-select v-model:value="formConf.endDaySourceType" :options="sourceTypeOptions" class="!w-100px" @change="formConf.endDay = ''" />
          <div style="width: calc(100% - 100px)">
            <jnpf-select
              v-model:value="formConf.endDay"
              :options="getDateDataSourceForm"
              :fieldNames="{ options: 'children' }"
              showSearch
              allowClear
              class="w-full"
              v-if="formConf.endDaySourceType == 1" />
            <jnpf-date-picker v-model:value="formConf.endDay" format="YYYY-MM-DD" v-else />
          </div>
        </a-input-group>
        <jnpf-time-picker
          class="ml-5px !w-180px"
          v-model:value="formConf.endTime"
          format="HH:mm"
          :minuteStep="5"
          placeholder="请选择"
          v-if="!formConf.allDay" />
      </div>
    </a-form-item>
    <a-form-item label="时长" v-if="formConf.duration !== -1 && !formConf.allDay">
      <jnpf-select v-model:value="formConf.duration" :options="durationList" />
    </a-form-item>
    <a-form-item label="创建人">
      <a-input-group compact>
        <jnpf-select v-model:value="formConf.creatorUserIdSourceType" :options="sourceTypeOptions" class="!w-100px" @change="formConf.creatorUserId = ''" />
        <div style="width: calc(100% - 100px)">
          <jnpf-select
            v-model:value="formConf.creatorUserId"
            :options="getUserDataSourceForm"
            :fieldNames="{ options: 'children' }"
            showSearch
            allowClear
            class="w-full"
            v-if="formConf.creatorUserIdSourceType == 1" />
          <jnpf-user-select v-model:value="formConf.creatorUserId" v-else />
        </div>
      </a-input-group>
    </a-form-item>
    <a-form-item label="参与人">
      <a-input-group compact>
        <jnpf-select v-model:value="formConf.toUserIdsSourceType" :options="sourceTypeOptions" class="!w-100px" @change="formConf.toUserIds = ''" />
        <div style="width: calc(100% - 100px)">
          <jnpf-select
            v-model:value="formConf.toUserIds"
            :options="getUserDataSourceForm"
            :fieldNames="{ options: 'children' }"
            showSearch
            allowClear
            class="w-full"
            v-if="formConf.toUserIdsSourceType == 1" />
          <jnpf-user-select v-model:value="formConf.toUserIds" multiple v-else />
        </div>
      </a-input-group>
    </a-form-item>
    <a-form-item label="标签颜色">
      <jnpf-color-picker v-model:value="formConf.color" />
    </a-form-item>
    <a-form-item label="提醒时间">
      <jnpf-select v-model:value="formConf.reminderTime" :options="formConf.allDay ? reminderTimeList_ : reminderTimeList" />
    </a-form-item>
    <a-form-item label="提醒方式" v-if="formConf.reminderTime !== -2">
      <jnpf-select v-model:value="formConf.reminderType" :options="remindList" />
    </a-form-item>
    <a-form-item label="发送配置" v-if="formConf.reminderTime !== -2 && formConf.reminderType == 2">
      <msg-modal :value="formConf.send" :title="formConf.sendName" messageSource="4" @change="onMsgChange" />
    </a-form-item>
    <a-form-item label="重复提醒">
      <jnpf-select v-model:value="formConf.repetition" :options="repeatReminderList" />
    </a-form-item>
    <a-form-item label="结束重复" v-if="formConf.repetition !== 1">
      <jnpf-date-picker v-model:value="formConf.repeatTime" format="YYYY-MM-DD" />
    </a-form-item>
  </a-form>
</template>
<script lang="ts" setup>
  import { computed, watch } from 'vue';
  import { cloneDeep } from 'lodash-es';
  import { durationList, reminderTimeList, reminderTimeList_, remindList, repeatReminderList } from '../helper/define';
  import HeaderContainer from './components/HeaderContainer.vue';
  import MsgModal from '@/components/FlowProcess/src/propPanel/components/MsgModal.vue';

  defineOptions({ inheritAttrs: false });

  const props = defineProps(['formFieldsOptions', 'formConf', 'updateJnpfData', 'dataSourceForm', 'updateBpmnProperties']);
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
  const getDateDataSourceForm = computed(() => filterDataSourceForm(['datePicker']));

  watch(
    () => props.formConf,
    () => props.updateJnpfData(),
    { deep: true, immediate: true },
  );

  function onAllDayChange() {
    props.formConf.reminderTime = -2;
  }
  function onMsgChange(id, item) {
    if (!id) {
      props.formConf.send = '';
      props.formConf.sendName = '';
    } else {
      if (props.formConf.send === id) return;
      props.formConf.send = id;
      props.formConf.sendName = item.fullName;
    }
  }
  function onTitleChange(val, item) {
    const title = val ? (props.formConf.titleSourceType == 1 ? item.fullName : val) : '';
    const content = title ? `创建[${title}]日程` : '';
    props.formConf.content = content;
    props.updateBpmnProperties('elementBodyName', content);
  }
  function filterDataSourceForm(notSupportList) {
    return cloneDeep(props.dataSourceForm).map(item => ({
      ...item,
      children: item.children.filter(o => o?.__config__?.jnpfKey && notSupportList.includes(o.__config__.jnpfKey)),
    }));
  }
</script>
