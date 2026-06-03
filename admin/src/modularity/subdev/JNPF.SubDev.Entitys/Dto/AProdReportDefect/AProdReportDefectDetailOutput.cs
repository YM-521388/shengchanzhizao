namespace JNPF.example.Entitys.Dto.AProdReportDefect;

/// <summary>
/// 不良品项详情输出参数.
/// </summary>
public class AProdReportDefectDetailOutput
{
    /// <summary>
    /// 报工ID.
    /// </summary>
    public string? F_ReportId { get; set; }

    /// <summary>
    /// 不良品项ID.
    /// </summary>
    public string? F_DefectId { get; set; }

    /// <summary>
    /// 数量.
    /// </summary>
    public int? F_Num { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

}