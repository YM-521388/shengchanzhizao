namespace JNPF.example.Entitys.Dto.AMaintDeviceItem;

/// <summary>
/// 物料清单详情输出参数.
/// </summary>
public class AMaintDeviceItemDetailOutput
{
    /// <summary>
    /// 设备ID.
    /// </summary>
    public string? F_DeviceId { get; set; }

    /// <summary>
    /// 物料ID.
    /// </summary>
    public string? F_GoodsId { get; set; }

    /// <summary>
    /// 数量.
    /// </summary>
    public int? F_Num { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    public string? F_Description { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

}