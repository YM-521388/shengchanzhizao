﻿﻿﻿﻿<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" :rules="rules" ref="dataForm" :errorType="['toast']" label-position="left" :label-width="100 * 1.5" label-align="right" class="jnpf-form">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='外协工单' prop="F_OutsourceId" required  :label-width="100 * 1.5">
					<JnpfPopupSelect v-model='dataForm.F_OutsourceId' propsValue='F_Id' relationField='F_GoodsNo' :columnOptions='F_OutsourceIdOptions' interfaceId='2014180492177444864' placeholder='请选择' :pageSize='20' popupType='dialog' popupTitle='选择数据' popupWidth='70%' :templateJson='F_OutsourceIdTemplateJson' vModel="F_OutsourceId"  type="popup" :extraOptions='[]'/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='合格数量' prop="F_PassNum" required  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_PassNum" :precision='0' :step='1.0'  />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='不合格数量' prop="F_FailNum" required  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_FailNum" :precision='0' :step='1.0'  />
				</u-form-item>
			</view>
<!-- 			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='外协类型' prop="F_OutsourceType"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_OutsourceType" placeholder='请输入'  :showCount='false' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='结算状态' prop="F_SettleStatus"  :label-width="100 * 1.5">
					<JnpfRadio v-model="dataForm.F_SettleStatus" :options="f_SettleStatusOptions" :props="f_SettleStatusProps" direction="horizontal" />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='结算单价' prop="F_SettleUnitPrice"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_SettleUnitPrice" placeholder='请输入'  :showCount='false' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='结算人' prop="F_SettleUserId"  :label-width="100 * 1.5">
					<JnpfUserSelect v-model="dataForm.F_SettleUserId" placeholder='请选择' selectType='all' />
				</u-form-item>
			</view> 
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='结算时间' prop="F_SettleTime"  :label-width="100 * 1.5">
					<JnpfDatePicker v-model="dataForm.F_SettleTime" type='date' format='yyyy-MM-dd' placeholder='请选择' />
				</u-form-item>
			</view> -->
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='结算附件' prop="F_SettleFiles"  :label-width="100 * 1.5">
					<JnpfUploadFile v-model="dataForm.F_SettleFiles" :limit='9' sizeUnit='MB' :fileSize='10' pathType="defaultPath"  :sortRule='[]' timeFormat='YYYY' buttonText='点击上传' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='结算备注' prop="F_SettleDesc"  :label-width="100 * 1.5">
					<JnpfTextarea v-model="dataForm.F_SettleDesc" placeholder='请输入'  :showCount='false' />
				</u-form-item>
			</view>
<!-- 			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='创建用户' prop="F_CreatorUserId"  :label-width="100 * 1.5">
					<JnpfOpenData v-model="dataForm.F_CreatorUserId" type='currUser' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='创建时间' prop="F_CreatorTime"  :label-width="100 * 1.5">
					<JnpfOpenData v-model="dataForm.F_CreatorTime" type='currTime' />
				</u-form-item>
			</view> -->
			<view class="jnpf-table">
				<view class="jnpf-table-title u-line-1" >{{'选择验收内容'}}
				</view>
				<view class="jnpf-table-item" v-for="(item,i) in dataForm.tableField561a64" :key="i">
					<view class="jnpf-table-item-title">
						<view class="jnpf-table-item-title-num">({{i+1}})</view>
						<view class="u-flex">
		        <view class="jnpf-table-copy-btn" @click="copyAProdOutsourceAcceptItemRow(i)">{{this.$t('common.copyText','复制')}}</view>
		        <view class="jnpf-table-delete-btn" @click="removeAProdOutsourceAcceptItemRow(i,true)">{{this.$t('common.delText','删除')}}</view>
						</view>
                    </view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='验收内容'  :label-width="100 * 1.5">
							<JnpfPopupSelect v-model="dataForm.tableField561a64[i].F_AcceptId" 
							propsValue='F_Id' relationField='F_Title' 
							:columnOptions='tableField561a64_F_AcceptIdOptions' 
							interfaceId='2014209857888063488' placeholder='请选择' 
							:pageSize='20' popupType='dialog' popupTitle='选择数据' 
							popupWidth='60%' :templateJson='tableField561a64_F_AcceptIdTemplateJson' 
							:vModel="'tableField561a64-F_AcceptId'+i" type="popup"/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='排序'  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableField561a64[i].F_SortCode" :precision='0' :step='1.0'  />
						</u-form-item>
					</view>
<!-- 					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='创建用户'  :label-width="100 * 1.5">
							<JnpfInput v-model="dataForm.tableField561a64[i].F_CreatorUserId" :placeholder="$t('component.jnpf.common.autoGenerate')" disabled/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='创建时间'  :label-width="100 * 1.5">
							<JnpfInput v-model="dataForm.tableField561a64[i].F_CreatorTime" :placeholder="$t('component.jnpf.common.autoGenerate')" disabled/>
						</u-form-item>
					</view> -->
                </view>
				<view class="jnpf-table-footer-btn" >
					<view class="jnpf-table-btn jnpf-table-primary-btn" @click="addAProdOutsourceAcceptItemRow()">
						<text class="jnpf-table-btn-icon icon-ym icon-ym-btn-add"/>
						<text class="jnpf-table-btn-text">{{this.$t('common.add1Text','添加')}}</text>
					</view>
				</view>
            </view>
		</u-form>
		<view class="buttom-actions" v-if="jurisdictionType != 'btn_detail'">
			<CustomButton :btnText="$t('common.cancelText')" btnIcon="icon-ym icon-ym-add-cancel" customIcon />
			<u-button v-if="$permission.hasBtnP('btn_edit',menuIds)"  class="buttom-btn" type="primary" @click="submitForm" :loading="btnLoading">{{$t('common.okText','确定')}}</u-button>
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
					F_OutsourceId:undefined,
					F_PassNum:undefined,
					F_FailNum:undefined,
					F_OutsourceType:undefined,
					F_SettleStatus:undefined,
					F_SettleUnitPrice:undefined,
					F_SettleUserId:undefined,
					F_SettleTime:undefined,
					F_SettleFiles:[],
					F_SettleDesc:undefined,
					F_CreatorUserId:undefined,
					F_CreatorTime:undefined,
					tableField561a64:[],
                },
                rules: {
					F_OutsourceId:[
						{
							required:true,
							message:this.$t('common.inputText')+' '+'外协工单',
							trigger:'change',
						},
					],
					F_PassNum:[
						{
							required:true,
							message:this.$t('common.inputText')+' '+'合格数量',
							trigger:[
  "blur",
  "change"
],
							type:'number'
						},
					],
					F_FailNum:[
						{
							required:true,
							message:this.$t('common.inputText')+' '+'不合格数量',
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
				F_OutsourceIdTemplateJson: [],
				F_OutsourceIdOptions: [
  {
    "value": "F_GoodsNo",
    "label": "商品编码"
  },
  {
    "value": "F_GoodsName",
    "label": "商品名称"
  },
  {
    "value": "F_SupplierName",
    "label": "供应商姓名"
  },
  {
    "value": "F_ContactPerson",
    "label": "联系人"
  },
  {
    "value": "F_ContactPhone",
    "label": "联系人电话"
  },
  {
    "value": "F_DetailAddress",
    "label": "详细地址"
  },
  {
    "value": "F_Price",
    "label": "外协单价"
  },
  {
    "value": "F_DeliveryDate",
    "label": "交货日期"
  },
  {
    "value": "F_State",
    "label": "状态"
  }
],
				f_SettleStatusProps:{'label':'fullName','value':'enCode'},
				f_SettleStatusOptions: [],
				tableField561a64_F_AcceptIdTemplateJson: [],
				tableField561a64_F_AcceptIdOptions: [
				      {
				    "value": "F_Title",
				    "label": "标题"
				  },
				  {
				    "value": "F_Content",
				    "label": "验收内容"
				  },
				  {
				    "value": "F_Description",
				    "label": "备注"
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
			this.getf_SettleStatusOptions();
            uni.$on('linkPageConfirm', (subVal, Vmodel) => {
				let item = {
						F_AcceptId:undefined,
						F_SortCode:undefined,
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
					}
               if ('AProdOutsourceAcceptItem' === Vmodel) subVal.forEach(t => this.dataForm.tableField561a64.push({...item,...t}));
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
			getf_SettleStatusOptions(){
				getDictionaryDataSelector('2014214471169478656').then(res => {
					this.f_SettleStatusOptions = res.data.list
				});
			},
    conversionDateTime(type) {
      const format = type === 'yyyy' ? 'yyyy-01-01 00:00:00' : type === 'yyyy-MM' ? 'yyyy-MM-01 00:00:00' :
        type === 'yyyy-MM-dd' ? 'yyyy-MM-dd 00:00:00' : type === 'yyyy-MM-dd HH:mm' ? 'yyyy-MM-dd HH:mm:00' : 'yyyy-MM-dd HH:mm:ss'
      const dataTime = this.jnpf.toDate(new Date(), format)
      return new Date(dataTime).getTime()
    },
	initDefaultData() {
      this.dataForm.F_SettleUserId = this.userInfo.userId
	},
			selfInit(){
				this.addAProdOutsourceAcceptItem();
			},
			resetForm() {
				this.dataForm.tableField561a64 = [];
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
                        url: '/api/example/AProdOutsourceAccept/' + this.dataForm.id,
                        method: 'get',
                    }).then(res => {
                        this.dataForm = res.data;
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
                            url: '/api/example/AProdOutsourceAccept/' + this.dataForm.id,
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
                            url: '/api/example/AProdOutsourceAccept',
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
			addAProdOutsourceAcceptItemRow() {
				let item = {
						F_AcceptId:undefined,
						F_SortCode:undefined,
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
					}
				this.dataForm.tableField561a64.push(item)
				this.initCollapse()
			},
			removeAProdOutsourceAcceptItemRow(i,showConfirm=false) {
				const handleRemove = () => {
					this.dataForm.tableField561a64.splice(i, 1)
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
			copyAProdOutsourceAcceptItemRow(i) {
				let item = JSON.parse(JSON.stringify(this.dataForm.tableField561a64[i]));
				item.id = '';
				item.length && item.map(o => delete o.rowData);
				item.F_CreatorUserId = undefined;
				item.F_CreatorTime = undefined;
				this.dataForm.tableField561a64.push(item);
			},
        }
    };
</script>
<style>
        page{
                background-color: #f0f2f6;
        }
</style>



