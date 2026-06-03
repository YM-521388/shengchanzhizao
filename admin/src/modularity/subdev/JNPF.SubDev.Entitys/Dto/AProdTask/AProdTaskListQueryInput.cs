using JNPF.Common.Filter;

namespace JNPF.example.Entitys.Dto.AProdTask;

/// <summary>
/// a_prod_task列表查询输入.
/// </summary>
public class AProdTaskListQueryInput : PageInputBase
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
    /// 获取任务物料.
    /// </summary>
    public string F_TaskId { get; set; }

    /// <summary>
    /// 加工单ID.
    /// </summary>
    public string? F_ProcessingId { get; set; }

    /// <summary>
    /// 工序ID.
    /// </summary>
    public string F_ProcessId { get; set; }

    /// <summary>
    /// 计划开始.
    /// </summary>
    public List<DateTime> F_PlanStartDate { get; set; }

    /// <summary>
    /// 计划结束.
    /// </summary>
    public List<DateTime> F_PlanEndDate { get; set; }

    /// <summary>
    /// 生产派工.
    /// </summary>
    public string F_ProdDispatch { get; set; }

    /// <summary>
    /// 质检派工.
    /// </summary>
    public string F_QcDispatch { get; set; }

    /// <summary>
    /// 员工ID.
    /// </summary>
    public string? UserId { get; set; }

}