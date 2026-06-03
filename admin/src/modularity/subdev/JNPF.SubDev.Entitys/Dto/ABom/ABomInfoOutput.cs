using JNPF.example.Entitys.Dto.ABomGoods;
 
namespace JNPF.example.Entitys.Dto.ABom;

/// <summary>
/// a_bom输出参数.
/// </summary>
public class ABomInfoOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

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
    public int F_IsDefault { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

    /// <summary>
    /// BOM商品结构信息.
    /// </summary>
    public List<ABomGoodsInfoOutput> tableFieldd2a80d { get; set; }

}