<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    title="自定义按钮脚本"
    helpMessage="小程序不支持在线JS脚本"
    :width="1000"
    @ok="handleSubmit"
    destroyOnClose
    class="form-script-modal">
    <div class="form-script-modal-body">
      <div class="main-board">
        <div class="main-board-editor">
          <MonacoEditor ref="editorRef" v-model="text" />
        </div>
        <div class="main-board-tips">
          <p>支持JavaScript的脚本，<ScriptDemo :type="type" /></p>
        </div>
      </div>
    </div>
  </BasicModal>
</template>
<script lang="ts" setup>
  import { ref } from 'vue';
  import { BasicModal, useModalInner } from '@/components/Modal';
  import { MonacoEditor } from '@/components/CodeEditor';
  import ScriptDemo from '@/components/FormGenerator/src/components/ScriptDemo.vue';

  const emit = defineEmits(['register', 'confirm']);
  const [registerModal, { closeModal }] = useModalInner(init);
  const editorRef = ref(null);
  const text = ref('');
  const props = defineProps(['type']);

  function init(data) {
    text.value = data.text;
  }
  function handleSubmit() {
    emit('confirm', text.value);
    closeModal();
  }
</script>
<style lang="less"></style>
