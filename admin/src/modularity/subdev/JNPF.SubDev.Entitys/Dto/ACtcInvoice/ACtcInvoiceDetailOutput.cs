using JNPF.Common.Models;
using Newtonsoft.Json;
using JNPF.example.Entitys.Dto.ACtcInvoiceLog;

namespace JNPF.example.Entitys.Dto.ACtcInvoice;

/// <summary>
/// a_ctc_invoice详情输出参数.
/// </summary>
public class ACtcInvoiceDetailOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 合同ID.
    /// </summary>
    public string? F_ContractId { get; set; }

    /// <summary>
    /// 开票金额(元).
    /// </summary>
    public decimal? F_Money { get; set; }

    /// <summary>
    /// 开票状态.
    /// </summary>
    public string? F_Status { get; set; }

    /// <summary>
    /// 申请附件.
    /// </summary>
    public object? F_ApplyFiles { get; set; }

    /// <summary>
    /// 申请备注.
    /// </summary>
    public string? F_ApplyRemark { get; set; }

    /// <summary>
    /// 开票附件.
    /// </summary>
    public object? F_InvoiceFiles { get; set; }

    /// <summary>
    /// 开票备注.
    /// </summary>
    public string? F_InvoiceRemark { get; set; }

    /// <summary>
    /// 申请人员.
    /// </summary>
    public string? F_ApplyUserId { get; set; }

    /// <summary>
    /// 申请时间.
    /// </summary>
    public string F_ApplyTime { get; set; }

    /// <summary>
    /// 开票人员.
    /// </summary>
    public string? F_InvoiceUserId { get; set; }

    /// <summary>
    /// 开票时间.
    /// </summary>
    public string F_InvoiceTime { get; set; }

    /// <summary>
    /// 合同发票管理日志.
    /// </summary>
    public List<ACtcInvoiceLogDetailOutput> tableField2cca74 { get; set; }

}