using JNPF.Common.CodeGenUpload;
using SqlSugar;
using JNPF.Extras.DatabaseAccessor.SqlSugar.Models;
using JNPF.example.Entitys.Dto.ACtcContractItemGood;

namespace JNPF.example.Entitys;

/// <summary>
/// 项目商品用料清单实体.
/// </summary>
[SugarTable("a_ctc_contract_item_good")]
public class ACtcContractItemGoodEntity : ITenantFilter
{
    /// <summary>
    /// 主键.
    /// </summary>
    [SugarColumn(ColumnName = "F_Id", IsPrimaryKey = true)]
    public string? id { get; set; }

    /// <summary>
    /// 合同商品ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_ProjectItemId")]
    public string? F_ProjectItemId { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    [SugarColumn(ColumnName = "F_GoodsId")]
    public string? F_GoodsId { get; set; }

    /// <summary>
    /// 单位用量.
    /// </summary>
    [SugarColumn(ColumnName = "F_Unit")]
    public int? F_Unit { get; set; }

    /// <summary>
    /// 生产用量.
    /// </summary>
    [SugarColumn(ColumnName = "F_ProdUnit")]
    public int? F_ProdUnit { get; set; }

    /// <summary>
    /// 入库工序.
    /// </summary>
    [SugarColumn(ColumnName = "F_StockInProcess")]
    public string? F_StockInProcess { get; set; }

    /// <summary>
    /// 投料工序.
    /// </summary>
    [SugarColumn(ColumnName = "F_FeedProcess")]
    public string? F_FeedProcess { get; set; }

    /// <summary>
    /// 排序.
    /// </summary>
    [SugarColumn(ColumnName = "F_SortCode")]
    public int? F_SortCode { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    [SugarColumn(ColumnName = "F_Description")]
    public string? F_Description { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    [SugarColumn(ColumnName = "F_CreatorTime")]
    [CodeGenUpload("F_CreatorTime", "{\r\n  \"tableName\": \"a_prod_project_good\",\r\n  \"regList\": null,\r\n  \"jnpfKey\": \"createTime\",\r\n  \"rule\": null,\r\n  \"ruleType\": null,\r\n  \"dictionaryType\": null,\r\n  \"required\": false,\r\n  \"unique\": false,\r\n  \"label\": \"创建时间\",\r\n  \"dataType\": null,\r\n  \"propsUrl\": null,\r\n  \"children\": null,\r\n  \"props\": null,\r\n  \"showLevel\": null,\r\n  \"templateJson\": null,\r\n  \"startTimeRule\": false,\r\n  \"startTimeTarget\": 0,\r\n  \"startTimeType\": 0,\r\n  \"startTimeValue\": null,\r\n  \"startRelationField\": null,\r\n  \"endTimeRule\": false,\r\n  \"endTimeTarget\": 0,\r\n  \"endTimeType\": 0,\r\n  \"endTimeValue\": null,\r\n  \"endRelationField\": null,\r\n  \"precision\": null,\r\n  \"formId\": \"formItemcceba0\",\r\n  \"ruleConfig\": null,\r\n  \"IsBusinessKey\": false,\r\n  \"tag\": \"JnpfOpenData\"\r\n}")]
    public DateTime? F_CreatorTime { get; set; }

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

}