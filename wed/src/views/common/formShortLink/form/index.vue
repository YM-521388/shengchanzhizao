<template>
  <div class="jnpf-content-wrapper short-link-wrapper">
    <div class="short-link-lock-wrapper" v-show="formPassUse">
      <div class="short-link-lock-form">
        <a-input-group compact class="enter-y">
          <a-input-password v-model:value="password" :placeholder="t('views.dynamicModel.passwordPlaceholder')" @keyup.enter="unLock()" />
          <a-button :loading="btnLoading" @click="unLock()">
            <template #icon><unlock-outlined /></template>
          </a-button>
        </a-input-group>
      </div>
    </div>
    <div class="short-link-main" v-if="!formPassUse">
      <a-popover placement="bottomRight">
        <template #content>
          <p class="shortLink-tip">{{ t('views.dynamicModel.scanAndShare') }}</p>
          <QrCode :value="state.formLink" :width="154" :options="{ margin: 1 }" class="my-5px" />
        </template>
        <i class="ym-custom ym-custom-qrcode icon-qrcode"></i>
      </a-popover>
      <div class="short-link-header">
        {{ config.fullName }}
      </div>
      <div class="short-link-content short-link-form">
        <Parser ref="parserRef" :formConf="formConf" :isShortLink="true" @submit="submitForm" :key="key" v-if="!loading" />
      </div>
      <div class="short-link-footer">
        <a-button type="primary" @click="handleSubmit" :loading="btnLoading">{{ getOkText }}</a-button>
        <a-button type="warning" class="ml-10px" @click="handleReset">{{ t('common.resetText') }}</a-button>
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
  import { getConfig, createModel, checkPwd } from '@/api/onlineDev/shortLink';
  import { reactive, toRefs, nextTick, ref, unref, onMounted, computed } from 'vue';
  import { createAsyncComponent } from '@/utils/factory/createAsyncComponent';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useI18n } from '@/hooks/web/useI18n';
  import { QrCode } from '@/components/Qrcode/index';
  import { UnlockOutlined } from '@ant-design/icons-vue';
  import { encryptByMd5 } from '@/utils/cipher';
  import dayjs from 'dayjs';
  import { getDateTimeUnit } from '@/utils/jnpf';

  interface State {
    formLink: string;
    formConf: any;
    config: any;
    loading: boolean;
    btnLoading: boolean;
    key: number;
    shortLinkId: string;
    formPassUse: number;
    password: string;
  }

  const props = defineProps(['config', 'modelId', 'isPreview']);
  const { createMessage } = useMessage();
  const { t } = useI18n();
  const parserRef = ref<any>(null);
  const state = reactive<State>({
    formLink: '',
    formConf: {},
    config: {},
    loading: false,
    btnLoading: false,
    key: +new Date(),
    shortLinkId: '',
    formPassUse: -1,
    password: '',
  });
  const { formConf, key, loading, config, btnLoading, formPassUse, password } = toRefs(state);
  const Parser = createAsyncComponent(() => import('@/components/FormGenerator/src/components/Parser.vue'));
  const validFieldsList = [
    'input',
    'textarea',
    'inputNumber',
    'switch',
    'datePicker',
    'timePicker',
    'colorPicker',
    'rate',
    'slider',
    'editor',
    'link',
    'text',
    'alert',
    'table',
    'collapse',
    'collapseItem',
    'tabItem',
    'tab',
    'row',
    'card',
    'groupTitle',
    'divider',
    'tableGrid',
    'tableGridTr',
    'tableGridTd',
    'location',
    'iframe',
    'steps',
    'stepItem',
  ];
  const selectFieldsList = ['radio', 'checkbox', 'select', 'treeSelect', 'cascader'];

  const getOkText = computed(() => {
    const text = state.formConf.confirmButtonTextI18nCode
      ? t(state.formConf.confirmButtonTextI18nCode, state.formConf.confirmButtonText)
      : state.formConf.confirmButtonText;
    return text || t('common.okText');
  });

  function init() {
    state.config = props.config;
    state.formConf = state.config.formData ? JSON.parse(state.config.formData) : {};
    state.formConf.fields = getRealFields(state.formConf.fields);
    fillFormData(state.formConf, {});
    nextTick(() => {
      state.loading = false;
      state.key = +new Date();
    });
  }
  function validFields(o) {
    if (!o.__config__ || !o.__config__.jnpfKey) return true;
    const jnpfKey = o.__config__.jnpfKey;
    if (validFieldsList.includes(jnpfKey) || (selectFieldsList.includes(jnpfKey) && o.__config__.dataType === 'static')) return true;
    return false;
  }
  function getRealFields(list) {
    let newList = list.filter(item => validFields(item));
    newList.forEach(o => o.__config__?.children && Array.isArray(o.__config__.children) && (o.__config__.children = getRealFields(o.__config__.children)));
    return newList;
  }
  function fillFormData(form, data) {
    const currDate = new Date();
    const loop = list => {
      for (let i = 0; i < list.length; i++) {
        let item = list[i];
        if (item.__vModel__) {
          if (item.__config__.defaultCurrent) {
            if (item.__config__.jnpfKey === 'datePicker') {
              item.__config__.defaultValue = dayjs(currDate).startOf(getDateTimeUnit(item.format)).valueOf();
            }
            if (item.__config__.jnpfKey === 'timePicker') {
              item.__config__.defaultValue = dayjs(currDate).format(item.format || 'HH:mm:ss');
            }
          }
        }
        if (item.__config__ && item.__config__.children && Array.isArray(item.__config__.children)) {
          loop(item.__config__.children);
        }
      }
    };
    loop(form.fields);
    form.formData = data;
  }
  function submitForm(data, callback) {
    if (!data) return;
    state.btnLoading = true;
    const dataForm = { data: JSON.stringify(data) };
    createModel(props.modelId, dataForm, state.config.encryption)
      .then(res => {
        createMessage.success(res.msg);
        if (callback && typeof callback === 'function') callback();
        state.btnLoading = false;
        handleReset();
      })
      .catch(() => {
        state.btnLoading = false;
      });
  }
  function handleReset() {
    getParser().handleReset();
  }
  function handleSubmit() {
    if (props.isPreview) return createMessage.warning('功能预览不支持数据保存');
    getParser().handleSubmit();
  }
  function getParser() {
    const parser = unref(parserRef);
    if (!parser) throw new Error('parser is null!');
    return parser;
  }
  function unLock() {
    if (!state.password) return createMessage.error(t('views.dynamicModel.passwordPlaceholder'));
    state.btnLoading = true;
    const query = {
      id: state.shortLinkId,
      type: 0,
      encryption: props.config.encryption,
      password: encryptByMd5(state.password),
    };
    checkPwd(query)
      .then(() => {
        state.btnLoading = false;
        state.formPassUse = 0;
        init();
      })
      .catch(() => {
        state.btnLoading = false;
      });
  }

  onMounted(() => {
    state.loading = true;
    getConfig(props.modelId, props.config.encryption).then(res => {
      state.formLink = res.data.formLink || '';
      state.shortLinkId = res.data.id || '';
      state.formPassUse = res.data.formPassUse || 0;
      if (state.formPassUse) return;
      init();
    });
  });
</script>
