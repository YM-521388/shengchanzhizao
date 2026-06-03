using JNPF.Common.CodeGenUpload;
using SqlSugar;
using JNPF.Extras.DatabaseAccessor.SqlSugar.Models;

namespace JNPF.example.Entitys;

/// <summary>
/// a_pu_plan实体.
/// </summary>
[SugarTable("a_pu_plan")]
public class APuPlanEntity : ITenantFilter
{
    /// <summary>
    /// 主键.
    /// </summary>
    [SugarColumn(ColumnName = "F_Id", IsPrimaryKey = true)]
    public string? id { get; set; }

    /// <summary>
    /// 计划编号.
    /// </summary>
    [SugarColumn(ColumnName = "F_PlanNo")]
    public string? F_PlanNo { get; set; }

    /// <summary>
    /// 计划名称.
    /// </summary>
    [SugarColumn(ColumnName = "F_PlanName")]
    public string? F_PlanName { get; set; }

    /// <summary>
    /// 供应商ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_SupplierId")]
    public string? F_SupplierId { get; set; }

    /// <summary>
    /// 金额.
    /// </summary>
    [SugarColumn(ColumnName = "F_Money")]
    public decimal? F_Money { get; set; }

    /// <summary>
    /// 交货日期.
    /// </summary>
    [SugarColumn(ColumnName = "F_DeliveryDate")]
    public DateTime? F_DeliveryDate { get; set; }

    /// <summary>
    /// 采购数量.
    /// </summary>
    [SugarColumn(ColumnName = "F_PurchaseNum")]
    public decimal? F_PurchaseNum { get; set; }

    /// <summary>
    /// 负责人.
    /// </summary>
    [SugarColumn(ColumnName = "F_ResponsibleUserId")]
    public string? F_ResponsibleUserId { get; set; }

    /// <summary>
    /// 审核状态.
    /// </summary>
    [SugarColumn(ColumnName = "F_CheckState")]
    public string? F_CheckState { get; set; }

    /// <summary>
    /// 审核人员.
    /// </summary>
    [SugarColumn(ColumnName = "F_CheckUserId")]
    public string? F_CheckUserId { get; set; }

    /// <summary>
    /// 审核时间.
    /// </summary>
    [SugarColumn(ColumnName = "F_CheckTime")]
    public DateTime? F_CheckTime { get; set; }


    /// <summary>
    /// 备注.
    /// </summary>
    [SugarColumn(ColumnName = "F_Description")]
    public string? F_Description { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorUserId")]
    [CodeGenUpload("F_CreatorUserId", "{\r\n  \"tableName\": \"a_pu_plan\",\r\n  \"regList\": null,\r\n  \"jnpfKey\": \"createUser\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"创建人员\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItem9f47d8\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfOpenData\"\r\n}")]
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorTime")]
    [CodeGenUpload("F_CreatorTime", "{\r\n  \"tableName\": \"a_pu_plan\",\r\n  \"regList\": null,\r\n  \"jnpfKey\": \"createTime\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"创建时间\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItem524b66\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfOpenData\"\r\n}")]
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
    /// 采购计划商品信息.
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(APuPlanItemEntity.F_PlanId), nameof(id))]
    public List<APuPlanItemEntity> APuPlanItemList { get; set; }

    /// <summary>
    /// 采购计划日志.
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(APuPlanLogEntity.F_PlanId), nameof(id))]
    public List<APuPlanLogEntity> APuPlanLogList { get; set; }

}