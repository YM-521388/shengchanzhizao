<template>
  <BasicModal v-bind="$attrs" @register="registerModal" title="辅助信息设置" width="800px" class="jnpf-log-detail-modal" @ok="handleSubmit" destroyOnClose>
    <div class="log-detail-board">
      <div class="left-board">
        <div class="item-box" :class="{ active: activeItem.id == item.id }" v-for="(item, index) in list" @click="handleClick(item, index)">
          <div class="top">
            <span class="fullName">{{ item.fullName }}</span>
          </div>
        </div>
      </div>
      <div class="center-board">
        <template v-if="activeItem.id == 1">
          <div class="top">
            <JnpfCheckboxSingle v-model:value="activeItem.config.on" label="启用" />
          </div>
          <div class="!mt-20px">
            提示内容：
            <a-textarea v-model:value="activeItem.config.content" :rows="17" />
          </div>
        </template>
        <template v-else-if="activeItem.id == 2">
          <div class="top">
            <JnpfCheckboxSingle v-model:value="activeItem.config.on" label="启用" />
          </div>
          <a-table :data-source="activeItem.config.linkList" :columns="columns" size="small" :pagination="false" rowKey="id" class="column-table !mt-20px">
            <template #bodyCell="{ column, record, index }">
              <template v-if="column.key === 'fullName'">
                <a-input v-model:value="record.fullName" placeholder="请输入" allowClear />
              </template>
              <template v-if="column.key === 'urlAddress'">
                <a-input v-model:value="record.urlAddress" placeholder="请输入" allowClear />
              </template>
              <template v-if="column.key === 'action'">
                <a-button type="link" danger @click="handleDel(index)"><i class="icon-ym icon-ym-app-delete"></i></a-button>
              </template>
            </template>
          </a-table>
          <span class="link-text inline-block mt-10px" @click="handleAdd()"><i class="icon-ym icon-ym-btn-add text-14px mr-4px"></i>新增链接</span>
        </template>
        <template v-else-if="activeItem.id == 3">
          <a-alert message="读取审批人已审批当前流程已归档的流程文件" showIcon type="warning" class="mt-10px" />
          <div class="top mt-10px">
            <JnpfCheckboxSingle v-model:value="activeItem.config.on" label="启用" />
          </div>
        </template>
        <template v-else-if="activeItem.id == 4">
          <a-alert message="通过数据接口查询数据" showIcon type="warning" class="mt-10px" />
          <div class="top mt-10px">
            <JnpfCheckboxSingle v-model:value="activeItem.config.on" label="启用" />
          </div>
          <a-table :data-source="activeItem.config.dataList" :columns="dataColumns" size="small" :pagination="false" rowKey="id" class="column-table !mt-20px">
            <template #bodyCell="{ column, record, index }">
              <template v-if="column.key === 'fullName'">
                {{ record.interfaceName }}
              </template>
              <template v-if="column.key === 'action'">
                <a-button type="link" danger @click="handleEditData(index, 'update')"><i class="icon-ym icon-ym-btn-edit"></i></a-button>
                <a-button type="link" danger @click="handleDataDel(index)"><i class="icon-ym icon-ym-app-delete"></i></a-button>
              </template>
            </template>
          </a-table>
          <span class="link-text inline-block mt-10px" @click="handleAddData()"><i class="icon-ym icon-ym-btn-add text-14px mr-4px"></i>新增数据</span>
        </template>
      </div>
    </div>
  </BasicModal>
  <AuxiliaryModal @register="registerAuxiliaryModal" @confirm="updateConf" />
</template>
<script lang="ts" setup>
  import { BasicModal, useModalInner, useModal } from '@/components/Modal';
  import { reactive, toRefs } from 'vue';
  import { useMessage } from '@/hooks/web/useMessage';
  import AuxiliaryModal from './AuxiliaryModal.vue';
  import { buildBitUUID } from '@/utils/uuid';

  const defaultList = [
    { id: 1, fullName: '提示', config: { on: 0, content: '' } },
    { id: 2, fullName: '链接', config: { on: 0, linkList: [] } },
    { id: 3, fullName: '附件', config: { on: 0 } },
    { id: 4, fullName: '数据', config: { on: 0, dataList: [] } },
  ];

  const dataColumns = [
    { title: '数据名称', dataIndex: 'fullName', key: 'fullName', width: 200 },
    { title: '操作', dataIndex: 'action', key: 'action', width: 50, fixed: 'right' },
  ];

  const columns = [
    { title: '链接名称', dataIndex: 'fullName', key: 'fullName', width: 200 },
    { title: '链接地址', dataIndex: 'urlAddress', key: 'urlAddress', width: 360 },
    { title: '操作', dataIndex: 'action', key: 'action', width: 50, fixed: 'right' },
  ];

  interface State {
    list: any[];
    activeKey: number;
    activeIndex: number;
    activeItem: any;
    formFieldsOptions: any[];
  }

  const emit = defineEmits(['register', 'confirm']);

  const [registerModal, { closeModal }] = useModalInner(init);
  const [registerAuxiliaryModal, { openModal }] = useModal();
  const { createMessage } = useMessage();

  const state = reactive<State>({
    list: [],
    activeKey: 1,
    activeIndex: 0,
    activeItem: {},
    formFieldsOptions: [],
  });
  const { list, activeItem } = toRefs(state);

  function init(data) {
    state.list = data.auxiliaryInfo?.length ? data.auxiliaryInfo : defaultList;
    state.formFieldsOptions = data.formFieldsOptions || [];
    state.activeKey = state.list[0].id;
    state.activeItem = state.list[0];
  }
  function handleClick(item, index) {
    state.activeItem = item;
    state.activeIndex = index;
    state.activeKey = 1;
  }
  function handleAdd() {
    state.activeItem.config.linkList.push({ fullName: '', urlAddress: '' });
  }
  function handleDel(index) {
    state.activeItem.config.linkList.splice(index, 1);
  }
  function handleAddData() {
    const config = {
      id: 'custom' + buildBitUUID(),
      columnOptions: [],
      interfaceId: '',
      interfaceName: '',
      templateJson: [],
    };
    openModal(true, { config, formFieldsOptions: state.formFieldsOptions, type: 'add' });
  }
  function updateConf(data) {
    const index = state.activeItem.config.dataList.findIndex(o => data.id == o.id);
    if (index == -1) return state.activeItem.config.dataList.push(data);
    state.activeItem.config.dataList = state.activeItem.config.dataList.map(o => (data.id == o.id ? data : o));
  }

  function handleEditData(index, type) {
    const config = state.activeItem.config.dataList[index];
    openModal(true, { config, formFieldsOptions: state.formFieldsOptions, type: type });
  }
  function handleDataDel(index) {
    state.activeItem.config.dataList.splice(index, 1);
  }
  function exist(list) {
    let isOk = true;
    for (let i = 0; i < list?.length; i++) {
      const e = list[i];
      if (!e.fullName) {
        createMessage.error('链接名称不能为空');
        isOk = false;
        break;
      }
      if (!e.urlAddress) {
        createMessage.error('链接地址不能为空');
        isOk = false;
        break;
      }
    }
    return isOk;
  }
  function handleSubmit() {
    if (!exist(state.list[1].config.linkList)) return;
    emit('confirm', state.list);
    closeModal();
  }
</script>
<style lang="less">
  .jnpf-log-detail-modal {
    .log-detail-board {
      display: flex;
      height: 550px;
      .left-board {
        width: 200px;
        height: 100%;
        overflow-y: auto;
        border-right: 1px solid @border-color-base;
        .active {
          background-color: @tree-node-selected-bg !important;
        }
        .item-box {
          padding: 8px 20px;
          cursor: pointer;

          &:hover {
            background-color: @selected-hover-bg;
          }
          .top {
            display: flex;
            align-items: center;
            .fullName {
              flex: 1;
              min-width: 0;
              overflow: hidden;
              text-overflow: ellipsis;
              white-space: nowrap;
              font-size: 14px;
              text-align: center;
            }
            .icon {
              width: 28px;
              height: 28px;
              border-radius: 50%;
              padding: 2px;
              text-align: center;
              color: #fff;
              transform: scale(0.65);
            }
            .icon-ym-fail {
              background-color: #ff4d4d;
            }
            .icon-ym-success {
              background-color: #55d187;
            }
          }
          .bottom {
            display: flex;
            align-items: center;
            .icon-ym-btn-refresh {
              color: @primary-color;
            }
          }
        }
      }
      .center-board {
        width: 100%;
        padding: 15px 20px;
        .top {
          display: flex;
          align-items: center;
          justify-content: space-between;
        }
        .column-table {
          height: 300px;
          overflow: auto;
          .icon-ym-btn-edit {
            color: @primary-color;
            font-size: 16px;
          }
        }
      }
    }
  }
</style>
