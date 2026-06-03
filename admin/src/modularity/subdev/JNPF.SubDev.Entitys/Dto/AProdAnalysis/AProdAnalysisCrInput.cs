using JNPF.example.Entitys.Dto.AProdAnalysisItem;
using JNPF.JsonSerialization;
using Newtonsoft.Json;
 
namespace JNPF.example.Entitys.Dto.AProdAnalysis;

/// <summary>
/// a_prod_analysis修改输入参数.
/// </summary>
public class AProdAnalysisCrInput
{
    /// <summary>
    /// 生产计划ID.
    /// </summary>
    public string? F_ProdPlanId { get; set; }

    /// <summary>
    /// 是否考虑主商品库存.
    /// </summary>
    public string F_ConsiderMainStock { get; set; }

    /// <summary>
    /// 是否考虑物料占用.
    /// </summary>
    public string F_ConsiderMaterialOccupy { get; set; }

    /// <summary>
    /// 是否考虑在制商品.
    /// </summary>
    public string F_ConsiderWipGoods { get; set; }

    /// <summary>
    /// 占用物料能否被其他单据出库.
    /// </summary>
    public string F_OccupyAllowOtherOut { get; set; }

    /// <summary>
    /// 是否考虑物料库存.
    /// </summary>
    public string F_ConsiderMaterialStock { get; set; }

    /// <summary>
    /// 是否考虑数量向上取整.
    /// </summary>
    public string F_RoundUpQty { get; set; }

    /// <summary>
    /// 是否考虑在途商品.
    /// </summary>
    public string F_ConsiderTransitGoods { get; set; }

    /// <summary>
    /// 分析人.
    /// </summary>
    public string F_AnalysisUserId { get; set; }

    /// <summary>
    /// 分析时间.
    /// </summary>
    public DateTime? F_AnalysisTime { get; set; }

    /// <summary>
    /// 状态.
    /// </summary>
    public string? F_State { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    public string? F_Description { get; set; }

    /// <summary>
    /// 创建用户.
    /// </summary>
    public string? F_CreatorUserId { get; set; }


    /// <summary>
    /// 物料分析商品列表信息.
    /// </summary>
    public List<AProdAnalysisItemCrInput> tableField4d57ab { get; set; }

}