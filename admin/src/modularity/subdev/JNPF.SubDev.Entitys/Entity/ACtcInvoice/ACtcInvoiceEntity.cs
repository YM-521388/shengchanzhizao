using JNPF.Common.CodeGenUpload;
using SqlSugar;
using JNPF.Extras.DatabaseAccessor.SqlSugar.Models;

namespace JNPF.example.Entitys;

/// <summary>
/// a_ctc_invoice实体.
/// </summary>
[SugarTable("a_ctc_invoice")]
public class ACtcInvoiceEntity : ITenantFilter
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
    [CodeGenUpload("F_ContractId", "F_ContractId", "2010889611072638976", "F_Id", "F_ContractCode", "", "{\r\n  \"tableName\": \"a_ctc_invoice\",\r\n  \"regList\": [],\r\n  \"jnpfKey\": \"popupSelect\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"合同ID\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItemfc9da2\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfPopupSelect\"\r\n}")]
    public string? F_ContractId { get; set; }

    /// <summary>
    /// 开票金额(元).
    /// </summary>
    [SugarColumn(ColumnName = "F_Money")]
    public decimal? F_Money { get; set; }

    /// <summary>
    /// 开票状态.
    /// </summary>
    [SugarColumn(ColumnName = "F_Status")]
    public string? F_Status { get; set; }

    /// <summary>
    /// 申请附件.
    /// </summary>
    [SugarColumn(ColumnName = "F_ApplyFiles")]
    public string? F_ApplyFiles { get; set; }

    /// <summary>
    /// 申请备注.
    /// </summary>
    [SugarColumn(ColumnName = "F_ApplyRemark")]
    public string? F_ApplyRemark { get; set; }

    /// <summary>
    /// 开票附件.
    /// </summary>
    [SugarColumn(ColumnName = "F_InvoiceFiles")]
    public string? F_InvoiceFiles { get; set; }

    /// <summary>
    /// 开票备注.
    /// </summary>
    [SugarColumn(ColumnName = "F_InvoiceRemark")]
    public string? F_InvoiceRemark { get; set; }

    /// <summary>
    /// 申请人员.
    /// </summary>
    [SugarColumn(ColumnName = "F_ApplyUserId")]
    public string? F_ApplyUserId { get; set; }

    /// <summary>
    /// 申请时间.
    /// </summary>
    [SugarColumn(ColumnName = "F_ApplyTime")]
    public DateTime? F_ApplyTime { get; set; }

    /// <summary>
    /// 开票人员.
    /// </summary>
    [SugarColumn(ColumnName = "F_InvoiceUserId")]
    public string? F_InvoiceUserId { get; set; }

    /// <summary>
    /// 开票时间.
    /// </summary>
    [SugarColumn(ColumnName = "F_InvoiceTime")]
    public DateTime? F_InvoiceTime { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorUserId")]
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorTime")]
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
    /// 合同发票管理日志.
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(ACtcInvoiceLogEntity.F_InvoiceId), nameof(id))]
    public List<ACtcInvoiceLogEntity> ACtcInvoiceLogList { get; set; }

}