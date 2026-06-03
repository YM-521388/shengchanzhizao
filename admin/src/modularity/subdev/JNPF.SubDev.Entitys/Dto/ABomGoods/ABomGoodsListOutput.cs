namespace JNPF.example.Entitys.Dto.ABomGoods;

/// <summary>
/// 设计子表输入参数.
/// </summary>
public class ABomGoodsListOutput
{
    public string? id { get; set; }
    /// <summary>
    /// 商品ID.
    /// </summary>
    public string? F_GoodsId { get; set; }
    public string? F_GoodsNo { get; set; }
    public string F_Specification { get; set; }

    /// <summary>
    /// 上级ID.
    /// </summary>
    public string? F_ParentId { get; set; }

    /// <summary>
    /// BOM关联ID（存储父级行的 F_Id 或 null）
    /// </summary>
    public string? F_BomId { get; set; }

    /// <summary>
    /// 投料工序.
    /// </summary>
    public string? F_Process { get; set; }

    /// <summary>
    /// 单位用量.
    /// </summary>
    public decimal? F_Unit { get; set; }
    public string? F_UnitTwo { get; set; }
    public string? F_NumUnit { get; set; }

    /// <summary>
    /// 库存.
    /// </summary>
    public decimal? F_InventoryNum { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    public string? F_Description { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

}