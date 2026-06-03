using JNPF.Common.CodeGenUpload;
using SqlSugar;
using JNPF.Extras.DatabaseAccessor.SqlSugar.Models;

namespace JNPF.example.Entitys;

/// <summary>
/// a_prod_outsource实体.
/// </summary>
[SugarTable("a_prod_outsource")]
public class AProdOutsourceEntity : ITenantFilter
{
    /// <summary>
    /// 主键.
    /// </summary>
    [SugarColumn(ColumnName = "F_Id", IsPrimaryKey = true)]
    public string? id { get; set; }



    /// <summary>
    /// 项目ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_ProjectId")]
    public string? F_ProjectId { get; set; }


    /// <summary>
    /// 商品ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_GoodsId")]
    [CodeGenUpload("F_GoodsId", "F_GoodsId", "2008721559174385664", "F_Id", "F_GoodsName", "", "{\n  \"tableName\": \"a_prod_outsource\",\n  \"regList\": [],\n  \"jnpfKey\": \"popupSelect\",\n  \"rule\": null,\n  \"ruleType\": null,\n  \"dictionaryType\": null,\n  \"required\": true,\n  \"unique\": false,\n  \"label\": \"商品\",\n  \"dataType\": null,\n  \"propsUrl\": null,\n  \"children\": null,\n  \"props\": null,\n  \"showLevel\": null,\n  \"templateJson\": null,\n  \"startTimeRule\": false,\n  \"startTimeTarget\": 0,\n  \"startTimeType\": 0,\n  \"startTimeValue\": null,\n  \"startRelationField\": null,\n  \"endTimeRule\": false,\n  \"endTimeTarget\": 0,\n  \"endTimeType\": 0,\n  \"endTimeValue\": null,\n  \"endRelationField\": null,\n  \"precision\": null,\n  \"formId\": \"formItem9e8b27\",\n  \"ruleConfig\": null,\n  \"IsBusinessKey\": false,\n  \"tag\": \"JnpfPopupSelect\"\n}")]
    public string? F_GoodsId { get; set; }


    /// <summary>
    /// BOMID.
    /// </summary>
    [SugarColumn(ColumnName = "F_BOMId")]
    [CodeGenUpload("F_BOMId", "F_BOMId", "2013188646957617152", "id", "F_BomCode", "", "{\n  \"tableName\": \"a_prod_outsource\",\n  \"regList\": [],\n  \"jnpfKey\": \"popupSelect\",\n  \"rule\": null,\n  \"ruleType\": null,\n  \"dictionaryType\": null,\n  \"required\": false,\n  \"unique\": false,\n  \"label\": \"BOM\",\n  \"dataType\": null,\n  \"propsUrl\": null,\n  \"children\": null,\n  \"props\": null,\n  \"showLevel\": null,\n  \"templateJson\": [\n    {\n      \"defaultValue\": \"\",\n      \"field\": \"goodsId\",\n      \"dataType\": \"varchar\",\n      \"required\": 0,\n      \"fieldName\": \"合同ID\",\n      \"relationField\": \"2008839754363310080\",\n      \"jnpfKey\": null,\n      \"complexHeaderList\": null,\n      \"sourceType\": 2,\n      \"isChildren\": false,\n      \"IsMultiple\": false\n    }\n  ],\n  \"startTimeRule\": false,\n  \"startTimeTarget\": 0,\n  \"startTimeType\": 0,\n  \"startTimeValue\": null,\n  \"startRelationField\": null,\n  \"endTimeRule\": false,\n  \"endTimeTarget\": 0,\n  \"endTimeType\": 0,\n  \"endTimeValue\": null,\n  \"endRelationField\": null,\n  \"precision\": null,\n  \"formId\": \"formItem40cc59\",\n  \"ruleConfig\": null,\n  \"IsBusinessKey\": false,\n  \"tag\": \"JnpfPopupSelect\"\n}")]
    public string? F_BOMId { get; set; }
    /// <summary>
    /// 外协工单号.
    /// </summary>
    [SugarColumn(ColumnName = "F_OutsourceNo")]
    public string? F_OutsourceNo { get; set; }

    /// <summary>
    /// 计划数.
    /// </summary>
    [SugarColumn(ColumnName = "F_PlanNum")]
    public int? F_PlanNum { get; set; }

    /// <summary>
    /// 供应商ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_SupplierId")]
    public string? F_SupplierId { get; set; }

    /// <summary>
    /// 供应商联系人.
    /// </summary>
    [SugarColumn(ColumnName = "F_ContactPerson")]
    public string? F_ContactPerson { get; set; }

    /// <summary>
    /// 供应商联系人电话.
    /// </summary>
    [SugarColumn(ColumnName = "F_ContactPhone")]
    public string? F_ContactPhone { get; set; }

    /// <summary>
    /// 供应商地区.
    /// </summary>
    [SugarColumn(ColumnName = "F_Region")]
    public string? F_Region { get; set; }

    /// <summary>
    /// 供应商详细地址.
    /// </summary>
    [SugarColumn(ColumnName = "F_DetailAddress")]
    public string? F_DetailAddress { get; set; }

    /// <summary>
    /// 外协单价.
    /// </summary>
    [SugarColumn(ColumnName = "F_Price")]
    public decimal? F_Price { get; set; }

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
    [SugarColumn(ColumnName = "F_Files")]
    public string? F_Files { get; set; }

    /// <summary>
    /// 状态.
    /// </summary>
    [SugarColumn(ColumnName = "F_State")]
    public string? F_State { get; set; }

    /// <summary>
    /// 描述.
    /// </summary>
    [SugarColumn(ColumnName = "F_Description")]
    public string? F_Description { get; set; }

    /// <summary>
    /// 暂停原因.
    /// </summary>
    [SugarColumn(ColumnName = "F_PauseReason")]
    public string? F_PauseReason { get; set; }

    /// <summary>
    /// 取消原因.
    /// </summary>
    [SugarColumn(ColumnName = "F_CancelReason")]
    public string? F_CancelReason { get; set; }

    /// <summary>
    /// 创建用户.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorUserId")]
    [CodeGenUpload("F_CreatorUserId", "{\n  \"tableName\": \"a_prod_outsource\",\n  \"regList\": null,\n  \"jnpfKey\": \"createUser\",\n  \"rule\": null,\n  \"ruleType\": null,\n  \"dictionaryType\": null,\n  \"required\": false,\n  \"unique\": false,\n  \"label\": \"创建用户\",\n  \"dataType\": null,\n  \"propsUrl\": null,\n  \"children\": null,\n  \"props\": null,\n  \"showLevel\": null,\n  \"templateJson\": null,\n  \"startTimeRule\": false,\n  \"startTimeTarget\": 0,\n  \"startTimeType\": 0,\n  \"startTimeValue\": null,\n  \"startRelationField\": null,\n  \"endTimeRule\": false,\n  \"endTimeTarget\": 0,\n  \"endTimeType\": 0,\n  \"endTimeValue\": null,\n  \"endRelationField\": null,\n  \"precision\": null,\n  \"formId\": \"formItem0d5fd0\",\n  \"ruleConfig\": null,\n  \"IsBusinessKey\": false,\n  \"tag\": \"JnpfOpenData\"\n}")]
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorTime")]
    [CodeGenUpload("F_CreatorTime", "{\n  \"tableName\": \"a_prod_outsource\",\n  \"regList\": null,\n  \"jnpfKey\": \"createTime\",\n  \"rule\": null,\n  \"ruleType\": null,\n  \"dictionaryType\": null,\n  \"required\": false,\n  \"unique\": false,\n  \"label\": \"创建时间\",\n  \"dataType\": null,\n  \"propsUrl\": null,\n  \"children\": null,\n  \"props\": null,\n  \"showLevel\": null,\n  \"templateJson\": null,\n  \"startTimeRule\": false,\n  \"startTimeTarget\": 0,\n  \"startTimeType\": 0,\n  \"startTimeValue\": null,\n  \"startRelationField\": null,\n  \"endTimeRule\": false,\n  \"endTimeTarget\": 0,\n  \"endTimeType\": 0,\n  \"endTimeValue\": null,\n  \"endRelationField\": null,\n  \"precision\": null,\n  \"formId\": \"formItemc9efa7\",\n  \"ruleConfig\": null,\n  \"IsBusinessKey\": false,\n  \"tag\": \"JnpfOpenData\"\n}")]
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
    /// 外协工单用料清单.
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(AProdOutsourceItemEntity.F_OutsourceId), nameof(id))]
    public List<AProdOutsourceItemEntity> AProdOutsourceItemList { get; set; }

}