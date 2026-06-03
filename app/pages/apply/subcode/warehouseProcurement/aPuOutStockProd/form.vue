<!-- 选择仓库--工单类型--工单号--商品列表 -->
<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" :rules="rules" ref="dataForm" :errorType="['toast']" label-position="left"
			:label-width="100 * 1.5" label-align="right" class="jnpf-form">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='生产出库单号' prop="F_StockOutNo" :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_StockOutNo" placeholder='请填写，忽略将自动生成' :showCount='false' />
				</u-form-item>
			</view>
<!-- 			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='仓库' prop="F_WarehouseId" required :label-width="100 * 1.5">
				<JnpfCascader @change='handleWorkOrder' v-model="dataForm.F_WarehouseId"
						:options="f_WarehouseIdOptions" :props="f_WarehouseIdProps" placeholder='请选择'
						:disabled="Boolean(dataForm.id)"/>
				</u-form-item>
			</view> -->
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='出库类型' prop="F_StockOutType" :label-width="100 * 1.5">
					<JnpfSelect v-model="dataForm.F_StockOutType" :options="f_StockOutTypeOptions"
						:props="f_StockOutTypeProps" placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='出库日期' prop="F_StockOutDate" :label-width="100 * 1.5">
					<JnpfDatePicker v-model="dataForm.F_StockOutDate" type='date' format='yyyy-MM-dd'
						placeholder='请选择' />
				</u-form-item>
			</view>


			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='工单类型' prop="F_Type" :label-width="100 * 1.5">
					<JnpfSelect v-model="dataForm.F_Type" :options="f_TypeOptions" :props="f_TypeProps" filterable
						placeholder='请选择' :disabled="Boolean(dataForm.id || WorkOrderId != '')" @change="ProcessBtn" />
				</u-form-item>
			</view>
			<u-form-item label='工单号' prop="F_WorkOrderId" required :label-width="100 * 1.5">
				<JnpfSelect v-model="dataForm.F_WorkOrderId" :options="f_WorkOrderIdOptions" :props="f_WorkOrderIdProps"
					placeholder='请选择'  @change="handleWorkOrder" />
			</u-form-item>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='出库套数' prop="F_Num" :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_Num" :precision='0' :step='1.0' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='备注' prop="F_Description" :label-width="100 * 1.5">
					<JnpfTextarea v-model="dataForm.F_Description" placeholder='请输入' :showCount='false' />
				</u-form-item>
			</view>
			<view class="jnpf-table">
				<view class="jnpf-table-title u-line-1">{{'商品'}}
				</view>
				<view class="u-p-l-20 u-p-r-20 form-item-box">
					<u-form-item label='添加商品'  :label-width="100 * 1.5" @click="addGoods" >
					</u-form-item>
				</view>
				<view class="jnpf-table-item" v-for="(item,i) in dataForm.tableField2752cf" :key="i">
					<view class="jnpf-table-item-title">
						<view class="jnpf-table-item-title-num">({{i+1}})</view>
						<view class="u-flex">
							<view class="jnpf-table-delete-btn" @click="removeAPuOutStockProdItemRow(i,true)">
								{{this.$t('common.delText','删除')}}</view>
						</view>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='商品' :label-width="100 * 1.5">
							<JnpfSelect disabled v-model="dataForm.tableField2752cf[i].F_CustomerId"
								:options="tableField2752cf_F_CustomerIdOptions"
								:props="tableField2752cf_F_CustomerIdProps" filterable placeholder='请选择' />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='库存' :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableField2752cf[i].F_Num" thousands :controls='false'
								:step='1.0' disabled/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='库存编码' :label-width="100 * 1.5">
							<JnpfInput v-model="dataForm.tableField2752cf[i].F_Encoding" disabled />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='出库数量' :label-width="100 * 1.5">
				          <view class="form-value" v-if="getUnitRatioLevels(dataForm.tableField2752cf[i])?.level2?.name">
				            <JnpfInputNumber
				              v-model="dataForm.tableField2752cf[i].F_NumLevel1"
				              :placeholder="dataForm.tableField2752cf[i]._quantityDisabled ? '自动带出' : '请输入'"
				              :controls="false"
				              disabled
				              :style="{ width: '80px' }"
				              @change="handleNumLevel1Change(dataForm.tableField2752cf[i], i)" />
				            <text>{{ getUnitRatioLevels(dataForm.tableField2752cf[i])?.level1?.name }}</text>
				            <JnpfInputNumber
				              v-model="dataForm.tableField2752cf[i].F_NumLevel2"
				              placeholder="请输入"
				              :controls="false"
				              :min="1"
				              :disabled="Boolean(dataForm.id)"
				              :max="dataForm.tableField2752cf[i]._F_NumLevel2Max"
				              :style="{ width: '80px' }"
				              @change="handleNumLevel2Change(dataForm.tableField2752cf[i], i)" />
				            <text>{{ getUnitRatioLevels(dataForm.tableField2752cf[i])?.level2?.name }}</text>
				          </view>
				          <view class="form-value" v-else>
				            <JnpfInputNumber
				              v-model="dataForm.tableField2752cf[i].F_NumLevel2"
				              placeholder="请输入"
				              :controls="false"
				              disabled
				              :min="1"
				              :max="dataForm.tableField2752cf[i]._F_NumLevel2Max"
				              :style="{ width: '80px' }"
				              @change="handleNumLevel2Change(dataForm.tableField2752cf[i])" />
				            <text>{{ getUnitRatioLevels(dataForm.tableField2752cf[i])?.level1?.name }}</text>
				          </view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='成本单价(元)' :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableField2752cf[i].F_Price" thousands :controls='false'
								:step='1.0' />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='入库仓库' prop="F_WarehouseID" required :label-width="100 * 1.5">
							<JnpfCascader :disabled="Boolean(dataForm.id)" v-model="dataForm.tableField2752cf[i].F_WarehouseID" :options="f_WarehouseIdOptions" :props="f_WarehouseIdProps" placeholder='请选择' filterable clearable />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='备注' :label-width="100 * 1.5">
							<JnpfTextarea v-model="dataForm.tableField2752cf[i].F_Description" placeholder='请输入'
								:showCount='false' />
						</u-form-item>
					</view>
				</view>
<!-- 				<view class="jnpf-table-footer-btn">
					<view class="jnpf-table-btn jnpf-table-primary-btn" @click="addAPuOutStockProdItemRow()">
						<text class="jnpf-table-btn-icon icon-ym icon-ym-btn-add" />
						<text class="jnpf-table-btn-text">{{this.$t('common.add1Text','添加')}}</text>
					</view>
				</view> -->
			</view>

		</u-form>
		<view class="buttom-actions" v-if="jurisdictionType != 'btn_detail'">
			<CustomButton :btnText="$t('common.cancelText')" btnIcon="icon-ym icon-ym-add-cancel" customIcon />
			<u-button class="buttom-btn" type="primary" @click="submitForm"
				:loading="btnLoading">{{$t('common.okText','确定')}}</u-button>
		</view>
	</view>
</template>
<script>
	import {
		getDictionaryDataSelector,
		getDataInterfaceRes,
		GetProcessingGoodsList,
		GetOutsourceGoodsList,
		getDataInterGoodsInventoryNo,
		getDataInterfaceDataInfoByIds
	} from '@/api/common'
	import {
		useBaseStore
	} from '@/store/modules/base'
	import request from '@/utils/request'
	import CustomButton from '@/components/CustomButton'
	export default {
		components: {
			CustomButton
		},
		data() {
			return {
				keyCode: +new Date(),
				btnLoading: false,
				addTableConf: {},
				dataForm: {
					id: '',
					F_StockOutNo: undefined,
					F_WarehouseId: [],
					F_StockOutType: undefined,
					F_StockOutDate: undefined,
					F_Type: "0",
					F_WorkOrderId: undefined,
					F_Num: undefined,
					F_Method: undefined,
					F_Description: undefined,
					F_CreatorUserId: undefined,
					F_CreatorTime: undefined,
					tableField2752cf: [],
					tableField49228c: [],
				},
				rules: {
					F_WarehouseId: [{
						required: true,
						message: this.$t('common.chooseText') + ' ' + '仓库',
						trigger: 'change',
						type: 'array'
					}, ],
					F_Type: [{
						required: true,
						message: this.$t('common.inputText') + ' ' + '工单类型',
						trigger: 'change',
					}, ],
					F_WorkOrderId: [{
						required: true,
						message: this.$t('common.chooseText') + ' ' + '工单号',
						trigger: 'change',
					}, ],
				},
				ruleList: {},
				regList: {},
				index: 0,
				idList: [],
				dataValue: {},
				jurisdictionType: '',
				addType: 0,
				f_WarehouseIdProps: {
					'label': 'F_Name',
					'value': 'F_Id',
					"children": "children"
				},
				f_WarehouseIdOptions: [],
				f_StockOutTypeProps: {
					'label': 'fullName',
					'value': 'enCode'
				},
				f_StockOutTypeOptions: [],
				f_TypeProps: {
					'label': 'fullName',
					'value': 'enCode'
				},
				f_TypeOptions: [],
				f_WorkOrderIdProps: {
					'label': 'F_ProcessNo',
					'value': 'F_Id'
				},
				f_WorkOrderIdOptions: [],
				f_MethodProps: {
					'label': 'fullName',
					'value': 'enCode'
				},
				F_MethodOptions: [],
				tableField2752cf_F_CustomerIdTemplateJson: [],
				tableField2752cf_F_CustomerIdOptions: [],
				tableField2752cf_F_CustomerIdProps: {
					'label': 'F_GoodsName',
					'value': 'id'
				},
				WorkOrderId:''
			};
		},
		watch: {
			'dataForm.tableField2752cf': {
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
			this.jurisdictionType = option.jurisdictionType
			let _title = ''
			if (option.jurisdictionType == 'btn_add') {
				_title = this.$t('common.add2Text', '新增')
			}
			if (option.jurisdictionType == 'btn_edit') {
				_title = this.$t('common.editText', '编辑')
			}
			if (option.jurisdictionType == 'btn_detail') {
				_title = this.$t('common.detailText', '详情')
			}
			this.dataForm.id = option.id || ''
			
			if(option.F_ProcessId){
				this.dataForm.F_WorkOrderId = option.F_ProcessId||''
				this.dataForm.F_Type  = option.F_Type||''
				this.WorkOrderId = option.F_ProcessId;
			}else{
				this.dataForm.F_Type = '0';
				this.dataForm.F_WorkOrderId = undefined;
				this.WorkOrderId = '';
			}
			this.idList = option.idList ? option.idList.split(",") : []
			uni.setNavigationBarTitle({
				title: _title
			})
			this.dataValue = JSON.parse(JSON.stringify(this.dataForm))
			this.initData()
			this.getf_WarehouseIdOptions();
			this.getf_StockOutTypeOptions();
			this.getf_TypeOptions();
			this.getf_WorkOrderIdOptions();
			this.getf_MethodOptions();
			this.gettableField2752cf_F_CustomerIdOptions()
			uni.$on('linkPageConfirm', (subVal, Vmodel) => {
				let item = {
					F_CustomerId: undefined,
					F_Num: undefined,
					F_Price: undefined,
					F_Description: undefined,
					F_CreatorUserId: undefined,
					F_CreatorTime: undefined,
				}
				if ('APuOutStockProdItem' === Vmodel) subVal.forEach(t => this.dataForm.tableField2752cf.push({
					...item,
					...t
				}));
				this.initCollapse();
			});
			uni.$on('linkPageConfirm', (subVal, Vmodel) => {
				let item = {
					F_Type: undefined,
					F_Title: undefined,
					F_Content: undefined,
					F_Reason: undefined,
					F_CreatorUserId: undefined,
					F_CreatorTime: undefined,
				}
				if ('APuOutStockProdLog' === Vmodel) subVal.forEach(t => this.dataForm.tableField49228c.push({
					...item,
					...t
				}));
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
			getf_WarehouseIdOptions() {
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
			getf_StockOutTypeOptions() {
				getDictionaryDataSelector('2013096194263355392').then(res => {
					this.f_StockOutTypeOptions = res.data.list
				});
			},
			getf_TypeOptions() {
				getDictionaryDataSelector('2014894783159472128').then(res => {
					this.f_TypeOptions = res.data.list
				});
			},
			getf_WorkOrderIdOptions() {
				this.f_WorkOrderIdOptions = []
				let templateJson = []
				let query = {
					paramList: []
				}
				getDataInterfaceRes('2013860549426810880', query).then(res => {
					let data = res.data
					this.f_WorkOrderIdOptions = Array.isArray(data) ? data : []
				});
			},
			getf_MethodOptions() {
				getDictionaryDataSelector('2014907575941861376').then(res => {
					this.f_MethodOptions = res.data.list
				});
			},

			gettableField2752cf_F_CustomerIdOptions() {
				let query = {
					paramList: [{
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
					}]
				}
				getDataInterfaceRes('2012085692393459712').then(res => {
					let data = res.data
					this.tableField2752cf_F_CustomerIdOptions = Array.isArray(data) ? data : []
				});
			},
			selfInit() {
				this.addAPuOutStockProdItem();
				this.addAPuOutStockProdLog();
			},
			resetForm() {
				this.dataForm.tableField2752cf = [];
				this.dataForm.tableField49228c = [];
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
								if (item[n] && !eval(element.pattern).test(item[n])) {
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
						url: '/api/example/APuOutStockProd/' + this.dataForm.id,
						method: 'get',
					}).then(async res => {
						this.dataForm = res.data;
						if(!this.dataForm.F_WarehouseId)this.dataForm.F_WarehouseId=[];
						if (this.dataForm.F_Type == '0') {
							this.getf_WorkOrderIdOptions();
						} else if (this.dataForm.F_Type == '1') {
						    this.getF_OutsourceIdOptions();
						}
						
						await this.enrichTableRowsGoodsMeta(this.dataForm.tableField2752cf);
					   for (const row of this.dataForm.tableField2752cf) {
					        this.fillNumLevelsFromF_Num(row);
					      }
					})
				}
			},
			submitForm(type) {
				this.$refs.dataForm.validate(valid => {
					if (!valid) return
					this.btnLoading = true
					if (this.dataForm.id) {
						request({
							url: '/api/example/APuOutStockProd/' + this.dataForm.id,
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
						}).catch(() => {
							this.btnLoading = false
						})
					} else {
						request({
							url: '/api/example/APuOutStockProd',
							method: 'post',
							data: this.dataForm,
						}).then(res => {
							uni.showToast({
								title: res.msg,
								complete: () => {
									setTimeout(() => {
										if (type == 1) {
											this.dataForm = JSON.parse(JSON.stringify(
												this.dataValue))
										} else {
											uni.$emit('refresh')
											uni.navigateBack()
										}
										this.btnLoading = false
									}, 1500)
								}
							})
						}).catch(() => {
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
							templateJson[i].defaultValue = formData[tableVModel] && formData[tableVModel][index] &&
								formData[tableVModel][index][childVModel] || ''
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
			openSelectDialog(key, value) {
				let data = {
					actionConfig: this.addTableConf[key + 'List' + value],
					formData: this.dataForm,
					tableVmodel: key
				}
				uni.navigateTo({
					url: '/pages/apply/tableLinkage/index?data=' + encodeURIComponent(JSON.stringify(data))
				})
			},
			addAPuOutStockProdItemRow() {
				let item = {
					F_CustomerId: undefined,
					F_Num: undefined,
					F_Price: undefined,
					F_Description: undefined,
					F_CreatorUserId: undefined,
					F_CreatorTime: undefined,
				}
				this.dataForm.tableField2752cf.push(item)
				this.initCollapse()
			},
			removeAPuOutStockProdItemRow(i, showConfirm = false) {
				const handleRemove = () => {
					this.dataForm.tableField2752cf.splice(i, 1)
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
			copyAPuOutStockProdItemRow(i) {
				let item = JSON.parse(JSON.stringify(this.dataForm.tableField2752cf[i]));
				item.id = '';
				item.length && item.map(o => delete o.rowData);
				item.F_CreatorUserId = undefined;
				item.F_CreatorTime = undefined;
				this.dataForm.tableField2752cf.push(item);
			},
			addAPuOutStockProdLogRow() {
				let item = {
					F_Type: undefined,
					F_Title: undefined,
					F_Content: undefined,
					F_Reason: undefined,
					F_CreatorUserId: undefined,
					F_CreatorTime: undefined,
				}
				this.dataForm.tableField49228c.push(item)
				this.initCollapse()
			},
			removeAPuOutStockProdLogRow(i, showConfirm = false) {
				const handleRemove = () => {
					this.dataForm.tableField49228c.splice(i, 1)
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
			copyAPuOutStockProdLogRow(i) {
				let item = JSON.parse(JSON.stringify(this.dataForm.tableField49228c[i]));
				item.id = '';
				item.length && item.map(o => delete o.rowData);
				item.F_CreatorUserId = undefined;
				item.F_CreatorTime = undefined;
				this.dataForm.tableField49228c.push(item);
			},
			// 仓库改变、工单号
			handleWorkOrder() {
				return
				if (this.dataForm.F_WorkOrderId && this.dataForm.F_WarehouseId) {
					switch (this.dataForm.F_Type) {
						case '0':
							this.getProcessingGoods()
							break;
						default:
							this.getOutsourceGoods()
							break;
					}
				}
			},
			// 加工单
			getProcessingGoods() {
				GetProcessingGoodsList(this.dataForm.F_WorkOrderId, this.dataForm.F_WarehouseId).then(res => {
					this.dataForm.tableField2752cf = [];
					res.data.forEach(o => {
						let item = {
							F_CustomerId: o.F_GoodsId,
							F_Num:  o.F_Unit,
							F_Price: undefined,
							F_Description: undefined,
							F_CreatorUserId: undefined,
							F_CreatorTime: undefined,
						}
						this.dataForm.tableField2752cf.push(item)
					});
				});
			},
			// 外协工单
			getOutsourceGoods() {
				GetOutsourceGoodsList(this.dataForm.F_WorkOrderId, this.dataForm.F_WarehouseId).then(res => {
					this.dataForm.tableField2752cf = [];
					res.data.forEach(o => {
						let item = {
							F_CustomerId: o.F_GoodsId,
							F_Num:  o.F_Unit,
							F_Price: undefined,
							F_Description: undefined,
							F_CreatorUserId: undefined,
							F_CreatorTime: undefined,
						}
						this.dataForm.tableField2752cf.push(item)
					});
				});
			},
			  //工单类型
			   ProcessBtn(val, option) {
			    if (val == '0') {
			      this.getf_WorkOrderIdOptions();
			    } else if (val == '1') {
			      this.getF_OutsourceIdOptions();
			    }
			    this.dataForm.F_WorkOrderId = undefined;
			    this.dataForm.tableField2752cf = [];
			  },
			    //外协工单
			     getF_OutsourceIdOptions() {
			      let templateJson = [];
			      let query = {
			        paramList: this.getParamList(templateJson, this.dataForm),
			      };
			      getDataInterfaceRes('2014969808717746176', query).then(res => {
			        let data = res.data;
					this.f_WorkOrderIdOptions = Array.isArray(data) ? data : []
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
								console.log('二维码商品数据',data);
								this.fillNumLevelsFromF_Num(temp);
								this.dataForm.tableField2752cf.push(temp);
							});
						}
					});
				},
				
				  /** 编辑模式下获取商品详情（含 F_Unit_Ratio）用于数量层级显示 */
				  async  enrichTableRowsGoodsMeta(rows) {
				    if (!rows?.length) return;
				    const ids = [
				      ...new Set(
				        rows
				          .map(r => r.F_CustomerId)
				          .filter(id => id != null && id !== '')
				          .map((id) => String(id)),
				      ),
				    ];
				    if (!ids.length) return;
				    const interfaceId = '2008721559174385664';
				    const query = {
				      ids,
				      interfaceId,
				      propsValue: 'F_Id',
				      relationField: 'F_GoodsName',
				      paramList: [],
				    };
				    try {
				      const resp = await getDataInterfaceDataInfoByIds(interfaceId, query);
				      const list = (resp && resp.data) || [];
				      const map = {};
				      list.forEach((item) => {
				        if (item && item.F_Id != null) map[String(item.F_Id)] = item;
				      });
				      rows.forEach((row) => {
				        const id = row.F_CustomerId != null ? String(row.F_CustomerId) : '';
				        const obj = id && map[id];
				        if (obj) {
				          row._F_CustomerObj = obj;
				          row.F_Unit_Ratio = obj.F_Unit_Ratio ?? row.F_Unit_Ratio;
				        }
				      });
				    } catch (e) {
				      console.error('enrichTableRowsGoodsMeta', e);
				    }
				  },
// ===============================计算======================start
  // 解析 F_Unit_Ratio（与 aPuReturnPrd 一致）：两级单位 { level1: { name, qty }, level2: { name, qty } }
   getUnitRatioLevels(record) {
    const raw = record?.F_Unit_Ratio || record?._F_CustomerObj?.F_Unit_Ratio;
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
   handleNumLevel1Change(record, index) {
    const levels = this.getUnitRatioLevels(record);
    if (!levels) return;
    const qty1 = Number(record.F_NumLevel1) || 0;
    const ratio = Number(levels.level2?.qty) || 0;
    record.F_NumLevel2 = qty1 * ratio;
    record._F_NumLevel2Max = record.F_NumLevel2;
    record.F_Num = record.F_NumLevel2;
  },

  /** 二级数量（最小单位）编辑：不超过该行默认/换算上限，并与 F_Num 同步 */
   handleNumLevel2Change(record, index) {
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
  },

  /** 回显：F_Num 存二级数量（最小单位），据此拆成一级 + 二级展示 */
   fillNumLevelsFromF_Num(row) {
    const levels = this.getUnitRatioLevels(row);
    if (!levels || row.F_Num == null || row.F_Num === '') return;
    const qty2 = Number(row.F_Num) || 0;
    const ratio = Number(levels.level2?.qty) || 0;
    row.F_NumLevel2 = qty2;
    row._F_NumLevel2Max = qty2;
    if (ratio > 0) {
      row.F_NumLevel1 = qty2 / ratio;
    } else {
      row.F_NumLevel1 = qty2;
    }
  },
// ===============================计算======================end
		}
	};
</script>
<style lang="scss" scoped>
	page {
		background-color: #f0f2f6;
	}
	.form-value{
		width: 100%;
		display: flex;
		justify-content: flex-end;
	}
</style>