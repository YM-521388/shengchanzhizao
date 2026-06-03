﻿﻿﻿﻿﻿<style lang="scss" scoped>
	.detail-text-box{
		width: 100%;
	}
	.popup-content {
			padding: 20rpx;
			// min-height: 300rpx;
			height: 800rpx;
			overflow-y: auto;
		}
	
</style>
<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" ref="dataForm" :errorType="['toast']" label-position="left" :label-width="100 * 1.5" label-align="right" class="jnpf-form">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='生产人员' prop="F_ProdUserId"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_ProdUserId}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='良品数' prop="F_GoodQty"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_GoodQty" :precision='0'  detailed disabled/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='不良品数' prop="F_GoodNotQty"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_GoodNotQty" :precision='0'  detailed disabled/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='开始时间' prop="F_StartTime"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_StartTime}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='结束时间' prop="F_EndTime"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_EndTime}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='工资单价' prop="F_WagePrice"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_WagePrice" :precision='2'  detailed disabled/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='附件' prop="F_Files"  :label-width="100 * 1.5">
					<JnpfUploadFile v-model="dataForm.F_Files" :noShowBtn="false" :limit='9' sizeUnit='MB' :fileSize='10'  detailed/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='备注' prop="F_Description"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_Description}}</view>
				</u-form-item>
			</view>
<!-- 			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='不良品项' prop="F_CreatorUserId"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_CreatorUserId}}</view>
				</u-form-item>
			</view> -->
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='结算状态' prop="F_SettleStatus" v-if='false'  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_SettleStatus}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='结算时间' prop="F_SettleTime" v-if='false'  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_SettleTime}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='结算人' prop="F_SettleUserId" v-if='false'  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_SettleUserId}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='报工类型' prop="F_State"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_State}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='生产任务' prop="F_TaskId"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_TaskId}}</view>
				</u-form-item>
			</view>
            <view class="jnpf-table" >
				<view class="jnpf-table-item" v-for="(item,i) in dataForm.tableField579169" :key="i">
					<view class="jnpf-table-item-title u-flex u-row-between">
						<view class="jnpf-table-item-title-num u-line-1 u-p-r-20 u-p-l-20" @click="clickIcon('不良品项','')">{{'不良品项'}}({{i+1}})
						</view>
                    </view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='不良品项'prop="F_DefectId"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableField579169[i].F_DefectId}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='数量'prop="F_Num"  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableField579169[i].F_Num" :precision='0'  detailed disabled/>
						</u-form-item>
					</view>
                </view>
            </view>
			<view class="jnpf-table" >
				<view class="jnpf-table-item" v-for="(item,i) in logsList" :key="i">
					<view class="jnpf-table-item-title u-flex u-row-between">
						<view class="jnpf-table-item-title-num u-line-1 u-p-r-20 u-p-l-20" @click="clickIcon('操作日志','')">{{'操作日志'}}({{i+1}})
						</view>
			        </view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='时间'prop="F_CreatorTime"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{item.F_CreatorTime}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='人员'prop="F_CreatorUserId"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{item.F_CreatorUserId}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='标题'prop="F_Title"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{item.F_Title}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='内容'prop="F_Content"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{item.F_Content}}</view>
						</u-form-item>
					</view>
			    </view>
			</view>
		</u-form>
		<view class="buttom-actions">
			<CustomButton class="u-flex buttom-btn-left-inner" :btnText="$t('common.cancelText')" btnIcon="icon-ym icon-ym-add-cancel" customIcon />
			<u-button  v-if="$permission.hasBtnP('btn_edit',menuIds)" class="buttom-btn" type="primary" @click.stop="handleEdit">{{$t('common.editText','编辑')}}</u-button>
			<u-button v-if="$permission.hasBtnP('btn_settle',menuIds)&&dataForm.F_SettleStatus!=='已结算'" class="buttom-btn" type="primary" :plain="true"
				@click.stop="jsOpen">结算</u-button>
		</view>
		<!-- 结算 -->
		<uni-popup ref="jsPopup" background-color="#fff" @change="jsChange">
			<view class="popup-content">
				<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
					<u-form :model="jsForm" :rules="jsRules" ref="jsForm" :errorType="['toast']" label-position="left"
						:label-width="100 * 1.5" label-align="right" class="jnpf-form">
						<view class="u-p-l-20 u-p-r-20 form-item-box">
							<view class="u-p-l-20 u-p-r-20 form-item-box">
								<u-form-item label='结算状态' required prop="F_SettleStatus"  :label-width="100 * 1.5">
									<JnpfSelect v-model="jsForm.F_SettleStatus" :options="f_SettleStatusOptions" :props="f_SettleStatusProps" filterable placeholder='请选择' />
								</u-form-item>
							</view>
							<u-form-item label='备注' prop="F_DescriptionXin" :label-width="100 * 1.5">
								<JnpfTextarea v-model="jsForm.F_DescriptionXin" :maxlength='500' placeholder='请输入'
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
	import { getDataInterfaceDataInfoByIds, getRelationFormDetail,getDictionaryDataSelector} from '@/api/common.js'
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
					F_ProdUserId:[],
					F_GoodQty:undefined,
					F_GoodNotQty:undefined,
					F_StartTime:undefined,
					F_EndTime:undefined,
					F_WagePrice:undefined,
					F_Files:[],
					F_Description:undefined,
					F_CreatorUserId:[],
					F_SettleStatus:"0",
					F_SettleTime:undefined,
					F_SettleUserId:undefined,
					F_State:"0",
					F_TaskId:undefined,
					F_CreatorTime:undefined,
					tableField579169:[],
					
                },
				jurisdictionType:'',
				idList:[],
				logsList:[],
				jsForm: {
					"F_DescriptionXin": undefined,
					F_SettleStatus:undefined,
				},
				jsRules: {
					F_SettleStatus: [{
						required:true,
						message:this.$t('common.inputText')+' '+'生产人员',
						trigger:'change',
						type:'array'
					}, ],
				},
				f_SettleStatusProps:{'label':'fullName','value':'enCode'},
				f_SettleStatusOptions: [],
				btnLoading:false
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
			this.getf_SettleStatus();
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
                        url: '/api/example/AProdReport/Detail/' + this.dataForm.id,
                        method: 'get',
                    }).then(res => {
                        this.dataForm = res.data;
						if(!this.dataForm.F_Files)this.dataForm.F_Files=[];

						this.initCollapse();
						this.getDefectList()
						this.getLogsList()
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
			// 获取不良品项
			getDefectList(){
				request({
				    url: '/api/example/AProdReport/DefectList',
				    method: 'post',
					data:{
						"currentPage": 1,
						"pageSize": 1000,
						"F_ReportId": this.dataForm.id
					}
				}).then(res => {
					let data=res.data
					if(!this.dataForm.tableField579169)this.dataForm.tableField579169=data.list;
				})
			},
			// 获取日志
			getLogsList(){
				request({
				    url: '/api/example/AProdReportLog/List',
				    method: 'post',
					data:{
						"currentPage": 1,
						"pageSize": 1000,
						"F_ReportId": this.dataForm.id
					}
				}).then(res => {
					let data=res.data
					this.logsList=data.list
				})
			},
			getf_SettleStatus(){
				getDictionaryDataSelector('2014214471169478656').then(res => {
					this.f_SettleStatusOptions = res.data.list
				});
			},
			jsClose() {
				this.$refs.jsPopup.close()
			},
			// 打开暂停取消弹窗
			jsOpen() {
				this.$refs.jsPopup.open('bottom')
				if (this.$refs.jsForm) this.$refs.jsForm.setRules(this.zqRules)
			},
			jsChange() {
			
			},
			// 提交
			submitForm() {
				this.$refs.jsForm.validate(valid => {
					if (!valid) return
					if (!this.jsForm.F_SettleStatus) {
						uni.showToast({
							title: '请填写结算状态',
							icon:'none'
						})
						return
					}
					this.btnLoading = true
					request({
						url: '/api/example/AProdReport/SettleUpdate/' + this.dataForm.id,
						method: 'put',
						data: {
							F_SettleStatus: this.jsForm.F_SettleStatus,
							F_DescriptionXin: this.jsForm.F_DescriptionXin,
							id:this.dataForm.id,
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
