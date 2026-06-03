namespace JNPF.example.Entitys.Dto.AProdProjectStep;

/// <summary>
/// 项目商品工序信息详情输出参数.
/// </summary>
public class AProdProjectStepDetailOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 项目商品ID.
    /// </summary>
    public string? F_ProjcetItemId { get; set; }

    /// <summary>
    /// 工序ID.
    /// </summary>
    public string? F_ProcessId { get; set; }

    /// <summary>
    /// 计划开始.
    /// </summary>
    public string F_StartDate { get; set; }

    /// <summary>
    /// 计划结束.
    /// </summary>
    public string F_EndDate { get; set; }

    /// <summary>
    /// 排序.
    /// </summary>
    public string F_SortCode { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    public string? F_Description { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

}