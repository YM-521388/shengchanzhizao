namespace JNPF.example.Entitys.Dto.APuReturnPrdItem;
 
/// <summary>
/// 商品输出参数.
/// </summary>
public class APuReturnPrdItemInfoOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? F_Id { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    public string? F_CustomerId { get; set; }
    public string? F_Specification { get; set; }
    public string F_GoodsName { get; set; }
    public string F_GoodsNo { get; set; }
    public string F_UnitTwo { get; set; }

    /// <summary>
    /// 退料数量.
    /// </summary>
    public int? F_Num { get; set; }
    public string? F_NumInfo { get; set; }

    /// <summary>
    /// 成本单价(元).
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
    /// 入库仓库（存库为 JSON 数组字符串；列表/详情接口可转为名称展示）.
    /// </summary>
    public List<string>? F_WarehouseID { get; set; }
    public string? F_WarehouseId { get; set; }

    /// <summary>
    /// 商品编码.
    /// </summary>
    public string? F_Encoding { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

}