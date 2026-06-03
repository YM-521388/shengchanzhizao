using JNPF.Common.Filter;

namespace JNPF.example.Entitys.Dto.AGoodsSpecification;

/// <summary>
/// 商品规格列表查询输入.
/// </summary>
public class AGoodsSpecificationListQueryInput : PageInputBase
{
    /// <summary>
    /// 高级查询.
    /// </summary>
    public string superQueryJson { get; set; }

    /// <summary>
    /// 选择导出数据ids.
    /// </summary>
    public string selectIds { get; set; }

    /// <summary>
    /// 选择导出数据key.
    /// </summary>
    public string selectKey { get; set; }

    /// <summary>
    /// 导出类型.
    /// </summary>
    public int dataType { get; set; }
    
    /// <summary>
    /// 关键词查询.
    /// </summary>
    public string jnpfKeyword { get; set; }

    /// <summary>
    /// 规格名称.
    /// </summary>
    public string F_Name { get; set; }

    /// <summary>
    /// 上级id.
    /// </summary>
    public List<string> F_ParentId { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    public string F_Description { get; set; }

}