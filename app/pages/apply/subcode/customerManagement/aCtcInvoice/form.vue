﻿﻿﻿﻿<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" :rules="rules" ref="dataForm" :errorType="['toast']" label-position="left" :label-width="100 * 1.5" label-align="right" class="jnpf-form">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='合同' prop="F_ContractId"  :label-width="100 * 1.5">
					<JnpfPopupSelect v-model='dataForm.F_ContractId' propsValue='F_Id' relationField='F_ContractCode' :columnOptions='F_ContractIdOptions' interfaceId='2010889611072638976' placeholder='请选择' :pageSize='20' popupType='dialog' popupTitle='选择数据' popupWidth='1000px' :templateJson='F_ContractIdTemplateJson' vModel="F_ContractId"  type="popup" :extraOptions='[]' @change="htChange"/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='开票金额(元)' prop="F_Money"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_Money" :step='1.0'  disabled/>
				</u-form-item>
			</view>
<!-- 			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='开票状态' prop="F_Status"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_Status" placeholder='请输入'  :showCount='false' />
				</u-form-item>
			</view> -->
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='申请附件' prop="F_ApplyFiles"  :label-width="100 * 1.5">
					<JnpfUploadFile v-model="dataForm.F_ApplyFiles" :limit='9' sizeUnit='MB' :fileSize='10' pathType="defaultPath"  :sortRule='[]' timeFormat='YYYY' buttonText='点击上传' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='申请备注' prop="F_ApplyRemark"  :label-width="100 * 1.5">
					<JnpfTextarea v-model="dataForm.F_ApplyRemark" placeholder='请输入'  :showCount='false' />
				</u-form-item>
			</view>
<!-- 			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='开票附件' prop="F_InvoiceFiles"  :label-width="100 * 1.5">
					<JnpfUploadFile v-model="dataForm.F_InvoiceFiles" :limit='9' sizeUnit='MB' :fileSize='10' pathType="defaultPath"  :sortRule='[]' timeFormat='YYYY' buttonText='点击上传' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='开票备注' prop="F_InvoiceRemark"  :label-width="100 * 1.5">
					<JnpfTextarea v-model="dataForm.F_InvoiceRemark" placeholder='请输入'  :showCount='false' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='申请人员' prop="F_ApplyUserId"  :label-width="100 * 1.5">
					<JnpfUserSelect v-model="dataForm.F_ApplyUserId" placeholder='请选择' selectType='all' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='申请时间' prop="F_ApplyTime"  :label-width="100 * 1.5">
					<JnpfDatePicker v-model="dataForm.F_ApplyTime" type='date' format='yyyy-MM-dd' placeholder='请选择' />
				</u-form-item>
			</view> -->
<!-- 			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='开票人员' prop="F_InvoiceUserId"  :label-width="100 * 1.5">
					<JnpfUserSelect v-model="dataForm.F_InvoiceUserId" placeholder='请选择' selectType='all' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='开票时间' prop="F_InvoiceTime"  :label-width="100 * 1.5">
					<JnpfDatePicker v-model="dataForm.F_InvoiceTime" type='date' format='yyyy-MM-dd' placeholder='请选择' />
				</u-form-item>
			</view> -->
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
					F_ContractId:undefined,
					F_Money:undefined,
					F_Status:"待开票",
					F_ApplyFiles:[],
					F_ApplyRemark:undefined,
					F_InvoiceFiles:[],
					F_InvoiceRemark:undefined,
					F_ApplyUserId:undefined,
					F_ApplyTime:undefined,
					F_InvoiceUserId:undefined,
					F_InvoiceTime:undefined,
					tableField2cca74:[],
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
				userInfo:{},
				jurisdictionType:'',
				addType: 0,
				F_ContractIdTemplateJson: [],
				F_ContractIdOptions: [
  {
    "value": "F_ContractCode",
    "label": "编号"
  },
  {
    "value": "F_PrepayAmount",
    "label": "预付款"
  },
  {
    "value": "F_SaleTotal",
    "label": "销售总数"
  },
  {
    "value": "F_ContractAmount",
    "label": "合同金额"
  },
  {
    "value": "F_IsTaxable",
    "label": "是否含税"
  },
  {
    "value": "F_AuditStatus",
    "label": "审核状态"
  },
  {
    "value": "F_Description",
    "label": "备注"
  },
  {
    "value": "F_CreatorTime",
    "label": "参加时间"
  }
],
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
            uni.$on('linkPageConfirm', (subVal, Vmodel) => {
				let item = {
						F_Title:undefined,
						F_Content:undefined,
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
					}
               if ('ACtcInvoiceLog' === Vmodel) subVal.forEach(t => this.dataForm.tableField2cca74.push({...item,...t}));
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
    conversionDateTime(type) {
      const format = type === 'yyyy' ? 'yyyy-01-01 00:00:00' : type === 'yyyy-MM' ? 'yyyy-MM-01 00:00:00' :
        type === 'yyyy-MM-dd' ? 'yyyy-MM-dd 00:00:00' : type === 'yyyy-MM-dd HH:mm' ? 'yyyy-MM-dd HH:mm:00' : 'yyyy-MM-dd HH:mm:ss'
      const dataTime = this.jnpf.toDate(new Date(), format)
      return new Date(dataTime).getTime()
    },
	initDefaultData() {
      this.dataForm.F_ApplyTime = this.conversionDateTime("yyyy-MM-dd");
      this.dataForm.F_ApplyUserId = this.userInfo.userId
	},
			selfInit(){
				this.addACtcInvoiceLog();
			},
			resetForm() {
				this.dataForm.tableField2cca74 = [];
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
                        url: '/api/example/ACtcInvoice/' + this.dataForm.id,
                        method: 'get',
                    }).then(res => {
                        this.dataForm = res.data;
						if(!this.dataForm.F_ApplyFiles)this.dataForm.F_ApplyFiles=[];
						if(!this.dataForm.F_InvoiceFiles)this.dataForm.F_InvoiceFiles=[];
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
                            url: '/api/example/ACtcInvoice/' + this.dataForm.id,
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
                            url: '/api/example/ACtcInvoice',
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
			addACtcInvoiceLogRow() {
				let item = {
						F_Title:undefined,
						F_Content:undefined,
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
					}
				this.dataForm.tableField2cca74.push(item)
				this.initCollapse()
			},
			removeACtcInvoiceLogRow(i,showConfirm=false) {
				const handleRemove = () => {
					this.dataForm.tableField2cca74.splice(i, 1)
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
			copyACtcInvoiceLogRow(i) {
				let item = JSON.parse(JSON.stringify(this.dataForm.tableField2cca74[i]));
				item.id = '';
				item.length && item.map(o => delete o.rowData);
				item.F_CreatorUserId = undefined;
				item.F_CreatorTime = undefined;
				this.dataForm.tableField2cca74.push(item);
			},
			// 合同改变
			htChange(e){
				console.log('合同改变',e);
				this.getCalculateContractAmountData(e)
			},
			// 获取合同开票金额
			getCalculateContractAmountData(id){
				request({
				    url: '/api/example/ACtcInvoice/CalculateContractAmount/' + id,
				    method: 'get',
				}).then(res => {
					this.dataForm.F_Money = res.data || 0
				})
			}
        }
    };
</script>
<style>
        page{
                background-color: #f0f2f6;
        }
</style>



