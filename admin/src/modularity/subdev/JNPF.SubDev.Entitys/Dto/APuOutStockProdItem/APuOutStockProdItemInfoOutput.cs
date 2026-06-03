namespace JNPF.example.Entitys.Dto.APuOutStockProdItem;
 
/// <summary>
/// 商品输出参数.
/// </summary>
public class APuOutStockProdItemInfoOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? F_Id { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    public string? F_CustomerId { get; set; }
    public string? F_Specification { get; set; }
    public string? F_GoodsName { get; set; }
    public string? F_GoodsNo { get; set; }
    public string? F_UnitTwo { get; set; }

    /// <summary>
    /// 库存.
    /// </summary>
    public decimal? F_InventoryNum { get; set; }

    /// <summary>
    /// 出库数量.
    /// </summary>
    public decimal? F_Num { get; set; }
    public string? F_NumInfo { get; set; }

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
    public List<string>? F_WarehouseID { get; set; }
    public string? F_WarehouseId { get; set; }

    /// <summary>
    /// 库存编码.
    /// </summary>
    public string? F_Encoding { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string? F_CreatorTime { get; set; }

}