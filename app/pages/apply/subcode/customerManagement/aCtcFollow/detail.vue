﻿﻿﻿<style lang="scss">
	.detail-text-box{
		width: 100%;
	}
</style>
<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" ref="dataForm" :errorType="['toast']" label-position="left" :label-width="100 * 1.5" label-align="right" class="jnpf-form">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='客户' prop="F_CustomerId"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_CustomerId}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='跟单类型' prop="F_FollowType"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_FollowType}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='联系人' prop="F_ContactId"  :label-width="100 * 1.5" class="popup-select">
				<view class="detail-text-box">
					<view class="jnpf-detail-text" >{{dataForm.F_ContactId}}</view>
					<DisplayList v-if="Object.keys(extraData).length" :extraObj="extraData.F_ContactId" :extraOptions='[]'></DisplayList>
				</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='跟单内容' prop="F_FollowContent"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_FollowContent}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='跟单状态' prop="F_State"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_State}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='下次跟单时间' prop="F_NextFollowTime"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_NextFollowTime}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='创建人员' prop="F_CreatorUserId" v-if='false'  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_CreatorUserId}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='创建时间' prop="F_CreatorTime" v-if='false'  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_CreatorTime}}</view>
				</u-form-item>
			</view>
		</u-form>
		<view class="buttom-actions">
			<CustomButton class="u-flex buttom-btn-left-inner" :btnText="$t('common.cancelText')" btnIcon="icon-ym icon-ym-add-cancel" customIcon />
			<u-button v-if="$permission.hasBtnP('btn_edit',menuIds)" class="buttom-btn" type="primary" @click.stop="handleEdit">{{$t('common.editText','编辑')}}</u-button>
			<u-button class="buttom-btn" v-if="$permission.hasBtnP('btn_completed',menuIds)&&dataForm.F_State!=='已完成'" :plain="true" @click.stop="handleOperate">{{getStateCode(dataForm.F_State)}}</u-button>
		</view>
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
		computed:{
		
		},
        data() {
            return {
			keyCode : +new Date(),
			interfaceRes:{
			  F_ContactId: [
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
			  F_ContactId: [],
			},
			extraData:{
			  F_ContactId: {},
			},
				menuIds:'',
				setting: {},
                dataForm: {
                    id:'',
					F_CustomerId:undefined,
					F_FollowType:undefined,
					F_ContactId:undefined,
					F_FollowContent:undefined,
					F_NextFollowTime:undefined,
					F_CreatorUserId:undefined,
					F_CreatorTime:undefined,
					F_State:'已完成',
                },
				jurisdictionType:'',
				idList:[],
				trackStatusData:[],
				status:0
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
			this.getTrackStatus()
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
			getF_ContactIdExtraInfo() {
				if(!this.dataForm.F_ContactId_id) return;
				const paramList = this.getParamList('F_ContactId');
				const query = {
					ids: [this.dataForm.F_ContactId_id],
					interfaceId: '2009459664370143232 ',
					propsValue:'F_Id' ,
					relationField:'F_ContactName' ,
					paramList,
				};
				getDataInterfaceDataInfoByIds('2009459664370143232 ', query).then(res => {
					const data = res.data && res.data.length ? res.data[0] : {};
					this.extraData.F_ContactId = data;
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
                        url: '/api/example/ACtcFollow/Detail/' + this.dataForm.id,
                        method: 'get',
                    }).then(res => {
                        this.dataForm = res.data;

						this.getF_ContactIdExtraInfo();
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
			getTrackStatus(){
				getDictionaryDataSelector('2015701595244859392').then(res => {
					this.trackStatusData = res.data.list
				});
			},
			getStateCode(){
					let title=''
					switch (this.dataForm.F_State){
						case '计划中':
						title='完成'
						this.status=1
							break;
						default:
							break;
					}
					return title
				
				
			},
			handleOperate(){
				uni.showModal({
					title: '提示',
					content: '确定已完成吗？',
					success: (res) => {
						if (res.confirm) {
							request({
							    url: '/api/example/ACtcFollow/Check/'+ this.dataForm.id + "/" + this
							.status,
							    method: 'get',
							}).then(res => {
								uni.showToast({
								    title: res.msg,
								    complete: () => {
								        setTimeout(() => {
											uni.$emit('refresh')
											uni.navigateBack()
								        }, 500)
								    }
								})
							})
						}
					}
				})
			}

        }
    };
</script>
