using JNPF.Common.Models;
using JNPF.JsonSerialization;
using Newtonsoft.Json;

namespace JNPF.example.Entitys.Dto.ACtcAddress;
 
/// <summary>
/// 客户地址修改输入参数.
/// </summary>
public class ACtcAddressCrInput
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
    /// 地区.
    /// </summary>
    public List<string> F_Region { get; set; }

    /// <summary>
    /// 地址.
    /// </summary>
    public string? F_Address { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>

}