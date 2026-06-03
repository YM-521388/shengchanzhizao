using JNPF.Common.CodeGenUpload;
using SqlSugar;
using JNPF.Extras.DatabaseAccessor.SqlSugar.Models;

namespace JNPF.example.Entitys;

/// <summary>
/// 商品实体.
/// </summary>
[SugarTable("a_pu_out_return_pu_item")]
public class APuOutReturnPuItemEntity : ITenantFilter
{
    /// <summary>
    /// 主键.
    /// </summary>
    [SugarColumn(ColumnName = "F_Id", IsPrimaryKey = true)]
    public string? F_Id { get; set; }

    /// <summary>
    /// 采购退货单ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_ReturnOutPUId")]
    public string? F_ReturnOutPUId { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_CustomerId")]
    [CodeGenUpload("F_CustomerId", "F_CustomerId", "2008721559174385664", "F_Id", "F_GoodsName", "", "{\n  \"tableName\": \"a_pu_out_return_pu_item\",\n  \"regList\": [],\n  \"jnpfKey\": \"popupSelect\",\n  \"rule\": null,\n  \"ruleType\": null,\n  \"dictionaryType\": null,\n  \"required\": false,\n  \"unique\": false,\n  \"label\": \"商品\",\n  \"dataType\": null,\n  \"propsUrl\": null,\n  \"children\": null,\n  \"props\": null,\n  \"showLevel\": null,\n  \"templateJson\": null,\n  \"startTimeRule\": false,\n  \"startTimeTarget\": 0,\n  \"startTimeType\": 0,\n  \"startTimeValue\": null,\n  \"startRelationField\": null,\n  \"endTimeRule\": false,\n  \"endTimeTarget\": 0,\n  \"endTimeType\": 0,\n  \"endTimeValue\": null,\n  \"endRelationField\": null,\n  \"precision\": null,\n  \"formId\": \"formItem78a7d8\",\n  \"ruleConfig\": null,\n  \"IsBusinessKey\": false,\n  \"tag\": \"JnpfPopupSelect\"\n}")]
    public string? F_CustomerId { get; set; }


    /// <summary>
    /// 退货数量.
    /// </summary>
    [SugarColumn(ColumnName = "F_Num")]
    public decimal? F_Num { get; set; }



    /// <summary>
    /// 入库仓库.
    /// </summary>
    [SugarColumn(ColumnName = "F_WarehouseID")]
    public string? F_WarehouseID { get; set; }

    /// <summary>
    /// 库存编码.
    /// </summary>
    [SugarColumn(ColumnName = "F_Encoding")]
    public string? F_Encoding { get; set; }

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
    /// 创建人员.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorUserId")]
    [CodeGenUpload("F_CreatorUserId", "{\r\n  \"tableName\": \"a_pu_out_return_pu_item\",\r\n  \"regList\": null,\r\n  \"jnpfKey\": \"createUser\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"创建人员\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItem64fafd\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfOpenData\"\r\n}")]
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorTime")]
    [CodeGenUpload("F_CreatorTime", "{\r\n  \"tableName\": \"a_pu_out_return_pu_item\",\r\n  \"regList\": null,\r\n  \"jnpfKey\": \"createTime\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"创建时间\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItemd3475b\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfOpenData\"\r\n}")]
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