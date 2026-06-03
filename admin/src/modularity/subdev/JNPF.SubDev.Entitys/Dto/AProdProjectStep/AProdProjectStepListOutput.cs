using JNPF.example.Entitys.Dto.AProdProjectStepItem;

namespace JNPF.example.Entitys.Dto.AProdProjectStep;

/// <summary>
/// 项目商品工序信息输入参数.
/// </summary>
public class AProdProjectStepListOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 项目商品ID.
    /// </summary>
    public string? F_ProjectItemId { get; set; }

    /// <summary>
    /// 工序ID.
    /// </summary>
    public string? F_ProcessId { get; set; }

    /// <summary>
    /// 计划开始.
    /// </summary>
    public DateTime? F_StartDate { get; set; }

    /// <summary>
    /// 计划结束.
    /// </summary>
    public DateTime? F_EndDate { get; set; }

    /// <summary>
    /// 排序.
    /// </summary>
    public int? F_SortCode { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    public string? F_Description { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

    /// <summary>
    /// 项目商品工序物料清单列表.
    /// </summary>
    public List<AProdProjectStepItemListOutput> tableField3b6f08 { get; set; }

}