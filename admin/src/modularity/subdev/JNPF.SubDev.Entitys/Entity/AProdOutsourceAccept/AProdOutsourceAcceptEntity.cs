using JNPF.Common.CodeGenUpload;
using SqlSugar;
using JNPF.Extras.DatabaseAccessor.SqlSugar.Models;

namespace JNPF.example.Entitys;

/// <summary>
/// a_prod_outsource_accept实体.
/// </summary>
[SugarTable("a_prod_outsource_accept")]
public class AProdOutsourceAcceptEntity : ITenantFilter
{
    /// <summary>
    /// 主键.
    /// </summary>
    [SugarColumn(ColumnName = "F_Id", IsPrimaryKey = true)]
    public string? id { get; set; }

    /// <summary>
    /// 外协工单ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_OutsourceId")]
    [CodeGenUpload("F_OutsourceId", "F_OutsourceId", "2014180492177444864", "F_Id", "F_OutsourceNo", "", "{\r\n  \"tableName\": \"a_prod_outsource_accept\",\r\n  \"regList\": [],\r\n  \"jnpfKey\": \"popupSelect\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": true,\r\n  \"unique\": false,\r\n  \"label\": \"外协工单\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItemb5a5af\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfPopupSelect\"\r\n}")]
    public string? F_OutsourceId { get; set; }

    /// <summary>
    /// 合格数量.
    /// </summary>
    [SugarColumn(ColumnName = "F_PassNum")]
    public int? F_PassNum { get; set; }

    /// <summary>
    /// 不合格数量.
    /// </summary>
    [SugarColumn(ColumnName = "F_FailNum")]
    public int? F_FailNum { get; set; }

    /// <summary>
    /// 外协类型.
    /// </summary>
    [SugarColumn(ColumnName = "F_OutsourceType")]
    public string? F_OutsourceType { get; set; }

    /// <summary>
    /// 结算状态.
    /// </summary>
    [SugarColumn(ColumnName = "F_SettleStatus")]
    public string? F_SettleStatus { get; set; }

    /// <summary>
    /// 结算单价.
    /// </summary>
    [SugarColumn(ColumnName = "F_SettleUnitPrice")]
    public decimal? F_SettleUnitPrice { get; set; }

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
    /// 结算附件.
    /// </summary>
    [SugarColumn(ColumnName = "F_SettleFiles")]
    public string? F_SettleFiles { get; set; }

    /// <summary>
    /// 结算备注.
    /// </summary>
    [SugarColumn(ColumnName = "F_SettleDesc")]
    public string? F_SettleDesc { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    [SugarColumn(ColumnName = "F_Description")]
    public string? F_Description { get; set; }

    /// <summary>
    /// 创建用户.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorUserId")]
    [CodeGenUpload("F_CreatorUserId", "{\r\n  \"tableName\": \"a_prod_outsource_accept\",\r\n  \"regList\": null,\r\n  \"jnpfKey\": \"createUser\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"创建用户\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItem12ce6b\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfOpenData\"\r\n}")]
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorTime")]
    [CodeGenUpload("F_CreatorTime", "{\r\n  \"tableName\": \"a_prod_outsource_accept\",\r\n  \"regList\": null,\r\n  \"jnpfKey\": \"createTime\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"创建时间\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItemb33c9e\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfOpenData\"\r\n}")]
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
    /// 外协验收内容.
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(AProdOutsourceAcceptItemEntity.F_OutsourceAcceptId), nameof(id))]
    public List<AProdOutsourceAcceptItemEntity> AProdOutsourceAcceptItemList { get; set; }

    /// <summary>
    /// 外协结算日志.
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(AProdOutsourceSettleLogEntity.F_AcceptId), nameof(id))]
    public List<AProdOutsourceSettleLogEntity> AProdOutsourceSettleLogList { get; set; }

}