using JNPF.Common.Filter;

namespace JNPF.example.Entitys.Dto.AGoods;

/// <summary>
/// 商品管理列表查询输入.
/// </summary>
public class AGoodsListQueryInput : PageInputBase
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
    /// 商品编码.
    /// </summary>
    public string F_Encoding { get; set; }


    /// <summary>
    /// 商品编号.
    /// </summary>
    public string F_GoodsNo { get; set; }

    /// <summary>
    /// 商品名称.
    /// </summary>
    public string F_GoodsName { get; set; }

    /// <summary>
    /// 商品分类ID.
    /// </summary>
    public List<string> F_CategoryId { get; set; }

    /// <summary>
    /// 规格.
    /// </summary>
    public string F_Specification { get; set; }

    /// <summary>
    /// 检验规范.
    /// </summary>
    public string F_InspectRule { get; set; }



    /// <summary>
    /// 商品类型.
    /// </summary>
    public object F_Type { get; set; }


    /// <summary>
    /// 创建时间.
    /// </summary>
    public List<DateTime> F_CreatorTime { get; set; }

    /// <summary>
    /// 创建用户.
    /// </summary>
    public string F_CreatorUserId { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    public string? contractId { get; set; }

}