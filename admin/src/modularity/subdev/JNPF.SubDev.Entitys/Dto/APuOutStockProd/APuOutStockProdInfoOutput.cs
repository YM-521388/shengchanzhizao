using JNPF.example.Entitys.Dto.APuOutStockProdItem;
using JNPF.example.Entitys.Dto.APuOutStockProdLog;
 
namespace JNPF.example.Entitys.Dto.APuOutStockProd;

/// <summary>
/// a_pu_out_stock_prod输出参数.
/// </summary>
public class APuOutStockProdInfoOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 生产出库单号.
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
    /// 出库套数.
    /// </summary>
    public int? F_Num { get; set; }

    /// <summary>
    /// 工单类型.
    /// </summary>
    public string? F_Type { get; set; }

    /// <summary>
    /// 操作方式.
    /// </summary>
    public string? F_Method { get; set; }

    /// <summary>
    /// 工单ID.
    /// </summary>
    public string F_WorkOrderId { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    public string? F_Description { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

    /// <summary>
    /// 生产出库单商品列表.
    /// </summary>
    public List<APuOutStockProdItemInfoOutput> tableField2752cf { get; set; }

    /// <summary>
    /// 生产出库单号日志.
    /// </summary>
    public List<APuOutStockProdLogInfoOutput> tableField49228c { get; set; }

}