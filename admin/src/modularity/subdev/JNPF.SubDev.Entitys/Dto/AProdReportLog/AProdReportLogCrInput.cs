using JNPF.JsonSerialization;
using Newtonsoft.Json;

namespace JNPF.example.Entitys.Dto.AProdReportLog;
 
/// <summary>
/// 报工记录日志修改输入参数.
/// </summary>
public class AProdReportLogCrInput
{
    /// <summary>
    /// 报工记录ID.
    /// </summary>
    public string? F_ReportId { get; set; }

    /// <summary>
    /// 标题.
    /// </summary>
    public string? F_Title { get; set; }

    /// <summary>
    /// 内容.
    /// </summary>
    public string? F_Content { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>

}