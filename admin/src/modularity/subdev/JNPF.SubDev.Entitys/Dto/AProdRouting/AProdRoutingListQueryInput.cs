using JNPF.Common.Filter;

namespace JNPF.example.Entitys.Dto.AProdRouting;

/// <summary>
/// a_prod_routing列表查询输入.
/// </summary>
public class AProdRoutingListQueryInput : PageInputBase
{
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
    /// 工艺路线编号.
    /// </summary>
    public string F_RoutingNo { get; set; }

    /// <summary>
    /// 工艺路线名称.
    /// </summary>
    public string F_RoutingName { get; set; }

}