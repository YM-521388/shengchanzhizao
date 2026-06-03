using JNPF.Common.Models;
using JNPF.example.Entitys.Dto.AProdTaskItem;
using JNPF.JsonSerialization;
using Newtonsoft.Json;
using SqlSugar;

namespace JNPF.example.Entitys.Dto.AProdTask;
 
/// <summary>
/// 生产任务修改输入参数.
/// </summary>
public class AProdTaskCrInput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string F_Id { get; set; }

    /// <summary>
    /// 加工单ID.
    /// </summary>
    public string F_ProcessingId { get; set; }

    /// <summary>
    /// 工序ID.
    /// </summary>
    public string F_ProcessId { get; set; }

    /// <summary>
    /// 计划开始.
    /// </summary>
    public DateTime? F_PlanStartDate { get; set; }

    /// <summary>
    /// 计划结束.
    /// </summary>
    public DateTime? F_PlanEndDate { get; set; }

    /// <summary>
    /// 生产派工.
    /// </summary>
    public List<string> F_ProdDispatch { get; set; }

    /// <summary>
    /// 质检派工.
    /// </summary>
    public List<string> F_QcDispatch { get; set; }

    /// <summary>
    /// 生产数量.
    /// </summary>
    public int? F_ProdQty { get; set; }

    /// <summary>
    /// 任务状态.
    /// </summary>
    public string F_TaskStatus { get; set; }

    /// <summary>
    /// 任务类型.
    /// </summary>
    public string F_TaskType { get; set; }

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
    /// 生产任务物料清单.
    /// </summary>
    public List<AProdTaskItemCrInput> tableField0bb944 { get; set; }

}