<template>
  <div class="auxiliary">
    <div v-for="item in list" :key="item.id">
      <div class="card-main" :class="{ 'auxiliary-card': item.id == 3 }" v-if="item.config.on">
        <a-card :title="item.fullName" size="small">
          <p v-if="item.id == 1">{{ item.config.content }}</p>
          <div v-if="item.id == 2">
            <div class="card-box" v-for="(linkItem, index) in item.config.linkList" :key="index">
              <span class="link-text" @click="clickLink(linkItem.urlAddress)">{{ linkItem.fullName }}</span>
              <a-divider class="!mt-10px" v-if="index < item.config.linkList.length - 1"></a-divider>
            </div>
          </div>
          <a-table v-if="item.id == 3" :data-source="item.config.fileList" :columns="columns" size="small" :pagination="false" rowKey="id">
            <template #bodyCell="{ column, record, index }">
              <template v-if="column.key === 'fileName'">
                <div class="file-img">
                  <img src="@/assets/images/document/pdf.png" class="w-30px h-30px" /><span class="link-text" @click="clickExport(record)">{{
                    record[column.key]
                  }}</span>
                </div>
              </template>
              <template v-if="column.key === 'fileDate'">
                <span @click="clickLink(index)">{{ formatToDate(record[column.key]) }}</span>
              </template>
            </template>
          </a-table>
          <div v-if="item.id == 4">
            <div class="card-box" v-for="(dataItem, dataIndex) in item.config.dataList" :key="dataIndex">
              <span style="cursor: pointer" @click="clickData(dataItem)">{{ dataItem.interfaceName }}</span>
              <a-divider class="!mt-10px" v-if="dataIndex < item.config.dataList.length - 1"></a-divider>
            </div>
          </div>
        </a-card>
      </div>
    </div>
  </div>
  <ViewModal :config="config" :formData="formData" ref="viewModalRef" />
  <Preview ref="filePreviewRef" />
</template>

<script lang="ts" setup>
  import { reactive, toRefs, unref, ref } from 'vue';
  import ViewModal from './components/ViewModal.vue';
  import { formatToDate } from '@/utils/dateUtil';
  import Preview from '../document/Preview.vue';

  defineProps({
    list: { type: Array as PropType<any[]>, default: () => [] },
    formData: { type: Object },
  });

  interface State {
    config: any;
  }

  const state = reactive<State>({
    config: {},
  });
  const { config } = toRefs(state);
  const viewModalRef = ref(null);
  const filePreviewRef = ref<any>(null);
  const columns = [
    { title: '文件名', dataIndex: 'fileName', key: 'fileName', width: 200 },
    { title: '归档日期', dataIndex: 'fileDate', key: 'fileDate', width: 100 },
  ];

  function clickLink(urlAddress) {
    window.open(urlAddress, '_blank');
  }
  function clickData(item) {
    state.config = item;
    openSelectDialog();
  }
  function clickExport(item) {
    let query = {
      name: item.fileName,
      url: item.uploaderUrl,
      fileId: item.fileName,
    };
    filePreviewRef.value?.init(query);
  }
  function openSelectDialog() {
    (unref(viewModalRef) as any)?.openViewModal();
  }
</script>
<style lang="less" scoped>
  .auxiliary {
    height: 100%;
    overflow: auto;
    .card-main {
      padding: 10px;
      .card-box {
        height: 40px;
        .link-text {
          color: @primary-color;
          cursor: pointer;
          text-decoration: underline;
          margin-bottom: 10px;
        }
        .data-text:hover {
          cursor: pointer;
        }
      }
    }
  }

  .auxiliary-card {
    :deep(.ant-card-body) {
      padding: unset !important;
    }
  }
  .file-img {
    display: flex;
    align-items: center;
  }
</style>
