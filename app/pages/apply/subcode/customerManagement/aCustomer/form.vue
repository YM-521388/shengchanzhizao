﻿﻿﻿﻿﻿﻿﻿﻿<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" :rules="rules" ref="dataForm" :errorType="['toast']" label-position="left" :label-width="100 * 1.5" label-align="right" class="jnpf-form">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='名称' prop="F_CustomerName"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_CustomerName" placeholder='请输入'  :showCount='false' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='编号' prop="F_CustomerCode"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_CustomerCode" placeholder='请填写，忽略将自动生成'  :showCount='false' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='客户标签' prop="F_CustomerTags"  :label-width="100 * 1.5">
					<JnpfSelect v-model="dataForm.F_CustomerTags" :options="f_CustomerTagsOptions" :props="f_CustomerTagsProps" placeholder='请选择' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='客户共享' prop="F_ShareUsers"  :label-width="100 * 1.5">
					<JnpfUserSelect v-model="dataForm.F_ShareUsers" multiple placeholder='请选择' selectType='all' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='是否关注' prop="F_IsFollow"  :label-width="100 * 1.5">
					<JnpfSwitch v-model="dataForm.F_IsFollow" />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='状态' prop="F_State"  :label-width="100 * 1.5">
					<JnpfSwitch v-model="dataForm.F_State" />
				</u-form-item>
			</view>
			<view class="jnpf-table">
				<view class="jnpf-table-title u-line-1" >{{'客户地址'}}
				</view>
				<view class="jnpf-table-item" v-for="(item,i) in dataForm.tableField6eed25" :key="i">
					<view class="jnpf-table-item-title">
						<view class="jnpf-table-item-title-num">({{i+1}})</view>
						<view class="u-flex">
		        <view class="jnpf-table-copy-btn" @click="copyACtcAddressRow(i)">{{this.$t('common.copyText','复制')}}</view>
		        <view class="jnpf-table-delete-btn" @click="removeACtcAddressRow(i,true)">{{this.$t('common.delText','删除')}}</view>
						</view>
                    </view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='地区'  :label-width="100 * 1.5">
							<JnpfAreaSelect v-model="dataForm.tableField6eed25[i].F_Region" placeholder='请选择' :level='2' />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='地址'  :label-width="100 * 1.5">
							<JnpfInput v-model="dataForm.tableField6eed25[i].F_Address" placeholder='请输入'  :showCount='false' />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='是否默认'  :label-width="100 * 1.5">
							<JnpfSwitch v-model="dataForm.tableField6eed25[i].F_IsDefault"  @change="val => onTableField6eed25IsDefaultChange(val,item, index)" />
						</u-form-item>
					</view>
                </view>
				<view class="jnpf-table-footer-btn" >
					<view class="jnpf-table-btn jnpf-table-primary-btn" @click="addACtcAddressRow()">
						<text class="jnpf-table-btn-icon icon-ym icon-ym-btn-add"/>
						<text class="jnpf-table-btn-text">{{this.$t('common.add1Text','添加')}}</text>
					</view>
				</view>
            </view>
			<view class="jnpf-table">
				<view class="jnpf-table-title u-line-1" >{{'客户联系人'}}
				</view>
				<view class="jnpf-table-item" v-for="(item,i) in dataForm.tableFieldddabab" :key="i">
					<view class="jnpf-table-item-title">
						<view class="jnpf-table-item-title-num">({{i+1}})</view>
						<view class="u-flex">
		        <view class="jnpf-table-copy-btn" @click="copyACtcContactRow(i)">{{this.$t('common.copyText','复制')}}</view>
		        <view class="jnpf-table-delete-btn" @click="removeACtcContactRow(i,true)">{{this.$t('common.delText','删除')}}</view>
						</view>
                    </view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='联系人'  :label-width="100 * 1.5">
							<JnpfInput v-model="dataForm.tableFieldddabab[i].F_ContactName" placeholder='请输入'  :showCount='false' />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='联系人电话'  :label-width="100 * 1.5">
							<JnpfInput v-model="dataForm.tableFieldddabab[i].F_ContactPhone" placeholder='请输入'  :showCount='false' />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='是否默认'  :label-width="100 * 1.5">
							<JnpfSwitch v-model="dataForm.tableFieldddabab[i].F_IsDefault" @change="val => onTableFieldddababIsDefaultChange(val, item, index)"/>
						</u-form-item>
					</view>
<!-- 					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='创建时间'  :label-width="100 * 1.5">
							<JnpfInput v-model="dataForm.tableFieldddabab[i].F_CreatorTime" :placeholder="$t('component.jnpf.common.autoGenerate')" disabled/>
						</u-form-item>
					</view> -->
                </view>
				<view class="jnpf-table-footer-btn" >
					<view class="jnpf-table-btn jnpf-table-primary-btn" @click="addACtcContactRow()">
						<text class="jnpf-table-btn-icon icon-ym icon-ym-btn-add"/>
						<text class="jnpf-table-btn-text">{{this.$t('common.add1Text','添加')}}</text>
					</view>
				</view>
            </view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='备注' prop="F_Description"  :label-width="100 * 1.5">
					<JnpfTextarea v-model="dataForm.F_Description" placeholder='请输入'  :showCount='false' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='附件' prop="F_Files"  :label-width="100 * 1.5">
					<JnpfUploadFile v-model="dataForm.F_Files" :limit='9' sizeUnit='MB' :fileSize='10' pathType="defaultPath"  :sortRule='[]' timeFormat='YYYY' buttonText='点击上传' />
				</u-form-item>
			</view>
<!-- 			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='领用人员' prop="F_RequisitionUserId"  :label-width="100 * 1.5">
					<JnpfUserSelect v-model="dataForm.F_RequisitionUserId" placeholder='请选择' selectType='all' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='领用时间' prop="F_RequisitionTime"  :label-width="100 * 1.5">
					<JnpfDatePicker v-model="dataForm.F_RequisitionTime" type='date' format='yyyy-MM-dd' placeholder='请选择' />
				</u-form-item>
			</view> -->
<!-- 			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='创建人员' prop="F_CreatorUserId"  :label-width="100 * 1.5">
					<JnpfOpenData v-model="dataForm.F_CreatorUserId" type='currUser' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='创建时间' prop="F_CreatorTime"  :label-width="100 * 1.5">
					<JnpfOpenData v-model="dataForm.F_CreatorTime" type='currTime' />
				</u-form-item>
			</view> -->
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
					F_CustomerName:undefined,
					F_CustomerCode:undefined,
					F_QRCode:undefined,
					F_IsPublic:undefined,
					F_CustomerTags:undefined,
					F_ShareUsers:[],
					F_IsFollow:0,
					F_State:1,
					tableField6eed25:[],
					tableFieldddabab:[],
					F_Description:undefined,
					F_Files:[],
					F_RequisitionUserId:undefined,
					F_RequisitionTime:undefined,
					F_CreatorUserId:undefined,
					F_CreatorTime:undefined,
                },
                rules: {
                },
				ruleList: {
				},
				regList: {
					tableFieldddabab: {
						F_ContactPhone: [{"pattern":/^1[3456789]\d{9}$|^0\d{2,3}-?\d{7,8}$/, "message":"请输入正确的联系方式"}],
					},
				},
				index: 0,
				idList: [],
				dataValue: {},
				jurisdictionType:'',
				addType: 0,
				f_CustomerTagsProps:{'label':'fullName','value':'id'},
				f_CustomerTagsOptions: [],
				tableField6eed25innerActiveKey:[],
				tableFieldddababinnerActiveKey:[]
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
			this.getf_CustomerTagsOptions();
            uni.$on('linkPageConfirm', (subVal, Vmodel) => {
				let item = {
						F_Region:[],
						F_Address:undefined,
						F_IsDefault:0,
						F_CreatorTime:undefined,
					}
			   this.initCollapse();
            });
            uni.$on('linkPageConfirm', (subVal, Vmodel) => {
				let item = {
						F_ContactName:undefined,
						F_ContactPhone:undefined,
						F_IsDefault:1,
						F_CreatorTime:undefined,
					}
               if ('ACtcContact' === Vmodel) subVal.forEach(t => this.dataForm.tableFieldddabab.push({...item,...t}));
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
			getf_CustomerTagsOptions(){
				getDictionaryDataSelector('2009094067723571200').then(res => {
					this.f_CustomerTagsOptions = res.data.list
				});
			},
			selfInit(){
				this.addACtcAddress();
				this.addACtcContact();
			},
			resetForm() {
				this.dataForm.tableField6eed25 = [];
				this.dataForm.tableFieldddabab = [];
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
                        url: '/api/example/ACustomer/' + this.dataForm.id,
                        method: 'get',
                    }).then(res => {
                        this.dataForm = res.data;
						this.dataForm.tableField6eed25=res.data.tableField6eed25.map(item=>{
							return{
								...item,
								jnpfId: buildUUID(),
							}
						})
						this.dataForm.tableFieldddabab=res.data.tableFieldddabab.map(item=>{
							return{
								...item,
								jnpfId: buildUUID(),
							}
						})
						if(!this.dataForm.F_Files)this.dataForm.F_Files=[];
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
                            url: '/api/example/ACustomer/' + this.dataForm.id,
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
                            url: '/api/example/ACustomer',
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
			addACtcAddressRow() {
				const isFirst = !(this.dataForm.tableField6eed25 && this.dataForm.tableField6eed25.length);
				let item = {
						jnpfId: buildUUID(),
						F_Region:[],
						F_Address:undefined,
						F_IsDefault:isFirst ? 1 : 0,
						F_CreatorTime:undefined,
					}
				this.dataForm.tableField6eed25.push(item)
			// 如果刚加入后表里只有这一条，确保它为默认
			    if (this.dataForm.tableField6eed25.length === 1) this.dataForm.tableField6eed25[0].F_IsDefault = 1;
			    this.tableField6eed25innerActiveKey.push(item.jnpfId);
				this.initCollapse()
			},
			removeACtcAddressRow(i,showConfirm=false) {
				const handleRemove = () => {
					this.dataForm.tableField6eed25.splice(i, 1)
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
			copyACtcAddressRow(i) {
				let item = JSON.parse(JSON.stringify(this.dataForm.tableField6eed25[i]));
				item.id = '';
				item.length && item.map(o => delete o.rowData);
				item.F_CreatorTime = undefined;
				// 复制时不自动继承“是否默认”为避免出现多个默认项，复制后的记录默认值设为0
				item.F_IsDefault = 0;
				item.jnpfId= buildUUID()
				this.dataForm.tableField6eed25.push(item);
				this.tableField6eed25innerActiveKey.push(item.jnpfId);
			},
			addACtcContactRow() {
				const isFirst = !(this.dataForm.tableFieldddabab && this.dataForm.tableFieldddabab.length);
				
				let item = {
						jnpfId: buildUUID(),
						F_ContactName:undefined,
						F_ContactPhone:undefined,
						F_IsDefault:isFirst ? 1 : 0,
						F_CreatorTime:undefined,
					}
				this.dataForm.tableFieldddabab.push(item)
				if (this.dataForm.tableFieldddabab.length === 1) this.dataForm.tableFieldddabab[0].F_IsDefault = 1;
				this.tableFieldddababinnerActiveKey.push(item.jnpfId);
				this.initCollapse()
			},
			removeACtcContactRow(i,showConfirm=false) {
				const handleRemove = () => {
					this.dataForm.tableFieldddabab.splice(i, 1)
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
			copyACtcContactRow(i) {
				let item = JSON.parse(JSON.stringify(this.dataForm.tableFieldddabab[i]));
				item.id = '';
				item.length && item.map(o => delete o.rowData);
				item.F_CreatorTime = undefined;
				 // 复制项不继承“是否默认”以避免同时存在多个默认
				item.F_IsDefault = 0;
				item.jnpfId= buildUUID()
				this.dataForm.tableFieldddabab.push(item);
				this.tableFieldddababinnerActiveKey.push(item.jnpfId);

			},
			  // 保证子表“客户地址”只有一条默认记录
			   onTableField6eed25IsDefaultChange(val, record, index) {
			    if (val) {
			      (this.dataForm.tableField6eed25 || []).forEach((r) => {
			        if (r && r.jnpfId !== record.jnpfId) r.F_IsDefault = 0;
			      });
			      record.F_IsDefault = 1;
			    } else {
			      record.F_IsDefault = 0;
			    }
			  },
			onTableFieldddababIsDefaultChange(val, record, index){
			  if (val) {
			      (this.dataForm.tableFieldddabab || []).forEach((r) => {
			        if (r && r.jnpfId !== record.jnpfId) r.F_IsDefault = 0;
			      });
			      record.F_IsDefault = 1;
			    } else {
			      record.F_IsDefault = 0;
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



