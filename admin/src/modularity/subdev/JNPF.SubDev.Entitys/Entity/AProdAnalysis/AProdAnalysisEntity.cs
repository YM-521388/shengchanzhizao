using JNPF.Common.CodeGenUpload;
using SqlSugar;
using JNPF.Extras.DatabaseAccessor.SqlSugar.Models;

namespace JNPF.example.Entitys;

/// <summary>
/// a_prod_analysis实体.
/// </summary>
[SugarTable("a_prod_analysis")]
public class AProdAnalysisEntity : ITenantFilter
{
    /// <summary>
    /// 主键.
    /// </summary>
    [SugarColumn(ColumnName = "F_Id", IsPrimaryKey = true)]
    public string? id { get; set; }

    /// <summary>
    /// 生产计划ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_ProdPlanId")]
    [CodeGenUpload("F_ProdPlanId", "F_ProdPlanId", "2014512148058869760", "F_Id", "F_PlanNo", "", "{\n  \"tableName\": \"a_prod_analysis\",\n  \"regList\": [],\n  \"jnpfKey\": \"popupSelect\",\n  \"rule\": null,\n  \"ruleType\": null,\n  \"dictionaryType\": null,\n  \"required\": false,\n  \"unique\": false,\n  \"label\": \"生产计划\",\n  \"dataType\": null,\n  \"propsUrl\": null,\n  \"children\": null,\n  \"props\": null,\n  \"showLevel\": null,\n  \"templateJson\": null,\n  \"startTimeRule\": false,\n  \"startTimeTarget\": 0,\n  \"startTimeType\": 0,\n  \"startTimeValue\": null,\n  \"startRelationField\": null,\n  \"endTimeRule\": false,\n  \"endTimeTarget\": 0,\n  \"endTimeType\": 0,\n  \"endTimeValue\": null,\n  \"endRelationField\": null,\n  \"precision\": null,\n  \"formId\": \"formItem378825\",\n  \"ruleConfig\": null,\n  \"IsBusinessKey\": false,\n  \"tag\": \"JnpfPopupSelect\"\n}")]
    public string? F_ProdPlanId { get; set; }

    /// <summary>
    /// 是否考虑主商品库存.
    /// </summary>
    [SugarColumn(ColumnName = "F_ConsiderMainStock")]
    public string? F_ConsiderMainStock { get; set; }

    /// <summary>
    /// 是否考虑物料占用.
    /// </summary>
    [SugarColumn(ColumnName = "F_ConsiderMaterialOccupy")]
    public string? F_ConsiderMaterialOccupy { get; set; }

    /// <summary>
    /// 是否考虑在制商品.
    /// </summary>
    [SugarColumn(ColumnName = "F_ConsiderWipGoods")]
    public string? F_ConsiderWipGoods { get; set; }

    /// <summary>
    /// 占用物料能否被其他单据出库.
    /// </summary>
    [SugarColumn(ColumnName = "F_OccupyAllowOtherOut")]
    public string? F_OccupyAllowOtherOut { get; set; }

    /// <summary>
    /// 是否考虑物料库存.
    /// </summary>
    [SugarColumn(ColumnName = "F_ConsiderMaterialStock")]
    public string? F_ConsiderMaterialStock { get; set; }

    /// <summary>
    /// 是否考虑数量向上取整.
    /// </summary>
    [SugarColumn(ColumnName = "F_RoundUpQty")]
    public string? F_RoundUpQty { get; set; }

    /// <summary>
    /// 是否考虑在途商品.
    /// </summary>
    [SugarColumn(ColumnName = "F_ConsiderTransitGoods")]
    public string? F_ConsiderTransitGoods { get; set; }

    /// <summary>
    /// 分析人.
    /// </summary>
    [SugarColumn(ColumnName = "F_AnalysisUserId")]
    public string? F_AnalysisUserId { get; set; }

    /// <summary>
    /// 分析时间.
    /// </summary>
    [SugarColumn(ColumnName = "F_AnalysisTime")]
    public DateTime? F_AnalysisTime { get; set; }

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
    /// 创建用户.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorUserId")]
    [CodeGenUpload("F_CreatorUserId", "{\n  \"tableName\": \"a_prod_analysis\",\n  \"regList\": null,\n  \"jnpfKey\": \"createUser\",\n  \"rule\": null,\n  \"ruleType\": null,\n  \"dictionaryType\": null,\n  \"required\": false,\n  \"unique\": false,\n  \"label\": \"创建用户\",\n  \"dataType\": null,\n  \"propsUrl\": null,\n  \"children\": null,\n  \"props\": null,\n  \"showLevel\": null,\n  \"templateJson\": null,\n  \"startTimeRule\": false,\n  \"startTimeTarget\": 0,\n  \"startTimeType\": 0,\n  \"startTimeValue\": null,\n  \"startRelationField\": null,\n  \"endTimeRule\": false,\n  \"endTimeTarget\": 0,\n  \"endTimeType\": 0,\n  \"endTimeValue\": null,\n  \"endRelationField\": null,\n  \"precision\": null,\n  \"formId\": \"formItemff7089\",\n  \"ruleConfig\": null,\n  \"IsBusinessKey\": false,\n  \"tag\": \"JnpfOpenData\"\n}")]
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorTime")]
    [CodeGenUpload("F_CreatorTime", "{\n  \"tableName\": \"a_prod_analysis\",\n  \"regList\": null,\n  \"jnpfKey\": \"createTime\",\n  \"rule\": null,\n  \"ruleType\": null,\n  \"dictionaryType\": null,\n  \"required\": false,\n  \"unique\": false,\n  \"label\": \"创建时间\",\n  \"dataType\": null,\n  \"propsUrl\": null,\n  \"children\": null,\n  \"props\": null,\n  \"showLevel\": null,\n  \"templateJson\": null,\n  \"startTimeRule\": false,\n  \"startTimeTarget\": 0,\n  \"startTimeType\": 0,\n  \"startTimeValue\": null,\n  \"startRelationField\": null,\n  \"endTimeRule\": false,\n  \"endTimeTarget\": 0,\n  \"endTimeType\": 0,\n  \"endTimeValue\": null,\n  \"endRelationField\": null,\n  \"precision\": null,\n  \"formId\": \"formItem74c93e\",\n  \"ruleConfig\": null,\n  \"IsBusinessKey\": false,\n  \"tag\": \"JnpfOpenData\"\n}")]
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
    /// 物料分析商品列表信息.
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(AProdAnalysisItemEntity.F_AnalysisId), nameof(id))]
    public List<AProdAnalysisItemEntity> AProdAnalysisItemList { get; set; }

}