using JNPF.Common.CodeGenUpload;
using SqlSugar;
using JNPF.Extras.DatabaseAccessor.SqlSugar.Models;

namespace JNPF.example.Entitys;

/// <summary>
/// a_pu_return_so实体.
/// </summary>
[SugarTable("a_pu_return_so")]
public class APuReturnSoEntity : ITenantFilter
{
    /// <summary>
    /// 主键.
    /// </summary>
    [SugarColumn(ColumnName = "F_Id", IsPrimaryKey = true)]
    public string? id { get; set; }

    /// <summary>
    /// 销售退货单号.
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
    /// 退货日期.
    /// </summary>
    [SugarColumn(ColumnName = "F_ReturnInDate")]
    public DateTime? F_ReturnInDate { get; set; }

    /// <summary>
    /// 合同单号ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_ContractId")]
    public string? F_ContractId { get; set; }

    /// <summary>
    /// 客户ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_CustomerId")]
    [CodeGenUpload("F_CustomerId", "F_CustomerId", "2009458830353764352", "F_Id", "F_CustomerName", "", "{\r\n  \"tableName\": \"a_pu_return_so\",\r\n  \"regList\": [],\r\n  \"jnpfKey\": \"popupSelect\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"客户\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItemc5d39b\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfPopupSelect\"\r\n}")]
    public string? F_CustomerId { get; set; }

    /// <summary>
    /// 联系人.
    /// </summary>
    [SugarColumn(ColumnName = "F_ContactId")]
    [CodeGenUpload("F_ContactId", "F_ContactId", "2009459664370143232", "F_Id", "F_ContactName", "", "{\r\n  \"tableName\": \"a_pu_return_so\",\r\n  \"regList\": [],\r\n  \"jnpfKey\": \"popupSelect\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"联系人\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": [\r\n    {\r\n      \"defaultValue\": \"\",\r\n      \"field\": \"Id\",\r\n      \"dataType\": \"varchar\",\r\n      \"required\": 0,\r\n      \"fieldName\": \"\",\r\n      \"relationField\": \"2009181616060108800\",\r\n      \"jnpfKey\": null,\r\n      \"complexHeaderList\": null,\r\n      \"sourceType\": 2,\r\n      \"isChildren\": false,\r\n      \"IsMultiple\": false\r\n    }\r\n  ],\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItema1a068\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfPopupSelect\"\r\n}")]
    public string? F_ContactId { get; set; }

    /// <summary>
    /// 地址.
    /// </summary>
    [SugarColumn(ColumnName = "F_AddressId")]
    [CodeGenUpload("F_AddressId", "F_AddressId", "2009527238009163776", "F_Id", "F_IsDefault", "", "{\r\n  \"tableName\": \"a_pu_return_so\",\r\n  \"regList\": [],\r\n  \"jnpfKey\": \"popupSelect\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"地址\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": [\r\n    {\r\n      \"defaultValue\": \"\",\r\n      \"field\": \"id\",\r\n      \"dataType\": \"varchar\",\r\n      \"required\": 0,\r\n      \"fieldName\": \"\",\r\n      \"relationField\": \"2009181616060108800\",\r\n      \"jnpfKey\": null,\r\n      \"complexHeaderList\": null,\r\n      \"sourceType\": 2,\r\n      \"isChildren\": false,\r\n      \"IsMultiple\": false\r\n    }\r\n  ],\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItem9943dc\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfPopupSelect\"\r\n}")]
    public string? F_AddressId { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    [SugarColumn(ColumnName = "F_Description")]
    public string? F_Description { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorUserId")]
    [CodeGenUpload("F_CreatorUserId", "{\r\n  \"tableName\": \"a_pu_return_so\",\r\n  \"regList\": null,\r\n  \"jnpfKey\": \"createUser\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"创建人员\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItem994040\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfOpenData\"\r\n}")]
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorTime")]
    [CodeGenUpload("F_CreatorTime", "{\r\n  \"tableName\": \"a_pu_return_so\",\r\n  \"regList\": null,\r\n  \"jnpfKey\": \"createTime\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"创建时间\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItem635021\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfOpenData\"\r\n}")]
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
    /// 销售退货单商品列表.
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(APuReturnSoItemEntity.F_ReturnInSOId), nameof(id))]
    public List<APuReturnSoItemEntity> APuReturnSoItemList { get; set; }

    /// <summary>
    /// 销售退货单日志.
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(APuReturnSoLogEntity.F_ReturnInSOId), nameof(id))]
    public List<APuReturnSoLogEntity> APuReturnSoLogList { get; set; }

}