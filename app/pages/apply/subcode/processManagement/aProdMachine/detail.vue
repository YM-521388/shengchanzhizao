<style lang="scss">
	.detail-text-box{
		width: 100%;
	}
</style>
<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" ref="dataForm" :errorType="['toast']" label-position="left" :label-width="100 * 1.5" label-align="right" class="jnpf-form">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='机台图片' prop="F_Image" class="preview-image-box"  :label-width="100 * 1.5">
					<JnpfUploadImg v-model="dataForm.F_Image" detailed />
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='机台编号' prop="F_MachineCode"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_MachineCode" detailed  :maskConfig='{
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
				<u-form-item label='机台名称' prop="F_MachineName"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_MachineName" detailed  :maskConfig='{
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
				<u-form-item label='机台规格' prop="F_MachineSpec"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_MachineSpec" detailed  :maskConfig='{
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
				<u-form-item label='机台状态' prop="F_MachineStatus"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_MachineStatus}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='机台类别' prop="F_MachineType"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_MachineType}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='车间' prop="F_WorkshopId"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_WorkshopId}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='产线' prop="F_LineId"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_LineId}}</view>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='单日运行时长' prop="F_DailyRunHours"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_DailyRunHours" :precision='0' addonAfter="小时/天"  detailed disabled/>
				</u-form-item>
			</view>
			<JnpfText content='标准工时'  :textStyle='{
  "color": "#000000",
  "text-align": "right",
  "font-weight": "normal",
  "font-style": "normal",
  "text-decoration": "none",
  "line-height": 32,
  "font-size": 14
}'/>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
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
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='标准产出' prop="F_StdOutput" left-icon="question-circle-fill" @clickIcon="clickIcon(label='标准产出','标准效率是单位时间内按照标准操作流程生产出的产品数量。标准效率 = 标准产出 / 标准工时。' )" :left-icon-style="{'color':'#a0acb7'}"  :label-width="100 * 1.5">
					<JnpfInputNumber v-model="dataForm.F_StdOutput" :precision='0'  detailed disabled/>
				</u-form-item>
			</view>
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='启用状态' prop="F_State"  :label-width="100 * 1.5">
					<view class="jnpf-detail-text">{{dataForm.F_State}}</view>
				</u-form-item>
			</view>
			<view class="jnpf-table" >
				<view class="jnpf-table-item" v-for="(item,i) in processList" :key="i">
					<view class="jnpf-table-item-title u-flex u-row-between">
						<view class="jnpf-table-item-title-num u-line-1 u-p-r-20 u-p-l-20" @click="clickIcon('生产工序','')">{{'生产工序'}}({{i+1}})
						</view>
			        </view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='工序'prop="F_ProcessCode"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{item.F_ProcessCode}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='车间'prop="F_WorkshopId"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{item.F_WorkshopId}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='机台'prop="F_MachineId"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{item.F_MachineId}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='生成人员'prop="F_ProdUserIds"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{item.F_ProdUserIds}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='质检人员'prop="F_QcUserIds"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{item.F_QcUserIds}}</view>
						</u-form-item>
					</view>
					<view class="u-p-l-20 u-p-r-20 form-item-box">
						<u-form-item label='不良品项' prop="F_DefectIds"  :label-width="100 * 1.5">
							<view class="jnpf-detail-text">{{item.F_DefectIds}}</view>
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
			},
			extraOptions:{
			},
			extraData:{
			},
				menuIds:'',
				setting: {},
                dataForm: {
                    id:'',
					F_Image:[],
					F_MachineCode:undefined,
					F_MachineName:undefined,
					F_MachineSpec:undefined,
					F_MachineStatus:undefined,
					F_MachineType:undefined,
					F_WorkshopId:undefined,
					F_LineId:undefined,
					F_DailyRunHours:undefined,
					F_StdHour:undefined,
					F_StdMinute:undefined,
					F_StdSecond:undefined,
					F_StdOutput:undefined,
					F_State:1,
					F_CreatorUserId:undefined,
					F_CreatorTime:undefined,
                },
				jurisdictionType:'',
				idList:[],
				processList:[],
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
			this.getProductProcessList()
			uni.$on('refresh', () => {
				this.initData()
				this.getProductProcessList()
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
                        url: '/api/example/AProdMachine/Detail/' + this.dataForm.id,
                        method: 'get',
                    }).then(res => {
                        this.dataForm = res.data;
						if(!this.dataForm.F_Image)this.dataForm.F_Image=[];
						if(!this.dataForm.F_MachineType)this.dataForm.F_MachineType=[];
						if(!this.dataForm.F_WorkshopId)this.dataForm.F_WorkshopId=[];

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
			getProductProcessList(){
				request({
				    url: '/api/example/AProdProcess/ProcessList',
				    method: 'post',
					data:{
						currentPage: 1,
						pageSize: 1000,
						machineId:this.dataForm.id
					}
				}).then(res => {
				    this.processList = res.data.list||[];
				})
			}
        }
    };
</script>
