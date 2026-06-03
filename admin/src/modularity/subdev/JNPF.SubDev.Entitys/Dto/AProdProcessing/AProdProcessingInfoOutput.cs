using JNPF.Common.Models;
using JNPF.example.Entitys.Dto.AProdBomItem;
using JNPF.example.Entitys.Dto.AProdTask;
 
namespace JNPF.example.Entitys.Dto.AProdProcessing;

/// <summary>
/// a_prod_processing输出参数.
/// </summary>
public class AProdProcessingInfoOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

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
    public string? F_GoodsId { get; set; }

    /// <summary>
    /// BOMID.
    /// </summary>
    public string F_BOMId { get; set; }

    /// <summary>
    /// 工艺路线ID.
    /// </summary>
    public string? F_RoutingId { get; set; }

    /// <summary>
    /// 生产计划ID.
    /// </summary>
    public string F_ProdPlanId { get; set; }

    /// <summary>
    /// 加工单号.
    /// </summary>
    public string? F_ProcessNo { get; set; }

    /// <summary>
    /// 计划数.
    /// </summary>
    public int? F_PlanQty { get; set; }

    /// <summary>
    /// 交货日期.
    /// </summary>
    public DateTime? F_DeliveryDate { get; set; }

    /// <summary>
    /// 优先级.
    /// </summary>
    public string F_Priority { get; set; }

    /// <summary>
    /// 计划开始.
    /// </summary>
    public DateTime? F_PlanStartDate { get; set; }

    /// <summary>
    /// 计划结束.
    /// </summary>
    public DateTime? F_PlanEndDate { get; set; }

    /// <summary>
    /// 图纸.
    /// </summary>
    public List<FileControlsModel> F_DrawingFile { get; set; }

    /// <summary>
    /// 客户.
    /// </summary>
    public string? F_CustomerName { get; set; }

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
    public string? F_LockSet { get; set; }

    /// <summary>
    /// 铰链.
    /// </summary>
    public string? F_Hinge { get; set; }

    /// <summary>
    /// 颜色.
    /// </summary>
    public string? F_Color { get; set; }

    /// <summary>
    /// 类别.
    /// </summary>
    public List<string> F_Type { get; set; }

    /// <summary>
    /// BOM图片.
    /// </summary>
    public List<FileControlsModel> F_BOMImages { get; set; }

    /// <summary>
    /// 状态.
    /// </summary>
    public string F_State { get; set; }

    /// <summary>
    /// 排单序号.
    /// </summary>
    public int? F_SequenceNo { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    public string? F_Description { get; set; }

    /// <summary>
    /// 创建用户.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

    /// <summary>
    /// 加工单用料清单.
    /// </summary>
    public List<AProdBomItemInfoOutput> tableField449e6c { get; set; }

    /// <summary>
    /// 生产任务.
    /// </summary>
    public List<AProdTaskInfoOutput> tableField0c720e { get; set; }

}