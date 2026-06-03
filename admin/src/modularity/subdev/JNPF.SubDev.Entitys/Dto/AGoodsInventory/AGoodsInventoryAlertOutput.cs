namespace JNPF.example.Entitys.Dto.AGoodsInventory;

/// <summary>
/// 商品库存阈值告警输出.
/// </summary>
public class AGoodsInventoryAlertOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string id { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    public string F_GoodsId { get; set; }

    /// <summary>
    /// 商品编号.
    /// </summary>
    public string F_GoodsNo { get; set; }

    /// <summary>
    /// 商品名称.
    /// </summary>
    public string F_GoodsName { get; set; }

    /// <summary>
    /// 单位.
    /// </summary>
    public string F_Unit { get; set; }

    /// <summary>
    /// 规格.
    /// </summary>
    public string F_Specification { get; set; }

    /// <summary>
    /// 当前总库存数量.
    /// </summary>
    public decimal F_CurrentNum { get; set; }

    /// <summary>
    /// 库存上限阈值.
    /// </summary>
    public int F_StockUpperLimit { get; set; }

    /// <summary>
    /// 库存下限阈值.
    /// </summary>
    public int F_StockLowerLimit { get; set; }

    /// <summary>
    /// 告警类型: 1-库存不足(低于下限), 2-库存过剩(超过上限).
    /// </summary>
    public int alertType { get; set; }

    /// <summary>
    /// 告警状态编码(字典enCode).
    /// </summary>
    public string enCode { get; set; }

    /// <summary>
    /// 告警状态描述(字典fullName).
    /// </summary>
    public string alertStatus { get; set; }

    /// <summary>
    /// 偏离值(超过上限或低于下限的数量).
    /// </summary>
    public decimal deviationValue { get; set; }
}
