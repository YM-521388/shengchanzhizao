using JNPF.JsonSerialization;
using Newtonsoft.Json;

namespace JNPF.example.Entitys.Dto.AGoodsInventoryQr;
 
/// <summary>
/// 库存二维码修改输入参数.
/// </summary>
public class AGoodsInventoryQrCrInput
{
    /// <summary>
    /// 商品ID.
    /// </summary>
    public string? F_GoodsId { get; set; }

    /// <summary>
    /// 二维码编号.
    /// </summary>
    public string? F_Code { get; set; }

    /// <summary>
    /// 数量.
    /// </summary>
    public int? F_Num { get; set; }

    /// <summary>
    /// 创建组织.
    /// </summary>
    public string? F_CreatorOrganizeId { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    public string? F_Description { get; set; }

    /// <summary>
    /// 创建用户.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>

}