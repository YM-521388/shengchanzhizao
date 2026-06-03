using JNPF.example.Entitys.Dto.AProdRoutingStep;
using JNPF.JsonSerialization;
using Newtonsoft.Json;
 
namespace JNPF.example.Entitys.Dto.AProdRouting;

/// <summary>
/// a_prod_routing修改输入参数.
/// </summary>
public class AProdRoutingCrInput
{
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
    /// 工艺路线工序信息.
    /// </summary>
    public List<AProdRoutingStepCrInput> tableField7b5631 { get; set; }

}