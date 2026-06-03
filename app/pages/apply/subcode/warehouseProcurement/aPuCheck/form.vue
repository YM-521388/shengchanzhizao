﻿﻿﻿﻿﻿﻿﻿﻿<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" :rules="rules" ref="dataForm" :errorType="['toast']" label-position="left" :label-width="100 * 1.5" label-align="right" class="jnpf-form">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='盘点日期' prop="F_CheckDate" required  :label-width="100 * 1.5">
					<JnpfDatePicker v-model="dataForm.F_CheckDate" type='date' format='yyyy-MM-dd' placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='盘点人' prop="F_CheckUserId" required  :label-width="100 * 1.5">
					<JnpfUserSelect v-model="dataForm.F_CheckUserId" placeholder='请选择' selectType='all' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='盘点仓库' prop="F_WarehouseId" required  :label-width="100 * 1.5">
					<JnpfCascader @change='changeWarehouseId' v-model="dataForm.F_WarehouseId" :options="f_WarehouseIdOptions" :props="f_WarehouseIdProps" placeholder='请选择'  :disabled="Boolean(dataForm.id)" filterable clearable />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='备注' prop="F_Description"  :label-width="100 * 1.5">
					<JnpfTextarea v-model="dataForm.F_Description" placeholder='请输入'  :showCount='true' />
				</u-form-item>
			</view>
			<view class="jnpf-table">
				<view class="jnpf-table-title u-line-1" >{{'选择库存商品'}}
				</view>
				<view class="u-p-l-20 u-p-r-20 form-item-box">
					<u-form-item label='添加商品'  :label-width="100 * 1.5" @click="addGoods" >
						<!-- <span @click="addGoods">添加</span> -->
						<!-- <JnpfPopupSelect interfaceName='getPopSelect' multiple v-model="tempGoodsSelector" propsValue='F_Id' relationField='F_GoodsName' :columnOptions='tableFieldecf5cb_F_CustomerIdOptions' interfaceId='2008721559174385664' placeholder='选择商品' hasPage :pageSize='20' popupType='dialog' popupTitle='选择数据' popupWidth='70%' :templateJson='tableFieldecf5cb_F_CustomerIdTemplateJson' :vModel="'tableFieldecf5cb-F_CustomerId'" type="popup"  @change="changeGoods" :extraOptions="[]"/> -->
					</u-form-item>
				</view>
<!-- 				<view class="u-p-l-20 u-p-r-20 form-item-box">
					<u-form-item label='商品选择'  :label-width="100 * 1.5">
						<JnpfPopupSelect  :disabled="Boolean(dataForm.id)" multiple interfaceName='getPopSelect'  v-model="tempGoodsSelector" propsValue='F_Code' relationField='F_Code' :columnOptions='GoodsListOptions' interfaceId='2034873228723359744' placeholder='选择对应仓库后，再选择商品' :pageSize='20' popupType='dialog' popupTitle='选择数据' popupWidth='60%' :templateJson='tableField91ea29_F_CustomerIdTemplateJson' :vModel="'tableField91ea29-F_CustomerId'" type="popup" @change='handleGoods'/>
					</u-form-item>
				</view> -->
				<view class="jnpf-table-item" v-for="(item,i) in dataForm.tableField91ea29" :key="i">
					<view class="jnpf-table-item-title">
						<view class="jnpf-table-item-title-num">({{i+1}})</view>
						<view class="u-flex">
		        <view class="jnpf-table-delete-btn" @click="removeAPuCheckItemRow(i,true)">{{this.$t('common.delText','删除')}}</view>
						</view>
                    </view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='商品'  :label-width="100 * 1.5">
							<JnpfSelect disabled v-model="dataForm.tableField91ea29[i].F_CustomerId" :options="tableField91ea29_F_CustomerIdOptions" :props="tableField91ea29_F_CustomerIdProps" placeholder='请选择'  />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='库存编码' prop="F_Code"  :label-width="100 * 1.5">
							<JnpfInput disabled v-model="dataForm.tableField91ea29[i].F_Code"  :showCount='false' />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='规格' prop="F_Specification"  :label-width="100 * 1.5">
							<JnpfInput disabled v-model="dataForm.tableField91ea29[i].F_Specification" :showCount='false' />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='盘点前数量'  :label-width="100 * 1.5">
							<view class="form-value" v-if="getUnitRatioLevels(dataForm.tableField91ea29[i])?.level2?.name">
		                        <JnpfInputNumber v-model="dataForm.tableField91ea29[i].F_NumLevel1" placeholder="" :controls="false" disabled :style="{ width: '80px' }" />
		                        <text>{{ getUnitRatioLevels(dataForm.tableField91ea29[i])?.level1?.name }}</text>
		                        <JnpfInputNumber v-model="dataForm.tableField91ea29[i].F_NumLevel2" placeholder="" :controls="false" disabled :style="{ width: '80px' }" />
		                        <text>{{ getUnitRatioLevels(dataForm.tableField91ea29[i])?.level2?.name }}</text>
		                    </view>
		                    <view class="form-value"  v-else>
		                        <JnpfInputNumber v-model="dataForm.tableField91ea29[i].F_NumLevel1" placeholder="" :controls="false" disabled :style="{ width: '80px' }" />
		                        <text>{{ getUnitRatioLevels(dataForm.tableField91ea29[i])?.level1?.name }}</text>
		                    </view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='已盘点数量'  :label-width="100 * 1.5">
		                   <view class="form-value"  v-if="getUnitRatioLevels(dataForm.tableField91ea29[i])?.level2?.name">
		                        <JnpfInputNumber v-model="dataForm.tableField91ea29[i].F_CheckQtyDoneLevel1" placeholder="请输入" :controls="false"
		                          :min="0" disabled :style="{ width: '80px' }"
		                          @change="handleCheckQtyDoneLevel1Change(dataForm.tableField91ea29[i])" />
		                        <text>{{ getUnitRatioLevels(dataForm.tableField91ea29[i])?.level1?.name }}</text>
		                        <JnpfInputNumber v-model="dataForm.tableField91ea29[i].F_CheckQtyDoneLevel2" placeholder="请输入" :controls="false"
		                          :min="0" :max="dataForm.tableField91ea29[i]._F_NumLevel2Max" :disabled="Boolean(dataForm.tableField91ea29[i].F_Id)" :style="{ width: '80px' }"
		                          @change="handleCheckQtyDoneLevel2Change(dataForm.tableField91ea29[i])" />
		                        <text>{{ getUnitRatioLevels(dataForm.tableField91ea29[i])?.level2?.name }}</text>
		                    </view>
		                    <view class="form-value"  v-else>
		                        <JnpfInputNumber v-model="dataForm.tableField91ea29[i].F_CheckQtyDoneLevel1" placeholder="请输入" :controls="false"
		                          :min="0" :max="1" :disabled="Boolean(dataForm.tableField91ea29[i].F_Id)" :style="{ width: '80px' }"
		                          @change="handleCheckQtyDoneLevel1Change(dataForm.tableField91ea29[i])" />
		                        <text>{{ getUnitRatioLevels(dataForm.tableField91ea29[i])?.level1?.name }}</text>
		                    </view>
							<!-- <JnpfInputNumber v-model="dataForm.tableField91ea29[i].F_CheckQtyDone" :precision='0' :step='1.0'  /> -->
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='差异数量' prop="F_Differences"  :label-width="100 * 1.5">
	                        <view class="form-value" >
								<text>{{dataForm.tableField91ea29[i].F_CheckQtyDoneLevel2 - dataForm.tableField91ea29[i].F_NumLevel2}}</text>
	                        	<text>{{ getUnitRatioLevels(dataForm.tableField91ea29[i])?.level2?.name }}</text>
	                        </view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='备注'  :label-width="100 * 1.5">
							<JnpfTextarea v-model="dataForm.tableField91ea29[i].F_Description" placeholder='请输入'  :showCount='false' />
						</u-form-item>
					</view>
                </view>
<!-- 				<view class="jnpf-table-footer-btn" >
					<view class="jnpf-table-btn jnpf-table-primary-btn" @click="addAPuCheckItemRow()">
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
	import { getDictionaryDataSelector,getDataInterfaceRes,getDataInterfaceDataInfoByIds,getDataInterGoodsInventoryNo   } from '@/api/common'
	import { useBaseStore } from '@/store/modules/base'
    import request from '@/utils/request'
	import CustomButton from '@/components/CustomButton'
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
					F_CheckDate:undefined,
					F_CheckUserId:undefined,
					F_WarehouseId:undefined,
					F_this:undefined,
					F_Description:undefined,
					F_CreatorUserId:undefined,
					F_CreatorTime:undefined,
					tableField91ea29:[],
					tableField960db3:[],
                },
                rules: {
					F_CheckDate:[
						{
							required:true,
							message:this.$t('common.chooseText')+' '+'盘点日期',
							trigger:'change',
							type:'number'
						},
					],
					F_CheckUserId:[
						{
							required:true,
							message:this.$t('common.chooseText')+' '+'盘点人',
							trigger:'change',
						},
					],
					F_WarehouseId:[
						{
							required:true,
							message:this.$t('common.chooseText')+' '+'盘点仓库',
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
				userInfo:{},
				jurisdictionType:'',
				addType: 0,
				f_WarehouseIdProps:{'label':'F_Name','value':'F_Id',"children": "children"},
				f_WarehouseIdOptions: [],
				tableField91ea29_F_CustomerIdTemplateJson: [
		        {
		          defaultValue: '',
		          field: 'warehouseId',
		          dataType: 'varchar',
		          required: 0,
		          fieldName: '',
		          relationField: 'F_WarehouseId',
		          jnpfKey: 'select',
		          complexHeaderList: null,
		          sourceType: 2,
		          isChildren: false,
		          IsMultiple: false,
		        },
				],
				GoodsListOptions: [
				{
				  value: 'F_Code',
				  label: '库存编码',
				},
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
				  value: 'F_NumInfo',
				  label: '库存数量',
				},
				{
				  value: 'F_Remark',
				  label: '备注',
				},
				],
				tempGoodsSelector:[],
		      tableField91ea29_F_CustomerIdOptions: [],
		      tableField91ea29_F_CustomerIdProps: { label: 'F_GoodsName', value: 'id' },
            };
        },
		watch: {
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

            uni.$on('linkPageConfirm', (subVal, Vmodel) => {
				let item = {
						F_CustomerId:undefined,
						F_CheckQtyBefore:undefined,
						F_CheckQtyDone:undefined,
						F_Description:undefined,
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
						F_CustomerIdOptions:[],
					}
               if ('APuCheckItem' === Vmodel) subVal.forEach(t => this.dataForm.tableField91ea29.push({...item,...t}));
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
               if ('APuCheckLog' === Vmodel) subVal.forEach(t => this.dataForm.tableField960db3.push({...item,...t}));
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
    conversionDateTime(type) {
      const format = type === 'yyyy' ? 'yyyy-01-01 00:00:00' : type === 'yyyy-MM' ? 'yyyy-MM-01 00:00:00' :
        type === 'yyyy-MM-dd' ? 'yyyy-MM-dd 00:00:00' : type === 'yyyy-MM-dd HH:mm' ? 'yyyy-MM-dd HH:mm:00' : 'yyyy-MM-dd HH:mm:ss'
      const dataTime = this.jnpf.toDate(new Date(), format)
      return new Date(dataTime).getTime()
    },
	initDefaultData() {
      this.dataForm.F_CheckDate = this.conversionDateTime("yyyy-MM-dd");
      this.dataForm.F_CheckUserId = this.userInfo.userId
	},
			selfInit(){
				this.addAPuCheckItem();
				this.addAPuCheckLog();
			},
			resetForm() {
				this.dataForm.tableField91ea29 = [];
				this.dataForm.tableField960db3 = [];
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
                        url: '/api/example/APuCheck/' + this.dataForm.id,
                        method: 'get',
                    }).then(async res => {
					      this.dataForm = res.data || {};
					      this.dataForm.tableField91ea29 = this.dataForm.tableField91ea29 || [];
					      this.tempGoodsSelector = [];
						this.tableField91ea29_F_CustomerIdTemplateJson[0].relationField=JSON.stringify(this.dataForm.F_WarehouseId)
					      
						  // 收集需要获取商品信息的 ID
					      const goodsIdsToFetch = [];
					      for (let i = 0; i < this.dataForm.tableField91ea29.length; i++) {
					        const element = this.dataForm.tableField91ea29[i];
					        if (element.F_CustomerId && !element.F_Unit_Ratio) {
					          goodsIdsToFetch.push(element.F_CustomerId);
					        }
					      }
					
					      // 批量获取商品信息
					      const goodsIdMap = new Map();
					      if (goodsIdsToFetch.length > 0) {
					        try {
					          const uniqueIds = [...new Set(goodsIdsToFetch)];
					          const goodsRes = await getDataInterfaceDataInfoByIds('2008721559174385664', {
					            ids: uniqueIds,
					            interfaceId: '2008721559174385664',
					            propsValue: 'F_Id',
					            relationField: 'F_GoodsName',
					            paramList: [],
					          });
					          if (goodsRes.data) {
					            for (const goods of goodsRes.data) {
					              goodsIdMap.set(goods.F_Id, goods);
					            }
					          }
					        } catch (e) {
					          console.error('获取商品信息失败', e);
					        }
					      }
					
					      for (let i = 0; i < this.dataForm.tableField91ea29.length; i++) {
					        const element = this.dataForm.tableField91ea29[i];
					        element.jnpfId = buildUUID();
					        this.tempGoodsSelector.push(this.dataForm.tableField91ea29[i].F_Code);
					
					        // 如果没有 F_Unit_Ratio，从商品信息获取
					        if (!element.F_Unit_Ratio && element.F_CustomerId) {
					          const goodsInfo = goodsIdMap.get(element.F_CustomerId);
					          if (goodsInfo) {
					            element.F_Unit_Ratio = goodsInfo.F_Unit_Ratio;
					          }
					        }
					
					        // 使用 F_Unit_Ratio 计算数量层级
					        const levels = this.getUnitRatioLevels(element);
					        if (levels && levels.level2?.qty) {
					          const ratio = Number(levels.level2.qty) || 1;
					          const totalBefore = Number(element.F_CheckQtyBefore) || 0;
					          const totalDone = Number(element.F_CheckQtyDone) || 0;
					          // 盘点前数量
					          element.F_NumLevel1 = 1;
					          element.F_NumLevel2 = totalBefore;
					          element._F_NumLevel2Max = totalBefore;
					          // 已盘点数量
					          element.F_CheckQtyDoneLevel1 = 1;
					          element.F_CheckQtyDoneLevel2 = totalDone;
					          // 差异数量
					          const diff = totalDone - totalBefore;
					          element.F_Differences = diff;
					        } else {
					          // 没有二级单位
					          const totalBefore = Number(element.F_CheckQtyBefore) || 0;
					          const totalDone = Number(element.F_CheckQtyDone) || 0;
					          element.F_NumLevel1 = totalBefore;
					          element.F_NumLevel2 = 0;
					          element._F_NumLevel2Max = totalBefore;
					          element.F_CheckQtyDoneLevel1 = totalDone;
					          element.F_CheckQtyDoneLevel2 = 0;
					          const diff = totalDone - totalBefore;
					          element.F_Differences = diff;
					        }
					      }
					      this.gettableField91ea29_F_CustomerIdOptions();
						  
                    })
				}else{
					this.initDefaultData()
					this.gettableField91ea29_F_CustomerIdOptions();
				}
            },
			   tableField91ea29Exist() {
			    let isOk = true;
			    for (let i = 0; i < this.dataForm.tableField91ea29.length; i++) {
			      const e = this.dataForm.tableField91ea29[i];
			      if (!e.F_CustomerId) {
			        createMessage.error('商品' + t('sys.validate.textRequiredSuffix'));
			        isOk = false;
			        break;
			      }
			      if (!e.F_CheckQtyDone && e.F_CheckQtyDone != 0) {
			        createMessage.error('已盘点数量' + t('sys.validate.textRequiredSuffix'));
			        isOk = false;
			        break;
			      }
			    }
			    return isOk;
			  },
			submitForm(type) {
                this.$refs.dataForm.validate(valid => {
                    if (!valid) return
					try {
						// 提交前重新计算 F_CheckQtyDone
						for (const row of this.dataForm.tableField91ea29 || []) {
						  const levels = this.getUnitRatioLevels(row);
						  if (levels?.level2?.name) {
						    row.F_CheckQtyDone = Number(row.F_CheckQtyDoneLevel2) || 0;
						  } else {
						    // 无二级单位：F_CheckQtyDone = Level1
						    row.F_CheckQtyDone = Number(row.F_CheckQtyDoneLevel1) || 0;
						  }
						if (!this.tableField91ea29Exist()) return;
						}
						this.btnLoading = true
						if (this.dataForm.id) {
						    request({
						        url: '/api/example/APuCheck/' + this.dataForm.id,
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
						        url: '/api/example/APuCheck',
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
					} catch (error) {
						//TODO handle the exception
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
			addAPuCheckItemRow() {
				let item = {
						F_CustomerId:undefined,
						F_CheckQtyBefore:undefined,
						F_CheckQtyDone:undefined,
						F_Description:undefined,
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
						F_CustomerIdOptions:[],
					}
				this.dataForm.tableField91ea29.push(item)
				this.initCollapse()
			},
			removeAPuCheckItemRow(i,showConfirm=false) {
				const handleRemove = () => {
					this.dataForm.tableField91ea29.splice(i, 1)
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
			copyAPuCheckItemRow(i) {
				let item = JSON.parse(JSON.stringify(this.dataForm.tableField91ea29[i]));
				item.id = '';
				item.length && item.map(o => delete o.rowData);
				item.F_CreatorUserId = undefined;
				item.F_CreatorTime = undefined;
				this.dataForm.tableField91ea29.push(item);
			},
			addAPuCheckLogRow() {
				let item = {
						F_Type:undefined,
						F_Title:undefined,
						F_Content:undefined,
						F_Reason:undefined,
						F_CreatorUserId:undefined,
						F_CreatorTime:undefined,
					}
				this.dataForm.tableField960db3.push(item)
				this.initCollapse()
			},
			removeAPuCheckLogRow(i,showConfirm=false) {
				const handleRemove = () => {
					this.dataForm.tableField960db3.splice(i, 1)
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
			copyAPuCheckLogRow(i) {
				let item = JSON.parse(JSON.stringify(this.dataForm.tableField960db3[i]));
				item.id = '';
				item.length && item.map(o => delete o.rowData);
				item.F_CreatorUserId = undefined;
				item.F_CreatorTime = undefined;
				this.dataForm.tableField960db3.push(item);
			},
			changeWarehouseId(e){
					this.tableField91ea29_F_CustomerIdTemplateJson[0].relationField=JSON.stringify(e)
			},
			   gettableField91ea29_F_CustomerIdOptions() {
			    let templateJson = [
			      {
			        defaultValue: '',
			        field: 'contractId',
			        dataType: 'varchar',
			        required: 0,
			        fieldName: '合同ID',
			        relationField: '',
			        jnpfKey: null,
			        complexHeaderList: null,
			        sourceType: 3,
			        isChildren: false,
			        IsMultiple: false,
			      },
			    ];
			    let query = {
			      paramList: this.getParamList(templateJson, this.dataForm),
			    };
			    getDataInterfaceRes('2012085692393459712', query).then(res => {
			      let data = res.data;
			      this.tableField91ea29_F_CustomerIdOptions = Array.isArray(data) ? data : [];
			    });
			  },
			    addGoods(){
			      uni.scanCode({
			        scanType: ['barCode'],
			        success: (res) => {
			          console.log('扫描结果，', res.result)
			          let query = {
			            paramList: [{ "field": "goodsNo", "defaultValue": res.result, "id": "b6217b", "dataType": "varchar", "required": "0", "fieldName": "库存编码", "source": null }]
			          }
			          getDataInterGoodsInventoryNo('2036268230699520000', query).then(res => {
			            let data = res.data
			            console.log(data, '二维码结果');
			            const newRow = {
			              jnpfId: buildUUID(),
			              ...data,
			              F_CustomerId: data.id,
			              F_Code: data.F_InventoryNo,
			              F_CheckQtyBefore: data.F_Num,
			              F_CheckQtyDone: data.F_Num,
			            };
			            // 计算数量层级
			            const levels = this.getUnitRatioLevels(newRow);
			            if (levels && levels.level2?.qty) {
			              const ratio = Number(levels.level2.qty) || 1;
			              const totalBefore = Number(newRow.F_CheckQtyBefore) || 0;
			              const totalDone = Number(newRow.F_CheckQtyDone) || 0;
			              // 盘点前数量
			              newRow.F_NumLevel1 = 1;
			              newRow.F_NumLevel2 = totalBefore;
			              newRow._F_NumLevel2Max = ratio;
			              // 已盘点数量
			              newRow.F_CheckQtyDoneLevel1 = 1;
			              newRow.F_CheckQtyDoneLevel2 = totalDone;
			            } else {
			              // 没有二级单位，只有一级
			              newRow.F_NumLevel1 = Number(newRow.F_CheckQtyBefore) || 0;
			              newRow.F_NumLevel2 = 0;
			              newRow._F_NumLevel2Max = newRow.F_NumLevel1;
			              newRow.F_CheckQtyDoneLevel1 = Number(newRow.F_CheckQtyDone) || 0;
			              newRow.F_CheckQtyDoneLevel2 = 0;
			            }
			            this.dataForm.tableField91ea29.push(newRow);
			          });
			        }
			      });
			    },
				async  handleGoods(val, option) {
				    /* 兜底 */
				    if (!Array.isArray(this.dataForm.tableField91ea29)) {
				      this.dataForm.tableField91ea29 = [];
				    }
				    const optionIdSet = new Set(option.map(o => o.id));
				
				    /* 1. 删掉已取消勾选的行 */
				    this.dataForm.tableField91ea29 = this.dataForm.tableField91ea29.filter(item => optionIdSet.has(item.F_CustomerId));
				
				    /* 2. 计算当前最大序号（0 表示还没有任何行） */
				    const maxSort = this.dataForm.tableField91ea29.reduce((max, item) => {
				      return item.F_SortCode > max ? item.F_SortCode : max;
				    }, 0);
				    /* 3. 追加本地还没有的行，序号依次累加 */
				    let nextSort = maxSort + 1;
				
				    // 收集需要获取商品信息的 ID
				    const goodsIdsToFetch = [];
				    const goodsIdMap = new Map();
				
				    for (const o of option) {
				      const exist = this.dataForm.tableField91ea29.some(item => item.id === o.id);
				      if (!exist && o.F_GoodsId && !o.F_Unit_Ratio) {
				        goodsIdsToFetch.push(o.F_GoodsId);
				      }
				    }
				
				    // 批量获取商品信息
				    if (goodsIdsToFetch.length > 0) {
				      try {
				        const uniqueIds = [...new Set(goodsIdsToFetch)];
				        const goodsRes = await getDataInterfaceDataInfoByIds('2008721559174385664', {
				          ids: uniqueIds,
				          interfaceId: '2008721559174385664',
				          propsValue: 'F_Id',
				          relationField: 'F_GoodsName',
				          paramList: [],
				        });
				        if (goodsRes.data) {
				          for (const goods of goodsRes.data) {
				            goodsIdMap.set(goods.F_Id, goods);
				          }
				        }
				      } catch (e) {
				        console.error('获取商品信息失败', e);
				      }
				    }
				
				    for (const o of option) {
				      const exist = this.dataForm.tableField91ea29.some(item => item.id === o.id);
				      if (!exist) {
				        // 获取单位比例：优先使用接口返回的，否则从商品信息获取
				        let unitRatio = o.F_Unit_Ratio;
				        if (!unitRatio && o.F_GoodsId) {
				          const goodsInfo = goodsIdMap.get(o.F_GoodsId);
				          if (goodsInfo) {
				            unitRatio = goodsInfo.F_Unit_Ratio;
				          }
				        }
				        const newRow= {
				          jnpfId: buildUUID(),
				          F_CustomerId: o.F_GoodsId,
				          F_Code: o.F_Code,
				          F_Specification: o.F_Specification,
				          F_Unit: o.F_Unit,
				          F_Unit_Ratio: unitRatio,
				          F_CheckQtyBefore: o.F_Num,
				          F_CheckQtyDone: o.F_Num,
				          F_Differences: 0,
				          F_Description: undefined,
				          F_CreatorUserId: undefined,
				          F_CreatorTime: undefined
,
				        };
				        // 计算数量层级
				        const levels = this.getUnitRatioLevels(newRow);
				        if (levels && levels.level2?.qty) {
				          const ratio = Number(levels.level2.qty) || 1;
				          const totalBefore = Number(newRow.F_CheckQtyBefore) || 0;
				          const totalDone = Number(newRow.F_CheckQtyDone) || 0;
				          // 盘点前数量
				          newRow.F_NumLevel1 = 1;
				          newRow.F_NumLevel2 = totalBefore;
				          newRow._F_NumLevel2Max = ratio;
				          // 已盘点数量
				          newRow.F_CheckQtyDoneLevel1 = 1;
				          newRow.F_CheckQtyDoneLevel2 = totalDone;
				        } else {
				          // 没有二级单位，只有一级
				          newRow.F_NumLevel1 = Number(newRow.F_CheckQtyBefore) || 0;
				          newRow.F_NumLevel2 = 0;
				          newRow._F_NumLevel2Max = newRow.F_NumLevel1;
				          newRow.F_CheckQtyDoneLevel1 = Number(newRow.F_CheckQtyDone) || 0;
				          newRow.F_CheckQtyDoneLevel2 = 0;
				        }
				        this.dataForm.tableField91ea29.push(newRow);
				        nextSort++; // 下一行继续 +1
				      }
				    }
				  },
				// ========================处理单位======================start
				  // 解析 F_Unit_Ratio，只取前两级，返回 { level1: { name, qty }, level2: { name, qty } } 或 null
				   getUnitRatioLevels(record) {
				    const raw = record?.F_Unit_Ratio;
				    if (!raw) return null;
				    try {
				      const str = typeof raw === 'string' ? raw : String(raw);
				      const arr = JSON.parse(str);
				      if (!Array.isArray(arr) || arr.length < 1) return null;
				      if (arr.length < 2) return { level1: arr[0], level2: null };
				      return { level1: arr[0], level2: arr[1] };
				    } catch (e) {
				      return null;
				    }
				  },
				
				  // 一级数量变化处理
				   handleCheckQtyDoneLevel1Change(record) {
				    const levels = this.getUnitRatioLevels(record);
				    if (!levels || !levels.level2?.qty) {
				      record.F_CheckQtyDone = Number(record.F_CheckQtyDoneLevel1) || 0;
				      this.QtyDoneBtn(record.jnpfId);
				      return;
				    }
				    // 有二级单位时，一级数量变化会改变总数量
				    const ratio = Number(levels.level2.qty) || 1;
				    const qty1 = Number(record.F_CheckQtyDoneLevel1) || 0;
				    const qty2 = Number(record.F_CheckQtyDoneLevel2) || 0;
				    record.F_CheckQtyDone = qty1 * ratio + qty2;
				    this.QtyDoneBtn(record.jnpfId);
				  },
				
				  // 二级数量变化处理
				   handleCheckQtyDoneLevel2Change(record) {
				    const levels = this.getUnitRatioLevels(record);
				    if (!levels || !levels.level2?.qty) {
				      record.F_CheckQtyDone = Number(record.F_CheckQtyDoneLevel2) || 0;
				      this.QtyDoneBtn(record.jnpfId);
				      return;
				    }
				    // 有二级单位时，二级数量变化会改变总数量
				    const ratio = Number(levels.level2.qty) || 1;
				    const qty1 = Number(record.F_CheckQtyDoneLevel1) || 0;
				    const qty2 = Number(record.F_CheckQtyDoneLevel2) || 0;
				    record.F_CheckQtyDone = qty1 * ratio + qty2;
				    this.QtyDoneBtn(record.jnpfId);
				  },
				QtyDoneBtn(jnpfId) {
				      // 使用 find 直接获取对象引用
				      const row = this.dataForm.tableField91ea29?.find(item => item.jnpfId === jnpfId);
				  
				      if (row) {
				        // 计算总数量
				        const levels = this.getUnitRatioLevels(row);
				        if (levels && levels.level2?.qty) {
				          const ratio = Number(levels.level2.qty) || 1;
				          row.F_CheckQtyDone = (Number(row.F_CheckQtyDoneLevel1) || 0) * ratio + (Number(row.F_CheckQtyDoneLevel2) || 0);
				        } else {
				          row.F_CheckQtyDone = Number(row.F_CheckQtyDoneLevel1) || Number(row.F_CheckQtyDoneLevel2) || 0;
				        }
				        // 计算差异数量
				        const diff = (row.F_CheckQtyDone || 0) - (row.F_CheckQtyBefore || 0);
				        row.F_Differences = diff;
				      }
				    }
				// ========================处理单位======================end
			
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



