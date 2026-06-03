using SqlSugar;

namespace JNPF.example.Entitys.Dto.ABomCategory;

/// <summary>
/// BOM类别管理输入参数.
/// </summary>
public class ABomCategoryListOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 分类名称.
    /// </summary>
    public string? F_Name { get; set; }

    /// <summary>
    /// 排序.
    /// </summary>
    public string F_SortCode { get; set; }

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
    public string F_CreatorTime { get; set; }

    /// <summary>
    /// 创建组织.
    /// </summary>
    public string? F_CreatorOrganizeId { get; set; }

}