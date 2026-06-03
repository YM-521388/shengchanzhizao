using JNPF.Common.Filter;

namespace JNPF.example.Entitys.Dto.ACtcQuote;

/// <summary>
/// a_ctc_quote列表查询输入.
/// </summary>
public class ACtcQuoteListQueryInput : PageInputBase
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
    /// 报价单号.
    /// </summary>
    public string F_QuoteCode { get; set; }

    /// <summary>
    /// 客户ID.
    /// </summary>
    public object F_CustomerId { get; set; }

    /// <summary>
    /// 业务员.
    /// </summary>
    public object F_SalesmanId { get; set; }

    /// <summary>
    /// 报价状态.
    /// </summary>
    public string F_QuoteStatus { get; set; }

}