namespace JNPF.example.Entitys.Dto.AProdTaskItem;
 
/// <summary>
/// 物料清单输出参数.
/// </summary>
public class AProdTaskItemInfoOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? F_Id { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    public string F_GoodsId { get; set; }
    public string F_Specification { get; set; }
    public string F_Unit { get; set; }

    /// <summary>
    /// 数量.
    /// </summary>
    public int? F_Num { get; set; }
    public int? F_SortCode { get; set; }

    /// <summary>
    /// 库存.
    /// </summary>
    public decimal? F_InventoryNum { get; set; }

    /// <summary>
    /// 描述.
    /// </summary>
    public string? F_Description { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

}