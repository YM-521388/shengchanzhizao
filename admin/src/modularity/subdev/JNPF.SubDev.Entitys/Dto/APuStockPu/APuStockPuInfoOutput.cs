using JNPF.example.Entitys.Dto.APuStockPuItem;
using JNPF.example.Entitys.Dto.APuStockPuLog;
 
namespace JNPF.example.Entitys.Dto.APuStockPu;

/// <summary>
/// a_pu_stock_pu输出参数.
/// </summary>
public class APuStockPuInfoOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 采购入库单号.
    /// </summary>
    public string? F_StockInNo { get; set; }

    /// <summary>
    /// 入库仓库ID.
    /// </summary>
    //public string F_WarehouseId { get; set; }
    public List<string> F_WarehouseId { get; set; }

    /// <summary>
    /// 采购单id.
    /// </summary>
    public string F_pu_Orderld { get; set; }

    /// <summary>
    /// 入库类型.
    /// </summary>
    public string F_StockInType { get; set; }

    /// <summary>
    /// 入库日期.
    /// </summary>
    public DateTime? F_StockInDate { get; set; }

    /// <summary>
    /// 收货人.
    /// </summary>
    public string F_StockInUserId { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    public string? F_Description { get; set; }

    /// <summary>
    /// 采购入库单商品列表.
    /// </summary>
    public List<APuStockPuItemInfoOutput> tableField4bd139 { get; set; }

    /// <summary>
    /// 采购入库单日志.
    /// </summary>
    public List<APuStockPuLogInfoOutput> tableFieldca527d { get; set; }

}