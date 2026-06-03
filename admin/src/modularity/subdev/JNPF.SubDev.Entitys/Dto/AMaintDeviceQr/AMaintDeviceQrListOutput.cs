using SqlSugar;

namespace JNPF.example.Entitys.Dto.AMaintDeviceQr;

/// <summary>
/// 设备二维码输入参数.
/// </summary>
public class AMaintDeviceQrListOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 二维码编号.
    /// </summary>
    public string? F_Code { get; set; }

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
    /// 绑定状态.
    /// </summary>
    public string? F_State { get; set; }

    /// <summary>
    /// 生成用户.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 生成时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

    /// <summary>
    /// 创建组织.
    /// </summary>
    public string? F_CreatorOrganizeId { get; set; }

}