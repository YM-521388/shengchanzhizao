using SqlSugar;

namespace JNPF.example.Entitys.Dto.AProdTask;

/// <summary>
/// a_prod_task输入参数.
/// </summary>
public class AProdTaskListOutput
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
    /// 商品.
    /// </summary>
    public string? F_GoodsName { get; set; }
    public string? F_GoodsNo { get; set; }
    public string? F_Specification { get; set; }

    /// <summary>
    /// 工序ID.
    /// </summary>
    public string? F_ProcessId { get; set; }
    public string? F_ProcessName { get; set; }
    public string? F_ProcessCode { get; set; }

    /// <summary>
    /// 生产人员.
    /// </summary>
    public string? F_ProdUserIds { get; set; }
    public string? F_ProdUserByIds { get; set; }

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
    public string? F_PlanStartDate { get; set; }

    /// <summary>
    /// 计划结束.
    /// </summary>
    public string? F_PlanEndDate { get; set; }

    /// <summary>
    /// 生产派工.
    /// </summary>
    public string? F_ProdDispatch { get; set; }

    /// <summary>
    /// 质检派工.
    /// </summary>
    public string? F_QcDispatch { get; set; }

    /// <summary>
    /// 任务开始时间.
    /// </summary>
    public string? F_StartDate { get; set; }

    /// <summary>
    /// 任务结束时间.
    /// </summary>
    public string? F_EndDate { get; set; }

    /// <summary>
    /// 生产数量.
    /// </summary>
    public int? F_ProdQty { get; set; }

    /// <summary>
    /// 完成数量.
    /// </summary>
    public int? F_CompletedQty { get; set; }

    /// <summary>
    /// 任务状态.
    /// </summary>
    public string? F_TaskStatus { get; set; }
    public string? F_TaskStatusCode { get; set; }

    /// <summary>
    /// 任务类型.
    /// </summary>
    public string? F_TaskType { get; set; }
    public string? F_TaskTypeCode { get; set; }

    /// <summary>
    /// 排序.
    /// </summary>
    public int? F_SortCode { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    public string? F_Description { get; set; }

    /// <summary>
    /// 暂停原因.
    /// </summary>
    public string? F_Reason { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

    /// <summary>
    /// 创建组织.
    /// </summary>
    public string? F_CreatorOrganizeId { get; set; }

}

public class AProdTaskAPPListOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// F_NodeId.
    /// </summary>
    public string? F_NodeId { get; set; }

    /// <summary>
    /// 上级工序ID.
    /// </summary>
    public string? F_ParentId { get; set; }

    /// <summary>
    /// 加工单ID.
    /// </summary>
    public string? F_ProcessingId { get; set; }

    /// <summary>
    /// 商品.
    /// </summary>
    public string? F_GoodsName { get; set; }

    /// <summary>
    /// 工序ID.
    /// </summary>
    public string? F_ProcessId { get; set; }
    public string? F_ProcessName { get; set; }

    /// <summary>
    /// 计划开始.
    /// </summary>
    public string? F_PlanStartDate { get; set; }

    /// <summary>
    /// 计划结束.
    /// </summary>
    public string? F_PlanEndDate { get; set; }

    /// <summary>
    /// 生产数量.
    /// </summary>
    public int? F_ProdQty { get; set; }

    /// <summary>
    /// 完成数量.
    /// </summary>
    public int? F_CompletedQty { get; set; }

    /// <summary>
    /// 任务状态.
    /// </summary>
    public string? F_TaskStatus { get; set; }
    public string? F_TaskStatusCode { get; set; }

    /// <summary>
    /// 排序.
    /// </summary>
    public int? F_SortCode { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    public string? F_Description { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

}