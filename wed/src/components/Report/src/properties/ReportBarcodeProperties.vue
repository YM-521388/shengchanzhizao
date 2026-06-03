<template>
  <a-form-item  label="内容">
    <a-textarea v-model:value="barcode.content" />
  </a-form-item>
</template>

<script setup lang="ts">
  defineProps(['barcode']);
</script>
