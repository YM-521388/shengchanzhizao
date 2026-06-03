namespace JNPF.example.Entitys.Dto.AProdPlanItem;
 
/// <summary>
/// 商品输出参数.
/// </summary>
public class AProdPlanItemInfoOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? F_Id { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    public string F_GoodsId { get; set; }
    public string? F_GoodsNo { get; set; }
    public string? F_Specification { get; set; }
    public string? F_Unit { get; set; }

    /// <summary>
    /// 计划数量.
    /// </summary>
    public int? F_Num { get; set; }
    public string? F_NumUnit { get; set; }

    /// <summary>
    /// 商品BOM.
    /// </summary>
    public string F_BOMId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

}