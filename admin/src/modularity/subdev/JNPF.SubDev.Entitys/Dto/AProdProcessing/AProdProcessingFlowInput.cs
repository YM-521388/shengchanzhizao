using JNPF.Common.Models;
using JNPF.example.Entitys.Dto.AProdTaskItem;
using Newtonsoft.Json;
using SqlSugar;

namespace JNPF.example.Entitys.Dto.AProdProcessing;

/// <summary>
/// 加工单流程输入参数.
/// </summary>
public class AProdProcessingFlowInput
{
    /// <summary>
    /// 路线ID.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 路线编号.
    /// </summary>
    public string? F_RoutingNo { get; set; }

    /// <summary>
    /// 路线名称.
    /// </summary>
    public string? F_RoutingName { get; set; }

    /// <summary>
    /// 版本ID.
    /// </summary>
    public string? flowId { get; set; }

    /// <summary>
    /// XML字符串(encodeURIComponent编码).
    /// </summary>
    public string? flowXml { get; set; }

    /// <summary>
    /// 流程节点.
    /// </summary>
    public Dictionary<string, FlowNode>? flowNodes { get; set; }

}

/// <summary>
/// 流程节点.
/// </summary>
public class FlowNode
{
    /// <summary>
    /// 生产任务ID.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 节点ID.
    /// </summary>
    public string? nodeId { get; set; }

    /// <summary>
    /// 节点类型.
    /// </summary>
    public string? type { get; set; }

    /// <summary>
    /// 节点名称.
    /// </summary>
    public string? nodeName { get; set; }

    /// <summary>
    /// 负责人回显.
    /// </summary>
    public string? content { get; set; }

    /// <summary>
    /// 表单ID.
    /// </summary>
    public string? formId { get; set; }

    /// <summary>
    /// 表单名称.
    /// </summary>
    public string? formName { get; set; }

    /// <summary>
    /// 任务状态.
    /// </summary>
    public string? F_TaskStatusCode { get; set; }

    /// <summary>
    /// 节点内容.
    /// </summary>
    public FlowNodeContent? formData { get; set; }

}


/// <summary>
/// 节点内容.
/// </summary>
public class FlowNodeContent
{
    /// <summary>
    /// F_BomId.
    /// </summary>
    public string? F_ProcessId { get; set; }

    /// <summary>
    /// F_BomId.
    /// </summary>
    public string? F_BOMId { get; set; }

    /// <summary>
    /// 加工单号.
    /// </summary>
    public string? F_ProcessNo { get; set; }

    /// <summary>
    /// 负责人.
    /// </summary>
    public string? F_ResponsibleUserId { get; set; }

    /// <summary>
    /// 工资单价.
    /// </summary>
    public decimal? F_WagePrice { get; set; }

    /// <summary>
    /// 标准工时(时).
    /// </summary>
    public int? F_StdHour { get; set; }

    /// <summary>
    /// 标准工时(分).
    /// </summary>
    public int? F_StdMinute { get; set; }

    /// <summary>
    /// 标准工时(秒).
    /// </summary>
    public int? F_StdSecond { get; set; }

    /// <summary>
    /// 计价方式.
    /// </summary>
    public string F_PriceType { get; set; }

    /// <summary>
    /// 单位关系(生产).
    /// </summary>
    public int? F_UnitRatio { get; set; }

    /// <summary>
    /// 单位关系(单位).
    /// </summary>
    public string F_ReportUnit { get; set; }

    /// <summary>
    /// 工序需外协.
    /// </summary>
    public string? F_IsOutsource { get; set; }

    /// <summary>
    /// 工序需质检.
    /// </summary>
    public string? F_IsQc { get; set; }

    /// <summary>
    /// 不良品需报废、返工.
    /// </summary>
    public string? F_DefectHandle { get; set; }

    /// <summary>
    /// 附件.
    /// </summary>
    public List<FileControlsModel>? F_Files { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    public string? F_Description { get; set; }




    /// <summary>
    /// 生产派工.
    /// </summary>
    public string? F_ProdDispatch { get; set; }

    /// <summary>
    /// 质检派工.
    /// </summary>
    public string? F_QcDispatch { get; set; }

    /// <summary>
    /// 计划开始.
    /// </summary>
    public DateTime? F_PlanStartDate { get; set; }

    /// <summary>
    /// 计划结束.
    /// </summary>
    public DateTime? F_PlanEndDate { get; set; }

    /// <summary>
    /// 生产数量.
    /// </summary>
    public int? F_ProdQty { get; set; }

    /// <summary>
    /// 任务状态.
    /// </summary>
    public string? F_TaskStatus { get; set; }


    /// <summary>
    /// 生产计划物料信息.
    /// </summary>
    public List<AProdTaskItemCrInput> tableField0bb944 { get; set; }
}
