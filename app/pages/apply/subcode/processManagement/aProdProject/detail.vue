﻿﻿﻿﻿﻿﻿﻿<style lang="scss">
	.detail-text-box{
		width: 100%;
	}
</style>
<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" ref="dataForm" :errorType="['toast']" label-position="left" :label-width="100 * 1.5" label-align="right" class="jnpf-form">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='项目编号' prop="F_ProjectNo"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_ProjectNo" detailed  :maskConfig='{
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
				<u-form-item label='项目名称' prop="F_ProjectName"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_ProjectName" detailed  :maskConfig='{
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
				<u-form-item label='项目类别' prop="F_ProjectTypeName"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_ProjectTypeName}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box" v-if="dataForm.F_ProjectTypeName == '合同项目'">
				<u-form-item label='合同' prop="F_ContractId"  :label-width="100 * 1.5" class="popup-select">
				<view class="detail-text-box">
					<view class="jnpf-detail-text" >{{dataForm.F_ContractId}}</view>
					<DisplayList v-if="Object.keys(extraData).length" :extraObj="extraData.F_ContractId" :extraOptions='[]'></DisplayList>
				</view>
				</u-form-item>
			</view>
			
<!-- 			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='项目状态' prop="F_Status"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_Status}}</view>
				</u-form-item>
			</view> -->
            <view class="jnpf-table" >
				<view class="jnpf-table-item" v-for="(item,i) in goodsList" :key="i">
					<view class="jnpf-table-item-title u-flex u-row-between">
						<view class="jnpf-table-item-title-num u-line-1 u-p-r-20 u-p-l-20" @click="clickIcon('商品','')">{{'商品'}}({{i+1}})
						</view>
                    </view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='工单编号'prop="F_WorkOrderNo"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{item.F_WorkOrderNo}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='商品'prop="F_GoodsId"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{item.F_GoodsId}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='计划数'prop="F_PlanNum"  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="item.F_PlanNum" :precision='0'  detailed disabled/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='交货日期'prop="F_DeliveryDate"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{item.F_DeliveryDate}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='优先级'prop="F_Priority"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{item.F_Priority}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='BOM'prop="F_BomId"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{item.F_BomId}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='客户'prop="F_CustomerName"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{item.F_CustomerName}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='BOM图片'prop="F_BomImages" class="preview-image-box"  :label-width="100 * 1.5">
							<JnpfUploadImg v-model="item.F_BomImages" detailed />
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='类别'prop="F_Category"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{item.F_Category}}</view>
						</u-form-item>
					</view>
                </view>
            </view>
		</u-form>
		<view class="buttom-actions">
			<CustomButton class="u-flex buttom-btn-left-inner" :btnText="$t('common.cancelText')" btnIcon="icon-ym icon-ym-add-cancel" customIcon />
			<u-button  v-if="$permission.hasBtnP('btn_edit',menuIds)" class="buttom-btn" type="primary" @click.stop="handleEdit">{{$t('common.editText','编辑')}}</u-button>
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
			  F_ContractId: [
  {
    "defaultValue": "",
    "field": "contractId",
    "dataType": "varchar",
    "required": 0,
    "fieldName": "合同ID",
    "relationField": "66666",
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
					F_ProjectNo:undefined,
					F_ProjectName:undefined,
					F_ContractId:undefined,
					F_ProjectType:undefined,
					F_Status:undefined,
					F_CreatorUserId:undefined,
					F_CreatorTime:undefined,
					F_UpdateUserId:undefined,
					F_UpdateTime:undefined,
					tableField751ecb:[],
                },
				jurisdictionType:'',
				idList:[],
				goodsList:[],
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
			this.getGoodsList()
			uni.$on('refresh', () => {
				this.initData() 
				this.getGoodsList()
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
					interfaceId: '2012085692393459712 ',
					propsValue:'id' ,
					relationField:'F_GoodsName' ,
					paramList,
				};
				getDataInterfaceDataInfoByIds('2012085692393459712 ', query).then(res => {
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
                        url: '/api/example/AProdProject/Detail/' + this.dataForm.id,
                        method: 'get',
                    }).then(res => {
                        this.dataForm = res.data;

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
			getGoodsList(){
				request({
				    url: '/api/example/AProdProjectItem/List',
				    method: 'post',
					data:{
						currentPage: 1,
						pageSize: 1000,
						F_ProjectId:this.dataForm.id
					}
				}).then(res => {
				    this.goodsList = res.data.list||[];
				})
			}
        }
    };
</script>
