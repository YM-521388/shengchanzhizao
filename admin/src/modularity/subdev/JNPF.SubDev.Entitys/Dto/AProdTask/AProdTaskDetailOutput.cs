using JNPF.example.Entitys.Dto.AProdTaskItem;

namespace JNPF.example.Entitys.Dto.AProdTask;

/// <summary>
/// 生产任务详情输出参数.
/// </summary>
public class AProdTaskDetailOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 加工单ID.
    /// </summary>
    public string? F_ProcessingId { get; set; }

    /// <summary>
    /// 工序ID.
    /// </summary>
    public string? F_ProcessId { get; set; }
    public string? F_ProcessName { get; set; }
    public string? F_ProcessCode { get; set; }
    public string? F_IsQc { get; set; }

    /// <summary>
    /// 车间ID.
    /// </summary>
    public string? F_WorkshopId { get; set; }

    /// <summary>
    /// 产线.
    /// </summary>
    public string? F_Line { get; set; }

    /// <summary>
    /// 报工单位.
    /// </summary>
    public string? F_ReportUnit { get; set; }

    /// <summary>
    /// 单位关系.
    /// </summary>
    public string F_UnitRatio { get; set; }

    /// <summary>
    /// 计价方式.
    /// </summary>
    public string? F_PriceType { get; set; }

    /// <summary>
    /// 工资单价.
    /// </summary>
    public decimal? F_WagePrice { get; set; }

    /// <summary>
    /// 标准工时.
    /// </summary>
    public string F_StdHour { get; set; }

    /// <summary>
    /// 机台ID.
    /// </summary>
    public string? F_MachineId { get; set; }

    /// <summary>
    /// 生产人员.
    /// </summary>
    public string? F_ProdUserIds { get; set; }

    /// <summary>
    /// 不良品项ID.
    /// </summary>
    public string? F_DefectIds { get; set; }

    /// <summary>
    /// 质检人员.
    /// </summary>
    public string? F_QcUserIds { get; set; }

    /// <summary>
    /// 计划开始.
    /// </summary>
    public string F_PlanStartDate { get; set; }

    /// <summary>
    /// 计划结束.
    /// </summary>
    public string F_PlanEndDate { get; set; }

    /// <summary>
    /// 生产派工.
    /// </summary>
    public string? F_ProdDispatch { get; set; }

    /// <summary>
    /// 质检派工.
    /// </summary>
    public string? F_QcDispatch { get; set; }

    /// <summary>
    /// 生产数量.
    /// </summary>
    public string F_ProdQty { get; set; }

    /// <summary>
    /// 任务状态.
    /// </summary>
    public string? F_TaskStatus { get; set; }

    /// <summary>
    /// 任务类型.
    /// </summary>
    public string? F_TaskType { get; set; }

    /// <summary>
    /// 排序.
    /// </summary>
    public string F_SortCode { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    public string? F_Description { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

    /// <summary>
    /// 生产任务物料清单.
    /// </summary>
    public List<AProdTaskItemDetailOutput> tableField0bb944 { get; set; }

}