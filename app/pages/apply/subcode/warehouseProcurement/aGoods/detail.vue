<style lang="scss">
	.detail-text-box{
		width: 100%;
	}
</style>
<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" ref="dataForm" :errorType="['toast']" label-position="left" :label-width="100 * 1.5" label-align="right" class="jnpf-form">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='商品编号' prop="F_GoodsNo"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_GoodsNo" detailed  :maskConfig='{
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
				<u-form-item label='商品名称' prop="F_GoodsName"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_GoodsName" detailed  :maskConfig='{
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
				<u-form-item label='类别' prop="F_CategoryId"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_CategoryId}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='单位' prop="F_Unit"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_Unit}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='销售单价' prop="F_SalePrice"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_SalePrice"  detailed disabled/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='成本单价' prop="F_CostPrice"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_CostPrice"  detailed disabled/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='售后佣金' prop="F_CommissionFixed"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_CommissionFixed"  detailed disabled/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='商品来源' prop="F_Source"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_Source}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='规格' prop="F_Specification"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_Specification" detailed  :maskConfig='{
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
				<u-form-item label='外协单价' prop="F_OutsourcePrice"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_OutsourcePrice"  detailed disabled/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='供应商' prop="F_SupplierId"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_SupplierId}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='检验规范' prop="F_InspectRule"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_InspectRule" detailed  :maskConfig='{
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
				<u-form-item label='库存上限警告值' prop="F_StockUpperLimit" left-icon="question-circle-fill" @clickIcon="clickIcon(label='库存上限警告值','设置该商品在仓库中的默认库存上限的警告值。' )" :left-icon-style="{'color':'#a0acb7'}"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_StockUpperLimit" :precision='1'  detailed disabled/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='库存下限警告值' prop="F_StockLowerLimit" left-icon="question-circle-fill" @clickIcon="clickIcon(label='库存下限警告值','设置该商品在仓库中的默认库存下限的警告值。' )" :left-icon-style="{'color':'#a0acb7'}"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_StockLowerLimit" :precision='1'  detailed disabled/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='商品图片' prop="F_Image" class="preview-image-box"  :label-width="100 * 1.5">
					<JnpfUploadImg v-model="dataForm.F_Image" detailed />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='附件' prop="F_AttachmentUrl"  :label-width="100 * 1.5">
					<JnpfUploadFile v-model="dataForm.F_AttachmentUrl" :noShowBtn="false" :limit='9' sizeUnit='MB' :fileSize='10'  detailed/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='备注' prop="F_Remark"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_Remark}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='商品简介' prop="F_Intro">
					<JnpfEditor  v-model="dataForm.F_Intro" detailed />
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
			},
			extraOptions:{
			},
			extraData:{
			},
				menuIds:'',
				setting: {},
                dataForm: {
                    id:'',
					F_GoodsNo:undefined,
					F_GoodsName:undefined,
					F_CategoryId:undefined,
					F_Unit:undefined,
					F_SalePrice:undefined,
					F_CostPrice:undefined,
					F_CommissionFixed:undefined,
					F_Source:undefined,
					F_Specification:undefined,
					F_OutsourcePrice:undefined,
					F_SupplierId:undefined,
					F_InspectRule:undefined,
					F_StockUpperLimit:undefined,
					F_StockLowerLimit:undefined,
					F_Image:[],
					F_AttachmentUrl:[],
					F_Remark:undefined,
					F_Intro:undefined,
					F_CreatorTime:undefined,
					F_CreatorUserId:undefined,
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
                        url: '/api/example/AGoods/Detail/' + this.dataForm.id,
                        method: 'get',
                    }).then(res => {
                        this.dataForm = res.data;
						if(!this.dataForm.F_CategoryId)this.dataForm.F_CategoryId=[];
						if(!this.dataForm.F_Image)this.dataForm.F_Image=[];
						if(!this.dataForm.F_AttachmentUrl)this.dataForm.F_AttachmentUrl=[];

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
