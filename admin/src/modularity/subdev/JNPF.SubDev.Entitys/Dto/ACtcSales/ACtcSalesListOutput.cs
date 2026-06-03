using SqlSugar;

namespace JNPF.example.Entitys.Dto.ACtcSales;

/// <summary>
/// 销售费用输入参数.
/// </summary>
public class ACtcSalesListOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 客户ID.
    /// </summary>
    public string? F_CustomerId { get; set; }

    /// <summary>
    /// 合同ID.
    /// </summary>
    public string? F_ContractId { get; set; }

    /// <summary>
    /// 支出日期.
    /// </summary>
    public string F_ExpendDate { get; set; }

    /// <summary>
    /// 支出类别.
    /// </summary>
    public string? F_ExpendType { get; set; }

    /// <summary>
    /// 金额.
    /// </summary>
    public decimal? F_Money { get; set; }

    /// <summary>
    /// 审核状态.
    /// </summary>
    public string? F_AuditStatus { get; set; }

    /// <summary>
    /// 附件.
    /// </summary>
    public object? F_Files { get; set; }

    /// <summary>
    /// 结算状态.
    /// </summary>
    public string? F_SettleStatus { get; set; }
    public string? F_SettleStatusCode { get; set; }

    /// <summary>
    /// 结算附件.
    /// </summary>
    public object? F_SettleFiles { get; set; }

    /// <summary>
    /// 结算备注.
    /// </summary>
    public string? F_SettleDesc { get; set; }

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

    /// <summary>
    /// 创建组织.
    /// </summary>
    public string? F_CreatorOrganizeId { get; set; }

}