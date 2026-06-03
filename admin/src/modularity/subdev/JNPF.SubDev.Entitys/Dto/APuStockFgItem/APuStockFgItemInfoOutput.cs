using SqlSugar;

namespace JNPF.example.Entitys.Dto.APuStockFgItem;
 
/// <summary>
/// 选择商品输出参数.
/// </summary>
public class APuStockFgItemInfoOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? F_Id { get; set; }

    /// <summary>
    /// 工单类型.
    /// </summary>
    public string? F_Type { get; set; }

    /// <summary>
    /// 入库仓库ID.
    /// </summary>
    public List<string> F_WarehouseID { get; set; }

    /// <summary>
    /// 商品编码.
    /// </summary>
    public string? F_Encoding { get; set; }


    /// <summary>
    /// 工单ID.
    /// </summary>
    public string? F_WorkOrderId { get; set; }

    /// <summary>
    /// 仓库ID.
    /// </summary>
    public string? F_WarehouseId { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    public string? F_CustomerId { get; set; }
    public string? F_Specification { get; set; }
    public string F_GoodsName { get; set; }
    public string F_GoodsNo { get; set; }
    public string F_UnitTwo { get; set; }

    /// <summary>
    /// 库存.
    /// </summary>
    public decimal? F_InventoryNum { get; set; }

    /// <summary>
    /// 已入库数量.
    /// </summary>
    public decimal? F_YseNum { get; set; }

    /// <summary>
    /// 计划数.
    /// </summary>
    public int? F_PlanQty { get; set; }

    /// <summary>
    /// 入库数量.
    /// </summary>
    public decimal? F_Num { get; set; }
    public string? F_NumInfo { get; set; }

    /// <summary>
    /// 成本单价(元).
    /// </summary>
    public decimal? F_Price { get; set; }

    /// <summary>
    /// 单位比例.
    /// </summary>
    public string? F_Unit_Ratio { get; set; }

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