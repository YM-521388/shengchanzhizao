namespace JNPF.example.Entitys.Dto.AProdTaskItem;

/// <summary>
/// 物料清单详情输出参数.
/// </summary>
public class AProdTaskItemDetailOutput
{
    /// <summary>
    /// 生产任务ID.
    /// </summary>
    public string? F_TaskId { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    public string? F_GoodsId { get; set; }
    public string? F_GoodsNo { get; set; }
    public string? F_Specification { get; set; }
    public string? F_UnitTwo { get; set; }

    /// <summary>
    /// 数量.
    /// </summary>
    public int? F_Num { get; set; }

    /// <summary>
    /// 描述.
    /// </summary>
    public string? F_Description { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

}