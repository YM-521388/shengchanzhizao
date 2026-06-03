<template>
  <div class="filed-box">
    <ScrollContainer class="!pt-0px">
      <a-form :model="activeData" layout="vertical" :colon="false">
        <a-collapse v-model:activeKey="activeKey" accordion ghost expandIconPosition="end">
          <template v-if="activeData.type == 'chart'">
            <ReportChartProperties :key="1" v-bind="getChartBind" @updateDefaultChart="updateDefaultChart" />
            <ReportChartCommonProperties :key="2" v-bind="getChartBind" />
            <ReportCharBarProperties :key="3" v-bind="getChartBind" v-if="activeData.chart.chartType == 'bar'" />
            <ReportChartDataProperties :key="4" v-bind="getChartBind" />
            <ReportChartMainTitleProperties :key="5" v-bind="getChartBind" />
            <ReportChartSubtitleProperties :key="6" v-bind="getChartBind" />
            <ReportChartXAxisProperties :key="7" v-bind="getChartBind" v-if="['bar', 'line'].includes(activeData.chart.chartType)" />
            <ReportChartYAxisProperties :key="8" v-bind="getChartBind" v-if="['bar', 'line'].includes(activeData.chart.chartType)" />
            <ReportChartLabelProperties :key="9" v-bind="getChartBind" />
            <ReportCharTooltipProperties :key="10" v-bind="getChartBind" />
            <ReportChartMarginProperties :key="11" v-bind="getChartBind" />
            <ReportChartLegendProperties :key="12" v-bind="getChartBind" />
            <ReportCharColorProperties :key="13" v-bind="getChartBind" />
          </template>
          <template v-else-if="activeData.type == 'floatImage'">
            <ReportImageProperties :key="1" :floatImage="activeData.floatImage" />
          </template>
          <template v-else>
            <a-collapse-panel :key="1" header="基础设置">
              <a-form-item name="type" label="单元类型">
                <jnpf-select v-model:value="activeData.type" :options="options" @change="onTypeChange" />
              </a-form-item>
              <template v-if="activeData.type === 'text'">
                <ReportTextProperties :text="activeData.text" v-bind="getCommonBind" />
              </template>
              <template v-if="activeData.type === 'dataSource'">
                <ReportDataProperties :dataSource="activeData.dataSource" v-bind="getCommonBind" />
              </template>
              <template v-if="activeData.type === 'parameter'">
                <ReportParamProperties :parameter="activeData.parameter" v-bind="getCommonBind" />
              </template>
            </a-collapse-panel>
          </template>
        </a-collapse>
      </a-form>
    </ScrollContainer>
  </div>
</template>

<script setup lang="ts">
  import { reactive, toRefs, onMounted, computed, watch, unref } from 'vue';
  import { ScrollContainer } from '@/components/Container';
  import ReportDataProperties from './ReportDataProperties.vue';
  import ReportChartProperties from './ReportChartProperties.vue';
  import ReportChartMainTitleProperties from './ReportChartMainTitleProperties.vue';
  import ReportChartSubtitleProperties from './ReportChartSubtitleProperties.vue';
  import ReportChartDataProperties from './ReportChartDataProperties.vue';
  import ReportChartLegendProperties from './ReportChartLegendProperties.vue';
  import ReportChartMarginProperties from './ReportChartMarginProperties.vue';
  import ReportChartCommonProperties from './ReportChartCommonProperties.vue';
  import ReportCharBarProperties from './ReportCharBarProperties.vue';
  import ReportChartXAxisProperties from './ReportChartXAxisProperties.vue';
  import ReportChartYAxisProperties from './ReportChartYAxisProperties.vue';
  import ReportChartLabelProperties from './ReportChartLabelProperties.vue';
  import ReportCharColorProperties from './ReportCharColorProperties.vue';
  import ReportCharTooltipProperties from './ReportCharTooltipProperties.vue';
  import ReportImageProperties from './ReportImageProperties.vue';
  import ReportTextProperties from './ReportTextProperties.vue';
  import ReportParamProperties from './ReportParamProperties.vue';

  interface State {
    activeKey: Number;
  }

  const props = defineProps([
    'activeSheetId',
    'activeData',
    'dataSetList',
    'queryList',
    'defaultEchartOptions',
    'jnpfUniverRef',
    'imageConfig',
    'cellFromDialogValue',
  ]);

  const state = reactive<State>({
    activeKey: 1,
  });
  const { activeKey } = toRefs(state);

  const options = [
    { id: 'text', fullName: '文本' },
    { id: 'dataSource', fullName: '数据集' },
    { id: 'parameter', fullName: '参数' },
    // { id: 'chart', fullName: '图表' },
    // { id: 'link', fullName: '链接' },
    // { id: 'image', fullName: '图片' },
    // { id: 'barcode', fullName: '条形码' },
    // { id: 'qrcode', fullName: '二维码' },
  ];

  const systemParameters = [
    {
      id: 'system',
      fullName: '系统参数',
      children: [
        { id: '@userId', fullName: '@userId（当前用户）' },
        {
          id: '@userAndSubordinates',
          fullName: '@userAndSubordinates（当前用户及下属）',
        },
        { id: '@organizeId', fullName: '@organizeId（当前组织）' },
        {
          id: '@organizationAndSuborganization',
          fullName: '@organizationAndSuborganization（当前组织及子组织）',
        },
        {
          id: '@branchManageOrganize',
          fullName: '@branchManageOrganize（当前分管组织）',
        },
      ],
    },
  ];

  const getCommonBind = computed(() => ({
    dataSetList: props.dataSetList,
    queryList: unref(getQueryList),
    getCustomRowNameList: props.activeData.getCustomRowNameList,
    getMaxRows: props.activeData.getMaxRows,
    jnpfUniverRef: props.jnpfUniverRef,
    startColumn: props.activeData.startColumn,
    startRow: props.activeData.startRow,
    cellFromDialogValue: props.cellFromDialogValue,
  }));
  const getChartBind = computed(() => ({
    dataSetList: props.dataSetList,
    chart: props.activeData.chart,
  }));

  const getQueryList = computed(() => {
    const treeData = { id: 'query', fullName: '查询参数' };

    const children = (props.queryList || [])
      .filter(item => item.sheet === props.activeSheetId)
      .flatMap(item =>
        (item.queryList || []).map(option => ({
          id: option.vModel,
          fullName: `${option.vModel}（${option.label}）`,
        })),
      );

    return children?.length ? [...systemParameters, { ...treeData, children }] : [...systemParameters];
  });

  watch(
    () => props.activeData.type,
    () => {
      state.activeKey = 1;
    },
    { deep: true },
  );

  function updateDefaultChart(type) {
    const newChart = {
      ...(props.defaultEchartOptions[type] || {}),
      classifyNameField: props.activeData.chart.classifyNameField,
      seriesNameField: props.activeData.chart.seriesNameField,
      seriesDataField: props.activeData.chart.seriesDataField,
      maxField: props.activeData.chart.maxField,
      summaryType: props.activeData.chart.summaryType,
      chartType: type,
      title: props.activeData.chart.title,
      tooltip: props.activeData.chart.tooltip,
    };
    props.activeData.chart = newChart;
  }
  function onTypeChange() {
    props.activeData.cellData.v = '';
  }

  onMounted(() => {
    state.activeKey = 1;
  });
</script>
