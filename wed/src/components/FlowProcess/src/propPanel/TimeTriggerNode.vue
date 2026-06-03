<template>
  <HeaderContainer :formConf="formConf" @onNodeNameChange="updateBpmnProperties('nodeName', $event)" />
  <a-form :colon="false" :model="formConf" class="config-content" layout="vertical">
    <a-form-item label="触发开始时间">
      <jnpf-date-picker v-model:value="formConf.startTime" format="YYYY-MM-DD HH:mm:ss" placeholder="选择开始时间" />
    </a-form-item>
    <a-form-item label="触发规则">
      <a-form-item>
        <div class="ant-form-item-label"><label class="ant-form-item-no-colon">Cron表达式</label></div>
        <jnpf-cron v-model:value="formConf.cron" @change="onCronChange" />
      </a-form-item>
    </a-form-item>
    <a-form-item label="触发结束时间">
      <jnpf-select v-model:value="formConf.endTimeType" :options="endTimeTypeOptions" class="mb-10px" />
      <a-input-number
        v-model:value="formConf.endLimit"
        placeholder="次数"
        :min="1"
        :precision="0"
        addonAfter="次后结束"
        @change="oneEndLimitChange"
        v-if="formConf.endTimeType === 1" />
      <jnpf-date-picker v-model:value="formConf.endTime" format="YYYY-MM-DD HH:mm:ss" placeholder="选择结束时间" v-if="formConf.endTimeType === 2" />
    </a-form-item>
  </a-form>
</template>
<script lang="ts" setup>
  import { watch } from 'vue';
  import HeaderContainer from './components/HeaderContainer.vue';
  defineOptions({ inheritAttrs: false });

  const endTimeTypeOptions = [
    { id: 1, fullName: '触发次数' },
    { id: 2, fullName: '指定时间' },
    { id: 3, fullName: '不结束' },
  ];
  const props = defineProps(['formConf', 'updateJnpfData', 'updateBpmnProperties']);
  watch(
    () => props.formConf,
    () => props.updateJnpfData(),
    { deep: true, immediate: true },
  );

  function oneEndLimitChange(val) {
    if (!val) props.formConf.endLimit = 1;
  }
  function onCronChange(val) {
    props.formConf.content = val;
    props.updateBpmnProperties('elementBodyName', val);
  }
</script>
