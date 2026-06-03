using JNPF.Common.Filter;

namespace JNPF.example.Entitys.Dto.AMaintRepairLog;

/// <summary>
/// 维修工单日志列表查询输入.
/// </summary>
public class AMaintRepairLogListQueryInput : PageInputBase
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
    /// 维修工单.
    /// </summary>
    public string F_RepairId { get; set; }

    /// <summary>
    /// 标题.
    /// </summary>
    public string F_Title { get; set; }

    /// <summary>
    /// 内容.
    /// </summary>
    public string F_Content { get; set; }

}