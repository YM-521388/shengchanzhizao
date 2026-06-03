<template>
  <HeaderContainer :formConf="formConf" @onNodeNameChange="updateBpmnProperties('nodeName', $event)" />
  <a-form :colon="false" :model="formConf" class="config-content" layout="vertical">
    <a-form-item label="选择通知">
      <div class="common-tip mb-12px">当通知成功接收到站内短信时触发。</div>
      <TemplateModal :value="formConf.noticeId" :messageType="1" :title="formConf.noticeName" placeholder="请选择" @change="onNoticeIdChange" />
    </a-form-item>
  </a-form>
</template>
<script lang="ts" setup>
  import { watch } from 'vue';
  import HeaderContainer from './components/HeaderContainer.vue';
  import TemplateModal from '@/views/msgCenter/sendConfig/components/TemplateModal.vue';
  defineOptions({ inheritAttrs: false });

  const props = defineProps(['formConf', 'updateJnpfData', 'updateBpmnProperties']);

  watch(
    () => props.formConf,
    () => props.updateJnpfData(),
    { deep: true, immediate: true },
  );

  function onNoticeIdChange(id, item) {
    props.formConf.noticeId = id;
    props.formConf.noticeName = item.fullName;
    props.formConf.content = `当[${item.fullName}]通知成功时`;
    props.updateBpmnProperties('elementBodyName', props.formConf.content);
  }
</script>
