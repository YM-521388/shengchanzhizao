using JNPF.Common.Filter;

namespace JNPF.example.Entitys.Dto.ACtcContact;

/// <summary>
/// 客户联系人列表查询输入.
/// </summary>
public class ACtcContactListQueryInput : PageInputBase
{
    /// <summary>
    /// 高级查询.
    /// </summary>
    public string superQueryJson { get; set; }

    /// <summary>
    /// 选择导出数据ids.
    /// </summary>
    public string selectIds { get; set; }

    /// <summary>
    /// 选择导出数据key.
    /// </summary>
    public string selectKey { get; set; }

    /// <summary>
    /// 导出类型.
    /// </summary>
    public int dataType { get; set; }
    
    /// <summary>
    /// 关键词查询.
    /// </summary>
    public string jnpfKeyword { get; set; }

    /// <summary>
    /// 客户ID.
    /// </summary>
    public string F_CustomerId { get; set; }

    /// <summary>
    /// 联系人.
    /// </summary>
    public string F_ContactName { get; set; }

    /// <summary>
    /// 联系人电话.
    /// </summary>
    public string F_ContactPhone { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public List<DateTime> F_CreatorTime { get; set; }

}