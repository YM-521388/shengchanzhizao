namespace JNPF.example.Entitys.Dto.APuOutStockSoItem;
 
/// <summary>
/// 商品列表输出参数.
/// </summary>
public class APuOutStockSoItemInfoOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? F_Id { get; set; }

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
    /// 库存编码.
    /// </summary>
    public string? F_Code { get; set; }

    /// <summary>
    /// 销售单价(元).
    /// </summary>
    public decimal? F_SalesPrice { get; set; }

    /// <summary>
    /// 类别.
    /// </summary>
    public string? F_Type { get; set; }

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