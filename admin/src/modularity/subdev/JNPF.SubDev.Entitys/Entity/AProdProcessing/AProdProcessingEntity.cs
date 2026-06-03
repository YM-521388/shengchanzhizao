using JNPF.Common.CodeGenUpload;
using SqlSugar;
using JNPF.Extras.DatabaseAccessor.SqlSugar.Models;

namespace JNPF.example.Entitys;

/// <summary>
/// a_prod_processing实体.
/// </summary>
[SugarTable("a_prod_processing")]
public class AProdProcessingEntity : ITenantFilter
{
    /// <summary>
    /// 主键.
    /// </summary>
    [SugarColumn(ColumnName = "F_Id", IsPrimaryKey = true)]
    public string? id { get; set; }

    /// <summary>
    /// 合同ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_ContractId")]
    public string? F_ContractId { get; set; }

    /// <summary>
    /// 项目ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_ProjectId")]
    public string? F_ProjectId { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_GoodsId")]
    [CodeGenUpload("F_GoodsId", "F_GoodsId", "2029803158963884032", "id", "F_GoodsName", "", "{\r\n  \"tableName\": \"a_prod_processing\",\r\n  \"regList\": [],\r\n  \"jnpfKey\": \"popupSelect\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": true,\r\n  \"unique\": false,\r\n  \"label\": \"商品\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": [\r\n    {\r\n      \"defaultValue\": \"\",\r\n      \"field\": \"contractId\",\r\n      \"dataType\": \"varchar\",\r\n      \"required\": 0,\r\n      \"fieldName\": \"合同ID\",\r\n      \"relationField\": \"\",\r\n      \"jnpfKey\": null,\r\n      \"complexHeaderList\": null,\r\n      \"sourceType\": 3,\r\n      \"isChildren\": false,\r\n      \"IsMultiple\": false\r\n    }\r\n  ],\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItem1c701c\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfPopupSelect\"\r\n}")]
    public string? F_GoodsId { get; set; }

    /// <summary>
    /// BOMID.
    /// </summary>
    [SugarColumn(ColumnName = "F_BOMId")]
    public string? F_BOMId { get; set; }

    /// <summary>
    /// 工艺路线ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_RoutingId")]
    public string? F_RoutingId { get; set; }

    /// <summary>
    /// 加工单xml.
    /// </summary>
    [SugarColumn(ColumnName = "F_XML")]
    public string? F_XML { get; set; }

    /// <summary>
    /// 生产计划ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_ProdPlanId")]
    public string? F_ProdPlanId { get; set; }

    /// <summary>
    /// 加工单号.
    /// </summary>
    [SugarColumn(ColumnName = "F_ProcessNo")]
    public string? F_ProcessNo { get; set; }

    /// <summary>
    /// 计划数.
    /// </summary>
    [SugarColumn(ColumnName = "F_PlanQty")]
    public int? F_PlanQty { get; set; }

    /// <summary>
    /// 交货日期.
    /// </summary>
    [SugarColumn(ColumnName = "F_DeliveryDate")]
    public DateTime? F_DeliveryDate { get; set; }

    /// <summary>
    /// 优先级.
    /// </summary>
    [SugarColumn(ColumnName = "F_Priority")]
    public string? F_Priority { get; set; }

    /// <summary>
    /// 计划开始.
    /// </summary>
    [SugarColumn(ColumnName = "F_PlanStartDate")]
    public DateTime? F_PlanStartDate { get; set; }

    /// <summary>
    /// 计划结束.
    /// </summary>
    [SugarColumn(ColumnName = "F_PlanEndDate")]
    public DateTime? F_PlanEndDate { get; set; }

    /// <summary>
    /// 图纸.
    /// </summary>
    [SugarColumn(ColumnName = "F_DrawingFile")]
    public string? F_DrawingFile { get; set; }

    /// <summary>
    /// 客户.
    /// </summary>
    [SugarColumn(ColumnName = "F_CustomerName")]
    public string? F_CustomerName { get; set; }

    /// <summary>
    /// 门板厚度.
    /// </summary>
    [SugarColumn(ColumnName = "F_DoorPlateThickness")]
    public string? F_DoorPlateThickness { get; set; }

    /// <summary>
    /// 箱体板厚.
    /// </summary>
    [SugarColumn(ColumnName = "F_BoxPlateThickness")]
    public string? F_BoxPlateThickness { get; set; }

    /// <summary>
    /// 安装板或安装梁.
    /// </summary>
    [SugarColumn(ColumnName = "F_InstallOrBeam")]
    public string? F_InstallOrBeam { get; set; }

    /// <summary>
    /// 锁具.
    /// </summary>
    [SugarColumn(ColumnName = "F_LockSet")]
    public string? F_LockSet { get; set; }

    /// <summary>
    /// 铰链.
    /// </summary>
    [SugarColumn(ColumnName = "F_Hinge")]
    public string? F_Hinge { get; set; }

    /// <summary>
    /// 颜色.
    /// </summary>
    [SugarColumn(ColumnName = "F_Color")]
    public string? F_Color { get; set; }

    /// <summary>
    /// 类别.
    /// </summary>
    [SugarColumn(ColumnName = "F_Type")]
    public string? F_Type { get; set; }

    /// <summary>
    /// BOM图片.
    /// </summary>
    [SugarColumn(ColumnName = "F_BOMImages")]
    public string? F_BOMImages { get; set; }

    /// <summary>
    /// 状态.
    /// </summary>
    [SugarColumn(ColumnName = "F_State")]
    public string? F_State { get; set; }

    /// <summary>
    /// 排单序号.
    /// </summary>
    [SugarColumn(ColumnName = "F_SequenceNo")]
    public int? F_SequenceNo { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    [SugarColumn(ColumnName = "F_Description")]
    public string? F_Description { get; set; }

    /// <summary>
    /// 创建用户.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorUserId")]
    [CodeGenUpload("F_CreatorUserId", "{\r\n  \"tableName\": \"a_prod_processing\",\r\n  \"regList\": null,\r\n  \"jnpfKey\": \"createUser\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"创建用户\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItem6d8aaa\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfOpenData\"\r\n}")]
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorTime")]
    [CodeGenUpload("F_CreatorTime", "{\r\n  \"tableName\": \"a_prod_processing\",\r\n  \"regList\": null,\r\n  \"jnpfKey\": \"createTime\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"创建时间\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItem3930be\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfOpenData\"\r\n}")]
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
    /// 加工单用料清单.
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(AProdBomItemEntity.F_ProcessingId), nameof(id))]
    public List<AProdBomItemEntity> AProdBomItemList { get; set; }

    /// <summary>
    /// 生产任务.
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(AProdTaskEntity.F_ProcessingId), nameof(id))]
    public List<AProdTaskEntity> AProdTaskList { get; set; }

}