import request from '@/utils/request'
// 根据采购单ID获取采购单商品列表
export function getOrderItems(orderId) {
	return request({
		url: `/api/example/APuStockPu/Actions/GetOrderItems?orderId=${orderId}`,
		method: 'get',
		options: {
			load: false
		}
	})
}

// 根据仓库 id 获取库存商品的商品 id 与数量
export function getInventoryByWarehouse(warehouseId) {
	return request({
		url: `/api/example/APuOutStockSo/InventoryByWarehouse/${warehouseId}`,
		method: 'get',
		options: {
			load: false
		}
	})
}
// 通过商品获取单位
export function getGoodsUnit(id) {
	return request({
		url: `/api/example/AGoods/GoodsUnit/${id}`,
		method: 'post',
		options: {
			load: false
		}
	})
}
// 根据BOM获取用料清单
export function getBOMGoodsList(bomId) {
	return request({
		url: `/api/example/ABom/BOMGoodsList/${bomId}`,
		method: 'post',
		options: {
			load: false
		}
	})
}
// 根据客户ID获取默认联系人和地址
export function getContractDefaultsByCustomer(customerId) {
	return request({
		url: `/api/example/ACtcQuote/Defaults/Customer/${customerId}`,
		method: 'get',
		options: {
			load: false
		}
	})
}