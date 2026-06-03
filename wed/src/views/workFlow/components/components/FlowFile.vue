<template>
  <div class="flow-file" v-if="state.showFileLoading">
    <Spin tip="正在归档..." />
    <div id="previewDesignedWrap" v-show="false"></div>
  </div>
</template>

<script lang="ts" setup>
  import { reactive, computed, unref, nextTick } from 'vue';
  import { hiprint } from 'vue-plugin-hiprint';
  import { getBatchData } from '@/api/system/printDev';
  import $ from 'jquery';
  import { useUserStore } from '@/store/modules/user';
  import dayjs from 'dayjs';
  import { uploadBlob } from '@/api/workFlow/document';
  import { Spin } from 'ant-design-vue';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useDefineSetting } from '@/hooks/setting/useDefineSetting';
  import { useGlobSetting } from '@/hooks/setting';

  interface State {
    id: string;
    opType: string;
    hiprintTemplate: any;
    dataList: any[];
    systemInfo: any;
    showFileLoading: boolean;
    formInfo: any[];
  }

  defineExpose({ init });
  const emit = defineEmits(['fileEnd']);
  const state = reactive<State>({
    id: '',
    opType: '',
    hiprintTemplate: undefined,
    dataList: [],
    systemInfo: {
      printer: '',
      printTime: '',
    },
    showFileLoading: false,
    formInfo: [],
  });
  const userStore = useUserStore();
  const { createMessage } = useMessage();
  const { getFlowStateContent } = useDefineSetting();
  const globSetting = useGlobSetting();

  const getUserInfo = computed(() => userStore.getUserInfo || {});

  /**
   * 初始化
   * @param data
   */
  function init(data) {
    state.id = data.id || '';
    state.formInfo = data.formInfo || [];
    state.opType = data.opType;
    if (!state.id || !state.formInfo.length) return fileEnd();
    state.showFileLoading = true;
    nextTick(() => getInfo());
  }
  async function getInfo() {
    $('#previewDesignedWrap').html(null);
    const { data: resData } = (await getBatchData({ id: state.id, formInfo: state.formInfo })) || {};
    if (!resData) return fileEnd();
    getSystemInfo();
    const printDataArr = resData?.map((item, index) => {
      let { printData, printTemplate, operatorRecordList = [], convertConfig = '' } = item || {};
      try {
        const targetTpl = JSON.parse(printTemplate);
        if (index === 0) {
          state.hiprintTemplate = new hiprint.PrintTemplate({ template: targetTpl });
        }
        if (convertConfig) printData = handleConvert(printData, convertConfig);
        printData.operatorRecordList = operatorRecordList.map(o => ({
          ...o,
          handleTime: dayjs(o.handleTime).format('YYYY-MM-DD HH:mm:ss'),
          handleStatus: getFlowStateContent(o.handleStatus),
        }));
        printData.systemInfo = state.systemInfo;
        return printData;
      } catch (error) {
        $('#previewDesignedWrap').append('<div class="print-single-wrap"><div class="tpl-invalid">模板已失效，请重新设计</div></div>');
        return null;
      }
    });
    if (!state.hiprintTemplate) return fileEnd();
    state.dataList = [...printDataArr];
    initHinnn();
    const tplHtml = state.hiprintTemplate?.getHtml(state.dataList);
    $('#previewDesignedWrap').html(tplHtml);
    handleUpload();
  }
  // 上传文件
  function handleUpload() {
    if (!state.hiprintTemplate || !state.dataList?.length) return fileEnd();
    state.hiprintTemplate
      ?.toPdf(state.dataList, `${state.id}_${dayjs().format('YYYYMMDDHHmmss')}`, { isDownload: false })
      .then(res => {
        const form = new FormData();
        form.append('file', res);
        form.append('taskId', state.formInfo[0].flowTaskId);
        uploadBlob(form)
          .then(() => {
            window.close();
            fileEnd('归档成功！');
          })
          .catch(() => {
            fileEnd();
          });
      })
      .catch(() => {
        fileEnd();
      });
  }
  function handleConvert(data, convertConfig) {
    const convertConfigList = JSON.parse(convertConfig);
    for (let i = 0; i < convertConfigList.length; i++) {
      const e = convertConfigList[i];
      if (e.type !== 'singleImg') continue;
      const table = e.field.split('.')[0];
      const field = e.field.split('.')[1];
      if (!Reflect.has(data, table)) continue;
      for (let j = 0; j < data[table].length; j++) {
        if (Reflect.has(data[table][j], field) && data[table][j][field]) {
          // 图片加前缀
          data[table][j][field] = globSetting.apiUrl + data[table][j][field];
        }
      }
    }
    return data;
  }
  // 获取系统信息
  function getSystemInfo() {
    const systemPrinter = unref(getUserInfo).userName + '/' + unref(getUserInfo).userAccount;
    const systemPrintTime = dayjs(new Date().getTime()).format('YYYY-MM-DD HH:mm:ss');
    state.systemInfo.printer = systemPrinter;
    state.systemInfo.printTime = systemPrintTime;
  }
  // 重写hinnn
  function initHinnn() {
    if (!(window as any).hinnn) return;
    (window as any).hinnn.apiUrl = globSetting.apiUrl;
    (window as any).hinnn.dateFormat = function (date, format) {
      if (!date) return '';
      if (!isNaN(date) && typeof date === 'string') date = Number(date);
      format = format.replaceAll('y', 'Y').replaceAll('d', 'D');
      return dayjs(date).format(format);
    };
  }
  function fileEnd(mes?) {
    state.showFileLoading = false;
    if (mes) {
      emit('fileEnd');
      createMessage.success(mes);
      return;
    }
    emit('fileEnd');
    createMessage.error(state.opType != '6' ? '归档失败，请联系管理员在流程监控中手动归档！' : '归档失败！');
  }
</script>
