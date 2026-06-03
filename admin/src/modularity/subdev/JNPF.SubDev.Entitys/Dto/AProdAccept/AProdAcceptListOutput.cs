namespace JNPF.example.Entitys.Dto.AProdAccept;

/// <summary>
/// 验收内容输入参数.
/// </summary>
public class AProdAcceptListOutput
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
    /// 验收内容.
    /// </summary>
    public string? F_Content { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    public string? F_Description { get; set; }

    /// <summary>
    /// 创建用户.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

}