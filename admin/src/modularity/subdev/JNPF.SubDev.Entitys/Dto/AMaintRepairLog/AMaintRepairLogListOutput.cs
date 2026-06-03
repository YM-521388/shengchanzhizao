namespace JNPF.example.Entitys.Dto.AMaintRepairLog;

/// <summary>
/// 维修工单日志输入参数.
/// </summary>
public class AMaintRepairLogListOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 标题.
    /// </summary>
    public string? F_Title { get; set; }

    /// <summary>
    /// 内容.
    /// </summary>
    public string? F_Content { get; set; }
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

}