using JNPF.example.Entitys.Dto.APuOrderItem;
using JNPF.example.Entitys.Dto.APuOrderLog;
using JNPF.JsonSerialization;
using Newtonsoft.Json;
 
namespace JNPF.example.Entitys.Dto.APuOrder;

/// <summary>
/// a_pu_order修改输入参数.
/// </summary>
public class APuOrderCrInput
{
    /// <summary>
    /// 采购单号.
    /// </summary>
    public string? F_OrderNo { get; set; }

    /// <summary>
    /// 供应商ID.
    /// </summary>
    public string F_SupplierId { get; set; }

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
    public string F_UserId { get; set; }

    /// <summary>
    /// 交货日期.
    /// </summary>
    public DateTime? F_DeliveryDate { get; set; }

    /// <summary>
    /// 采购数量.
    /// </summary>
    public decimal? F_PurchaseNum { get; set; }

    /// <summary>
    /// 审核状态.
    /// </summary>
    public string F_CheckState { get; set; }

    /// <summary>
    /// 审核人员.
    /// </summary>
    public string F_CheckUserId { get; set; }

    /// <summary>
    /// 审核时间.
    /// </summary>
    public DateTime? F_CheckTime { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    public string? F_Description { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    public string? F_CreatorUserId { get; set; }


    /// <summary>
    /// 采购单商品信息.
    /// </summary>
    public List<APuOrderItemCrInput> tableFieldf01abd { get; set; }

    /// <summary>
    /// 采购单日志.
    /// </summary>
    public List<APuOrderLogCrInput> tableField8b2a57 { get; set; }

}