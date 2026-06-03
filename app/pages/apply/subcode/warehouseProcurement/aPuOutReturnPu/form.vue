﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿<!-- 选择采购单--商品列表-->
<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" :rules="rules" ref="dataForm" :errorType="['toast']" label-position="left" :label-width="100 * 1.5" label-align="right" class="jnpf-form">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='采购退货单号' prop="F_ReturnOutNo"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_ReturnOutNo" placeholder='请填写，忽略将自动生成'  :showCount='false' />
				</u-form-item>
			</view>
<!-- 			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='仓库' prop="F_WarehouseId" required  :label-width="100 * 1.5">
					<JnpfCascader v-model="dataForm.F_WarehouseId" :options="f_WarehouseIdOptions" :props="f_WarehouseIdProps" placeholder='请选择' filterable clearable/>
				</u-form-item>
			</view> -->
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='出库类型' prop="F_ReturnOutType"  :label-width="100 * 1.5">
					<JnpfSelect v-model="dataForm.F_ReturnOutType" :options="f_ReturnOutTypeOptions" :props="f_ReturnOutTypeProps" placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='采购单号' prop="F_OrderId" required  :label-width="100 * 1.5">
					<JnpfPopupSelect  @change="handleOrderSelectChange" v-model='dataForm.F_OrderId' propsValue='F_Id' relationField='F_OrderNo' :columnOptions='F_OrderIdOptions' interfaceId='2011984543933927424' placeholder='请选择' hasPage :pageSize='20' popupType='dialog' popupTitle='选择数据' popupWidth='70%' :templateJson='F_OrderIdTemplateJson' vModel="F_OrderId"  type="popup" :extraOptions='[]'/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='退货日期' prop="F_ReturnOutDate" required  :label-width="100 * 1.5">
					<JnpfDatePicker v-model="dataForm.F_ReturnOutDate" type='date' format='yyyy-MM-dd' placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='备注' prop="F_Description"  :label-width="100 * 1.5">
					<JnpfTextarea v-model="dataForm.F_Description" placeholder='请输入'  :showCount='true' />
				</u-form-item>
			</view>
			<view class="jnpf-table">
				<view class="jnpf-table-title u-line-1" >{{'商品'}}
				</view>
				<view class="u-p-l-20 u-p-r-20 form-item-box">
					<u-form-item label='添加商品'  :label-width="100 * 1.5" @click="addGoods" >
					</u-form-item>
				</view>
				<view class="jnpf-table-item" v-for="(item,i) in dataForm.tableFieldf5c8ef" :key="i">
					<view class="jnpf-table-item-title">
						<view class="jnpf-table-item-title-num">({{i+1}})</view>
						<view class="u-flex">
		        <view class="jnpf-table-delete-btn" @click="removeAPuOutReturnPuItemRow(i,true)">{{this.$t('common.delText','删除')}}</view>
						</view>
                    </view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='商品'  :label-width="100 * 1.5">
							<JnpfPopupSelect disabled  v-model="dataForm.tableFieldf5c8ef[i].F_CustomerId" propsValue='F_Id' relationField='F_GoodsName' :columnOptions='tableFieldf5c8ef_F_CustomerIdOptions' interfaceId='2008721559174385664' placeholder='请选择' hasPage :pageSize='20' popupType='dialog' popupTitle='选择数据' popupWidth='40%' :templateJson='tableFieldf5c8ef_F_CustomerIdTemplateJson' :vModel="'tableFieldf5c8ef-F_CustomerId'+i" type="popup"/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='库存' :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableFieldf5c8ef[i].F_Num" thousands :controls='false'
								:step='1.0' disabled/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='库存编码' :label-width="100 * 1.5">
							<JnpfInput v-model="dataForm.tableFieldf5c8ef[i].F_Encoding" disabled />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='退货数量'  :label-width="100 * 1.5">
							<!-- <JnpfInputNumber v-model="dataForm.tableFieldf5c8ef[i].F_Num" :precision='0' :step='1.0'  /> -->
		                  <view class="form-value" v-if="getUnitRatioLevels(dataForm.tableFieldf5c8ef[i])?.level2?.name">
		                    <JnpfInputNumber
		                      v-model="dataForm.tableFieldf5c8ef[i].F_NumLevel1"
		                      :placeholder="dataForm.tableFieldf5c8ef[i]._quantityDisabled ? '自动带出' : '请输入'"
		                      :controls="false"
		                      disabled
		                      :style="{ width: '80px' }"
		                      @change="handleNumLevel1Change(dataForm.tableFieldf5c8ef[i], i)" />
		                    <text>{{ getUnitRatioLevels(dataForm.tableFieldf5c8ef[i])?.level1?.name }}</text>
		                    <JnpfInputNumber
		                      v-model="dataForm.tableFieldf5c8ef[i].F_NumLevel2"
		                      placeholder="请输入"
		                      :controls="false"
		                      :min="1"
							  :disabled="Boolean(dataForm.id)"
		                      :max="dataForm.tableFieldf5c8ef[i]._F_NumLevel2Max"
		                      :style="{ width: '80px' }"
		                      @change="handleNumLevel2Change(dataForm.tableFieldf5c8ef[i], i)" />
		                    <text>{{ getUnitRatioLevels(dataForm.tableFieldf5c8ef[i])?.level2?.name }}</text>
		                  </view>
		                  <view  class="form-value" v-else>
		                    <JnpfInputNumber
		                      v-model="dataForm.tableFieldf5c8ef[i].F_NumLevel2"
		                      placeholder="请输入"
		                      :controls="false"
		                      disabled
		                      :min="1"
		                      :max="dataForm.tableFieldf5c8ef[i]._F_NumLevel2Max"
		                      :style="{ width: '80px' }"
		                      @change="handleNumLevel2Change(dataForm.tableFieldf5c8ef[i])" />
		                    <text>{{ getUnitRatioLevels(dataForm.tableFieldf5c8ef[i])?.level1?.name }}</text>
		                  </view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='成本单价(元)'  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableFieldf5c8ef[i].F_Price" :step='1.0'  />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='入库仓库' prop="F_WarehouseID" required :label-width="100 * 1.5">
							<JnpfCascader :disabled="Boolean(dataForm.id)" v-model="dataForm.tableFieldf5c8ef[i].F_WarehouseID" :options="f_WarehouseIdOptions" :props="f_WarehouseIdProps" placeholder='请选择' filterable clearable />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='备注'  :label-width="100 * 1.5">
							<JnpfTextarea v-model="dataForm.tableFieldf5c8ef[i].F_Description" placeholder='请输入'  :showCount='false' />
						</u-form-item>
					</view>
                </view>
<!-- 				<view class="jnpf-table-footer-btn" >
					<view class="jnpf-table-btn jnpf-table-primary-btn" @click="addAPuOutReturnPuItemRow()">
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
	import { getDictionaryDataSelector,getDataInterfaceRes,getOrderItems,getDataInterGoodsInventoryNo,getDataInterfaceDataInfoByIds  } from '@/api/common'
	import { useBaseStore } from '@/store/modules/base'
    import request from '@/utils/request'
	import CustomButton from '@/components/CustomButton'
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
					F_ReturnOutNo:undefined,
					F_WarehouseId:undefined,
					F_ReturnOutType:undefined,
					F_OrderId:undefined,
					F_ReturnOutDate:undefined,
					F_Description:undefined,
					F_CreatorUserId:undefined,
					F_CreatorTime:undefined,
					tableFieldf5c8ef:[],
					tableField3ff267:[],
                },
                rules: {
					F_WarehouseId:[
						{
							required:true,
							message:this.$t('common.chooseText')+' '+'仓库',
							trigger:'change',
							type:'array'
						},
					],
					F_OrderId:[
						{
							required:true,
							message:this.$t('common.chooseText')+' '+'采购单号',
							trigger:'change',
						},
					],
					F_ReturnOutDate:[
						{
							required:true,
							message:this.$t('common.chooseText')+' '+'退货日期',
							trigger:'change',
							type:'number'
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
				f_ReturnOutTypeProps:{'label':'fullName','value':'enCode'},
				f_ReturnOutTypeOptions: [],
				F_OrderIdTemplateJson: [],
				F_OrderIdOptions: [
  {
    "value": "F_OrderNo",
    "label": "采购编号"
  },
  {
    "value": "F_SupplierId",
    "label": "供应商"
  },
  {
    "value": "F_ProdPlanId",
    "label": "生产计划"
  },
  {
    "value": "F_Money",
    "label": "商品金额"
  },
  {
    "value": "F_PurchaseNum",
    "label": "采购数量"
  },
  {
    "value": "F_DeliveryDate",
    "label": "交货日期"
  },
  {
    "value": "F_Description",
    "label": "备注"
  }
],
				tableFieldf5c8ef_F_CustomerIdTemplateJson: [
  {
    "defaultValue": "",
    "field": "id",
    "dataType": "varchar",
    "required": 0,
    "fieldName": "",
    "relationField": "1111111111111",
    "jnpfKey": null,
    "complexHeaderList": null,
    "sourceType": 2,
    "isChildren": false,
    "IsMultiple": false
  }
],
				tableFieldf5c8ef_F_CustomerIdOptions: [
 {
    "value": "F_GoodsName",
    "label": "商品名称"
  },
  {
    "value": "F_GoodsNo",
    "label": "商品编号"
  },
  {
    "value": "F_Unit",
    "label": "单位"
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
            };
        },
		watch: {
			'dataForm.tableFieldf5c8ef': {
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
				deep: true
			},
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
			this.getf_ReturnOutTypeOptions();
            uni.$on('linkPageConfirm', (subVal, Vmodel) => {
				let item = {
						F_CustomerId:undefined,
						F_Num:undefined,
						F_Price:undefined,
						F_Description:undefined,
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
						F_CustomerIdOptions:[],
					}
               if ('APuOutReturnPuItem' === Vmodel) subVal.forEach(t => this.dataForm.tableFieldf5c8ef.push({...item,...t}));
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
               if ('APuOutReturnPuLog' === Vmodel) subVal.forEach(t => this.dataForm.tableField3ff267.push({...item,...t}));
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
			getf_ReturnOutTypeOptions(){
				getDictionaryDataSelector('2013096194263355392').then(res => {
					this.f_ReturnOutTypeOptions = res.data.list
				});
			},
    conversionDateTime(type) {
      const format = type === 'yyyy' ? 'yyyy-01-01 00:00:00' : type === 'yyyy-MM' ? 'yyyy-MM-01 00:00:00' :
        type === 'yyyy-MM-dd' ? 'yyyy-MM-dd 00:00:00' : type === 'yyyy-MM-dd HH:mm' ? 'yyyy-MM-dd HH:mm:00' : 'yyyy-MM-dd HH:mm:ss'
      const dataTime = this.jnpf.toDate(new Date(), format)
      return new Date(dataTime).getTime()
    },
	initDefaultData() {
      this.dataForm.F_ReturnOutDate = this.conversionDateTime("yyyy-MM-dd");
	},
			selfInit(){
				this.addAPuOutReturnPuItem();
				this.addAPuOutReturnPuLog();
			},
			resetForm() {
				this.dataForm.tableFieldf5c8ef = [];
				this.dataForm.tableField3ff267 = [];
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
                        url: '/api/example/APuOutReturnPu/' + this.dataForm.id,
                        method: 'get',
                    }).then(async res => {
                        this.dataForm = res.data
						await this.enrichTableRowsGoodsMeta(this.dataForm.tableFieldf5c8ef);
						for (const row of this.dataForm.tableFieldf5c8ef) {
						     this.fillNumLevelsFromF_Num(row);
						   }
						if(!this.dataForm.F_WarehouseId)this.dataForm.F_WarehouseId=[];
                    })
				}else{
					this.initDefaultData()
				}
            },
			submitForm(type) {
                this.$refs.dataForm.validate(valid => {
                    if (!valid) return
                    this.btnLoading = true
                    if (this.dataForm.id) {
                        request({
                            url: '/api/example/APuOutReturnPu/' + this.dataForm.id,
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
                            url: '/api/example/APuOutReturnPu',
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
			addAPuOutReturnPuItemRow() {
				let item = {
						F_CustomerId:undefined,
						F_Num:undefined,
						F_Price:undefined,
						F_Description:undefined,
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
						F_CustomerIdOptions:[],
					}
				this.dataForm.tableFieldf5c8ef.push(item)
				this.initCollapse()
			},
			removeAPuOutReturnPuItemRow(i,showConfirm=false) {
				const handleRemove = () => {
					this.dataForm.tableFieldf5c8ef.splice(i, 1)
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
			copyAPuOutReturnPuItemRow(i) {
				let item = JSON.parse(JSON.stringify(this.dataForm.tableFieldf5c8ef[i]));
				item.id = '';
				item.length && item.map(o => delete o.rowData);
				item.F_CreatorUserId = undefined;
				item.F_CreatorTime = undefined;
				this.dataForm.tableFieldf5c8ef.push(item);
			},
			addAPuOutReturnPuLogRow() {
				let item = {
						F_Type:undefined,
						F_Title:undefined,
						F_Content:undefined,
						F_Reason:undefined,
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
					}
				this.dataForm.tableField3ff267.push(item)
				this.initCollapse()
			},
			removeAPuOutReturnPuLogRow(i,showConfirm=false) {
				const handleRemove = () => {
					this.dataForm.tableField3ff267.splice(i, 1)
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
			copyAPuOutReturnPuLogRow(i) {
				let item = JSON.parse(JSON.stringify(this.dataForm.tableField3ff267[i]));
				item.id = '';
				item.length && item.map(o => delete o.rowData);
				item.F_CreatorUserId = undefined;
				item.F_CreatorTime = undefined;
				this.dataForm.tableField3ff267.push(item);
			},
			async handleOrderSelectChange(val){
				return
				// 清空子表
				      this.dataForm.tableFieldf5c8ef = [];
				      if (!val) return;
				      const res = await getOrderItems(val);
				      const list = (res && res.data) || res || [];
				      if (!Array.isArray(list) || !list.length) return;
				      for (let i = 0; i < list.length; i++) {
				        const item = list[i];
				        const row = {
				          F_CustomerId: item.F_CustomerId ?? item.F_CustomerId,
				          F_Num: item.F_Num ?? item.F_Num,
				          F_Price: undefined,
				          F_Description: undefined,
				          F_CreatorUserId: undefined,
				          F_CreatorTime: undefined,
				        };
				        this.dataForm.tableFieldf5c8ef.push(row);
				      }
			},
			addGoods(){
				// let query = {paramList: [{"field": "goodsNo","defaultValue": 'CA1SP2026031900019',"id": "b6217b","dataType": "varchar","required": "0","fieldName": "库存编码","source": null}]
				// }
				// getDataInterGoodsInventoryNo('2036268230699520000', query).then(res => {
				// 	let data = res.data
				// 	let temp={
				// 		...data,
				// 		F_CustomerId:data.id,
				// 		F_Encoding:data.F_InventoryNo,
				// 	}
				// 	console.log('二维码商品数据',data);
				// 	this.fillNumLevelsFromF_Num(temp);
				// 	this.dataForm.tableFieldf5c8ef.push(temp);
				// });
				// return
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
							console.log('二维码商品数据',data);
							this.fillNumLevelsFromF_Num(temp);
							this.dataForm.tableFieldf5c8ef.push(temp);
						});
					}
				});
			},
			
			  /** 编辑模式下获取商品详情（含 F_Unit_Ratio）用于数量层级显示 */
			  async  enrichTableRowsGoodsMeta(rows) {
			    if (!rows?.length) return;
			    const ids = [
			      ...new Set(
			        rows
			          .map(r => r.F_CustomerId)
			          .filter(id => id != null && id !== '')
			          .map((id) => String(id)),
			      ),
			    ];
			    if (!ids.length) return;
			    const interfaceId = '2008721559174385664';
			    const query = {
			      ids,
			      interfaceId,
			      propsValue: 'F_Id',
			      relationField: 'F_GoodsName',
			      paramList: [],
			    };
			    try {
			      const resp = await getDataInterfaceDataInfoByIds(interfaceId, query);
			      const list = (resp && resp.data) || [];
			      const map = {};
			      list.forEach((item) => {
			        if (item && item.F_Id != null) map[String(item.F_Id)] = item;
			      });
			      rows.forEach((row) => {
			        const id = row.F_CustomerId != null ? String(row.F_CustomerId) : '';
			        const obj = id && map[id];
			        if (obj) {
			          row._F_CustomerObj = obj;
			          row.F_Unit_Ratio = obj.F_Unit_Ratio ?? row.F_Unit_Ratio;
			        }
			      });
			    } catch (e) {
			      console.error('enrichTableRowsGoodsMeta', e);
			    }
			  },
			// ===============================计算======================start
			  // 解析 F_Unit_Ratio（与 aPuReturnPrd 一致）：两级单位 { level1: { name, qty }, level2: { name, qty } }
			   getUnitRatioLevels(record) {
			    const raw = record?.F_Unit_Ratio || record?._F_CustomerObj?.F_Unit_Ratio;
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
			   handleNumLevel1Change(record, index) {
			    const levels = this.getUnitRatioLevels(record);
			    if (!levels) return;
			    const qty1 = Number(record.F_NumLevel1) || 0;
			    const ratio = Number(levels.level2?.qty) || 0;
			    record.F_NumLevel2 = qty1 * ratio;
			    record._F_NumLevel2Max = record.F_NumLevel2;
			    record.F_Num = record.F_NumLevel2;
			  },
			
			  /** 二级数量（最小单位）编辑：不超过该行默认/换算上限，并与 F_Num 同步 */
			   handleNumLevel2Change(record, index) {
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
			  },
			// ===============================计算======================end
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



