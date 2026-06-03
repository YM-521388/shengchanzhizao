namespace JNPF.example.Entitys.Dto.APuStockOtherItem;

/// <summary>
/// 入库商品列表详情输出参数.
/// </summary>
public class APuStockOtherItemDetailOutput
{
    /// <summary>
    /// 其他入库单ID.
    /// </summary>
    public string? F_StockInOTId { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    public string? F_CustomerId { get; set; }

    /// <summary>
    /// 数量.
    /// </summary>
    public int? F_Num { get; set; }
    public string? F_NumInfo { get; set; }

    /// <summary>
    /// 成本单价(元).
    /// </summary>
    public decimal? F_Price { get; set; }

    /// <summary>
    /// 销售单价(元).
    /// </summary>
    public decimal? F_Sales_Price { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    public string? F_Description { get; set; }


    /// <summary>
    /// 入库仓库id.
    /// </summary>
    public string? F_WarehouseID { get; set; }

    /// <summary>
    /// 商品编码.
    /// </summary>
    public string? F_Encoding { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

}