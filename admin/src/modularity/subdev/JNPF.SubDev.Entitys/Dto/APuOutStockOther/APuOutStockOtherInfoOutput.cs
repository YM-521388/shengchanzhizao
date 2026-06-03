using JNPF.example.Entitys.Dto.APuOutStockOtherItem;
using JNPF.example.Entitys.Dto.APuOutStockOtherLog;
 
namespace JNPF.example.Entitys.Dto.APuOutStockOther;

/// <summary>
/// a_pu_out_stock_other输出参数.
/// </summary>
public class APuOutStockOtherInfoOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 其他出库单号.
    /// </summary>
    public string? F_StockOutNo { get; set; }

    /// <summary>
    /// 仓库ID.
    /// </summary>
    public List<string> F_WarehouseId { get; set; }

    /// <summary>
    /// 出库类型.
    /// </summary>
    public string F_StockOutType { get; set; }

    /// <summary>
    /// 出库日期.
    /// </summary>
    public DateTime? F_StockOutDate { get; set; }

    /// <summary>
    /// 发货人.
    /// </summary>
    public string F_StockOutUserId { get; set; }

    /// <summary>
    /// 加工单号.
    /// </summary>
    public string? F_ProcessNo { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

    /// <summary>
    /// 其他出库单商品列表.
    /// </summary>
    public List<APuOutStockOtherItemInfoOutput> tableField44e07d { get; set; }

    /// <summary>
    /// 其他出库单号日志.
    /// </summary>
    public List<APuOutStockOtherLogInfoOutput> tableField211772 { get; set; }

}