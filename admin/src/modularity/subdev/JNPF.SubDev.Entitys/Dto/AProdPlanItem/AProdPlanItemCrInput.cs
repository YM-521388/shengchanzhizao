using JNPF.Common.Models;
using JNPF.JsonSerialization;
using Newtonsoft.Json;

namespace JNPF.example.Entitys.Dto.AProdPlanItem;
 
/// <summary>
/// 商品修改输入参数.
/// </summary>
public class AProdPlanItemCrInput
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
    /// 计划数量.
    /// </summary>
    public int? F_Num { get; set; }

    /// <summary>
    /// 商品BOM.
    /// </summary>
    public string F_BOMId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>

}