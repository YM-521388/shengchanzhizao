﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" :rules="rules" ref="dataForm" :errorType="['toast']" label-position="left" :label-width="100 * 1.5" label-align="right" class="jnpf-form">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='外协工单号' prop="F_OutsourceNo"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_OutsourceNo" placeholder='请填写，忽略将自动生成'  :showCount='false' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='商品' prop="F_GoodsId" required  :label-width="100 * 1.5">
					<JnpfPopupSelect v-model='dataForm.F_GoodsId' 
					propsValue='F_Id' relationField='F_GoodsName' 
					:columnOptions='F_GoodsIdOptions' 
					interfaceId='2008721559174385664' 
					placeholder='请选择' hasPage :pageSize='20' 
					popupType='dialog' popupTitle='选择数据' popupWidth='60%' 
					:templateJson='F_GoodsIdTemplateJson' vModel="F_GoodsId"  
					type="popup" :extraOptions='[]'/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='计划数' prop="F_PlanNum" required  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_PlanNum" :precision='0' :step='1.0'  controls/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='BOM' prop="F_BOMId"  :label-width="100 * 1.5">
				<!-- 	<JnpfSelect v-model="dataForm.F_BOMId" 
					:options="F_BOMIdOptions" 
					:props="F_BOMIdProps" 
					filterable placeholder='请选择' 
					:disabled="!dataForm.F_GoodsId"/> -->
					<JnpfPopupSelect v-model='dataForm.F_BOMId' propsValue="id" relationField='F_BomCode'  :columnOptions='optionsObj.F_BOMIdOptions'  interfaceId='2013188646957617152'  placeholder='请选择' hasPage :pageSize='20'  popupType='dialog' popupTitle='选择数据' popupWidth='60%' @change="handleBOMChange" :templateJson='dynamicF_BOMIdTemplateJson' vModel="F_BOMId"   :disabled="!dataForm.F_GoodsId" type="popup" :extraOptions='[]'/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='供应商' prop="F_SupplierId"  :label-width="100 * 1.5">
					<JnpfSelect v-model="dataForm.F_SupplierId" :options="f_SupplierIdOptions" :props="f_SupplierIdProps" placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='供应商联系人' prop="F_ContactPerson"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_ContactPerson" placeholder='请输入'  :showCount='false' />
					<!-- <JnpfPopupSelect v-model='dataForm.F_ContactPerson' propsValue='F_Id' relationField='F_ContactName' :columnOptions='F_ContactPersonOptions' interfaceId='2009459664370143232' placeholder='请选择' :pageSize='20' popupType='dialog' popupTitle='选择数据' popupWidth='800px' :templateJson='F_ContactPersonTemplateJson' vModel="F_ContactPerson"  :formData='dataForm' type="popup" :extraOptions='[]'/> -->
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='供应商联系人电话' prop="F_ContactPhone"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_ContactPhone" placeholder='请输入'  :showCount='false' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='供应商地区' prop="F_Region"  :label-width="100 * 1.5">
					<JnpfAreaSelect v-model="dataForm.F_Region" :level='2' placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='供应商详细地址' prop="F_DetailAddress"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_DetailAddress" placeholder='请输入'  :showCount='false' />
					<!-- <JnpfSelect v-model="dataForm.F_DetailAddress" :options="f_DetailAddressOptions" :props="f_DetailAddressProps" placeholder='请选择' /> -->
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='外协单价' prop="F_Price"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_Price" :step='1.0'  />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='交货日期' prop="F_DeliveryDate"  :label-width="100 * 1.5">
					<JnpfDatePicker v-model="dataForm.F_DeliveryDate" type='date' format='yyyy-MM-dd' placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='优先级' prop="F_Priority" required :label-width="100 * 1.5">
					<JnpfSelect v-model="dataForm.F_Priority" :options="f_PriorityOptions" :props="f_PriorityProps" placeholder='请选择' />
				</u-form-item>
			</view>
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
				<u-form-item label='图纸' prop="F_Files"  :label-width="100 * 1.5">
					<JnpfUploadFile v-model="dataForm.F_Files" :limit='9' sizeUnit='MB' :fileSize='10' pathType="defaultPath"  :sortRule='[
  "2"
]' timeFormat='YYYYMMDD' buttonText='点击上传' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='描述' prop="F_Description"  :label-width="100 * 1.5">
					<JnpfTextarea v-model="dataForm.F_Description" placeholder='请输入'  :showCount='false' />
				</u-form-item>
			</view>
			<view class="jnpf-table">
				<view class="jnpf-table-title u-line-1" >{{'用料清单'}}
				</view>
				<view class="u-p-l-20 u-p-r-20 form-item-box">
					<u-form-item label='批量商品选择'  :label-width="100 * 1.8">
						<JnpfPopupSelect interfaceName='getPopSelect' 
						multiple v-model="tempGoodsSelector" 
						propsValue='id' 
						relationField='F_GoodsName' 
						:columnOptions='tableField7a8044_F_GoodsIdOptions' 
						interfaceId='2012085692393459712' 
						placeholder='请选择' hasPage :pageSize='20' 
						popupType='dialog' popupTitle='选择商品' 
						:templateJson='optionsObj.tableField7a8044_F_GoodsIdTemplateJson' 
						type="popup"  
						:extraOptions="optionsObj.GoodsList" 
						@change="handleGoodsSelectorChange"
						style='width: 100% !important;'/>
						<!--  -->
					</u-form-item>
				</view>
				
				<view class="jnpf-table-item" v-for="(item,i) in dataForm.tableField7a8044" :key="i">
					<view class="jnpf-table-item-title">
						<view class="jnpf-table-item-title-num">({{i+1}})</view>
						<view class="u-flex">
		        <view class="jnpf-table-delete-btn" @click="removeAProdOutsourceItemRow(i,true)">{{this.$t('common.delText','删除')}}</view>
						</view>
                    </view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='商品'  :label-width="100 * 1.5">
							<JnpfPopupSelect v-model="dataForm.tableField7a8044[i].F_GoodsId" 
							propsValue='F_Id' relationField='F_GoodsName' 
							:columnOptions='tableField7a8044_F_GoodsIdOptions' 
							interfaceId='2008721559174385664' placeholder='请选择' 
							hasPage :pageSize='20' popupType='dialog' popupTitle='选择数据' 
							popupWidth='60%' :templateJson='tableField7a8044_F_GoodsIdTemplateJson' 
							:vModel="'tableField7a8044-F_GoodsId'+i" type="popup" disabled/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='商品编号'  :label-width="100 * 1.5">
					      <JnpfInput v-model="dataForm.tableField7a8044[i].F_GoodsNo" placeholder="请输入" disabled/>
					      </u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='规格'  :label-width="100 * 1.5">
					      <JnpfInput disabled v-model="dataForm.tableField7a8044[i].F_Specification" placeholder="请输入" />
					      </u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='单位用量'  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableField7a8044[i].F_Unit" placeholder="请输入" :controls="false"  @change="handleF_UnitChange(dataForm.tableField7a8044[i])" />
							<text>{{ dataForm.tableField7a8044[i].F_NumUnit }}</text>
							<view v-if="dataForm.tableField7a8044[i].F_Unit > dataForm.tableField7a8044[i].F_InventoryNum" style="color: #ff4d4f; font-size: 12px; margin-top: 4px">
							用量不能大于库存数量 {{ dataForm.tableField7a8044[i].F_InventoryNum }}
							</view>
							<!-- <JnpfInputNumber v-model="dataForm.tableField7a8044[i].F_Unit" :precision='0' :step='1.0'  /> -->
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='单位'  :label-width="100 * 1.5">
					      <JnpfInput v-model="dataForm.tableField7a8044[i].F_UnitTwo" placeholder="请输入" disabled/>
					      </u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='备注'  :label-width="100 * 1.5">
							<JnpfTextarea v-model="dataForm.tableField7a8044[i].F_Description" placeholder='请输入'  :showCount='false' />
						</u-form-item>
					</view>
                </view>
				<!-- <view class="jnpf-table-footer-btn" >
					<view class="jnpf-table-btn jnpf-table-primary-btn" @click="addAProdOutsourceItemRow()">
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
	import { getDictionaryDataSelector,getDataInterfaceRes, getDataInterfaceDataInfoByIds } from '@/api/common'
	import { useBaseStore } from '@/store/modules/base'
    import request from '@/utils/request'
	import { buildUUID } from '@/utils/uuid';
	import CustomButton from '@/components/CustomButton'
	import { getGoodsUnit,getBOMGoodsList } from '@/api/subcode/subcode'
	
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
					F_OutsourceNo:undefined,
					F_GoodsId:undefined,
					F_PlanNum:undefined,
					F_BOMId:undefined,
					F_SupplierId:undefined,
					F_ContactPerson:undefined,
					F_ContactPhone:undefined,
					F_Region:[],
					F_DetailAddress:undefined,
					F_Price:undefined,
					F_DeliveryDate:undefined,
					F_Priority:'1',
					F_PlanStartDate:undefined,
					F_PlanEndDate:undefined,
					F_Files:[],
					F_this:0,
					F_Description:undefined,
					F_CreatorUserId:undefined,
					F_CreatorTime:undefined,
					tableField7a8044:[],
                },
                rules: {
					F_GoodsId:[
						{
							required:true,
							message:this.$t('common.chooseText')+' '+'商品',
							trigger:'change',
						},
					],
					F_Priority:[
						{
							required:true,
							message:this.$t('common.chooseText')+' '+'优先级',
							trigger:'change',
						},
					],
					F_PlanNum:[
						{
							required:true,
							message:this.$t('common.inputText')+' '+'计划数',
							trigger:[
  "blur",
  "change"
],
							type:'number'
						},
					],
					F_ContactPhone:[
						{
							pattern:/^1[3456789]\d{9}$|^0\d{2,3}-?\d{7,8}$/,
							message:this.$t('sys.validate.phone','请输入正确的联系方式'),
							trigger:'blur'
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
				selectedtableField7a8044RowKeys: [],
				optionsObj: {
					GoodsList: [],
					tableField7a8044_F_GoodsIdTemplateJson: [],
					F_BOMIdOptions: [
						{
						  value: 'F_BomCode',
						  label: 'BOM编号',
						},
						{
						  value: 'F_BomName',
						  label: 'BOM名称',
						},
					],
					F_BOMIdList: []
				},
				F_GoodsIdTemplateJson: [],
				F_GoodsIdOptions: [
			      {
			        value: 'F_GoodsNo',
			        label: '商品编号',
			      },
			      {
			        value: 'F_GoodsName',
			        label: '商品名称',
			      },
			      {
			        value: 'F_Type',
			        label: '商品类型',
			      },
			      {
			        value: 'F_SalePrice',
			        label: '销售单价',
			      },
			      {
			        value: 'F_CostPrice',
			        label: '成本单价',
			      },
				],
				F_BOMIdTemplateJson: [
					  {
					    "defaultValue": "",
					    "field": "goodsId",
					    "dataType": "varchar",
					    "required": 0,
					    "fieldName": "合同ID",
					    "relationField": "2008839754363310080",
					    "jnpfKey": null,
					    "complexHeaderList": null,
					    "sourceType": 2,
					    "isChildren": false,
					    "IsMultiple": false
					  }
				],
				F_BOMIdProps:{ label: 'F_BomName', value: 'id' },
				f_SupplierIdProps:{'label':'F_SupplierName','value':'F_Id'},
				f_SupplierIdOptions: [],
				F_ContactPersonTemplateJson: [
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
				F_ContactPersonOptions: [
				  {
					"value": "F_ContactName",
					"label": "联系人姓名"
				  },
				  {
					"value": "F_ContactPhone",
					"label": "联系人电话"
				  }
				],
				f_DetailAddressProps:{'label':'F_Address','value':'F_Id'},
				f_DetailAddressOptions: [],
				f_PriorityProps:{'label':'fullName','value':'enCode'},
				f_PriorityOptions: [],
				tableField7a8044_F_GoodsIdTemplateJson: [],
				tableField7a8044_F_GoodsIdOptions: [
			      {
			        value: 'F_GoodsName',
			        label: '商品名',
			      },
			      {
			        value: 'F_GoodsNo',
			        label: '商品编号',
			      },
			      {
			        value: 'F_Specification',
			        label: '商品规格',
			      },
			      {
			        value: 'F_Type',
			        label: '商品类型',
			      },
			      // {
			      //   value: 'F_CategoryId',
			      //   label: '商品类别',
			      // },
			      // {
			      //   value: 'F_Unit',
			      //   label: '单位',
			      // },
			      {
			        value: 'F_InventoryNum',
			        label: '库存',
			      },
			      // {
			      //   value: 'F_InspectRule',
			      //   label: '检验规范',
			      // },
			      {
			        value: 'F_Remark',
			        label: '备注',
			      },
				],
				tempGoodsSelector: [],
            };
        },
		watch: {},
		  computed: {
    dynamicF_BOMIdTemplateJson() {
      return [
        {
          defaultValue: '',
          field: 'goodsId',
          dataType: 'varchar',
          required: 0,
          fieldName: '合同ID',
          relationField: this.dataForm.F_GoodsId || '',
          jnpfKey: null,
          complexHeaderList: null,
          sourceType: 2,
          isChildren: false,
          IsMultiple: false,
        }
      ];
    }
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
			// this.getf_BOMIdOptions();
			this.getf_SupplierIdOptions();
			this.getf_DetailAddressOptions();
			this.getf_PriorityOptions();
            uni.$on('linkPageConfirm', (subVal, Vmodel) => {
				let item = {
						F_GoodsId:undefined,
						F_Unit:undefined,
						F_Description:undefined,
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
					}
               if ('AProdOutsourceItem' === Vmodel) subVal.forEach(t => this.dataForm.tableField7a8044.push({...item,...t}));
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
			// 批量商品选择处理方法
			handleGoodsSelectorChange(val, option) {
				/* 兜底保证是数组 */
				    if (!Array.isArray(this.dataForm.tableField7a8044)) {
				      this.dataForm.tableField7a8044 = [];
				    }
				
				    /* 1. 把 option 里的 id 做成 Set，方便比对 */
				    const optionIdSet = new Set(option.map(o => o.id));
				
				    /* 2. 删除本地已不在 option 里的行 */
				    this.dataForm.tableField7a8044 = this.dataForm.tableField7a8044.filter(item => optionIdSet.has(item.F_GoodsId));
				
				    /* 3. 把 option 里本地还没有的行加进来 */
				    option.forEach(o => {
				
				      const exist = this.dataForm.tableField7a8044.findIndex(item => item.F_GoodsId === o.id) !== -1;
				      if (!exist) {
				        getGoodsUnit(o.id).then(res => {
				          const newRow = {
				            jnpfId: buildUUID(),
				            F_GoodsId: o.id,
				            F_GoodsNo: o.F_GoodsNo,
				            F_Specification: o.F_Specification,
				            F_NumUnit: res.data?.split('/').length > 1 ? res.data?.split('/')[1] : res.data?.split('/')[0],
				            F_UnitTwo: res?.data,
				            F_InventoryNum: o.F_InventoryNum,
				            F_Unit: 1,
				            F_Description: undefined,
				            F_CreatorTime: undefined,
				          };
				          this.dataForm.tableField7a8044.push(newRow);
				        });
				      }
				    });
			},
			
			getf_BOMIdOptions(){
				this.F_BOMIdOptions = []
				let templateJson = [
				  {
					"defaultValue": "",
					"field": "goodsId",
					"dataType": "varchar",
					"required": 0,
					"fieldName": "合同ID",
					"relationField": "F_GoodsId",
					"jnpfKey": "popupSelect",
					"complexHeaderList": null,
					"sourceType": 1,
					"isChildren": false,
					"IsMultiple": false
				  }
				]
				let query = {
					paramList: this.getParamList(templateJson, this.dataForm)
				}
				getDataInterfaceRes('2013188646957617152', query).then(res => {
					let data = res.data
					this.F_BOMIdOptions = Array.isArray(data) ? data : []
				});
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
			getf_DetailAddressOptions(){
				this.f_DetailAddressOptions = []
				let templateJson = [
  {
    "defaultValue": "",
    "field": "id",
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
]
				let query = {
					paramList: this.getParamList(templateJson, this.dataForm)
				}
				getDataInterfaceRes('2009527238009163776', query).then(res => {
					let data = res.data
					this.f_DetailAddressOptions = Array.isArray(data) ? data : []
				});
			},
			getf_PriorityOptions(){
				getDictionaryDataSelector('2013182853290004480').then(res => {
					this.f_PriorityOptions = res.data.list
				});
			},
			selfInit(){
				this.addAProdOutsourceItem();
			},
			resetForm() {
				this.dataForm.tableField7a8044 = [];
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
                        url: '/api/example/AProdOutsource/' + this.dataForm.id,
                        method: 'get',
                    }).then(res => {
						this.tempGoodsSelector = []
                        this.dataForm = res.data;
						if(!this.dataForm.F_Region)this.dataForm.F_Region=[];
						if(!this.dataForm.F_Files)this.dataForm.F_Files=[];

					this.dataForm.tableField7a8044.map(item=>{
					this.tempGoodsSelector.push(item.F_GoodsId);
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
                            url: '/api/example/AProdOutsource/' + this.dataForm.id,
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
                            url: '/api/example/AProdOutsource',
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
			addAProdOutsourceItemRow() {
				let item = {
						F_GoodsId:undefined,
						F_Unit:undefined,
						F_Description:undefined,
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
					}
				this.dataForm.tableField7a8044.push(item)
				this.initCollapse()
			},
			removeAProdOutsourceItemRow(i,showConfirm=false) {
				const handleRemove = () => {
					this.dataForm.tableField7a8044.splice(i, 1)
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
			copyAProdOutsourceItemRow(i) {
				let item = JSON.parse(JSON.stringify(this.dataForm.tableField7a8044[i]));
				item.id = '';
				item.length && item.map(o => delete o.rowData);
				item.F_CreatorUserId = undefined;
				item.F_CreatorTime = undefined;
				this.dataForm.tableField7a8044.push(item);
			},
			// 处理单位用量变化
			 handleF_UnitChange(record) {
			  // 触发响应式更新,确保错误提示及时显示
			  const index = this.dataForm.tableField7a8044.findIndex(item => item.jnpfId === record.jnpfId);
			  if (index > -1) {
			    this.dataForm.tableField7a8044[index] = { ...record };
			  }
			},
			  // 处理BOM选择变化
			   handleBOMChange(bomId) {
			    if (bomId) {
			      // 调用后端API获取BOM用料清单
			      getBOMGoodsList(bomId)
			        .then(res => {
			          const goodsList = res.data || [];
			          // 清空现有用料清单
			          this.dataForm.tableField7a8044 = [];
			          // 将BOM用料清单填充到表格中
			          goodsList.forEach(item => {
			            const rowData = {
			              jnpfId: buildUUID(),
			              F_GoodsId: item.F_GoodsId,
			              F_Unit: item.F_Unit,
			              F_InventoryNum: item.F_InventoryNum,
			              F_Description: '',
			              F_CreatorUserId: undefined,
			              F_CreatorTime: undefined,
			            };
			            this.dataForm.tableField7a8044.push(rowData);
			          });
			        })
			        .catch(() => {
			          createMessage.error('获取BOM用料清单失败');
			          this.dataForm.tableField7a8044 = [];
			        });
			    } else {
			      // 如果清空BOM选择，清空用料清单
			      this.dataForm.tableField7a8044 = [];
			    }
			  }
        }
    };
</script>
<style>
        page{
                background-color: #f0f2f6;
        }
		
		.u-form-item--left__content__label{
			justify-content: unset !important;
		}
</style>



