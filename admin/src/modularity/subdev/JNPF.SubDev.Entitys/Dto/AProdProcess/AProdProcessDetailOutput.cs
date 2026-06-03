using JNPF.Common.Models;
using Newtonsoft.Json;

namespace JNPF.example.Entitys.Dto.AProdProcess;

/// <summary>
/// 工序管理详情输出参数.
/// </summary>
public class AProdProcessDetailOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 工序名.
    /// </summary>
    public string? F_ProcessName { get; set; }

    /// <summary>
    /// 工序编号.
    /// </summary>
    public string? F_ProcessCode { get; set; }

    /// <summary>
    /// 车间ID.
    /// </summary>
    public string? F_WorkshopId { get; set; }

    /// <summary>
    /// 产线.
    /// </summary>
    public string? F_Line { get; set; }

    /// <summary>
    /// 报工单位.
    /// </summary>
    public string? F_ReportUnit { get; set; }

    /// <summary>
    /// 单位关系.
    /// </summary>
    public string F_UnitRatio { get; set; }

    /// <summary>
    /// 计价方式.
    /// </summary>
    public string? F_PriceType { get; set; }

    /// <summary>
    /// 工资单价.
    /// </summary>
    public decimal? F_WagePrice { get; set; }

    /// <summary>
    /// 标准工时(时).
    /// </summary>
    public string F_StdHour { get; set; }

    /// <summary>
    /// 标准工时(分).
    /// </summary>
    public string F_StdMinute { get; set; }

    /// <summary>
    /// 标准工时(秒).
    /// </summary>
    public string F_StdSecond { get; set; }

    /// <summary>
    /// 附件.
    /// </summary>
    public object? F_Files { get; set; }

    /// <summary>
    /// 机台ID.
    /// </summary>
    public string? F_MachineId { get; set; }

    /// <summary>
    /// 生产人员.
    /// </summary>
    public string? F_ProdUserIds { get; set; }

    /// <summary>
    /// 不良品项ID.
    /// </summary>
    public string? F_DefectIds { get; set; }

    /// <summary>
    /// 工序需外协.
    /// </summary>
    public string F_IsOutsource { get; set; }

    /// <summary>
    /// 工序需质检.
    /// </summary>
    public string F_IsQc { get; set; }

    /// <summary>
    /// 不良品需报废、返工.
    /// </summary>
    public string F_DefectHandle { get; set; }

    /// <summary>
    /// 质检人员.
    /// </summary>
    public string? F_QcUserIds { get; set; }

    /// <summary>
    /// 生产任务计时.
    /// </summary>
    public string F_TaskTimer { get; set; }

    /// <summary>
    /// 状态.
    /// </summary>
    public string F_State { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    public string? F_Description { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

}