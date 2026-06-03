using SqlSugar;

namespace JNPF.example.Entitys.Dto.ACtcContact;

/// <summary>
/// 客户联系人输入参数.
/// </summary>
public class ACtcContactListOutput
{
    /// <summary>
    /// 客户ID.
    /// </summary>
    public string? F_CustomerId { get; set; }

    public string? id { get; set; }

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

    /// <summary>
    /// 创建组织.
    /// </summary>
    public string? F_CreatorOrganizeId { get; set; }

}