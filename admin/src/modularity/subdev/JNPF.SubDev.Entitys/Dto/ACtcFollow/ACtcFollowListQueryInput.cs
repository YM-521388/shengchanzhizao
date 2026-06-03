using JNPF.Common.Filter;

namespace JNPF.example.Entitys.Dto.ACtcFollow;

/// <summary>
/// 客户跟单列表查询输入.
/// </summary>
public class ACtcFollowListQueryInput : PageInputBase
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
    public object F_CustomerId { get; set; }

    /// <summary>
    /// 跟单类型.
    /// </summary>
    public object F_FollowType { get; set; }

    /// <summary>
    /// 跟单内容.
    /// </summary>
    public string F_FollowContent { get; set; }

    /// <summary>
    /// 下次跟单时间.
    /// </summary>
    public List<DateTime> F_NextFollowTime { get; set; }

}