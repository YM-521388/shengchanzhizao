using JNPF.Common.Filter;

namespace JNPF.example.Entitys.Dto.ACtcFinance;

/// <summary>
/// 收支记录列表查询输入.
/// </summary>
public class ACtcFinanceListQueryInput : PageInputBase
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
    /// 类型(收/支).
    /// </summary>
    public string F_Type { get; set; }

    /// <summary>
    /// 金额.
    /// </summary>
    public List<decimal?> F_Money { get; set; }

    /// <summary>
    /// 审核状态.
    /// </summary>
    public string F_AuditStatus { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public List<DateTime> F_CreatorTime { get; set; }

}