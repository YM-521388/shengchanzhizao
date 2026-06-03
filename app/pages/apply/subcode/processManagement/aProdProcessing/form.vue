﻿﻿﻿﻿﻿﻿﻿﻿<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" :rules="rules" ref="dataForm" :errorType="['toast']" label-position="left" :label-width="100 * 1.5" label-align="right" class="jnpf-form">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='加工单号' prop="F_ProcessNo"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_ProcessNo" placeholder='请填写，忽略将自动生成'  :showCount='false' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='商品' prop="F_GoodsId" required  :label-width="100 * 1.5">
					<JnpfPopupSelect v-model='dataForm.F_GoodsId' propsValue='id' relationField='F_GoodsName' :columnOptions='F_GoodsIdOptions' interfaceId='2029803158963884032' placeholder='请选择' hasPage :pageSize='20' popupType='dialog' popupTitle='选择数据' popupWidth='1000px' :templateJson='F_GoodsIdTemplateJson' vModel="F_GoodsId" @change='F_GoodsIdChange'  type="popup" :extraOptions='[]'/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='BOM' prop="F_BOMId"  :label-width="100 * 1.5">
					<JnpfSelect v-model="dataForm.F_BOMId" :options="F_BOMIdOptions" :props="F_BOMIdProps" filterable placeholder='请选择' :disabled="!dataForm.F_GoodsId" @change="F_BomChange"/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='计划数' prop="F_PlanQty" required  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_PlanQty" :precision='0' :min='1' :step='1.0'  />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='交货日期' prop="F_DeliveryDate"  :label-width="100 * 1.5">
					<JnpfDatePicker v-model="dataForm.F_DeliveryDate" type='date' format='yyyy-MM-dd' placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='优先级' prop="F_Priority" required :label-width="100 * 1.5">
					<JnpfSelect v-model="dataForm.F_Priority" :options="F_PriorityOptions" :props="F_PriorityProps" placeholder='请选择' />
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
				<u-form-item label='图纸' prop="F_DrawingFile"  :label-width="100 * 1.5">
					<JnpfUploadFile v-model="dataForm.F_DrawingFile" :limit='9' sizeUnit='MB' :fileSize='10' pathType="selfPath"  :sortRule='[
  "1",
  "2"
]' timeFormat='YYYYMMDD' buttonText='点击上传' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='客户' prop="F_CustomerName" required  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_CustomerName" placeholder='请输入'  :showCount='false' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='类别' prop="F_Type" required  :label-width="100 * 1.5">
					<JnpfCascader v-model="dataForm.F_Type" :options="F_TypeOptions" :props="F_TypeProps" placeholder='请选择' filterable clearable />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='工艺路线' required  :label-width="100 * 1.5">
					<JnpfSelect :disabled="Boolean(dataForm.id)" v-model="dataForm.F_RoutingId" placeholder='请选择' :options="tableField751ecb_F_RoutingIdOptions" :props="tableField751ecb_F_RoutingIdProps" filterable />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='BOM图片' prop="F_BOMImages"  :label-width="100 * 1.5">
					<JnpfUploadImg v-model="dataForm.F_BOMImages" sizeUnit='MB' :fileSize='10' pathType="selfPath"  :sortRule='[
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
			
			<view class="jnpf-table">
				<view class="jnpf-table-title u-line-1" @click="clickIcon('用料清单', '选择BOM后将拉取数据填入用料清单' )">{{'用料清单'}}
					<u-icon :name="'question-circle-fill'" class="u-m-l-10" color="#a0acb7" />
				</view>
				<view class="u-p-l-20 u-p-r-20 form-item-box">
					<u-form-item label='添加用料'  :label-width="100 * 1.5">
						<JnpfPopupSelect interfaceName='getPopSelect' 
						multiple v-model="tempGoodsSelector" 
						propsValue='id' 
						relationField='F_GoodsName' 
						:columnOptions='optionsObj.GoodsListOptions' 
						interfaceId='2030840782923108352' 
						placeholder='请选择' hasPage :pageSize='20' 
						popupType='dialog' popupTitle='选择数据' 
						:templateJson='optionsObj.GoodsListTemplateJson' 
						:vModel="'tableFieldc87c94-F_CustomerId'" type="popup"  
						@change="GoodsListBtn" 
						:extraOptions="optionsObj.GoodsList" 
						style='width: 100% !important;'/>
					</u-form-item>
				</view>
				
				<view class="jnpf-table-item" v-for="(item,i) in dataForm.tableField449e6c" :key="i">
					<view class="jnpf-table-item-title">
						<view class="jnpf-table-item-title-num">({{i+1}})</view>
						<view class="u-flex">
							<view class="jnpf-table-delete-btn" @click="removeAProdBomItemRow(i,true)">{{this.$t('common.delText','删除')}}</view>
						</view>
                    </view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='排序' required  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableField449e6c[i].F_SortCode" :precision='0' :min='1' :step='1.0'  />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='商品'  :label-width="100 * 1.5">
							<JnpfSelect v-model="dataForm.tableField449e6c[i].F_GoodsId" :options="tableField449e6c_F_GoodsIdOptions" :props="tableField449e6c_F_GoodsIdProps" filterable disabled />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='单位用量'  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableField449e6c[i].F_Unit" :precision='0' :min='1' :step='1.0'  @change="handleF_UnitChange(dataForm.tableField449e6c[i])"/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='单位'  :label-width="100 * 1.5">
							<JnpfInput v-model="dataForm.tableField449e6c[i].F_UnitTwo" placeholder="" :controls="false" disabled/>
						</u-form-item>
					</view>
<!-- 					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='入库工序'  :label-width="100 * 1.5">
							<JnpfSelect v-model="dataForm.tableField449e6c[i].F_StockInProcess" placeholder='请选择' :options="tableField449e6c_F_StockInProcessOptions" :props="tableField449e6c_F_StockInProcessProps" filterable />
						</u-form-item>
					</view> -->
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='备注'  :label-width="100 * 1.5">
							<JnpfInput v-model="dataForm.tableField449e6c[i].F_Description" placeholder='请输入'  :showCount='false' />
						</u-form-item>
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
	import { getGoodsUnit,getDictionaryDataSelector,getDataInterfaceRes,getBOMGoodsList,getRoutingStepList } from '@/api/common'
	import { useBaseStore } from '@/store/modules/base'
    import request from '@/utils/request'
	import { buildUUID } from '@/utils/uuid';
	import CustomButton from '@/components/CustomButton'
    export default {
		components:{ CustomButton },
        data() {
            return {
				tempGoodsSelector:[],
				keyCode : +new Date(),
                btnLoading: false,
				addTableConf:{
				},
                dataForm: {
                    id:'',
					F_ProcessNo:undefined,
					F_GoodsId:undefined,
					F_BOMId:undefined,
					F_PlanQty:undefined,
					F_DeliveryDate:undefined,
					F_Priority:'1',
					F_PlanStartDate:undefined,
					F_PlanEndDate:undefined,
					F_DrawingFile:[],
					F_CustomerName:undefined,
					F_DoorPlateThickness:undefined,
					F_BoxPlateThickness:undefined,
					F_InstallOrBeam:undefined,
					F_LockSet:undefined,
					F_Hinge:undefined,
					F_Color:undefined,
					F_Type:[],
					F_BOMImages:[],
					F_Description:undefined,
					F_CustomerId:undefined,
					F_ProjectId:undefined,
					F_ProdPlanId:undefined,
					F_State:undefined,
					F_CreatorUserId:undefined,
					F_CreatorTime:undefined,
					tableField0c720e:[],
					tableField449e6c:[],
					F_SequenceNo:undefined,
                },
				tableField449e6cinnerActiveKey: [],
				optionsObj: {
				      GoodsListOptions: [
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
				          label: '商品类别',
				        },
				        {
				          value: 'F_InventoryNum',
				          label: '库存',
				        },
				        {
				          value: 'F_Remark',
				          label: '备注',
				        },
				      ],
				      GoodsListTemplateJson: [],
				      GoodsList: [],
				    },
                rules: {
					F_GoodsId:[
						{
							required:true,
							message:this.$t('common.inputText')+' '+'商品',
							trigger:'change',
						},
					],
					F_PlanQty:[
						{
							required:true,
							message:this.$t('common.inputText')+' '+'计划数',
							trigger:["blur","change"],
							type:'number'
						},
					],
					F_Priority:[
						{
							required:true,
							message:this.$t('common.inputText')+' '+'优先级',
							trigger:'change',
						},
					],
					F_CustomerName:[
						{
							required:true,
							message:this.$t('common.inputText')+' '+'客户',
							trigger:'blur',
						},
					],
					F_DoorPlateThickness:[
						{
							required:true,
							message:this.$t('common.inputText')+' '+'门板厚度',
							trigger:'change',
						},
					],
					F_BoxPlateThickness:[
						{
							required:true,
							message:this.$t('common.inputText')+' '+'箱体板厚',
							trigger:'change',
						},
					],
					F_InstallOrBeam:[
						{
							required:true,
							message:this.$t('common.inputText')+' '+'安装板或安装梁',
							trigger:'change',
						},
					],
					F_LockSet:[
						{
							required:true,
							message:this.$t('common.inputText')+' '+'锁具',
							trigger:'blur',
						},
					],
					F_Hinge:[
						{
							required:true,
							message:this.$t('common.inputText')+' '+'铰链',
							trigger:'blur',
						},
					],
					F_Color:[
						{
							required:true,
							message:this.$t('common.inputText')+' '+'颜色',
							trigger:'blur',
						},
					],
					F_Type:[
						{
							required:true,
							message:this.$t('common.inputText')+' '+'类别',
							trigger:'change',
							type:'array'
						},
					],
                },
				// ruleList: {
				// 	tableField0c720e: {
				// 		'F_SortCode': '排序不能为空',
				// 	},
				// 	tableField449e6c: {
				// 		'F_SortCode': '排序不能为空',
				// 	},
				// },
				regList: {
				},
				index: 0,
				idList: [],
				dataValue: {},
				jurisdictionType:'',
				addType: 0,
				F_GoodsIdTemplateJson: [
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
],
				F_GoodsIdOptions: [
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
					  value: 'F_InspectRule',
					  label: '检验规范',
					},
					{
					  value: 'F_Remark',
					  label: '备注',
					},
				],
				F_BOMIdProps:{ label: 'F_BomName', value: 'id' },
				F_BOMIdOptions: [],
				F_PriorityProps:{'label':'fullName','value':'enCode'},
				F_PriorityOptions: [],
				F_DoorPlateThicknessProps:{'label':'fullName','value':'enCode'},
				F_DoorPlateThicknessOptions: [],
				F_BoxPlateThicknessProps:{'label':'fullName','value':'enCode'},
				F_BoxPlateThicknessOptions: [],
				F_InstallOrBeamProps:{'label':'fullName','value':'enCode'},
				F_InstallOrBeamOptions: [],
				F_TypeProps:{
					"label": "F_Name",
					"value": "F_Id",
					"children": "children"
				},
				F_TypeOptions: [],
				f_CustomerIdProps:{'label':'F_GoodsName','value':'id'},
				F_CustomerIdOptions: [],
				f_ProjectIdProps:{'label':'F_ProjectName','value':'id'},
				F_ProjectIdOptions: [],
				f_ProdPlanIdProps:{'label':'F_PlanName','value':'id'},
				F_ProdPlanIdOptions: [],
				f_StateProps:{'label':'fullName','value':'enCode'},
				F_StateOptions: [],
				tableField0c720e_F_ProcessIdProps:{'label':'F_ProcessName','value':'id'},
				tableField0c720e_F_ProcessIdOptions : [],
				tableField449e6c_F_GoodsIdProps:{'label':'F_GoodsName','value':'id'},
				tableField449e6c_F_GoodsIdOptions : [],
				tableField449e6c_F_StockInProcessProps:{'label':'F_ProcessName','value':'id'},
				tableField449e6c_F_StockInProcessOptions : [],
				F_SequenceNoProps:{ label: 'F_RoutingName', value: 'id' },
				F_SequenceNoOptions: [],
				tableField751ecb_F_RoutingIdProps:{'label':'F_RoutingName','value':'id'},
				tableField751ecb_F_RoutingIdOptions : [],
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
			this.getf_BOMIdOptions();
			this.getf_PriorityOptions();
			this.getf_DoorPlateThicknessOptions();
			this.getf_BoxPlateThicknessOptions();
			this.getf_InstallOrBeamOptions();
			this.getf_TypeOptions();
			this.getf_CustomerIdOptions();
			this.getf_ProjectIdOptions();
			this.getf_ProdPlanIdOptions();
			this.getf_StateOptions();
			this.gettableField0c720e_F_ProcessIdOptions();
			this.gettableField449e6c_F_GoodsIdOptions();
			this.gettableField449e6c_F_StockInProcessOptions();
			this.getf_SequenceNoOptions();
			this.gettableField751ecb_F_RoutingIdOptions();
            uni.$on('linkPageConfirm', (subVal, Vmodel) => {
				let item = {
						F_SortCode:undefined,
						F_ProcessId:undefined,
						F_PlanStartDate:undefined,
						F_PlanEndDate:undefined,
						F_Description:undefined,
						F_CreatorTime:undefined,
					}
               if ('AProdTask' === Vmodel) subVal.forEach(t => this.dataForm.tableField0c720e.push({...item,...t}));
			   this.initCollapse();
            });
            uni.$on('linkPageConfirm', (subVal, Vmodel) => {
				let item = {
						F_SortCode:undefined,
						F_GoodsId:undefined,
						F_Unit:undefined,
						F_StockInProcess:undefined,
						F_Description:undefined,
						F_CreatorTime:undefined,
					}
               if ('AProdBomItem' === Vmodel) subVal.forEach(t => this.dataForm.tableField449e6c.push({...item,...t}));
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
			getf_BOMIdOptions(){
				this.F_BOMIdOptions = []
			    let templateJson = [
			      {
			        defaultValue: '',
			        field: 'goodsId',
			        dataType: 'varchar',
			        required: 0,
			        fieldName: '合同ID',
			        relationField: 'F_GoodsId',
			        jnpfKey: 'popupSelect',
			        complexHeaderList: null,
			        sourceType: 1,
			        isChildren: false,
			        IsMultiple: false,
			      },
			    ];	
				let query = {
					paramList: this.getParamList(templateJson, this.dataForm)
				}
				getDataInterfaceRes('2013188646957617152', query).then(res => {
					let data = res.data
					this.F_BOMIdOptions = Array.isArray(data) ? data : []
				});
			},
			getf_PriorityOptions(){
				getDictionaryDataSelector('2013182853290004480').then(res => {
					this.F_PriorityOptions = res.data.list
				});
			},
			getf_DoorPlateThicknessOptions(){
				getDictionaryDataSelector('2013183327061807104').then(res => {
					this.F_DoorPlateThicknessOptions = res.data.list
				});
			},
			getf_BoxPlateThicknessOptions(){
				getDictionaryDataSelector('2013183327061807104').then(res => {
					this.F_BoxPlateThicknessOptions = res.data.list
				});
			},
			getf_InstallOrBeamOptions(){
				getDictionaryDataSelector('2013183327061807104').then(res => {
					this.F_InstallOrBeamOptions = res.data.list
				});
			},
			getf_TypeOptions(){
				this.F_TypeOptions = []
				let templateJson = []
				let query = {
					paramList: this.getParamList(templateJson, this.dataForm)
				}
				getDataInterfaceRes('2008414575283802112', query).then(res => {
					let data = res.data
					this.F_TypeOptions = Array.isArray(data) ? data : []
				});
			},
			getf_CustomerIdOptions(){
				this.f_CustomerIdOptions = []
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
					paramList: this.getParamList(templateJson, this.dataForm)
				}
				getDataInterfaceRes('2012085692393459712', query).then(res => {
					let data = res.data
					this.f_CustomerIdOptions = Array.isArray(data) ? data : []
				});
			},
			getf_ProjectIdOptions(){
				this.f_ProjectIdOptions = []
				let templateJson = []
				let query = {
					paramList: this.getParamList(templateJson, this.dataForm)
				}
				getDataInterfaceRes('2013822233163730944', query).then(res => {
					let data = res.data
					this.f_ProjectIdOptions = Array.isArray(data) ? data : []
				});
			},
			getf_ProdPlanIdOptions(){
				this.f_ProdPlanIdOptions = []
				let templateJson = []
				let query = {
					paramList: this.getParamList(templateJson, this.dataForm)
				}
				getDataInterfaceRes('2013822515994038272', query).then(res => {
					let data = res.data
					this.f_ProdPlanIdOptions = Array.isArray(data) ? data : []
				});
			},
			getf_StateOptions(){
				getDictionaryDataSelector('2013819135649255424').then(res => {
					this.f_StateOptions = res.data.list
				});
			},
			gettableField0c720e_F_ProcessIdOptions(i){
				this.tableField0c720e_F_ProcessIdOptions = []
				let templateJson = []
				let query = {
					paramList: this.getParamList(templateJson, this.dataForm , i)
				}
				getDataInterfaceRes('2012092092830060544', query).then(res => {
					let data = res.data
					this.tableField0c720e_F_ProcessIdOptions = Array.isArray(data) ? data : []
				});
			},
			gettableField449e6c_F_GoodsIdOptions(i){
				this.tableField449e6c_F_GoodsIdOptions = []
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
					this.tableField449e6c_F_GoodsIdOptions = Array.isArray(data) ? data : []
				});
			},
			gettableField449e6c_F_StockInProcessOptions(i){
				this.tableField449e6c_F_StockInProcessOptions = []
				let templateJson = []
				let query = {
					paramList: this.getParamList(templateJson, this.dataForm , i)
				}
				getDataInterfaceRes('2012092092830060544', query).then(res => {
					let data = res.data
					this.tableField449e6c_F_StockInProcessOptions = Array.isArray(data) ? data : []
				});
			},
			getf_SequenceNoOptions(){
				// this.F_SequenceNoOptions = []
				let templateJson = []
				let query = {
					paramList: this.getParamList(templateJson, this.dataForm)
				}
				getDataInterfaceRes('2013852230570086400', query).then(res => {
					let data = res.data
					this.F_SequenceNoOptions = Array.isArray(data) ? data : []
				});
			},
			gettableField751ecb_F_RoutingIdOptions(i){
				this.tableField751ecb_F_RoutingIdOptions = []
				let templateJson = []
				let query = {
					paramList: this.getParamList(templateJson, this.dataForm , i)
				}
				getDataInterfaceRes('2013852230570086400', query).then(res => {
					let data = res.data
					this.tableField751ecb_F_RoutingIdOptions = Array.isArray(data) ? data : []
				});
			},
			selfInit(){
				this.addAProdTask();
				this.addAProdBomItem();
			},
			resetForm() {
				this.dataForm.tableField0c720e = [];
				this.dataForm.tableField449e6c = [];
				this.$refs.dataForm.resetFields();
				this.initCollapse();
			},
			initData() {
				const baseStore = useBaseStore()
				baseStore.updateRelationData({})
                if (this.dataForm.id) {
                    request({
                        url: '/api/example/AProdProcessing/' + this.dataForm.id,
                        method: 'get',
                    }).then(res => {
                        this.dataForm = res.data;
						if(!this.dataForm.F_DrawingFile)this.dataForm.F_DrawingFile=[];
						if(!this.dataForm.F_Type)this.dataForm.F_Type=[];
						if(!this.dataForm.F_BOMImages)this.dataForm.F_BOMImages=[];
                   this.tempGoodsSelector=[]
					this.dataForm.tableField449e6c.map(item=>{
					   this.tempGoodsSelector.push(item.F_GoodsId)
					})
					})
                }
            },
			validateSortCode(list) {
				for (let item of list) {
					if (item.F_SortCode === null || item.F_SortCode === undefined || item.F_SortCode === '' || isNaN(item.F_SortCode)) {
					uni.showToast({
						title: `排序不能为空`,
						icon: 'none',
						duration: 3000
					});
					return false; // 验证失败，返回false
					}
					if (item.F_Unit > item.F_InventoryNum) {
					uni.showToast({
						title: `排序${item.F_SortCode}单位用量不能大于库存数量`+item.F_InventoryNum,
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
					if (!this.validateSortCode(this.dataForm.tableField449e6c))  return; 
					      
                    this.btnLoading = true
                    if (this.dataForm.id) {
                        request({
                            url: '/api/example/AProdProcessing/' + this.dataForm.id,
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
                            url: '/api/example/AProdProcessing',
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
			F_GoodsIdChange(){
				this.dataForm.F_BOMId = undefined
				this.getf_BOMIdOptions()
			},
			addAProdTaskRow() {
				let item = {
						F_SortCode:undefined,
						F_ProcessId:undefined,
						F_PlanStartDate:undefined,
						F_PlanEndDate:undefined,
						F_Description:undefined,
						F_CreatorTime:undefined,
					}
				this.dataForm.tableField0c720e.push(item)
				this.initCollapse()
			},
			removeAProdTaskRow(i,showConfirm=false) {
				const handleRemove = () => {
					this.dataForm.tableField0c720e.splice(i, 1)
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
			copyAProdTaskRow(i) {
				let item = JSON.parse(JSON.stringify(this.dataForm.tableField0c720e[i]));
				item.id = '';
				item.length && item.map(o => delete o.rowData);
				item.F_CreatorTime = undefined;
				this.dataForm.tableField0c720e.push(item);
			},
			
			GoodsListBtn(val, option) {
			    /* 兜底 */
			    if (!Array.isArray(this.dataForm.tableField449e6c)) {
			      this.dataForm.tableField449e6c = [];
			    }
			    const optionIdSet = new Set(option.map(o => o.id));
			
			    /* 1. 删掉已取消勾选的行 */
			    this.dataForm.tableField449e6c = this.dataForm.tableField449e6c.filter(item => optionIdSet.has(item.F_GoodsId));
			
			    /* 2. 计算当前最大序号（0 表示还没有任何行） */
			    const maxSort = this.dataForm.tableField449e6c.reduce((max, item) => {
			      return item.F_SortCode > max ? item.F_SortCode : max;
			    }, 0);
			
			    /* 3. 追加本地还没有的行，序号依次累加 */
			    let nextSort = maxSort + 1;
			    option.forEach(o => {
			      const exist = this.dataForm.tableField449e6c.some(item => item.F_GoodsId === o.id);
			      if (!exist) {
					   getGoodsUnit(o.id).then(res => {
					            const newRow = {
					              jnpfId: buildUUID(),
					              F_GoodsId: o.id,
					              F_UnitTwo: res?.data,
					              F_Unit: 1,
					              F_NumUnit: res.data?.split('/').length > 1 ? res.data?.split('/')[1] : res.data?.split('/')[0],
					              F_InventoryNum: o.F_InventoryNum,
					              F_StockInProcess: undefined,
					              F_Description: undefined,
					              F_CreatorTime: undefined,
					              F_SortCode: nextSort, // ← 自动累加
					            };
					            this.dataForm.tableField449e6c.push(newRow);
					            this.tableField449e6cinnerActiveKey.push(newRow.jnpfId);
					            nextSort++; // 下一行继续 +1
					          });
			        // const newRow = {
			        //   jnpfId: buildUUID(),
			        //   F_GoodsId: o.id,
			        //   F_UnitTwo: o.F_Unit,
			        //   F_Unit: 1,
			        //   F_StockInProcess: undefined,
			        //   F_Description: undefined,
			        //   F_CreatorTime: undefined,
			        //   F_SortCode: nextSort, // ← 自动累加
			        // };
			        // this.dataForm.tableField449e6c.push(newRow);
			        // this.tableField449e6cinnerActiveKey.push(newRow.jnpfId);
			        // nextSort++; // 下一行继续 +1
			      }
			    });
			  },
			
			RoutingStrBtn(e){
				console.log('e',JSON.stringify(e))
				if(e){
					getRoutingStepList(e).then(res => {
						console.log('res',JSON.stringify(res))
						this.dataForm.tableField0c720e = []
						let nextSort = 1;
						res.data.forEach(o => {
						  const newRow = {
							jnpfId: buildUUID(),
							F_ProcessId: o.F_ProcessId,
							F_PlanStartDate: undefined,
							F_PlanEndDate: undefined,
							F_Description: undefined,
							F_CreatorTime: undefined,
							F_SortCode: nextSort, // ← 自动累加
						  };
						  this.dataForm.tableField0c720e.push(newRow);
						  nextSort++; // 下一行继续 +1
						});
					}).catch(err=>{
						console.log('error',err)
					})
				}
			},
			F_BomChange(e){
				console.log('e',JSON.stringify(e))
				if(e){
					getBOMGoodsList(e).then(res => {
						console.log('res',JSON.stringify(res))
						this.dataForm.tableField449e6c = [];
						this.GoodsList = [];
						let nextSort = 1;
						res.data.forEach(o => {
							let newRow = {
							  jnpfId: buildUUID(),
							  F_GoodsId: o.F_GoodsId,
							  F_UnitTwo: o.F_UnitTwo,
							  F_Unit: o.F_Unit,
							  F_StockInProcess: undefined,
							  F_Description: undefined,
							  F_CreatorTime: undefined,
							  F_SortCode: nextSort, // ← 自动累加
							};
							this.dataForm.tableField449e6c.push(newRow);
							this.tableField449e6cinnerActiveKey.push(newRow.jnpfId);
							nextSort++; // 下一行继续 +1
							this.GoodsList.push(o.F_GoodsId);
						});
					}).catch(err=>{
						console.log('error',err)
					})
				}
			},
			addAProdBomItemRow() {
				let item = {
						F_SortCode:undefined,
						F_GoodsId:undefined,
						F_Unit:undefined,
						F_StockInProcess:undefined,
						F_Description:undefined,
						F_CreatorTime:undefined,
					}
				this.dataForm.tableField449e6c.push(item)
				this.initCollapse()
			},
			removeAProdBomItemRow(i,showConfirm=false) {
				const handleRemove = () => {
					this.dataForm.tableField449e6c.splice(i, 1)
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
			copyAProdBomItemRow(i) {
				let item = JSON.parse(JSON.stringify(this.dataForm.tableField449e6c[i]));
				item.id = '';
				item.length && item.map(o => delete o.rowData);
				item.F_CreatorTime = undefined;
				this.dataForm.tableField449e6c.push(item);
			},
			// 处理单位用量变化
			handleF_UnitChange(record) {
			// 触发响应式更新,确保错误提示及时显示
			const index = this.dataForm.tableField449e6c.findIndex(item => item.jnpfId === record.jnpfId);
			if (index > -1) {
				this.dataForm.tableField449e6c[index] = { ...record };
				}
			}
        }
    };
</script>
<style>
        page{
                background-color: #f0f2f6;
        }
		
		.u-form-item{
			width: 100% !important;
		}
</style>



