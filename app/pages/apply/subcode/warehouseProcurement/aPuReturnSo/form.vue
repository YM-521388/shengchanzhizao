﻿﻿﻿﻿﻿<!-- 选择合同--商品列表-->
<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" :rules="rules" ref="dataForm" :errorType="['toast']" label-position="left" :label-width="100 * 1.5" label-align="right" class="jnpf-form">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='销售退货单号' prop="F_ReturnInNo"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_ReturnInNo" placeholder='请填写，忽略将自动生成'  :showCount='false' />
				</u-form-item>
			</view>
			<!-- <view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='仓库' prop="F_WarehouseId" required  :label-width="100 * 1.5">
					<JnpfCascader :disabled="Boolean(!!dataForm.id)" v-model="dataForm.F_WarehouseId" :options="f_WarehouseIdOptions" :props="f_WarehouseIdProps" placeholder='请选择' filterable clearable />
				</u-form-item>
			</view> -->
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='入库类型' prop="F_ReturnInType"  :label-width="100 * 1.5">
					<JnpfSelect   v-model="dataForm.F_ReturnInType" :options="f_ReturnInTypeOptions" :props="f_ReturnInTypeProps" placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='合同单号' prop="F_ContractId"  :label-width="100 * 1.5">
					<JnpfSelect  @change="handleContractChange" v-model="dataForm.F_ContractId" :options="f_ContractIdOptions" :props="f_ContractIdProps" placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='退货日期' prop="F_ReturnInDate"  :label-width="100 * 1.5">
					<JnpfDatePicker v-model="dataForm.F_ReturnInDate" type='date' format='yyyy-MM-dd' placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='客户' prop="F_CustomerId"  :label-width="100 * 1.5">
					<JnpfPopupSelect v-model='dataForm.F_CustomerId' propsValue='F_Id' relationField='F_CustomerName' :columnOptions='f_CustomerIdOptions' interfaceId='2009458830353764352' placeholder='请选择' hasPage :pageSize='20' popupType='dialog' popupTitle='选择数据' popupWidth='50%' :templateJson='f_CustomerIdTemplateJson' vModel="F_CustomerId"  type="popup" :extraOptions='[]' @change="customerChange"/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='联系人' prop="F_ContactId"  :label-width="100 * 1.5">
					<JnpfPopupSelect v-model='dataForm.F_ContactId' propsValue='F_Id' relationField='F_ContactName' :columnOptions='F_ContactIdOptions' interfaceId='2009459664370143232' placeholder='请选择' :pageSize='20' popupType='dialog' popupTitle='选择数据' popupWidth='800px' :templateJson='F_ContactIdTemplateJson' vModel="F_ContactId"  :formData='dataForm' type="popup" :extraOptions='[]'/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='地址' prop="F_AddressId"  :label-width="100 * 1.5">
					<JnpfPopupSelect v-model='dataForm.F_AddressId' propsValue='F_Id' relationField='F_Address' :columnOptions='F_AddressIdOptions' interfaceId='2009527238009163776' placeholder='请选择' :pageSize='20' popupType='dialog' popupTitle='选择数据' popupWidth='800px' :templateJson='F_AddressIdTemplateJson' vModel="F_AddressId"  :formData='dataForm' type="popup" :extraOptions='[]'/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='备注' prop="F_Description"  :label-width="100 * 1.5">
					<JnpfTextarea v-model="dataForm.F_Description" placeholder='请输入'  :showCount='false' />
				</u-form-item>
			</view>
			<view class="jnpf-table">
				<view class="jnpf-table-title u-line-1" >{{'商品'}}</view>
				<view class="u-p-l-20 u-p-r-20 form-item-box">
					<u-form-item label='添加商品'  :label-width="100 * 1.5" @click="addGoods" >
						<!-- <span @click="addGoods">添加</span> -->
						<!-- <JnpfPopupSelect interfaceName='getPopSelect' multiple v-model="tempGoodsSelector" propsValue='F_Id' relationField='F_GoodsName' :columnOptions='tableFieldecf5cb_F_CustomerIdOptions' interfaceId='2008721559174385664' placeholder='选择商品' hasPage :pageSize='20' popupType='dialog' popupTitle='选择数据' popupWidth='70%' :templateJson='tableFieldecf5cb_F_CustomerIdTemplateJson' :vModel="'tableFieldecf5cb-F_CustomerId'" type="popup"  @change="changeGoods" :extraOptions="[]"/> -->
					</u-form-item>
				</view>
				<view class="jnpf-table-item" v-for="(item,i) in dataForm.tableFieldcfb049" :key="i">
					<view class="jnpf-table-item-title">
						<view class="jnpf-table-item-title-num">({{i+1}})</view>
						<view class="u-flex">
		        <view class="jnpf-table-delete-btn" @click="removeAPuReturnSoItemRow(i,true)">{{this.$t('common.delText','删除')}}</view>
						</view>
                    </view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='商品'  :label-width="100 * 1.5">
							<JnpfPopupSelect disabled v-model="dataForm.tableFieldcfb049[i].F_CustomerId" propsValue='F_Id' relationField='F_GoodsName' :columnOptions='tableFieldcfb049_F_CustomerIdOptions' interfaceId='2008721559174385664' placeholder='请选择' :pageSize='20' popupType='dialog' popupTitle='选择数据' popupWidth='800px' :templateJson='tableFieldcfb049_F_CustomerIdTemplateJson' :vModel="'tableFieldcfb049-F_CustomerId'+i" type="popup"/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='退货数量'  :label-width="100 * 1.5">
							<!-- <JnpfInputNumber v-model="dataForm.tableFieldcfb049[i].F_Num" :precision='0' :step='1.0'  /> -->
							<view class="form-value" v-if="getUnitRatioLevels(dataForm.tableFieldcfb049[i])">
								<JnpfInputNumber :value="1" placeholder="1" :controls="false" :disabled="true" :style="{ width: '80px' }" />
								<text>{{ getUnitRatioLevels(dataForm.tableFieldcfb049[i])?.level1?.name }}</text>
								<JnpfInputNumber v-model="dataForm.tableFieldcfb049[i].F_Num" placeholder="请输入" :controls="false" :max="getUnitRatioLevels(dataForm.tableFieldcfb049[i])?.level2?.qty" :style="{ width: '80px' }" />
								<text>{{ getUnitRatioLevels(dataForm.tableFieldcfb049[i])?.level2?.name }}</text>
						 
							</view>
							<template v-else>
							  <JnpfInputNumber
								v-model="dataForm.tableFieldcfb049[i].F_Num"
								placeholder="请输入"
								:step="1.0"
								:controls="true"
								:style="{
								  width: '100%',
								}" />
							</template>
						</u-form-item>
					</view>
					<u-form-item label='仓库' prop="F_WarehouseID" required  :label-width="100 * 1.5">
						<JnpfCascader :disabled="Boolean(!!dataForm.id)" v-model="dataForm.tableFieldcfb049[i].F_WarehouseID" :options="f_WarehouseIdOptions" :props="f_WarehouseIdProps" placeholder='请选择' filterable clearable />
					</u-form-item>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='销售单价(元)'  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableFieldcfb049[i].F_Price" :step='1.0'  />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='备注'  :label-width="100 * 1.5">
							<JnpfTextarea v-model="dataForm.tableFieldcfb049[i].F_Description" placeholder='请输入'  :showCount='false' />
						</u-form-item>
					</view>
                </view>
<!-- 				<view class="jnpf-table-footer-btn" >
					<view class="jnpf-table-btn jnpf-table-primary-btn" @click="addAPuReturnSoItemRow()">
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
	import { getDictionaryDataSelector,getDataInterfaceRes,getContractItemsByContractId,getContractRelationById,getDataInterGoodsInventoryNo } from '@/api/common'
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
					F_ReturnInNo:undefined,
					F_WarehouseId:[],
					F_ReturnInType:undefined,
					F_ContractId:undefined,
					F_ReturnInDate:undefined,
					F_CustomerId:undefined,
					F_ContactId:undefined,
					F_AddressId:undefined,
					F_Description:undefined,
					F_CreatorUserId:undefined,
					F_CreatorTime:undefined,
					tableFieldcfb049:[],
					tableField0c9c23:[],
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
				f_ReturnInTypeProps:{'label':'fullName','value':'id'},
				f_ReturnInTypeOptions: [],
				f_ContractIdProps:{'label':'F_ContractCode','value':'F_Id'},
				f_ContractIdOptions: [],
				f_CustomerIdTemplateJson: [],
				f_CustomerIdOptions: [
  {
    "value": "F_CustomerName",
    "label": "客户姓名"
  },
  {
    "value": "F_CustomerCode",
    "label": "客户编号"
  }
],
				F_ContactIdTemplateJson: [
  {
    "defaultValue": "",
    "field": "Id",
    "dataType": "varchar",
    "required": 0,
    "fieldName": "",
    "relationField": "2009181616060108800",
    "jnpfKey": null,
    "complexHeaderList": null,
    "sourceType": 2,
    "isChildren": false,
    "IsMultiple": false
  }
],
				F_ContactIdOptions: [
  {
    "value": "F_ContactName",
    "label": "姓名"
  },
  {
    "value": "F_ContactPhone",
    "label": "电话"
  }
],
				F_AddressIdTemplateJson: [
  {
    "defaultValue": "",
    "field": "id",
    "dataType": "varchar",
    "required": 0,
    "fieldName": "",
    "relationField": "",
    "jnpfKey": null,
    "complexHeaderList": null,
    "sourceType": 2,
    "isChildren": false,
    "IsMultiple": false
  }
],
				F_AddressIdOptions: [
  {
    "value": "F_Address",
    "label": "地址"
  }
],
				tableFieldcfb049_F_CustomerIdTemplateJson: [],
				tableFieldcfb049_F_CustomerIdOptions: [
{
    "value": "F_GoodsName",
    "label": "商品名称"
  },
  {
    "value": "F_GoodsNo",
    "label": "商品编码"
  },
  // {
  //   "value": "F_Image",
  //   "label": "商品图片"
  // },
  {
    "value": "F_Unit",
    "label": "单位"
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
selectedtableFieldcfb049RowKeys:[]
            };
        },
		watch: {
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
			this.getf_ReturnInTypeOptions();
			this.getf_ContractIdOptions();
            uni.$on('linkPageConfirm', (subVal, Vmodel) => {
				let item = {
						F_CustomerId:undefined,
						F_Num:undefined,
						F_Price:undefined,
						F_Description:undefined,
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
					}
               if ('APuReturnSoItem' === Vmodel) subVal.forEach(t => this.dataForm.tableFieldcfb049.push({...item,...t}));
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
               if ('APuReturnSoLog' === Vmodel) subVal.forEach(t => this.dataForm.tableField0c9c23.push({...item,...t}));
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
			getf_ReturnInTypeOptions(){
				getDictionaryDataSelector('2012074650393251840').then(res => {
					this.f_ReturnInTypeOptions = res.data.list
				});
			},
			getf_ContractIdOptions(){
				this.f_ContractIdOptions = []
				let templateJson = []
				let query = {
					paramList: this.getParamList(templateJson, this.dataForm)
				}
				getDataInterfaceRes('2010889611072638976', query).then(res => {
					let data = res.data
					this.f_ContractIdOptions = Array.isArray(data) ? data : []
				});
			},
    conversionDateTime(type) {
      const format = type === 'yyyy' ? 'yyyy-01-01 00:00:00' : type === 'yyyy-MM' ? 'yyyy-MM-01 00:00:00' :
        type === 'yyyy-MM-dd' ? 'yyyy-MM-dd 00:00:00' : type === 'yyyy-MM-dd HH:mm' ? 'yyyy-MM-dd HH:mm:00' : 'yyyy-MM-dd HH:mm:ss'
      const dataTime = this.jnpf.toDate(new Date(), format)
      return new Date(dataTime).getTime()
    },
	initDefaultData() {
      this.dataForm.F_ReturnInDate = this.conversionDateTime("yyyy-MM-dd");
	},
			selfInit(){
				this.addAPuReturnSoItem();
				this.addAPuReturnSoLog();
			},
			resetForm() {
				this.dataForm.tableFieldcfb049 = [];
				this.dataForm.tableField0c9c23 = [];
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
                        url: '/api/example/APuReturnSo/' + this.dataForm.id,
                        method: 'get',
                    }).then(res => {
                        this.dataForm = res.data;
						if(!this.dataForm.F_WarehouseId)this.dataForm.F_WarehouseId=[];
						this.F_ContactIdTemplateJson[0].relationField=this.dataForm.F_CustomerId
						this.F_AddressIdTemplateJson[0].relationField=this.dataForm.F_CustomerId
                    })
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
					if (!this.validateSortCode(this.dataForm.tableFieldcfb049))  return;
                    this.btnLoading = true
                    if (this.dataForm.id) {
                        request({
                            url: '/api/example/APuReturnSo/' + this.dataForm.id,
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
                            url: '/api/example/APuReturnSo',
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
			addAPuReturnSoItemRow() {
				let item = {
						F_CustomerId:undefined,
						F_Num:undefined,
						F_Price:undefined,
						F_Description:undefined,
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
					}
				this.dataForm.tableFieldcfb049.push(item)
				this.initCollapse()
			},
			removeAPuReturnSoItemRow(i,showConfirm=false) {
				const handleRemove = () => {
					this.dataForm.tableFieldcfb049.splice(i, 1)
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
			copyAPuReturnSoItemRow(i) {
				let item = JSON.parse(JSON.stringify(this.dataForm.tableFieldcfb049[i]));
				item.id = '';
				item.length && item.map(o => delete o.rowData);
				item.F_CreatorUserId = undefined;
				item.F_CreatorTime = undefined;
				this.dataForm.tableFieldcfb049.push(item);
			},
			addAPuReturnSoLogRow() {
				let item = {
						F_Type:undefined,
						F_Title:undefined,
						F_Content:undefined,
						F_Reason:undefined,
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
					}
				this.dataForm.tableField0c9c23.push(item)
				this.initCollapse()
			},
			removeAPuReturnSoLogRow(i,showConfirm=false) {
				const handleRemove = () => {
					this.dataForm.tableField0c9c23.splice(i, 1)
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
			copyAPuReturnSoLogRow(i) {
				let item = JSON.parse(JSON.stringify(this.dataForm.tableField0c9c23[i]));
				item.id = '';
				item.length && item.map(o => delete o.rowData);
				item.F_CreatorUserId = undefined;
				item.F_CreatorTime = undefined;
				this.dataForm.tableField0c9c23.push(item);
			},
			customerChange(e){
				this.F_ContactIdTemplateJson[0].relationField = e
				this.F_AddressIdTemplateJson[0].relationField=e
			},
			handleContractChange(selected ){
				// 先清空现有商品行
				   // this.dataForm.tableFieldcfb049 = [];
				   this.selectedtableFieldcfb049RowKeys = [];
								 
				   // 清空关联字段（以防选择为空或响应为空）
				   this.dataForm.F_CustomerId = undefined;
				   this.dataForm.F_ContactId = undefined;
				   this.dataForm.F_AddressId = undefined;
								 
				   if (!selected) {
				     return;
				   }
				   // this.getContractItemsByContract(selected)
				   this.getContractRelation(selected)
				   
			},
			 getContractItemsByContract(selected){
				  
				  // 获取合同明细项并填充子表
				  // let item = {
				  // 		F_CustomerId:undefined,
				  // 		F_Num:undefined,
				  // 		F_Price:undefined,
				  // 		F_Description:undefined,
				  // 		F_CreatorUserId:undefined,
				  // 		F_CreatorTime:undefined,
				  // 	}
				  // this.dataForm.tableFieldcfb049.push(item)
				    getContractItemsByContractId(String(selected))
				      .then(res => {
				        const list = (res && res.data) || [];
				        if (!Array.isArray(list) || list.length === 0) return;
				        list.forEach((item) => {
				          const row = {
				            F_CustomerId: item.F_CustomerId ?? undefined,
				            F_Num: item.F_SaleQty ?? undefined,
				            F_Price: item.F_Price ?? undefined,
				            F_Description: undefined,
				            F_CreatorUserId: undefined,
				            F_CreatorTime: undefined,
				          };
				          this.dataForm.tableFieldcfb049.push(row);
				          // 尝试填充展示字段（商品编号/规格）
				          try {
				            fillRowDisplayFields(row);
				          } catch (e) {
				            // ignore
				          }
				        });
				      })
				      .catch(err => {
				        console.error('getContractItemsByContractId error', err);
				      });
			},
			 getContractRelation(selected){
				 // 同时调用后端接口获取合同关联（客户/联系人/地址）并填写到表单
				    getContractRelationById(String(selected))
				      .then(res => {
				        const data = (res && res.data) || {};
				        // 只在返回值存在时赋值，保持未返回字段为 undefined
				        if (data.F_CustomerId !== undefined) this.dataForm.F_CustomerId = data.F_CustomerId;
				        if (data.F_ContactId !== undefined) this.dataForm.F_ContactId = data.F_ContactId;
				        if (data.F_AddressId !== undefined) this.dataForm.F_AddressId = data.F_AddressId;
						this.F_ContactIdTemplateJson[0].relationField =  data.F_CustomerId
						this.F_AddressIdTemplateJson[0].relationField= data.F_CustomerId
				      })
				      .catch(err => {
				        console.error('getContractRelationById error', err);
				      });
			},
			  /**
			   * 加载已有行时填充展示字段（优先使用缓存的 extraOptions，否则按 id 拉取）
			   */
			   fillRowDisplayFields(record) {
			    try {
			      const id = record.F_CustomerId;
			      if (!id) return;
			      const opts = this.optionsObj.tableFieldcfb049_F_CustomerIdOptions || [];
			      const found = opts.find((o) => o.F_Id === id || o.id === id || o.value === id);
			      if (found && Object.keys(found).length) {
			        record.F_GoodsNo = found.F_GoodsNo ?? found.GoodsNo ?? record.F_GoodsNo;
			        record.F_Specification = found.F_Specification ?? found.Specification ?? record.F_Specification;
			        return;
			      }
			      // 回退：按 id 请求接口获取完整数据（interfaceId 与弹窗一致）
			      try {
			        const query = { paramList: [{ defaultValue: id, field: 'Id', dataType: 'varchar' }] };
			        getDataInterfaceRes('2008721559174385664', query).then((res) => {
			          const data = Array.isArray(res.data) ? res.data[0] : res.data;
			          if (!data) return;
			          record.F_GoodsNo = data.F_GoodsNo ?? data.GoodsNo ?? record.F_GoodsNo;
			          record.F_Specification = data.F_Specification ?? data.Specification ?? record.F_Specification;
			        });
			      } catch (e) {
			        // ignore
			      }
			    } catch (e) {
			      // ignore
			    }
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
			  				this.dataForm.tableFieldcfb049.push(temp);
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
			  // 初始化数量级别：新增时计算默认值，回显时使用 F_Num 的值
			  // F_NumLevel1 固定为1（因为总是1箱）
			   fillNumLevelsFromF_Num(row) {
			    const levels = getUnitRatioLevels(row);
			    if (!levels) return;
			    // 数量大单位默认为1且不可编辑
			    row.F_NumLevel1 = 1;
			    // 如果 F_Num 有值（回显情况），使用 F_Num 的值；否则（新增情况）根据比例计算默认值
			    if (row.F_Num !== undefined && row.F_Num !== null) {
			      // 回显：F_Num 存的是 F_NumLevel2 的值
			      row.F_NumLevel2 = row.F_Num;
			    } else {
			      // 新增：根据比例计算默认值 = 1 * level2.qty
			      const ratio = Number(levels.level2?.qty) || 0;
			      row.F_NumLevel2 = 1 * ratio;
			    }
			  }
			// ======================计算===================end
        }
    };
</script>
<style>
        page{
                background-color: #f0f2f6;
        }
		
		.form-value{
			width: 100%;
			display: flex;
			justify-content: flex-end;
		}
</style>



