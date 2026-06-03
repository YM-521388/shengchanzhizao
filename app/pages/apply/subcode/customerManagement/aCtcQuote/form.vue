<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" :rules="rules" ref="dataForm" :errorType="['toast']" label-position="left" :label-width="100 * 1.5" label-align="right" class="jnpf-form">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='报价单号' prop="F_QuoteCode"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_QuoteCode" placeholder='请填写，忽略将自动生成'  :showCount='false' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='客户' prop="F_CustomerId" required  :label-width="100 * 1.5">
					<JnpfSelect v-model="dataForm.F_CustomerId" :options="f_CustomerIdOptions" :props="f_CustomerIdProps" placeholder='请选择' @change="customerChange"/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='联系人' prop="F_ContactId"  :label-width="100 * 1.5">
					<JnpfPopupSelect v-model='dataForm.F_ContactId' propsValue='F_Id' relationField='F_ContactName' :columnOptions='F_ContactIdOptions' interfaceId='2009459664370143232' placeholder='请选择' hasPage :pageSize='20' popupType='dialog' popupTitle='选择数据' popupWidth='800px' :templateJson='F_ContactIdTemplateJson' vModel="F_ContactId"  :formData='dataForm' type="popup" :extraOptions='[]' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='客户地址' prop="F_AddressId"  :label-width="100 * 1.5">
					<JnpfSelect v-model="dataForm.F_AddressId" :options="f_AddressIdOptions" :props="f_AddressIdProps" placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='报价金额' prop="F_QuoteAmount"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_QuoteAmount" :step='1.0'  disabled placeholder=''/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='商品总数' prop="F_GoodsTotal"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_GoodsTotal"  :min='0' :step='1.0'  disabled placeholder=''/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='交货日期' prop="F_DeliveryDate"  :label-width="100 * 1.5">
					<JnpfDatePicker v-model="dataForm.F_DeliveryDate" type='date' format='yyyy-MM-dd' placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='报价日期' prop="F_QuoteDate"  :label-width="100 * 1.5">
					<JnpfDatePicker v-model="dataForm.F_QuoteDate" type='date' format='yyyy-MM-dd' placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='业务员' prop="F_SalesmanId" required  :label-width="100 * 1.5">
					<JnpfUserSelect v-model="dataForm.F_SalesmanId" placeholder='请选择' selectType='all' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='备注' prop="F_Description"  :label-width="100 * 1.5">
					<JnpfTextarea v-model="dataForm.F_Description" placeholder='请输入'  :showCount='true' />
				</u-form-item>
			</view>
			<view class="jnpf-table">
				<view class="jnpf-table-title u-line-1" >{{'选择报价单商品'}}
				</view>
				<view class="jnpf-table-item" v-for="(item,i) in dataForm.tableFielddc29f7" :key="i">
					<view class="jnpf-table-item-title">
						<view class="jnpf-table-item-title-num">({{i+1}})</view>
						<view class="u-flex">
		        <view class="jnpf-table-delete-btn" @click="removeACtcQuoteItemRow(i,true)">{{this.$t('common.delText','删除')}}</view>
						</view>
                    </view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='商品'  :label-width="100 * 1.5">
							<JnpfPopupSelect v-model="dataForm.tableFielddc29f7[i].F_CustomerId" propsValue='F_Id' relationField='F_GoodsName' :columnOptions='tableFielddc29f7_F_CustomerIdOptions' interfaceId='2008721559174385664' placeholder='请选择' hasPage :pageSize='20' popupType='dialog' popupTitle='选择数据' popupWidth='60%' :templateJson='tableFielddc29f7_F_CustomerIdTemplateJson' :vModel="'tableFielddc29f7-F_CustomerId'+i" type="popup"/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='单价'  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableFielddc29f7[i].F_Price" :step='1.0' @change="updateRowAmount(i)" />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='销售数量'  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableFielddc29f7[i].F_SaleQty" :step='1.0'  @change="(e) => handleSaleQty(e,i)"/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='折扣'  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableFielddc29f7[i].F_Discount" :step='1.0'  @change="(e) => handleDiscount(e,i)"/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='优惠金额'  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableFielddc29f7[i].F_DiscountAmount" :step='1.0'   @change="onDiscountMoneyChange(i)"/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='商品备注'  :label-width="100 * 1.5">
							<JnpfInput v-model="dataForm.tableFielddc29f7[i].F_Description" placeholder='请输入'  :showCount='false' />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='附件'  :label-width="100 * 1.5">
							<JnpfUploadFile v-model="dataForm.tableFielddc29f7[i].F_Files" :limit='9' sizeUnit='MB' :fileSize='10' pathType="defaultPath"  :sortRule='[]' timeFormat='YYYY' buttonText='点击上传' />
						</u-form-item>
					</view>
                </view>
				<view class="jnpf-table-footer-btn" >
					<view class="jnpf-table-btn jnpf-table-primary-btn" @click="addACtcQuoteItemRow()">
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
					F_QuoteCode:undefined,
					F_CustomerId:undefined,
					F_ContactId:undefined,
					F_AddressId:undefined,
					F_QuoteAmount:0,
					F_GoodsTotal:0,
					F_DeliveryDate:undefined,
					F_QuoteDate:undefined,
					F_SalesmanId:undefined,
					F_QuoteStatus:undefined,
					F_AuditStatus:undefined,
					F_Description:undefined,
					tableFielddc29f7:[],
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
					F_SalesmanId:[
						{
							required:true,
							message:this.$t('common.inputText')+' '+'业务员',
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
				userInfo:{},
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
				F_ContactIdOptions: [
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
				tableFielddc29f7_F_CustomerIdTemplateJson: [],
				tableFielddc29f7_F_CustomerIdOptions: [
  {
    "value": "F_GoodsNo",
    "label": "编号"
  },
  {
    "value": "F_GoodsName",
    "label": "商品名称"
  },
  {
    "value": "F_Unit",
    "label": "单位"
  },
  {
    "value": "F_Specification",
    "label": "规则"
  },
  {
    "value": "F_SalePrice",
    "label": "销售单价"
  },
  {
    "value": "F_CostPrice",
    "label": "成本单价"
  }
],
            };
        },
		watch: {
			'dataForm.tableFielddc29f7': {
				handler: function(list, oldValue) {
					        try {
					          // 计算销售数量总和（F_SaleQty），保留两位小数
					          const totalSaleQty = (Array.isArray(list) ? list : []).reduce((acc, row) => {
					            const qty = Number(row.F_SaleQty) || 0;
					            return acc + (isNaN(qty) ? 0 : qty);
					          }, 0);
					          this.dataForm.F_GoodsTotal = Math.round(totalSaleQty * 100) / 100;
					          // 计算子表金额合计并赋值到报价金额（F_QuoteAmount）
					          try {
					            const sum = (Array.isArray(list) ? list : []).reduce((acc, row) => {
					              // 优先使用后端可能已填的 F_Amount，否则尝试用价格*数量计算
					              const amount =
					                typeof row.F_Amount !== 'undefined' && row.F_Amount !== null ? Number(row.F_Amount) : (Number(row.F_Price) || 0) * (Number(row.F_SaleQty) || 0);
					              return acc + (isNaN(amount) ? 0 : amount);
					            }, 0);
					            // 保留两位小数
					            this.dataForm.F_QuoteAmount = Math.round(sum * 100) / 100;
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
            uni.$on('linkPageConfirm', (subVal, Vmodel) => {
				let item = {
						F_CustomerId:"2008720094968025088",
						F_Price:undefined,
						F_SaleQty:undefined,
						F_Discount:undefined,
						F_DiscountAmount:0,
						F_Description:undefined,
						F_Files:[],
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
					}
               if ('ACtcQuoteItem' === Vmodel) subVal.forEach(t => this.dataForm.tableFielddc29f7.push({...item,...t}));
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
					this.f_CustomerIdOptions = Array.isArray(data) ? data :[]			

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
					this.f_AddressIdOptions = Array.isArray(data) ? data : [],
					this.dataForm.F_AddressId= this.f_AddressIdOptions[0].F_Id || this.f_AddressIdOptions[0].id;
				});
			},
    conversionDateTime(type) {
      const format = type === 'yyyy' ? 'yyyy-01-01 00:00:00' : type === 'yyyy-MM' ? 'yyyy-MM-01 00:00:00' :
        type === 'yyyy-MM-dd' ? 'yyyy-MM-dd 00:00:00' : type === 'yyyy-MM-dd HH:mm' ? 'yyyy-MM-dd HH:mm:00' : 'yyyy-MM-dd HH:mm:ss'
      const dataTime = this.jnpf.toDate(new Date(), format)
      return new Date(dataTime).getTime()
    },
	initDefaultData() {
      this.dataForm.F_SalesmanId = this.userInfo.userId
	},
			selfInit(){
				this.addACtcQuoteItem();
			},
			resetForm() {
				this.dataForm.tableFielddc29f7 = [];
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
                        url: '/api/example/ACtcQuote/' + this.dataForm.id,
                        method: 'get',
                    }).then(res => {
                        this.dataForm = res.data;
						this.getf_AddressIdOptions();
						this.F_ContactIdTemplateJson[0].relationField=this.dataForm.F_CustomerId
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
                            url: '/api/example/ACtcQuote/' + this.dataForm.id,
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
                            url: '/api/example/ACtcQuote',
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
			addACtcQuoteItemRow() {
				let item = {
						F_CustomerId:"2008720094968025088",
						F_Price:undefined,
						F_SaleQty:undefined,
						F_Discount:undefined,
						F_DiscountAmount:0,
						F_Description:undefined,
						F_Files:[],
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
					}
				this.dataForm.tableFielddc29f7.push(item)
				this.initCollapse()
			},
			removeACtcQuoteItemRow(i,showConfirm=false) {
				const handleRemove = () => {
					this.dataForm.tableFielddc29f7.splice(i, 1)
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
			copyACtcQuoteItemRow(i) {
				let item = JSON.parse(JSON.stringify(this.dataForm.tableFielddc29f7[i]));
				item.id = '';
				item.length && item.map(o => delete o.rowData);
				item.F_CreatorUserId = undefined;
				item.F_CreatorTime = undefined;
				this.dataForm.tableFielddc29f7.push(item);
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
			// 销售
			handleSaleQty(e,index){
			let record =this.dataForm.tableFielddc29f7[index]
			if (!record) return;
			record.F_SaleQty=e
			this.updateRowAmount(index)
			},
			updateRowAmount(index) {
				let record =this.dataForm.tableFielddc29f7[index]
				if (!record) return;
				
			    try {
			      record.F_Amount = (Number(record.F_Price) || 0) * (Number(record.F_SaleQty) || 0);
				  const factor = record.F_Discount ? Number(record.F_Discount) / 10 : 1;
			      record.F_AfterDiscount = Math.round(record.F_Amount * factor * 100) / 100;
			      record.F_DiscountAmount = Math.round(((record.F_Amount || 0) - (record.F_AfterDiscount || 0)) * 100) / 100;
			   
				} catch (e) {
			      record.F_Amount = 0;
			      record.F_AfterDiscount = 0;
			    }
			},
			// 折扣
			handleDiscount(e,index){
			let record =this.dataForm.tableFielddc29f7[index]
			record.F_Discount=e
			this.updateRowAmount(index)
			},
			/**
			   * 当人工修改 优惠金额(F_DiscountMoney) 时，同步计算 优惠后金额 和 折扣 字段
			   */
			onDiscountMoneyChange(index){
				let record =this.dataForm.tableFielddc29f7[index]
				    if (!record) return;
				    const rawVal = Number(record.F_DiscountMoney) || 0;
				    // 计算金额优先尝试使用已存在的 F_Amount，否则用单价*数量
				    const amount = Number(record.F_Amount) || (Number(record.F_Price) || 0) * (Number(record.F_Num) || 0);
				    let val = rawVal;
				    if (val < 0) val = 0;
				    if (val > amount) val = amount;
				    record.F_DiscountMoney = Number(val.toFixed(2));
				    const discountedAmount = amount - record.F_DiscountMoney;
				    record.F_DiscountedAmount = Number(discountedAmount.toFixed(2));
				    // 维护折扣字段的语义（折扣系数 = discountedAmount / amount -> 折扣输入为折扣系数*10）
				    if (amount > 0) {
				      record.F_Discount = Number(((discountedAmount / amount) * 10).toFixed(2));
				    } else {
				      record.F_Discount = 0;
				    }
				    // 同步主表金额（子表优惠后金额之和）
				    // this.syncMoneySum();
			},
			
        }
    };
</script>
<style>
        page{
                background-color: #f0f2f6;
        }
</style>



