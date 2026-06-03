<template>
  <HeaderContainer :formConf="formConf" @onNodeNameChange="updateBpmnProperties('nodeName', $event)" />
  <a-form :colon="false" :model="formConf" class="config-content" layout="vertical">
    <a-form-item label="webhook URL">
      <p class="common-tip mb-10px">系统生成的URL，用来接收请求字段！</p>
      <a-input :value="getSysConfig.jnpfDomain + formConf.webhookUrl" readonly>
        <template #addonAfter>
          <span class="cursor-pointer" @click="handleCopy(getSysConfig.jnpfDomain + formConf.webhookUrl)">复制链接</span>
        </template>
      </a-input>
    </a-form-item>
    <a-form-item label="添加接口字段">
      <div class="mb-10px">
        <span class="link-text mr-20px inline-block" @click="addItem"><i class="icon-ym icon-ym-btn-add text-14px mr-4px"></i>自定义添加</span>
        <span class="link-text mr-20px inline-block" @click="openBatchModal(true, {})">
          <i class="icon-ym icon-ym-btn-add text-14px mr-4px"></i>从JSON格式添加
        </span>
        <span
          class="link-text mr-20px inline-block"
          @click="
            openWebhookRequestModal(true, {
              url: formConf.webhookGetFieldsUrl,
              randomStr: formConf.webhookRandomStr,
              id: props.flowInfo.flowId,
              type: 'workFlow',
            })
          ">
          <i class="icon-ym icon-ym-btn-add text-14px mr-4px"></i>从请求接口添加
        </span>
      </div>
      <div class="condition-main">
        <div class="condition-list">
          <a-row :gutter="8" v-for="(item, index) in formConf.formFieldList" :key="index" class="mt-10px">
            <a-col :span="10">
              <a-input v-model:value="item.id" placeholder="字段" @change="onItemChange(item)" />
            </a-col>
            <a-col :span="13">
              <a-input v-model:value="item.fullName" placeholder="字段说明" @change="onItemChange(item)" />
            </a-col>
            <a-col :span="1" class="text-center">
              <i class="icon-ym icon-ym-btn-clearn" @click="delItem(index)" />
            </a-col>
          </a-row>
        </div>
      </div>
    </a-form-item>
  </a-form>
  <BatchModal @register="registerBatchModal" @confirm="addItemForOther" />
  <WebhookRequestModal @register="registerWebhookRequestModal" @confirm="addItemForOther" />
</template>
<script lang="ts" setup>
  import { watch, computed, unref } from 'vue';
  import HeaderContainer from './components/HeaderContainer.vue';
  import BatchModal from '@/components/IntegrateProcess/src/propPanel/modal/BatchModal.vue';
  import WebhookRequestModal from '@/components/IntegrateProcess/src/propPanel/modal/WebhookRequestModal.vue';
  import { useAppStore } from '@/store/modules/app';
  import { useCopyToClipboard } from '@/hooks/web/useCopyToClipboard';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useModal } from '@/components/Modal';

  defineOptions({ inheritAttrs: false });

  const appStore = useAppStore();
  const getSysConfig = computed(() => appStore.getSysConfigInfo);
  const { createMessage } = useMessage();
  const [registerBatchModal, { openModal: openBatchModal }] = useModal();
  const [registerWebhookRequestModal, { openModal: openWebhookRequestModal }] = useModal();

  const props = defineProps(['formFieldsOptions', 'formConf', 'updateJnpfData', 'updateBpmnProperties', 'flowInfo']);

  watch(
    () => props.formConf,
    () => props.updateJnpfData(),
    { deep: true, immediate: true },
  );
  watch(
    () => props.formConf.formFieldList,
    val => {
      props.formConf.content = val?.length ? '已设置触发' : '';
      props.updateBpmnProperties('elementBodyName', props.formConf.content);
    },
    { deep: true, immediate: true },
  );

  function handleCopy(text) {
    const { isSuccessRef } = useCopyToClipboard(text);
    unref(isSuccessRef) && createMessage.success('复制成功');
  }
  function addItem() {
    props.formConf.formFieldList.push({ id: '', fullName: '', label: '' });
  }
  function onItemChange(item) {
    item.label = item.fullName ? item.id + '(' + item.fullName + ')' : item.id;
  }
  function delItem(index) {
    props.formConf.formFieldList.splice(index, 1);
  }
  function addItemForOther(data) {
    const list: any[] = data.map(o => ({ ...o, label: o.fullName ? o.id + '(' + o.fullName + ')' : o.id }));
    if (!props.formConf.formFieldList.length) return (props.formConf.formFieldList = list);
    for (let i = 0; i < list.length; i++) {
      const e = list[i];
      if (!e.id) {
        props.formConf.formFieldList.push(e);
      } else {
        const findIndex = props.formConf.formFieldList.findIndex(o => o.id === e.id);
        findIndex < 0 ? props.formConf.formFieldList.push(e) : (props.formConf.formFieldList[findIndex] = e);
      }
    }
  }
</script>
