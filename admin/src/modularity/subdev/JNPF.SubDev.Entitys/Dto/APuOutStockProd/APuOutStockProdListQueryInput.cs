using JNPF.Common.Filter;

namespace JNPF.example.Entitys.Dto.APuOutStockProd;

/// <summary>
/// a_pu_out_stock_prod列表查询输入.
/// </summary>
public class APuOutStockProdListQueryInput : PageInputBase
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
    /// id.
    /// </summary>
    public string F_StockOutProdId { get; set; }

    /// <summary>
    /// 生产出库单号.
    /// </summary>
    public string F_StockOutNo { get; set; }

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
    public List<DateTime> F_StockOutDate { get; set; }

    /// <summary>
    /// 操作方式.
    /// </summary>
    public string F_Method { get; set; }

    /// <summary>
    /// 工单ID.
    /// </summary>
    public string F_WorkOrderId { get; set; }

}