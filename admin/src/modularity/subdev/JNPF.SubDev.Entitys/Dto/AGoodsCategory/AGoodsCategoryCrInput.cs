using JNPF.JsonSerialization;
using Newtonsoft.Json;

namespace JNPF.example.Entitys.Dto.AGoodsCategory;
 
/// <summary>
/// 商品管理类别修改输入参数.
/// </summary>
public class AGoodsCategoryCrInput
{
    /// <summary>
    /// 上级ID.
    /// </summary>
    public string F_ParentId { get; set; }

    /// <summary>
    /// 分类名称.
    /// </summary>
    public string? F_Name { get; set; }

    /// <summary>
    /// 分类编号.
    /// </summary>
    public string? F_Code { get; set; }

    /// <summary>
    /// 排序.
    /// </summary>
    public int? F_SortCode { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    public string? F_Description { get; set; }

    /// <summary>
    /// 创建人.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>

}