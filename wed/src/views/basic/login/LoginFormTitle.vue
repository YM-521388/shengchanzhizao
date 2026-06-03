<template>
  <div>
    <a-image class="login-logo" :src="apiUrl + getLoginIcon" :fallback="loginLogo" :preview="false" v-if="getLoginIcon" />
    <img class="login-logo" :src="loginLogo" v-else />
  </div>
</template>
<script lang="ts" setup>
  import { ref, computed } from 'vue';
  import { Image as AImage } from 'ant-design-vue';
  import { useGlobSetting } from '@/hooks/setting';
  import loginLogo from '@/assets/images/login_logo.png';

  const globSetting = useGlobSetting();
  const apiUrl = ref(globSetting.apiUrl);

  const getLoginIcon = computed(() => localStorage.getItem('_APP_LOGIN_LOGO_') || '');
</script>
