using JNPF.Common.CodeGenUpload;
using SqlSugar;
using JNPF.Extras.DatabaseAccessor.SqlSugar.Models;

namespace JNPF.example.Entitys;

/// <summary>
/// 工序管理实体.
/// </summary>
[SugarTable("a_prod_process")]
public class AProdProcessEntity : ITenantFilter
{
    /// <summary>
    /// 主键.
    /// </summary>
    [SugarColumn(ColumnName = "F_Id", IsPrimaryKey = true)]
    public string? id { get; set; }

    /// <summary>
    /// 工序名.
    /// </summary>
    [SugarColumn(ColumnName = "F_ProcessName")]
    public string? F_ProcessName { get; set; }

    /// <summary>
    /// 工序编号.
    /// </summary>
    [SugarColumn(ColumnName = "F_ProcessCode")]
    public string? F_ProcessCode { get; set; }

    /// <summary>
    /// 车间ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_WorkshopId")]
    public string? F_WorkshopId { get; set; }

    /// <summary>
    /// 产线.
    /// </summary>
    [SugarColumn(ColumnName = "F_Line")]
    public string? F_Line { get; set; }

    /// <summary>
    /// 报工单位.
    /// </summary>
    [SugarColumn(ColumnName = "F_ReportUnit")]
    public string? F_ReportUnit { get; set; }

    /// <summary>
    /// 单位关系.
    /// </summary>
    [SugarColumn(ColumnName = "F_UnitRatio")]
    public int? F_UnitRatio { get; set; }

    /// <summary>
    /// 计价方式.
    /// </summary>
    [SugarColumn(ColumnName = "F_PriceType")]
    public string? F_PriceType { get; set; }

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
    /// 附件.
    /// </summary>
    [SugarColumn(ColumnName = "F_Files")]
    public string? F_Files { get; set; }

    /// <summary>
    /// 机台ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_MachineId")]
    [CodeGenUpload("F_MachineId", "F_MachineId", "2011729284707782656", "id", "F_MachineName", "", "{\r\n  \"tableName\": \"a_prod_process\",\r\n  \"regList\": [],\r\n  \"jnpfKey\": \"popupSelect\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"机台\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItem1fbce6\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfPopupSelect\"\r\n}")]
    public string? F_MachineId { get; set; }

    /// <summary>
    /// 生产人员.
    /// </summary>
    [SugarColumn(ColumnName = "F_ProdUserIds")]
    public string? F_ProdUserIds { get; set; }

    /// <summary>
    /// 不良品项ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_DefectIds")]
    public string? F_DefectIds { get; set; }

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
    /// 质检人员.
    /// </summary>
    [SugarColumn(ColumnName = "F_QcUserIds")]
    public string? F_QcUserIds { get; set; }

    /// <summary>
    /// 生产任务计时.
    /// </summary>
    [SugarColumn(ColumnName = "F_TaskTimer")]
    public string? F_TaskTimer { get; set; }

    /// <summary>
    /// 状态.
    /// </summary>
    [SugarColumn(ColumnName = "F_State")]
    public string? F_State { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    [SugarColumn(ColumnName = "F_Description")]
    public string? F_Description { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorTime")]
    [CodeGenUpload("F_CreatorTime", "{\r\n  \"tableName\": \"a_prod_process\",\r\n  \"regList\": null,\r\n  \"jnpfKey\": \"createTime\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"创建时间\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItem1ec919\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfOpenData\"\r\n}")]
    public DateTime? F_CreatorTime { get; set; }

    /// <summary>
    /// 创建组织.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorOrganizeId")]
    public string? F_CreatorOrganizeId { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorUserId")]
    [CodeGenUpload("F_CreatorUserId", "{\r\n  \"tableName\": \"a_prod_process\",\r\n  \"regList\": null,\r\n  \"jnpfKey\": \"createUser\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"创建人员\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItem6baedb\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfOpenData\"\r\n}")]
    public string? F_CreatorUserId { get; set; }

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
    /// 流程引擎ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_Flow_Id")]
    public string FlowId { get; set; }
    
    /// <summary>
    /// 获取或设置 租户id.
    /// </summary>
    [SugarColumn(ColumnName = "F_TENANT_ID", ColumnDescription = "租户id", IsPrimaryKey = true)]
    public string TenantId { get; set; }
}