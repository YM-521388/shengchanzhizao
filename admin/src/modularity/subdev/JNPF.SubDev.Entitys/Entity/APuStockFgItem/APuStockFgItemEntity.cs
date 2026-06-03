using JNPF.Common.CodeGenUpload;
using SqlSugar;
using JNPF.Extras.DatabaseAccessor.SqlSugar.Models;

namespace JNPF.example.Entitys;

/// <summary>
/// 选择商品实体.
/// </summary>
[SugarTable("a_pu_stock_fg_item")]
public class APuStockFgItemEntity : ITenantFilter
{
    /// <summary>
    /// 主键.
    /// </summary>
    [SugarColumn(ColumnName = "F_Id", IsPrimaryKey = true)]
    public string? F_Id { get; set; }

    /// <summary>
    /// 成品入库单ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_StockInFGId")]
    public string? F_StockInFGId { get; set; }

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
    /// 商品ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_CustomerId")]
    [CodeGenUpload("F_CustomerId", "F_CustomerId", "2013490232657317888", "F_Id", "GoodsName", "", "{\n  \"tableName\": \"a_pu_stock_fg_item\",\n  \"regList\": [],\n  \"jnpfKey\": \"popupSelect\",\n  \"rule\": null,\n  \"ruleType\": null,\n  \"dictionaryType\": null,\n  \"required\": false,\n  \"unique\": false,\n  \"label\": \"商品\",\n  \"dataType\": null,\n  \"propsUrl\": null,\n  \"children\": null,\n  \"props\": null,\n  \"showLevel\": null,\n  \"templateJson\": null,\n  \"startTimeRule\": false,\n  \"startTimeTarget\": 0,\n  \"startTimeType\": 0,\n  \"startTimeValue\": null,\n  \"startRelationField\": null,\n  \"endTimeRule\": false,\n  \"endTimeTarget\": 0,\n  \"endTimeType\": 0,\n  \"endTimeValue\": null,\n  \"endRelationField\": null,\n  \"precision\": null,\n  \"formId\": \"formItembd0013\",\n  \"ruleConfig\": null,\n  \"IsBusinessKey\": false,\n  \"tag\": \"JnpfPopupSelect\"\n}")]
    public string? F_CustomerId { get; set; }

    /// <summary>
    /// 入库数量.
    /// </summary>
    [SugarColumn(ColumnName = "F_Num")]
    public decimal? F_Num { get; set; }

    /// <summary>
    /// 成本单价(元).
    /// </summary>
    [SugarColumn(ColumnName = "F_Price")]
    public decimal? F_Price { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    [SugarColumn(ColumnName = "F_Description")]
    public string? F_Description { get; set; }


    /// <summary>
    /// 入库仓库ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_WarehouseID")]
    public string? F_WarehouseID { get; set; }

    /// <summary>
    /// 商品编码.
    /// </summary>
    [SugarColumn(ColumnName = "F_Encoding")]
    public string? F_Encoding { get; set; }


    /// <summary>
    /// 创建人员.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorUserId")]
    [CodeGenUpload("F_CreatorUserId", "{\n  \"tableName\": \"a_pu_stock_fg_item\",\n  \"regList\": null,\n  \"jnpfKey\": \"createUser\",\n  \"rule\": null,\n  \"ruleType\": null,\n  \"dictionaryType\": null,\n  \"required\": false,\n  \"unique\": false,\n  \"label\": \"创建人员\",\n  \"dataType\": null,\n  \"propsUrl\": null,\n  \"children\": null,\n  \"props\": null,\n  \"showLevel\": null,\n  \"templateJson\": null,\n  \"startTimeRule\": false,\n  \"startTimeTarget\": 0,\n  \"startTimeType\": 0,\n  \"startTimeValue\": null,\n  \"startRelationField\": null,\n  \"endTimeRule\": false,\n  \"endTimeTarget\": 0,\n  \"endTimeType\": 0,\n  \"endTimeValue\": null,\n  \"endRelationField\": null,\n  \"precision\": null,\n  \"formId\": \"formItem932032\",\n  \"ruleConfig\": null,\n  \"IsBusinessKey\": false,\n  \"tag\": \"JnpfOpenData\"\n}")]
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorTime")]
    [CodeGenUpload("F_CreatorTime", "{\n  \"tableName\": \"a_pu_stock_fg_item\",\n  \"regList\": null,\n  \"jnpfKey\": \"createTime\",\n  \"rule\": null,\n  \"ruleType\": null,\n  \"dictionaryType\": null,\n  \"required\": false,\n  \"unique\": false,\n  \"label\": \"创建时间\",\n  \"dataType\": null,\n  \"propsUrl\": null,\n  \"children\": null,\n  \"props\": null,\n  \"showLevel\": null,\n  \"templateJson\": null,\n  \"startTimeRule\": false,\n  \"startTimeTarget\": 0,\n  \"startTimeType\": 0,\n  \"startTimeValue\": null,\n  \"startRelationField\": null,\n  \"endTimeRule\": false,\n  \"endTimeTarget\": 0,\n  \"endTimeType\": 0,\n  \"endTimeValue\": null,\n  \"endRelationField\": null,\n  \"precision\": null,\n  \"formId\": \"formItem6ef332\",\n  \"ruleConfig\": null,\n  \"IsBusinessKey\": false,\n  \"tag\": \"JnpfOpenData\"\n}")]
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