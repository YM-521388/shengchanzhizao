namespace JNPF.example.Entitys.Dto.AProdBomItem;

/// <summary>
/// 根据采购单获取物料库存输出
/// </summary>
public class APuOrderInventoryOutput
{
    /// <summary>
    /// 仓库名称（拼接）
    /// </summary>
    public string? F_WarehouseId { get; set; }

    /// <summary>
    /// 仓库ID列表（原始JSON格式）
    /// </summary>
    public string? F_WarehouseIdList { get; set; }

    /// <summary>
    /// 数量
    /// </summary>
    public decimal? F_Num { get; set; }

    /// <summary>
    /// 编码
    /// </summary>
    public string? F_Code { get; set; }

    /// <summary>
    /// 商品ID
    /// </summary>
    public string? F_GoodsId { get; set; }

    /// <summary>
    /// 商品名称
    /// </summary>
    public string? F_GoodsName { get; set; }

    /// <summary>
    /// 商品编号
    /// </summary>
    public string? F_GoodsNo { get; set; }

    /// <summary>
    /// 采购数量（来自采购单商品 a_pu_order_item.F_Num，按 F_GoodsId 匹配）.
    /// </summary>
    public decimal? F_OrderNum { get; set; }

    /// <summary>
    /// 单位换算比例（来自商品表 a_goods.F_Unit_Ratio，按 F_GoodsId 匹配）.
    /// </summary>
    public string? F_Unit_Ratio { get; set; }
}
