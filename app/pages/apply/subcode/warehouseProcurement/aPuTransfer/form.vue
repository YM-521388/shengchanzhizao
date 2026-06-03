﻿﻿﻿﻿<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" :rules="rules" ref="dataForm" :errorType="['toast']" label-position="left" :label-width="100 * 1.5" label-align="right" class="jnpf-form">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='调拨日期' prop="F_TransferDate" required  :label-width="100 * 1.5">
					<JnpfDatePicker v-model="dataForm.F_TransferDate" type='date' format='yyyy-MM-dd' placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='调拨人' prop="F_TransferUserId"  :label-width="100 * 1.5">
					<JnpfUserSelect v-model="dataForm.F_TransferUserId" placeholder='请选择' selectType='all' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='发出仓库' prop="F_FromWarehouseId" required  :label-width="100 * 1.5">
					<JnpfCascader @change='changeWarehouseId' v-model="dataForm.F_FromWarehouseId" :options="f_FromWarehouseIdOptions" :props="f_FromWarehouseIdProps" placeholder='请选择'  :disabled="Boolean(dataForm.id)" clearable />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='收入仓库' prop="F_ToWarehouseId" required  :label-width="100 * 1.5">
					<JnpfCascader v-model="dataForm.F_ToWarehouseId" :options="filteredToWarehouseOptions" :props="f_ToWarehouseIdProps" placeholder='请选择' :disabled="Boolean(dataForm.id)" filterable clearable />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='备注' prop="F_Description"  :label-width="100 * 1.5">
					<JnpfTextarea v-model="dataForm.F_Description" placeholder='请输入'  :showCount='false' />
				</u-form-item>
			</view>
			<view class="jnpf-table">
				<view class="jnpf-table-title u-line-1" >{{'选择商品'}}
				</view>
				<view class="u-p-l-20 u-p-r-20 form-item-box" >
					<u-form-item label='商品选择'  :label-width="100 * 1.5">
						<JnpfPopupSelect multiple interfaceName='getPopSelect'  v-model="tempGoodsSelector" propsValue='F_GoodsId' relationField='F_GoodsName' :columnOptions='tableFieldaf2f04_F_CustomerIdOptions' interfaceId='2015038141710340096' placeholder='选择对应仓库后，再选择商品' :pageSize='20' popupType='dialog' popupTitle='选择数据' popupWidth='60%' :templateJson='tableFieldaf2f04_F_CustomerIdTemplateJson' :vModel="'tableFieldaf2f04-F_CustomerId'" type="popup" @change='handleGoods'/>
					</u-form-item>
				</view>
				<view class="jnpf-table-item" v-for="(item,i) in dataForm.tableFieldaf2f04" :key="i">
					<view class="jnpf-table-item-title">
						<view class="jnpf-table-item-title-num">({{i+1}})</view>
						<view class="u-flex">
		        <view class="jnpf-table-delete-btn" @click="removeAPuTransferItemRow(i,true)">{{this.$t('common.delText','删除')}}</view>
						</view>
                    </view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='商品'  :label-width="100 * 1.5">
							<JnpfPopupSelect  disabled v-model="dataForm.tableFieldaf2f04[i].F_CustomerId" propsValue='F_GoodsId' relationField='F_GoodsName' :columnOptions='tableFieldaf2f04_F_CustomerIdOptions' interfaceId='2015038141710340096' placeholder='请选择' hasPage :pageSize='20' popupType='dialog' popupTitle='选择数据' popupWidth='60%' :templateJson='tableFieldaf2f04_F_CustomerIdTemplateJson' :formData='dataForm' :rowIndex='i' :vModel="'tableFieldaf2f04-F_CustomerId'+i" type="popup"/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='数量'  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableFieldaf2f04[i].F_Num" :precision='0' :step='1.0'  />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='成本单价(元)'  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableFieldaf2f04[i].F_Price" :step='1.0'  />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='销售单价(元)'  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableFieldaf2f04[i].F_SalesPrice" :step='1.0'  />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='备注'  :label-width="100 * 1.5">
							<JnpfTextarea v-model="dataForm.tableFieldaf2f04[i].F_Description" placeholder='请输入'  :showCount='false' />
						</u-form-item>
					</view>
                </view>
<!-- 				<view class="jnpf-table-footer-btn" >
					<view class="jnpf-table-btn jnpf-table-primary-btn" @click="addAPuTransferItemRow()">
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
		computed:{
				filteredToWarehouseOptions(){
					 // 确保选项数组存在，默认为空数组
					    const options = this.f_ToWarehouseIdOptions ?? [];
					    const fromWarehouseId = this.dataForm?.F_FromWarehouseId;
					    // 如果没有选择发出仓库，返回全部选项
					    if (!fromWarehouseId) return options;
					
					    // 安全获取 value 字段名
					    const valueKey = this.f_ToWarehouseIdProps?.F_Id ?? 'F_Id';
					
					    // 过滤掉与发出仓库相同的选项
					    return options.filter(option => option?.[valueKey] !== fromWarehouseId);
				},
			
		},
        data() {
            return {
				keyCode : +new Date(),
                btnLoading: false,
				addTableConf:{
				},
                dataForm: {
                    id:'',
					F_TransferDate:undefined,
					F_TransferUserId:undefined,
					F_FromWarehouseId:undefined,
					F_ToWarehouseId:undefined,
					F_Description:undefined,
					F_CreatorUserId:undefined,
					F_CreatorTime:undefined,
					tableFieldaf2f04:[],
					tableFieldc39d26:[],
                },
                rules: {
					F_TransferDate:[
						{
							required:true,
							message:this.$t('common.chooseText')+' '+'调拨日期',
							trigger:'change',
							type:'number'
						},
					],
					F_FromWarehouseId:[
						{
							required:true,
							message:this.$t('common.chooseText')+' '+'发出仓库',
							trigger:'change',
							type:'array'
						},
					],
					F_ToWarehouseId:[
						{
							required:true,
							message:this.$t('common.chooseText')+' '+'收入仓库',
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
				f_FromWarehouseIdProps:{'label':'F_Name','value':'F_Id',"children": "children"},
				f_FromWarehouseIdOptions: [],
				f_ToWarehouseIdProps:{'label':'F_Name','value':'F_Id',"children": "children"},
				f_ToWarehouseIdOptions: [],
				tableFieldaf2f04_F_CustomerIdTemplateJson: [
{
  "defaultValue": "",
  "field": "warehouseId",
  "dataType": "varchar",
  "required": 0,
  "fieldName": "",
  "relationField": "F_FromWarehouseId",
  "jnpfKey": "select",
  "complexHeaderList": null,
  "sourceType": 1,
  "isChildren": false,
  "IsMultiple": false
},
],

				tableFieldaf2f04_F_CustomerIdOptions: [
  {
    "value": "F_GoodsName",
    "label": "商品名称 "
  },
  {
    "value": "F_GoodsNo",
    "label": "商品编号"
  },
  {
    "value": "F_Specification",
    "label": "规格"
  },
  {
    "value": "F_CategoryId",
    "label": "商品类别"
  },
  {
    "value": "F_Num",
    "label": "数量"
  },
  {
    "value": "F_Price",
    "label": "价格"
  }
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
			this.getf_FromWarehouseIdOptions();
			this.getf_ToWarehouseIdOptions();
            uni.$on('linkPageConfirm', (subVal, Vmodel) => {
				let item = {
						F_CustomerId:undefined,
						F_Num:undefined,
						F_Price:undefined,
						F_SalesPrice:undefined,
						F_Description:undefined,
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
						F_CustomerIdOptions:[],
					}
               if ('APuTransferItem' === Vmodel) subVal.forEach(t => this.dataForm.tableFieldaf2f04.push({...item,...t}));
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
               if ('APuTransferLog' === Vmodel) subVal.forEach(t => this.dataForm.tableFieldc39d26.push({...item,...t}));
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
			getf_FromWarehouseIdOptions(){
				this.f_FromWarehouseIdOptions = []
				let templateJson = []
				let query = {
					paramList: this.getParamList(templateJson, this.dataForm)
				}
				getDataInterfaceRes('2021045201115680768', query).then(res => {
					let data = res.data
					this.f_FromWarehouseIdOptions = Array.isArray(data) ? data : []
				});
			},
			getf_ToWarehouseIdOptions(){
				this.f_ToWarehouseIdOptions = []
				let templateJson = []
				let query = {
					paramList: this.getParamList(templateJson, this.dataForm)
				}
				getDataInterfaceRes('2021045201115680768', query).then(res => {
					let data = res.data
					this.f_ToWarehouseIdOptions = Array.isArray(data) ? data : []
				});
			},
    conversionDateTime(type) {
      const format = type === 'yyyy' ? 'yyyy-01-01 00:00:00' : type === 'yyyy-MM' ? 'yyyy-MM-01 00:00:00' :
        type === 'yyyy-MM-dd' ? 'yyyy-MM-dd 00:00:00' : type === 'yyyy-MM-dd HH:mm' ? 'yyyy-MM-dd HH:mm:00' : 'yyyy-MM-dd HH:mm:ss'
      const dataTime = this.jnpf.toDate(new Date(), format)
      return new Date(dataTime).getTime()
    },
	initDefaultData() {
      this.dataForm.F_TransferDate = this.conversionDateTime("yyyy-MM-dd");
      this.dataForm.F_TransferUserId = this.userInfo.userId
	},
			selfInit(){
				this.addAPuTransferItem();
				this.addAPuTransferLog();
			},
			resetForm() {
				this.dataForm.tableFieldaf2f04 = [];
				this.dataForm.tableFieldc39d26 = [];
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
				this.tempGoodsSelector = []
                if (this.dataForm.id) {
                    request({
                        url: '/api/example/APuTransfer/' + this.dataForm.id,
                        method: 'get',
                    }).then(res => {
                        this.dataForm = res.data;
						if(!this.dataForm.F_FromWarehouseId)this.dataForm.F_FromWarehouseId=[];
						if(!this.dataForm.F_ToWarehouseId)this.dataForm.F_ToWarehouseId=[];
						this.tableFieldaf2f04_F_CustomerIdTemplateJson[0].defaultValue=this.dataForm.F_WarehouseId
						this.dataForm.tableFieldaf2f04.map(item=>{
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
                            url: '/api/example/APuTransfer/' + this.dataForm.id,
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
                            url: '/api/example/APuTransfer',
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
			addAPuTransferItemRow() {
				let item = {
						F_CustomerId:undefined,
						F_Num:undefined,
						F_Price:undefined,
						F_SalesPrice:undefined,
						F_Description:undefined,
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
						F_CustomerIdOptions:[],
					}
				this.dataForm.tableFieldaf2f04.push(item)
				this.initCollapse()
			},
			removeAPuTransferItemRow(i,showConfirm=false) {
				const handleRemove = () => {
					this.dataForm.tableFieldaf2f04.splice(i, 1)
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
			copyAPuTransferItemRow(i) {
				let item = JSON.parse(JSON.stringify(this.dataForm.tableFieldaf2f04[i]));
				item.id = '';
				item.length && item.map(o => delete o.rowData);
				item.F_CreatorUserId = undefined;
				item.F_CreatorTime = undefined;
				this.dataForm.tableFieldaf2f04.push(item);
			},
			addAPuTransferLogRow() {
				let item = {
						F_Type:undefined,
						F_Title:undefined,
						F_Content:undefined,
						F_Reason:undefined,
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
					}
				this.dataForm.tableFieldc39d26.push(item)
				this.initCollapse()
			},
			removeAPuTransferLogRow(i,showConfirm=false) {
				const handleRemove = () => {
					this.dataForm.tableFieldc39d26.splice(i, 1)
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
			copyAPuTransferLogRow(i) {
				let item = JSON.parse(JSON.stringify(this.dataForm.tableFieldc39d26[i]));
				item.id = '';
				item.length && item.map(o => delete o.rowData);
				item.F_CreatorUserId = undefined;
				item.F_CreatorTime = undefined;
				this.dataForm.tableFieldc39d26.push(item);
			},
			changeWarehouseId(e){
				this.tableFieldaf2f04_F_CustomerIdTemplateJson[0].defaultValue=e
			},
			handleGoods(ids,option){
				/* 兜底 */
				 if (!Array.isArray(this.dataForm.tableFieldaf2f04)) {
				   this.dataForm.tableFieldaf2f04 = [];
				 }
				 const optionIdSet = new Set(option.map(o => o.id));
								
				 /*  删掉已取消勾选的行 */
				 this.dataForm.tableFieldaf2f04 = this.dataForm.tableFieldaf2f04.filter(item => optionIdSet.has(item.F_WorkOrderId));
				 option.forEach(o => {
				   const exist = this.dataForm.tableFieldaf2f04.some(item => item.F_WorkOrderId === o.F_GoodsId);
					if (!exist) {
						let item = {
								F_CustomerId:o.F_GoodsId,
								F_Num:undefined,
								F_Price:undefined,
								F_SalesPrice:undefined,
								F_Description:undefined,
								F_CreatorUserId:undefined,
								F_CreatorTime:undefined,
								F_CustomerIdOptions:[],
							}
						this.dataForm.tableFieldaf2f04.push(item)
				   }
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



