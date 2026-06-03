using SqlSugar;

namespace JNPF.example.Entitys.Dto.ACtcFollow;

/// <summary>
/// 客户跟单输入参数.
/// </summary>
public class ACtcFollowListOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 客户ID.
    /// </summary>
    public string? F_CustomerId { get; set; }

    /// <summary>
    /// 跟单类型.
    /// </summary>
    public string? F_FollowType { get; set; }

    /// <summary>
    /// 联系人ID.
    /// </summary>
    public string? F_ContactId { get; set; }

    /// <summary>
    /// 跟单内容.
    /// </summary>
    public string? F_FollowContent { get; set; }

    /// <summary>
    /// 下次跟单时间.
    /// </summary>
    public string F_NextFollowTime { get; set; }

    /// <summary>
    /// 跟单状态.
    /// </summary>
    public string F_State { get; set; }
    public string F_StateCode { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

    /// <summary>
    /// 创建组织.
    /// </summary>
    public string? F_CreatorOrganizeId { get; set; }

}