using JNPF.JsonSerialization;
using Newtonsoft.Json;

namespace JNPF.example.Entitys.Dto.ACtcContractItemGood;
 
/// <summary>
/// 项目商品用料清单修改输入参数.
/// </summary>
public class ACtcContractItemGoodCrInput
{
    public string id { get; set; }
    /// <summary>
    /// 项目商品ID.
    /// </summary>
    public string? F_ProjectItemId { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    public string F_GoodsId { get; set; }

    /// <summary>
    /// 单位用量.
    /// </summary>
    public int? F_Unit { get; set; }

    /// <summary>
    /// 生产用量.
    /// </summary>
    public int? F_ProdUnit { get; set; }

    /// <summary>
    /// 入库工序.
    /// </summary>
    public string F_StockInProcess { get; set; }

    /// <summary>
    /// 投料工序.
    /// </summary>
    public string F_FeedProcess { get; set; }

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