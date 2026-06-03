namespace JNPF.example.Entitys.Dto.AMaintDeviceItem;
 
/// <summary>
/// 物料清单输出参数.
/// </summary>
public class AMaintDeviceItemInfoOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? F_Id { get; set; }

    /// <summary>
    /// 物料ID.
    /// </summary>
    public string F_GoodsId { get; set; }
    public string F_GoodsName { get; set; }
    public string F_GoodsNo { get; set; }
    public string F_Specification { get; set; }
    public string F_Unit { get; set; }
    public decimal? F_InventoryNum { get; set; }

    /// <summary>
    /// 数量.
    /// </summary>
    public int? F_Num { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    public string? F_Description { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

}