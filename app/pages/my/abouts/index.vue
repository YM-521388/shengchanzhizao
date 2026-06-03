<template>
	<view class="abouts-v">
		<view class="head-box u-flex-col">
			<view class="head-inner">
				<view class="version">
					{{sysVersion}}
				</view>
				<image src="/static/image/jnpf.png" mode="widthFix" class="head-img"></image>
			</view>
		</view>

		<view class="content u-p-l-32 u-p-r-32 u-p-t-30 u-font-28">
			<text></text>
		</view>
		<view class="copyright">{{copyright}}</view>
	</view>
</template>

<script>
	import resources from '@/libs/resources.js'
	export default {
		data() {
			return {
				logoSrc: resources.banner.loginlogo,
				copyright: 'Copyright © 2026 绵阳凌翔机械制造有限公司出品',
				sysVersion: ''
			}
		},
		onLoad() {
			this.sysVersion = uni.getStorageSync('sysVersion') || this.define.sysVersion
			this.copyright = uni.getStorageSync('copyright') || 'Copyright © 2024 绵阳凌翔机械制造有限公司出品'
		},

	}
</script>

<style lang="scss">
	.abouts-v {
		.head-box {
			height: 308rpx;
			background: url('@/pages/my/static/image/about-head.png') center no-repeat;
			background-size: 100% 100%;
			align-items: center;
			justify-content: center;

			.head-inner {
				position: relative;

				.head-img {
					width: 212rpx;
					height: 60rpx;
					margin-top: 20rpx;
				}

				.version {
					position: absolute;
					background-color: #FFFFFF;
					color: #0F5BD2;
					padding: 0 8rpx;
					border-radius: 20rpx 0rpx 20rpx 0rpx;
					top: -34rpx;
					left: 218rpx;
				}
			}
		}

		.abouts-hd {
			width: 100%;
			align-items: center;
			background-color: #3281ff;
			height: 280rpx;
			color: #FFFFFF;
			padding-top: 20rpx;

			image {
				width: 160rpx;
				height: 160rpx;
			}
		}

		.copyright {
			height: 80rpx;
			bottom: 0;
			line-height: 80rpx;

		}

		.content {
			height: calc(100vh - 486rpx);
			overflow-y: scroll;
			line-height: 48rpx;
		}
	}
</style>