using JNPF.Common.Filter;

namespace JNPF.example.Entitys.Dto.AProdOutsource;

/// <summary>
/// a_prod_outsource列表查询输入.
/// </summary>
public class AProdOutsourceListQueryInput : PageInputBase
{
    /// <summary>
    /// 高级查询.
    /// </summary>
    public string superQueryJson { get; set; }

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
    /// ID.
    /// </summary>
    public string F_OutsourceId { get; set; }

    /// <summary>
    /// 外协工单号.
    /// </summary>
    public string F_OutsourceNo { get; set; }

    /// <summary>
    /// 计划数.
    /// </summary>
    public List<decimal?> F_PlanNum { get; set; }

    /// <summary>
    /// 供应商ID.
    /// </summary>
    public object F_SupplierId { get; set; }

    /// <summary>
    /// 供应商联系人电话.
    /// </summary>
    public string F_ContactPhone { get; set; }

    /// <summary>
    /// 供应商详细地址.
    /// </summary>
    public string F_DetailAddress { get; set; }

    /// <summary>
    /// 交货日期.
    /// </summary>
    public List<DateTime> F_DeliveryDate { get; set; }

    /// <summary>
    /// 优先级.
    /// </summary>
    public object F_Priority { get; set; }

    /// <summary>
    /// 计划开始.
    /// </summary>
    public List<DateTime> F_PlanStartDate { get; set; }

    /// <summary>
    /// 计划结束.
    /// </summary>
    public List<DateTime> F_PlanEndDate { get; set; }

}