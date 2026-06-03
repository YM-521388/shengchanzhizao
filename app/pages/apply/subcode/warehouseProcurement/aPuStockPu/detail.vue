﻿﻿﻿﻿﻿﻿﻿<style lang="scss">
	.detail-text-box{
		width: 100%;
	}
</style>
<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" ref="dataForm" :errorType="['toast']" label-position="left" :label-width="100 * 1.5" label-align="right" class="jnpf-form">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='采购入库单号' prop="F_StockInNo"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_StockInNo" detailed  :maskConfig='{
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
				<u-form-item label='入库类型' prop="F_StockInType"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_StockInType}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='入库日期' prop="F_StockInDate"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_StockInDate}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='收货人' prop="F_StockInUserId"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_StockInUserId}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='备注' prop="F_Description"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_Description}}</view>
				</u-form-item>
			</view>
            <view class="jnpf-table" >
				<view class="jnpf-table-item" v-for="(item,i) in dataForm.tableField4bd139" :key="i">
					<view class="jnpf-table-item-title u-flex u-row-between">
						<view class="jnpf-table-item-title-num u-line-1 u-p-r-20 u-p-l-20" @click="clickIcon('采购入库商品','')">{{'采购入库商品'}}({{i+1}})
						</view>
                    </view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='商品'prop="F_CustomerId"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableField4bd139[i].F_CustomerId}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='商品编码'prop="F_Encoding"  :label-width="100 * 1.5">
							<JnpfInput v-model="dataForm.tableField4bd139[i].F_Encoding" disabled/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='数量'prop="F_NumInfo"  :label-width="100 * 1.5">
							<JnpfInput v-model="dataForm.tableField4bd139[i].F_NumInfo" disabled/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='成本单价(元)'prop="F_Price"  :label-width="100 * 1.5">
							<JnpfInputNumber v-model="dataForm.tableField4bd139[i].F_Price"  detailed disabled/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='入库仓库'prop="F_WarehouseID"  :label-width="100 * 1.5">
							<JnpfInput v-model="dataForm.tableField4bd139[i].F_WarehouseID" disabled/>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='备注'prop="F_Description"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableField4bd139[i].F_Description}}</view>
						</u-form-item>
					</view>
                </view>
            </view>
            <view class="jnpf-table" >
				<view class="jnpf-table-item" v-for="(item,i) in dataForm.tableFieldca527d" :key="i">
					<view class="jnpf-table-item-title u-flex u-row-between">
						<view class="jnpf-table-item-title-num u-line-1 u-p-r-20 u-p-l-20" @click="clickIcon('采购入库日志','')">{{'采购入库日志'}}({{i+1}})
						</view>
                    </view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='时间'prop="F_CreatorTime"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableFieldca527d[i].F_CreatorTime}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='人员'prop="F_CreatorUserId"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableFieldca527d[i].F_CreatorUserId}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='标题'prop="F_Title"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableFieldca527d[i].F_Title}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='内容'prop="F_Content"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{dataForm.tableFieldca527d[i].F_Content}}</view>
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
			},
			extraOptions:{
			},
			extraData:{
			},
				menuIds:'',
				setting: {},
                dataForm: {
                    id:'',
					F_StockInNo:undefined,
					F_WarehouseId:undefined,
					F_StockInType:undefined,
					F_StockInDate:undefined,
					F_StockInUserId:undefined,
					F_Description:undefined,
					tableField4bd139:[],
					tableFieldca527d:[],
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
                        url: '/api/example/APuStockPu/Detail/' + this.dataForm.id,
                        method: 'get',
                    }).then(res => {
                        this.dataForm = res.data;

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
