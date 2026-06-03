<template>
  <div class="dialog-select-cell-content">
    <div>
      <div class="univer-sheet-range-selector-dialog-item">
        <span class="univer-input-affix-wrapper univer-input-affix-wrapper-middle univer-input-not-allow-clear" style="width: 100%">
          <input type="text" placeholder="请选择" readonly class="univer-input" :value="dialogSelectCellDataCache" />
        </span>
      </div>
    </div>
    <footer style="display: flex; justify-content: flex-end; margin-top: 20px">
      <button class="univer-button univer-button-default univer-button-middle" @click="handleCancel">取消</button>
      <button
        class="univer-button univer-button-primary univer-button-middle"
        :disabled="!dialogSelectCellDataCache"
        style="margin-left: 10px"
        @click="handleConfirm"
        >确认</button
      >
    </footer>
  </div>
</template>

<script lang="ts" setup>
  import { storeToRefs } from 'pinia';
  import { useJnpfUniverStore } from '../../../store';

  defineOptions({ name: 'JnpfUniverDialogSelectCell' });

  interface PropsType {
    value: string;
  }
  const props = defineProps<PropsType>();

  const jnpfUniverStore = useJnpfUniverStore(props?.value);
  const { dialogSelectCellDataCache } = storeToRefs(jnpfUniverStore);

  // 点击取消
  const handleCancel = () => {
    jnpfUniverStore?.cancelDialogSelectCell();
  };

  // 点击确认
  const handleConfirm = () => {
    jnpfUniverStore?.confirmDialogSelectCell();
  };
</script>
