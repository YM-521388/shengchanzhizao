namespace JNPF.example.Entitys.Dto.AProdRoutingStepItem;
 
/// <summary>
/// 工艺路线工序物料信息输出参数.
/// </summary>
public class AProdRoutingStepItemInfoOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 工艺ID.
    /// </summary>
    public string? F_StepId { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    public string? F_GoodsId { get; set; }

    /// <summary>
    /// 数量.
    /// </summary>
    public int? F_Num { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    public string? F_Description { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

}