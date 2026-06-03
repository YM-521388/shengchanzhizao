<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" ref="dataForm" :errorType="['toast']" label-position="left" :label-width="100 * 1.5"
			label-align="right" class="jnpf-form">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='加工单号' prop="F_ProcessNo" :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_ProcessNo" detailed :maskConfig='{
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
				<u-form-item label='商品' prop="F_GoodsId" :label-width="100 * 1.5" class="popup-select">
					<view class="detail-text-box">
						<view class="jnpf-detail-text">{{dataForm.F_GoodsId}}</view>
						<DisplayList v-if="Object.keys(extraData).length" :extraObj="extraData.F_GoodsId"
							:extraOptions='[]'></DisplayList>
					</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='BOM' prop="F_BOMId" :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_BOMId}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='计划数' prop="F_PlanQty" :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_PlanQty" :precision='0' detailed disabled />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='交货日期' prop="F_DeliveryDate" :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_DeliveryDate}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='优先级' prop="F_Priority" :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_Priority}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='计划开始' prop="F_PlanStartDate" :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_PlanStartDate}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='计划结束' prop="F_PlanEndDate" :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_PlanEndDate}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='图纸' prop="F_DrawingFile" :label-width="100 * 1.5">
					<JnpfUploadFile v-model="dataForm.F_DrawingFile" :noShowBtn="false" :limit='9' sizeUnit='MB'
						:fileSize='10' detailed />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='客户' prop="F_CustomerName" :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_CustomerName" detailed :maskConfig='{
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
			<!-- 			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='门板厚度' prop="F_DoorPlateThickness" :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_DoorPlateThickness}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='箱体板厚' prop="F_BoxPlateThickness" :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_BoxPlateThickness}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='安装板或安装梁' prop="F_InstallOrBeam" :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_InstallOrBeam}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='锁具' prop="F_LockSet" :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_LockSet" detailed :maskConfig='{
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
				<u-form-item label='铰链' prop="F_Hinge" :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_Hinge" detailed :maskConfig='{
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
				<u-form-item label='颜色' prop="F_Color" :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_Color" detailed :maskConfig='{
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
			</view> -->
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='类别' prop="F_Type" :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_Type}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='BOM图片' prop="F_BOMImages" class="preview-image-box" :label-width="100 * 1.5">
					<JnpfUploadImg v-model="dataForm.F_BOMImages" detailed />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='备注' prop="F_Description" :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_Description}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='合同' prop="F_CustomerId" v-if='false' :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_CustomerId}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='项目' prop="F_ProjectId" v-if='false' :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_ProjectId}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='生产计划' prop="F_ProdPlanId" v-if='false' :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_ProdPlanId}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='状态' prop="F_State" v-if='false' :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_State}}</view>
				</u-form-item>
			</view>
			<view class="jnpf-table">
				<view class="jnpf-table-item" v-for="(item,i) in dataForm.tableField0c720e" :key="i">
					<view class="jnpf-table-item-title u-flex u-row-between">
						<view class="jnpf-table-item-title-num u-line-1 u-p-r-20 u-p-l-20"
							@click="clickIcon('生产任务','')">{{'生产任务'}}({{i+1}})
						</view>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='排序' prop="F_SortCode" :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableField0c720e[i].F_SortCode" :precision='0' detailed
								disabled />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='工序' prop="F_ProcessId" :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableField0c720e[i].F_ProcessId}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='计划开始' prop="F_PlanStartDate" :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableField0c720e[i].F_PlanStartDate}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='计划结束' prop="F_PlanEndDate" :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableField0c720e[i].F_PlanEndDate}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='备注' prop="F_Description" :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableField0c720e[i].F_Description}}</view>
						</u-form-item>
					</view>
				</view>
			</view>
			<view class="jnpf-table">
				<view class="jnpf-table-item" v-for="(item,i) in dataForm.tableField449e6c" :key="i">
					<view class="jnpf-table-item-title u-flex u-row-between">
						<view class="jnpf-table-item-title-num u-line-1 u-p-r-20 u-p-l-20"
							@click="clickIcon('用料清单', '选择BOM后将拉取数据填入用料清单' )">{{'用料清单'}}({{i+1}})
							<u-icon :name="'question-circle-fill'" class="u-m-l-10" color="#a0acb7" />
						</view>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='排序' prop="F_SortCode" :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableField449e6c[i].F_SortCode" :precision='0' detailed
								disabled />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='商品' prop="F_GoodsId" :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableField449e6c[i].F_GoodsId}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='单位用量' prop="F_Unit" :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableField449e6c[i].F_Unit" :precision='0' detailed
								disabled />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='单位' prop="F_UnitTwo" :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableField449e6c[i].F_UnitTwo}}</view>
						</u-form-item>
					</view>
					<!-- 					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='入库工序' prop="F_StockInProcess" :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableField449e6c[i].F_StockInProcess}}</view>
						</u-form-item>
					</view> -->
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='备注' prop="F_Description" :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableField449e6c[i].F_Description}}</view>
						</u-form-item>
					</view>
				</view>
			</view>
			<!-- 			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='从工艺路线中添加' prop="F_SequenceNo" :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_SequenceNo}}</view>
				</u-form-item>
			</view> -->
		</u-form>
		<view class="buttom-actions">
			<CustomButton class="u-flex buttom-btn-left-inner" :btnText="$t('common.cancelText')"
				btnIcon="icon-ym icon-ym-add-cancel" customIcon />
			<u-button v-if="$permission.hasBtnP('btn_edit',menuIds)" class="buttom-btn" type="primary"
				@click.stop="handleEdit">{{$t('common.editText','编辑')}}</u-button>
			<u-button class="buttom-btn" type="primary" :plain="true" @click.stop="moreOpen">更多</u-button>
		</view>
		<!-- 更多弹窗 -->
		<uni-popup ref="morePopup" background-color="#fff">
			<view class="popup-content">
				<view class="classify-scroll-view u-flex">
					<template v-for="(item,index) in moreList">
						<u-button v-if="item.ifShow&&$permission.hasBtnP(item.auth,menuIds)" :key="index"
							@click="classifyItem(item,index)" class="classify-item u-font-28 u-line-1"
							:class="btnActive===index?'btnActive':''">
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
	import {
		useBaseStore
	} from '@/store/modules/base'
	import DisplayList from '@/components/displayList'
	import CustomButton from '@/components/CustomButton'
	import {
		getDataInterfaceDataInfoByIds,
		getRelationFormDetail,
		getDictionaryDataSelector
	} from '@/api/common.js'
	export default {
		components: {
			DisplayList,
			CustomButton
		},
		computed: {
			moreList() {
				return this.getMoreList()
			}
		},
		data() {
			return {
				keyCode: +new Date(),
				interfaceRes: {
					F_GoodsId: [{
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
					}],
				},
				extraOptions: {
					F_GoodsId: [],
				},
				extraData: {
					F_GoodsId: {},
				},
				menuIds: '',
				setting: {},
				dataForm: {
					id: '',
					F_ProcessNo: undefined,
					F_GoodsId: undefined,
					F_BOMId: undefined,
					F_PlanQty: undefined,
					F_DeliveryDate: undefined,
					F_Priority: undefined,
					F_PlanStartDate: undefined,
					F_PlanEndDate: undefined,
					F_DrawingFile: [],
					F_CustomerName: undefined,
					F_DoorPlateThickness: undefined,
					F_BoxPlateThickness: undefined,
					F_InstallOrBeam: undefined,
					F_LockSet: undefined,
					F_Hinge: undefined,
					F_Color: undefined,
					F_Type: [],
					F_BOMImages: [],
					F_Description: undefined,
					F_CustomerId: undefined,
					F_ProjectId: undefined,
					F_ProdPlanId: undefined,
					F_State: undefined,
					F_CreatorUserId: undefined,
					F_CreatorTime: undefined,
					tableField0c720e: [],
					tableField449e6c: [],
					F_SequenceNo: undefined,
				},
				jurisdictionType: '',
				idList: [],
				btnLoading: false,
				btnActive: null,
				f_StateOptions: [],
				enCode: '',
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
				reasonLabel: ''
			};
		},
		watch: {},
		onLoad(option) {
			this.menuIds = option.menuIds
			this.jurisdictionType = option.jurisdictionType
			this.dataForm.id = option.id || ''
			this.idList = option.idList
			uni.setNavigationBarTitle({
				title: this.$t('common.detailText')
			})
			this.initData()
			this.getListData()
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
				if (!this.dataForm.F_GoodsId_id) return;
				const paramList = this.getParamList('F_GoodsId');
				const query = {
					ids: [this.dataForm.F_GoodsId_id],
					interfaceId: '2012085692393459712 ',
					propsValue: 'id',
					relationField: 'F_GoodsName',
					paramList,
				};
				getDataInterfaceDataInfoByIds('2012085692393459712 ', query).then(res => {
					const data = res.data && res.data.length ? res.data[0] : {};
					this.extraData.F_GoodsId = data;
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
						url: '/api/example/AProdProcessing/Detail/' + this.dataForm.id,
						method: 'get',
					}).then(res => {
						this.dataForm = res.data;
						if (!this.dataForm.F_DrawingFile) this.dataForm.F_DrawingFile = [];
						if (!this.dataForm.F_Type) this.dataForm.F_Type = [];
						if (!this.dataForm.F_BOMImages) this.dataForm.F_BOMImages = [];

						this.getF_GoodsIdExtraInfo();
						this.initCollapse();
					})
					request({
						url: '/api/example/AProdProcessing/' + this.dataForm.id,
						method: 'get',
					}).then(res => {
						this.dataForm.F_StateId = res.data.F_State;
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
						`/pages/apply/dynamicModel/detail?config=` + this.jnpf.base64.encode(JSON.stringify(
							config), "UTF-8");
					uni.navigateTo({
						url: url
					})
				})
			},
			handleEdit() {
				let btnType = 'btn_edit';
				if (!btnType) return
				this.jumPage(this.dataForm.id, btnType)
			},
			jumPage(id, btnType) {
				if (!id) btnType = 'btn_add'
				let query = id ? id : ''
				uni.navigateTo({
					url: './form?id=' + query + '&jurisdictionType=' + btnType + '&menuIds=' + this.menuIds +
						'&idList=' + this.idList
				})
			},
			getMoreList() {
				return [{
						label: '未开始',
						ifShow: this.dataForm.F_StateId == '1',
						status: '0',
						method: "handleState",
						auth: 'btn_notStart',
					}, {
						label: '开始',
						ifShow: this.dataForm.F_StateId == '0',
						status: '1',
						method: "handleState",
						auth: 'btn_start',
					}, {
						label: '完成',
						ifShow: this.dataForm.F_StateId == '0' || this.dataForm.F_StateId == '1',
						status: '2',
						method: "handleState",
						auth: 'btn_complete',
					}, {
						label: '生产中',
						ifShow: this.dataForm.F_StateId == '2',
						status: '1',
						method: "handleState",
						auth: 'btn_producing',
					}, {
						label: '取消',
						ifShow: this.dataForm.F_StateId != '4',
						status: '4',
						method: "handleState",
						auth: 'btn_cancel',
					},
					{
						label: '暂停',
						ifShow: this.dataForm.F_StateId == '1',
						status: '3',
						method: "handleState",
						auth: 'btn_pause',
					},
					{
						label: '继续',
						ifShow: this.dataForm.F_StateId == '3',
						status: '5',
						method: "handleState",
						auth: 'btn_continue',
					}, {
						label: '生产出库',
						ifShow: true,
						method: "handleOutStockProd",
						auth: 'btn_prodOut',
					}, {
						label: '成品入库',
						ifShow: true,
						method: "handleStockFg",
						auth: 'btn_fgIn',
					}, {
						label: '生产退料',
						ifShow: true,
						method: "handleReturnPrd",
						auth: 'btn_prodReturn',
					},
				]
			},
			moreOpen() {
				this.$refs.morePopup.open('bottom')
			},
			ztqxClose() {
				this.$refs.ztqxPopup.close()
			},
			// 打开暂停取消弹窗
			ztqxOpen() {
				this.$refs.ztqxPopup.open('bottom')
				if (this.$refs.zqForm) this.$refs.zqForm.setRules(this.zqRules)
			},
			ztqxChange() {

			},
			classifyItem(item, index) {
				this.btnActive = index
				this[item.method](this.dataForm.id, item.status)
				return
				if (item.enCode === '3' || item.enCode === '4') {
					this.reasonLabel = item.enCode === '3' ? '暂停原因' : '取消原因'
					this.ztqxOpen()
					return
				}
				uni.showModal({
					title: '提示',
					content: '你确定修改状态？',
					success: (res) => {
						if (res.confirm) {
							request({
								url: '/api/example/AProdProcessing/Check/' + this.dataForm.id + "/" +
									item.enCode,
								method: 'get',
							}).then(res => {
								this.$u.toast(res.msg)
								uni.$emit('refresh')
								uni.navigateBack()
							})
						}
					}
				})
			},
			//修改状态
			handleState(id, state) {
				if (state === '3' || state === '4') {
					this.reasonLabel = state === '3' ? '暂停原因' : '取消原因'
					this.enCode = state
					this.ztqxOpen()
				} else {
					uni.showModal({
						title: '提示',
						content: '你确定修改状态？',
						success: (res) => {
							if (res.confirm) {
								request({
									url: '/api/example/AProdProcessing/Check/' + id + "/" + state,
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
			// 成品入库
			handleStockFg() {
				const data = {
					id: '',
					idList: "",
					menuIds: "2013898224796045312",
					F_Method: '0',
					F_ProcessId: this.dataForm.id,
					F_Type: '0',
					fullName: '成品入库',
				};
				let btnType = 'btn_add'
				let url = '/pages/apply/subcode/warehouseProcurement/aPuStockFg/form?id=' + data.id +
					'&jurisdictionType=' + btnType + '&menuIds=' + data.menuIds + '&idList=' +
					data.idList + '&F_Method=' + data.F_Method + '&F_ProcessId=' + data.F_ProcessId +
					'&F_Type=' + data.F_Type;
				uni.navigateTo({
					url: url
				})
			},
			// 生产出库
			handleOutStockProd(){
				const data = {
					id: '',
					idList: "",
					menuIds: "2014532760915546112",
					F_Method: '0',
					F_ProcessId: this.dataForm.id,
					F_Type: '0',
					fullName: '生产出库',
				};
				let btnType = 'btn_add'
				let url = '/pages/apply/subcode/warehouseProcurement/aPuOutStockProd/form?id=' + data.id +
					'&jurisdictionType=' + btnType + '&menuIds=' + data.menuIds + '&idList=' +
					data.idList + '&F_Method=' + data.F_Method + '&F_ProcessId=' + data.F_ProcessId +
					'&F_Type=' + data.F_Type;
				uni.navigateTo({
					url: url
				})
			},
			// 生产退料
			handleReturnPrd(){
				const data = {
					id: '',
					idList: "",
					menuIds: "2013871169236766720",
					F_Method: '0',
					F_ProcessId: this.dataForm.id,
					F_Type: '0',
					fullName: '生产退料',
				};
				let btnType = 'btn_add'
				let url = '/pages/apply/subcode/warehouseProcurement/aPuReturnPrd/form?id=' + data.id +
					'&jurisdictionType=' + btnType + '&menuIds=' + data.menuIds + '&idList=' +
					data.idList + '&F_Method=' + data.F_Method + '&F_ProcessId=' + data.F_ProcessId +
					'&F_Type=' + data.F_Type;
				uni.navigateTo({
					url: url
				})
			},
			// 提交
			submitForm() {
				this.$refs.zqForm.validate(valid => {
					if (!valid) return
					if (!this.zqForm.F_Reason) {
						uni.showToast({
							title: '请填写' + this.reasonLabel,
							icon: 'none'
						})
						return
					}
					this.btnLoading = true
					request({
						url: '/api/example/AProdProcessing/ReasonUpdate/' + this.dataForm.id + "/" + this
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
									this.enCode = ''
								}, 1500)
							}
						})
					}).catch(() => {
						this.btnLoading = false
					})
				})
			},
			getListData() {
				const baseStore = useBaseStore()
				baseStore.updateRelationData({})
				if (this.dataForm.id) {
					request({
						url: '/api/example/AProdProcessing/BomItemList',
						method: 'post',
						data: {
							"currentPage": 1,
							"pageSize": 10000,
							"F_ProcessingId": this.dataForm.id
						}
					}).then(res => {
						this.dataForm.tableField449e6c = res.data.list;
					})
				}
			},
		},

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