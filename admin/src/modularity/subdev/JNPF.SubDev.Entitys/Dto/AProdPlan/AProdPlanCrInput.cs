using JNPF.example.Entitys.Dto.AProdPlanItem;
using JNPF.JsonSerialization;
using Newtonsoft.Json;
 
namespace JNPF.example.Entitys.Dto.AProdPlan;

/// <summary>
/// a_prod_plan修改输入参数.
/// </summary>
public class AProdPlanCrInput
{
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
    public DateTime? F_DeliveryDate { get; set; }

    /// <summary>
    /// 物料分析状态.
    /// </summary>
    public string F_State { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    public string? F_Description { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    public string? F_CreatorUserId { get; set; }


    /// <summary>
    /// 生产计划商品列表.
    /// </summary>
    public List<AProdPlanItemCrInput> tableField2152f8 { get; set; }

}