<template>
	<view class="jnpf-popup-select">
		<u-input input-align='right' :type="formType" v-model="innerValue" disabled @click="openSelect"
			:placeholder="placeholder" class="u-m-b-10">
		</u-input>
		<DisplayList :extraObj="extraObj" :extraOptions="extraOptions"
			v-if="Object.keys(extraObj).length && innerValue">
		</DisplayList>
	</view>
</template>
<script>
	import DisplayList from '@/components/displayList'
	import {
		getPopSelect,
		getDataInterfaceDataInfoByIds
	} from '@/api/common.js'
	import {
		useBaseStore
	} from '@/store/modules/base'
	const baseStore = useBaseStore()
	export default {
		name: 'jnpf-popup-select',
		components: {
			DisplayList
		},
		props: {
			modelValue: {
				default: ''
			},
			formType: {
				type: String,
				default: 'select'
			},
			placeholder: {
				type: String,
				default: '请选择'
			},
			disabled: {
				type: Boolean,
				default: false
			},
			columnOptions: {
				type: Array,
				default: () => []
			},
			extraOptions: {
				type: Array,
				default: () => []
			},
			relationField: {
				type: String,
				default: ''
			},
			propsValue: {
				type: String,
				default: ''
			},
			popupTitle: {
				type: String,
				default: ''
			},
			interfaceId: {
				type: String,
				default: ''
			},
			hasPage: {
				type: Boolean,
				default: false
			},
			pageSize: {
				type: Number,
				default: 100000
			},
			vModel: {
				type: String,
				default: ''
			},
			rowIndex: {
				default: null
			},
			formData: {
				type: Object
			},
			templateJson: {
				type: Array,
				default: () => []
			},
			multiple: {
				type: Boolean,
				default: false
			},
			interfaceName: {
			    type: String,
			    default: 'getDataInterfaceDataInfoByIds'
			}
		},
		data() {
			return {
				selectShow: false,
				innerValue: '',
				defaultValue: '',
				current: null,
				defaultOptions: [],
				firstVal: '',
				firstId: 0,
				selectData: [],
				extraObj: {},
				justConfirmed: false, // 标记刚刚手动确认选择，防止 setDefault 覆盖 innerValue
			}
		},
		watch: {
			modelValue: {
				handler: function(newValue, oldValue) {
					this.setDefault()
				},
				immediate: true,
				deep: true
			},
		},
		created() {
			uni.$on('confirm', (subVal, innerValue, list, selectData) => {
				this.confirm(subVal, innerValue, list, selectData)
			})
			this.setDefault()
		},
		methods: {
			setDefault() {
				// 如果刚刚手动确认选择，跳过 setDefault 防止覆盖 innerValue
				if (this.justConfirmed) {
					this.justConfirmed = false;
					return;
				}
				if (this.modelValue) {
					if (!this.interfaceId) return
					let query = {
						ids: this.multiple ? this.modelValue : [this.modelValue],
						interfaceId: this.interfaceId,
						propsValue: this.propsValue,
						relationField: this.relationField,
						paramList: this.getParamList()
					}
					let api={
				          getPopSelect,
				          getDataInterfaceDataInfoByIds
				        }
					api[this.interfaceName](this.interfaceId, query).then(res => {
						if (this.multiple) {
							this.selectData = res.data.list || []
							let label = []
							this.selectData.forEach((o, i) => {
								for (let j = 0; j < query.ids.length; j++) {
									if (query.ids[j] == o[this.propsValue]) {
										if (!!o[this.relationField]) label.push(o[this.relationField])
									}
								}
							})
							this.innerValue = label.length == 1 ? label[0] : label.join(',')
						} else {
							const data = res.data && res.data.length ? res.data[0] : {};
							this.extraObj = data
							this.innerValue = data[this.relationField]
							if (!this.vModel) return
							let relationData = baseStore.relationData
							relationData[this.vModel] = data
							baseStore.updateRelationData(relationData)
						}
					})
				} else {
					this.innerValue = ''
					if (!this.vModel) return
					let relationData = baseStore.relationData
					relationData[this.vModel] = {}
					baseStore.updateRelationData(relationData)
				}
			},
			getParamList() {
				let templateJson = this.templateJson
				if (!this.formData) return templateJson
				for (let i = 0; i < templateJson.length; i++) {
					if (templateJson[i].relationField && templateJson[i].sourceType == 1) {
						if (templateJson[i].relationField.includes('-')) {
							let tableVModel = templateJson[i].relationField.split('-')[0]
							let childVModel = templateJson[i].relationField.split('-')[1]
							templateJson[i].defaultValue = this.formData[tableVModel] && this.formData[tableVModel][this
								.rowIndex
							] && this.formData[tableVModel][this.rowIndex][childVModel] || ''
						} else {
							templateJson[i].defaultValue = this.formData[templateJson[i].relationField] || ''
						}
					}
				}
				return templateJson
			},
			openSelect() {
				if (this.disabled) return
				const pageSize = this.hasPage ? this.pageSize : 100000
				let data = {
					columnOptions: this.columnOptions,
					relationField: this.relationField,
					type: 'popup',
					propsValue: this.propsValue,
					modelId: this.interfaceId,
					hasPage: this.hasPage,
					pageSize,
					id: !this.multiple ? [this.modelValue] : this.modelValue,
					vModel: this.vModel,
					popupTitle: this.popupTitle || '选择数据',
					innerValue: this.innerValue,
					paramList: this.getParamList(),
					multiple: this.multiple,
					selectData: this.selectData
				}
				const dataStr = JSON.stringify(data);
				// 如果数据太长，使用全局存储而不是 URL 参数传递
				if (dataStr.length > 2000) {
					baseStore.updateRelationData({ __popSelectData: data });
					uni.navigateTo({
						url: '/pages/apply/popSelect/index?useStore=1'
					});
				} else {
					uni.navigateTo({
						url: '/pages/apply/popSelect/index?data=' + encodeURIComponent(dataStr)
					});
				}
			},
			confirm(subVal, innerValue, vModel, selectData) {
				if (vModel === this.vModel) {
					this.justConfirmed = true; // 标记刚刚手动确认，防止 setDefault 覆盖 innerValue
					this.firstVal = innerValue;
					this.firstId = subVal;
					this.innerValue = innerValue;
					this.extraObj = selectData
					this.$emit('update:modelValue', subVal)
					this.$emit('change', subVal, selectData)
				}
			},
		}
	}
</script>

<style lang="scss">
	.jnpf-popup-select {
		width: 100%;
		height: 100%;
	}
</style>