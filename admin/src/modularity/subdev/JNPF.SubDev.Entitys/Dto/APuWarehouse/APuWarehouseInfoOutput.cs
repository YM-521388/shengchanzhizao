namespace JNPF.example.Entitys.Dto.APuWarehouse;
 
/// <summary>
/// 仓库管理2输出参数.
/// </summary>
public class APuWarehouseInfoOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 上级ID.
    /// </summary>
    public List<string> F_ParentId { get; set; }

    /// <summary>
    /// 仓库名.
    /// </summary>
    public string? F_Name { get; set; }

    /// <summary>
    /// 管理员.
    /// </summary>
    public List<string> F_UsersId { get; set; }

    /// <summary>
    /// 仓库位置.
    /// </summary>
    public List<string> F_Location { get; set; }

    /// <summary>
    /// 详细地址.
    /// </summary>
    public string? F_Address { get; set; }

    /// <summary>
    /// 二维码.
    /// </summary>
    public string? F_QRCode { get; set; }

    /// <summary>
    /// 状态.
    /// </summary>
    public int F_State { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

}