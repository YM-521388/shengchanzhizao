using JNPF.Common.CodeGenUpload;
using SqlSugar;
using JNPF.Extras.DatabaseAccessor.SqlSugar.Models;

namespace JNPF.example.Entitys;

/// <summary>
/// a_ctc_contract实体.
/// </summary>
[SugarTable("a_ctc_contract")]
public class ACtcContractEntity : ITenantFilter
{
    /// <summary>
    /// 主键.
    /// </summary>
    [SugarColumn(ColumnName = "F_Id", IsPrimaryKey = true)]
    public string? id { get; set; }

    /// <summary>
    /// 合同编号.
    /// </summary>
    [SugarColumn(ColumnName = "F_ContractCode")]
    public string? F_ContractCode { get; set; }

    /// <summary>
    /// 客户合同号.
    /// </summary>
    [SugarColumn(ColumnName = "F_CustomerCode")]
    public string? F_CustomerCode { get; set; }

    /// <summary>
    /// 客户ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_CustomerId")]
    public string? F_CustomerId { get; set; }

    /// <summary>
    /// 联系人ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_ContactId")]
    [CodeGenUpload("F_ContactId", "F_ContactId", "2009459664370143232", "F_Id", "F_ContactName", "", "{\r\n  \"tableName\": \"a_ctc_contract\",\r\n  \"regList\": [],\r\n  \"jnpfKey\": \"popupSelect\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"联系人ID\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": [\r\n    {\r\n      \"defaultValue\": \"\",\r\n      \"field\": \"Id\",\r\n      \"dataType\": \"varchar\",\r\n      \"required\": 0,\r\n      \"fieldName\": \"\",\r\n      \"relationField\": \"2009181616060108800\",\r\n      \"jnpfKey\": null,\r\n      \"complexHeaderList\": null,\r\n      \"sourceType\": 2,\r\n      \"isChildren\": false,\r\n      \"IsMultiple\": false\r\n    }\r\n  ],\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItema91d50\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfPopupSelect\"\r\n}")]
    public string? F_ContactId { get; set; }

    /// <summary>
    /// 客户地址ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_AddressId")]
    public string? F_AddressId { get; set; }

    /// <summary>
    /// 预付款.
    /// </summary>
    [SugarColumn(ColumnName = "F_PrepayAmount")]
    public decimal? F_PrepayAmount { get; set; }

    /// <summary>
    /// 销售总数.
    /// </summary>
    [SugarColumn(ColumnName = "F_SaleTotal")]
    public int? F_SaleTotal { get; set; }

    /// <summary>
    /// 付款周期（天）.
    /// </summary>
    [SugarColumn(ColumnName = "F_PaymentCycle")]
    public int? F_PaymentCycle { get; set; }

    /// <summary>
    /// 合同金额.
    /// </summary>
    [SugarColumn(ColumnName = "F_ContractAmount")]
    public decimal? F_ContractAmount { get; set; }

    /// <summary>
    /// 交货日期.
    /// </summary>
    [SugarColumn(ColumnName = "F_DeliveryDate")]
    public DateTime? F_DeliveryDate { get; set; }

    /// <summary>
    /// 是否含税.
    /// </summary>
    [SugarColumn(ColumnName = "F_IsTaxable")]
    public string? F_IsTaxable { get; set; }

    /// <summary>
    /// 业务员.
    /// </summary>
    [SugarColumn(ColumnName = "F_SalesmanId")]
    public string? F_SalesmanId { get; set; }

    /// <summary>
    /// 审核状态.
    /// </summary>
    [SugarColumn(ColumnName = "F_AuditStatus")]
    public string? F_AuditStatus { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    [SugarColumn(ColumnName = "F_Description")]
    public string? F_Description { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorUserId")]
    [CodeGenUpload("F_CreatorUserId", "{\r\n  \"tableName\": \"a_ctc_contract\",\r\n  \"regList\": null,\r\n  \"jnpfKey\": \"createUser\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"创建人员\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItem616ab1\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfOpenData\"\r\n}")]
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorTime")]
    [CodeGenUpload("F_CreatorTime", "{\r\n  \"tableName\": \"a_ctc_contract\",\r\n  \"regList\": null,\r\n  \"jnpfKey\": \"createTime\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"创建时间\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItem03e630\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfOpenData\"\r\n}")]
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
    /// 合同订单商品.
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(ACtcContractItemEntity.F_ContractId), nameof(id))]
    public List<ACtcContractItemEntity> ACtcContractItemList { get; set; }

}