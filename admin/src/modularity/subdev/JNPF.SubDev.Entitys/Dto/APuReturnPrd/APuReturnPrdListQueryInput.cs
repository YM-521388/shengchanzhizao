using JNPF.Common.Filter;

namespace JNPF.example.Entitys.Dto.APuReturnPrd;

/// <summary>
/// a_pu_return_prd列表查询输入.
/// </summary>
public class APuReturnPrdListQueryInput : PageInputBase
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
    /// ID.
    /// </summary>
    public string F_ReturnInPRDId { get; set; }

    /// <summary>
    /// 生产退料单号.
    /// </summary>
    public string F_ReturnInNo { get; set; }

    /// <summary>
    /// 仓库ID.
    /// </summary>
    public List<string> F_WarehouseId { get; set; }

    /// <summary>
    /// 入库类型.
    /// </summary>
    public object F_ReturnInType { get; set; }

    /// <summary>
    /// 退料日期.
    /// </summary>
    public List<DateTime> F_ReturnInDate { get; set; }

    /// <summary>
    /// 工单类型.
    /// </summary>
    public string F_Type { get; set; }

    /// <summary>
    /// 工单ID.
    /// </summary>
    public object F_WorkOrderId { get; set; }

}