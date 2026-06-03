using JNPF.Common.Models;
using JNPF.JsonSerialization;
using Newtonsoft.Json;

namespace JNPF.example.Entitys.Dto.ACtcContact;
 
/// <summary>
/// 客户联系人修改输入参数.
/// </summary>
public class ACtcContactCrInput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string F_Id { get; set; }

    /// <summary>
    /// 是否默认.
    /// </summary>
    public int? F_IsDefault { get; set; }

    /// <summary>
    /// 联系人.
    /// </summary>
    public string? F_ContactName { get; set; }

    /// <summary>
    /// 联系人电话.
    /// </summary>
    public string? F_ContactPhone { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>

}