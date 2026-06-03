using JNPF.Common.CodeGenUpload;
using SqlSugar;
using JNPF.Extras.DatabaseAccessor.SqlSugar.Models;

namespace JNPF.example.Entitys;

/// <summary>
/// a_pu_return_prd实体.
/// </summary>
[SugarTable("a_pu_return_prd")]
public class APuReturnPrdEntity : ITenantFilter
{
    /// <summary>
    /// 主键.
    /// </summary>
    [SugarColumn(ColumnName = "F_Id", IsPrimaryKey = true)]
    public string? id { get; set; }

    /// <summary>
    /// 生产退料单号.
    /// </summary>
    [SugarColumn(ColumnName = "F_ReturnInNo")]
    public string? F_ReturnInNo { get; set; }

    /// <summary>
    /// 仓库ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_WarehouseId")]
    public string? F_WarehouseId { get; set; }

    /// <summary>
    /// 入库类型.
    /// </summary>
    [SugarColumn(ColumnName = "F_ReturnInType")]
    public string? F_ReturnInType { get; set; }

    /// <summary>
    /// 退料日期.
    /// </summary>
    [SugarColumn(ColumnName = "F_ReturnInDate")]
    public DateTime? F_ReturnInDate { get; set; }

    /// <summary>
    /// 工单类型.
    /// </summary>
    [SugarColumn(ColumnName = "F_Type")]
    public string? F_Type { get; set; }

    /// <summary>
    /// 工单ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_WorkOrderId")]
    public string? F_WorkOrderId { get; set; }

    /// <summary>
    /// 操作方式.
    /// </summary>
    [SugarColumn(ColumnName = "F_Method")]
    public string? F_Method { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    [SugarColumn(ColumnName = "F_Description")]
    public string? F_Description { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorUserId")]
    [CodeGenUpload("F_CreatorUserId", "{\n  \"tableName\": \"a_pu_return_prd\",\n  \"regList\": null,\n  \"jnpfKey\": \"createUser\",\n  \"rule\": null,\n  \"ruleType\": null,\n  \"dictionaryType\": null,\n  \"required\": false,\n  \"unique\": false,\n  \"label\": \"创建人员\",\n  \"dataType\": null,\n  \"propsUrl\": null,\n  \"children\": null,\n  \"props\": null,\n  \"showLevel\": null,\n  \"templateJson\": null,\n  \"startTimeRule\": false,\n  \"startTimeTarget\": 0,\n  \"startTimeType\": 0,\n  \"startTimeValue\": null,\n  \"startRelationField\": null,\n  \"endTimeRule\": false,\n  \"endTimeTarget\": 0,\n  \"endTimeType\": 0,\n  \"endTimeValue\": null,\n  \"endRelationField\": null,\n  \"precision\": null,\n  \"formId\": \"formItem268125\",\n  \"ruleConfig\": null,\n  \"IsBusinessKey\": false,\n  \"tag\": \"JnpfOpenData\"\n}")]
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorTime")]
    [CodeGenUpload("F_CreatorTime", "{\n  \"tableName\": \"a_pu_return_prd\",\n  \"regList\": null,\n  \"jnpfKey\": \"createTime\",\n  \"rule\": null,\n  \"ruleType\": null,\n  \"dictionaryType\": null,\n  \"required\": false,\n  \"unique\": false,\n  \"label\": \"创建时间\",\n  \"dataType\": null,\n  \"propsUrl\": null,\n  \"children\": null,\n  \"props\": null,\n  \"showLevel\": null,\n  \"templateJson\": null,\n  \"startTimeRule\": false,\n  \"startTimeTarget\": 0,\n  \"startTimeType\": 0,\n  \"startTimeValue\": null,\n  \"startRelationField\": null,\n  \"endTimeRule\": false,\n  \"endTimeTarget\": 0,\n  \"endTimeType\": 0,\n  \"endTimeValue\": null,\n  \"endRelationField\": null,\n  \"precision\": null,\n  \"formId\": \"formItema42d3c\",\n  \"ruleConfig\": null,\n  \"IsBusinessKey\": false,\n  \"tag\": \"JnpfOpenData\"\n}")]
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
    /// 生产退料单商品列表.
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(APuReturnPrdItemEntity.F_ReturnInPRDId), nameof(id))]
    public List<APuReturnPrdItemEntity> APuReturnPrdItemList { get; set; }

    /// <summary>
    /// 生产退料单日志.
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(APuReturnPrdLogEntity.F_ReturnInPRDId), nameof(id))]
    public List<APuReturnPrdLogEntity> APuReturnPrdLogList { get; set; }

}