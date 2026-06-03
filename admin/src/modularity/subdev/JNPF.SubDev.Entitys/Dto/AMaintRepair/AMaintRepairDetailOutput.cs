namespace JNPF.example.Entitys.Dto.AMaintRepair;

/// <summary>
/// 维修工单详情输出参数.
/// </summary>
public class AMaintRepairDetailOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 工单编号.
    /// </summary>
    public string? F_RepairNo { get; set; }

    /// <summary>
    /// 维修设备.
    /// </summary>
    public string? F_DeviceId { get; set; }

    /// <summary>
    /// 分组.
    /// </summary>
    public string? F_GroupId { get; set; }

    /// <summary>
    /// 派单方式.
    /// </summary>
    public string? F_DispatchType { get; set; }

    /// <summary>
    /// 处理人.
    /// </summary>
    public string? F_HandlerUserId { get; set; }

    /// <summary>
    /// 故障描述.
    /// </summary>
    public string? F_Description { get; set; }

    /// <summary>
    /// 接单时间.
    /// </summary>
    public string F_ReceiveTime { get; set; }

    /// <summary>
    /// 处理时间.
    /// </summary>
    public string F_HandleTime { get; set; }

    /// <summary>
    /// 状态.
    /// </summary>
    public string? F_State { get; set; }

    /// <summary>
    /// 暂停原因.
    /// </summary>
    public string? F_PauseReason { get; set; }

    /// <summary>
    /// 取消原因.
    /// </summary>
    public string? F_CancelReason { get; set; }

    /// <summary>
    /// 审核人.
    /// </summary>
    public string? F_AuditorUserId { get; set; }

    /// <summary>
    /// 审核时间.
    /// </summary>
    public string F_AuditTime { get; set; }

    /// <summary>
    /// 审核备注.
    /// </summary>
    public string F_AuditReason { get; set; }

    /// <summary>
    /// 是否解决问题.
    /// </summary>
    public string? F_IsSolved { get; set; }

    /// <summary>
    /// 处理结果.
    /// </summary>
    public string? F_HandleResult { get; set; }

    /// <summary>
    /// 报修人.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 报修时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

    /// <summary>
    /// 创建组织.
    /// </summary>
    public string? F_CreatorOrganizeId { get; set; }

}