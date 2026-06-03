namespace JNPF.example.Entitys.Dto.AProdRoutingStep;

/// <summary>
/// 工艺路线工序管理输入参数.
/// </summary>
public class AProdRoutingStepListOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 工序ID.
    /// </summary>
    public string? F_ProcessId { get; set; }

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
    /// 计价方式.
    /// </summary>
    public string? F_PriceType { get; set; }

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
    /// 生产任务计时.
    /// </summary>
    public string F_TaskTimer { get; set; }

    /// <summary>
    /// 附件.
    /// </summary>
    public object? F_Files { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    public string? F_Description { get; set; }

}