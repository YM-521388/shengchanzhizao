using SqlSugar;

namespace JNPF.example.Entitys.Dto.APuOutStockOther;

/// <summary>
/// a_pu_out_stock_other输入参数.
/// </summary>
public class APuOutStockOtherListOutput
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
    public string? F_WarehouseId { get; set; }

    /// <summary>
    /// 出库类型.
    /// </summary>
    public string? F_StockOutType { get; set; }

    /// <summary>
    /// 出库日期.
    /// </summary>
    public string F_StockOutDate { get; set; }

    /// <summary>
    /// 发货人.
    /// </summary>
    public string? F_StockOutUserId { get; set; }

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
    /// 创建组织.
    /// </summary>
    public string? F_CreatorOrganizeId { get; set; }

}