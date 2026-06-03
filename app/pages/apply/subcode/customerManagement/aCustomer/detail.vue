﻿﻿﻿﻿﻿﻿﻿﻿<style lang="scss">
	.detail-text-box{
		width: 100%;
	}
</style>
<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" ref="dataForm" :errorType="['toast']" label-position="left" :label-width="100 * 1.5" label-align="right" class="jnpf-form">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='名称' prop="F_CustomerName"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_CustomerName" detailed  :maskConfig='{
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
				<u-form-item label='编号' prop="F_CustomerCode"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_CustomerCode" detailed  :maskConfig='{
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
				<u-form-item label='二维码' prop="F_QRCode"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_QRCode" detailed  :maskConfig='{
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
				<u-form-item label='客户标签' prop="F_CustomerTags"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_CustomerTags}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='客户共享' prop="F_ShareUsers"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_ShareUsers}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='关注' prop="F_IsFollow"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_IsFollow" detailed  :maskConfig='{
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
				<u-form-item label='状态' prop="F_State"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_State}}</view>
				</u-form-item>
			</view>
            <view class="jnpf-table" >
				<view class="jnpf-table-item" v-for="(item,i) in dataForm.tableField6eed25" :key="i">
					<view class="jnpf-table-item-title u-flex u-row-between">
						<view class="jnpf-table-item-title-num u-line-1 u-p-r-20 u-p-l-20" @click="clickIcon('客户地址','')">{{'客户地址'}}({{i+1}})
						</view>
                    </view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='地区'prop="F_Region"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableField6eed25[i].F_Region}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='地址'prop="F_Address"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableField6eed25[i].F_Address}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='是否默认'prop="F_IsDefault"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableField6eed25[i].F_IsDefault}}</view>
						</u-form-item>
					</view>
                </view>
            </view>
            <view class="jnpf-table" >
				<view class="jnpf-table-item" v-for="(item,i) in dataForm.tableFieldddabab" :key="i">
					<view class="jnpf-table-item-title u-flex u-row-between">
						<view class="jnpf-table-item-title-num u-line-1 u-p-r-20 u-p-l-20" @click="clickIcon('客户联系人','')">{{'客户联系人'}}({{i+1}})
						</view>
                    </view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='联系人'prop="F_ContactName"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableFieldddabab[i].F_ContactName}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='联系人电话'prop="F_ContactPhone"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableFieldddabab[i].F_ContactPhone}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='是否默认'prop="F_IsDefault"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableFieldddabab[i].F_IsDefault}}</view>
						</u-form-item>
					</view>
                </view>
            </view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='备注' prop="F_Description"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_Description}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='附件' prop="F_Files"  :label-width="100 * 1.5">
					<JnpfUploadFile v-model="dataForm.F_Files" :noShowBtn="false" :limit='9' sizeUnit='MB' :fileSize='10'  detailed/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='领用人员' prop="F_RequisitionUserId"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_RequisitionUserId}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='领用时间' prop="F_RequisitionTime"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_RequisitionTime}}</view>
				</u-form-item>
			</view>
			<view class="jnpf-table" >
				<view class="jnpf-table-item" v-for="(item,i) in logs" :key="i">
					<view class="jnpf-table-item-title u-flex u-row-between">
						<view class="jnpf-table-item-title-num u-line-1 u-p-r-20 u-p-l-20" @click="clickIcon('操作日志','')">{{'操作日志'}}({{i+1}})
						</view>
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
			<u-button v-if="$permission.hasBtnP('btn_edit',menuIds)" class="buttom-btn" type="primary" @click.stop="handleEdit">{{$t('common.editText','编辑')}}</u-button>
			<u-button v-if="$permission.hasBtnP('btn_moveSeas',menuIds)" class="buttom-btn" type="primary"  :loading="btnLoading" :plain="true" @click.stop="handleMove">{{$t('common.moveHighSeas','移入公海')}}</u-button>
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
			},
			extraOptions:{
			},
			extraData:{
			},
				menuIds:'',
				setting: {},
                dataForm: {
                    id:'',
					F_CustomerName:undefined,
					F_CustomerCode:undefined,
					F_QRCode:undefined,
					F_IsPublic:undefined,
					F_CustomerTags:undefined,
					F_ShareUsers:[],
					F_IsFollow:undefined,
					F_State:0,
					tableField6eed25:[],
					tableFieldddabab:[],
					F_Description:undefined,
					F_Files:[],
					F_RequisitionUserId:undefined,
					F_RequisitionTime:undefined,
					F_CreatorUserId:undefined,
					F_CreatorTime:undefined,
                },
				jurisdictionType:'',
				idList:[],
				btnLoading:false,
				logs:[]
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
                        url: '/api/example/ACustomer/Detail/' + this.dataForm.id,
                        method: 'get',
                    }).then(res => {
                        this.dataForm = res.data.detail;
						if(!this.dataForm.F_Files)this.dataForm.F_Files=[];
						this.logs=res.data.logs||[]
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
			// 移入公海
			handleMove(){
				this.btnLoading = true
				uni.showModal({
				    title: '提示',
				    content: '确定将该客户移入公海吗？',
				    success: res => {
				        if (res.confirm) {
				            request({
				                url: '/api/example/ACustomer/MoveToPublicSea/' + this.dataForm.id,
				                method: 'post'
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
				
				        }else{
							 this.btnLoading = false
						}
				    }
				})
			}
        }
    };
</script>
