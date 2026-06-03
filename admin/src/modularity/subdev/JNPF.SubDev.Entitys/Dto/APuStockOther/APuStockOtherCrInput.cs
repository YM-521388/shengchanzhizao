using JNPF.example.Entitys.Dto.APuStockOtherItem;
using JNPF.example.Entitys.Dto.APuStockOtherLog;
using JNPF.JsonSerialization;
using Newtonsoft.Json;
 
namespace JNPF.example.Entitys.Dto.APuStockOther;

/// <summary>
/// a_pu_stock_other修改输入参数.
/// </summary>
public class APuStockOtherCrInput
{
    /// <summary>
    /// 其他入库单号.
    /// </summary>
    public string? F_StockInNo { get; set; }

    /// <summary>
    /// 供应商ID.
    /// </summary>
    public string F_SupplierId { get; set; }

    /// <summary>
    /// 入库仓库ID.
    /// </summary>
    public List<string> F_WarehouseId { get; set; }

    /// <summary>
    /// 入库类型.
    /// </summary>
    public string F_StockInType { get; set; }

    /// <summary>
    /// 入库日期.
    /// </summary>
    public DateTime? F_StockInDate { get; set; }

    /// <summary>
    /// 收货人.
    /// </summary>
    public string F_StockInUserId { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    public string? F_Description { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    public string? F_CreatorUserId { get; set; }


    /// <summary>
    /// 其他入库单商品列表.
    /// </summary>
    public List<APuStockOtherItemCrInput> tableFieldecf5cb { get; set; }

    /// <summary>
    /// 其他入库单日志.
    /// </summary>
    public List<APuStockOtherLogCrInput> tableFielde1e6d9 { get; set; }

}