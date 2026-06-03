using SqlSugar;

namespace JNPF.example.Entitys.Dto.APuOutReturnPu;

/// <summary>
/// a_pu_out_return_pu输入参数.
/// </summary>
public class APuOutReturnPuListOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 采购退货单号.
    /// </summary>
    public string? F_ReturnOutNo { get; set; }

    /// <summary>
    /// 仓库ID.
    /// </summary>
    public string? F_WarehouseId { get; set; }

    /// <summary>
    /// 出库类型.
    /// </summary>
    public string? F_ReturnOutType { get; set; }

    /// <summary>
    /// 退货日期.
    /// </summary>
    public string F_ReturnOutDate { get; set; }

    /// <summary>
    /// 采购单ID.
    /// </summary>
    public string? F_OrderId { get; set; }

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