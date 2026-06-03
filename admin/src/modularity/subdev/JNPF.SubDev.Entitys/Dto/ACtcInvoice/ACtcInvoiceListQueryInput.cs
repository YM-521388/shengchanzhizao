using JNPF.Common.Filter;

namespace JNPF.example.Entitys.Dto.ACtcInvoice;

/// <summary>
/// a_ctc_invoice列表查询输入.
/// </summary>
public class ACtcInvoiceListQueryInput : PageInputBase
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
    /// 开票状态.
    /// </summary>
    public string F_Status { get; set; }

    /// <summary>
    /// 申请人员.
    /// </summary>
    public object F_ApplyUserId { get; set; }

    /// <summary>
    /// 开票人员.
    /// </summary>
    public object F_InvoiceUserId { get; set; }

    /// <summary>
    /// 开票时间.
    /// </summary>
    public List<DateTime> F_InvoiceTime { get; set; }

}