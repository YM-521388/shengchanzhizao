using JNPF.JsonSerialization;
using Newtonsoft.Json;

namespace JNPF.example.Entitys.Dto.AMaintDeviceQr;
 
/// <summary>
/// 设备二维码修改输入参数.
/// </summary>
public class AMaintDeviceQrCrInput
{
    /// <summary>
    /// 数量.
    /// </summary>
    public int? F_Num { get; set; }

    /// <summary>
    /// 二维码编号.
    /// </summary>
    public string? F_Code { get; set; }

    /// <summary>
    /// 生成用户.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 生成时间.
    /// </summary>

}