using SqlSugar;

namespace JNPF.example.Entitys.Dto.ACtcFinance;

/// <summary>
/// 收支记录输入参数.
/// </summary>
public class ACtcFinanceListOutput
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
    /// 类型(收/支).
    /// </summary>
    public string? F_Type { get; set; }

    /// <summary>
    /// 金额.
    /// </summary>
    public decimal? F_Money { get; set; }

    /// <summary>
    /// 附件.
    /// </summary>
    public object? F_Files { get; set; }

    /// <summary>
    /// 审核状态.
    /// </summary>
    public string? F_AuditStatus { get; set; }

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