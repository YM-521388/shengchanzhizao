namespace JNPF.example.Entitys.Dto.AProdOutsourceSettleLog;

/// <summary>
/// 操作日志详情输出参数.
/// </summary>
public class AProdOutsourceSettleLogDetailOutput
{
    /// <summary>
    /// 外协验收ID.
    /// </summary>
    public string? F_AcceptId { get; set; }

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
    public string F_CreatorTime { get; set; }

}