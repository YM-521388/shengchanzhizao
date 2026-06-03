namespace JNPF.example.Entitys.Dto.APuCheckItem;
 
/// <summary>
/// 选择库存商品输出参数.
/// </summary>
public class APuCheckItemInfoOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? F_Id { get; set; }

    /// <summary>
    /// 编码.
    /// </summary>
    public string? F_Code { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    public string? F_CustomerId { get; set; }
    public string? F_Specification { get; set; }
    public string F_GoodsName { get; set; }
    public string F_GoodsNo { get; set; }
    public string F_UnitTwo { get; set; }

    /// <summary>
    /// 盘点前数量.
    /// </summary>
    public decimal? F_CheckQtyBefore { get; set; }
    public string? F_CheckQtyBeforeInfo { get; set; }

    /// <summary>
    /// 已盘点数量.
    /// </summary>
    public decimal? F_CheckQtyDone { get; set; }
    public string? F_CheckQtyDoneInfo { get; set; }

    /// <summary>
    /// 差异数量.
    /// </summary>
    public decimal? F_Differences { get; set; }
    public string? F_DifferencesInfo { get; set; }

    /// <summary>
    /// 单位.
    /// </summary>
    public string? F_Unit { get; set; }

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

}