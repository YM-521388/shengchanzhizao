using JNPF.Common.CodeGenUpload;
using SqlSugar;
using JNPF.Extras.DatabaseAccessor.SqlSugar.Models;

namespace JNPF.example.Entitys;

/// <summary>
/// 维修工单实体.
/// </summary>
[SugarTable("a_maint_repair")]
public class AMaintRepairEntity : ITenantFilter
{
    /// <summary>
    /// 主键.
    /// </summary>
    [SugarColumn(ColumnName = "F_Id", IsPrimaryKey = true)]
    public string? id { get; set; }

    /// <summary>
    /// 工单编号.
    /// </summary>
    [SugarColumn(ColumnName = "F_RepairNo")]
    public string? F_RepairNo { get; set; }

    /// <summary>
    /// 维修设备.
    /// </summary>
    [SugarColumn(ColumnName = "F_DeviceId")]
    [CodeGenUpload("F_DeviceId", "F_DeviceId", "2016476904974061568", "id", "F_DeviceName", "", "{\r\n  \"tableName\": \"a_maint_repair\",\r\n  \"regList\": [],\r\n  \"jnpfKey\": \"popupSelect\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": true,\r\n  \"unique\": false,\r\n  \"label\": \"维修设备\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItem53e724\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfPopupSelect\"\r\n}")]
    public string? F_DeviceId { get; set; }

    /// <summary>
    /// 分组.
    /// </summary>
    [SugarColumn(ColumnName = "F_GroupId")]
    public string? F_GroupId { get; set; }

    /// <summary>
    /// 派单方式.
    /// </summary>
    [SugarColumn(ColumnName = "F_DispatchType")]
    public string? F_DispatchType { get; set; }

    /// <summary>
    /// 处理人.
    /// </summary>
    [SugarColumn(ColumnName = "F_HandlerUserId")]
    public string? F_HandlerUserId { get; set; }

    /// <summary>
    /// 故障描述.
    /// </summary>
    [SugarColumn(ColumnName = "F_Description")]
    public string? F_Description { get; set; }

    /// <summary>
    /// 接单时间.
    /// </summary>
    [SugarColumn(ColumnName = "F_ReceiveTime")]
    public DateTime? F_ReceiveTime { get; set; }

    /// <summary>
    /// 处理时间.
    /// </summary>
    [SugarColumn(ColumnName = "F_HandleTime")]
    public DateTime? F_HandleTime { get; set; }

    /// <summary>
    /// 状态.
    /// </summary>
    [SugarColumn(ColumnName = "F_State")]
    public string? F_State { get; set; }

    /// <summary>
    /// 暂停原因.
    /// </summary>
    [SugarColumn(ColumnName = "F_PauseReason")]
    public string? F_PauseReason { get; set; }

    /// <summary>
    /// 取消原因.
    /// </summary>
    [SugarColumn(ColumnName = "F_CancelReason")]
    public string? F_CancelReason { get; set; }

    /// <summary>
    /// 审核人.
    /// </summary>
    [SugarColumn(ColumnName = "F_AuditorUserId")]
    public string? F_AuditorUserId { get; set; }
    
    /// <summary>
    /// 审核时间.
    /// </summary>
    [SugarColumn(ColumnName = "F_AuditTime")]
    public DateTime? F_AuditTime { get; set; }

    /// <summary>
    /// 审核备注.
    /// </summary>
    [SugarColumn(ColumnName = "F_AuditReason")]
    public string? F_AuditReason { get; set; }

    /// <summary>
    /// 是否解决问题.
    /// </summary>
    [SugarColumn(ColumnName = "F_IsSolved")]
    public string? F_IsSolved { get; set; }

    /// <summary>
    /// 处理结果.
    /// </summary>
    [SugarColumn(ColumnName = "F_HandleResult")]
    public string? F_HandleResult { get; set; }

    /// <summary>
    /// 报修人.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorUserId")]
    [CodeGenUpload("F_CreatorUserId", "{\r\n  \"tableName\": \"a_maint_repair\",\r\n  \"regList\": null,\r\n  \"jnpfKey\": \"createUser\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"报修人\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItemff9448\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfOpenData\"\r\n}")]
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 报修时间.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorTime")]
    [CodeGenUpload("F_CreatorTime", "{\r\n  \"tableName\": \"a_maint_repair\",\r\n  \"regList\": null,\r\n  \"jnpfKey\": \"createTime\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"报修时间\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItem37d140\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfOpenData\"\r\n}")]
    public DateTime? F_CreatorTime { get; set; }

    /// <summary>
    /// 创建组织.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorOrganizeId")]
    [CodeGenUpload("F_CreatorOrganizeId", "{\r\n  \"tableName\": \"a_maint_repair\",\r\n  \"regList\": null,\r\n  \"jnpfKey\": \"currOrganize\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"创建组织\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": \"last\",\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItem4e298a\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfOpenData\"\r\n}")]
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