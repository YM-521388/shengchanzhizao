import { upperFirst } from 'lodash-es';
import { getDataInterfaceRes } from '@/api/systemData/dataInterface';
import { useBaseStore } from '@/store/modules/base';
import { computed, reactive } from 'vue';
import * as newEcharts from 'echarts';
import { useI18n } from '@/hooks/web/useI18n';

interface State {
  searchSchemas: any[];
}

const { t } = useI18n();

export const useReport = (formApi: any = {}) => {
  const baseStore = useBaseStore();
  const state = reactive<State>({
    searchSchemas: [],
  });

  const searchSchemas = computed(() => state.searchSchemas);

  // 更新图表数据
  function getRealEchart(echarts, chartData) {
    if (!echarts) return null;
    Object.keys(echarts).forEach(key => {
      const chart = echarts[key];
      const option = chart.option;
      const styleType = option.styleType;
      const colorList = option.color?.list || [];
      let barTypeList = [];
      if (chart.echartType === 'bar') barTypeList = option.bar.barTypeList;
      const dataList = chartData.filter(o => o.drawingId === key);
      let data: any = {};
      if (option.title.textI18nCode) option.title.text = t(option.title.textI18nCode, option.title.text);
      if (option.title.subtextI18nCode) option.title.subtext = t(option.title.subtextI18nCode, option.title.subtext);
      if (dataList.length) data = dataList[0].field;
      const { classifyNameField = [], seriesDataField = [], seriesNameField = [], maxField = [] } = data;
      // 柱状图和线性图
      if (['bar', 'line'].includes(chart.echartType)) {
        const series = seriesDataField?.map((o, index) => {
          return {
            name: seriesNameField[index],
            data: o,
            type: chart.echartType == 'bar' && styleType == 7 ? getEchartType(barTypeList, classifyNameField[index]) : chart.echartType,
            itemStyle: { color: getColor(colorList, index) },
            ...(chart.echartType === 'line'
              ? {
                  smooth: styleType == 2,
                  step: styleType == 3,
                  stack: styleType == 4 ? 'total' : '',
                  lineStyle: { width: chart.line?.width },
                  symbolSize: chart.line?.symbolSize,
                }
              : {}),
            ...(chart.echartType === 'line' && option.areaStyle ? { areaStyle: option.areaStyle } : {}),
            ...(chart.echartType === 'bar'
              ? {
                  showBackground: styleType == 4,
                  stack: styleType == 5 || styleType == 7 ? seriesNameField[index] : styleType == 2 || styleType == 6 ? 'total' : '',
                }
              : ''),
          };
        });
        chart.option.series = series;
        chart.option.legend.data = seriesNameField;
        chart.option.xAxis.data = classifyNameField;
      }
      // 饼图
      if (chart.echartType === 'pie') {
        const series = seriesDataField?.map((o, index) => {
          const data = o?.map((item, sIndex) => {
            return {
              value: item,
              name: classifyNameField?.[sIndex],
            };
          });
          return {
            name: seriesNameField[index],
            type: 'pie',
            radius: styleType == 2 ? ['30%', '60%'] : '50%',
            center: [option.seriesCenter.seriesCenterLeft + '%', option.seriesCenter.seriesCenterTop + '%'],
            roseType: option.pie.roseType ? 'area' : '',
            label: getLabel(option),
            color: getPieColor(colorList),
            data: getPieData(option.pie, data),
            emphasis: {
              itemStyle: {
                shadowBlur: 10,
                shadowOffsetX: 0,
                shadowColor: 'rgba(0, 0, 0, 0.5)',
              },
            },
          };
        });
        chart.option.series = series;
      }
      // 雷达图
      if (chart.echartType === 'radar') {
        let series: any = [];
        let data: any = [];
        const newMaxField: any = [];
        for (let index = 0; index < seriesDataField.length; index++) {
          const element = seriesDataField[index];
          if (!maxField.length) element.length && newMaxField.push(Math.max(...element));
          let item = {
            value: element,
            name: seriesNameField?.[index],
            areaStyle: { color: getColor(colorList, index), opacity: option.seriesAreaStyleOpacity },
          };
          data.push(item);
        }
        series = [
          {
            type: chart.echartType,
            data,
          },
        ];
        if (!maxField.length) maxField.push(...newMaxField);
        const indicator = maxField?.map((o, sIndex) => {
          return {
            max: o,
            name: classifyNameField?.[sIndex],
          };
        });
        const radarObj = {
          axisName: option.radar.axisName,
          indicator: indicator,
          shape: styleType == 1 ? 'polygon' : 'circle',
          center: [option.seriesCenter.seriesCenterLeft + '%', option.seriesCenter.seriesCenterTop + '%'],
        };
        chart.option.series = series;
        chart.option.radar = radarObj;
      }
    });
    return echarts;
  }
  // 获取搜索内容
  function getSearchSchema(queryList, sheetId?) {
    if (!queryList.length) return (state.searchSchemas = []);
    let item: any = [];
    if (sheetId) {
      item = queryList.filter(o => o.sheet === sheetId)[0];
    } else {
      item = queryList[0];
    }
    if (!item) return (state.searchSchemas = []);
    const list = item.queryList || [];
    const schemas = list.map((option: any) => {
      const { label, type, vModel, config, searchMultiple } = option;
      const field = vModel?.replace('.', '-');
      let newType = getType(type);
      if (type === 'select') {
        config.fieldNames = { value: config.propsValue };
      }
      if (searchMultiple) config.multiple = true;
      return {
        field,
        label,
        component: newType,
        componentProps: config,
      };
    });
    // 异步更新 select 类型字段的 schema
    for (const cur of schemas) {
      const config = cur?.componentProps || {};
      if (cur?.component === 'Select') {
        if (config.dataType === 'dictionary' && config.dictionaryType) {
          baseStore.getDicDataSelector(config.dictionaryType).then(res => {
            formApi.updateSchema([{ field: cur.field, componentProps: { options: res } }]);
          });
        } else if (config.dataType === 'dynamic' && config.propsUrl) {
          const query = { paramList: config.templateJson || [] };
          getDataInterfaceRes(config.propsUrl, query).then(res => {
            const data = Array.isArray(res.data) ? res.data : [];
            formApi.updateSchema([{ field: cur.field, componentProps: { options: data } }]);
          });
        }
      }
    }
    state.searchSchemas = schemas;
  }
  function clearSearchSchema() {
    state.searchSchemas = [];
  }
  function getType(type) {
    if (type == 'date') return 'DateRange';
    if (type == 'time') return 'TimeRange';
    if (type == 'inputNumber') return 'NumberRange';
    return upperFirst(type);
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
  function getEchartType(list, title) {
    const arr = list.find(ele => title == ele.id);
    if (arr && arr.type) return arr.type;
  }

  return {
    getRealEchart,
    getSearchSchema,
    searchSchemas,
    clearSearchSchema,
  };
};
