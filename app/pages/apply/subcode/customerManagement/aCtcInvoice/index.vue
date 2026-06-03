<template>
	<view class="dynamicModel-list-v">
		<view class="u-flex top-btn" :class="slide2" v-show="ids.length">
			<view class="button-left" @click.stop="cancel">
				<p class="u-m-t-10 u-font-28">取消</p>
			</view>
			<view class="button-center">
				<p class="u-m-t-10 u-font-28">已选中{{ids.length}}条</p>
			</view>
			<view class="button-right u-m-t-12" @click.stop="checkAll">
				<p class="icon-ym icon-ym-app-checkAll " :style="{'color':this.checkedAll ? '#0293fc' : '#303133'}">
				</p>
			</view>
		</view>
		<view class="head-warp com-dropdown">
			<u-dropdown class="u-dropdown" ref="uDropdown" type="page">
				<u-dropdown-item :title="$t('app.apply.sort')">
					<view class="screen-box">
						<view class="screen-list" v-if="sortOptions.length">
							<view class="u-p-l-20 u-p-r-20 list">
								<scroll-view scroll-y="true" style="height: 100%;">
									<u-cell-group :border="false">
										<u-cell-item @click="cellClick(item)" :arrow="false"
											:title="item.labelI18nCode ? $t(item.labelI18nCode) + ' ' + $t(item.sortType) : item.label"
											v-for="(item, index) in sortOptions" :key="index"
											:title-style="{ color: sortValue.includes(item.value) ? '#2979ff' : '#606266' }">
											<u-icon v-if="sortValue.includes(item.value)" name="checkbox-mark"
												color="#2979ff" size="32" />
										</u-cell-item>
									</u-cell-group>
								</scroll-view>
							</view>
						</view>
						<view v-else class="notData-box u-flex-col">
							<view class="u-flex-col notData-inner">
								<image :src="icon" class="iconImg"></image>
								<text class="notData-inner-text">{{$t('common.noData')}}</text>
							</view>
						</view>
						<view class="buttom-actions" v-if="sortOptions.length" style="z-index: 1;">
							<u-button class="buttom-btn" @click="handleSortReset">{{$t('common.cancelText')}}</u-button>
							<u-button class="buttom-btn" type="primary"
								@click="handleSortSearch">{{$t('common.okText')}}</u-button>
						</view>
					</view>
				</u-dropdown-item>
				<u-dropdown-item :title="$t('app.apply.screen')">
					<view class="screen-box u-flex-col">
						<view class="screen-list">
							<view class="u-p-l-20 u-p-r-20 list">
								<scroll-view scroll-y="true" style="height: 100%;">
									<u-form :model="searchForm" ref="searchForm" :errorType="['toast']"
										label-position="left" label-width="150">
										<u-form-item  label='开票状态'  prop="F_Status">
										    <JnpfInput v-model="searchForm.F_Status"  :placeholder="$t('common.inputText') + ' 开票状态'"  />
                                        </u-form-item>
                                        <u-form-item  label='开票人员'  prop="F_InvoiceUserId">
                                            <JnpfUserSelect v-model="searchForm.F_InvoiceUserId"  :placeholder="$t('common.chooseText') + ' 开票人员'"  selectType='all' />
                                        </u-form-item>
                                        <u-form-item  label='开票时间'  prop="F_InvoiceTime">
                                            <JnpfDateRange v-model="searchForm.F_InvoiceTime" format='yyyy-MM-dd'/>
                                        </u-form-item>
                                        <u-form-item  label='申请人员'  prop="F_ApplyUserId">
                                            <JnpfUserSelect v-model="searchForm.F_ApplyUserId"  :placeholder="$t('common.chooseText') + ' 申请人员'"  selectType='all' />
                                        </u-form-item>
									</u-form>
								</scroll-view>
							</view>
							<view class="u-flex screen-btn">
								<text class="btn btn1" @click="reset">{{$t('common.resetText')}}</text>
								<text class="btn btn2" type="primary"
									@click="closeDropdown">{{$t('common.searchText')}}</text>
							</view>
						</view>
					</view>
				</u-dropdown-item>
			</u-dropdown>
		</view>
		<view class="list-warp">
			<mescroll-uni ref="mescrollRef" @init="mescrollInit" @down="downCallback" @up="upCallback" :fixed="false"
				:down="downOption" :up="upOption">
				<view class="list">
					<view class="list-box">
						<u-swipe-action :show="item.show" :index="index" v-for="(item, index) in list" :key="item.id"
							:options="options" @click="actionClick" @open="open" :btnWidth="160" class="u-m-t-20">
							<view class="item" @click="goDetail(item.id)">
								<view class="u-m-b-10 checkbox_box" v-if="showTop">
									<u-checkbox @change="checkboxChange($event,item)" v-model="item.checked"
										class="checkbox" @tap.stop shape="circle" />
								</view>
								<view class="item-cell" v-for="(column,i) in columnList" :key="i">
									<template v-if="column.jnpfKey != 'table'">
										<text class="item-cell-label">{{ column.labelI18nCode ? $t(column.labelI18nCode, column.label) : column.label }}:</text>
										<text class="item-cell-content" v-if="column.jnpfKey=='inputNumber'">{{toThousands(item[column.prop],column)}}</text> <!-- 开票金额 -->
										<text class="item-cell-content text-primary"
											@click.stop="relationFormClick(item,column)"
											v-else-if="column.jnpfKey == 'relationForm'">{{item[column.prop]}}</text>
										<view class="item-cell-content" v-else-if="column.jnpfKey == 'sign'">
											<JnpfSign v-model="item[column.prop]" detailed align='left' />
										</view>
										<view class="item-cell-content" v-else-if="column.jnpfKey == 'signature'">
											<JnpfSignature v-model="item[column.prop]" detailed align='left' />
										</view>
										<view class="item-cell-content" v-else-if="column.jnpfKey == 'uploadImg'"
											@click.stop>
											<JnpfUploadImg v-model="item[column.prop]" detailed simple
												v-if="item[column.prop]&&item[column.prop].length" />
										</view>
										<view class="item-cell-content" v-else-if="column.jnpfKey == 'uploadFile'"
											@click.stop>
											<JnpfUploadFile v-model="item[column.prop]" detailed
												v-if="item[column.prop]&&item[column.prop].length" align="left" />
										</view>
										<view class="item-cell-content" v-else-if="column.jnpfKey == 'rate'">
											<JnpfRate v-model="item[column.prop]" :max="column.count"
												:allowHalf="column.allowHalf" disabled />
										</view>
										<view class="item-cell-content item-cell-slider"
											v-else-if="column.jnpfKey == 'slider'">
											<JnpfSlider v-model="item[column.prop]" :min="column.min" :max="column.max"
												:step="column.step" disabled />
										</view>
										<view class="item-cell-content" v-else-if="column.jnpfKey == 'input'">
											<!-- <text class="item-cell-content">{{item[column.prop]}}</text> -->
											<JnpfInput v-model="item[column.prop]" detailed showOverflow
												:useMask="column.useMask" :maskConfig="column.maskConfig"
												align='left' />
										</view>
										<text class="item-cell-content" v-else>{{item[column.prop]}}</text>
									</template>
									<tableCell v-else @click.stop class="tableCell"
										:label="column.labelI18nCode ? $t(column.labelI18nCode, column.label) : column.label"
										:childList="item[column.prop]" :children="column.children" ref="tableCell"
										:pageLen="3" @cRelationForm="relationFormClick" />
								</view>
<!-- 								<view class="btn-container">
									<u-button type="info" :loading="btnLoading" :plain="true" v-if="item.F_Status == '开票完成'">开票完成</u-button>
									<u-button type="primary" :loading="btnLoading" :plain="true"
										@click="handleInvoice(item)" v-else>开票完成</u-button>
								</view> -->
							</view>

						</u-swipe-action>

					</view>
				</view>
			</mescroll-uni>
		</view>
		<view v-if="$permission.hasBtnP('btn_add',menuIds)" class="com-addBtn" @click="addPage()">
			<u-icon name="plus" size="60" color="#fff" />
		</view>
		<view class="u-flex bottom-btn" :class="slide" v-if="showTop && list.length">
			<view class="button-preIcon" @click.stop="batchDelete">
				<p class="icon-ym icon-ym-app-delete u-m-b-8"></p>
				<p class="u-m-t-10 u-font-24">{{$t('common.batchDelText')}}</p>
			</view>
		</view>
	</view>
</template>

<script>
	import {
		getDictionaryDataSelector,
		getDataInterfaceRes
	} from '@/api/common'
	import resources from '@/libs/resources.js'
	import MescrollMixin from "@/uni_modules/mescroll-uni/components/mescroll-uni/mescroll-mixins.js";
	import request from '@/utils/request'
	import columnList from './columnList'
	import tableCell from '@/pages/apply/dynamicModel/components/tableCell.vue'
	export default {
		mixins: [MescrollMixin],
		components: {
			tableCell
		},
		data() {
			return {
				slide: '',
				slide2: '',
				checkedAll: false,
				ids: [],
				showTop: false,
				icon: resources.message.nodata,
				sortValue: [],
				searchForm: {
					jnpfKeyword: '',
                    F_Status:undefined,
                    F_InvoiceUserId:undefined,
                    F_InvoiceTime:undefined,
                    F_ApplyUserId:undefined,
				},
				downOption: {
					use: true,
					auto: false
				},
				dataOptions: {
					tableField2cca74: {},
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
						zIndex: 5
					},
					textNoMore: this.$t('common.noMoreData'),
					toTop: {
						bottom: 250
					}
				},
				list: [],
				appColumnList: columnList,
				listQuery: {
					sort: 'desc',
					sidx: 'id',
					keyword: '',
					json: ''
				},
				options: [{
					text: this.$t('common.delText', '删除'),
					value: 'remove',
					style: {
						backgroundColor: '#dd524d'
					}
				}],
				sortOptions: [],
				menuIds: '',
				columnList: [],
				isTree: false,
				userInfo: {},
				firstInitSearchData: false,
				btnLoading: false
			}
		},
		onLoad(e) {
			this.setDefaultQuery()
			this.userInfo = uni.getStorageSync('userInfo') || {}
			this.menuIds = e.menuId
			uni.$on('refresh', () => {
				this.list = [];
				this.mescroll.resetUpScroll();
			})
			this.getColumnList();
			uni.hideLoading();
		},
		onUnload() {
			uni.$off('refresh')
		},
		methods: {
			checkboxChange(e, item) {
				if (e.value) {
					this.ids.push(item.id)
				} else {
					this.ids = this.ids.filter(o => o !== item.id)
				}
				if (this.ids.length) {
					this.showTop = true
				}
			},
			batchDelete() {
				const item = this.ids
				if (!item.length) {
					return this.$u.toast("请选择一条数据")
				}
				const _data = {
					ids: item
				}
				uni.showModal({
					title: '提示',
					content: '删除后数据无法恢复',
					success: (res) => {
						if (res.confirm) {
							request({
								url: '/api/example/ACtcInvoice/batchRemove',
								data: _data,
								method: 'post'
							}).then(res => {
								uni.showToast({
									title: res.msg,
									complete: () => {
										this.cancel()
										this.$u.toast(res.msg)
										this.mescroll.resetUpScroll()
									}
								})
							})
						}
					}
				})
			},
			openBatchOperate() {
				this.showTop = !this.showTop
				if (this.showTop) {
					this.slide = 'slide-up'
					this.slide2 = 'slide-up2'
				}
			},
			checkAll() {
				this.checkedAll = !this.checkedAll
				this.showTop = true
				this.ids = []
				this.list = this.list.map(o => ({
					...o,
					checked: this.checkedAll,
				}))
				this.list.forEach(o => {
					if (this.checkedAll) this.ids.push(o.id)
				})
			},
			cancel() {
				this.list = this.list.map(o => ({
					...o,
					checked: false
				}))
				this.showTop = false
				this.checkedAll = false
				this.ids = []
			},
			toThousands(val, column) {
				if (val || val == 0) {
					let valList = val.toString().split('.')
					let num = Number(valList[0])
					let newVal = column.thousands ? num.toLocaleString() : num
					return valList[1] ? newVal + '.' + valList[1] : newVal
				} else {
					return val
				}
			},
			upCallback(page) {
				let query = {
					currentPage: page.num,
					pageSize: page.size,
					...this.listQuery,
					...this.searchForm,
					menuId: this.menuIds
				}
				query.F_ApplyUserId = query.F_ApplyUserId ? [query.F_ApplyUserId] : null
				query.F_InvoiceUserId = query.F_InvoiceUserId ? [query.F_InvoiceUserId] : null
				request({
					url: '/api/example/ACtcInvoice/List',
					method: 'post',
					data: query,
				}).then(res => {
					if (page.num == 1) this.list = [];
					const list = res.data.list.map(o => {
						return {
							show: false,
							...o
						}
					});
					this.mescroll.endSuccess(res.data.list.length);
					this.list = this.list.concat(list);
					// console.log('list',this.list)
				}).catch(() => {
					this.mescroll.endErr();
				})
			},
			actionClick(itemIndex, btnIndex) {
				if (this.options[btnIndex].value === 'remove') return this.handleClick(itemIndex)
			},
			handleClick(index) {
				const item = this.list[index]
				let data = {
					ids: [item.id]
				}
				uni.showModal({
					title: '提示',
					content: '此操作将永久删除该数据, 是否继续？',
					success: (res) => {
						if (res.confirm) {
							request({
								url: '/api/example/ACtcInvoice/batchRemove',
								method: 'post',
								data,
							}).then(res => {
								this.$u.toast(res.msg)
								this.list.splice(index, 1)
								this.mescroll.resetUpScroll();
							})
						}
					}
				})
			},
			open(index) {
				this.list[index].show = true;
				this.list.map((val, idx) => {
					if (index != idx) this.list[idx].show = false;
				})
			},
			addPage() {
				this.jumPage()
			},
			jumPage(id, btnType) {
				if (!id) btnType = 'btn_add'
				let query = id ? id : '';
				let idList = [];
				for (let i = 0; i < this.list.length; i++) {
					idList.push(this.list[i].id)
				}
				let url = './form?id=' + query + '&jurisdictionType=' + btnType + '&menuIds=' + this.menuIds + '&idList=' +
					idList;
				if (btnType == 'btn_detail') url = './detail?id=' + query + '&jurisdictionType=' + btnType + '&menuIds=' +
					this.menuIds + '&idList=' + idList;
				uni.navigateTo({
					url: url
				})
			},
			search() {
				if (this.isPreview == '1') return
				this.searchTimer && clearTimeout(this.searchTimer)
				this.searchTimer = setTimeout(() => {
					this.list = [];
					this.mescroll.resetUpScroll();
				}, 300)
			},
			goDetail(id) {
				let btnType = 'btn_detail';
				if (!btnType) return
				this.jumPage(id, btnType)
			},
			getColumnList() {
				let columnPermissionList = []
				let _appColumnList = JSON.parse(JSON.stringify(this.appColumnList))
				this.$nextTick(() => {
					this.columnList = this.transformColumnList(_appColumnList, this.dataOptions)
				})
			},
			transformColumnList(columnList, dataOptions) {
				let list = []
				for (let i = 0; i < columnList.length; i++) {
					let e = columnList[i];
					let {
						prop: columProp,
						label
					} = e;
					let option = null;
					let options = `${columProp}Options`;
					if (!columProp.includes('-')) {
						option = dataOptions[options] || null;
						columProp = columProp
						if (label.length > 4) label = label.substring(0, 4);
						e.label = label;
						e.prop = columProp;
						e.option = option;
						list.push({
							...e
						});
					} else {
						e.vModel = columProp.split('-')[1]
						e.childLabel = e.label.split('-')[1]
						options = e.vModel + "Options"
						let prop = columProp.split('-')[0]
						let label = e.label.split('-')[0]
						let newItem = {
							align: "center",
							jnpfKey: "table",
							prop,
							label: e.fullNameI18nCode && e.fullNameI18nCode[0] ? this.$t(e.fullNameI18nCode[0]) : e
								.label.split('-')[0],
							children: []
						}
						if (!list.some(o => o.prop === prop)) list.push(newItem)
						for (let i = 0; i < list.length; i++) {
							if (list[i].prop === prop) {
								if (dataOptions[prop]) {
									option = dataOptions[prop][options]
								}
								e.option = option
								list[i].children.push(e)
								break
							}
						}
					}
				}
				return list
			},
			cellClick(item) {
				const findIndex = this.sortValue.findIndex(o => o === item.value);
				if (findIndex < 0) {
					const findLikeIndex = this.sortValue.findIndex(o => o.indexOf(item.sidx) > -1);
					if (findLikeIndex > -1) this.sortValue.splice(findLikeIndex, 1)
					this.sortValue.push(item.value)
				} else {
					this.sortValue.splice(findIndex, 1)
				}
			},
			handleSortReset() {
				this.sortValue = []
			},
			handleSortSearch() {
				if (this.sortValue.length) {
					this.listQuery.sidx = this.sortValue.join(',')
				} else {
					this.setDefaultQuery()
				}
				this.$refs.uDropdown.close();
				this.$nextTick(() => {
					this.list = [];
					this.mescroll.resetUpScroll();
				})
			},
			setDefaultQuery() {
				const defaultSortConfig = ([]).map(o =>
					(o.sort === 'desc' ? '-' : '') + o.field);
				this.listQuery.sidx = defaultSortConfig.join(',')
			},
			reset() {
				this.searchForm = {}
				this.$refs.searchForm.resetFields()
			},
			closeDropdown() {
				this.$refs.uDropdown.close();
				this.$nextTick(() => {
					this.list = [];
					this.mescroll.resetUpScroll();
				})
			},
			relationFormClick(item, column) {
				let vModel = column.vModel ? column.vModel + "_id" : column.__vModel__ + "_id"
				let id = item[vModel]
				let modelId = column.modelId
				if (!id || !modelId) return
				let config = {
					modelId: modelId,
					id: id,
					propsValue: column.propsValue,
					formTitle: this.$t('common.detailText'),
					noShowBtn: 1
				}
				this.$nextTick(() => {
					const url = '/pages/apply/dynamicModel/detail?config=' + this.jnpf.base64.encode(JSON
						.stringify(config), "UTF-8")
					uni.navigateTo({
						url: url
					})
				})
			},
			// 开票完成
			async handleInvoice(item) {
				this.btnLoading = true
				let {
					data
				} = await request({
					url: '/api/example/ACtcInvoice/' + item.id,
					method: 'GET'
				})
				let params = {
					...data,
					F_Status: "开票完成",
				}
				request({
					url: '/api/example/ACtcInvoice/' + item.id,
					method: 'PUT',
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
				}).catch(() => {
					this.btnLoading = false
				})
			},
			getDetail() {
				request({
					url: '/api/example/ACtcInvoice/2013783905592807424',
					method: 'get',
					data: params,
				}).then(res => {

				}).catch(() => {

				})
			}
		}
	}
</script>

<style lang="scss">
	page {
		background-color: #f0f2f6;
		height: 100%;
		/* #ifdef MP-ALIPAY */
		position: absolute;
		top: 0;
		left: 0;
		width: 100%;
		/* #endif */
	}

	.top-btn {
		height: 80rpx;
		position: fixed;
		width: 100%;
		top: 0;
		left: 0;
		background-color: #fff;
		/* #ifdef MP-WEIXIN */
		z-index: 99;
		/* #endif */
		/* #ifndef MP-WEIXIN */
		z-index: 999;
		/* #endif */
		justify-content: space-between;
		padding: 0 20rpx;

		.button-left {
			color: #0293fc;
		}

		.button-right {
			width: 30rpx;
			height: 30rpx;
		}
	}

	.slide-down2 {
		animation: slide-down2 0.5s forwards;
		opacity: 1;
		transform: translateY(0);
	}

	.slide-up2 {
		animation: slide-up2 0.5s forwards;
		opacity: 0;
		transform: translateY(-100%);
	}

	.slide-down {
		animation: slide-down 0.5s forwards;
		opacity: 1;
		transform: translateY(0);
	}

	.slide-up {
		animation: slide-up 0.5s forwards;
		opacity: 0;
		transform: translateY(100%);
	}

	.bottom-btn {
		height: 100rpx;
		position: fixed;
		width: 100%;
		bottom: 0;
		left: 0;
		background-color: #0293fc;
		z-index: 9;
		justify-content: space-around;

		.button-preIcon {
			color: #fff;
			text-align: center;
			width: 20%;

			.btn-icon {
				height: 32rpx;
			}
		}
	}

	@keyframes slide-up {
		to {
			transform: translateY(0);
			opacity: 1;
		}
	}

	@keyframes slide-down {
		to {
			transform: translateY(100%);
			opacity: 0;
		}
	}

	@keyframes slide-up2 {
		to {
			transform: translateY(0);
			opacity: 1;
		}
	}

	@keyframes slide-down2 {
		to {
			transform: translateY(-100%);
			opacity: 0;
		}
	}

	:deep(.u-cell) {
		padding: 0rpx;
		height: 112rpx;
	}

	.buttom-actions {
		z-index: 1;
	}

	.screen-box {
		background-color: #fff;
		height: 100%;

		.screen-btn {
			width: 100%;
			height: 2.75rem;

			.btn {
				width: 50%;
				height: 2.75rem;
				text-align: center;
				line-height: 2.75rem;
				box-shadow: 0px -4rpx 20rpx #F8F8F8
			}

			.btn1 {
				color: #606266;
			}

			.btn2 {
				background-color: #2979ff;
				color: #fff;
			}
		}

		.screen-list {
			width: 100%;
			/* #ifdef APP-HARMONY */
			height: calc(100vh - 82rpx);
			/* #endif */
			/* #ifndef APP-HARMONY */
			height: 100%;

			/* #endif */
			.list {
				height: calc(100% - 88rpx);
				overflow-y: scroll;
			}
		}
	}

	.notData-box {
		width: 100%;
		height: 100%;
		justify-content: center;
		align-items: center;
		padding-bottom: 200rpx;

		.notData-inner {
			width: 280rpx;
			height: 308rpx;
			align-items: center;

			.iconImg {
				width: 100%;
				height: 100%;
			}

			.notData-inner-text {
				padding: 30rpx 0;
				color: #909399;
			}
		}
	}

	.btn-container {
		display: flex;
		justify-content: space-between;
		height: 80rpx;
		margin-bottom: 20rpx;
	}
</style>