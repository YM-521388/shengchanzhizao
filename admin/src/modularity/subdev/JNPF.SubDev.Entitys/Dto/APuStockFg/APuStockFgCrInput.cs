using JNPF.example.Entitys.Dto.APuStockFgItem;
using JNPF.example.Entitys.Dto.APuStockFgLog;
using JNPF.JsonSerialization;
using Newtonsoft.Json;
 
namespace JNPF.example.Entitys.Dto.APuStockFg;

/// <summary>
/// a_pu_stock_fg修改输入参数.
/// </summary>
public class APuStockFgCrInput
{
    /// <summary>
    /// 成品入库单号.
    /// </summary>
    public string? F_StockInNo { get; set; }

    /// <summary>
    /// 工单类型.
    /// </summary>
    public string F_Type { get; set; }

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
    /// 成品入库单商品列表.
    /// </summary>
    public List<APuStockFgItemCrInput> tableFieldc43f9b { get; set; }

    /// <summary>
    /// 成品入库单日志.
    /// </summary>
    public List<APuStockFgLogCrInput> tableFieldfcaa70 { get; set; }

}