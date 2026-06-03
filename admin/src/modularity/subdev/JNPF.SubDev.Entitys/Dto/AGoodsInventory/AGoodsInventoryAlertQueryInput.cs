using JNPF.Common.Filter;

namespace JNPF.example.Entitys.Dto.AGoodsInventory;

/// <summary>
/// 商品库存阈值告警查询输入.
/// </summary>
public class AGoodsInventoryAlertQueryInput : PageInputBase
{
    /// <summary>
/// 告警类型(字典enCode筛选，为空则全部).
/// </summary>
public string alertType { get; set; }

/// <summary>
/// 商品名称(模糊查询).
/// </summary>
public string F_GoodsName { get; set; }
}
