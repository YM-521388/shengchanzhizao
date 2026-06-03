using JNPF.Common.CodeGenUpload;
using SqlSugar;
using JNPF.Extras.DatabaseAccessor.SqlSugar.Models;

namespace JNPF.example.Entitys;

/// <summary>
/// 操作日志实体.
/// </summary>
[SugarTable("a_pu_return_prd_log")]
public class APuReturnPrdLogEntity : ITenantFilter
{
    /// <summary>
    /// 主键.
    /// </summary>
    [SugarColumn(ColumnName = "F_Id", IsPrimaryKey = true)]
    public string? F_Id { get; set; }

    /// <summary>
    /// 生产退料单Id.
    /// </summary>
    [SugarColumn(ColumnName = "F_ReturnInPRDId")]
    public string? F_ReturnInPRDId { get; set; }

    /// <summary>
    /// 类型.
    /// </summary>
    [SugarColumn(ColumnName = "F_Type")]
    public string? F_Type { get; set; }

    /// <summary>
    /// 标题.
    /// </summary>
    [SugarColumn(ColumnName = "F_Title")]
    public string? F_Title { get; set; }

    /// <summary>
    /// 内容.
    /// </summary>
    [SugarColumn(ColumnName = "F_Content")]
    public string? F_Content { get; set; }

    /// <summary>
    /// 修改原因.
    /// </summary>
    [SugarColumn(ColumnName = "F_Reason")]
    public string? F_Reason { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorUserId")]
    [CodeGenUpload("F_CreatorUserId", "{\n  \"tableName\": \"a_pu_return_prd_log\",\n  \"regList\": null,\n  \"jnpfKey\": \"createUser\",\n  \"rule\": null,\n  \"ruleType\": null,\n  \"dictionaryType\": null,\n  \"required\": false,\n  \"unique\": false,\n  \"label\": \"创建人员\",\n  \"dataType\": null,\n  \"propsUrl\": null,\n  \"children\": null,\n  \"props\": null,\n  \"showLevel\": null,\n  \"templateJson\": null,\n  \"startTimeRule\": false,\n  \"startTimeTarget\": 0,\n  \"startTimeType\": 0,\n  \"startTimeValue\": null,\n  \"startRelationField\": null,\n  \"endTimeRule\": false,\n  \"endTimeTarget\": 0,\n  \"endTimeType\": 0,\n  \"endTimeValue\": null,\n  \"endRelationField\": null,\n  \"precision\": null,\n  \"formId\": \"formItem44a793\",\n  \"ruleConfig\": null,\n  \"IsBusinessKey\": false,\n  \"tag\": \"JnpfOpenData\"\n}")]
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorTime")]
    [CodeGenUpload("F_CreatorTime", "{\n  \"tableName\": \"a_pu_return_prd_log\",\n  \"regList\": null,\n  \"jnpfKey\": \"createTime\",\n  \"rule\": null,\n  \"ruleType\": null,\n  \"dictionaryType\": null,\n  \"required\": false,\n  \"unique\": false,\n  \"label\": \"创建时间\",\n  \"dataType\": null,\n  \"propsUrl\": null,\n  \"children\": null,\n  \"props\": null,\n  \"showLevel\": null,\n  \"templateJson\": null,\n  \"startTimeRule\": false,\n  \"startTimeTarget\": 0,\n  \"startTimeType\": 0,\n  \"startTimeValue\": null,\n  \"startRelationField\": null,\n  \"endTimeRule\": false,\n  \"endTimeTarget\": 0,\n  \"endTimeType\": 0,\n  \"endTimeValue\": null,\n  \"endRelationField\": null,\n  \"precision\": null,\n  \"formId\": \"formItem4dcbc0\",\n  \"ruleConfig\": null,\n  \"IsBusinessKey\": false,\n  \"tag\": \"JnpfOpenData\"\n}")]
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

}