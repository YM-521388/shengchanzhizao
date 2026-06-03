namespace JNPF.example.Entitys.Dto.APuPlanItem;

/// <summary>
/// 选择采购商品详情输出参数.
/// </summary>
public class APuPlanItemDetailOutput
{
    /// <summary>
    /// 采购计划ID.
    /// </summary>
    public string? F_PlanId { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    public string? F_CustomerId { get; set; }

    /// <summary>
    /// 供应商ID.
    /// </summary>
    public string? F_SupplierId { get; set; }

    /// <summary>
    /// 数量.
    /// </summary>
    public decimal? F_Num { get; set; }

    /// <summary>
    /// 单价.
    /// </summary>
    public decimal? F_Price { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    public string? F_Description { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }
    
    /// <summary>
    /// 商品编号.
    /// </summary>
    public string? F_GoodsNo { get; set; }
    public string? F_GoodsName { get; set; }

    /// <summary>
    /// 单位.
    /// </summary>
    public string? F_Unit { get; set; }

    /// <summary>
    /// 规格.
    /// </summary>
    public string? F_Specification { get; set; }

    /// <summary>
    /// 商品图片.
    /// </summary>
    public string? F_Image { get; set; }

}