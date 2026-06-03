<!-- ﻿﻿﻿﻿﻿﻿﻿仓库--商品 -->
<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" :rules="rules" ref="dataForm" :errorType="['toast']" label-position="left" :label-width="100 * 1.5" label-align="right" class="jnpf-form">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='其他出库单号' prop="F_StockOutNo"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_StockOutNo" placeholder='请填写，忽略将自动生成'  :showCount='false' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='仓库' prop="F_WarehouseId"  :label-width="100 * 1.5">
					<JnpfCascader @change='changeWarehouseId' v-model="dataForm.F_WarehouseId" :options="f_WarehouseIdOptions" :props="f_WarehouseIdProps" placeholder='请选择' filterable clearable />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='出库类型' prop="F_StockOutType"  :label-width="100 * 1.5">
					<JnpfSelect v-model="dataForm.F_StockOutType" :options="f_StockOutTypeOptions" :props="f_StockOutTypeProps" placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='出库日期' prop="F_StockOutDate" required  :label-width="100 * 1.5">
					<JnpfDatePicker v-model="dataForm.F_StockOutDate" type='date' format='yyyy-MM-dd' placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='发货人' prop="F_StockOutUserId"  :label-width="100 * 1.5">
					<JnpfUserSelect v-model="dataForm.F_StockOutUserId" placeholder='请选择' selectType='all' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='加工单号' prop="F_ProcessNo"  :label-width="100 * 1.5">
					<JnpfSelect v-model="dataForm.F_ProcessNo" :options="f_ProcessNoOptions" :props="f_ProcessNoProps" placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="jnpf-table">
				<view class="jnpf-table-title u-line-1" >{{'商品管理'}}
				</view>
				<view class="u-p-l-20 u-p-r-20 form-item-box">
					<u-form-item label='批量商品选择'  :label-width="100 * 1.5">
						<JnpfPopupSelect multiple interfaceName='getPopSelect' v-model="tempGoodsSelector" propsValue='F_GoodsId' relationField='F_GoodsName' :columnOptions='tableField44e07d_F_CustomerIdOptions' interfaceId='2015036768293883904' placeholder='选择对应仓库后，再选择商品' :pageSize='20' popupType='dialog' popupTitle='选择数据' popupWidth='60%' :templateJson='tableField44e07d_F_CustomerIdTemplateJson' :vModel="'tableField44e07d-F_CustomerId'" type="popup" @change='handleGoods'/>
					</u-form-item>
				</view>
				<view class="jnpf-table-item" v-for="(item,i) in dataForm.tableField44e07d" :key="i">
					<view class="jnpf-table-item-title">
						<view class="jnpf-table-item-title-num">({{i+1}})</view>
						<view class="u-flex">
		        <view class="jnpf-table-delete-btn" @click="removeAPuOutStockOtherItemRow(i,true)">{{this.$t('common.delText','删除')}}</view>
						</view>
                    </view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='商品'  :label-width="100 * 1.5">
							<JnpfPopupSelect disabled v-model="dataForm.tableField44e07d[i].F_CustomerId" propsValue='F_GoodsId' relationField='F_GoodsName' :columnOptions='tableField44e07d_F_CustomerIdOptions' interfaceId='2015036768293883904' placeholder='请选择' hasPage :pageSize='20' popupType='dialog' popupTitle='选择数据' popupWidth='60%' :templateJson='tableField44e07d_F_CustomerIdTemplateJson' :vModel="'tableField44e07d-F_CustomerId'+i" type="popup"/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='数量'  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableField44e07d[i].F_Num" :precision='0' :step='1.0'  controls />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='成本单价(元)'  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableField44e07d[i].F_Price" :step='1.0'  />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='备注'  :label-width="100 * 1.5">
							<JnpfTextarea v-model="dataForm.tableField44e07d[i].F_Description" placeholder='请输入'  :showCount='false' />
						</u-form-item>
					</view>
                </view>
<!-- 				<view class="jnpf-table-footer-btn" >
					<view class="jnpf-table-btn jnpf-table-primary-btn" @click="addAPuOutStockOtherItemRow()">
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
	import { getDictionaryDataSelector,getDataInterfaceRes } from '@/api/common'
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
					F_StockOutNo:undefined,
					F_WarehouseId:[],
					F_StockOutType:undefined,
					F_StockOutDate:undefined,
					F_StockOutUserId:undefined,
					F_ProcessNo:undefined,
					F_CreatorUserId:undefined,
					F_CreatorTime:undefined,
					tableField44e07d:[],
					tableField211772:[],
                },
                rules: {
					F_StockOutDate:[
						{
							required:true,
							message:this.$t('common.inputText')+' '+'出库日期',
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
				f_ProcessNoProps:{'label':'F_ProcessNo','value':'F_Id'},
				f_ProcessNoOptions: [],
				f_StockOutTypeProps:{'label':'fullName','value':'enCode'},
				f_StockOutTypeOptions: [],
				tableField44e07d_F_CustomerIdTemplateJson: [
					{
					    "defaultValue": "",
					    "field": "id",
					    "dataType": "varchar",
					    "required": 0,
					    "fieldName": "",
					    "relationField": "2015023878744707072",
					    "jnpfKey": null,
					    "complexHeaderList": null,
					    "sourceType": 2,
					    "isChildren": false,
					    "IsMultiple": false
					}
				],
				tableField44e07d_F_CustomerIdOptions: [
   {
          value: 'F_GoodsName',
          label: '商品',
        },
        {
          value: 'F_GoodsNo',
          label: '商品编码',
        },
        {
          value: 'F_Num',
          label: '数量',
        },
],
tempGoodsSelector:[]
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
			this.getf_StockOutTypeOptions();
			this.getF_ProcessNoOptions();
            uni.$on('linkPageConfirm', (subVal, Vmodel) => {
				let item = {
						F_CustomerId:undefined,
						F_Num:undefined,
						F_Price:undefined,
						F_Description:undefined,
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
					}
               if ('APuOutStockOtherItem' === Vmodel) subVal.forEach(t => this.dataForm.tableField44e07d.push({...item,...t}));
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
               if ('APuOutStockOtherLog' === Vmodel) subVal.forEach(t => this.dataForm.tableField211772.push({...item,...t}));
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
			getf_StockOutTypeOptions(){
				getDictionaryDataSelector('2013096194263355392').then(res => {
					this.f_StockOutTypeOptions = res.data.list
				});
			},
    conversionDateTime(type) {
      const format = type === 'yyyy' ? 'yyyy-01-01 00:00:00' : type === 'yyyy-MM' ? 'yyyy-MM-01 00:00:00' :
        type === 'yyyy-MM-dd' ? 'yyyy-MM-dd 00:00:00' : type === 'yyyy-MM-dd HH:mm' ? 'yyyy-MM-dd HH:mm:00' : 'yyyy-MM-dd HH:mm:ss'
      const dataTime = this.jnpf.toDate(new Date(), format)
      return new Date(dataTime).getTime()
    },
	initDefaultData() {
      this.dataForm.F_StockOutDate = this.conversionDateTime("yyyy-MM-dd");
      this.dataForm.F_StockOutUserId = this.userInfo.userId
	},
			selfInit(){
				this.addAPuOutStockOtherItem();
				this.addAPuOutStockOtherLog();
			},
			resetForm() {
				this.dataForm.tableField44e07d = [];
				this.dataForm.tableField211772 = [];
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
                        url: '/api/example/APuOutStockOther/' + this.dataForm.id,
                        method: 'get',
                    }).then(res => {
                        this.dataForm = res.data;
						if(!this.dataForm.F_WarehouseId)this.dataForm.F_WarehouseId=[];
						this.tempGoodsSelector = []
						this.tableField44e07d_F_CustomerIdTemplateJson[0].relationField=this.dataForm.F_WarehouseId
						this.dataForm.tableField44e07d.map(item=>{
						this.tempGoodsSelector.push(item.F_CustomerId);
						})
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
                            url: '/api/example/APuOutStockOther/' + this.dataForm.id,
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
                            url: '/api/example/APuOutStockOther',
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
			addAPuOutStockOtherItemRow() {
				let item = {
						F_CustomerId:undefined,
						F_Num:undefined,
						F_Price:undefined,
						F_Description:undefined,
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
					}
				this.dataForm.tableField44e07d.push(item)
				this.initCollapse()
			},
			removeAPuOutStockOtherItemRow(i,showConfirm=false) {
				const handleRemove = () => {
					this.dataForm.tableField44e07d.splice(i, 1)
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
			copyAPuOutStockOtherItemRow(i) {
				let item = JSON.parse(JSON.stringify(this.dataForm.tableField44e07d[i]));
				item.id = '';
				item.length && item.map(o => delete o.rowData);
				item.F_CreatorUserId = undefined;
				item.F_CreatorTime = undefined;
				this.dataForm.tableField44e07d.push(item);
			},
			addAPuOutStockOtherLogRow() {
				let item = {
						F_Type:undefined,
						F_Title:undefined,
						F_Content:undefined,
						F_Reason:undefined,
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
					}
				this.dataForm.tableField211772.push(item)
				this.initCollapse()
			},
			removeAPuOutStockOtherLogRow(i,showConfirm=false) {
				const handleRemove = () => {
					this.dataForm.tableField211772.splice(i, 1)
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
			copyAPuOutStockOtherLogRow(i) {
				let item = JSON.parse(JSON.stringify(this.dataForm.tableField211772[i]));
				item.id = '';
				item.length && item.map(o => delete o.rowData);
				item.F_CreatorUserId = undefined;
				item.F_CreatorTime = undefined;
				this.dataForm.tableField211772.push(item);
			},
			changeWarehouseId(e){
				this.tableField44e07d_F_CustomerIdTemplateJson[0].relationField=e
			},
			handleGoods(ids,option){
				console.log(option,'option');
				/* 兜底 */
				 if (!Array.isArray(this.dataForm.tableField44e07d)) {
				   this.dataForm.tableField44e07d = [];
				 }
				 const optionIdSet = new Set(option.map(o => o.id));
								
				 /*  删掉已取消勾选的行 */
				 this.dataForm.tableField44e07d = this.dataForm.tableField44e07d.filter(item => optionIdSet.has(item.F_WorkOrderId));
				 option.forEach(o => {
				   const exist = this.dataForm.tableField44e07d.some(item => item.F_WorkOrderId === o.F_Id);
						
					if (!exist) {
				   let item = {
				   		F_CustomerId:o.F_GoodsId,
				   		F_Num:undefined,
				   		F_Price:undefined,
				   		F_SalesPrice:undefined,
				   		F_Type:undefined,
				   		F_Description:undefined,
				   		F_CreatorUserId:undefined,
				   		F_CreatorTime:undefined,
				   	}
				     this.dataForm.tableField44e07d.push(item);
				   }
				 });
			},
			   getF_ProcessNoOptions() {
			    let templateJson = []
			    let query = {
			      paramList: this.getParamList(templateJson, this.dataForm)
			    }
			    getDataInterfaceRes('2014981107073814528', query).then(res => {
			      let data = res.data;
			      this.f_ProcessNoOptions = Array.isArray(data) ? data : [];
			    });
			  }
        }
    };
</script>
<style>
        page{
                background-color: #f0f2f6;
        }
</style>



