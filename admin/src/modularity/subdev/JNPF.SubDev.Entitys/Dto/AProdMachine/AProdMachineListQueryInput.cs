using JNPF.Common.Filter;

namespace JNPF.example.Entitys.Dto.AProdMachine;

/// <summary>
/// 机台管理列表查询输入.
/// </summary>
public class AProdMachineListQueryInput : PageInputBase
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
    /// 机台编号.
    /// </summary>
    public string F_MachineCode { get; set; }

    /// <summary>
    /// 机台名称.
    /// </summary>
    public string F_MachineName { get; set; }

    /// <summary>
    /// 机台状态.
    /// </summary>
    public string F_MachineStatus { get; set; }

    /// <summary>
    /// 机台类别.
    /// </summary>
    public List<string> F_MachineType { get; set; }

    /// <summary>
    /// 车间.
    /// </summary>
    public List<string> F_WorkshopId { get; set; }

    /// <summary>
    /// 产线.
    /// </summary>
    public string F_LineId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public List<DateTime> F_CreatorTime { get; set; }

}