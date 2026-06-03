using JNPF.JsonSerialization;
using Newtonsoft.Json;

namespace JNPF.example.Entitys.Dto.AGoodsSpecification;
 
/// <summary>
/// 商品规格修改输入参数.
/// </summary>
public class AGoodsSpecificationCrInput
{
    /// <summary>
    /// 规格名称.
    /// </summary>
    public string? F_Name { get; set; }

    /// <summary>
    /// 上级id.
    /// </summary>
    public List<string> F_ParentId { get; set; }

    /// <summary>
    /// 排序.
    /// </summary>
    public int? F_SortCode { get; set; }

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