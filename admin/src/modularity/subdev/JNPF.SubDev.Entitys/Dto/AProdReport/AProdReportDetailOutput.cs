using JNPF.Common.Models;
using Newtonsoft.Json;
using JNPF.example.Entitys.Dto.AProdReportDefect;

namespace JNPF.example.Entitys.Dto.AProdReport;

/// <summary>
/// a_prod_report详情输出参数.
/// </summary>
public class AProdReportDetailOutput
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
    public string F_GoodQty { get; set; }

    /// <summary>
    /// 不良品数.
    /// </summary>
    public string F_GoodNotQty { get; set; }

    /// <summary>
    /// 开始时间.
    /// </summary>
    public string F_StartTime { get; set; }

    /// <summary>
    /// 结束时间.
    /// </summary>
    public string F_EndTime { get; set; }

    /// <summary>
    /// 附件.
    /// </summary>
    public object? F_Files { get; set; }

    /// <summary>
    /// 结算状态.
    /// </summary>
    public string? F_SettleStatus { get; set; }

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
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

    /// <summary>
    /// 报工不良品项信息.
    /// </summary>
    public List<AProdReportDefectDetailOutput> tableField579169 { get; set; }

}