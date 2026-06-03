namespace JNPF.example.Entitys.Dto.ACtcAddress;

/// <summary>
/// 客户地址详情输出参数.
/// </summary>
public class ACtcAddressDetailOutput
{
    /// <summary>
    /// 客户ID.
    /// </summary>
    public string? F_CustomerId { get; set; }

    /// <summary>
    /// 是否默认.
    /// </summary>
    public string F_IsDefault { get; set; }

    /// <summary>
    /// 地区.
    /// </summary>
    public string? F_Region { get; set; }

    /// <summary>
    /// 地址.
    /// </summary>
    public string? F_Address { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

}