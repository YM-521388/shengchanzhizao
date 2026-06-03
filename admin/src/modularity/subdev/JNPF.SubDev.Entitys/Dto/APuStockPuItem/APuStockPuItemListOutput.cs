namespace JNPF.example.Entitys.Dto.APuStockPuItem;

/// <summary>
/// 采购入库商品输入参数.
/// </summary>
public class APuStockPuItemListOutput
{
    /// <summary>
    /// 采购入库单ID.
    /// </summary>
    public string? F_StockInPUId { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    public string? F_CustomerId { get; set; }

    /// <summary>
    /// 数量.
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
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }



    /// <summary>
    /// 入库仓库ID.
    /// </summary>
    public string? F_WarehouseID { get; set; }

    /// <summary>
    /// 商品编码.
    /// </summary>
    public string? F_Encoding { get; set; }



}