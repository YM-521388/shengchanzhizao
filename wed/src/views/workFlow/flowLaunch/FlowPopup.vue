<template>
  <BasicPopup v-bind="$attrs" @register="registerPopup" :title="t('routes.workFlow-addFlow')" class="full-popup" destroyOnClose>
    <FlowList @select="onSelect" :isEntrust="isEntrust" :delegateUser="delegateUser" />
  </BasicPopup>
</template>
<script lang="ts" setup>
  import { ref } from 'vue';
  import { BasicPopup, usePopupInner } from '@/components/Popup';
  import FlowList from '../flowQuickLaunch/FlowList.vue';
  import { useI18n } from '@/hooks/web/useI18n';

  const emit = defineEmits(['register', 'select']);
  const [registerPopup, { closePopup }] = usePopupInner(init);
  const { t } = useI18n();

  const isEntrust = ref<boolean>(false);
  const delegateUser = ref<string>('');

  function init(data) {
    isEntrust.value = data.isEntrust || false;
    delegateUser.value = data.delegateUser || '';
  }

  function onSelect(data) {
    emit('select', data);
    closePopup();
  }
</script>
