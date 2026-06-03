<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" :rules="rules" ref="dataForm" :errorType="['toast']" label-position="left" :label-width="100 * 1.5" label-align="right" class="jnpf-form">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='其他入库单号' prop="F_StockInNo"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_StockInNo" placeholder='请填写，忽略将自动生成'  :showCount='false' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='供应商' prop="F_SupplierId"  :label-width="100 * 1.5">
					<JnpfSelect v-model="dataForm.F_SupplierId" :options="f_SupplierIdOptions" :props="f_SupplierIdProps" placeholder='请选择' />
				</u-form-item>
			</view>
			<!-- <view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='入库仓库' prop="F_WarehouseId" required  :label-width="100 * 1.5">
					<JnpfCascader v-model="dataForm.F_WarehouseId" :options="f_WarehouseIdOptions" :props="f_WarehouseIdProps" placeholder='请选择' filterable clearable :disabled="Boolean(dataForm.id)"/>
				</u-form-item>
			</view> -->
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='入库类型' prop="F_StockInType"  :label-width="100 * 1.5">
					<JnpfSelect v-model="dataForm.F_StockInType" :options="f_StockInTypeOptions" :props="f_StockInTypeProps" placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='入库日期' prop="F_StockInDate" required  :label-width="100 * 1.5">
					<JnpfDatePicker v-model="dataForm.F_StockInDate" type='date' format='yyyy-MM-dd' placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='收货人' prop="F_StockInUserId"  :label-width="100 * 1.5">
					<JnpfUserSelect v-model="dataForm.F_StockInUserId" placeholder='请选择' selectType='all' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='备注' prop="F_Description"  :label-width="100 * 1.5">
					<JnpfTextarea v-model="dataForm.F_Description" placeholder='请输入'  :showCount='false' />
				</u-form-item>
			</view>
			<view class="jnpf-table">
				<view class="jnpf-table-title u-line-1" >{{'入库商品列表'}}
				</view>
				<view class="u-p-l-20 u-p-r-20 form-item-box">
					<u-form-item label='添加商品'  :label-width="100 * 1.5" @click="addGoods" >
						<!-- <span @click="addGoods">添加</span> -->
						<!-- <JnpfPopupSelect interfaceName='getPopSelect' multiple v-model="tempGoodsSelector" propsValue='F_Id' relationField='F_GoodsName' :columnOptions='tableFieldecf5cb_F_CustomerIdOptions' interfaceId='2008721559174385664' placeholder='选择商品' hasPage :pageSize='20' popupType='dialog' popupTitle='选择数据' popupWidth='70%' :templateJson='tableFieldecf5cb_F_CustomerIdTemplateJson' :vModel="'tableFieldecf5cb-F_CustomerId'" type="popup"  @change="changeGoods" :extraOptions="[]"/> -->
					</u-form-item>
				</view>
				<view class="jnpf-table-item" v-for="(item,i) in dataForm.tableFieldecf5cb" :key="i">
					<view class="jnpf-table-item-title">
						<view class="jnpf-table-item-title-num">({{i+1}})</view>
						<view class="u-flex">
		        <view class="jnpf-table-delete-btn" @click="removeAPuStockOtherItemRow(i,true)">{{this.$t('common.delText','删除')}}</view>
						</view>
                    </view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='商品'  :label-width="100 * 1.5">
							<JnpfPopupSelect  :disabled="!!Boolean(dataForm.id)" v-model="dataForm.tableFieldecf5cb[i].F_CustomerId" propsValue='F_Id' relationField='F_GoodsName' :columnOptions='tableFieldecf5cb_F_CustomerIdOptions' interfaceId='2008721559174385664' placeholder='请选择' hasPage :pageSize='20' popupType='dialog' popupTitle='选择数据' popupWidth='70%' :templateJson='tableFieldecf5cb_F_CustomerIdTemplateJson' :vModel="'tableFieldecf5cb-F_CustomerId'+i" type="popup"/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='库存编码'  :label-width="100 * 1.5">
							<JnpfInput disabled v-model="dataForm.tableFieldecf5cb[i].F_Encoding" :precision='0' :step='1.0'  />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='数量'  :label-width="100 * 1.5">
							<view class="form-value" v-if="getUnitRatioLevels(dataForm.tableFieldecf5cb[i])?.level2?.name">
							    <JnpfInputNumber disabled  @change="handleNumLevel1Change(dataForm.tableFieldecf5cb[i])" :value="1" placeholder="1" :controls="false"  :style="{ width: '80px' }" />
							    <text>{{ getUnitRatioLevels(dataForm.tableFieldecf5cb[i])?.level1?.name }}</text>
							    <JnpfInputNumber  :disabled="!!Boolean(dataForm.id)"  @change="handleNumLevel2Change(dataForm.tableFieldecf5cb[i])" v-model="dataForm.tableFieldecf5cb[i].F_Num" placeholder="请输入" :controls="false" :max="getUnitRatioLevels(dataForm.tableFieldecf5cb[i])?.level2?.qty" :style="{ width: '80px' }" />
							    <text>{{ getUnitRatioLevels(dataForm.tableFieldecf5cb[i])?.level2?.name }}</text>         
							</view>
							<view class="form-value" v-else>
							    <JnpfInputNumber v-model="dataForm.tableFieldecf5cb[i].F_NumLevel2" placeholder="请输入" :step="1.0" disabled :controls="false" :style="{ width: '100%' }" />
								<text>{{ getUnitRatioLevels(dataForm.tableFieldecf5cb[i])?.level1?.name }}</text>
							</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item @click="handleF_Warehouse(i)"  label='入库仓库' prop="F_WarehouseID" required  :label-width="100 * 1.5">
							<!-- <JnpfCascader v-model="dataForm.tableFieldecf5cb[i].F_WarehouseID" :options="f_WarehouseIdOptions" :props="f_WarehouseIdProps" placeholder='请选择' filterable clearable :disabled="Boolean(dataForm.id)"/> -->
							<JnpfCascader v-if='dataForm.tableFieldecf5cb[i].F_WarehouseID' disabled v-model="dataForm.tableFieldecf5cb[i].F_WarehouseID" :options="f_WarehouseIdOptions" :props="f_WarehouseIdProps" placeholder='请选择' filterable clearable />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='成本单价(元)'  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableFieldecf5cb[i].F_Price" :step='1.0'  />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='销售单价(元)'  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableFieldecf5cb[i].F_Sales_Price" :step='1.0'  />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='备注'  :label-width="100 * 1.5">
							<JnpfTextarea v-model="dataForm.tableFieldecf5cb[i].F_Description" placeholder='请输入'  :showCount='false' />
						</u-form-item>
					</view>
                </view>
<!-- 				<view class="jnpf-table-footer-btn" >
					<view class="jnpf-table-btn jnpf-table-primary-btn" @click="addAPuStockOtherItemRow()">
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
	import { getDictionaryDataSelector,getDataInterfaceRes,getDataInterGoodsInventoryNo } from '@/api/common'
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
					F_StockInNo:undefined,
					F_SupplierId:undefined,
					F_WarehouseId:[],
					F_StockInType:undefined,
					F_StockInDate:undefined,
					F_StockInUserId:undefined,
					F_Description:undefined,
					F_CreatorUserId:undefined,
					F_CreatorTime:undefined,
					tableFieldecf5cb:[],
					tableFielde1e6d9:[],
                },
                rules: {
					F_WarehouseId:[
						{
							required:true,
							message:this.$t('common.chooseText')+' '+'入库仓库',
							trigger:'change',
							type:'array'
						},
					],
					F_StockInDate:[
						{
							required:true,
							message:this.$t('common.chooseText')+' '+'入库日期',
							trigger:'change',
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
				f_SupplierIdProps:{'label':'F_SupplierName','value':'F_Id'},
				f_SupplierIdOptions: [],
				f_WarehouseIdProps:{'label':'F_Name','value':'F_Id',"children": "children"},
				f_WarehouseIdOptions: [],
				f_StockInTypeProps:{'label':'fullName','value':'enCode'},
				f_StockInTypeOptions: [],
				tableFieldecf5cb_F_CustomerIdTemplateJson: [],
				tableFieldecf5cb_F_CustomerIdOptions: [
  {
    "value": "F_GoodsName",
    "label": "商品名称"
  },
  {
    "value": "F_GoodsNo",
    "label": "商品编号"
  },
  {
    "value": "F_Unit",
    "label": "单位"
  },
  {
    "value": "F_Specification",
    "label": "规格"
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
tempGoodsSelector:[]
            };
        },
		watch: {
			'dataForm.tableFieldecf5cb': {
				handler: function(newVal, oldValue) {
				      if (!Array.isArray(newVal)) return;
				      for (let i = 0; i < newVal.length; i++) {
				        const row = newVal[i] || {};
				        const levels = this.getUnitRatioLevels(row);
				        if (levels) {
				          if (levels.level2?.qty) {
				            // 每条最大单位数量固定为 1，二级数量自动换算
				            row.F_NumLevel1 = 1;
				            row.F_NumLevel2 = row.F_Num;
				            // row._F_NumLevel2Max = row.F_NumLevel2;
				            row.F_Num = row.F_Num;
				          } else {
				            // 每条最大单位数量固定为 1，二级数量自动换算
				            row.F_NumLevel1 = 1;
				            row.F_NumLevel2 = 1;
				            // row._F_NumLevel2Max = row.F_NumLevel2;
				            row.F_Num = row.F_NumLevel2;
				          }
				        } else {
				          row.F_Num = row.F_Num;
				          row.F_NumLevel1 = 1;
				          row.F_NumLevel2 = 1;
				        }
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
			this.getf_SupplierIdOptions();
			this.getf_WarehouseIdOptions();
			this.getf_StockInTypeOptions();
            uni.$on('linkPageConfirm', (subVal, Vmodel) => {
				let item = {
						F_CustomerId:undefined,
						F_Num:undefined,
						F_Price:undefined,
						F_Sales_Price:undefined,
						F_Description:undefined,
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
					}
               if ('APuStockOtherItem' === Vmodel) subVal.forEach(t => this.dataForm.tableFieldecf5cb.push({...item,...t}));
			   this.initCollapse();
            });
            uni.$on('linkPageConfirm', (subVal, Vmodel) => {
				let item = {
						F_Title:undefined,
						F_Content:undefined,
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
					}
               if ('APuStockOtherLog' === Vmodel) subVal.forEach(t => this.dataForm.tableFielde1e6d9.push({...item,...t}));
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
			getf_SupplierIdOptions(){
				this.f_SupplierIdOptions = []
				let templateJson = []
				let query = {
					paramList: this.getParamList(templateJson, this.dataForm)
				}
				getDataInterfaceRes('2008457397298925568', query).then(res => {
					let data = res.data
					this.f_SupplierIdOptions = Array.isArray(data) ? data : []
				});
			},
			getf_WarehouseIdOptions(){
				this.f_WarehouseIdOptions = []
				let templateJson = []
				let query = {
					paramList: this.getParamList(templateJson, this.dataForm)
				}
				getDataInterfaceRes('2021045201115680768', query).then(res => {
					let data = res.data
					this.f_WarehouseIdOptions = Array.isArray(data) ? data : []
				});
			},
			getf_StockInTypeOptions(){
				getDictionaryDataSelector('2012074650393251840').then(res => {
					this.f_StockInTypeOptions = res.data.list
				});
			},
    conversionDateTime(type) {
      const format = type === 'yyyy' ? 'yyyy-01-01 00:00:00' : type === 'yyyy-MM' ? 'yyyy-MM-01 00:00:00' :
        type === 'yyyy-MM-dd' ? 'yyyy-MM-dd 00:00:00' : type === 'yyyy-MM-dd HH:mm' ? 'yyyy-MM-dd HH:mm:00' : 'yyyy-MM-dd HH:mm:ss'
      const dataTime = this.jnpf.toDate(new Date(), format)
      return new Date(dataTime).getTime()
    },
	initDefaultData() {
      this.dataForm.F_StockInDate = this.conversionDateTime("yyyy-MM-dd");
      this.dataForm.F_StockInUserId = this.userInfo.userId
	},
			selfInit(){
				this.addAPuStockOtherItem();
				this.addAPuStockOtherLog();
			},
			resetForm() {
				this.dataForm.tableFieldecf5cb = [];
				this.dataForm.tableFielde1e6d9 = [];
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
                        url: '/api/example/APuStockOther/' + this.dataForm.id,
                        method: 'get',
                    }).then(res => {
                        this.dataForm = res.data;
						if(!this.dataForm.F_WarehouseId)this.dataForm.F_WarehouseId=[];
						this.tempGoodsSelector = []
						this.dataForm.tableFieldecf5cb.map(item=>{
							this.tempGoodsSelector.push(item.F_CustomerId);
						})
                    })
				}else{
					this.initDefaultData()
				}
            },
			validateSortCode(list) {
				for (let item of list) {
					if (!item.F_WarehouseID) {
					uni.showToast({
						title: `入库仓库不能为空`,
						icon: 'none',
						duration: 3000
					});
					return false; // 验证失败，返回false
					}
				}
				return true;
			},
			submitForm(type) {
                this.$refs.dataForm.validate(valid => {
                    if (!valid) return
					if (!this.validateSortCode(this.dataForm.tableFieldecf5cb))  return;
                    this.btnLoading = true
                    if (this.dataForm.id) {
                        request({
                            url: '/api/example/APuStockOther/' + this.dataForm.id,
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
                            url: '/api/example/APuStockOther',
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
			addAPuStockOtherItemRow() {
				let item = {
						F_CustomerId:undefined,
						F_Num:undefined,
						F_Price:undefined,
						F_Sales_Price:undefined,
						F_Description:undefined,
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
					}
				this.dataForm.tableFieldecf5cb.push(item)
				this.initCollapse()
			},
			removeAPuStockOtherItemRow(i,showConfirm=false) {
				const handleRemove = () => {
					this.dataForm.tableFieldecf5cb.splice(i, 1)
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
			copyAPuStockOtherItemRow(i) {
				let item = JSON.parse(JSON.stringify(this.dataForm.tableFieldecf5cb[i]));
				item.id = '';
				item.length && item.map(o => delete o.rowData);
				item.F_CreatorUserId = undefined;
				item.F_CreatorTime = undefined;
				this.dataForm.tableFieldecf5cb.push(item);
			},
			addAPuStockOtherLogRow() {
				let item = {
						F_Title:undefined,
						F_Content:undefined,
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
					}
				this.dataForm.tableFielde1e6d9.push(item)
				this.initCollapse()
			},
			removeAPuStockOtherLogRow(i,showConfirm=false) {
				const handleRemove = () => {
					this.dataForm.tableFielde1e6d9.splice(i, 1)
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
			copyAPuStockOtherLogRow(i) {
				let item = JSON.parse(JSON.stringify(this.dataForm.tableFielde1e6d9[i]));
				item.id = '';
				item.length && item.map(o => delete o.rowData);
				item.F_CreatorUserId = undefined;
				item.F_CreatorTime = undefined;
				this.dataForm.tableFielde1e6d9.push(item);
			},
			changeWarehouseId(e){
				// this.tableFieldecf5cb_F_CustomerIdTemplateJson[0].defaultValue=e
			},
			changeGoods(ids,option){
				/* 兜底 */
				if (!Array.isArray(this.dataForm.tableFieldecf5cb)) {
				 this.dataForm.tableFieldecf5cb = [];
				}
				const optionIdSet = new Set(option.map(o => o.id));
							
				/*  删掉已取消勾选的行 */
				this.dataForm.tableFieldecf5cb = this.dataForm.tableFieldecf5cb.filter(item => optionIdSet.has(item.F_WorkOrderId));
				option.forEach(o => {
				 const exist = this.dataForm.tableFieldecf5cb.some(item => item.F_WorkOrderId === o.F_Id);	
				if (!exist) {
				 let item = {
						F_CustomerId:o.F_Id,
						F_Num:undefined,
						F_Price:undefined,
						F_Sales_Price:undefined,
						F_Description:undefined,
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
					}
				   this.dataForm.tableFieldecf5cb.push(item);
				 }
				});
			},
			handleF_Warehouse(i){
				if(this.dataForm.id) return
				uni.scanCode({
				  scanType: ['barCode'],
				  success: (res) => {
				    console.log('扫描结果，', res.result)
					let query = {
					  paramList: [
					    {
					      "field": "id",
					      "defaultValue": '2014233789731049472',
					      "id": "cf7fca",
					      "dataType": "varchar",
					      "required": "0",
					      "fieldName": "",
					      "source": null
					    }
					  ]
									
					}
					getDataInterfaceRes('2038551415311437824', query).then(res => {
					  let data = res.data
					  this.dataForm.tableFieldecf5cb[i].F_WarehouseID = data.F_ParentId
					  console.log(data, '二维码结果');	
					});
				  }
				});
			},
			addGoods(){
				uni.scanCode({
					scanType: ['barCode'],
					success:  (res)=> {
						console.log('扫描结果，',res.result)
						let query = {paramList: [{"field": "goodsNo","defaultValue": res.result,"id": "b6217b","dataType": "varchar","required": "0","fieldName": "库存编码","source": null}]
						}
						getDataInterGoodsInventoryNo('2036268230699520000', query).then(res => {
							let data = res.data
							let temp={
								...data,
								F_CustomerId:data.id,
								F_Encoding:data.F_InventoryNo,
							}
							this.dataForm.tableFieldecf5cb.push(temp);
						});
					}
				});
			},
			// ======================计算===================start
			  // 解析 F_Unit_Ratio，只取前两级，返回 { level1: { name, qty }, level2: { name, qty } } 或 null
			   getUnitRatioLevels(record) {
			    const raw = record?.F_Unit_Ratio;
			    if (!raw) return null;
			    try {
			      const str = typeof raw === 'string' ? raw : String(raw);
			      const arr = JSON.parse(str);
			      if (!Array.isArray(arr)) return null;
			      if (arr.length < 2) return { level1: arr[0], level2: null };
			      return { level1: arr[0], level2: arr[1] };
			    } catch (e) {
			      return null;
			    }
			     },
			  // 初始化数量级别：新增时计算默认值，回显时使用 F_Num 的值
			  // F_NumLevel1 固定为1（因为总是1箱）
			   fillNumLevelsFromF_Num(row) {
			    const levels = this.getUnitRatioLevels(row);
			    if (!levels) return;
			    // 如果 F_Num 有值（回显情况），使用 F_Num 的值；否则（新增情况）根据比例计算默认值
			    if (row.F_Num !== undefined && row.F_Num !== null && row.F_Num !== '') {
			      const qty2 = Number(row.F_Num) || 0;
			      const ratio = Number(levels.level2?.qty) || 0;
			      // 回显：F_Num 存的是 F_NumLevel2 的值
			      row.F_NumLevel2 = qty2;
			      row._F_NumLevel2Max = qty2;
			      if (ratio > 0) {
			        row.F_NumLevel1 = qty2 / ratio;
			      } else {
			        row.F_NumLevel1 = qty2;
			      }
			    } else {
			      // 新增：根据比例计算默认值 = 1 * level2.qty
			      row.F_NumLevel1 = 1;
			      const ratio = Number(levels.level2?.qty) || 0;
			      row.F_NumLevel2 = 1 * ratio;
			      row._F_NumLevel2Max = row.F_NumLevel2;
			      row.F_Num = row.F_NumLevel2;
			    }
			  },
			    // 一级数量（如箱子）改变时，按比例计算二级数量（如盒）：二级 = 一级 * level2.qty
			     handleNumLevel1Change(record) {
			      const levels = this.getUnitRatioLevels(record);
			      if (!levels) return;
			      const qty1 = Number(record.F_NumLevel1) || 0;
			      const ratio = Number(levels.level2?.qty) || 0;
			      record.F_NumLevel2 = qty1 * ratio;
			      record._F_NumLevel2Max = record.F_NumLevel2;
			      record.F_Num = record.F_NumLevel2;
			    },
			  
			    /** 二级数量（最小单位）编辑：不超过该行默认/换算上限，并与 F_Num 同步 */
			     handleNumLevel2Change(record) {
			      const levels = this.getUnitRatioLevels(record);
			      if (!levels) return;
			      let v = Number(record.F_NumLevel2);
			      if (Number.isNaN(v)) v = 0;
			      const max = Number(record._F_NumLevel2Max);
			      if (record._F_NumLevel2Max != null && !Number.isNaN(max) && v > max) {
			        v = max;
			        record.F_NumLevel2 = v;
			      }
			      record.F_Num = v;
			    }
			// ======================计算===================end
        }
    };
</script>
<style lang="scss" scoped>
        page{
                background-color: #f0f2f6;
        }
		.form-value{
			width: 100%;
			display: flex;
			justify-content: flex-end;
		}
</style>



