using JNPF.Common.Models;
using Newtonsoft.Json;
using JNPF.example.Entitys.Dto.ACtcAddress;
using JNPF.example.Entitys.Dto.ACtcContact;

namespace JNPF.example.Entitys.Dto.ACustomer;

/// <summary>
/// a_customer详情输出参数.
/// </summary>
public class ACustomerDetailOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 名称.
    /// </summary>
    public string? F_CustomerName { get; set; }

    /// <summary>
    /// 编号.
    /// </summary>
    public string? F_CustomerCode { get; set; }

    /// <summary>
    /// 二维码.
    /// </summary>
    public string? F_QRCode { get; set; }

    /// <summary>
    /// 公海客户.
    /// </summary>
    public string? F_IsPublic { get; set; }

    /// <summary>
    /// 客户标签.
    /// </summary>
    public string? F_CustomerTags { get; set; }

    /// <summary>
    /// 客户共享.
    /// </summary>
    public string? F_ShareUsers { get; set; }

    /// <summary>
    /// 关注.
    /// </summary>
    public string? F_IsFollow { get; set; }

    /// <summary>
    /// 状态.
    /// </summary>
    public string F_State { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    public string? F_Description { get; set; }

    /// <summary>
    /// 附件.
    /// </summary>
    public object? F_Files { get; set; }

    /// <summary>
    /// 领用时间.
    /// </summary>
    public string F_RequisitionTime { get; set; }

    /// <summary>
    /// 领用人员.
    /// </summary>
    public string? F_RequisitionUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 客户地址.
    /// </summary>
    public List<ACtcAddressDetailOutput> tableField6eed25 { get; set; }

    /// <summary>
    /// 客户联系人.
    /// </summary>
    public List<ACtcContactDetailOutput> tableFieldddabab { get; set; }

}