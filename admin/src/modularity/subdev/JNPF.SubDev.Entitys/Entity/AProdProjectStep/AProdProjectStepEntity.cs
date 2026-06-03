using JNPF.Common.CodeGenUpload;
using SqlSugar;
using JNPF.Extras.DatabaseAccessor.SqlSugar.Models;
using JNPF.example.Entitys.Dto.AProdProjectStepItem;

namespace JNPF.example.Entitys;

/// <summary>
/// 项目商品工序信息实体.
/// </summary>
[SugarTable("a_prod_project_step")]
public class AProdProjectStepEntity : ITenantFilter
{
    /// <summary>
    /// 主键.
    /// </summary>
    [SugarColumn(ColumnName = "F_Id", IsPrimaryKey = true)]
    public string? id { get; set; }

    /// <summary>
    /// 项目商品ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_ProjectItemId")]
    public string? F_ProjectItemId { get; set; }

    /// <summary>
    /// 工序ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_ProcessId")]
    public string? F_ProcessId { get; set; }

    /// <summary>
    /// 计划开始.
    /// </summary>
    [SugarColumn(ColumnName = "F_StartDate")]
    public DateTime? F_StartDate { get; set; }

    /// <summary>
    /// 计划结束.
    /// </summary>
    [SugarColumn(ColumnName = "F_EndDate")]
    public DateTime? F_EndDate { get; set; }

    /// <summary>
    /// 排序.
    /// </summary>
    [SugarColumn(ColumnName = "F_SortCode")]
    public int? F_SortCode { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    [SugarColumn(ColumnName = "F_Description")]
    public string? F_Description { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorTime")]
    [CodeGenUpload("F_CreatorTime", "{\r\n  \"tableName\": \"a_prod_project_step\",\r\n  \"regList\": null,\r\n  \"jnpfKey\": \"createTime\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"创建时间\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItemd1a3d8\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfOpenData\"\r\n}")]
    public DateTime? F_CreatorTime { get; set; }

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
    /// 项目商品工序物料清单列表.
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public List<AProdProjectStepItemCrInput> tableField3b6f08 { get; set; }

    [Navigate(NavigateType.OneToMany, nameof(AProdProjectStepItemEntity.F_ProjectStepId), nameof(id))]
    public List<AProdProjectStepItemEntity> AProdProjectStepItemList { get; set; }
}