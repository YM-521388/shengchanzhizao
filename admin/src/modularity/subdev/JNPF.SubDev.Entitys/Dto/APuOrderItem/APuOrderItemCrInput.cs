using JNPF.Common.Models;
using JNPF.JsonSerialization;
using Newtonsoft.Json;

namespace JNPF.example.Entitys.Dto.APuOrderItem;
 
/// <summary>
/// 采购单商品修改输入参数.
/// </summary>
public class APuOrderItemCrInput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string F_Id { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    public string? F_CustomerId { get; set; }

    /// <summary>
    /// 供应商ID.
    /// </summary>
    public string F_SupplierId { get; set; }

    /// <summary>
    /// 数量.
    /// </summary>
    public decimal? F_Num { get; set; }

    /// <summary>
    /// 单价.
    /// </summary>
    public decimal? F_Price { get; set; }

    /// <summary>
    /// 折扣.
    /// </summary>
    public decimal? F_Discount { get; set; }

    /// <summary>
    /// 优惠金额.
    /// </summary>
    public decimal? F_DiscountMoney { get; set; }

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