using JNPF.Common.Models;
using JNPF.JsonSerialization;
using Newtonsoft.Json;

namespace JNPF.example.Entitys.Dto.ABomGoods;
 
/// <summary>
/// 设计子表修改输入参数.
/// </summary>
public class ABomGoodsCrInput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string F_Id { get; set; }


    /// <summary>
    /// 子表父级id.
    /// </summary>
    public string? F_BomId { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    public string? F_GoodsId { get; set; }

    /// <summary>
    /// 投料工序.
    /// </summary>
    public string F_Process { get; set; }

    /// <summary>
    /// 单位用量.
    /// </summary>
    public decimal? F_Unit { get; set; }

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

}