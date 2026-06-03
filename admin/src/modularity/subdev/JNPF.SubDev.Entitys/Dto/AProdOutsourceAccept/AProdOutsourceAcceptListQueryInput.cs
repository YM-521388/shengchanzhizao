using JNPF.Common.Filter;

namespace JNPF.example.Entitys.Dto.AProdOutsourceAccept;

/// <summary>
/// a_prod_outsource_accept列表查询输入.
/// </summary>
public class AProdOutsourceAcceptListQueryInput : PageInputBase
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
    /// 外协类型.
    /// </summary>
    public string F_OutsourceType { get; set; }

    /// <summary>
    /// 结算状态.
    /// </summary>
    public string F_SettleStatus { get; set; }

    /// <summary>
    /// 结算人.
    /// </summary>
    public object F_SettleUserId { get; set; }

    /// <summary>
    /// 结算时间.
    /// </summary>
    public List<DateTime> F_SettleTime { get; set; }

}