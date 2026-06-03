namespace JNPF.example.Entitys.Dto.APuReturnSoItem;

/// <summary>
/// 商品输入参数.
/// </summary>
public class APuReturnSoItemListOutput
{
    /// <summary>
    /// 销售退货单ID.
    /// </summary>
    public string? F_ReturnInSOId { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    public string? F_CustomerId { get; set; }

    /// <summary>
    /// 退货数量.
    /// </summary>
    public decimal? F_Num { get; set; }

    /// <summary>
    /// 销售单价(元).
    /// </summary>
    public decimal? F_Price { get; set; }

    /// <summary>
    /// 入库仓库id.
    /// </summary>
    public string? F_WarehouseID { get; set; }

    /// <summary>
    /// 商品编码.
    /// </summary>
    public string? F_Encoding { get; set; }

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