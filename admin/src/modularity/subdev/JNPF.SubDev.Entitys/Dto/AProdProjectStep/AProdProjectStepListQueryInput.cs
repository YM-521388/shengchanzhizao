using JNPF.Common.Filter;

namespace JNPF.example.Entitys.Dto.AProdProjectStep;

/// <summary>
/// 项目商品工序信息列表查询输入.
/// </summary>
public class AProdProjectStepListQueryInput : PageInputBase
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
    /// 项目商品ID.
    /// </summary>
    public string F_ProjcetItemId { get; set; }

    /// <summary>
    /// 工序ID.
    /// </summary>
    public string F_ProcessId { get; set; }

    /// <summary>
    /// 计划开始.
    /// </summary>
    public List<DateTime> F_StartDate { get; set; }

    /// <summary>
    /// 计划结束.
    /// </summary>
    public List<DateTime> F_EndDate { get; set; }

}