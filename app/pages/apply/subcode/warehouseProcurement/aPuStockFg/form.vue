<!-- 工单类型--商品 -->
﻿﻿﻿﻿<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" :rules="rules" ref="dataForm" :errorType="['toast']" label-position="left" :label-width="100 * 1.5" label-align="right" class="jnpf-form">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='成品入库单号' prop="F_StockInNo"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_StockInNo" placeholder='请填写，忽略将自动生成'  :showCount='false' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='入库日期' prop="F_StockInDate" required  :label-width="100 * 1.5">
					<JnpfDatePicker v-model="dataForm.F_StockInDate" type='date' format='yyyy-MM-dd' placeholder='请选择' />
				</u-form-item>
			</view>
			<!-- <view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='入库仓库' prop="F_WarehouseId" required  :label-width="100 * 1.5">
					<JnpfCascader v-model="dataForm.F_WarehouseId" :options="f_WarehouseIdOptions" :props="f_WarehouseIdProps" placeholder='请选择' filterable clearable  :disabled="Boolean(dataForm.id)"/>
				</u-form-item>
			</view> -->
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='工单类型' prop="F_Type" required  :label-width="100 * 1.5">
					<JnpfSelect v-model="dataForm.F_Type" :options="f_ProcessIdOptions" :props="f_ProcessIdProps" placeholder='请选择'  :disabled="Boolean(dataForm.id || WorkOrderId != '')"  @change="changeWorkOrder" />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='入库类型' prop="F_StockInType"  :label-width="100 * 1.5">
					<JnpfSelect v-model="dataForm.F_StockInType" :options="f_StockInTypeOptions" :props="f_StockInTypeProps" placeholder='请选择'/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='备注' prop="F_Description"  :label-width="100 * 1.5">
					<JnpfTextarea v-model="dataForm.F_Description" placeholder='请输入'  :showCount='true' />
				</u-form-item>
			</view>
			<view class="jnpf-table">
				<view class="jnpf-table-title u-line-1" >{{'选择入库商品'}}</view>
				<view class="u-p-l-20 u-p-r-20 form-item-box">
					<u-form-item label='加工单'  :label-width="100 * 1.5" v-if="dataForm.F_Type == '0'" @click="addGoods" >
						<!-- <JnpfPopupSelect interfaceName='getPopSelect' multiple v-model="processingList" propsValue='id' relationField='F_ProcessNo' :columnOptions='optionsObj.ProcessingListOptions' interfaceId='2014906814872817664' placeholder='请选择' hasPage :pageSize='20' popupType='dialog' popupTitle='选择数据' popupWidth='70%' :templateJson='optionsObj.ProcessingListTemplateJson' :vModel="'tableFieldc87c94-F_CustomerId'" type="popup"  @change="changeProcessOut" :extraOptions="optionsObj.ProcessingList"/> -->
					</u-form-item>
					<u-form-item label='外协工单'  :label-width="100 * 1.5" v-else>
						<JnpfPopupSelect  interfaceName='getPopSelect' multiple v-model="outsourceList" propsValue='id' relationField='F_OutsourceNo' :columnOptions='optionsObj.OutsourceListOptions' interfaceId='2014907198513221632' placeholder='请选择' hasPage :pageSize='20' popupType='dialog' popupTitle='选择数据' popupWidth='70%' :templateJson='optionsObj.OutsourceListTemplateJson' :vModel="'tableFieldc87c94-F_CustomerId'" type="popup"  @change="changeProcessOut" :extraOptions="optionsObj.OutsourceList"/>
					</u-form-item>
				</view>
				<view class="jnpf-table-item" v-for="(item,i) in dataForm.tableFieldc43f9b" :key="i">
					<view class="jnpf-table-item-title">
						<view class="jnpf-table-item-title-num">({{i+1}})</view>
						<view class="u-flex">
		        <view class="jnpf-table-delete-btn" @click="removeAPuStockFgItemRow(i,true)">{{this.$t('common.delText','删除')}}</view>
						</view>
                    </view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='工单号'  :label-width="100 * 1.5">
							<JnpfSelect :disabled="Boolean(dataForm.id)" v-model="dataForm.tableFieldc43f9b[i].F_WorkOrderId" :options="tableFieldc43f9b_F_WorkOrderIdOptions" :props="getF_WorkOrderIdProps" filterable />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='商品'  :label-width="100 * 1.5">
							<JnpfSelect disabled v-model="dataForm.tableFieldc43f9b[i].F_CustomerId" :options="tableFieldc43f9b_F_CustomerIdOptions" :props="tableFieldc43f9b_F_CustomerIdProps" filterable  />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='库存编码'  :label-width="100 * 1.5">
							<JnpfInput disabled v-model="dataForm.tableFieldc43f9b[i].F_Encoding" :precision='0' :step='1.0'  />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='入库数量'  :label-width="100 * 1.5">
							<!-- <JnpfInputNumber v-model="dataForm.tableFieldc43f9b[i].F_Num" :precision='0' :min='0' :step='1.0'  /> -->
							<view class="form-value" v-if="getUnitRatioLevels(dataForm.tableFieldc43f9b[i])?.level2?.name">
								<JnpfInputNumber disabled @change="handleNumLevel1Change(dataForm.tableFieldc43f9b[i])":value="1" placeholder="1" :controls="false" :disabled="true" :style="{ width: '80px' }" />
								<text>{{ getUnitRatioLevels(dataForm.tableFieldc43f9b[i])?.level1?.name }}</text>
								<JnpfInputNumber @change="handleNumLevel2Change(dataForm.tableFieldc43f9b[i])" :disabled="Boolean(dataForm.id)" v-model="dataForm.tableFieldc43f9b[i].F_Num" placeholder="请输入" :controls="false" :max="getUnitRatioLevels(dataForm.tableFieldc43f9b[i])?.level2?.qty" :style="{ width: '80px' }" />
								<text>{{ getUnitRatioLevels(dataForm.tableFieldc43f9b[i])?.level2?.name }}</text>
													 
							</view>
							<view  class="form-value" v-else>
							  <JnpfInputNumber  disabled  @change="handleNumLevel2Change(dataForm.tableFieldc43f9b[i])" v-model="dataForm.tableFieldc43f9b[i].F_Num"	placeholder="请输入"	:step="1.0"	:controls="false" :style="{width: '100%'}" />
							  <text>{{ getUnitRatioLevels(dataForm.tableFieldc43f9b[i])?.level1?.name }}</text>
							</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='成本单价(元)'  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableFieldc43f9b[i].F_Price" :precision='2' :min='0' :step='1.0'  />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='入库仓库' prop="F_WarehouseID" required  :label-width="100 * 1.5"  @click="handleF_Warehouse(i)">
							<JnpfCascader v-if='dataForm.tableFieldc43f9b[i].F_WarehouseID' disabled v-model="dataForm.tableFieldc43f9b[i].F_WarehouseID" :options="f_WarehouseIdOptions" :props="f_WarehouseIdProps" placeholder='请选择' filterable clearable />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='备注'  :label-width="100 * 1.5">
							<JnpfTextarea v-model="dataForm.tableFieldc43f9b[i].F_Description" placeholder='请输入'  :showCount='false' />
						</u-form-item>
					</view>
                </view>
			<!-- 	<view class="jnpf-table-footer-btn" >
					<view class="jnpf-table-btn jnpf-table-primary-btn" @click="addAPuStockFgItemRow()">
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
	import { buildUUID } from '@/utils/uuid';
    export default {
		components:{ CustomButton },
		computed:{
			getF_WorkOrderIdProps(){
				return this.dataForm.F_Type=='0' ? this.tableFieldc43f9b_F_WorkOrderIdProps:  this.tableFieldc43f9b_F_WorkOrderIdProps2
				
			}
		},
        data() {
            return {
				keyCode : +new Date(),
                btnLoading: false,
				addTableConf:{
				},
                dataForm: {
                    id:'',
					F_StockInNo:undefined,
					F_StockInDate:undefined,
					F_WarehouseId:undefined,
					F_ProcessId:"0",
					F_StockInType:undefined,
					F_Method:undefined,
					F_Description:undefined,
					F_CreatorTime:undefined,
					tableFieldc43f9b:[],
					tableFieldfcaa70:[],
                },
                rules: {
					F_StockInDate:[
						{
							required:true,
							message:this.$t('common.chooseText')+' '+'入库日期',
							trigger:'change',
							type:'number'
						},
					],
					F_WarehouseId:[
						{
							required:true,
							message:this.$t('common.chooseText')+' '+'入库仓库',
							trigger:'change',
							type:'array'
						},
					],
					F_ProcessId:[
						{
							required:true,
							message:this.$t('common.chooseText')+' '+'工单类型',
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
				jurisdictionType:'',
				addType: 0,
				f_WarehouseIdProps:{'label':'F_Name','value':'F_Id',"children": "children"},
				f_WarehouseIdOptions: [],
				f_ProcessIdProps:{'label':'fullName','value':'enCode'},
				f_ProcessIdOptions: [],
				f_StockInTypeProps:{'label':'fullName','value':'enCode'},
				f_StockInTypeOptions: [],
				f_MethodProps:{'label':'fullName','value':'enCode'},
				f_MethodOptions: [],
				tableFieldc43f9b_F_WorkOrderIdProps:{'label':'F_ProcessNo','value':'id'},
				tableFieldc43f9b_F_WorkOrderIdProps2:{'label':'F_OutsourceNo','value':'id'},
				tableFieldc43f9b_F_WorkOrderIdOptions : [],
				tableFieldc43f9b_F_CustomerIdProps:{'label':'F_GoodsName','value':'id'},
				tableFieldc43f9b_F_CustomerIdOptions : [],
				WorkOrderId:"",
				processingList:[],
				outsourceList:[],
				optionsObj:{
					ProcessingListOptions: [
					        {
					          value: 'F_ProcessNo',
					          label: '加工单号',
					        },
					        {
					          value: 'F_GoodsName',
					          label: '商品名称',
					        },
					        {
					          value: 'F_GoodsNo',
					          label: '商品编号',
					        },
					        {
					          value: 'F_Specification',
					          label: '商品规格',
					        },
					        {
					          value: 'F_InventoryNum',
					          label: '库存数量',
					        },
					        {
					          value: 'F_PlanQty',
					          label: '计划数量',
					        },
					        {
					          value: 'F_YseNum',
					          label: '已入库数量',
					        },
					        {
					          value: 'F_Description',
					          label: '备注',
					        },
					      ],
					      ProcessingListTemplateJson: [],
					      OutsourceListOptions: [
					        {
					          value: 'F_OutsourceNo',
					          label: '外协工单号',
					        },
					        {
					          value: 'F_GoodsName',
					          label: '商品名称',
					        },
					        {
					          value: 'F_GoodsNo',
					          label: '商品编号',
					        },
					        {
					          value: 'F_Specification',
					          label: '商品规格',
					        },
					        {
					          value: 'F_InventoryNum',
					          label: '库存数量',
					        },
					        {
					          value: 'F_PlanNum',
					          label: '计划数量',
					        },
					        {
					          value: 'F_YseNum',
					          label: '已入库数量',
					        },
					        {
					          value: 'F_Description',
					          label: '备注',
					        },
					      ]
				}
            };
        },
		watch: {
			'dataForm.tableFieldc43f9b': {
				handler: function(newVal, oldValue) {
				if (!Array.isArray(newVal)) return;
				      for (let i = 0; i < newVal.length; i++) {
				        const row = newVal[i] || {};
				        const levels = this.getUnitRatioLevels(row);
				       if (levels) {
				          if (levels.level2?.qty) {
				            // 每条最大单位数量固定为 1，二级数量自动换算
				            row.F_NumLevel1 = 1;
				            row.F_NumLevel2 = row.F_Num;
				            // row._F_NumLevel2Max = row.F_NumLevel2;
				            row.F_Num = row.F_Num;
				          } else {
				            // 每条最大单位数量固定为 1，二级数量自动换算
				            row.F_NumLevel1 = 1;
				            row.F_NumLevel2 = 1;
				            // row._F_NumLevel2Max = row.F_NumLevel2;
				            row.F_Num = row.F_NumLevel2;
				          }
				        } else {
				          row.F_Num = row.F_Num;
				          row.F_NumLevel1 = 1;
				          row.F_NumLevel2 = 1;
				        }
				      }
				},
				immediate: true,
				deep: true,
			}
        },
        onLoad(option) {
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
			if (option.F_ProcessId) {
			     this.dataForm.F_Type = option.F_Type;
			     this.WorkOrderId = option.F_ProcessId;
			} else {
			     this.dataForm.F_Type = '0';
			     this.WorkOrderId = '';
			}
			if(!this.dataForm.id){
				this.gettableFieldc43f9b_F_WorkOrderIdOptions();
			}
			this.idList = option.idList ? option.idList.split(",") : []
            uni.setNavigationBarTitle({
                title: _title
            })
			this.dataValue = JSON.parse(JSON.stringify(this.dataForm))
            this.initData()
			this.getf_WarehouseIdOptions();
			this.getf_ProcessIdOptions();
			this.getf_StockInTypeOptions();
			this.getf_MethodOptions();
			this.gettableFieldc43f9b_F_CustomerIdOptions();
            uni.$on('linkPageConfirm', (subVal, Vmodel) => {
				let item = {
						F_WorkOrderId:undefined,
						F_CustomerId:undefined,
						F_Num:undefined,
						F_Price:undefined,
						F_Description:undefined,
						F_CreatorTime:undefined,
					}
               if ('APuStockFgItem' === Vmodel) subVal.forEach(t => this.dataForm.tableFieldc43f9b.push({...item,...t}));
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
			getf_ProcessIdOptions(){
				getDictionaryDataSelector('2014894783159472128').then(res => {
					this.f_ProcessIdOptions = res.data.list
				});
			},
			getf_StockInTypeOptions(){
				getDictionaryDataSelector('2012074650393251840').then(res => {
					this.f_StockInTypeOptions = res.data.list
				});
			},
			getf_MethodOptions(){
				getDictionaryDataSelector('2014907575941861376').then(res => {
					this.f_MethodOptions = res.data.list
				});
			},
			gettableFieldc43f9b_F_WorkOrderIdOptions(i){
				if(this.dataForm.F_Type=='1') return
				this.tableFieldc43f9b_F_WorkOrderIdOptions = []
				let templateJson = []
				let query = {
					paramList: this.getParamList(templateJson, this.dataForm , i)
				}
				getDataInterfaceRes('2014906814872817664', query).then(res => {
					let data = res.data
					this.tableFieldc43f9b_F_WorkOrderIdOptions = Array.isArray(data) ? data : [];
				});
			},
			gettableFieldc43f9b_F_OutsourceIdOptions(i){
				this.tableFieldc43f9b_F_WorkOrderIdOptions = []
				let templateJson = []
				let query = {
					paramList: this.getParamList(templateJson, this.dataForm , i)
				}
				getDataInterfaceRes('2014907198513221632', query).then(res => {
					let data = res.data
					this.tableFieldc43f9b_F_WorkOrderIdOptions = Array.isArray(data) ? data : [];
				});
			},
			gettableFieldc43f9b_F_CustomerIdOptions(i){
				this.tableFieldc43f9b_F_CustomerIdOptions = []
				let templateJson = [
  {
    "defaultValue": "",
    "field": "contractId",
    "dataType": "varchar",
    "required": 0,
    "fieldName": "合同ID",
    "relationField": "",
    "jnpfKey": null,
    "complexHeaderList": null,
    "sourceType": 3,
    "isChildren": false,
    "IsMultiple": false
  }
]
				let query = {
					paramList: this.getParamList(templateJson, this.dataForm , i)
				}
				getDataInterfaceRes('2012085692393459712', query).then(res => {
					let data = res.data
					this.tableFieldc43f9b_F_CustomerIdOptions = Array.isArray(data) ? data : []
				});
			},
			selfInit(){
				this.addAPuStockFgItem();
				this.addAPuStockFgLog();
			},
			resetForm() {
				this.dataForm.tableFieldc43f9b = [];
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
                        url: '/api/example/APuStockFg/' + this.dataForm.id,
                        method: 'get',
                    }).then(res => {
                        this.dataForm = res.data;
						if(!this.dataForm.F_WarehouseId)this.dataForm.F_WarehouseId=[];
						this.processingList = [];
						this.outsourceList = [];
						this.dataForm.tableFieldc43f9b.map(item=>{
							if (this.dataForm.F_Type == '0') {
							    this.processingList.push(item.F_WorkOrderId);
								this.gettableFieldc43f9b_F_WorkOrderIdOptions();
							} else {
							    this.outsourceList.push(item.F_WorkOrderId);
								this.gettableFieldc43f9b_F_OutsourceIdOptions();
							}
							// 回显入库数量：F_Num 拆分为一级 + 二级展示
							this.fillNumLevelsFromF_Num(item);
						})
                    })
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
					if (!this.validateSortCode(this.dataForm.tableFieldc43f9b))  return;
                    this.btnLoading = true
					
                    if (this.dataForm.id) {
                        request({
                            url: '/api/example/APuStockFg/' + this.dataForm.id,
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
                            url: '/api/example/APuStockFg',
                            method: 'post',
                            data: this.dataForm,
                        }).then(res => {
                            uni.showToast({
                                title: res.msg,
                                complete: () => {
                                    setTimeout(() => {
										if (type == 1) {
											this.dataForm = JSON.parse(JSON.stringify(this.dataValue))
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
			addAPuStockFgItemRow() {
				let item = {
						F_WorkOrderId:undefined,
						F_CustomerId:undefined,
						F_Num:undefined,
						F_Price:undefined,
						F_Description:undefined,
						F_CreatorTime:undefined,
					}
				this.dataForm.tableFieldc43f9b.push(item)
				this.initCollapse()
			},
			removeAPuStockFgItemRow(i,showConfirm=false) {
				const handleRemove = () => {
					this.dataForm.tableFieldc43f9b.splice(i, 1)
					if (this.dataForm.F_Type == '0') {
						this.processingList.splice(i, 1);
					}else {
						this.outsourceList.splice(i, 1);
					}
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
			copyAPuStockFgItemRow(i) {
				let item = JSON.parse(JSON.stringify(this.dataForm.tableFieldc43f9b[i]));
				item.id = '';
				item.length && item.map(o => delete o.rowData);
				item.F_CreatorTime = undefined;
				this.dataForm.tableFieldc43f9b.push(item);
			},
			// 加工单/外协工单改变
			changeProcessOut(ids,option){
				   /* 兜底 */
				    if (!Array.isArray(this.dataForm.tableFieldc43f9b)) {
				      this.dataForm.tableFieldc43f9b = [];
				    }
				    const optionIdSet = new Set(option.map(o => o.id));
				
				    /*  删掉已取消勾选的行 */
				    this.dataForm.tableFieldc43f9b = this.dataForm.tableFieldc43f9b.filter(item => optionIdSet.has(item.F_WorkOrderId));
				    option.forEach(o => {
				      const exist = this.dataForm.tableFieldc43f9b.some(item => item.F_WorkOrderId === o.id);
					  if (!exist) {
				        const newRow = {
							jnpfId: buildUUID(),
							F_WorkOrderId: o.id,
							F_CustomerId: o.F_GoodsId,
							F_Num: undefined,
							F_Price: undefined,
							F_Description: undefined,
				        };
				        this.dataForm.tableFieldc43f9b.push(newRow);
				      }
				    });
			},
			// 工单类型修改
			changeWorkOrder(val){
				switch (val){
					case '0':
					this.gettableFieldc43f9b_F_WorkOrderIdOptions();
					this.outsourceList = [];
					this.dataForm.tableFieldc43f9b = [];
					break;
					default:
					this.processingList = [];
					this.dataForm.tableFieldc43f9b = [];
					this.gettableFieldc43f9b_F_OutsourceIdOptions();
					break;
				}
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
					  this.dataForm.tableFieldc43f9b[i].F_WarehouseID = data.F_ParentId
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
			  					F_CustomerId:data.id,
			  					F_Encoding:data.F_InventoryNo,
			  				}
							console.log(data,'二维码结果');
			  				this.dataForm.tableFieldc43f9b.push(temp);
			  			});
			  		}
			  	});
			  },
			// ======================计算===================start
			  // 解析 F_Unit_Ratio，只取前两级，返回 { level1: { name, qty }, level2: { name, qty } } 或 null
			     // 解析 F_Unit_Ratio（与采购计划 Form 一致）：两级单位 { level1: { name, qty }, level2: { name, qty } }
			    getUnitRatioLevels(record) {
			       const raw = record?.F_Unit_Ratio;
			       if (!raw) return null;
			       try {
			         const str = typeof raw === 'string' ? raw : String(raw);
			         const arr = JSON.parse(str);
			         if (!Array.isArray(arr)) return null;
			         if (arr.length < 2) return { level1: arr[0], level2: null };
			         return { level1: arr[0], level2: arr[1] };
			       } catch (e) {
			         return null;
			       }
			     },
			   
			      handleNumLevel1Change(record) {
			       const levels = this.getUnitRatioLevels(record);
			       if (!levels) return;
			       const qty1 = Number(record.F_NumLevel1) || 0;
			       const ratio = Number(levels.level2?.qty) || 0;
			       record.F_NumLevel2 = qty1 * ratio;
			       record._F_NumLevel2Max = record.F_NumLevel2;
			       record.F_Num = record.F_NumLevel2;
			     },
			     /** 二级数量（最小单位）编辑：不超过该行默认/换算上限，并与 F_Num 同步 */
			      handleNumLevel2Change(record) {
			       const levels = this.getUnitRatioLevels(record);
			       if (!levels) return;
			       let v = Number(record.F_NumLevel2);
			       if (Number.isNaN(v)) v = 0;
			       const max = Number(record._F_NumLevel2Max);
			       if (record._F_NumLevel2Max != null && !Number.isNaN(max) && v > max) {
			         v = max;
			         record.F_NumLevel2 = v;
			       }
			       record.F_Num = v;
			     },
			   
			     /** 回显：F_Num 存二级数量（最小单位），据此拆成一级 + 二级展示 */
			      fillNumLevelsFromF_Num(row) {
			       const levels = this.getUnitRatioLevels(row);
			       if (!levels || row.F_Num == null || row.F_Num === '') return;
			       const qty2 = Number(row.F_Num) || 0;
			       const ratio = Number(levels.level2?.qty) || 0;
			       row.F_NumLevel2 = qty2;
			       row._F_NumLevel2Max = qty2;
			       if (ratio > 0) {
			         row.F_NumLevel1 = qty2 / ratio;
			       } else {
			         row.F_NumLevel1 = qty2;
			       }
			     }
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
</style>



