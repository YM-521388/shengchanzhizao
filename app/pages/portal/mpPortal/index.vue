<template>
	<view class="menhu-v">
		<uni-nav-bar class='nav' :fixed="true" :statusBar="true" :border="false">
			<!-- 左边插槽 -->
			<template #left>
				<view class="">
					<uni-icons class='icon-ym icon-ym-app-role-toggle' color="#222222" size="20"
						@click="showSelectBox(0)" />
				</view>
			</template>
			<template #default>
				<view class="nav-left">
					<view class="nav-left-text">
						ERP
					</view>
				</view>
			</template>
		</uni-nav-bar>
		<view class="content">
			<view v-if="userInfo.appPortalId">
				<view class="portal-select">
					<view class="u-flex portal-select-inner" @click.stop="showSelectBox(1)">
						<text class="portal-select-text u-line-1">{{portalTitle}}</text>
						<uni-icons class='right-icons' type="down" color="#000000" size="14"
							v-if="portalList.length > 0 && userInfo.appPortalId"
							:class="{'select-right-icons':showSelect && type ==1}" />
					</view>
				</view>
				<mescroll-body ref="mescrollRef" @init="mescrollInit" @down="downCallback" :down="downOption"
					@up="upCallback" :up="upOption" :bottombar="false" top='10'>
					<view class="portal-v" v-if="authConfig.type==0">
						<view v-if="formData.length">
							<view class="portal-box" v-for="(item,index) in formData" :key="index">
								<portalItem :item='item' ref="portalItem" :key="key" v-if="item.show" />
							</view>
						</view>
						<view v-else class="portal-v portal-nodata">
							<view class="u-flex-col" style="align-items: center;">
								<u-image width="280rpx" height="280rpx" :src="emptyImg1"></u-image>
								<text class="u-m-t-20" style="color: #909399;">{{$t('common.noData')}}</text>
							</view>
						</view>
					</view>
					<view v-if="authConfig.type==1">
						<view v-if="authConfig.linkType==1" style="height:calc(100vh - 100px)">
							<web-view :src="authConfig.customUrl" v-if="showWebView"></web-view>
						</view>
						<view v-else class="portal-v portal-nodata">
							<view class="u-flex-col" style="align-items: center;">
								<u-image width="280rpx" height="280rpx" :src="emptyImg"></u-image>
								<text class="u-m-t-20" style="color: #909399;">当前内容无法在APP上显示，请前往PC门户查看～～</text>
							</view>
						</view>
					</view>
				</mescroll-body>
			</view>
			<view class="portal-v" v-else>
				<view class="portal-box">
					<defaultPortal></defaultPortal>
				</view>
			</view>
		</view>



		<!-- <u-popup v-model="showSelect" mode="top" class="select-box" height="600rpx">
			<view :style="{'margin-top':statusBarHeight+88+'rpx'}"></view>
			<view v-for="(item,index) in portalList" :key="index" class="select-item" @click="selectItem(item,index)">
				<text class="u-m-r-12 u-font-40"
					:class="[item.icon,{'currentItem':item.id === userInfo.appPortalId}]" />
				<text class="item-text sysName">{{item.fullName}}</text>
				<u-icon name="checkbox-mark " class="currentItem" v-if="item.id === userInfo.appPortalId"></u-icon>
			</view>
		</u-popup> -->
		<u-popup v-model="showSelect" mode="bottom" class="select-box" height="600rpx">
			<view class="search-box" v-if="type == 1">
				<u-search :placeholder="$t('app.apply.pleaseKeyword')" v-model="keyword" height="72"
					:show-action="false" bg-color="#f0f2f6" shape="square" search-icon-color="#909399" />
			</view>
			<view v-for="(item,index) in portalList" :key="index" class="select-item" @click="selectItem(item,index)">
				<text class="u-m-r-12 u-font-30"
					:class="[item.icon,{'currentItem':item.isDefault || item.id === item.appPortalId }]" />
				<text class="item-text sysName"
					:class="{'currentItem':item.isDefault || item.id === item.appPortalId}">{{item.fullName}}</text>
				<u-icon name="checkbox-mark " class="currentItem"
					v-if="item.isDefault || item.id === item.appPortalId"></u-icon>
			</view>
		</u-popup>
	</view>
</template>
<script>
	var wv; //计划创建的webview
	import {
		PortalList,
		SetPortal,
		auth
	} from '@/api/portal/portal.js'
	import {
		getUserOrganizes
	} from '@/api/common.js'
	import MescrollMixin from "@/uni_modules/mescroll-uni/components/mescroll-uni/mescroll-mixins.js"
	import IndexMixin from '@/pages/index/mixin.js'
	import chat from '@/libs/chat.js'
	import portalItem from '@/pages/portal/components/index.vue'
	import defaultPortal from '@/pages/portal/components/defaultPortal.vue'
	import resources from '@/libs/resources.js'
	import emptyImg from '@/static/image/defPortal.png'
	import {
		useChatStore
	} from '@/store/modules/chat'
	import {
		useUserStore
	} from '@/store/modules/user'

	export default {
		mixins: [MescrollMixin, IndexMixin],
		components: {
			portalItem,
			defaultPortal
		},
		data() {
			return {
				showWebView: true,
				emptyImg: emptyImg,
				emptyImg1: resources.message.nodata,
				rightIcon: '',
				key: +new Date(),
				formData: [],
				portalTitle: '门户',
				statusBarHeight: '',
				showSelect: false,
				selectData: {
					name: '',
					id: ''
				},
				portalList: [],
				id: '',
				userInfo: {},
				downOption: {
					use: true,
					auto: true
				},
				upOption: {
					page: {
						num: 0,
						size: 50,
						time: null
					},
					empty: {
						use: false,
					},
					textNoMore: this.$t('app.apply.noMoreData'),
				},
				authConfig: {},
				token: '',
				type: 0,
			};
		},
		computed: {
			isPortalListValid() {
				return Array.isArray(this.portalList) && this.portalList.length;
			},
			columnList() {
				return this.portalList.filter((o) => (o.fullName && o.fullName.match(this.keyword)))
			}
		},
		onShow() {
			this.$forceUpdate()
		},
		onLoad(e) {
			uni.setStorageSync('token', e.token)
			this.$nextTick(() => {
				this.getCurrentUser()
			})
			const chatStore = useChatStore()
			if (!chatStore.getSocket) chat && chat.initSocket()
			uni.$on('refresh', () => {
				this.formData = [];
				this.mescroll.resetUpScroll();
			})
		},
		methods: {
			getCurrentUser() {
				const userStore = useUserStore()
				userStore.getCurrentUser().then(() => {
					this.userInfo = uni.getStorageSync('userInfo') || {}
					if (!this.userInfo.appPortalId) return
					this.getPortalList()
				})
			},
			upCallback(keyword) {
				auth(this.userInfo.appPortalId).then(res => {
					this.authConfig = res.data || {}
					let data = JSON.parse(res.data.formData) || {};
					this.formData = []
					this.formData = data.layout ? JSON.parse(JSON.stringify(data.layout)) : []
					this.handelFormData(data)
					if (data.refresh.autoRefresh) {
						setInterval(() => {
							uni.$emit('proRefresh')
						}, data.refresh.autoRefreshTime * 60000)
					}
					this.mescroll.endSuccess(this.formData.length);
					this.key = +new Date()
				}).catch(() => {
					this.mescroll.endSuccess(0);
					this.mescroll.endErr();
					this.key = +new Date()
				})
			},
			handelFormData(data) {
				const loop = (list) => {
					list.forEach(o => {
						o.allRefresh = data.refresh
						o.show = false
						o.platform = 'mp'
						if (o.visibility && o.visibility.length && o.visibility.includes('app')) o.show =
							true
						if (o.children && o.children.length) loop(o.children)
					})
					this.key = +new Date()
				}
				loop(this.formData)
				this.dataList = this.formData.filter(o => o.show)
				if (this.dataList.length < 1) {
					this.formData = this.dataList
					this.mescroll.endSuccess(this.dataList.length);
				}
			},
			getPortalList() {
				PortalList().then(res => {
					let list = res.data.list || [];
					this.portalList = []
					list.map(o => {
						o.children && this.portalList.push(...o.children)
						this.portalList.forEach(o => {
							o.appPortalId = this.userInfo.appPortalId
							if (o.id === o.appPortalId) this.portalTitle = o.fullName
						})
					})
				})
			},
			showSelectBox(type) {
				this.type = type;
				if (type === 0) {
					getUserOrganizes().then(res => {
						this.portalList = res.data;
						this.portalList.forEach(o => {
							o.icon = 'icon-ym icon-ym-flow-node-condition';
						});
						if (this.isPortalListValid) this.showSelect = !this.showSelect;
					});
				} else {
					if (this.isPortalListValid) this.showSelect = !this.showSelect;

					this.getPortalList();
				}
			},
			getStatusBarHeight() {
				let that = this;
				wx.getSystemInfo({
					success: function(res) {
						that.statusBarHeight = res.statusBarHeight;
					},
				});
			},
			selectItem(item, index) {
				SetPortal(item.id).then(res => {
					this.portalTitle = this.portalList[index].fullName
					this.userInfo.appPortalId = item.id
					this.mescroll.resetUpScroll();
					this.showSelectBox()
					uni.setStorageSync('userInfo', this.userInfo)
				})
			}
		}
	}
</script>

<style lang="scss">
	page {
		background-color: #f0f2f6;
	}

	.menhu-v {
		.portal-v {
			padding: 0 20rpx
		}

		.nav {
			z-index: 99999;

			:deep(.uni-navbar__content) {
				z-index: 99999;
			}

			:deep(.uni-navbar__header-container) {
				justify-content: center;
			}

			.nav-left {
				max-width: 100%;
				display: flex;
				align-items: center;

				.nav-left-text {
					font-weight: 700;
					font-size: 32rpx;
					flex: 1;
					min-width: 0;
					white-space: nowrap;
					overflow: hidden;
					text-overflow: ellipsis;
				}

				.right-icons {
					font-weight: 700;
					margin-top: 2px;
					margin-left: 4px;
					transition-duration: 0.3s;
				}

				.select-right-icons {
					transform: rotate(-180deg);
				}
			}
		}

		.portal-select {
			background-color: #fff;
			height: 80rpx;
			padding-left: 20rpx;
			line-height: 80rpx;

			.portal-select-inner {
				width: 200rpx;
				height: 100%;

				.portal-select-text {
					color: #303133;
				}
			}

			.right-icons {
				font-weight: 700;
				margin-top: 2px;
				margin-left: 4px;
				transition-duration: 0.3s;
				color: #606266 !important;
			}

			.select-right-icons {
				transform: rotate(-180deg);
			}
		}

		.select-box {
			overflow-y: scroll;

			.currentItem {
				color: #2979FF;
			}

			.select-item {
				height: 100rpx;
				display: flex;
				align-items: center;
				padding: 0 20rpx;
				font-size: 30rpx;
				color: #303133;
				text-align: left;
				position: relative;

				&::after {
					content: " ";
					position: absolute;
					left: 2%;
					top: 0;
					box-sizing: border-box;
					width: 96%;
					height: 1px;
					transform: scale(1, .3);
					border: 0 solid #e4e7ed;
					z-index: 2;
					border-bottom-width: 1px;
				}

				.sysName {
					flex: 1;
					overflow: auto;
					min-width: 0;
				}
			}
		}
	}

	:deep(.portal-nodata) {
		position: absolute;
		top: 450rpx;
		width: 100%;
		text-align: center;
		z-index: 100;
		background-color: #f0f2f6;
	}
</style>