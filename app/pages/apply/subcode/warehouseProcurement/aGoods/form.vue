﻿﻿﻿﻿<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" :rules="rules" ref="dataForm" :errorType="['toast']" label-position="left" :label-width="100 * 1.5" label-align="right" class="jnpf-form">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='商品编号' prop="F_GoodsNo"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_GoodsNo" placeholder='请填写，忽略将自动生成'  :showCount='false' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='商品名称' prop="F_GoodsName" required  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_GoodsName" placeholder='请输入'  :showCount='false' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='类别' prop="F_CategoryId"  :label-width="100 * 1.5">
					<JnpfCascader v-model="dataForm.F_CategoryId" :options="f_CategoryIdOptions" :props="f_CategoryIdProps" placeholder='请选择' clearable />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='单位' prop="F_Unit"  :label-width="100 * 1.5">
					<JnpfCascader v-model="dataForm.F_Unit" :options="f_UnitOptions" :props="f_UnitProps" placeholder='请选择' filterable clearable />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='单位比例' prop="F_Unit_Ratio"  :label-width="100 * 1.5">
					<!-- <JnpfInput v-model="dataForm.F_Unit_Ratio" placeholder='请输入'  :showCount='false' /> -->
					<template #label>单位比例</template>
		              <template v-if="unitRatioList.length > 0">
		                <div class="unit-ratio-table">
		                  <table class="jnpf-unit-ratio-table">
		                    <thead>
		                      <tr>
		                        <th>名称</th>
		                        <th>数量</th>
		                      </tr>
		                    </thead>
		                    <tbody>
		                      <tr v-for="(row, index) in unitRatioList" :key="row.id">
		                        <td>{{ row.name }}</td>
		                        <td>
		                          <JnpfInputNumber v-model="row.qty" placeholder="请输入" :min="0" :controls="true"
		                            :disabled="index === 0" :style="{ width: '100%' }"
		                            @update="val => updateUnitRatioQty(index, val)" />
		                        </td>
		                      </tr>
		                    </tbody>
		                  </table>
		                </div>
		              </template>
		              <JnpfInput v-else v-model:value="dataForm.F_Unit_Ratio" placeholder="请先选择单位" allowClear disabled
		                :style="{ width: '100%' }" :showCount="false" />
				</u-form-item>
			</view>				
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='销售单价' prop="F_SalePrice"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_SalePrice" :step='1.0'  controls/>
				</u-form-item>
			</view>		
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='成本单价' prop="F_CostPrice"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_CostPrice" :step='1.0'  />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='售后佣金' prop="F_CommissionFixed"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_CommissionFixed" :step='1.0'  />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='商品来源' prop="F_Source"  :label-width="100 * 1.5">
					<JnpfSelect v-model="dataForm.F_Source" :options="f_SourceOptions" :props="f_SourceProps" placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='规格' prop="F_Specification"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_Specification" placeholder='请输入'  :showCount='false' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='外协单价' prop="F_OutsourcePrice"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_OutsourcePrice" :step='1.0'  />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='供应商' prop="F_SupplierId"  :label-width="100 * 1.5">
					<JnpfSelect v-model="dataForm.F_SupplierId" :options="f_SupplierIdOptions" :props="f_SupplierIdProps" placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='检验规范' prop="F_InspectRule"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_InspectRule" placeholder='请输入'  :showCount='false' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='库存上限警告值' prop="F_StockUpperLimit" left-icon="question-circle-fill" @clickIcon="clickIcon(label='库存上限警告值', '设置该商品在仓库中的默认库存上限的警告值。' )" :left-icon-style="{'color':'#a0acb7'}"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_StockUpperLimit"   />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='库存下限警告值' prop="F_StockLowerLimit" left-icon="question-circle-fill" @clickIcon="clickIcon(label='库存下限警告值', '设置该商品在仓库中的默认库存下限的警告值。' )" :left-icon-style="{'color':'#a0acb7'}"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_StockLowerLimit"  />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='商品图片' prop="F_Image"  :label-width="100 * 1.5">
					<JnpfUploadImg v-model="dataForm.F_Image" sizeUnit='MB' :fileSize='10' pathType="defaultPath"  :sortRule='[]' timeFormat='YYYY' buttonText='点击上传' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='附件' prop="F_AttachmentUrl"  :label-width="100 * 1.5">
					<JnpfUploadFile v-model="dataForm.F_AttachmentUrl" :limit='9' sizeUnit='MB' :fileSize='10' pathType="defaultPath"  :sortRule='[]' timeFormat='YYYY' buttonText='点击上传' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='备注' prop="F_Remark"  :label-width="100 * 1.5">
					<JnpfTextarea v-model="dataForm.F_Remark" placeholder='请输入'  :showCount='false' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item prop="F_Intro"  :label-width="100 * 1.5">
					<JnpfEditor v-model='dataForm.F_Intro' />
				</u-form-item>
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
					F_GoodsNo:undefined,
					F_GoodsName:undefined,
					F_CategoryId:undefined,
					F_Unit:undefined,
					F_SalePrice:undefined,
					F_CostPrice:undefined,
					F_CommissionFixed:undefined,
					F_Source:undefined,
					F_Specification:undefined,
					F_OutsourcePrice:undefined,
					F_SupplierId:undefined,
					F_InspectRule:undefined,
					F_StockUpperLimit:undefined,
					F_StockLowerLimit:undefined,
					F_Image:[],
					F_AttachmentUrl:[],
					F_Remark:undefined,
					F_Intro:undefined,
					F_CreatorTime:undefined,
					F_CreatorUserId:undefined,
                },
                rules: {
					F_GoodsName:[
						{
							required:true,
							message:this.$t('common.inputText')+' '+'商品名称',
							trigger:'blur',
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
				f_CategoryIdProps:{"label": "F_Name","value": "F_Id","children": "children"},
				f_CategoryIdOptions: [],
				f_UnitProps:{"label": "F_Name","value": "F_Id","children": "children"},
				f_UnitOptions: [],
				f_SourceProps:{'label':'fullName','value':'enCode'},
				f_SourceOptions: [],
				f_SupplierIdProps:{'label':'F_SupplierName','value':'F_Id'},
				f_SupplierIdOptions: [],
				unitRatioList:[]
            };
        },
		watch: {
		    'dataForm.F_Unit': {
		      handler(newVal) {
		        this.handleUnitChange();
		      },
		      immediate: true,
		      deep: true
		    },
		    'f_UnitOptions': {
		      handler(newVal) {
		        this.handleUnitChange();
		      },
		      immediate: true,
		      deep: true
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
			this.getf_CategoryIdOptions();
			this.getf_UnitOptions();
			this.getf_SourceOptions();
			this.getf_SupplierIdOptions();
        },
        onReady() {
            this.$refs.dataForm.setRules(this.rules);
        },
        methods: {
		    handleUnitChange() {
		      const unitVal = this.dataForm.F_Unit;
		      const options = this.f_UnitOptions;
		      
		      let ids = [];
		      if (Array.isArray(unitVal)) {
		        ids = unitVal;
		      } else if (typeof unitVal === 'string' && unitVal.trim()) {
		        try {
		          const parsed = JSON.parse(unitVal);
		          ids = Array.isArray(parsed) ? parsed : [];
		        } catch (_) {}
		      }
		      
		      const opts = Array.isArray(options) ? options : [];
		      const path = this.resolveUnitPath(ids, opts);
		      
		      if (path.length === 0) {
		        this.unitRatioList = [];
		        this.dataForm.F_Unit_Ratio = undefined;
		        return;
		      }
		      
		      const existingQty = this.parseUnitRatioJson(this.dataForm.F_Unit_Ratio);
		      const list = path.map((item, i) => ({
		        id: item.id,
		        name: item.name,
		        rate: 1,
		        qty: existingQty.has(item.id) ? existingQty.get(item.id) : (i === 0 ? 1 : 0)
		      }));
		      
		      this.unitRatioList = list;
		      this.syncUnitRatioToForm();
		    },
		    
		    resolveUnitPath(unitIds,options) {
		      if (!Array.isArray(unitIds) || unitIds.length === 0) return [];
		      const flat = this.flattenUnitOptions(options || [], 'F_Name', 'F_Id', 'children');
		      return unitIds.map(id => ({ id, name: flat.get(id)?.name ?? id })).filter(x => x.name);
		    },
		    
		    parseUnitRatioJson(str) {
		      const map = new Map();
		      if (str == null || typeof str !== 'string' || !str.trim()) return map;
		      try {
		        const arr = JSON.parse(str);
		        if (!Array.isArray(arr)) return map;
		        for (const item of arr) {
		          if (item && item.id != null) map.set(String(item.id), Number(item.qty) || 0);
		        }
		      } catch (_) { }
		      return map;
		    },
		    
		    syncUnitRatioToForm() {
		      const list = this.unitRatioList.map(({ id, name, rate, qty }) => ({ id, name, rate, qty }));
		      this.dataForm.F_Unit_Ratio = list.length > 0 ? JSON.stringify(list) : undefined;
		    },
			 flattenUnitOptions(nodes, labelKey = 'F_Name', valueKey = 'F_Id', childrenKey = 'children') {
			  const map = new Map();
			  function walk(list) {
			    if (!Array.isArray(list)) return;
			    for (const n of list) {
			      const id = n[valueKey];
			      const name = n[labelKey] ?? '';
			      if (id) map.set(id, { id, name });
			      if (n[childrenKey]?.length) walk(n[childrenKey]);
			    }
			  }
			  walk(nodes);
			  return map;
			},
			initCollapse() {
				setTimeout(() => {
					this.keyCode = +new Date()
				}, 50)
			},
			onCollapseChange() {
				this.initCollapse()
			},
			getf_CategoryIdOptions(){
				this.f_CategoryIdOptions = []
				let templateJson = []
				let query = {
					paramList: this.getParamList(templateJson, this.dataForm)
				}
				getDataInterfaceRes('2008414575283802112', query).then(res => {
					let data = res.data
					this.f_CategoryIdOptions = Array.isArray(data) ? data : []
				});
			},
			getf_UnitOptions(){
				this.f_UnitOptions = []
				let templateJson = []
				let query = {
					paramList: this.getParamList(templateJson, this.dataForm)
				}
				getDataInterfaceRes('2034507723852353536', query).then(res => {
					let data = res.data
					this.f_UnitOptions = Array.isArray(data) ? data : []
				});
			},
			getf_SourceOptions(){
				getDictionaryDataSelector('2008448951216377856').then(res => {
					this.f_SourceOptions = res.data.list
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
                        url: '/api/example/AGoods/' + this.dataForm.id,
                        method: 'get',
                    }).then(res => {
                        this.dataForm = res.data;
						if(!this.dataForm.F_CategoryId)this.dataForm.F_CategoryId=[];
						if(!this.dataForm.F_Image)this.dataForm.F_Image=[];
						if(!this.dataForm.F_AttachmentUrl)this.dataForm.F_AttachmentUrl=[];
                    })
                }
            },
			submitForm(type) {
                this.$refs.dataForm.validate(valid => {
                    if (!valid) return
                    this.btnLoading = true
                    if (this.dataForm.id) {
                        request({
                            url: '/api/example/AGoods/' + this.dataForm.id,
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
                        }).catch((error)=>{
							this.btnLoading = false
						})
                    } else {
                        request({
                            url: '/api/example/AGoods',
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
	/** 同步 unitRatioList 到 dataForm.F_Unit_Ratio（JSON 字符串） */
	 syncUnitRatioToForm() {
	  const list = this.unitRatioList.map(({ id, name, rate, qty }) => ({ id, name, rate, qty }));
	  this.dataForm.F_Unit_Ratio = list.length > 0 ? JSON.stringify(list) : undefined;
	},
	
	/** 用户修改某一行的数量 */
	 updateUnitRatioQty(index, val) {
	  const list = [...this.unitRatioList];
	  if (index >= 0 && index < list.length) {
	    list[index] = { ...list[index], qty: val ?? 0 };
	    this.unitRatioList = list;
	    syncUnitRatioToForm();
	  }
	}
        }
    };
</script>
<style lang="scss" scoped>
        page{
                background-color: #f0f2f6;
        }
		.unit-ratio-table{
			width: 100%;
			display: flex;
			justify-content: flex-end;
		}
</style>



