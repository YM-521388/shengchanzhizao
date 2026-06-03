using JNPF.Common.Filter;

namespace JNPF.example.Entitys.Dto.APuCheck;

/// <summary>
/// a_pu_check列表查询输入.
/// </summary>
public class APuCheckListQueryInput : PageInputBase
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
    /// ID.
    /// </summary>
    public string F_CheckId { get; set; }

    /// <summary>
    /// 盘点日期.
    /// </summary>
    public List<DateTime> F_CheckDate { get; set; }

    /// <summary>
    /// 盘点人.
    /// </summary>
    public string F_CheckUserId { get; set; }

    /// <summary>
    /// 盘点仓库.
    /// </summary>
    public string F_WarehouseId { get; set; }


}