<style lang="scss">
	.detail-text-box{
		width: 100%;
	}
</style>
<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" ref="dataForm" :errorType="['toast']" label-position="left" :label-width="100 * 1.5" label-align="right" class="jnpf-form">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='外协工单' prop="F_OutsourceId"  :label-width="100 * 1.5" class="popup-select">
				<view class="detail-text-box">
					<view class="jnpf-detail-text" >{{dataForm.F_OutsourceId}}</view>
					<DisplayList v-if="Object.keys(extraData).length" :extraObj="extraData.F_OutsourceId" :extraOptions='[]'></DisplayList>
				</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='合格数量' prop="F_PassNum"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_PassNum" :precision='0'  detailed disabled/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='不合格数量' prop="F_FailNum"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_FailNum" :precision='0'  detailed disabled/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='外协类型' prop="F_OutsourceType"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_OutsourceType" detailed  :maskConfig='{
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
				<u-form-item label='结算状态' prop="F_SettleStatus"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_SettleStatus}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='结算单价' prop="F_SettleUnitPrice"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_SettleUnitPrice" detailed  :maskConfig='{
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
				<u-form-item label='结算人' prop="F_SettleUserId"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_SettleUserId}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='结算时间' prop="F_SettleTime"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_SettleTime}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='结算附件' prop="F_SettleFiles"  :label-width="100 * 1.5">
					<JnpfUploadFile v-model="dataForm.F_SettleFiles" :noShowBtn="false" :limit='9' sizeUnit='MB' :fileSize='10'  detailed/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='结算备注' prop="F_SettleDesc"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_SettleDesc}}</view>
				</u-form-item>
			</view>
            <view class="jnpf-table" >
				<view class="jnpf-table-item" v-for="(item,i) in dataForm.tableField561a64" :key="i">
					<view class="jnpf-table-item-title u-flex u-row-between">
						<view class="jnpf-table-item-title-num u-line-1 u-p-r-20 u-p-l-20" @click="clickIcon('选择验收内容','')">{{'选择验收内容'}}({{i+1}})
						</view>
                    </view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='验收内容'prop="F_AcceptId"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableField561a64[i].F_AcceptId}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='排序'prop="F_SortCode"  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableField561a64[i].F_SortCode" :precision='0'  detailed disabled/>
						</u-form-item>
					</view>
                </view>
            </view>
		</u-form>
		<view class="buttom-actions">
			<CustomButton class="u-flex buttom-btn-left-inner" :btnText="$t('common.cancelText')" btnIcon="icon-ym icon-ym-add-cancel" customIcon />
			<u-button v-if="$permission.hasBtnP('btn_edit',menuIds)"  class="buttom-btn" type="primary" @click.stop="handleEdit">{{$t('common.editText','编辑')}}</u-button>
		</view>
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
			  F_OutsourceId: [],
			},
			extraOptions:{
			  F_OutsourceId: [],
			},
			extraData:{
			  F_OutsourceId: {},
			},
				menuIds:'',
				setting: {},
                dataForm: {
                    id:'',
					F_OutsourceId:undefined,
					F_PassNum:undefined,
					F_FailNum:undefined,
					F_OutsourceType:undefined,
					F_SettleStatus:undefined,
					F_SettleUnitPrice:undefined,
					F_SettleUserId:undefined,
					F_SettleTime:undefined,
					F_SettleFiles:[],
					F_SettleDesc:undefined,
					F_CreatorUserId:undefined,
					F_CreatorTime:undefined,
					tableField561a64:[],
                },
				jurisdictionType:'',
				idList:[],
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
			getF_OutsourceIdExtraInfo() {
				if(!this.dataForm.F_OutsourceId_id) return;
				const paramList = this.getParamList('F_OutsourceId');
				const query = {
					ids: [this.dataForm.F_OutsourceId_id],
					interfaceId: '2014180492177444864 ',
					propsValue:'F_Id' ,
					relationField:'F_GoodsNo' ,
					paramList,
				};
				getDataInterfaceDataInfoByIds('2014180492177444864 ', query).then(res => {
					const data = res.data && res.data.length ? res.data[0] : {};
					this.extraData.F_OutsourceId = data;
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
                        url: '/api/example/AProdOutsourceAccept/Detail/' + this.dataForm.id,
                        method: 'get',
                    }).then(res => {
                        this.dataForm = res.data;
						if(!this.dataForm.F_SettleFiles)this.dataForm.F_SettleFiles=[];

						this.getF_OutsourceIdExtraInfo();
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
        }
    };
</script>
