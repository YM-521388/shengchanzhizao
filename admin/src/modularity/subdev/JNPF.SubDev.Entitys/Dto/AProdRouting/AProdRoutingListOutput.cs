using SqlSugar;

namespace JNPF.example.Entitys.Dto.AProdRouting;

/// <summary>
/// a_prod_routing输入参数.
/// </summary>
public class AProdRoutingListOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 工艺路线编号.
    /// </summary>
    public string? F_RoutingNo { get; set; }

    /// <summary>
    /// 工艺路线名称.
    /// </summary>
    public string? F_RoutingName { get; set; }

    /// <summary>
    /// 工序个数.
    /// </summary>
    public int? F_ProcessNum { get; set; }

    /// <summary>
    /// 工序列表.
    /// </summary>
    public string? F_ProcessList { get; set; }

    /// <summary>
    /// 创建组织.
    /// </summary>
    public string? F_CreatorOrganizeId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

}