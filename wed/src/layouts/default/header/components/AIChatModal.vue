<template>
  <a-modal v-model:open="visible" title="AI助手" :width="1000" class="ai-modal" :footer="null" :maskClosable="false" @cancel="handleCancel">
    <template #closeIcon>
      <ModalClose :canFullscreen="false" @cancel="handleCancel" />
    </template>

    <div class="ai-container">
      <!-- 左侧对话列表 -->
      <div class="chat-sidebar">
        <div class="new-chat-btn" @click="startNewChat">
          <plus-outlined />
          <span>开启新对话</span>
        </div>
        <a-divider />
        <div class="chat-list">
          <div
            v-for="(chat, index) in chatHistory"
            :key="chat.id"
            class="chat-item"
            :class="{ active: currentChatIndex === index }"
            v-if="chatHistory.length"
            @click="switchChat(index)">
            <div class="chat-info">
              <span class="chat-title">{{ chat.fullName }}</span>
              <span class="chat-date"
                >{{ formatToDate(chat.creatorTime) }}
                <span class="close-btn" @click="deleteChatHistory(chat.id)"> <i class="icon-ym icon-ym-btn-clearn" /> </span
              ></span>
            </div>
          </div>
          <div class="noData-txt" v-else> <img src="@/assets/images/chatImg/noData.png" class="noData-img" />暂无历史对话</div>
        </div>
      </div>

      <!-- 右侧主要内容区 -->
      <div class="chat-main">
        <!-- 对话标题 -->
        <div class="chat-title" v-if="currentChat">
          <a-input
            v-if="isEditingTitle"
            v-model:value="editingTitle"
            :maxlength="30"
            class="title-input"
            @pressEnter="saveTitle"
            @blur="saveTitle"
            ref="titleInputRef" />
          <div v-else class="title-text" @click="startEditTitle">
            {{ currentChat.fullName }}
          </div>
        </div>
        <div class="chat-content" ref="chatContentRef">
          <template v-if="messageList.length === 0">
            <div class="welcome-message">
              <div class="message-item">
                <div class="ai-avatar">
                  <img src="@/assets/images/chatImg/robot.png" alt="AI助手头像" />
                </div>
                <div class="content">
                  <div class="title"> 我是大迈，JnpfAI智能助手，很高兴能为您服务！ </div>
                  <div class="subtitle"> 我可以为您解答各种问题，请问有什么能帮助您的吗？ </div>
                </div>
              </div>
            </div>
          </template>
          <template v-else>
            <div class="message-list">
              <div v-for="(msg, index) in messageList" :key="index" class="message-item" :class="msg.type === 1 ? 'user' : 'ai'">
                <template v-if="msg.type === 0">
                  <div class="message-content">{{ msg.content }}</div>
                  <!-- 消息操作按钮 -->
                  <div class="message-actions">
                    <div class="action-buttons">
                      <a-button type="text" class="action-btn" @click="copyMessage(msg.content)">
                        <CopyOutlined />
                      </a-button>
                      <a-button type="text" class="action-btn" @click="regenerateMessage(msg.questionText)">
                        <SyncOutlined />
                      </a-button>
                    </div>
                  </div>
                </template>
                <template v-else-if="msg.type === 1">
                  <div class="message-content">{{ msg.content }}</div>
                </template>
                <template v-else-if="msg.type === 'loading'">
                  <LoadingOutlined :style="{ fontSize: '24px' }" />
                </template>
              </div>
            </div>
          </template>
        </div>
        <!-- 底部区域 -->
        <div class="chat-footer">
          <!-- 快捷问题 -->
          <div class="quick-questions">
            <a v-for="question in quickQuestions" :key="question" class="question-link" @click="handleQuickQuestion(question)">
              {{ question }}
            </a>
          </div>

          <!-- 输入框区域 -->
          <div class="input-container">
            <a-textarea v-model:value="inputMessage" class="input-box" placeholder="请输入问题描述" @keydown="handleKeydown" />
            <div class="send-button" @click="sendMessage()"><i class="icon-ym icon-ym-release"></i></div>
          </div>
        </div>
      </div>
    </div>
  </a-modal>
</template>

<script lang="ts" setup>
  import { ref, computed, nextTick, unref } from 'vue';
  import { Modal as AModal } from 'ant-design-vue';
  import ModalClose from '@/components/Modal/src/components/ModalClose.vue';
  import { formatToDate } from '@/utils/dateUtil';
  import { PlusOutlined, CopyOutlined, SyncOutlined, LoadingOutlined } from '@ant-design/icons-vue';
  import { send, historyList, historyGet, historySave, historyDelete } from '@/api/system/aiChat';
  import { useCopyToClipboard } from '@/hooks/web/useCopyToClipboard';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useI18n } from '@/hooks/web/useI18n';

  defineOptions({
    name: 'AIChatModal',
    inheritAttrs: false,
  });
  defineExpose({ openModal });

  const visible = ref(false);
  const isLoading = ref(false);
  const inputMessage = ref('');
  const sending = ref(false);
  const currentChatIndex = ref(0);
  const chatContentRef = ref<HTMLElement | null>(null);
  const messageList = ref<any[]>([]);
  const isEditingTitle = ref(false);
  const editingTitle = ref('');
  const titleInputRef = ref();
  // 快捷问题列表
  const quickQuestions = ['JNPF是什么', 'JNPF的性能特点', 'JNPF支持哪些数据库'];
  // 聊天历史
  const chatHistory = ref<any[]>([]);
  const currentChat = computed(() => chatHistory.value[currentChatIndex.value]);
  const { createMessage, createConfirm } = useMessage();
  const { t } = useI18n();

  function openModal() {
    visible.value = true;
    isLoading.value = false;
    inputMessage.value = '';
    messageList.value = [];
    isEditingTitle.value = false;
    editingTitle.value = '';
    currentChatIndex.value = 0;
    getHistoryList();
  }
  function handleCancel() {
    visible.value = false;
  }
  function handleKeydown(event) {
    if (event.key === 'Enter') return sendMessage();
  }
  function deleteChatHistory(id) {
    createConfirm({
      iconType: 'warning',
      title: t('common.tipTitle'),
      content: t('common.delTip'),
      onOk: () => {
        historyDelete(id).then(res => {
          createMessage.success(res.msg);
          messageList.value = [];
          getHistoryList();
        });
      },
    });
  }
  async function getHistoryList() {
    historyList().then(res => {
      chatHistory.value = res.data;
      if (chatHistory.value.length && !isLoading.value) getHistory();
    });
  }
  async function copyMessage(content) {
    if (!content) return;
    const { isSuccessRef } = useCopyToClipboard(content);
    unref(isSuccessRef) && createMessage.success('复制成功');
  }

  function regenerateMessage(content) {
    sendMessage(content, true);
  }
  // 开始编辑标题
  function startEditTitle() {
    editingTitle.value = currentChat.value.fullName;
    isEditingTitle.value = true;
    nextTick(() => {
      titleInputRef.value?.focus();
    });
  }
  // 保存标题
  function saveTitle() {
    if (editingTitle.value.trim()) {
      currentChat.value.title = editingTitle.value.trim();
    }
    const userMsg: any = {
      id: currentChat.value.id || '',
      fullName: editingTitle.value.trim(),
    };
    historySave(userMsg).then(() => {
      getHistoryList();
    });
    isEditingTitle.value = false;
  }
  function startNewChat() {
    if ((!messageList.value.length && chatHistory.value.length) || sending.value) return;
    const newChat: any = {
      id: '',
      fullName: '新对话',
      data: [],
    };
    historySave(newChat).then(() => {
      chatHistory.value.unshift(newChat);
      currentChatIndex.value = 0;
      messageList.value = [];
      getHistoryList();
    });
  }
  async function switchChat(index: number) {
    currentChatIndex.value = index;
    getHistory();
  }
  async function getHistory() {
    const res = await historyGet(chatHistory.value[currentChatIndex.value]?.id);
    messageList.value = res.data;
  }
  async function sendMessage(content?: string, isRefresh = false) {
    if (isLoading.value) return;

    // 检查输入
    if ((!inputMessage.value || sending.value) && !isRefresh) return createMessage.warning('请输入问题描述');
    isLoading.value = true;

    const userMessage = isRefresh ? content : inputMessage.value;
    const oneUserMsg = {
      content: userMessage,
      type: 1,
    };
    messageList.value.push(oneUserMsg);

    const userMsg = {
      id: currentChat.value?.id || '',
      fullName: !currentChat.value?.id ? userMessage : '',
      data: messageList.value,
    };

    await historySave(userMsg);

    // 如果是新对话，获取历史列表并更新当前对话
    if (!currentChat.value?.id) {
      await getHistoryList();
    }

    // 添加loading消息
    const loadingMsg = { type: 'loading' };
    messageList.value.push(loadingMsg);
    nextTick(() => {
      scrollToBottom();
    });

    try {
      // 清空输入框
      inputMessage.value = '';
      sending.value = true;
      const res = await send({ keyword: userMessage });

      messageList.value = messageList.value.filter(msg => msg.type !== 'loading');
      setTimeout(() => {
        const oneAiMsg = {
          questionText: userMessage,
          content: res.data,
          type: 0,
        };
        messageList.value.push(oneAiMsg);
        const aiMsg = {
          id: currentChat.value.id || '',
          fullName: '',
          data: messageList.value,
        };
        historySave(aiMsg);
        sending.value = false;
        isLoading.value = false;
        nextTick(() => {
          scrollToBottom();
        });
      }, 1000);
    } catch (error) {
      messageList.value = messageList.value.filter(msg => msg.type !== 'loading');
      sending.value = false;
      isLoading.value = false;
    }
  }
  async function handleQuickQuestion(question: string) {
    inputMessage.value = question;
    await sendMessage();
  }
  async function scrollToBottom() {
    if (chatContentRef.value) {
      chatContentRef.value.scrollTop = chatContentRef.value.scrollHeight;
    }
  }
</script>

<style lang="less">
  .ai-modal {
    :deep(.ant-modal-content) {
      padding: 0;

      .ant-modal-header {
        margin: 0;
        padding: 16px 24px;
        border-bottom: 1px solid @border-color-base;
      }

      .ant-modal-body {
        padding: 0;
        height: 500px;
      }
    }
  }

  .ai-container {
    display: flex;
    height: 100%;
  }

  .chat-sidebar {
    width: 250px;
    border-right: 1px solid @border-color-base;
    display: flex;
    flex-direction: column;

    .new-chat-btn {
      height: 48px;
      display: flex;
      align-items: center;
      gap: 8px;
      padding: 0 16px;
      color: @primary-color;
      cursor: pointer;
      font-size: 14px;
    }

    .ant-divider-horizontal {
      margin: 1px 0;
    }

    .chat-list {
      margin-top: 8px;
      height: 600px;
      overflow-y: auto;

      .chat-item {
        padding: 12px 16px;
        display: flex;
        align-items: center;
        gap: 8px;
        cursor: pointer;
        border-bottom: 1px solid @border-color-base;
        color: #666;
        font-size: 14px;

        .chat-info {
          width: 100%;
          display: flex;
          justify-content: space-between;
          align-items: center;
          font-size: 14px;
          // color: #333;
          .chat-title {
            max-width: calc(100% - 90px);
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
          }
          .chat-date {
            width: 81px;
            font-size: 12px;
            .close-btn {
              color: @error-color;
              cursor: pointer;
            }
          }
        }
        &:hover {
          background: #f5f5f5;
        }

        &.active {
          background: #e6f7ff;
        }
      }
      .noData-txt {
        text-align: center;

        .noData-img {
          margin-left: 80px;
          margin-top: 20px;
        }
      }
    }
  }

  .chat-main {
    flex: 1;
    display: flex;
    flex-direction: column;

    .chat-title {
      padding: 8px;
      text-align: center;
      position: relative;

      .title-input {
        max-width: 200px;
        margin: 0 auto;
        text-align: center;
      }

      .title-text {
        font-size: 14px;
        cursor: pointer;
        display: inline-block;
        padding: 4px 12px;
        border-radius: 4px;
        &:hover {
          border-color: #e8e8e8;
        }
      }
    }
    .chat-content {
      height: 550px;
      overflow-y: auto;
      padding: 20px;
      scroll-behavior: smooth;
      .welcome-message {
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        height: 100%;
        text-align: center;
        .message-item {
          display: flex;
          align-items: flex-start;
          gap: 12px;
          .ai-avatar {
            flex-shrink: 0;
            margin-top: 10px;
          }

          .content {
            flex: 1;

            .title {
              font-size: 16px;
              font-weight: 500;
              margin-bottom: 4px;
              font-weight: bold;
            }

            .subtitle {
              font-size: 14px;
              line-height: 1.5;
            }
          }
        }
      }

      .message-list {
        .message-item {
          display: flex;
          gap: 12px;
          margin-bottom: 20px;
          &.ai {
            display: unset !important;
          }
          &:last-child {
            margin-bottom: 0;
          }

          .ai-avatar,
          .user-avatar {
            width: 32px;
            height: 32px;
            border-radius: 4px;
          }

          .user-avatar {
            background: @primary-color;
            display: flex;
            align-items: center;
            justify-content: center;
          }

          .message-content {
            padding: 12px 16px;
            background: @app-content-background;
            border-radius: 4px;
            font-size: 14px;
            line-height: 1.6;
            max-width: 400px;
          }

          .message-actions {
            margin-top: 8px;

            .action-buttons {
              display: flex;
              gap: 4px;

              .action-btn {
                padding: 4px;

                &:hover {
                  color: @primary-color;
                }
                .anticon {
                  font-size: 16px;
                }
              }
            }
          }

          &.user {
            flex-direction: row-reverse;

            .message-content {
              background: @primary-color;
            }
          }
        }
      }
    }

    .chat-footer {
      .quick-questions {
        padding: 8px 0;
        display: flex;
        gap: 12px;
        padding-left: 12px;

        .question-link {
          display: inline-block;
          border: 1px solid @border-color-base;
          border-radius: 15px;
          padding: 0px 10px;
          margin-right: 10px;
          cursor: pointer;

          &:hover {
            color: darken(@primary-color, 10%);
          }
        }
      }

      .input-container {
        border: 1px solid @app-content-background;
        border-radius: 20px;
        padding: 8px 15px;
        display: flex;
        align-items: center;
        margin: 0 12px 10px;
        background-color: @app-content-background;
      }
      /* 输入框样式 */
      .input-box {
        border: none;
        outline: none;
        flex: 1;
        font-size: 14px;
        box-shadow: none;
        background-color: @app-content-background;
      }

      .send-button {
        width: 30px;
        height: 30px;
        background-size: contain;
        background-repeat: no-repeat;
        cursor: pointer;
        .icon-ym {
          font-size: 24px;
          line-height: 28px;
        }
      }
    }
  }
</style>
