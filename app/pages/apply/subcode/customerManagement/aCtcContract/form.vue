<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" :rules="rules" ref="dataForm" :errorType="['toast']" label-position="left" :label-width="100 * 1.5" label-align="right" class="jnpf-form">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='合同编号' prop="F_ContractCode"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_ContractCode" placeholder='请填写，忽略将自动生成'  :showCount='false' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='客户' prop="F_CustomerId"  :label-width="100 * 1.5">
					<JnpfSelect v-model="dataForm.F_CustomerId" :options="f_CustomerIdOptions" :props="f_CustomerIdProps" placeholder='请选择'  @change="customerChange"/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='联系人' prop="F_ContactId"  :label-width="100 * 1.5">
					<JnpfPopupSelect v-model='dataForm.F_ContactId' propsValue='F_Id' relationField='F_ContactName' :columnOptions='f_ContactIdOptions' interfaceId='2009459664370143232' placeholder='请选择' :pageSize='20' popupType='dialog' popupTitle='选择数据' popupWidth='800px' :templateJson='F_ContactIdTemplateJson' vModel="F_ContactId"  :formData='dataForm' type="popup" :extraOptions='[]'/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='客户地址' prop="F_AddressId"  :label-width="100 * 1.5">
					<JnpfSelect v-model="dataForm.F_AddressId" :options="f_AddressIdOptions" :props="f_AddressIdProps" placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='预付款' prop="F_PrepayAmount"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_PrepayAmount" :step='1.0'  />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='销售总数' prop="F_SaleTotal"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_SaleTotal" :precision='0' :step='1.0'  disabled placeholder=''/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='合同金额' prop="F_ContractAmount"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_ContractAmount" :step='1.0'  disabled placeholder=''/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='交货日期' prop="F_DeliveryDate"  :label-width="100 * 1.5">
					<JnpfDatePicker v-model="dataForm.F_DeliveryDate" type='date' format='yyyy-MM-dd' placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='业务员' prop="F_SalesmanId"  :label-width="100 * 1.5">
					<JnpfUserSelect v-model="dataForm.F_SalesmanId" placeholder='请选择' selectType='all' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='是否含税' prop="F_IsTaxable"  :label-width="100 * 1.5">
					<JnpfSwitch v-model="dataForm.F_IsTaxable" />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='备注' prop="F_Description"  :label-width="100 * 1.5">
					<JnpfTextarea v-model="dataForm.F_Description" placeholder='请输入'  :showCount='true' />
				</u-form-item>
			</view>
			<view class="jnpf-table">
				<view class="jnpf-table-title u-line-1" >{{'选择合同商品'}}
				</view>
				<view class="jnpf-table-item" v-for="(item,i) in dataForm.tableFieldf4dfaf" :key="i">
					<view class="jnpf-table-item-title">
						<view class="jnpf-table-item-title-num">({{i+1}})</view>
						<view class="u-flex">
		        <view class="jnpf-table-copy-btn" @click="copyACtcContractItemRow(i)">{{this.$t('common.copyText','复制')}}</view>
		        <view class="jnpf-table-delete-btn" @click="removeACtcContractItemRow(i,true)">{{this.$t('common.delText','删除')}}</view>
						</view>
                    </view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='商品'  :label-width="100 * 1.5">
							<JnpfPopupSelect v-model="dataForm.tableFieldf4dfaf[i].F_CustomerId" propsValue='F_Id' relationField='F_GoodsName' :columnOptions='tableFieldf4dfaf_F_CustomerIdOptions' interfaceId='2008721559174385664' placeholder='请选择' :pageSize='20' popupType='dialog' popupTitle='选择数据' popupWidth='800px' :templateJson='tableFieldf4dfaf_F_CustomerIdTemplateJson' :vModel="'tableFieldf4dfaf-F_CustomerId'+i" type="popup"/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='单价'  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableFieldf4dfaf[i].F_Price" :step='1.0'  @change="() => recalcRowAmount(i)"/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='销售数量'  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableFieldf4dfaf[i].F_SaleQty" :precision='0' :step='1.0'  controls @change="() => recalcRowAmount(i)"/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='折扣'  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableFieldf4dfaf[i].F_Discount" :step='1.0'   @change="(e) => handleDiscount(e,i)"/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='商品备注'  :label-width="100 * 1.5">
							<JnpfInput v-model="dataForm.tableFieldf4dfaf[i].F_Description" placeholder='请输入'  :showCount='false' />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='门板厚度'  :label-width="100 * 1.5">
							<JnpfSelect v-model="dataForm.tableFieldf4dfaf[i].F_DoorPlateThickness" placeholder='请选择' :options="tableFieldf4dfaf_F_DoorPlateThicknessOptions" :props="tableFieldf4dfaf_F_DoorPlateThicknessProps" />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='箱体板厚'  :label-width="100 * 1.5">
							<JnpfSelect v-model="dataForm.tableFieldf4dfaf[i].F_BoxPlateThickness" placeholder='请选择' :options="tableFieldf4dfaf_F_BoxPlateThicknessOptions" :props="tableFieldf4dfaf_F_BoxPlateThicknessProps" />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='安装或安装梁'  :label-width="100 * 1.5">
							<JnpfSelect v-model="dataForm.tableFieldf4dfaf[i].F_InstallOrBeam" placeholder='请选择' :options="tableFieldf4dfaf_F_InstallOrBeamOptions" :props="tableFieldf4dfaf_F_InstallOrBeamProps" />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='锁具'  :label-width="100 * 1.5">
							<JnpfInput v-model="dataForm.tableFieldf4dfaf[i].F_LockSet" placeholder='请输入'  :showCount='false' />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='铰链'  :label-width="100 * 1.5">
							<JnpfInput v-model="dataForm.tableFieldf4dfaf[i].F_Hinge" placeholder='请输入'  :showCount='false' />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='颜色'  :label-width="100 * 1.5">
							<JnpfInput v-model="dataForm.tableFieldf4dfaf[i].F_Color" placeholder='请输入'  :showCount='false' />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='材质'  :label-width="100 * 1.5">
							<JnpfSelect v-model="dataForm.tableFieldf4dfaf[i].F_Material" placeholder='请选择' :options="tableFieldf4dfaf_F_MaterialOptions" :props="tableFieldf4dfaf_F_MaterialProps" />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='附件'  :label-width="100 * 1.5">
							<JnpfUploadFile v-model="dataForm.tableFieldf4dfaf[i].F_Files" :limit='9' sizeUnit='MB' :fileSize='10' pathType="defaultPath"  :sortRule='[]' timeFormat='YYYY' buttonText='点击上传' />
						</u-form-item>
					</view>
                </view>
				<view class="jnpf-table-footer-btn" >
					<view class="jnpf-table-btn jnpf-table-primary-btn" @click="addACtcContractItemRow()">
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
	import { getContractDefaultsByCustomer } from '@/api/subcode/subcode'
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
					F_ContractCode:undefined,
					F_CustomerId:undefined,
					F_ContactId:undefined,
					F_AddressId:undefined,
					F_PrepayAmount:0,
					F_SaleTotal:0,
					F_ContractAmount:0,
					F_DeliveryDate:undefined,
					F_SalesmanId:undefined,
					F_IsTaxable:0,
					F_AuditStatus:undefined,
					F_Description:undefined,
					F_CreatorUserId:undefined,
					F_CreatorTime:undefined,
					tableFieldf4dfaf:[],
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
				f_CustomerIdProps:{'label':'F_CustomerName','value':'F_Id'},
				f_CustomerIdOptions: [],
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
				f_ContactIdOptions: [
  {
    "value": "F_ContactName",
    "label": "姓名"
  },
  {
    "value": "F_ContactPhone",
    "label": "电话"
  }
],
				f_AddressIdProps:{'label':'F_Address','value':'F_Id'},
				f_AddressIdOptions: [],
				tableFieldf4dfaf_F_CustomerIdTemplateJson: [],
				tableFieldf4dfaf_F_CustomerIdOptions: [
  {
    "value": "F_GoodsNo",
    "label": "编码"
  },
  {
    "value": "F_GoodsName",
    "label": "名称"
  },
  {
    "value": "F_Unit",
    "label": "单位"
  },
  {
    "value": "F_CostPrice",
    "label": "成本单价"
  },
  {
    "value": "F_SalePrice",
    "label": "销售单价"
  },
  {
    "value": "F_Specification",
    "label": "规则"
  },
  {
    "value": "F_Source",
    "label": "商品来源"
  },
  {
    "value": "F_Remark",
    "label": "备注"
  }
],
				tableFieldf4dfaf_F_DoorPlateThicknessProps:{'label':'fullName','value':'enCode'},
				tableFieldf4dfaf_F_DoorPlateThicknessOptions : [],
				tableFieldf4dfaf_F_BoxPlateThicknessProps:{'label':'fullName','value':'enCode'},
				tableFieldf4dfaf_F_BoxPlateThicknessOptions : [],
				tableFieldf4dfaf_F_InstallOrBeamProps:{'label':'fullName','value':'enCode'},
				tableFieldf4dfaf_F_InstallOrBeamOptions : [],
				tableFieldf4dfaf_F_MaterialProps:{'label':'fullName','value':'enCode'},
				tableFieldf4dfaf_F_MaterialOptions : [],
            };
        },
		watch: {
			'dataForm.tableFieldf4dfaf': {
				handler: function(newVal, oldValue) {
						  try {
						          const rows = Array.isArray(newVal) ? newVal: [];
						        try {
									// 销售总数
									const sumSale = rows.reduce((acc, row) => {
									       const v = typeof row.F_SaleQty !== 'undefined' && row.F_SaleQty !== null ? Number(row.F_SaleQty) || 0 : 0;
									       return acc + (isNaN(v) ? 0 : v);
									     }, 0);
									 // 保留两位小数
									this.dataForm.F_SaleTotal=sumSale;
									// 合同金额
									const sum = rows.reduce((acc, row) => {
									const v =
									    typeof row.F_AmountAfterDiscount !== 'undefined' && row.F_AmountAfterDiscount !== null
									    ? Number(row.F_AmountAfterDiscount)
									    : (Number(row.F_Price) || 0) * (Number(row.F_SaleQty) || 0);
									return acc + (isNaN(v) ? 0 : v);
									}, 0);
										this.dataForm.F_ContractAmount= Math.round(sum * 100) / 100;
						        } catch (e) {
						          // ignore
						        }
						      } catch (e) {
						        // ignore
						      }
				},
				immediate: true,
				deep: true
			},
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
			this.getf_CustomerIdOptions();
			this.gettableFieldf4dfaf_F_DoorPlateThicknessOptions();
			this.gettableFieldf4dfaf_F_BoxPlateThicknessOptions();
			this.gettableFieldf4dfaf_F_InstallOrBeamOptions();
			this.gettableFieldf4dfaf_F_MaterialOptions();			
            uni.$on('linkPageConfirm', (subVal, Vmodel) => {
				let item = {
						F_CustomerId:undefined,
						F_Price:undefined,
						F_SaleQty:undefined,
						F_Discount:undefined,
						F_Description:undefined,
						F_DoorPlateThickness:undefined,
						F_BoxPlateThickness:undefined,
						F_InstallOrBeam:undefined,
						F_LockSet:undefined,
						F_Hinge:undefined,
						F_Color:undefined,
						F_Material:undefined,
						F_F_AuditStatus:undefined,
						F_Files:[],
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
					}
               if ('ACtcContractItem' === Vmodel) subVal.forEach(t => this.dataForm.tableFieldf4dfaf.push({...item,...t}));
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
			getf_AddressIdOptions(){
				this.f_AddressIdOptions = []
				let templateJson = [
  {
    "defaultValue": "",
    "field": "id",
    "dataType": "varchar",
    "required": 0,
    "fieldName": "",
    "relationField": this.dataForm.F_CustomerId,
    "jnpfKey": null,
    "complexHeaderList": null,
    "sourceType": 2,
    "isChildren": false,
    "IsMultiple": false
  }
]
				let query = {
					paramList: this.getParamList(templateJson, this.dataForm)
				}
				getDataInterfaceRes('2009527238009163776', query).then(res => {
					let data = res.data
					this.f_AddressIdOptions = Array.isArray(data) ? data : []
				});
			},
			gettableFieldf4dfaf_F_DoorPlateThicknessOptions(i){
				getDictionaryDataSelector('2013183327061807104').then(res => {
					this.tableFieldf4dfaf_F_DoorPlateThicknessOptions = res.data.list
				});
			},
			gettableFieldf4dfaf_F_BoxPlateThicknessOptions(i){
				getDictionaryDataSelector('2013183327061807104').then(res => {
					this.tableFieldf4dfaf_F_BoxPlateThicknessOptions = res.data.list
				});
			},
			gettableFieldf4dfaf_F_InstallOrBeamOptions(i){
				getDictionaryDataSelector('2013183327061807104').then(res => {
					this.tableFieldf4dfaf_F_InstallOrBeamOptions = res.data.list
				});
			},
			gettableFieldf4dfaf_F_MaterialOptions(i){
				getDictionaryDataSelector('2016346179754921984').then(res => {
					this.tableFieldf4dfaf_F_MaterialOptions = res.data.list
				});
			},						
			selfInit(){
				this.addACtcContractItem();
			},
			resetForm() {
				this.dataForm.tableFieldf4dfaf = [];
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
                        url: '/api/example/ACtcContract/' + this.dataForm.id,
                        method: 'get',
                    }).then(res => {
                        this.dataForm = res.data;
						this.F_ContactIdTemplateJson[0].relationField=this.dataForm.F_CustomerId
						this.getf_AddressIdOptions();
					})
                }
            },
			submitForm(type) {
                this.$refs.dataForm.validate(valid => {
                    if (!valid) return
                    this.btnLoading = true
                    if (this.dataForm.id) {
                        request({
                            url: '/api/example/ACtcContract/' + this.dataForm.id,
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
                            url: '/api/example/ACtcContract',
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
			addACtcContractItemRow() {
				let item = {
						F_CustomerId:undefined,
						F_Price:undefined,
						F_SaleQty:undefined,
						F_Discount:undefined,
						F_Description:undefined,
						F_DoorPlateThickness:undefined,
						F_BoxPlateThickness:undefined,
						F_InstallOrBeam:undefined,
						F_LockSet:undefined,
						F_Hinge:undefined,
						F_Color:undefined,
						F_Material:undefined,
						F_F_AuditStatus:undefined,
						F_Files:[],
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
					}
				this.dataForm.tableFieldf4dfaf.push(item)
				this.initCollapse()
			},
			removeACtcContractItemRow(i,showConfirm=false) {
				const handleRemove = () => {
					this.dataForm.tableFieldf4dfaf.splice(i, 1)
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
			copyACtcContractItemRow(i) {
				let item = JSON.parse(JSON.stringify(this.dataForm.tableFieldf4dfaf[i]));
				item.id = '';
				item.length && item.map(o => delete o.rowData);
				item.F_CreatorUserId = undefined;
				item.F_CreatorTime = undefined;
				this.dataForm.tableFieldf4dfaf.push(item);
			},
			customerChange(val){
				this.F_ContactIdTemplateJson[0].relationField = val
				this.dataForm.F_ContactId=''
				
				    try {
				      // relationField 已由 watch 保证，这里确保地址选项即时刷新
				      this.getf_AddressIdOptions()
				      if (val) {
				        getContractDefaultsByCustomer(val).then(async (res) => {
				          // defHttp usually returns { code, msg, data }, accept either shape
				          const payload = res && res.data ? res.data : res;
				          const data = payload || {};
				          // 联系人：用后端返回的数组覆盖 extraOptions，然后在下一 tick 设置 v-model
				          if (Array.isArray(data.contacts) && data.contacts.length) {
				            const mappedContacts = data.contacts.map((c) => ({
				              ...c,
				              F_Id: c.F_Id || c.id,
				              F_ContactName: c.F_ContactName || c.fullName || c.F_ContactName,
				              id: c.F_Id || c.id,
				              fullName: c.F_ContactName || c.fullName,
				            }));
				            this.dataForm.F_ContactId = mappedContacts[0].F_Id || mappedContacts[0].id;
				          } else {
				            this.dataForm.F_ContactId = undefined;
				          }
				          // 地址：直接替换地址下拉数据，再在下一 tick 设置 v-model
				          if (Array.isArray(data.addresses) && data.addresses.length) {
				            const mappedAddrs = data.addresses.map((a) => ({
				              ...a,
				              F_Id: a.F_Id || a.id,
				              F_Address: a.F_Address || a.address || a.F_Address,
				              F_Region: a.F_Region || a.region || a.F_Region,
				              id: a.F_Id || a.id,
				            }));
				            this.dataForm.F_AddressId = mappedAddrs[0].F_Id || mappedAddrs[0].id;
				          } else {
				            this.dataForm.F_AddressId = undefined;
				          }
				        });
				      } else {
				        this.dataForm.F_ContactId = undefined;
				        this.dataForm.F_AddressId = undefined;
				      }
				    } catch (e) {
				      // ignore
				    }
			},
			handleDiscount(e,index){
			let record =this.dataForm.tableFieldf4dfaf[index]
			record.F_Discount=e
			this.recalcRowAmount(index)
			},
		   recalcRowAmount(index) {
			   let record =this.dataForm.tableFieldf4dfaf[index]
			   
		    try {
		      const qty = Number(record.F_SaleQty) || 0;
		      const discountInput = Number(record.F_Discount) || 10;
		      // discount input is in '折' (e.g. 5 => 5折 => 50% => 0.5)
		      const discountMultiplier = Number.isFinite(discountInput) && !Number.isNaN(discountInput) ? discountInput / 10 : 1;
		      const unitPrice = Number(record.F_Price) || 0;
		      const originalAmount = unitPrice * qty;
		      const amountAfter = originalAmount * discountMultiplier;
		      record.F_AmountAfterDiscount = Number.isFinite(amountAfter) ? parseFloat(amountAfter.toFixed(2)) : 0;
		      // 优惠金额 = 原价 - 折后金额
		      const discountAmount = originalAmount - record.F_AmountAfterDiscount;
		      record.F_DiscountAmount = Number.isFinite(discountAmount) ? parseFloat(discountAmount.toFixed(2)) : 0;
		    } catch (e) {
		      // ignore
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



