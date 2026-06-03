<template>
  <div class="bigmap-container" id="bigScreen">
    <div class="title-container">
      <div class="time">{{ state.newTime }}</div>
      <div class="title">生产中控大屏</div>
      <div class="fullscreen" @click="changeMode">{{ getScreenMode() === 'notfull' ? '全屏' : '退出全屏' }}</div>
    </div>
    <div class="content-container">
      <div class="top-wrapper">
        <div class="width-32-container chart-coomon-container">
          <div class="title-common-container">不良品项分布 </div>
          <div class="content-common-container">
            <div class="echarts_container" id="charDistr"></div>

          </div>
        </div>
        <div class="width-35-container num-container">
          <div class="grid-item" :style="{ 'background-color': item.bgColor }" v-for="(item, index) in state.numData"
            :key="index">
            <div>{{ item.name }}</div>
            <div>
              <span class="num">{{ item.num }}</span>
              <span class="unit">{{ item.unit }}</span>
            </div>
          </div>
        </div>
        <div class="width-32-container chart-coomon-container">
          <div class="title-common-container"> 员工产能趋势(最近30天)</div>
          <div class="content-common-container">
            <div class="echarts_container" id="charTrend"></div>
          </div>
        </div>
      </div>
      <div class="middle-wrapper">
        <div class="width-32-container chart-coomon-container">
          <div class="title-common-container">人员不良品率(Top10) </div>
          <div class="content-common-container">
            <div class="echarts_container" id="chartRate"></div>

          </div>
        </div>
        <div class="width-35-container">
          <div class="title-common-container">实时报工数据 </div>
          <div class="content-common-container">
            <div class="table-header-container">
              <div class="table-w-20">时间</div>
              <div class="table-w-10">报工人</div>
              <div class="table-w-10">工序</div>
              <div class="table-w-15">产品</div>
              <div class="table-w-15">编号</div>
              <div class="table-w-15">良品数</div>
              <div class="table-w-15">不良品数</div>
            </div>
            <div class="table-content-container">
              <ScrollList :data="state.realtimeReportData" :item-height="40" ref="realtimeRef">
                <template #item="{ item }">
                  <div class="table-item-common">
                    <div class="table-w-20">
                      <div class="text-ellipsis" :title="item.F_CreatorTime">{{ item.F_CreatorTime }}</div>
                    </div>
                    <div class="table-w-10">
                      <div class="text-ellipsis" :title="item.F_CreatorUserId">{{ item.F_CreatorUserId }}</div>
                    </div>
                    <div class="table-w-10">
                      <div class="text-ellipsis" :title="item.F_ProcessName">{{ item.F_ProcessName }}</div>
                    </div>
                    <div class="table-w-15">
                      <div class="text-ellipsis" :title="item.F_GoodsName">{{ item.F_GoodsName }}</div>
                    </div>
                    <div class="table-w-15">
                      <div class="text-ellipsis" :title="item.F_GoodsNo">{{ item.F_GoodsNo }}</div>
                    </div>
                    <div class="table-w-15">
                      <div class="text-ellipsis" :title="item.F_GoodQty">{{ item.F_GoodQty }}</div>
                    </div>
                    <div class="table-w-15">
                      <div class="text-ellipsis" :title="item.F_GoodNotQty">{{ item.F_GoodNotQty }}</div>
                    </div>
                  </div>
                </template>
              </ScrollList>
            </div>
          </div>
        </div>
        <div class="width-32-container">
          <div class="title-common-container">人员生产统计 </div>
          <div class="content-common-container">

            <div class="table-header-container">
              <div class="table-w-25">人员</div>
              <div class="table-w-25">今日报工数</div>
              <div class="table-w-25">本周不良品数</div>
              <div class="table-w-25">本月不良品数</div>
            </div>
            <div class="table-content-container">
              <ScrollList :data="state.productStatisticsData" :item-height="40" ref="productRef">
                <template #item="{ item }">
                  <div class="table-item-common">
                    <div class="table-w-25">
                      <div class="text-ellipsis" :title="item.RealName">{{ item.RealName }}</div>
                    </div>
                    <div class="table-w-25">
                      <div class="text-ellipsis" :title="item.DayNum">{{ item.DayNum }}</div>
                    </div>
                    <div class="table-w-25">
                      <div class="text-ellipsis" :title="item.WeekNum">{{ item.WeekNum }}</div>
                    </div>
                    <div class="table-w-25">
                      <div class="text-ellipsis" :title="item.MonthNum">{{ item.MonthNum }}</div>
                    </div>
                  </div>
                </template>
              </ScrollList>
            </div>

          </div>
        </div>
      </div>
      <div class="bottom-wrapper">
        <div class="title-common-container">工单执行进度</div>
        <div class="content-common-container">

          <div class="table-header-container">
            <div class="table-w-10">工单编号</div>
            <div class="table-w-10">产品</div>
            <div class="table-w-10">编号</div>
            <div class="table-w-5">规格</div>
            <div class="table-w-5">计划数</div>
            <div class="table-w-5">优先级</div>
            <div class="table-w-5">工序数</div>
            <div class="table-w-5">
              <a-tooltip placement="top">
                <template #title>
                  <span>已完成工序</span>
                </template>
                <div class="text-ellipsis">已完成工序</div>
              </a-tooltip>
            </div>
            <div class="table-w-5">工序进度</div>
            <div class="table-w-40">工序名称</div>
          </div>
          <div class="table-content-container">
              <ScrollList :data="state.workOrderData"  ref="productRef">
                <template #item="{ item }" #index="{index}">
                  <div class="table-item">
                <div class="table-w-10">
                  <div class="text-ellipsis" :title="item.F_ProcessNo">{{ item.F_ProcessNo }}</div>
                </div>
                <div class="table-w-40">
                  <div class="process-flow-container">
                    <template v-for="(task, index) in item.ProdTaskList" :key="task.id">
                      <div class="process-flow-item">
                        <!-- 进度环 - SVG实现 -->
                        <div class="progress-circle-wrapper">
                          <svg class="progress-svg" viewBox="0 0 44 44">
                            <!-- 背景圆环 -->
                            <circle :class="task.F_CompletedQty == '0' ? 'progress-track-0' : 'progress-track'" cx="22"
                              cy="22" r="18" />
                            <!-- 进度圆环 -->
                            <circle class="progress-bar" cx="22" cy="22" r="18" :stroke="getTaskColor(task)"
                              :stroke-dasharray="113.097" :stroke-dashoffset="getCircleOffset(task)"
                              transform="rotate(-90 22 22)" />
                          </svg>
                          <!-- 百分比文字 -->
                          <div class="circle-text">
                            <span class="circle-percent">{{ getTaskPercent(task) }}%</span>
                          </div>
                        </div>
                        <!-- 名称 -->
                        <div class="process-name">{{ task.F_ProcessName }}</div>
                      </div>
                      <!-- 连接线 -->
                      <div class="process-connector" v-if="index < item.ProdTaskList.length - 1"></div>
                    </template>
                  </div>
                </div>
                  </div>
                </template>
              </ScrollList>
            <!-- <div class="static-list" v-if="state.workOrderData.length <= 3">

              <div class="table-item" v-for="(item, index) in state.workOrderData">
                <div class="table-w-10">
                  <div class="text-ellipsis" :title="item.F_ProcessNo">{{ item.F_ProcessNo }}</div>
                </div>
                <div class="table-w-10">
                  <div class="text-ellipsis" :title="item.F_GoodsName">{{ item.F_GoodsName }}</div>
                </div>
                <div class="table-w-10">
                  <div class="text-ellipsis" :title="item.F_GoodsNo">{{ item.F_GoodsNo }}</div>
                </div>
                <div class="table-w-5">
                  <div class="text-ellipsis" :title="item.F_Specification">{{ item.F_Specification }}</div>
                </div>
                <div class="table-w-5">
                  <div class="text-ellipsis" :title="item.F_PlanQty">{{ item.F_PlanQty }}</div>
                </div>
                <div class="table-w-5">
                  <div class="text-ellipsis" :title="item.F_Priority">{{ item.F_Priority }}</div>
                </div>
                <div class="table-w-5">
                  <div class="text-ellipsis" :title="item.F_TaskAllNum">{{ item.F_TaskAllNum }}</div>
                </div>
                <div class="table-w-5">
                  <div class="text-ellipsis" :title="item.F_TaskYseNum">{{ item.F_TaskYseNum }}</div>
                </div>
                <div class="table-w-5">
                  <div class="text-ellipsis" :title="item.F_OrderTask">{{ item.F_OrderTask }}</div>
                </div>
                <div class="table-w-40">
                  <div class="process-flow-container">
                    <template v-for="(task, index) in item.ProdTaskList" :key="task.id">
                      <div class="process-flow-item">
                        进度环 - SVG实现
                        <div class="progress-circle-wrapper">
                          <svg class="progress-svg" viewBox="0 0 44 44">
                            背景圆环
                            <circle :class="task.F_CompletedQty == '0' ? 'progress-track-0' : 'progress-track'" cx="22"
                              cy="22" r="18" />
                            进度圆环
                            <circle class="progress-bar" cx="22" cy="22" r="18" :stroke="getTaskColor(task)"
                              :stroke-dasharray="113.097" :stroke-dashoffset="getCircleOffset(task)"
                              transform="rotate(-90 22 22)" />
                          </svg>
                          百分比文字
                          <div class="circle-text">
                            <span class="circle-percent">{{ getTaskPercent(task) }}%</span>
                          </div>
                        </div>
                        工序名称
                        <div class="process-name">{{ task.F_ProcessName }}</div>
                      </div>
                      连接线
                      <div class="process-connector" v-if="index < item.ProdTaskList.length - 1"></div>
                    </template>
                  </div>
                </div>

              </div>
            </div>
            <vue3-seamless-scroll v-else :list="state.workOrderData" :config="state.scrollConfig"
              class="seamless-scroll">
              <div class="table-item" v-for="(item, index) in state.workOrderData">
                <div class="table-w-10">
                  <div class="text-ellipsis" :title="item.F_ProcessNo">{{ item.F_ProcessNo }}</div>
                </div>
                <div class="table-w-10">
                  <div class="text-ellipsis" :title="item.F_GoodsName">{{ item.F_GoodsName }}</div>
                </div>
                <div class="table-w-10">
                  <div class="text-ellipsis" :title="item.F_GoodsNo">{{ item.F_GoodsNo }}</div>
                </div>
                <div class="table-w-5">
                  <div class="text-ellipsis" :title="item.F_Specification">{{ item.F_Specification }}</div>
                </div>
                <div class="table-w-5">
                  <div class="text-ellipsis" :title="item.F_PlanQty">{{ item.F_PlanQty }}</div>
                </div>
                <div class="table-w-5">
                  <div class="text-ellipsis" :title="item.F_Priority">{{ item.F_Priority }}</div>
                </div>
                <div class="table-w-5">
                  <div class="text-ellipsis" :title="item.F_TaskAllNum">{{ item.F_TaskAllNum }}</div>
                </div>
                <div class="table-w-5">
                  <div class="text-ellipsis" :title="item.F_TaskYseNum">{{ item.F_TaskYseNum }}</div>
                </div>
                <div class="table-w-5">
                  <div class="text-ellipsis" :title="item.F_OrderTask">{{ item.F_OrderTask }}</div>
                </div>
                <div class="table-w-40">
                  <div class="process-flow-container">
                    <template v-for="(task, index) in item.ProdTaskList" :key="task.id">
                      <div class="process-flow-item">
                        进度环 - SVG实现
                        <div class="progress-circle-wrapper">
                          <svg class="progress-svg" viewBox="0 0 44 44">
                            背景圆环
                            <circle :class="task.F_CompletedQty == '0' ? 'progress-track-0' : 'progress-track'" cx="22"
                              cy="22" r="18" />
                            进度圆环
                            <circle class="progress-bar" cx="22" cy="22" r="18" :stroke="getTaskColor(task)"
                              :stroke-dasharray="113.097" :stroke-dashoffset="getCircleOffset(task)"
                              transform="rotate(-90 22 22)" />
                          </svg>
                          百分比文字
                          <div class="circle-text">
                            <span class="circle-percent">{{ getTaskPercent(task) }}%</span>
                          </div>
                        </div>
                        工序名称
                        <div class="process-name">{{ task.F_ProcessName }}</div>
                      </div>
                      连接线
                      <div class="process-connector" v-if="index < item.ProdTaskList.length - 1"></div>
                    </template>
                  </div>
                </div>

              </div>
            </vue3-seamless-scroll> -->
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { Vue3SeamlessScroll } from "vue3-seamless-scroll";
import ScrollList from './components/ScrollList.vue'

import * as echarts from 'echarts'

import { useFullscreen } from '@/hooks/fullscreen/useFullscreen';
import { onMounted, reactive, onUnmounted, computed, ref } from 'vue';
import { getNum, getProductTrend, getDistr, getDefectRate, getRealtimeReport, getProductStatistics, getWorkOrder } from '@/api/bigmap/bigmap';
const { enterScreen, changeMode, getScreenMode, setScreenMode } = useFullscreen('bigScreen', 'notfull');

let trendDom: echarts.ECharts | null = null
let distrDom: echarts.ECharts | null = null
let rateDom: echarts.ECharts | null = null
const state = reactive({
  newTime: '',
  timer: null,
  numData: [
    {
      fieldName: "DaiShegnChanGongDan",
      name: "待生产工单",
      num: 0,
      unit: "单",
      bgColor: "rgb(144, 147, 153)"
    },
    {
      fieldName: "JinXingZhongGongDan",
      name: "进行中工单",
      num: 0,
      unit: "单",
      bgColor: "rgb(64, 158, 255)"
    },
    {
      fieldName: "WanChengGongDan",
      name: " 完成工单",
      num: 0,
      unit: "单",
      bgColor: "rgb(103, 194, 58)"
    },
    {
      fieldName: "JinRiBaoGong",
      name: "今日报工",
      num: 0,
      unit: "件",
      bgColor: "rgb(64, 158, 255)"
    },
    {
      fieldName: "JinRiBuLiangPin",
      name: "今日不良品",
      num: 0,
      unit: "件",
      bgColor: "rgb(245, 108, 108)"
    },
    {
      fieldName: "JinEiBuLiangPinLv",
      name: "今日不良品率",
      num: 0,
      unit: "%",
      bgColor: "rgb(230, 162, 60"
    },
    {
      fieldName: "JiaJiGongDan",
      name: "加急工单",
      num: 0,
      unit: "单",
      bgColor: "rgb(245, 108, 108"
    },
    {
      fieldName: "YuQiGongDan",
      name: "逾期工单",
      num: 0,
      unit: "单",
      bgColor: "rgb(245, 108, 108"
    },
    {
      fieldName: "YuQiRenWu",
      name: "逾期任务",
      num: 0,
      unit: "个",
      bgColor: "rgb(245, 108, 108)"
    }
  ],
  productData: {
    xAxis: [],
    series: [],
    name: ""
  },
  distrData: [],
  rateData: {
    xAxis: [],
    series: [],
  },
  realtimeReportData: [] as any[],
  productStatisticsData: [] as any[],
  workOrderData: [] as any[],
  scrollConfig: {
    direction: 'up',
    step: 1, // 滚动步长，建议1-3之间
    limitMoveNum: 4, // 数据超过4条才开始滚动
    hoverStop: true,
    openWatch: true,
    singleHeight: 40, // 单行高度，必须与CSS中的.table-item-common高度一致
    waitTime: 1000, // 等待1秒后开始滚动
    switchDelay: 800, // 切换延迟
    ease: true, // 开启平滑滚动
    hover:true
  }
});
/**
 * 获取任务百分比（显示用）
 * @param {Object} task - 工序任务对象
 * @returns {Number} 百分比整数
 */
const getTaskPercent = (task) => {
  const completedQty = Number(task.F_CompletedQty) || 0;
  const prodQty = Number(task.F_ProdQty) || 0;

  if (completedQty === 0 || prodQty === 0) {
    return 0;
  }

  const percent = (completedQty / prodQty) * 100;
  return Math.round(percent);
};

/**
 * 获取进度环颜色（根据状态码）
 * @param {Object} task - 工序任务对象
 * @returns {String} 颜色值
 */
const getTaskColor = (task) => {
  const statusCode = task.F_TaskStatusCode;
  const colorMap = {
    '0': '#ccc',    // 未开始 - 灰色
    '1': '#1eaceb',    // 进行中 - 蓝色
    '2': '#25a210',    // 已完成 - 绿色
    '3': '#E03845'     // 已暂停 - 红色
  };
  return colorMap[statusCode] || '#ccc'; // 空值默认未开始
};

/**
 * 获取进度环偏移量
 * @param {Object} task - 工序任务对象
 * @returns {Number} 偏移量
 */
const getCircleOffset = (task) => {
  const percent = getTaskPercent(task);
  const circumference = 2 * Math.PI * 18;
  const offset = circumference * (1 - percent / 100);
  return offset;
};
// ==============================接口获取数据 start===================
// 获取数量
const getNumData = () => {
  getNum().then(res => {
    let data = res.data || []
    assignDataToNumData(data)
  }).catch(err => { })
};
const assignDataToNumData = (data) => {
  if (!data || data.length === 0) {
    return;
  }
  const dataObj = data[0];
  state.numData.forEach(item => {
    if (dataObj.hasOwnProperty(item.fieldName)) {
      item.num = dataObj[item.fieldName];
    }
  });
};
// 获取员工产能趋势
const getProductTrendData = () => {
  getProductTrend().then(res => {
    let data = res.data || []
    state.productData.xAxis = data.map(item => item.Date);
    state.productData.series = data.map(item => item.Value);
    state.productData.name = data[0].UserName || ''
    initTrendChart()

  }).catch(err => { })
}
// 不良品项分布
const getDistrData = () => {
  getDistr().then(res => {
    let data = res.data || []
    state.distrData = data.map(item => ({
      'value': item.F_AllNum,
      'name': item.F_DefectName
    }));
    initDistrChart()

  }).catch(err => { })
}
// 人员不良品率
const getDefectRateData = () => {
  getDefectRate().then(res => {
    let data = res.data || []
    state.rateData.xAxis = data.map(item => item.UserName);
    state.rateData.series = data.map(item => item.BadRate);
    console.log(state.rateData, 'state.rateData', data)
    initRatedChart()
  }).catch(err => { })
}
// 实时报工数据
const getRealtimeReportData = () => {
  getRealtimeReport().then(res => {
    let data = res.data || []
    state.realtimeReportData = data
  }).catch(err => { })
}
// 人员生产统计
const getProductStatisticsData = () => {
  getProductStatistics().then(res => {
    let data = res.data || []
    state.productStatisticsData = data

  }).catch(err => { })
}
// 工单执行进度
const getWorkOrderData = () => {
  getWorkOrder().then(res => {
    let data = res.data || []
    state.workOrderData = [data[0]]
  }).catch(err => { })
}


// ==============================图表
const initTrendChart = () => {
  trendDom = echarts.init((document.getElementById('charTrend')) || null)
  const option = {
    backgroundColor: 'transparent',
    grid: {
      left: '5%',
      right: '5%',
      top: '15%',
      bottom: '5%',
      containLabel: true,
    },
    legend: {
      show: true,
      right: '5%',
      top: '0',
      textStyle: {
        color: '#fff',
      },
    },
    tooltip: {
      trigger: 'axis'
    },
    xAxis: [{
      type: 'category',
      data: state.productData.xAxis,
      axisLine: {
        lineStyle: {
          color: "#fff"
        }
      }
    }],
    yAxis: [{
      type: 'value',
      splitNumber: 4,
      splitLine: {
        lineStyle: {
          type: 'dashed',
          color: '#fff'
        }
      },
      axisLine: {
        show: false,
        lineStyle: {
          color: "#fff"
        },
      },
      nameTextStyle: {
        color: "#999"
      },
      splitArea: {
        show: false
      }
    }],
    series: [{
      name: state.productData.name,
      type: 'line',
      data: state.productData.series,
      lineStyle: {
        normal: {
          width: 4,
          color: '#47A3F2'
        }
      },
      itemStyle: {
        normal: {
          color: '#fff',
          borderWidth: 10,
          /*shadowColor: 'rgba(72,216,191, 0.3)',
          shadowBlur: 100,*/
          borderColor: "#A9F387"
        }
      },
      smooth: true
    }]
  }
  trendDom.setOption(option)
}
const initDistrChart = () => {
  distrDom = echarts.init((document.getElementById('charDistr')) || null)
  const option = {
    series: [{
      type: 'pie',
      selectedMode: 'single',
      radius: ['30%', '60%'],
      color: ['#FF9400', '#00CED1', '#59ADF3', '#FF999A', '#FFCC67'],
      label: {
        normal: {
          show: true,
          position: 'outside',
          textStyle: {
            color: '#fff',
            fontWeight: 'bold',
            fontSize: 14
          },
          formatter: function (params) {
            return '{name|' + params.name + '}' + '{value|:' + params.value + '}' + '\n{fontColor|' + params.percent + '%}';
          },
          rich: {
            name: {
              color: 'inherit',
            },
            value: {
              color: 'inherit',
            },
            fontColor: {
              color: 'inherit',
              fontWeight: 'bold',
              padding: [5, 0, 0, 0],
            },
          },
        }
      },
      labelLine: {
        normal: {
          show: true
        }
      },
      data: state.distrData
    }]

  }
  distrDom.setOption(option)
}
const initRatedChart = () => {
  rateDom = echarts.init((document.getElementById('chartRate')) || null)
  const option = {
    backgroundColor: 'transparent',
    grid: {
      left: '5%',
      right: '5%',
      top: '15%',
      bottom: '5%',
      containLabel: true,
    },
    legend: {
      show: true,
      right: '5%',
      top: '0',
      textStyle: {
        color: '#fff',
      },
    },
    tooltip: {
      trigger: 'axis',
      axisPointer: {
        type: 'shadow'
      }
    },
    xAxis: [{
      type: 'category',
      data: state.rateData.xAxis,
      axisLine: {
        lineStyle: {
          color: 'rgba(255,255,255,0.12)'
        }
      },
      axisLabel: {
        margin: 10,
        color: '#e2e9ff',
        textStyle: {
          fontSize: 14
        },
      },
    }],
    yAxis: [{
      axisLabel: {
        formatter: '{value}',
        color: '#e2e9ff',
      },
      axisLine: {
        show: false
      },
      splitLine: {
        lineStyle: {
          color: 'rgba(255,255,255,0.12)'
        }
      }
    }],
    series: [{
      type: 'bar',
      data: state.rateData.series,
      barWidth: '10px',
      itemStyle: {
        normal: {
          color: new echarts.graphic.LinearGradient(0, 0, 0, 1, [{
            offset: 0,
            color: '#75F7FA' // 0% 处的颜色
          }, {
            offset: 1,
            color: '#09A7A9' // 100% 处的颜色
          }], false),
          barBorderRadius: [20, 20, 20, 20],
          shadowColor: 'rgba(0,160,221,1)',
          shadowBlur: 4,
        }
      },
      label: {
        normal: {
          "show": true,
          "position": "top",
          "distance": 10,
          fontSize: 16,
          "color": "#01fff4"
        }
      }
    }]
  }
  rateDom.setOption(option)
}
// ==============================接口获取数据 end===================
// 获取时间
const formatTime = () => {
  const now = new Date()
  const year = now.getFullYear()
  const month = String(now.getMonth() + 1).padStart(2, '0')
  const day = String(now.getDate()).padStart(2, '0')
  const hours = String(now.getHours()).padStart(2, '0')
  const minutes = String(now.getMinutes()).padStart(2, '0')
  const seconds = String(now.getSeconds()).padStart(2, '0')

  state.newTime = `${year}-${month}-${day} ${hours}:${minutes}:${seconds}`
}
onMounted(() => {
  getNumData()
  getProductTrendData()
  getDistrData()
  getDefectRateData()
  getRealtimeReportData()
  getProductStatisticsData()
  getWorkOrderData()
  //自动进入全屏
  enterScreen();
  formatTime() // 初始化显示
  state.timer = setInterval(formatTime, 1000)

  window.addEventListener(
    'resize',
    () => {
      trendDom && trendDom.resize()
      distrDom && distrDom.resize()
      rateDom && rateDom.resize()

    },
    false,
  ) // false代表事件句柄在冒泡阶段执行

});
onUnmounted(() => {
  trendDom?.dispose()
  distrDom?.dispose()
  rateDom?.dispose()

  window.removeEventListener('resize', trendDom)
  window.removeEventListener('resize', distrDom)
  window.removeEventListener('resize', rateDom)

  trendDom = null
  distrDom = null
  rateDom = null

  if (state.timer) {
    clearInterval(state.timer);
    state.timer = null;
  }
});
// =======================全屏====================
// 监听浏览器全屏状态变化
document.addEventListener('fullscreenchange', () => {
  if (!document.fullscreenElement) {
    setScreenMode('notfull');
  }
});
// 根据屏幕大小计算字体大小========start
const setFontSize = () => {
  const width = window.innerWidth;
  const size = (100 / 1920) * width;
  let fontsize = size / (100 / 14);
  if (fontsize <= 8) {
    fontsize = 8;
  }
  if (fontsize >= 26) {
    fontsize = 26;
  }
  document.documentElement.style.fontSize = fontsize + 'px';
  document.body.style.fontSize = fontsize + 'px';
};
window.addEventListener('resize', setFontSize);
document.addEventListener('DOMContentLoaded', setFontSize);
</script>

<style scoped lang="less">
.bigmap-container {
  width: 100%;
  height: 100%;
  background: url('@/assets/images/bigmap/bg1.png') no-repeat center center / 100% 100%;

  .title-container {
    width: 100%;
    height: 5.1429rem;
    display: flex;
    align-items: center;
    justify-content: space-around;
    padding: 2.1429rem;
    background: url('@/assets/images/bigmap/title-bg.png') no-repeat center center / 100% 100%;
    color: #fff;

    .title {
      flex: 1;
      text-align: center;
      font-size: 20px;
      font-weight: 800;
    }

    .time {
      width: 20%;
    }

    .fullscreen {
      width: 20%;
      text-align: right;
      cursor: pointer;
    }
  }

  .content-container {
    width: 100%;
    height: calc(100% - 5.1429rem);
    padding: 1.4286rem;

    .title-common-container {
      width: 100%;
      height: 2.1429rem;
      line-height: 2.1429rem;
      color: #fff;
      display: flex;
      padding-left: 20px;
    }

    .content-common-container {
      width: 100%;
      height: calc(100% - 2.1429rem);
      color: #fff;
    }

    .chart-coomon-container {
      box-shadow: rgb(131, 191, 246) 0px 0px 25px 3px inset;
    }

    .width-32-container {
      width: 32.5%;
      height: 100%;
    }

    .width-35-container {
      width: calc(35% - 2.8571rem);
      margin: 0 1.4286rem;
      height: 100%;

    }

    .width-49-container {
      width: 49%;
      height: 100%;
    }

    .width-51-container {
      width: calc(51% - 1.4286rem);
      height: 100%;
      margin-left: 1.4286rem;
    }

    .echarts_container {
      width: 100%;
      height: 100%;
    }

    .no-data {
      width: 100%;
      height: 100%;
      display: flex;
      justify-content: center;
      align-items: center;
    }

    .table-header-container {
      width: 100%;
      display: flex;
      height: 40px;
      line-height: 40px;
      background-color: #0E3C82;
      text-align: center;
      box-sizing: border-box;
    }

    .table-w-40 {
      width: 40%;

      .progress-item-wrapper {
        display: flex;
      }
    }

    .table-w-25 {
      width: 25%;
    }

    .table-w-20 {
      width: 20%;
    }

    .table-w-15 {
      width: 15%;
    }

    .table-w-10 {
      width: 10%;
      height: 100%;
    }

    .table-w-5 {
      width: 5%;
    }

    .table-content-container {
      overflow: hidden;
      height: calc(100% - 40px);

      .seamless-scroll {
        height: 100%;
        overflow: hidden;
        will-change: transform;
      }

      .seamless-scroll .table-item-common {
        transform: translateZ(0);
        backface-visibility: hidden;
      }

      .static-list {
        height: 100%;
      }

      .table-item-common {
        width: 100%;
        display: flex;
        height: 40px;
        line-height: 40px;
        // background-color: #233E53;
        text-align: center;
        box-sizing: border-box;
      }

      
      .table-item:nth-child(2n) {
        background-color: #034669;
      }

      .table-item {
        height: 100%;
        display: flex;
        align-items: center;
        padding: 5px 0;
        background-color: #233E53;

      }

      .text-ellipsis {
        width: 100%;
        white-space: nowrap;
        overflow: hidden;
        text-overflow: ellipsis;
        display: inline-block;
        padding: 0 10px;
        cursor: pointer;
        text-align: center;
      }
    }

    .top-wrapper {
      width: 100%;
      height: 33%;
      display: flex;

      .num-container {
        display: grid;
        grid-template-columns: repeat(3, 1fr);
        gap: 10px;
        color: #fff;

        .grid-item {
          display: flex;
          flex-flow: column;
          justify-content: center;
          align-items: center;

          .num {
            font-weight: 800;
            font-size: 28px;
          }

          .unit {
            font-size: 18px;

          }
        }
      }

    }

    .middle-wrapper {
      width: 100%;
      height: calc(34% - 2.8571rem);
      margin: 20px 0;
      display: flex;
    }

    .bottom-wrapper {
      width: 100%;
      height: 33%;

      /* 工艺路线流程容器 */
      .process-flow-container {
        display: flex;
       flex-wrap: wrap;
        align-items: flex-start;
        justify-content: flex-start;
        padding: 8px 0;
        gap: 0;
      }

      /* 单个工序项 */
      .process-flow-item {
        display: flex;
        flex-direction: column;
        align-items: center;
        position: relative;
      }

      /* 进度环包装器 */
      .progress-circle-wrapper {
        position: relative;
        width: 32px;
        height: 32px;
        display: flex;
        align-items: center;
        justify-content: center;
      }

      /* SVG样式 */
      .progress-svg {
        width: 100%;
        height: 100%;
        transform: rotate(0deg);
      }

      /* 背景轨道 */
      .progress-track {
        fill: none;
        stroke: #fff;
        stroke-width: 3;
      }

      .progress-track-0 {
        fill: none;
        stroke: #ccc;
        stroke-width: 3;
      }

      /* 进度条 - 使用直角端点确保进度计算精准 */
      .progress-bar {
        fill: none;
        stroke-width: 3;
        stroke-linecap: butt;
        /* 改为直角端点，确保stroke-dashoffset计算精准 */
        transition: stroke-dashoffset 0.3s ease;
      }

      /* 百分比文字 */
      .circle-text {
        position: absolute;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%);
        display: flex;
        align-items: center;
        justify-content: center;
      }

      .circle-percent {
        font-size: 9px;
        font-weight: 600;
        color: #fff;
        line-height: 1;
        text-shadow: 0 1px 2px rgba(0, 0, 0, 0.3);
      }

      /* 工序名称 */
      .process-name {
        font-size: 10px;
        color: #fff;
        text-align: center;
        white-space: nowrap;
        max-width: 60px;
        height: 30px;
        // overflow: hidden;
        // text-overflow: ellipsis;
        // height: 2.1429rem;
      }

      /* 连接线 */
      .process-connector {
        width: 20px;
        height: 2px;
        background: linear-gradient(to right, rgba(255, 255, 255, 0.3), rgba(255, 255, 255, 0.6), rgba(255, 255, 255, 0.3));
        margin: 15px 4px 0 4px;
        position: relative;
        flex-shrink: 0;

        /* 箭头 */
        &::after {
          content: '';
          position: absolute;
          right: -4px;
          top: 50%;
          transform: translateY(-50%);
          width: 0;
          height: 0;
          border-left: 5px solid rgba(255, 255, 255, 0.6);
          border-top: 3px solid transparent;
          border-bottom: 3px solid transparent;
        }
      }

    }
  }
}

.scroll-item {
  height: 40px;
  /* 必须与singleHeight一致 */
  line-height: 40px;
  padding: 0 10px;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}


/* 响应式调整：小屏幕自动缩小 */
@media (max-width: 768px) {
  .progress-circle-wrapper {
    width: 80px;
    height: 80px;
  }

  .progress-circle-bg,
  .progress-circle-fill {
    width: 60px;
    height: 60px;
  }

  .progress-line {
    width: 20px;
  }
}
</style>
