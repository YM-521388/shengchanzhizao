namespace JNPF.example.Entitys.Dto.AProdProjectItem;

/// <summary>
/// 项目商品列表输入参数.
/// </summary>
public class AProdProjectItemListOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 项目ID.
    /// </summary>
    public string? F_ProjectId { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    public string? F_GoodsId { get; set; }

    /// <summary>
    /// 工单编号.
    /// </summary>
    public string? F_WorkOrderNo { get; set; }

    /// <summary>
    /// 计划数.
    /// </summary>
    public string F_PlanNum { get; set; }

    /// <summary>
    /// 交货日期.
    /// </summary>
    public string F_DeliveryDate { get; set; }

    /// <summary>
    /// 优先级.
    /// </summary>
    public string? F_Priority { get; set; }

    /// <summary>
    /// BOMID.
    /// </summary>
    public string? F_BomId { get; set; }

    /// <summary>
    /// 图纸.
    /// </summary>
    public object? F_DrawingFiles { get; set; }

    /// <summary>
    /// 门板厚度.
    /// </summary>
    public string? F_DoorPlateThickness { get; set; }

    /// <summary>
    /// 箱体板厚.
    /// </summary>
    public string? F_BoxPlateThickness { get; set; }

    /// <summary>
    /// 安装板或安装梁.
    /// </summary>
    public string? F_InstallPlateOrBeam { get; set; }

    /// <summary>
    /// 客户.
    /// </summary>
    public string? F_CustomerName { get; set; }

    /// <summary>
    /// 锁具.
    /// </summary>
    public string? F_LockSet { get; set; }

    /// <summary>
    /// 铰链.
    /// </summary>
    public string? F_Hinge { get; set; }

    /// <summary>
    /// BOM图片.
    /// </summary>
    public object? F_BomImages { get; set; }

    /// <summary>
    /// 颜色.
    /// </summary>
    public string? F_Color { get; set; }

    /// <summary>
    /// 类别.
    /// </summary>
    public string? F_Category { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

}