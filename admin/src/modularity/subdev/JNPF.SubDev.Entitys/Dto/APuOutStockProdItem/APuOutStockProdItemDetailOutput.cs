namespace JNPF.example.Entitys.Dto.APuOutStockProdItem;

/// <summary>
/// 商品详情输出参数.
/// </summary>
public class APuOutStockProdItemDetailOutput
{
    /// <summary>
    /// 生产出库单ID.
    /// </summary>
    public string? F_StockOutProdId { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    public string? F_CustomerId { get; set; }

    /// <summary>
    /// 出库数量.
    /// </summary>
    public decimal? F_Num { get; set; }

    /// <summary>
    /// 成本单价(元).
    /// </summary>
    public decimal? F_Price { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    public string? F_Description { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    public string? F_CreatorUserId { get; set; }


    /// <summary>
    /// 储存仓库.
    /// </summary>
    public string? F_WarehouseID { get; set; }

    /// <summary>
    /// 商品编码.
    /// </summary>
    public string? F_Encoding { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

}