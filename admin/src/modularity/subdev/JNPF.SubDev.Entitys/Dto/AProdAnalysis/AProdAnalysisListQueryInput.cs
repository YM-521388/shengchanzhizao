using JNPF.Common.Filter;

namespace JNPF.example.Entitys.Dto.AProdAnalysis;

/// <summary>
/// a_prod_analysis列表查询输入.
/// </summary>
public class AProdAnalysisListQueryInput : PageInputBase
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
    /// 分析人.
    /// </summary>
    public object F_AnalysisUserId { get; set; }

    /// <summary>
    /// 分析时间.
    /// </summary>
    public List<DateTime> F_AnalysisTime { get; set; }

    /// <summary>
    /// 状态.
    /// </summary>
    public string F_State { get; set; }

    /// <summary>
    /// 创建用户.
    /// </summary>
    public string F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public List<DateTime> F_CreatorTime { get; set; }

}