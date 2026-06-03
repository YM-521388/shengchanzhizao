namespace JNPF.example.Entitys.Dto.APuTransferItem;
 
/// <summary>
/// 选择商品输出参数.
/// </summary>
public class APuTransferItemInfoOutput
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
    public string F_GoodsName { get; set; }
    public string F_GoodsNo { get; set; }
    public string F_UnitTwo { get; set; }

    /// <summary>
    /// 库存.
    /// </summary>
    public int? F_InventoryNum { get; set; }

    /// <summary>
    /// 数量.
    /// </summary>
    public decimal? F_Num { get; set; }

    /// <summary>
    /// 成本单价(元).
    /// </summary>
    public decimal? F_Price { get; set; }

    /// <summary>
    /// 销售单价(元).
    /// </summary>
    public decimal? F_SalesPrice { get; set; }

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

}