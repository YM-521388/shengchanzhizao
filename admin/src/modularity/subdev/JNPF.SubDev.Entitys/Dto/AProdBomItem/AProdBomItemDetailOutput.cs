namespace JNPF.example.Entitys.Dto.AProdBomItem;

/// <summary>
/// 用料清单详情输出参数.
/// </summary>
public class AProdBomItemDetailOutput
{
    /// <summary>
    /// 加工单ID.
    /// </summary>
    public string? F_ProcessingId { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    public string? F_GoodsId { get; set; }

    /// <summary>
    /// 单位用量.
    /// </summary>
    public int? F_Unit { get; set; }

    /// <summary>
    /// 入库工序.
    /// </summary>
    public string? F_StockInProcess { get; set; }

    /// <summary>
    /// 排序.
    /// </summary>
    public int? F_SortCode { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    public string? F_Description { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

}