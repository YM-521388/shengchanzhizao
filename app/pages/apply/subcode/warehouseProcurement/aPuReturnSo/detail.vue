﻿﻿﻿﻿<style lang="scss">
	.detail-text-box{
		width: 100%;
	}
</style>
<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" ref="dataForm" :errorType="['toast']" label-position="left" :label-width="100 * 1.5" label-align="right" class="jnpf-form">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='销售退货单号' prop="F_ReturnInNo"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_ReturnInNo" detailed  :maskConfig='{
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
				<u-form-item label='仓库' prop="F_WarehouseId"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_WarehouseId}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='入库类型' prop="F_ReturnInType"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_ReturnInType}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='合同单号' prop="F_ContractId"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_ContractId}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='退货日期' prop="F_ReturnInDate"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_ReturnInDate}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='客户' prop="F_CustomerId"  :label-width="100 * 1.5" class="popup-select">
				<view class="detail-text-box">
					<view class="jnpf-detail-text" >{{dataForm.F_CustomerId}}</view>
					<DisplayList v-if="Object.keys(extraData).length" :extraObj="extraData.F_CustomerId" :extraOptions='[]'></DisplayList>
				</view>
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
				<u-form-item label='地址' prop="F_AddressId"  :label-width="100 * 1.5" class="popup-select">
				<view class="detail-text-box">
					<view class="jnpf-detail-text" >{{dataForm.F_AddressId}}</view>
					<DisplayList v-if="Object.keys(extraData).length" :extraObj="extraData.F_AddressId" :extraOptions='[]'></DisplayList>
				</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='备注' prop="F_Description"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_Description}}</view>
				</u-form-item>
			</view>
            <view class="jnpf-table" >
				<view class="jnpf-table-item" v-for="(item,i) in dataForm.tableFieldcfb049" :key="i">
					<view class="jnpf-table-item-title u-flex u-row-between">
						<view class="jnpf-table-item-title-num u-line-1 u-p-r-20 u-p-l-20" @click="clickIcon('商品','')">{{'商品'}}({{i+1}})
						</view>
                    </view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='商品'prop="F_CustomerId"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableFieldcfb049[i].F_CustomerId}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='退货数量'prop="F_Num"  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableFieldcfb049[i].F_Num" :precision='0'  detailed disabled/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='销售单价(元)'prop="F_Price"  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableFieldcfb049[i].F_Price"  detailed disabled/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='备注'prop="F_Description"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableFieldcfb049[i].F_Description}}</view>
						</u-form-item>
					</view>
                </view>
            </view>
            <view class="jnpf-table" >
				<view class="jnpf-table-item" v-for="(item,i) in dataForm.tableField0c9c23" :key="i">
					<view class="jnpf-table-item-title u-flex u-row-between">
						<view class="jnpf-table-item-title-num u-line-1 u-p-r-20 u-p-l-20" @click="clickIcon('操作日志','')">{{'操作日志'}}({{i+1}})
						</view>
                    </view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='时间'prop="F_CreatorTime"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableField0c9c23[i].F_CreatorTime}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='人员'prop="F_CreatorUserId"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableField0c9c23[i].F_CreatorUserId}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='标题'prop="F_Title"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableField0c9c23[i].F_Title}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='内容'prop="F_Content"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableField0c9c23[i].F_Content}}</view>
						</u-form-item>
					</view>
                </view>
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
			  F_CustomerId: [],
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
			  F_AddressId: [
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
			  F_CustomerId: [],
			  F_ContactId: [],
			  F_AddressId: [],
			},
			extraData:{
			  F_CustomerId: {},
			  F_ContactId: {},
			  F_AddressId: {},
			},
				menuIds:'',
				setting: {},
                dataForm: {
                    id:'',
					F_ReturnInNo:undefined,
					F_WarehouseId:undefined,
					F_ReturnInType:undefined,
					F_ContractId:undefined,
					F_ReturnInDate:undefined,
					F_CustomerId:undefined,
					F_ContactId:undefined,
					F_AddressId:undefined,
					F_Description:undefined,
					F_CreatorUserId:undefined,
					F_CreatorTime:undefined,
					tableFieldcfb049:[],
					tableField0c9c23:[],
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
			getF_CustomerIdExtraInfo() {
				if(!this.dataForm.F_CustomerId_id) return;
				const paramList = this.getParamList('F_CustomerId');
				const query = {
					ids: [this.dataForm.F_CustomerId_id],
					interfaceId: '2009458830353764352 ',
					propsValue:'F_Id' ,
					relationField:'F_CustomerName' ,
					paramList,
				};
				getDataInterfaceDataInfoByIds('2009458830353764352 ', query).then(res => {
					const data = res.data && res.data.length ? res.data[0] : {};
					this.extraData.F_CustomerId = data;
				});
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
			getF_AddressIdExtraInfo() {
				if(!this.dataForm.F_AddressId_id) return;
				const paramList = this.getParamList('F_AddressId');
				const query = {
					ids: [this.dataForm.F_AddressId_id],
					interfaceId: '2009527238009163776 ',
					propsValue:'F_Id' ,
					relationField:'F_IsDefault' ,
					paramList,
				};
				getDataInterfaceDataInfoByIds('2009527238009163776 ', query).then(res => {
					const data = res.data && res.data.length ? res.data[0] : {};
					this.extraData.F_AddressId = data;
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
                        url: '/api/example/APuReturnSo/Detail/' + this.dataForm.id,
                        method: 'get',
                    }).then(res => {
                        this.dataForm = res.data;

						this.getF_CustomerIdExtraInfo();
						this.getF_ContactIdExtraInfo();
						this.getF_AddressIdExtraInfo();
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
