using JNPF.Common.Filter;

namespace JNPF.example.Entitys.Dto.ASupplier;

/// <summary>
/// 供应商管理2列表查询输入.
/// </summary>
public class ASupplierListQueryInput : PageInputBase
{
    /// <summary>
    /// 高级查询.
    /// </summary>
    public string superQueryJson { get; set; }

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
    /// 供应商名称.
    /// </summary>
    public string F_SupplierName { get; set; }

    /// <summary>
    /// 供应商编号.
    /// </summary>
    public string F_SupplierNo { get; set; }

    /// <summary>
    /// 联系人.
    /// </summary>
    public string F_ContactPerson { get; set; }

    /// <summary>
    /// 联系人电话.
    /// </summary>
    public string F_ContactPhone { get; set; }

    /// <summary>
    /// 地区.
    /// </summary>
    public List<string> F_Region { get; set; }

    /// <summary>
    /// 供应商标签.
    /// </summary>
    public object F_SupplierTags { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    public string F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public List<DateTime> F_CreatorTime { get; set; }

}