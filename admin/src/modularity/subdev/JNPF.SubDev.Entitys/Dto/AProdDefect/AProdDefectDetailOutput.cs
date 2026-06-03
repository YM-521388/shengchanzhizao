namespace JNPF.example.Entitys.Dto.AProdDefect;

/// <summary>
/// 不良品项详情输出参数.
/// </summary>
public class AProdDefectDetailOutput
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
    /// 创建人员.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

}