﻿<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" ref="dataForm" :errorType="['toast']" label-position="left" :label-width="100 * 1.5" label-align="right" class="jnpf-form">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='外协工单号' prop="F_OutsourceNo"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_OutsourceNo" detailed  :maskConfig='{
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
				<u-form-item label='商品' prop="F_GoodsId"  :label-width="100 * 1.5" class="popup-select">
				<view class="detail-text-box">
					<view class="jnpf-detail-text" >{{dataForm.F_GoodsId}}</view>
					<DisplayList v-if="Object.keys(extraData).length" :extraObj="extraData.F_GoodsId" :extraOptions='[]'></DisplayList>
				</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='计划数' prop="F_PlanNum"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_PlanNum" :precision='0'  detailed disabled/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='BOM' prop="F_BOMId"  :label-width="100 * 1.5" class="popup-select">
					<view class="detail-text-box">
										<view class="jnpf-detail-text" >{{dataForm.F_BOMId}}</view>
										<DisplayList v-if="Object.keys(extraData).length" :extraObj="extraData.F_BOMId" :extraOptions='[]'></DisplayList>
									</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='供应商' prop="F_SupplierId"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_SupplierId}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='供应商联系人' prop="F_ContactPerson"  :label-width="100 * 1.5" class="popup-select">
				<view class="detail-text-box">
					<view class="jnpf-detail-text" >{{dataForm.F_ContactPerson}}</view>
					<DisplayList v-if="Object.keys(extraData).length" :extraObj="extraData.F_ContactPerson" :extraOptions='[]'></DisplayList>
				</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='供应商联系人电话' prop="F_ContactPhone"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_ContactPhone" detailed  :maskConfig='{
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
				<u-form-item label='供应商地区' prop="F_Region"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_Region}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='供应商详细地址' prop="F_DetailAddress"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_DetailAddress}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='外协单价' prop="F_Price"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_Price"  detailed disabled/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='交货日期' prop="F_DeliveryDate"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_DeliveryDate}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='优先级' prop="F_Priority"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_Priority}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='计划开始' prop="F_PlanStartDate"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_PlanStartDate}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='计划结束' prop="F_PlanEndDate"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_PlanEndDate}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='图纸' prop="F_Files"  :label-width="100 * 1.5">
					<JnpfUploadFile v-model="dataForm.F_Files" :noShowBtn="false" :limit='9' sizeUnit='MB' :fileSize='10'  detailed/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='状态' prop="F_State"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_State" detailed  :maskConfig='{
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
				<u-form-item label='描述' prop="F_Description"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_Description}}</view>
				</u-form-item>
			</view>
            <view class="jnpf-table" >
				<view class="jnpf-table-item" v-for="(item,i) in dataForm.tableField7a8044" :key="i">
					<view class="jnpf-table-item-title u-flex u-row-between">
						<view class="jnpf-table-item-title-num u-line-1 u-p-r-20 u-p-l-20" @click="clickIcon('用料清单','')">{{'用料清单'}}({{i+1}})
						</view>
                    </view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='商品名'prop="F_GoodsName"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableField7a8044[i].F_GoodsName}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='商品编码'prop="F_GoodsNo"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableField7a8044[i].F_GoodsNo}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='商品规格'prop="F_Specification"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableField7a8044[i].F_Specification}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='单位用量'prop="F_NumUnit"  :label-width="100 * 1.5">
							<JnpfInput v-model="dataForm.tableField7a8044[i].F_NumUnit" disabled/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='单位'prop="F_UnitTwo"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableField7a8044[i].F_UnitTwo}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='备注'prop="F_Description"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableField7a8044[i].F_Description}}</view>
						</u-form-item>
					</view>
                </view>
            </view>
		</u-form>
		<view class="buttom-actions">
			<CustomButton class="u-flex buttom-btn-left-inner" :btnText="$t('common.cancelText')" btnIcon="icon-ym icon-ym-add-cancel" customIcon />
			<u-button  v-if="$permission.hasBtnP('btn_edit',menuIds)" class="buttom-btn" type="primary" @click.stop="handleEdit">{{$t('common.editText','编辑')}}</u-button>
			<!-- <u-button class="buttom-btn" type="primary" :plain="true" @click.stop="handleComplete" >{{dataForm.F_State==='生产中'?'完成':"退回"}}</u-button> -->
			<u-button class="buttom-btn" type="primary" :plain="true" @click.stop="moreOpen">更多</u-button>
		</view>
		<!-- 更多弹窗 -->
		<uni-popup ref="morePopup" background-color="#fff" >
			<view class="popup-content">
				<view class="classify-scroll-view u-flex">
					<template v-for="(item,index) in moreList">
						<u-button v-if="item.ifShow&&$permission.hasBtnP(item.auth,menuIds)" :key="index" @click="classifyItem(item,index)"
							class="classify-item u-font-28 u-line-1" :class="btnActive===index?'btnActive':''" >
							{{item.label}}
						</u-button>
					</template>
					
				</view>
			</view>
		</uni-popup>
		<!-- 暂停/取消 -->
		<uni-popup ref="ztqxPopup" background-color="#fff">
			<view class="popup-content">
				<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
					<u-form :model="zqForm" :rules="zqRules" ref="zqForm" :errorType="['toast']" label-position="left"
						:label-width="100 * 1.5" label-align="right" class="jnpf-form">
						<view class="u-p-l-20 u-p-r-20 form-item-box">
							<u-form-item :label='reasonLabel' prop="F_Reason" required :label-width="100 * 1.5">
								<JnpfTextarea v-model="zqForm.F_Reason" :maxlength='500' placeholder='请输入'
									:showCount='false' />
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
        computed:{
        	moreList(){
				return this.getMoreList()
        	}
        },
		data() {
            return {
			keyCode : +new Date(),
			interfaceRes:{
			  F_GoodsId: [],
			  F_BOMId: [],
			  F_ContactPerson: [
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
			},
			extraOptions:{
			  F_GoodsId: [],
			  F_BOMId: [],
			  F_ContactPerson: [],
			},
			extraData:{
			  F_GoodsId: {},
			  F_BOMId: {},
			  F_ContactPerson: {},
			},
				menuIds:'',
				setting: {},
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
					F_Priority:undefined,
					F_PlanStartDate:undefined,
					F_PlanEndDate:undefined,
					F_Files:[],
					F_State:undefined,
					F_Description:undefined,
					F_CreatorUserId:undefined,
					F_CreatorTime:undefined,
					tableField7a8044:[],
                },
				jurisdictionType:'',
				idList:[],
				F_BOMIdOptions: [
			  {
			    "value": "F_BomCode",
			    "label": "BOM编号"
			  },
			  {
			    "value": "F_BomName",
			    "label": "BOM名称"
			  }
			],
			btnActive: null,
			zqForm: {
				"F_Reason": undefined,
			},
			zqRules: {
				F_Reason: [{
					required: true,
					message: this.$t('common.inputText'),
					trigger: 'blur',
				}],
			},
			reasonLabel:'',
			btnLoading:false,
			routeData:{},
			enCode:'',
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
		onReady() {
			//如果需要兼容微信小程序，并且校验规则中含有方法等，只能通过setRules方法设置规则。
			if (this.$refs.zqForm) this.$refs.zqForm.setRules(this.zqRules)
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
			getF_GoodsIdExtraInfo() {
				if(!this.dataForm.F_GoodsId_id) return;
				const paramList = this.getParamList('F_GoodsId');
				const query = {
					ids: [this.dataForm.F_GoodsId_id],
					interfaceId: '2008721559174385664 ',
					propsValue:'F_Id' ,
					relationField:'F_GoodsName' ,
					paramList,
				};
				getDataInterfaceDataInfoByIds('2008721559174385664 ', query).then(res => {
					const data = res.data && res.data.length ? res.data[0] : {};
					this.extraData.F_GoodsId = data;
				});
			},
			getF_BOMIdExtraInfo() {
				if(!this.dataForm.F_BOMId_id) return;
				const paramList = this.getParamList('F_BOMId');
				const query = {
					ids: [this.dataForm.F_BOMId_id],
					interfaceId: '2008713719516893184 ',
					propsValue:'F_Id' ,
					relationField:'F_ParentId' ,
					paramList,
				};
				getDataInterfaceDataInfoByIds('2008713719516893184 ', query).then(res => {
					const data = res.data && res.data.length ? res.data[0] : {};
					this.extraData.F_BOMId = data;
				});
			},
			getF_ContactPersonExtraInfo() {
				if(!this.dataForm.F_ContactPerson_id) return;
				const paramList = this.getParamList('F_ContactPerson');
				const query = {
					ids: [this.dataForm.F_ContactPerson_id],
					interfaceId: '2009459664370143232 ',
					propsValue:'F_Id' ,
					relationField:'F_ContactName' ,
					paramList,
				};
				getDataInterfaceDataInfoByIds('2009459664370143232 ', query).then(res => {
					const data = res.data && res.data.length ? res.data[0] : {};
					this.extraData.F_ContactPerson = data;
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
                        url: '/api/example/AProdOutsource/Detail/' + this.dataForm.id,
                        method: 'get',
                    }).then(res => {
                        this.dataForm = res.data;
						if(!this.dataForm.F_Region)this.dataForm.F_Region=[];
						if(!this.dataForm.F_Files)this.dataForm.F_Files=[];

						this.getF_GoodsIdExtraInfo();
						this.getF_BOMIdExtraInfo();
						this.getF_ContactPersonExtraInfo();
						this.initCollapse();
						this.getGoodsList()
						request({
						    url: '/api/example/AProdOutsource/' + this.dataForm.id,
						    method: 'get',
						}).then(res => {
						    this.dataForm.F_StateId = res.data.F_State;
						})
                    })
                }
            },
			getGoodsList(){
				request({
				    url: '/api/example/AProdOutsource/GoodsList',
				    method: 'post',
					data:{
						    "currentPage": 1,
						    "pageSize": 10000,
						    "F_OutsourceId": this.dataForm.id
						}
				}).then(res => {
				    this.dataForm.tableField7a8044=res.data.list
				})
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
			getMoreList(){
				return [{
					label: '开始',
					ifShow: this.dataForm.F_StateId == '0',
					status:'1',
					method:"handleState",
					auth: 'btn_start',
					},
					{
					label: '完成',
					ifShow: this.dataForm.F_StateId == '1',
					status:'2',
					method:"handleState",
					auth: 'btn_complete',
				},{
					label: '撤回',
					ifShow: this.dataForm.F_StateId == '1' || this.dataForm.F_StateId == '2',
					status:'6',
					method:"handleState",
					auth: 'btn_return',
				},{
					label: '暂停',
					ifShow: this.dataForm.F_StateId == '1',
					status:'3',
					method:"handleState",
					auth: 'btn_pause',
				},{
					label: '继续',
					ifShow: this.dataForm.F_StateId == '3',
					status:'5',
					method:"handleState",
					auth: 'btn_continue',
				},{
					label: '取消',
					ifShow: this.dataForm.F_StateId != '4',
					status:'4',
					method:"handleState",
					auth: 'btn_cancel',
				},]
			},
			moreOpen() {
				this.$refs.morePopup.open('bottom')
			},
			moreClose() {
				this.$refs.morePopup.close()
			},
			classifyItem(item, index) {
				this.btnActive = index
				this[item.method](this.dataForm.id,item.status)
			},
			// 打开暂停取消弹窗
			ztqxOpen() {
				this.$refs.ztqxPopup.open('bottom')
				if (this.$refs.zqForm) this.$refs.zqForm.setRules(this.zqRules)
			},
			  //修改状态
			handleState(id, state) {
				if (state === '3' || state === '4') {
					this.reasonLabel=state === '3'?'暂停原因':'取消原因'
					this.enCode=state
					this.ztqxOpen()
				}else {
					uni.showModal({
						title: '提示',
						content: '你确定修改状态？',
						success: (res) => {
							if (res.confirm) {
								request({
									url: '/api/example/AProdOutsource/Check/' +id + "/" +state,
									method: 'get',
								}).then(res => {
									this.$u.toast(res.msg)
									uni.$emit('refresh')
									uni.navigateBack()
								})
							}
						}
					})
				}
			},
			// 提交
			submitForm() {
				this.$refs.zqForm.validate(valid => {
					if (!valid) return
					if (!this.zqForm.F_Reason) {
						uni.showToast({
							title: '请填写'+this.reasonLabel,
							icon:'none'
						})
						return
					}
					this.btnLoading = true
					request({
						url: '/api/example/AProdOutsource/ReasonUpdate/' + this.dataForm.id + "/" + this
							.enCode,
						method: 'put',
						data: {
							id: this.dataForm.id,
							F_Reason: this.zqForm.F_Reason,
							tableField0bb944: []
						},
					}).then(res => {
						uni.showToast({
							title: res.msg,
							complete: () => {
								setTimeout(() => {
									uni.$emit('refresh')
									uni.navigateBack()
									this.btnLoading = false
									this.enCode=''
								}, 1500)
							}
						})
					}).catch(() => {
						this.btnLoading = false
					})
				})
			}
        }
    };
</script>
<style lang="scss" scoped>
	.detail-text-box {
		width: 100%;
	}

	.popup-content {
		padding: 20rpx;
		min-height: 300rpx;
		max-height: 600rpx;
		overflow-y: auto;

		.classify-scroll-view {
			padding-bottom: 70rpx;
			padding: 0 20rpx 64rpx;
			justify-content: flex-start;
			flex-wrap: wrap;

			.classify-item {
				width: 22%;
				text-align: center;
				margin: 10rpx;
				background-color: #EEF0F4;
				border-radius: 8rpx;
				background: rgba(238, 240, 244, 0.39);
				color: #666666;
				line-height: 58rpx;
				padding: 0 20rpx;
			}

			.btnActive {
				color: #00A578;
			}
		}
	}
</style>