﻿﻿﻿﻿﻿﻿﻿﻿<style lang="scss">
	.detail-text-box{
		width: 100%;
	}
</style>
<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" ref="dataForm" :errorType="['toast']" label-position="left" :label-width="100 * 1.5" label-align="right" class="jnpf-form">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='报价单号' prop="F_QuoteCode"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_QuoteCode" detailed  :maskConfig='{
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
				<u-form-item label='客户' prop="F_CustomerId"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_CustomerId}}</view>
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
				<u-form-item label='客户地址' prop="F_AddressId"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_AddressId}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='报价金额' prop="F_QuoteAmount"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_QuoteAmount"  detailed disabled/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='商品总数' prop="F_GoodsTotal"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_GoodsTotal" :precision='1'  detailed disabled/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='交货日期' prop="F_DeliveryDate"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_DeliveryDate}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='报价日期' prop="F_QuoteDate"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_QuoteDate}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='业务员' prop="F_SalesmanId"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_SalesmanId}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='报价状态' prop="F_QuoteStatus"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_QuoteStatus" detailed  :maskConfig='{
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
				<u-form-item label='备注' prop="F_Description"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_Description}}</view>
				</u-form-item>
			</view>
            <view class="jnpf-table" >
				<view class="jnpf-table-item" v-for="(item,i) in dataForm.tableFielddc29f7" :key="i">
					<view class="jnpf-table-item-title u-flex u-row-between">
						<view class="jnpf-table-item-title-num u-line-1 u-p-r-20 u-p-l-20" @click="clickIcon('选择报价单商品','')">{{'选择报价单商品'}}({{i+1}})
						</view>
                    </view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='商品'prop="F_CustomerId"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableFielddc29f7[i].F_CustomerId}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='单价'prop="F_Price"  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableFielddc29f7[i].F_Price"  detailed disabled/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='销售数量'prop="F_SaleQty"  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableFielddc29f7[i].F_SaleQty"  detailed disabled/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='折扣'prop="F_Discount"  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableFielddc29f7[i].F_Discount"  detailed disabled/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='优惠金额'prop="F_DiscountAmount"  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableFielddc29f7[i].F_DiscountAmount"  detailed disabled/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='商品备注'prop="F_Description"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableFielddc29f7[i].F_Description}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='附件'prop="F_Files"  :label-width="100 * 1.5">
							<JnpfUploadFile v-model="dataForm.tableFielddc29f7[i].F_Files" :noShowBtn="false" :limit='9' sizeUnit='MB' :fileSize='10'  detailed/>
						</u-form-item>
					</view>
                </view>
            </view>
		</u-form>
		<view class="buttom-actions">
			<CustomButton class="u-flex buttom-btn-left-inner" :btnText="$t('common.cancelText')" btnIcon="icon-ym icon-ym-add-cancel" customIcon />
			<u-button v-if="$permission.hasBtnP('btn_edit',menuIds)" class="buttom-btn" type="primary" @click.stop="handleEdit">{{$t('common.editText','编辑')}}</u-button>
			<u-button v-if="$permission.hasBtnP('btn_generate',menuIds)" class="buttom-btn"  @click.stop="handleGenerate">生成合同</u-button>
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
					F_QuoteCode:undefined,
					F_CustomerId:undefined,
					F_ContactId:undefined,
					F_AddressId:undefined,
					F_QuoteAmount:undefined,
					F_GoodsTotal:undefined,
					F_DeliveryDate:undefined,
					F_QuoteDate:undefined,
					F_SalesmanId:undefined,
					F_QuoteStatus:undefined,
					F_AuditStatus:undefined,
					F_Description:undefined,
					tableFielddc29f7:[],
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
                        url: '/api/example/ACtcQuote/Detail/' + this.dataForm.id,
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
			handleGenerate(){
				let btnType = 'btn_generate';
				uni.navigateTo({
					url: './generateContract?id=' + this.dataForm.id + '&jurisdictionType=' + btnType + '&menuIds=' + this.menuIds + '&idList='+this.idList
				})
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
