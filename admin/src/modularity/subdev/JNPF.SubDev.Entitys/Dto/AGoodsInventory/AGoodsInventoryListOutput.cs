using SqlSugar;

namespace JNPF.example.Entitys.Dto.AGoodsInventory;

/// <summary>
/// 商品库存输入参数.
/// </summary>
public class AGoodsInventoryListOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    public string? F_GoodsId { get; set; }
    public string? F_GoodsNo { get; set; }

    /// <summary>
    /// 商品名称.
    /// </summary>
    public string? F_GoodsName { get; set; }

    /// <summary>
    /// 商品分类ID.
    /// </summary>
    public string? F_CategoryId { get; set; }

    /// <summary>
    /// 库存编码.
    /// </summary>
    public string? F_Code { get; set; }

    /// <summary>
    /// 商品类型.
    /// </summary>
    public string? F_Type { get; set; }

    /// <summary>
    /// 单位.
    /// </summary>
    public string? F_Unit { get; set; }

    /// <summary>
    /// 规格.
    /// </summary>
    public string? F_Specification { get; set; }

    /// <summary>
    /// 检验规范.
    /// </summary>
    public string? F_InspectRule { get; set; }

    /// <summary>
    /// 销售单价.
    /// </summary>
    public decimal? F_SalePrice { get; set; }

    /// <summary>
    /// 成本单价.
    /// </summary>
    public decimal? F_CostPrice { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    public string F_Remark { get; set; }

    /// <summary>
    /// 仓库ID.
    /// </summary>
    public string? F_WarehouseId { get; set; }
    public List<string>? F_WarehouseListId { get; set; }

    /// <summary>
    /// 库存数量.
    /// </summary>
    public decimal? F_Num { get; set; }
    public string? F_NumInfo { get; set; }
    public int? F_Count { get; set; }

    /// <summary>
    /// 单位比例.
    /// </summary>
    public string? F_Unit_Ratio { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

}