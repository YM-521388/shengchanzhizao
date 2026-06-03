using JNPF.Common.Filter;

namespace JNPF.example.Entitys.Dto.AProdProcess;

/// <summary>
/// 工序管理列表查询输入.
/// </summary>
public class AProdProcessListQueryInput : PageInputBase
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
    /// 工序名.
    /// </summary>
    public string F_ProcessName { get; set; }

    /// <summary>
    /// 工序编号.
    /// </summary>
    public string F_ProcessCode { get; set; }

    /// <summary>
    /// 车间ID.
    /// </summary>
    public List<string> F_WorkshopId { get; set; }

    /// <summary>
    /// 产线.
    /// </summary>
    public string F_Line { get; set; }

    /// <summary>
    /// 生产人员.
    /// </summary>
    public string F_ProdUserIds { get; set; }

    /// <summary>
    /// 不良品项ID.
    /// </summary>
    public string F_DefectIds { get; set; }

    /// <summary>
    /// 质检人员.
    /// </summary>
    public string F_QcUserIds { get; set; }

    /// <summary>
    /// 机台id.
    /// </summary>
    public string machineId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public List<DateTime> F_CreatorTime { get; set; }

}