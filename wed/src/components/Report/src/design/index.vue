<template>
  <div :class="prefixCls">
    <div class="design-wrap">
      <div class="flex-row">
        <div class="left-side">
          <a-tabs class="average-tabs" v-model:activeKey="activeKey" :tabBarGutter="12">
            <a-tab-pane key="1" tab="单元属性" />
            <a-tab-pane key="2" tab="数据源" />
            <a-tab-pane key="3" tab="报表属性" />
          </a-tabs>
          <div class="tabs-content">
            <div v-show="activeKey === '1'">
              <ReportProperties v-bind="getPropertiesBind" v-if="!!state.activeData" />
            </div>
            <div class="dataSet-content" v-show="activeKey === '2'">
              <div class="dataSet-content-header">
                <a-button type="primary" size="small" preIcon="icon-ym icon-ym-btn-add" @click="handleAddDataSet">数据集</a-button>
                <a-button type="link" size="small" @click="handleConvertConfig" class="!p-0">数据转换</a-button>
              </div>
              <div class="dataSet-content-main" style="height: calc(100% - 102px)">
                <BasicLeftTree
                  ref="dataSetTreeRef"
                  :showSearch="false"
                  :treeData="dataSetList"
                  :fieldNames="{ key: 'jnpfId', title: 'fullName' }"
                  :actionList="actionList" />
              </div>
            </div>
            <div class="dataSet-content" v-show="activeKey === '3'">
              <a-form :colon="false" layout="vertical">
                <a-form-item label="查询">
                  <a-button block @click="handleOpenQueryModal">查询配置</a-button>
                </a-form-item>
                <a-form-item label="排序">
                  <a-button block @click="handleOpenSortModal">排序配置</a-button>
                </a-form-item>
              </a-form>
            </div>
          </div>
        </div>
        <div class="center-side" @click="clickDesignModule">
          <JnpfUniver
            ref="jnpfUniverRef"
            :key="key"
            @focus-float-echart="handleFocusEchart"
            @change-cell="handleChangeCellFromSelection"
            @focusFloatImage="handleFocusFloatImage"
            @change-dialog-select-cell="getCellFromDialogSelect"
            @preview="handlePreview" />
        </div>
      </div>
    </div>
  </div>
  <DataSetModal @register="registerDataSetModal" @confirm="onDataSetConfirm" />
  <ConvertModal @register="registerConvertModal" @confirm="updateConvertConfig" />
  <QueryModal @register="registerQueryModal" @confirm="onQueryConfirm" />
  <SortModal @register="registerSortModal" @confirm="onSortConfirm" />
  <ReportPreview @register="registerPreview" />
</template>

<script lang="ts" setup>
  import { getPreviewDesign, uploadFileExcel, uploadImg } from '@/api/onlineDev/report';
  import { ref, reactive, toRefs, h, provide, onMounted, computed, watch, unref, nextTick } from 'vue';
  import { BasicLeftTree, TreeActionItem } from '@/components/Tree';
  import { useMessage } from '@/hooks/web/useMessage';
  import { useI18n } from '@/hooks/web/useI18n';
  import { useModal } from '@/components/Modal';
  import { useDesign } from '@/hooks/web/useDesign';
  import { DataSetModal } from '@/components/CommonModal';
  import { FormOutlined, DeleteOutlined } from '@ant-design/icons-vue';
  import { cloneDeep, omit } from 'lodash-es';
  import { useDebounceFn } from '@vueuse/core';
  import ConvertModal from '@/components/PrintDesign/PrintDesign/ConvertModal.vue';
  import QueryModal from '../components/QueryModal.vue';
  import SortModal from '../components/SortModal.vue';
  import ReportProperties from '../properties/index.vue';
  import { defaultCellProperties, getFloatImageUrl, getFloatUrl, interceptUrl, getAlphabetFromIndexRule } from '../helper';
  import { JnpfUniver } from '../jnpfUniver/index';
  import ReportPreview from '../reportPreview/index.vue';
  import { useGlobSetting } from '@/hooks/setting';
  import * as newEcharts from 'echarts';

  const props = defineProps({
    dataSetList: { type: Array, default: () => [] },
    convertConfig: { type: Array, default: () => [] },
    versionState: { type: Number, default: 0 },
    customs: { type: String as PropType<string | undefined>, default: undefined },
    snapshot: { type: String as PropType<string | undefined>, default: undefined },
    echarts: { type: String as PropType<string | undefined>, default: undefined },
    queryList: { type: String as PropType<string | undefined>, default: undefined },
    cells: { type: String as PropType<string | undefined>, default: undefined },
    versionId: { type: String as PropType<string | undefined>, default: undefined },
    sortList: { type: String as PropType<string | undefined>, default: undefined },
  });

  defineOptions({ name: 'reportDesign' });
  const emit = defineEmits(['closeVersionPopover', 'changeModalLoading']);
  const jnpfUniverRef = ref();

  provide('getDataSetList', () => state.dataSetList);

  interface State {
    activeKey: string;
    dataSetList: any[];
    convertConfig: any[];
    selectionsInfo: any[];
    firstCellConfig: object;
    queryList: any[];
    sortList: any[];
    activeData: any;
    jnpfUniverAPI: any;
    isInitCell: boolean;
    cellFromDialogValue: string;
    key: any;
    sheetId: string;
  }

  /**
   * 变量集合
   */
  const dataSetTreeRef = ref();
  const state = reactive<State>({
    activeKey: '1',
    dataSetList: [],
    convertConfig: [],
    selectionsInfo: [],
    firstCellConfig: {},
    queryList: [],
    sortList: [],
    activeData: null,
    jnpfUniverAPI: null,
    isInitCell: true,
    cellFromDialogValue: '',
    key: '',
    sheetId: '',
  });
  const { activeKey, dataSetList, key } = toRefs(state);

  const { prefixCls } = useDesign('report-designer');
  const { createConfirm, createMessage } = useMessage();
  const { t } = useI18n();
  const [registerDataSetModal, { openModal: openDataSetModal }] = useModal();
  const [registerConvertModal, { openModal: openConvertModal }] = useModal();
  const [registerQueryModal, { openModal: openQueryModal }] = useModal();
  const [registerPreview, { openModal: openPreviewModal }] = useModal();
  const [registerSortModal, { openModal: openSortModal }] = useModal();
  const debounceUpdateCellProperties = useDebounceFn(updateCellProperties, 200);
  const globSetting = useGlobSetting();
  const requestQueue: any = ref([]);
  const isRequesting = ref(false);
  const imgList: any = ref([]);

  const actionList: TreeActionItem[] = [
    {
      render: node => {
        if (!node.children) return null;
        return h(FormOutlined, {
          class: 'mr-4px',
          title: '编辑',
          onClick: e => {
            e.stopPropagation();
            handleEditDataSet(node);
          },
        });
      },
    },
    {
      render: node => {
        if (!node.children) return null;
        return h(DeleteOutlined, {
          class: 'mr-4px',
          title: '删除',
          onClick: e => {
            e.stopPropagation();
            handleDeleteDataSet(node);
          },
        });
      },
    },
  ];

  const getDefaultEchartOptions = computed(() => unref(jnpfUniverRef)?.getDefaultFloatEchartOptions() ?? {});
  const getPropertiesBind = computed(() => ({
    activeData: state.activeData,
    dataSetList: state.dataSetList,
    queryList: state.queryList,
    defaultEchartOptions: unref(getDefaultEchartOptions),
    jnpfUniverRef,
    cellFromDialogValue: state.cellFromDialogValue,
    activeSheetId: state.jnpfUniverAPI?.getActiveWorkbook()?.getActiveSheet().getSheetId(),
  }));

  watch(
    () => state.activeData,
    () => {
      // 更新属性
      debounceUpdateCellProperties();
    },
    { deep: true },
  );

  const handleEditDataSet = (node: any) => {
    const data: any = omit(node, ['fullName_title', 'children']);
    handleOpenDataSetModal(data);
  };
  const handleDeleteDataSet = (node: any) => {
    createConfirm({
      iconType: 'warning',
      title: t('common.tipTitle'),
      content: '确定要删除该数据集吗?',
      onOk: () => {
        state.dataSetList = state.dataSetList.filter(o => o.jnpfId !== node.jnpfId);
      },
    });
  };
  // 添加数据集
  function handleAddDataSet() {
    handleOpenDataSetModal();
  }
  // 数据转换配置
  function handleConvertConfig() {
    const fieldOptions = state.dataSetList.map(o => ({
      ...o,
      disabled: true,
      children: !o.children ? [] : o.children.map(c => ({ ...c, fullName: o.fullName + '.' + c.fullName, id: o.fullName + '.' + c.id })),
    }));
    openConvertModal(true, { list: state.convertConfig, fieldOptions });
  }
  function updateConvertConfig(data) {
    state.convertConfig = data;
  }
  function handleOpenDataSetModal(data = null) {
    openDataSetModal(true, { data, list: state.dataSetList });
  }
  function onDataSetConfirm(data: any) {
    state.dataSetList = data.map((o: any) => ({
      ...o,
      children: !o.children ? [] : o.children.map(c => ({ ...c, jnpfId: o.fullName + '.' + c.id, fullName: c.label ? `${c.id}（${c.label}）` : c.id })),
    }));
  }
  // 暴露数据
  const getData = async () => {
    return new Promise(async resolve => {
      await unref(jnpfUniverRef)?.setCellEditVisible();
      const { snapshot, floatEcharts, customs, floatImages } = unref(jnpfUniverRef)?.getDesignWorkbookData();

      let resources = await resourcesList(snapshot.resources, floatImages);
      snapshot.resources = resources;
      const floatImagesList = interceptUrl(floatImages, globSetting.reportApiUrl);
      const customCells = !customs?.length
        ? []
        : customs.map(o => ({ col: o.col, row: o.row, sheet: o.sheetId, type: o.cellData?.custom?.type, custom: o.cellData?.custom || {} }));

      const cells = { cells: customCells, floatEcharts, floatImages: floatImagesList };
      const data = {
        dataSetList: state.dataSetList.map(o => omit(o, ['fullName_title', 'children'])),
        convertConfig: JSON.stringify(state.convertConfig),
        queryList: JSON.stringify(state.queryList),
        sortList: JSON.stringify(state.sortList || []),
        snapshot: snapshot ? JSON.stringify(snapshot) : null,
        cells: JSON.stringify(cells),
      };
      resolve(data);
    });
  };
  async function resourcesList(list, floatImages) {
    requestQueue.value = [];
    imgList.value = [];
    if (!list.length) return;
    for (let index = 0; index < list.length; index++) {
      const element = list[index];
      if (element.name != 'SHEET_DRAWING_PLUGIN') continue;
      const eleData = element.data ? JSON.parse(element.data) : {};
      for (const key in eleData) {
        for (const imageKey in eleData[key].data) {
          const imageData = eleData[key].data[imageKey];
          const { componentKey, source, imageSourceType, drawingId } = imageData;
          if (componentKey === 'JnpfUniverFloatEchart') continue;
          if (floatImages && !source) {
            await handleFloatImages(floatImages, drawingId, imageData);
          } else if (source && imageSourceType === 'BASE64') {
            await updateImageSource(source, drawingId, imageSourceType, imageData);
          }
          if (source && imageSourceType === 'URL') {
            imageData.source = source.split(globSetting.reportApiUrl)[1];
          }
        }
      }
      element.data = JSON.stringify(eleData);
    }
    return list;
  }
  async function handleFloatImages(floatImages, drawingId, imageData) {
    const imageType = floatImages[drawingId].imageType;
    const { src, source } = floatImages[drawingId].option;
    if (source === 2 || imageType === 'BASE64') {
      await crossDomainToUrl(src, drawingId, imageType || 'URL');
      const item = imgList.value.find(o => o.drawingId === drawingId);
      imageData.source = item.url;
      imageData.imageSourceType = 'URL';
    } else if (source === 1) {
      imageData.source = src.split(globSetting.reportApiUrl)[1];
      imageData.imageSourceType = 'URL';
    }
  }
  async function updateImageSource(source, drawingId, type, imageData) {
    await crossDomainToUrl(source, drawingId, type);
    const item = imgList.value.find(o => o.drawingId === drawingId);
    imageData.source = item.url;
    imageData.imageSourceType = 'URL';
  }
  //将base64 以及url 请求到后端处理为了解决跨域问题
  async function crossDomainToUrl(img, drawingId, imgType) {
    return new Promise(resolve => {
      requestQueue.value.push({ value: img, drawingId: drawingId, imgType: imgType });
      resolve(getURL());
    });
  }
  async function getURL() {
    if (isRequesting.value || requestQueue.value.length === 0) {
      return;
    }
    isRequesting.value = true;
    const requestFn = requestQueue.value.shift();
    try {
      let query = {
        imgValue: requestFn.value,
        imgType: requestFn.imgType,
      };
      await uploadImg(query).then(res => {
        imgList.value.push({ drawingId: requestFn.drawingId, url: res.data.url });
      });
    } catch (error: any) {
      createMessage.error(error);
    } finally {
      isRequesting.value = false;
      getURL();
    }
  }
  // 销毁示例
  const handleDisposeUnit = () => {
    unref(jnpfUniverRef)?.handleDisposeUnit();
  };
  // 获取sheet列表
  function getSheetList() {
    const sheets = state.jnpfUniverAPI?.getActiveWorkbook()?.getSheets();
    const sheetList = sheets.map(o => ({ id: o.getSheetId(), fullName: o.getSheetName() }));
    return sheetList;
  }
  function init() {
    state.dataSetList = (props.dataSetList as any[]).map(o => ({
      ...o,
      jnpfId: o.id,
      children: !o.children ? [] : o.children.map(c => ({ ...c, jnpfId: o.fullName + '.' + c.id, fullName: c.label ? `${c.id}（${c.label}）` : c.id })),
    }));
    state.convertConfig = props.convertConfig;
    state.queryList = props.queryList ? JSON.parse(props.queryList) : [];
    state.sortList = props.sortList ? JSON.parse(props.sortList) : [];
    const customs = props.cells ? JSON.parse(props.cells) : {};
    const floatEcharts = customs.floatEcharts || {};
    let floatImages = customs.floatImages || {};
    floatImages = getFloatImageUrl(floatImages, globSetting.reportApiUrl);
    let snapshot = props.snapshot ? JSON.parse(props.snapshot) : {};
    if (snapshot.resources?.length) {
      let item = getFloatUrl(snapshot.resources, globSetting.reportApiUrl);
      snapshot.resources = item.list;
    }
    state.key = +new Date();
    nextTick(() => {
      handleCreate(snapshot, floatEcharts, floatImages);
    });
  }
  // 创建报表实例
  function handleCreate(snapshot, floatEcharts?, floatImages?) {
    const res = unref(jnpfUniverRef)?.handleCreateDesignUnit({
      snapshot,
      floatEcharts,
      floatImages,
    });
    state.jnpfUniverAPI = res ? res?.jnpfUniverAPI : null;

    onReportCommandExecuted();
  }
  function onReportCommandExecuted() {
    state.jnpfUniverAPI?.onCommandExecuted((command: any) => {
      const { id: commandId } = command ?? {};
      // 单元格清除内容/清除全部
      if (commandId === 'sheet.command.clear-selection-all' || commandId === 'sheet.command.clear-selection-content') {
        state.activeData.type = 'text';
        state.activeData.cellData.v = '';
        state.activeData.text = defaultCellProperties.text;
        updateCellProperties();
      }
      // 导入excel
      if (commandId === 'jnpf-sheets.operation.import-excel-file') importExcel();
      // 切换sheet
      if (commandId === 'sheet.operation.set-worksheet-active') {
        state.sheetId = command.params.subUnitId;
      }
    });
  }
  function importExcel() {
    const input = document.createElement('input');
    input.type = 'file';
    input.accept = '.xlsx,.xls,.csv';
    input.click();
    input.onchange = () => {
      const file = input.files?.[0];
      if (!file) return;
      const formData = new FormData();
      formData.append('file', file);
      uploadFileExcel(formData).then(res => {
        const snapshot = res.data.snapshot ? JSON.parse(res.data.snapshot) : null;
        state.key = +new Date();
        nextTick(() => {
          handleCreate(snapshot);
        });
      });
    };
  }
  function handleOpenQueryModal() {
    const sheetList = getSheetList();
    openQueryModal(true, { dataSetList: state.dataSetList, queryList: cloneDeep(state.queryList), sheetList });
  }
  function handleOpenSortModal() {
    const sheetList = getSheetList();
    openSortModal(true, { dataSetList: state.dataSetList, sortList: cloneDeep(state.sortList), sheetList });
  }
  function onQueryConfirm(data: any) {
    state.queryList = data;
  }
  function onSortConfirm(data: any) {
    state.sortList = data;
  }
  // 图表聚焦
  function handleFocusEchart(data: any) {
    const custom = { ...data.option, chartType: data.echartType };
    const activeData: any = {
      drawingId: data.drawingId,
      type: 'chart',
      custom,
    };
    onActive(activeData);
  }
  // 悬浮图片聚焦
  function handleFocusFloatImage(data: any) {
    state.activeData.type = 'floatImage';
    state.activeData.floatImage = { ...data };
    const floatImage = { ...data };
    floatImage.option.source = floatImage.option.source || 1;
    if (floatImage.option.src && floatImage.option.src.indexOf(globSetting.reportApiUrl) > -1 && floatImage.option.source == 1 && floatImage.imageType == 'URL')
      floatImage.option.src = floatImage.option.src.split(globSetting.reportApiUrl)[1];
    state.activeData.floatImage = floatImage;
  }
  // 单元格选中
  function handleChangeCellFromSelection(data: any) {
    const getActiveSheet = state.jnpfUniverAPI?.getActiveWorkbook()?.getActiveSheet();
    const activeData: any = {
      ...data,
      type: data.cellData?.custom?.type || 'text',
      custom: data.cellData.custom || {},
      getMaxColumns: getActiveSheet?.getMaxColumns(),
      getMaxRows: getActiveSheet?.getMaxRows(),
      getCustomRowNameList: getCustomRowNameList(getActiveSheet?.getMaxColumns()),
    };
    state.isInitCell = true;
    onActive(activeData);
  }
  function getCustomRowNameList(max) {
    let list: any = [];
    for (let i = 0; i <= max - 1; i++) {
      let ele = getAlphabetFromIndexRule(i);
      list.push(ele);
    }
    return list;
  }
  // 获取弹窗选择的结果
  function getCellFromDialogSelect(res: any) {
    state.cellFromDialogValue = res || '';
  }
  // 点击单元格或者图表
  function onActive(data) {
    const { type = 'text', custom = {} } = data;
    state.activeData = data;
    state.activeData.type = type;
    state.activeData.text = cloneDeep(type === 'text' && JSON.stringify(custom) !== '{}' ? custom : defaultCellProperties.text);
    state.activeData.chart = cloneDeep(type === 'chart' ? custom : unref(getDefaultEchartOptions).bar);
    state.activeData.parameter = cloneDeep(type === 'parameter' ? custom : defaultCellProperties.parameter);
    state.activeData.dataSource = cloneDeep(type === 'dataSource' ? custom : defaultCellProperties.dataSource);
  }
  // 更新单元格或者图表属性
  function updateCellProperties() {
    if (state.isInitCell) return (state.isInitCell = false);
    const data = state.activeData;
    if (['dataSource', 'text', 'parameter'].includes(data.type)) {
      const cell = {
        startColumn: data.startColumn,
        startRow: data.startRow,
        cellData: {
          ...data.cellData,
          ...(data.type === 'parameter' ? { v: data[data.type].field } : {}),
          ...(data.type === 'dataSource' ? { v: data[data.type].field } : {}),
          custom: { ...data[data.type], type: data.type },
        },
      };
      unref(jnpfUniverRef)?.updateCellsData([cell]);
      return;
    }
    if (data.type === 'chart') {
      const dataCopy = cloneDeep(data);
      const option = getRealChartOptions(dataCopy.chart);
      const item = {
        drawingId: dataCopy.drawingId,
        echartType: dataCopy.chart.chartType,
        option,
      };
      unref(jnpfUniverRef)?.updateFloatEchartConfig(item);
    }
    if (data.type === 'floatImage') {
      const item = cloneDeep(data.floatImage);
      if (item.option.source === 1 && item.imageType === 'URL' && item.option.src) item.option.src = globSetting.reportApiUrl + item.option.src;
      unref(jnpfUniverRef)?.updateFloatImageConfig(item);
    }
  }
  // 完善图表的option
  function getRealChartOptions(option) {
    const styleType = option.styleType;
    const data = [-5, -20, -36, -10, -10, -20];
    option.series = option.series.map((o, index) => ({
      ...o,
      data: option.chartType == 'bar' && styleType == 6 && index == 1 ? data : o.data,
      type: option.chartType,
      ...(['bar', 'line'].includes(option.chartType) ? { itemStyle: { color: getColor(option.color.list, index) } } : {}),
      ...(option.chartType === 'line'
        ? {
            smooth: styleType == 2,
            step: styleType == 3,
            stack: styleType == 4 ? 'total' : '',
            lineStyle: { width: option.line.width },
            symbolSize: option.line.symbolSize,
          }
        : {}),
      ...(option.chartType === 'line' && option.areaStyle ? { areaStyle: option.areaStyle } : {}),
      ...(option.chartType === 'bar' ? { showBackground: styleType == 4 } : ''),
      ...(option.chartType === 'bar' ? { stack: styleType == 5 ? o.name : styleType == 2 || styleType == 6 ? 'total' : '' } : ''),
      ...(option.chartType === 'pie'
        ? {
            radius: styleType == 2 ? ['30%', '60%'] : '50%',
            center: [option.seriesCenter.seriesCenterLeft + '%', option.seriesCenter.seriesCenterTop + '%'],
            roseType: option.pie.roseType ? 'area' : '',
            label: getLabel(option),
            color: getPieColor(option.color.list),
            data: getPieData(option.pie, o.data),
            emphasis: {
              itemStyle: {
                shadowBlur: 10,
                shadowOffsetX: 0,
                shadowColor: 'rgba(0, 0, 0, 0.5)',
              },
            },
          }
        : {}),
      ...(option.chartType === 'radar'
        ? {
            color: getColor(option.color.list, index),
            areaStyle: {
              opacity: option.seriesAreaStyleOpacity,
            },
          }
        : {}),
    }));
    if (option.chartType === 'bar' || option.chartType === 'line') {
      option.xAxis.type = option.xAxis.type == 'category' ? 'category' : 'value';
      option.xAxis.nameTextStyle.fontWeight = option.xAxis.nameTextStyle.fontWeight ? 'bolder' : '';
      option.xAxis.axisLabel.fontWeight = option.xAxis.axisLabel.fontWeight ? 'bolder' : '';
      option.xAxis.axisLabel.color = option.color.AxisTextStyleColor ? option.color.AxisTextStyleColor : option.xAxis.axisLabel.color;
      option.xAxis.axisLine.show = option.xAxis.show;
      option.xAxis.axisLine.lineStyle.color = option.color.AxisLineStyleColor ? option.color.AxisLineStyleColor : option.xAxis.axisLine.lineStyle.color;

      option.yAxis.type = option.xAxis.type == 'category' ? 'value' : 'category';
      option.yAxis.nameTextStyle.fontWeight = option.yAxis.nameTextStyle.fontWeight ? 'bolder' : '';
      option.yAxis.axisLabel.fontWeight = option.yAxis.axisLabel.fontWeight ? 'bolder' : '';
      option.yAxis.axisLabel.color = option.color.AxisTextStyleColor ? option.color.AxisTextStyleColor : option.yAxis.axisLabel.color;
      option.yAxis.axisLine.show = option.yAxis.show;
      option.yAxis.axisLine.lineStyle.color = option.color.AxisLineStyleColor ? option.color.AxisLineStyleColor : option.yAxis.axisLine.lineStyle.color;

      option.label.fontWeight = option.label.fontWeight ? 'bolder' : '';

      if (option.chartType === 'line' && option.areaStyle) {
        option.xAxis.boundaryGap = false;
        option.areaStyle = {};
      }
      option.legend.top = option.legendTop + '%';
      option.legend.left = option.legendLeft + '%';
    }
    if (option.chartType === 'radar') {
      option.radar.shape = styleType == 1 ? 'polygon' : 'circle';
      option.radar.axisName.fontWeight = option.radar.axisName.fontWeight ? 'bolder' : 'normal';
      option.radar.center = [option.seriesCenter.seriesCenterLeft + '%', option.seriesCenter.seriesCenterTop + '%'];
    }
    return option;
  }
  function clickDesignModule() {
    emit('closeVersionPopover');
  }
  function handlePreview() {
    emit('changeModalLoading', true);
    getPreviewDesign(props.versionId)
      .then(_ => {
        openPreviewModal(true, { id: '', versionId: props.versionId, sheetId: state.sheetId });
      })
      .finally(() => {
        return emit('changeModalLoading', false);
      });
  }
  function getPieColor(colorList) {
    let colors: any = [];
    if (colorList && colorList.length) {
      const list: any[] = [];
      colorList.map((_item, index) => {
        const color = getColor(colorList, index) || '#71B6F5';
        list.push(color);
      });
      colors = list;
    }
    return colors;
  }
  function getColor(colorList, index) {
    const barColor = colorList || [];
    if (barColor[index]) {
      const color1 = barColor[index].color1;
      const color2 = barColor[index].color2;
      if (color2 && color1)
        return new newEcharts.graphic.LinearGradient(0, 0, 0, 1, [
          { offset: 0, color: color1 },
          { offset: 1, color: color2 },
        ]);
      return color1;
    }
  }
  function getLabel(option) {
    let label: any = {};
    label.show = option.label.show;
    label.position = option.seriesLabelPosition;
    const seriesLabelShowInfo = option.seriesLabelShowInfo;
    if (seriesLabelShowInfo && seriesLabelShowInfo.length) {
      if (seriesLabelShowInfo.includes('count') && seriesLabelShowInfo.includes('percent')) {
        label.formatter = '{b}: {c} ({d}%)';
      } else if (seriesLabelShowInfo.includes('count')) {
        label.formatter = '{b}: {c} ';
      } else if (seriesLabelShowInfo.includes('percent')) {
        label.formatter = '{b}: ({d}%) ';
      }
    }
    return label;
  }
  function getPieData(option, list) {
    if (option.showZero) list = list.filter(item => item.value != 0);
    if (option.sortable) {
      for (let i = 0; i < list.length - 1; i++) {
        for (let j = 0; j < list.length - 1; j++) {
          if (list[j].value > list[j + 1].value) {
            let t = list[j];
            list[j] = list[j + 1];
            list[j + 1] = t;
          }
        }
      }
    }
    return list;
  }

  onMounted(() => {
    init();
  });

  defineExpose({ getData, handleDisposeUnit });
</script>

<style lang="less">
  @import '../../style/index.less';

  .tabs-content {
    overflow-x: hidden;
    overflow-y: auto;
    .right-radio {
      .ant-radio-button-wrapper {
        padding: 0 11px;
      }
    }
  }

  .dataSet-content {
    padding: 10px;
    overflow: hidden;
  }

  .set-query-wrap {
    border-top: 1px solid @border-color-base1;
    padding: 10px;
    box-sizing: border-box;
  }

  .univer-dialog-root .univer-dialog-mask {
    background: #00000080 !important;
  }
</style>
