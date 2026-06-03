namespace JNPF.example.Entitys.Dto.APuPlan;

/// <summary>
/// a_pu_plan输入参数.
/// </summary>
public class APuPlanListOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 计划编号.
    /// </summary>
    public string? F_PlanNo { get; set; }

    /// <summary>
    /// 计划名称.
    /// </summary>
    public string? F_PlanName { get; set; }

    /// <summary>
    /// 供应商ID.
    /// </summary>
    public string? F_SupplierId { get; set; }

    /// <summary>
    /// 金额.
    /// </summary>
    public decimal? F_Money { get; set; }

    /// <summary>
    /// 交货日期.
    /// </summary>
    public string F_DeliveryDate { get; set; }

    /// <summary>
    /// 采购数量.
    /// </summary>
    public string F_PurchaseNum { get; set; }

    /// <summary>
    /// 负责人.
    /// </summary>
    public string F_ResponsibleUserId { get; set; }

    /// <summary>
    /// 审核状态.
    /// </summary>
    public string? F_CheckState { get; set; }
    public string? F_CheckStateInfo { get; set; }

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

}