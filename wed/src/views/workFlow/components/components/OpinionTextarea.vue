<template>
  <div class="approval-opinion" :class="{ 'approval-opinion-style': styleType == 1 }">
    <a-form-item-rest>
      <jnpf-textarea class="opinion-textarea" v-model:value="innerValue" :placeholder="placeholder" @change="onChange" />
      <div class="opinion-extra">
        <div class="common-words">
          <div v-for="item in state.commonList" @click="insertOpinion(item.commonWordsText)" :title="item.commonWordsText">{{ item.commonWordsText }}</div>
        </div>
        <a-button type="link" class="btn" @click="addCommonWord">设为常用语</a-button>
        <CommonWordsPopover :list="state.list" class="btn" @confirm="insertOpinion" />
      </div>
    </a-form-item-rest>
  </div>
</template>
<script lang="ts" setup>
  import { reactive, toRefs, onMounted, watch } from 'vue';
  import { getCommonWordsSelector, create } from '@/api/system/commonWords';
  import { useMessage } from '@/hooks/web/useMessage';
  import CommonWordsPopover from './CommonWordsPopover.vue';
  import { cloneDeep } from 'lodash-es';

  interface State {
    innerValue: any;
    commonList: any[];
    list: any[];
  }

  const state = reactive<State>({
    innerValue: '',
    commonList: [],
    list: [],
  });
  const { innerValue } = toRefs(state);
  const { createMessage } = useMessage();
  const emit = defineEmits(['update:value', 'change']);
  const props = defineProps({
    value: { type: String, default: '' },
    commonWordsCount: { type: Number, default: 5 },
    placeholder: { type: String, default: '请输入' },
    styleType: { type: Number, default: 0 },
    messageText: { type: String, default: '' },
  });

  watch(
    () => props.value,
    val => {
      state.innerValue = val;
    },
    { immediate: true },
  );

  function initData() {
    state.list = [];
    getCommonWordsSelector().then(res => {
      const data = cloneDeep(res.data.list) || [];
      state.commonList = data.splice(0, props.commonWordsCount);
      state.list = data;
    });
  }
  function insertOpinion(val) {
    state.innerValue += val;
    onChange(state.innerValue);
  }
  function onChange(value) {
    emit('update:value', value);
    emit('change', value);
  }
  function addCommonWord() {
    let message = props.messageText || '请先填写审批意见';
    if (!state.innerValue) return createMessage.warning(message);
    const query = { commonWordsText: state.innerValue, commonWordsType: 1 };
    create(query).then(res => {
      createMessage.success(res.msg);
      initData();
    });
  }

  onMounted(() => {
    initData();
  });
</script>
<style lang="less">
  html[data-theme='dark'] {
    .approval-opinion {
      border: 1px solid #424242;
      background-color: rgba(255, 255, 255, 0.08);
    }
  }

  .approval-opinion {
    border-radius: 4px;
    border: 1px solid #d9d9d9;
    .opinion-textarea {
      background: unset;
      border-width: 0;
      &:focus {
        box-shadow: unset !important;
        border-inline-end-width: 0 !important;
      }
    }
    .opinion-extra {
      display: flex;
      padding: 0 10px 10px;
      .common-words {
        flex: 1;
        min-width: 0;
        display: flex;
        flex-shrink: 0;
        overflow-x: auto;
        div {
          height: 32px;
          line-height: 32px;
          max-width: 160px;
          cursor: pointer;
          overflow: hidden;
          text-overflow: ellipsis;
          white-space: nowrap;
          text-align: center;
          border-radius: 4px;
          padding: 0 12px;
          background-color: @component-background;
          border: 1px dashed @border-color-base;
          margin-right: 8px;
          flex-shrink: 0;
        }
      }
      .btn {
        flex-shrink: 0;
      }
    }
  }
</style>
