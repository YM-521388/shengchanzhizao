namespace JNPF.example.Entitys.Dto.AProdAnalysis;

/// <summary>
/// 生产计划商品信息输出参数.
/// </summary>
public class AProdAnalysisGoodsOutput
{
    /// <summary>
    /// 商品编号.
    /// </summary>
    public string? F_GoodsNo { get; set; }

    /// <summary>
    /// 商品名称.
    /// </summary>
    public string? F_GoodsName { get; set; }

    /// <summary>
    /// 商品来源.
    /// </summary>
    public string? F_Source { get; set; }
}
