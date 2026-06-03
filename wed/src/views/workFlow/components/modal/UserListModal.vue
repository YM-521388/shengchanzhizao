<template>
  <BasicModal v-bind="$attrs" @register="registerModal" title="发起人员" :width="600" :footer="null" destroyOnClose class="jnpf-flow-user-list-modal">
    <div class="flow-user-list">
      <ScrollContainer>
        <div class="user-item-main" v-for="item in userList" :key="item.id" @click="handleSelectUser(item.id)">
          <a-avatar class="user-avatar" :size="40" :src="apiUrl + item.headIcon" />
          <div class="user-text">
            <p class="user-name">{{ item.fullName }}</p>
            <p class="user-organize" :title="item.organize">{{ item.organize }}</p>
          </div>
        </div>
      </ScrollContainer>
    </div>
  </BasicModal>
</template>
<script lang="ts" setup>
  import { ref } from 'vue';
  import { BasicModal, useModalInner } from '@/components/Modal';
  import { useGlobSetting } from '@/hooks/setting';
  import { ScrollContainer } from '@/components/Container';

  const userList = ref<any[]>([]);
  const emit = defineEmits(['register', 'confirm']);
  const globSetting = useGlobSetting();
  const apiUrl = ref(globSetting.apiUrl);
  const [registerModal, { closeModal }] = useModalInner(init);

  function init(data) {
    userList.value = data.userList || [];
  }
  function handleSelectUser(id) {
    emit('confirm', id);
    closeModal();
  }
</script>
