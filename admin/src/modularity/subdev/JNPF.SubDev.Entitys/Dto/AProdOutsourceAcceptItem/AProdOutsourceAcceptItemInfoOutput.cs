namespace JNPF.example.Entitys.Dto.AProdOutsourceAcceptItem;
 
/// <summary>
/// 选择验收内容输出参数.
/// </summary>
public class AProdOutsourceAcceptItemInfoOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? F_Id { get; set; }

    /// <summary>
    /// 验收内容ID.
    /// </summary>
    public string? F_AcceptId { get; set; }

    /// <summary>
    /// 排序.
    /// </summary>
    public int? F_SortCode { get; set; }

    /// <summary>
    /// 创建用户.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

}