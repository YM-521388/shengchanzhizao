using JNPF.Common.Models;
using JNPF.JsonSerialization;
using Newtonsoft.Json;

namespace JNPF.example.Entitys.Dto.AProdBomItem;
 
/// <summary>
/// 用料清单修改输入参数.
/// </summary>
public class AProdBomItemCrInput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string F_Id { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    public string F_GoodsId { get; set; }

    /// <summary>
    /// 单位用量.
    /// </summary>
    public int? F_Unit { get; set; }

    /// <summary>
    /// 入库工序.
    /// </summary>
    public string F_StockInProcess { get; set; }

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

}