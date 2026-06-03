using JNPF.example.Entitys.Dto.APuReturnSoItem;
using JNPF.example.Entitys.Dto.APuReturnSoLog;
using SqlSugar;

namespace JNPF.example.Entitys.Dto.APuReturnSo;

/// <summary>
/// a_pu_return_so输入参数.
/// </summary>
public class APuReturnSoListOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 销售退货单号.
    /// </summary>
    public string? F_ReturnInNo { get; set; }

    /// <summary>
    /// 仓库ID.
    /// </summary>
    public string? F_WarehouseId { get; set; }

    /// <summary>
    /// 入库类型.
    /// </summary>
    public string? F_ReturnInType { get; set; }

    /// <summary>
    /// 退货日期.
    /// </summary>
    public string F_ReturnInDate { get; set; }

    /// <summary>
    /// 合同单号ID.
    /// </summary>
    public string? F_ContractId { get; set; }

    /// <summary>
    /// 客户ID.
    /// </summary>
    public string? F_CustomerId { get; set; }

    /// <summary>
    /// 联系人.
    /// </summary>
    public string? F_ContactId { get; set; }

    /// <summary>
    /// 地址.
    /// </summary>
    public string? F_AddressId { get; set; }

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

    /// <summary>
    /// .
    /// </summary>
    public List<APuReturnSoItemListOutput> tableFieldcfb049 { get; set; }

    /// <summary>
    /// .
    /// </summary>
    public List<APuReturnSoLogListOutput> tableField0c9c23 { get; set; }

}