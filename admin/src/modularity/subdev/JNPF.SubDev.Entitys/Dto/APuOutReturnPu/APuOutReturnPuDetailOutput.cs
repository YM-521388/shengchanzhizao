using JNPF.example.Entitys.Dto.APuOutReturnPuItem;
using JNPF.example.Entitys.Dto.APuOutReturnPuLog;

namespace JNPF.example.Entitys.Dto.APuOutReturnPu;

/// <summary>
/// a_pu_out_return_pu详情输出参数.
/// </summary>
public class APuOutReturnPuDetailOutput
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
    /// 采购退货单商品列表.
    /// </summary>
    public List<APuOutReturnPuItemDetailOutput> tableFieldf5c8ef { get; set; }

    /// <summary>
    /// 采购退货单日志.
    /// </summary>
    public List<APuOutReturnPuLogDetailOutput> tableField3ff267 { get; set; }

}