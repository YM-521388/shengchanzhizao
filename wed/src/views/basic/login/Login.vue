<template>
  <div :class="prefixCls">
    <!-- <div class="login-version" v-if="getSysConfig && getSysConfig.sysVersion">
      <p class="login-version-text">{{ getSysConfig.sysVersion }}</p>
    </div> -->
    <div class="flex items-center absolute right-60px top-80px">
      <AppDarkModeToggle class="enter-x" v-if="!sessionTimeout" />
    </div>
    <!-- <div class="login-header">
      <a class="login-company-logo" target="_blank" href="">
        <img class="login-company-logo-img -enter-x" src="@/assets/images/login-company-logo.png" alt="" />
      </a>
    </div> -->
    <div class="login-content">
      <div class="login-left hidden xl:block">
        <LoginFormTitle class="-enter-x" />
        <img class="login-banner -enter-x" src="@/assets/images/login-banner.png" alt="" />
      </div>
      <div :class="`${prefixCls}-form`" class="enter-x h-630px xl:h-full">
        <LoginFormTitle class="-enter-x xl:hidden" />
        <LoginForm />
      </div>
    </div>
    <div class="copyright">{{ getSysConfig.copyright }}</div>

    <!-- APP download QR codes -->
    <div class="login-app-download">
      <div class="app-download-item">
        <div class="app-qr-wrapper" @mouseenter="showManageQr = true" @mouseleave="showManageQr = false">
          <div class="app-icon-btn">
            <img src="/app.png" class="app-icon-img" alt="管理端" />
          </div>
          <div class="app-qr-popup" v-show="showManageQr">
            <div class="app-qr-inner">
              <img src="/appewm.png" alt="管理端二维码" class="app-qr-img" />
            </div>
          </div>
        </div>
        <span class="app-name">管理端</span>
      </div>
      <div class="app-download-item">
        <div class="app-qr-wrapper" @mouseenter="showStaffQr = true" @mouseleave="showStaffQr = false">
          <div class="app-icon-btn">
            <img src="/app.png" class="app-icon-img" alt="员工端" />
          </div>
          <div class="app-qr-popup" v-show="showStaffQr">
            <div class="app-qr-inner">
              <img src="/apperpYG.png" alt="员工端二维码" class="app-qr-img" />
            </div>
          </div>
        </div>
        <span class="app-name">员工端</span>
      </div>
    </div>
  </div>
</template>
<script lang="ts" setup>
  import { computed, ref } from 'vue';
  import { AppDarkModeToggle } from '@/components/Application';
  import LoginFormTitle from './LoginFormTitle.vue';
  import LoginForm from './LoginForm.vue';
  import { useDesign } from '@/hooks/web/useDesign';
  import { useAppStore } from '@/store/modules/app';
  defineProps({
    sessionTimeout: {
      type: Boolean,
    },
  });

  const { prefixCls } = useDesign('login-container');
  const appStore = useAppStore();

  const getSysConfig = computed(() => appStore.getSysConfigInfo);
  const showManageQr = ref(false);
  const showStaffQr = ref(false);
</script>
<style lang="less">
  @import url('./index.less');
  /* APP download QR codes styles */
  .login-app-download {
    position: fixed;
    left: 50%;
    transform: translateX(-50%);
    bottom: 72px;
    z-index: 1050;
    display: flex;
    align-items: center;
    justify-content: center;
    gap: 40px;
  }
  .app-download-item {
    display: flex;
    flex-direction: column;
    align-items: center;
    gap: 8px;
  }
  .app-qr-wrapper {
    position: relative;
    display: flex;
    align-items: center;
    justify-content: center;
  }
  .app-icon-btn {
    width: 48px;
    height: 50px;
    background: transparent;
    border-radius: 14px;
    display: flex;
    align-items: center;
    justify-content: center;
    box-shadow: 0 6px 18px rgba(10, 132, 255, 0.2);
    cursor: pointer;
  }
  .app-icon-img {
    width: 36px;
    height: 36px;
    object-fit: contain;
    display: block;
  }
  .app-qr-img {
    width: 160px;
    height: 160px;
    max-width: 220px;
    object-fit: contain;
    display: block;
  }
  .app-qr-popup {
    position: absolute;
    bottom: 60px;
    display: flex;
    align-items: center;
    justify-content: center;
    pointer-events: auto;
  }
  .app-qr-inner {
    pointer-events: auto;
    background: #fff;
    padding: 12px;
    border-radius: 8px;
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
    display: flex;
    flex-direction: column;
    align-items: center;
    gap: 8px;
  }
  .app-qr-label {
    font-size: 14px;
    color: #333;
    font-weight: 500;
  }
  .app-name {
    font-size: 14px;
    color: #666;
    font-weight: 500;
  }
</style>
