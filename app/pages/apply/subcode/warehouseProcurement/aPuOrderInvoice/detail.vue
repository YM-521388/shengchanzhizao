﻿﻿<style lang="scss">
	.detail-text-box{
		width: 100%;
	}
</style>
<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" ref="dataForm" :errorType="['toast']" label-position="left" :label-width="100 * 1.5" label-align="right" class="jnpf-form">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='采购单' prop="F_OrderId"  :label-width="100 * 1.5" class="popup-select">
				<view class="detail-text-box">
					<view class="jnpf-detail-text" >{{dataForm.F_OrderId}}</view>
					<DisplayList v-if="Object.keys(extraData).length" :extraObj="extraData.F_OrderId" :extraOptions='[]'></DisplayList>
				</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='供应商' prop="F_SupplierId"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_SupplierId}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='发票金额' prop="F_Money"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_Money"  detailed disabled/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='开票日期' prop="F_InvoiceDate"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_InvoiceDate}}</view>
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
		</u-form>
		<view class="buttom-actions">
			<CustomButton class="u-flex buttom-btn-left-inner" :btnText="$t('common.cancelText')" btnIcon="icon-ym icon-ym-add-cancel" customIcon />
			<u-button v-if="$permission.hasBtnP('btn_edit',menuIds)" class="buttom-btn" type="primary" @click.stop="handleEdit">{{$t('common.editText','编辑')}}</u-button>
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
			  F_OrderId: [],
			},
			extraOptions:{
			  F_OrderId: [],
			},
			extraData:{
			  F_OrderId: {},
			},
				menuIds:'',
				setting: {},
                dataForm: {
                    id:'',
					F_OrderId:undefined,
					F_SupplierId:undefined,
					F_Money:undefined,
					F_InvoiceDate:undefined,
					F_Files:[],
					F_Description:undefined,
					F_CreatorUserId:undefined,
					F_CreatorTime:undefined,
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
			getF_OrderIdExtraInfo() {
				if(!this.dataForm.F_OrderId_id) return;
				const paramList = this.getParamList('F_OrderId');
				const query = {
					ids: [this.dataForm.F_OrderId_id],
					interfaceId: '2011984543933927424 ',
					propsValue:'F_Id' ,
					relationField:'F_OrderNo' ,
					paramList,
				};
				getDataInterfaceDataInfoByIds('2011984543933927424 ', query).then(res => {
					const data = res.data && res.data.length ? res.data[0] : {};
					this.extraData.F_OrderId = data;
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
                        url: '/api/example/APuOrderInvoice/Detail/' + this.dataForm.id,
                        method: 'get',
                    }).then(res => {
                        this.dataForm = res.data;
						if(!this.dataForm.F_Files)this.dataForm.F_Files=[];

						this.getF_OrderIdExtraInfo();
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
