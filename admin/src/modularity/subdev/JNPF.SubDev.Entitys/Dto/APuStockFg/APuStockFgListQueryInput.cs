using JNPF.Common.Filter;

namespace JNPF.example.Entitys.Dto.APuStockFg;

/// <summary>
/// a_pu_stock_fg列表查询输入.
/// </summary>
public class APuStockFgListQueryInput : PageInputBase
{
    /// <summary>
    /// 高级查询.
    /// </summary>
    public string superQueryJson { get; set; }

    /// <summary>
    /// 选择导出数据ids.
    /// </summary>
    public string selectIds { get; set; }

    /// <summary>
    /// 选择导出数据key.
    /// </summary>
    public string selectKey { get; set; }

    /// <summary>
    /// 导出类型.
    /// </summary>
    public int dataType { get; set; }
    
    /// <summary>
    /// 关键词查询.
    /// </summary>
    public string jnpfKeyword { get; set; }

    /// <summary>
    /// ID.
    /// </summary>
    public string F_StockFgId { get; set; }

    /// <summary>
    /// 成品入库单号.
    /// </summary>
    public string F_StockInNo { get; set; }

    /// <summary>
    /// 工单类型.
    /// </summary>
    public string F_Type { get; set; }

    /// <summary>
    /// 入库仓库ID.
    /// </summary>
    //public string F_WarehouseId { get; set; }
    public List<string> F_WarehouseId { get; set; }

    /// <summary>
    /// 入库类型.
    /// </summary>
    public string F_StockInType { get; set; }

    /// <summary>
    /// 操作方式.
    /// </summary>
    public string F_Method { get; set; }


    /// <summary>
    /// 入库日期.
    /// </summary>
    public List<DateTime> F_StockInDate { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    public string F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public List<DateTime> F_CreatorTime { get; set; }

}