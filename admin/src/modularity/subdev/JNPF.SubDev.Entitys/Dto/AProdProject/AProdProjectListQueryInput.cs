using JNPF.Common.Filter;

namespace JNPF.example.Entitys.Dto.AProdProject;

/// <summary>
/// a_prod_project列表查询输入.
/// </summary>
public class AProdProjectListQueryInput : PageInputBase
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
    /// 项目编号.
    /// </summary>
    public string F_ProjectNo { get; set; }

    /// <summary>
    /// 项目名称.
    /// </summary>
    public string F_ProjectName { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public List<DateTime> F_CreatorTime { get; set; }

    /// <summary>
    /// 修改时间.
    /// </summary>
    public List<DateTime> F_UpdateTime { get; set; }

}