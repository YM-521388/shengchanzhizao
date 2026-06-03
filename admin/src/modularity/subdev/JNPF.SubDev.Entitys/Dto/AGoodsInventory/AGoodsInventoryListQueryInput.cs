using JNPF.Common.Filter;

namespace JNPF.example.Entitys.Dto.AGoodsInventory;

/// <summary>
/// 商品库存列表查询输入.
/// </summary>
public class AGoodsInventoryListQueryInput : PageInputBase
{
    /// <summary>
    /// 选择导出数据ids.
    /// </summary>
    public string selectIds { get; set; }

    /// <summary>
    /// 选择导出数据key.
    /// </summary>
    public string selectKey { get; set; }

    /// <summary>
    /// 导出类型.
    /// </summary>
    public int dataType { get; set; }
    
    /// <summary>
    /// 关键词查询.
    /// </summary>
    public string jnpfKeyword { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    public string F_GoodsId { get; set; }

    /// <summary>
    /// 仓库ID.
    /// </summary>
    //public List<string> F_WarehouseId { get; set; }
    public string F_WarehouseId { get; set; }

    /// <summary>
    /// 库存数量.
    /// </summary>
    public List<decimal?> F_Num { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public List<DateTime> F_CreatorTime { get; set; }

}