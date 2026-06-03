using JNPF.Common.Models;
using Newtonsoft.Json;

namespace JNPF.example.Entitys.Dto.APuOrderPay;

/// <summary>
/// 采购单付款记录详情输出参数.
/// </summary>
public class APuOrderPayDetailOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 采购单ID.
    /// </summary>
    public string? F_OrderId { get; set; }

    /// <summary>
    /// 供应商ID.
    /// </summary>
    public string? F_SupplierId { get; set; }

    /// <summary>
    /// 付款金额.
    /// </summary>
    public decimal? F_Money { get; set; }

    /// <summary>
    /// 付款日期.
    /// </summary>
    public string F_PayDate { get; set; }

    /// <summary>
    /// 附件.
    /// </summary>
    public object? F_Files { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    public string? F_Description { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

}