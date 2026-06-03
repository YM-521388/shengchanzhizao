<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" :rules="rules" ref="dataForm" :errorType="['toast']" label-position="left" :label-width="100 * 1.5" label-align="right" class="jnpf-form">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='工序名' prop="F_ProcessName" required  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_ProcessName" placeholder='请输入'  :showCount='false' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='工序编号' prop="F_ProcessCode"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_ProcessCode" placeholder='请输入'  :showCount='false' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='车间' prop="F_WorkshopId"  :label-width="100 * 1.5">
					<JnpfCascader v-model="dataForm.F_WorkshopId" :options="f_WorkshopIdOptions" :props="f_WorkshopIdProps" placeholder='请选择' filterable clearable />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='产线' prop="F_Line"  :label-width="100 * 1.5">
					<JnpfSelect v-model="dataForm.F_Line" :options="f_LineOptions" :props="f_LineProps" filterable placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='报工单位' prop="F_ReportUnit"  :label-width="100 * 1.5">
					<JnpfSelect v-model="dataForm.F_ReportUnit" :options="f_ReportUnitOptions" :props="f_ReportUnitProps" placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='单位关系' prop="F_UnitRatio"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_UnitRatio" :step='1.0'  />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='计价方式' prop="F_PriceType" required  :label-width="100 * 1.5">
					<JnpfSelect v-model="dataForm.F_PriceType" :options="f_PriceTypeOptions" :props="f_PriceTypeProps" filterable placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='工资单价' prop="F_WagePrice"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_WagePrice" :precision='2' :step='1.0'  />
				</u-form-item>
			</view>
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
				<u-form-item label='启用状态' prop="F_this"  :label-width="100 * 1.5">
					<JnpfSwitch v-model="dataForm.F_State" />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='附件' prop="F_Files"  :label-width="100 * 1.5">
					<JnpfUploadFile v-model="dataForm.F_Files" :limit='3' sizeUnit='MB' :fileSize='10' pathType="selfPath"  :sortRule='[
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
			<view prop="active1">
				<u-tabs :is-scroll="false" :list="active1Title" name="title" v-model="active1Current" @change="active1Change"></u-tabs>
				<view>
					<view v-show="0 == active1Current">
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='机台' prop="F_MachineId"  :label-width="100 * 1.5">
							<JnpfPopupSelect interfaceName='getPopSelect'
							multiple v-model="dataForm.F_MachineId" 
							propsValue='id' 
							relationField='F_MachineName' 
							:columnOptions='f_MachineIdOptions' 
							interfaceId='2011729284707782656' 
							placeholder='请选择' hasPage :pageSize='20' 
							popupType='dialog' popupTitle='选择数据' 
							:templateJson='f_MachineIdTemplateJson' 
							:vModel="'tableFieldc87c94-F_CustomerId'" type="popup"  
							:extraOptions="optionsObj.F_MachineId" 
							style='width: 100% !important;'/>
							
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='生产人员' prop="F_ProdUserIds"  :label-width="100 * 1.5">
							<JnpfUserSelect v-model="dataForm.F_ProdUserIds" multiple placeholder='请选择' selectType='all' />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='不良品项' prop="F_DefectIds"  :label-width="100 * 1.5">
							<JnpfSelect v-model="dataForm.F_DefectIds" :options="f_DefectIdsOptions" :props="f_DefectIdsProps" filterable placeholder='请选择' multiple />
						</u-form-item>
					</view>
					</view>
					<view v-show="1 == active1Current">
			<JnpfText content='开启配置后工序默认为外协工序，外协工序无法报工和质检。'  :textStyle='{
  "color": "#F20808",
  "text-align": "left",
  "font-weight": "bold",
  "font-style": "normal",
  "text-decoration": "none",
  "line-height": 20,
  "font-size": 14
}'/>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='工序需外协' prop="F_IsOutsource"  :label-width="100 * 1.5">
							<JnpfSwitch @change="IsOutsourceBtn" v-model="dataForm.F_IsOutsource" />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='工序需质检' prop="F_IsQc"  :label-width="100 * 1.5">
							<JnpfSwitch  @change="IsQcBtn" v-model="dataForm.F_IsQc" />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box"  v-if="dataForm.F_IsQc == 1">
				<u-form-item label='不良品需报废、返工' prop="F_DefectHandle"  :label-width="100 * 1.5" >
							<JnpfSwitch v-model="dataForm.F_DefectHandle" />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box" v-if="dataForm.F_IsQc == 1">
				<u-form-item label='质检人员' prop="F_QcUserIds"  :label-width="100 * 1.5">
							<JnpfUserSelect v-model="dataForm.F_QcUserIds" multiple placeholder='请选择' selectType='all' />
						</u-form-item>
					</view>
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
					F_ProcessName:undefined,
					F_ProcessCode:undefined,
					F_WorkshopId:[],
					F_Line:undefined,
					F_ReportUnit:undefined,
					F_UnitRatio:undefined,
					F_PriceType:"0",
					F_WagePrice:undefined,
					F_StdHour:0,
					F_StdMinute:0,
					F_StdSecond:0,
					F_State:1,
					F_Files:[],
					F_Description:undefined,
					F_CreatorUserId:undefined,
					F_CreatorTime:undefined,
					F_MachineId:undefined,
					F_ProdUserIds:[],
					F_DefectIds:[],
					F_IsOutsource:0,
					F_IsQc:0,
					F_DefectHandle:0,
					F_QcUserIds:[],
					F_TaskTimer:0,
                },
                rules: {
					F_ProcessName:[
						{
							required:true,
							message:this.$t('common.inputText')+' '+'工序名',
							trigger:'blur',
						},
					],
					F_PriceType:[
						{
							required:true,
							message:this.$t('common.inputText')+' '+'计价方式',
							trigger:'change',
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
				f_WorkshopIdProps:{
  "label": "F_Name",
  "value": "F_Id",
  "children": "children"
},
				f_WorkshopIdOptions: [],
				f_LineProps:{'label':'fullName','value':'enCode'},
				f_LineOptions: [],
				f_ReportUnitProps:{'label':'fullName','value':'enCode'},
				f_ReportUnitOptions: [],
				f_PriceTypeProps:{'label':'fullName','value':'enCode'},
				f_PriceTypeOptions: [],
				f_MachineIdTemplateJson: [],
				optionsObj:{      
					F_MachineId: [],
				},
				f_MachineIdOptions: [
  {
    "value": "F_MachineCode",
    "label": "机台编号"
  },
  {
    "value": "F_MachineName",
    "label": "机台名称"
  },
  {
    "value": "F_MachineSpec",
    "label": "机台规格"
  },
  {
    "value": "F_MachineStatus",
    "label": "机台状态"
  },
  {
    "value": "F_MachineType",
    "label": "机台类别"
  },
  {
    "value": "F_WorkshopId",
    "label": "车间"
  },
  {
    "value": "F_LineId",
    "label": "产线"
  }
],
				f_DefectIdsProps:{'label':'F_Name','value':'F_Id'},
				f_DefectIdsOptions: [],
				active1Title:[
  {
    "title": "生产信息"
  },
  {
    "title": "质检/外协信息"
  }
],
				active1Current:2-1,
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
			this.getf_WorkshopIdOptions();
			this.getf_LineOptions();
			this.getf_ReportUnitOptions();
			this.getf_PriceTypeOptions();
			this.getf_DefectIdsOptions();
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
			getf_LineOptions(){
				getDictionaryDataSelector('2011623836151320576').then(res => {
					this.f_LineOptions = res.data.list
				});
			},
			getf_ReportUnitOptions(){
				getDictionaryDataSelector('2008448689420505088').then(res => {
					this.f_ReportUnitOptions = res.data.list
				});
			},
			getf_PriceTypeOptions(){
				getDictionaryDataSelector('2011642610875240448').then(res => {
					this.f_PriceTypeOptions = res.data.list
				});
			},
			getf_DefectIdsOptions(){
				this.f_DefectIdsOptions = []
				let templateJson = []
				let query = {
					paramList: this.getParamList(templateJson, this.dataForm)
				}
				getDataInterfaceRes('2011648481097289728', query).then(res => {
					let data = res.data
					this.f_DefectIdsOptions = Array.isArray(data) ? data : []
				});
			},
			active1Change(index)
			{
				this.active1Current = index;
				this.initCollapse()
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
                        url: '/api/example/AProdProcess/' + this.dataForm.id,
                        method: 'get',
                    }).then(res => {
                        this.dataForm = res.data;
						if(!this.dataForm.F_WorkshopId)this.dataForm.F_WorkshopId=[];
						if(!this.dataForm.F_Files)this.dataForm.F_Files=[];
                    })
                }
            },
			submitForm(type) {
                this.$refs.dataForm.validate(valid => {
                    if (!valid) return
                    this.btnLoading = true
                    if (this.dataForm.id) {
                        request({
                            url: '/api/example/AProdProcess/' + this.dataForm.id,
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
                            url: '/api/example/AProdProcess',
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
	   IsOutsourceBtn(val) {
	    if (val == 1) {
	      this.dataForm.F_IsQc = 0;
	    }
	  },
	   IsQcBtn(val) {
	    if (val == 1) {
	      this.dataForm.F_IsOutsource = 0;
	    }
	  }
        }
    };
</script>
<style>
        page{
                background-color: #f0f2f6;
        }
</style>



