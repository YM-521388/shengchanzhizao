namespace JNPF.example.Entitys.Dto.AMaintDevice;

/// <summary>
/// a_maint_device输入参数.
/// </summary>
public class AMaintDeviceListOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 设备编号.
    /// </summary>
    public string? F_DeviceNo { get; set; }

    /// <summary>
    /// 设备名称.
    /// </summary>
    public string? F_DeviceName { get; set; }

    /// <summary>
    /// 设备规格.
    /// </summary>
    public string? F_DeviceSpec { get; set; }

    /// <summary>
    /// 设备状态.
    /// </summary>
    public string? F_DeviceStatus { get; set; }
    public string? F_DeviceStatusCode { get; set; }

    /// <summary>
    /// 设备类别.
    /// </summary>
    public string? F_DeviceType { get; set; }

    /// <summary>
    /// 分组.
    /// </summary>
    public string? F_GroupId { get; set; }

    /// <summary>
    /// 设备负责人.
    /// </summary>
    public string? F_DeviceUsersId { get; set; }

    /// <summary>
    /// 车间.
    /// </summary>
    public string? F_WorkshopId { get; set; }

    /// <summary>
    /// 产线.
    /// </summary>
    public string? F_LineId { get; set; }

    /// <summary>
    /// 设备二维码.
    /// </summary>
    public string? F_DeviceQrCode { get; set; }

    /// <summary>
    /// 启用状态.
    /// </summary>
    public string F_EnabledMark { get; set; }

    /// <summary>
    /// 创建用户.
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