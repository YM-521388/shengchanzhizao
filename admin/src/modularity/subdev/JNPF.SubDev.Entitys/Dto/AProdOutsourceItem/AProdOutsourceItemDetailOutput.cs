namespace JNPF.example.Entitys.Dto.AProdOutsourceItem;

/// <summary>
/// 用料清单详情输出参数.
/// </summary>
public class AProdOutsourceItemDetailOutput
{
    /// <summary>
    /// 外协工单ID.
    /// </summary>
    public string? F_OutsourceId { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    public string? F_GoodsId { get; set; }

    /// <summary>
    /// 单位用量.
    /// </summary>
    public int? F_Unit { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    public string? F_Description { get; set; }

    /// <summary>
    /// 创建用户.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

}