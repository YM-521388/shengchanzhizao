using JNPF.example.Entitys.Dto.APuStockFgItem;
using JNPF.example.Entitys.Dto.APuStockFgLog;
using SqlSugar;

namespace JNPF.example.Entitys.Dto.APuStockFg;

/// <summary>
/// a_pu_stock_fg输入参数.
/// </summary>
public class APuStockFgListOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 成品入库单号.
    /// </summary>
    public string? F_StockInNo { get; set; }

    /// <summary>
    /// 工单类型.
    /// </summary>
    public string? F_Type { get; set; }

    /// <summary>
    /// 入库仓库ID.
    /// </summary>
    public string? F_WarehouseId { get; set; }

    /// <summary>
    /// 入库类型.
    /// </summary>
    public string? F_StockInType { get; set; }
    public string? F_StockInTypeCode { get; set; }

    /// <summary>
    /// 入库日期.
    /// </summary>
    public string F_StockInDate { get; set; }

    /// <summary>
    /// 操作方式.
    /// </summary>
    public string? F_Method { get; set; }

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

    /// <summary>
    /// .
    /// </summary>
    public List<APuStockFgItemListOutput> tableFieldc43f9b { get; set; }

    /// <summary>
    /// .
    /// </summary>
    public List<APuStockFgLogListOutput> tableFieldfcaa70 { get; set; }

}