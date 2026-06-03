using SqlSugar;

namespace JNPF.example.Entitys.Dto.APuOutStockSo;

/// <summary>
/// a_pu_out_stock_so输入参数.
/// </summary>
public class APuOutStockSoListOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 销售出库单号.
    /// </summary>
    public string? F_StockOutNo { get; set; }

    /// <summary>
    /// 仓库ID.
    /// </summary>
    public string? F_WarehouseId { get; set; }

    /// <summary>
    /// 出库类型.
    /// </summary>
    public string? F_StockOutType { get; set; }

    /// <summary>
    /// 出库日期.
    /// </summary>
    public string F_StockOutDate { get; set; }

    /// <summary>
    /// 发货人.
    /// </summary>
    public string? F_StockOutUserId { get; set; }

    /// <summary>
    /// 客户ID.
    /// </summary>
    public string? F_CustomerId { get; set; }

    /// <summary>
    /// 联系人.
    /// </summary>
    public string? F_ContactId { get; set; }

    /// <summary>
    /// 地址.
    /// </summary>
    public string? F_AddressId { get; set; }

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
    /// 创建组织.
    /// </summary>
    public string? F_CreatorOrganizeId { get; set; }

}