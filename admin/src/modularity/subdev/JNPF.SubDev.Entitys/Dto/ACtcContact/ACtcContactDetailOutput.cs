namespace JNPF.example.Entitys.Dto.ACtcContact;

/// <summary>
/// 客户联系人详情输出参数.
/// </summary>
public class ACtcContactDetailOutput
{

    public string? id { get; set; }

    /// <summary>
    /// 客户ID.
    /// </summary>
    public string? F_CustomerId { get; set; }

    /// <summary>
    /// 是否默认.
    /// </summary>
    public string F_IsDefault { get; set; }

    /// <summary>
    /// 联系人.
    /// </summary>
    public string? F_ContactName { get; set; }

    /// <summary>
    /// 联系人电话.
    /// </summary>
    public string? F_ContactPhone { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

}