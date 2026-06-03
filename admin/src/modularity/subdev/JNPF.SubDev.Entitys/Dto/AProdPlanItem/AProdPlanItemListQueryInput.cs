using JNPF.Common.Filter;

namespace JNPF.example.Entitys.Dto.AProdPlanItem;

/// <summary>
/// 生产计划商品列表列表查询输入.
/// </summary>
public class AProdPlanItemListQueryInput : PageInputBase
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
    /// 商品ID.
    /// </summary>
    public string F_GoodsId { get; set; }

    /// <summary>
    /// 生产计划ID.
    /// </summary>
    public string F_ProdPlanId { get; set; }

}