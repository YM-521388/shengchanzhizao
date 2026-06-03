﻿﻿<style lang="scss">
	.detail-text-box{
		width: 100%;
	}
</style>
<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" ref="dataForm" :errorType="['toast']" label-position="left" :label-width="100 * 1.5" label-align="right" class="jnpf-form">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='合同' prop="F_ContractId"  :label-width="100 * 1.5" class="popup-select">
				<view class="detail-text-box">
					<view class="jnpf-detail-text" >{{dataForm.F_ContractId}}</view>
					<DisplayList v-if="Object.keys(extraData).length" :extraObj="extraData.F_ContractId" :extraOptions='[]'></DisplayList>
				</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='开票金额(元)' prop="F_Money"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_Money"  detailed disabled/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='开票状态' prop="F_Status"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_Status" detailed  :maskConfig='{
  "filler": "*",
  "maskType": 1,
  "prefixType": 1,
  "prefixLimit": 0,
  "prefixSpecifyChar": "",
  "suffixType": 1,
  "suffixLimit": 0,
  "suffixSpecifyChar": "",
  "ignoreChar": "",
  "useUnrealMask": false,
  "unrealMaskLength": 1
}' />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='申请附件' prop="F_ApplyFiles"  :label-width="100 * 1.5">
					<JnpfUploadFile v-model="dataForm.F_ApplyFiles" :noShowBtn="false" :limit='9' sizeUnit='MB' :fileSize='10'  detailed/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='申请备注' prop="F_ApplyRemark"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_ApplyRemark}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='开票附件' prop="F_InvoiceFiles"  :label-width="100 * 1.5">
					<JnpfUploadFile v-model="dataForm.F_InvoiceFiles" :noShowBtn="false" :limit='9' sizeUnit='MB' :fileSize='10'  detailed/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='开票备注' prop="F_InvoiceRemark"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_InvoiceRemark}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='申请人员' prop="F_ApplyUserId"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_ApplyUserId}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='申请时间' prop="F_ApplyTime"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_ApplyTime}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='开票人员' prop="F_InvoiceUserId"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_InvoiceUserId}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='开票时间' prop="F_InvoiceTime"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_InvoiceTime}}</view>
				</u-form-item>
			</view>
            <view class="jnpf-table" >
				<view class="jnpf-table-item" v-for="(item,i) in dataForm.tableField2cca74" :key="i">
					<view class="jnpf-table-item-title u-flex u-row-between">
						<view class="jnpf-table-item-title-num u-line-1 u-p-r-20 u-p-l-20" @click="clickIcon('操作日志','')">{{'操作日志'}}({{i+1}})
						</view>
                    </view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='标题'prop="F_Title"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableField2cca74[i].F_Title}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='内容'prop="F_Content"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableField2cca74[i].F_Content}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='创建人员'prop="F_CreatorUserId"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableField2cca74[i].F_CreatorUserId}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='创建时间'prop="F_CreatorTime"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableField2cca74[i].F_CreatorTime}}</view>
						</u-form-item>
					</view>
                </view>
            </view>
		</u-form>
		<view class="buttom-actions">
			<CustomButton class="u-flex buttom-btn-left-inner" :btnText="$t('common.cancelText')" btnIcon="icon-ym icon-ym-add-cancel" customIcon />
			<u-button v-if="$permission.hasBtnP('btn_edit',menuIds)" class="buttom-btn" type="primary" @click.stop="handleEdit">{{$t('common.editText','编辑')}}</u-button>
			<u-button class="buttom-btn" :loading="btnLoading" v-if="dataForm.F_Status != '开票完成'"
				@click="handleOpen">开票完成</u-button>
		</view>
		<uni-popup ref="kpPopup" background-color="#fff">
				<view class="popup-content">
					<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
						<u-form :model="kpForm" :rules="kpRules" ref="kpForm" :errorType="['toast']" label-position="left"
							:label-width="100 * 1.5" label-align="right" class="jnpf-form">
							<view class="u-p-l-20 u-p-r-20 form-item-box">
								<u-form-item label='合同' prop="F_ContractId"  :label-width="100 * 1.5">
									<JnpfPopupSelect disabled v-model='kpForm.F_ContractId' propsValue='F_Id' relationField='F_ContractCode' :columnOptions='F_ContractIdOptions' interfaceId='2010889611072638976' placeholder='请选择' :pageSize='20' popupType='dialog' popupTitle='选择数据' popupWidth='1000px' :templateJson='F_ContractIdTemplateJson' vModel="F_ContractId"  type="popup" :extraOptions='[]'/>
								</u-form-item>
							</view>
							<view class="u-p-l-20 u-p-r-20 form-item-box">
								<u-form-item label='开票金额(元)' prop="F_Money"  :label-width="100 * 1.5">
									<JnpfInputNumber disabled v-model="kpForm.F_Money" :step='1.0' />
								</u-form-item>
							</view>
							<view class="u-p-l-20 u-p-r-20 form-item-box">
								<u-form-item label='申请附件' prop="F_ApplyFiles"  :label-width="100 * 1.5">
									<JnpfUploadFile v-model="kpForm.F_ApplyFiles" :limit='9' sizeUnit='MB' :fileSize='10' pathType="defaultPath"  :sortRule='[]' timeFormat='YYYY' buttonText='点击上传' />
								</u-form-item>
							</view>
							<view class="u-p-l-20 u-p-r-20 form-item-box">
								<u-form-item label='申请备注' prop="F_ApplyRemark"  :label-width="100 * 1.5">
									<JnpfTextarea v-model="kpForm.F_ApplyRemark" placeholder='请输入'  :showCount='false' />
								</u-form-item>
							</view>
							<view class="u-p-l-20 u-p-r-20 form-item-box">
								<u-form-item label='开票附件' prop="F_InvoiceFiles"  :label-width="100 * 1.5">
									<JnpfUploadFile v-model="kpForm.F_InvoiceFiles" :limit='9' sizeUnit='MB' :fileSize='10' pathType="defaultPath"  :sortRule='[]' timeFormat='YYYY' buttonText='点击上传' />
								</u-form-item>
							</view>
							<view class="u-p-l-20 u-p-r-20 form-item-box">
								<u-form-item label='开票备注' prop="F_InvoiceRemark"  :label-width="100 * 1.5">
									<JnpfTextarea v-model="kpForm.F_InvoiceRemark" placeholder='请输入'  :showCount='false' />
								</u-form-item>
							</view>
						</u-form>
						<view class="buttom-actions">
							<CustomButton :btnText="$t('common.cancelText')" btnIcon="icon-ym icon-ym-add-cancel"
								customIcon />
							<u-button class="buttom-btn" type="primary" @click="submitForm"
								:loading="btnLoading">{{$t('common.okText','确定')}}</u-button>
						</view>
					</view>
				</view>
			</uni-popup>
		
	</view>
</template>
<script>
    import request from '@/utils/request'
	import { useBaseStore } from '@/store/modules/base'
	import DisplayList from '@/components/displayList'
	import CustomButton from '@/components/CustomButton'
	import { getDataInterfaceDataInfoByIds, getRelationFormDetail} from '@/api/common.js'
    export default {
		components:{ DisplayList,CustomButton },
        data() {
            return {
			keyCode : +new Date(),
			interfaceRes:{
			  F_ContractId: [],
			},
			extraOptions:{
			  F_ContractId: [],
			},
			extraData:{
			  F_ContractId: {},
			},
				menuIds:'',
				setting: {},
                dataForm: {
                    id:'',
					F_ContractId:undefined,
					F_Money:undefined,
					F_Status:"待开票",
					F_ApplyFiles:[],
					F_ApplyRemark:undefined,
					F_InvoiceFiles:[],
					F_InvoiceRemark:undefined,
					F_ApplyUserId:undefined,
					F_ApplyTime:undefined,
					F_InvoiceUserId:undefined,
					F_InvoiceTime:undefined,
					tableField2cca74:[],
                },
				jurisdictionType:'',
				idList:[],
				kpForm: {
				    id:'',
					F_OrderId:undefined,
					F_SupplierId:undefined,
					F_Money:undefined,
					F_PayDate:undefined,
					F_Files:[],
					F_Description:undefined,
					F_CreatorUserId:undefined,
					F_CreatorTime:undefined,
				},
				kpRules: {
						F_Money:[
							{
								required:true,
								message:this.$t('common.inputText')+' '+'付款金额',
								trigger:["blur","change"],
								type:'number'
							},
						],
						F_PayDate:[
							{
								required:true,
								message:this.$t('common.inputText')+' '+'付款日期',
								trigger:'change',
								type:'number'
							},
						],
					},
					btnLoading:false,	
					F_ContractIdTemplateJson: [],
					F_ContractIdOptions: [
					  {
					    "value": "F_ContractCode",
					    "label": "编号"
					  },
					  {
					    "value": "F_PrepayAmount",
					    "label": "预付款"
					  },
					  {
					    "value": "F_SaleTotal",
					    "label": "销售总数"
					  },
					  {
					    "value": "F_ContractAmount",
					    "label": "合同金额"
					  },
					  {
					    "value": "F_IsTaxable",
					    "label": "是否含税"
					  },
					  {
					    "value": "F_AuditStatus",
					    "label": "审核状态"
					  },
					  {
					    "value": "F_Description",
					    "label": "备注"
					  },
					  {
					    "value": "F_CreatorTime",
					    "label": "参加时间"
					  }
					],			
				
            };
        },
		watch: {
        },
        onLoad(option) {
			this.menuIds = option.menuIds
			this.jurisdictionType = option.jurisdictionType
            this.dataForm.id = option.id || ''
			this.idList = option.idList
            uni.setNavigationBarTitle({
                title: this.$t('common.detailText')
            })
            this.initData()
			uni.$on('refresh', () => {
				this.initData()                              
			})
		},
        methods: {
			getParamList(key) {
				let templateJson = this.interfaceRes[key];
				if (!templateJson || !templateJson.length || !this.dataForm) return templateJson;
				for (let i = 0; i < templateJson.length; i++) {
					if (templateJson[i].relationField && templateJson[i].sourceType == 1) {
						templateJson[i].defaultValue = this.dataForm[templateJson[i].relationField + '_id'] || '';
					}
				}
				return templateJson;
			},
			getF_ContractIdExtraInfo() {
				if(!this.dataForm.F_ContractId_id) return;
				const paramList = this.getParamList('F_ContractId');
				const query = {
					ids: [this.dataForm.F_ContractId_id],
					interfaceId: '2010889611072638976 ',
					propsValue:'F_Id' ,
					relationField:'F_ContractCode' ,
					paramList,
				};
				getDataInterfaceDataInfoByIds('2010889611072638976 ', query).then(res => {
					const data = res.data && res.data.length ? res.data[0] : {};
					this.extraData.F_ContractId = data;
				});
			},
			initCollapse() {
				setTimeout(() => {
					this.keyCode = +new Date()
				}, 50)
			},
			onCollapseChange() {
				this.initCollapse()
			},
            initData() {
				const baseStore = useBaseStore()
				baseStore.updateRelationData({})
                if (this.dataForm.id) {
                    request({
                        url: '/api/example/ACtcInvoice/Detail/' + this.dataForm.id,
                        method: 'get',
                    }).then(res => {
                        this.dataForm = res.data;
						if(!this.dataForm.F_ApplyFiles)this.dataForm.F_ApplyFiles=[];
						if(!this.dataForm.F_InvoiceFiles)this.dataForm.F_InvoiceFiles=[];

						this.getF_ContractIdExtraInfo();
						this.initCollapse();
                    })
                }
            },
			clickIcon(label, tipLabel) {
				uni.showModal({
					title: label || '',
					content: tipLabel || '',
					showCancel: false,
				});
			},
			toDetail(id, modelId, propsValue) {
				if (!id) return
				let config = {
					modelId: modelId,
					id: id,
					propsValue: propsValue,
					formTitle: this.$t('common.detailText'),
					noShowBtn: 1
				}
				this.$nextTick(() => {
					const url =
						`/pages/apply/dynamicModel/detail?config=` + this.jnpf.base64.encode(JSON.stringify(config),"UTF-8");
					uni.navigateTo({
						url: url
					})
				})
			},
			handleEdit() {
				let btnType = 'btn_edit';
				if(!btnType) return
				this.jumPage(this.dataForm.id,btnType)
			},
			jumPage(id,btnType){
				if (!id) btnType = 'btn_add'
				let query = id ? id : ''
				uni.navigateTo({
					url: './form?id=' + query + '&jurisdictionType=' + btnType + '&menuIds=' + this.menuIds + '&idList='+this.idList
				})
			},
			handleClose() {
					this.$refs.kpPopup.close()
				},
				handleOpen(){
					console.log(this.dataForm,999);
					this.kpForm.F_ContractId=this.dataForm.F_ContractId_id
					this.kpForm.F_Money=this.dataForm.F_Money
					this.$refs.kpPopup.open('bottom')
					if (this.$refs.kpForm) this.$refs.kpForm.setRules(this.kpRules)
				},
				getf_SupplierIdOptions(){
					this.f_SupplierIdOptions = []
					let templateJson = []
					let query = {
						paramList: this.getParamList(templateJson, this.kpForm)
					}
					getDataInterfaceRes('2008457397298925568', query).then(res => {
						let data = res.data
						this.f_SupplierIdOptions = Array.isArray(data) ? data : []
					});
				},
					// 开票完成提交
				submitForm() {
					this.$refs.kpForm.validate(async valid => {
						if (!valid) return
						this.btnLoading = true
						// let {
						// 	data
						// } = await request({
						// 	url: '/api/example/ACtcInvoice/' + item.id,
						// 	method: 'GET'
						// })
						let userInfo=uni.getStorageSync('userInfo') || {}
						let params = {
							...this.kpForm,
							id:this.dataForm.id,
							F_Status: "开票完成",
							F_InvoiceUserId: userInfo.userId ? userInfo.userId : '',
							 F_ApplyUserId: userInfo.userId ? userInfo.userId : '',
							F_ApplyTime:new Date().getTime(),
							F_InvoiceTime:new Date().getTime(),
						}
						request({
							url: '/api/example/ACtcInvoice/' + this.dataForm.id,
							method: 'PUT',
							data: params,
						}).then(res => {
							uni.showToast({
								title: res.msg,
								complete: () => {
									setTimeout(() => {
										uni.$emit('refresh')
										uni.navigateBack()
										this.btnLoading = false
									}, 1500)
								}
							})
						}).catch(() => {
							this.btnLoading = false
						})
					})
				},
        }
    };
</script>
