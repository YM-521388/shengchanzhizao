namespace JNPF.example.Entitys.Dto.AProdRoutingStepItem;

/// <summary>
/// 工艺路线工序物料信息输入参数.
/// </summary>
public class AProdRoutingStepItemListOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    public string? F_GoodsId { get; set; }
    public string? F_GoodsNo { get; set; }
    public string? F_Specification { get; set; }
    public string? F_Unit { get; set; }

    /// <summary>
    /// 库存.
    /// </summary>
    public decimal? F_InventoryNum { get; set; }

    /// <summary>
    /// 数量.
    /// </summary>
    public string F_Num { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    public string? F_Description { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

}