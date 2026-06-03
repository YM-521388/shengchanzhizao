namespace JNPF.example.Entitys.Dto.AProdOutsourceItem;
 
/// <summary>
/// 用料清单输出参数.
/// </summary>
public class AProdOutsourceItemInfoOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? F_Id { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    public string? F_GoodsId { get; set; }
    public string? F_GoodsName { get; set; }
    public string? F_GoodsNo { get; set; }
    public string? F_Specification { get; set; }
    public string? F_UnitTwo { get; set; }

    /// <summary>
    /// 成本单价.
    /// </summary>
    public decimal? F_CostPrice { get; set; }

    /// <summary>
    /// 库存.
    /// </summary>
    public decimal? F_InventoryNum { get; set; }

    /// <summary>
    /// 单位用量.
    /// </summary>
    public int? F_Unit { get; set; }
    public string? F_NumUnit { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    public string? F_Description { get; set; }

    /// <summary>
    /// 创建用户.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

}