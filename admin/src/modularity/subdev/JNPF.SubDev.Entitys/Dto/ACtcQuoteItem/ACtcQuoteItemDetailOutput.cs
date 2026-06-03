using JNPF.Common.Security;
using JNPF.Common.Models;
using Newtonsoft.Json;

namespace JNPF.example.Entitys.Dto.ACtcQuoteItem;

/// <summary>
/// 选择报价单商品详情输出参数.
/// </summary>
public class ACtcQuoteItemDetailOutput
{
    /// <summary>
    /// 报价单ID.
    /// </summary>
    public string? F_QuoteId { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    public string? F_CustomerId { get; set; }

    /// <summary>
    /// 单价.
    /// </summary>
    public decimal? F_Price { get; set; }

    /// <summary>
    /// 销售数量.
    /// </summary>
    public int? F_SaleQty { get; set; }
    public string? F_UnitTwo { get; set; }

    /// <summary>
    /// 折扣.
    /// </summary>
    public decimal? F_Discount { get; set; }

    /// <summary>
    /// 优惠金额.
    /// </summary>
    public decimal? F_DiscountAmount { get; set; }

    /// <summary>
    /// 商品备注.
    /// </summary>
    public string? F_Description { get; set; }

    /// <summary>
    /// 附件.
    /// </summary>
    public List<FileControlsModel> F_Files { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

}