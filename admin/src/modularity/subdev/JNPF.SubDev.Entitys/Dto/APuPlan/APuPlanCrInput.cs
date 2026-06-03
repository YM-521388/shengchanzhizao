using JNPF.example.Entitys.Dto.APuPlanItem;
using JNPF.example.Entitys.Dto.APuPlanLog;
using JNPF.JsonSerialization;
using Newtonsoft.Json;
 
namespace JNPF.example.Entitys.Dto.APuPlan;

/// <summary>
/// a_pu_plan修改输入参数.
/// </summary>
public class APuPlanCrInput
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
    /// 供应商ID.
    /// </summary>
    public string F_SupplierId { get; set; }

    /// <summary>
    /// 金额.
    /// </summary>
    public decimal? F_Money { get; set; }

    /// <summary>
    /// 交货日期.
    /// </summary>
    public DateTime? F_DeliveryDate { get; set; }

    /// <summary>
    /// 采购数量.
    /// </summary>
    public decimal? F_PurchaseNum { get; set; }

    /// <summary>
    /// 负责人.
    /// </summary>
    public string F_ResponsibleUserId { get; set; }

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
    /// 采购计划商品信息.
    /// </summary>
    public List<APuPlanItemCrInput> tableFieldc87c94 { get; set; }

    /// <summary>
    /// 采购计划日志.
    /// </summary>
    public List<APuPlanLogCrInput> tableFielde82301 { get; set; }

}