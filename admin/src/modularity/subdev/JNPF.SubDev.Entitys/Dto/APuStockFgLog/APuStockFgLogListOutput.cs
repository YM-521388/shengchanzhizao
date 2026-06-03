namespace JNPF.example.Entitys.Dto.APuStockFgLog;

/// <summary>
/// 日志记录输入参数.
/// </summary>
public class APuStockFgLogListOutput
{
    public string? id { get; set; }
    /// <summary>
    /// 成品入库单Id.
    /// </summary>
    public string? F_StockInFGId { get; set; }

    /// <summary>
    /// 类型.
    /// </summary>
    public string? F_Type { get; set; }

    /// <summary>
    /// 标题.
    /// </summary>
    public string? F_Title { get; set; }

    /// <summary>
    /// 内容.
    /// </summary>
    public string? F_Content { get; set; }

    /// <summary>
    /// 修改原因.
    /// </summary>
    public string? F_Reason { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

}