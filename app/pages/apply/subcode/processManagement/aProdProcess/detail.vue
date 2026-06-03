﻿﻿﻿﻿<style lang="scss">
	.detail-text-box{
		width: 100%;
	}
</style>
<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" ref="dataForm" :errorType="['toast']" label-position="left" :label-width="100 * 1.5" label-align="right" class="jnpf-form">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='工序名' prop="F_ProcessName"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_ProcessName" detailed  :maskConfig='{
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
				<u-form-item label='工序编号' prop="F_ProcessCode"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_ProcessCode" detailed  :maskConfig='{
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
				<u-form-item label='车间' prop="F_WorkshopId"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_WorkshopId}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='产线' prop="F_Line"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_Line}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='报工单位' prop="F_ReportUnit"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_ReportUnit}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='单位关系' prop="F_UnitRatio"  :label-width="100 * 1.5">
					<!-- <JnpfInputNumber v-model="dataForm.F_UnitRatio"  detailed disabled/> -->
					<view class="jnpf-detail-text">{{dataForm.F_UnitRatio}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='计价方式' prop="F_PriceType"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_PriceType}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='工资单价' prop="F_WagePrice"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_WagePrice" :precision='2'  detailed disabled/>
				</u-form-item>
			</view>
<!-- 			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='标准工时' prop="F_StdHour"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_StdHour" :precision='0' addonAfter="小时"  detailed disabled/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item  prop="F_StdMinute"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_StdMinute" :precision='0' addonAfter="分钟"  detailed disabled/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item  prop="F_StdSecond"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_StdSecond" :precision='0' addonAfter="秒"  detailed disabled/>
				</u-form-item>
			</view> -->
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='启用状态' prop="F_State"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_State}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='附件' prop="F_Files"  :label-width="100 * 1.5">
					<JnpfUploadFile v-model="dataForm.F_Files" :noShowBtn="false" :limit='3' sizeUnit='MB' :fileSize='10'  detailed/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='备注' prop="F_Description"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_Description}}</view>
				</u-form-item>
			</view>
			<view prop="active1">
				<u-tabs :is-scroll="false" :list="active1Title" name="title" v-model="active1Current" @change="active1Change"/>
				<view>
					<view v-show="0 == active1Current">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='机台' prop="F_MachineId"  :label-width="100 * 1.5" class="popup-select">
				<view class="detail-text-box">
					<view class="jnpf-detail-text" >{{dataForm.F_MachineId}}</view>
					<DisplayList v-if="Object.keys(extraData).length" :extraObj="extraData.F_MachineId" :extraOptions='[]'></DisplayList>
				</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='生产人员' prop="F_ProdUserIds"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_ProdUserIds}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='不良品项' prop="F_DefectIds"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_DefectIds}}</view>
				</u-form-item>
			</view>
					</view>
					<view v-show="1 == active1Current">
			<JnpfText content='开启配置后工序默认为外协工序，外协工序无法报工和质检。'  :textStyle='{
  "color": "#F20808",
  "text-align": "left",
  "font-weight": "bold",
  "font-style": "normal",
  "text-decoration": "none",
  "line-height": 20,
  "font-size": 14
}'/>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='工序需外协' prop="F_IsOutsource"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_IsOutsource}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='工序需质检' prop="F_IsQc"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_IsQc}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='不良品需报废、返工' prop="F_DefectHandle"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_DefectHandle}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='质检人员' prop="F_QcUserIds"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_QcUserIds}}</view>
				</u-form-item>
			</view>
					</view>
					<view v-show="2 == active1Current">
			<JnpfText content='开启后可以通过打卡的方式记录生产的总用时。'  :textStyle='{
  "color": "#F20808",
  "text-align": "left",
  "font-weight": "bold",
  "font-style": "normal",
  "text-decoration": "none",
  "line-height": 20,
  "font-size": 14
}'/>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='生产任务计时' prop="F_TaskTimer"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_TaskTimer}}</view>
				</u-form-item>
			</view>
					</view>
				</view>
			</view>
		</u-form>
		<view class="buttom-actions">
			<CustomButton class="u-flex buttom-btn-left-inner" :btnText="$t('common.cancelText')" btnIcon="icon-ym icon-ym-add-cancel" customIcon />
			<u-button  v-if="$permission.hasBtnP('btn_edit',menuIds)"  class="buttom-btn" type="primary" @click.stop="handleEdit">{{$t('common.editText','编辑')}}</u-button>
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
					F_ProcessName:undefined,
					F_ProcessCode:undefined,
					F_WorkshopId:[],
					F_Line:undefined,
					F_ReportUnit:undefined,
					F_UnitRatio:undefined,
					F_PriceType:"0",
					F_WagePrice:undefined,
					F_StdHour:0,
					F_StdMinute:0,
					F_StdSecond:0,
					F_State:1,
					F_Files:[],
					F_Description:undefined,
					F_CreatorUserId:undefined,
					F_CreatorTime:undefined,
					F_MachineId:undefined,
					F_ProdUserIds:[],
					F_DefectIds:[],
					F_IsOutsource:0,
					F_IsQc:0,
					F_DefectHandle:0,
					F_QcUserIds:[],
					F_TaskTimer:0,
                },
				jurisdictionType:'',
				idList:[],
				active1Title:[
  {
    "title": "生产信息"
  },
  {
    "title": "质检/外协信息"
  }
],
				active1Current:2-1,
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
			getF_MachineIdExtraInfo() {
				if(!this.dataForm.F_MachineId_id) return;
				const paramList = this.getParamList('F_MachineId');
				const query = {
					ids: [this.dataForm.F_MachineId_id],
					interfaceId: '2011729284707782656',
					propsValue:'id' ,
					relationField:'F_MachineName' ,
					paramList,
				};
				getDataInterfaceDataInfoByIds('2011729284707782656', query).then(res => {
					const data = res.data && res.data.length ? res.data[0] : {};
					this.extraData.F_MachineId = data;
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
			active1Change(index)
			{
				this.active1Current = index;
				this.initCollapse()
			},
            initData() {
				const baseStore = useBaseStore()
				baseStore.updateRelationData({})
                if (this.dataForm.id) {
                    request({
                        url: '/api/example/AProdProcess/Detail/' + this.dataForm.id,
                        method: 'get',
                    }).then(res => {
                        this.dataForm = res.data;
						if(!this.dataForm.F_WorkshopId)this.dataForm.F_WorkshopId=[];
						if(!this.dataForm.F_Files)this.dataForm.F_Files=[];

						this.getF_MachineIdExtraInfo();
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
