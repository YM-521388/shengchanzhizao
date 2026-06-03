﻿﻿﻿﻿﻿﻿<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" :rules="rules" ref="dataForm" :errorType="['toast']" label-position="left" :label-width="100 * 1.5" label-align="right" class="jnpf-form">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='项目编号' prop="F_ProjectNo"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_ProjectNo" placeholder='请填写，忽略将自动生成'  :showCount='false' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='项目名称' prop="F_ProjectName" required  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_ProjectName" placeholder='请输入'  :showCount='false' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='项目类别' prop="F_ProjectType"  :label-width="100 * 1.5">
					<JnpfSelect v-model="dataForm.F_ProjectType" :options="f_ProjectTypeOptions" :props="f_ProjectTypeProps" filterable placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box" v-if="dataForm.F_ProjectType == 1">
				<u-form-item label='合同' prop="F_ContractId" required  :label-width="100 * 1.5">
					<JnpfSelect @change="handleContract" v-model="dataForm.F_ContractId" :options="optionsObj.F_ContractIdOptions" :props="optionsObj.F_ContractIdProps" filterable placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="jnpf-table">
				<view class="jnpf-table-title u-line-1" >{{'商品'}}
				</view>
				<view class="u-p-l-20 u-p-r-20 form-item-box">
					<u-form-item label='选择商品'  :label-width="100 * 1.5">
						<JnpfPopupSelect interfaceName='getPopSelect' 
						multiple v-model="tempGoodsSelector" 
						propsValue='id' 
						relationField='F_GoodsName' 
						:columnOptions='optionsObj.GoodsListOptions' 
						interfaceId='2029803158963884032' 
						placeholder='请选择' hasPage :pageSize='20' 
						popupType='dialog' popupTitle='选择数据' 
						:templateJson='optionsObj.GoodsListTemplateJson' 
						:vModel="'tableFieldc87c94-F_CustomerId'" type="popup"  
						@change="GoodsListBtn" 
						:extraOptions="optionsObj.GoodsList" 
						style='width: 100% !important;'/>
					</u-form-item>
				</view>
				<view class="jnpf-table-item" v-for="(item,i) in dataForm.tableField751ecb" :key="i">
					<view class="jnpf-table-item-title">
						<view class="jnpf-table-item-title-num">({{i+1}})</view>
						<view class="u-flex">
							<view class="jnpf-table-delete-btn" @click="removeAProdProjectItemRow(i,true)">{{this.$t('common.delText','删除')}}</view>
						</view>
                    </view>
					
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='工单编号'  :label-width="100 * 1.5">
							<JnpfInput v-model="dataForm.tableField751ecb[i].F_WorkOrderNo" placeholder='请填写，忽略将自动生成'  :showCount='false' />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='商品' required  :label-width="100 * 1.5">
							<JnpfSelect v-model="dataForm.tableField751ecb[i].F_GoodsId" :options="tableField751ecb_F_GoodsIdOptions" :props="tableField751ecb_F_GoodsIdProps" disabled @change='F_GoodsIdTableChange(i)' />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='计划数' required  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableField751ecb[i].F_PlanNum" :precision='0' :min='1' :step='1.0'  />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='交货日期'  :label-width="100 * 1.5">
							<JnpfDatePicker v-model="dataForm.tableField751ecb[i].F_DeliveryDate" type='date' format='yyyy-MM-dd' placeholder='请选择' />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='优先级'  :label-width="100 * 1.5">
							<JnpfSelect v-model="dataForm.tableField751ecb[i].F_Priority" placeholder='请选择' :options="tableField751ecb_F_PriorityOptions" :props="tableField751ecb_F_PriorityProps" filterable />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='BOM'  :label-width="100 * 1.5">
							<JnpfSelect v-model="dataForm.tableField751ecb[i].F_BomId" 
							:options="tableField751ecb_F_BomIdOptions" 
							:props="tableField751ecb_F_BomIdProps" filterable 
							placeholder='请选择' :disabled="!dataForm.tableField751ecb[i].F_GoodsId"
							propsValue="id"/>
							<!-- 
							<JnpfSelect v-model="dataForm.tableField751ecb[i].F_BomId" 
							placeholder='请选择' 
							:options="dataForm.tableField751ecb[i].F_BomIdOptions" 
							:props="tableField751ecb_F_BomIdProps" filterable /> -->
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='图纸'  :label-width="100 * 1.5">
							<JnpfUploadFile v-model="dataForm.tableField751ecb[i].F_DrawingFiles" :limit='9' sizeUnit='MB' :fileSize='10' pathType="selfPath"  :sortRule='[
  "1",
  "2"
]' timeFormat='YYYYMMDD' buttonText='点击上传' />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='客户' required  :label-width="100 * 1.5">
							<JnpfInput v-model="dataForm.tableField751ecb[i].F_CustomerName" placeholder='请输入'  :showCount='false' />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='BOM图片'  :label-width="100 * 1.5">
							<JnpfUploadImg v-model="dataForm.tableField751ecb[i].F_BomImages" pathType="selfPath" sizeUnit='MB' :fileSize='10'  :sortRule='[
  "1",
  "2"
]' timeFormat='YYYYMMDD' buttonText='点击上传' />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='类别' required  :label-width="100 * 1.5">
							<JnpfCascader v-model="dataForm.tableField751ecb[i].F_Category" placeholder='请选择' :options="tableField751ecb_F_CategoryOptions" :props="tableField751ecb_F_CategoryProps" filterable clearable />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='工艺路线' required  :label-width="100 * 1.5">
							<JnpfSelect v-model="dataForm.tableField751ecb[i].F_RoutingId" placeholder='请选择' :options="tableField751ecb_F_RoutingIdOptions" :props="tableField751ecb_F_RoutingIdProps" filterable />
						</u-form-item>
					</view>
                </view>
				<!-- <view class="jnpf-table-footer-btn" >
					<view class="jnpf-table-btn jnpf-table-primary-btn" @click="addAProdProjectItemRow()">
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
	import { getDictionaryDataSelector,getDataInterfaceRes } from '@/api/common'
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
					F_ProjectNo:undefined,
					F_ProjectName:undefined,
					F_ContractId:undefined,
					F_ProjectType:undefined,
					F_Status:undefined,
					F_CreatorUserId:undefined,
					F_CreatorTime:undefined,
					F_UpdateUserId:undefined,
					F_UpdateTime:undefined,
					tableField751ecb:[],
					GoodsList: []
                },
                rules: {
					F_ProjectName:[
						{
							required:true,
							message:this.$t('common.inputText')+' '+'项目名称',
							trigger:'blur',
						},
					],
					F_ContractId:[
						{
							required:true,
							message:this.$t('common.inputText')+' '+'合同',
							trigger:'change',
						},
					],
                },
				ruleList: {
					tableField751ecb: {
						'F_GoodsId': '商品不能为空',
						'F_PlanNum': '计划数不能为空',
						'F_CustomerName': '客户不能为空',
						'F_Category': '类别不能为空',
						'F_RoutingId': '工艺路线不能为空',
					},
				},
				regList: {
				},
				tableField751ecb_F_BomIdOptions: [],
				index: 0,
				idList: [],
				dataValue: {},
				jurisdictionType:'',
				addType: 0,
				F_ContractIdTemplateJson: [
				  {
					"defaultValue": "",
					"field": "contractId",
					"dataType": "varchar",
					"required": 0,
					"fieldName": "合同ID",
					"relationField": "66666",
					"jnpfKey": null,
					"complexHeaderList": null,
					"sourceType": 2,
					"isChildren": false,
					"IsMultiple": false
				  }
				],
				F_ContractIdOptions: [
				  {
					"value": "F_GoodsName",
					"label": "商品名"
				  },
				  {
					"value": "F_GoodsNo",
					"label": "商品编号"
				  },
				  {
					"value": "F_Specification",
					"label": "商品规格"
				  },
				  {
					"value": "F_CategoryId",
					"label": "商品类别"
				  },
				  {
					"value": "F_Unit",
					"label": "单位"
				  },
				  {
					"value": "F_InspectRule",
					"label": "检验规范"
				  },
				  {
					"value": "F_Remark",
					"label": "备注"
				  }
				],
				f_ProjectTypeProps:{'label':'fullName','value':'enCode'},
				f_ProjectTypeOptions: [],
				f_StatusProps:{'label':'fullName','value':'enCode'},
				f_StatusOptions: [],
				tableField751ecb_F_GoodsIdProps:{'label':'F_GoodsName','value':'id'},
				tableField751ecb_F_GoodsIdOptions : [],
				tableField751ecb_F_PriorityProps:{'label':'fullName','value':'enCode'},
				tableField751ecb_F_PriorityOptions : [],
				tableField751ecb_F_BomIdProps:{'label':'F_BomName','value':'id'},
				tableField751ecb_F_DoorPlateThicknessProps:{'label':'fullName','value':'enCode'},
				tableField751ecb_F_DoorPlateThicknessOptions : [],
				tableField751ecb_F_BoxPlateThicknessProps:{'label':'fullName','value':'enCode'},
				tableField751ecb_F_BoxPlateThicknessOptions : [],
				tableField751ecb_F_InstallPlateOrBeamProps:{'label':'fullName','value':'enCode'},
				tableField751ecb_F_InstallPlateOrBeamOptions : [],
				tableField751ecb_F_CategoryProps:{
				  "label": "F_Name",
				  "value": "F_Id",
				  "children": "children"
				},
				tableField751ecb_F_CategoryOptions: [],
				tableField751ecb_F_RoutingIdProps:{'label':'F_RoutingName','value':'id'},
				tableField751ecb_F_RoutingIdOptions : [],
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
					  label: '商品类型',
					},
					{
					  value: 'F_Remark',
					  label: '备注',
					},
				  ],
				  GoodsListTemplateJson: [
					{
					  defaultValue: '',
					  field: 'contractId',
					  dataType: 'varchar',
					  required: 0,
					  fieldName: '合同',
					  relationField: 'F_ContractId',
					  jnpfKey: 'select',
					  complexHeaderList: null,
					  sourceType: 1,
					  isChildren: false,
					  IsMultiple: false,
					},
				  ],
					GoodsList: [],
					F_ContractIdOptions: [],
					F_ContractIdProps: { label: 'F_ContractCode', value: 'F_Id' },

				}
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
			this.getf_ProjectTypeOptions();
			this.getf_StatusOptions();
			this.getF_ContractIdOptions();
			this.gettableField751ecb_F_GoodsIdOptions();
			this.gettableField751ecb_F_PriorityOptions();
			this.gettableField751ecb_F_DoorPlateThicknessOptions();
			this.gettableField751ecb_F_BomIdOptions();
			this.gettableField751ecb_F_BoxPlateThicknessOptions();
			this.gettableField751ecb_F_InstallPlateOrBeamOptions();
			this.gettableField751ecb_F_CategoryOptions();
			this.gettableField751ecb_F_RoutingIdOptions();
            uni.$on('linkPageConfirm', (subVal, Vmodel) => {
				let item = {
						F_WorkOrderNo:undefined,
						F_GoodsId:undefined,
						F_PlanNum:1,
						F_DeliveryDate:undefined,
						F_Priority:undefined,
						F_BomId:undefined,
						F_DrawingFiles:[],
						F_DoorPlateThickness:undefined,
						F_BoxPlateThickness:undefined,
						F_InstallPlateOrBeam:undefined,
						F_CustomerName:undefined,
						F_LockSet:undefined,
						F_Hinge:undefined,
						F_BomImages:[],
						F_Color:undefined,
						F_Category:[],
						F_ParentId:undefined,
						F_CreatorTime:undefined,
						F_BomIdOptions:[],
					}
               if ('AProdProjectItem' === Vmodel) subVal.forEach(t => this.dataForm.tableField751ecb.push({...item,...t}));
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
			getf_ProjectTypeOptions(){
				getDictionaryDataSelector('2013172950349516800').then(res => {
					this.f_ProjectTypeOptions = res.data.list
				});
			},
			getf_StatusOptions(){
				getDictionaryDataSelector('2013173013452820480').then(res => {
					this.f_StatusOptions = res.data.list
				});
			},
			getF_ContractIdOptions() {
			    let templateJson = [];
			    let query = {
			      paramList: this.getParamList(templateJson, this.dataForm),
			    };
			    getDataInterfaceRes('2010889611072638976', query).then(res => {
			      let data = res.data;
			      this.optionsObj.F_ContractIdOptions = Array.isArray(data) ? data : [];
			    });
			  },
			gettableField751ecb_F_GoodsIdOptions(i){
				this.tableField751ecb_F_GoodsIdOptions = []
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
				getDataInterfaceRes('2029803158963884032', query).then(res => {
					let data = res.data
					this.tableField751ecb_F_GoodsIdOptions = Array.isArray(data) ? data : [];
				});
			},
			gettableField751ecb_F_PriorityOptions(i){
				getDictionaryDataSelector('2013182853290004480').then(res => {
					this.tableField751ecb_F_PriorityOptions = res.data.list
				});
			},
			gettableField751ecb_F_BomIdOptions(i){
				// this.dataForm.tableField751ecb[i].F_BomIdOptions = []
				let templateJson = [
				  {
					"defaultValue": "",
					"field": "goodsId",
					"dataType": "varchar",
					"required": 0,
					"fieldName": "合同ID",
					"relationField": "tableField751ecb-F_GoodsId",
					"jnpfKey": "select",
					"complexHeaderList": null,
					"sourceType": 1,
					"isChildren": false,
					"IsMultiple": false
				  }
				]
				let query = {
					paramList: this.getParamList(templateJson, this.dataForm , i)
				}
				getDataInterfaceRes('2013188646957617152', query).then(res => {
					let data = res.data
					this.tableField751ecb_F_BomIdOptions = Array.isArray(data) ? data : []
				});
			},
			
			gettableField751ecb_F_DoorPlateThicknessOptions(i){
				getDictionaryDataSelector('2013183327061807104').then(res => {
					this.tableField751ecb_F_DoorPlateThicknessOptions = res.data.list
				});
			},
			gettableField751ecb_F_BoxPlateThicknessOptions(i){
				getDictionaryDataSelector('2013183327061807104').then(res => {
					this.tableField751ecb_F_BoxPlateThicknessOptions = res.data.list
				});
			},
			gettableField751ecb_F_InstallPlateOrBeamOptions(i){
				getDictionaryDataSelector('2013183327061807104').then(res => {
					this.tableField751ecb_F_InstallPlateOrBeamOptions = res.data.list
				});
			},
			gettableField751ecb_F_CategoryOptions(i){
				this.tableField751ecb_F_CategoryOptions = []
				let templateJson = []
				let query = {
					paramList: this.getParamList(templateJson, this.dataForm , i)
				}
				getDataInterfaceRes('2008414575283802112', query).then(res => {
					let data = res.data
					this.tableField751ecb_F_CategoryOptions = Array.isArray(data) ? data : []
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
				this.addAProdProjectItem();
			},
			resetForm() {
				this.dataForm.tableField751ecb = [];
				this.$refs.dataForm.resetFields();
				this.initCollapse();
			},
			GoodsListBtn(val, option) {
			    /* 兜底保证是数组 */
			    if (!Array.isArray(this.dataForm.tableField751ecb)) {
			      this.dataForm.tableField751ecb = [];
			    }
			
			    /* 1. 把 option 里的 id 做成 Set，方便比对 */
			    const optionIdSet = new Set(option.map(o => o.id));
			
			    /* 2. 删除本地已不在 option 里的行 */
			    this.dataForm.tableField751ecb = this.dataForm.tableField751ecb.filter(item => optionIdSet.has(item.F_GoodsId));
			
			    /* 3. 把 option 里本地还没有的行加进来 */
			    option.forEach((o, i) => {
			      const exist = this.dataForm.tableField751ecb.findIndex(item => item.F_GoodsId === o.id) !== -1;
			      if (!exist) {
			        const newRow = {
			          jnpfId: buildUUID(),
			          F_GoodsId: o.id,
			          F_WorkOrderNo: undefined,
			          F_PlanNum: 1,
			          F_DeliveryDate: undefined,
			          F_Priority: undefined,
			          F_BomId: undefined,
			          tableField751ecb_F_BomIdOptions: [],
			          F_DrawingFiles: [],
			          F_DoorPlateThickness: undefined,
			          F_BoxPlateThickness: undefined,
			          F_InstallPlateOrBeam: undefined,
			          F_CustomerName: undefined,
			          F_LockSet: undefined,
			          F_Hinge: undefined,
			          F_BomImages: [],
			          F_Color: undefined,
			          F_Category: [],
			          F_CreatorTime: undefined,
			        };
			        this.dataForm.tableField751ecb.push(newRow);			       
			      }
			      this.gettableField751ecb_F_BomIdOptions(i);
			    });
			},
			// gettableField751ecb_F_BomIdOptions(i) {
			//       let templateJson = [
			//         {
			//           defaultValue: '',
			//           field: 'goodsId',
			//           dataType: 'varchar',
			//           required: 0,
			//           fieldName: '合同ID',
			//           relationField: 'tableField751ecb-F_GoodsId',
			//           jnpfKey: 'select',
			//           complexHeaderList: null,
			//           sourceType: 1,
			//           isChildren: false,
			//           IsMultiple: false,
			//         },
			//       ];
			//       let query = {
			//         paramList: getParamList(templateJson, state.dataForm, i),
			//       };
			//       getDataInterfaceRes('2013188646957617152', query).then(res => {
			//         let data = res.data;
			//         this.dataForm.tableField751ecb[i].tableField751ecb_F_BomIdOptions = Array.isArray(data) ? data : [];
			//       });
			// },
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
                        url: '/api/example/AProdProject/' + this.dataForm.id,
                        method: 'get',
                    }).then(res => {
                        this.dataForm = res.data;
						this.tempGoodsSelector = []
						this.dataForm.tableField751ecb.map(item=>{
						this.tempGoodsSelector.push(item.F_GoodsId);
						})
                    })
                }
            },
			submitForm(type) {
                this.$refs.dataForm.validate(valid => {
                    if (!valid) return
					if (!!this.checkChildRule()) return this.$u.toast(
						this.checkChildRule()
					)
                    this.btnLoading = true
                    if (this.dataForm.id) {
                        request({
                            url: '/api/example/AProdProject/' + this.dataForm.id,
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
                            url: '/api/example/AProdProject',
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
			F_GoodsIdTableChange(i){
				this.dataForm.tableField751ecb[i].F_BomId = undefined;
				this.gettableField751ecb_F_BomIdOptions(i);
				// this.dataForm.tableField751ecb[i].F_BomId = undefined;
				// this.gettableField751ecb_F_BomIdOptions(i);
				// this.dataForm.tableField751ecb[i].F_BomId = undefined;
				// this.gettableField751ecb_F_BomIdOptions(i);
			},
			addAProdProjectItemRow() {
				let item = {
						F_WorkOrderNo:undefined,
						F_GoodsId:undefined,
						F_PlanNum:1,
						F_DeliveryDate:undefined,
						F_Priority:undefined,
						F_BomId:undefined,
						F_DrawingFiles:[],
						F_DoorPlateThickness:undefined,
						F_BoxPlateThickness:undefined,
						F_InstallPlateOrBeam:undefined,
						F_CustomerName:undefined,
						F_LockSet:undefined,
						F_Hinge:undefined,
						F_BomImages:[],
						F_Color:undefined,
						F_Category:[],
						F_ParentId:undefined,
						F_CreatorTime:undefined,
						F_BomIdOptions:[],
					}
				this.dataForm.tableField751ecb.push(item)
				this.gettableField751ecb_F_BomIdOptions(this.dataForm.tableField751ecb.length - 1)
				this.gettableField751ecb_F_BomIdOptions(this.dataForm.tableField751ecb.length - 1)
				this.gettableField751ecb_F_BomIdOptions(this.dataForm.tableField751ecb.length - 1)
				this.initCollapse()
			},
			removeAProdProjectItemRow(i,showConfirm=false) {
				const handleRemove = () => {
					this.dataForm.tableField751ecb.splice(i, 1)
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
			copyAProdProjectItemRow(i) {
				let item = JSON.parse(JSON.stringify(this.dataForm.tableField751ecb[i]));
				item.id = '';
				item.length && item.map(o => delete o.rowData);
				item.F_CreatorTime = undefined;
				this.dataForm.tableField751ecb.push(item);
			},
			handleContract(e){
				this.optionsObj.GoodsListTemplateJson[0].defaultValue=e
			}
        }
    };
</script>
<style>
        page{
                background-color: #f0f2f6;
        }
</style>



