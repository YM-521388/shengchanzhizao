using JNPF.Common.CodeGenUpload;
using SqlSugar;
using JNPF.Extras.DatabaseAccessor.SqlSugar.Models;

namespace JNPF.example.Entitys;

/// <summary>
/// a_prod_report实体.
/// </summary>
[SugarTable("a_prod_report")]
public class AProdReportEntity : ITenantFilter
{
    /// <summary>
    /// 主键.
    /// </summary>
    [SugarColumn(ColumnName = "F_Id", IsPrimaryKey = true)]
    public string? id { get; set; }

    /// <summary>
    /// 生产任务ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_TaskId")]
    public string? F_TaskId { get; set; }

    /// <summary>
    /// 生产人员.
    /// </summary>
    [SugarColumn(ColumnName = "F_ProdUserId")]
    public string? F_ProdUserId { get; set; }

    /// <summary>
    /// 良品数.
    /// </summary>
    [SugarColumn(ColumnName = "F_GoodQty")]
    public int? F_GoodQty { get; set; }

    /// <summary>
    /// 不良品数.
    /// </summary>
    [SugarColumn(ColumnName = "F_GoodNotQty")]
    public int? F_GoodNotQty { get; set; }

    /// <summary>
    /// 开始时间.
    /// </summary>
    [SugarColumn(ColumnName = "F_StartTime")]
    public DateTime? F_StartTime { get; set; }

    /// <summary>
    /// 结束时间.
    /// </summary>
    [SugarColumn(ColumnName = "F_EndTime")]
    public DateTime? F_EndTime { get; set; }

    /// <summary>
    /// 附件.
    /// </summary>
    [SugarColumn(ColumnName = "F_Files")]
    public string? F_Files { get; set; }

    /// <summary>
    /// 结算状态.
    /// </summary>
    [SugarColumn(ColumnName = "F_SettleStatus")]
    public string? F_SettleStatus { get; set; }

    /// <summary>
    /// 结算人.
    /// </summary>
    [SugarColumn(ColumnName = "F_SettleUserId")]
    public string? F_SettleUserId { get; set; }

    /// <summary>
    /// 结算时间.
    /// </summary>
    [SugarColumn(ColumnName = "F_SettleTime")]
    public DateTime? F_SettleTime { get; set; }

    /// <summary>
    /// 工资单价.
    /// </summary>
    [SugarColumn(ColumnName = "F_WagePrice")]
    public decimal? F_WagePrice { get; set; }

    /// <summary>
    /// 报工类型.
    /// </summary>
    [SugarColumn(ColumnName = "F_State")]
    public string? F_State { get; set; }

    /// <summary>
    /// 描述.
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
    [CodeGenUpload("F_CreatorTime", "{\r\n  \"tableName\": \"a_prod_report\",\r\n  \"regList\": null,\r\n  \"jnpfKey\": \"createTime\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"创建时间\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItemdfe45d\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfOpenData\"\r\n}")]
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

    /// <summary>
    /// 报工不良品项信息.
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(AProdReportDefectEntity.F_ReportId), nameof(id))]
    public List<AProdReportDefectEntity> AProdReportDefectList { get; set; }

}