﻿﻿﻿﻿﻿﻿﻿﻿<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" :rules="rules" ref="dataForm" :errorType="['toast']" label-position="left" :label-width="100 * 1.5" label-align="right" class="jnpf-form">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='计划编号' prop="F_PlanNo"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_PlanNo" placeholder='请填写，忽略将自动生成'  :showCount='false' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='计划名称' prop="F_PlanName" required  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_PlanName" placeholder='请输入'  :showCount='false' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='供应商' prop="F_SupplierId"  :label-width="100 * 1.5">
					<JnpfSelect v-model="dataForm.F_SupplierId" :options="f_SupplierIdOptions" :props="f_SupplierIdProps" placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='金额' prop="F_Money"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_Money" :step='1.0' disabled  />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='交货日期' prop="F_DeliveryDate"  :label-width="100 * 1.5">
					<JnpfDatePicker v-model="dataForm.F_DeliveryDate" type='date' format='yyyy-MM-dd' placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='采购数量' prop="F_PurchaseNum"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_PurchaseNum" :step='1.0' disabled  />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='备注' prop="F_Description"  :label-width="100 * 1.5">
					<JnpfTextarea v-model="dataForm.F_Description" placeholder='请输入'  :showCount='false' />
				</u-form-item>
			</view>
			<view class="jnpf-table">
				<view class="jnpf-table-title u-line-1" >{{'选择采购商品'}}
				</view>
				<view class="u-p-l-20 u-p-r-20 form-item-box">
					<u-form-item label='商品选择'  :label-width="100 * 1.5">
						<JnpfPopupSelect multiple interfaceName='getPopSelect'  v-model="tempGoodsSelector" propsValue='F_Id' relationField='F_GoodsName' :columnOptions='tableFieldc87c94_F_CustomerIdOptions' interfaceId='2008721559174385664' placeholder='请选择' :pageSize='20' popupType='dialog' popupTitle='选择数据' popupWidth='60%' :templateJson='tableFieldc87c94_F_CustomerIdTemplateJson' :vModel="'tableFieldc87c94-F_CustomerId'" type="popup" @change='handleGoods'/>
					</u-form-item>
				</view>
				<view class="jnpf-table-item" v-for="(item,i) in dataForm.tableFieldc87c94" :key="i">
					<view class="jnpf-table-item-title">
						<view class="jnpf-table-item-title-num">({{i+1}})</view>
						<view class="u-flex">
		        <view class="jnpf-table-delete-btn" @click="removeAPuPlanItemRow(i,true)">{{this.$t('common.delText','删除')}}</view>
						</view>
                    </view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='商品'  :label-width="100 * 1.5">
							<JnpfPopupSelect disabled v-model="dataForm.tableFieldc87c94[i].F_CustomerId" propsValue='F_Id' relationField='F_GoodsName' :columnOptions='tableFieldc87c94_F_CustomerIdOptions' interfaceId='2008721559174385664' placeholder='请选择' hasPage :pageSize='20' popupType='dialog' popupTitle='选择数据' popupWidth='70%' :templateJson='tableFieldc87c94_F_CustomerIdTemplateJson2' :vModel="'tableFieldc87c94-F_CustomerId'+i" type="popup"/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='供应商'  :label-width="100 * 1.5">
							<JnpfSelect v-model="dataForm.tableFieldc87c94[i].F_SupplierId" placeholder='请选择' :options="tableFieldc87c94_F_SupplierIdOptions" :props="tableFieldc87c94_F_SupplierIdProps" />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='数量'  :label-width="100 * 1.5">
							<!-- <JnpfInputNumber v-model="dataForm.tableFieldc87c94[i].F_Num" :precision='0' :step='1.0'  controls /> -->
						<view class="form-value"  v-if="getUnitRatioLevels(dataForm.tableFieldc87c94[i])?.level2?.name">
						    <JnpfInputNumber  v-model="dataForm.tableFieldc87c94[i].F_NumLevel1"  placeholder="请输入" :controls="false" :style="{ width: '80px' }"
						        @input="(val) => handleNumLevel1Change(val, i)"/>
						    <text>{{ getUnitRatioLevels(dataForm.tableFieldc87c94[i])?.level1?.name }}</text>
						    <JnpfInputNumber v-model="dataForm.tableFieldc87c94[i].F_NumLevel2" placeholder="自动计算" :controls="false" disabled :style="{ width: '80px' }" />
						    <text>{{ getUnitRatioLevels(dataForm.tableFieldc87c94[i])?.level2?.name }}</text>
						                   
						</view>
						<view  class="form-value" v-else>
							 <JnpfInputNumber
							    v-model="dataForm.tableFieldc87c94[i].F_NumLevel1"
							    placeholder="请输入"
							    :controls="false"
							    :style="{ width: '80px' }"
							    @input="(val) => handleNumLevel1Change(val, i)" />
							<text>{{ getUnitRatioLevels(dataForm.tableFieldc87c94[i])?.level1?.name }}</text>
						</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='单价'  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableFieldc87c94[i].F_Price" :step='1.0'  />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='备注'  :label-width="100 * 1.5">
							<JnpfTextarea v-model="dataForm.tableFieldc87c94[i].F_Description" placeholder='请输入'  :showCount='false' />
						</u-form-item>
					</view>
                </view>
<!-- 				<view class="jnpf-table-footer-btn" >
					<view class="jnpf-table-btn jnpf-table-primary-btn" @click="addAPuPlanItemRow()">
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
	import { getDictionaryDataSelector,getDataInterfaceRes,getDataInterfaceDataInfoByIds } from '@/api/common'
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
					F_PlanNo:undefined,
					F_PlanName:undefined,
					F_SupplierId:undefined,
					F_Money:0,
					F_DeliveryDate:undefined,
					F_PurchaseNum:0,
					F_Description:undefined,
					F_CreatorUserId:undefined,
					F_CreatorTime:undefined,
					tableFieldc87c94:[],
					tableFielde82301:[],
                },
                rules: {
					F_PlanName:[
						{
							required:true,
							message:this.$t('common.inputText')+' '+'计划名称',
							trigger:'blur',
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
				f_SupplierIdProps:{'label':'F_SupplierName','value':'F_Id'},
				f_SupplierIdOptions: [],
				tableFieldc87c94_F_CustomerIdTemplateJson: [],
				tableFieldc87c94_F_CustomerIdTemplateJson2:[],
				tableFieldc87c94_F_CustomerIdOptions: [
  {
    "value": "F_GoodsName",
    "label": "商品名称"
  },
  {
    "value": "F_GoodsNo",
    "label": "商品编码"
  },
  {
    "value": "F_Specification",
    "label": "规格"
  },
  {
    "value": "F_Remark",
    "label": "备注"
  },
  {
    "value": "F_Source",
    "label": "商品来源"
  }
],
				tableFieldc87c94_F_SupplierIdProps:{'label':'F_SupplierName','value':'F_Id'},
				tableFieldc87c94_F_SupplierIdOptions : [],
				tempGoodsSelector:[]
            };
        },
		watch: {
			'dataForm.tableFieldc87c94': {
				handler: function(newVal, oldValue) {
				let purchaseNum = 0;
			      let total = 0;
			      if (Array.isArray(newVal)) {
			        for (let i = 0; i < newVal.length; i++) {
			          const row = newVal[i] || {};
			          const levels = this.getUnitRatioLevels(row);
			          if (levels && row.F_NumLevel1 != null && row.F_NumLevel1 !== '') {
			            if (levels.level2?.qty) {
			              const qty1 = Number(row.F_NumLevel1) || 1;
			              const ratio = Number(levels.level2?.qty) || 1;
			              row.F_NumLevel2 = qty1 * ratio;
			              row.F_Num = row.F_NumLevel1;
			            } else {
			              row.F_Num = row.F_NumLevel1;
			            }
			          }
			          const amount = this.calculateRowAmount(row);
			          // 将计算结果写回行上的 F_Amount（用于展示）
			          row.F_Amount = amount;
			          total += amount;
			          purchaseNum += levels ? Number(row.F_NumLevel1) || 0 : Number(row.F_Num) || 0;
			        }
			      }
			      // 写入表单顶级数量字段（采购数量：子表数量之和）
			      this.dataForm.F_PurchaseNum = purchaseNum;
			      // 写入表单顶级金额字段（数值型）
			      this.dataForm.F_Money = total;
					// 旧代码
					// let purchaseNum = 0;
					// let total = 0;
					// if (Array.isArray(newVal)) {
					//   for (let i = 0; i < newVal.length; i++) {
					//     const row = newVal[i] || {};
					//     const num = Number(row.F_Num) || 0;
					//     const price = Number(row.F_Price) || 0;
					//     const amount = num * price;
					//     // 将计算结果写回行上的 F_Amount（用于展示）
					//     row.F_Amount = amount;
					//     total += amount;
					//     purchaseNum += num;
					//   }
					// }
					// this.dataForm.F_PurchaseNum = purchaseNum;
					// this.dataForm.F_Money = total;
						  
				},
				immediate: true,
				deep: true
			},
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
			this.gettableFieldc87c94_F_SupplierIdOptions();
            uni.$on('linkPageConfirm', (subVal, Vmodel) => {
				let item = {
						F_CustomerId:undefined,
						F_SupplierId:undefined,
						F_Num:undefined,
						F_Price:undefined,
						F_Description:undefined,
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
					}
               if ('APuPlanItem' === Vmodel) subVal.forEach(t => this.dataForm.tableFieldc87c94.push({...item,...t}));
			   this.initCollapse();
            });
            uni.$on('linkPageConfirm', (subVal, Vmodel) => {
				let item = {
						F_Title:undefined,
						F_Content:undefined,
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
					}
               if ('APuPlanLog' === Vmodel) subVal.forEach(t => this.dataForm.tableFielde82301.push({...item,...t}));
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
			gettableFieldc87c94_F_SupplierIdOptions(i){
				this.tableFieldc87c94_F_SupplierIdOptions = []
				let templateJson = []
				let query = {
					paramList: this.getParamList(templateJson, this.dataForm , i)
				}
				getDataInterfaceRes('2008457397298925568', query).then(res => {
					let data = res.data
					this.tableFieldc87c94_F_SupplierIdOptions = Array.isArray(data) ? data : []
				});
			},
			selfInit(){
				this.addAPuPlanItem();
				this.addAPuPlanLog();
			},
			resetForm() {
				this.dataForm.tableFieldc87c94 = [];
				this.dataForm.tableFielde82301 = [];
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
                        url: '/api/example/APuPlan/' + this.dataForm.id,
                        method: 'get',
                    }).then(res => {
            			  const idsToFetch = [];
                        this.dataForm = res.data;
            			this.tempGoodsSelector = []
            			this.dataForm.tableFieldc87c94.map(item=>{
            			this.tempGoodsSelector.push(item.F_CustomerId);
            			this.fillNumLevelsFromF_Num(item)
            			if (item.F_CustomerId) idsToFetch.push(item.F_CustomerId);
            			})
            			
            			     if (idsToFetch.length) {
            			          const uniqueIds = Array.from(new Set(idsToFetch));
            			          const interfaceId = '2008721559174385664';
            			          const query = {
            			            ids: uniqueIds,
            			            interfaceId,
            			            propsValue: 'F_Id',
            			            relationField: 'F_GoodsName',
            			            paramList: [],
            			          };
            			          getDataInterfaceDataInfoByIds(interfaceId, query)
            			            .then(resp => {
            			              const list = resp.data || [];
            			              const idKey = 'F_Id';
            			              const map = {};
            			              list.forEach((item) => {
            			                if (item && item[idKey] != null) map[String(item[idKey])] = item;
            			              });
                          const rows = this.dataForm.tableFieldc87c94;
            	              for (let i = 0; i < rows.length; i++) {
            	                const r = rows[i];
            			                if (!r) continue;
            			                const found = map[String(r.F_CustomerId)];
            			                if (found) {
            			                  // reuse existing helper to map fields into the row
            			                  this.handleCustomerSelectChange(found, r, i);
            			                  this.fillNumLevelsFromF_Num(r);
            			                }
            			              }
            			            })
            			            .catch(() => {
            			              // ignore errors - fallback is showing empty display fields
            			            });
            			        }
                    })
                }
            },
            submitForm(type) {
                this.$refs.dataForm.validate(valid => {
                    if (!valid) return
                    this.btnLoading = true
                    if (this.dataForm.id) {
                        request({
                            url: '/api/example/APuPlan/' + this.dataForm.id,
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
                            url: '/api/example/APuPlan',
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
			addAPuPlanItemRow() {
				let item = {
						F_CustomerId:undefined,
						F_SupplierId:undefined,
						F_Num:undefined,
						F_Price:undefined,
						F_Description:undefined,
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
					}
				this.dataForm.tableFieldc87c94.push(item)
				this.initCollapse()
			},
			removeAPuPlanItemRow(i,showConfirm=false) {
				const handleRemove = () => {
					this.dataForm.tableFieldc87c94.splice(i, 1)
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
			copyAPuPlanItemRow(i) {
				let item = JSON.parse(JSON.stringify(this.dataForm.tableFieldc87c94[i]));
				item.id = '';
				item.length && item.map(o => delete o.rowData);
				item.F_CreatorUserId = undefined;
				item.F_CreatorTime = undefined;
				this.dataForm.tableFieldc87c94.push(item);
			},
			addAPuPlanLogRow() {
				let item = {
						F_Title:undefined,
						F_Content:undefined,
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
					}
				this.dataForm.tableFielde82301.push(item)
				this.initCollapse()
			},
			removeAPuPlanLogRow(i,showConfirm=false) {
				const handleRemove = () => {
					this.dataForm.tableFielde82301.splice(i, 1)
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
			copyAPuPlanLogRow(i) {
				let item = JSON.parse(JSON.stringify(this.dataForm.tableFielde82301[i]));
				item.id = '';
				item.length && item.map(o => delete o.rowData);
				item.F_CreatorUserId = undefined;
				item.F_CreatorTime = undefined;
				this.dataForm.tableFielde82301.push(item);
			},
			handleGoods(ids,option){
				/* 兜底 */
				 if (!Array.isArray(this.dataForm.tableFieldc87c94)) {
				   this.dataForm.tableFieldc87c94 = [];
				 }
				 const optionIdSet = new Set(option.map(o => o.id));
								
				 /*  删掉已取消勾选的行 */
				 this.dataForm.tableFieldc87c94 = this.dataForm.tableFieldc87c94.filter(item => optionIdSet.has(item.F_WorkOrderId));
				 option.forEach(o => {
				   const exist = this.dataForm.tableFieldc87c94.some(item => item.F_WorkOrderId === o.F_Id);
					if (!exist) {
					let item = {
							F_CustomerId:o.F_Id,
							F_SupplierId:undefined,
							F_Num:undefined,
							F_Price:undefined,
							F_Description:undefined,
							F_CreatorUserId:undefined,
							F_CreatorTime:undefined,
							F_NumLevel1: undefined,
							F_NumLevel2: undefined,
							F_Unit_Ratio:o.F_Unit_Ratio
						}
					this.dataForm.tableFieldc87c94.push(item)
					
				   }
				 });
			},
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
				handleNumLevel1Change(val, index) {
				 const record = this.dataForm.tableFieldc87c94[index];
				 const levels = this.getUnitRatioLevels(record);
				 if (!levels) return;
				 const qty1 = Number(val) || 0;
				 const ratio = Number(levels.level2?.qty) || 0;
				 // 使用 $set 确保响应式更新
				 this.$set(record, 'F_NumLevel1', qty1);
				 this.$set(record, 'F_NumLevel2', qty1 * ratio);
				 this.$set(record, 'F_Num', qty1);
			   },
			 
			   // 回显时：F_Num 存的是第一框（一级数量），据此填充 F_NumLevel1 并计算 F_NumLevel2
			    fillNumLevelsFromF_Num(row) {
			     const levels = this.getUnitRatioLevels(row);
			     if (!levels || row.F_Num == null || row.F_Num === '') return;
			     const qty1 = Number(row.F_Num) || 0;
			     const ratio = Number(levels.level2?.qty) || 0;
			     row.F_NumLevel1 = qty1;
			     row.F_NumLevel2 = qty1 * ratio;
			   },
			 
			   // 计算单行金额：有两级单位时用一级数量（如箱）* 单价；否则用数量 * 单价
			    calculateRowAmount(record) {
			     const levels = this.getUnitRatioLevels(record);
			     const num = levels != null ? Number(record.F_NumLevel1) || 0 : Number(record.F_Num) || 0;
			     const price = Number(record.F_Price) || 0;
			     return num * price;
			   },
				
				   handleCustomerSelectChange(selected, record, rowIndex) {
				    const data = Array.isArray(selected) ? selected[0] : selected;
				    if (!data) {
				      record.F_GoodsNo = undefined;
				      record.F_Specification = undefined;
				      record.F_Image = undefined;
				      record.F_Unit_Ratio = undefined;
				      return;
				    }
				    // 尝试使用常见字段名进行赋值，若后端字段不同可根据实际字段调整
				    record.F_GoodsNo = data.F_GoodsNo ?? data.GoodsNo ?? data.F_GoodsName ?? data.name ?? record.F_GoodsNo;
				    record.F_Specification = data.F_Specification ?? data.Specification ?? data.spec ?? record.F_Specification;
				    record.F_Unit_Ratio = data.F_Unit_Ratio ?? record.F_Unit_Ratio;
				    // 处理图片字段：支持直接url、相对路径、数组或JSON字符串
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
				      // 如果已经是完整地址就直接用，否则加上 apiUrl 前缀
				      if (/^https?:\/\//i.test(String(imgVal))) {
				        record.F_Image = String(imgVal);
				      } else {
				        // 防止重复拼接
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



