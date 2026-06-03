using JNPF.Common.Models;
using JNPF.example.Entitys.Dto.AProdReportDefect;
using JNPF.JsonSerialization;
using Newtonsoft.Json;
 
namespace JNPF.example.Entitys.Dto.AProdReport;

/// <summary>
/// a_prod_report修改输入参数.
/// </summary>
public class AProdReportCrInput
{
    public string F_TaskId { get; set; }
    /// <summary>
    /// 生产人员.
    /// </summary>
    public string F_ProdUserId { get; set; }

    /// <summary>
    /// 良品数.
    /// </summary>
    public int? F_GoodQty { get; set; }

    /// <summary>
    /// 不良品数.
    /// </summary>
    public int? F_GoodNotQty { get; set; }

    /// <summary>
    /// 开始时间.
    /// </summary>
    public DateTime? F_StartTime { get; set; }

    /// <summary>
    /// 结束时间.
    /// </summary>
    public DateTime? F_EndTime { get; set; }

    /// <summary>
    /// 附件.
    /// </summary>
    public List<FileControlsModel> F_Files { get; set; }

    /// <summary>
    /// 结算状态.
    /// </summary>
    public string F_SettleStatus { get; set; }

    /// <summary>
    /// 结算人.
    /// </summary>
    public string F_SettleUserId { get; set; }

    /// <summary>
    /// 结算时间.
    /// </summary>
    public DateTime? F_SettleTime { get; set; }

    /// <summary>
    /// 工资单价.
    /// </summary>
    public decimal? F_WagePrice { get; set; }

    /// <summary>
    /// 报工类型.
    /// </summary>
    public string F_State { get; set; }

    /// <summary>
    /// 描述.
    /// </summary>
    public string? F_Description { get; set; }
    public string? F_DescriptionXin { get; set; }

    /// <summary>
    /// 创建用户.
    /// </summary>
    public List<string> F_CreatorUserId { get; set; }


    /// <summary>
    /// 报工不良品项信息.
    /// </summary>
    public List<AProdReportDefectCrInput> tableField579169 { get; set; }



    /// <summary>
    /// 生产人员 - APP.
    /// </summary>
    public string? F_ProdAPPUserId { get; set; }
}