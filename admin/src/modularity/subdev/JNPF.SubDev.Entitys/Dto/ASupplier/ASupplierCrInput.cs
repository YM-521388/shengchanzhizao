using JNPF.Common.Models;
using JNPF.JsonSerialization;
using Newtonsoft.Json;

namespace JNPF.example.Entitys.Dto.ASupplier;
 
/// <summary>
/// 供应商管理2修改输入参数.
/// </summary>
public class ASupplierCrInput
{
    /// <summary>
    /// 供应商名称.
    /// </summary>
    public string? F_SupplierName { get; set; }

    /// <summary>
    /// 供应商编号.
    /// </summary>
    public string? F_SupplierNo { get; set; }

    /// <summary>
    /// 联系人.
    /// </summary>
    public string? F_ContactPerson { get; set; }

    /// <summary>
    /// 联系人电话.
    /// </summary>
    public string? F_ContactPhone { get; set; }

    /// <summary>
    /// 地区.
    /// </summary>
    public List<string> F_Region { get; set; }

    /// <summary>
    /// 详细地址.
    /// </summary>
    public string? F_DetailAddress { get; set; }

    /// <summary>
    /// 供应商标签.
    /// </summary>
    public List<string> F_SupplierTags { get; set; }

    /// <summary>
    /// 附件.
    /// </summary>
    public List<FileControlsModel> F_AttachmentUrls { get; set; }

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

    /// <summary>
    /// 是否启用.
    /// </summary>
    public int? F_StartUsing { get; set; }

}