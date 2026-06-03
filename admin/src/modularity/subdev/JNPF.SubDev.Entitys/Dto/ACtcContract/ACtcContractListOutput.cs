using JNPF.example.Entitys.Dto.ACtcContractItem;
using SqlSugar;
namespace JNPF.example.Entitys.Dto.ACtcContract;

/// <summary>
/// a_ctc_contract输入参数.
/// </summary>
public class ACtcContractListOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 合同编号.
    /// </summary>
    public string? F_ContractCode { get; set; }

    /// <summary>
    /// 客户合同号.
    /// </summary>
    public string? F_CustomerCode { get; set; }

    /// <summary>
    /// 客户ID.
    /// </summary>
    public string? F_CustomerId { get; set; }

    /// <summary>
    /// 联系人ID.
    /// </summary>
    public string? F_ContactId { get; set; }

    /// <summary>
    /// 客户地址ID.
    /// </summary>
    public string? F_AddressId { get; set; }

    /// <summary>
    /// 预付款.
    /// </summary>
    public decimal? F_PrepayAmount { get; set; }

    /// <summary>
    /// 付款周期（天）.
    /// </summary>
    public int? F_PaymentCycle { get; set; }

    /// <summary>
    /// 销售总数.
    /// </summary>
    public string F_SaleTotal { get; set; }

    /// <summary>
    /// 合同金额.
    /// </summary>
    public decimal? F_ContractAmount { get; set; }

    /// <summary>
    /// 交货日期.
    /// </summary>
    public string F_DeliveryDate { get; set; }

    /// <summary>
    /// 是否含税.
    /// </summary>
    public string F_IsTaxable { get; set; }

    /// <summary>
    /// 业务员.
    /// </summary>
    public string? F_SalesmanId { get; set; }

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

    /// <summary>
    /// .
    /// </summary>
    public List<ACtcContractItemListOutput> tableFieldf4dfaf { get; set; }

}