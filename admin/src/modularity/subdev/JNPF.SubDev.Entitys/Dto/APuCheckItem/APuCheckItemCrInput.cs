using JNPF.Common.Models;
using JNPF.JsonSerialization;
using Newtonsoft.Json;

namespace JNPF.example.Entitys.Dto.APuCheckItem;
 
/// <summary>
/// 选择库存商品修改输入参数.
/// </summary>
public class APuCheckItemCrInput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string F_Id { get; set; }

    /// <summary>
    /// 库存编码.
    /// </summary>
    public string F_Code { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    public string? F_CustomerId { get; set; }

    /// <summary>
    /// 盘点前数量.
    /// </summary>
    public decimal? F_CheckQtyBefore { get; set; }

    /// <summary>
    /// 已盘点数量.
    /// </summary>
    public decimal? F_CheckQtyDone { get; set; }

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