using JNPF.Common.Filter;

namespace JNPF.example.Entitys.Dto.APuStockOther;

/// <summary>
/// a_pu_stock_other列表查询输入.
/// </summary>
public class APuStockOtherListQueryInput : PageInputBase
{
    /// <summary>
    /// 高级查询.
    /// </summary>
    public string superQueryJson { get; set; }

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
    /// 其他入库单号.
    /// </summary>
    public string F_StockInNo { get; set; }

    /// <summary>
    /// 入库仓库ID.
    /// </summary>
    public List<string> F_WarehouseId { get; set; }

    /// <summary>
    /// 入库类型.
    /// </summary>
    public object F_StockInType { get; set; }

    /// <summary>
    /// 入库日期.
    /// </summary>
    public List<DateTime> F_StockInDate { get; set; }

}