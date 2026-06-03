using JNPF.Common.Filter;

namespace JNPF.example.Entitys.Dto.APuTransfer;

/// <summary>
/// a_pu_transfer列表查询输入.
/// </summary>
public class APuTransferListQueryInput : PageInputBase
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
    public string F_TransferId { get; set; }

    /// <summary>
    /// 调拨日期.
    /// </summary>
    public List<DateTime> F_TransferDate { get; set; }

    /// <summary>
    /// 调拨人.
    /// </summary>
    public object F_TransferUserId { get; set; }

    /// <summary>
    /// 发出仓库.
    /// </summary>
    public string F_FromWarehouseId { get; set; }

    /// <summary>
    /// 收入仓库.
    /// </summary>
    public string F_ToWarehouseId { get; set; }


    /// <summary>
    /// 创建人员.
    /// </summary>
    public string F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public List<DateTime> F_CreatorTime { get; set; }

}