<template>
  <div class="jnpf-content-wrapper bg-white">
    <TplList @select="onSelect" />
    <PrintBrowse @register="registerPrintBrowse" />
  </div>
</template>

<script lang="ts" setup>
  import { useModal } from '@/components/Modal';
  import TplList from './TplList.vue';
  import PrintBrowse from '@/components/PrintDesign/printBrowse/index.vue';

  const [registerPrintBrowse, { openModal: openPrintBrowse }] = useModal();

  defineOptions({ name: 'workFlow-printTemplate' });

  function onSelect({ id }) {
    openPrintBrowse(true, { id, formInfo: [{ formId: '' }] });
  }
</script>
