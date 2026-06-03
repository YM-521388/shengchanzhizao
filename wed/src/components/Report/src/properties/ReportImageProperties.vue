<template>
  <a-collapse-panel>
    <template #header>基础设置</template>
    <a-form-item label="图片来源">
      <jnpf-radio
        v-model:value="floatImage.option.source"
        :options="imageSourceList"
        optionType="button"
        buttonStyle="solid"
        @change="onSourceChange"
        class="right-radio-more" />
    </a-form-item>
    <a-form-item v-if="floatImage.option.source == 2">
      <template #label>图片地址<BasicHelp text="地址以http://或https://为开头" /></template>
      <a-input v-model:value="floatImage.option.src" placeholder="请输入" />
    </a-form-item>
    <a-form-item label="上传图片" v-else>
      <JnpfUploadImgSingle v-model:value="floatImage.option.src" :actionPrefix="getActionPrefix" type="/api/Report/data/upload/file" @change="onChange" />
    </a-form-item>
  </a-collapse-panel>
</template>

<script setup lang="ts">
  import { computed } from 'vue';
  import { useGlobSetting } from '@/hooks/setting';

  const props = defineProps(['floatImage']);

  const globSetting = useGlobSetting();
  const getActionPrefix = computed(() => globSetting.reportApiUrl);
  const imageSourceList = [
    { fullName: '本地上传', id: 1 },
    { fullName: 'URL路径', id: 2 },
  ];

  function onSourceChange() {
    props.floatImage.option.src = '';
    onChange();
  }
  function onChange() {
    props.floatImage.imageType = 'URL';
  }
</script>
