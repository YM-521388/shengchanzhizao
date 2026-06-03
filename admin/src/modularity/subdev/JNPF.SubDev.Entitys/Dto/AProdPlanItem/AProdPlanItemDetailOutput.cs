namespace JNPF.example.Entitys.Dto.AProdPlanItem;

/// <summary>
/// 商品详情输出参数.
/// </summary>
public class AProdPlanItemDetailOutput
{
    public string? id { get; set; }
    /// <summary>
    /// 生产计划ID.
    /// </summary>
    public string? F_ProdPlanId { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    public string? F_GoodsId { get; set; }
    public string? F_GoodsNo { get; set; }
    public string? F_Specification { get; set; }
    public string? F_Unit { get; set; }

    /// <summary>
    /// 计划数量.
    /// </summary>
    public int? F_Num { get; set; }

    /// <summary>
    /// 商品BOM.
    /// </summary>
    public string? F_BOMId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

}