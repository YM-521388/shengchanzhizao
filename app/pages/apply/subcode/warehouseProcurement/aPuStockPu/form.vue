﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" :rules="rules" ref="dataForm" :errorType="['toast']" label-position="left" :label-width="100 * 1.5" label-align="right" class="jnpf-form">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='采购入库单号' prop="F_StockInNo"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_StockInNo" placeholder='请填写，忽略将自动生成'  :showCount='false' />
				</u-form-item>
			</view>
<!-- 			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='入库仓库' prop="F_WarehouseId"  :label-width="100 * 1.5">
					<JnpfCascader v-model="dataForm.F_WarehouseId" :options="f_WarehouseIdOptions" :props="f_WarehouseIdProps" placeholder='请选择' filterable clearable :disabled="Boolean(dataForm.id)"/>
				</u-form-item>
			</view> -->
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='入库类型' prop="F_StockInType"  :label-width="100 * 1.5">
					<JnpfSelect v-model="dataForm.F_StockInType" :options="f_StockInTypeOptions" :props="f_StockInTypeProps" placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='入库日期' prop="F_StockInDate" required  :label-width="100 * 1.5">
					<JnpfDatePicker v-model="dataForm.F_StockInDate" type='date' format='yyyy-MM-dd' placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='收货人' prop="F_StockInUserId" required  :label-width="100 * 1.5">
					<JnpfUserSelect v-model="dataForm.F_StockInUserId" placeholder='请选择' selectType='all' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='采购单' prop="F_pu_Orderld"  :label-width="100 * 1.5">
					<JnpfSelect v-model="dataForm.F_pu_Orderld" :options="f_pu_OrderldOptions" :props="f_pu_OrderldProps" placeholder='请选择' @change="handleOrderChange"/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='备注' prop="F_Description"  :label-width="100 * 1.5">
					<JnpfTextarea v-model="dataForm.F_Description" placeholder='请输入'  :showCount='false' />
				</u-form-item>
			</view>
			<view class="jnpf-table">
				<view class="jnpf-table-title u-line-1" >{{'采购入库商品'}}</view>
				<view class="u-p-l-20 u-p-r-20 form-item-box">
					<u-form-item label='添加商品'  :label-width="100 * 1.5" @click="addGoods" >
						<!-- <text @click="addGoods">添加</text> -->
						<!-- <JnpfPopupSelect interfaceName='getPopSelect' multiple v-model="tempGoodsSelector" propsValue='F_Id' relationField='F_GoodsName' :columnOptions='tableFieldecf5cb_F_CustomerIdOptions' interfaceId='2008721559174385664' placeholder='选择商品' hasPage :pageSize='20' popupType='dialog' popupTitle='选择数据' popupWidth='70%' :templateJson='tableFieldecf5cb_F_CustomerIdTemplateJson' :vModel="'tableFieldecf5cb-F_CustomerId'" type="popup"  @change="changeGoods" :extraOptions="[]"/> -->
					</u-form-item>
				</view>
				<!-- <view class="jnpf-table-title u-line-1" >{{'采购入库商品'}}</view>
				<view class="u-p-l-20 u-p-r-20 form-item-box">
					<u-form-item label='批量商品选择'  :label-width="100 * 1.5">
						<JnpfPopupSelect multiple interfaceName='getPopSelect'   v-model="tempGoodsSelector" propsValue='F_Id' relationField='F_GoodsName' :columnOptions='tableField4bd139_F_CustomerIdOptions' interfaceId='2008721559174385664' placeholder='请选择' hasPage :pageSize='20' popupType='dialog' popupTitle='选择数据' popupWidth='70%' :templateJson='tableField4bd139_F_CustomerIdTemplateJson' :vModel="'tableField4bd139-F_CustomerId'" type="popup" @change='handleGoods' />
					</u-form-item>
				</view>
				<view class="u-p-l-20 u-p-r-20 form-item-box">
					<u-form-item label='批量入库仓库'  :label-width="100 * 1.5">
						<JnpfCascader v-model="batchWarehouseValue" :options="f_WarehouseIdOptions" :props="f_WarehouseIdProps" placeholder='请选择仓库' filterable clearable />
					</u-form-item>
					<view class="batch-container">
						
						<view class="btn">
							<u-button type="primary" @click="handleBatchAssignWarehouse" :disabled="!selectedtableField4bd139RowKeys.length">
							批量赋值
							</u-button>
						</view>
						<text class="text-gray-400 text-xs" v-if="selectedtableField4bd139RowKeys.length">
							(已选 {{ selectedtableField4bd139RowKeys.length }} 条)
						</text>							
					</view>	
				</view>	 -->

						
				<view class="jnpf-table-item" v-for="(item,i) in dataForm.tableField4bd139" :key="i">
					<view class="jnpf-table-item-title">
						<view class="jnpf-table-item-title-num">({{i+1}})</view>
						<view class="u-flex">
							<view class="jnpf-table-delete-btn" @click="removeAPuStockPuItemRow(i,true)">{{this.$t('common.delText','删除')}}</view>
						</view>
                    </view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='商品'  :label-width="100 * 1.5">
							<JnpfPopupSelect disabled="" v-model="dataForm.tableField4bd139[i].F_CustomerId" propsValue='F_Id' relationField='F_GoodsName' :columnOptions='tableField4bd139_F_CustomerIdOptions' interfaceId='2008721559174385664' placeholder='请选择' hasPage :pageSize='20' popupType='dialog' popupTitle='选择数据' popupWidth='70%' :templateJson='tableField4bd139_F_CustomerIdTemplateJson' :vModel="'tableField4bd139-F_CustomerId'+i" type="popup"/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='数量'  :label-width="100 * 1.5">
							<!-- <JnpfInputNumber v-model="dataForm.tableField4bd139[i].F_Num" :precision='0' :step='1.0'  /> -->	
							 <!-- @input="(val) => handleNumLevel1Change(val, i)" -->
		                    <view class="form-value" v-if="getUnitRatioLevels(dataForm.tableField4bd139[i])">
		                      <JnpfInputNumber v-model="dataForm.tableField4bd139[i].F_NumLevel1"
		                        :placeholder="dataForm.tableField4bd139[i]._quantityDisabled ? '自动带出' : '请输入'" :controls="false" disabled
		                        :style="{ width: '80px' }" @change="handleNumLevel1Change(dataForm.tableField4bd139[i],i)" />
		                      <text>{{ getUnitRatioLevels(dataForm.tableField4bd139[i])?.level1?.name }}</text>
		                      <JnpfInputNumber v-model="dataForm.tableField4bd139[i].F_NumLevel2" placeholder="请输入" :controls="false"
		                        :disabled="Boolean(dataForm.id)" :min="1" :max="dataForm.tableField4bd139[i]._F_NumLevel2Max" :style="{ width: '80px' }"
		                        @change="handleNumLevel2Change(dataForm.tableField4bd139[i],i)" />
		                      <text>{{ getUnitRatioLevels(dataForm.tableField4bd139[i])?.level2?.name }}</text>
		                    </view>
		                    <view class="form-value" v-else>
		                      <JnpfInputNumber v-model="dataForm.tableField4bd139[i].F_NumLevel2" placeholder="请输入" :controls="false" disabled
		                        :min="1" :max="dataForm.tableField4bd139[i]._F_NumLevel2Max" :style="{ width: '80px' }"
		                        @change="handleNumLevel2Change(dataForm.tableField4bd139[i],i)" />
		                      <text>{{ getUnitRatioLevels(dataForm.tableField4bd139[i])?.level1?.name }}</text>
		                    </view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='成本单价(元)'  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableField4bd139[i].F_Price" :step='1.0'  />
						</u-form-item>
					</view>
<!-- 					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='成本金额(元)'  :label-width="100 * 1.5">
							 <view class="form-value">{{ jnpf.thousandsFormat(calculateRowCostAmount(dataForm.tableField4bd139[i])) }}</view>
						</u-form-item>
					</view> -->
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='库存编码'  :label-width="100 * 1.5">
							 <JnpfInput disabled v-model="dataForm.tableField4bd139[i].F_Encoding" placeholder='请输入'  :showCount='false' />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item @click="handleF_Warehouse(i)" label='入库仓库' prop="F_WarehouseID" required :label-width="100 * 1.5">
							<!-- <JnpfCascader :disabled="Boolean(dataForm.id)"  v-model="dataForm.tableField4bd139[i].F_WarehouseID" :options="f_WarehouseIdOptions" :props="f_WarehouseIdProps" placeholder='请选择' filterable clearable /> -->
							<JnpfCascader v-if='dataForm.tableField4bd139[i].F_WarehouseID' disabled v-model="dataForm.tableField4bd139[i].F_WarehouseID" :options="f_WarehouseIdOptions" :props="f_WarehouseIdProps" placeholder='请选择' filterable clearable />							
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='备注'  :label-width="100 * 1.5">
							<JnpfTextarea v-model="dataForm.tableField4bd139[i].F_Description" placeholder='请输入'  :showCount='false' :disabled="Boolean(!!dataForm.id)"/>
						</u-form-item>
					</view>
                </view>
<!-- 				<view class="jnpf-table-footer-btn" >
					<view class="jnpf-table-btn jnpf-table-primary-btn" @click="addAPuStockPuItemRow()">
						<text class="jnpf-table-btn-icon icon-ym icon-ym-btn-add"/>
						<text class="jnpf-table-btn-text">{{this.$t('common.add1Text','添加')}}</text>
					</view>
				</view> -->
            </view>
		</u-form>
		<view class="buttom-actions" v-if="jurisdictionType != 'btn_detail'">
			<CustomButton :btnText="$t('common.cancelText')" btnIcon="icon-ym icon-ym-add-cancel" customIcon />
			<u-button class="buttom-btn" type="primary" @click="submitForm" :loading="btnLoading">{{$t('common.okText','确定')}}</u-button>
		</view>
	</view>
</template>
<script>
	import { getDictionaryDataSelector,getDataInterfaceRes,getDataInterGoodsInventoryNo } from '@/api/common'
	import { useBaseStore } from '@/store/modules/base'
    import request from '@/utils/request'
	import CustomButton from '@/components/CustomButton'
	import { getOrderItems } from '@/api/subcode/subcode'
	import { buildUUID } from '@/utils/uuid';

    export default {
		components:{ CustomButton },
        data() {
            return {
				keyCode : +new Date(),
                btnLoading: false,
				addTableConf:{
				},
                dataForm: {
                    id:'',
					F_StockInNo:undefined,
					F_WarehouseId:[],
					F_StockInType:undefined,
					F_StockInDate:undefined,
					F_StockInUserId:undefined,
					F_Description:undefined,
					tableField4bd139:[],
					tableFieldca527d:[],
                },
                rules: {
					F_StockInDate:[
						{
							required:true,
							message:this.$t('common.inputText')+' '+'入库日期',
							trigger:'change',
							type:'number'
						},
					],
					F_StockInUserId:[
						{
							required:true,
							message:this.$t('common.inputText')+' '+'收货人',
							trigger:'change',
						},
					],
                },
				ruleList: {
				},
				regList: {
				},
				index: 0,
				idList: [],
				dataValue: {},
				userInfo:{},
				jurisdictionType:'',
				addType: 0,
				f_WarehouseIdProps:{'label':'F_Name','value':'F_Id',"children": "children"},
				f_WarehouseIdOptions: [],
				f_StockInTypeProps:{'label':'fullName','value':'enCode'},
				f_StockInTypeOptions: [],
				tableField4bd139_F_CustomerIdTemplateJson: [],
				tableField4bd139_F_CustomerIdOptions: [
  {
    "value": "F_GoodsNo",
    "label": "商品编号"
  },
  {
	"value": "F_GoodsName",
    "label": "商品名称"
  },
  {
    "value": "F_Unit",
    "label": "单位"
  },
  {
    "value": "F_Specification",
    "label": "规格"
  },
  {
    "value": "F_SalePrice",
    "label": "销售单价"
  },
  {
    "value": "F_CostPrice",
    "label": "成本单价"
  },
  {
    "value": "F_Source",
    "label": "商品来源"
  },
  {
    "value": "F_Remark",
    "label": "备注"
  }
],
				tempGoodsSelector:[],
				f_pu_OrderldProps: { label: 'F_OrderNo', value: 'F_Id' },
				f_pu_OrderldOptions:[],
				batchWarehouseValue: undefined,
				selectedtableField4bd139RowKeys: [],
            };
        },
		watch: {
			'dataForm.tableField4bd139': {
				handler: function(newVal, oldValue) {
			      if (!Array.isArray(newVal)) return;
			      for (let i = 0; i < newVal.length; i++) {
			        const row = newVal[i] || {};
			        const levels = this.getUnitRatioLevels(row);
			        row.F_Num = row.F_Num;
			        row.F_NumLevel1 = 1;
			      }
				},
				immediate: true,
				deep: true,
			}
        },
        onLoad(option) {
			this.userInfo = uni.getStorageSync('userInfo') || {}
			this.jurisdictionType = option.jurisdictionType
			let _title = ''
			if(option.jurisdictionType =='btn_add'){
				_title = this.$t('common.add2Text','新增')
            }
			if(option.jurisdictionType =='btn_edit'){
				_title = this.$t('common.editText','编辑')
            }
			if(option.jurisdictionType =='btn_detail'){
				_title = this.$t('common.detailText','详情')
            }
            this.dataForm.id = option.id || ''
			this.idList = option.idList ? option.idList.split(",") : []
            uni.setNavigationBarTitle({
                title: _title
            })
			this.dataValue = JSON.parse(JSON.stringify(this.dataForm))
            this.initData()
			this.getf_WarehouseIdOptions();
			this.getf_StockInTypeOptions();
			this.getF_pu_OrderldOptions();
            uni.$on('linkPageConfirm', async (subVal, Vmodel) => {
				if ('APuStockPuItem' === Vmodel) {
					for (const t of subVal) {
						let item = {
							F_CustomerId: t.F_Id || t.id,
							F_Encoding: t.F_InventoryNo || t.F_Encoding,
							F_Num: undefined,
							F_Price: undefined,
							F_Description: undefined,
							F_CreatorUserId: undefined,
							F_CreatorTime: undefined,
						};
						const newItem = { ...item, ...t };
						
						// 查询商品信息获取 F_Unit_Ratio
						if (newItem.F_Encoding) {
							const query = {
								paramList: [{
									field: "goodsNo",
									defaultValue: newItem.F_Encoding,
									id: "b6217b",
									dataType: "varchar",
									required: "0",
									fieldName: "库存编码",
									source: null
								}]
							};
							try {
								const res = await getDataInterfaceRes('2036268230699520000', query);
								newItem.F_Unit_Ratio = res.data.F_Unit_Ratio;
							} catch (e) {
								console.error('查询商品单位比例失败:', e);
							}
						}
						
						this.dataForm.tableField4bd139.push(newItem);
						// 初始化数量级别
						const index = this.dataForm.tableField4bd139.length - 1;
						this.fillNumLevelsFromF_Num(newItem, index);
					}
				}
				this.initCollapse();
            });
            uni.$on('linkPageConfirm', (subVal, Vmodel) => {
				let item = {
						F_Type:undefined,
						F_Title:undefined,
						F_Content:undefined,
						F_Reason:undefined,
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
					}
               if ('APuStockPuLog' === Vmodel) subVal.forEach(t => this.dataForm.tableFieldca527d.push({...item,...t}));
			   this.initCollapse();
            });
        },
        onReady() {
            this.$refs.dataForm.setRules(this.rules);
        },
        methods: {
			initCollapse() {
				setTimeout(() => {
					this.keyCode = +new Date()
				}, 50)
			},
			onCollapseChange() {
				this.initCollapse()
			},
			getf_WarehouseIdOptions(){
				this.f_WarehouseIdOptions = []
				let templateJson = []
				let query = {
					paramList: this.getParamList(templateJson, this.dataForm)
				}
				getDataInterfaceRes('2021045201115680768', query).then(res => {
					let data = res.data
					this.f_WarehouseIdOptions = Array.isArray(data) ? data : []
				});
			},
			getf_StockInTypeOptions(){
				getDictionaryDataSelector('2012074650393251840').then(res => {
					this.f_StockInTypeOptions = res.data.list
				});
			},
			   getF_pu_OrderldOptions() {
			    let templateJson = [];
			    let query = {
			      paramList: this.getParamList(templateJson, this.dataForm),
			    };
			    getDataInterfaceRes('2011984543933927424', query).then(res => {
			      let data = res.data;
			      this.f_pu_OrderldOptions = Array.isArray(data) ? data : [];
			    });
			  },
    conversionDateTime(type) {
      const format = type === 'yyyy' ? 'yyyy-01-01 00:00:00' : type === 'yyyy-MM' ? 'yyyy-MM-01 00:00:00' :
        type === 'yyyy-MM-dd' ? 'yyyy-MM-dd 00:00:00' : type === 'yyyy-MM-dd HH:mm' ? 'yyyy-MM-dd HH:mm:00' : 'yyyy-MM-dd HH:mm:ss'
      const dataTime = this.jnpf.toDate(new Date(), format)
      return new Date(dataTime).getTime()
    },
	initDefaultData() {
      this.dataForm.F_StockInDate = this.conversionDateTime("yyyy-MM-dd");
      this.dataForm.F_StockInUserId = this.userInfo.userId
	},
			selfInit(){
				this.addAPuStockPuItem();
				this.addAPuStockPuLog();
			},
			resetForm() {
				this.dataForm.tableField4bd139 = [];
				this.dataForm.tableFieldca527d = [];
				this.$refs.dataForm.resetFields();
				this.initCollapse();
			},
			checkChildRule() {
				let title = [];
				let _ruleList = this.ruleList
				for (let k in _ruleList) {
					let childData = this.dataForm[k]
					childData.forEach((item, index) => {
						for (let model in _ruleList[k]) {
							if (item[model] instanceof Array) {
								if (item[model].length == 0) {
									title.push(_ruleList[k][model])
								}
							} else if (!item[model]) {
								title.push(_ruleList[k][model])
							}
						}
					})
				}
				let _regList = this.regList
				for (let k in _regList) {
					let childData = this.dataForm[k]
					for (let n in _regList[k]) {
						for (let i = 0; i < _regList[k][n].length; i++) {
							const element = _regList[k][n][i]
							childData.forEach((item, index) => {
								if (item[n]&&  !eval(element.pattern).test(item[n])) {
									title.push(element.message)
								}
							})
						}
					}
				}
				if (title.length > 0) {
					return title[0]
				}
			},
            initData() {
				const baseStore = useBaseStore()
				baseStore.updateRelationData({})
                if (this.dataForm.id) {
					request({
					  url: `/api/example/APuStockPu/${this.dataForm.id}`,
					  method: 'get',
					}).then(async res => {
					  this.dataForm = res.data;
					  
					  // 确保 F_WarehouseId 是数组
					  if (!this.dataForm.F_WarehouseId) {
					    this.dataForm.F_WarehouseId = [];
					  }
					  
					  // 初始化临时数组
					  this.tempGoodsSelector = [];
					  
					  // 提取所有需要查询的商品
					  const goodsList = this.dataForm.tableField4bd139 || [];
					  
					  // 并行处理所有商品查询
					  const queryPromises = goodsList.map(async (item, index) => {
					    // 记录商品ID
					    this.tempGoodsSelector.push(item.F_CustomerId);
					    
					    // 构建查询参数
					    const query = {
					      paramList: [
					        {
					          field: "goodsNo",
					          defaultValue: item.F_Encoding,
					          id: "b6217b",
					          dataType: "varchar",
					          required: "0",
					          fieldName: "库存编码",
					          source: null
					        }
					      ]
					    };
					    
					    try {
					      const res = await getDataInterfaceRes('2036268230699520000', query);
					      item.F_Unit_Ratio = res.data.F_Unit_Ratio;
					      this.fillNumLevelsFromF_Num(item, index);
					    } catch (error) {
					      console.error(`查询商品 ${item.F_Encoding} 失败:`, error);
					      // 可以选择设置默认值或继续执行
					      item.F_Unit_Ratio = null;
					    }
					    
					    return item;
					  });
					  
                      // 等待所有查询完成
					  await Promise.all(queryPromises);
					  
					  // 强制触发视图更新，确保 F_NumLevel2 等字段显示正确
					  this.dataForm.tableField4bd139 = [...this.dataForm.tableField4bd139];
					}).catch(error => {
					  console.error('请求失败:', error);
					  // 处理错误，比如提示用户
					});
				}else{
					this.initDefaultData()
				}
            },
			validateSortCode(list) {
				for (let item of list) {
					if (!item.F_WarehouseID) {
					uni.showToast({
						title: `入库仓库不能为空`,
						icon: 'none',
						duration: 3000
					});
					return false; // 验证失败，返回false
					}
				}
				return true;
			},
			submitForm(type) {
                this.$refs.dataForm.validate(valid => {
                    if (!valid) return
					if (!this.validateSortCode(this.dataForm.tableField4bd139))  return;
                    this.btnLoading = true
                    if (this.dataForm.id) {
                        request({
                            url: '/api/example/APuStockPu/' + this.dataForm.id,
                            method: 'put',
                            data: this.dataForm,
                        }).then(res => {
                            uni.showToast({
                                title: res.msg,
                                complete: () => {
                                    setTimeout(() => {
										if (type != 1) {
											uni.$emit('refresh')
											uni.navigateBack()
										}
                                        this.btnLoading = false
                                    }, 1500)
                                }
                            })
                        }).catch(()=>{
							this.btnLoading = false
						})
                    } else {
                        request({
                            url: '/api/example/APuStockPu',
                            method: 'post',
                            data: this.dataForm,
                        }).then(res => {
                            uni.showToast({
                                title: res.msg,
                                complete: () => {
                                    setTimeout(() => {
										if (type == 1) {
											this.dataForm = JSON.parse(JSON.stringify(this.dataValue))
											this.initDefaultData()
										} else {
											uni.$emit('refresh')
											uni.navigateBack()
										}
                                        this.btnLoading = false
                                    }, 1500)
                                }
                            })
                        }).catch(()=>{
							this.btnLoading = false
						})
                    }
                });
            },
	/**
       * 获取参数列表
       */
    getParamList(templateJson, formData, index) {
      for (let i = 0; i < templateJson.length; i++) {
        if (templateJson[i].relationField && templateJson[i].sourceType == 1) {
          //区分是否子表
          if (templateJson[i].relationField.includes('-')) {
            let tableVModel = templateJson[i].relationField.split('-')[0]
            let childVModel = templateJson[i].relationField.split('-')[1]
            templateJson[i].defaultValue = formData[tableVModel] && formData[tableVModel][index] && formData[tableVModel][index][childVModel] || ''
          } else {
            templateJson[i].defaultValue = formData[templateJson[i].relationField] || ''
          }
        }
      }
      return templateJson
    },
			clickIcon(label, tipLabel) {
				uni.showModal({
					title: label || '',
					content: tipLabel || '',
					showCancel: false,
				});
			},
	exist() {
		let title = []
		let _regList = this.regList
		for (let k in _regList) {
			let childData = this.dataForm[k]
			for (let n in _regList[k]) {
				for (let i = 0; i < _regList[k][n].length; i++) {
					const element = _regList[k][n][i]
					if (element.pattern) {
						element.pattern = element.pattern.toString()
                        let start = element.pattern.indexOf('/')
                        let stop = element.pattern.lastIndexOf('/')
                        let str = element.pattern.substring(start + 1, stop)
                        let reg = new RegExp(str)
                        element.pattern = reg
					}
					childData.forEach((item, index) => {
						if (item[n] && !element.pattern.test(item[n])) {
							title.push(element.message)
						}
					})
				}
			}
		}
		if (title.length > 0) {
			return title[0]
		}
	},
	openSelectDialog(key,value) {
		let data = {
			actionConfig: this.addTableConf[key+'List'+value],
			formData: this.dataForm,
			tableVmodel: key
		}
		uni.navigateTo({
			url: '/pages/apply/tableLinkage/index?data=' + encodeURIComponent(JSON.stringify(data))
		})
	},
			addAPuStockPuItemRow() {
				let item = {
						F_CustomerId:undefined,
						F_Num:undefined,
						F_Price:undefined,
						F_Description:undefined,
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
					}
				this.dataForm.tableField4bd139.push(item)
				this.initCollapse()
			},
			removeAPuStockPuItemRow(i,showConfirm=false) {
				const handleRemove = () => {
					this.dataForm.tableField4bd139.splice(i, 1)
					this.tempGoodsSelector.splice(i, 1)
					this.initCollapse()
				};
				if (!showConfirm) return handleRemove();
				uni.showModal({
					title: this.$t('common.tipTitle'),
					content: this.$t('common.delTip'),
					success: (res) => {
						if (res.confirm) handleRemove()
					}
				})
			},
			copyAPuStockPuItemRow(i) {
				let item = JSON.parse(JSON.stringify(this.dataForm.tableField4bd139[i]));
				item.id = '';
				item.length && item.map(o => delete o.rowData);
				item.F_CreatorUserId = undefined;
				item.F_CreatorTime = undefined;
				this.dataForm.tableField4bd139.push(item);
			},
			addAPuStockPuLogRow() {
				let item = {
						F_Type:undefined,
						F_Title:undefined,
						F_Content:undefined,
						F_Reason:undefined,
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
					}
				this.dataForm.tableFieldca527d.push(item)
				this.initCollapse()
			},
			removeAPuStockPuLogRow(i,showConfirm=false) {
				const handleRemove = () => {
					this.dataForm.tableFieldca527d.splice(i, 1)
					this.initCollapse()
				};
				if (!showConfirm) return handleRemove();
				uni.showModal({
					title: this.$t('common.tipTitle'),
					content: this.$t('common.delTip'),
					success: (res) => {
						if (res.confirm) handleRemove()
					}
				})
			},
			copyAPuStockPuLogRow(i) {
				let item = JSON.parse(JSON.stringify(this.dataForm.tableFieldca527d[i]));
				item.id = '';
				item.length && item.map(o => delete o.rowData);
				item.F_CreatorUserId = undefined;
				item.F_CreatorTime = undefined;
				this.dataForm.tableFieldca527d.push(item);
			},
async handleGoods(ids, option) {
			/* 兜底 */
			if (!Array.isArray(this.dataForm.tableField4bd139)) {
				this.dataForm.tableField4bd139 = [];
			}
			const optionIdSet = new Set(option.map(o => o.id));

			/*  删掉已取消勾选的行 */
			this.dataForm.tableField4bd139 = this.dataForm.tableField4bd139.filter(item => optionIdSet.has(item.F_WorkOrderId));
			
			for (const o of option) {
				const exist = this.dataForm.tableField4bd139.some(item => item.F_WorkOrderId === o.F_Id);
				if (!exist) {
					let item = {
						F_CustomerId: o.F_Id,
						F_Encoding: o.F_InventoryNo,
						F_Num: undefined,
						F_Price: undefined,
						F_Description: undefined,
						F_CreatorUserId: undefined,
						F_CreatorTime: undefined,
					};
					
					// 查询商品信息获取 F_Unit_Ratio
					if (item.F_Encoding) {
						const query = {
							paramList: [{
								field: "goodsNo",
								defaultValue: item.F_Encoding,
								id: "b6217b",
								dataType: "varchar",
								required: "0",
								fieldName: "库存编码",
								source: null
							}]
						};
						try {
							const res = await getDataInterfaceRes('2036268230699520000', query);
							item.F_Unit_Ratio = res.data.F_Unit_Ratio;
						} catch (e) {
							console.error('查询商品单位比例失败:', e);
						}
					}
					
					this.dataForm.tableField4bd139.push(item);
					// 初始化数量级别
					const index = this.dataForm.tableField4bd139.length - 1;
					this.fillNumLevelsFromF_Num(item, index);
				}
			}
		},
			  // 采购单变化时获取商品列表：每行对应采购单子表一行；带单位换算时展示「一级+二级」且数量由采购单带出、不可改（与计划单展示一致，一级框在带入时禁用）
			  async  handleOrderChange(orderId) {
				  return
			    if (!orderId) {
			      // 清空商品列表
			      this.dataForm.tableField4bd139 = [];
			      return;
			    }
			
			    try {
			      // 获取采购单商品列表
			      const res = await getOrderItems(orderId);
			      const items = res.data || [];
			
			      if (!items.length) {
			        this.dataForm.tableField4bd139 = [];
			        return;
			      }
			
			      const tableItems = [];
			      for (const item of items) {
			        // 根据商品ID获取商品详细信息
			        let goodsInfo = null;
			        const goodsId = item.F_CustomerId || item.GoodsId;
			        if (goodsId) {
			          const goodsQuery = {
			            paramList: [{ defaultValue: goodsId, field: 'F_Id', dataType: 'varchar' }],
			          };
			          const goodsRes = await getDataInterfaceRes('2008721559174385664', goodsQuery);
			          goodsInfo = Array.isArray(goodsRes.data) ? goodsRes.data[0] : goodsRes.data;
			        }
			
			        const orderQty = Math.max(1, parseInt(String(item.F_Num), 10) || 0);
			        const price = item.F_Price ?? 0;
			        // 生成基础行数据（含单位换算）
			        const baseRow = {
			          F_CustomerId: item.F_CustomerId || item.GoodsId || undefined,
			          OrderNo: item.F_OrderNo || item.OrderNo || '',
			          SupplierName: item.SupplierName || '',
			          GoodsName: item.GoodsName ?? goodsInfo?.F_GoodsName ?? goodsInfo?.GoodsName ?? '',
			          GoodsNo: item.GoodsNo ?? goodsInfo?.GoodsNo ?? goodsInfo?.F_GoodsNo ?? '',
			          Specification: item.Specification ?? goodsInfo?.Specification ?? goodsInfo?.F_Specification ?? '',
			          F_Unit_Ratio: goodsInfo?.F_Unit_Ratio ?? item.F_Unit_Ratio,
			          F_Price: price,
			          F_Description: item.F_Description,
			          F_Encoding: undefined,
			          F_WarehouseID: [],
			          F_CreatorUserId: undefined,
			          F_CreatorTime: undefined,
			          _quantityDisabled: true,
			        };
			
                    // 按采购单数量拆成多条，每条最大单位数量为 1
			        for (let i = 0; i < orderQty; i++) {
			          const row = {
			            ...baseRow,
			            jnpfId: buildUUID(),
			          };
			          const levels = this.getUnitRatioLevels(row);
			          if (levels) {
			            // 每条最大单位数量固定为 1，二级数量自动换算
			            const ratio = Number(levels.level2?.qty) || 0;
			            this.$set(row, 'F_NumLevel1', 1);
			            this.$set(row, 'F_NumLevel2', 1 * ratio);
			            this.$set(row, 'F_Num', 1);
			          } else {
			            this.$set(row, 'F_Num', 1);
			          }
			          tableItems.push(row);
			        }
			      }
			      this.dataForm.tableField4bd139 = tableItems;
			    } catch (e) {
			      console.error('获取采购单商品失败:', e);
			    }
			  },
				// =================数量================
				   getUnitRatioLevels(record) {
				    const raw = record?.F_Unit_Ratio;
				    if (!raw) return null;
				    try {
				      const str = typeof raw === 'string' ? raw : String(raw);
				      const arr = JSON.parse(str);
				      if (!Array.isArray(arr) || arr.length < 2) return null;
				      return { level1: arr[0], level2: arr[1] };
				    } catch (e) {
				      return null;
				    }
				  },
				
               handleNumLevel1Change(record,i) {
				    const levels = this.getUnitRatioLevels(record);
				    if (!levels) return;
				    const qty1 = Number(record.F_NumLevel1) || 0;
				    const ratio = Number(levels.level2?.qty) || 0;
				    const qty2 = qty1 * ratio;
				    this.$set(record, 'F_NumLevel2', qty2);
				    this.$set(record, '_F_NumLevel2Max', qty2);
				    this.$set(record, 'F_Num', qty2);
				  },

				  /** 二级数量（最小单位）编辑：不超过该行默认/换算上限，并与 F_Num 同步 */
				   handleNumLevel2Change(record,i) {
				    const levels = this.getUnitRatioLevels(record);
				    if (!levels) return;
				    let v = Number(record.F_NumLevel2);
				    if (Number.isNaN(v)) v = 0;
				    const max = Number(record._F_NumLevel2Max);
				    if (record._F_NumLevel2Max != null && !Number.isNaN(max) && v > max) {
				      v = max;
				      this.$set(record, 'F_NumLevel2', v);
				    }
				    this.$set(record, 'F_Num', v);
				  },
				
              /** 回显/初始化：根据 F_Num 或比例计算 F_NumLevel1 和 F_NumLevel2 */
				   fillNumLevelsFromF_Num(row, index) {
				    const levels = this.getUnitRatioLevels(row);
				    if (!levels) return;
				    
				    const ratio = Number(levels.level2?.qty) || 0;
				    
				    // 如果 F_Num 有值（回显情况），使用 F_Num 的值；否则（新增情况）根据比例计算默认值
				    if (row.F_Num !== undefined && row.F_Num !== null && row.F_Num !== '') {
				      // 回显：F_Num 存的是二级数量
				      const qty2 = Number(row.F_Num) || 0;
				      this.$set(row, 'F_NumLevel2', qty2);
				      this.$set(row, '_F_NumLevel2Max', qty2);
				      if (ratio > 0) {
				        this.$set(row, 'F_NumLevel1', qty2 / ratio);
				      } else {
				        this.$set(row, 'F_NumLevel1', qty2);
				      }
				    } else {
				      // 新增：F_NumLevel1 固定为1，F_NumLevel2 根据比例计算
				      this.$set(row, 'F_NumLevel1', 1);
				      this.$set(row, 'F_NumLevel2', 1 * ratio);
				      this.$set(row, '_F_NumLevel2Max', 1 * ratio);
				      this.$set(row, 'F_Num', 1 * ratio);
				    }
				  },
				// =================数量================end
				  // 批量赋值仓库给选中的子表行
				   handleBatchAssignWarehouse() {
				    if (!this.batchWarehouseValue) {
				      createMessage.warning('请先选择入库仓库');
				      return;
				    }
				    if (!this.selectedtableField4bd139RowKeys.length) {
				      createMessage.warning('请先勾选要赋值的子表数据');
				      return;
				    }
				    const selectedKeys = this.selectedtableField4bd139RowKeys;
				    let assignCount = 0;
				    for (const row of this.dataForm.tableField4bd139) {
				      if (selectedKeys.includes(row.jnpfId)) {
				        row.F_WarehouseID = [...this.batchWarehouseValue];
				        assignCount++;
				      }
				    }
				    createMessage.success(`已成功赋值 ${assignCount} 条数据`);
				  },
			handleF_Warehouse(i){
				if(this.dataForm.id) return
				uni.scanCode({
				  scanType: ['barCode'],
				  success: (res) => {
				    console.log('扫描结果，', res.result)
					let query = {
					  paramList: [
					    {
					      "field": "id",
					      "defaultValue": '2014233789731049472',
					      "id": "cf7fca",
					      "dataType": "varchar",
					      "required": "0",
					      "fieldName": "",
					      "source": null
					    }
					  ]
									
					}
					getDataInterfaceRes('2038551415311437824', query).then(res => {
					  let data = res.data
					  this.dataForm.tableField4bd139[i].F_WarehouseID = data.F_ParentId
					  console.log(data, '二维码结果');	
					});
				  }
				});
			},
			addGoods(){
			  	uni.scanCode({
					scanType: ['barCode'],
			  		success:  (res)=> {
			  			console.log('扫描结果，',res.result)
			  			let query = {paramList: [{"field": "goodsNo","defaultValue": res.result,"id": "b6217b","dataType": "varchar","required": "0","fieldName": "库存编码","source": null}]
			  			}
			  			getDataInterGoodsInventoryNo('2036268230699520000', query).then(res => {
			  				let data = res.data
			  				let temp={
			  					...data,
			  					jnpfId:buildUUID(),
			  					F_CustomerId:data.id,
			  					F_Encoding:data.F_InventoryNo,
			  				}
			  				const levels = this.getUnitRatioLevels(temp);
			  				 if (levels) {
			  				            if (levels.level2?.qty) {
			  				              // 每条最大单位数量固定为 1，二级数量自动换算
			  				              temp.F_NumLevel1 = 1;
			  				              const ratio = Number(levels.level2?.qty) || 0;
			  				              temp.F_NumLevel2 = 1 * ratio;
			  				              temp._F_NumLevel2Max = temp.F_NumLevel2;
			  				              temp.F_Num = temp.F_NumLevel2;
			  				            } else {
			  				              // 每条最大单位数量固定为 1，二级数量自动换算
			  				              temp.F_NumLevel1 = 1;
			  				              temp.F_NumLevel2 = 1;
			  				              temp._F_NumLevel2Max = temp.F_NumLevel2;
			  				              temp.F_Num = temp.F_NumLevel2;
			  				            }
			  				          } else {
			  				            temp.F_Num = 1;
			  				            temp.F_NumLevel2 = 1;
			  				          }
			  				console.log('二维码数据',data);
			  				this.dataForm.tableField4bd139.push(temp);
			  			});
			  		}
			  	});
			  },
			// ======================计算===================start
			  // 解析 F_Unit_Ratio，只取前两级，返回 { level1: { name, qty }, level2: { name, qty } } 或 null
			   getUnitRatioLevels(record) {
			       const raw = record?.F_Unit_Ratio;
			       if (!raw) return null;
			       try {
			         const str = typeof raw === 'string' ? raw : String(raw);
			         const arr = JSON.parse(str);
			         if (!Array.isArray(arr) || arr.length < 2) return null;
			         return { level1: arr[0], level2: arr[1] };
			       } catch (e) {
			         return null;
			       }
			     },
			// ======================计算===================end
				
        }
    };
</script>
<style lang="scss" scoped>
        page{
                background-color: #f0f2f6;
        }
		.form-value{
			width: 100%;
			display: flex;
			justify-content: flex-end;
		}
		.batch-container{
			width: 100%;
			display: flex;
			justify-content: flex-end;
			.btn{
				width: 100px;
				height: 80rpx;
				line-height: 80rpx;
			}
		}
</style>



