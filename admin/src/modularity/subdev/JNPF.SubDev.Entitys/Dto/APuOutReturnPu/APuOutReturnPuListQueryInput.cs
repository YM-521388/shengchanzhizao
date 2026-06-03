using JNPF.Common.Filter;

namespace JNPF.example.Entitys.Dto.APuOutReturnPu;

/// <summary>
/// a_pu_out_return_pu列表查询输入.
/// </summary>
public class APuOutReturnPuListQueryInput : PageInputBase
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
    /// 采购退货单号.
    /// </summary>
    public string F_ReturnOutNo { get; set; }

    /// <summary>
    /// 仓库ID.
    /// </summary>
    public List<string> F_WarehouseId { get; set; }

    /// <summary>
    /// 出库类型.
    /// </summary>
    public object F_ReturnOutType { get; set; }

    /// <summary>
    /// 退货日期.
    /// </summary>
    public List<DateTime> F_ReturnOutDate { get; set; }

}