using JNPF.example.Entitys.Dto.ABomGoods;
using SqlSugar;
namespace JNPF.example.Entitys.Dto.ABom;

/// <summary>
/// a_bom输入参数.
/// </summary>
public class ABomListOutput
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
    public string? F_CategoryId { get; set; }

    /// <summary>
    /// 商品.
    /// </summary>
    public string? F_GoodsId { get; set; }
    public string? F_GoodsNo { get; set; }

    /// <summary>
    /// 状态.
    /// </summary>
    public string? F_State { get; set; }

    /// <summary>
    /// 默认BOM.
    /// </summary>
    public string F_IsDefault { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

    /// <summary>
    /// 创建组织.
    /// </summary>
    public string? F_CreatorOrganizeId { get; set; }

    /// <summary>
    /// .
    /// </summary>
    public List<ABomGoodsListOutput> tableFieldd2a80d { get; set; }

}