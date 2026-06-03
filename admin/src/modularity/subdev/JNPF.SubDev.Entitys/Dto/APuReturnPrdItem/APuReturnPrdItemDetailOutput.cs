namespace JNPF.example.Entitys.Dto.APuReturnPrdItem;

/// <summary>
/// 商品详情输出参数.
/// </summary>
public class APuReturnPrdItemDetailOutput
{
    /// <summary>
    /// 生产退料单ID.
    /// </summary>
    public string? F_ReturnInPRDId { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    public string? F_CustomerId { get; set; }

    /// <summary>
    /// 退料数量.
    /// </summary>
    public int? F_Num { get; set; }

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
    /// 入库仓库Id.
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