using SqlSugar;

namespace JNPF.example.Entitys.Dto.APuTransfer;

/// <summary>
/// a_pu_transfer输入参数.
/// </summary>
public class APuTransferListOutput
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
    public string F_TransferDate { get; set; }

    /// <summary>
    /// 调拨人.
    /// </summary>
    public string? F_TransferUserId { get; set; }

    /// <summary>
    /// 发出仓库.
    /// </summary>
    public string? F_FromWarehouseId { get; set; }

    /// <summary>
    /// 收入仓库.
    /// </summary>
    public string? F_ToWarehouseId { get; set; }

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