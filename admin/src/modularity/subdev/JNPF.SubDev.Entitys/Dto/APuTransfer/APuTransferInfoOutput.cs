using JNPF.example.Entitys.Dto.APuTransferItem;
using JNPF.example.Entitys.Dto.APuTransferLog;
 
namespace JNPF.example.Entitys.Dto.APuTransfer;

/// <summary>
/// a_pu_transfer输出参数.
/// </summary>
public class APuTransferInfoOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 调拨单号.
    /// </summary>
    public string? F_TransferNo { get; set; }

    /// <summary>
    /// 调拨日期.
    /// </summary>
    public DateTime? F_TransferDate { get; set; }

    /// <summary>
    /// 调拨人.
    /// </summary>
    public string F_TransferUserId { get; set; }

    /// <summary>
    /// 发出仓库.
    /// </summary>
    public List<string> F_FromWarehouseId { get; set; }

    /// <summary>
    /// 收入仓库.
    /// </summary>
    public List<string> F_ToWarehouseId { get; set; }


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
    /// 调拨单商品列表.
    /// </summary>
    public List<APuTransferItemInfoOutput> tableFieldaf2f04 { get; set; }

    /// <summary>
    /// 调拨单日志.
    /// </summary>
    public List<APuTransferLogInfoOutput> tableFieldc39d26 { get; set; }

}