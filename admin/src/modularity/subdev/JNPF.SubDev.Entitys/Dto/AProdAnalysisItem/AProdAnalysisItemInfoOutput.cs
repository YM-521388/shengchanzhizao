namespace JNPF.example.Entitys.Dto.AProdAnalysisItem;
 
/// <summary>
/// 物料分析商品列表信息输出参数.
/// </summary>
public class AProdAnalysisItemInfoOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? F_Id { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    public string? F_GoodsId { get; set; }

    /// <summary>
    /// 商品上级ID.
    /// </summary>
    public string? F_ParentId { get; set; }

    /// <summary>
    /// 单位用量.
    /// </summary>
    public int? F_Unit { get; set; }

    /// <summary>
    /// 需求.
    /// </summary>
    public int? F_DemandQty { get; set; }

    /// <summary>
    /// 可用库存.
    /// </summary>
    public int? F_AvailableStock { get; set; }

    /// <summary>
    /// 在制库存.
    /// </summary>
    public int? F_WipStock { get; set; }

    /// <summary>
    /// 在途库存.
    /// </summary>
    public int? F_TransitStock { get; set; }

    /// <summary>
    /// 应自产.
    /// </summary>
    public int? F_ShouldSelfMake { get; set; }

    /// <summary>
    /// 应外协.
    /// </summary>
    public int? F_ShouldOutsource { get; set; }

    /// <summary>
    /// 应采购.
    /// </summary>
    public int? F_ShouldPurchase { get; set; }

    /// <summary>
    /// 创建用户.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

}