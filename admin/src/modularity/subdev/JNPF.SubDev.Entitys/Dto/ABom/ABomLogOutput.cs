using System;

namespace JNPF.example.Entitys.Dto.ABom;

/// <summary>
/// BOM 日志输出 DTO
/// </summary>
public class ABomLogOutput
{
    public string? id { get; set; }
    public string? F_BomId { get; set; }
    public string? F_Type { get; set; }
    public string? F_Title { get; set; }
    public string? F_Content { get; set; }
    public string? F_Reason { get; set; }
    /// <summary>
    /// 创建人员（已转换为姓名/账号）
    /// </summary>
    public string? F_CreatorUserId { get; set; }
    /// <summary>
    /// 创建时间（格式化字符串）
    /// </summary>
    public string? F_CreatorTime { get; set; }
}


