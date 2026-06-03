using JNPF.Common.Filter;

namespace JNPF.example.Entitys.Dto.ACustomer;

/// <summary>
/// a_customer列表查询输入.
/// </summary>
public class ACustomerListQueryInput : PageInputBase
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
    /// 名称.
    /// </summary>
    public string F_CustomerName { get; set; }

    /// <summary>
    /// 编号.
    /// </summary>
    public string F_CustomerCode { get; set; }

    /// <summary>
    /// 客户标签.
    /// </summary>
    public string? F_CustomerTags { get; set; }

    /// <summary>
    /// 领用人员.
    /// </summary>
    public object F_RequisitionUserId { get; set; }

}