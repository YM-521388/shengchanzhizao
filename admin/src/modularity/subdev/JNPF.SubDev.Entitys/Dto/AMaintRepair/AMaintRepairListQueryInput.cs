using JNPF.Common.Filter;

namespace JNPF.example.Entitys.Dto.AMaintRepair;

/// <summary>
/// 维修工单列表查询输入.
/// </summary>
public class AMaintRepairListQueryInput : PageInputBase
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
    /// 工单编号.
    /// </summary>
    public string F_RepairNo { get; set; }

    /// <summary>
    /// 处理人.
    /// </summary>
    public string F_HandlerUserId { get; set; }

    /// <summary>
    /// 状态.
    /// </summary>
    public string F_State { get; set; }

    /// <summary>
    /// 审核人.
    /// </summary>
    public string F_AuditorUserId { get; set; }

    /// <summary>
    /// 是否解决问题.
    /// </summary>
    public string F_IsSolved { get; set; }

    /// <summary>
    /// 报修人.
    /// </summary>
    public string F_CreatorUserId { get; set; }

}