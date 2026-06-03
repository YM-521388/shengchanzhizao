using JNPF.Common.CodeGenUpload;
using SqlSugar;
using JNPF.Extras.DatabaseAccessor.SqlSugar.Models;
using JNPF.example.Entitys.Dto.AProdProjectStep;
using JNPF.example.Entitys.Dto.AProdProjectGood;

namespace JNPF.example.Entitys;

/// <summary>
/// 商品实体.
/// </summary>
[SugarTable("a_prod_project_item")]
public class AProdProjectItemEntity : ITenantFilter
{
    /// <summary>
    /// 主键.
    /// </summary>
    [SugarColumn(ColumnName = "F_Id", IsPrimaryKey = true)]
    public string? F_Id { get; set; }

    /// <summary>
    /// 项目ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_ProjectId")]
    public string? F_ProjectId { get; set; }

    /// <summary>
    /// 上级ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_ParentId")]
    public string? F_ParentId { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_GoodsId")]
    public string? F_GoodsId { get; set; }

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
    [SugarColumn(ColumnName = "F_InstallPlateOrBeam")]
    public string? F_InstallPlateOrBeam { get; set; }

    /// <summary>
    /// 客户.
    /// </summary>
    [SugarColumn(ColumnName = "F_CustomerName")]
    public string? F_CustomerName { get; set; }

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
    /// BOM图片.
    /// </summary>
    [SugarColumn(ColumnName = "F_BomImages")]
    public string? F_BomImages { get; set; }

    /// <summary>
    /// 颜色.
    /// </summary>
    [SugarColumn(ColumnName = "F_Color")]
    public string? F_Color { get; set; }

    /// <summary>
    /// 类别.
    /// </summary>
    [SugarColumn(ColumnName = "F_Category")]
    public string? F_Category { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorTime")]
    [CodeGenUpload("F_CreatorTime", "{\r\n  \"tableName\": \"a_prod_project_item\",\r\n  \"regList\": null,\r\n  \"jnpfKey\": \"createTime\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"创建时间\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItemc5d481\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfOpenData\"\r\n}")]
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
    /// 项目商品工序列表.
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public List<AProdProjectStepCrInput> Process { get; set; }

    [Navigate(NavigateType.OneToMany, nameof(AProdProjectStepEntity.F_ProjectItemId), nameof(F_Id))]
    public List<AProdProjectStepEntity> AProdProjectStepList { get; set; }

    /// <summary>
    /// 项目商品用料清单.
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public List<AProdProjectGoodCrInput> ProjectGoods { get; set; }

    [Navigate(NavigateType.OneToMany, nameof(AProdProjectGoodEntity.F_ProjectItemId), nameof(F_Id))]
    public List<AProdProjectGoodEntity> AProdProjectGoodList { get; set; }
}