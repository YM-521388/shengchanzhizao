using SqlSugar;

namespace JNPF.example.Entitys.Dto.AProdPlan;

/// <summary>
/// a_prod_plan输入参数.
/// </summary>
public class AProdPlanListOutput
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
    /// 交货日期.
    /// </summary>
    public string F_DeliveryDate { get; set; }

    /// <summary>
    /// 物料分析状态.
    /// </summary>
    public string? F_State { get; set; }

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