using JNPF.Common.Filter;

namespace JNPF.example.Entitys.Dto.AProdPlan;

/// <summary>
/// a_prod_plan列表查询输入.
/// </summary>
public class AProdPlanListQueryInput : PageInputBase
{
    /// <summary>
    /// 选择导出数据ids.
    /// </summary>
    public string selectIds { get; set; }

    /// <summary>
    /// 选择导出数据key.
    /// </summary>
    public string selectKey { get; set; }

    /// <summary>
    /// 导出类型.
    /// </summary>
    public int dataType { get; set; }
    
    /// <summary>
    /// 关键词查询.
    /// </summary>
    public string jnpfKeyword { get; set; }

    /// <summary>
    /// 计划编号.
    /// </summary>
    public string F_PlanNo { get; set; }

    /// <summary>
    /// 计划名称.
    /// </summary>
    public string F_PlanName { get; set; }

    /// <summary>
    /// 交货日期.
    /// </summary>
    public List<DateTime> F_DeliveryDate { get; set; }

    /// <summary>
    /// 物料分析状态.
    /// </summary>
    public string F_State { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public List<DateTime> F_CreatorTime { get; set; }

}