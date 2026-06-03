using JNPF.Common.Filter;

namespace JNPF.example.Entitys.Dto.AProdDefect;

/// <summary>
/// 不良品项列表查询输入.
/// </summary>
public class AProdDefectListQueryInput : PageInputBase
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
    /// 不良品项编号.
    /// </summary>
    public string F_DefectCode { get; set; }

    /// <summary>
    /// 不良品项名称.
    /// </summary>
    public string F_DefectName { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public List<DateTime> F_CreatorTime { get; set; }

}