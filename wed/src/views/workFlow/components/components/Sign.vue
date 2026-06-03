<template>
  <div class="approval-sign-main">
    <div class="sign-main">
      <template v-if="!signImg">
        <vue-esign ref="esignRef" :height="300" :width="580" :lineWidth="lineWidth" @drawEnd="onDrawEnd" />
        <div class="tip" v-show="!hasDrew">{{ t('component.jnpf.sign.operateTip') }}</div>
      </template>
      <img class="sign-img" :src="signImg" v-else />
    </div>
    <div class="sign-btn">
      <a-checkbox class="left-btn" v-model:checked="useSignNext" @change="onUseSignNextChange">下次继续使用此签名</a-checkbox>
      <div class="right-btn">
        <a-button type="link" @click="handleReset">清除签名</a-button>
        <a-button type="link" @click="openSignListModal">调用签名</a-button>
      </div>
    </div>
  </div>
  <SignListModal ref="signListModalRef" @confirm="onConfirm" />
</template>
<script lang="ts" setup>
  import { Form } from 'ant-design-vue';
  import { ref, unref, watch, nextTick, onMounted } from 'vue';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useUserStore } from '@/store/modules/user';
  import { useI18n } from '@/hooks/web/useI18n';
  import vueEsign from '@/components/Jnpf/Sign/src/esign.vue';
  import SignListModal from '@/components/Jnpf/Sign/src/SignListModal.vue';
  import { buildUUID } from '@/utils/uuid';

  defineProps({
    lineWidth: { type: Number, default: 3 },
    isDefault: { type: Number, default: 0 },
    submitOnConfirm: { type: Boolean, default: false },
    value: { type: String, default: '' },
  });
  const emit = defineEmits(['register', 'update:value', 'change', 'useSignNextChange']);
  defineExpose({ openModal });
  const hasDrew = ref(false);
  const signImg = ref('');
  const esignRef = ref(null);
  const useSignNext = ref(false);
  const signListModalRef = ref(null);
  const confirmLoading = ref(false);
  const formItemContext = Form.useInjectFormItemContext();
  const { createMessage } = useMessage();
  const { t } = useI18n();
  const userStore = useUserStore();
  const userInfo = userStore.getUserInfo;

  watch(
    () => (getESign() as unknown as Recordable)?.hasDrew,
    val => {
      hasDrew.value = val;
    },
  );

  function openModal() {
    confirmLoading.value = false;
    nextTick(() => {
      handleReset();
    });
  }
  function getESign() {
    const esign = unref(esignRef);
    if (!esign) return null;
    return esign;
  }
  function handleReset() {
    signImg.value = '';
    emit('update:value', '');
    emit('change', '', '');
    nextTick(() => {
      hasDrew.value = false;
      (getESign() as unknown as Recordable)?.reset();
    });
  }
  function openSignListModal() {
    const signListRef = unref(signListModalRef) as any;
    signListRef?.openModal(signImg.value);
  }
  function onConfirm(value, signId) {
    signImg.value = value;
    emit('update:value', value);
    emit('change', value, signId);
    formItemContext.onFieldChange();
  }
  function onDrawEnd() {
    (getESign() as unknown as Recordable)
      .generate()
      .then(res => {
        emit('update:value', res);
        emit('change', res, buildUUID());
      })
      .catch(() => {
        createMessage.warning(t('component.jnpf.sign.signPlaceholder'));
      });
  }
  function onUseSignNextChange() {
    emit('useSignNextChange', useSignNext.value);
  }

  onMounted(() => {
    signImg.value = userInfo.signImg;
    emit('update:value', signImg.value);
    emit('change', signImg.value, userInfo.signId);
    nextTick(() => {
      if (!signImg.value) handleReset();
    });
  });
</script>
<style lang="less">
  .approval-sign-main {
    width: 500px;
    border: 1px solid @border-color-base1;
    background-color: @app-content-background;
    .sign-main {
      width: 100%;
      height: 300px;
      box-sizing: border-box;
      display: flex;
      justify-content: center;
      align-items: center;
      position: relative;
      overflow: hidden;
      .tip {
        height: 300px;
        text-align: center;
        position: absolute;
        left: 0;
        top: 0;
        right: 0;
        color: @text-color-secondary;
        font-size: 16px;
        pointer-events: none;
        display: flex;
        justify-content: center;
        align-items: center;
      }
    }
    .sign-img {
      object-fit: contain;
      width: 100%;
      height: 100%;
    }
    .sign-btn {
      height: 54px;
      display: flex;
      align-items: center;
      justify-content: space-between;
      margin: 0 10px;
      border-top: 1px solid @border-color-base;
      .ant-btn {
        padding: 4px 5px;
      }
    }
  }
</style>
