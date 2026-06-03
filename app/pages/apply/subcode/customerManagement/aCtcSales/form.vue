﻿﻿﻿﻿<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" :rules="rules" ref="dataForm" :errorType="['toast']" label-position="left" :label-width="100 * 1.5" label-align="right" class="jnpf-form">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='客户' prop="F_CustomerId" required  :label-width="100 * 1.5">
					<JnpfSelect v-model="dataForm.F_CustomerId" :options="f_CustomerIdOptions" :props="f_CustomerIdProps" placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='合同' prop="F_ContractId"  :label-width="100 * 1.5">
					<JnpfPopupSelect v-model='dataForm.F_ContractId' propsValue='F_Id' relationField='F_ContractCode' :columnOptions='F_ContractIdOptions' interfaceId='2010894701401608192' placeholder='请选择' hasPage :pageSize='20' popupType='dialog' popupTitle='选择数据' popupWidth='60%' :templateJson='F_ContractIdTemplateJson' vModel="F_ContractId"  :formData='dataForm' type="popup" :extraOptions='[]'/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='支出日期' prop="F_ExpendDate" required  :label-width="100 * 1.5">
					<JnpfDatePicker v-model="dataForm.F_ExpendDate" type='date' format='yyyy-MM-dd' placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='支出类别' prop="F_ExpendType" required  :label-width="100 * 1.5">
					<JnpfSelect v-model="dataForm.F_ExpendType" :options="f_ExpendTypeOptions" :props="f_ExpendTypeProps" placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='金额' prop="F_Money" required  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_Money" :step='1.0'  />
				</u-form-item>
			</view>
<!-- 			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='审核状态' prop="F_AuditStatus"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_AuditStatus" placeholder='请输入'  :showCount='false' />
				</u-form-item>
			</view> -->
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='附件' prop="F_Files"  :label-width="100 * 1.5">
					<JnpfUploadFile v-model="dataForm.F_Files" :limit='9' sizeUnit='MB' :fileSize='10' pathType="defaultPath"  :sortRule='[]' timeFormat='YYYY' buttonText='点击上传' />
				</u-form-item>
			</view>
<!-- 			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='结算状态' prop="F_SettleStatus"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_SettleStatus" placeholder='请输入'  :showCount='false' />
				</u-form-item>
			</view> -->
			<view class="u-p-l-20 u-p-r-20 form-item-box" v-if="false">
				<u-form-item label='结算附件' prop="F_SettleFiles"  :label-width="100 * 1.5">
					<JnpfUploadFile v-model="dataForm.F_SettleFiles" :limit='9' sizeUnit='MB' :fileSize='10' pathType="defaultPath"  :sortRule='[]' timeFormat='YYYY' buttonText='点击上传' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box" v-if="false">
				<u-form-item label='结算备注' prop="F_SettleDesc"  :label-width="100 * 1.5">
					<JnpfTextarea v-model="dataForm.F_SettleDesc" placeholder='请输入'  :showCount='false' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='备注' prop="F_Description"  :label-width="100 * 1.5">
					<JnpfTextarea v-model="dataForm.F_Description" placeholder='请输入'  :showCount='false' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='创建人员' prop="F_CreatorUserId" v-if='false'  :label-width="100 * 1.5">
					<JnpfOpenData v-model="dataForm.F_CreatorUserId" type='currUser' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='创建时间' prop="F_CreatorTime" v-if='false'  :label-width="100 * 1.5">
					<JnpfOpenData v-model="dataForm.F_CreatorTime" type='currTime' />
				</u-form-item>
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
		  computed: {
		    F_ContractIdTemplateJson() {
		      return [
		        {
		          defaultValue: '',
		          field: 'id',
		          dataType: 'varchar',
		          required: 0,
		          fieldName: '',
		          relationField: this.dataForm.F_CustomerId || '', // 注意这里要用 this
		          jnpfKey: null,
		          complexHeaderList: null,
		          sourceType: 2,
		          isChildren: false,
		          IsMultiple: false,
		        },
		      ];
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
					F_CustomerId:undefined,
					F_ContractId:undefined,
					F_ExpendDate:undefined,
					F_ExpendType:undefined,
					F_Money:undefined,
					F_AuditStatus:undefined,
					F_Files:[],
					F_SettleStatus:undefined,
					F_SettleFiles:[],
					F_SettleDesc:undefined,
					F_Description:undefined,
					F_CreatorUserId:undefined,
					F_CreatorTime:undefined,
                },
                rules: {
					F_CustomerId:[
						{
							required:true,
							message:this.$t('common.inputText')+' '+'客户',
							trigger:'change',
						},
					],
					F_ExpendDate:[
						{
							required:true,
							message:this.$t('common.inputText')+' '+'支出日期',
							trigger:'change',
							type:'number'
						},
					],
					F_ExpendType:[
						{
							required:true,
							message:this.$t('common.inputText')+' '+'支出类别',
							trigger:'change',
						},
					],
					F_Money:[
						{
							required:true,
							message:this.$t('common.inputText')+' '+'金额',
							trigger:[
  "blur",
  "change"
],
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
				f_CustomerIdProps:{'label':'F_CustomerName','value':'F_Id'},
				f_CustomerIdOptions: [],

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
				f_ExpendTypeProps:{'label':'fullName','value':'enCode'},
				f_ExpendTypeOptions: [],
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
			this.getf_CustomerIdOptions();
			this.getf_ExpendTypeOptions();
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
			getf_CustomerIdOptions(){
				this.f_CustomerIdOptions = []
				let templateJson = []
				let query = {
					paramList: this.getParamList(templateJson, this.dataForm)
				}
				getDataInterfaceRes('2009458830353764352', query).then(res => {
					let data = res.data
					this.f_CustomerIdOptions = Array.isArray(data) ? data : []
				});
			},
			getf_ExpendTypeOptions(){
				getDictionaryDataSelector('2010895416983425024').then(res => {
					this.f_ExpendTypeOptions = res.data.list
				});
			},
    conversionDateTime(type) {
      const format = type === 'yyyy' ? 'yyyy-01-01 00:00:00' : type === 'yyyy-MM' ? 'yyyy-MM-01 00:00:00' :
        type === 'yyyy-MM-dd' ? 'yyyy-MM-dd 00:00:00' : type === 'yyyy-MM-dd HH:mm' ? 'yyyy-MM-dd HH:mm:00' : 'yyyy-MM-dd HH:mm:ss'
      const dataTime = this.jnpf.toDate(new Date(), format)
      return new Date(dataTime).getTime()
    },
	initDefaultData() {
      this.dataForm.F_ExpendDate = this.conversionDateTime("yyyy-MM-dd");
	},
			selfInit(){
			},
			resetForm() {
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
                        url: '/api/example/ACtcSales/' + this.dataForm.id,
                        method: 'get',
                    }).then(res => {
                        this.dataForm = res.data;
						if(!this.dataForm.F_Files)this.dataForm.F_Files=[];
						if(!this.dataForm.F_SettleFiles)this.dataForm.F_SettleFiles=[];
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
                            url: '/api/example/ACtcSales/' + this.dataForm.id,
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
                            url: '/api/example/ACtcSales',
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
        }
    };
</script>
<style>
        page{
                background-color: #f0f2f6;
        }
</style>



