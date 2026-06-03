<template>
  <a-tooltip placement="top">
    <template #title>帮助文档</template>
    <a-button type="text" class="help-doc-btn" @click="openHelpDoc">
      <template #icon>
        <QuestionCircleOutlined />
      </template>
    </a-button>
  </a-tooltip>
</template>

<script lang="ts" setup>
  import { QuestionCircleOutlined } from '@ant-design/icons-vue';

  const HELP_DOC_BASE_URL = 'http://183.220.98.191:8100/handbook/';

  const props = defineProps({
    /** 文档ID，作为查询参数 code 拼接到基础地址后面，如 '10001' */
    docId: {
      type: String,
      required: true,
    },
  });

  function openHelpDoc() {
    const url = HELP_DOC_BASE_URL + '?code=' + props.docId;
    window.open(url, '_blank');
  }
</script>

<style scoped>
  .help-doc-btn {
    display: inline-flex;
    align-items: center;
    justify-content: center;
    color: rgba(0, 0, 0, 0.45);
    font-size: 16px;
    transition: color 0.3s;
  }
  .help-doc-btn:hover {
    color: #1890ff;
  }
</style>
