using JNPF.Common.CodeGenUpload;
using SqlSugar;
using JNPF.Extras.DatabaseAccessor.SqlSugar.Models;
using JNPF.example.Entitys.Dto.ACtcContractItemGood;

namespace JNPF.example.Entitys;

/// <summary>
/// 选择合同商品实体.
/// </summary>
[SugarTable("a_ctc_contract_item")]
public class ACtcContractItemEntity : ITenantFilter
{
    /// <summary>
    /// 主键.
    /// </summary>
    [SugarColumn(ColumnName = "F_Id", IsPrimaryKey = true)]
    public string? F_Id { get; set; }

    /// <summary>
    /// 合同ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_ContractId")]
    public string? F_ContractId { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_CustomerId")]
    [CodeGenUpload("F_CustomerId", "F_CustomerId", "2008721559174385664", "F_Id", "F_GoodsName", "", "{\r\n  \"tableName\": \"a_ctc_contract_item\",\r\n  \"regList\": [],\r\n  \"jnpfKey\": \"popupSelect\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"商品\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formIteme8002e\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfPopupSelect\"\r\n}")]
    public string? F_CustomerId { get; set; }

    /// <summary>
    /// 单价.
    /// </summary>
    [SugarColumn(ColumnName = "F_Price")]
    public decimal? F_Price { get; set; }

    /// <summary>
    /// 销售数量.
    /// </summary>
    [SugarColumn(ColumnName = "F_SaleQty")]
    public int? F_SaleQty { get; set; }

    /// <summary>
    /// 折扣.
    /// </summary>
    [SugarColumn(ColumnName = "F_Discount")]
    public decimal? F_Discount { get; set; }

    /// <summary>
    /// 工艺路线ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_RoutingId")]
    public string? F_RoutingId { get; set; }

    /// <summary>
    /// 工单编号.
    /// </summary>
    [SugarColumn(ColumnName = "F_WorkOrderNo")]
    public string? F_WorkOrderNo { get; set; }

    /// <summary>
    /// 计划数.
    /// </summary>
    [SugarColumn(ColumnName = "F_PlanNum")]
    public int? F_PlanNum { get; set; }

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
    /// BOMID.
    /// </summary>
    [SugarColumn(ColumnName = "F_BomId")]
    public string? F_BomId { get; set; }

    /// <summary>
    /// 图纸.
    /// </summary>
    [SugarColumn(ColumnName = "F_DrawingFiles")]
    public string? F_DrawingFiles { get; set; }

    /// <summary>
    /// 类别.
    /// </summary>
    [SugarColumn(ColumnName = "F_Category")]
    public string? F_Category { get; set; }

    /// <summary>
    /// 商品备注.
    /// </summary>
    [SugarColumn(ColumnName = "F_Description")]
    public string? F_Description { get; set; }

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
    /// 安装或安装梁.
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
    /// 材质.
    /// </summary>
    [SugarColumn(ColumnName = "F_Material")]
    public string? F_Material { get; set; }

    /// <summary>
    /// 审核状态.
    /// </summary>
    [SugarColumn(ColumnName = "F_F_AuditStatus")]
    public string? F_F_AuditStatus { get; set; }

    /// <summary>
    /// 附件.
    /// </summary>
    [SugarColumn(ColumnName = "F_Files")]
    public string? F_Files { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorUserId")]
    [CodeGenUpload("F_CreatorUserId", "{\r\n  \"tableName\": \"a_ctc_contract_item\",\r\n  \"regList\": null,\r\n  \"jnpfKey\": \"createUser\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"创建人员\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItema78fc3\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfOpenData\"\r\n}")]
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorTime")]
    [CodeGenUpload("F_CreatorTime", "{\r\n  \"tableName\": \"a_ctc_contract_item\",\r\n  \"regList\": null,\r\n  \"jnpfKey\": \"createTime\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"创建时间\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItem14678c\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfOpenData\"\r\n}")]
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

    /// <summary>
    /// 合同商品用料清单.
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public List<ACtcContractItemGoodCrInput> ProjectGoods { get; set; }

    /// <summary>
    /// 合同商品用料清单.
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(ACtcContractItemGoodEntity.F_ProjectItemId), nameof(F_Id))]
    public List<ACtcContractItemGoodEntity> ACtcContractItemGoodList { get; set; }

}