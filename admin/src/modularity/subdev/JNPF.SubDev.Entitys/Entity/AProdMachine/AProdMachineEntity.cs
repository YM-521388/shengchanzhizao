using JNPF.Common.CodeGenUpload;
using SqlSugar;
using JNPF.Extras.DatabaseAccessor.SqlSugar.Models;

namespace JNPF.example.Entitys;

/// <summary>
/// 机台管理实体.
/// </summary>
[SugarTable("a_prod_machine")]
public class AProdMachineEntity : ITenantFilter
{
    /// <summary>
    /// 主键.
    /// </summary>
    [SugarColumn(ColumnName = "F_Id", IsPrimaryKey = true)]
    public string? id { get; set; }

    /// <summary>
    /// 机台编号.
    /// </summary>
    [SugarColumn(ColumnName = "F_MachineCode")]
    public string? F_MachineCode { get; set; }

    /// <summary>
    /// 机台名称.
    /// </summary>
    [SugarColumn(ColumnName = "F_MachineName")]
    public string? F_MachineName { get; set; }

    /// <summary>
    /// 机台规格.
    /// </summary>
    [SugarColumn(ColumnName = "F_MachineSpec")]
    public string? F_MachineSpec { get; set; }

    /// <summary>
    /// 机台状态.
    /// </summary>
    [SugarColumn(ColumnName = "F_MachineStatus")]
    public string? F_MachineStatus { get; set; }

    /// <summary>
    /// 机台图片.
    /// </summary>
    [SugarColumn(ColumnName = "F_Image")]
    public string? F_Image { get; set; }

    /// <summary>
    /// 机台类别.
    /// </summary>
    [SugarColumn(ColumnName = "F_MachineType")]
    public string? F_MachineType { get; set; }

    /// <summary>
    /// 车间.
    /// </summary>
    [SugarColumn(ColumnName = "F_WorkshopId")]
    public string? F_WorkshopId { get; set; }

    /// <summary>
    /// 产线.
    /// </summary>
    [SugarColumn(ColumnName = "F_LineId")]
    public string? F_LineId { get; set; }

    /// <summary>
    /// 单日运行时长.
    /// </summary>
    [SugarColumn(ColumnName = "F_DailyRunHours")]
    public int? F_DailyRunHours { get; set; }

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
    /// 标准产出.
    /// </summary>
    [SugarColumn(ColumnName = "F_StdOutput")]
    public int? F_StdOutput { get; set; }

    /// <summary>
    /// 状态.
    /// </summary>
    [SugarColumn(ColumnName = "F_State")]
    public string? F_State { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorUserId")]
    [CodeGenUpload("F_CreatorUserId", "{\r\n  \"tableName\": \"a_prod_machine\",\r\n  \"regList\": null,\r\n  \"jnpfKey\": \"createUser\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"创建人员\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItem7c1f80\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfOpenData\"\r\n}")]
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorTime")]
    [CodeGenUpload("F_CreatorTime", "{\r\n  \"tableName\": \"a_prod_machine\",\r\n  \"regList\": null,\r\n  \"jnpfKey\": \"createTime\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"创建时间\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItem5c61fb\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfOpenData\"\r\n}")]
    public DateTime? F_CreatorTime { get; set; }

    /// <summary>
    /// 创建组织.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorOrganizeId")]
    public string? F_CreatorOrganizeId { get; set; }

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