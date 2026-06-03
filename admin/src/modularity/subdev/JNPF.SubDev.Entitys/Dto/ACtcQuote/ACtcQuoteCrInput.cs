using JNPF.example.Entitys.Dto.ACtcQuoteItem;
using JNPF.JsonSerialization;
using Newtonsoft.Json;
 
namespace JNPF.example.Entitys.Dto.ACtcQuote;

/// <summary>
/// a_ctc_quote修改输入参数.
/// </summary>
public class ACtcQuoteCrInput
{
    /// <summary>
    /// 报价单号.
    /// </summary>
    public string? F_QuoteCode { get; set; }

    /// <summary>
    /// 客户ID.
    /// </summary>
    public string F_CustomerId { get; set; }

    /// <summary>
    /// 联系人ID.
    /// </summary>
    public string? F_ContactId { get; set; }

    /// <summary>
    /// 客户地址ID.
    /// </summary>
    public string F_AddressId { get; set; }

    /// <summary>
    /// 报价金额.
    /// </summary>
    public decimal? F_QuoteAmount { get; set; }

    /// <summary>
    /// 商品总数.
    /// </summary>
    public int? F_GoodsTotal { get; set; }

    /// <summary>
    /// 交货日期.
    /// </summary>
    public DateTime? F_DeliveryDate { get; set; }

    /// <summary>
    /// 报价日期.
    /// </summary>
    public DateTime? F_QuoteDate { get; set; }

    /// <summary>
    /// 业务员.
    /// </summary>
    public string F_SalesmanId { get; set; }

    /// <summary>
    /// 报价状态.
    /// </summary>
    public string? F_QuoteStatus { get; set; }

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
    /// 合同报价单商品.
    /// </summary>
    public List<ACtcQuoteItemCrInput> tableFielddc29f7 { get; set; }

}