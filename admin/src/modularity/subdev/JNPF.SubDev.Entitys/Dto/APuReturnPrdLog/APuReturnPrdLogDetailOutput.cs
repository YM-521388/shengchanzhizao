namespace JNPF.example.Entitys.Dto.APuReturnPrdLog;

/// <summary>
/// 操作日志详情输出参数.
/// </summary>
public class APuReturnPrdLogDetailOutput
{
    /// <summary>
    /// 生产退料单Id.
    /// </summary>
    public string? F_ReturnInPRDId { get; set; }

    /// <summary>
    /// 类型.
    /// </summary>
    public string? F_Type { get; set; }

    /// <summary>
    /// 标题.
    /// </summary>
    public string? F_Title { get; set; }

    /// <summary>
    /// 内容.
    /// </summary>
    public string? F_Content { get; set; }

    /// <summary>
    /// 修改原因.
    /// </summary>
    public string? F_Reason { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

}