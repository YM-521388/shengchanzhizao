using JNPF.Common.Filter;

namespace JNPF.example.Entitys.Dto.APuOutStockSo;

/// <summary>
/// a_pu_out_stock_so列表查询输入.
/// </summary>
public class APuOutStockSoListQueryInput : PageInputBase
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
    /// 销售出库单号.
    /// </summary>
    public string F_StockOutNo { get; set; }

    /// <summary>
    /// 仓库ID.
    /// </summary>
    public List<string> F_WarehouseId { get; set; }

    /// <summary>
    /// 出库类型.
    /// </summary>
    public object F_StockOutType { get; set; }

    /// <summary>
    /// 出库日期.
    /// </summary>
    public List<DateTime> F_StockOutDate { get; set; }

    /// <summary>
    /// 发货人.
    /// </summary>
    public object F_StockOutUserId { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    public string F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public List<DateTime> F_CreatorTime { get; set; }

}