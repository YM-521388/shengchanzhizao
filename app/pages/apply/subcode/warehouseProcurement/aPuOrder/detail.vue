﻿﻿﻿﻿﻿﻿﻿<style lang="scss">
	.detail-text-box{
		width: 100%;
	}
</style>
<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" ref="dataForm" :errorType="['toast']" label-position="left" :label-width="100 * 1.5" label-align="right" class="jnpf-form">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='采购单号' prop="F_OrderNo"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_OrderNo" detailed  :maskConfig='{
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
				<u-form-item label='供应商' prop="F_SupplierId"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_SupplierId}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='商品金额' prop="F_Money"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_Money"  detailed disabled/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='采购数量' prop="F_PurchaseNum"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_PurchaseNum"  detailed disabled/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='采购计划' prop="F_ProdPlanId"  :label-width="100 * 1.5" class="popup-select">
				<view class="detail-text-box">
					<view class="jnpf-detail-text" >{{dataForm.F_ProdPlanId}}</view>
					<DisplayList v-if="Object.keys(extraData).length" :extraObj="extraData.F_ProdPlanId" :extraOptions='[]'></DisplayList>
				</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='采购人' prop="F_UserId"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_UserId}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='交货日期' prop="F_DeliveryDate"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_DeliveryDate}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='备注' prop="F_Description"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_Description}}</view>
				</u-form-item>
			</view>
            <view class="jnpf-table" >
				<view class="jnpf-table-item" v-for="(item,i) in dataForm.tableFieldf01abd" :key="i">
					<view class="jnpf-table-item-title u-flex u-row-between">
						<view class="jnpf-table-item-title-num u-line-1 u-p-r-20 u-p-l-20" @click="clickIcon('采购单商品','')">{{'采购单商品'}}({{i+1}})
						</view>
                    </view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='商品'prop="F_CustomerId"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableFieldf01abd[i].F_CustomerId}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='供应商'prop="F_SupplierId"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableFieldf01abd[i].F_SupplierId}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='数量'prop="F_NumInfo"  :label-width="100 * 1.5">
							<JnpfInput v-model="dataForm.tableFieldf01abd[i].F_NumInfo" disabled/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='单价'prop="F_Price"  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableFieldf01abd[i].F_Price"  detailed disabled/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='折扣'prop="F_Discount"  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableFieldf01abd[i].F_Discount"  detailed disabled/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='优惠金额'prop="F_DiscountMoney"  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableFieldf01abd[i].F_DiscountMoney"  detailed disabled/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='备注'prop="F_Description"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableFieldf01abd[i].F_Description}}</view>
						</u-form-item>
					</view>
                </view>
            </view>
            <view class="jnpf-table" >
				<view class="jnpf-table-item" v-for="(item,i) in dataForm.tableField8b2a57" :key="i">
					<view class="jnpf-table-item-title u-flex u-row-between">
						<view class="jnpf-table-item-title-num u-line-1 u-p-r-20 u-p-l-20" @click="clickIcon('采购单日志','')">{{'采购单日志'}}({{i+1}})
						</view>
                    </view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='标题'prop="F_Title"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableField8b2a57[i].F_Title}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='内容'prop="F_Content"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableField8b2a57[i].F_Content}}</view>
						</u-form-item>
					</view>
                </view>
            </view>
		</u-form>
		<view class="buttom-actions">
			<CustomButton class="u-flex buttom-btn-left-inner" :btnText="$t('common.cancelText')" btnIcon="icon-ym icon-ym-add-cancel" customIcon />
			<u-button v-if="$permission.hasBtnP('btn_edit',menuIds)" class="buttom-btn" type="primary" @click.stop="handleEdit">{{$t('common.editText','编辑')}}</u-button>
			<u-button v-if="$permission.hasBtnP('btn_edit',menuIds)" class="buttom-btn"  @click.stop="handleOpen">付款</u-button>
		</view>
		
		<uni-popup ref="fkPopup" background-color="#fff">
			<view class="popup-content">
				<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
					<u-form :model="fkForm" :rules="fkRules" ref="fkForm" :errorType="['toast']" label-position="left"
						:label-width="100 * 1.5" label-align="right" class="jnpf-form">
						<view class="u-p-l-20 u-p-r-20 form-item-box">
							<u-form-item label='供应商' prop="F_SupplierId"  :label-width="100 * 1.5">
								<JnpfSelect v-model="fkForm.F_SupplierId" :options="f_SupplierIdOptions" :props="f_SupplierIdProps" placeholder='请选择' />
							</u-form-item>
						</view>
						<view class="u-p-l-20 u-p-r-20 form-item-box">
							<u-form-item label='付款金额' prop="F_Money" required  :label-width="100 * 1.5">
								<JnpfInputNumber v-model="fkForm.F_Money" :step='1.0'  />
							</u-form-item>
						</view>
						<view class="u-p-l-20 u-p-r-20 form-item-box">
							<u-form-item label='付款日期' prop="F_PayDate" required  :label-width="100 * 1.5">
								<JnpfDatePicker v-model="fkForm.F_PayDate" type='date' format='yyyy-MM-dd' placeholder='请选择' />
							</u-form-item>
						</view>
						<view class="u-p-l-20 u-p-r-20 form-item-box">
							<u-form-item label='附件' prop="F_Files"  :label-width="100 * 1.5">
								<JnpfUploadFile v-model="fkForm.F_Files" :limit='9' sizeUnit='MB' :fileSize='10' pathType="defaultPath"  :sortRule='[]' timeFormat='YYYY' buttonText='点击上传' />
							</u-form-item>
						</view>
						<view class="u-p-l-20 u-p-r-20 form-item-box">
							<u-form-item label='备注' prop="F_Description"  :label-width="100 * 1.5">
								<JnpfTextarea v-model="fkForm.F_Description" placeholder='请输入'  :showCount='false' />
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
	import { getDataInterfaceDataInfoByIds, getRelationFormDetail,getDataInterfaceRes} from '@/api/common.js'
    export default {
		components:{ DisplayList,CustomButton },
        data() {
            return {
			keyCode : +new Date(),
			interfaceRes:{
			  F_ProdPlanId: [],
			},
			extraOptions:{
			  F_ProdPlanId: [],
			},
			extraData:{
			  F_ProdPlanId: {},
			},
				menuIds:'',
				setting: {},
                dataForm: {
                    id:'',
					F_OrderNo:undefined,
					F_SupplierId:undefined,
					F_Money:0,
					F_PurchaseNum:0,
					F_ProdPlanId:undefined,
					F_UserId:undefined,
					F_DeliveryDate:undefined,
					F_Description:undefined,
					F_CreatorUserId:undefined,
					F_CreatorTime:undefined,
					tableFieldf01abd:[],
					tableField8b2a57:[],
                },
				jurisdictionType:'',
				
				fkForm: {
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
				fkRules: {
					F_Money:[
						{
							required:true,
							message:this.$t('common.inputText')+' '+'付款金额',
							trigger:[
  "blur",
  "change"
],
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
				f_SupplierIdProps:{'label':'F_SupplierName','value':'F_Id'},
				f_SupplierIdOptions: [],
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
			this.fkForm.F_OrderId =this.dataForm.id
            this.initData()
			this.getf_SupplierIdOptions();
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
			getF_ProdPlanIdExtraInfo() {
				if(!this.dataForm.F_ProdPlanId_id) return;
				const paramList = this.getParamList('F_ProdPlanId');
				const query = {
					ids: [this.dataForm.F_ProdPlanId_id],
					interfaceId: '2011711502578487296 ',
					propsValue:'F_Id' ,
					relationField:'F_PlanNo' ,
					paramList,
				};
				getDataInterfaceDataInfoByIds('2011711502578487296 ', query).then(res => {
					const data = res.data && res.data.length ? res.data[0] : {};
					this.extraData.F_ProdPlanId = data;
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
                        url: '/api/example/APuOrder/Detail/' + this.dataForm.id,
                        method: 'get',
                    }).then(res => {
                        this.dataForm = res.data;

						this.getF_ProdPlanIdExtraInfo();
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
				this.$refs.fkPopup.close()
			},
			handleOpen(){
				this.$refs.fkPopup.open('bottom')
				if (this.$refs.fkForm) this.$refs.fkForm.setRules(this.fkRules)
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
			// 提交
			submitForm() {
				this.$refs.fkForm.validate(valid => {
					if (!valid) return
					if (!this.fkForm.F_Money) {
						uni.showToast({
							title: '请填写付款金额',
							icon: 'none'
						})
						return
					}
					if (!this.fkForm.F_PayDate) {
						uni.showToast({
							title: '请填写付款日期',
							icon: 'none'
						})
						return
					}
					this.btnLoading = true
					request({
						url: '/api/example/APuOrderPay',
						method: 'post',
						data: this.fkForm,
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
