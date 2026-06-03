using JNPF.Common.Filter;

namespace JNPF.example.Entitys.Dto.AProdProjectItem;

/// <summary>
/// 项目商品列表列表查询输入.
/// </summary>
public class AProdProjectItemListQueryInput : PageInputBase
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
    /// 项目ID.
    /// </summary>
    public string F_ProjectId { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    public string F_GoodsId { get; set; }

    /// <summary>
    /// 工单编号.
    /// </summary>
    public string F_WorkOrderNo { get; set; }

}