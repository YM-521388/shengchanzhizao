namespace JNPF.example.Entitys.Dto.APuOrderLog;

/// <summary>
/// 采购单日志详情输出参数.
/// </summary>
public class APuOrderLogDetailOutput
{
    /// <summary>
    /// 采购单ID.
    /// </summary>
    public string? F_OrderId { get; set; }

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
    public string F_CreatorTime { get; set; }

}