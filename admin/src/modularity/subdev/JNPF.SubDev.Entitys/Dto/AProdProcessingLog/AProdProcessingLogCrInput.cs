using JNPF.JsonSerialization;
using Newtonsoft.Json;

namespace JNPF.example.Entitys.Dto.AProdProcessingLog;
 
/// <summary>
/// 加工单日志修改输入参数.
/// </summary>
public class AProdProcessingLogCrInput
{
    /// <summary>
    /// 加工单ID.
    /// </summary>
    public string F_ProcessingId { get; set; }

    /// <summary>
    /// 标题.
    /// </summary>
    public string? F_Title { get; set; }

    /// <summary>
    /// 内容.
    /// </summary>
    public string? F_Content { get; set; }

    /// <summary>
    /// 创建用户.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>

}