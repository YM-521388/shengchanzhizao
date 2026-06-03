using JNPF.Common.Models;
using JNPF.JsonSerialization;
using Newtonsoft.Json;
using JNPF.example.Entitys.Dto.ACtcContractItemGood;

namespace JNPF.example.Entitys.Dto.ACtcContractItem;
 
/// <summary>
/// 选择合同商品修改输入参数.
/// </summary>
public class ACtcContractItemCrInput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string F_Id { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    public string? F_CustomerId { get; set; }

    /// <summary>
    /// 单价.
    /// </summary>
    public decimal? F_Price { get; set; }

    /// <summary>
    /// 销售数量.
    /// </summary>
    public int? F_SaleQty { get; set; }

    /// <summary>
    /// 折扣.
    /// </summary>
    public decimal? F_Discount { get; set; }/// <summary>
                                            /// 工艺路线ID.
                                            /// </summary>
    public string F_RoutingId { get; set; }

    /// <summary>
    /// 工单编号.
    /// </summary>
    public string? F_WorkOrderNo { get; set; }

    /// <summary>
    /// 计划数.
    /// </summary>
    public int? F_PlanNum { get; set; }

    /// <summary>
    /// 交货日期.
    /// </summary>
    public DateTime? F_DeliveryDate { get; set; }

    /// <summary>
    /// 优先级.
    /// </summary>
    public string F_Priority { get; set; }

    /// <summary>
    /// BOMID.
    /// </summary>
    public string F_BomId { get; set; }

    /// <summary>
    /// 图纸.
    /// </summary>
    public List<FileControlsModel> F_DrawingFiles { get; set; }

    /// <summary>
    /// 类别.
    /// </summary>
    public List<string> F_Category { get; set; }

    /// <summary>
    /// 商品备注.
    /// </summary>
    public string? F_Description { get; set; }

    /// <summary>
    /// 门板厚度.
    /// </summary>
    public string? F_DoorPlateThickness { get; set; }

    /// <summary>
    /// 箱体板厚.
    /// </summary>
    public string? F_BoxPlateThickness { get; set; }

    /// <summary>
    /// 安装或安装梁.
    /// </summary>
    public string? F_InstallOrBeam { get; set; }

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
    /// 材质.
    /// </summary>
    public string? F_Material { get; set; }

    /// <summary>
    /// 审核状态.
    /// </summary>
    public string? F_F_AuditStatus { get; set; }

    /// <summary>
    /// 附件.
    /// </summary>
    public List<FileControlsModel> F_Files { get; set; }

    /// <summary>
    /// 创建人员.
    /// </summary>
    public string? F_CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>

    /// <summary>
    /// 用料清单.
    /// </summary>
    public List<ACtcContractItemGoodCrInput> ProjectGoods { get; set; }
}