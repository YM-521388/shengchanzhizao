<template>
	<view class="jnpf-wrap jnpf-wrap-form dynamicModel-form-v">
		<u-form :model="dataForm" :rules="rules" ref="dataForm" :errorType="['toast']" label-position="left" :label-width="100 * 1.5" label-align="right" class="jnpf-form">
			<view class="u-p-l-20 u-p-r-20 form-item-box">
				<u-form-item label='合同编号' prop="F_ContractCode"  :label-width="100 * 1.5">
					<JnpfInput v-model="dataForm.F_ContractCode" placeholder='请输入'  :showCount='false' />
				</u-form-item>
				<view class="u-p-l-20 u-p-r-20 form-item-box">
					<u-form-item label='金额' prop="F_Money" required :label-width="100 * 1.5">
						<JnpfInputNumber v-model="dataForm.F_Money" :step='1.0'  />
					</u-form-item>
				</view>
				<view class="u-p-l-20 u-p-r-20 form-item-box">
					<u-form-item label='备注' prop="F_Description"  :label-width="100 * 1.5">
						<JnpfTextarea v-model="dataForm.F_Description" placeholder='请输入'  :showCount='true' />
					</u-form-item>
				</view>
				<view class="u-p-l-20 u-p-r-20 form-item-box">
					<u-form-item label='附件'  :label-width="100 * 1.5">
						<JnpfUploadFile v-model="dataForm.F_Files" :limit='9' sizeUnit='MB' :fileSize='10' pathType="defaultPath"  :sortRule='[]' timeFormat='YYYY' buttonText='点击上传' />
					</u-form-item>
				</view>
			</view>
		</u-form>
		<view class="buttom-actions">
			<CustomButton :btnText="$t('common.cancelText')" btnIcon="icon-ym icon-ym-add-cancel" customIcon />
			<u-button class="buttom-btn" type="primary" @click="submitForm" :loading="btnLoading">{{$t('common.okText','确定')}}</u-button>
		</view>
	</view>
</template>

<script>
	import request from '@/utils/request'
	import CustomButton from '@/components/CustomButton'
	export default {
		components:{ CustomButton },
		data() {
			return {
				btnLoading: false,
				dataForm: {
				    id:'',
					F_Money:undefined,
					F_Files:undefined,
					F_Description:undefined,
					F_ContractCode:undefined,
					F_ContractId: undefined,
					F_Type: "",
				},
				rules:{
					F_Money: {
						type: 'number',
						required: true,
						message: '请输入金额',
						trigger: ['blur', 'change']
					},
				}
			}
		},
		onLoad(option) {
			this.dataForm.id = option.id || ''
			let type=option.type === '1'? "收入" : "支出"
			this.dataForm.F_Type = type || ''
			this.initData()
			uni.setNavigationBarTitle({
			    title: option.type === '1' ? '收款' : '支出'
			})
		},
		onReady() {
				//如果需要兼容微信小程序，并且校验规则中含有方法等，只能通过setRules方法设置规则。
		    	this.$refs.dataForm.setRules(this.rules)
		    },
		methods: {
			initData() {
			    if (this.dataForm.id) {
			        request({
			            url: '/api/example/ACtcContract/Detail/' + this.dataForm.id,
			            method: 'get',
			        }).then(res => {
						let data=res.data
			            this.dataForm.F_ContractCode = data.F_ContractCode;
			        })
			    }
			},
			submitForm(){
				this.$refs.dataForm.validate(valid=>{
					if (!valid) return
					let params={
						F_Money:this.dataForm.F_Money,
						F_Files:this.dataForm.F_Files,
						F_Description:this.dataForm.F_Description,
						F_ContractCode:this.dataForm.F_ContractCode,
						F_ContractId: this.dataForm.id,
						F_Type: this.dataForm.F_Type,
					}
					this.btnLoading = true
					request({
					    url: '/api/example/ACtcFinance',
					    method: 'post',
					    data: params,
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
					}).catch(()=>{
						this.btnLoading = false
					})
				})
			}
		}
	}
</script>

<style lang="scss">

</style>
