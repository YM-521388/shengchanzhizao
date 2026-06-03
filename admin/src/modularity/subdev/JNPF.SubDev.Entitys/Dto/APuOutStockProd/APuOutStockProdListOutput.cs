using SqlSugar;

namespace JNPF.example.Entitys.Dto.APuOutStockProd;

/// <summary>
/// a_pu_out_stock_prod输入参数.
/// </summary>
public class APuOutStockProdListOutput
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
    public string? F_WarehouseId { get; set; }

    /// <summary>
    /// 出库类型.
    /// </summary>
    public string? F_StockOutType { get; set; }
    public string? F_StockOutTypeCode { get; set; }

    /// <summary>
    /// 出库日期.
    /// </summary>
    public string F_StockOutDate { get; set; }

    /// <summary>
    /// 出库套数.
    /// </summary>
    public string F_Num { get; set; }

    /// <summary>
    /// 工单类型.
    /// </summary>
    public string? F_Type { get; set; }
    public string? F_TypeCode { get; set; }

    /// <summary>
    /// 操作方式.
    /// </summary>
    public string? F_Method { get; set; }

    /// <summary>
    /// 工单ID.
    /// </summary>
    public string? F_WorkOrderId { get; set; }

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
    /// 创建组织.
    /// </summary>
    public string? F_CreatorOrganizeId { get; set; }

}