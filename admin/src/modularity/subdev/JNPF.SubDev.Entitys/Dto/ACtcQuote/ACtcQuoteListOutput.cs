using JNPF.example.Entitys.Dto.ACtcQuoteItem;
using SqlSugar;
namespace JNPF.example.Entitys.Dto.ACtcQuote;

/// <summary>
/// a_ctc_quote输入参数.
/// </summary>
public class ACtcQuoteListOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 报价单号.
    /// </summary>
    public string? F_QuoteCode { get; set; }

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
    /// 报价金额.
    /// </summary>
    public decimal? F_QuoteAmount { get; set; }

    /// <summary>
    /// 商品总数.
    /// </summary>
    public string F_GoodsTotal { get; set; }

    /// <summary>
    /// 交货日期.
    /// </summary>
    public string F_DeliveryDate { get; set; }

    /// <summary>
    /// 报价日期.
    /// </summary>
    public string F_QuoteDate { get; set; }

    /// <summary>
    /// 业务员.
    /// </summary>
    public string? F_SalesmanId { get; set; }

    /// <summary>
    /// 报价状态.
    /// </summary>
    public string? F_QuoteStatus { get; set; }
    public string? F_QuoteStatusNo { get; set; }

    /// <summary>
    /// 审核状态.
    /// </summary>
    public string? F_AuditStatus { get; set; }

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
    public List<ACtcQuoteItemListOutput> tableFielddc29f7 { get; set; }

}