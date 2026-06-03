using JNPF.Common.CodeGenUpload;
using SqlSugar;
using JNPF.Extras.DatabaseAccessor.SqlSugar.Models;
using JNPF.example.Entitys.Dto.AProdRoutingStepItem;

namespace JNPF.example.Entitys;

/// <summary>
/// 工序实体.
/// </summary>
[SugarTable("a_prod_routing_step")]
public class AProdRoutingStepEntity : ITenantFilter
{
    /// <summary>
    /// 主键.
    /// </summary>
    [SugarColumn(ColumnName = "F_Id", IsPrimaryKey = true)]
    public string? F_Id { get; set; }

    /// <summary>
    /// 工艺路线ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_RoutingId")]
    public string? F_RoutingId { get; set; }

    /// <summary>
    /// 工序ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_ProcessId")]
    [CodeGenUpload("F_ProcessId", "F_ProcessId", "2011984543933927424", "F_Id", "F_OrderNo", "", "{\r\n  \"tableName\": \"a_prod_routing_step\",\r\n  \"regList\": [],\r\n  \"jnpfKey\": \"popupSelect\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"工序\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItemeebd1c\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfPopupSelect\"\r\n}")]
    public string? F_ProcessId { get; set; }

    /// <summary>
    /// F_BomId.
    /// </summary>
    [SugarColumn(ColumnName = "F_BOMId")]
    public string? F_BOMId { get; set; }

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
    /// 工资单价.
    /// </summary>
    [SugarColumn(ColumnName = "F_WagePrice")]
    public decimal? F_WagePrice { get; set; }

    /// <summary>
    /// 标准工时(时).
    /// </summary>
    [SugarColumn(ColumnName = "F_StdHour")]
    public int? F_StdHour { get; set; }

    /// <summary>
    /// 标准工时(分).
    /// </summary>
    [SugarColumn(ColumnName = "F_StdMinute")]
    public int? F_StdMinute { get; set; }

    /// <summary>
    /// 标准工时(秒).
    /// </summary>
    [SugarColumn(ColumnName = "F_StdSecond")]
    public int? F_StdSecond { get; set; }

    /// <summary>
    /// 计价方式.
    /// </summary>
    [SugarColumn(ColumnName = "F_PriceType")]
    public string? F_PriceType { get; set; }

    /// <summary>
    /// 单位关系(生产).
    /// </summary>
    [SugarColumn(ColumnName = "F_UnitRatioProd")]
    public int? F_UnitRatioProd { get; set; }

    /// <summary>
    /// 单位关系(报工).
    /// </summary>
    [SugarColumn(ColumnName = "F_UnitRatioReport")]
    public int? F_UnitRatioReport { get; set; }

    /// <summary>
    /// 单位关系(单位).
    /// </summary>
    [SugarColumn(ColumnName = "F_UnitRatioBase")]
    public string? F_UnitRatioBase { get; set; }

    /// <summary>
    /// 工序需外协.
    /// </summary>
    [SugarColumn(ColumnName = "F_IsOutsource")]
    public string? F_IsOutsource { get; set; }

    /// <summary>
    /// 工序需质检.
    /// </summary>
    [SugarColumn(ColumnName = "F_IsQc")]
    public string? F_IsQc { get; set; }

    /// <summary>
    /// 不良品需报废、返工.
    /// </summary>
    [SugarColumn(ColumnName = "F_DefectHandle")]
    public string? F_DefectHandle { get; set; }

    /// <summary>
    /// 生产任务计时.
    /// </summary>
    [SugarColumn(ColumnName = "F_TaskTimer")]
    public string? F_TaskTimer { get; set; }

    /// <summary>
    /// 附件.
    /// </summary>
    [SugarColumn(ColumnName = "F_Files")]
    public string? F_Files { get; set; }

    /// <summary>
    /// 排序.
    /// </summary>
    [SugarColumn(ColumnName = "F_SortCode")]
    public int? F_SortCode { get; set; }

    /// <summary>
    /// 上级节点ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_ParentId")]
    public string? F_ParentId { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    [SugarColumn(ColumnName = "F_Description")]
    public string? F_Description { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorTime")]
    [CodeGenUpload("F_CreatorTime", "{\r\n  \"tableName\": \"a_prod_routing_step\",\r\n  \"regList\": null,\r\n  \"jnpfKey\": \"createTime\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"创建时间\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItem34bb3f\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfOpenData\"\r\n}")]
    public DateTime? F_CreatorTime { get; set; }

    /// <summary>
    /// 逻辑删除.
    /// </summary>
    [SugarColumn(ColumnName = "F_Delete_Mark")]
    public int? DeleteMark{ get; set; }

    /// <summary>
    /// 获取或设置 删除时间.
    /// </summary>
    [SugarColumn(ColumnName = "F_DELETE_TIME", ColumnDescription = "删除时间")]
    public DateTime? DeleteTime { get; set; }

    /// <summary>
    /// 获取或设置 删除用户.
    /// </summary>
    [SugarColumn(ColumnName = "F_DELETE_USER_ID", ColumnDescription = "删除用户")]
    public string DeleteUserId { get; set; }


    /// <summary>
    /// 获取或设置 租户id.
    /// </summary>
    [SugarColumn(ColumnName = "F_TENANT_ID", ColumnDescription = "租户id", IsPrimaryKey = true)]
    public string TenantId { get; set; }

    /// <summary>
    /// 工序物料信息.
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public List<AProdRoutingStepItemCrInput>  tableField3b6f08 { get; set; }

    [Navigate(NavigateType.OneToMany, nameof(AProdRoutingStepItemEntity.F_StepId), nameof(F_Id))]
    public List<AProdRoutingStepItemEntity> AProdRoutingStepItemList { get; set; }
}