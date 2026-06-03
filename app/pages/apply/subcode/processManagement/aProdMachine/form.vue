﻿﻿﻿﻿﻿<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" :rules="rules" ref="dataForm" :errorType="['toast']" label-position="left" :label-width="100 * 1.5" label-align="right" class="jnpf-form">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='机台图片' prop="F_Image"  :label-width="100 * 1.5">
					<JnpfUploadImg v-model="dataForm.F_Image" sizeUnit='MB' :fileSize='10' pathType="selfPath"  :sortRule='[
  "1",
  "2"
]' timeFormat='YYYYMMDD' buttonText='点击上传' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='机台编号' prop="F_MachineCode"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_MachineCode" placeholder='请填写，忽略将自动生成'  :showCount='false' disabled/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='机台名称' prop="F_MachineName" required  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_MachineName" placeholder='请输入'  :showCount='false' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='机台规格' prop="F_MachineSpec"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_MachineSpec" placeholder='请输入'  :showCount='false' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='机台状态' prop="F_MachineStatus" required  :label-width="100 * 1.5">
					<JnpfSelect v-model="dataForm.F_MachineStatus" :options="f_MachineStatusOptions" :props="f_MachineStatusProps" filterable placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='机台类别' prop="F_MachineType" required  :label-width="100 * 1.5">
					<JnpfCascader v-model="dataForm.F_MachineType" :options="f_MachineTypeOptions" :props="f_MachineTypeProps" placeholder='请选择' filterable clearable />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='车间' prop="F_WorkshopId"  :label-width="100 * 1.5">
					<JnpfCascader v-model="dataForm.F_WorkshopId" :options="f_WorkshopIdOptions" :props="f_WorkshopIdProps" placeholder='请选择' filterable clearable />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='产线' prop="F_LineId"  :label-width="100 * 1.5">
					<JnpfSelect v-model="dataForm.F_LineId" :options="f_LineIdOptions" :props="f_LineIdProps" placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='单日运行时长' prop="F_DailyRunHours"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_DailyRunHours" :precision='0' addonAfter="小时/天" :step='1.0'  />
				</u-form-item>
			</view>
			<JnpfText content='标准工时'  :textStyle='{
  "color": "#000000",
  "text-align": "right",
  "font-weight": "normal",
  "font-style": "normal",
  "text-decoration": "none",
  "line-height": 32,
  "font-size": 14
}'/>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='标准工时' prop="F_StdHour"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_StdHour" :precision='0' addonAfter="小时" :step='1.0'  />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item  prop="F_StdMinute"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_StdMinute" :precision='0' addonAfter="分钟" :step='1.0'  />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item  prop="F_StdSecond"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_StdSecond" :precision='0' addonAfter="秒" :step='1.0'  />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='标准产出' prop="F_StdOutput" left-icon="question-circle-fill" @clickIcon="clickIcon(label='标准产出', '标准效率是单位时间内按照标准操作流程生产出的产品数量。标准效率 = 标准产出 / 标准工时。' )" :left-icon-style="{'color':'#a0acb7'}"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_StdOutput" :precision='0' :step='1.0'  />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='启用状态' prop="F_State"  :label-width="100 * 1.5">
					<JnpfSwitch v-model="dataForm.F_State" />
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
        data() {
            return {
				keyCode : +new Date(),
                btnLoading: false,
				addTableConf:{
				},
                dataForm: {
                    id:'',
					F_Image:[],
					F_MachineCode:undefined,
					F_MachineName:undefined,
					F_MachineSpec:undefined,
					F_MachineStatus:undefined,
					F_MachineType:undefined,
					F_WorkshopId:undefined,
					F_LineId:undefined,
					F_DailyRunHours:undefined,
					F_StdHour:undefined,
					F_StdMinute:undefined,
					F_StdSecond:undefined,
					F_StdOutput:undefined,
					F_State:1,
					F_CreatorUserId:undefined,
					F_CreatorTime:undefined,
                },
                rules: {
					F_MachineName:[
						{
							required:true,
							message:this.$t('common.inputText')+' '+'机台名称',
							trigger:'blur',
						},
					],
					F_MachineStatus:[
						{
							required:true,
							message:this.$t('common.chooseText')+' '+'机台状态',
							trigger:'change',
						},
					],
					F_MachineType:[
						{
							required:true,
							message:this.$t('common.chooseText')+' '+'机台类别',
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
				jurisdictionType:'',
				addType: 0,
				f_MachineStatusProps:{'label':'fullName','value':'enCode'},
				f_MachineStatusOptions: [],
				f_MachineTypeProps:{
  "label": "F_Name",
  "value": "F_Id",
  "children": "children"
},
				f_MachineTypeOptions: [],
				f_WorkshopIdProps:{
  "label": "F_Name",
  "value": "F_Id",
  "children": "children"
},
				f_WorkshopIdOptions: [],
				f_LineIdProps:{'label':'fullName','value':'enCode'},
				f_LineIdOptions: [],
            };
        },
		watch: {
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
			this.getf_MachineStatusOptions();
			this.getf_MachineTypeOptions();
			this.getf_WorkshopIdOptions();
			this.getf_LineIdOptions();
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
			getf_MachineStatusOptions(){
				getDictionaryDataSelector('2011620395194650624').then(res => {
					this.f_MachineStatusOptions = res.data.list
				});
			},
			getf_MachineTypeOptions(){
				this.f_MachineTypeOptions = []
				let templateJson = []
				let query = {
					paramList: this.getParamList(templateJson, this.dataForm)
				}
				getDataInterfaceRes('2011621811959238656', query).then(res => {
					let data = res.data
					this.f_MachineTypeOptions = Array.isArray(data) ? data : []
				});
			},
			getf_WorkshopIdOptions(){
				this.f_WorkshopIdOptions = []
				let templateJson = []
				let query = {
					paramList: this.getParamList(templateJson, this.dataForm)
				}
				getDataInterfaceRes('2011621894347952128', query).then(res => {
					let data = res.data
					this.f_WorkshopIdOptions = Array.isArray(data) ? data : []
				});
			},
			getf_LineIdOptions(){
				getDictionaryDataSelector('2011623836151320576').then(res => {
					this.f_LineIdOptions = res.data.list
				});
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
                        url: '/api/example/AProdMachine/' + this.dataForm.id,
                        method: 'get',
                    }).then(res => {
                        this.dataForm = res.data;
						if(!this.dataForm.F_Image)this.dataForm.F_Image=[];
						if(!this.dataForm.F_MachineType)this.dataForm.F_MachineType=[];
						if(!this.dataForm.F_WorkshopId)this.dataForm.F_WorkshopId=[];
                    })
                }
            },
			submitForm(type) {
                this.$refs.dataForm.validate(valid => {
                    if (!valid) return
                    this.btnLoading = true
                    if (this.dataForm.id) {
                        request({
                            url: '/api/example/AProdMachine/' + this.dataForm.id,
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
                            url: '/api/example/AProdMachine',
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
        }
    };
</script>
<style>
        page{
                background-color: #f0f2f6;
        }
</style>



