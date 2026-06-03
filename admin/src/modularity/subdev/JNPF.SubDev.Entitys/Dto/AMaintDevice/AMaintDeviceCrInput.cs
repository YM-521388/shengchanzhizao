using JNPF.Common.Models;
using JNPF.example.Entitys.Dto.AMaintDeviceItem;
using JNPF.JsonSerialization;
using Newtonsoft.Json;
 
namespace JNPF.example.Entitys.Dto.AMaintDevice;

/// <summary>
/// a_maint_device修改输入参数.
/// </summary>
public class AMaintDeviceCrInput
{
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
    public string F_DeviceStatus { get; set; }

    /// <summary>
    /// 设备类别.
    /// </summary>
    public List<string> F_DeviceType { get; set; }

    /// <summary>
    /// 分组.
    /// </summary>
    public string F_GroupId { get; set; }

    /// <summary>
    /// 设备负责人.
    /// </summary>
    public List<string> F_DeviceUsersId { get; set; }

    /// <summary>
    /// 车间.
    /// </summary>
    public List<string> F_WorkshopId { get; set; }

    /// <summary>
    /// 产线.
    /// </summary>
    public string F_LineId { get; set; }

    /// <summary>
    /// 设备二维码.
    /// </summary>
    public string? F_DeviceQrCode { get; set; }

    /// <summary>
    /// 设备图片.
    /// </summary>
    public List<FileControlsModel> F_DeviceImages { get; set; }

    /// <summary>
    /// 启用状态.
    /// </summary>
    public int? F_EnabledMark { get; set; }

    /// <summary>
    /// 创建用户.
    /// </summary>
    public string? F_CreatorUserId { get; set; }


    /// <summary>
    /// 设备物料清单.
    /// </summary>
    public List<AMaintDeviceItemCrInput> tableField72c841 { get; set; }

}