using JNPF.Common.Filter;

namespace JNPF.example.Entitys.Dto.AProdRoutingStep;

/// <summary>
/// 工艺路线工序管理列表查询输入.
/// </summary>
public class AProdRoutingStepListQueryInput : PageInputBase
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

}