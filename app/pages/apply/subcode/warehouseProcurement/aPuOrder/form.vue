﻿﻿﻿﻿﻿﻿﻿﻿<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" :rules="rules" ref="dataForm" :errorType="['toast']" label-position="left" :label-width="100 * 1.5" label-align="right" class="jnpf-form">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='采购单号' prop="F_OrderNo"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_OrderNo" placeholder='请填写，忽略将自动生成'  :showCount='false' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='供应商' prop="F_SupplierId"  :label-width="100 * 1.5">
					<JnpfSelect v-model="dataForm.F_SupplierId" :options="f_SupplierIdOptions" :props="f_SupplierIdProps" placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='商品金额' prop="F_Money"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_Money" :step='1.0' disabled  />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='采购数量' prop="F_PurchaseNum"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_PurchaseNum" :step='1.0' disabled  />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='采购计划' prop="F_ProdPlanId"  :label-width="100 * 1.5">
					<JnpfPopupSelect v-model='dataForm.F_ProdPlanId' propsValue='F_Id' relationField='F_PlanName' :columnOptions='F_ProdPlanIdOptions' interfaceId='2011711502578487296' placeholder='请选择' :pageSize='20' popupType='dialog' popupTitle='选择数据' popupWidth='60%' :templateJson='F_ProdPlanIdTemplateJson' vModel="F_ProdPlanId"  type="popup" :extraOptions='[]' @change='handleProdPlanChange'/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='采购人' prop="F_UserId"  :label-width="100 * 1.5">
					<JnpfUserSelect v-model="dataForm.F_UserId" placeholder='请选择' selectType='all' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='交货日期' prop="F_DeliveryDate"  :label-width="100 * 1.5">
					<JnpfDatePicker v-model="dataForm.F_DeliveryDate" type='date' format='yyyy-MM-dd' placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='备注' prop="F_Description"  :label-width="100 * 1.5">
					<JnpfTextarea v-model="dataForm.F_Description" placeholder='请输入'  :showCount='false' />
				</u-form-item>
			</view>
			<view class="jnpf-table">
				<view class="jnpf-table-title u-line-1" >{{'采购单商品'}}
				</view>
				<view class="u-p-l-20 u-p-r-20 form-item-box">
					<u-form-item label='商品选择'  :label-width="100 * 1.5">
						<JnpfPopupSelect disabled multiple interfaceName='getPopSelect'  v-model="tempGoodsSelector" propsValue='F_Id' relationField='F_GoodsName' :columnOptions='tableFieldf01abd_F_CustomerIdOptions' interfaceId='2008721559174385664' placeholder='请选择' :pageSize='20' popupType='dialog' popupTitle='选择数据' popupWidth='60%' :templateJson='tableFieldf01abd_F_CustomerIdTemplateJson' :vModel="'tableFieldf01abd-F_CustomerId'" type="popup" @change='handleGoods'/>
					</u-form-item>
				</view>
				<view class="jnpf-table-item" v-for="(item,i) in dataForm.tableFieldf01abd" :key="i">
					<view class="jnpf-table-item-title">
						<view class="jnpf-table-item-title-num">({{i+1}})</view>
						<view class="u-flex">
		        <view class="jnpf-table-delete-btn" @click="removeAPuOrderItemRow(i,true)">{{this.$t('common.delText','删除')}}</view>
						</view>
                    </view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='商品'  :label-width="100 * 1.5">
							<JnpfPopupSelect disabled v-model="dataForm.tableFieldf01abd[i].F_CustomerId" propsValue='F_Id' relationField='F_GoodsName' :columnOptions='tableFieldf01abd_F_CustomerIdOptions' interfaceId='2008721559174385664' placeholder='请选择' :pageSize='20' popupType='dialog' popupTitle='选择数据' popupWidth='70%' :templateJson='tableFieldf01abd_F_CustomerIdTemplateJson' :vModel="'tableFieldf01abd-F_CustomerId'+i" type="popup"/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='供应商'  :label-width="100 * 1.5">
							<JnpfSelect v-model="dataForm.tableFieldf01abd[i].F_SupplierId" placeholder='请选择' :options="tableFieldf01abd_F_SupplierIdOptions" :props="tableFieldf01abd_F_SupplierIdProps" />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='数量'  :label-width="100 * 1.5">
						<view class="form-value" v-if="getUnitRatioLevels(dataForm.tableFieldf01abd[i])?.level2?.name">
							<JnpfInputNumber v-model="dataForm.tableFieldf01abd[i].F_NumLevel1" placeholder="请输入" :controls="false"
					          :style="{ width: '80px' }" @input="(val) => handleNumLevelChange(val, i)" />
							   <!-- @change="handleNumLevelChange(dataForm.tableFieldf01abd[i])" -->
					        <text>{{ getUnitRatioLevels(dataForm.tableFieldf01abd[i])?.level1?.name }}</text>
					        <JnpfInputNumber v-model="dataForm.tableFieldf01abd[i].F_NumLevel2" placeholder="自动计算" :controls="false"
					          disabled :style="{ width: '80px' }" />
					        <text>{{ getUnitRatioLevels(dataForm.tableFieldf01abd[i])?.level2?.name }}</text>
					
					    </view>
					      <view class="form-value" v-else>
					        <JnpfInputNumber v-model="dataForm.tableFieldf01abd[i].F_NumLevel1" placeholder="请输入" :controls="false"
					          :style="{ width: '80px' }" @change="handleNumLevelChange(dataForm.tableFieldf01abd[i])" />
					        <text>{{ getUnitRatioLevels(dataForm.tableFieldf01abd[i])?.level1?.name }}</text>
					      </view>
							<!-- <JnpfInputNumber v-model="dataForm.tableFieldf01abd[i].F_Num" :precision='0' :step='1.0'  controls @change="recalcTableRow(i)"/> -->
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='单价'  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableFieldf01abd[i].F_Price" :step='1.0'  @change="recalcTableRow(i)"/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='折扣'  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableFieldf01abd[i].F_Discount" :step='1.0'  @change="(e) => handleDiscount(e,i)"/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='优惠金额'  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableFieldf01abd[i].F_DiscountMoney" :step='1.0'  @change="onDiscountMoneyChange(i)"/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='备注'  :label-width="100 * 1.5">
							<JnpfTextarea v-model="dataForm.tableFieldf01abd[i].F_Description" placeholder='请输入'  :showCount='false' />
						</u-form-item>
					</view>
                </view>
<!-- 				<view class="jnpf-table-footer-btn" >
					<view class="jnpf-table-btn jnpf-table-primary-btn" @click="addAPuOrderItemRow()">
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
	import { getDictionaryDataSelector,getDataInterfaceRes,getProdPlanItems,getDataInterfaceDataInfoByIds } from '@/api/common'
	import { useBaseStore } from '@/store/modules/base'
    import request from '@/utils/request'
	import CustomButton from '@/components/CustomButton'
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
					F_OrderNo:undefined,
					F_SupplierId:undefined,
					F_Money:0,
					F_PurchaseNum:0,
					F_ProdPlanId:undefined,
					F_UserId:undefined,
					F_DeliveryDate:undefined,
					F_Description:undefined,
					F_CreatorUserId:undefined,
					F_CreatorTime:undefined,
					tableFieldf01abd:[],
					tableField8b2a57:[],
                },
                rules: {
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
				f_SupplierIdProps:{'label':'F_SupplierName','value':'F_Id'},
				f_SupplierIdOptions: [],
				F_ProdPlanIdTemplateJson: [],
				F_ProdPlanIdOptions: [
  {
    "value": "F_PlanNo",
    "label": "采购计划编号"
  },
  {
    "value": "F_PlanName",
    "label": "采购计划名"
  },
  {
    "value": "F_SupplierId",
    "label": "供应商名"
  },
  {
    "value": "F_Money",
    "label": "金额"
  },
  {
    "value": "F_PurchaseNum",
    "label": "数量"
  }
],
				tableFieldf01abd_F_CustomerIdTemplateJson: [],
				tableFieldf01abd_F_CustomerIdOptions: [
  {
    "value": "F_GoodsName",
    "label": "商品名称"
  },
  {
    "value": "F_GoodsNo",
    "label": "商品编码"
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
    "value": "F_Source",
    "label": "商品来源"
  },
  {
    "value": "F_Remark",
    "label": "备注"
  }
],
				tableFieldf01abd_F_SupplierIdProps:{'label':'F_SupplierName','value':'F_Id'},
				tableFieldf01abd_F_SupplierIdOptions : [],
				tempGoodsSelector:[]
            };
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
			this.idList = option.idList ? option.idList.split(",") : []
            uni.setNavigationBarTitle({
                title: _title
            })
			this.dataValue = JSON.parse(JSON.stringify(this.dataForm))
            this.initData()
			this.getf_SupplierIdOptions();
			this.gettableFieldf01abd_F_SupplierIdOptions();
            uni.$on('linkPageConfirm', (subVal, Vmodel) => {
				let item = {
						F_CustomerId:undefined,
						F_SupplierId:undefined,
						F_Num:undefined,
						F_Price:undefined,
						F_Discount:undefined,
						F_DiscountMoney:undefined,
						F_Description:undefined,
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
					}
               if ('APuOrderItem' === Vmodel) subVal.forEach(t => this.dataForm.tableFieldf01abd.push({...item,...t}));
			   this.initCollapse();
            });
            uni.$on('linkPageConfirm', (subVal, Vmodel) => {
				let item = {
						F_Title:undefined,
						F_Content:undefined,
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
					}
               if ('APuOrderLog' === Vmodel) subVal.forEach(t => this.dataForm.tableField8b2a57.push({...item,...t}));
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
			getf_SupplierIdOptions(){
				this.f_SupplierIdOptions = []
				let templateJson = []
				let query = {
					paramList: this.getParamList(templateJson, this.dataForm)
				}
				getDataInterfaceRes('2008457397298925568', query).then(res => {
					let data = res.data
					this.f_SupplierIdOptions = Array.isArray(data) ? data : []
				});
			},
			gettableFieldf01abd_F_SupplierIdOptions(i){
				this.tableFieldf01abd_F_SupplierIdOptions = []
				let templateJson = []
				let query = {
					paramList: this.getParamList(templateJson, this.dataForm , i)
				}
				getDataInterfaceRes('2008457397298925568', query).then(res => {
					let data = res.data
					this.tableFieldf01abd_F_SupplierIdOptions = Array.isArray(data) ? data : []
				});
			},
			selfInit(){
				this.addAPuOrderItem();
				this.addAPuOrderLog();
			},
			resetForm() {
				this.dataForm.tableFieldf01abd = [];
				this.dataForm.tableField8b2a57 = [];
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
                        url: '/api/example/APuOrder/' + this.dataForm.id,
                        method: 'get',
                    }).then(res => {
                        this.dataForm = res.data;
						this.tempGoodsSelector = []
						this.dataForm.tableFieldf01abd.map((item,index)=>{
						this.tempGoodsSelector.push(item.F_CustomerId);
							item.jnpfId = buildUUID();
							// 先根据 F_Num（一级数量）与 F_Unit_Ratio 填充两级输入框，再重算金额
							this.fillNumLevelsFromF_Num(item, index);
							this.recalcTableRow(index);
						})
						this.populateMissingCustomerDisplayFields();
						this.syncMoneySum();
                    })
                }
            },
			submitForm(type) {
                this.$refs.dataForm.validate(valid => {
                    if (!valid) return
                    this.btnLoading = true
                    if (this.dataForm.id) {
                        request({
                            url: '/api/example/APuOrder/' + this.dataForm.id,
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
                            url: '/api/example/APuOrder',
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
			addAPuOrderItemRow() {
				let item = {
						F_CustomerId:undefined,
						F_SupplierId:undefined,
						F_Num:undefined,
						F_Price:undefined,
						F_Discount:undefined,
						F_DiscountMoney:undefined,
						F_Description:undefined,
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
					}
				this.dataForm.tableFieldf01abd.push(item)
				this.initCollapse()
			},
			removeAPuOrderItemRow(i,showConfirm=false) {
				const handleRemove = () => {
					this.dataForm.tableFieldf01abd.splice(i, 1)
					this.tempGoodsSelector.splice(i, 1)
					this.initCollapse()
			      // 同步采购数量为子表行数
			      this.syncPurchaseNum();
			      // 同步主表金额
			      this.syncMoneySum();
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
			copyAPuOrderItemRow(i) {
				let item = JSON.parse(JSON.stringify(this.dataForm.tableFieldf01abd[i]));
				item.id = '';
				item.length && item.map(o => delete o.rowData);
				item.F_CreatorUserId = undefined;
				item.F_CreatorTime = undefined;
				this.dataForm.tableFieldf01abd.push(item);
			},
			addAPuOrderLogRow() {
				let item = {
						F_Title:undefined,
						F_Content:undefined,
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
					}
				this.dataForm.tableField8b2a57.push(item)
				this.initCollapse()
			},
			removeAPuOrderLogRow(i,showConfirm=false) {
				const handleRemove = () => {
					this.dataForm.tableField8b2a57.splice(i, 1)
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
			copyAPuOrderLogRow(i) {
				let item = JSON.parse(JSON.stringify(this.dataForm.tableField8b2a57[i]));
				item.id = '';
				item.length && item.map(o => delete o.rowData);
				item.F_CreatorUserId = undefined;
				item.F_CreatorTime = undefined;
				this.dataForm.tableField8b2a57.push(item);
			},
			handleDiscount(e,index){
			let record =this.dataForm.tableFieldf01abd[index]
			record.F_Discount=e
			this.recalcTableRow(index)
			},
			  /**
			   * 重新计算子表单行金额、优惠后金额和优惠金额
			   * 规则：
			   *  - F_Amount = F_Price * F_Num
			   *  - F_DiscountedAmount = F_Amount * F_Discount
			   *  - F_DiscountMoney = F_Amount - F_DiscountedAmount
			   */
			   recalcTableRow(index) {
				   let record =this.dataForm.tableFieldf01abd[index]
				       if (!record) return;
				       const levels = this.getUnitRatioLevels(record);
				       // 有两级单位时：F_Num 存第一框（一级数量）；二级数量仅展示/换算
				       if (levels && record.F_NumLevel1 != null && record.F_NumLevel1 !== '') {
				         const qty1 = Number(record.F_NumLevel1) || 0;
				         const ratio = Number(levels.level2?.qty) || 0;
				         this.$set(record, 'F_NumLevel2', qty1 * ratio);
				         this.$set(record, 'F_Num', qty1);
				       }
				       const price = Number(record.F_Price) || 0;
				       const num = levels ? Number(record.F_NumLevel1) || 0 : Number(record.F_Num) || 0;
				       const amount = price * num;
				       this.$set(record, 'F_Amount', Number(amount.toFixed(2)));
				       // 采购计划回填等未填折扣时：优惠金额保持为空，优惠后金额=金额；若用户填了折扣则从折扣反算
				       const discountInput = record.F_Discount != null && record.F_Discount !== '';
				       let discountMoney;
				       if (discountInput) {
				         const discount = Number(record.F_Discount) || 0;
				         const discountFactor = discount / 10;
				         discountMoney = amount - amount * discountFactor;
				       } else {
				         discountMoney = Number(record.F_DiscountMoney) || 0;
				       }
				       if (discountMoney < 0) discountMoney = 0;
				       if (discountMoney > amount) discountMoney = amount;
				       this.$set(record, 'F_DiscountMoney', discountMoney === 0 ? undefined : Number(discountMoney.toFixed(2)));
				       this.$set(record, 'F_DiscountedAmount', Number((amount - discountMoney).toFixed(2)));
				       // 同步主表金额（子表优惠后金额之和）
				       this.syncMoneySum();
				       // 同步采购数量为子表数量之和（当编辑行的数量时也要更新总采购数）
				       this.syncPurchaseNum();
				   // 旧代码
				   // return
				   // let record =this.dataForm.tableFieldf01abd[index]
			    // if (!record) return;
			    // const price = Number(record.F_Price) || 0;
			    // const num = Number(record.F_Num) || 0;
			    // const amount = price * num;
			    // // 折扣语义为“输入 5 表示 5/10 的折扣” => 折扣系数 = discount / 10
			    // const discount = Number(record.F_Discount) || 0;
			    // const discountFactor = discount / 10;
			    // const discountedAmount = amount * discountFactor;
			    // const discountMoney = amount - discountedAmount;
			    // // 保持两位小数
			    // record.F_Amount = Number(amount.toFixed(2));
			    // record.F_DiscountedAmount = Number(discountedAmount.toFixed(2));
			    // record.F_DiscountMoney = Number(discountMoney.toFixed(2));
			    // // 同步主表金额（子表优惠后金额之和）
			    // this.syncMoneySum();
			    // // 同步采购数量为子表数量之和（当编辑行的数量时也要更新总采购数）
			    // this.syncPurchaseNum();
			  },
    /**
		     * 将子表 `tableFieldf01abd` 的 `F_Amount` 求和并写入主表 `F_Money`
		     */
		     syncMoneySum() {
		      try {
		            const list = this.dataForm.tableFieldf01abd || [];
		            let sum = 0;
		            for (let i = 0; i < list.length; i++) {
		              const v = Number(list[i].F_Amount) || 0;
		              sum += v;
		            }
		            // 保持两位小数
		            this.$set(this.dataForm, 'F_Money', Number(sum.toFixed(2)));
		          } catch (e) {
		            // 容错，不应阻塞业务
		          }
		    },
			  // 将主表采购数量同步为子表数量字段之和（F_Num 的总和）
			   syncPurchaseNum() {
			    try {
			      const list = this.dataForm.tableFieldf01abd || [];
			      let sum = 0;
			      for (let i = 0; i < list.length; i++) {
			        const v = Number(list[i].F_Num) || 0;
			        sum += v;
			      }
			      this.$set(this.dataForm, 'F_PurchaseNum', sum);
			    } catch (e) {
			      // ignore
			    }
			},
				  /**
				   * 当人工修改 优惠金额(F_DiscountMoney) 时，同步计算 优惠后金额 和 折扣 字段
				   */
			   onDiscountMoneyChange(index){
					let record =this.dataForm.tableFieldf01abd[index]
				    if (!record) return;
				    const rawVal = Number(record.F_DiscountMoney) || 0;
				    // 计算金额优先尝试使用已存在的 F_Amount，否则用单价*数量（有双单位时用一级数量）
				    const levels = this.getUnitRatioLevels(record);
				    const amount = Number(record.F_Amount) || (Number(record.F_Price) || 0) * (levels ? Number(record.F_NumLevel1) || 0 : Number(record.F_Num) || 0);
				    let val = rawVal;
				    if (val < 0) val = 0;
				    if (val > amount) val = amount;
				    this.$set(record, 'F_DiscountMoney', Number(val.toFixed(2)));
				    const discountedAmount = amount - val;
				    this.$set(record, 'F_DiscountedAmount', Number(discountedAmount.toFixed(2)));
				    // 维护折扣字段的语义（折扣系数 = discountedAmount / amount -> 折扣输入为折扣系数*10）
				    if (amount > 0) {
				      this.$set(record, 'F_Discount', Number(((discountedAmount / amount) * 10).toFixed(2)));
				    } else {
				      this.$set(record, 'F_Discount', 0);
				    }
				    // 同步主表金额（子表优惠后金额之和）
				    this.syncMoneySum();
				},
				handleGoods(ids,option){
					this.dataForm.tableFieldf01abd = [];
					this.selectedtableFieldf01abdRowKeys = [];
					this.tempGoodsSelector = [];
					/* 兜底 */
					 if (!Array.isArray(this.dataForm.tableFieldf01abd)) {
					   this.dataForm.tableFieldf01abd = [];
					 }
					 const optionIdSet = new Set(option.map(o => o.id));
									
					 /*  删掉已取消勾选的行 */
					 this.dataForm.tableFieldf01abd = this.dataForm.tableFieldf01abd.filter(item => optionIdSet.has(item.F_WorkOrderId));
					 option.forEach(o => {
					const exist = this.dataForm.tableFieldf01abd.some(item => item.F_WorkOrderId === o.F_Id);
						if (!exist) {
						let item = {
								F_CustomerId:o.F_Id,
								F_SupplierId:undefined,
								F_Num:undefined,
								F_Price:undefined,
								F_Description:undefined,
								F_CreatorUserId:undefined,
								F_CreatorTime:undefined,
							}
						this.dataForm.tableFieldf01abd.push(item)
						
						if (item.F_CustomerId) ids.push(item.F_CustomerId);
						// this.tempGoodsSelector = ids;
					   }
					 });
				},
				handleProdPlanChange(selectedProdPlanId){
					    this.dataForm.tableFieldf01abd = [];
					    this.selectedtableFieldf01abdRowKeys = [];
					    this.tempGoodsSelector = [];
					    if (!selectedProdPlanId) {
					      this.syncPurchaseNum();
					      this.syncMoneySum();
					      return;
					    }
					    // 调用后端接口获取物料列表
					    getProdPlanItems(selectedProdPlanId)
					      .then(resp => {
					        const list = (resp && resp.data) || resp || [];
					        const ids = [];
						      // 清空原有子表
						      this.dataForm.tableFieldf01abd = [];
						      this.selectedtableFieldf01abdRowKeys = [];
						      this.tempGoodsSelector = [];
							     list.forEach((it,index) => {
							            const newRow = {
							              jnpfId: buildUUID(),
							              F_CustomerId: it.F_GoodsId ?? it.F_GoodsId,
							              F_SupplierId: it.F_SupplierId,
							              F_Num: it.F_Num ?? 0,
							              F_Price: it.F_Price ?? 0,
							              F_Discount: undefined,
							              F_DiscountMoney: undefined,
							              F_Description: undefined,
							              F_CreatorUserId: undefined,
							              F_CreatorTime: undefined,
							              _F_CustomerObj: null,
							            };
									this.dataForm.tableFieldf01abd.push(newRow);
									this.fillNumLevelsFromF_Num(newRow, this.dataForm.tableFieldf01abd.length - 1);
						            this.recalcTableRow(index);
							            if (newRow.F_CustomerId) ids.push(newRow.F_CustomerId);
							          });
									          this.tempGoodsSelector = ids;
										// 填充子表展示字段（商品编号/规格/图片）
										this.populateMissingCustomerDisplayFields();
										this.syncPurchaseNum();
										this.syncMoneySum();
					      })
				},
				
				// ======================拆分单位和数量=====================start
				// 解析 F_Unit_Ratio，只取前两级，返回 { level1: { name, qty }, level2: { name, qty } } 或 null
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
				
				  // 一级数量（如箱子）改变时，按比例计算二级数量（如盒）：二级 = 一级 * level2.qty
				   handleNumLevelChange(val, index) {
					const record = this.dataForm.tableFieldf01abd[index];
				    const levels = this.getUnitRatioLevels(record);
				    if (!levels) return;
				    const qty1 = Number(val) || 0;
				    const ratio = Number(levels.level2?.qty) || 0;
				    // 使用 $set 确保响应式更新
				    this.$set(record, 'F_NumLevel1', qty1);
				    this.$set(record, 'F_NumLevel2', qty1 * ratio);
				    this.$set(record, 'F_Num', qty1);
				    this.$nextTick(() => {
				      this.recalcTableRow(index);
				    });
				  },
				
				  // 回显/回填时：F_Num 存第一框（一级数量），据此填充 F_NumLevel1 并计算 F_NumLevel2（与采购计划一致）
				   fillNumLevelsFromF_Num(row) {
				    const levels = this.getUnitRatioLevels(row);
				    if (!levels || row.F_Num == null || row.F_Num === '') return;
				    const qty1 = Number(row.F_Num) || 0;
				    const ratio = Number(levels.level2?.qty) || 0;
				    row.F_NumLevel1 = qty1;
				    row.F_NumLevel2 = qty1 * ratio;
				  },

				// ======================拆分单位和数量=====================end
				
				// 采购计划时再次请求接口（获取单位）
				  async populateMissingCustomerDisplayFields() {
				    try {
				      const rows = this.dataForm.tableFieldf01abd || [];
				      const idsToFetch = [];
				      for (let i = 0; i < rows.length; i++) {
				        const r = rows[i];
				        if (!r) continue;
				        const hasDisplay = r.F_GoodsNo || r.F_Specification || r.F_Image;
				        if (!hasDisplay && r.F_CustomerId) {
				          if (typeof r.F_CustomerId === 'object') {
				            const id = r.F_CustomerId.F_Id ?? r.F_CustomerId.id ?? r.F_CustomerId.value;
				            if (id) idsToFetch.push(String(id));
				          } else {
				            idsToFetch.push(String(r.F_CustomerId));
				          }
				        }
				      }
				      if (!idsToFetch.length) return;
				      const uniqueIds = Array.from(new Set(idsToFetch));
				      const interfaceId = '2008721559174385664';
				      const query = {
				        ids: uniqueIds,
				        interfaceId,
				        propsValue: 'F_Id',
				        relationField: 'F_GoodsName',
				        paramList: [],
				      };
				      const resp = await getDataInterfaceDataInfoByIds(interfaceId, query);
				      const list = (resp && resp.data) || [];
				      const map = {};
				      list.forEach((item) => {
				        if (item && item.F_Id != null) map[String(item.F_Id)] = item;
				      });
				      for (let i = 0; i < rows.length; i++) {
				        const r = rows[i];
				        if (!r) continue;
				        const id =
				          r && r.F_CustomerId && typeof r.F_CustomerId === 'object' ? r.F_CustomerId.F_Id ?? r.F_CustomerId.id ?? r.F_CustomerId.value : r.F_CustomerId;
				        if (id) {
				          const found = map[String(id)];
				          if (found) this.handleCustomerSelectChange(found, r);
				          this.fillNumLevelsFromF_Num(r, i);
				          this.recalcTableRow(i);
				        }
				      }
				    } catch (e) {
				      // ignore errors to avoid blocking UI
				    }
				  },
				     handleCustomerSelectChange(selected, record) {
				      const data = Array.isArray(selected) ? selected[0] : selected;
				      if (!data) {
				        record.F_GoodsNo = undefined;
				        record.F_Specification = undefined;
				        record.F_Image = undefined;
				        record.F_Unit_Ratio = undefined;
				        record.F_NumLevel1 = undefined;
				        record.F_NumLevel2 = undefined;
				        return;
				      }
				      record.F_GoodsNo = data.F_GoodsNo ?? data.GoodsNo ?? data.F_GoodsName ?? data.name ?? record.F_GoodsNo;
				      record.F_Specification = data.F_Specification ?? data.Specification ?? data.spec ?? record.F_Specification;
				      record.F_Unit_Ratio = data.F_Unit_Ratio ?? record.F_Unit_Ratio;
				      let imgVal = data.F_Image ?? data.Image ?? data.img ?? data.picture ?? data.fileUrl ?? data.url ?? data.imageUrl ?? null;
				      try {
				        if (typeof imgVal === 'string' && imgVal.trim().startsWith('[')) {
				          const parsed = JSON.parse(imgVal);
				          if (Array.isArray(parsed) && parsed.length) {
				            const first = parsed[0];
				            imgVal = first.url ?? first.fileUrl ?? first.value ?? first.thumbUrl ?? first.name ?? imgVal;
				          }
				        } else if (Array.isArray(imgVal) && imgVal.length) {
				          const first = imgVal[0];
				          imgVal = first.url ?? first.fileUrl ?? first.value ?? first.thumbUrl ?? first.name ?? imgVal[0];
				        }
				      } catch (e) {
				        // ignore parse errors
				      }
				      if (!imgVal) {
				        record.F_Image = undefined;
				      } else {
				        if (/^https?:\/\//i.test(String(imgVal))) {
				          record.F_Image = String(imgVal);
				        } else {
				          const s = String(imgVal);
				          record.F_Image = s.startsWith(apiUrl) ? s : apiUrl.replace(/\/$/, '') + (s.startsWith('/') ? '' : '/') + s;
				        }
				      }
				    }
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



