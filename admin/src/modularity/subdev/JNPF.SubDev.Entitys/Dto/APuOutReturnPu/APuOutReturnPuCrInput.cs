using JNPF.example.Entitys.Dto.APuOutReturnPuItem;
using JNPF.example.Entitys.Dto.APuOutReturnPuLog;
using JNPF.JsonSerialization;
using Newtonsoft.Json;
 
namespace JNPF.example.Entitys.Dto.APuOutReturnPu;

/// <summary>
/// a_pu_out_return_pu修改输入参数.
/// </summary>
public class APuOutReturnPuCrInput
{
    /// <summary>
    /// 采购退货单号.
    /// </summary>
    public string? F_ReturnOutNo { get; set; }

    /// <summary>
    /// 仓库ID.
    /// </summary>
    public List<string> F_WarehouseId { get; set; }

    /// <summary>
    /// 出库类型.
    /// </summary>
    public string F_ReturnOutType { get; set; }

    /// <summary>
    /// 退货日期.
    /// </summary>
    public DateTime? F_ReturnOutDate { get; set; }

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
    /// 采购退货单商品列表.
    /// </summary>
    public List<APuOutReturnPuItemCrInput> tableFieldf5c8ef { get; set; }

    /// <summary>
    /// 采购退货单日志.
    /// </summary>
    public List<APuOutReturnPuLogCrInput> tableField3ff267 { get; set; }

}