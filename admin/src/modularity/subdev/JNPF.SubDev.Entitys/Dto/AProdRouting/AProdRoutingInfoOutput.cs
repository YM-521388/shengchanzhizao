using JNPF.example.Entitys.Dto.AProdRoutingStep;
 
namespace JNPF.example.Entitys.Dto.AProdRouting;

/// <summary>
/// a_prod_routing输出参数.
/// </summary>
public class AProdRoutingInfoOutput
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
    /// 创建用户.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

    /// <summary>
    /// 工艺路线工序信息.
    /// </summary>
    public List<AProdRoutingStepInfoOutput> tableField7b5631 { get; set; }

}