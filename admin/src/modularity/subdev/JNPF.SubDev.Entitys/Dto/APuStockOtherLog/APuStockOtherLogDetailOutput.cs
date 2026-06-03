namespace JNPF.example.Entitys.Dto.APuStockOtherLog;

/// <summary>
/// 设计子表详情输出参数.
/// </summary>
public class APuStockOtherLogDetailOutput
{
    /// <summary>
    /// 其他入库单Id.
    /// </summary>
    public string? F_StockInOTId { get; set; }

    /// <summary>
    /// 标题.
    /// </summary>
    public string? F_Title { get; set; }

    /// <summary>
    /// 内容.
    /// </summary>
    public string? F_Content { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

}