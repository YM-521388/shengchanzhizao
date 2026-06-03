namespace JNPF.example.Entitys.Dto.AProdTaskLog;
 
/// <summary>
/// 生产任务暂停记录输出参数.
/// </summary>
public class AProdTaskLogInfoOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 生产任务ID.
    /// </summary>
    public string? F_TaskId { get; set; }

    /// <summary>
    /// 标题.
    /// </summary>
    public string? F_Title { get; set; }

    /// <summary>
    /// 暂停原因.
    /// </summary>
    public string? F_Content { get; set; }

    /// <summary>
    /// 创建用户.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

}