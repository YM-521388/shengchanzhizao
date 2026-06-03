using SqlSugar;

namespace JNPF.example.Entitys.Dto.AProdDefect;

/// <summary>
/// 不良品项输入参数.
/// </summary>
public class AProdDefectListOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 不良品项编号.
    /// </summary>
    public string? F_DefectCode { get; set; }

    /// <summary>
    /// 不良品项名称.
    /// </summary>
    public string? F_DefectName { get; set; }

    /// <summary>
    /// 总数量.
    /// </summary>
    public int? F_AllNum { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

    /// <summary>
    /// 创建组织.
    /// </summary>
    public string? F_CreatorOrganizeId { get; set; }

}