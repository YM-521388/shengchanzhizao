using JNPF.example.Entitys.Dto.APuOrderItem;
using JNPF.example.Entitys.Dto.APuOrderLog;

namespace JNPF.example.Entitys.Dto.APuOrder;

/// <summary>
/// a_pu_order详情输出参数.
/// </summary>
public class APuOrderDetailOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 采购单号.
    /// </summary>
    public string? F_OrderNo { get; set; }

    /// <summary>
    /// 供应商ID.
    /// </summary>
    public string? F_SupplierId { get; set; }

    /// <summary>
    /// 生产计划ID.
    /// </summary>
    public string? F_ProdPlanId { get; set; }

    /// <summary>
    /// 商品金额.
    /// </summary>
    public decimal? F_Money { get; set; }

    /// <summary>
    /// 采购人.
    /// </summary>
    public string? F_UserId { get; set; }

    /// <summary>
    /// 交货日期.
    /// </summary>
    public string F_DeliveryDate { get; set; }

    /// <summary>
    /// 采购数量.
    /// </summary>
    public string F_PurchaseNum { get; set; }

    /// <summary>
    /// 审核状态.
    /// </summary>
    public string? F_CheckState { get; set; }

    /// <summary>
    /// 审核人员.
    /// </summary>
    public string? F_CheckUserId { get; set; }

    /// <summary>
    /// 审核时间.
    /// </summary>
    public string F_CheckTime { get; set; }

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
    /// 采购单商品信息.
    /// </summary>
    public List<APuOrderItemDetailOutput> tableFieldf01abd { get; set; }

    /// <summary>
    /// 采购单日志.
    /// </summary>
    public List<APuOrderLogDetailOutput> tableField8b2a57 { get; set; }

}