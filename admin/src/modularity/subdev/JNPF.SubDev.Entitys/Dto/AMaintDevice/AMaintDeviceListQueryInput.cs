using JNPF.Common.Filter;

namespace JNPF.example.Entitys.Dto.AMaintDevice;

/// <summary>
/// a_maint_device列表查询输入.
/// </summary>
public class AMaintDeviceListQueryInput : PageInputBase
{
    /// <summary>
    /// 选择导出数据ids.
    /// </summary>
    public string selectIds { get; set; }

    /// <summary>
    /// 选择导出数据key.
    /// </summary>
    public string selectKey { get; set; }

    /// <summary>
    /// 导出类型.
    /// </summary>
    public int dataType { get; set; }
    
    /// <summary>
    /// 关键词查询.
    /// </summary>
    public string jnpfKeyword { get; set; }

    /// <summary>
    /// 设备ID.
    /// </summary>
    public string F_DeviceId { get; set; }

    /// <summary>
    /// 设备编号.
    /// </summary>
    public string F_DeviceNo { get; set; }

    /// <summary>
    /// 设备名称.
    /// </summary>
    public string F_DeviceName { get; set; }

    /// <summary>
    /// 设备状态.
    /// </summary>
    public string F_DeviceStatus { get; set; }

    /// <summary>
    /// 设备类别.
    /// </summary>
    public List<string> F_DeviceType { get; set; }

}