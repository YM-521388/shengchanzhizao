using JNPF.Common.Filter;

namespace JNPF.example.Entitys.Dto.ACtcSales;

/// <summary>
/// 销售费用列表查询输入.
/// </summary>
public class ACtcSalesListQueryInput : PageInputBase
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
    /// 支出日期.
    /// </summary>
    public List<DateTime> F_ExpendDate { get; set; }

    /// <summary>
    /// 支出类别.
    /// </summary>
    public object F_ExpendType { get; set; }

    /// <summary>
    /// 金额.
    /// </summary>
    public List<decimal?> F_Money { get; set; }

    /// <summary>
    /// 审核状态.
    /// </summary>
    public string F_AuditStatus { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    public string F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public List<DateTime> F_CreatorTime { get; set; }

}