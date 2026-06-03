namespace JNPF.example.Entitys.Dto.APuOutReturnPuItem;
 
/// <summary>
/// 商品输出参数.
/// </summary>
public class APuOutReturnPuItemInfoOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? F_Id { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    public string? F_CustomerId { get; set; }

    /// <summary>
    /// 退货数量.
    /// </summary>
    public decimal? F_Num { get; set; }

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
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }


    /// <summary>
    /// 入库仓库.
    /// </summary>
    public List<string> F_WarehouseID { get; set; }
    public string? F_WarehouseId { get; set; }

    /// <summary>
    /// 库存编码.
    /// </summary>
    public string? F_Encoding { get; set; }



}