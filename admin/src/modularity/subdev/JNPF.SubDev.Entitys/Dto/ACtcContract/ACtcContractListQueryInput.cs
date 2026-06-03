using JNPF.Common.Filter;

namespace JNPF.example.Entitys.Dto.ACtcContract;

/// <summary>
/// a_ctc_contract列表查询输入.
/// </summary>
public class ACtcContractListQueryInput : PageInputBase
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
    /// 合同编号.
    /// </summary>
    public string F_ContractCode { get; set; }

    /// <summary>
    /// 客户合同号.
    /// </summary>
    public string F_CustomerCode { get; set; }

    /// <summary>
    /// 客户ID.
    /// </summary>
    public object F_CustomerId { get; set; }

    /// <summary>
    /// 预付款.
    /// </summary>
    public List<decimal?> F_PrepayAmount { get; set; }

    /// <summary>
    /// 交货日期.
    /// </summary>
    public List<DateTime> F_DeliveryDate { get; set; }

    /// <summary>
    /// 业务员.
    /// </summary>
    public object F_SalesmanId { get; set; }

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