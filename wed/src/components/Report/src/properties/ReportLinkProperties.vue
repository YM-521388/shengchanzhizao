<template>
  <a-form-item  label="目标">
    <a-input v-model:value="link.target" />
  </a-form-item>
  <a-form-item  label="地址">
    <a-textarea v-model:value="link.url" />
  </a-form-item>
</template>

<script setup lang="ts">
  defineProps(['link']);
</script>
