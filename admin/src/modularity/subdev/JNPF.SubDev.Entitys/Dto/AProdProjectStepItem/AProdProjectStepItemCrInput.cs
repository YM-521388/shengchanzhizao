using JNPF.JsonSerialization;
using Newtonsoft.Json;

namespace JNPF.example.Entitys.Dto.AProdProjectStepItem;
 
/// <summary>
/// 项目商品工序物料信息修改输入参数.
/// </summary>
public class AProdProjectStepItemCrInput
{
    public string? id { get; set; }
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
    public int? F_Num { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    public string? F_Description { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>

}