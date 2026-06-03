using JNPF.Common.Filter;

namespace JNPF.example.Entitys.Dto.APuWarehouse;

/// <summary>
/// 仓库管理2列表查询输入.
/// </summary>
public class APuWarehouseListQueryInput : PageInputBase
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
    /// 上级ID.
    /// </summary>
    public List<string> F_ParentId { get; set; }

    /// <summary>
    /// 仓库名.
    /// </summary>
    public string F_Name { get; set; }

    /// <summary>
    /// 管理员.
    /// </summary>
    public string F_UsersId { get; set; }

    /// <summary>
    /// 仓库位置.
    /// </summary>
    public List<string> F_Location { get; set; }

    /// <summary>
    /// 详细地址.
    /// </summary>
    public string F_Address { get; set; }

    /// <summary>
    /// 二维码.
    /// </summary>
    public string F_QRCode { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public List<DateTime> F_CreatorTime { get; set; }

}