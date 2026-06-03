﻿<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" :rules="rules" ref="dataForm" :errorType="['toast']" label-position="left" :label-width="100 * 1.5" label-align="right" class="jnpf-form">
			<!-- <view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='加工单' prop="F_ProcessingId"  :label-width="100 * 1.5">
					<JnpfSelect v-model="dataForm.F_ProcessingId" :options="F_ProcessingIdOptions" :props="F_ProcessingIdProps" filterable placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='工序' prop="F_ProcessId"  :label-width="100 * 1.5">
					<JnpfSelect v-model="dataForm.F_ProcessId" :options="F_ProcessIdOptions" :props="F_ProcessIdProps" filterable placeholder='请选择' />
				</u-form-item>
			</view> -->
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='计划开始' prop="F_PlanStartDate"  :label-width="100 * 1.5">
					<JnpfDatePicker v-model="dataForm.F_PlanStartDate" type='date' format='yyyy-MM-dd' placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='计划结束' prop="F_PlanEndDate"  :label-width="100 * 1.5">
					<JnpfDatePicker v-model="dataForm.F_PlanEndDate" type='date' format='yyyy-MM-dd' placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='生产派工' prop="F_ProdDispatch"  :label-width="100 * 1.5">
					<JnpfSelect v-model="dataForm.F_ProdDispatch" :options="F_ProdDispatchOptions" :props="F_ProdDispatchProps" placeholder='请选择' />
					<!-- <JnpfUserSelect v-model="dataForm.F_ProdDispatch" placeholder='请选择' selectType='all' /> -->
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='质检派工' prop="F_QcDispatch"  :label-width="100 * 1.5">
					<JnpfSelect v-model="dataForm.F_QcDispatch" :options="F_QcDispatchOptions" :props="F_QcDispatchProps" placeholder='请选择' />
					<!-- <JnpfUserSelect v-model="dataForm.F_QcDispatch" placeholder='请选择' selectType='all' /> -->
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='生产数量' prop="F_ProdQty"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_ProdQty" :precision='0' :min='1' :step='1.0'  />
				</u-form-item>
			</view>
			<!-- <view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='排序' prop="F_SortCode"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_SortCode" :precision='0' :min='1' :step='1.0'  />
				</u-form-item>
			</view> -->
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='备注' prop="F_Description"  :label-width="100 * 1.5">
					<JnpfTextarea v-model="dataForm.F_Description" placeholder='请输入'  :showCount='false' />
				</u-form-item>
			</view>
			<!-- <view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='任务状态' prop="F_TaskStatus" v-if='false'  :label-width="100 * 1.5">
					<JnpfSelect v-model="dataForm.F_TaskStatus" :options="F_TaskStatusOptions" :props="F_TaskStatusProps" filterable placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='任务类型' prop="F_TaskType" v-if='false'  :label-width="100 * 1.5">
					<JnpfSelect v-model="dataForm.F_TaskType" :options="F_TaskTypeOptions" :props="F_TaskTypeProps" filterable placeholder='请选择' />
				</u-form-item>
			</view> -->
			<!-- <view class="jnpf-table">
				<view class="jnpf-table-title u-line-1" >{{'物料清单'}}
				</view>
				<view class="jnpf-table-item" v-for="(item,i) in dataForm.tableField0bb944" :key="i">
					<view class="jnpf-table-item-title">
						<view class="jnpf-table-item-title-num">({{i+1}})</view>
						<view class="u-flex">
		        <view class="jnpf-table-copy-btn" @click="copyAProdTaskItemRow(i)">{{this.$t('common.copyText','复制')}}</view>
		        <view class="jnpf-table-delete-btn" @click="removeAProdTaskItemRow(i,true)">{{this.$t('common.delText','删除')}}</view>
						</view>
                    </view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='商品'  :label-width="100 * 1.5">
							<JnpfSelect v-model="dataForm.tableField0bb944[i].F_GoodsId" placeholder='请选择' :options="tableField0bb944_F_GoodsIdOptions" :props="tableField0bb944_F_GoodsIdProps" />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='数量'  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableField0bb944[i].F_Num" :precision='0' :min='1' :step='1.0'  />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='备注'  :label-width="100 * 1.5">
							<JnpfInput v-model="dataForm.tableField0bb944[i].F_Description" :maxlength='500' placeholder='请输入'  :showCount='false' />
						</u-form-item>
					</view>
                </view>
				<view class="jnpf-table-footer-btn" >
					<view class="jnpf-table-btn jnpf-table-primary-btn" @click="addAProdTaskItemRow()">
						<text class="jnpf-table-btn-icon icon-ym icon-ym-btn-add"/>
						<text class="jnpf-table-btn-text">{{this.$t('common.add1Text','添加')}}</text>
					</view>
				</view>
            </view>
		 -->
		</u-form>
		<view class="buttom-actions" v-if="jurisdictionType != 'btn_detail'">
			<CustomButton :btnText="$t('common.cancelText')" btnIcon="icon-ym icon-ym-add-cancel" customIcon />
			<u-button class="buttom-btn" type="primary" @click="submitForm" :loading="btnLoading">{{$t('common.okText','确定')}}</u-button>
		</view>
	</view>
</template>
<script>
	import { getDictionaryDataSelector,getDataInterfaceRes,getUserCon } from '@/api/common'
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
					F_ProcessingId:undefined,
					F_ProcessId:undefined,
					F_PlanStartDate:undefined,
					F_PlanEndDate:undefined,
					F_ProdDispatch:[],
					F_QcDispatch:[],
					F_ProdQty:undefined,
					F_SortCode:undefined,
					F_Description:undefined,
					F_TaskStatus:undefined,
					F_TaskType:undefined,
					F_CreatorTime:undefined,
					tableField0bb944:[],
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
				F_ProdDispatchOptions: [],
				F_ProdDispatchProps: { label: 'fullName', value: 'id' },
				F_QcDispatchOptions: [],
				F_QcDispatchProps: { label: 'fullName', value: 'id' },
				f_ProcessingIdProps:{'label':'F_ProcessNo','value':'F_Id'},
				F_ProcessingIdOptions: [],
				f_ProcessIdProps:{'label':'F_ProcessName','value':'id'},
				F_ProcessIdOptions: [],
				f_TaskStatusProps:{'label':'fullName','value':'enCode'},
				F_TaskStatusOptions: [],
				f_TaskTypeProps:{'label':'fullName','value':'enCode'},
				F_TaskTypeOptions: [],
				tableField0bb944_F_GoodsIdProps:{'label':'F_GoodsName','value':'id'},
				tableField0bb944_F_GoodsIdOptions : [],
            };
        },
		watch: {
        },
        onLoad(option) {
			console.log('option',option)
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
			this.getf_ProcessingIdOptions();
			this.getf_ProcessIdOptions();
			this.getf_TaskStatusOptions();
			this.getf_TaskTypeOptions();
			this.gettableField0bb944_F_GoodsIdOptions();
            uni.$on('linkPageConfirm', (subVal, Vmodel) => {
				let item = {
						F_GoodsId:undefined,
						F_Num:undefined,
						F_Description:undefined,
						F_CreatorTime:undefined,
					}
               if ('AProdTaskItem' === Vmodel) subVal.forEach(t => this.dataForm.tableField0bb944.push({...item,...t}));
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
			getf_ProcessingIdOptions(){
				this.f_ProcessingIdOptions = []
				let templateJson = []
				let query = {
					paramList: this.getParamList(templateJson, this.dataForm)
				}
				getDataInterfaceRes('2013860549426810880', query).then(res => {
					let data = res.data
					this.f_ProcessingIdOptions = Array.isArray(data) ? data : []
				});
			},
			getf_ProcessIdOptions(){
				this.f_ProcessIdOptions = []
				let templateJson = []
				let query = {
					paramList: this.getParamList(templateJson, this.dataForm)
				}
				getDataInterfaceRes('2012092092830060544', query).then(res => {
					let data = res.data
					this.f_ProcessIdOptions = Array.isArray(data) ? data : []
				});
			},
			getf_TaskStatusOptions(){
				getDictionaryDataSelector('2013859706900189184').then(res => {
					this.f_TaskStatusOptions = res.data.list
				});
			},
			getf_TaskTypeOptions(){
				getDictionaryDataSelector('2013859641523572736').then(res => {
					this.f_TaskTypeOptions = res.data.list
				});
			},
			gettableField0bb944_F_GoodsIdOptions(i){
				this.tableField0bb944_F_GoodsIdOptions = []
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
					this.tableField0bb944_F_GoodsIdOptions = Array.isArray(data) ? data : []
				});
			},
			selfInit(){
				this.addAProdTaskItem();
			},
			resetForm() {
				this.dataForm.tableField0bb944 = [];
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
                        url: '/api/example/AProdTask/' + this.dataForm.id,
                        method: 'get',
                    }).then(res => {
                        this.dataForm = res.data;
						getUserCon(this.dataForm.id).then(res => {
						      this.F_ProdDispatchOptions = res.data.prodUserList || [];
						      this.F_QcDispatchOptions = res.data.qcUserList || [];
						    }).catch(err => {
							  console.log('获取用户信息时出错:', err, item);
							})
                    })
                }
            },
			submitForm(type) {
                this.$refs.dataForm.validate(valid => {
                    if (!valid) return
                    this.btnLoading = true
                    if (this.dataForm.id) {
                        request({
                            url: '/api/example/AProdTask/' + this.dataForm.id,
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
                            url: '/api/example/AProdTask',
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
			addAProdTaskItemRow() {
				let item = {
						F_GoodsId:undefined,
						F_Num:undefined,
						F_Description:undefined,
						F_CreatorTime:undefined,
					}
				this.dataForm.tableField0bb944.push(item)
				this.initCollapse()
			},
			removeAProdTaskItemRow(i,showConfirm=false) {
				const handleRemove = () => {
					this.dataForm.tableField0bb944.splice(i, 1)
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
			copyAProdTaskItemRow(i) {
				let item = JSON.parse(JSON.stringify(this.dataForm.tableField0bb944[i]));
				item.id = '';
				item.length && item.map(o => delete o.rowData);
				item.F_CreatorTime = undefined;
				this.dataForm.tableField0bb944.push(item);
			},
        }
    };
</script>
<style>
        page{
                background-color: #f0f2f6;
        }
</style>



