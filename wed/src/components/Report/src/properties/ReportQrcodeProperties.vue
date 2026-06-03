<template>
  <a-form-item  label="内容">
    <a-textarea v-model:value="qrcode.content" />
  </a-form-item>
</template>

<script setup lang="ts">
  defineProps(['qrcode']);
</script>
