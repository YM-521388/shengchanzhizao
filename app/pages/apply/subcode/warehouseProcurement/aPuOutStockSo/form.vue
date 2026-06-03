<!-- ﻿﻿﻿﻿﻿﻿﻿﻿仓库--商品 -->
<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" :rules="rules" ref="dataForm" :errorType="['toast']" label-position="left" :label-width="100 * 1.5" label-align="right" class="jnpf-form">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='销售出库单号' prop="F_StockOutNo"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_StockOutNo" placeholder='请填写，忽略将自动生成'  :showCount='false' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='仓库' prop="F_WarehouseId" required  :label-width="100 * 1.5">
					<JnpfCascader @change='changeWarehouseId' v-model="dataForm.F_WarehouseId" :options="f_WarehouseIdOptions" :props="f_WarehouseIdProps" placeholder='请选择' filterable clearable/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='出库类型' prop="F_StockOutType"  :label-width="100 * 1.5">
					<JnpfSelect v-model="dataForm.F_StockOutType" :options="f_StockOutTypeOptions" :props="f_StockOutTypeProps" placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='出库日期' prop="F_StockOutDate" required  :label-width="100 * 1.5">
					<JnpfDatePicker v-model="dataForm.F_StockOutDate" type='date' format='yyyy-MM-dd' placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='发货人' prop="F_StockOutUserId"  :label-width="100 * 1.5">
					<JnpfUserSelect v-model="dataForm.F_StockOutUserId" placeholder='请选择' selectType='all' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='客户' prop="F_CustomerId"  :label-width="100 * 1.5">
					<JnpfPopupSelect v-model='dataForm.F_CustomerId' propsValue='F_Id' relationField='F_CustomerName' :columnOptions='F_CustomerIdOptions' interfaceId='2009458830353764352' placeholder='请选择' hasPage :pageSize='20' popupType='dialog' popupTitle='选择数据' popupWidth='800px' :templateJson='F_CustomerIdTemplateJson' vModel="F_CustomerId"  type="popup" :extraOptions='[]' @change="customerChange"/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='联系人' prop="F_ContactId"  :label-width="100 * 1.5">
					<JnpfSelect v-model="dataForm.F_ContactId" :options="f_ContactIdOptions" :props="f_ContactIdProps" placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='地址' prop="F_AddressId"  :label-width="100 * 1.5">
					<JnpfSelect v-model="dataForm.F_AddressId" :options="f_AddressIdOptions" :props="f_AddressIdProps" placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='备注' prop="F_Description"  :label-width="100 * 1.5">
					<JnpfTextarea v-model="dataForm.F_Description" placeholder='请输入'  :showCount='false' />
				</u-form-item>
			</view>
			<view class="jnpf-table">
				<view class="jnpf-table-title u-line-1" >{{'商品列表'}}
				</view>
				<view class="u-p-l-20 u-p-r-20 form-item-box">
					<u-form-item label='添加商品'  :label-width="100 * 1.5" @click="addGoods" >
					</u-form-item>
				</view>
<!-- 				<view class="u-p-l-20 u-p-r-20 form-item-box">
					<u-form-item label='批量商品选择'  :label-width="100 * 1.5">
						<JnpfPopupSelect multiple interfaceName='getPopSelect' v-model="tempGoodsSelector" propsValue='F_GoodsId' relationField='F_GoodsName' :columnOptions='tableFielde48def_F_CustomerIdOptions' interfaceId='2015036768293883904' placeholder='选择对应仓库后，再选择商品' :pageSize='20' popupType='dialog' popupTitle='选择数据' popupWidth='60%' :templateJson='tableFielde48def_F_CustomerIdTemplateJson' :vModel="'tableFielde48def-F_CustomerId'" type="popup" @change='handleGoods'/>
					</u-form-item>
				</view> -->
				<view class="jnpf-table-item" v-for="(item,i) in dataForm.tableFielde48def" :key="i">
					<view class="jnpf-table-item-title">
						<view class="jnpf-table-item-title-num">({{i+1}})</view>
						<view class="u-flex">
							<view class="jnpf-table-delete-btn" @click="removeAPuOutStockSoItemRow(i,true)">{{this.$t('common.delText','删除')}}</view>
						</view>
                    </view>
<!-- 					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='入库仓库' prop="F_WarehouseID" required :label-width="100 * 1.5">
							<JnpfCascader  @change='(e) => changeWarehouseId(e, i)' v-model="dataForm.tableFielde48def[i].F_WarehouseID" :options="f_WarehouseIdOptions" :props="f_WarehouseIdProps" placeholder='请选择' filterable clearable />
						</u-form-item>
					</view> -->
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='商品'  :label-width="100 * 1.5">
							<JnpfPopupSelect :disabled="Boolean(dataForm.id)"  v-model="dataForm.tableFielde48def[i].F_CustomerId" propsValue='F_GoodsId' relationField='F_GoodsName' :columnOptions='tableFielde48def_F_CustomerIdOptions' :interfaceId='interfaceId' placeholder='请选择' :pageSize='20' popupType='dialog' popupTitle='选择数据' popupWidth='60%' :templateJson='tableFielde48def_F_CustomerIdTemplateJson' :vModel="'tableFielde48def-F_CustomerId'+i" type="popup"/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='库存编码' :label-width="100 * 1.5">
							<JnpfInput v-model="dataForm.tableFielde48def[i].F_Code" disabled />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='出库数量'  :label-width="100 * 1.5">
							<!-- <JnpfInputNumber v-model="dataForm.tableFielde48def[i].F_Num" :precision='0' :step='1.0'  controls /> -->
				          <view class="form-value" v-if="getUnitRatioLevels(dataForm.tableFielde48def[i])?.level2?.name">
				            <JnpfInputNumber
				              v-model="dataForm.tableFielde48def[i].F_NumLevel1"
				              :placeholder="dataForm.tableFielde48def[i]._quantityDisabled ? '自动带出' : '请输入'"
				              :controls="false"
				              disabled
				              :style="{ width: '80px' }"
				              @change="handleNumLevel1Change(dataForm.tableFielde48def[i], i)" />
				            <text>{{ getUnitRatioLevels(dataForm.tableFielde48def[i])?.level1?.name }}</text>
				            <JnpfInputNumber
				              v-model="dataForm.tableFielde48def[i].F_NumLevel2"
				              placeholder="请输入"
				              :controls="false"
				              :min="1"
				              :disabled="Boolean(dataForm.id)"
				              :max="dataForm.tableFielde48def[i]._F_NumLevel2Max"
				              :style="{ width: '80px' }"
				              @change="handleNumLevel2Change(dataForm.tableFielde48def[i], i)" />
				            <text>{{ getUnitRatioLevels(dataForm.tableFielde48def[i])?.level2?.name }}</text>
				          </view>
				          <view class="form-value" v-else>
				            <JnpfInputNumber
				              v-model="dataForm.tableFielde48def[i].F_NumLevel2"
				              placeholder="请输入"
				              :controls="false"
				              disabled
				              :min="1"
				              :max="dataForm.tableFielde48def[i]._F_NumLevel2Max"
				              :style="{ width: '80px' }"
				              @change="handleNumLevel2Change(dataForm.tableFielde48def[i])" />
				            <text>{{ getUnitRatioLevels(dataForm.tableFielde48def[i])?.level1?.name }}</text>
				          </view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='成本单价(元)'  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableFielde48def[i].F_Price" :step='1.0'  />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='销售单价(元)'  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableFielde48def[i].F_SalesPrice" :step='1.0'  />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='类别'  :label-width="100 * 1.5">
							<JnpfInput v-model="dataForm.tableFielde48def[i].F_Type" placeholder='请输入'  :showCount='false' />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='备注'  :label-width="100 * 1.5">
							<JnpfTextarea v-model="dataForm.tableFielde48def[i].F_Description" placeholder='请输入'  :showCount='false' />
						</u-form-item>
					</view>
                </view>
<!-- 				<view class="jnpf-table-footer-btn" >
					<view class="jnpf-table-btn jnpf-table-primary-btn" @click="addAPuOutStockSoItemRow()">
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
	import { getDictionaryDataSelector,getDataInterfaceRes,getDataInterGoodsInventoryNo,getDataInterfaceDataInfoByIds  } from '@/api/common'
	import { useBaseStore } from '@/store/modules/base'
    import request from '@/utils/request'
	import CustomButton from '@/components/CustomButton'
	import { getInventoryByWarehouse } from '@/api/subcode/subcode'
	import { buildUUID } from '@/utils/uuid';
	
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
					F_StockOutNo:undefined,
					F_WarehouseId:[],
					F_StockOutType:undefined,
					F_StockOutDate:undefined,
					F_StockOutUserId:undefined,
					F_CustomerId:undefined,
					F_ContactId:undefined,
					F_AddressId:undefined,
					F_Description:undefined,
					F_CreatorUserId:undefined,
					F_CreatorTime:undefined,
					tableFielde48def:[],
					tableField67a4d7:[],
                },
                rules: {
					F_WarehouseId:[
						{
							required:true,
							message:this.$t('common.chooseText')+' '+'仓库',
							trigger:'change',
							type:'array'
						},
					],
					F_StockOutDate:[
						{
							required:true,
							message:this.$t('common.chooseText')+' '+'出库日期',
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
				f_WarehouseIdProps:{'label':'F_Name','value':'F_Id',"children": "children"},
				f_WarehouseIdOptions: [],
				f_StockOutTypeProps:{'label':'fullName','value':'enCode'},
				f_StockOutTypeOptions: [],
				F_CustomerIdTemplateJson: [],
				F_CustomerIdOptions: [
  {
    "value": "F_CustomerName",
    "label": "客户姓名"
  },
  {
    "value": "F_CustomerCode",
    "label": "客户编码"
  }
],
				f_ContactIdProps:{'label':'F_ContactName','value':'F_Id'},
				f_ContactIdOptions: [],
				f_AddressIdProps:{'label':'F_Address','value':'F_Id'},
				f_AddressIdOptions: [],
				tableFielde48def_F_CustomerIdTemplateJson: [{
          defaultValue: '',
          field: 'warehouseId',
          dataType: 'varchar',
          required: 0,
          fieldName: '',
          // 使用表单中选中的仓库 ID 作为接口参数（动态绑定）
          relationField: '',
          jnpfKey: null,
          complexHeaderList: null,
          // sourceType=1 会让 getParamList 从 formData 中取值并填充 defaultValue
          sourceType: 2,
          isChildren: false,
          IsMultiple: false,
        },],
				tableFielde48def_F_CustomerIdOptions: [
        {
          value: 'F_Code',
          label: '库存编码',
        },
        {
          value: 'F_GoodsName',
          label: '商品名称',
        },
        {
          value: 'F_GoodsNo',
          label: '商品编码',
        },
        {
          value: 'F_Type',
          label: '商品类型',
        },
        {
          value: 'F_Num',
          label: '商品数量',
        },
],
tempGoodsSelector:[],
inventoryByWarehouse: [],
interfaceId:'2034873228723359744'
            };
        },
		watch: {
			'dataForm.tableFielde48def': {
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
			this.getf_WarehouseIdOptions();
			this.getf_StockOutTypeOptions();

            uni.$on('linkPageConfirm', (subVal, Vmodel) => {
				let item = {
						F_CustomerId:undefined,
						F_Num:undefined,
						F_Price:undefined,
						F_SalesPrice:undefined,
						F_Type:undefined,
						F_Description:undefined,
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
					}
               if ('APuOutStockSoItem' === Vmodel) subVal.forEach(t => this.dataForm.tableFielde48def.push({...item,...t}));
			   this.initCollapse();
            });
            uni.$on('linkPageConfirm', (subVal, Vmodel) => {
				let item = {
						F_Type:undefined,
						F_Title:undefined,
						F_Content:undefined,
						F_Reason:undefined,
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
					}
               if ('APuOutStockSoLog' === Vmodel) subVal.forEach(t => this.dataForm.tableField67a4d7.push({...item,...t}));
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
			getf_StockOutTypeOptions(){
				getDictionaryDataSelector('2013096194263355392').then(res => {
					this.f_StockOutTypeOptions = res.data.list
				});
			},
			getf_ContactIdOptions(){
				this.f_ContactIdOptions = []
				let templateJson = [
  {
    "defaultValue": "",
    "field": "Id",
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
				getDataInterfaceRes('2009459664370143232', query).then(res => {
					let data = res.data
					this.f_ContactIdOptions = Array.isArray(data) ? data : []
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
    conversionDateTime(type) {
      const format = type === 'yyyy' ? 'yyyy-01-01 00:00:00' : type === 'yyyy-MM' ? 'yyyy-MM-01 00:00:00' :
        type === 'yyyy-MM-dd' ? 'yyyy-MM-dd 00:00:00' : type === 'yyyy-MM-dd HH:mm' ? 'yyyy-MM-dd HH:mm:00' : 'yyyy-MM-dd HH:mm:ss'
      const dataTime = this.jnpf.toDate(new Date(), format)
      return new Date(dataTime).getTime()
    },
	initDefaultData() {
      this.dataForm.F_StockOutDate = this.conversionDateTime("yyyy-MM-dd");
	},
			selfInit(){
				this.addAPuOutStockSoItem();
				this.addAPuOutStockSoLog();
			},
			resetForm() {
				this.dataForm.tableFielde48def = [];
				this.dataForm.tableField67a4d7 = [];
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
                        url: '/api/example/APuOutStockSo/' + this.dataForm.id,
                        method: 'get',
                    }).then(async res => {
                        this.dataForm = res.data;
						if(!this.dataForm.F_WarehouseId)this.dataForm.F_WarehouseId=[];
						this.tempGoodsSelector = []
						this.getf_ContactIdOptions();
						this.getf_AddressIdOptions();
						this.tableFielde48def_F_CustomerIdTemplateJson[0].relationField = JSON.stringify(this.dataForm.F_WarehouseId);
						// this.tableFielde48def_F_CustomerIdTemplateJson[0].defaultValue=this.dataForm.F_WarehouseId
						// this.dataForm.tableFielde48def.map(item=>{
						// this.tempGoodsSelector.push(item.F_CustomerId);
						// })
						await this.enrichTableRowsGoodsMeta(this.dataForm.tableFielde48def);
						for (const row of this.dataForm.tableFielde48def) {
						     this.fillNumLevelsFromF_Num(row);
						   }
					})
				}else{
					this.initDefaultData()
				}
            },
			submitForm(type) {
                this.$refs.dataForm.validate(valid => {
                    if (!valid) return
					console.log('提交',this.dataForm);
                    this.btnLoading = true
                    if (this.dataForm.id) {
                        request({
                            url: '/api/example/APuOutStockSo/' + this.dataForm.id,
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
                            url: '/api/example/APuOutStockSo',
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
			addAPuOutStockSoItemRow() {
				let item = {
						F_CustomerId:undefined,
						F_Num:undefined,
						F_Price:undefined,
						F_SalesPrice:undefined,
						F_Type:undefined,
						F_Description:undefined,
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
					}
				this.dataForm.tableFielde48def.push(item)
				this.initCollapse()
			},
			removeAPuOutStockSoItemRow(i,showConfirm=false) {
				const handleRemove = () => {
					this.dataForm.tableFielde48def.splice(i, 1)
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
			copyAPuOutStockSoItemRow(i) {
				let item = JSON.parse(JSON.stringify(this.dataForm.tableFielde48def[i]));
				item.id = '';
				item.length && item.map(o => delete o.rowData);
				item.F_CreatorUserId = undefined;
				item.F_CreatorTime = undefined;
				this.dataForm.tableFielde48def.push(item);
			},
			addAPuOutStockSoLogRow() {
				let item = {
						F_Type:undefined,
						F_Title:undefined,
						F_Content:undefined,
						F_Reason:undefined,
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
					}
				this.dataForm.tableField67a4d7.push(item)
				this.initCollapse()
			},
			removeAPuOutStockSoLogRow(i,showConfirm=false) {
				const handleRemove = () => {
					this.dataForm.tableField67a4d7.splice(i, 1)
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
			copyAPuOutStockSoLogRow(i) {
				let item = JSON.parse(JSON.stringify(this.dataForm.tableField67a4d7[i]));
				item.id = '';
				item.length && item.map(o => delete o.rowData);
				item.F_CreatorUserId = undefined;
				item.F_CreatorTime = undefined;
				this.dataForm.tableField67a4d7.push(item);
			},
			customerChange(e){
				this.getf_ContactIdOptions()
				this.getf_AddressIdOptions()
			},
			changeWarehouseId(e){
				 this.tableFielde48def_F_CustomerIdTemplateJson[0].relationField = JSON.stringify(e);
				this.dataForm.tableFielde48def.map(item=>{
					 item.F_CustomerId=''
				 })
			},
			handleGoods(ids,option){
				/* 兜底 */
				 if (!Array.isArray(this.dataForm.tableFielde48def)) {
				   this.dataForm.tableFielde48def = [];
				 }
				 const optionIdSet = new Set(option.map(o => o.id));
								
				 /*  删掉已取消勾选的行 */
				 this.dataForm.tableFielde48def = this.dataForm.tableFielde48def.filter(item => optionIdSet.has(item.F_WorkOrderId));
				 option.forEach(o => {
				   const exist = this.dataForm.tableFielde48def.some(item => item.F_WorkOrderId === o.F_Id);
					if (!exist) {
				   let item = {
				   		F_CustomerId:o.F_GoodsId,
				   		F_Num:o.F_Num,
				   		F_Price:undefined,
				   		F_SalesPrice:undefined,
				   		F_Type:undefined,
				   		F_Description:undefined,
				   		F_CreatorUserId:undefined,
				   		F_CreatorTime:undefined,
				   	}
				     this.dataForm.tableFielde48def.push(item);
				   }
				 });
			},
			addGoods(){
				// let query = {paramList: [{"field": "goodsNo","defaultValue": 'CA1SP20260319000114',"id": "b6217b","dataType": "varchar","required": "0","fieldName": "库存编码","source": null}]
				// }
				// getDataInterGoodsInventoryNo('2036268230699520000', query).then(res => {
				// 	let data = res.data
				// 	let temp={
				// 		...data,
				// 		F_CustomerId:data.id,
				// 		F_Code:data.F_InventoryNo,
				// 	}
				// 	console.log('二维码商品数据',data);
				// 	this.fillNumLevelsFromF_Num(temp);
				// 	this.dataForm.tableFielde48def.push(temp);
				// });
				// return
				if(!this.dataForm.F_WarehouseId.length){
					uni.showToast({
						icon :"error",
						title: '请先选择仓库',
						duration: 2000
					});
					return
				}
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
								F_Code:data.F_InventoryNo,
							}
							console.log('二维码商品数据',data);
							this.fillNumLevelsFromF_Num(temp);
							this.dataForm.tableFielde48def.push(temp);
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
        page{
                background-color: #f0f2f6;
        }
		.form-value{
			width: 100%;
			display: flex;
			justify-content: flex-end;
		}
</style>



