using JNPF.Common.Filter;

namespace JNPF.example.Entitys.Dto.APuOrderInvoice;

/// <summary>
/// 采购单发票单管理列表查询输入.
/// </summary>
public class APuOrderInvoiceListQueryInput : PageInputBase
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
    /// 供应商ID.
    /// </summary>
    public object F_SupplierId { get; set; }

    /// <summary>
    /// 开票日期.
    /// </summary>
    public List<DateTime> F_InvoiceDate { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    public string F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public List<DateTime> F_CreatorTime { get; set; }

}