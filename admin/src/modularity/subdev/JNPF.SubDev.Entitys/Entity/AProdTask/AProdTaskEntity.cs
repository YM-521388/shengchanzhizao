using JNPF.Common.CodeGenUpload;
using SqlSugar;
using JNPF.Extras.DatabaseAccessor.SqlSugar.Models;
using JNPF.example.Entitys.Dto.AProdTaskItem;

namespace JNPF.example.Entitys;

/// <summary>
/// 生产任务实体.
/// </summary>
[SugarTable("a_prod_task")]
public class AProdTaskEntity : ITenantFilter
{
    /// <summary>
    /// 主键.
    /// </summary>
    [SugarColumn(ColumnName = "F_Id", IsPrimaryKey = true)]
    public string? id { get; set; }

   /// <summary>
    /// 加工单ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_ProcessingId")]
    public string? F_ProcessingId { get; set; }

    /// <summary>
    /// 上级工序ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_ParentId")]
    public string? F_ParentId { get; set; }

    /// <summary>
    /// 工序ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_ProcessId")]
    public string? F_ProcessId { get; set; }

    /// <summary>
    /// 节点ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_NodeId")]
    public string? F_NodeId { get; set; }

    /// <summary>
    /// 节点名称.
    /// </summary>
    [SugarColumn(ColumnName = "F_NodeName")]
    public string? F_NodeName { get; set; }

    /// <summary>
    /// 类型(节点类型).
    /// </summary>
    [SugarColumn(ColumnName = "F_Type")]
    public string? F_Type { get; set; }

    /// <summary>
    /// 负责人.
    /// </summary>
    [SugarColumn(ColumnName = "F_ResponsibleUserId")]
    public string? F_ResponsibleUserId { get; set; }

    /// <summary>
    /// 计划开始.
    /// </summary>
    [SugarColumn(ColumnName = "F_PlanStartDate")]
    public DateTime? F_PlanStartDate { get; set; }

    /// <summary>
    /// 计划结束.
    /// </summary>
    [SugarColumn(ColumnName = "F_PlanEndDate")]
    public DateTime? F_PlanEndDate { get; set; }

    /// <summary>
    /// 生产派工.
    /// </summary>
    [SugarColumn(ColumnName = "F_ProdDispatch")]
    public string? F_ProdDispatch { get; set; }

    /// <summary>
    /// 质检派工.
    /// </summary>
    [SugarColumn(ColumnName = "F_QcDispatch")]
    public string? F_QcDispatch { get; set; }

    /// <summary>
    /// 任务开始时间.
    /// </summary>
    [SugarColumn(ColumnName = "F_StartDate")]
    public DateTime? F_StartDate { get; set; }

    /// <summary>
    /// 任务结束时间.
    /// </summary>
    [SugarColumn(ColumnName = "F_EndDate")]
    public DateTime? F_EndDate { get; set; }

    /// <summary>
    /// 生产数量.
    /// </summary>
    [SugarColumn(ColumnName = "F_ProdQty")]
    public int? F_ProdQty { get; set; }

    /// <summary>
    /// 任务状态.
    /// </summary>
    [SugarColumn(ColumnName = "F_TaskStatus")]
    public string? F_TaskStatus { get; set; }

    /// <summary>
    /// 任务类型.
    /// </summary>
    [SugarColumn(ColumnName = "F_TaskType")]
    public string? F_TaskType { get; set; }

    /// <summary>
    /// 排序.
    /// </summary>
    [SugarColumn(ColumnName = "F_SortCode")]
    public int? F_SortCode { get; set; }

    /// <summary>
    /// 暂停原因.
    /// </summary>
    [SugarColumn(ColumnName = "F_Reason")]
    public string? F_Reason { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    [SugarColumn(ColumnName = "F_Description")]
    public string? F_Description { get; set; }

    /// <summary>
    /// 创建用户.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorUserId")]
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorTime")]
    [CodeGenUpload("F_CreatorTime", "{\r\n  \"tableName\": \"a_prod_task\",\r\n  \"regList\": null,\r\n  \"jnpfKey\": \"createTime\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"创建时间\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItem3a6761\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfOpenData\"\r\n}")]
    public DateTime? F_CreatorTime { get; set; }

    /// <summary>
    /// 创建组织.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorOrganizeId")]
    public string? F_CreatorOrganizeId { get; set; }

    /// <summary>
    /// 流程引擎ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_Flow_Id")]
    public string FlowId { get; set; }
    
    /// <summary>
    /// 获取或设置 租户id.
    /// </summary>
    [SugarColumn(ColumnName = "F_TENANT_ID", ColumnDescription = "租户id", IsPrimaryKey = true)]
    public string TenantId { get; set; }

    /// <summary>
    /// 生产任务物料清单.
    /// </summary>
    /// <summary>
    [SugarColumn(IsIgnore = true)]
    public List<AProdTaskItemCrInput> tableField0bb944 { get; set; }

    [Navigate(NavigateType.OneToMany, nameof(AProdTaskItemEntity.F_TaskId), nameof(id))]
    public List<AProdTaskItemEntity> AProdTaskItemList { get; set; }


}