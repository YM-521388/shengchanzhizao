namespace JNPF.example.Entitys.Dto.AMaintDeviceQr;
 
/// <summary>
/// 设备二维码输出参数.
/// </summary>
public class AMaintDeviceQrInfoOutput
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
    /// 生成用户.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 生成时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

}