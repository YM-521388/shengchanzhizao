using JNPF.Common.Filter;

namespace JNPF.example.Entitys.Dto.AProdProcessing;

/// <summary>
/// a_prod_processing列表查询输入.
/// </summary>
public class AProdProcessingListQueryInput : PageInputBase
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
    /// 加工单ID.
    /// </summary>
    public string F_ProcessingId { get; set; }

    /// <summary>
    /// 合同ID.
    /// </summary>
    public string F_ContractId { get; set; }

    /// <summary>
    /// 项目ID.
    /// </summary>
    public string F_ProjectId { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    public string F_GoodstId { get; set; }

    /// <summary>
    /// 生产计划ID.
    /// </summary>
    public string F_ProdPlanId { get; set; }

    /// <summary>
    /// 加工单号.
    /// </summary>
    public string F_ProcessNo { get; set; }

    /// <summary>
    /// 计划数.
    /// </summary>
    public List<decimal?> F_PlanQty { get; set; }

    /// <summary>
    /// 交货日期.
    /// </summary>
    public List<DateTime> F_DeliveryDate { get; set; }

    /// <summary>
    /// 优先级.
    /// </summary>
    public string F_Priority { get; set; }

    /// <summary>
    /// 计划开始.
    /// </summary>
    public List<DateTime> F_PlanStartDate { get; set; }

    /// <summary>
    /// 计划结束.
    /// </summary>
    public List<DateTime> F_PlanEndDate { get; set; }

    /// <summary>
    /// 客户.
    /// </summary>
    public string F_CustomerName { get; set; }

    /// <summary>
    /// 门板厚度.
    /// </summary>
    public string F_DoorPlateThickness { get; set; }

    /// <summary>
    /// 箱体板厚.
    /// </summary>
    public string F_BoxPlateThickness { get; set; }

    /// <summary>
    /// 安装板或安装梁.
    /// </summary>
    public string F_InstallOrBeam { get; set; }

    /// <summary>
    /// 锁具.
    /// </summary>
    public string F_LockSet { get; set; }

    /// <summary>
    /// 铰链.
    /// </summary>
    public string F_Hinge { get; set; }

    /// <summary>
    /// 颜色.
    /// </summary>
    public string F_Color { get; set; }

    /// <summary>
    /// 类别.
    /// </summary>
    public List<string> F_Type { get; set; }

    /// <summary>
    /// 状态.
    /// </summary>
    public string F_State { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    public string F_Description { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public List<DateTime> F_CreatorTime { get; set; }

}