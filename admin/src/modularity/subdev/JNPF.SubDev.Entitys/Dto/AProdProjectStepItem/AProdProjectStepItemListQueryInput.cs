using JNPF.Common.Filter;

namespace JNPF.example.Entitys.Dto.AProdProjectStepItem;

/// <summary>
/// 项目商品工序物料信息列表查询输入.
/// </summary>
public class AProdProjectStepItemListQueryInput : PageInputBase
{
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
    /// 项目商品工序ID.
    /// </summary>
    public string F_ProjectStepId { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    public string F_GoodsId { get; set; }

    /// <summary>
    /// 数量.
    /// </summary>
    public List<decimal?> F_Num { get; set; }

}