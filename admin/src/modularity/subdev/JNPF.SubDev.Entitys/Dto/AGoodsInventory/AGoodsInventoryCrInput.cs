using JNPF.JsonSerialization;
using Newtonsoft.Json;

namespace JNPF.example.Entitys.Dto.AGoodsInventory;
 
/// <summary>
/// 商品库存修改输入参数.
/// </summary>
public class AGoodsInventoryCrInput
{
    /// <summary>
    /// 商品ID.
    /// </summary>
    public string F_GoodsId { get; set; }

    /// <summary>
    /// 仓库ID.
    /// </summary>
    public List<string> F_WarehouseId { get; set; }

    /// <summary>
    /// 库存数量.
    /// </summary>
    public decimal? F_Num { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>

}