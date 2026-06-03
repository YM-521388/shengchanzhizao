/* process.env.NODE_ENV设置生产环境模式 */
// #ifndef MP
// const baseURL = process.env.NODE_ENV === "production" ? "https://app.java.jnpfsoft.com" : "http://183.220.98.191:8098"
// const baseURL = "http://183.220.98.191:8098"
// const webSocketUrl = process.env.NODE_ENV === "production" ? "wss://app.java.jnpfsoft.com/websocket" :
// 	"ws://183.220.98.191:8094/websocket"
// const report = process.env.NODE_ENV === 'production' ? 'https://java.jnpfsoft.com/Report' : 'http://localhost:8200'
// const flow = process.env.NODE_ENV === 'production' ? 'https://java.jnpfsoft.com' : 'http://localhost:3100'

const baseURL = "http://127.0.0.1:5000"
const webSocketUrl = "ws://127.0.0.1:5000/api/message/websocket"
const report = baseURL + '/Report'
const flow = 'http://127.0.0.1:5000'


// #endif

// #ifdef MP
// const baseURL = "http://183.220.98.191:8098"
// const webSocketUrl = "ws://183.220.98.191:8094/websocket"
// const report = 'http://localhost:8200'
// const flow = 'http://localhost:3100'


const baseURL = "http://127.0.0.1:5000"
const webSocketUrl = "ws://127.0.0.1:5000/api/message/websocket"
const report = baseURL + '/Report'
const flow = 'http://127.0.0.1:5000'


// #endif

const define = {
	copyright: "Copyright @ 2026 绵阳凌翔机械制造有限公司",
	sysVersion: "V1.1.0",
	baseURL, // 接口前缀
	report,
	flow,
	webSocketUrl,
	comUploadUrl: baseURL + '/api/file/Uploader/',
	timeout: 1000000,
	aMapWebKey: '09485f01587712b3c04e5a9abf324237',
	cipherKey: 'EY8WePvjM5GGwQzn', // 加密key
}
export default define