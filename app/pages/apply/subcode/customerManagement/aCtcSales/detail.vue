﻿﻿﻿﻿<style lang="scss">
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
				<u-form-item label='合同' prop="F_ContractId"  :label-width="100 * 1.5" class="popup-select">
				<view class="detail-text-box">
					<view class="jnpf-detail-text" >{{dataForm.F_ContractId}}</view>
					<DisplayList v-if="Object.keys(extraData).length" :extraObj="extraData.F_ContractId" :extraOptions='[]'></DisplayList>
				</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='支出日期' prop="F_ExpendDate"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_ExpendDate}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='支出类别' prop="F_ExpendType"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_ExpendType}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='金额' prop="F_Money"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_Money"  detailed disabled/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='附件' prop="F_Files"  :label-width="100 * 1.5">
					<JnpfUploadFile v-model="dataForm.F_Files" :noShowBtn="false" :limit='9' sizeUnit='MB' :fileSize='10'  detailed/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='结算状态' prop="F_SettleStatus"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_SettleStatus" detailed  :maskConfig='{
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
				<u-form-item label='结算附件' prop="F_SettleFiles"  :label-width="100 * 1.5">
					<JnpfUploadFile v-model="dataForm.F_SettleFiles" :noShowBtn="false" :limit='9' sizeUnit='MB' :fileSize='10'  detailed/>
					<!-- <JnpfUploadFile v-model="dataForm.F_SettleFiles" :limit='9' sizeUnit='MB' :fileSize='10' pathType="defaultPath"  :sortRule='[]' timeFormat='YYYY' buttonText='点击上传' /> -->
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='结算备注' prop="F_SettleDesc"  :label-width="100 * 1.5">
					<!-- <JnpfTextarea v-model="dataForm.F_SettleDesc" placeholder='请输入'  :showCount='false' /> -->
					<view class="jnpf-detail-text">{{dataForm.F_SettleDesc}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='备注' prop="F_Description"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_Description}}</view>
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
			<u-button   v-if="$permission.hasBtnP('btn_edit',menuIds)" class="buttom-btn" type="primary" @click.stop="handleEdit">{{$t('common.editText','编辑')}}</u-button>
			<u-button v-if="$permission.hasBtnP('btn_settlement', menuIds) && dataForm.F_SettleStatus!=='已结算'" class="buttom-btn" :plain="true" @click.stop="jsOpen">结算</u-button>
		</view>
		<!-- 结算 -->
		<uni-popup ref="jsPopup" background-color="#fff" @change="jsChange">
			<view class="popup-content">
				<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
					<u-form :model="jsForm" :rules="jsRules" ref="jsForm" :errorType="['toast']" label-position="left"
						:label-width="100 * 1.5" label-align="right" class="jnpf-form">
						<view class="u-p-l-20 u-p-r-20 form-item-box">
							<view class="u-p-l-20 u-p-r-20 form-item-box">
								<u-form-item label='结算附件' required prop="F_SettleFiles"  :label-width="100 * 1.5">
									<JnpfUploadFile v-model="jsForm.F_SettleFiles" :limit='9' sizeUnit='MB' :fileSize='10' pathType="defaultPath"  :sortRule='[]' timeFormat='YYYY' buttonText='点击上传' />
								</u-form-item>
							</view>
							<u-form-item label='结算备注' prop="F_SettleDesc" :label-width="100 * 1.5">
								<JnpfTextarea v-model="jsForm.F_SettleDesc" :maxlength='500' placeholder='请输入'
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
			  F_ContractId: [
  {
    "defaultValue": "",
    "field": "id",
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
			  F_ContractId: [],
			},
			extraData:{
			  F_ContractId: {},
			},
				menuIds:'',
				setting: {},
                dataForm: {
                    id:'',
					F_CustomerId:undefined,
					F_ContractId:undefined,
					F_ExpendDate:undefined,
					F_ExpendType:undefined,
					F_Money:undefined,
					F_AuditStatus:undefined,
					F_Files:[],
					F_SettleStatus:'已结算',
					F_SettleFiles:[],
					F_SettleDesc:undefined,
					F_Description:undefined,
					F_CreatorUserId:undefined,
					F_CreatorTime:undefined,
                },
				jurisdictionType:'',
				idList:[],
				jsForm: {
					"F_SettleDesc": undefined,
					F_SettleFiles:undefined,
				},
				jsRules: {
				},
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
					interfaceId: '2010894701401608192 ',
					propsValue:'F_Id' ,
					relationField:'F_ContractCode' ,
					paramList,
				};
				getDataInterfaceDataInfoByIds('2010894701401608192 ', query).then(res => {
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
                        url: '/api/example/ACtcSales/Detail/' + this.dataForm.id,
                        method: 'get',
                    }).then(res => {
                        this.dataForm = res.data;
						if(!this.dataForm.F_Files)this.dataForm.F_Files=[];
						if(!this.dataForm.F_SettleFiles)this.dataForm.F_SettleFiles=[];

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
					this.btnLoading = true
					request({
						url: '/api/example/ACtcSales/Check/' + this.dataForm.id,
						method: 'put',
						data: {
							F_SettleFiles: this.jsForm.F_SettleFiles,
							F_SettleDesc: this.jsForm.F_SettleDesc,
							id:this.dataForm.id,
							tableField0bb944:[]
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
