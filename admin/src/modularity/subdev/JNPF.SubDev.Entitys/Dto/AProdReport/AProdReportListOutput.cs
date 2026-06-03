using Microsoft.VisualBasic;
using SqlSugar;

namespace JNPF.example.Entitys.Dto.AProdReport;

/// <summary>
/// a_prod_report输入参数.
/// </summary>
public class AProdReportListOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 生产人员.
    /// </summary>
    public string? F_ProdUserId { get; set; }

    /// <summary>
    /// 良品数.
    /// </summary>
    public int? F_GoodQty { get; set; }

    /// <summary>
    /// 不良品数.
    /// </summary>
    public int? F_GoodNotQty { get; set; }

    /// <summary>
    /// 报工数量.
    /// </summary>
    public int? F_Num { get; set; }

    /// <summary>
    /// 开始时间.
    /// </summary>
    public string F_StartTime { get; set; }

    /// <summary>
    /// 结束时间.
    /// </summary>
    public string F_EndTime { get; set; }

    /// <summary>
    /// 开始时间.
    /// </summary>
    public DateTime? F_StartTimeOld { get; set; }

    /// <summary>
    /// 结束时间.
    /// </summary>
    public DateTime? F_EndTimeOld { get; set; }

    /// <summary>
    /// 报工时长.
    /// </summary>
    public string F_DurationStr { get; set; }

    /// <summary>
    /// 报工时长时间（分钟）.
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public int F_DurationMinutes { get; set; }

    /// <summary>
    /// 附件.
    /// </summary>
    public object? F_Files { get; set; }

    /// <summary>
    /// 结算状态.
    /// </summary>
    public string? F_SettleStatus { get; set; }
    public string? F_SettleStatusCode { get; set; }

    /// <summary>
    /// 结算人.
    /// </summary>
    public string? F_SettleUserId { get; set; }

    /// <summary>
    /// 结算时间.
    /// </summary>
    public string F_SettleTime { get; set; }

    /// <summary>
    /// 工资单价.
    /// </summary>
    public decimal? F_WagePrice { get; set; }

    /// <summary>
    /// 报工类型.
    /// </summary>
    public string? F_State { get; set; }

    /// <summary>
    /// 描述.
    /// </summary>
    public string? F_Description { get; set; }

    /// <summary>
    /// 创建人.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }


    /// <summary>
    /// 商品.
    /// </summary>
    public string? F_GoodsName { get; set; }
    public string? F_GoodsNo { get; set; }
    public string? F_Specification { get; set; }

    /// <summary>
    /// 计划开始.
    /// </summary>
    public string? F_PlanStartDate { get; set; }

    /// <summary>
    /// 计划结束.
    /// </summary>
    public string? F_PlanEndDate { get; set; }

    /// <summary>
    /// 工序名.
    /// </summary>
    public string? F_ProcessName { get; set; }

    /// <summary>
    /// 计价方式.
    /// </summary>
    public string? F_PriceType { get; set; }

    /// <summary>
    /// 加工单ID.
    /// </summary>
    public string? F_ProcessingId { get; set; }

    /// <summary>
    /// 加工单状态.
    /// </summary>
    public string? F_ProcessingState { get; set; }
    public string? F_ProcessingStateCode { get; set; }

    /// <summary>
    /// 创建组织.
    /// </summary>
    public string? F_CreatorOrganizeId { get; set; }

}

public class AProdReportAPPListOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 生产人员.
    /// </summary>
    public string? F_ProdUserId { get; set; }

    /// <summary>
    /// 良品数.
    /// </summary>
    public int? F_GoodQty { get; set; }

    /// <summary>
    /// 不良品数.
    /// </summary>
    public int? F_GoodNotQty { get; set; }

    /// <summary>
    /// 报工数量.
    /// </summary>
    public int? F_Num { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

}