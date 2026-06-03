using JNPF.example.Entitys.Dto.ABomGoods;
using JNPF.JsonSerialization;
using Newtonsoft.Json;
 
namespace JNPF.example.Entitys.Dto.ABom;

/// <summary>
/// a_bom修改输入参数.
/// </summary>
public class ABomCrInput
{
    /// <summary>
    /// BOM编号.
    /// </summary>
    public string? F_BomCode { get; set; }

    /// <summary>
    /// BOM名称.
    /// </summary>
    public string? F_BomName { get; set; }

    /// <summary>
    /// 类别.
    /// </summary>
    public List<string> F_CategoryId { get; set; }

    /// <summary>
    /// 商品.
    /// </summary>
    public string? F_GoodsId { get; set; }

    /// <summary>
    /// 状态.
    /// </summary>
    public string? F_State { get; set; }

    /// <summary>
    /// 默认BOM.
    /// </summary>
    public int? F_IsDefault { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    public string? F_CreatorUserId { get; set; }


    /// <summary>
    /// 修改原因 .
    /// </summary>
    public string? F_Reason { get; set; }


    /// <summary>
    /// BOM商品结构信息.
    /// </summary>
    public List<ABomGoodsCrInput> tableFieldd2a80d { get; set; }

}