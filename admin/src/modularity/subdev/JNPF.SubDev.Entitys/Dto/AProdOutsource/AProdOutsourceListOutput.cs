using SqlSugar;

namespace JNPF.example.Entitys.Dto.AProdOutsource;

/// <summary>
/// a_prod_outsource输入参数.
/// </summary>
public class AProdOutsourceListOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    public string? F_GoodsId { get; set; }
    public string? F_GoodsName { get; set; }
    public string? F_GoodsNo { get; set; }
    public string? F_CategoryId { get; set; }
    public string? F_Specification { get; set; }

    /// <summary>
    /// 库存.
    /// </summary>
    public decimal? F_InventoryNum { get; set; }

    /// <summary>
    /// 已入库数量.
    /// </summary>
    public decimal? F_YseNum { get; set; }

    /// <summary>
    /// BOMID.
    /// </summary>
    public string? F_BOMId { get; set; }

    /// <summary>
    /// 外协工单号.
    /// </summary>
    public string? F_OutsourceNo { get; set; }

    /// <summary>
    /// 计划数.
    /// </summary>
    public string F_PlanNum { get; set; }

    /// <summary>
    /// 供应商ID.
    /// </summary>
    public string? F_SupplierId { get; set; }

    /// <summary>
    /// 供应商联系人.
    /// </summary>
    public string? F_ContactPerson { get; set; }

    /// <summary>
    /// 供应商联系人电话.
    /// </summary>
    public string? F_ContactPhone { get; set; }

    /// <summary>
    /// 供应商地区.
    /// </summary>
    public string? F_Region { get; set; }

    /// <summary>
    /// 供应商详细地址.
    /// </summary>
    public string? F_DetailAddress { get; set; }

    /// <summary>
    /// 外协单价.
    /// </summary>
    public decimal? F_Price { get; set; }

    /// <summary>
    /// 交货日期.
    /// </summary>
    public string F_DeliveryDate { get; set; }

    /// <summary>
    /// 优先级.
    /// </summary>
    public string? F_Priority { get; set; }
    public string? F_PriorityCode { get; set; }

    /// <summary>
    /// 计划开始.
    /// </summary>
    public string F_PlanStartDate { get; set; }

    /// <summary>
    /// 计划结束.
    /// </summary>
    public string F_PlanEndDate { get; set; }

    /// <summary>
    /// 图纸.
    /// </summary>
    public object? F_Files { get; set; }

    /// <summary>
    /// 成本单价.
    /// </summary>
    public decimal? F_CostPrice { get; set; }

    /// <summary>
    /// 单位比例.
    /// </summary>
    public string? F_Unit_Ratio { get; set; }

    /// <summary>
    /// 状态.
    /// </summary>
    public string? F_State { get; set; }
    public string? F_StateId { get; set; }

    /// <summary>
    /// 描述.
    /// </summary>
    public string? F_Description { get; set; }

    /// <summary>
    /// 暂停原因.
    /// </summary>
    public string? F_PauseReason { get; set; }

    /// <summary>
    /// 取消原因.
    /// </summary>
    public string? F_CancelReason { get; set; }

    /// <summary>
    /// 创建用户.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

}