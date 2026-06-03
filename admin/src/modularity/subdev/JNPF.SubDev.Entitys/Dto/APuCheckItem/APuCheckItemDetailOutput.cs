namespace JNPF.example.Entitys.Dto.APuCheckItem;

/// <summary>
/// 选择库存商品详情输出参数.
/// </summary>
public class APuCheckItemDetailOutput
{
    /// <summary>
    /// 盘点ID.
    /// </summary>
    public string? F_CheckId { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    public string? F_CustomerId { get; set; }

    /// <summary>
    /// 盘点前数量.
    /// </summary>
    public decimal? F_CheckQtyBefore { get; set; }

    /// <summary>
    /// 已盘点数量.
    /// </summary>
    public decimal? F_CheckQtyDone { get; set; }

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