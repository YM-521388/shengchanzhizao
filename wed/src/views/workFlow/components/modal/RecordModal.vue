<template>
  <BasicModal v-bind="$attrs" @register="registerModal" :title="state.title" destroyOnClose class="jnpf-flow-record-modal" :footer="null">
    <div class="record-container">
      <div class="record-item" v-for="item in state.list" v-if="state.list?.length">
        <div class="record-item-top">
          <a-avatar :size="40" :src="apiUrl + item.headIcon" class="avatar" />
          <div class="user-info">
            <div class="user-name">{{ item.userName }}</div>
            <div class="handle-time">{{ formatToDateTime(item.handleTime) }}</div>
          </div>
          <div class="handle-type">
            <a-tag :color="getHexColor(getFlowStateColor(item.handleType))" :bordered="false" class="handle-status">
              {{ getFlowStateContent(item.handleType) }}
            </a-tag>
            <ArrowRightOutlined class="arrow-right" v-if="item.handleUserName" />
            <span class="handle-user-name" :title="item.handleUserName">{{ item.handleUserName }}</span>
          </div>
        </div>
        <div class="record-item-bottom" v-if="item.handleOpinion || item.fileList?.length || item.signImg">
          <div class="opinion">{{ item.handleOpinion }}</div>
          <div class="file" v-if="item.fileList?.length">
            <jnpf-upload-file v-model:value="item.fileList" type="workFlow" detailed simple />
          </div>
          <div class="sign" v-if="item.signImg">
            签名：
            <Image :width="80" :src="item.signImg" :preview="false" class="sign-img" @click="handlePreview(item.signImg)" />
          </div>
          <div class="sign" v-for="child in item.approvalField"> {{ child.fieldName }}：{{ child.value }} </div>
        </div>
      </div>
      <jnpf-empty v-if="!state.list?.length && !state.loading" />
    </div>
  </BasicModal>
</template>
<script lang="ts" setup>
  import { reactive, ref } from 'vue';
  import { Image } from 'ant-design-vue';
  import { ArrowRightOutlined } from '@ant-design/icons-vue';
  import { BasicModal, useModalInner } from '@/components/Modal';
  import { getRecordList } from '@/api/workFlow/task';
  import { useGlobSetting } from '@/hooks/setting';
  import { useDefineSetting } from '@/hooks/setting/useDefineSetting';
  import { createImgPreview } from '@/components/Preview/index';
  import { formatToDateTime } from '@/utils/dateUtil';

  interface State {
    list: any[];
    loading: boolean;
    title: string;
  }

  const emit = defineEmits(['register', 'confirm', 'close']);
  const state = reactive<State>({
    list: [],
    loading: false,
    title: '',
  });
  const globSetting = useGlobSetting();
  const apiUrl = ref(globSetting.apiUrl);
  const { getFlowStateContent, getFlowStateColor, getHexColor } = useDefineSetting();
  const [registerModal, { changeLoading }] = useModalInner(init);

  function init(data) {
    changeLoading(true);
    state.loading = true;
    state.list = [];
    state.title = data.title;
    getRecordList(data.taskId, data.nodeId).then(res => {
      state.list = res.data.map(o => ({ ...o, fileList: o.fileList ? JSON.parse(o.fileList) : [] })) || [];
      changeLoading(false);
      state.loading = false;
    });
  }
  function handlePreview(img) {
    createImgPreview({ imageList: [img] });
  }
</script>
<style lang="less">
  .jnpf-flow-record-modal {
    .ant-modal-body {
      & > .scrollbar {
        padding: 0 20px;
      }
    }
    .record-container {
      height: 600px;
      padding-top: 20px;
      .record-item {
        display: flex;
        flex-direction: column;
        border-bottom: 1px solid @border-color-base1;
        margin-bottom: 12px;
        padding-bottom: 12px;
        .record-item-top {
          display: flex;
          .avatar {
            margin-right: 12px;
          }
          .user-info {
            flex: 1;
            overflow: hidden;
            display: flex;
            flex-direction: column;
            .user-name {
              font-size: 14px;
              overflow: hidden;
              white-space: nowrap;
              text-overflow: ellipsis;
            }
            .handle-time {
              color: @text-color-secondary;
              font-size: 14px;
            }
          }
          .handle-type {
            display: flex;
            align-items: center;
            flex-shrink: 0;
            .handle-status {
              border-radius: 50px;
              height: 32px;
              line-height: 32px;
              padding-inline: 18px;
              font-size: 14px;
            }
            .arrow-right {
              width: 20px;
              height: 20px;
              margin-right: 10px;
              display: flex;
              align-items: center;
              justify-content: center;
              border-radius: 50%;
              color: @primary-color;
              background-color: @app-content-background;
            }
            .handle-user-name {
              max-width: 150px;
              overflow: hidden;
              white-space: nowrap;
              text-overflow: ellipsis;
            }
          }
        }
        .record-item-bottom {
          margin: 4px 0 0 52px;
          padding: 10px;
          border-radius: 4px;
          background-color: @app-content-background;
          .file {
            margin-top: 10px;
          }
          .sign {
            margin-top: 10px;
            display: flex;
            align-items: center;
            word-break: break-all;
            .sign-img {
              width: 80px;
              cursor: pointer;
            }
          }
        }
      }
    }
  }
</style>
