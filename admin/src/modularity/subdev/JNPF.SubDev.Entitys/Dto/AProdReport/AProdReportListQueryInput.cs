using JNPF.Common.Filter;
using SqlSugar;

namespace JNPF.example.Entitys.Dto.AProdReport;

/// <summary>
/// a_prod_report列表查询输入.
/// </summary>
public class AProdReportListQueryInput : PageInputBase
{
    /// <summary>
    /// 选择导出数据ids.
    /// </summary>
    public string selectIds { get; set; }

    /// <summary>
    /// 选择导出数据key.
    /// </summary>
    public string selectKey { get; set; }

    /// <summary>
    /// 导出类型.
    /// </summary>
    public int dataType { get; set; }
    
    /// <summary>
    /// 关键词查询.
    /// </summary>
    public string jnpfKeyword { get; set; }

    /// <summary>
    /// 报工ID.
    /// </summary>
    public string F_ReportId { get; set; }

    /// <summary>
    /// 加工单ID.
    /// </summary>
    public string F_ProcessingId { get; set; }

    /// <summary>
    /// 工序ID.
    /// </summary>
    public string F_ProcessId { get; set; }

    /// <summary>
    /// 生产人员.
    /// </summary>
    public string F_ProdUserId { get; set; }

    /// <summary>
    /// 良品数.
    /// </summary>
    public List<decimal?> F_GoodQty { get; set; }

    /// <summary>
    /// 不良品数.
    /// </summary>
    public List<decimal?> F_GoodNotQty { get; set; }

    /// <summary>
    /// 开始时间.
    /// </summary>
    public List<DateTime> F_StartTime { get; set; }

    /// <summary>
    /// 结束时间.
    /// </summary>
    public List<DateTime> F_EndTime { get; set; }

    /// <summary>
    /// 描述.
    /// </summary>
    public string F_Description { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public List<DateTime> F_CreatorTime { get; set; }

    /// <summary>
    /// 不良品项ID.
    /// </summary>
    public string tableField579169_F_DefectId { get; set; }


    /// <summary>
    /// 生产任务ID.
    /// </summary>
    public string? F_TaskId { get; set; }
}