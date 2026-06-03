﻿﻿﻿﻿﻿﻿<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" :rules="rules" ref="dataForm" :errorType="['toast']" label-position="left" :label-width="100 * 1.5" label-align="right" class="jnpf-form">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='生产人员' prop="F_ProdUserId" required  :label-width="100 * 1.5">
					<JnpfUserSelect v-model="dataForm.F_ProdUserId" multiple placeholder='请选择' selectType='all' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='良品数' prop="F_GoodQty"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_GoodQty" :precision='0' :min='0' :step='1.0'  />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='不良品数' prop="F_GoodNotQty"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_GoodNotQty" :precision='0' :min='0' :step='1.0'  />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='开始时间' prop="F_StartTime"  :label-width="100 * 1.5">
					<JnpfDatePicker v-model="dataForm.F_StartTime" type='date' format='yyyy-MM-dd HH:mm' placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='结束时间' prop="F_EndTime"  :label-width="100 * 1.5">
					<JnpfDatePicker v-model="dataForm.F_EndTime" type='date' format='yyyy-MM-dd HH:mm' placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='附件' prop="F_Files"  :label-width="100 * 1.5">
					<JnpfUploadFile v-model="dataForm.F_Files" :limit='9' sizeUnit='MB' :fileSize='10' pathType="selfPath"  :sortRule='[
  "1",
  "2"
]' timeFormat='YYYYMMDD' buttonText='点击上传' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='备注' prop="F_Description"  :label-width="100 * 1.5">
					<JnpfTextarea v-model="dataForm.F_Description" :maxlength='500' placeholder='请输入'  :showCount='false' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='结算状态' prop="F_SettleStatus" v-if='false'  :label-width="100 * 1.5">
					<JnpfSelect v-model="dataForm.F_SettleStatus" :options="f_SettleStatusOptions" :props="f_SettleStatusProps" placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='结算时间' prop="F_SettleTime" v-if='false'  :label-width="100 * 1.5">
					<JnpfDatePicker v-model="dataForm.F_SettleTime" type='date' format='yyyy-MM-dd HH:mm:ss' placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='结算人' prop="F_SettleUserId" v-if='false'  :label-width="100 * 1.5">
					<JnpfUserSelect v-model="dataForm.F_SettleUserId" placeholder='请选择' selectType='all' />
				</u-form-item>
			</view>
			<view class="jnpf-table">
				<view class="jnpf-table-title u-line-1" >{{'不良品项'}}
				</view>
				<view class="jnpf-table-item" v-for="(item,i) in dataForm.tableField579169" :key="i">
					<view class="jnpf-table-item-title">
						<view class="jnpf-table-item-title-num">({{i+1}})</view>
						<view class="u-flex">
							<view class="jnpf-table-delete-btn" @click="removeAProdReportDefectRow(i,true)">{{this.$t('common.delText','删除')}}</view>
						</view>
                    </view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='不良品项'  :label-width="100 * 1.5">
							<JnpfSelect v-model="dataForm.tableField579169[i].F_DefectId" :options="tableField579169_F_DefectIdOptions" :props="tableField579169_F_DefectIdProps" multiple/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='数量'  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableField579169[i].F_Num" :precision='0' :min='0' :step='1.0'  />
						</u-form-item>
					</view>
                </view>
				<view class="jnpf-table-footer-btn" >
					<view class="jnpf-table-btn jnpf-table-primary-btn" @click="addAProdReportDefectRow()">
						<text class="jnpf-table-btn-icon icon-ym icon-ym-btn-add"/>
						<text class="jnpf-table-btn-text">{{this.$t('common.add1Text','添加')}}</text>
					</view>
				</view>
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
					F_ProdUserId:[],
					F_GoodQty:undefined,
					F_GoodNotQty:undefined,
					F_StartTime:undefined,
					F_EndTime:undefined,
					F_WagePrice:undefined,
					F_Files:[],
					F_Description:undefined,
					F_CreatorUserId:[],
					F_SettleStatus:"0",
					F_SettleTime:undefined,
					F_SettleUserId:undefined,
					F_State:"0",
					F_TaskId:undefined,
					F_CreatorTime:undefined,
					tableField579169:[],
                },
                rules: {
					F_ProdUserId:[
						{
							required:true,
							message:this.$t('common.inputText')+' '+'生产人员',
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
				f_CreatorUserIdProps:{'label':'fullName','value':'id'},
				f_CreatorUserIdOptions: [],
				f_SettleStatusProps:{'label':'fullName','value':'enCode'},
				f_SettleStatusOptions: [],
				f_StateProps:{'label':'fullName','value':'enCode'},
				f_StateOptions: [],
				f_TaskIdProps:{'label':'F_Id','value':'F_Id'},
				f_TaskIdOptions: [],
				tableField579169_F_DefectIdProps:{'label':'fullName','value':'id'},
				tableField579169_F_DefectIdOptions : [],
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
			  if (!option.id) {
			      this.dataForm.F_TaskId = option.F_TaskId;
			      this.dataForm.F_ProcessId = option.F_ProcessId;
			      this.dataForm.F_GoodQty = option.F_ProdQty;
			      if (option.F_ProdUserByIds) {
			          this.dataForm.F_ProdUserId = JSON.parse(option.F_ProdUserByIds);
			      } else {
			          this.dataForm.F_ProdUserId = [];
			      }
				  this.gettableField579169_F_DefectIdOptions2();
			    }
			this.idList = option.idList ? option.idList.split(",") : []
            uni.setNavigationBarTitle({
                title: _title
            })
			this.dataValue = JSON.parse(JSON.stringify(this.dataForm))
            this.initData()
			this.getf_CreatorUserIdOptions();
			this.getf_SettleStatusOptions();
			this.getf_StateOptions();
			this.getf_TaskIdOptions();
            uni.$on('linkPageConfirm', (subVal, Vmodel) => {
				let item = {
						F_DefectId:undefined,
						F_Num:undefined,
						F_CreatorTime:undefined,
					}
               if ('AProdReportDefect' === Vmodel) subVal.forEach(t => this.dataForm.tableField579169.push({...item,...t}));
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
			getf_CreatorUserIdOptions(){
				this.f_CreatorUserIdOptions = []
				let templateJson = [
  {
    "defaultValue": "",
    "field": "processId",
    "dataType": "varchar",
    "required": 0,
    "fieldName": "",
    "relationField": "F_TaskId",
    "jnpfKey": "select",
    "complexHeaderList": null,
    "sourceType": 1,
    "isChildren": false,
    "IsMultiple": false
  }
]
				let query = {
					paramList: this.getParamList(templateJson, this.dataForm)
				}
				getDataInterfaceRes('2014256316289257472', query).then(res => {
					let data = res.data
					this.f_CreatorUserIdOptions = Array.isArray(data) ? data : []
				});
			},
			getf_SettleStatusOptions(){
				getDictionaryDataSelector('2014214471169478656').then(res => {
					this.f_SettleStatusOptions = res.data.list
				});
			},
			getf_StateOptions(){
				getDictionaryDataSelector('2014253420491444224').then(res => {
					this.f_StateOptions = res.data.list
				});
			},
			getf_TaskIdOptions(){
				this.f_TaskIdOptions = []
				let templateJson = []
				let query = {
					paramList: this.getParamList(templateJson, this.dataForm)
				}
				getDataInterfaceRes('2014269102549504000', query).then(res => {
					let data = res.data
					this.f_TaskIdOptions = Array.isArray(data) ? data : []
				});
			},
			gettableField579169_F_DefectIdOptions2() {
			    let templateJson = [];
			    let query = {
			      paramList: this.getParamList(templateJson, this.dataForm),
			    };
			    getDataInterfaceRes('2011648481097289728', query).then(res => {
			      let data = res.data;
			      this.tableField579169_F_DefectIdOptions = Array.isArray(data) ? data : [];
				  this.tableField579169_F_DefectIdProps = {'label':'F_Name','value':'F_Id'};
			    });
			},
			gettableField579169_F_DefectIdOptions(){
				// this.tableField579169_F_DefectIdOptions = []
				let templateJson = [{
					"defaultValue": '',
					"field": "processId",
					"dataType": "varchar",
					"required": 0,
					"fieldName": "",
					"relationField": "F_ProcessId",
					"jnpfKey": "select",
					"complexHeaderList": null,
					"sourceType": 1,
					"isChildren": false,
					"IsMultiple": false
				}]
				let query = {
					paramList: this.getParamList(templateJson, this.dataForm )
				}
				getDataInterfaceRes('2014256316289257472', query).then(res => {
					let data = res.data
					this.tableField579169_F_DefectIdOptions = Array.isArray(data) ? data : [],
					this.tableField579169_F_DefectIdProps = {'label':'fullName','value':'id'};
				});
			},
    conversionDateTime(type) {
      const format = type === 'yyyy' ? 'yyyy-01-01 00:00:00' : type === 'yyyy-MM' ? 'yyyy-MM-01 00:00:00' :
        type === 'yyyy-MM-dd' ? 'yyyy-MM-dd 00:00:00' : type === 'yyyy-MM-dd HH:mm' ? 'yyyy-MM-dd HH:mm:00' : 'yyyy-MM-dd HH:mm:ss'
      const dataTime = this.jnpf.toDate(new Date(), format)
      return new Date(dataTime).getTime()
    },
	initDefaultData() {
      this.dataForm.F_StartTime = this.conversionDateTime("yyyy-MM-dd HH:mm");
      this.dataForm.F_EndTime = this.conversionDateTime("yyyy-MM-dd HH:mm");
	},
			selfInit(){
				this.addAProdReportDefect();
			},
			resetForm() {
				this.dataForm.tableField579169 = [];
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
                        url: '/api/example/AProdReport/' + this.dataForm.id,
                        method: 'get',
                    }).then(res => {
                        this.dataForm = res.data;
						this.gettableField579169_F_DefectIdOptions();
						if(!this.dataForm.F_Files)this.dataForm.F_Files=[];
						this.dataForm.tableField579169=this.dataForm.tableField579169.map(item=>{
							item.F_DefectId=item.F_DefectId.split(',')
							return item
						})
                    })
				}else{
					this.initDefaultData()
				}
            },
			submitForm(type) {
				this.dataForm.tableField579169.map(temp=>{
					temp.F_DefectId=temp.F_DefectId.join(',')
				})
                this.$refs.dataForm.validate(valid => {
                    if (!valid) return
                    this.btnLoading = true
                    if (this.dataForm.id) {
                        request({
                            url: '/api/example/AProdReport/' + this.dataForm.id,
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
                            url: '/api/example/AProdReport',
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
			F_TaskIdChange(){
				this.dataForm.F_CreatorUserId = []
				this.getf_CreatorUserIdOptions()
				this.dataForm.F_CreatorUserId = []
				this.getf_CreatorUserIdOptions()
				this.dataForm.F_CreatorUserId = []
				this.getf_CreatorUserIdOptions()
			},
			addAProdReportDefectRow() {
				let item = {
						F_DefectId:undefined,
						F_Num:undefined,
						F_CreatorTime:undefined,
					}
				this.dataForm.tableField579169.push(item)
				this.initCollapse()
			},
			removeAProdReportDefectRow(i,showConfirm=false) {
				const handleRemove = () => {
					this.dataForm.tableField579169.splice(i, 1)
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
			copyAProdReportDefectRow(i) {
				let item = JSON.parse(JSON.stringify(this.dataForm.tableField579169[i]));
				item.id = '';
				item.length && item.map(o => delete o.rowData);
				item.F_CreatorTime = undefined;
				this.dataForm.tableField579169.push(item);
			},
        }
    };
</script>
<style>
        page{
                background-color: #f0f2f6;
        }
</style>



