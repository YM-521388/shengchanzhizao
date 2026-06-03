﻿﻿﻿﻿<style lang="scss">
	.detail-text-box{
		width: 100%;
	}
</style>
<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" ref="dataForm" :errorType="['toast']" label-position="left" :label-width="100 * 1.5" label-align="right" class="jnpf-form">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='加工单' prop="F_ProcessingId"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_ProcessingId}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='工序' prop="F_ProcessName"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_ProcessName}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='工序编号' prop="F_ProcessCode"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_ProcessCode}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='是否需要质检' prop="F_ProcessCode"  :label-width="100 * 2">
					<view class="jnpf-detail-text">{{dataForm.F_IsQc}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='车间' prop="F_ProcessCode"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_WorkshopId}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='产线' prop="F_ProcessCode"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_Line}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='报工单位' prop="F_ProcessCode"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_ReportUnit}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='单位关系' prop="F_ProcessCode"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_UnitRatio}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='计价方式' prop="F_ProcessCode"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_PriceType}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='工资单价' prop="F_ProcessCode"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_WagePrice}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='标准工时' prop="F_ProcessCode"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_StdHour}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='机台' prop="F_ProcessCode"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_MachineId}}</view>
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
<!-- 			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='任务类型' prop="F_ProcessCode"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_TaskType}}</view>
				</u-form-item>
			</view> -->
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='生产人员' prop="F_ProcessCode"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_ProdUserIds}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='生产派工' prop="F_ProdDispatch"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_ProdDispatch}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='质检人员' prop="F_QcDispatch"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_QcUserIds}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='质检派工' prop="F_QcDispatch"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_QcDispatch}}</view>
				</u-form-item>
			</view>
<!-- 			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='不良品项' prop="F_QcDispatch"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_DefectIds}}</view>
				</u-form-item>
			</view> -->
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='生产数量' prop="F_ProdQty"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_ProdQty" :precision='0'  detailed disabled/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='任务状态' prop="F_QcDispatch"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_TaskStatus}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='排序' prop="F_SortCode"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_SortCode" :precision='0'  detailed disabled/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='备注' prop="F_Description"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_Description}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='任务状态' prop="F_TaskStatus" v-if='false'  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_TaskStatus}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='任务类型' prop="F_TaskType" v-if='false'  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_TaskType}}</view>
				</u-form-item>
			</view>
<!--            <view class="jnpf-table" >
				<view class="jnpf-table-item" v-for="(item,i) in dataForm.tableField0bb944" :key="i">
					<view class="jnpf-table-item-title u-flex u-row-between">
						<view class="jnpf-table-item-title-num u-line-1 u-p-r-20 u-p-l-20" @click="clickIcon('物料清单','')">{{'物料清单'}}({{i+1}})
						</view>
                    </view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='商品'prop="F_GoodsId"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableField0bb944[i].F_GoodsId}}</view>
						</u-form-item>
					</view>
					
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='商品编号'prop="F_Description"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableField0bb944[i].F_GoodsNo}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='商品规格'prop="F_Description"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableField0bb944[i].F_Specification}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='数量'prop="F_Num"  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableField0bb944[i].F_Num" :precision='0'  detailed disabled/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='单位'prop="F_Num"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableField0bb944[i].F_UnitTwo}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='备注'prop="F_Description"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableField0bb944[i].F_Description}}</view>
						</u-form-item>
					</view>
                </view>
            </view> -->
		</u-form>
		<view class="buttom-actions">
			<CustomButton class="u-flex buttom-btn-left-inner" :btnText="$t('common.cancelText')" btnIcon="icon-ym icon-ym-add-cancel" customIcon />
			<u-button  v-if="$permission.hasBtnP('btn_edit',menuIds)"  class="buttom-btn" type="primary" @click.stop="handleEdit">{{$t('common.editText','编辑')}}</u-button>
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
		<!-- 暂停原因 -->
		<uni-popup ref="ztPopup" background-color="#fff">
			<view class="popup-content">
				<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
					<u-form :model="ztForm" :rules="ztRules" ref="ztForm" :errorType="['toast']" label-position="left"
						:label-width="100 * 1.5" label-align="right" class="jnpf-form">
						<view class="u-p-l-20 u-p-r-20 form-item-box">
							<u-form-item label='暂停原因' prop="F_Reason" required :label-width="100 * 1.5">
								<JnpfTextarea v-model="ztForm.F_Reason" :maxlength='500' placeholder='请输入'
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
        data() {
            return {
			keyCode : +new Date(),
			interfaceRes:{
			},
			extraOptions:{
			},
			extraData:{
			},
				menuIds:'',
				setting: {},
                dataForm: {
                    id:'',
					F_ProcessingId:undefined,
					F_ProcessId:undefined,
					F_ProcessCode: undefined,
					F_PlanStartDate:undefined,
					F_PlanEndDate:undefined,
					F_ProdDispatch:undefined,
					F_QcDispatch:undefined,
					F_ProdQty:undefined,
					F_SortCode:undefined,
					F_Description:undefined,
					F_TaskStatus:undefined,
					F_TaskType:undefined,
					F_CreatorTime:undefined,
					tableField0bb944:[],
                },
				jurisdictionType:'',
				idList:[],
				btnActive: null,
				ztForm: {
					"F_Reason": undefined,
				},
				ztRules: {
					F_Reason: [{
						required: true,
						message: this.$t('common.inputText') + ' ' + '暂停原因',
						trigger: 'blur',
					}, ],
				},
				btnLoading:false,
				routeData:{},
				moreList:[]
            };
        },
		watch: {
        },
        onLoad(option) {
			this.menuIds = option.menuIds
			this.jurisdictionType = option.jurisdictionType
            this.dataForm.id = option.id || ''
			this.idList = option.idList
			this.routeData={
				F_ProdUserByIds:option.F_ProdUserByIds
			}
            uni.setNavigationBarTitle({
                title: this.$t('common.detailText')
            })
            this.initData()
			uni.$on('refresh', () => {
				this.initData()                              
			})
		},
		computed:{
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
			initCollapse() {
				setTimeout(() => {
					this.keyCode = +new Date()
				}, 50)
			},
			onCollapseChange() {
				this.initCollapse()
			},
          async  initData() {
				const baseStore = useBaseStore()
				baseStore.updateRelationData({})
                if (this.dataForm.id) {
					
				request({
					url: '/api/example/AProdTask/Detail/' + this.dataForm.id,
					method: 'get',
				}).then(res => {
					this.dataForm = res.data;
					this.initCollapse();
				})

				request({
					url: '/api/example/AProdTask/' + this.dataForm.id,
					method: 'get',
				}).then(res => {
					let data=res.data||{}
					this.dataForm.F_TaskStatusCode=data.F_TaskStatus
					this.dataForm.F_ProcessId=data.F_ProcessId
					this.moreList=this.getMoreList()
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
			getMoreList(){
				return [{
					label: '开始工序',
					ifShow: this.dataForm.F_TaskStatusCode == '0',
					status:'1',
					method:"handleState",
					auth: 'btn_startProcess',
				},{
					label: '报工',
					ifShow: this.dataForm.F_TaskStatusCode == '1',
					method:"handleReport",
					auth: 'btn_report',
				},{
					label: '未开始',
					ifShow: this.dataForm.F_TaskStatusCode != '3' && this.dataForm.F_TaskStatusCode != '0',
					status:'0',
					method:"handleState",
					auth: 'btn_notStart',
				},{
					label: '进行中',
					ifShow: this.dataForm.F_TaskStatusCode != '3'&& this.dataForm.F_TaskStatusCode != '1',
					status:'1',
					method:"handleState",
					auth: 'btn_inProgress',
				},{
					label: '已完成',
					ifShow: this.dataForm.F_TaskStatusCode != '3' && this.dataForm.F_TaskStatusCode != '2',
					status:'2',
					method:"handleState",
					auth: 'btn_completed',
				},{
					label: '暂停',
					ifShow: this.dataForm.F_TaskStatusCode == '1',
					status:'3',
					method:"handleState",
					auth: 'btn_pause',
				},{
					label: '取消暂停',
					ifShow: this.dataForm.F_TaskStatusCode == '3',
					status:'4',
					method:"handleState",
					auth: 'btn_cancelPause',
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
			  //修改状态
			handleState(id, state) {
				if (state == '3') {
					const data = {
					id: id,
					fullName: '暂停原因',
					};
					this.$refs.ztPopup.open('bottom')
				} else {
					uni.showModal({
						title: '提示',
						content: '你确定修改状态？',
						success: (res) => {
							if (res.confirm) {
								request({
									url: '/api/example/AProdTask/Check/' +id + "/" +state,
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
			  // 报工
			handleReport() {
			    const data = {
					id: '',
					idList:"",
					menuIds:"2014516365863555072",
					F_TaskId: this.dataForm.id,
					F_ProcessId: this.dataForm.F_ProcessId,
					F_ProcessName: this.dataForm.F_ProcessName,
					F_ProdUserByIds: this.routeData.F_ProdUserByIds,
					F_ProdQty: this.dataForm.F_ProdQty,
			    };
				this.moreClose()
				let btnType = 'btn_add'
				let url = '/pages/apply/subcode/processManagement/aProdReport/form?id='+data.id+'&jurisdictionType='+btnType+'&menuIds='+data.menuIds+'&idList='+data.idList+'&F_TaskId='+this.dataForm.id+'&F_ProcessId='+data.F_ProcessId+'&F_ProcessName='+this.dataForm.F_ProcessName+'&F_ProdUserByIds='+data.F_ProdUserByIds+'&F_ProdQty='+this.dataForm.F_ProdQty;
				uni.navigateTo({
					url: url
				})
			},
			  // 暂停原因提交
			  submitForm() {
			  	this.$refs.ztForm.validate(valid => {
			  		if (!valid) return
			  		if (!this.ztForm.F_Reason) {
			  			uni.showToast({
			  				title: '请填写暂停原因',
			  				icon:'none'
			  			})
			  			return
			  		}
			  		this.btnLoading = true
			  		request({
			  			url: '/api/example/AProdTask/ReasonUpdate/'+this.dataForm.id,
			  			method: 'put',
			  			data: {
			  				id: this.dataForm.id,
			  				F_Reason: this.ztForm.F_Reason,
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