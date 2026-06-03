<template>
	<view class="jnpf-pop-select">
		<mescroll-body ref="mescrollRef" @init="mescrollInit" @down="downCallback" @up="upCallback" :sticky="true"
			:down="downOption" :up="upOption">
			<view class="search-box search-box_sticky">
				<u-search :placeholder="$t('app.apply.pleaseKeyword')" v-model="keyword" height="72"
					:show-action="false" @change="search" bg-color="#f0f2f6" shape="square">
				</u-search>
			</view>
			<view class="u-flex-col tableList">
				<view class="u-flex list-card" v-for="(item,index) in list" :key="index">
					<u-radio-group v-model="selectId[0]" v-if="!onLoadData.multiple">
						<u-radio :name="item[publicField]" @change="radioChange(item)">
							<view class="u-flex-col fieldContent u-m-l-10">
								<view v-for="(column,c) in onLoadData.columnOptions" :key="c"
									class="fieldList u-line-1 u-flex">
									<view class="val">{{column.label+':'}} {{item[column.value]}}</view>
								</view>
							</view>
						</u-radio>
					</u-radio-group>
					<u-checkbox-group wrap v-if="onLoadData.multiple">
						<u-checkbox v-model="item.checked" @change="checkboxChange($event,item)"
							:name="item[publicField]">
							<view class="u-flex-col fieldContent u-m-l-10">
								<view class="fieldList u-line-1 u-flex" v-for="(column,c) in onLoadData.columnOptions"
									:key="c">
									<view class="val">{{column.label+':'}} {{item[column.value]}}</view>
								</view>
							</view>
						</u-checkbox>
					</u-checkbox-group>
				</view>
			</view>
		</mescroll-body>
		<!-- 底部按钮 -->
		<view class="flowBefore-actions">
			<u-button class="buttom-btn" @click.stop="handleClose()">{{$t('common.cancelText')}}</u-button>
			<u-button class="buttom-btn" type="primary" @click.stop="handleConfirm()">{{$t('common.okText')}}</u-button>
		</view>
	</view>
</template>

<script>
		import {
			getRelationSelect,
			getPopSelect
		} from '@/api/common.js'
		import resources from '@/libs/resources.js'
		import MescrollMixin from "@/uni_modules/mescroll-uni/components/mescroll-uni/mescroll-mixins.js";
		import { useBaseStore } from '@/store/modules/base';
		export default {
		mixins: [MescrollMixin],
		data() {
			return {
				downOption: {
					use: true,
					auto: true
				},
				upOption: {
					page: {
						num: 0,
						size: 20,
						time: null
					},
					empty: {
						use: true,
						icon: resources.message.nodata,
						tip: this.$t('common.noData'),
						fixed: true,
						top: "300rpx",
					},
					textNoMore: this.$t('app.apply.noMoreData'),
				},
				list: [],
				type: '',
				onLoadData: {},
				keyword: '',
				innerValue: '',
				listQuery: {
					keyword: ''
				},
				modelId: '',
				cur: null,
				firstVal: '',
				firstId: 0,
				selectId: [],
				publicField: '',
				selectData: [],
				columnOptions: [],
				newSelctData: []
			}
		},
		onLoad(e) {
			try {
				// 判断是否使用全局存储传递数据
				if (e.useStore === '1') {
					const baseStore = useBaseStore();
					this.onLoadData = baseStore.relationData.__popSelectData || {};
					// 清理存储的数据
					baseStore.updateRelationData({ __popSelectData: null });
				} else if (e.data) {
					this.onLoadData = JSON.parse(decodeURIComponent(e.data));
				} else {
					console.error('No data provided to popSelect');
					this.onLoadData = {};
				}
			} catch (error) {
				console.error('JSON parse error:', error);
				console.error('Raw data:', e.data);
				uni.showToast({
					title: '数据加载失败',
					icon: 'none'
				});
				this.onLoadData = {};
			}
			for (let i = 0; i < this.onLoadData.columnOptions.length; i++) {
				this.columnOptions.push(this.onLoadData.columnOptions[i].value)
			}
			this.innerValue = this.onLoadData.innerValue
			this.type = this.onLoadData.type;
			if (this.type === 'relation') {
				this.publicField = 'id'
				if (this.onLoadData.id) this.selectId = [this.onLoadData.id]
			} else {
				this.publicField = this.onLoadData.propsValue
				if (this.onLoadData.id) this.selectId = this.onLoadData.id
			}
			this.modelId = this.onLoadData.modelId
			uni.setNavigationBarTitle({
				title: this.onLoadData.popupTitle
			})
		},
		methods: {
			upCallback(page) {
				const method = this.type === 'popup' ? getPopSelect : getRelationSelect
				const paramList = this.onLoadData.paramList
				let query = {
					...this.listQuery,
					currentPage: page.num,
					pageSize: this.onLoadData.hasPage ? this.onLoadData.pageSize : 10000,
					interfaceId: this.onLoadData.modelId,
					propsValue: this.onLoadData.propsValue,
					relationField: this.onLoadData.relationField,
					columnOptions: this.columnOptions.join(','),
					paramList
				}
				if (this.type === 'relation') query = {
					...query,
					queryType: this.onLoadData.queryType
				}
				method(this.modelId, query, {
					load: page.num == 1
				}).then(res => {
					if (!this.onLoadData.hasPage) {
						this.mescroll.endBySize(res.data.list.length, res.data.pagination.total)
					} else {
						this.mescroll.endSuccess(res.data.list.length);
					}
					if (page.num == 1) this.list = [];
					this.list = this.list.concat(res.data.list);
					if (this.onLoadData.multiple) {
						this.list = this.list.map((o, i) => ({
							...o,
							checked: false
						}))
						if (this.selectId.length){
							this.setSelectValue()
							this.selectData=this.filterDataByIds(this.selectId,this.list)
						} 
					} else {
						var index = this.list.findIndex((item) => {
							return item[this.publicField] == this.selectId
						})
						if (index >= 0) this.selectData = [this.list[index]]
					}
				}).catch(() => {
					this.mescroll.endErr();
				})
			},
			setSelectValue() {
				outer: for (let i = 0; i < this.selectId.length; i++) {
					inner: for (let j = 0; j < this.list.length; j++) {
						if (this.selectId[i] === this.list[j][this.publicField]) {
							this.list[j].checked = true
							break inner
						}
					}
				}
			},
			interfaceDataHandler(data) {
				if (!data.dataProcessing) return data.list
				const dataHandler = this.jnpf.getScriptFunc(data.dataProcessing)
				if (!dataHandler) return data.list
				return dataHandler(data.list)
			},
			radioChange(item) {
				this.selectId = []
				this.selectData = []
				this.selectId.push(item[this.publicField]);
				this.selectData.push(item)
			},
			checkboxChange(e, item) {
				if (e.value) {
					this.selectId.push(e.name)
					this.newSelctData.push(item)
				} else {
					this.newSelctData = this.newSelctData.filter(o => o[this.publicField] != e.name && !e.value)
					this.selectId = this.selectId.filter(o => o != e.name)
					this.selectData = this.selectData.filter(o => o[this.publicField] != e.name)
				}
			},
			handleConfirm() {
				if (this.onLoadData.multiple) {
					this.selectData = this.selectData.concat(this.newSelctData)
					// 多选时，从选中的数据中提取 relationField 的值作为 innerValue
					const relationField = this.onLoadData.relationField;
					const selectedLabels = this.selectData.map(item => item[relationField]).filter(Boolean);
					const innerValue = selectedLabels.length > 0 ? selectedLabels.join(',') : '';
					uni.$emit('confirm', this.selectId, innerValue, this.onLoadData.vModel, this.selectData)
					uni.navigateBack();
				} else {
					this.list.map((o, i) => {
						if (this.selectId == o[this.publicField]) {
							this.firstId = o[this.publicField];
							const val = this.type == 'popup' ? o[this.onLoadData.propsValue] : o[this.publicField];
							const emit = this.type == 'popup' ? 'confirm' : 'confirm1'
							// 单选时，从选中的数据中提取 relationField 的值作为 innerValue
							const relationField = this.onLoadData.relationField;
							const innerValue = o[relationField] || '';
							uni.$emit(emit, val, innerValue, this.onLoadData.vModel, this.selectData[0])
							uni.navigateBack();
						}
					})
				}
			},
			handleClose() {
				this.selectId = ""
				uni.navigateBack();
			},
			search() {
				// 节流,避免输入过快多次请求
				this.searchTimer && clearTimeout(this.searchTimer)
				this.searchTimer = setTimeout(() => {
					this.list = [];
					this.listQuery.keyword = this.keyword
					this.listQuery.currentPage = 1
					this.mescroll.resetUpScroll();
				}, 300)
			},
			filterDataByIds(ids, list) {
			      if (!Array.isArray(ids) || !Array.isArray(list) || ids.length === 0) {
			        return [];
			      }
			      return list.filter(item => ids.includes(item.id));
			}
		}
	}
</script>

<style scoped lang="scss">
	page {
		background-color: #f0f2f6;
	}

	.jnpf-pop-select {
		width: 100%;
		height: 100%;
		padding-bottom: 106rpx;
		background-color: #f0f2f6;
		.tableList {
			overflow: hidden auto;
			padding: 0 20rpx;

			.list-card {
				background-color: #fff;
				width: 100%;
				border-radius: 8rpx;
				margin-top: 20rpx;
				padding: 20rpx 20rpx;

				.fieldContent {
					width: 100%;
					margin-top: -14rpx;

					.fieldList {
						width: 752rpx;

						.key {
							width: 136rpx;
							margin-right: 10rpx;
							text-align: right;
							overflow: hidden;
							white-space: nowrap;
							text-overflow: ellipsis;
							line-height: 60rpx;
						}

						.val {
							flex: 0.85;
							overflow: hidden;
							white-space: nowrap;
							text-overflow: ellipsis;
						}
					}
				}
			}
		}

		.nodata {
			margin-top: 258rpx;
			justify-content: center;
			align-items: center;

			image {
				width: 280rpx;
				height: 215rpx;
			}
		}
	}
</style>