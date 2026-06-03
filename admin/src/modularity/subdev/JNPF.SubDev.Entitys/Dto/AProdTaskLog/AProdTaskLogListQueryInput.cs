using JNPF.Common.Filter;

namespace JNPF.example.Entitys.Dto.AProdTaskLog;

/// <summary>
/// 生产任务暂停记录列表查询输入.
/// </summary>
public class AProdTaskLogListQueryInput : PageInputBase
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
    /// 标题.
    /// </summary>
    public string F_Title { get; set; }

    /// <summary>
    /// 暂停原因.
    /// </summary>
    public string F_Content { get; set; }

}